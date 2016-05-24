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
    public class EstruturaTrocaIngressoPreco
    {
        public enum enumTipoTroca
        {
            Preco,
            Ingresso
        }
        public enum enumModoTroca
        {
            ComDevolucao,
            ComPagamento,
            Nada
        }

        /// <summary>
        /// Identifica qual o tipo da troca sendo realizada
        /// </summary>
        public enumTipoTroca TipoTroca { get; set; }

        /// <summary>
        /// Identifica se a troca tem Devolução, Pagamento ou nada
        /// </summary>
        public enumModoTroca ModoTroca { get; set; }

        /// <summary>
        /// Primeiro campo int é o ID do Ingresso
        /// Segundo campo int é o ID do Preço Novo
        /// </summary>
        public Dictionary<int, int> IngressosPrecos { get; set; }

        /// <summary>
        /// Soma do valor antigo dos Ingressos que estão sendo trocados
        /// Venda = é usados para o crédido do cliente que ele já pagou antes da troca (ex: Se ele tinha Ingr R$ 15,00 e trocou por um de R$ 20,00 este valor é R$ 15,00)
        /// </summary>
        public decimal ValorIngressosCompraAntiga { get; set; }

        /// <summary>
        /// Valor novo dos ingressos = R$ 20,00 se baseado no exemplo do campo (ValorIngressosCompraAntiga)
        /// </summary>
        public decimal ValorIngressosCompraNova { get; set; }

        /// <summary>
        /// Valor da conveniência da compra antiga.
        /// </summary>
        public decimal ValorConvenienciaCompraAntiga { get; set; }

        /// <summary>
        /// Valor da conveniência da compra nova.
        /// </summary>
        public decimal ValorConvenienciaCompraNova { get; set; }

        /// <summary>
        /// Valor do Seguro e da Entrega.
        /// </summary>
        public decimal ValorSeguroEntrega { get; set; }

        /// <summary>
        /// Valor do Total da nova compra.
        /// </summary>
        public decimal ValorTotalNovaCompra { get { return ValorIngressosCompraNova + ValorConvenienciaCompraNova; } }

        /// <summary>
        /// Valor a ser cobrado na diferença da Troca 
        /// </summary>
        public decimal ValorDiferencaCobrado { get { return ValorTotalNovaCompra - ValorIngressosCompraAntiga; } }

        /// <summary>
        /// DataTable com os Ingressos sendo trocados, para montagem dos Grids na tela
        /// </summary>
        public DataTable DataTableIngressosTroca { get; set; }

        /// <summary>
        /// DataTable com todos os ingressos da compra original
        /// </summary>
        public DataTable DataTableIngressosOriginais { get; set; }

        /// <summary>
        /// Estrutura de dados para execução do cancelamento
        /// </summary>
        public EstruturaCancelamento DadosCancelamento { get; set; }

        /// <summary>
        /// Estrutura de Dados para a compra refenrente a troca
        /// </summary>
        public EstruturaTrocaIngressoCompra DadosTrocaingressoCompra { get; set; }

        /// <summary>
        /// Estrutura para retorno de Dados na execução da troca
        /// </summary>
        public EstruturaTrocaIngressoRetorno  DadosTrocaIngressoRetorno { get; set; }


        public string ClienteNome
        {
            get
            {
                try
                {
                    if (DadosCancelamento != null &&
                        !string.IsNullOrWhiteSpace(DadosCancelamento.ClienteNome))
                        return DadosCancelamento.ClienteNome;
                    else if (DataTableIngressosOriginais != null &&
                        DataTableIngressosOriginais.Rows.Count > 0)
                        return DataTableIngressosOriginais.Rows[0][NOME_CLIENTE].ToString();
                    else return string.Empty;
                }
                catch { return string.Empty; }
            }
        }

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


        #region Construtores
        /// <summary>
        /// Utilizar sempre este construtor
        /// </summary>
        /// <param name="tipoTroca">Identifica qual o tipo de troca que esta sendo realizado</param>
        public EstruturaTrocaIngressoPreco(enumTipoTroca tipoTroca)
        {
            TipoTroca = tipoTroca;
        }

        /// <summary>
        /// Não utilize este construtor, mas ele é necessário para que o Serialize funcione
        /// </summary>
        public EstruturaTrocaIngressoPreco()
        {
        } 
        #endregion
       
    }
}
