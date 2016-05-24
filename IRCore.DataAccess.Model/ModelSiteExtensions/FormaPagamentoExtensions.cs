namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class FormaPagamento
    {
        public enumFormaPagamento NomeAsEnum
        {
            get { return (enumFormaPagamento)Enum.Parse(typeof(enumFormaPagamento),Nome); }
            set { Nome = (value).ToString(); }
        }

    }
}