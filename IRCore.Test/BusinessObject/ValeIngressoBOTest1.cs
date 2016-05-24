using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using IRCore.BusinessObject.Models;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class ValeIngressoBOTest1
    {

        ValeIngressoBO valeIngressoBO = new ValeIngressoBO();
        
        /// <summary>
        /// Testar a validade dos vale ingressos
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_ValidarIngresso()
        {
            tValeIngresso valeIngressoValido = valeIngressoBO.ValidarCodigo("V67YU28JK8");
            tValeIngresso valeIngressoInexistente = valeIngressoBO.ValidarCodigo("X35YTE6171");
            tValeIngresso valeIngressoNaoDisponivel = valeIngressoBO.ValidarCodigo("00200105023098");
            Assert.AreEqual(true, valeIngressoValido != null && valeIngressoValido.ValeIngressoTipo != null && valeIngressoInexistente == null && valeIngressoNaoDisponivel == null);
        }

        /// <summary>
        /// Verifica se pode ser inserido um vale ingresso em uma compra que ainda não tem vales.
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_VerificarCompraSemVales()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.CarrinhoItens = carrinhoBO.Listar("0003zo4qtrvb2ckt4c2qdy00", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra = carrinhoBO.CalcularValores(compra);
            RetornoModel<CompraModel> retorno = valeIngressoBO.VerificarCompra(compra);
            Assert.AreEqual(true, retorno.Sucesso);
        }

        /// <summary>
        /// Verifica se pode ser inserido um vale ingresso em uma compra com valor zero
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_VerificarCompraComValorZerado()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.CarrinhoItens = carrinhoBO.Listar("0003zo4qtrvb2ckt4c2qdy00", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra.ValeIngressos = new List<tValeIngresso>();
            compra.ValeIngressos.Add(valeIngressoBO.ValidarCodigo("abc123teste1"));
            compra = carrinhoBO.CalcularValores(compra);
            RetornoModel<CompraModel> retorno = valeIngressoBO.VerificarCompra(compra);
            Assert.AreEqual(false, retorno.Sucesso && retorno.Mensagem.Equals("O carrinho está com valor total 0, por isso não podem ser adicionados VIRs."));
        }

        /// <summary>
        /// Verifica se pode ser inserido um vale ingresso com Vale Nao Aculativo Inserido
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_VerificarCompraValeNaoAcumulativo()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.CarrinhoItens = carrinhoBO.Listar("0003zo4qtrvb2ckt4c2qdy00", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra.ValeIngressos = new List<tValeIngresso>();
            compra.ValeIngressos.Add(valeIngressoBO.ValidarCodigo("abc123teste2"));
            compra = carrinhoBO.CalcularValores(compra);
            RetornoModel<CompraModel> retorno = valeIngressoBO.VerificarCompra(compra);
            Assert.AreEqual(true, !retorno.Sucesso && retorno.Mensagem.Equals("A compra possui um vale ingresso não acumulativo, por isso não podem se adicionados VIRs."));
        }

        /// <summary>
        /// Inserir um vale acumulativo e depois inserir um não acumulativo
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_InserirVales()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.CarrinhoItens = carrinhoBO.Listar("0003zo4qtrvb2ckt4c2qdy00", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra = carrinhoBO.CalcularValores(compra);
            tValeIngresso valeIngressoNaoAcumulativo = valeIngressoBO.ValidarCodigo("abc123teste2");
            tValeIngresso valeIngressoAcumulativo = valeIngressoBO.ValidarCodigo("abc123teste3");
            RetornoModel<CompraModel> retorno = valeIngressoBO.InserirValeIngresso(compra, valeIngressoAcumulativo);
            compra = retorno.Retorno;
            Assert.AreEqual(true, retorno.Sucesso && compra.Total.ValorTotal == 18 );
            //Segunda parte
            retorno = valeIngressoBO.InserirValeIngresso(compra, valeIngressoNaoAcumulativo);
            compra = retorno.Retorno;
            Assert.AreEqual(true, !retorno.Sucesso && retorno.Mensagem.Equals("A compra possui um ou mais vale(s) ingresso(s) e o vale ingresso que está sendo inserido não é acumulativo, por isso não pode ser adicionado."));
        }

        /// <summary>
        /// Inserir um vale acumulativo e depois inserir um não acumulativo
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_RemoverVale()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.CarrinhoItens = carrinhoBO.Listar("0003zo4qtrvb2ckt4c2qdy00", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra = carrinhoBO.CalcularValores(compra);
            tValeIngresso valeIngressoNaoAcumulativo = valeIngressoBO.ValidarCodigo("abc123teste2");
            tValeIngresso valeIngressoAcumulativo = valeIngressoBO.ValidarCodigo("abc123teste3");
            RetornoModel<CompraModel> retorno = valeIngressoBO.InserirValeIngresso(compra, valeIngressoAcumulativo);
            compra = retorno.Retorno;
            compra = valeIngressoBO.RemoverValeIngresso(compra, valeIngressoAcumulativo);
            Assert.AreEqual(true, compra.Total != null && compra.Total.ValorTotal == 118 && compra.ValeIngressos.Count == 0);
        }

        /// <summary>
        /// Verifica se pode ser inserido um vale com um item de cota Bin inserido
        /// </summary>
        [TestMethod]
        public void ValeIngressoBO_VerificarCompraCotaBIN()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            CotaBO cotaBO = new CotaBO();
            compra.CarrinhoItens = carrinhoBO.Listar("02kfpvahztwniota0ugz2nnv", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            //Seta os valores em compra que são necessários para carregar o objeto Cota
            compra = carrinhoBO.CalcularValores(compra);
            compra.SessionID = "02kfpvahztwniota0ugz2nnv";
            compra.Login = new Login();
            compra.Login.ClienteID = 3465690;

            //carrega cota
            compra = cotaBO.CarregarCotaInformacao(compra);
            
            compra.ValeIngressos = new List<tValeIngresso>();
            compra.ValeIngressos.Add(valeIngressoBO.ValidarCodigo("abc123teste2"));
            compra = carrinhoBO.CalcularValores(compra);
            RetornoModel<CompraModel> retorno = valeIngressoBO.VerificarCompra(compra);
            Assert.AreEqual(true, !retorno.Sucesso && retorno.Mensagem.Equals("O carrinho possui um item com cota bin, por isso não podem ser adicionados VIRs."));
        }

        
    }
}
