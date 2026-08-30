using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Utilities
{
    public class clsHasher
    {
        public static string Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashText = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashText).Replace("-", "").ToLower();
            }
        }
    }
}
