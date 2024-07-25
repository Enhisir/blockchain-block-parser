using System.Collections.Immutable;
using BlockchainBlockParser.CompactSize;
using BlockchainBlockParser.Header;
using BlockchainBlockParser.Transaction;

namespace BlockchainBlockParser.Block;

public class BlockchainBlockBuilder(Stream blockStream)
{
    private BlockchainBlockHeader? _header;

    private CompactSize.CompactSize? _transactionCount;

    private ImmutableList<Transaction.Transaction>? _transactions;

    public async Task<BlockchainBlock> BuildDefaultAsync()
    {
        await WithHeaderAsync();
        await WithTransactionsAsync();
        
        return Build();
    }
    
    private async Task WithHeaderAsync()
    {
        var headerBuilder = new BlockchainBlockHeaderBuilder(blockStream);
        _header = await headerBuilder.BuildDefaultAsync();
    }

    private async Task WithTransactionsAsync()
    {
        var transactionCount = await CompactSizeHelper.ParseAsync(blockStream);
        
        var transactions = new Transaction.Transaction[transactionCount.Count];
        for (var i = 0; i < transactionCount.Count; i++)
        {
            var transactionBuilder = new TransactionBuilder(blockStream);
            transactions[i] = await transactionBuilder.BuildDefaultAsync();
        }

        _transactionCount = transactionCount;
        _transactions = transactions.ToImmutableList();
    }

    
    
    private BlockchainBlock Build()
    {
        return new BlockchainBlock()
        {
            Header = _header!,
            TransactionCount = _transactionCount!.Value,
            Transactions = _transactions!
        };
    }
}