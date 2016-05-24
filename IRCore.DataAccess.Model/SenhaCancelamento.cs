using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class SenhaCancelamentoModel
    {
        public int ID { get; set; }
        public string SenhaCancelamento  { get; set; }
        public string SenhaCompra { get; set; }
        public DateTime DataCancelamento { get; set; }
        public DateTime DataCompra { get; set; }
        public int CanalID { get; set; }
        public string CanalNome { get; set; }
        public int LojaID { get; set; }
        public string LojaNome { get; set; }
        public decimal ValorCancelamento { get; set; }
        public decimal ValorCompra { get; set; }
        public string StatusCancelamento { get; set; }
        public int Ingressos { get; set; }
        public int ValeIngressos { get; set; }
        public bool TaxaConveniencia { get; set; }
        public bool TaxaEntrega { get; set; }
        public bool SeguroMondial { get; set; }
    }
}
