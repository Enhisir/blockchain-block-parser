namespace BlockchainBlockParser.Export;

public interface IBlockchainBlockExport
{
    public Task ExportAsync(BlockchainBlock block, string filePath);
}