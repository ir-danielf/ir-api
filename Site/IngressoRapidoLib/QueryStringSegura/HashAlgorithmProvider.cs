using System.Security.Cryptography;

namespace IngressoRapido.QueryStringSegura
{
    /// <summary>
    /// Provides hash functions for any <see cref="HashAlgorithm"/>.  By default,
    /// this provider uses the <see cref="MD5"/> algorithm.
    /// </summary>
	public class HashAlgorithmProvider : IHashProvider
	{
        private HashAlgorithm algorithm;

        /// <summary>
        /// Creates a default instance of this provider.
        /// </summary>
        public HashAlgorithmProvider() : this(MD5CryptoServiceProvider.Create()) {}

        /// <summary>
        /// Creates an instance with a specified hash algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to use to compute the hash.</param>
        public HashAlgorithmProvider(HashAlgorithm algorithm) 
        {
            this.algorithm = algorithm;
        }

        /// <summary>
        /// Computes the hash value of a byte array using the specified algorithm.
        /// </summary>
        /// <param name="buffer">The buffer to hash.</param>
        /// <returns>The computed hash.</returns>
        public byte[] Hash(byte[] buffer) 
        {
            return algorithm.ComputeHash(buffer);
        }
	}
}
