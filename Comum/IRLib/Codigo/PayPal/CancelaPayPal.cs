using IRLib.PayPal.Enums;
using System;
using IRLib.ExpressCheckout;

namespace IRLib.PayPal
{
    public class CancelaPayPal
    {
        public bool RefundPayPal(string TransactionID)
        {
            try
            {
                var expressCheckoutApi = PayPalApiFactory.instance.ExpressCheckout();
                var refound = expressCheckoutApi.RefundTransaction(TransactionID);

                refound.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                refound.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                if (expressCheckoutApi.inSandbox())
                    refound.sandbox().execute();
                else
                    refound.execute();

                return refound.ResponseNVP.Ack == Ack.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RefundPartial(string TransactionID, decimal valor)
        {
            bool retorno = false;

            RefundTransaction Refound = PayPalApiFactory.instance.ExpressCheckout().RefundTransaction(TransactionID);

            Refound.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
            Refound.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;
            Refound.RefundType = RefundType.Partial.ToString();
            Refound.Amt = valor.ToString().Replace(',', '.');
            
            Refound.execute();

            if (Refound.ResponseNVP.Ack != Ack.SUCCESS)
                throw new Exception("Não foi possivel realizar o estorno do pagamento junto ao PayPal, tente novamente!");
            else
                retorno = true;

            return retorno;
        }
    }
}
