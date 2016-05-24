using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaTaxasEventoPacotes
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int pacoteID;
        public int PacoteID
        {
            get { return pacoteID; }
            set { pacoteID = value; }
        }

        private int quantidade;
        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        private int canalPacoteId;
        public int CanalPacoteId
        {
            get { return canalPacoteId; }
            set { canalPacoteId = value; }
        }

        private int canalID;
        public int CanalID
        {
            get { return canalID; }
            set { canalID = value; }
        }

        private int eventoID;
        public int EventoID
        {
            get { return eventoID; }
            set { eventoID = value; }
        }

        private int taxaConveniencia;
        public int TaxaConveniencia
        {
            get { return taxaConveniencia; }
            set { taxaConveniencia = value; }
        }

        private decimal taxaMinima;
        public decimal TaxaMinima
        {
            get { return taxaMinima; }
            set { taxaMinima = value; }
        }

        private decimal taxaMaxima;
        public decimal TaxaMaxima
        {
            get { return taxaMaxima; }
            set { taxaMaxima = value; }
        }

        private int taxaComissao;
        public int TaxaComissao
        {
            get { return taxaComissao; }
            set { taxaComissao = value; }
        }

        private decimal comissaoMinima;
        public decimal ComissaoMinima
        {
            get { return comissaoMinima; }
            set { comissaoMinima = value; }
        }

        private decimal comissaoMaxima;
        public decimal ComissaoMaxima
        {
            get { return comissaoMaxima; }
            set { comissaoMaxima = value; }
        }
    }
}
