#if UNIT_TESTS
using System;
using NUnit.Framework;

namespace IngressoRapido.QueryStringSegura.Tests
{
    [TestFixture]
    public class SecureQueryStringFixture 
    {
        readonly byte[] key = new byte[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};
        
        [Test]
        public void AddParameters() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            secureQueryString["test"] = "value";
            Assert.AreEqual(secureQueryString["test"], "value");
        }

        [Test]
        public void GetAndSetParameters() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            secureQueryString["test"] = "value";
            SecureQueryString inputString = new SecureQueryString(key, secureQueryString.ToString());
            Assert.AreEqual("value", inputString["test"]);
        }

        [Test]
        public void LongParameters() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            secureQueryString["test"] = "valuevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevaluevalue";
            Console.WriteLine(secureQueryString);
            Console.WriteLine(secureQueryString.ToString().Length.ToString());
            Console.WriteLine(secureQueryString["test"].Length.ToString());
        }

        [Test]
        public void UpperBitParameters() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            string name = Char.ToString(Char.MaxValue);
            string value = Char.ToString((char)(Char.MaxValue - 1));
            secureQueryString[name] = value;
            Assert.IsNotNull(secureQueryString.ToString());
        }

        [Test]
        [ExpectedException(typeof(InvalidQueryStringException))]
        public void BadString() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key, "gey45yvehgf!");
        }

        [Test]
        public void ExpireTimeProperty() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            secureQueryString.ExpireTime = DateTime.Now.Add(TimeSpan.FromMinutes(1));
            SecureQueryString inputString = new SecureQueryString(key, secureQueryString.ToString());
            Assert.AreEqual(0, DateTime.Compare(secureQueryString.ExpireTime.Date, inputString.ExpireTime.Date));
            Assert.AreEqual(secureQueryString.ExpireTime.Hour, inputString.ExpireTime.Hour);
            Assert.AreEqual(secureQueryString.ExpireTime.Minute, inputString.ExpireTime.Minute);
            Assert.AreEqual(secureQueryString.ExpireTime.Second, inputString.ExpireTime.Second);
        }

        [Test]
        public void SymmetricCryptoProviderProperty() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            ISymmetricCryptoProvider provider = new MockSymmetricCryptoProvider();
            secureQueryString.SymmetricCryptoProvider = provider;
            Assert.AreSame(provider, secureQueryString.SymmetricCryptoProvider);
        }

        [Test]
        public void HashProviderProperty() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            IHashProvider provider = new MockHashProvider();
            secureQueryString.HashProvider = provider;
            Assert.AreSame(provider, secureQueryString.HashProvider);
        }

        [Test]
        [ExpectedException(typeof(ExpiredQueryStringException))]
        public void Expire() 
        {
            SecureQueryString secureQueryString = new SecureQueryString(key);
            secureQueryString.ExpireTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(1));
            SecureQueryString inputString = new SecureQueryString(key, secureQueryString.ToString());
        }
    }
}
#endif