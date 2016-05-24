using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class LojaADOTest1 : MasterTest
    {

        private LojaADO lojaADO;

        public LojaADOTest1()
            : base()
        {
            lojaADO = new LojaADO(ado);
        }

        [TestMethod]
        public void LojaADO_Consultar()
        {
            // teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var loja = lojaADO.Consultar(999999999);
            Assert.IsNull(loja);

            loja = lojaADO.Consultar(1);
            Assert.AreEqual("POS C.C tarde", loja.Nome);
        }
    }
}
