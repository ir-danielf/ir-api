namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public partial class tEntregaControle
    {

        private tEntrega _Entrega;
        public tEntrega Entrega { get; set; }

        public bool AtivaAsBool
        {
            get { return (Ativa != "F"); }
            set { Ativa = (value) ? "T" : "F"; }
        }
 

    }
}