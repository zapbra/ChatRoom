using System.Security.Cryptography;
using System.Text;

namespace UserAuthentication.Security
{
    public static class HashAlgorithmUtility
    {
        
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
