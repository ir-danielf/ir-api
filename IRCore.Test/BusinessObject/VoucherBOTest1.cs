using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.DataAccess.Model;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class VoucherBOTest1 : MasterTest
    {
        private VoucherBO voucherBO = new VoucherBO();
        
        [TestMethod]
        public void VoucherBO_Listar()
        {
            UsuarioBO usuarioBo = new UsuarioBO(ado);


            //teste do método Listar(int pageNumber, int pageSize, enumVoucherStatus enumvoucherstatus, string busca = null)
            var voucher = voucherBO.Listar(1, 1, 1, enumVoucherStatus.nenhum);

            Assert.AreNotEqual(0, voucher.Count);
            if (voucher.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(voucher[0].Codigo));
            }
        }


        [TestMethod]
        public void VoucherBO_ListarCodigo()
        {

            //teste do método Listar(string[] codigos)
            string[] voucherCodigos = { "E1" };
            var voucher = voucherBO.Listar(voucherCodigos);

            Assert.AreNotEqual(0, voucher.Count);
            if (voucher.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(voucher[0].Codigo));
            }
        }

        [TestMethod]
        public void VoucherBO_Contar()
        {
            UsuarioBO usuarioBo = new UsuarioBO(ado);

            //teste do método Contar(enumVoucherStatus enumvoucherstatus = enumVoucherStatus.nenhum, string busca = null, bool lazyLoadingEnabled = false)
            var voucher = voucherBO.Contar(1, enumVoucherStatus.nenhum);

            Assert.AreNotEqual(0, voucher);
        }

        [TestMethod]
        public void VoucherBO_Consultar()
        {
            //teste do método Consultar(int idParceiro, string codigoVoucher)
            var voucher = voucherBO.Consultar(1, "E1");
            Assert.AreEqual(1, voucher.ID);
        }

        [TestMethod]
        public void VoucherBO_Consultar2()
        {
            //teste do método Consultar(string urlContextoParceiro, string codigoVoucher)
            var voucher = voucherBO.Consultar(1, "E900");
            Assert.AreEqual(900, voucher.ID);
        }


        //[TestMethod]
        //public void VoucherBO_VerificarExpirado()
        //{
        //    UsuarioBO usuarioBo = new UsuarioBO(ado);

        //    //teste do método VerificarExpirado(Voucher voucher, int minExpiracao, bool atualizaData = true)
        //    var vouches = voucherBO.Listar(1, 1, 1, enumVoucherStatus.bloqueado);

        //    if (vouches.Count > 0)
        //    {
        //        var primeiroVoucher = vouches[0];
        //        var voucher = voucherBO.VerificarExpirado(primeiroVoucher, false);
        //        Assert.AreEqual(true, voucher);
        //    }
        //}
    }
}
