using BlockchainBlockParser.Block;
using BlockchainBlockParser.Export;

using var textStream = new StreamReader("00000000000005b2a3eb689758e3dba709d1af596f03175e965212701df4aa26.txt");
var blockText = textStream.ReadLine()!.Trim();

using var byteStream = new MemoryStream(Convert.FromHexString(blockText));
var blockBuilder = new BlockchainBlockBuilder(byteStream);

var block = await blockBuilder.BuildDefaultAsync();
Console.WriteLine($"block hash: {block.Hash}");
Console.WriteLine($"block size: {block.Size}");

var exporter = new BlockchainBlockExporterCsv();
await exporter.ExportAsync(block, ".");