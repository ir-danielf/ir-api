using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class FormaPagamentoADOTest1 : MasterTest
    {
        private FormaPagamentoADO formaPagamentoADO;

        public FormaPagamentoADOTest1()
            : base()
        {
            formaPagamentoADO = new FormaPagamentoADO(ado);
        }

        [TestMethod]
        public void FormaPagamentoADO_ListarEvento()
        {
            List<FormaPagamento> lista = formaPagamentoADO.ListarEvento(33863);

            Assert.AreNotEqual(lista.Count, 0);
        }
    }
}
