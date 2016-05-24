
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Models
{
    public class CancelamentoLoteModel
    {
        public int CaixaID { get; set; }
        public int LojaID { get; set; }
        public int CanalID { get; set; }
        public int UsuarioID { get; set; }
        public int EmpresaID { get; set; }
        public bool EstornarEntrega { get; set; }
        public bool EstornarConveniencia { get; set; }
        public bool EstornarSeguro { get; set; }
        public IRLib.CancelamentoIngresso.EstruturaCancelamento.enuTipoCancelamento TipoCancelamento { get; set; }
        public bool TemDevolucao { get; set; }
        public int LocalID { get; set; }        
        public int MotivoCancelamento { get; set; }
        public int SubMotivoCancelamento { get; set; }        
        public IRLib.CancelamentoIngresso.EstruturaCancelamento.enuFormaDevolucao FormaDevolucao { get; set; }
        public List<int> IngressosID { get; set; }

        public IRLib.CancelamentoIngresso.EstruturaCancelamento.EstruturaDevolucaoDeposito dadosBancarios { get; set; }

        public IRLib.CancelamentoIngresso.EstruturaCancelamento.EstruturaDevolucaoEstornoCC dadosCartaoCredito { get; set; }

        public int SupervisorID { get; set; }
    }

    public class CancelamentoLoteIngressoPendenteDados
    {
        public int IngressoID { get; set; }
        public int PendenciaID { get; set; }
        public IRLib.CancelamentoIngresso.EstruturaCancelamento DadosCancelamento { get; set; }
    }

    public class CancelamentoLoteRelatorioDadosPorApresentacao
    {
        public string Horario { get; set; }
        public int Ordem {get;set;}
        public string TituloLinha{get;set;}
        public int Valor{get;set;}
    }

    public class CancelamentoLoteRelatorioDadosOperacao
    {
        public int TotalOpA { get; set; }
        public int ResolvidosOpA { get; set; }
        public string DescricaoOpA { get; set; }
        public int TotalOpB { get; set; }
        public int ResolvidosOpB { get; set; }
        public string DescricaoOpB { get; set; }
        public int TotalOpC { get; set; }
        public int ResolvidosOpC { get; set; }
        public string DescricaoOpC { get; set; }
        public int TotalOpD { get; set; }
        public int ResolvidosOpD { get; set; }
        public string DescricaoOpD { get; set; }
        public int TotalOpE { get; set; }
        public int ResolvidosOpE { get; set; }
        public string DescricaoOpE { get; set; }
        public int TotalOpF { get; set; }
        public int ResolvidosOpF { get; set; }
        public string DescricaoOpF { get; set; }
        public int TotalOpG { get; set; }
        public int ResolvidosOpG { get; set; }
        public string DescricaoOpG { get; set; }

    }
}
