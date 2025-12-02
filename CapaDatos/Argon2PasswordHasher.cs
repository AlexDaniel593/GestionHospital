using System;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace CapaDatos
{
    /// <summary>
    /// Custom password hasher using Argon2id algorithm
    /// Winner of the Password Hashing Competition 2015
    /// </summary>
    public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 4; // Number of iterations
        private const int MemorySize = 65536; // 64 MB
        private const int DegreeOfParallelism = 2; // Number of threads

        public string HashPassword(TUser user, string password)
        {
            // Generate a cryptographically secure random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using Argon2id
            byte[] hash = HashPasswordWithSalt(password, salt);

            // Combine salt and hash for storage
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Return Base64 encoded string
            return Convert.ToBase64String(hashBytes);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            try
            {
                // Decode the stored hash
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Extract salt and hash
                byte[] salt = new byte[SaltSize];
                byte[] hash = new byte[HashSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);
                Array.Copy(hashBytes, SaltSize, hash, 0, HashSize);

                // Hash the provided password with the extracted salt
                byte[] testHash = HashPasswordWithSalt(providedPassword, salt);

                // Compare the hashes using constant-time comparison
                if (CryptographicOperations.FixedTimeEquals(hash, testHash))
                {
                    return PasswordVerificationResult.Success;
                }

                return PasswordVerificationResult.Failed;
            }
            catch
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using (var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = DegreeOfParallelism;
                argon2.Iterations = Iterations;
                argon2.MemorySize = MemorySize;

                return argon2.GetBytes(HashSize);
            }
        }
    }
}
