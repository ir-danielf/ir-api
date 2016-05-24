
namespace IRLib.Assinaturas.Models
{
    public class Calendario
    {
        public int Line { get; set; }
        public bool HasValue { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public int ApresentacaoID { get; set; }
    }

    public class CalendarioIngressos
    {
        public int Day { get; set; }
        public bool Available { get; set; }
        public int ApresentacaoID { get; set; }
    }
}
