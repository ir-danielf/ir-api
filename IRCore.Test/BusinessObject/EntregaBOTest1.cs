using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.Model.Enumerator;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class EntregaBOTest1
    {

        EntregaBO entregaBO = new EntregaBO();
        CarrinhoBO carrinhoBO = new CarrinhoBO();

        /// <summary>
        /// Teste que deve retornar apenas entregas sem endereço.
        /// A apresentação mais próxima é no dia 27/08/2014 e o teste foi feito no dia 26/08/2014 desta forma ele não tem prazo para entrega a domicilio.
        /// </summary>
        [TestMethod]
        public void EntregaBO_ListarEntregasSemEndereco()
        {
            ClienteBO clienteBO = new ClienteBO(1);
            CompraModel compra = new CompraModel();
            compra.Login = clienteBO.Consultar(1);
            compra.CarrinhoItens = carrinhoBO.Listar("ev001", enumCarrinhoStatus.reservado);
            compra = entregaBO.Carregar(compra);
            Assert.AreEqual(true, compra.Login.Cliente.EnderecoList.Count == 0 && compra.EntregaControles.Count > 0);
        }

        /// <summary>
        /// Teste que deve retornar entregas com endereço (menos sedex) e sem endereço.
        /// A apresentação mais próxima é no dia 02/09/2014 e o teste foi feito no dia 26/08/2014 desta forma ele não tem prazo para entrega de sedex, mas é possível
        /// com mensageiro e/ou mensagerio agendado.
        /// </summary>
        [TestMethod]
        public void EntregaBO_ListarEntregasSemSedex()
        {
            ClienteBO clienteBO = new ClienteBO(1);
            CompraModel compra = new CompraModel();
            compra.Login = clienteBO.Consultar(1);
            compra.CarrinhoItens = carrinhoBO.Listar("ev002", enumCarrinhoStatus.reservado);
            compra = entregaBO.Carregar(compra);
            
            Assert.AreEqual(true, compra.Login.Cliente.EnderecoList.Count == 2 && compra.EntregaControles.Count > 0);
        }


        /// <summary>
        /// Teste que deve retornar entregas com endereço(menos mensageiro agendado) e sem endereço.
        /// A apresentação mais próxima é no dia 30/09/2014 e o teste foi feito no dia 26/08/2014
        /// </summary>
        [TestMethod]
        public void EntregaBO_ListarEntregasSemMensageiroAgendado()
        {
            ClienteBO clienteBO = new ClienteBO(1);
            CompraModel compra = new CompraModel();
            compra.Login = clienteBO.Consultar(1);
            compra.CarrinhoItens = carrinhoBO.Listar("ev003", enumCarrinhoStatus.reservado);
            compra = entregaBO.Carregar(compra);
            Assert.AreEqual(true, compra.Login.Cliente.EnderecoList.Count == 2 && compra.EntregaControles.Count > 0);
        }

        /// <summary>
        /// Teste que deve retornar entregas com endereço(Mensageiro,Mensageiro Agendado e Sedex) e sem endereço.
        /// A apresentação mais próxima é no dia 02/10/2014 e o teste foi feito no dia 26/08/2014 desta forma ele tem prazo para mensageiros e sedex
        /// </summary>
        [TestMethod]
        public void EntregaBO_ListarEntregasComMensageiroAgendadoESedex()
        {
            ClienteBO clienteBO = new ClienteBO(1);
            CompraModel compra = new CompraModel();
            compra.Login = clienteBO.Consultar(1);
            compra.CarrinhoItens = carrinhoBO.Listar("ev004", enumCarrinhoStatus.reservado);
            compra = entregaBO.Carregar(compra);
            Assert.AreEqual(true, compra.Login.Cliente.EnderecoList.Count == 3 && compra.EntregaControles.Count > 0);
        }

        /// <summary>
        /// Teste que não deve retornar entregas. O carrinho possui 2 eventos diferentes e eles não possuem nenhuma forma de entrega em comum
        /// </summary>
        [TestMethod]
        public void EntregaBO_ListarEntregasEventosEntregasDiferentes()
        {
            ClienteBO clienteBO = new ClienteBO(1);
            CompraModel compra = new CompraModel();
            compra.Login = clienteBO.Consultar(1);
            compra.CarrinhoItens = carrinhoBO.Listar("ev005", enumCarrinhoStatus.reservado);
            compra = entregaBO.Carregar(compra);
            Assert.AreEqual(true, compra.Login.Cliente.EnderecoList.Count == 0 && compra.EntregaControles.Count == 0);
        }


        /// <summary>
        /// Teste que deve retornar entregas sem endereço. O carrinho possui 2 eventos diferentes e eles possuem formas de entrega em comum
        /// A apresentação mais próxima é no dia 27/08/2014 e o teste foi feito no dia 26/08/2014 desta forma ele não tem prazo para entregas a domicilio
        /// </summary>
        [TestMethod]
        public void EntregaBO_ListarEntregasEventosDiferentesEntregasSemEndereco()
        {
            ClienteBO clienteBO = new ClienteBO(1);
            CompraModel compra = new CompraModel();
            compra.Login = clienteBO.Consultar(1);
            compra.CarrinhoItens = carrinhoBO.Listar("ev006", enumCarrinhoStatus.reservado);
            compra = entregaBO.Carregar(compra);
            Assert.AreEqual(true, compra.Login.Cliente.EnderecoList.Count == 0 && compra.EntregaControles.Count > 0);
        }


    }
}
