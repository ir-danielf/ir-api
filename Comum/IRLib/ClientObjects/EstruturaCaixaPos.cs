using System;

namespace IRLib
{
    [Serializable]
    public class EstruturaCaixaPos
    {
        public int EventoID { get; set; }
        public int CaixaID { get; set; }
        public string Evento { get; set; }
        public string DataAberturaCaixa { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
