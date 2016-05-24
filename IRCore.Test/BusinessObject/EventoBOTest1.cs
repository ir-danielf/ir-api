using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class EventoBOTest1
    {

        EventoBO eventoBO = new EventoBO();

        [TestMethod]
        public void EventoBO_ListarParceiro()
        {
            VoucherBO voucherBO = new VoucherBO();
            Voucher voucher = voucherBO.Consultar(1, "RE002");
            // teste do método ListarParceiro(int pageNumber, int pageSize, Voucher voucher)
            var evento = eventoBO.ListarVoucher(1,1,voucher);
            Assert.AreNotEqual(0, evento.Count);
            if (evento.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(evento[0].Nome));
            }

        }


    }
}
