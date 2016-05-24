using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCodigoPromoValidacao
    {
        public int CotaItemID { get; set; }

        public int CotaItemIDAPS { get; set; }

        public int ParceiroID { get; set; }

        public string CodigoValidacao { get; set; }

        public int ApresentacaoID { get; set; }

        public int Quantidade { get; set; }

        public int QuantidadePorCodigo { get; set; }

        public Enumerators.TipoParceiro Tipo { get; set; }

        public string Url { get; set; }

        public string Identificacao { get; set; }
    }
}
