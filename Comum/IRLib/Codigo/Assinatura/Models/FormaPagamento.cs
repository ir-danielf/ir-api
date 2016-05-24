
namespace IRLib.Assinaturas.Models
{
    public class FormaPagamento
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Parcelas { get; set; }
        public int ForcaParcela { get; set; }
    }
}