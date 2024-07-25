namespace BlockchainBlockParser.Header;

/// <summary>
/// Builds Blockchain block header.
/// All methods except BuildDefault marked as private
/// because of determinacy of the process
/// </summary>
/// <param name="blockStream"></param>
internal class BlockchainBlockHeaderBuilder(Stream blockStream)
{
    private byte[] _rawData = new byte[BlockchainBlockHeaderSizes.Total];

    private string? _hash;
    
    private int? _version;

    private string? _previousBlockHash;

    private string? _merkleRoot;

    private DateTime? _timestamp;

    private uint? _bits;

    private uint? _nonce;

    public async Task<BlockchainBlockHeader> BuildDefaultAsync()
    {
        await LoadRawDataAsync();
        
        return WithHash()
            .WithVersion()
            .WithPreviousBlockHash()
            .WithMerkleRoot()
            .WithTimestamp()
            .WithBits()
            .WithNonce()
            .Build();
    }

    private async Task LoadRawDataAsync()
    {
        _rawData = await BytesHelper.ReadInfoAsync(blockStream, BlockchainBlockHeaderSizes.Total);
    }
    
    private BlockchainBlockHeaderBuilder WithHash()
    {
        _hash = BytesHelper.BytesToString(BytesHelper.DoubleHash(_rawData));
        
        return this;
    }

    private BlockchainBlockHeaderBuilder WithVersion()
    {
        var versionBytes = _rawData
            .Take(BlockchainBlockHeaderSizes.Version)
            .ToArray();
        
        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(versionBytes);
        
        _version = BitConverter.ToInt32(versionBytes);

        return this;
    }
    
    private BlockchainBlockHeaderBuilder WithPreviousBlockHash()
    {
        const int skipBytes = BlockchainBlockHeaderSizes.Version;
        
        var previousBlockHashBytes = _rawData
            .Skip(skipBytes)
            .Take(BlockchainBlockHeaderSizes.PreviousBlockHash)
            .ToArray();
        
        if (BitConverter.IsLittleEndian) // by default data stored in natural byte order
            Array.Reverse(previousBlockHashBytes);
        
        _previousBlockHash = BytesHelper.BytesToString(previousBlockHashBytes);
        
        return this;
    }
    
    private BlockchainBlockHeaderBuilder WithMerkleRoot()
    {
        const int skipBytes = BlockchainBlockHeaderSizes.Version 
                              + BlockchainBlockHeaderSizes.PreviousBlockHash;

        var merkleRoot = _rawData
                .Skip(skipBytes)
                .Take(BlockchainBlockHeaderSizes.MerkleRoot)
                .ToArray();
        
        if (BitConverter.IsLittleEndian) // by default data stored in natural byte order
            Array.Reverse(merkleRoot);
        
        _merkleRoot = BytesHelper.BytesToString(merkleRoot);

        return this;
    }
    
    private BlockchainBlockHeaderBuilder WithTimestamp()
    {
        const int skipBytes = BlockchainBlockHeaderSizes.Version 
                              + BlockchainBlockHeaderSizes.PreviousBlockHash
                              + BlockchainBlockHeaderSizes.MerkleRoot;
        
        var timestampBytes = _rawData
            .Skip(skipBytes)
            .Take(BlockchainBlockHeaderSizes.Timestamp)
            .ToArray();
        
        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(timestampBytes);
        
        var timestampUint = BitConverter.ToUInt32(timestampBytes);
        _timestamp = DateTime.UnixEpoch.AddSeconds(Convert.ToDouble(timestampUint)); 
        // used explicit type conversion to avoid bugs or unclear points
        
        return this;
    }
    
    private BlockchainBlockHeaderBuilder WithBits()
    {
        const int skipBytes = BlockchainBlockHeaderSizes.Version 
                              + BlockchainBlockHeaderSizes.PreviousBlockHash
                              + BlockchainBlockHeaderSizes.MerkleRoot
                              + BlockchainBlockHeaderSizes.Timestamp;
        
        var bitsBytes = _rawData
            .Skip(skipBytes)
            .Take(BlockchainBlockHeaderSizes.Bits)
            .ToArray();
        
        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(bitsBytes);
        
        _bits = BitConverter.ToUInt32(bitsBytes);

        return this;
    }
    
    private BlockchainBlockHeaderBuilder WithNonce()
    {
        var nonceBytes = _rawData
            .TakeLast(BlockchainBlockHeaderSizes.Nonce)
            .ToArray();

        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(nonceBytes);
        
        _nonce = BitConverter.ToUInt32(nonceBytes);
        
        return this;
    }
    
    private BlockchainBlockHeader Build()
    {
        return new BlockchainBlockHeader()
        {
            Hash = _hash!,
            Version = _version!.Value,
            PreviousBlockHash = _previousBlockHash!,
            MerkleRoot = _merkleRoot!,
            Timestamp = _timestamp!.Value,
            Bits = _bits!.Value,
            Nonce = _nonce!.Value
        };
    }
}