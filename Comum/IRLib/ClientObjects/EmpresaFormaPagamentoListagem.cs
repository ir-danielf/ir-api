using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EmpresaFormaPagamentoListagem
    {
		public int Indice;
        public int ID;
        public int FormaPagamentoID;
        public int EmpresaID;
        public int Dias;
        public decimal TaxaAdm;
        public bool IR;
    }
}
