using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEntregaAgenda
    {
        public int EntregaID { get; set; }
        public int EntregaControleID { get; set; }
        public string Nome { get; set; }
        public string Evento { get; set; }
        public string Periodo { get; set; }
        public int PrazoEntrega { get; set; }
        public string ProcedimentoEntrega { get; set; }
        public string Tipo { get; set; }
        public int DiasTriagem { get; set; }
        public decimal Valor { get; set; }
        public DataPeriodoSelecionado dataPeriodoSelecionado = new DataPeriodoSelecionado();
        public int PDVIDSelecionado { get; set; }
        public int PrioridadeProcedimento { get; set; }
    }

    public class EstrutraEntregaSimples
    {
        public int EntregaID { get; set; }
        public int EntregaControleID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Procedimento { get; set; }
    }
}
