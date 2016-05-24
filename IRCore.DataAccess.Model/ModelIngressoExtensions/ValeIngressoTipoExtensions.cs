namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tValeIngressoTipo
    {
        public bool AcumulativoAsBool
        {
            get
            {
                return Acumulativo == "T";
            }
            set
            {
                Acumulativo = value ? "T" : "F";
            }
        }

        public int NumeroIncidencias
        {
            get
            {
                return (this.TrocaIngresso == "T" ? 1 : 0) + (this.TrocaConveniencia == "T" ? 1 : 0) + (this.TrocaEntrega == "T" ? 1 : 0);
            }
        }

        public bool TrocaIngressoAsBool
        {
            get
            {
                return TrocaIngresso == "T";
            }
            set
            {
                TrocaIngresso = value ? "T" : "F";
            }
        }

        public bool TrocaEntregaAsBool
        {
            get
            {
                return TrocaEntrega == "T";
            }
            set
            {
                TrocaEntrega = value ? "T" : "F";
            }
        }

        public bool TrocaConvenienciaAsBool
        {
            get
            {
                return TrocaConveniencia == "T";
            }
            set
            {
                TrocaConveniencia = value ? "T" : "F";
            }
        }
    }
}
