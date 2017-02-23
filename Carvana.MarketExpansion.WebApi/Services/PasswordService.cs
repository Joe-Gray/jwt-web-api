using System;
using System.Security.Cryptography;
using Carvana.MarketExpansion.WebApi.Exceptions;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class PasswordService : IPasswordService
    {
        //// References:
        //// https://crackstation.net/hashing-security.htm
        //// https://github.com/defuse/password-hashing/blob/master/PasswordStorage.cs

        private readonly int _saltBytes = 24;
        private readonly int _hashBytes = 18;
        private readonly int _pbkdf2Iterations = 64000;
        private readonly int _hashSections = 5;
        private readonly int _hashAlgorithmIndex = 0;
        private readonly int _iterationIndex = 1;
        private readonly int _hashSizeIndex = 2;
        private readonly int _saltIndex = 3;
        private readonly int _hashIndex = 4;
        private readonly char _delimiter = '.' ;
        private readonly string _hashAlgorithm = "sha1";

        public string HashPassword(string password)
        {
            var salt = CreateRandomSalt();
            var hash = CreateHashedPassword(password, salt, _pbkdf2Iterations, _hashBytes);
            var encodedSalt = Convert.ToBase64String(salt);
            var encodedHash = Convert.ToBase64String(hash);

            var parts =
                $"{_hashAlgorithm}{_delimiter}{_pbkdf2Iterations}{_delimiter}{hash.Length}{_delimiter}{encodedSalt}{_delimiter}{encodedHash}";

            return parts;
        }

        public bool IsPasswordValid(string password, string hashedPassword)
        {
            var split = hashedPassword.Split(_delimiter);
            GuardAgainstInvalidHash(split);

            var iterations = GetIterations(split);
            var salt = GetSalt(split);
            var hash = GetHash(split);

            GuardAgainstInvalidHashSize(split, hash.Length);
            
            var testHash = CreateHashedPassword(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private byte[] CreateRandomSalt()
        {
            using (var csprng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[_saltBytes];
                csprng.GetBytes(salt);
                return salt;
            }
        }
        
        private byte[] CreateHashedPassword(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        private void GuardAgainstInvalidHash(string[] splitHash)
        {
            if (splitHash.Length != _hashSections)
            {
                throw new InvalidHashException("Fields are missing from the password hash.");
            }

            if (splitHash[_hashAlgorithmIndex] != _hashAlgorithm)
            {
                throw new CannotPerformOperationException("Unsupported hash type.");
            }
        }

        private int GetIterations(string[] splitHash)
        {
            int iterations;
            if (!int.TryParse(splitHash[_iterationIndex], out iterations))
            {
                throw new InvalidHashException("Could not parse the iteration count as an integer.");
            }

            return iterations;
        }

        private byte[] GetSalt(string[] splitHash)
        {
            try
            {
                var salt = Convert.FromBase64String(splitHash[_saltIndex]);
                return salt;
            }
            catch (Exception ex)
            {
                throw new InvalidHashException("Base64 decoding of salt failed.", ex);
            }
        }

        private byte[] GetHash(string[] splitHash)
        {
            try
            {
                var hash = Convert.FromBase64String(splitHash[_hashIndex]);
                return hash;
            }
            catch (Exception ex)
            {
                throw new InvalidHashException("Base64 decoding of hash output failed.", ex);
            }
        }

        private void GuardAgainstInvalidHashSize(string[] splitHash, int hashLength)
        {
            int storedHashSize;
            if (!int.TryParse(splitHash[_hashSizeIndex], out storedHashSize))
            {
                throw new InvalidHashException("Could not parse the hash size as an integer.");
            }

            if (storedHashSize != hashLength)
            {
                throw new InvalidHashException("Hash length doesn't match stored hash length.");
            }
        }

        private bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
