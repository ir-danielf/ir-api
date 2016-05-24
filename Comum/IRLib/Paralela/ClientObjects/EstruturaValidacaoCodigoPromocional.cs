using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaValidacaoCodigoPromocional
    {
        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public String CodigoPromocional { get; set; }
    }
}
