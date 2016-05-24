using System;
using System.Drawing;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaFormaPagamento
    {
        public int CotaItemID { get; set; }
        public int FormaPagamentoID { get; set; }
        public string FormaPagamento { get; set; }
        public bool Adicionar { get; set; }
        public int Tipo { get; set; }
        public decimal Valor { get; set; }
        public int CartaoID { get; set; }
        public string Cartao { get; set; }
        public bool ComprasCartaoVisible { get; set; }
    }

    [Serializable]
    public class EstruturaFormaPagamentoSerie
    {
        public int ID { get; set; }
        public int FormaPagamentoID { get; set; }
        public Enumerators.TipoAcaoCanal Acao { get; set; }
        public Bitmap ImagemAcao { get; set; }
        public string FormaPagamento { get; set; }
        public string Bandeira { get; set; }
        public string Tipo { get; set; }
    }

}
