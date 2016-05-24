using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class PrecoADOTest1: MasterTest
    {
        private PrecoADO precoADO;
        
        public PrecoADOTest1() : base(){
            precoADO = new PrecoADO(ado);
        }

        [TestMethod]
        public void PrecoADO_ConsultarParceiro()
        {
            // ConsultarParceiro(int setorId, int apresentacaoId, bool lazyLoadingEnabled = false)
            var preco = precoADO.ConsultarParceiro(17910, 16653, 1);
            Assert.IsNull(preco);

            preco = precoADO.ConsultarParceiro(17910, 167653, 1);
            Assert.AreEqual("Media Partner", preco.Nome);
        }
    }
}
