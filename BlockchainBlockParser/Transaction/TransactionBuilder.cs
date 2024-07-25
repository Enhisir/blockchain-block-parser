using System.Collections.Immutable;
using BlockchainBlockParser.CompactSize;
using BlockchainBlockParser.Transaction.Input;
using BlockchainBlockParser.Transaction.Output;

namespace BlockchainBlockParser.Transaction;

public class TransactionBuilder(Stream blockStream)
{
    private readonly long _previousPosition = blockStream.Position;

    private byte[]? _rawData;

    private int? _version;

    private CompactSize.CompactSize? _inputCount;

    private ImmutableList<Input.Input>? _inputs;

    private CompactSize.CompactSize? _outputCount;

    private ImmutableList<Output.Output>? _outputs;

    private uint? _locktime;

    public async Task<Transaction> BuildDefaultAsync()
    {
        await WithVersionAsync();
        await WithInputsAsync();
        await WithOutputsAsync();
        await WithLocktimeAsync();
        await WithRawDataAsync();
        
        return Build();
    }
    
    private Transaction Build()
    {
        var hashString = BytesHelper.BytesToString(BytesHelper.DoubleHash(_rawData!));
        
        return new Transaction()
        {
            Hash = hashString,
            Version = _version!.Value,
            InputCount = _inputCount!.Value,
            Inputs = _inputs!,
            OutputCount = _outputCount!.Value,
            Outputs = _outputs!,
            Locktime = _locktime!.Value,
            RawData = _rawData!.ToImmutableList()
        };
    }
    
    private async Task WithVersionAsync()
    {
        var versionBytes = await BytesHelper.ReadInfoAsync(blockStream, TransactionSizes.Version);
        
        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(versionBytes);
        
        _version = BitConverter.ToInt32(versionBytes);
    }

    private async Task WithInputsAsync()
    {
        var inputCount = await CompactSizeHelper.ParseAsync(blockStream);

        var inputs = new Input.Input[inputCount.Count];
        for (var i = 0; i < inputCount.Count; i++)
        {
            await using var builder = new InputBuilder(blockStream);
            inputs[i] = await builder.BuildDefaultAsync();
        }

        _inputCount = inputCount;
        _inputs = inputs.ToImmutableList();
    }
    
    private async Task WithOutputsAsync()
    {
        var outputCount = await CompactSizeHelper.ParseAsync(blockStream);

        var outputs = new Output.Output[outputCount.Count];
        for (var i = 0; i < outputCount.Count; i++)
        {
            await using var builder = new OutputBuilder(blockStream);
            outputs[i] = await builder.BuildDefaultAsync();
        }

        _outputCount = outputCount;
        _outputs = outputs.ToImmutableList();
    }

    private async Task WithLocktimeAsync()
    {
        var locktimeBytes = await BytesHelper.ReadInfoAsync(blockStream, TransactionSizes.Locktime);
        
        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(locktimeBytes);

        _locktime = BitConverter.ToUInt32(locktimeBytes);
    }

    private async Task WithRawDataAsync()
    {
        var size = blockStream.Position - _previousPosition;
        blockStream.Position = _previousPosition;
        _rawData = await BytesHelper.ReadInfoAsync(blockStream, size);
    }
}