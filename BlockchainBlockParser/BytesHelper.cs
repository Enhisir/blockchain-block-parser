using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace BlockchainBlockParser;

public static class BytesHelper
{
    public static byte[] DoubleHash(byte[] data, bool reverse = true)
    {
        var hash = SHA256.HashData(SHA256.HashData(data));
        if (reverse) Array.Reverse(hash);
        return hash;
    }

    public static string BytesToString(byte[] data)
        => BitConverter.ToString(data)
            .Replace("-", "")
            .ToLower();
    
    public static async Task<byte[]> ReadInfoAsync(Stream byteStream, long size)
    {
        var resultBytes = new byte[size];
        var resultSize = await byteStream.ReadAsync(resultBytes);
        
        if (resultSize != size) throw new ValidationException();
        return resultBytes;
    }
}