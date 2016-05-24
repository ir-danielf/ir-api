using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRCore.DataAccess.ADO.Models
{
    public class RelatorioTaxaEntrega
    {
        public string TaxaEntrega { get; set; }
        public string Area { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotalTaxa { get; set; }
    }
}