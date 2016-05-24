using System;

namespace IRLib.PayPal.Exceptions
{
    public class PayPalException : Exception
    {
        public string CodigoErro { get; set; }

        public PayPalException() : base() { }

        public PayPalException(string msg) : base(msg) { }

        public PayPalException(string msg, string codigoErro) : base(msg)
        {
            this.CodigoErro = codigoErro;
        }
    }
}