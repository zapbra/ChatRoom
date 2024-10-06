using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;
using System.Text;

namespace UserAuthentication.Security
{
    public static class HashAlgorithmUtility
    {
        // This will be changed and cleaned up at a later date
        // Just to keep track of, which hashing algorith is 
        public static string HA_NAME { get; } = "SHA256";
        public static string HA { get;  } = "SHA";
        public static string HA_VERSION { get;  } = "256";
        
        public static string GetHashAlgorithmDetail(string detail)
        {
            return detail.ToLower() switch 
                {
                "name" => HA_NAME,
                "algorithm" => HA_VERSION,
                "version" => HA_VERSION,
                _ => throw new ArgumentException("Invalid detail specified", nameof(detail))
                };
        }
        public static string HashPassword(string password)
        {
            // creates SHA256 instance
            SHA256 hash = SHA256.Create();
            // converts password to string of bytes, required for hashing
            var passwordBytes = Encoding.Default.GetBytes(password);
            // get hashed bytes
            var hashedPassword = hash.ComputeHash(passwordBytes);
            // return hashed bytes as string
            return Convert.ToHexString(hashedPassword);

        }

        public static string GenerateSalt(int length = 16)
        {
            var saltBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
    }
}
