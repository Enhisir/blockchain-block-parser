using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BlockchainBlockParser.Header;

namespace BlockchainBlockParser.Transaction;

public class TransactionBuilder(Stream blockStream)
{
    private int? _version;

    private CompactSize.CompactSize? _inputCount;

    private ImmutableList<Input.Input>? _inputs;

    private CompactSize.CompactSize? _outputCount;

    private ImmutableList<Output.Output>? _outputs;

    private int? _locktime;
    
    public Transaction Build()
    {
        
    }
    
    private async Task WithVersionAsync()
    {
        var versionBytes = await ReadInfo(TransactionSizes.Version);

        // Little-Indian.
        // Learn more on https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-byte-array-to-an-int
        Array.Reverse(versionBytes);
        
        _version = BitConverter.ToInt32(versionBytes);
    }

    
    private async Task<byte[]> ReadInfo(long size)
    {
        var resultBytes = new byte[size];
        var resultSize = await blockStream.ReadAsync(resultBytes);
        
        if (resultSize != size) throw new ValidationException();
        return resultBytes;
    }
}