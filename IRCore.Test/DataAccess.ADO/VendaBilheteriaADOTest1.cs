using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class VendaBilheteriaADOTest1 : MasterTest
    {

        private VendaBilheteriaADO vendaBilheteriaADO;

        public VendaBilheteriaADOTest1()
            : base()
        {
            vendaBilheteriaADO = new VendaBilheteriaADO(ado);
        }

        [TestMethod]
        public void VendaBilheteriaADO_Consultar()
        {
            // teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var vendaBilheteria = vendaBilheteriaADO.Consultar(1);
            Assert.AreEqual("20051203184008", vendaBilheteria.DataVenda);
        }
    }
}
