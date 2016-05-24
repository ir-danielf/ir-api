using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaRelatoriosSangria
    {
        public string Canal { get; set; }
        public int CanalID { get; set; }
        public string Login { get; set; }
        public string Supervisor { get; set; }
        public string Motivo { get; set; }
        public string Evento { get; set; }
        public int EventoID { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Responsavel { get; set; }
        public string Identificacao { get; set; }
        public int Caixa { get; set; }
        public decimal Valor { get; set; }
        public String ValorReal
        {
            get
            {
                return Valor.ToString("c");
            }
        }

    }
}
