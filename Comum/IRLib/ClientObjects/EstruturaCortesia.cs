using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCortesia
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int LocalID { get; set; }
        public int CorID { get; set; }
        public string Obs { get; set; }
        public string Padrao { get; set; }
        public int ParceiroMidiaID { get; set; }
    }
}
