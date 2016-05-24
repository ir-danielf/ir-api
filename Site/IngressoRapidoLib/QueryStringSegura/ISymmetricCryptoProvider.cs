
namespace IngressoRapido.QueryStringSegura
{
    /// <summary>
    /// A contract for symmetric cryptographic functions used by a <see cref="SecureQueryString"/>.
    /// </summary>
    /// <remarks>
    /// You may have noticed that this interface does not require a key. This is to support
    /// "keyless" protection mechanisms such as DPAPI. Please review the implementation found
    /// in <see cref="SymmetricAlgorithmProvider"/> to understand how to use keys.
    /// </remarks>
	public interface ISymmetricCryptoProvider
	{
        /// <summary>
        /// Encrypts a plaintext value.
        /// </summary>
        /// <param name="plaintext">The value to encrypt.</param>
        /// <returns>The resulting ciphertext.</returns>
		byte[] Encrypt(byte[] plaintext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <returns>The resulting plaintext.</returns>
        byte[] Decrypt(byte[] ciphertext);
	}
}
