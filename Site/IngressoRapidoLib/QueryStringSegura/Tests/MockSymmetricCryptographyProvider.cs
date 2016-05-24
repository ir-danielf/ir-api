#if UNIT_TESTS
using System;
using NUnit.Framework;

namespace IngressoRapido.QueryStringSegura.Tests
{
	public class MockSymmetricCryptoProvider : ISymmetricCryptoProvider
    {
        public byte[] Encrypt(byte[] plaintext)
        {
            return null;
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            return null;
        }
    }
}
#endif