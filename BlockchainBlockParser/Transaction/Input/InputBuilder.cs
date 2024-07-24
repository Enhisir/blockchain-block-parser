using System.ComponentModel.DataAnnotations;
using BlockchainBlockParser.CompactSize;

namespace BlockchainBlockParser.Transaction.Input;

public class InputBuilder(Stream blockStream) : IDisposable, IAsyncDisposable
{
    private readonly MemoryStream _inputStream = new();

    public async Task<Input> BuildDefault()
    {
        await WithTxIdAndVoutAsync();
        await WithScriptSigAsync();
        await WithSequenceAsync();
        return Build();
    }
    
    public void Dispose()
    {
        blockStream.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return blockStream.DisposeAsync();
    }
    
    private async Task WithTxIdAndVoutAsync()
    {
        var begin = await ReadInfo(InputSizes.Txid + InputSizes.Vout);
        await _inputStream.WriteAsync(begin);
    }

    private async Task WithScriptSigAsync()
    {
        var scriptSigSize = await CompactSizeHelper.ParseAsync(blockStream);
        await _inputStream.WriteAsync(scriptSigSize.RawData);
        
        var scriptSigBytes = await ReadInfo(scriptSigSize.Count);
        await _inputStream.WriteAsync(scriptSigBytes);
    }
    
    private async Task WithSequenceAsync()
    {
        var sequenceBytes = await ReadInfo(InputSizes.Sequence);
        await _inputStream.WriteAsync(sequenceBytes);
    }

    private Input Build()
    {
        var data = _inputStream.ToArray();
        return new Input()
        {
            Hash = BytesHelper.BytesToString(BytesHelper.DoubleHash(data, reverse: true)),
            RawData = data
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