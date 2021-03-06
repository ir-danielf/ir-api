
namespace IRLib.Paralela.PayPal.Enums
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
}