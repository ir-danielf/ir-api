using System;

namespace IRLib.Paralela.ClientObjects
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
