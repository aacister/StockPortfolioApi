using System;
using System.Security.Cryptography;
using System.Text;

namespace StockPortfolio.Api
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HMACSHA512 x = new HMACSHA512(Encoding.UTF8.GetBytes("secret"));

        public byte[] Hash(string password, byte[] salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password);

            var allBytes = new byte[bytes.Length + salt.Length];
            Buffer.BlockCopy(bytes, 0, allBytes, 0, bytes.Length);
            Buffer.BlockCopy(salt, 0, allBytes, bytes.Length, salt.Length);

            return x.ComputeHash(allBytes);
        }
    }
}