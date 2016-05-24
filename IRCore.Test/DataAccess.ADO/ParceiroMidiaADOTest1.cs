using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class ParceiroMidiaADOTest1 : MasterTest
    {
        private ParceiroMidiaADO parceiroMidiaADO;
        private ParceiroMidiaEventoADO parceiroMidiaEventoADO;

        public ParceiroMidiaADOTest1()
            : base()
        {
            parceiroMidiaADO = new ParceiroMidiaADO(ado);
            parceiroMidiaEventoADO = new ParceiroMidiaEventoADO(ado);
        }

        [TestMethod]
        public void ParcerioMidiaADOT_Consultar()
        {
            //teste do método Consultar(string urlContexto, bool lazyLoadingEnabled = false)
            var parceiro = parceiroMidiaADO.Consultar("Elemidia");
            Assert.AreEqual("Elemidia", parceiro.Nome);
        }

        [TestMethod]
        public void ParcerioMidiaADOT_Consultar2()
        {
            //teste do método Consultar(int parceiroId, bool lazyLoadingEnabled = false)
            var parceiro = parceiroMidiaADO.Consultar(1);
            Assert.AreEqual("Elemidia", parceiro.Nome);
        }

        [TestMethod]
        public void ParcerioMidiaADOT_Listar()
        {
            //teste do método Listar(int pageNumber, int pageSize, string[] busca = null, bool lazyLoadingEnabled = false)
            var parceiro = parceiroMidiaADO.Listar(1, 1);
            Assert.AreNotEqual(0, parceiro.Count);
            if (parceiro.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(parceiro[0].Nome));
            }
        }

        [TestMethod]
        public void ParcerioMidiaADOT_Listar2()
        {
            //teste do método Listar(string[] busca = null, bool lazyLoadingEnabled = false)
            var parceiro = parceiroMidiaADO.Listar();
            Assert.AreNotEqual(0, parceiro.Count);
            if (parceiro.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(parceiro[0].Nome));
            }
        }

        [TestMethod]
        public void ParcerioMidiaEventoADOT_Comparar()
        {
            var p1 = parceiroMidiaEventoADO.Consultar(1, 1);
            var p2 = parceiroMidiaEventoADO.Consultar(1, 1);
            Assert.AreEqual(null, p1);
            Assert.AreEqual(null,p2);
        }
        [TestMethod]
        public void ParcerioMidiaEventoADOT_CompararUrlContexto()
        {
            var p1 = parceiroMidiaADO.Consultar("elemidia");
            var p2 = parceiroMidiaADO.Consultar("elemidia");
            Assert.AreEqual(p2.ID, p1.ID);
            
        }
    }
}
