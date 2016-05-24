using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    [Serializable]
    public class EstruturaCancelamento
    {
        #region Enums de Cancelamento
        public enum enuTipoCancelamento
        {
            [Description("Normal")]
            Normal,
            [Description("DevolucaoDinheiroSemCancelamento")]
            DevolucaoDinheiroSemCancelamento,
            [Description("CancelamentoSemDevolucaoDinheiro")]
            CancelamentoSemDevolucaoDinheiro,
            [Description("TrocaSemDevolucaoDinheiro")]
            TrocaSemDevolucaoDinheiro,
            [Description("TrocaComDevolucaoDinheiro")]
            TrocaComDevolucaoDinheiro
        }

        public enum enuFormaDevolucao
        {
            [Description("SemDevolucao")]
            SemDevolucao,
            [Description("Dinheiro")]
            Dinheiro,
            [Description("EstornoCC")]
            EstornoCC,
            [Description("Deposito")]
            Deposito,
            [Description("PayPal")]
            PayPal
        }

        public enum enuEntregaStatus
        {
            [Description("SacEntregaInexistente")]
            EntregaInexistente,

            [Description("SacEntregaEfetuada")]
            EntregaJaEfetuada,

            [Description("SacEntregaNaoEfetuada")]
            EntregaNaoEfetuada,

            [Description("SacEntregaCancelada")]
            EntregaCancelada
        }

        public enum enuStatus
        {
            [Description("CancelamentoPendente")]
            CancelamentoPendente,

            [Description("CancelamentoProcessado")]
            CancelamentoProcessado,

            [Description("CancelamentoAutomatico")]
            CancelamentoAutomatico,

            [Description("CancelamentoCancelado")]
            CancelamentoCancelado,

            [Description("CancelamentoNaoAutorizado")]
            CancelamentoNaoAutorizado
        }

        public enum enuConvenienciaStatus
        {
            [Description("SacConvenienciaNaoCancelavel")]
            NaoCancelavel,

            [Description("SacConvenienciaCancelada")]
            Cancelada
        }

        public enum enuIngressoStatus
        {
            [Description("SacIngressoAindaNaoEntregue")]
            IngressoNaoSaiuEntrega,

            [Description("SacIngressoPosseCliente")]
            IngressoEmPosseCliente,

            [Description("SacIngressoAguardandoDevolucao")]
            IngressoAguardandoDevolucao,

            [Description("SacIngressoPosseIR")]
            IngressoEmPosseIR
        }
        public enum enuPrazoDevolucaoStatus
        {
            [Description("SacPrazoNormal")]
            DevolucaoPrazoNormal,

            [Description("SacPrazoPendencias")]
            DevolucaoPrazoPendencias
        }
        #endregion

        #region Estrutura de Dados de Devolução
        [Serializable]
        public class EstruturaDevolucaoDinheiro
        {
            [DisplayName("Nome")]
            public string Nome { get; set; }

            [DisplayName("Email de Notificação")]
            public string Email { get; set; }

            public EstruturaDevolucaoDinheiro()
            {
                Nome = string.Empty;
                Email = string.Empty;
            }
        }
        [Serializable]
        public class EstruturaDevolucaoDeposito
        {
            [DisplayName("Corrente")]
            public bool IsContaCorrente { get; set; }

            [DisplayName("Nome do Correntista")]
            public string Correntista { get; set; }

            public string CPF { get; set; }

            [Browsable(false)]
            public string Banco { get; set; }

            public string Agencia { get; set; }

            public string Conta { get; set; }

            public string Digito { get; set; }

            [DisplayName("Email de Notificação")]
            public string Email { get; set; }

            public EstruturaDevolucaoDeposito()
            {
                IsContaCorrente = true;
                Banco = string.Empty;
                Conta = string.Empty;
                Agencia = string.Empty;
                CPF = string.Empty;
                Correntista = string.Empty;
                Email = string.Empty;
            }
        }
        [Serializable]
        public class EstruturaDevolucaoEstornoCC
        {
            [DisplayName("Nome do titular")]
            public string TitularCartao { get; set; }

            [DisplayName("CPF do titular")]
            public string TitularCPF { get; set; }

            public string Bandeira { get; set; }

            [DisplayName("Numero do cartão")]
            public string NumeroCartao { get; set; }

            [Browsable(false)]
            public int Estabelecimento { get; set; }

            [Browsable(false)]
            public string NSU { get; set; }

            [Browsable(false)]
            public int Rede { get; set; }

            [DisplayName("Email de Notificação")]
            public string Email { get; set; }

            public EstruturaDevolucaoEstornoCC()
            {
                NSU = string.Empty;
                NumeroCartao = string.Empty;
                TitularCartao = string.Empty;
                TitularCPF = string.Empty;
                Bandeira = string.Empty;
                Email = string.Empty;
            }
        }
        #endregion

        /// <summary>
        ///  Status = 'A'
        /// </summary>
        public const string STATUS_ESTORNO_AUTOMATICO = "A";  //Estorno automática, durante o cancelamento e/ou devolução

        /// <summary>
        ///  Status = 'S'
        /// </summary>
        public const string STATUS_ESTORNO_SOLICITADO = "S";  //Estorno solicitado aguardando devolução de Ingressos

        /// <summary>
        ///  Status = 'P'
        /// </summary>
        public const string STATUS_ESTORNO_PENDENTE = "P";    //Estorno pendente

        /// <summary>
        ///  Status = 'C'
        /// </summary>
        public const string STATUS_ESTORNO_CANCELADO = "C";   //Solicitação de estorno foi cancelada

        #region Propriedades
        public int CaixaID { get; set; }
        public int LocalID { get; set; }
        public int LojaID { get; set; }
        public int CanalID { get; set; }
        public int UsuarioID { get; set; }
        public int EmpresaID { get; set; }

        public bool EhCanalPresente { get; set; }

        public string SenhaVenda { get; set; }
        public int VendaBilheteriaIDVenda { get; set; }
        public bool CancelamentoFraude { get; set; }
        public int EntregaControleID { get; set; }
        public int EntregaAgendaID { get; set; }

        public DataTable DadosItensVendidos { get; set; }
        public DataTable DadosIngressosVendidos { get; set; }
        public DataTable DadosPagamentos { get; set; }

        public enuEntregaStatus EntregaStatus { get; set; }
        public enuConvenienciaStatus ConvenienciaStatus { get; set; }
        public enuIngressoStatus IngressoStatus { get; set; }
        public enuPrazoDevolucaoStatus PrazoDevolucaoStatus { get; set; }

        public int MotivoCancelamento { get; set; }
        public int SubMotivoCancelamento { get; set; }

        public enuTipoCancelamento TipoCancelamento { get; set; }
        public enuFormaDevolucao FormaDevolucao { get; set; }

        public int ClienteID { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteEmail { get; set; }
        public int CartaoID { get; set; }

        public bool TemDevolucao { get; set; }

        public decimal ValorEntregaTotal { get; set; }
        public decimal ValorEntregaEstornado { get; set; }
        public decimal ValorConvenienciaTotal { get; set; }
        public decimal ValorConvenienciaEstornada { get; set; }
        public decimal ValorIngressosTotal { get; set; }
        public decimal ValorIngressosEstornado { get; set; }
        public decimal ValorSeguroTotal { get; set; }
        public decimal ValorSeguroEstornado { get; set; }

        
        public decimal ValorCompraTotal
        {
            get
            {
                return ValorEntregaTotal + ValorConvenienciaTotal + ValorIngressosTotal + ValorSeguroTotal;
            }
        }
        public decimal ValorEstornoTotal
        {
            get
            {
                return ValorEntregaEstornado + ValorConvenienciaEstornada + ValorIngressosEstornado + ValorSeguroEstornado;
            }
        }

        public bool Autorizado { get; set; }
        public string NumeroChamado { get; set; }
        public int SupervisorID { get; set; }


        public EstruturaDevolucaoDeposito DadosDeposito { get; set; }
        public EstruturaDevolucaoDinheiro DadosDinheiro { get; set; }
        public EstruturaDevolucaoEstornoCC DadosEstornoCC { get; set; }


        public bool EstornoEfetuado { get; set; }
        public string NotaFiscalCliente { get; set; }
        public string NotaFiscalEstabelecimento { get; set; }


        public string SenhaCancelamento { get; set; }
        public int VendaBilheteriaIDCancelamento { get; set; }

        public enuStatus Status { get; set; }

        public int PendenciaID { get; set; }


        public int CaixaIDDevolucao { get; set; }
        public int LocalIDDevolucao { get; set; }
        public int LojaIDDevolucao { get; set; }
        public int CanalIDDevolucao { get; set; }
        public int EmpresaIDDevolucao { get; set; }
        public int UsuarioIDDevolucao { get; set; }

        public decimal ValorEstornoparcial { get; set; }
        #endregion

        #region Metodos Publicos
        public void CarregarDadosEstornoCC(DataTable dadosCartao)
        {
            DadosEstornoCC = new EstruturaDevolucaoEstornoCC();

            for (int i = 0; i < dadosCartao.Rows.Count; i++)
            {
                DadosEstornoCC.Bandeira = Convert.ToString(dadosCartao.Rows[i]["Bandeira"]);
                DadosEstornoCC.NumeroCartao = Convert.ToString(dadosCartao.Rows[i]["NumeroCartao"]);
                DadosEstornoCC.TitularCartao = Convert.ToString(dadosCartao.Rows[i]["TitularCartao"]);
            }
        }
        #endregion
    }


}

