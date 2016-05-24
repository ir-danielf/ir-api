using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaBlackList2
    {
        public int ID { get; set; }
        public string Tipo {get;set;}
        public string Identificador {get;set;}
        public string TimeStamp {get;set;}
    }
}
