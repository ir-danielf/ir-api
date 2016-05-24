
namespace IngressoRapido.QueryStringSegura
{
    /// <summary>
    /// A contract for hashing functions used by a <see cref="SecureQueryString"/>.
    /// </summary>
	public interface IHashProvider
	{
        /// <summary>
        /// Computes the hash value from bytes.
        /// </summary>
        /// <param name="buffer">The buffer to hash.</param>
        /// <returns>The computed hash.</returns>
        byte[] Hash(byte[] buffer);
	}
}
