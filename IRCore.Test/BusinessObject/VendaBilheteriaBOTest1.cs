using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class VendaBilheteriaBOTest1
    {

        VendaBilheteriaBO vendaBilheteriaBO = new VendaBilheteriaBO();

        [TestMethod]
        public void VendaBilheteriaBO_ListarCliente()
        {
            Login login = new ClienteBO(1).Consultar(10);
            var list = vendaBilheteriaBO.ListarCliente(3, 2, login);
            List<tVendaBilheteria> compras = new List<tVendaBilheteria>();
            foreach (var item in list)
            {
                compras.Add(item);
            }
            Assert.AreEqual(true, compras.Count >= 2 && compras[0].DataVendaAsDateTime >= compras[1].DataVendaAsDateTime);
        }

        [TestMethod]
        public void VendaBilheteriaBO_Consultar()
        {
            tVendaBilheteria venda = vendaBilheteriaBO.Consultar(6011930);

            Assert.AreEqual(true, venda != null);
        }
    }
}
