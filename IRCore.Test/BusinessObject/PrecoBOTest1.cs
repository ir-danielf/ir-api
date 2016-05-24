using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class PrecoBOTest1
    {

        private PrecoBO precoBO = new PrecoBO();

        [TestMethod]
        public void PrecoBO_ConsultarParceiro()
        {
            // teste do método ConsultarParceiro(int setorId, int apresentacaoId)

            var preco = precoBO.ConsultarParceiro(17910, 16653, 1);
            Assert.IsNull(preco);

            preco = precoBO.ConsultarParceiro(17910, 167653, 1);
            Assert.AreEqual("Media Partner", preco.Nome);
        }
    }
}
