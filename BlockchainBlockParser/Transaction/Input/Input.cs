using System.Collections.Immutable;

namespace BlockchainBlockParser.Transaction.Input;

public class Input
{
    // TODO: refactor, move hash to TXID
    
    public string Hash { get; init; } = null!;
    
    public ImmutableList<byte> RawData { get; init; } = null!;

    public int Size => RawData.Count;
}