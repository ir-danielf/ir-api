using IRLib.CancelamentoIngresso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.TrocaIngresso
{
    [Serializable]
    public class EstruturaTrocaIngressoCredito
    {
        
        public enum enumModoTroca
        {
            ComDevolucao,
            ComPagamento,
            Nada
        }

        /// <summary>
        /// Identifica se a troca tem Devolução, Pagamento ou nada
        /// </summary>
        public enumModoTroca ModoTroca { get; set; }

        public List<int> IngressosIds { get; set; }

        public decimal ValorIngressosCompraAntiga { get; set; }

        public decimal ValorIngressosCompraNova { get; set; }

        public decimal ValorConvenienciaCompraAntiga { get; set; }

        public decimal ValorConvenienciaCompraNova { get; set; }

        public decimal ValorEntregaCompraAntiga { get; set; }

        public decimal ValorEntregaCompraNova { get; set; }

        public decimal ValorSeguroCompraAntiga { get; set; }

        public decimal ValorSeguroCompraNova { get; set; }

        public decimal ValorTotalCompraNova { get { return ValorIngressosCompraNova + ValorConvenienciaCompraNova + ValorEntregaCompraNova + ValorSeguroCompraNova; } }
        public decimal ValorTotalCompraAntiga { get { return ValorIngressosCompraAntiga + ValorConvenienciaCompraAntiga + ValorEntregaCompraAntiga + ValorSeguroCompraAntiga; } }

        public decimal ValorDiferencaCobrado { get { return ValorTotalCompraNova - ValorTotalCompraAntiga; } }

        public DataTable DataTableIngressosTroca { get; set; }

        public DataTable DataTableItensTroca { get; set; }

        public EstruturaCancelamento DadosCancelamento { get; set; }

        public EstruturaTrocaIngressoCompra DadosTrocaingressoCompra { get; set; }

        
        public EstruturaTrocaIngressoRetorno  DadosTrocaIngressoRetorno { get; set; }


        /// <summary>
        /// DataTable com todos os ingressos da compra original
        /// </summary>
        public DataTable DataTableIngressosOriginais { get; set; }

        public int ClienteID { get; set; }
        public int VendaBilheteriaID { get; set; }

        public DataTable DataTableFormaPagamentoOriginais { get; set; }

        #region CONSTANTES COLUNAS DadosGrid

        public const string RESERVA_ID = "ReservaID";
        public const string INGRESSO_ID = "IngressoID";
        public const string EVENTO = "Evento";
        public const string EVENTO_ID = "EventoID";
        public const string HORARIO = "Horario";
        public const string SETOR = "Setor";
        public const string TIPO_LUGAR = "TipoLugar";
        public const string CODIGO = "Cod";
        public const string PRECO = "Preco";
        public const string CORTESIA = "Cortesia";
        public const string VALOR = "Valor R$";
        public const string TAXA_CONVENIENCIA = "TX %";
        public const string VALOR_CONVENIENCIA = "Conv R$";
        public const string CONVENIENCIA_MINIMA = "TaxaMin(R$)";
        public const string CONVENIENCIA_MAXIMA = "TaxaMax(R$)";
        public const string CLIENTE = "Cliente";
        public const string CPF = "CPF";
        public const string SERIE = "Serie";
        public const string APRESENTACAO_ID = "ApresentacaoID";
        public const string APRESENTACAO_SETOR_ID = "ApresentacaoSetorID";
        public const string OBRIGATORIEDADE_ID = "ObrigatoriedadeID";
        public const string TIPO_CODIGO_BARRA = "TipoCodigoBarra";
        public const string PRECO_ID = "PrecoID";
        public const string GERENCIAMENTO_INGRESSOS_ID = "GerenciamentoIngressosID";
        public const string BLOQUEIO_ID = "BloqueioID";
        public const string CORTESIA_ID = "CortesiaID";
        public const string CODIGO_SEQUENCIAL = "CodigoSequencial";
        public const string CODIGO_BARRA = "CodigoBarra";
        public const string TIPO_PACOTE_INGRESSO = "TipoPacoteIngresso";
        public const string PACOTE_ID = "PacoteID";
        public const string PERMITIR_CANCELAMENTO_AVULSO = "PermitirCancelamentoAvulso";
        
        public const string LOCAL = "Local";
        public const string PRECO_ORIGINAL = "PrecoOriginal";
        public const string VALOR_ORIGINAL = "ValorOriginal";
        public const string NOME_CLIENTE = "NomeCliente";
        
        

        public const string APRESENTACAO_COTA_ID = "ApresentacaoCotaID";
        public const string APRESENTACAO_SETOR_COTA_ID = "ApresentacaoSetorCotaID";
        public const string COTA_ITEM_ID_ORIGINAL = "CotaItemIDOriginal";
        public const string COTA_ITEM_ID_APS_ORIGINAL = "CotaItemIDAPSOriginal";
        public const string COTA = "Cota";
        public const string COTA_ITEM_ID = "CotaItemID";
        public const string COTA_ITEM_ID_APS = "CotaItemIDAPS";
        public const string DONO_ID = "DonoID";
        public const string NOMINAL = "Nominal";
        public const string VALIDA_BIN = "ValidaBin";
        public const string VALIDA_BIN_APS = "ValidaBinAPS";
        public const string STATUS_CODIGO_PROMO = "StatusCodigoPromo";
        public const string PARCEIRO_ID = "ParceiroID";
        public const string PARCEIRO_ID_APS = "ParceiroIDAPS";
        public const string QUANTIDADE_COTA = "QuantidadeCota";
        public const string QUANTIDADE_POR_CLIENTE_COTA = "QuantidadePorClienteCota";
        public const string QUANTIDADE_COTA_APS = "QuantidadeCotaAPS";
        public const string QUANTIDADE_POR_CLIENTE_COTA_APS = "QuantidadePorClienteCotaAPS";
        public const string QUANTIDADE_APRESENTACAO = "QuantidadeApresentacao";
        public const string QUANTIDADE_POR_CLIENTE_APRESENTACAO = "QuantidadePorClienteApresentacao";
        public const string QUANTIDADE_APRESENTACAO_SETOR = "QuantidadeApresentacaoSetor";
        public const string QUANTIDADE_POR_CLIENTE_APRESENTACAO_SETOR = "quantidadePorClienteApresentacaoSetor";
        public const string CODIGO_PROMO = "CodigoPromo";
        public const string PRECO_INICIA_COM = "PrecoIniciaCom";
        

        #endregion


        public string ClienteNome { get; set; }

        public string DescricaoEntrega { get; set; }

        public bool TrocaConvNaoAutorizada { get; set; }
    }
}
