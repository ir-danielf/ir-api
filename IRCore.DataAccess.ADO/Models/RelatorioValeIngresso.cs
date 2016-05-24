using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRCore.DataAccess.ADO.Models
{
    public class RelatorioValeIngresso
    {
        public string TaxaEntrega { get; set; }
        public string Area { get; set; }
        public string Periodo { get; set; }
        public int Quantidade { get; set; }
        public string ValorTotalTaxa { get; set; }
    }
}