using System.Collections.Immutable;

namespace BlockchainBlockParser.Transaction.Output;

public class Output
{
    public ImmutableList<byte> RawData { get; init; } = null!;

    public int Size => RawData.Count;
}