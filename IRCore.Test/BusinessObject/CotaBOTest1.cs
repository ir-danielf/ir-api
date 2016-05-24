using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using IRCore.BusinessObject.Models;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class CotaBOTest1
    {

        CotaBO cotaBO = new CotaBO();
        
        /// <summary>
        /// Verifica se pode ser inserido um vale ingresso em uma compra que ainda não tem vales.
        /// </summary>
        [TestMethod]
        public void CotaBO_VerificarCota()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.Login = new Login();
            compra.CarrinhoItens = carrinhoBO.Listar("02kfpvahztwniota0ugz2nnv", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra = carrinhoBO.CalcularValores(compra);
            compra.SessionID = "02kfpvahztwniota0ugz2nnv";
            compra.Login.ClienteID = 3465690;
            compra = cotaBO.CarregarCotaInformacao(compra);
            Assert.AreEqual(true, compra.CarrinhoItens[0].CotaItemObject.NominalAsBool || compra.CarrinhoItens[0].CotaItemObject.ValidaBinAsBool);
        }

        /// <summary>
        /// Verifica se pode ser inserido um vale ingresso em uma compra que ainda não tem vales.
        /// </summary>
        [TestMethod]
        public void CotaBO_ValidarCotas()
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO();
            CompraModel compra = new CompraModel();
            compra.Login = new Login();
            compra.CarrinhoItens = carrinhoBO.Listar("02kfpvahztwniota0ugz2nnv", IRCore.DataAccess.Model.Enumerator.enumCarrinhoStatus.reservado);
            compra = carrinhoBO.CalcularValores(compra);
            compra.SessionID = "02kfpvahztwniota0ugz2nnv";
            compra.Login.ClienteID = 3465690;
            compra = cotaBO.CarregarCotaInformacao(compra);
            tDonoIngresso dono = new DonoIngressoBO().Consultar(2);
            foreach (var item in compra.CarrinhoItens)
            {
                item.CotaItemObject.DonoIngresso = new tDonoIngresso();
                item.CotaItemObject.DonoIngresso.ID = dono.ID;
                item.CotaItemObject.DonoIngresso.ClienteID = dono.ClienteID;
                item.CotaItemObject.DonoIngresso.Nome = dono.Nome;
                item.CotaItemObject.DonoIngresso.CPF = dono.CPF;
                item.CotaItemObject.DonoIngresso.NomeResponsavel = dono.NomeResponsavel;
                item.CotaItemObject.DonoIngresso.CPFResponsavel = dono.CPFResponsavel;
                item.CotaItemObject.DonoIngresso.Telefone = dono.Telefone;
                item.CotaItemObject.DonoIngresso.RG = dono.RG;
                item.CotaItemObject.DonoIngresso.DataNascimentoAsDateTime = new DateTime(1990, 01, 01);
                item.CotaItemObject.DonoIngresso.Email = dono.Email;
            }
            compra = cotaBO.ValidarCotas(compra).Retorno;
            Assert.AreEqual(true, compra.CarrinhoItens[0].CotaItemObject.NominalAsBool || compra.CarrinhoItens[0].CotaItemObject.ValidaBinAsBool);
        }
    }
}
