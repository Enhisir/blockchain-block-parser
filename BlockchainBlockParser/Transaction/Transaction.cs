using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace BlockchainBlockParser.Transaction;

public class Transaction
{
    [Length(32, 32)] public string Hash { get; init; } = null!;
    
    public int Version { get; init; }
    
    public CompactSize.CompactSize InputCount { get; init; }

    public ImmutableList<Input.Input> Inputs { get; init; } = null!;
    
    public CompactSize.CompactSize OutputCount { get; init; }
    
    public ImmutableList<Output.Output> Outputs { get; init; } = null!;
    
    public uint Locktime { get; init; }

    public ImmutableList<byte> RawData { get; init; } = null!;

    public int Size => RawData.Count;

    /*public int Size => TransactionSizes.Version
                       + InputCount.Size
                       + Inputs.Select(i => i.Size).Sum()
                       + TransactionSizes.Locktime;*/
}