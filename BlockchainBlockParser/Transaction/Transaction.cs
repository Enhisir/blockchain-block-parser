using System.Collections.Immutable;

namespace BlockchainBlockParser.Transaction;

public class Transaction
{
    public int Size => TransactionSizes.Version
                       + InputCount.Size
                       + Inputs.Select(i => i.Size).Sum()
                       + TransactionSizes.Locktime;
    
    public int Version { get; init; }
    
    public CompactSize.CompactSize InputCount { get; init; }

    public ImmutableList<Input.Input> Inputs { get; init; } = null!;
    
    public CompactSize.CompactSize OutputCount { get; init; }
    
    public ImmutableList<Output.Output> Outputs { get; init; } = null!;
    
    public int Locktime { get; init; }
}