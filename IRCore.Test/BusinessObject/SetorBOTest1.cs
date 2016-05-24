using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class SetorBOTest1
    {

        private SetorBO setorBO = new SetorBO();


        [TestMethod]
        public void SetorADO_Consultar()
        {
            var setor = setorBO.Consultar(99999999, 99999999); //arrumar
            Assert.IsNull(setor);

            //teste do método Consultar(int setorId)
            setor = setorBO.Consultar(3041, 151427);
            Assert.AreEqual("PISTA VIP", setor.Nome);
        }
    }
}
