
namespace IRLib.Paralela.ExpressCheckout
{
    public class GetExpressCheckoutDetailsOperation : ExpressCheckoutApi.Operation
    {
        public GetExpressCheckoutDetailsOperation(ExpressCheckoutApi ec, string token)
            : base(ec)
        {
            RequestNVP.Method = "GetExpressCheckoutDetails";
            Token = token;
        }
    }
}