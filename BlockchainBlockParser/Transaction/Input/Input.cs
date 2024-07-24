namespace BlockchainBlockParser.Transaction.Input;

public class Input
{
    public string Hash { get; init; } = null!;
    
    public byte[] RawData { get; init; } = null!;

    public int Size => RawData.Length;
}