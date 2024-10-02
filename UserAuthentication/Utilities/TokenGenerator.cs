using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Security.Cryptography;

namespace UserAuthentication.Utilities
{
    public class TokenGenerator
    {
        public static string GenerateConfirmationToken(int length =32)
        {
            var tokenBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }


            return Convert.ToBase64String(tokenBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

       
    }
}
