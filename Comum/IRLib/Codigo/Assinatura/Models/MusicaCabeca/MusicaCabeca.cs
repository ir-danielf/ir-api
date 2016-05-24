
namespace IRLib.Assinaturas.Models
{
    public class MusicaCabeca
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Local { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public int QuantidadeCota { get; set; }
        public int QuantidadeNormal { get; set; }
        public int QuantidadeVagas
        {
            get
            {
                return QuantidadeCota + QuantidadeNormal;
            }
        }
        public int QuantidadeInscritosCota { get; set; }
        public int QuantidadeInscritosNormal { get; set; }
        public int QuantidadeInscritos
        {
            get
            {
                return QuantidadeInscritosCota + QuantidadeInscritosNormal;
            }
        }
        public string DataLimiteCota { get; set; }
        public string Detalhes { get; set; }
        public bool Inscrito { get; set; }
        public string InscritoValor
        {
            get
            {
                return Inscrito ? "Sim" : "Não";
            }
        }
        public bool Disponivel { get; set; }
        public string DisponivelValor
        {
            get
            {
                return Disponivel ? "Sim" : "Não";
            }
        }
    }
}
