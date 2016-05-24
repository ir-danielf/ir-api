using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaContratoFormaPagamento
    {
        public int FormaPagamentoTipoID;
        public int Dias;
        public decimal TaxaAdm;
    }
}
