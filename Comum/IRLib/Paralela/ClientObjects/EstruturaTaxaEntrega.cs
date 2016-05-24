using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaTaxaEntrega : ICloneable
    {
        public int ID { get; set; }
        public int RegiaoID { get; set; }
        public string RegiaoNome { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Prazo { get; set; }
        public bool Disponivel { get; set; }
        public string Obs { get; set; }
        public string Estado { get; set; }
        public string ProcedimentoEntrega { get; set; }
        public int DiasTriagem { get; set; }
        public bool Padrao { get; set; }
        public bool EnviaAlerta { get; set; }
        public bool PermitirImpressaoInternet { get; set; }
        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
