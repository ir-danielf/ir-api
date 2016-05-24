namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tCotaItem
    {
        public bool ValidaBinAsBool
        {
            get
            {
                return ValidaBin == "T";
            }
            set
            {
                ValidaBin = value ? "T" : "F";
            }
        }

        public bool NominalAsBool
        {
            get
            {
                return Nominal == "T";
            }
            set
            {
                Nominal = value ? "T" : "F";
            }
        }

        public bool QuantidadeAsBool
        {
            get
            {
                return Quantidade > 0;
            }
        }

        public bool QuantidadeValidada { get; set; }

        public tCota Cota { get; set; }

        public bool ValidaCodigoPromocional { get; set; }

        public bool TemTermo { get; set; }

        public tDonoIngresso DonoIngresso { get; set; }

        public tObrigatoriedade Obrigatoriedade { get; set; }

        public tParceiro Parceiro { get; set; }

        public bool Verificado { get; set; }

        public string Mensagem { get; set; }

        public string CodigoPromocional { get; set; }
    }
}
