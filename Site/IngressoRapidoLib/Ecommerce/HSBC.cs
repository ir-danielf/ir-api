using CTLib;
using IngressoRapido.Lib;
using IRLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IngressoRapido.Ecommerce
{

    public class HSBC
    {
        #region " Atributos - Properties "

        public string Token { get; set; }

        public string TokenTemporario { get; set; }

        public int ClienteID { get; set; }

        public long NrPedido { get; set; }

        public decimal ValorTotal { get; set; }

        public int QtdItems { get; set; }

        public string FormaPagamento { get; set; }

        public string FormaPagamentoID { get; set; }

        public string StatusCompra { get; set; }

        public string Descricao { get; set; }

        public string SessionID { get; set; }

        public int TaxaEntregaID { get; set; }

        public decimal TaxaEntregaValor { get; set; }

        public bool LugaresSeparados { get; set; }

        public string HSBCNome { get; set; }

        public string HSBCCep { get; set; }

        public string HSBCCpf { get; set; }

        public string HSBCTelefone { get; set; }

        public string HSBCIndPagamento { get; set; }

        public string HSBCMensagem1 { get; set; }

        public string HSBCMensagem2 { get; set; }

        public string Acao { get; set; }

        public string HSBCEmail { get; set; }

        private List<int> _ingressoID;
        public List<int> IngressoID
        {
            get { return _ingressoID; }
            set { _ingressoID = value; }
        }

        //private List<string> _evento;
        //public List<string> Evento
        //{
        //    get { return _evento; }
        //    set { _evento = value; }
        //}

        //private List<string> _local;
        //public List<string> Local
        //{
        //    get { return _local; }
        //    set { _local = value; }
        //}

        //private List<string> _setor;
        //public List<string> Setor
        //{
        //    get { return _setor; }
        //    set { _setor = value; }
        //}

        //private int _cupomID;
        //public int CupomID
        //{
        //    get { return _cupomID; }
        //    set { _cupomID = value; }
        //}

        //private List<int> _listaApresentacoes;
        //public List<int> ListaApresentacoes
        //{
        //    get { return _listaApresentacoes; }
        //    set { _listaApresentacoes = value; }
        //}

        //private IRLib.ClientObjects.EstruturaValeIngresso valeIngresso;
        //public IRLib.ClientObjects.EstruturaValeIngresso ValeIngresso
        //{
        //    get { return valeIngresso; }
        //    set { valeIngresso = value; }
        //}

        #endregion


        /// <summary>
        /// Criado por Caio Maganha Rosa
        /// Data: 16/09/09
        /// Utilização: Inclui nova compra feita atraves de parceria com o HSBC
        /// </summary>
        public void InserirNovaCompra()
        {
            DAL oDAL = new DAL();
            try
            {
                SqlParameter[] Parametros = new SqlParameter[23];

                Parametros[0] = new SqlParameter("@ClienteID", SqlDbType.Int);
                Parametros[0].Value = this.ClienteID;

                Parametros[1] = new SqlParameter("@Nr_Pedido", SqlDbType.BigInt);
                Parametros[1].Value = this.NrPedido;

                Parametros[2] = new SqlParameter("@ValorTotal", SqlDbType.Int);
                Parametros[2].Value = this.ValorTotal;

                Parametros[3] = new SqlParameter("@FormaPagamentoID", SqlDbType.Int);
                Parametros[3].Value = this.FormaPagamentoID;

                Parametros[4] = new SqlParameter("@StatusCompra", SqlDbType.NVarChar);
                Parametros[4].Value = this.StatusCompra;

                Parametros[5] = new SqlParameter("@QtdItems", SqlDbType.Int);
                Parametros[5].Value = this.QtdItems;

                Parametros[6] = new SqlParameter("@TokenTemporario", SqlDbType.NVarChar);
                Parametros[6].Value = this.TokenTemporario;

                Parametros[7] = new SqlParameter("@Token", SqlDbType.NVarChar, 50);
                Parametros[7].Value = this.Token;

                Parametros[8] = new SqlParameter("@SessionID", SqlDbType.NVarChar);
                Parametros[8].Value = this.SessionID;

                Parametros[9] = new SqlParameter("@TaxaEntregaID", SqlDbType.Int);
                Parametros[9].Value = this.TaxaEntregaID;

                Parametros[10] = new SqlParameter("@DataCompra", SqlDbType.DateTime);
                Parametros[10].Value = DateTime.Now;

                Parametros[11] = new SqlParameter("@LugaresSeparados", SqlDbType.Bit);
                Parametros[11].Value = 0;

                Parametros[12] = new SqlParameter("@HSBCNome", SqlDbType.NVarChar);
                Parametros[12].Value = this.HSBCNome;

                Parametros[13] = new SqlParameter("@HSBCCpf", SqlDbType.NVarChar);
                Parametros[13].Value = this.HSBCCpf;

                Parametros[14] = new SqlParameter("@HSBCCep", SqlDbType.NVarChar);
                Parametros[14].Value = this.HSBCCep;

                Parametros[15] = new SqlParameter("@HSBCIndPagamento", SqlDbType.NVarChar);
                Parametros[15].Value = this.HSBCIndPagamento;

                Parametros[16] = new SqlParameter("@HSBCTelefone", SqlDbType.NVarChar);
                Parametros[16].Value = this.HSBCTelefone;

                Parametros[17] = new SqlParameter("@HSBCMensagem1", SqlDbType.NVarChar);
                Parametros[17].Value = this.HSBCMensagem1;

                Parametros[18] = new SqlParameter("@HSBCMensagem2", SqlDbType.NVarChar);
                Parametros[18].Value = this.HSBCMensagem2;

                Parametros[19] = new SqlParameter("@HSBCEmail", SqlDbType.NVarChar);
                Parametros[19].Value = this.HSBCEmail;

                Parametros[20] = new SqlParameter("@CupomID", SqlDbType.Int);
                Parametros[20].Value = 0;

                Parametros[21] = new SqlParameter("@ApresentacoesID", SqlDbType.NVarChar);
                Parametros[21].Value = "0";

                Parametros[22] = new SqlParameter("@TaxaEntregaValor", SqlDbType.Decimal);
                Parametros[22].Value = this.TaxaEntregaValor;


                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("INSERT INTO HSBC ");
                stbSQL.Append("     (       ClienteID, ");
                stbSQL.Append("             NumeroPedido, ");
                stbSQL.Append("             Token, ");
                stbSQL.Append("             TokenTemporario, ");
                stbSQL.Append("             ValorTotal, ");
                stbSQL.Append("             QuantidadeItems, ");
                stbSQL.Append("             FormaPagamentoID, ");
                stbSQL.Append("             SessionID, ");
                stbSQL.Append("             TaxaEntregaID, ");
                stbSQL.Append("             DataCompra, ");
                stbSQL.Append("             StatusCompra, ");
                stbSQL.Append("             LugaresSeparados, ");
                stbSQL.Append("             HSBCNome, ");
                stbSQL.Append("             HSBCCpf, ");
                stbSQL.Append("             HSBCCep, ");
                stbSQL.Append("             HSBCTelefone, ");
                stbSQL.Append("             HSBCIndPagamento, ");
                stbSQL.Append("             HSBCMensagem1, ");
                stbSQL.Append("             HSBCMensagem2, ");
                stbSQL.Append("             HSBCEmail, ");
                stbSQL.Append("             CupomID, ");
                stbSQL.Append("             listaApresentacoesID, ");
                stbSQL.Append("             TaxaEntregaValor ) ");
                stbSQL.Append("VALUES ");
                stbSQL.Append("     (       @ClienteID, ");
                stbSQL.Append("             @Nr_Pedido, ");
                stbSQL.Append("             @Token, ");
                stbSQL.Append("             @TokenTemporario, ");
                stbSQL.Append("             @ValorTotal, ");
                stbSQL.Append("             @QtdItems, ");
                stbSQL.Append("             @FormaPagamentoID, ");
                stbSQL.Append("             @SessionID, ");
                stbSQL.Append("             @TaxaEntregaID, ");
                stbSQL.Append("             @DataCompra, ");
                stbSQL.Append("             @StatusCompra, ");
                stbSQL.Append("             @LugaresSeparados, ");
                stbSQL.Append("             @HSBCNome, ");
                stbSQL.Append("             @HSBCCpf, ");
                stbSQL.Append("             @HSBCCep, ");
                stbSQL.Append("             @HSBCTelefone, ");
                stbSQL.Append("             @HSBCIndPagamento, ");
                stbSQL.Append("             @HSBCMensagem1, ");
                stbSQL.Append("             @HSBCMensagem2, ");
                stbSQL.Append("             @HSBCEmail, ");
                stbSQL.Append("             @CupomID, ");
                stbSQL.Append("             @ApresentacoesID, ");
                stbSQL.Append("             @TaxaEntregaValor )");



                DAL oDal = new DAL();
                oDal.Execute(stbSQL.ToString(), Parametros);

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

        //private void InserirIngressos()
        //{
        //    DAL oDAL = new DAL();
        //    try
        //    {
        //        SqlParameter[] Parametros = new SqlParameter[3];
        //        Parametros[0] = new SqlParameter("@ClienteID", SqlDbType.Int);
        //        Parametros[0].Value = this.ClienteID;

        //        Parametros[1] = new SqlParameter("@Nr_Pedido", SqlDbType.BigInt);
        //        Parametros[1].Value = this.NrPedido;
        //        StringBuilder stbSQL = new StringBuilder();

        //        foreach (int IngressoID in this.IngressoID)
        //        {

        //            Parametros[2] = new SqlParameter("@IngressoID", SqlDbType.BigInt);
        //            Parametros[2].Value = IngressoID;

        //            stbSQL.Append("INSERT INTO HSBCIngresso ");
        //            stbSQL.Append("     (       ClienteID, ");
        //            stbSQL.Append("             NumeroPedido, ");
        //            stbSQL.Append("             IngressoID ) ");
        //            stbSQL.Append("     (       @ClienteID, ");
        //            stbSQL.Append("             @Nr_Pedido, ");
        //            stbSQL.Append("             @IngressoID ) ");
        //            oDAL.Execute(stbSQL.ToString(), Parametros);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao inserir os Ingressos");
        //    }

        //    finally
        //    {
        //        oDAL.ConnClose();
        //    }
        //}

        //public bool ValidarCompraDuplicada(CarrinhoLista oCarrinhoLista)
        //{
        //    DAL oDAL = new DAL();
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Consulta Retornou um erro" + ex.Message);
        //    }
        //}

        public void InserirCarrinho(CarrinhoLista oCarrinhoLista)
        {
            DAL oDAL = new DAL();

            try
            {
                foreach (Carrinho oCarrinho in oCarrinhoLista)
                {
                    SqlParameter[] Parametros = new SqlParameter[11];
                    Parametros[0] = new SqlParameter("@ClienteID", SqlDbType.Int);
                    Parametros[0].Value = this.ClienteID;

                    Parametros[1] = new SqlParameter("@Nr_Pedido", SqlDbType.BigInt);
                    Parametros[1].Value = this.NrPedido;
                    StringBuilder stbSQL = new StringBuilder();

                    Parametros[2] = new SqlParameter("@IngressoID", SqlDbType.Int);
                    Parametros[2].Value = oCarrinho.IngressoID;

                    Parametros[3] = new SqlParameter("@ApresentacaoID", SqlDbType.Int);
                    Parametros[3].Value = oCarrinho.ApresentacaoID;

                    Parametros[4] = new SqlParameter("@EventoID", SqlDbType.Int);
                    Parametros[4].Value = oCarrinho.EventoID;

                    Parametros[5] = new SqlParameter("@LocalID", SqlDbType.Int);
                    Parametros[5].Value = oCarrinho.LocalID;

                    Parametros[6] = new SqlParameter("@PrecoExclusivoCodigoID", SqlDbType.Int);
                    Parametros[6].Value = oCarrinho.PrecoExclusivoCodigoID;

                    Parametros[7] = new SqlParameter("@DataInclusao", SqlDbType.DateTime);
                    Parametros[7].Value = DateTime.Now;

                    Parametros[8] = new SqlParameter("@Evento", SqlDbType.NVarChar);
                    Parametros[8].Value = oCarrinho.Evento;

                    Parametros[9] = new SqlParameter("@Local", SqlDbType.NVarChar);
                    Parametros[9].Value = oCarrinho.Local;

                    Parametros[10] = new SqlParameter("@Setor", SqlDbType.NVarChar);
                    Parametros[10].Value = oCarrinho.Setor;

                    stbSQL.Append("INSERT INTO HSBCIngressos ");
                    stbSQL.Append("     (       IngressoID, ");
                    stbSQL.Append("             ApresentacaoID, ");
                    stbSQL.Append("             EventoID, ");
                    stbSQL.Append("             LocalID, ");
                    stbSQL.Append("             NumeroPedido, ");
                    stbSQL.Append("             ClienteID, ");
                    stbSQL.Append("             PrecoExclusivoCodigoID, ");
                    stbSQL.Append("             DataInclusao, ");
                    stbSQL.Append("             Evento, ");
                    stbSQL.Append("             Local, ");
                    stbSQL.Append("             Setor )");
                    stbSQL.Append("VALUES ");
                    stbSQL.Append("     (       @IngressoID, ");
                    stbSQL.Append("             @ApresentacaoID, ");
                    stbSQL.Append("             @EventoID, ");
                    stbSQL.Append("             @LocalID, ");
                    stbSQL.Append("             @Nr_Pedido, ");
                    stbSQL.Append("             @ClienteID, ");
                    stbSQL.Append("             @PrecoExclusivoCodigoID, ");
                    stbSQL.Append("             @DataInclusao, ");
                    stbSQL.Append("             @Evento, ");
                    stbSQL.Append("             @Local, ");
                    stbSQL.Append("             @Setor ) ");

                    oDAL.Execute(stbSQL.ToString(), Parametros);
                    oDAL.ConnClose();
                    Parametros = null;
                }
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
        /// <summary>
        /// Atualiza o status da compra e retorna informações do cliente para o WS de integração
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="nrPedido"></param>
        /// <returns></returns>
        public bool AtualizarCompra(string Status, long nrPedido)
        {
            bool Atualizado = false;
            string data = DateTime.Now.ToString("yyyyMMddHHmmss");
            data = data.Replace("/", "").Replace(":", "").Replace(" ", "");

            DAL oDAL = new DAL();
            try
            {
                SqlParameter[] Parametros = new SqlParameter[3];

                Parametros[0] = new SqlParameter("@Status", SqlDbType.NVarChar);
                Parametros[0].Value = Status;

                Parametros[1] = new SqlParameter("@DataValidacao", SqlDbType.NVarChar);
                Parametros[1].Value = data;

                Parametros[2] = new SqlParameter("@NumeroPedido", SqlDbType.BigInt);
                Parametros[2].Value = nrPedido;


                StringBuilder stbSql = new StringBuilder();
                stbSql.Append("UPDATE HSBC SET StatusCompra=@Status, ");
                stbSql.Append("DataValidacao=@DataValidacao ");
                stbSql.Append("WHERE NumeroPedido =@NumeroPedido");
                oDAL.Execute(stbSql.ToString(), Parametros);
                Atualizado = true;
                return Atualizado;
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

        /// <summary>
        /// Consulta o banco e verifica os dados de Input passados pelo HSBC
        /// </summary>
        /// <returns></returns>
        public bool ValidarCompra()
        {
            DAL oDAL = new DAL();
            try
            {
                SqlParameter[] Parametros = new SqlParameter[2];

                Parametros[0] = new SqlParameter("@NumeroPedido", SqlDbType.BigInt);
                Parametros[0].Value = this.NrPedido;

                Parametros[1] = new SqlParameter("@Token", SqlDbType.NVarChar);
                Parametros[1].Value = this.Token;


                StringBuilder stbConsulta = new StringBuilder();
                stbConsulta.Append("SELECT Count(ID) FROM HSBC (NOLOCK) ");
                stbConsulta.Append("WHERE NumeroPedido =@NumeroPedido ");
                stbConsulta.Append("AND (TokenTemporario=@Token ");
                stbConsulta.Append("OR Token=@Token )");

                int qtd = (int)oDAL.Scalar(stbConsulta.ToString(), Parametros);

                if (qtd != 0)
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

        //public DataSet ConsultarCompraDataSet(string NumeroPedido)
        //{
        //    DAL oDAL = new DAL();
        //    try
        //    {
        //        DataSet dsConsulta = new DataSet();
        //        StringBuilder stbConsultaCompra = new StringBuilder();
        //        StringBuilder stbConsultaIngressos = new StringBuilder();

        //        SqlParameter Parametro = new SqlParameter("@NumeroPedido", SqlDbType.BigInt);
        //        Parametro.Value = NumeroPedido;

        //        stbConsultaCompra.Append("SELECT HSBCNome, HSBCEmail, DataCompra, DataValidacao, StatusCompra, ValorTotal, ClienteID, SessionID, NumeroPedido ");
        //        stbConsultaCompra.Append("FROM HSBC ");
        //        stbConsultaCompra.Append("WHERE NumeroPedido=@NumeroPedido");
        //        dsConsulta.Tables.Add(oDAL.SelectToDataTable(stbConsultaCompra.ToString(),Parametro));

        //        stbConsultaIngressos.Append("SELECT IngressoID FROM HSBCIngressos ");
        //        stbConsultaIngressos.Append("WHERE NumeroPedido=@NumeroPedido");
        //        dsConsulta.Tables.Add(oDAL.SelectToDataTable(stbConsultaIngressos.ToString(), Parametro));




        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable ConsultarCompraDataTable()
        {
            DAL oDAL = new DAL();
            try
            {
                DataTable dttRetorno = new DataTable();
                StringBuilder stbConsulta = new StringBuilder();
                stbConsulta.Append("Select IsNull(HSBCNome,''), IsNull(HSBCEmail,''), IsNull(ValorTotal,''), ");
                stbConsulta.Append("ClienteID, SessionID, TaxaEntregaID, FormaPagamentoID, isNull(CupomID, 0), isNull(listaApresentacoesID, 0) FROM HSBC (NOLOCK) ");
                stbConsulta.Append("WHERE NumeroPedido='" + NrPedido + "'");
                dttRetorno = oDAL.SelectToDataTable(stbConsulta.ToString());
                return dttRetorno;
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

        public DataTable ConsultarIngressos()
        {
            DAL oDAL = new DAL();
            DataTable dttIngressos = new DataTable();
            try
            {
                StringBuilder stbConsulta = new StringBuilder();
                stbConsulta.Append("SELECT IngressoID FROM HSBCIngressos (NOLOCK) ");
                stbConsulta.Append("WHERE NumeroPedido =" + this.NrPedido);

                dttIngressos = oDAL.SelectToDataTable(stbConsulta.ToString());
                return dttIngressos;
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
        /// <summary>
        /// Metodo utilizado para verificar se o numero gerado na ProcessoHSBC ja esta sendo utilizado ou nao
        /// </summary>
        /// <param name="nrPedido"></param>
        /// <returns></returns>
        public static bool ValidarNumeroPedido(long nrPedido)
        {
            DAL oDAL = new DAL();
    
            try
            {
                int qtd = (int)oDAL.Scalar("SELECT Count(ID) FROM HSBC (NOLOCK) WHERE NumeroPedido='" + nrPedido + "'");
                if (qtd != 0)
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

        /// <summary>
        /// Metodo utilizado para verificar se o numero gerado na ProcessoHSBC ja esta sendo utilizado ou nao
        /// </summary>
        /// <param name="nrPedido"></param>
        /// <returns></returns>
        public static bool ValidarNumeroPedido(string nrPedido)
        {
            DAL oDAL = new DAL();

            try
            {
                int qtd = (int)oDAL.Scalar("SELECT Count(ID) FROM HSBC (NOLOCK) WHERE NumeroPedido='" + nrPedido + "'");
                if (qtd != 0)
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

        /// <summary>
        /// Revalida o numero para evitar duplicação de pedido
        /// </summary>
        /// <param name="NumeroPedido"></param>
        /// <param name="ClienteID"></param>
        /// <returns></returns>
        public static bool RevalidarNumeroPedido(long NumeroPedido, int ClienteID)
        {
            DAL oDAL = new DAL();
            try
            {
                int qtd = (int)oDAL.Scalar("SELECT Count(ID) FROM HSBC (NOLOCK) WHERE NumeroPedido='" + NumeroPedido + "' AND ClienteID='" + ClienteID + "'");
                if (qtd != 0)
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
        /// <summary>
        /// Metodo que instancia o WS da HSBC, inserindo uma nova compra
        /// </summary>
        /// <returns></returns>
        //public string IntegrarHSBC()
        //{
        //    try
        //    {
        //        ServicePointManager.CertificatePolicy = new IRLib.TrustAllCertificatePolicy();
        //        //this.HSBCMensagem2 = "Ok.";
        //        ShoppingService WebServiceHSBC = new ShoppingService();
        //        WebServiceHSBC.addOrder(this.Token, this.NrPedido, this.ValorTotal, this.Descricao, this.QtdItems, this.FormaPagamento, out Mensagem2HSBC);
        //        return HSBCMensagem2.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// Converte Decimal para inclusão no banco, HSBC utiliza somente xx.xx, base de dados xx,xx
        /// </summary>
        /// <param name="Valor"></param>
        /// <returns></returns>
        public string ConverterValor(string Valor)
        {
            try
            {
                if (Valor.Contains("."))
                    Valor = Valor.Replace(".", ",");
                return Valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string[] VenderHSBC()
        {
            try
            {
                int[] holder = new int[2];
                string[] retornoVender = new string[2];
                Bilheteria bilheteria = new Bilheteria();
                //retornoVender = bilheteria.VenderWeb(this.IngressoID.ToArray(), Convert.ToInt32(this.ClienteID), this.SessionID, Convert.ToInt32(this.TaxaEntregaID), this.TaxaEntregaValor, Convert.ToInt32(this.FormaPagamentoID), 0, holder, );
                return retornoVender;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelarReservas()
        {
            try
            {
                Carrinho oCarrinho = new Carrinho();
                oCarrinho.ExpirarTodasReservas(Convert.ToInt32(this.ClienteID), this.SessionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
