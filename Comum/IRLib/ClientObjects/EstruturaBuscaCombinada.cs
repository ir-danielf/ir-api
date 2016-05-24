using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaBuscaCombinada
    {
        public bool EncontrouLocais { get; set; }
        public bool EncontrouDatas { get; set; }
        public List<EstruturaEvento> Eventos { get; set; }
    }
}
