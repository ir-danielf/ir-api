using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.Model.Enumerator;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class IngressoADOTest1 : MasterTest
    {
        private IngressoADO ingressoADO;
        
        public IngressoADOTest1() : base(){
            ingressoADO = new IngressoADO(ado);
        }

        [TestMethod]
        public void IngressoADO_Consultar()
        {
            //teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var listarParceiro = ingressoADO.Consultar(999999999);
            Assert.IsNull(listarParceiro);

            listarParceiro = ingressoADO.Consultar(119745);
            Assert.AreEqual(48, listarParceiro.PrecoID);
        }

        [TestMethod]
        public void IngressoADO_ListarParceiroStatus()
        {
            //teste do método ListarParceiroStatus(int parceiroId, int setorId, int apresentacaoId, enumIngressoStatus status, bool lazyLoadingEnabled = false)
            var listarParceiro = ingressoADO.ListarParceiroStatus(1, 17910, 167669, enumIngressoStatus.bloqueado,2);
           
            Assert.AreNotEqual(0, listarParceiro.Count);
            if (listarParceiro.Count > 0)
            {
               Assert.AreEqual(false, string.IsNullOrEmpty(listarParceiro[0].Codigo));
            }

            var listarParceiroVazio = ingressoADO.ListarParceiroStatus(1, 17910, 167653, enumIngressoStatus.bloqueado,2);
            Assert.AreNotEqual(0, listarParceiroVazio.Count);
        }
    }
}
