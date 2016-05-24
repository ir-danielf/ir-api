#if UNIT_TESTS
using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace IngressoRapido.QueryStringSegura.Tests
{
	[TestFixture]
	public class HashAlgorithmProviderFixture
	{
        [Test]
        public void MD5Hash() 
        {
            byte[] plaintext = new byte[] {0,1,2,3};
            HashAlgorithmProvider provider = new HashAlgorithmProvider();
            byte[] hash = provider.Hash(plaintext);
            
            Assert.IsFalse(CommonUtil.CompareBytes(plaintext, hash));

            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] hash2 = md5.ComputeHash(plaintext);
            
            Assert.IsTrue(CommonUtil.CompareBytes(hash, hash2));
        }

        [Test]
        public void SHA1Hash() 
        {
            byte[] plaintext = new byte[] {0,2,3,4};
            HashAlgorithmProvider provider = new HashAlgorithmProvider(SHA1CryptoServiceProvider.Create());
            byte[] hash = provider.Hash(plaintext);

            Assert.IsFalse(CommonUtil.CompareBytes(plaintext, hash));

            SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            byte[] hash2 = sha1.ComputeHash(plaintext);

            Assert.IsTrue(CommonUtil.CompareBytes(hash, hash2));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashNullBytes() 
        {
            HashAlgorithmProvider provider = new HashAlgorithmProvider();
            byte[] hash = provider.Hash(null);
        }
	}
}
#endif