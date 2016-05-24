using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class TipoBOTest1
    {
        [TestMethod]
        public void TipoBO_Listar()
        {
            TipoBO tipoBo = new TipoBO();
            List<Tipo> tipos = tipoBo.Listar();
        }
    }
}
