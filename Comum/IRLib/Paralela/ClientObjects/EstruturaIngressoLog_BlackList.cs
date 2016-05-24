using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaIngressoLog_BlackList
    {
        public int IngressoID;
        public DateTime TimeStamp;
        public string Acao;
        public string CodigoBarra;
        public string Obs;
    }
}
