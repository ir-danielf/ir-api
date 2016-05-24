using CTLib;
using IngressoRapido.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IngressoRapido.Ecommerce
{
    public class ItauShopLine
    {

        private string clienteID;
        public string ClienteID
        {
            get { return clienteID; }
            set { clienteID = value; }
        }

        private string sessionID;
        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        private string codEmp;
        public string CodEmp
        {
            get { return codEmp; }
            set { codEmp = value; }
        }

        private string pedido;
        public string Pedido
        {
            get { return pedido; }
            set { pedido = value; }
        }

        private string valor;
        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private string tipPag;
        public string TipPag
        {
            get { return tipPag; }
            set { tipPag = value; }
        }

        //private string sitPag;
        //public string SitPag
        //{
        //    get { return sitPag; }
        //    set { sitPag = value; }
        //}

        public enuSituacaoPagamento SitPag { get; set; }


        private string dataPagamento;
        public string DataPagamento
        {
            get { return dataPagamento; }
            set { dataPagamento = value; }
        }

        private string formaPagamento;
        public string FormaPagamento
        {
            get { return formaPagamento; }
            set { formaPagamento = value; }
        }

        private string codAutorizacao;
        public string CodAutorizacao
        {
            get { return codAutorizacao; }
            set { codAutorizacao = value; }
        }

        private string numId;
        public string NumID
        {
            get { return numId; }
            set { numId = value; }
        }

        private string compVenda;
        public string CompVenda
        {
            get { return compVenda; }
            set { compVenda = value; }
        }

        private string tipCart;
        public string TipCart
        {
            get { return tipCart; }
            set { tipCart = value; }
        }


        public int taxaEntregaID;
        public int TaxaEntregaID
        {
            get { return taxaEntregaID; }
            set { taxaEntregaID = value; }
        }

        public int EnderecoID { get; set; }
        public int PDVSelecionado { get; set; }
        public string DataSelecionada { get; set; }


        public decimal taxaEntregaValor;
        public decimal TaxaEntregaValor
        {
            get { return taxaEntregaValor; }
            set { taxaEntregaValor = value; }
        }

        public int formaPagamentoID;
        public int FormaPagamentoID
        {
            get { return formaPagamentoID; }
            set { formaPagamentoID = value; }
        }

        public string msgErro;
        public string MsgErro
        {
            get { return msgErro; }
            set { msgErro = value; }
        }



        private List<int> _listaApresentacoes;
        public List<int> ListaApresentacoes
        {
            get { return _listaApresentacoes; }
            set { _listaApresentacoes = value; }
        }
        private IRLib.ClientObjects.EstruturaValeIngresso valeIngresso;
        public IRLib.ClientObjects.EstruturaValeIngresso ValeIngresso
        {
            get { return valeIngresso; }
            set { valeIngresso = value; }
        }

        #region Enumeradores

        /// <summary>
        /// Código de inscrição do sacado.
        /// </summary>
        public enum enuCodigoInscricao
        {
            [System.ComponentModel.Description("Não definido.")]
            NaoDefinido = 0,
            [System.ComponentModel.Description("CPF.")]
            CPF = 1,
            [System.ComponentModel.Description("CNPJ.")]
            CNPJ = 2
        }

        /// <summary>
        /// Tipo de pagamento escolhido pelo comprador.
        /// </summary>
        public enum enuTipoPagamento
        {
            [System.ComponentModel.Description("Pagamento ainda não escolhido.")]
            NaoEscolhido = 0,
            [System.ComponentModel.Description("Pagamento à vista (TEF e CDC).")]
            PagamentoAVista = 1,
            [System.ComponentModel.Description("Bloqueto.")]
            Bloqueto = 2,
            [System.ComponentModel.Description("Cartão Itaucard.")]
            CartaoItauCard = 3
        }

        /// <summary>
        /// Situação de pagamento do pedido.
        /// </summary>
        public enum enuSituacaoPagamento
        {
            [System.ComponentModel.Description("Pagamento efetuado.")]
            PagamentoEfetuado = 0,
            [System.ComponentModel.Description("Não finalizada (tente novamente).")]
            NaoFinalizada = 1,
            [System.ComponentModel.Description("Erro no processamento da consulta (tente novamente).")]
            ErroProcessamentoConsulta = 2,
            [System.ComponentModel.Description("Pagamento não localizado (consulta fora de prazo ou pedido não registrado no banco).")]
            PagamentoNaoLocalizado = 3,
            [System.ComponentModel.Description("Bloqueto emitido com sucesso.")]
            BloquetoEmitidoSucesso = 4,
            [System.ComponentModel.Description("Pagamento efetuado, aguardando compensação.")]
            AguardandoCompensacao = 5,
            [System.ComponentModel.Description("Pagamento não compensado.")]
            PagamentoNaoCompensado = 6
        }

        /// <summary>
        /// Formato do retorno da consulta.
        /// </summary>
        public enum enuFormatoConsulta
        {
            [System.ComponentModel.Description("Formato de página HTML para consulta visual.")]
            HTML = 0,
            [System.ComponentModel.Description("Formato XML.")]
            XML = 1
        }

        public enum enuTipoCartao
        {
            [System.ComponentModel.Description("Bandeira Mastercard ou Diners.")]
            REDECARD = 'M',
            [System.ComponentModel.Description("Visa.")]
            VISANET = 'V'
        }

        #endregion

        #region Estruturas

        public struct EstruturaRetornoTipoPagamento
        {
            private string codigo_empresa;
            /// <summary>
            /// Código da Empresa ou Código do Site.
            /// </summary>
            public string CodigoEmpresa
            {
                get { return codigo_empresa; }
                set { codigo_empresa = value; }
            }

            private int numero_pedido;
            /// <summary>
            /// Número do Pedido.
            /// </summary>
            public int NumeroPedido
            {
                get { return numero_pedido; }
                set { numero_pedido = value; }
            }

            private enuTipoPagamento tipo_pagamento;
            /// <summary>
            /// Tipo de pagamento escolhido pelo comprador.
            /// </summary>
            public enuTipoPagamento TipoPagamento
            {
                get { return tipo_pagamento; }
                set { tipo_pagamento = value; }
            }
        }

        /// <summary>
        /// Estrutura para consulta das transações.
        /// </summary>
        public struct EstrututraConsulta
        {
            private enuFormatoConsulta formato;
            /// <summary>
            /// Formato do retorno da consulta.
            /// </summary>
            public enuFormatoConsulta Formato
            {
                get { return formato; }
                set { formato = value; }
            }

            private enuTipoPagamento tipo_pagamento;
            /// <summary>
            /// Tipo de pagamento escolhido pelo comprador.
            /// </summary>
            public enuTipoPagamento TipoPagamento
            {
                get { return tipo_pagamento; }
                set { tipo_pagamento = value; }
            }

            private enuSituacaoPagamento situacao_pedido;
            /// <summary>
            /// Situação de pagamento do pedido.
            /// </summary>
            public enuSituacaoPagamento SituacaoPedido
            {
                get { return situacao_pedido; }
                set { situacao_pedido = value; }
            }

            private DateTime data_pagamento;
            /// <summary>
            /// Data do pagamento.
            /// </summary>
            public DateTime DataPagamento
            {
                get { return data_pagamento; }
                set { data_pagamento = value; }
            }

            private string numero_autorizacao;
            /// <summary>
            /// Número de autorização - preenchido somente quando pagamento efetuado com cartão Itaucard.
            /// </summary>
            public string NumeroAutorizacao
            {
                get { return numero_autorizacao; }
                set { numero_autorizacao = value; }
            }

            private string nsu_transacao;
            /// <summary>
            /// NSU da transação – preenchido somente quando pagamento efetuado com cartão Itaucard.
            /// </summary>
            public string NsuTransacao
            {
                get { return nsu_transacao; }
                set { nsu_transacao = value; }
            }

            private string numero_comprovante_venda;
            /// <summary>
            /// Número do comprovante de venda – preenchido somente quando pagamento efetuado com cartão Itaucard, bandeira Mastercard/Diners.
            /// </summary>
            public string NumeroComprovanteVenda
            {
                get { return numero_comprovante_venda; }
                set { numero_comprovante_venda = value; }
            }

            private enuTipoCartao tipo_cartao;
            /// <summary>
            /// Tipo de cartão Itaucard escolhido pelo comprador.
            /// </summary>
            public enuTipoCartao TipoCartao
            {
                get { return tipo_cartao; }
                set { tipo_cartao = value; }
            }

        }

        /// <summary>
        /// Estrutura para postagem de transação.
        /// </summary>
        public struct EstrututraNovoPagamento
        {
            private int numero_pedido;
            /// <summary>
            /// Número do Pedido.
            /// </summary>
            public int NumeroPedido
            {
                get { return numero_pedido; }
                set { numero_pedido = value; }
            }

            private decimal valor;
            /// <summary>
            /// Valor Total do Pagamento.
            /// </summary>
            public decimal Valor
            {
                get { return valor; }
                set { valor = value; }
            }

            private string observacao;
            /// <summary>
            /// Espaço disponível para enviar uma linha de mensagem única por pedido ou um parâmetro indicando qual Mensagem Adicional (1) se quer apresentar.
            /// </summary>
            public string Observacao
            {
                get { return observacao; }
                set { observacao = Util.CortaTexto(value, 40); }
            }

            private string nome_sacado;
            /// <summary>
            /// Nome do sacado.
            /// </summary>
            public string NomeSacado
            {
                get { return nome_sacado; }
                set { nome_sacado = Util.CortaTexto(value, 30); }
            }

            private enuCodigoInscricao codigo_inscricao;
            /// <summary>
            /// Código de inscrição do sacado.
            /// </summary>
            public enuCodigoInscricao CodigoInscricao
            {
                get { return codigo_inscricao; }
                set { codigo_inscricao = value; }
            }

            private string numero_inscricao;
            /// <summary>
            /// Número de inscrição do sacado.
            /// </summary>
            public string NumeroInscricao
            {
                get { return numero_inscricao; }
                set { numero_inscricao = Util.CortaTexto(value, 14); }
            }

            private string endereco_sacado;
            /// <summary>
            /// Endereço do sacado.
            /// </summary>
            public string EnderecoSacado
            {
                get { return endereco_sacado; }
                set { endereco_sacado = Util.CortaTexto(value, 40); }
            }

            private string bairro_sacado;
            /// <summary>
            /// Bairro do sacado.
            /// </summary>
            public string BairroSacado
            {
                get { return bairro_sacado; }
                set { bairro_sacado = Util.CortaTexto(value, 15); }
            }

            private string cep_sacado;
            /// <summary>
            /// CEP do sacado.
            /// </summary>
            public string CepSacado
            {
                get { return cep_sacado; }
                set { cep_sacado = Util.CortaTexto(value, 8); }
            }

            private string cidade_sacado;
            /// <summary>
            /// Cidade do sacado.
            /// </summary>
            public string CidadeSacado
            {
                get { return cidade_sacado; }
                set { cidade_sacado = Util.CortaTexto(value, 15); }
            }

            private string estado_sacado;
            /// <summary>
            /// Estado do sacado.
            /// </summary>
            public string EstadoSacado
            {
                get { return estado_sacado; }
                set { estado_sacado = Util.CortaTexto(value, 2); }
            }

            private DateTime data_vencimento;
            /// <summary>
            /// Data de vencimento do título.
            /// </summary>
            public DateTime DataVencimento
            {
                get { return data_vencimento; }
                set { data_vencimento = value; }
            }

            private string obs_adicional1;
            /// <summary>
            /// Espaço disponível para enviar uma linha de mensagem única por pedido, que só será exibida se o campo Observação contiver o texto “3”.
            /// </summary>
            public string ObsAdicional1
            {
                get { return obs_adicional1; }
                set { obs_adicional1 = Util.CortaTexto(value, 60); }
            }

            private string obs_adicional2;
            /// <summary>
            /// Espaço disponível para enviar uma linha de mensagem única por pedido, que só será exibida se o campo Observação contiver o texto “3”.
            /// </summary>
            public string ObsAdicional2
            {
                get { return obs_adicional2; }
                set { obs_adicional2 = Util.CortaTexto(value, 60); }
            }

            private string obs_adicional3;
            /// <summary>
            /// Espaço disponível para enviar uma linha de mensagem única por pedido, que só será exibida se o campo Observação contiver o texto “3”.
            /// </summary>
            public string ObsAdicional3
            {
                get { return obs_adicional3; }
                set { obs_adicional3 = Util.CortaTexto(value, 60); }
            }
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// A Chave de Criptografia é um código alfanumérico criado pelo site com exatamente 16 posições para dar segurança à transmissão dos dados que irão trafegar entre o servidor do site e o servidor do banco no momento da geração do Itaú Shopline.
        /// </summary>
        public static string ChaveCriptografia
        {
            get
            {
                string ret = string.Empty;
                if (ConfigurationManager.AppSettings["ItauShopLineChave"] != null)
                    ret = ConfigurationManager.AppSettings["ItauShopLineChave"].ToString();

                return ret;
            }
        }

        /// <summary>
        /// O Código do Site é um código criado pelo banco que identifica a conta corrente onde serão creditados os pagamentos efetuados em ambiente seguro Itaú Shopline.
        /// </summary>
        public static string CodigoSite
        {
            get
            {
                string ret = string.Empty;
                if (ConfigurationManager.AppSettings["ItauShopLineCodigo"] != null)
                    ret = ConfigurationManager.AppSettings["ItauShopLineCodigo"].ToString();

                return ret;
            }
        }

        /// <summary>
        /// Parâmetro enviado na geração do Itaú Shopline que indica que o site deseja receber o Retorno Online do Tipo do Pagamento.
        /// </summary>
        public static string UrlRetorno
        {
            get
            {
                string ret = string.Empty;
                if (ConfigurationManager.AppSettings["ItauShopLineUrlRetorno"] != null)
                    ret = ConfigurationManager.AppSettings["ItauShopLineUrlRetorno"].ToString();

                return ret;
            }
        }

        /// <summary>
        /// Parâmetro enviado na geração do Itaú Shopline que indica que o site deseja receber o Retorno Online do Tipo do Pagamento.
        /// </summary>
        public static string UrlConsulta
        {
            get
            {
                string ret = string.Empty;
                if (ConfigurationManager.AppSettings["ItauShopLineUrlConsulta"] != null)
                    ret = ConfigurationManager.AppSettings["ItauShopLineUrlConsulta"].ToString();

                return ret;
            }
        }

        public static string UrlPagamento
        {
            get
            {
                string ret = string.Empty;
                if (ConfigurationManager.AppSettings["ItauShopLineUrlPagamento"] != null)
                    ret = ConfigurationManager.AppSettings["ItauShopLineUrlPagamento"].ToString();

                return ret;
            }
        }

        #endregion

        #region Variáveis

        public static string numChaveCriptografia = ChaveCriptografia;
        public static string numCodigoSite = CodigoSite;
        public static string strUrlRetorna = UrlRetorno;
        public static string strUrlConsulta = UrlConsulta;

        #endregion


        public int InserePedido()
        {

            DAL oDAL = new DAL();
            int retorno = 0;

            string Pedido = this.pedido;
            string Valor = this.valor;
            int clienteID = Convert.ToInt32(this.clienteID);
            string sessionID = this.sessionID;

            string strSql = " INSERT INTO ItauShopline " +
                            " ( " +
                            " ClienteID, " +
                            " NumeroPedido, " +
                            " ValorTotal, " +
                            " DataCompra, " +
                            " DataPagamento, " +
                            " DataValidacao, " +
                            " TimeStamp,  " +
                            " SitPagamento,  " +
                            " SessionID " +
                            " ) " +
                            " VALUES " +
                            " ( " +
                            clienteID + ", " +
                            "" + Pedido + ", " +
                            "" + Valor.Replace(",", ".") + "," +
                            "'" + DateTime.Now.ToString("yyyyMMddhhMMss") + "'," +
                            "'" + DateTime.Now.ToString("yyyyMMddhhMMss") + "'," +
                            "'" + DateTime.Now.ToString("yyyyMMddhhMMss") + "'," +
                            "'" + DateTime.Now.ToString("yyyyMMddhhMMss") + "'," +
                            " 0," +
                            "'" + sessionID + "'" +
                            " )";
            try
            {
                retorno = oDAL.Execute(strSql);
                oDAL.ConnClose();
                return retorno;
            }
            catch
            {
                oDAL.ConnClose();
                return 0;
            }
        }
        public int AtualizaRetorno(string msgErro)
        {
            string Pedido = this.pedido;
            string CodEmp = this.codEmp;
            string TipPag = this.tipPag;
            string FormaPagamento = this.formaPagamento;

            DAL oDAL = new DAL();
            StringBuilder strSql = new StringBuilder();
            int retorno = 0;
            if (retorno == 0)
            {
                strSql.AppendLine(" UPDATE ItauShopline ");
                strSql.AppendLine("   SET DataValidacao = '" + DateTime.Now.ToString("yyyyMMddhhMMss") + "'");
                if (msgErro != "")
                {
                    strSql.AppendLine("      ,StatusCompra = '" + (int)SitPag + "'");
                    strSql.AppendLine("      ,CodEmp = '" + CodEmp + "'");
                    strSql.AppendLine("      ,TipoPagamento = '" + tipPag + "'");
                    strSql.AppendLine("      ,SitPagamento = '" + (int)this.SitPag + "'");
                    strSql.AppendLine("      ,FormaPagto = '" + FormaPagamento + "'");
                }
                else
                {
                    strSql.AppendLine("      ,StatusCompra = 'ER'");
                    strSql.AppendLine("      ,SitPagamento = 'ER'");
                }
                strSql.AppendLine("      ,TimeStamp = '" + DateTime.Now.ToString("yyyyMMddhhMMss") + "'");
                strSql.AppendLine(" WHERE (NumeroPedido = " + Convert.ToInt32(Pedido) + ")");

                try
                {
                    retorno = oDAL.Execute(strSql.ToString());
                    oDAL.ConnClose();
                    return retorno;
                }
                catch
                {
                    oDAL.ConnClose();
                    return 0;
                }
                finally
                {
                    oDAL.ConnClose();
                }
            }
            else
                return 0;
        }

        public bool Consulta(int pedido)
        {


            DAL oDAL = new DAL();
            try
            {
                SqlParameter[] Parametros = new SqlParameter[1];

                Parametros[0] = new SqlParameter("@NumeroPedido", SqlDbType.Int);
                Parametros[0].Value = pedido;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT  Count(id) FROM ItauShopline (NOLOCK) ");
                strSql.Append("WHERE NumeroPedido =@NumeroPedido ");

                int reader = Convert.ToInt32(oDAL.Scalar(strSql.ToString(), Parametros));

                if (reader > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public static bool ValidarNumeroPedido(string nrPedido)
        {
            DAL oDAL = new DAL();
            IDataReader reader = null;
            try
            {
                reader = oDAL.SelectToIDataReader("SELECT NumeroPedido FROM ItauShopline (NOLOCK) WHERE NumeroPedido='" + nrPedido + "'");
                if (reader.Read())
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public static bool RevalidarNumeroPedido(string NumeroPedido, string ClienteID)
        {
            DAL oDAL = new DAL();
            IDataReader reader = null;
            try
            {
                reader = oDAL.SelectToIDataReader("SELECT NumeroPedido, ClienteID FROM ItauShopline (NOLOCK) WHERE NumeroPedido='" + NumeroPedido + "' AND ClienteID='" + ClienteID + "'");
                if (reader.Read())
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possivel revalidar o Pedido");
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

    }
}
