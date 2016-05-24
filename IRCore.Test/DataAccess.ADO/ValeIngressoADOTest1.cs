using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.Model.Enumerator;
using System.Collections.Generic;
using IRCore.DataAccess.Model;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class ValeIngressoADOTest1: MasterTest
    {
        private ValeIngressoADO valeIngressoADO;

        public ValeIngressoADOTest1()
            : base()
        {
            valeIngressoADO = new ValeIngressoADO(ado);
        }

        [TestMethod]
        public void ValeIngressoADO_Compara_Listar()
        {
            List<int> ids = new List<int>() { 1255, 2324 };
            List<tValeIngresso> l1 = valeIngressoADO.Listar(ids);
            List<tValeIngresso> l2 = valeIngressoADO.Listar(ids);
            Assert.AreEqual(l1.Count, l2.Count);
        }
    }
}
