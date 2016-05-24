using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class ClienteADOTest1 : MasterTest
    {
        private ClienteADO clienteADO;
        
        public ClienteADOTest1() : base(){
            clienteADO = new ClienteADO(ado);
        }

        [TestMethod]
        public void ClienteADO_Consultar()
        {
            //teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var cliente = clienteADO.Consultar(9999999);
            Assert.IsNull(cliente);

            cliente = clienteADO.Consultar(1);
            Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);
        }

        [TestMethod]
        public void ClienteADO_ConsultarUsername()
        {
            //teste do método ConsultarUsername(string username, bool lazyLoadingEnabled = false) por CPF
            var cliente = clienteADO.ConsultarUsername("34256856862");
            Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);

            //teste do método ConsultarUsername(string username, bool lazyLoadingEnabled = false) por email
            cliente = clienteADO.ConsultarUsername("evandro.silva.araujo@gmail.com");
            Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);
        }

        [TestMethod]
        public void ClienteADO_ConsultarEmailCPF()
        {
            //teste do método ConsultarEmailCPF(string email, string cpf, bool lazyLoadingEnabled = false)
            var cliente = clienteADO.ConsultarEmailCPF("evandro.silva.araujo@gmail.com", "34256856862");
            Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);
        }

        [TestMethod]
        public void ClienteADO_ConsultarEmail()
        {
            //teste do método ConsultarEmail(string email, bool lazyLoadingEnabled = false)
            var cliente = clienteADO.ConsultarEmail("evandro.silva.araujo@gmail.com");
            Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);
        }

        [TestMethod]
        public void ClienteADO_ConsultarCPF()
        {
            //teste do método ConsultarCPF(string cpf, bool lazyLoadingEnabled = false)
            var cliente = clienteADO.ConsultarCPF("34256856862");
            Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);
        }
        
        [TestMethod]
        public void ClienteADO_ListarContatoTipo()
        {
            //teste do método ListarContatoTipo(bool lazyLoadingEnabled = false)
            var contato = clienteADO.ListarContatoTipo();
            Assert.AreNotEqual(0, contato.Count);
            if (contato.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(contato[0].Nome));
            }
        }

        [TestMethod]
        public void ClienteADO_ListarEndereco()
        {
            //teste do método ListarEndereco(int clienteId, bool lazyLoadingEnabled = false)
            var cliente = clienteADO.ListarEndereco(1688921);
            Assert.AreNotEqual(0, cliente.Count);

            if (cliente.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(cliente[0].Nome));
            }
        }

        [TestMethod]
        public void ClienteADO_ConsultarFacebook()
        {
            //teste do método ConsultarFacebook(string faceBookUserID, bool lazyLoadingEnabled = false)

            //var cliente = clienteADO.ConsultarFacebook();
            //Assert.AreEqual("EVANDRO SILVA DE  ARAUJO", cliente.Nome);
        }

        [TestMethod]
        public void ClienteADO_ComparaListarContatoTipo()
        {
            //teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            var cliente = clienteADO.ListarContatoTipo();
            var cliente1 = clienteADO.ListarContatoTipo();
            Assert.AreEqual(cliente1.Count, cliente.Count);
        }
    }
}