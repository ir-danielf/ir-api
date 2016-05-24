using System;
using IRCore.BusinessObject.Enumerator;
using IRCore.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRCore.Test.Payment
{
    [TestClass]
    public class AdyenTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tipoPagamento = IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen;

            var compraTemporaria = new IRLib.ClientObjects.EstruturaCompraTemporaria
            {
                ValorTotal = 1,
                Parcelas = 1,
                Bandeira = "VISA",
                ClienteID = 123456789,
                FormaPagamentoID = 1
            };

            //request
            var valorTotalVir = 0;
            var numeroCartao = "4901720266910889";
            var validadeCartao = "0823";
            var codigoSeguranca = "085";
            var nomeCartao = "Evandro Araújo";
            var cartaoId = 0;
            var cartaoOutraPessoa = false;
            var payerId = "";
            var token = "";
            var ip = "192.168.6.72";
            var diferenca = 0;
            var coeficiente = 0;

            //response
            string codigoAutenticacao;
            string codigoReferencia;
            string codigoReferenciaCancel;

            var pagamento = IRLib.ClientObjects.EstruturaPagamento.Montar(tipoPagamento, compraTemporaria, valorTotalVir, numeroCartao, validadeCartao, codigoSeguranca, nomeCartao, cartaoId, cartaoOutraPessoa, payerId, token, ip, diferenca, coeficiente);
            pagamento.ClienteEmail = "earaujo@ingressorapido.com.br";
            pagamento.SessionID = Guid.NewGuid().ToString();
            pagamento.ClienteNome = "Muhammad";

            //perform payment
            var result = pagamento.oAdyen.EfetuarPagamento();

            if (result)
            {
                codigoAutenticacao = pagamento.oAdyen.CodigoAutenticacao;
                codigoReferencia = pagamento.oAdyen.CodigoReferencia;
            }

            //perform cancel
            var resultCancelamento = pagamento.oAdyen.CancelarPagamento();

            if (resultCancelamento)
            {
                codigoReferenciaCancel = pagamento.oAdyen.CodigoReferencia;
            }
        }

        [TestMethod]
        public void QuebraSenha()
        {
            var senha1 = "Yq8bJT1XgxXH+NKgl03evw==";
            //var senha2 = "Zu3pA34c+gvH+NKgl03evw==";

            var senha_decrypt1 = IRLib.Criptografia.Crypto.Decriptografar(senha1, ConfiguracaoAppUtil.Get(enumConfiguracaoBO.chaveCriptografiaLogin));
            //var senha_decrypt2 = IRLib.Criptografia.Crypto.Decriptografar(senha2, ConfiguracaoAppUtil.Get(enumConfiguracaoBO.chaveCriptografiaLogin));

            Assert.IsFalse(false);
        }
    }
}
