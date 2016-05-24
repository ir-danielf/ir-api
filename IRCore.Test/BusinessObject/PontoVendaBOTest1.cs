using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class PontoVendaBOTest1
    {
        private PontoVendaBO pontoVendaBo = new PontoVendaBO();

        [TestMethod]
        public void PontoVendaBO_ListarCidade()
        {
            // teste do método ListarCidade(string cidade, string uf)
            //var listarCidade = pontoVendaBo.ListarCidade("Porto Alegre", "RS");

            //Assert.AreNotEqual(0, listarCidade.Count);
            //if (listarCidade.Count > 0)
            //{
            //    Assert.AreEqual(false, string.IsNullOrEmpty(listarCidade[0].Cidade));
            //}
        }
    }
}
