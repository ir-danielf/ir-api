namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tVendaBilheteriaFormaPagamento
    {
        public tValeIngresso tValeIngresso { get; set; }

        public tCartao tCartao { get; set; }
    }
}
