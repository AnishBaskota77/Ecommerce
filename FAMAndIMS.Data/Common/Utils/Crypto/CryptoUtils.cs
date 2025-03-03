using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Common.Utils.Crypto
{
    public class CryptoUtils
    {
        public static bool CheckEqualHashHmacSha512(string valueToCheck, string hashValue, string saltKey)
        {
            if (string.IsNullOrWhiteSpace(valueToCheck)) throw new ArgumentNullException(nameof(valueToCheck));
            if (string.IsNullOrWhiteSpace(hashValue)) throw new ArgumentNullException(nameof(hashValue));
            if (string.IsNullOrWhiteSpace(saltKey)) throw new ArgumentNullException(nameof(saltKey));

            using var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(saltKey));
            var hashCurrentComputed = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes(valueToCheck));

            return Convert.ToBase64String(hashCurrentComputed).Equals(hashValue);
        }
        public static string HashHmacSha512(string value, string saltKey)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrWhiteSpace(saltKey)) throw new ArgumentNullException(nameof(saltKey));

            using var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(saltKey));
            var hash = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes(value));

            return Convert.ToBase64String(hash);
        }
        public static string GenerateKeySalt(int saltLength = 128)
        {
            byte[] bytesBuffer = new byte[saltLength * 2];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytesBuffer);
            }

            return Convert.ToBase64String(bytesBuffer)[..saltLength];
        }
    }
}
