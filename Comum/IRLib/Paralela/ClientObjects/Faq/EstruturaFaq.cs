using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaFaq
    {
        public int ID { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public int FaqTipoID { get; set; }
        public string Tags { get; set; }

    }
}
