using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaContratoFormaPagamento
    {
        public int FormaPagamentoTipoID;
        public int Dias;
        public decimal TaxaAdm;
    }
}
