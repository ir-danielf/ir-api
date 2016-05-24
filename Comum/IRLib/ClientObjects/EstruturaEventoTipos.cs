using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEventoTipos
    {
        public int ID { get; set; }
        public int EventoID { get; set; }
        public int TipoID { get; set; }
        public string Tipo { get; set; }
        public int SubtipoID { get; set; }
        public string Subtipo { get; set; }
        public bool Adicionar { get; set; }
        public bool AdicionarAntigo { get; set; }

    }
}
