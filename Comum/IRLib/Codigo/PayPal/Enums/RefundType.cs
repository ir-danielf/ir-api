namespace IRLib.PayPal.Enums
{
    public sealed class RefundType : AbstractTypeSafeEnumerationString
    {
        public static readonly RefundType Full = new RefundType("Full ", "Full");
        public static readonly RefundType Partial = new RefundType("Partial", "Partial");
        public static readonly RefundType ExternalDispute = new RefundType("ExternalDispute", "ExternalDispute");
        public static readonly RefundType Other = new RefundType("Other ", "Other");


        private RefundType(string name, string value) : base(name, value) { }
    }
}