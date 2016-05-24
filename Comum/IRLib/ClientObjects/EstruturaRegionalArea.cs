using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRegionalArea
    {
        public bool Distribuido { get; set; }
        public int RegionalAreaID { get; set; }
        public int EntregaAreaID { get; set; }
        public string EntregaAreaNome { get; set; }


    }
}
