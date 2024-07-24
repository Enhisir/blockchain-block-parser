using System.ComponentModel.DataAnnotations;

namespace BlockchainBlockParser.Header;

public record BlockchainBlockHeader
{
    public int Size => BlockchainBlockHeaderSizes.Total;
    
    public int Version { get; init; }
    
    [Length(32, 32)] public string PreviousBlockHash { get; init; } = null!;
    
    [Length(32, 32)] public string MerkleRoot { get; init; } = null!;
    
    // unix-time timestamp
    public DateTime Timestamp { get; init; }
    
    [Length(4, 4)] public string Bits { get; init; } = null!;
    
    public uint Nonce { get; init; }
}