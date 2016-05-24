using IRLib.Nvp;
using IRLib.PayPal.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace IRLib.ExpressCheckout
{
    public class ExpressCheckoutApi
    {
        private static readonly String PAYPAL_LIVE = ConfigurationManager.AppSettings["producaoPayPal"];
        private static readonly String PAYPAL_SANDBOX = ConfigurationManager.AppSettings["sandboxPayPal"];

        private static readonly String version = "84.0";

        private String user;
        private String pswd;
        private String signature;
        private Boolean _sandbox;

        public ExpressCheckoutApi()
        {
            this.LoadConfigFromXML();
        }

        public ExpressCheckoutApi(String user, String pswd, String signature, Boolean sandbox = false)
        {
            this.user = user;
            this.pswd = pswd;
            this.signature = signature;
            this._sandbox = sandbox;
        }

        RequestNVP createNVP()
        {
            RequestNVP nvp = new RequestNVP();

            nvp.Version = version;
            nvp.User = user;
            nvp.Password = pswd;
            nvp.Signature = signature;

            return nvp;
        }

        public DoExpressCheckoutPaymentOperation DoExpressCheckoutPayment(string token, string payerid, double valor, PaymentAction action)
        {
            return new DoExpressCheckoutPaymentOperation(this, token, payerid, valor, action);
        }


        public RefundTransaction RefundTransaction(string transaction)
        {
            return new RefundTransaction(this, transaction);

        }

        ResponseNVP execute(RequestNVP requestNvp)
        {
            var url = _sandbox ? PAYPAL_SANDBOX : PAYPAL_LIVE;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = request.GetRequestStream())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(requestNvp.ToString());

                stream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ResponseNVP responseNvp;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string result = reader.ReadToEnd();

                    responseNvp = (ResponseNVP)result.ToString();
                } 
            }

            return responseNvp;
        }

        public bool inSandbox()
        {
            return _sandbox;
        }

        public ExpressCheckoutApi sandbox()
        {
            _sandbox = true;

            return this;
        }

        public GetExpressCheckoutDetailsOperation GetExpressCheckoutDetails(string token)
        {
            return new GetExpressCheckoutDetailsOperation(this, token);
        }

        public SetExpressCheckoutOperation SetExpressCheckout(string returnURL, string cancelURL)
        {
            return new SetExpressCheckoutOperation(this, returnURL, cancelURL);
        }
        
        /*Load PayPal API settings*/
        private void LoadConfigFromXML()
        {
            this.user = ConfigurationManager.AppSettings["usuarioPayPal"];
            this.pswd = ConfigurationManager.AppSettings["senhaPayPal"];
            this.signature = ConfigurationManager.AppSettings["assinaturaPayPal"];
            this._sandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["ambienteTestePayPal"]);
        }

        public abstract class Operation
        {
            private ExpressCheckoutApi ec;
            private RequestNVP requestNVP;
            private ResponseNVP responseNVP;
            private CurrencyCode currencyCode = CurrencyCode.DEFAULT;
            private LocaleCode localeCode = LocaleCode.DEFAULT;

            public Operation(ExpressCheckoutApi ec)
            {
                this.ec = ec;

                requestNVP = ec.createNVP();
            }

            public CurrencyCode CurrencyCode
            {
                get { return currencyCode; }
                set
                {
                    currencyCode = value;
                    updateCurrency();
                }
            }

            public LocaleCode LocaleCode
            {
                get { return localeCode; }
                set
                {
                    localeCode = value;
                    requestNVP.Set("LOCALECODE", value.ToString());
                }
            }

            public string Token
            {
                get { return ResponseNVP.Get("TOKEN"); }
                set { requestNVP.Set("TOKEN", value); }
            }

            public RequestNVP RequestNVP
            {
                get { return requestNVP; }
            }

            public ResponseNVP ResponseNVP
            {
                get { return responseNVP; }
            }

            public virtual Operation execute()
            {
                responseNVP = ec.execute(requestNVP);

                return this;
            }

            public bool inSandbox()
            {
                return ec.inSandbox();
            }

            public Operation sandbox()
            {
                ec.sandbox();

                return this;
            }

            protected virtual void updateCurrency() { }
        }

        public abstract class OperationWithPaymentRequest : Operation
        {
            private Dictionary<int, PaymentRequest> paymentRequest = new Dictionary<int, PaymentRequest>();

            public OperationWithPaymentRequest(ExpressCheckoutApi ec) : base(ec) { }

            public PaymentRequest PaymentRequest(int n)
            {
                if (n >= 0 && n <= 9)
                {
                    PaymentRequest request;

                    if (!paymentRequest.TryGetValue(n, out request))
                    {
                        request = new PaymentRequest(this, n);
                        request.CurrencyCode = this.CurrencyCode;

                        paymentRequest[n] = request;
                    }

                    return request;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            protected override void updateCurrency()
            {
                for (int i = 0, t = paymentRequest.Count; i < t; ++i)
                {
                    paymentRequest[i].CurrencyCode = this.CurrencyCode;
                }
            }
        }
    }
}