using System;
using System.Runtime.Serialization;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEventoDestaque
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Evento { get; set; }

        [DataMember]
        public string Local { get; set; }

        [DataMember]
        public string Imagem { get; set; }
    }
}
