namespace BlockchainBlockParser.Export.Mapping;

public class BlockchainBlockDto
{
    public string Hash { get; set; }
    
    public int Version { get; set; }
    
    public string PreviousBlockHash { get; set; }
    
    public string MerkleRoot { get; set; }
    
    public uint Timestamp { get; set; }
    
    public uint Bits { get; set; }
    
    public uint Nonce { get; set; }

    public int TransactionCount { get; set; }
    
    public int Size { get; set; }
}