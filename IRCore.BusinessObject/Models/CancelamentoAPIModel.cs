using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Models
{
    public class CancelamentoAPIModel
    {
        public string SenhaCompra { get; set; }
        public List<int> Ingressos { get; set; }
        public bool CancelaEntrega { get; set; }
        public bool CancelaSeguro { get; set; }
        public bool CienteDevolucao { get; set; }
        public string FormaDevolucao { get; set; }
        public DadosDepositoModel DadosDeposito { get; set; }
        public bool CartaoProprio { get; set; }
        public DadosCartaoModel DadosCartao { get; set; }
    }

    public class DadosDepositoModel
    {
        public string Poupanca { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string DV { get; set; }
        public string Conta { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
    }

    public class DadosCartaoModel
    {
        public string TitularNome { get; set; }
        public string TitularCPF { get; set; }
    }
}
