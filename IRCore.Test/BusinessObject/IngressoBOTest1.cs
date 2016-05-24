using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRLib;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class IngressoBOTest1
    {
        private IngressoBO ingressoBO = new IngressoBO();

        [TestMethod]
        public void IngressoBO_ListarParceiroStatus()
        {
            // teste do método ListarParceiroStatus(int parceiroId, int setorId, int apresentacaoId, enumIngressoStatus status)
            var ingresso = ingressoBO.ListarParceiroStatus(1, 17910, 167669, enumIngressoStatus.bloqueado, 2);

            Assert.AreNotEqual(0, ingresso.Count);

            if (ingresso.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(ingresso[0].Codigo));
            }

            var listarParceiroStatus = ingressoBO.ListarParceiroStatus(1, 17910, 167653, enumIngressoStatus.bloqueado, 2);
            Assert.AreNotEqual(0, listarParceiroStatus.Count);
        }

        [TestMethod]
        public void IngressoBO_Reservar()
        { 
            
        }

        [TestMethod]
        public void GerarCodigoBarra()
        {
            var ingressoUm = new Carrinho { IngressoID = 250954405 };
            var ingressoDois = new Carrinho { IngressoID = 250954455 };

            var ingressos = new List<Carrinho> { ingressoUm, ingressoDois };

            var ingressosId = ingressos.Select(x => (int)x.IngressoID).ToArray();
            var bilheteria = new Bilheteria();
            bilheteria.RegistrarImpressao(ingressosId, 21113, 2, 4627525, 2, 1537, false, 0, null, 0, false);

        }
    }
}
