using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaLeituraCodigo
    {
        public int EventoID;
        public int ApresentacaoID;
        public DateTime DataLeitura;
        public string CodigoBarra;
        public string Portaria;
        public int CodigoResultado;
        public int SetorID;
        public string SetorNome;
        public string PrecoNome;
        public int Coletor;
    }
}
