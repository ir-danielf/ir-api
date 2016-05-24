using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.Model.Enumerator;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class VoucherADOTest1: MasterTest
    {
        private VoucherADO voucherADO;
        
        public VoucherADOTest1() : base(){
            voucherADO = new VoucherADO(ado);
        }

        [TestMethod]
        public void VoucherADO_Consultar()
        {
            var voucher = voucherADO.Consultar(5928397);
            Assert.IsNull(voucher);

            //teste do método Consultar(int id, bool lazyLoadingEnabled = false)
            voucher = voucherADO.Consultar(1);
            Assert.AreEqual("E1", voucher.Codigo);
        }

        [TestMethod]
        public void VoucherADO_Listar()
        {
            //teste do método Listar(enumVoucherStatus enumvoucherstatus = enumVoucherStatus.nenhum, string busca = null, bool lazyLoadingEnabled = false)
            var voucher = voucherADO.Listar(1);
            
            Assert.AreNotEqual(0, voucher.Count);
            if (voucher.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(voucher[0].Codigo));
            }

        }

        [TestMethod]
        public void VoucherADO_ListarCodigo()
        {
            //teste do método Listar(string[] codigos, bool lazyLoadingEnabled = false)
            string[] voucherCodigos = {"E1"};
            var voucher = voucherADO.Listar(voucherCodigos);

            Assert.AreNotEqual(0, voucher.Count);
            if (voucher.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(voucher[0].Codigo));
            }

        }
        [TestMethod]
        public void VoucherADO_Contar()
        {
            //teste do método Contar(enumVoucherStatus enumvoucherstatus = enumVoucherStatus.nenhum, string busca = null, bool lazyLoadingEnabled = false)
            var voucher = voucherADO.Contar(1);

            Assert.AreNotEqual(0, voucher);
        }

        [TestMethod]
        public void VoucherADO_Consultar2()
        {
            //teste do método Consultar(int idParceiro, string codigoVoucher, bool lazyLoadingEnabled = false)
            var voucher = voucherADO.Consultar(1, "E1");
            Assert.AreEqual(1, voucher.ID);
        }
    }
}
