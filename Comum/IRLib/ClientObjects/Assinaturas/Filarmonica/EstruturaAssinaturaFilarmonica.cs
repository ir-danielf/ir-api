
namespace IRLib.ClientObjects.Assinaturas.Filarmonica
{
    public class EstruturaAssinaturaFilarmonica
    {
        public int ID { get; set; }
        public string Assinatura { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }

        public string Nome { get; set; }
    }

    public class EstruturaAssinaturaFilarmonicaPreco
    {
        public int AssinaturaID { get; set; }
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string ValorStr
        {
            get { return this.Valor.ToString("#.00"); }
        }
    }
}
