using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using System.Linq;
using IRCore.DataAccess.Model;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class ApresentacaoADOTest1 : MasterTest
    {

        private ApresentacaoADO apresentacaoADO;
        
        public ApresentacaoADOTest1() : base(){
            apresentacaoADO = new ApresentacaoADO(ado);
        }
        

        [TestMethod]
        public void ApresentacaoADO_Consultar()
        {
            var apresentacao = apresentacaoADO.Consultar(45667778);
            Assert.IsNull(apresentacao);

            //teste do método Consultar(int apresentacaoId)
            apresentacao = apresentacaoADO.Consultar(151666);
            Assert.AreEqual(30206, apresentacao.EventoID);
            Assert.AreEqual("20140830163000", apresentacao.Horario);
        }

        [TestMethod]
        public void ApresentacaoADO_ConsultarMapaEsquematico()
        {
            //teste do método ConsultarMapaEsquematico(int apresentacaoId) por apresentação
            var apresentacao1 = apresentacaoADO.ConsultarMapaEsquematico(72737);
            Assert.AreEqual("Mapa Platéia A a N", apresentacao1.Nome);

            //teste do método ConsultarMapaEsquematico(int apresentacaoId) por evento
            var apresentacao2 = apresentacaoADO.ConsultarMapaEsquematico(156);
            Assert.AreEqual("Tom Jazz 1", apresentacao2.Nome);
        }
    }
}
