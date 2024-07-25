using System.Collections.Immutable;
using BlockchainBlockParser.CompactSize;

namespace BlockchainBlockParser.Transaction.Input;

public class InputBuilder(Stream blockStream) : IDisposable, IAsyncDisposable
{
    private readonly MemoryStream _inputStream = new();

    public async Task<Input> BuildDefaultAsync()
    {
        await WithTxIdAndVoutAsync();
        await WithScriptSigAsync();
        await WithSequenceAsync();
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
    
    private async Task WithTxIdAndVoutAsync()
    {
        var begin = await BytesHelper.ReadInfoAsync(blockStream, InputSizes.Txid + InputSizes.Vout);
        await _inputStream.WriteAsync(begin);
    }

    private async Task WithScriptSigAsync()
    {
        var scriptSigSize = await CompactSizeHelper.ParseAsync(blockStream);
        await _inputStream.WriteAsync(scriptSigSize.RawData);
        
        var scriptSigBytes = await BytesHelper.ReadInfoAsync(blockStream, scriptSigSize.Count);
        await _inputStream.WriteAsync(scriptSigBytes);
    }
    
    private async Task WithSequenceAsync()
    {
        var sequenceBytes = await BytesHelper.ReadInfoAsync(blockStream, InputSizes.Sequence);
        await _inputStream.WriteAsync(sequenceBytes);
    }

    private Input Build()
    {
        var data = _inputStream.ToArray();
        return new Input()
        {
            RawData = data.ToImmutableList()
        };
    }
}