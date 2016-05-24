using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class ParceiroMidiaBOTest1
    {

        private ParceiroMidiaBO parceiroMidiaBO = new ParceiroMidiaBO();

        [TestMethod]
        public void ParceiroMidiaBO_Consultar()
        {
            //teste do método Consultar(string urlContexto)
            var parceiro = parceiroMidiaBO.Consultar("Elemidia");
            Assert.AreEqual("Elemidia", parceiro.Nome);
        }
    }
}
