using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoValoresValeIngresso
    {
        public EstruturaRetornoValoresValeIngresso()
        {
            ListVirs = new List<EstruturaTrocaValeIngresso>();
        }
        public decimal ValorEntrega { get; set; }
        public decimal ValorConveniencia { get; set; }
        public decimal ValorIngressos { get; set; }
        public bool ValidadoVIR { get; set; }
        public List<EstruturaTrocaValeIngresso> ListVirs { get; set; }
        public bool ValidadoVIRAcumulativo { get; set; }
        public string CodigoTrocaFixo { get; set; }
        public decimal ValorTotal
        {
            get
            {
                return this.ValorConveniencia + this.ValorEntrega + this.ValorIngressos;
            }
        }
        public string Mensagem { get; set; }
    }
}
