namespace BlockchainBlockParser.Transaction.Output;

public class Output
{
    public string Hash { get; init; } = null!;
    
    public byte[] RawData { get; init; } = null!;

    public int Size => RawData.Length;
}