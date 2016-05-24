using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCotasRetornoDistribuicao
    {
        public int EventoID { get; set; }
        public int ApresentacaoID { get; set; }
        public int SetorID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public string Evento { get; set; }
        public string Horario { get; set; }
        public string Setor { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadePorCliente { get; set; }
    }
}
