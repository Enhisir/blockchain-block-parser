namespace BlockchainBlockParser.Header;

/// <summary>
/// Sizes of header and its properties in bytes
/// </summary>
public static class BlockchainBlockHeaderSizes
{
    public const int Total = 80;

    public const int Version = 4;

    public const int PreviousBlockHash = 32;

    public const int MerkleRoot = 32;

    public const int Timestamp = 4;

    public const int Bits = 4;
    
    public const int Nonce = 4;
}