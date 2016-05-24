using IRLib.Paralela.ExpressCheckout;
using IRLib.Paralela.PayPal.Enums;
using System;
using System.Configuration;

namespace IRLib.Paralela.PayPal
{
    public class CancelaPayPal
    {
        string Usuario = Convert.ToString(ConfigurationManager.AppSettings["Usuario"]);
        string Senha = Convert.ToString(ConfigurationManager.AppSettings["Senha"]);
        string Assinatura = Convert.ToString(ConfigurationManager.AppSettings["Assinatura"]);
        bool AmbienteTestePayPal = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTestePayPal"]);

        public bool RefundPayPal(string TransactionID)
        {
            try
            {
                bool retorno = false;

                RefundTransaction Refound = PayPalApiFactory.instance.ExpressCheckout(
                Usuario,
                Senha,
                Assinatura
                ).RefundTransaction(TransactionID);

                Refound.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                Refound.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                if (AmbienteTestePayPal)
                    Refound.sandbox().execute();
                else
                    Refound.execute();

                if (Refound.ResponseNVP.Ack == Ack.SUCCESS)
                    retorno = true;

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
