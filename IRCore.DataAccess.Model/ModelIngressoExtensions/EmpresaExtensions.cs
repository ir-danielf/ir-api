namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public partial class tEmpresa
    {
        public bool EmpresaPromoveAsBool
        {
            get { return (EmpresaPromove != "F"); }
            set { EmpresaPromove = (value) ? "T" : "F"; } 
        }

        public bool EmpresaVendeAsBool
        {
            get { return (EmpresaVende != "F"); }
            set { EmpresaVende = (value) ? "T" : "F"; }
        }
        public bool BannerPadraoSiteAsBool
        {
            get { return (BannerPadraoSite != "F"); }
            set { BannerPadraoSite = (value) ? "T" : "F"; }
        }

    }
}