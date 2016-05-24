using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class EmpresaADOTest1 : MasterTest
    {

        private EmpresaADO empresaADO;

        public EmpresaADOTest1()
            : base()
        {
            empresaADO = new EmpresaADO(ado);
        }

        [TestMethod]
        public void EmpresaADO_Consultar()
        {
            // teste do método Consultar(int empresaId, bool lazyLoadingEnabled = true)
            var empresa = empresaADO.Consultar(9999999);
            Assert.IsNull(empresa);

            empresa = empresaADO.Consultar(3);
            Assert.AreEqual("EIR - SP Show Tickets", empresa.Nome);
        }

        [TestMethod]
        public void EmpresaADO_Listar()
        {
            // teste do método Listar(string busca = null, int empresaId = 0, bool lazyLoadingEnabled = false)
            var empresa = empresaADO.Listar();
            Assert.AreNotEqual(0, empresa.Count);

            if (empresa.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(empresa[0].Nome));
            }
        }
        [TestMethod]
        public void EmpresaADO_Listar2()
        {
            // teste do método Listar(int pageNumber, int pageSize, string busca = null, int empresaId = 0, bool lazyLoadingEnabled = false)
            var empresa = empresaADO.Listar(1,1);
            Assert.AreNotEqual(0, empresa.Count);

            if (empresa.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(empresa[0].Nome));
            }
        }
    }
}
