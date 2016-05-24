using System;


namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaDespesas : ICloneable
    {
        public int ID { get; set; }
        public int LocalID { get; set; }
        public int EventoID { get; set; }
        public int ApresentacaoID { get; set; }
        public int TipoPagamentoID { get; set; }
        public string Nome { get; set; }
        public bool ValorLiquido { get; set; }
        public string TipoValor
        {
            get
            {
                if (ValorLiquido)
                    return "Valor Liquido";
                else
                    return "Valor Bruto";

            }
        }
        public string ValorVisualizacao
        {
            get
            {
                if (PorPorcentagem)
                    return Valor + "%";
                else
                    return Valor.ToString("c");

            }
        }
        public string ValorMinimoVisualizacao
        {
            get
            {
                return ValorMinimo.ToString("c");

            }
        }
        public string Obs { get; set; }
        public bool PorPorcentagem { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorMinimo { get; set; }

        public bool TipoFormaPagamento { get; set; }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }


}
