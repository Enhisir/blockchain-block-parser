using System.Collections.Immutable;
using BlockchainBlockParser.Header;

namespace BlockchainBlockParser.Block;

public class BlockchainBlock
{
    public BlockchainBlockHeader Header { get; init; } = null!;
    
    public CompactSize.CompactSize TransactionCount { get; init; }
    
    public ImmutableList<Transaction.Transaction> Transactions { get; init; } = null!;

    public Transaction.Transaction CoinbaseTransaction => Transactions.First();
    
    public string Hash => Header.Hash;

    public int Size => Header.Size
                       + TransactionCount.Size
                       + Transactions.Sum(tx => tx.Size);
}