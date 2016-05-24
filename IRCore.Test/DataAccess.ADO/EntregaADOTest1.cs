using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using System.Collections.Generic;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class EntregaADOTest1 : MasterTest
    {

        private EntregaADO entregaADO;

        public EntregaADOTest1()
            : base()
        {
            entregaADO = new EntregaADO(ado);
        }


        [TestMethod]
        public void EntregaADO_ListarEvento()
        {
            // teste do método ListarEvento(List<int> listEventos, DateTime dataApresentacaoMaisProxima)            
        }

        [TestMethod]
        public void EntregaADO_ListarEvento2()
        {
            // teste do método ListarEvento(List<int> listEventos, DateTime dataApresentacaoMaisProxima, int entregaId)
        }

        [TestMethod]
        public void EntregaADO_VerificarCEP()
        {
            // teste do método VerificarCEP(string cep, List<int> entregaControleIDs)
        }

        [TestMethod]
        public void EntregaADO_VerificarCEPBlackList()
        {
            // teste do método VerificarCEPBlackList(string cep) - SEM DADOS NO BANCO
            /*var entrega = entregaADO.VerificarCEPBlackList("00000000");
            Assert.IsNull(entrega);

            entrega = entregaADO.VerificarCEPBlackList("91360080");
            Assert.AreEqual(21, entrega.ID);*/
        }

        [TestMethod]
        public void EntregaADO_Compara_ListarInEventos()
        {
            List<int> p = new List<int>() { 386, 16130 };
            var l2 = entregaADO.ListarInEventos(p);
            var l1 = entregaADO.ListarInEventos(p);
            Assert.AreEqual(l1.Count, l2.Count);
        }
    }
}