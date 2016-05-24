#if UNIT_TESTS
using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace IngressoRapido.QueryStringSegura.Tests
{
    [TestFixture]
    public class SymmetricAlgorithmProviderFixture 
    {
        private byte[] key = new byte[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};

        [Test]
        public void AESEncryptAndDecrypt() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(key);
            byte[] plaintext = new byte[] {5,4,3,2};
            byte[] ciphertext = provider.Encrypt(plaintext);
            byte[] plaintext2 = provider.Decrypt(ciphertext);
         
            Assert.IsFalse(CommonUtil.CompareBytes(plaintext, ciphertext));   
            Assert.IsTrue(CommonUtil.CompareBytes(plaintext, plaintext2));
        }

        [Test]
        public void AESEncryptAndDecryptWithBadKey() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(key);
            byte[] plaintext = new byte[] {5,4,6,2};
            byte[] ciphertext = provider.Encrypt(plaintext);
            SymmetricAlgorithmProvider badProvider = new SymmetricAlgorithmProvider(new byte[]{1,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
            try 
            {
                byte[] plaintext2 = badProvider.Decrypt(ciphertext);
                Assert.IsFalse(CommonUtil.CompareBytes(plaintext, plaintext2));
            } 
            catch (CryptographicException) 
            {
            }
        }

        [Test]
        public void TripleDESEncryptAndDecrypt() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(TripleDESCryptoServiceProvider.Create(), key);
            byte[] plaintext = new byte[] {5,4,3,2};
            byte[] ciphertext = provider.Encrypt(plaintext);
            byte[] plaintext2 = provider.Decrypt(ciphertext);
         
            Assert.IsFalse(CommonUtil.CompareBytes(plaintext, ciphertext));   
            Assert.IsTrue(CommonUtil.CompareBytes(plaintext, plaintext2));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullBytes() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(key);
            provider.Encrypt(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptZeroBytes() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(key);
            provider.Encrypt(new byte[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullBytes() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(key);
            provider.Decrypt(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptZeroBytes() 
        {
            SymmetricAlgorithmProvider provider = new SymmetricAlgorithmProvider(key);
            provider.Decrypt(new byte[0]);
        }
    }
}
#endif