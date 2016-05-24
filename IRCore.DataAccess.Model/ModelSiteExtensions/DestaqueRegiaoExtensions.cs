namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class DestaqueRegiao
    {
        public enumDestaqueRegiaoTipo TipoAsEnum
        {
            get { return (enumDestaqueRegiaoTipo)Tipo[0]; }
            set { Tipo = ((char)value).ToString(); }
        }

    }
}
