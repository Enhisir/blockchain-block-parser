namespace BlockchainBlockParser.CompactSize;

// variable, 1-9 bytes (with size type)
public readonly struct CompactSize
{
    public CompactSizeType SizeType { get; init; }
    
    public long Count { get; init; }
    
    public int Size => CompactSizeHelper.CountCompactSizeBytesFromType(SizeType);
    
    public byte[] RawData { get; init; }
}