using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Infrastructure;

namespace User.Infrastracture.Services
{
    public class PasswordService : IPasswordService
    {
        public bool ComparePasswordWithHash(string hash, string salt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            var computedHash = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(password), saltBytes));

            return hash == computedHash;
        }

        public (string hash, string salt) HashPassword(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(password), salt);

            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }
        private static byte[] GenerateSalt()
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                var salt = new byte[128 / 8];
                generator.GetBytes(salt);
                return salt;
            }
        }
        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }
    }
}
