using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaGerenciamentoIngressos : ICloneable
    {
        public int GerenciamentoIngressosID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int PrecoTipoID { get; set; }
        public string Evento { get; set; }
        public string Data { get; set; }
        public string TipoPreco { get; set; }
        public string Horario { get; set; }
        public string Label { get; set; }
        public int Vendido { get; set; }
        public int Disponivel { get; set; }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
