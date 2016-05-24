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
    public class CompraPagamentoModel
    {
        public FormaPagamento formaPagamento { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string CodigoSeguranca { get; set; }
        public int MesValidade { get; set; }
        public int AnoValidade { get; set; }
        public decimal Valor { get; set; }
        public string IP { get; set; }
        public int? ParceiroID { get; set; }
        public string CodigoTrocaFixo { get; set; }
        public PagamentoPayPal PayPal { get; set; }
    }
}
