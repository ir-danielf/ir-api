using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaBlackListCliente
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Status { get; set; }
        public bool StatusBloqueado { get; set; }
        public bool StatusLiberado { get; set; }
        public int NivelCliente { get; set; }
        public bool BomCliente { get; set; }
        public bool MalCliente { get; set; }

        
    }
}
