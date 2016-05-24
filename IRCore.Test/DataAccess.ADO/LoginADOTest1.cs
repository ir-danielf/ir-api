using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class LoginADOTest1 : MasterTest
    {
        private LoginADO loginADO;
        
        public LoginADOTest1() : base(){
            loginADO = new LoginADO(1, ado);
        }

        [TestMethod]
        public void LoginADO_Consultar()
        {
            //teste do método Consultar(int clienteID, bool lazyLoadingEnabled = false)
            var login = loginADO.Consultar(5928397);
            Assert.IsNull(login);

            login = loginADO.Consultar(1);
            Assert.AreEqual("evandro.silva.araujo@gmail.com", login.Email);
        }

        [TestMethod]
        public void LoginADO_ConsultarUsername()
        {
            //teste do método ConsultarUsername(string username, bool lazyLoadingEnabled = false)

            //por email
            var login = loginADO.ConsultarUsername("evandro.silva.araujo@gmail.com");
            Assert.AreEqual(1, login.ClienteID);

            //por CPF
            login = loginADO.ConsultarUsername("34256856862");
            Assert.AreEqual(1, login.ClienteID);
        }

        [TestMethod]
        public void LoginADO_ConsultarEmailCPF()
        {
            //teste do método ConsultarEmailCPF(string email, string cpf, bool lazyLoadingEnabled = false)
            var login = loginADO.ConsultarEmailCPF("evandro.silva.araujo@gmail.com", "34256856862");
            Assert.AreEqual("20130815123617", login.DataCadastro);
        }

        [TestMethod]
        public void LoginADO_ConsultarCPF()
        {
            //teste do método ConsultarCPF(string cpf, bool lazyLoadingEnabled = false)
            var login = loginADO.ConsultarCPF("34256856862");
            Assert.AreEqual("evandro.silva.araujo@gmail.com", login.Email);
        }

        [TestMethod]
        public void LoginADO_ConsultarEmail()
        {
            //teste do método ConsultarEmail(string email, bool lazyLoadingEnabled = false)

            //por email
            var login = loginADO.ConsultarEmail("evandro.silva.araujo@gmail.com");
            Assert.AreEqual(1, login.ClienteID);
        }

        [TestMethod]
        public void LoginADO_ConsultarFacebook()
        {
            //teste do método ConsultarFacebook(string faceBookUserID, bool lazyLoadingEnabled = false)

            //var login = loginADO.ConsultarFacebook(""); //colocar user como parametro
            //Assert.AreEqual("evandro.silva.araujo@gmail.com", login.Email);
        }
    }
}
