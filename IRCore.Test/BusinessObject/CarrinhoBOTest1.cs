using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using IRCore.BusinessObject.Models;
using System.Web;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class CarrinhoBOTest1
    {
        CarrinhoBO carrinhoBO = new CarrinhoBO();

        [TestMethod]
        public void CarrinhoBO_ListarVoucher() //ajustar pois banco esta vazio
        {
            //teste do método ListarVoucher(int idVoucher)
            /*var carrinho = carrinhoBO.ListarVoucher(1);

            Assert.AreNotEqual(0, carrinho.Count);
            if (carrinho.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(carrinho[0].Evento));
            } */
        }

        [TestMethod]
        public void CarrinhoBO_ListarVoucher2() //ajustar pois banco esta vazio
        {
            //teste do método ListarVoucher(int idVoucher, string sessionId)
            /*var carrinho = carrinhoBO.ListarVoucher(1, "");

           Assert.AreNotEqual(0, carrinho.Count);
            if (carrinho.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(carrinho[0].Evento));
            }*/
        }

        [TestMethod]
        public void CarrinhoBO_Salvar()
        {
            // teste do método Salvar(Carrinho carrinho)

        }

        [TestMethod]
        public void CarrinhoBO_ReservarVoucher()
        {
            // teste do método ReservarVoucher(Voucher voucher, int setorId, int apresentacaoId, int quantidade, int clienteId = 0)
            // var carrinho = carrinhoBO.ReservarVoucher(, 1, 1, 1);
        }

        [TestMethod]
        public void ExpirarVoucher()
        {
            // teste do método ExpirarVoucher(int voucherId)
            // carrinhoBO.ExpirarVoucher(1);
        }




        [TestMethod]
        public void SolicitaReservaLugarMarcado()
        {
            try
            {
                CarrinhoBO CarrinhoBO = new CarrinhoBO();
                CompraModel compra = new CompraModel();
                compra.SessionID = "1sadasdlasdjkahskjdAA";

                //Logado Maicon
                ClienteBO ClienteBO = new ClienteBO(1);
                //compra.Login = ClienteBO.ConsultarCPF("01259436012");

                RetornoModel<CompraModel> result = CarrinhoBO.SolicitarReservaIngresso(compra, 176794654, 2210938);

                Console.WriteLine(result.Mensagem);
                Console.WriteLine(result.Retorno);
                Console.WriteLine(result.Sucesso);

                Assert.AreNotEqual(null, result);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [TestMethod]
        public void SolicitaReservaMesaFechada()
        {
            try
            {
                CarrinhoBO CarrinhoBO = new CarrinhoBO();
                CompraModel compra = new CompraModel();
                compra.SessionID = "1sadasdlasdjkahskjdAA";

                //Logado Maicon
                ClienteBO ClienteBO = new ClienteBO(1);
                //compra.Login = ClienteBO.ConsultarCPF("01259436012");

                RetornoModel<CompraModel> result = CarrinhoBO.SolicitarReservaMesaFechada(compra, 163464, 2199235, 1637525);

                Console.WriteLine(result.Mensagem);
                Console.WriteLine(result.Retorno);
                Console.WriteLine(result.Sucesso);

                Assert.AreNotEqual(null, result);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void SolicitaReservaMesaAberta()
        {
            try
            {
                CarrinhoBO CarrinhoBO = new CarrinhoBO();
                CompraModel compra = new CompraModel();
                compra.SessionID = "1sadasdlasdjkahskjdAA";

                //Logado Maicon
                ClienteBO ClienteBO = new ClienteBO(1);
                //compra.Login = ClienteBO.ConsultarCPF("01259436012");

                RetornoModel<CompraModel> result = CarrinhoBO.SolicitarReservaMesaAberta(compra, 163464, 1637525, 2, 0);

                Console.WriteLine(result.Mensagem);
                Console.WriteLine(result.Retorno);
                Console.WriteLine(result.Sucesso);

                Assert.AreNotEqual(null, result);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }



        [TestMethod]
        public void SolicitaReservaPacote()
        {
            try
            {
                var carrinhoBO_ = new CarrinhoBO();
                var compra = new CompraModel { SessionID = "mm21sadasdlasdjkahskjdAA" };

                var pacotesQuantidade = new List<PacoteReservaModel>
                {
                    new PacoteReservaModel {PacoteID = 8979, Quantidade = 1}
                };

                var retorno = carrinhoBO_.SolicitarReservaPacote(compra, pacotesQuantidade);
                Console.WriteLine(retorno.Mensagem);
                Console.WriteLine(retorno.Retorno);
                Console.WriteLine(retorno.Sucesso);
                Assert.AreNotEqual(null, retorno);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
