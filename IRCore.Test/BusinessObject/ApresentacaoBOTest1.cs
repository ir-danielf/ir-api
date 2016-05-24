using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.ADO;
using System.Linq;
using IRCore.DataAccess.Model;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class ApresentacaoBOTest1 : MasterTest
    {
        ApresentacaoBO apresentacaoBO;

        public ApresentacaoBOTest1() : base()
        {
            apresentacaoBO = new ApresentacaoBO(ado);
        }
        

        [TestMethod]
        public void ApresentacaoBO_Consultar()
        {
            var apresentacao = apresentacaoBO.Consultar(45667778);
            Assert.IsNull(apresentacao);

            //teste do método Consultar(int apresentacaoId)
            apresentacao = apresentacaoBO.Consultar(151666);
            Assert.AreEqual(30206, apresentacao.EventoID);
            Assert.AreEqual("20140830163000", apresentacao.Horario);
        }

        [TestMethod]
        public void ApresentacaoBO_ConsultarMapaEsquematico()
        {
            //teste do método ConsultarMapaEsquematico(int apresentacaoId) por apresentação
            var apresentacao1 = apresentacaoBO.ConsultarMapaEsquematico(72737);
            Assert.AreEqual("Mapa Platéia A a N", apresentacao1.Nome);

            //teste do método ConsultarMapaEsquematico(int apresentacaoId) por evento
            var apresentacao2 = apresentacaoBO.ConsultarMapaEsquematico(156);
            Assert.AreEqual("Tom Jazz 1", apresentacao2.Nome);
        }
    }
}
