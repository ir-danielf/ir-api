using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaTransacoesIngressosComprados
    {
        public int ID { get; set; }
        public string Local { get; set; }
        public int EventoID { get; set; }
        public string Evento { get; set; }
        public DateTime Apresentacao { get; set; }
        public string Setor { get; set; }
        public string PrecoNome { get; set; }
        public decimal Valor { get; set; }
        public string Codigo { get; set; }
        public string Status { get; set; }
        public decimal TaxaConveniencia { get; set; }

        public int LugarID { get; set; }

        public string TipoLugar { get; set; }
    }
}
