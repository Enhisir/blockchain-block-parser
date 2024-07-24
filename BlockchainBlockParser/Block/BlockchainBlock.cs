using System.Collections.Immutable;
using BlockchainBlockParser.Header;

namespace BlockchainBlockParser.Block;

public class BlockchainBlock
{
    public int Size => Header.Size + TransactionCount.Size;
    // 32 bytes, calculated on client
    public string Hash { get; init; } = null!;

    // 80 bytes
    public BlockchainBlockHeader Header { get; init; } = null!;
    
    public CompactSize.CompactSize TransactionCount { get; init; }

    // variable
    public ImmutableList<string> Transactions { get; init; } = null!;

    public string CoinbaseTransaction => Transactions.First(); // remake with Validation Exception Fault
}