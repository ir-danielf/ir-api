using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class UsuarioADOTest1 : MasterTest
    {

        private UsuarioADO usuarioADO;
        
        public UsuarioADOTest1() : base(){
            usuarioADO = new UsuarioADO(ado);
        }

        [TestMethod]
        public void UsuarioADO_Consultar()
        {
            //teste do método Consultar(int usuarioId, bool lazyLoadingEnabled = true)
            var usuario = usuarioADO.Consultar(2);
            Assert.AreEqual("Kátia Lattufe", usuario.Nome);
        }

        [TestMethod]
        public void UsuarioADO_Consultar2()
        {
            //teste do método Consultar(string login, bool lazyLoadingEnabled = true)
            var usuario = usuarioADO.Consultar(".sunderam");
            Assert.AreEqual("Kátia Lattufe", usuario.Nome);
        }

        [TestMethod]
        public void UsuarioADO_Listar()
        {
            //teste do método Listar(int pageNumber, int pageSize, string busca = null, int empresaId = 0, bool lazyLoadingEnabled = false)
            var usuario = usuarioADO.Listar(1,1);
            Assert.IsNotNull(usuario.Count);

            if (usuario.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(usuario[0].Nome));
            }
        }
    }
}
