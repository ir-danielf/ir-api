using System;
using System.Security.Cryptography;
using System.Text;

namespace IRLib.Paralela.Criptografia
{
    public static class Crypto
    {
        public static string Criptografar(string Texto, string ChavePrivada)
        {
            byte[] Resultados;
            UTF8Encoding UTF8 = new UTF8Encoding();

            //Tipo de hash MD5, poderia usar outro qualquer...
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(ChavePrivada));

            //Triple DES pra realmente usar 128 bits
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Texto);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Resultados = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Resultados);
        }

        public static string Decriptografar(string Texto, string ChavePrivada)
        {
            byte[] Resultados;

            UTF8Encoding UTF8 = new UTF8Encoding();

            //Hash de tipo MD5
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(ChavePrivada));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(Texto);

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Resultados = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Resultados);
        }
    }
}
