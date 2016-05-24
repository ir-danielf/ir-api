using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.Model;
using System.Linq;
using IRCore.Util;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class EventoADOTest1 : MasterTest
    {
        private EventoADO eventoADO;
        
        public EventoADOTest1() : base(){
            eventoADO = new EventoADO(ado);
        }


        [TestMethod]
        public void EventoADO_ListarParceiro2()
        {
            //teste do método ListarParceiro(int pageNumber, int pageSize, int idParceiro, bool lazyLoadingEnabled = false)            
        
        }


        [TestMethod]
        public void EventoADO_Consultar()
        {
            var evento = eventoADO.Consultar(28726);
            Assert.IsNull(evento);

            //teste do método Consultar(int idEvento, bool lazyLoadingEnabled = true)
            evento = eventoADO.Consultar(33212);
            Assert.AreEqual(evento.Nome, "700 Mil Horas");
        }

        [TestMethod]
        public void EventoADO_Consultar_InfoMeia()
        {
            var infoLeiMeia = eventoADO.ConsultarInfoMeiaEntrada(45894, 2);
            
        }

    }
}