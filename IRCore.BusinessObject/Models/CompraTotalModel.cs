using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Models
{
    public class CompraTotalModel
    {
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTaxaEntrega { get; set; }
        public decimal ValorTaxaConveniencia { get; set; }
        public decimal ValorIngressos { get; set; }

    }
}
