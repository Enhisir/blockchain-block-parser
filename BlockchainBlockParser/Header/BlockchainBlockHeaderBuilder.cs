using System.ComponentModel.DataAnnotations;

namespace BlockchainBlockParser.Header;

/// <summary>
/// Builds Blockchain block header.
/// All methods except BuildDefault marked as private
/// because of determinacy of the process
/// </summary>
/// <param name="blockStream"></param>
internal class BlockchainBlockHeaderBuilder(Stream blockStream)
{
    private int? _version;

    private string? _previousBlockHash;

    private string? _merkleRoot;

    private DateTime? _timestamp;

    private string? _bits;

    private uint? _nonce;

    public async Task<BlockchainBlockHeader> BuildDefaultAsync()
    {
        await WithVersionAsync();
        await WithPreviousBlockHashAsync();
        await WithMerkleRootAsync();
        await WithTimestampAsync();
        await WithBitsAsync();
        await WithNonceAsync();

        return Build();
    }
    
    private async Task<byte[]> ReadInfo(int size)
    {
        var resultBytes = new byte[size];
        var resultSize = await blockStream.ReadAsync(resultBytes);
        
        if (resultSize != size) throw new ValidationException();
        return resultBytes;
    }

    private async Task WithVersionAsync()
    {
        var versionBytes = await ReadInfo(BlockchainBlockHeaderSizes.Version);

        // Little-Indian.
        // Learn more on https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-byte-array-to-an-int
        Array.Reverse(versionBytes);
        
        _version = BitConverter.ToInt32(versionBytes);
    }
    
    private async Task WithPreviousBlockHashAsync()
    {
        var previousBlockHashBytes = await ReadInfo(BlockchainBlockHeaderSizes.PreviousBlockHash);
        
        _previousBlockHash = BytesHelper.BytesToString(previousBlockHashBytes);
    }
    
    private async Task WithMerkleRootAsync()
    {
        var merkleRoot = await ReadInfo(BlockchainBlockHeaderSizes.MerkleRoot);
        
        _merkleRoot = BytesHelper.BytesToString(merkleRoot);
    }
    
    private async Task WithTimestampAsync()
    {
        var timestampBytes = await ReadInfo(BlockchainBlockHeaderSizes.Timestamp);
        
        // Little-Indian
        Array.Reverse(timestampBytes);
        
        var timestampUint = BitConverter.ToUInt32(timestampBytes);
        _timestamp = DateTime.UnixEpoch.AddSeconds(Convert.ToDouble(timestampUint)); 
        // used explicit type conversion to avoid bugs or unclear points
    }
    
    private async Task WithBitsAsync()
    {
        var bitsBytes = await ReadInfo(BlockchainBlockHeaderSizes.Bits);
        
        // Little-Indian
        Array.Reverse(bitsBytes);
        
        _bits = BytesHelper.BytesToString(bitsBytes);
    }
    
    private async Task WithNonceAsync()
    {
        var nonceBytes = await ReadInfo(BlockchainBlockHeaderSizes.Nonce);
        
        // Little-Indian
        Array.Reverse(nonceBytes);
        
        _nonce = BitConverter.ToUInt32(nonceBytes);
    }
    
    private BlockchainBlockHeader Build()
    {
        return new BlockchainBlockHeader()
        {
            Version = _version!.Value,
            PreviousBlockHash = _previousBlockHash!,
            MerkleRoot = _merkleRoot!,
            Timestamp = _timestamp!.Value,
            Bits = _bits!,
            Nonce = _nonce!.Value
        };
    }
}