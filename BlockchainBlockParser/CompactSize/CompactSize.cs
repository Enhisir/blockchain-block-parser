namespace BlockchainBlockParser.CompactSize;

// variable, 1-9 bytes (with size type)
public readonly struct CompactSize
{
    public CompactSizeType SizeType { get; init; }
    
    public long Count { get; init; }
    
    public int Size => 1 + CompactSizeHelper.CountCompactSizeBytesFromType(SizeType);
}