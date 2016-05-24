using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class ClienteBOTest1
    {

        private ClienteBO clienteBO = new ClienteBO(1);

        [TestMethod]
        public void ClienteBO_Logar()
        {
            // teste do método Logar(string username, string password, HttpContextBase httpContext = null, bool isPersistent = true)
            var login = clienteBO.Logar("leon.puccinelli@rcadigital.com.br", "rca@2013");
            Assert.AreEqual("00759432040", login.Retorno.CPF);

        }

        [TestMethod]
        public void ClienteBO_Consultar()
        {
            // teste do método Consultar(int clienteID, bool salvarLogin = true, bool loadEndereco = true)
            var consultar = clienteBO.Consultar(3623506);
            Assert.AreEqual("leon.puccinelli@rcadigital.com.br", consultar.Email);

        }

        [TestMethod]
        public void ClienteBO_ConsultarEmailCPF()
        {
            // teste do método ConsultarEmailCPF(string email, string cpf, bool salvarLogin = true, bool loadEndereco = true)
            var consultar = clienteBO.ConsultarEmailCPF("leon.puccinelli@rcadigital.com.br", "00759432040");
            Assert.AreEqual("20140606104117", consultar.DataCadastro);

        }

        [TestMethod]
        public void ClienteBO_ConsultarCPF()
        {
            // teste do método ConsultarCPF(string cpf, bool salvarLogin = true, bool loadEndereco = true)
            var consultar = clienteBO.ConsultarCPF("00759432040");
            Assert.AreEqual("leon.puccinelli@rcadigital.com.br", consultar.Email);

        }

        [TestMethod]
        public void ClienteBO_ConsultarEmail()
        {
            // teste do método ConsultarEmail(string email, bool salvarLogin = true, bool loadEndereco = true)
            var consultar = clienteBO.ConsultarEmail("leon.puccinelli@rcadigital.com.br");
            Assert.AreEqual("00759432040", consultar.CPF);

        }

        [TestMethod]
        public void ClienteBO_ConsultarUsername()
        {
            // teste do método ConsultarUsername(string username, bool salvarLogin = true, bool loadEndereco = true)
            var consultar = clienteBO.ConsultarUsername("leon.puccinelli@rcadigital.com.br");
            Assert.AreEqual("00759432040", consultar.CPF);

        }
    }
}
