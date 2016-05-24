using System;
using System.IO;
using System.Security.Cryptography;

namespace IngressoRapido.QueryStringSegura
{
	/// <summary>
	/// Provides cryptographic functions for any <see cref="SymmetricAlgorithm"/>.  
	/// By default, this provider uses the <see cref="RijndaelManaged"/> (AES) algorithm.
	/// </summary>
	/// <remarks>
	/// The IV (Initialization Vector) is randomly generated and prepended to the resulting
	/// ciphertext. This is because the IV ultimately uses the same distribution as the 
	/// ciphertext.
	/// </remarks>
	public class SymmetricAlgorithmProvider : ISymmetricCryptoProvider
	{
        int IVSize;
        SymmetricAlgorithm algorithm;
        
        /// <summary>
        /// Creates an instance using <see cref="RijndaelManaged"/> as the algorithm.
        /// </summary>
        /// <param name="key">The key to use for this algorithm.</param>
        public SymmetricAlgorithmProvider(byte[] key) : this(RijndaelManaged.Create(), key)
        {
        }

        /// <summary>
        /// Creates an instance with a specified algorithm and key.
        /// </summary>
        /// <param name="algorithm">The algorithm to use for cryptographic functions.</param>
        /// <param name="key">The key to use for this algorithm.</param>
		public SymmetricAlgorithmProvider(SymmetricAlgorithm algorithm, byte[] key)
		{
		    this.algorithm = algorithm;
            algorithm.Key = key;
            algorithm.GenerateIV();
            IVSize = algorithm.IV.Length;
		}

        /// <summary>
        /// Encrypts a plaintext value.
        /// </summary>
        /// <param name="plaintext">The value to encrypt.</param>
        /// <returns>The resulting ciphertext.</returns>
        public byte[] Encrypt(byte[] plaintext) 
        {
            ValidateByteArrayParam("plaintext", plaintext);
         
            algorithm.GenerateIV();
   
            byte[] ciphertext = null;
            using (ICryptoTransform transform = (ICryptoTransform)algorithm.CreateEncryptor()) 
            {
                ciphertext = Transform(transform, plaintext);
            }

            return PrependIVToCipher(ciphertext);
        }

        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <returns>The resulting plaintext.</returns>
        public byte[] Decrypt(byte[] ciphertext) 
        {
            ValidateByteArrayParam("ciphertext", ciphertext);

            algorithm.IV = GetIVFromCipher(ciphertext);

            byte[] plaintext = null;
            using (ICryptoTransform transform = (ICryptoTransform)algorithm.CreateDecryptor()) 
            {
                plaintext = Transform(transform, StripIVFromCipher(ciphertext));
            }
            
            return plaintext;
        }

        private byte[] Transform(ICryptoTransform transform, byte[] buffer) 
        {
            byte[] returnBuffer = null;

            using (MemoryStream stream = new MemoryStream()) 
            {
                CryptoStream cryptoStream = null; 
                try 
                {
                    cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                    cryptoStream.Write(buffer, 0, buffer.Length);
                    cryptoStream.FlushFinalBlock();
                    returnBuffer = stream.ToArray();
                } 
                finally 
                {
                    if (cryptoStream != null) cryptoStream.Close();
                }
            }   

            return returnBuffer;
        }

        private void ValidateByteArrayParam(string paramName, byte[] value) 
        {
            if (value == null || value.Length == 0) 
            {
                throw new ArgumentNullException(paramName);
            }
        }

        private byte[] PrependIVToCipher(byte[] ciphertext) 
        {
            byte[] buffer = new byte[ciphertext.Length + algorithm.IV.Length];
            Buffer.BlockCopy(algorithm.IV, 0, buffer, 0, algorithm.IV.Length);
            Buffer.BlockCopy(ciphertext, 0, buffer, algorithm.IV.Length, ciphertext.Length);

            return buffer;
        }

        private byte[] GetIVFromCipher(byte[] ciphertext) 
        {
            byte[] buffer = new byte[IVSize];
            Buffer.BlockCopy(ciphertext, 0, buffer, 0, IVSize);
            return buffer;
        }

        private byte[] StripIVFromCipher(byte[] ciphertext) 
        {
            byte[] buffer = new byte[ciphertext.Length - IVSize];
            Buffer.BlockCopy(ciphertext, IVSize, buffer, 0, buffer.Length);
            return buffer;
        }
	}
}
