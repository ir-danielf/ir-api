#if UNIT_TESTS
using System;
using NUnit.Framework;

namespace IngressoRapido.QueryStringSegura.Tests 
{
    [TestFixture]
    public class CommonUtilFixture 
    {
        [Test]
        public void CompareEqualBytes() 
        {
            byte[] array1 = new byte[] {0,1,2};
            byte[] array2 = new byte[] {0,1,2};

            Assert.IsTrue(CommonUtil.CompareBytes(array1, array2));
        }

        [Test]
        public void CompareNonEqualBytes() 
        {
            byte[] array1 = new byte[] {0,1,2};
            byte[] array2 = new byte[] {3,1,2};

            Assert.IsFalse(CommonUtil.CompareBytes(array1, array2));
        }
    }
}
#endif