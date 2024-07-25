using System.ComponentModel.DataAnnotations;

namespace BlockchainBlockParser.CompactSize;

public static class CompactSizeHelper
{
    public static async Task<CompactSize> ParseAsync(Stream blockStream)
    {
        await using var dataStream = new MemoryStream();
        
        var countOrSizeType = blockStream.ReadByte();
        dataStream.Write([ Convert.ToByte(countOrSizeType) ]);

        if (countOrSizeType <= (int)CompactSizeType.FC)
        {
            return new CompactSize() { SizeType = CompactSizeType.FC, Count = countOrSizeType };
        }

        var countBytesSize = Convert.ToInt32(Math.Pow(2, countOrSizeType - (int)CompactSizeType.FC));
        var countBytes = new byte[CountCompactSizeBytesFromType(CompactSizeType.FF)];
        var resultSize = await blockStream.ReadAsync(countBytes, 0, countBytesSize);
        if (resultSize != countBytesSize) throw new ValidationException();
        dataStream.Write(countBytes);
        
        if (!BitConverter.IsLittleEndian) // by default data stored in Little-Indian
            Array.Reverse(countBytes);

        return new CompactSize()
        {
            SizeType = (CompactSizeType)countOrSizeType,
            Count = BitConverter.ToInt64(countBytes),
            RawData = dataStream.ToArray()
        };
    }

    public static int CountCompactSizeBytesFromType(CompactSizeType type) 
        => type is CompactSizeType.FC 
            ? 1 
            : 1 + Convert.ToInt32(Math.Pow(2, type - CompactSizeType.FC));
}