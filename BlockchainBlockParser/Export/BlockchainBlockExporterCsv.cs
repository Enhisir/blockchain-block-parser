using System.Globalization;
using AutoMapper;
using BlockchainBlockParser.Block;
using BlockchainBlockParser.Export.Mapping;
using CsvHelper;

namespace BlockchainBlockParser.Export;

public class BlockchainBlockExporterCsv : IBlockchainBlockExport
{
    private MapperConfiguration _config = new (cfg =>
    {
        cfg.CreateMap<BlockchainBlock, BlockchainBlockDto>()
            .ForPath(dest => dest.Version, 
                opt => opt.MapFrom(src => src.Header.Version))
            .ForPath(dest => dest.PreviousBlockHash, 
            opt => opt.MapFrom(src => src.Header.PreviousBlockHash))
            .ForPath(dest => dest.MerkleRoot, 
                opt => opt.MapFrom(src => src.Header.MerkleRoot))
            .ForPath(dest => dest.Timestamp, 
                opt 
                    => opt.MapFrom(src => (src.Header.Timestamp - DateTime.UnixEpoch.ToUniversalTime()).TotalSeconds))
            .ForPath(dest => dest.Bits, 
                opt => opt.MapFrom(src => src.Header.Bits))
            .ForPath(dest => dest.Nonce, 
                opt => opt.MapFrom(src => src.Header.Nonce))
            .ForPath(dest => dest.Size, 
                opt => opt.MapFrom(src => src.Header.Size))
            .ForPath(dest => dest.TransactionCount,
                opt => opt.MapFrom(src => src.TransactionCount.Count));
        
        cfg.CreateMap<Transaction.Transaction, TransactionDto>()
                .ForPath(dest => dest.InputCount,
                    opt => opt.MapFrom(src => src.InputCount.Count))
                .ForPath(dest => dest.OutputCount, 
                    opt => opt.MapFrom(src => src.OutputCount.Count));
    });
    
    public async Task ExportAsync(BlockchainBlock block, string directoryPath)
    {
        var mapper = new Mapper(_config);

        await ExportBlockAsync(mapper, block, directoryPath);
        await ExportBlockTransactionsAsync(mapper, block, directoryPath);
    }

    private async Task ExportBlockAsync(Mapper mapper, BlockchainBlock block, string directoryPath)
    {
        var blockDto = mapper.Map<BlockchainBlockDto>(block);
        var blockInfoPath = Path.Combine(directoryPath, $"block-{block.Hash}-info.csv");
        await using var blockWriter = new StreamWriter(blockInfoPath);
        await using var blockCsv = new CsvWriter(blockWriter, CultureInfo.InvariantCulture);
        blockCsv.WriteHeader<BlockchainBlockDto>();
        await blockCsv.NextRecordAsync();
        await blockCsv.WriteRecordsAsync([ blockDto ]);
    }
    
    private async Task ExportBlockTransactionsAsync(Mapper mapper, BlockchainBlock block, string directoryPath)
    {
        var transactionsDto = mapper.Map<List<TransactionDto>>(block.Transactions);
        var blockInfoPath = Path.Combine(directoryPath, $"block-{block.Hash}-transactions.csv");
        await using var blockWriter = new StreamWriter(blockInfoPath);
        await using var blockCsv = new CsvWriter(blockWriter, CultureInfo.InvariantCulture);
        blockCsv.WriteHeader<TransactionDto>();
        await blockCsv.NextRecordAsync();
        await blockCsv.WriteRecordsAsync(transactionsDto);
    }
}