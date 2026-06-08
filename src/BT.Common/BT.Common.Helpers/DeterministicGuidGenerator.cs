using System.Security.Cryptography;
using System.Text;

namespace BT.Common.Helpers;

public static class DeterministicGuidGenerator
{
    public static Guid GenerateDeterministicId(string hash, GuidHashType guidHashType)
    {
        return guidHashType == GuidHashType.SHA256
            ? GenerateDeterministicIdSHA256(hash)
            : GenerateDeterministicIdMd5(hash);
    }

    private static Guid GenerateDeterministicIdSHA256(string hash)
    {
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(hash));
        var byteData = SHA256.HashData(ms);

        // XOR folding: Fold the SHA-256 hash in half and XOR the two halves
        // https://en.wikipedia.org/wiki/XOR_swap_algorithm
        // The reason for it is that Guids are 128 bits, so we need to get 128 bits from the SHA256 hash
        var guidBytes = new byte[16];
        for (var i = 0; i < 16; i++)
        {
            guidBytes[i] = (byte)(byteData[i] ^ byteData[i + 16]);
        }

        return new Guid(guidBytes);
    }

    private static Guid GenerateDeterministicIdMd5(string hash)
    {
        var bytes = Encoding.UTF8.GetBytes(hash);

        var hashedVal = MD5.HashData(bytes);

        return new Guid(hashedVal);
    }
    public enum GuidHashType
    {
        SHA256 = 1,
        MD5,
    }
}

