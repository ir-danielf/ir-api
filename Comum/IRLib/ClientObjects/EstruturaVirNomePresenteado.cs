using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaVirNomePresenteado
    {
        public int ID { get; set; }
        public string NomeVir { get; set; }
        public string NomePresenteado { get; set; }
    }
}
