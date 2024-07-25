using BlockchainBlockParser.Block;

using var textStream = new StreamReader("00000000000005b2a3eb689758e3dba709d1af596f03175e965212701df4aa26.txt");

var blockText = textStream.ReadLine()!.Trim();

Console.WriteLine(blockText.Length / 2);

using var byteStream = new MemoryStream(Convert.FromHexString(blockText));
var blockBuilder = new BlockchainBlockBuilder(byteStream);

var block = await blockBuilder.BuildDefaultAsync();
Console.WriteLine(block.Hash);
Console.WriteLine(block.Size);
Console.WriteLine(block.Size - block.Header.Size - block.Transactions.Sum(tx => tx.Size));

/*var str = Convert.FromHexString("02000000019945a5a440f2d3712ff095cb1efefada1cc52e139defedb92a313daed49d5678010000006a473044022031b6a6b79c666d5568a9ac7c116cacf277e11521aebc6794e2b415ef8c87c899022001fe272499ea32e6e1f6e45eb656973fbb55252f7acc64e1e1ac70837d5b7d9f0121023dec241e4851d1ec1513a48800552bae7be155c6542629636bcaa672eee971dcffffffff01a70200000000000017a9148ce773d254dc5df886b95848880e0b40f10564328700000000");
var stream = new MemoryStream(str);
var txBuilder = new TransactionBuilder(stream);
var tx = await txBuilder.BuildDefaultAsync();*/

/*Console.WriteLine($"tx: {tx.Hash}");
foreach (var i in tx.Inputs)
{
    Console.WriteLine($"in {i}: {i.Hash}");
}*/
/*Console.WriteLine(BytesHelper.BytesToString(BytesHelper.DoubleHash(str)));*/
