using System.Security.Cryptography;

namespace LoreGlyph.Helpers;

public static class LinkGeneration
{
    public static string GenerateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
    }
}

