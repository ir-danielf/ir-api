using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaFaqTipo
    {
        public int ID { get; set; }
        public string Nome { get; set; }
    }

    [Serializable]
    public class EstruturaFaqExibicao
    {
        public string ID { get; set; }
        public string Nome { get; set; }
    }

}
