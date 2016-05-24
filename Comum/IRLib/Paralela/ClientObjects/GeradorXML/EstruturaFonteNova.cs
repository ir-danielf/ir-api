using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaFonteNova
    {
        public int ID { get; set; }

        public string CodigoBarra { get; set; }

        public int PrecoTipoID { get; set; }

        public string Codigo { get; set; }

        public string LugarMarcado { get; set; }

        public decimal Valor { get; set; }

        public int VendaBilheteriaID { get; set; }

        public int ClienteID { get; set; }

        public string Cliente { get; set; }

        public string Chave { get; set; }

        public string Evento { get; set; }

        public string aIdEvento { get; set; }
    }
}
