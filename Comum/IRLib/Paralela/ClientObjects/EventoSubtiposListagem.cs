
using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EventoSubtiposListagem
    {
        public int TipoID { get; set; }
        public string Tipo { get; set; }
        public int SubtipoID { get; set; }
        public string Subtipo { get; set; }
        public bool Adicionar { get; set; }
    }
}
