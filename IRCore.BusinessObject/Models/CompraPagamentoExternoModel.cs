
using IRCore.DataAccess.Model;

namespace IRCore.BusinessObject.Models
{
    public class CompraPagamentoExternoModel
    {
        public FormaPagamento formaPagamento { get; set; }
        public decimal valor { get; set; }
        public string codigoAutorizacao { get; set; }
        public string NSU { get; set; }
        public string IP { get; set; }
    }
}