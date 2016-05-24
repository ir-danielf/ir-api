using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class CortesiaModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int LocalID { get; set; }
        public int CorID { get; set; }
        public string Obs { get; set; }
        public string Padrao { get; set; }
        public int ParceiroMidiaID { get; set; }

        public class CortesiaModelInsercao
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public int LocalID { get; set; }
            public int CorID { get; set; }
            public string Obs { get; set; }
            public string Padrao { get; set; }
            public int ParceiroMidiaID { get; set; }
            public int UsuarioID { get; set; }
        }
    }
}
