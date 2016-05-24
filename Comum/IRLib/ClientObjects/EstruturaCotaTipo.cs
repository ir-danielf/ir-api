using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotaTipo
    {
        public int ID { get; set; }

        public string Descricao { get; set; }

        public int UsuarioID { get; set; }
    }
}
