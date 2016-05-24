using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRCore.DataAccess.ADO.Models
{
    public class RelatorioValeIngressoVenda
    {
        public string Canal { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
    }
}