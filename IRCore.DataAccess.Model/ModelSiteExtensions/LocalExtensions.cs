namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using IRCore.Util;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class Local
    {
        public virtual List<Evento> Evento { get; set; }

        public double Distancia { get; set; }

    }
}
