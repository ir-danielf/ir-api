using System;
using System.Configuration;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using IRLib.ExpressCheckout;

namespace IRLib.PayPal
{

    /// <remarks>
    /// Interface para criação dos objetos de integração com as APIs do PayPal.
    /// </remarks>
    public class PayPalApiFactory
    {
        private static PayPalApiFactory paypalInstance;
        
        private PayPalApiFactory()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Validator);
        }

        /// <summary>
        /// Instância de PayPal para integração com as várias APIs do PayPal.
        /// </summary>
        public static PayPalApiFactory instance
        {
            get { return paypalInstance ?? (paypalInstance = new PayPalApiFactory()); }
        }

        public ExpressCheckoutApi ExpressCheckout()
        {
            return new ExpressCheckoutApi();
        }

        public ExpressCheckoutApi ExpressCheckout(String user, String pwd, String signature)
        {
            return new ExpressCheckoutApi(user, pwd, signature);
        }

        bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IgnoreSslErrors"]))
                return true;
            else
                return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}