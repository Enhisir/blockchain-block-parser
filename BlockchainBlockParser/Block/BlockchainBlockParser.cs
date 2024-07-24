using BlockchainBlockParser.Header;

namespace BlockchainBlockParser.Block;

public class BlockchainBlockParser(Stream blockchainBlockStream)
{
    private readonly Stream _stream = blockchainBlockStream;
    
    private async Task<BlockchainBlockHeader> ParseHeaderAsync(Stream blockStream)
    {
        var builder = new BlockchainBlockHeaderBuilder(blockStream);
        return await builder.BuildDefaultAsync();
    }
    
    /*public void Dispose()
    {
        _stream.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _stream.DisposeAsync().Preserve();
    }*/
}