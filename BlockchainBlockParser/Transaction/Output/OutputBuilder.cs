using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
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
        var amount = await ReadInfo(OutputSizes.Amount);
        await _inputStream.WriteAsync(amount);
    }

    private async Task WithScriptPubKeyAsync()
    {
        var scriptPubKeySize = await CompactSizeHelper.ParseAsync(blockStream);
        await _inputStream.WriteAsync(scriptPubKeySize.RawData);
        
        var scriptPubKeySizeBytes = await ReadInfo(scriptPubKeySize.Count);
        await _inputStream.WriteAsync(scriptPubKeySizeBytes);
    }

    private Output Build()
    {
        var data = _inputStream.ToArray();
        return new Output()
        {
            Hash = BytesHelper.BytesToString(BytesHelper.DoubleHash(data)),
            RawData = data.ToImmutableList()
        };
    }
    
    private async Task<byte[]> ReadInfo(long size)
    {
        var resultBytes = new byte[size];
        var resultSize = await blockStream.ReadAsync(resultBytes);
        
        if (resultSize != size) throw new ValidationException();
        return resultBytes;
    }
}