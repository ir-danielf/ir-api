using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class LocalADOTest1 : MasterTest
    {

        private LocalADO localADO;

        public LocalADOTest1()
            : base()
        {
            localADO = new LocalADO(ado);
        }

        [TestMethod]
        public void TLocalADO_Consultar()
        {
            // teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var local = localADO.Consultar(999999999);
            Assert.IsNull(local);

            local = localADO.Consultar(6);
            Assert.AreEqual("HSBC Brasil", local.Nome);
        }
    }
}
