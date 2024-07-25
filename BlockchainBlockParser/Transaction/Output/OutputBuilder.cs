using System.Collections.Immutable;
using BlockchainBlockParser.CompactSize;

namespace BlockchainBlockParser.Transaction.Output;

public class OutputBuilder(Stream blockStream) : IDisposable, IAsyncDisposable
{
    private readonly MemoryStream _inputStream = new();

    public async Task<Output> BuildDefaultAsync()
    {
        await WithAmountAsync();
        await WithScriptPubKeyAsync();
        return Build();
    }
    
    public void Dispose()
    {
        _inputStream.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _inputStream.DisposeAsync();
    }
    
    private async Task WithAmountAsync()
    {
        var amount = await BytesHelper.ReadInfoAsync(blockStream, OutputSizes.Amount);
        await _inputStream.WriteAsync(amount);
    }

    private async Task WithScriptPubKeyAsync()
    {
        var scriptPubKeySize = await CompactSizeHelper.ParseAsync(blockStream);
        await _inputStream.WriteAsync(scriptPubKeySize.RawData);
        
        var scriptPubKeySizeBytes = await BytesHelper.ReadInfoAsync(blockStream, scriptPubKeySize.Count);
        await _inputStream.WriteAsync(scriptPubKeySizeBytes);
    }

    private Output Build()
    {
        var data = _inputStream.ToArray();
        return new Output()
        {
            RawData = data.ToImmutableList()
        };
    }
}