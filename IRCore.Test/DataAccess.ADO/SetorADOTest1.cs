using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class SetorADOTest1 : MasterTest
    {
        private SetorADO setorADO;
        
        public SetorADOTest1() : base(){
            setorADO = new SetorADO(ado);
        }


        [TestMethod]
        public void SetorADO_Consultar()
        {
            var setor = setorADO.Consultar(99999999, 99999999); //arrumar
            Assert.IsNull(setor);

            //teste do método Consultar(int setorId, bool lazyLoadingEnabled = false)
            setor = setorADO.Consultar(3041, 151427);
            Assert.AreEqual("PISTA VIP", setor.Nome);

        }
    }
}
