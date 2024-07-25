namespace BlockchainBlockParser.Export.Mapping;

public class TransactionDto
{
    public string Hash { get; set; }
    
    public int Version { get; set; }
    
    public int InputCount { get; set; }
    
    public int OutputCount { get; set; }
    
    public uint Locktime { get; set; }
    
    public int Size { get; set; }
}