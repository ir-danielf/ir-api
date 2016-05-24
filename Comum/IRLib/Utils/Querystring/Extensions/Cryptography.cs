using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Extensions
{
    public static class CryptographyExtensions
    {
        public static string Encrypt(this string str)
        {
            return Utils.Cryptography.Encryption.EncryptText(str);
        }

        public static string Decrypt(this string str)
        {
            return Utils.Cryptography.Encryption.DecryptText(str);
        }
    }
}
