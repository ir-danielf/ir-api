using System;
using System.Runtime.Serialization;

namespace IRLib.ClientObjects
{
    [Serializable]
    //essa estrutura pode ser usada para qq clase que precise retornar ID e Nome.
    public class EstruturaIDNome
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Nome { get; set; }

        public decimal Valor { get; set; }
    }
}

