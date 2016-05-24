using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using System.Linq;
using System.Collections.Generic;
using IRCore.DataAccess.Model;
using IRCore.BusinessObject;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class CarrinhoADOTest1 : MasterTest
    {
        private CarrinhoADO carrinhoADO;

        public CarrinhoADOTest1()
            : base()
        {
            carrinhoADO = new CarrinhoADO(ado);
        }

        [TestMethod]
        public void CarrinhoADO_ListarVoucher() //ajustar pois banco esta vazio
        {
            //teste do método ListarVoucher(int idVoucher, bool lazyLoadingEnabled = false)
            /*var carrinho = carrinhoADO.ListarVoucher(1);

            Assert.AreNotEqual(0, carrinho.Count);
            if (carrinho.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(carrinho[0].Evento));
            } */
        }

        [TestMethod]
        public void CarrinhoADO_ListarVoucher2() //ajustar pois banco esta vazio
        {
            //teste do método ListarVoucher(int idVoucher, string sessionId, bool lazyLoadingEnabled = false)
            /* var carrinho = carrinhoADO.ListarVoucher(1, "");

           Assert.AreNotEqual(0, carrinho.Count);
            if (carrinho.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(carrinho[0].Evento));
            } */
        }

        [TestMethod]
        public void CarrinhoADO_ValidaPrecoID()
        {
            CarrinhoADO carrinho = new CarrinhoADO();
            Assert.IsTrue(carrinho.ValidaPrecoID(163464, 15278, 2210927));
            Assert.IsFalse(carrinho.ValidaPrecoID(163464, 15278, 1424));
        }
    }
}