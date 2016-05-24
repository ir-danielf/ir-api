using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    /// <summary>
    /// Gerenciador do CancelamentoGerenciador
    /// </summary>
    [ObjectType(ObjectType.RemotingType.CAO)]
    public class CancelamentoGerenciador : MarshalByRefObject, ISponsoredObject
    {

        //colunas da GRID
        public const string ID = "ID";
        public const string TABELA_GRID = "Grid";
        public const string TABELA_RESERVA = "Reserva";
        public const string TABELA_INFO_PACOTE = "TabelaInfoPacote";
        public const string TABELA_INFO_ASSINATURA = "TabelaInfoAssinatura";

        public const string PRECOID = "PrecoID";
        public const string CORTESIAID = "CortesiaID";
        public const string BLOQUEIOID = "BloqueioID";

        public const string INGRESSOID = "IngressoID";
        public const string RESERVAID = "ReservaID";
        public const string EVENTO_PACOTE = "Evento/Pacote/Vale Ingresso";
        public const string EVENTO = "Evento";
        public const string PACOTE = "Pacote";
        public const string PACOTEID = "PacoteID";
        public const string HORARIO = "Horário";
        public const string SETOR_PRODUTO = "Setor/Produto";
        public const string CODIGO = "Cod";
        public const string PRECO = "Preço";
        public const string CORTESIA = "Cortesia";
        public const string BLOQUEIO = "Bloqueio";
        public const string VALOR = "Valor";
        public const string CONV = "Conv";
        public const string VALOR_CONV = "Valor Conv";
        public const string STATUS = "Status";
        public const string STATUS_DETALHADO = "StatusDetalhado";
        public const string TIPO = "Tipo";
        public const string TIPO_LUGAR = "TipoLugar";
        public const string TAXA_COMISSAO = "TaxaComissao";
        public const string COMISSAO_VALOR = "ComissaoValor";
        public const string PACOTE_GRUPO = "PacoteGrupo";
        public const string PACOTE_COMPLETO = "PacoteCompleto";
        public const string OBSERVACAO = "Obs";
        public const string NIVELRISCO = "NivelRisco";
        public const string CANCELADO = "Cancelado";
        public const string ENTREGA_AGENDA_ID = "EntregaAgendaID";
        public const string PAGAMENTO_PROCESSADO = "PagamentoProcessado";
        public const char TIPO_PACOTE = 'P'; //tipos de reserva
        public const char TIPO_INGRESSO = 'I';
        public const string TIPO_CODIGO_BARRA = "TipoCodigoBarra";
        public const string CODIGO_BARRA = "CodigoBarra";
        public const string EVENTO_ID = "EventoID";
        public const string APRESENTACAO_SETOR_ID = "ApresentacaoSetorID";
        public const string ASSINATURA_CLIENTE_ID = "AssinaturaClienteID";
        public const string TAXAPROCESSAMENTOVALOR = "TaxaProcessamentoValor";
        public const string TAXAPROCESSAMENTOCANCELADA = "TaxaProcessamentoCancelada";
        public const string VALORSEGURO = "ValorSeguro";
        //public const string ACAO = "Ação";
        public const string CLIENTE = "Cliente";
        public const string CLIENTEID = "ClienteID";

        public const string EMAIL = "Email";
        public const string CANAL = "Canal";
        public const string INFO_VENDA = "InfoVenda";
        public const string VENDEDOR = "Vendedor";
        public const string DATA_VENDA = "DataVenda";
        public const string DATA_CAIXA = "DataCaixa";
        public const string SENHA = "Senha";
        public const string STATUS_VENDA = "StatusVenda";
        public const string INGRESSO_ID = "IngressoID";
        public const string VENDA_BILHETERIA_ID = "VendaBilheteriaID";
        public const string VENDA_BILHETERIA_ITEM_ID = "VendaBilheteriaItemID";
        public const string INGRESSOLOG_ID = "IngressoLogID";
        public const string VALOR_TOTAL = "ValorTotal";
        public const string DISPONIVEL_AJUSTE = "DisponivelAjuste";
        public const string TAXA_ENTREGA_VALOR = "TaxaEntregaValor";
        public const string ENTREGA_CONTROLE_ID = "EntregaControleID";
        public const string TAXA_CONV_VALOR_TOTAL = "TaxaConvenienciaValorTotal";
        public const string TAXA_ENTREGA_TIPO = "TaxaEntregaTipo";
        public const string TRANSACTION_ID = "TransactionID";

        private int canalID;
        private int caixaID;
        private int perfilID;
        private int usuarioID;
        private int localID;
        private int empresaID;

        public CancelamentoGerenciador()
        {
            canalID = 0;
            usuarioID = 0;
            caixaID = 0;
            perfilID = 0;
            localID = 0;
            empresaID = 0;
        }

        public int CaixaID
        {
            set { caixaID = value; }
        }

        public int UsuarioID
        {
            set { usuarioID = value; }
        }

        public int CanalID
        {
            set { canalID = value; }
        }

        public int PerfilID
        {
            set { perfilID = value; }
        }

        public int LocalID
        {
            set { localID = value; }
        }

        public int EmpresaID
        {
            set { empresaID = value; }
        }

        //tabela de retorno do banco de dados
        private DataTable EstruturaTabela()
        { //auxiliar

            DataTable tabela = new DataTable("Tabela");

            tabela.Columns.Add(INGRESSO_ID, typeof(int));
            tabela.Columns.Add(INGRESSOLOG_ID, typeof(int));
            tabela.Columns.Add(VENDA_BILHETERIA_ID, typeof(int));
            tabela.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
            tabela.Columns.Add(EVENTO, typeof(string));
            tabela.Columns.Add(PACOTE, typeof(string));
            tabela.Columns.Add(PACOTEID, typeof(int));
            tabela.Columns.Add(HORARIO, typeof(string));
            tabela.Columns.Add(SETOR_PRODUTO, typeof(string));
            tabela.Columns.Add(CODIGO, typeof(string));
            tabela.Columns.Add(PRECO, typeof(string));
            tabela.Columns.Add(CORTESIA, typeof(string));
            tabela.Columns.Add(BLOQUEIO, typeof(string));
            tabela.Columns.Add(VALOR, typeof(decimal));
            tabela.Columns.Add(STATUS, typeof(string));
            tabela.Columns.Add(STATUS_DETALHADO, typeof(string));
            tabela.Columns.Add(CONV, typeof(int));
            tabela.Columns.Add(VALOR_CONV, typeof(decimal));
            tabela.Columns.Add(PRECOID, typeof(int));
            tabela.Columns.Add(CORTESIAID, typeof(int));
            tabela.Columns.Add(BLOQUEIOID, typeof(int));
            tabela.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tabela.Columns.Add(TAXA_COMISSAO, typeof(int));
            tabela.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tabela.Columns.Add(OBSERVACAO, typeof(string));
            tabela.Columns.Add(PACOTE_GRUPO, typeof(int));
            tabela.Columns.Add(TIPO_CODIGO_BARRA, typeof(string));
            tabela.Columns.Add(CODIGO_BARRA, typeof(string));
            tabela.Columns.Add(EVENTO_ID, typeof(int));
            tabela.Columns.Add(APRESENTACAO_SETOR_ID, typeof(int));
            tabela.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));
            return tabela;
        }

        #region estruturas

        public static DataSet EstruturaGrid()
        {

            DataSet ds = new DataSet("Reservas");

            DataTable tGrid = new DataTable(TABELA_GRID);
            DataTable tInfoPacote = new DataTable(TABELA_INFO_PACOTE);
            DataTable tReserva = new DataTable(TABELA_RESERVA);
            DataTable tInfoAssinatura = new DataTable(TABELA_INFO_ASSINATURA);

            DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
            colGridReservaID.Unique = true;
            colGridReservaID.AutoIncrement = true;
            colGridReservaID.AutoIncrementStep = 1;
            colGridReservaID.AutoIncrementSeed = 1;

            tGrid.Columns.Add(colGridReservaID);
            //tGrid.Columns.Add(INGRESSO_ID, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(INGRESSOLOG_ID, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(VENDA_BILHETERIA_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TIPO, typeof(char)).DefaultValue = "-"; //guarda o tipo de reserva (ingresso ou pacote)
            tGrid.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-";//guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
            tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tGrid.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(PACOTE_GRUPO, typeof(int));
            tGrid.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));


            tInfoPacote.Columns.Add(RESERVAID, typeof(int));
            tInfoPacote.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoPacote.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoPacote.Columns.Add(TIPO, typeof(char)).DefaultValue = "-"; //guarda o tipo de reserva (ingresso ou pacote)
            tInfoPacote.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-"; //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tInfoPacote.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoPacote.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoPacote.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            tInfoAssinatura.Columns.Add(RESERVAID, typeof(int));
            tInfoAssinatura.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(VALOR, typeof(decimal));
            tInfoAssinatura.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoAssinatura.Columns.Add(TIPO, typeof(char)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoAssinatura.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoAssinatura.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoAssinatura.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;
            tInfoAssinatura.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            //TABELA_RESERVA
            tReserva.Columns.Add(RESERVAID, typeof(int));
            tReserva.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
            tReserva.Columns.Add(INGRESSOID, typeof(int));
            tReserva.Columns.Add(PACOTEID, typeof(int)); //Se for Pacote eh o ID do Pacote.
            tReserva.Columns.Add(PRECOID, typeof(int));
            tReserva.Columns.Add(CORTESIAID, typeof(int));
            tReserva.Columns.Add(BLOQUEIOID, typeof(int));
            tReserva.Columns.Add(TIPO_CODIGO_BARRA, typeof(string));
            tReserva.Columns.Add(CODIGO_BARRA, typeof(string));
            tReserva.Columns.Add(EVENTO_ID, typeof(int));
            tReserva.Columns.Add(APRESENTACAO_SETOR_ID, typeof(int));
            tReserva.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            ds.Tables.Add(tGrid);
            ds.Tables.Add(tReserva);
            ds.Tables.Add(tInfoPacote);
            ds.Tables.Add(tInfoAssinatura);
            //Grid com a Reserva
            DataColumn colReservaID = tReserva.Columns[RESERVAID];
            DataRelation dr1 = new DataRelation("GridXReserva", colGridReservaID, colReservaID, true);

            ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colGridReservaID, colReservaID);
            idKeyRestraint1.DeleteRule = Rule.Cascade;
            tReserva.Constraints.Add(idKeyRestraint1);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr1);

            //Grid com a InfoPacote
            DataColumn colInfoPacoteID = tInfoPacote.Columns[RESERVAID];
            DataRelation dr2 = new DataRelation("GridXInfoPacote", colGridReservaID, colInfoPacoteID, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colGridReservaID, colInfoPacoteID);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tInfoPacote.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr2);

            //Grid com a InfoAssinatura
            DataColumn colInfoAssinaturaID = tInfoAssinatura.Columns[RESERVAID];
            DataRelation dr3 = new DataRelation("GridXInfoAssinatura", colGridReservaID, colInfoAssinaturaID, true);

            ForeignKeyConstraint idKeyRestraint3 = new ForeignKeyConstraint(colGridReservaID, colInfoAssinaturaID);
            idKeyRestraint3.DeleteRule = Rule.Cascade;
            tInfoAssinatura.Constraints.Add(idKeyRestraint3);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr3);

            return ds;
        }

        public static DataSet EstruturaGridVisualizar()
        {

            DataSet ds = new DataSet("Visualizacao");

            DataTable tGrid = new DataTable(TABELA_GRID);
            DataTable tInfoPacote = new DataTable(TABELA_INFO_PACOTE);

            DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
            colGridReservaID.Unique = true;
            colGridReservaID.AutoIncrement = true;
            colGridReservaID.AutoIncrementStep = 1;
            colGridReservaID.AutoIncrementSeed = 1;

            tGrid.Columns.Add(colGridReservaID);
            tGrid.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(EVENTO_PACOTE, typeof(string));
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tGrid.Columns.Add(HORARIO, typeof(string));
            tGrid.Columns.Add(SETOR_PRODUTO, typeof(string));
            tGrid.Columns.Add(CODIGO, typeof(string));
            tGrid.Columns.Add(PRECO, typeof(string));
            tGrid.Columns.Add(CORTESIA, typeof(string));
            tGrid.Columns.Add(BLOQUEIO, typeof(string));
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(STATUS, typeof(string));
            tGrid.Columns.Add(STATUS_DETALHADO, typeof(string));
            tGrid.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
            tGrid.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = ""; //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
            tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tGrid.Columns.Add(OBSERVACAO, typeof(string));
            tGrid.Columns.Add(PACOTE_GRUPO, typeof(int));
            tGrid.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;

            tInfoPacote.Columns.Add(RESERVAID, typeof(int));
            tInfoPacote.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(EVENTO_PACOTE, typeof(string));
            tInfoPacote.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tInfoPacote.Columns.Add(HORARIO, typeof(string));
            tInfoPacote.Columns.Add(SETOR_PRODUTO, typeof(string));
            tInfoPacote.Columns.Add(CODIGO, typeof(string));
            tInfoPacote.Columns.Add(PRECO, typeof(string));
            tInfoPacote.Columns.Add(CORTESIA, typeof(string));
            tInfoPacote.Columns.Add(BLOQUEIO, typeof(string));
            tInfoPacote.Columns.Add(VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(STATUS, typeof(string));
            tInfoPacote.Columns.Add(STATUS_DETALHADO, typeof(string));
            tInfoPacote.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoPacote.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoPacote.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
            tInfoPacote.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = ""; //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tInfoPacote.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoPacote.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(OBSERVACAO, typeof(string));
            tInfoPacote.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoPacote.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;

            ds.Tables.Add(tGrid);
            ds.Tables.Add(tInfoPacote);

            //Grid com a InfoPacote
            DataColumn colInfoPacoteID = tInfoPacote.Columns[RESERVAID];
            DataRelation dr2 = new DataRelation("GridXInfoPacote", colGridReservaID, colInfoPacoteID, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colGridReservaID, colInfoPacoteID);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tInfoPacote.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr2);

            return ds;

        }

        public static DataTable EstruturaInfoVenda()
        {

            DataTable tGrid = new DataTable(INFO_VENDA);

            tGrid.Columns.Add("ID", typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(TAXA_CONV_VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(TAXA_ENTREGA_VALOR, typeof(decimal));
            tGrid.Columns.Add(ENTREGA_CONTROLE_ID, typeof(int));
            tGrid.Columns.Add(TAXA_ENTREGA_TIPO, typeof(string));
            tGrid.Columns.Add(VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(STATUS_VENDA, typeof(string));
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tGrid.Columns.Add(CLIENTE, typeof(string));
            tGrid.Columns.Add(CLIENTEID, typeof(int));
            tGrid.Columns.Add(EMAIL, typeof(string));
            tGrid.Columns.Add(CANAL, typeof(string));
            tGrid.Columns.Add(VENDEDOR, typeof(string));
            tGrid.Columns.Add(SENHA, typeof(string));
            tGrid.Columns.Add(DATA_VENDA, typeof(DateTime));
            tGrid.Columns.Add(DATA_CAIXA, typeof(DateTime));
            tGrid.Columns.Add(OBSERVACAO, typeof(string));
            tGrid.Columns.Add(NIVELRISCO, typeof(int));
            tGrid.Columns.Add(ENTREGA_AGENDA_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(PAGAMENTO_PROCESSADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TAXAPROCESSAMENTOVALOR, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TAXAPROCESSAMENTOCANCELADA, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(VALORSEGURO, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TRANSACTION_ID, typeof(string)).DefaultValue = string.Empty;

            return tGrid;
        }

        #endregion

        public DataTable FormasPagamento(int canalID, int vendaID, int empresaID)
        {

            try
            {

                BD bd = new BD();

                DataTable tabela = new DataTable("FormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Dias", typeof(int));
                tabela.Columns.Add("TaxaAdm", typeof(decimal));
                tabela.Columns.Add("IR", typeof(string));

                string sql = "SELECT fp.ID,Nome,efp.Dias,efp.TaxaAdm,efp.IR " +
                    "FROM tFormaPagamento as fp (NOLOCK) " +
                    "INNER JOIN tCanalFormaPagamento as cfp (NOLOCK) ON cfp.FormaPagamentoID=fp.ID AND cfp.CanalID= " + canalID + " " +
                    "LEFT JOIN tEmpresaFormaPagamento AS efp (NOLOCK) ON efp.FormaPagamentoID= fp.ID AND efp.EmpresaID = " + empresaID + " " +
                    "UNION " +
                    "SELECT fp.ID, Nome,efp.Dias,efp.TaxaAdm,efp.IR " +
                    "FROM tVendaBilheteria (NOLOCK) " +
                    "INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID " +
                    "INNER JOIN tFormaPagamento AS fp (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = fp.ID " +
                    "LEFT JOIN tEmpresaFormaPagamento AS efp (NOLOCK) ON efp.FormaPagamentoID= fp.ID AND efp.EmpresaID = " + empresaID + " " +
                    "WHERE tVendaBilheteriaFormaPagamento.VendaBilheteriaID=" + vendaID + " " +
                    "ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Dias"] = bd.LerInt("Dias");
                    linha["TaxaAdm"] = bd.LerDecimal("TaxaAdm");
                    linha["IR"] = bd.LerString("IR");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string MontaSQLPesquisa(string strCriterio)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, ");
            sb.Append("vb.EntregaControleID, vb.EntregaAgendaID, vb.ValorTotal, vb.Status, vb.Senha, vb.DataVenda, vb.Obs, ");
            sb.Append("c.Nome AS Cliente, vb.ClienteID , c.Email, tCanal.Nome AS Canal, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, vb.NivelRisco, ");
            sb.Append("IsNull(e.Tipo, '') AS TaxaEntregaTipo , c.CNPJ , c.NomeFantasia, IsNull(vb.PagamentoProcessado, 'T') AS PagamentoProcessado, TaxaProcessamentoValor, TaxaProcessamentoCancelada, ISNULL(vb.ValorSeguro, 0) AS ValorSeguro, ISNULL(TransactionID,'') as TransactionID  ");
            sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
            sb.Append("LEFT JOIN tVendaBilheteriaFormaPagamento AS vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
            sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
            sb.Append("INNER JOIN tUsuario (NOLOCK) AS u ON u.ID = tCaixa.UsuarioID ");
            sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
            sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
            if (perfilID == Perfil.LOCAL_SUPERVISOR || 
                perfilID == Perfil.SAC_OPERADOR ||
                perfilID == Perfil.SAC_SUPERVISOR ||
                perfilID == Perfil.SAC_OPERADOR_NOVO ||
                perfilID == Perfil.SAC_SUPERVISOR_NOVO ||
                perfilID == Perfil.SEGURANCA_ESPECIAL)
                sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
            // Adiciona Tabelas para Chegar até o LocalID

            sb.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
            sb.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON ec.ID = vb.EntregaControleID ");
            sb.Append("LEFT JOIN tEntrega e (NOLOCK) ON e.ID = ec.EntregaID ");
            sb.Append("WHERE (1 = 1) " + strCriterio);

            return sb.ToString();
        }

        private string MontaSQLIngressos(string strCriterio)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT tIngresso.VendaBilheteriaID, tIngresso.Status, LOG.ID, LOG.IngressoID, LOG.VendaBilheteriaItemID, tVendaBilheteria.Obs, ");
            sb.Append("tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacao.DisponivelAjuste, tSetor.Nome AS Setor, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tPreco.Nome AS Preco, tPreco.Valor, tIngresso.Codigo, tCortesia.Nome AS Cortesia, tBloqueio.Nome AS Bloqueio, tPacote.ID AS PacoteID, tPacote.Nome AS Pacote, tVendaBilheteriaItem.TaxaConveniencia, tVendaBilheteriaItem.TaxaConvenienciaValor, tVendaBilheteriaItem.TaxaComissao, tVendaBilheteriaItem.ComissaoValor, tVendaBilheteriaItem.PacoteGrupo, ");
            sb.Append("tEvento.TipoCodigoBarra, tIngresso.CodigoBarra, tIngresso.ApresentacaoSetorID, tIngresso.EventoID , tIngresso.AssinaturaClienteID, ISNULL(tAssinatura.Nome, '') as AssinaturaNome ");
            sb.Append("FROM tIngressolog AS LOG (NOLOCK) ");
            sb.Append("INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = LOG.IngressoID ");
            sb.Append("INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID ");
            sb.Append("INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID ");
            sb.Append("INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID ");
            sb.Append("LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID ");
            sb.Append("LEFT JOIN tCortesia (NOLOCK) ON tCortesia.ID = tIngresso.CortesiaID ");
            sb.Append("LEFT JOIN tBloqueio (NOLOCK) ON tBloqueio.ID = tIngresso.BloqueioID ");
            sb.Append("INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID ");
            sb.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID ");
            sb.Append("LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID ");
            sb.Append("LEFT JOIN tAssinaturaCliente (NOLOCK) ON tAssinaturaCliente.ID = tIngresso.AssinaturaClienteID ");
            sb.Append("LEFT JOIN tAssinatura (NOLOCK) ON tAssinatura.ID = tAssinaturaCliente.AssinaturaID ");
            sb.Append("WHERE (1 = 1) " + strCriterio);
            return sb.ToString();
        }

        private string MontaSQLValorPacote(string strCriterio)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("SUM(tPreco.Valor) as Valor, ");
            sb.Append("tVendaBilheteriaItem.TaxaConveniencia, ");
            sb.Append("SUM(tVendaBilheteriaItem.TaxaConvenienciaValor) as TaxaConvenienciaValor, ");
            sb.Append("tVendaBilheteriaItem.TaxaComissao, ");
            sb.Append("SUM(tVendaBilheteriaItem.ComissaoValor) as ComissaoValor ");
            sb.Append("FROM tIngressolog AS LOG (NOLOCK) ");
            sb.Append("LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID ");
            sb.Append("INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID ");
            sb.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID ");
            sb.Append("LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID ");
            sb.Append("WHERE (1 = 1) " + strCriterio);
            sb.Append(" group by tVendaBilheteriaItem.TaxaConveniencia, ");
            sb.Append("tVendaBilheteriaItem.TaxaComissao ");

            return sb.ToString();
        }

        /// <summary>
        /// Pesquisa pelo codigo de barras
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarCodigoBarras(string codigoBarra)
        {

            try
            {

                DataSet retorno = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                string sql = "";
                int vendaID = 0;

                BD bd = new BD();

                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "(CodigoBarra='" + codigoBarra + "' OR CodigoBarraCliente='" + codigoBarra + "')";
                ingressoLista.FiltroSQL = "(Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "')";
                ingressoLista.Carregar();

                if (ingressoLista.Tamanho > 0)
                {
                    int ingressoID = int.Parse(ingressoLista.ToString());
                    sql = "SELECT dbo.BuscaUltimaVendaBilheteriaID(" + ingressoID + ") AS VendaBilheteriaID";

                    bd.Consulta(sql);
                    if (bd.Consulta().Read())
                    {
                        vendaID = bd.LerInt("VendaBilheteriaID");
                    }

                }

                if (vendaID == 0)
                    throw new CancelamentoGerenciadorException("Ingresso não existe ou está disponível.\nA venda desse ingresso não foi localizada ou já foi cancelado.");

                switch (perfilID)
                {
                    case Perfil.CANAL_BILHETEIRO:
                        string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                        //"AND (tCaixa.DataAbertura LIKE '" + hoje + "%' ) " +
                        sql = MontaSQLPesquisa("" +
                            "AND (tCaixa.UsuarioID = " + usuarioID + ") " +
                            "AND (SUBSTRING(tCaixa.DataAbertura, 1, 8) = '" + hoje + "' ) " +
                            "AND (tCaixa.DataFechamento = '') " +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        sql = MontaSQLPesquisa("" +
                            "AND (tCanal.ID = " + canalID + ") " +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        sql = MontaSQLPesquisa("" +
                             "AND " +
                             "( " +
                             "      ( " +
                             "          tCanal.ID = " + canalID + " " +
                             "      ) " +
                             "      OR " +
                             "      ( " +
                             "          tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " " +
                             "      ) " +
                             ") " +
                             "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        sql = MontaSQLPesquisa("" +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    default:
                        throw new CancelamentoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();

                    linha["ID"] = bd.LerInt("ID");
                    linha[TAXA_CONV_VALOR_TOTAL] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha[TAXA_ENTREGA_VALOR] = bd.LerDecimal("TaxaEntregaValor");
                    linha[ENTREGA_CONTROLE_ID] = bd.LerInt("EntregaControleID");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                    linha[STATUS_VENDA] = bd.LerString("Status");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        linha[CLIENTE] = bd.LerString("Cliente");
                    else
                        linha[CLIENTE] = bd.LerString("NomeFantasia");

                    linha[CLIENTEID] = bd.LerInt(CLIENTEID);
                    linha[EMAIL] = bd.LerString("Email");
                    linha[CANAL] = bd.LerString("Canal");
                    linha[VENDEDOR] = bd.LerString("Vendedor");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                    linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                    linha[NIVELRISCO] = bd.LerInt("NivelRisco");
                    linha[TAXA_ENTREGA_TIPO] = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                    linha[ENTREGA_AGENDA_ID] = bd.LerInt("EntregaAgendaID");
                    linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                    linha[TAXAPROCESSAMENTOCANCELADA] = bd.LerBoolean(TAXAPROCESSAMENTOCANCELADA);
                    linha[TAXAPROCESSAMENTOVALOR] = bd.LerDecimal(TAXAPROCESSAMENTOVALOR);
                    linha[VALORSEGURO] = bd.LerDecimal(VALORSEGURO);
                    linha[TRANSACTION_ID] = bd.LerString(TRANSACTION_ID);


                    info.Rows.Add(linha);

                }
                bd.Fechar();

                DataTable tabela = EstruturaTabela();

                //trazer os ingressos
                sql = MontaSQLIngressos("" +
                "AND (LOG.VendaBilheteriaItemID = dbo.BuscaCodigoUltimaVendaBilheteriaItemID ('" + codigoBarra + "')) " +
                "AND (LOG.Acao = '" + IngressoLog.VENDER + "' OR LOG.Acao = '" + IngressoLog.PRE_RESERVA + "') " +
                "AND (LOG.VendaBilheteriaID = " + vendaID + ") " +
                "");
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("VendaBilheteriaID") == vendaID)
                    {

                        DataRow linha = tabela.NewRow();

                        linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                        linha[INGRESSOLOG_ID] = bd.LerInt("ID");
                        linha[VENDA_BILHETERIA_ID] = bd.LerInt("VendaBilheteriaID");
                        linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");
                        linha[EVENTO] = bd.LerString("Evento");
                        linha[PACOTE] = bd.LerString("Pacote");
                        linha[PACOTEID] = bd.LerInt("PacoteID");
                        linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                        linha[SETOR_PRODUTO] = bd.LerString("Setor");
                        linha[CODIGO] = bd.LerString("Codigo");
                        linha[PRECO] = bd.LerString("Preco");
                        if (bd.LerString("Cortesia") != "")
                            linha[CORTESIA] = bd.LerString("Cortesia");
                        if (bd.LerString("Bloqueio") != "")
                            linha[BLOQUEIO] = bd.LerString("Bloqueio");
                        linha[PRECOID] = bd.LerInt("PrecoID");
                        linha[CORTESIAID] = bd.LerInt("CortesiaID");
                        linha[BLOQUEIOID] = bd.LerInt("BloqueioID");
                        linha[VALOR] = bd.LerDecimal("Valor");
                        linha[STATUS] = bd.LerString("Status");
                        linha[CONV] = bd.LerInt("TaxaConveniencia");
                        linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                        linha[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                        linha[COMISSAO_VALOR] = bd.LerDecimal("ComissaoValor");
                        linha[DISPONIVEL_AJUSTE] = bd.LerString("DisponivelAjuste");
                        linha[PACOTE_GRUPO] = bd.LerString("PacoteGrupo");
                        linha[TIPO_CODIGO_BARRA] = bd.LerString("TipoCodigoBarra");
                        linha[CODIGO_BARRA] = bd.LerString(CODIGO_BARRA);
                        linha[EVENTO_ID] = bd.LerInt(EVENTO_ID);
                        linha[APRESENTACAO_SETOR_ID] = bd.LerInt(APRESENTACAO_SETOR_ID);

                        tabela.Rows.Add(linha);
                    }
                }

                DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);

                foreach (DataRow linha in tabelaTmp.Rows)
                {

                    int vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                    DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                    DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();
                    novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];

                    //somar o valor
                    object valor = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                    novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;

                    Pacote oP = new Pacote();
                    oP.Ler((int)linhas[0][PACOTEID]);


                    if (oP.PermitirCancelamentoAvulso.Valor)
                    {

                        novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];

                        novoItemGrid[TIPO] = TIPO_INGRESSO;

                        if (linhas.Length > 1)
                        {
                            novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                        }
                        else
                        {
                            novoItemGrid[TIPO_LUGAR] = Setor.Pista;
                        }

                        novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                        novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                        novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                        novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                        if (linhas[0][CORTESIA] != DBNull.Value)
                            novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                        if (linhas[0][BLOQUEIO] != DBNull.Value)
                            novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                        novoItemGrid[PRECO] = (string)linhas[0][PRECO];

                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = 0;
                        novoItemGrid[PACOTE_COMPLETO] = true;
                        novoItemGrid[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];
                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), info.Rows[0][TAXA_ENTREGA_TIPO].ToString());
                        novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];

                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[CONV] = (int)linhas[0][CONV];
                        novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];

                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);


                    }
                    else
                    {

                        if ((int)linhas[0][PACOTEID] != 0)
                        {
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            novoItemGrid[TIPO] = TIPO_PACOTE;
                        }
                        else
                        {
                            novoItemGrid[TIPO] = TIPO_INGRESSO;
                            if (linhas.Length > 1)
                            {
                                novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                            }
                            else
                            {
                                novoItemGrid[TIPO_LUGAR] = Setor.Pista;
                            }
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                            novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                            novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                            if (linhas[0][CORTESIA] != DBNull.Value)
                                novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                            if (linhas[0][BLOQUEIO] != DBNull.Value)
                                novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                            novoItemGrid[PRECO] = (string)linhas[0][PRECO];
                        }
                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[CONV] = (int)linhas[0][CONV];
                        novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];

                        //object valorConv = tabela.Compute("SUM(["+VALOR_CONV+"])",VENDA_BILHETERIA_ITEM_ID+"="+vendaBilheteriaItemID);
                        //|novoItemGrid[VALOR_CONV] = (valorConv!=DBNull.Value) ? Convert.ToDecimal(valorConv) : 0;
                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                    }


                    for (int i = 0; i < linhas.Length; i++)
                    {
                        DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                        novoItemReserva[INGRESSOID] = (int)linhas[i][INGRESSO_ID];
                        novoItemReserva[PACOTEID] = (int)linhas[0][PACOTEID];
                        novoItemReserva[PRECOID] = (int)linhas[i][PRECOID];
                        novoItemReserva[CORTESIAID] = (int)linhas[i][CORTESIAID];
                        //novoItemReserva[DISPONIVEL_AJUSTE] = (String)linhas[i][DISPONIVEL_AJUSTE];
                        novoItemReserva[BLOQUEIOID] = (int)linhas[i][BLOQUEIOID];
                        novoItemReserva[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[i][VENDA_BILHETERIA_ITEM_ID];
                        novoItemReserva[TIPO_CODIGO_BARRA] = linhas[i][TIPO_CODIGO_BARRA];
                        novoItemReserva[CODIGO_BARRA] = linhas[i][CODIGO_BARRA];
                        novoItemReserva[EVENTO_ID] = linhas[i][EVENTO_ID];
                        novoItemReserva[APRESENTACAO_SETOR_ID] = linhas[i][APRESENTACAO_SETOR_ID];

                        retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                    }

                }


                retorno.Tables.Add(info);

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Pesquisa pelo codigo do ingresso
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarCodigoIngresso(int apresentacaoSetorID, string codigo)
        {
            try
            {
                DataSet retorno = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                string sql = "";
                int vendaID = 0;

                BD bd = new BD();

                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "ApresentacaoSetorID=" + apresentacaoSetorID;
                ingressoLista.FiltroSQL = "Codigo='" + codigo + "'";
                ingressoLista.FiltroSQL = "(Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "')";
                ingressoLista.Carregar();

                if (ingressoLista.Tamanho > 0)
                {
                    int ingressoID = int.Parse(ingressoLista.ToString());
                    sql = "SELECT dbo.BuscaUltimaVendaBilheteriaID(" + ingressoID + ") AS VendaBilheteriaID";

                    bd.Consulta(sql);
                    if (bd.Consulta().Read())
                    {
                        vendaID = bd.LerInt("VendaBilheteriaID");
                    }
                }

                if (vendaID == 0)
                    throw new CancelamentoGerenciadorException("Ingresso não existe ou está disponível.\nA venda desse ingresso não foi localizada ou já foi cancelado.");

                switch (perfilID)
                {
                    case Perfil.CANAL_BILHETEIRO:
                        string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                        sql = MontaSQLPesquisa("" +
                            "AND (tCaixa.UsuarioID = " + usuarioID + ") " +
                            "AND (SUBSTRING(tCaixa.DataAbertura, 1, 8) = '" + hoje + "' ) " +
                            "AND (tCaixa.DataFechamento = '') " +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        sql = MontaSQLPesquisa("" +
                            "AND (tCanal.ID = " + canalID + ") " +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        sql = MontaSQLPesquisa("" +
                             "AND " +
                             "( " +
                             "      ( " +
                             "          tCanal.ID = " + canalID + " " +
                             "      ) " +
                             "      OR " +
                             "      ( " +
                             "          tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " " +
                             "      ) " +
                             ") " +
                             "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        sql = MontaSQLPesquisa("" +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    default:
                        throw new CancelamentoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();

                    linha["ID"] = bd.LerInt("ID");
                    linha[TAXA_CONV_VALOR_TOTAL] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha[TAXA_ENTREGA_VALOR] = bd.LerDecimal("TaxaEntregaValor");
                    linha[ENTREGA_CONTROLE_ID] = bd.LerInt("EntregaControleID");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                    linha[STATUS_VENDA] = bd.LerString("Status");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        linha[CLIENTE] = bd.LerString("Cliente");
                    else
                        linha[CLIENTE] = bd.LerString("NomeFantasia");

                    linha[CLIENTEID] = bd.LerInt(CLIENTEID);
                    linha[EMAIL] = bd.LerString("Email");
                    linha[CANAL] = bd.LerString("Canal");
                    linha[VENDEDOR] = bd.LerString("Vendedor");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                    linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                    linha[TAXA_ENTREGA_TIPO] = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                    linha[ENTREGA_AGENDA_ID] = bd.LerInt("EntregaAgendaID");
                    linha[NIVELRISCO] = bd.LerInt("NivelRisco");
                    linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                    linha[TAXAPROCESSAMENTOCANCELADA] = bd.LerBoolean(TAXAPROCESSAMENTOCANCELADA);
                    linha[TAXAPROCESSAMENTOVALOR] = bd.LerDecimal(TAXAPROCESSAMENTOVALOR);
                    linha[VALORSEGURO] = bd.LerDecimal(VALORSEGURO);
                    linha[TRANSACTION_ID] = bd.LerString(TRANSACTION_ID);

                    info.Rows.Add(linha);
                }
                bd.Fechar();

                DataTable tabela = EstruturaTabela();

                //trazer os ingressos
                sql = MontaSQLIngressos("" +
                    "AND (LOG.VendaBilheteriaItemID = dbo.BuscaUltimaVendaBilheteriaItemID ('" + codigo + "', " + apresentacaoSetorID + ")) " +
                    "AND (LOG.Acao = '" + IngressoLog.VENDER + "' OR LOG.Acao = '" + IngressoLog.PRE_RESERVA + "') " +
                    "AND (LOG.VendaBilheteriaID = " + vendaID + ") " +
                    "");
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("VendaBilheteriaID") == vendaID)
                    {
                        DataRow linha = tabela.NewRow();

                        linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                        linha[INGRESSOLOG_ID] = bd.LerInt("ID");
                        linha[VENDA_BILHETERIA_ID] = bd.LerInt("VendaBilheteriaID");
                        linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");
                        linha[EVENTO] = bd.LerString("Evento");
                        linha[PACOTE] = bd.LerString("Pacote");
                        linha[PACOTEID] = bd.LerInt("PacoteID");
                        linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                        linha[SETOR_PRODUTO] = bd.LerString("Setor");
                        linha[CODIGO] = bd.LerString("Codigo");
                        linha[PRECO] = bd.LerString("Preco");
                        if (bd.LerString("Cortesia") != "")
                            linha[CORTESIA] = bd.LerString("Cortesia");
                        if (bd.LerString("Bloqueio") != "")
                            linha[BLOQUEIO] = bd.LerString("Bloqueio");
                        linha[PRECOID] = bd.LerInt("PrecoID");
                        linha[CORTESIAID] = bd.LerInt("CortesiaID");
                        linha[BLOQUEIOID] = bd.LerInt("BloqueioID");
                        linha[VALOR] = bd.LerDecimal("Valor");
                        linha[STATUS] = bd.LerString("Status");
                        linha[CONV] = bd.LerInt("TaxaConveniencia");
                        linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                        linha[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                        linha[COMISSAO_VALOR] = bd.LerDecimal("ComissaoValor");
                        linha[DISPONIVEL_AJUSTE] = bd.LerString("DisponivelAjuste");
                        linha[PACOTE_GRUPO] = bd.LerString("PacoteGrupo");
                        linha[TIPO_CODIGO_BARRA] = bd.LerString("TipoCodigoBarra");
                        linha[CODIGO_BARRA] = bd.LerString("CodigoBarra");
                        linha[EVENTO_ID] = bd.LerInt(EVENTO_ID);
                        linha[APRESENTACAO_SETOR_ID] = bd.LerInt(APRESENTACAO_SETOR_ID);
                        tabela.Rows.Add(linha);
                    }
                }

                DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);

                foreach (DataRow linha in tabelaTmp.Rows)
                {

                    int vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                    DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                    DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                    novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];

                    //somar o valor
                    object valor = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                    novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;

                    Pacote oP = new Pacote();
                    oP.Ler((int)linhas[0][PACOTEID]);

                    if (oP.PermitirCancelamentoAvulso.Valor)
                    {
                        novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];

                        novoItemGrid[TIPO] = TIPO_INGRESSO;

                        if (linhas.Length > 1)
                            novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                        else
                            novoItemGrid[TIPO_LUGAR] = Setor.Pista;

                        novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                        novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                        novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                        novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                        if (linhas[0][CORTESIA] != DBNull.Value)
                            novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                        if (linhas[0][BLOQUEIO] != DBNull.Value)
                            novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                        novoItemGrid[PRECO] = (string)linhas[0][PRECO];

                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = 0;
                        novoItemGrid[PACOTE_COMPLETO] = true;
                        novoItemGrid[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];
                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];

                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[CONV] = (int)linhas[0][CONV];
                        novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];

                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                    }
                    else
                    {
                        if ((int)linhas[0][PACOTEID] != 0)
                        {
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            novoItemGrid[TIPO] = TIPO_PACOTE;
                        }
                        else
                        {
                            novoItemGrid[TIPO] = TIPO_INGRESSO;

                            if (linhas.Length > 1)
                                novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                            else
                                novoItemGrid[TIPO_LUGAR] = Setor.Pista;

                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                            novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                            novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                            if (linhas[0][CORTESIA] != DBNull.Value)
                                novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                            if (linhas[0][BLOQUEIO] != DBNull.Value)
                                novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                            novoItemGrid[PRECO] = (string)linhas[0][PRECO];
                        }
                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[CONV] = (int)linhas[0][CONV];
                        novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];

                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                    }

                    for (int i = 0; i < linhas.Length; i++)
                    {
                        DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                        novoItemReserva[INGRESSOID] = (int)linhas[i][INGRESSO_ID];
                        novoItemReserva[PACOTEID] = (int)linhas[0][PACOTEID];
                        novoItemReserva[PRECOID] = (int)linhas[i][PRECOID];
                        novoItemReserva[CORTESIAID] = (int)linhas[i][CORTESIAID];
                        novoItemReserva[BLOQUEIOID] = (int)linhas[i][BLOQUEIOID];
                        novoItemReserva[TIPO_CODIGO_BARRA] = linhas[i][TIPO_CODIGO_BARRA];
                        novoItemReserva[CODIGO_BARRA] = linhas[i][CODIGO_BARRA];
                        novoItemReserva[EVENTO_ID] = linhas[i][EVENTO_ID];
                        novoItemReserva[APRESENTACAO_SETOR_ID] = linhas[i][APRESENTACAO_SETOR_ID];
                        novoItemReserva[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[i][VENDA_BILHETERIA_ITEM_ID];

                        retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                    }
                }

                retorno.Tables.Add(info);

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pesquisa pela senha
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarSenha(string senha)
        {
            try
            {
                DataSet retorno = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                int vendaID = 0;

                BD bd = new BD();

                string sql;

                int LojaID = (int)bd.ConsultaValor("SELECT LojaID From tCaixa (NOLOCK) Where ID = " + caixaID);

                switch (perfilID)
                {
                    case Perfil.CANAL_BILHETEIRO:
                        string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                        sql = MontaSQLPesquisa(
                            "AND (tCaixa.UsuarioID = " + usuarioID + ") " +
                            "AND (SUBSTRING(tCaixa.DataAbertura, 1, 8) = '" + hoje + "' ) " +
                            "AND (vb.Senha = '" + senha + "') ");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        sql = MontaSQLPesquisa(
                            "AND (tCanal.ID = " + canalID + ") " +
                            "AND (vb.Senha = '" + senha + "')");
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        sql = MontaSQLPesquisa(
                            "AND ( ( tCanal.ID = " + canalID + " ) OR ( tIngresso.EmpresaID = " + empresaID +
                            " AND tIngresso.LocalID = " + localID + " )) " +
                            "AND (vb.Senha = '" + senha + "') ");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        sql = MontaSQLPesquisa("" +
                            "AND (vb.Senha = '" + senha + "') " +
                            "");
                        break;
                    case Perfil.SEGURANCA_ESPECIAL:
                        sql = MontaSQLPesquisa(
                            "AND ( (tCanal.ID = " + canalID + " ) OR ( tIngresso.EmpresaID = "
                            + empresaID + " AND tIngresso.LocalID = " + localID + ")) " +
                            "AND (vb.Senha = '" + senha + "')");
                        break;
                    default:
                        throw new CancelamentoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();

                    vendaID = bd.LerInt("ID");
                    linha["ID"] = vendaID;
                    linha[TAXA_CONV_VALOR_TOTAL] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha[TAXA_ENTREGA_VALOR] = bd.LerDecimal("TaxaEntregaValor");
                    linha[ENTREGA_CONTROLE_ID] = bd.LerInt("EntregaControleID");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                    linha[STATUS_VENDA] = bd.LerString("Status");
                    String nomeCliente = bd.LerString("Cliente");
                    if (!String.IsNullOrEmpty(nomeCliente))
                        linha[CLIENTE] = bd.LerString("Cliente");
                    else
                        linha[CLIENTE] = bd.LerString("NomeFantasia");

                    linha[CLIENTEID] = bd.LerInt(CLIENTEID);
                    linha[EMAIL] = bd.LerString("Email");
                    linha[CANAL] = bd.LerString("Canal");
                    linha[VENDEDOR] = bd.LerString("Vendedor");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                    linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                    linha[OBSERVACAO] = bd.LerString("Obs");
                    linha[NIVELRISCO] = bd.LerInt("NivelRisco");
                    linha[TAXA_ENTREGA_TIPO] = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                    linha[ENTREGA_AGENDA_ID] = bd.LerInt("EntregaAgendaID");
                    linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                    linha[TAXAPROCESSAMENTOCANCELADA] = bd.LerBoolean(TAXAPROCESSAMENTOCANCELADA);
                    linha[TAXAPROCESSAMENTOVALOR] = bd.LerDecimal(TAXAPROCESSAMENTOVALOR);
                    linha[VALORSEGURO] = bd.LerDecimal(VALORSEGURO);
                    linha[TRANSACTION_ID] = bd.LerString(TRANSACTION_ID);

                    info.Rows.Add(linha);

                }
                bd.Fechar();

                if (vendaID != 0)
                {
                    //trazer os ingressos
                    sql = MontaSQLIngressos("" +
                        "AND (LOG.Acao='" + IngressoLog.VENDER + "' OR LOG.Acao='" + IngressoLog.PRE_RESERVA + "') " +
                        "AND (LOG.VendaBilheteriaID = " + vendaID + ") " +
                        "AND " +
                        "        ( " +
                        "            tIngresso.Status = '" + Ingresso.PRE_RESERVA + "' " +
                        "        OR " +
                        "            tIngresso.Status = '" + Ingresso.AGUARDANDO_TROCA + "' " +
                        "        OR " +
                        "            tIngresso.Status = '" + Ingresso.VENDIDO + "' " +
                        "        OR " +
                        "            tIngresso.Status = '" + Ingresso.IMPRESSO + "' " +
                        "        OR " +
                        "            tIngresso.Status = '" + Ingresso.ENTREGUE + "' " +
                        "        ) " +
                        "");
                    bd.Consulta(sql);

                    DataTable tabela = EstruturaTabela();

                    while (bd.Consulta().Read())
                    {
                        if (bd.LerInt("VendaBilheteriaID") == vendaID)
                        {

                            DataRow linha = tabela.NewRow();

                            linha[VENDA_BILHETERIA_ID] = vendaID;

                            linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                            linha[INGRESSOLOG_ID] = bd.LerInt("ID");
                            linha[DISPONIVEL_AJUSTE] = bd.LerString("DisponivelAjuste");
                            linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");

                            if (bd.LerInt("AssinaturaClienteID") > 0)
                            {
                                linha[EVENTO] = bd.LerString("AssinaturaNome");
                                linha[HORARIO] = "-";
                                linha[BLOQUEIO] = "-";
                                linha[ASSINATURA_CLIENTE_ID] = bd.LerInt("AssinaturaClienteID");
                            }
                            else
                            {
                                linha[ASSINATURA_CLIENTE_ID] = 0;
                                linha[EVENTO] = bd.LerString("Evento");
                                linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                                if (bd.LerString("Bloqueio") != "")
                                    linha[BLOQUEIO] = bd.LerString("Bloqueio");
                            }

                            linha[PACOTE] = bd.LerString("Pacote");
                            linha[PACOTEID] = bd.LerInt("PacoteID");

                            linha[SETOR_PRODUTO] = bd.LerString("Setor");
                            linha[CODIGO] = bd.LerString("Codigo");
                            linha[PRECO] = bd.LerString("Preco");
                            if (bd.LerString("Cortesia") != "")
                                linha[CORTESIA] = bd.LerString("Cortesia");
                            linha[PRECOID] = bd.LerInt("PrecoID");
                            linha[CORTESIAID] = bd.LerInt("CortesiaID");
                            linha[BLOQUEIOID] = bd.LerInt("BloqueioID");
                            linha[VALOR] = bd.LerDecimal("Valor");
                            linha[STATUS] = bd.LerString("Status");
                            linha[CONV] = bd.LerInt("TaxaConveniencia");
                            linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                            linha[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                            linha[COMISSAO_VALOR] = bd.LerDecimal("ComissaoValor");
                            linha[OBSERVACAO] = bd.LerString("Obs");
                            linha[PACOTE_GRUPO] = bd.LerInt("PacoteGrupo");
                            linha[TIPO_CODIGO_BARRA] = bd.LerString("TipoCodigoBarra");
                            linha[CODIGO_BARRA] = bd.LerString("CodigoBarra");
                            linha[EVENTO_ID] = bd.LerInt(EVENTO_ID);
                            linha[APRESENTACAO_SETOR_ID] = bd.LerInt(APRESENTACAO_SETOR_ID);
                            tabela.Rows.Add(linha);
                        }
                    }

                    DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);

                    int pacoteIdAtual = 0;
                    int pacoteReservaID = 0;
                    int pacoteGrupo = 0;
                    int vBItemId = 0;
                    int assinaturaClienteID = 0;
                    List<int> lstAssinaturaClienteID = new List<int>();

                    foreach (DataRow linha in tabelaTmp.Rows)
                    {
                        int vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                        DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                        DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();
                        DataRow novoInfoPacote = retorno.Tables[TABELA_INFO_PACOTE].NewRow();

                        novoInfoPacote[PACOTE_GRUPO] = 0;

                        vBItemId = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = vBItemId;

                        assinaturaClienteID = (int)linhas[0][ASSINATURA_CLIENTE_ID];

                        object valor = DBNull.Value;
                        object valorIndividual = DBNull.Value;
                        if (assinaturaClienteID > 0)
                        {
                            //soma valor Assinatura
                            valor = tabela.Compute("SUM(Valor)", ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                            novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                            valorIndividual = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                        }
                        else
                        {
                            //somar o valor
                            valor = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                            novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                        }

                        Pacote oP = new Pacote();

                        if (oP.CancelamentoAvulso((int)linhas[0][PACOTEID]))
                        {
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            novoItemGrid[TIPO] = TIPO_PACOTE;

                            novoInfoPacote[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                            novoInfoPacote[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                            novoInfoPacote[TIPO] = TIPO_INGRESSO;
                            if (linhas.Length > 1)
                            {
                                novoInfoPacote[TIPO_LUGAR] = Setor.MesaFechada;
                            }
                            else
                            {
                                novoInfoPacote[TIPO_LUGAR] = Setor.Pista;
                            }

                            novoInfoPacote[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                            novoInfoPacote[HORARIO] = (string)linhas[0][HORARIO];
                            novoInfoPacote[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            novoInfoPacote[CODIGO] = (string)linhas[0][CODIGO];
                            if (linhas[0][CORTESIA] != DBNull.Value)
                                novoInfoPacote[CORTESIA] = (string)linhas[0][CORTESIA];
                            if (linhas[0][BLOQUEIO] != DBNull.Value)
                                novoInfoPacote[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                            novoInfoPacote[PRECO] = (string)linhas[0][PRECO];
                            novoInfoPacote[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoInfoPacote[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoInfoPacote[CONV] = (int)linhas[0][CONV];
                            novoInfoPacote[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                            novoInfoPacote[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoInfoPacote[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                            novoInfoPacote[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                            novoInfoPacote[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];

                            BD bdAux = new BD();

                            string sqlValor = MontaSQLValorPacote("AND LOG.VendaBilheteriaID = " + vendaID + " AND  tVendaBilheteriaItem.PacoteID = " + (int)linhas[0][PACOTEID] + " AND  tVendaBilheteriaItem.PacoteGrupo = " + (int)linhas[0][PACOTE_GRUPO] + " ");

                            bdAux.Consulta(sqlValor);

                            while (bdAux.Consulta().Read())
                            {
                                novoItemGrid[CONV] = bdAux.LerInt("TaxaConveniencia");
                                novoItemGrid[VALOR_CONV] = bdAux.LerDecimal("TaxaConvenienciaValor");
                                novoItemGrid[TAXA_COMISSAO] = bdAux.LerInt("TaxaComissao");
                                novoItemGrid[COMISSAO_VALOR] = bdAux.LerDecimal("ComissaoValor");
                                novoItemGrid[VALOR] = bdAux.LerDecimal("Valor");
                            }

                            bdAux.Fechar();

                            novoItemGrid[PACOTE_COMPLETO] = true;
                            novoItemGrid[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];
                            novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoItemGrid[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;
                        }
                        else
                        {
                            if ((int)linhas[0][PACOTEID] != 0)
                            {
                                novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                                novoItemGrid[TIPO] = TIPO_PACOTE;
                            }
                            else
                            {
                                novoItemGrid[TIPO] = TIPO_INGRESSO;

                                if (linhas.Length > 1)
                                    novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                                else
                                    novoItemGrid[TIPO_LUGAR] = Setor.Pista;

                                novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                                novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                                novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                                novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                                if (linhas[0][CORTESIA] != DBNull.Value)
                                    novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                                if (linhas[0][BLOQUEIO] != DBNull.Value)
                                    novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                                novoItemGrid[PRECO] = (string)linhas[0][PRECO];
                            }
                            novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoItemGrid[CONV] = (int)linhas[0][CONV];
                            novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                            novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoItemGrid[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                            novoItemGrid[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                            novoItemGrid[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;

                        }
                        //object valorConv = tabela.Compute("SUM(["+VALOR_CONV+"])",VENDA_BILHETERIA_ITEM_ID+"="+vendaBilheteriaItemID);
                        //|novoItemGrid[VALOR_CONV] = (valorConv!=DBNull.Value) ? Convert.ToDecimal(valorConv) : 0;
                        if (pacoteIdAtual != oP.Control.ID)
                        {
                            pacoteGrupo = 0;
                        }

                        if (!lstAssinaturaClienteID.Contains(assinaturaClienteID) && (!oP.PermitirCancelamentoAvulso.Valor || pacoteGrupo != (int)novoInfoPacote[PACOTE_GRUPO]))
                        {
                            retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                            pacoteIdAtual = oP.Control.ID;
                            pacoteReservaID = (int)novoItemGrid[RESERVAID];
                            pacoteGrupo = (int)novoInfoPacote[PACOTE_GRUPO];

                            if (assinaturaClienteID > 0)
                                lstAssinaturaClienteID.Add(assinaturaClienteID);
                        }

                        if (assinaturaClienteID > 0)
                        {
                            DataRow[] dr = retorno.Tables[TABELA_RESERVA].Select(ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                            if (dr.Length > 0)
                            {
                                int possuiReservaID = (int)dr[0][RESERVAID];
                                if (possuiReservaID > 0)
                                    pacoteReservaID = possuiReservaID;
                            }

                            DataRow novoAssinatura = retorno.Tables[TABELA_INFO_ASSINATURA].NewRow();
                            for (int i = 0; i < novoItemGrid.ItemArray.Length; i++)
                            {
                                novoAssinatura[i] = novoItemGrid[i];
                            }

                            novoAssinatura[VALOR] = (valorIndividual != DBNull.Value) ? Convert.ToDecimal(valorIndividual) : 0;
                            novoAssinatura[RESERVAID] = pacoteReservaID;

                            retorno.Tables[TABELA_INFO_ASSINATURA].Rows.Add(novoAssinatura);
                        }

                        for (int i = 0; i < linhas.Length; i++)
                        {
                            DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                            novoItemReserva[RESERVAID] = oP.PermitirCancelamentoAvulso.Valor || assinaturaClienteID > 0 ? pacoteReservaID : (int)novoItemGrid[RESERVAID];
                            novoItemReserva[VENDA_BILHETERIA_ITEM_ID] = vBItemId;
                            novoItemReserva[INGRESSOID] = (int)linhas[i][INGRESSO_ID];
                            novoItemReserva[PACOTEID] = (int)linhas[i][PACOTEID];
                            novoItemReserva[PRECOID] = (int)linhas[i][PRECOID];
                            novoItemReserva[CORTESIAID] = (int)linhas[i][CORTESIAID];
                            novoItemReserva[BLOQUEIOID] = (int)linhas[i][BLOQUEIOID];
                            novoItemReserva[TIPO_CODIGO_BARRA] = linhas[i][TIPO_CODIGO_BARRA];
                            novoItemReserva[CODIGO_BARRA] = linhas[i][CODIGO_BARRA];
                            novoItemReserva[EVENTO_ID] = linhas[i][EVENTO_ID];
                            novoItemReserva[APRESENTACAO_SETOR_ID] = linhas[i][APRESENTACAO_SETOR_ID];
                            novoItemReserva[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;

                            retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                        }


                        if (oP.PermitirCancelamentoAvulso.Valor)
                        {
                            novoInfoPacote[RESERVAID] = pacoteReservaID;
                            retorno.Tables[TABELA_INFO_PACOTE].Rows.Add(novoInfoPacote);
                        }
                    }
                }

                retorno.Tables.Add(info);

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pesquisa pela senha de cliente
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarVenda(int vendaID)
        {
            try
            {
                DataSet retorno = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                BD bd = new BD();

                string sql;

                switch (perfilID)
                {

                    case Perfil.CANAL_BILHETEIRO:
                        string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                        sql = MontaSQLPesquisa("" +
                            "AND (tCaixa.UsuarioID = " + usuarioID + ") " +
                            "AND (SUBSTRING(tCaixa.DataAbertura, 1, 8) = '" + hoje + "' ) " +
                            "AND (tCaixa.DataFechamento = '') " +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        sql = MontaSQLPesquisa("" +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        sql = MontaSQLPesquisa("" +
                             "AND " +
                             "( " +
                             "      ( " +
                             "          tCanal.ID = " + canalID + " " +
                             "      ) " +
                             "      OR " +
                             "      ( " +
                             "          tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " " +
                             "      ) " +
                             ") " +
                             "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                    case Perfil.SEGURANCA_ESPECIAL:
                        sql = MontaSQLPesquisa("" +
                            "AND (vb.ID = " + vendaID + ") " +
                            "");
                        break;
                    default:
                        throw new CancelamentoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(sql);

                // Zera o ID da Venda
                vendaID = 0;

                if (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();

                    vendaID = bd.LerInt("ID");
                    linha["ID"] = vendaID;
                    linha[TAXA_CONV_VALOR_TOTAL] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha[TAXA_ENTREGA_VALOR] = bd.LerDecimal("TaxaEntregaValor");
                    linha[ENTREGA_CONTROLE_ID] = bd.LerInt("EntregaControleID");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                    linha[STATUS_VENDA] = bd.LerString("Status");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        linha[CLIENTE] = bd.LerString("Cliente");
                    else
                        linha[CLIENTE] = bd.LerString("NomeFantasia");
                    linha[CLIENTEID] = bd.LerInt(CLIENTEID);
                    linha[CANAL] = bd.LerString("Canal");
                    linha[VENDEDOR] = bd.LerString("Vendedor");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                    linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                    linha[NIVELRISCO] = bd.LerInt("NivelRisco");
                    linha[TAXA_ENTREGA_TIPO] = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                    linha[ENTREGA_AGENDA_ID] = bd.LerInt("EntregaAgendaID");
                    linha[EMAIL] = bd.LerString("Email");
                    linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                    linha[TAXAPROCESSAMENTOCANCELADA] = bd.LerBoolean(TAXAPROCESSAMENTOCANCELADA);
                    linha[TAXAPROCESSAMENTOVALOR] = bd.LerDecimal(TAXAPROCESSAMENTOVALOR);
                    linha[VALORSEGURO] = bd.LerDecimal(VALORSEGURO);
                    linha[TRANSACTION_ID] = bd.LerString(TRANSACTION_ID);

                    info.Rows.Add(linha);

                }
                bd.Fechar();

                // Se o perfil do usuário não permitir acessar a senha, informar o usuário.
                if (vendaID == 0)
                    throw new CancelamentoGerenciadorException("Essa senha não pode ser visualizada por esse perfil.");

                DataTable tabela = EstruturaTabela();

                //trazer os ingressos
                sql = MontaSQLIngressos("" +
                "AND (LOG.Acao = '" + IngressoLog.VENDER + "' OR LOG.Acao = '" + IngressoLog.PRE_RESERVA + "') " +
                "AND (LOG.VendaBilheteriaID = " + vendaID + ") " +
                "AND " +
                "       ( " +
                "           tIngresso.Status = '" + Ingresso.PRE_RESERVA + "' " +
                "       OR " +
                "           tIngresso.Status = '" + Ingresso.AGUARDANDO_TROCA + "' " +
                "       OR " +
                "           tIngresso.Status = '" + Ingresso.VENDIDO + "' " +
                "       OR " +
                "           tIngresso.Status = '" + Ingresso.IMPRESSO + "' " +
                "       OR " +
                "           tIngresso.Status = '" + Ingresso.ENTREGUE + "' " +
                "       ) " +
                "");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("VendaBilheteriaID") == vendaID)
                    {

                        DataRow linha = tabela.NewRow();

                        linha[VENDA_BILHETERIA_ID] = vendaID;

                        linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                        linha[INGRESSOLOG_ID] = bd.LerInt("ID");
                        linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");


                        if (bd.LerInt("AssinaturaClienteID") > 0)
                        {
                            linha[EVENTO] = bd.LerString("AssinaturaNome");
                            linha[HORARIO] = "-";
                            linha[BLOQUEIO] = "-";
                            linha[ASSINATURA_CLIENTE_ID] = bd.LerInt("AssinaturaClienteID");
                        }
                        else
                        {
                            linha[ASSINATURA_CLIENTE_ID] = 0;
                            linha[EVENTO] = bd.LerString("Evento");
                            linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                            if (bd.LerString("Bloqueio") != "")
                                linha[BLOQUEIO] = bd.LerString("Bloqueio");
                        }

                        linha[PACOTE] = bd.LerString("Pacote");
                        linha[PACOTEID] = bd.LerInt("PacoteID");

                        linha[SETOR_PRODUTO] = bd.LerString("Setor");
                        linha[CODIGO] = bd.LerString("Codigo");
                        linha[PRECO] = bd.LerString("Preco");
                        if (bd.LerString("Cortesia") != "")
                            linha[CORTESIA] = bd.LerString("Cortesia");

                        linha[PRECOID] = bd.LerInt("PrecoID");
                        linha[CORTESIAID] = bd.LerInt("CortesiaID");
                        linha[BLOQUEIOID] = bd.LerInt("BloqueioID");
                        linha[VALOR] = bd.LerDecimal("Valor");
                        linha[STATUS] = bd.LerString("Status");
                        linha[CONV] = bd.LerInt("TaxaConveniencia");
                        linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                        linha[DISPONIVEL_AJUSTE] = bd.LerString("DisponivelAjuste");
                        linha[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                        linha[COMISSAO_VALOR] = bd.LerDecimal("ComissaoValor");
                        linha[PACOTE_GRUPO] = bd.LerDecimal("PacoteGrupo");
                        linha[TIPO_CODIGO_BARRA] = bd.LerString("TipoCodigoBarra");
                        linha[CODIGO_BARRA] = bd.LerString("CodigoBarra");
                        linha[EVENTO_ID] = bd.LerInt(EVENTO_ID);
                        linha[APRESENTACAO_SETOR_ID] = bd.LerInt(APRESENTACAO_SETOR_ID);
                        tabela.Rows.Add(linha);
                    }
                }

                object oValorConv = tabela.Compute("SUM([" + VALOR_CONV + "])", VENDA_BILHETERIA_ID + "=" + vendaID);
                info.Rows[0][TAXA_CONV_VALOR_TOTAL] = (oValorConv != DBNull.Value) ? Convert.ToDecimal(oValorConv) : 0;

                DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);

                int pacoteIdAtual = 0;
                int pacoteReservaID = 0;
                int pacoteGrupo = 0;
                int vBItemId = 0;
                int assinaturaClienteID = 0;
                List<int> lstAssinaturaClienteID = new List<int>();

                foreach (DataRow linha in tabelaTmp.Rows)
                {

                    int vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                    DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                    DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                    DataRow novoInfoPacote = retorno.Tables[TABELA_INFO_PACOTE].NewRow();
                    novoInfoPacote[PACOTE_GRUPO] = 0;

                    //novoItemGrid[INGRESSO_ID] = (int)linhas[0][INGRESSO_ID];
                    //novoItemGrid[INGRESSOLOG_ID] = (int)linhas[0][INGRESSOLOG_ID];
                    //novoItemGrid[VENDA_BILHETERIA_ID] = (int)linhas[0][VENDA_BILHETERIA_ID];
                    vBItemId = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                    novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = vBItemId;

                    assinaturaClienteID = (int)linhas[0][ASSINATURA_CLIENTE_ID];

                    object valor = DBNull.Value;
                    object valorIndividual = DBNull.Value;
                    if (assinaturaClienteID > 0)
                    {
                        //soma valor Assinatura
                        valor = tabela.Compute("SUM(Valor)", ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                        novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                        valorIndividual = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                    }
                    else
                    {
                        //somar o valor
                        valor = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                        novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                    }


                    Pacote oP = new Pacote();
                    oP.Ler((int)linhas[0][PACOTEID]);


                    if (oP.PermitirCancelamentoAvulso.Valor)
                    {
                        novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                        novoItemGrid[TIPO] = TIPO_PACOTE;

                        novoInfoPacote[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                        novoInfoPacote[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                        novoInfoPacote[TIPO] = TIPO_INGRESSO;
                        if (linhas.Length > 1)
                        {
                            novoInfoPacote[TIPO_LUGAR] = Setor.MesaFechada;
                        }
                        else
                        {
                            novoInfoPacote[TIPO_LUGAR] = Setor.Pista;
                        }

                        novoInfoPacote[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                        novoInfoPacote[HORARIO] = (string)linhas[0][HORARIO];
                        novoInfoPacote[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                        novoInfoPacote[CODIGO] = (string)linhas[0][CODIGO];
                        if (linhas[0][CORTESIA] != DBNull.Value)
                            novoInfoPacote[CORTESIA] = (string)linhas[0][CORTESIA];
                        if (linhas[0][BLOQUEIO] != DBNull.Value)
                            novoInfoPacote[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                        novoInfoPacote[PRECO] = (string)linhas[0][PRECO];
                        novoInfoPacote[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoInfoPacote[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoInfoPacote[CONV] = (int)linhas[0][CONV];
                        novoInfoPacote[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                        novoInfoPacote[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                        novoInfoPacote[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                        novoInfoPacote[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                        novoInfoPacote[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];

                        BD bdAux = new BD();
                        string sqlValor = MontaSQLValorPacote("AND LOG.VendaBilheteriaID = " + vendaID + " AND  tVendaBilheteriaItem.PacoteID = " + (int)linhas[0][PACOTEID] + " AND  tVendaBilheteriaItem.PacoteGrupo = " + (int)linhas[0][PACOTE_GRUPO] + " ");

                        bdAux.Consulta(sqlValor);
                        while (bdAux.Consulta().Read())
                        {

                            novoItemGrid[CONV] = bdAux.LerInt("TaxaConveniencia");
                            novoItemGrid[VALOR_CONV] = bdAux.LerDecimal("TaxaConvenienciaValor");
                            novoItemGrid[TAXA_COMISSAO] = bdAux.LerInt("TaxaComissao");
                            novoItemGrid[COMISSAO_VALOR] = bdAux.LerDecimal("ComissaoValor");
                            novoItemGrid[VALOR] = bdAux.LerDecimal("Valor");

                        }
                        bdAux.Fechar();

                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = 0;
                        novoItemGrid[PACOTE_COMPLETO] = true;
                        novoItemGrid[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];
                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];

                    }
                    else
                    {
                        if ((int)linhas[0][PACOTEID] != 0)
                        {
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            novoItemGrid[TIPO] = TIPO_PACOTE;
                        }
                        else
                        {
                            novoItemGrid[TIPO] = TIPO_INGRESSO;
                            if (linhas.Length > 1)
                            {
                                novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                            }
                            else
                            {
                                novoItemGrid[TIPO_LUGAR] = Setor.Pista;
                            }
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                            novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                            novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                            if (linhas[0][CORTESIA] != DBNull.Value)
                                novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                            if (linhas[0][BLOQUEIO] != DBNull.Value)
                                novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                            novoItemGrid[PRECO] = (string)linhas[0][PRECO];
                        }
                        novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                        novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                        novoItemGrid[CONV] = (int)linhas[0][CONV];
                        novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                        novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                        novoItemGrid[ASSINATURA_CLIENTE_ID] = (int)linhas[0][ASSINATURA_CLIENTE_ID];

                        //object valorConv = tabela.Compute("SUM(["+VALOR_CONV+"])",VENDA_BILHETERIA_ITEM_ID+"="+vendaBilheteriaItemID);
                        //|novoItemGrid[VALOR_CONV] = (valorConv!=DBNull.Value) ? Convert.ToDecimal(valorConv) : 0;


                    }


                    //AKI
                    if (pacoteIdAtual != oP.Control.ID)
                    {
                        pacoteGrupo = 0;
                    }


                    if (!lstAssinaturaClienteID.Contains(assinaturaClienteID) && (!oP.PermitirCancelamentoAvulso.Valor || pacoteGrupo != (int)novoInfoPacote[PACOTE_GRUPO]))
                    {

                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                        pacoteIdAtual = oP.Control.ID;
                        pacoteReservaID = (int)novoItemGrid[RESERVAID];
                        pacoteGrupo = (int)novoInfoPacote[PACOTE_GRUPO];

                        if (assinaturaClienteID > 0)
                            lstAssinaturaClienteID.Add(assinaturaClienteID);

                    }

                    if (assinaturaClienteID > 0)
                    {
                        DataRow[] dr = retorno.Tables[TABELA_RESERVA].Select(ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                        if (dr.Length > 0)
                        {
                            int possuiReservaID = (int)dr[0][RESERVAID];
                            if (possuiReservaID > 0)
                                pacoteReservaID = possuiReservaID;
                        }

                        DataRow novoAssinatura = retorno.Tables[TABELA_INFO_ASSINATURA].NewRow();
                        for (int i = 0; i < novoItemGrid.ItemArray.Length; i++)
                        {
                            novoAssinatura[i] = novoItemGrid[i];
                        }

                        novoAssinatura[VALOR] = (valorIndividual != DBNull.Value) ? Convert.ToDecimal(valorIndividual) : 0;
                        novoAssinatura[RESERVAID] = pacoteReservaID;

                        retorno.Tables[TABELA_INFO_ASSINATURA].Rows.Add(novoAssinatura);

                    }

                    for (int i = 0; i < linhas.Length; i++)
                    {
                        DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[RESERVAID] = oP.PermitirCancelamentoAvulso.Valor || assinaturaClienteID > 0 ? pacoteReservaID : (int)novoItemGrid[RESERVAID];
                        novoItemReserva[VENDA_BILHETERIA_ITEM_ID] = vBItemId;
                        novoItemReserva[INGRESSOID] = (int)linhas[i][INGRESSO_ID];
                        novoItemReserva[PACOTEID] = (int)linhas[0][PACOTEID];
                        novoItemReserva[PRECOID] = (int)linhas[i][PRECOID];
                        novoItemReserva[CORTESIAID] = (int)linhas[i][CORTESIAID];
                        novoItemReserva[BLOQUEIOID] = (int)linhas[i][BLOQUEIOID];
                        novoItemReserva[TIPO_CODIGO_BARRA] = linhas[i][TIPO_CODIGO_BARRA];
                        novoItemReserva[CODIGO_BARRA] = linhas[i][CODIGO_BARRA];
                        novoItemReserva[EVENTO_ID] = linhas[i][EVENTO_ID];
                        novoItemReserva[APRESENTACAO_SETOR_ID] = linhas[i][APRESENTACAO_SETOR_ID];
                        novoItemReserva[ASSINATURA_CLIENTE_ID] = linhas[i][ASSINATURA_CLIENTE_ID];

                        retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                    }


                    if (oP.PermitirCancelamentoAvulso.Valor)
                    {
                        novoInfoPacote[RESERVAID] = pacoteReservaID;
                        retorno.Tables[TABELA_INFO_PACOTE].Rows.Add(novoInfoPacote);
                    }

                }

                retorno.Tables.Add(info);

                return retorno;

            }
            catch (CancelamentoGerenciadorException cgEx)
            {
                throw cgEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EstruturaRetornoVendaValeIngresso PesquisarVendaValeIngresso(int vendaID, int perfilID, int caixaID, int canalID, int empresaID, int localID)
        {
            BD bd = new BD();
            try
            {
                //inicializa o retorno
                EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
                retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();

                String sql = string.Empty;
                StringBuilder stbSQL = new StringBuilder();
                //Monta a sql da informações de venda de acordo com o perfil
                switch (perfilID)
                {
                    case Perfil.CANAL_BILHETEIRO:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID, vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaControleID, vb.EntregaAgendaID, u.Nome as Vendedor, vb.DataVenda, ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal  , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE ca.ID = " + caixaID + " ");
                        stbSQL.Append("AND ca.DataFechamento='' AND vb.ID=" + vendaID);
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaControleID, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal  , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE tCanal.ID=" + canalID + " ");
                        stbSQL.Append("AND vb.ID=" + vendaID);
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaControleID, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, vb.Status, u.Nome as Vendedor, tCanal.Nome AS Canal  , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE vb.ID = '" + vendaID + "' ");
                        stbSQL.Append("AND ( vi.CanalID = " + canalID + " OR l.ID = " + localID + " ) ");
                        break;
                    case Perfil.SEGURANCA_ESPECIAL:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaControleID, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal  , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE tCanal.ID=" + canalID + " ");
                        stbSQL.Append("AND vb.ID=" + vendaID);
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaControleID, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal  , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE ");
                        stbSQL.Append("vb.ID=" + vendaID);
                        break;
                    default:
                        throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(stbSQL.ToString());
                decimal valorTotal, taxaEntregaValor;

                //Preenche o objeto de retorno com as informações de venda.
                if (bd.Consulta().Read())
                {
                    valorTotal = bd.LerDecimal("ValorTotal");
                    taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");

                    retorno.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    retorno.ClienteID = bd.LerInt("ClienteID");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        retorno.ClienteNome = bd.LerString("ClienteNome");
                    else
                        retorno.ClienteNome = bd.LerString("NomeFantasia");
                    retorno.ValorTotalEntrega = taxaEntregaValor;
                    retorno.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                    retorno.EntregaControleID = bd.LerInt("EntregaControleID");
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    retorno.DataAberturaCaixa = bd.LerDateTime("CaixaAbertura");
                    retorno.ValorTotalVenda = valorTotal;
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.Senha = bd.LerString("Senha");
                    retorno.EntregaAgendaID = bd.LerInt("EntregaAgendaID");
                }

                bd.FecharConsulta();
                stbSQL = new StringBuilder();

                if (retorno.VendaBilheteriaID != 0)
                {
                    stbSQL.Append("SELECT DISTINCT vi.ID , vit.CodigoTrocaFixo, vi.CodigoTroca, vi.CodigoBarra, vit.SaudacaoNominal, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                    stbSQL.Append("vit.Valor, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status, vit.ValorTipo ");
                    stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                    stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                    stbSQL.Append("INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                    stbSQL.Append("INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = viLog.VendaBilheteriaItemID ");
                    stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                    stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND vi.VendaBilheteriaID = " + retorno.VendaBilheteriaID + " ");
                    stbSQL.Append("AND vi.Status IN ('" + (char)ValeIngresso.enumStatus.Vendido + "', '" + (char)ValeIngresso.enumStatus.Impresso + "')");
                    stbSQL.Append("ORDER BY vi.ID ");

                    bd.Consulta(stbSQL.ToString());
                    EstruturaImpressaoVir impressaoItem;
                    string codigoTrocaFixo = string.Empty;
                    while (bd.Consulta().Read())
                    {
                        impressaoItem = new EstruturaImpressaoVir();

                        string valorTipo = bd.LerString("ValorTipo");

                        if (ValeIngressoTipo.EnumValorTipo.Porcentagem == (ValeIngressoTipo.EnumValorTipo)Convert.ToChar(valorTipo))
                        {
                            impressaoItem.Valor = 0;
                            impressaoItem.Porcentagem = (int)bd.LerDecimal("Valor");
                        }
                        else
                        {
                            impressaoItem.Valor = bd.LerDecimal("Valor");
                            impressaoItem.Porcentagem = 0;
                        }

                        impressaoItem.ValeIngressoID = bd.LerInt("ID");
                        impressaoItem.ValeIngressoTipoID = bd.LerInt("ValeIngressoTipoID");
                        impressaoItem.ValeIngressoNome = bd.LerString("Nome");
                        impressaoItem.CodigoBarra = bd.LerString("CodigoBarra");
                        codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                        impressaoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                        impressaoItem.CodigoTroca = impressaoItem.CodigoTrocaFixo ? codigoTrocaFixo : bd.LerString("CodigoTroca");
                        impressaoItem.Status = (ValeIngresso.enumStatus)Convert.ToChar(bd.LerString("Status"));
                        impressaoItem.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                        impressaoItem.ClientePresenteado = bd.LerString("ClienteNome");
                        impressaoItem.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                        impressaoItem.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                        impressaoItem.ValidadeData = bd.LerDateTime("ValidadeData");
                        impressaoItem.ValidadeEmDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                        retorno.EstruturaImpressaoVir.Add(impressaoItem);
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaRetornoVendaValeIngresso PesquisarCodigoBarrasValeIngresso(string codigoBarra, int perfilID, int caixaID, int canalID, int empresaID, int localID)
        {
            BD bd = new BD();
            try
            {
                EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
                retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                EstruturaImpressaoVir retornoItem;

                String sql = string.Empty;
                StringBuilder stbSQL = new StringBuilder();
                //Monta a sql da informações de venda de acordo com o perfil
                switch (perfilID)
                {
                    case Perfil.CANAL_BILHETEIRO:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID, vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID, u.Nome as Vendedor, vb.DataVenda, ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal  , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE tCaixa.ID = " + caixaID + " ");
                        stbSQL.Append("AND tCaixa.DataFechamento='' AND viLog.CodigoBarra= '" + codigoBarra + "' ");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, u.Nome as Vendedor , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE tCanal.ID=" + canalID + " ");
                        stbSQL.Append("AND viLog.CodigoBarra= '" + codigoBarra + "' ");
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, u.Nome as Vendedor , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID  ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE viLog.CodigoBarra= '" + codigoBarra + "' ");
                        stbSQL.Append("AND ( vi.CanalID = " + canalID + " OR l.ID = " + localID + " ) ");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, u.Nome as Vendedor , c.CNPJ , c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID  ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE viLog.CodigoBarra= '" + codigoBarra + "' ");
                        break;
                    default:
                        throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(stbSQL.ToString());
                decimal valorTotal, taxaEntregaValor;

                //Preenche o objeto de retorno com as informações de venda.
                if (bd.Consulta().Read())
                {
                    valorTotal = bd.LerDecimal("ValorTotal");
                    taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");
                    retorno.EntregaControleID = bd.LerInt("EntregaControleID");
                    retorno.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    retorno.ClienteID = bd.LerInt("ClienteID");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        retorno.ClienteNome = bd.LerString("ClienteNome");
                    else
                        retorno.ClienteNome = bd.LerString("NomeFantasia");
                    retorno.ValorTotalEntrega = taxaEntregaValor;
                    retorno.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    retorno.DataAberturaCaixa = bd.LerDateTime("CaixaAbertura");
                    retorno.ValorTotalVenda = valorTotal;
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.Senha = bd.LerString("Senha");
                    retorno.EntregaAgendaID = bd.LerInt("EntregaAgendaID");
                }
                else
                    throw new ReimpressaoGerenciadorException("Não foram encontrados Vale Ingressos com esse Código de Barra!");

                bd.FecharConsulta();
                stbSQL = new StringBuilder();

                stbSQL.Append("SELECT DISTINCT vi.ID , vit.CodigoTrocaFixo, vi.CodigoTroca, vi.CodigoBarra, vit.SaudacaoNominal, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                stbSQL.Append("         vit.Valor, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status, , vit.ValorTipo ");
                stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = viLog.VendaBilheteriaItemID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND vi.VendaBilheteriaID = " + retorno.VendaBilheteriaID + " ");
                stbSQL.Append("AND vi.Status IN ('" + (char)ValeIngresso.enumStatus.Vendido + "', '" + (char)ValeIngresso.enumStatus.Impresso + "')");
                stbSQL.Append("ORDER BY vi.ID ");
                bd.Consulta(stbSQL.ToString());

                string validadeData = string.Empty;
                string codigoTrocaFixo = string.Empty;
                while (bd.Consulta().Read())
                {
                    retornoItem = new EstruturaImpressaoVir();
                    retornoItem.ValeIngressoID = bd.LerInt("ID");
                    codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    retornoItem.CodigoTroca = codigoTrocaFixo.Length > 0 ? codigoTrocaFixo : bd.LerString("CodigoTroca");
                    retornoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0 ? true : false;
                    retornoItem.CodigoBarra = bd.LerString("CodigoBarra");

                    validadeData = bd.LerString("ValidadeData");
                    if (validadeData.Trim() != string.Empty)
                        retornoItem.ValidadeData = bd.LerDateTime("ValidadeData");

                    else
                    {
                        retornoItem.ValidadeEmDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                        retornoItem.ValidadeData = System.DateTime.Now.AddDays(retornoItem.ValidadeEmDiasImpressao);
                    }
                    retornoItem.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                    retornoItem.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                    retornoItem.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                    retornoItem.ValeIngressoTipoID = bd.LerInt("ValeIngressoTipoID");

                    string valorTipo = bd.LerString("ValorTipo");
                    if (ValeIngressoTipo.EnumValorTipo.Porcentagem == (ValeIngressoTipo.EnumValorTipo)Convert.ToChar(valorTipo))
                    {
                        retornoItem.Valor = 0;
                        retornoItem.Porcentagem = (int)bd.LerDecimal("Valor");
                    }
                    else
                    {
                        retornoItem.Valor = bd.LerDecimal("Valor");
                        retornoItem.Porcentagem = 0;
                    }
                    retornoItem.ValeIngressoNome = bd.LerString("Nome");
                    retornoItem.ClientePresenteado = bd.LerString("ClienteNome");

                    switch (Convert.ToChar(bd.LerString("Status")))
                    {
                        case (char)ValeIngresso.enumStatus.Vendido:
                            retornoItem.Status = ValeIngresso.enumStatus.Vendido;
                            break;
                        case (char)ValeIngresso.enumStatus.Impresso:
                            retornoItem.Status = ValeIngresso.enumStatus.Impresso;
                            break;
                    }
                    retorno.EstruturaImpressaoVir.Add(retornoItem);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaRetornoVendaValeIngresso PesquisarSenhaValeIngresso(string senha, int perfilID, int caixaID, int canalID, int empresaID, int localID)
        {
            BD bd = new BD();
            try
            {
                //inicializa o retorno
                EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
                retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();

                String sql = string.Empty;
                StringBuilder stbSQL = new StringBuilder();
                //Monta a sql da informações de venda de acordo com o perfil
                switch (perfilID)
                {

                    case Perfil.CANAL_BILHETEIRO:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID,vb.ClienteID,c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID,vb.EntregaControleID, u.Nome AS Vendedor, vb.DataVenda, ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, c.CNPJ, c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' AND ca.ID =" + caixaID + " AND ca.DataFechamento=''");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID,vb.EntregaControleID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, u.Nome AS Vendedor, c.CNPJ, c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' AND tCanal.ID =" + canalID);
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.EntregaAgendaID,vb.EntregaControleID, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, u.Nome as Vendedor, c.CNPJ, c.NomeFantasia, ISNULL(TransactionID,'') as TransactionID ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' ");
                        //stbSQL.Append("AND ( vi.CanalID = " + canalID + " OR l.ID = " + localID + " ) ");
                        break;
                    default:
                        throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");
                }
                bd.Consulta(stbSQL.ToString());
                decimal valorTotal, taxaEntregaValor;

                //Preenche o objeto de retorno com as informações de venda.
                if (bd.Consulta().Read())
                {
                    valorTotal = bd.LerDecimal("ValorTotal");
                    taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");
                    retorno.EntregaControleID = bd.LerInt("EntregaControleID");
                    retorno.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    retorno.ClienteID = bd.LerInt("ClienteID");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        retorno.ClienteNome = bd.LerString("ClienteNome");
                    else
                        retorno.ClienteNome = bd.LerString("NomeFantasia");
                    retorno.ValorTotalEntrega = taxaEntregaValor;
                    retorno.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    retorno.DataAberturaCaixa = bd.LerDateTime("CaixaAbertura");
                    retorno.ValorTotalVenda = valorTotal;
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.Senha = senha;
                    retorno.EntregaAgendaID = bd.LerInt("EntregaAgendaID");
                }
                else
                    throw new ReimpressaoGerenciadorException("Não foram encontrados Vale Ingressos com essa senha!");

                bd.FecharConsulta();
                stbSQL = new StringBuilder();

                if (retorno.VendaBilheteriaID != 0)
                {
                    stbSQL.Append("SELECT DISTINCT vi.ID, vit.CodigoTrocaFixo, vi.CodigoTroca, vi.CodigoBarra, vit.SaudacaoNominal, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                    stbSQL.Append("         vit.Valor, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status, vit.ValorTipo ");
                    stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                    stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                    stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = viLog.VendaBilheteriaItemID ");
                    stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND vi.VendaBilheteriaID = " + retorno.VendaBilheteriaID + " ");
                    stbSQL.Append("AND vi.Status IN ('" + (char)ValeIngresso.enumStatus.Vendido + "', '" + (char)ValeIngresso.enumStatus.Impresso + "')");
                    stbSQL.Append("ORDER BY vi.ID ");

                    bd.Consulta(stbSQL.ToString());
                    EstruturaImpressaoVir impressaoItem;
                    string codigoTrocaFixo = string.Empty;
                    while (bd.Consulta().Read())
                    {
                        impressaoItem = new EstruturaImpressaoVir();
                        impressaoItem.ValeIngressoID = bd.LerInt("ID");
                        impressaoItem.ValeIngressoTipoID = bd.LerInt("ValeIngressoTipoID");
                        impressaoItem.ValeIngressoNome = bd.LerString("Nome");
                        impressaoItem.CodigoBarra = bd.LerString("CodigoBarra");
                        codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                        impressaoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                        impressaoItem.CodigoTroca = impressaoItem.CodigoTrocaFixo ? codigoTrocaFixo : bd.LerString("CodigoTroca");
                        impressaoItem.Status = (ValeIngresso.enumStatus)Convert.ToChar(bd.LerString("Status"));
                        impressaoItem.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                        impressaoItem.ClientePresenteado = bd.LerString("ClienteNome");
                        impressaoItem.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                        impressaoItem.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                        impressaoItem.ValidadeData = bd.LerDateTime("ValidadeData");
                        impressaoItem.ValidadeEmDiasImpressao = bd.LerInt("ValidadeDiasImpressao");

                        string valorTipo = bd.LerString("ValorTipo");
                        if (ValeIngressoTipo.EnumValorTipo.Porcentagem == (ValeIngressoTipo.EnumValorTipo)Convert.ToChar(valorTipo))
                        {
                            impressaoItem.Valor = 0;
                            impressaoItem.Porcentagem = (int)bd.LerDecimal("Valor");
                        }
                        else
                        {
                            impressaoItem.Valor = bd.LerDecimal("Valor");
                            impressaoItem.Porcentagem = 0;
                        }
                        retorno.EstruturaImpressaoVir.Add(impressaoItem);
                    }
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

    }

    [Serializable]
    public class CancelamentoGerenciadorException : Exception
    {

        public CancelamentoGerenciadorException() : base() { }

        public CancelamentoGerenciadorException(string msg) : base(msg) { }

        public CancelamentoGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
