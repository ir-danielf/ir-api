
namespace IRLib.Paralela.ExpressCheckout
{
    public class RefundTransaction : ExpressCheckoutApi.OperationWithPaymentRequest
    {
        public RefundTransaction(ExpressCheckoutApi ec, string transactionid)
            : base(ec)
        {
            RequestNVP.Method = "RefundTransaction";
            TransactionID = transactionid;
            RefundType = "Full";
        }

        public string TransactionID
        {
            get { return RequestNVP.Get("TRANSACTIONID"); }
            set { RequestNVP.Set("TRANSACTIONID", value); }
        }

        public string Amt
        {
            get { return RequestNVP.Get("AMT"); }
            set { RequestNVP.Set("AMT", value); }
        }

        public string Note
        {
            get { return RequestNVP.Get("NOTE"); }
            set { RequestNVP.Set("NOTE", value); }
        }

        public string RefundType
        {
            get { return RequestNVP.Get("REFUNDTYPE"); }
            set { RequestNVP.Set("REFUNDTYPE", value); }
        }


    }
}
