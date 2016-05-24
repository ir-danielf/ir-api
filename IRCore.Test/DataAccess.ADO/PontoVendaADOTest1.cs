using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class PontoVendaADOTest1 : MasterTest
    {
        private PontoVendaADO pontoVendaADO;
        
        public PontoVendaADOTest1() : base(){
            pontoVendaADO = new PontoVendaADO(ado);
        }

        [TestMethod]
        public void PontoVendaADO_ListarCidade()
        {
            ////teste do método ListarCidade(string cidade, string uf, bool lazyLoadingEnabled = true)
            //var pontoVenda = pontoVendaADO.ListarCidade("Porto Alegre","RS");

            //Assert.AreNotEqual(0, pontoVenda.Count);
            //if (pontoVenda.Count > 0)
            //{
            //    Assert.AreEqual(false, string.IsNullOrEmpty(pontoVenda[0].Cidade));
            //}
        }

        [TestMethod]
        public void PontoVendaADO_Consultar()
        {
            //teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var pontoVenda = pontoVendaADO.Consultar(99999999);
            Assert.IsNull(pontoVenda);
            
            pontoVenda = pontoVendaADO.Consultar(5);
            Assert.AreEqual("SHOW TICKETS", pontoVenda.Nome);
        }

        [TestMethod]
        public void PontoVendaADO_CompararID()
        {
            var pv1 = pontoVendaADO.Consultar(5);
            var pv2 = pontoVendaADO.Consultar(5);
            Assert.AreEqual(pv1.ID,pv2.ID);
        }

        [TestMethod]
        public void PontoVendaADO_CompararCidadeUF()
        {
            //var pv1 = pontoVendaADO.ListarCidade("Porto Alegre","RS");
            //var pv2 = pontoVendaADO.ListarCidade("Porto Alegre","RS");
            //Assert.AreEqual(pv1.Count, pv2.Count);
        }
    }
}
