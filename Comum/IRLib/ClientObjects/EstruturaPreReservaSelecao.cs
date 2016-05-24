using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaPreReservaSelecao
    {
        private int apresentacaosetorid;
        private int indice;

        public int ApresentacaoSetorID
        {
            get { return apresentacaosetorid; }
            set { apresentacaosetorid = value; }
        }

        public int Indice
        {
            get { return indice; }
            set { indice = value; }
        }
    }
}
