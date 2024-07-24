using System.ComponentModel.DataAnnotations;

namespace BlockchainBlockParser.CompactSize;

public static class CompactSizeHelper
{
    public static async Task<CompactSize> ParseAsync(Stream blockStream, int offset)
    {
        var countOrSizeType = blockStream.ReadByte();

        if (countOrSizeType <= (int)CompactSizeType.FC)
        {
            return new CompactSize() { SizeType = CompactSizeType.FC, Count = countOrSizeType };
        }

        var countBytesSize = Convert.ToInt32(Math.Pow(2, countOrSizeType - (int)CompactSizeType.FC));
        var countBytes = new byte[CountCompactSizeBytesFromType(CompactSizeType.FF)];
        var resultSize = await blockStream
            .ReadAsync(countBytes, offset, countBytesSize);
        if (resultSize != countBytesSize) throw new ValidationException();
        
        // Little-Indian
        Array.Reverse(countBytes);

        return new CompactSize()
        {
            SizeType = (CompactSizeType)countOrSizeType,
            Count = BitConverter.ToInt64(countBytes)
        };
    }

    public static int CountCompactSizeBytesFromType(CompactSizeType type) 
        => Convert.ToInt32(Math.Pow(2, type - CompactSizeType.FC));
}