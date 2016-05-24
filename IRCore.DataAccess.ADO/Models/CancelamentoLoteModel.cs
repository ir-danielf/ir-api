using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class CancelamentoLoteMailModel
    {
        public string Cliente { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string CPF { get; set; }

        public DateTime Data { get; set; }
        public string Canal { get; set; }
        public string Pagamento { get; set; }
        public string Evento { get; set; }
        public string DescricaoEmail { get; set; }

    }

    public class CancelamentoLoteIngressoPendente
    {
        public int PendenciaID { get; set; }
        public int IngressoID { get; set; }
    }

    public class CancelamentoLoteDadosCancelamento
    {
        public int EntregaControleID { get; set; }
        public int EntregaAgendaID {get;set;}
        public string SenhaVenda {get;set;}
        public int ClienteID { get; set; }
        public int VendaBilheteriaIDVenda { get; set; }
        public bool Fraude { get; set; }

        public decimal ValorEntregaTotal { get; set; }
        public decimal ValorConvenienciaTotal { get; set; }
        public decimal ValorIngressosTotal { get; set; }
        public decimal ValorSeguroTotal { get; set; }
    }

    public class CancelamentoLoteValoresPendentes
    {
        public decimal EntregaValor { get; set; }
        public decimal SeguroValor { get; set; }
        public decimal ConvenienciaValor { get; set; }
        public decimal IngressoValor { get; set; }
    }

    public class CancelamentoLoteDadosImpressao
    {
        public int qtdIngresso { get; set; }
        public int qtdEntrega { get; set; }
        public int qtdETicket { get; set; }
        public int qtdImpresso { get; set; }
    }

    public class CancelamentoLoteDadosCliente
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Cartao { get; set; }
        public string Bandeira { get; set; }

    }

    public class CancelamentoRelatorioDadosBasicos
    {
        public string Evento { get; set; }
        public string Local { get; set; }
        public DateTime Data { get; set; }
        public string Regional { get; set; }
        public string Empresa { get; set; }
        public string Usuario { get; set; }
        public string Motivo { get; set; }
    }

    public class CancelamentoRelatorioDadosTotais
    {
        public int TotalApresentacoes { get; set; }
        public int TotalIngressos { get; set; }
        public int Disponiveis { get; set; }
        public int Vendidos { get; set; }
        public int Impressos { get; set; }
        public bool VendaAtiva { get; set; }
        public int PacotesDesativados { get; set; }
    }

    public class CancelamentoRelatorioDadosOperacoes
    {
        public string Operacao { get; set; }
        public int Total { get; set; }
        public int Resolvido { get; set; }
        public string Descricao { get; set; }
        public string AcaoIR { get; set; }
        public string Cancelamento { get; set; }

    }

    public class CancelamentoRelatorioMatrizApresentacoes
    {
        public string Apresentacao1 { get; set; }
        public string Apresentacao2 { get; set; }
        public string Apresentacao3 { get; set; }
        public string Apresentacao4 { get; set; }

    }

   
}
