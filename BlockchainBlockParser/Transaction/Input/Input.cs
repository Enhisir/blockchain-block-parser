using System.Collections.Immutable;

namespace BlockchainBlockParser.Transaction.Input;

public class Input
{
    public ImmutableList<byte> RawData { get; init; } = null!;

    public int Size => RawData.Count;
}