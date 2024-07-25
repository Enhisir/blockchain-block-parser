using System.ComponentModel.DataAnnotations;

namespace BlockchainBlockParser.Header;

public record BlockchainBlockHeader
{
    [Length(32, 32)] public string Hash { get; init; } = null!;
    
    public int Size => BlockchainBlockHeaderSizes.Total;
    
    public int Version { get; init; }
    
    [Length(32, 32)] public string PreviousBlockHash { get; init; } = null!;
    
    [Length(32, 32)] public string MerkleRoot { get; init; } = null!;
    
    // unix-time timestamp
    public DateTime Timestamp { get; init; }
    
    public uint Bits { get; init; }
    
    public uint Nonce { get; init; }
}