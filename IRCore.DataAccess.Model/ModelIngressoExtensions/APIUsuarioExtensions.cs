namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class APIUsuario
    {

        public enumAPITipoAcesso TipoAcessoAsEnum
        {

            get { return (enumAPITipoAcesso)TipoAcesso; }
            set { TipoAcesso = ((int)value); }
        } 

    }
}