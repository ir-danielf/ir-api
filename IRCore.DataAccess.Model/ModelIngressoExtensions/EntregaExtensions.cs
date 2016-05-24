namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tEntrega
    {
        public enumEntregaTipo TipoAsEnum
        {

            get { return (enumEntregaTipo)Tipo[0]; }
            set { Tipo = ((char)value).ToString(); }
        }

        public List<ParceiroMidiaEntrega> ParceiroMidiaEntrega { get; set; }

    }
}