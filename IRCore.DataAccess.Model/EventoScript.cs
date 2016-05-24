using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class EventoScriptModel
    {
        public int ID { get; set; }

        public int EventoID { get; set; }

        public string EventoNome { get; set; }

        public string Script { get; set; }

    }
}
