using System.Collections.Immutable;

namespace BlockchainBlockParser.Transaction.Output;

public class Output
{
    // TODO: refactor, add Amount and other stuff
    
    public string Hash { get; init; } = null!;

    public ImmutableList<byte> RawData { get; init; } = null!;

    public int Size => RawData.Count;
}