
namespace IRLib.PayPal.Enums
{
    public abstract class AbstractTypeSafeEnumeration
    {
        protected readonly string name;
        protected readonly int value;

        protected AbstractTypeSafeEnumeration(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public abstract class AbstractTypeSafeEnumerationString
    {
        protected readonly string name;
        protected readonly string value;

        protected AbstractTypeSafeEnumerationString(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}