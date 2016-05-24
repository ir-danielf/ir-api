using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.ADO;
using System.Linq;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class FormaPagamentoBOTest1 : MasterTest
    {
        FormaPagamentoBO formaPagamentoBO;

        public FormaPagamentoBOTest1()
            : base()
        {
            formaPagamentoBO = new FormaPagamentoBO(ado);
        }

        [TestMethod]
        public void FormasPagamentoBO_ListarFormas()
        {
            List<FormaPagamento> formasPagamento = formaPagamentoBO.ListarEvento(3857056, "432ac9a3-e98f-4040-898f-4c6bebb8d9ed", 56.64m);
            Assert.AreEqual(1, 1);

        }
    }
}
