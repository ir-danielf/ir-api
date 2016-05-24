using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    /// <summary>
    /// CaixaRelatorios
    /// </summary>
    public class CaixaRelatorios : MarshalByRefObject
    {
        public const string ID = "ID";

        public const string ACAO = "Ação";
        public const string CODIGO = "Cod";
        public const string CONV = "Conv";

        public const string STATUS = "Status";

        public const string CORTESIA = "Cortesia";
        public const string DATAVENDA = "DataVenda";
        public const string ENTREGA = "Entrega";
        public const string EVENTO = "Evento";

        public const string EVENTO_PACOTE = "Evento/Pacote/Vale Ingresso";
        public const string FORMA_PAGAMENTO = "FormaPagamento";
        public const string FORMA_PAGAMENTO_ID = "FormaPagamentoID";

        public const string HORARIO = "Horário";
        public const string MOTIVO = "Motivo";
        public const string PACOTE = "Pacote";
        public const string PACOTE_ID = "PacoteID";
        public const string INGRESSO_ID = "IngressoID";
        public const string PRECO = "Preço";
        public const string QTDE = "Qtd";
        public const string SENHA = "Senha";
        public const string SETOR_PRODUTO = "Setor/Produto";
        public const string TOTAL = "Total";
        public const string VALOR = "Valor";
        public const string VALOR_CONV = "Valor Conv";
        public const string VALE_INGRESSO = "Vale_Ingresso";
        public const string PACOTE_GRUPO = "PacoteGrupo";
        public const string PERMITIR_CANCELAMENTO_AVULSO = "PermitirCancelamentoAvulso";
        public const string ASSINATURA_CLIENTE_ID = "AssinaturaClienteID";
        public const string ASSINATURA_ID = "AssinaturaID";
        public const string ASSINATURA = "Assinatura";

        public const string VENDA_BILHETERIA_ID = "VendaBilheteriaID";
        public const string VENDA_BILHETERIA_ITEM_ID = "VendaBilheteriaItemID";

        //TOTAL
        public const string QTDE_DE_TOTAL = "Ingressos";
        public const string VALOR_CONV_DE_TOTAL = "Conveniência";
        public const string TOTAL_DE_TOTAL = "Total";
        public const string VALOR_DE_TOTAL = "Valor";
        public const string ENTREGA_DE_TOTAL = "Entrega";
        public const string TAXA_PROCESSAMENTO_VALOR = "TaxaProcessamentoValor";

        public const string REPORT_TYPE_RETIRADA = "R";
        public const string REPORT_TYPE_FECHAMENTO = "F";

        public CaixaRelatorios() { }

        private DataSet estruturaDetalhe()
        {

            DataSet ds = new DataSet();

            DataTable principal = new DataTable("Principal");

            principal.Columns.Add(ACAO, typeof(string));
            principal.Columns.Add(SENHA, typeof(string));
            principal.Columns.Add(DATAVENDA, typeof(string));
            principal.Columns.Add(EVENTO_PACOTE, typeof(string));
            principal.Columns.Add(HORARIO, typeof(string));
            principal.Columns.Add(SETOR_PRODUTO, typeof(string));
            principal.Columns.Add(CODIGO, typeof(string));
            principal.Columns.Add(PRECO, typeof(string));
            principal.Columns.Add(CORTESIA, typeof(string));
            principal.Columns.Add(VALOR, typeof(decimal));
            principal.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            principal.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            principal.Columns.Add(ENTREGA, typeof(decimal)).DefaultValue = 0;
            principal.Columns.Add(TOTAL, typeof(decimal));
            principal.Columns.Add(VENDA_BILHETERIA_ID, typeof(int));
            principal.Columns.Add(QTDE, typeof(int)); //nao eh usado na tela
            principal.Columns.Add(VALE_INGRESSO, typeof(bool)).DefaultValue = false;
            principal.Columns.Add(PACOTE_GRUPO, typeof(int));
            principal.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));
            principal.Columns.Add(ASSINATURA_ID, typeof(int));
            principal.Columns.Add(ASSINATURA, typeof(string));
            principal.Columns.Add(PERMITIR_CANCELAMENTO_AVULSO, typeof(bool));
            principal.Columns.Add(TAXA_PROCESSAMENTO_VALOR, typeof(decimal)).DefaultValue = 0;

            DataTable total = new DataTable("Total");

            total.Columns.Add(".", typeof(string));
            total.Columns.Add(QTDE_DE_TOTAL, typeof(int));
            total.Columns.Add(VALOR_DE_TOTAL, typeof(decimal));
            total.Columns.Add(VALOR_CONV_DE_TOTAL, typeof(decimal));
            total.Columns.Add(ENTREGA_DE_TOTAL, typeof(decimal));
            total.Columns.Add(TAXA_PROCESSAMENTO_VALOR, typeof(decimal));
            total.Columns.Add(TOTAL_DE_TOTAL, typeof(decimal));

            DataTable reimpressao = new DataTable("Reimpressao");

            reimpressao.Columns.Add(QTDE, typeof(int));
            reimpressao.Columns.Add(STATUS, typeof(string));
            reimpressao.Columns.Add(SENHA, typeof(string));
            reimpressao.Columns.Add(DATAVENDA, typeof(string));
            reimpressao.Columns.Add(EVENTO, typeof(string));
            reimpressao.Columns.Add(MOTIVO, typeof(string));

            ds.Tables.Add(principal);
            ds.Tables.Add(total);
            ds.Tables.Add(reimpressao);

            return ds;

        }

        /// <summary>
        /// Rotina de captura de informações detalhadas do caixa
        /// </summary>
        /// <param name="caixaID">ID do Caixa</param>
        /// <returns></returns>
        public DataSet Detalhe(int caixaID)
        {

            try
            {

                DataSet buffer = this.estruturaDetalhe();

                BD bd = new BD();
                #region Ingressos
                string sql = @"SELECT LOG.IngressoID, LOG.VendaBilheteriaID, LOG.VendaBilheteriaItemID, tVendaBilheteriaItem.Acao, 
                        tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tEvento.Nome AS Evento, 
                        tApresentacao.Horario, 
                        tSetor.Nome AS Setor, tIngresso.Codigo, 
                        tPreco.Nome AS Preco, tPreco.Valor, 
                        tCortesia.Nome AS Cortesia,
                        tPacote.ID as  PacoteID,
                        tPacote.Nome as  Pacote,
                        tVendaBilheteriaItem.PacoteGrupo, 
                        tPacote.PermitirCancelamentoAvulso,
                        tVendaBilheteriaItem.TaxaConveniencia,
                        tVendaBilheteriaItem.TaxaConvenienciaValor, tVendaBilheteria.TaxaEntregaValor,
                        TaxaProcessamentoCancelada,
                        TaxaProcessamentoValor,
                        LOG.AssinaturaClienteID,
                        tAssinatura.ID as AssinaturaID,
                        tAssinatura.Nome as Assinatura  " +
                    "FROM tIngressolog AS LOG (NOLOCK) " +
                    "INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = LOG.IngressoID " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                    "LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID " +
                    "LEFT JOIN tCortesia (NOLOCK) ON tCortesia.ID = LOG.CortesiaID " +
                    "INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID " +
                    "INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID " +
                    "LEFT JOIN tAssinaturaCliente (NOLOCK) ON tAssinaturaCliente.ID = LOG.AssinaturaClienteID " +
                    "LEFT JOIN tAssinatura (NOLOCK) ON tAssinatura.ID = tAssinaturaCliente.AssinaturaID " +
                    "WHERE " +
                    "  (LOG.Acao = '" + IngressoLog.CANCELAR + "' OR LOG.Acao = '" + IngressoLog.VENDER + "' OR LOG.Acao = '" + IngressoLog.PRE_RESERVA + "') " +
                    "AND " +
                    "  (LOG.CaixaID = " + caixaID + ") " +
                    "ORDER BY tVendaBilheteria.DataVenda,tIngresso.AssinaturaClienteID DESC";

                bd.Consulta(sql);

                DataTable tabela = buffer.Tables["Principal"].Clone();
                tabela.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
                tabela.Columns.Add(PACOTE_ID, typeof(int));
                tabela.Columns.Add(INGRESSO_ID, typeof(int));
                tabela.Columns.Add(PACOTE, typeof(string));
                //tabela.Columns.Add(VALE_INGRESSO, typeof(bool)).DefaultValue = false;
                while (bd.Consulta().Read())
                {

                    DataRow linha = tabela.NewRow();

                    linha[ACAO] = VendaBilheteriaItem.AcaoDescritiva(bd.LerString("Acao"));
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATAVENDA] = bd.LerStringFormatoDataHora("DataVenda");
                    linha[EVENTO_PACOTE] = bd.LerString("Evento");
                    linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                    linha[SETOR_PRODUTO] = bd.LerString("Setor");
                    linha[CODIGO] = bd.LerString("Codigo");
                    linha[PRECO] = bd.LerString("Preco");
                    linha[CORTESIA] = bd.LerString("Cortesia");
                    linha[VALOR] = bd.LerDecimal("Valor");
                    linha[CONV] = bd.LerInt("TaxaConveniencia");
                    linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                    linha[ENTREGA] = bd.LerDecimal("TaxaEntregaValor");
                    linha[PACOTE] = bd.LerString("Pacote");
                    linha[VENDA_BILHETERIA_ID] = bd.LerInt("VendaBilheteriaID");
                    linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");
                    linha[PACOTE_ID] = bd.LerInt("PacoteID");
                    linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                    linha[ASSINATURA_CLIENTE_ID] = bd.LerInt("AssinaturaClienteID");
                    linha[ASSINATURA_ID] = bd.LerInt("AssinaturaID");
                    linha[ASSINATURA] = bd.LerString("Assinatura");
                    linha[PACOTE_GRUPO] = bd.LerInt("PacoteGrupo");
                    linha[PERMITIR_CANCELAMENTO_AVULSO] = bd.LerBoolean("PermitirCancelamentoAvulso");
                    linha[TAXA_PROCESSAMENTO_VALOR] = bd.LerDecimal(TAXA_PROCESSAMENTO_VALOR);

                    tabela.Rows.Add(linha);

                }


                #endregion

                bd.FecharConsulta();

                #region Vale Ingressos

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT l.ValeIngressoID, l.VendaBilheteriaID, l.VendaBilheteriaItemID, tVendaBilheteriaItem.Acao, tVendaBilheteria.Senha, ");
                stbSQL.Append("tVendaBilheteria.DataVenda, tValeIngressoTipo.Nome, tValeIngressoTipo.Valor, tVendaBilheteria.TaxaEntregaValor ");
                stbSQL.Append("     FROM tValeIngressoLog l (NOLOCK)");
                stbSQL.Append("     INNER JOIN tValeIngresso (NOLOCK) ON tValeIngresso.ID = l.ValeIngressoID ");
                stbSQL.Append("     INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngressoTipo.ID = tValeIngresso.ValeIngressoTipoID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteriaItem (NOLOCK) ON l.VendaBilheteriaItemID = tVendaBilheteriaItem.ID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID ");
                stbSQL.Append("WHERE ");
                stbSQL.Append("     (l.Acao = '" + IngressoLog.CANCELAR + "' OR l.Acao = '" + IngressoLog.VENDER + "') ");
                stbSQL.Append("AND ");
                stbSQL.Append("     l.CaixaID = " + caixaID + " ");
                stbSQL.Append("     ORDER BY tVendaBilheteria.DataVenda DESC");

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();

                    linha[ACAO] = VendaBilheteriaItem.AcaoDescritiva(bd.LerString("Acao"));
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATAVENDA] = bd.LerStringFormatoDataHora("DataVenda");
                    linha[EVENTO_PACOTE] = bd.LerString("Nome");
                    linha[HORARIO] = " - ";
                    linha[SETOR_PRODUTO] = " - ";
                    linha[CODIGO] = " - ";
                    linha[PRECO] = " - ";
                    linha[CORTESIA] = " - ";
                    linha[VALOR] = bd.LerDecimal("Valor");
                    linha[CONV] = 0;
                    linha[VALOR_CONV] = 0;
                    linha[ENTREGA] = bd.LerDecimal("TaxaEntregaValor");
                    linha[PACOTE] = " - ";
                    linha[VENDA_BILHETERIA_ID] = bd.LerInt("VendaBilheteriaID");
                    linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");
                    linha[PACOTE_ID] = 0;
                    linha[INGRESSO_ID] = bd.LerInt("ValeIngressoID");
                    linha[VALE_INGRESSO] = true;

                    tabela.Rows.Add(linha);
                }
                #endregion
                bd.Fechar();

                DataTable tabelaTmp = TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);
                int vendaBilheteriaItemID;
                int ingressoID;
                string acao;
                int pAtual = -1;
                int pgAtual = -1;
                int vbAtual = -1;

                Dictionary<int, string> lstAssinaturaCliente = new Dictionary<int, string>();

                foreach (DataRow linha in tabelaTmp.Rows)
                {
                    bool adicionar = true;

                    DataRow itemNovo = buffer.Tables["Principal"].NewRow();

                    vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                    DataRow[] linhaAtual = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                    if (linhaAtual[0][PERMITIR_CANCELAMENTO_AVULSO] != DBNull.Value && (bool)linhaAtual[0][PERMITIR_CANCELAMENTO_AVULSO])
                    {
                        #region Pacote Avulso

                        int PacoteId = (int)linhaAtual[0][PACOTE_ID];
                        int PacoteGrupo = (int)linhaAtual[0][PACOTE_GRUPO];
                        int VendaBilheteriaID = (int)linhaAtual[0][VENDA_BILHETERIA_ID];

                        if (pAtual == PacoteId && pgAtual == PacoteGrupo && vbAtual == VendaBilheteriaID)
                        {
                            adicionar = false;

                        }
                        else
                        {
                            adicionar = true;

                        }

                        pAtual = PacoteId;
                        pgAtual = PacoteGrupo;
                        vbAtual = VendaBilheteriaID;


                        DataRow[] linhas = tabela.Select(PACOTE_ID + "=" + PacoteId + " AND " + PACOTE_GRUPO + "=" + PacoteGrupo + " AND " + VENDA_BILHETERIA_ID + "=" + VendaBilheteriaID);

                        ingressoID = (int)linhas[0][INGRESSO_ID];

                        object valor = tabela.Compute("SUM(" + VALOR + ")", PACOTE_ID + "=" + PacoteId + " AND " + PACOTE_GRUPO + "=" + PacoteGrupo + " AND " + VENDA_BILHETERIA_ID + "=" + VendaBilheteriaID);

                        itemNovo[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;

                        object valorConv = tabela.Compute("SUM( [" + VALOR_CONV + "])", PACOTE_ID + "=" + PacoteId + " AND " + PACOTE_GRUPO + "=" + PacoteGrupo + " AND " + VENDA_BILHETERIA_ID + "=" + VendaBilheteriaID);

                        itemNovo[VALOR_CONV] = (valorConv != DBNull.Value) ? Convert.ToDecimal(valorConv) : 0;

                        if ((int)linhas[0][PACOTE_ID] != 0)
                        {

                            itemNovo[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            if ((string)linhas[0][CODIGO] != null)
                                itemNovo[CODIGO] = (string)linhas[0][CODIGO];

                        }
                        else
                        {

                            itemNovo[EVENTO_PACOTE] = (string)linhas[0][EVENTO_PACOTE];
                            itemNovo[HORARIO] = (string)linhas[0][HORARIO];
                            itemNovo[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            itemNovo[CODIGO] = (string)linhas[0][CODIGO];
                            if ((string)linhas[0][CORTESIA] != "")
                                itemNovo[CORTESIA] = (string)linhas[0][CORTESIA];
                            itemNovo[PRECO] = (string)linhas[0][PRECO];

                        }

                        acao = (string)linhas[0][ACAO];

                        if (!Convert.ToBoolean(linhas[0][VALE_INGRESSO]))
                        {
                            // Verifica se a venda ou o cancelamento foi feito através de pré-reserva
                            if (acao == VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA) || acao == VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO))
                            {
                                sql = "SELECT tIngressoLog.Acao " +
                                "FROM tIngressoLog (NOLOCK) " +
                                "WHERE " +
                                "   (tIngressoLog.IngressoID = " + ingressoID + ") " +
                                "AND " +
                                "   (tIngressoLog.VendaBilheteriaID < " + (int)linhas[0][VENDA_BILHETERIA_ID] + ") " +
                                "ORDER BY tIngressoLog.TimeStamp DESC";

                                bd.Consulta(sql);
                                if (bd.Consulta().Read())
                                {
                                    if (bd.LerString("Acao") == VendaBilheteriaItem.PRE_RESERVA && acao == VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA))
                                        acao = "Vendido Pré-Reserva";

                                    if (bd.LerString("Acao") == VendaBilheteriaItem.PRE_RESERVA && acao == VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO))
                                        acao = "Cancelado Pré-Reserva";

                                }
                            }
                        }
                        itemNovo[ACAO] = acao;

                        itemNovo[SENHA] = (string)linhas[0][SENHA];
                        itemNovo[DATAVENDA] = (string)linhas[0][DATAVENDA];

                        itemNovo[CONV] = (int)linhas[0][CONV];


                        itemNovo[TOTAL] = (decimal)itemNovo[VALOR] + (decimal)itemNovo[VALOR_CONV];
                        itemNovo[VENDA_BILHETERIA_ID] = (int)linhas[0][VENDA_BILHETERIA_ID];

                        itemNovo[QTDE] = linhas.Length;
                        itemNovo[VALE_INGRESSO] = linhas[0][VALE_INGRESSO];

                        #endregion Pacote Avulso
                    }
                    if (linhaAtual[0][ASSINATURA_ID] != DBNull.Value && (int)linhaAtual[0][ASSINATURA_ID] > 0)
                    {
                        #region Assinatura

                        DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                        ingressoID = (int)linhas[0][INGRESSO_ID];
                        int AssinaturaCliente = (int)linhaAtual[0][ASSINATURA_CLIENTE_ID];

                        acao = (string)linhas[0][ACAO];

                        if (lstAssinaturaCliente.ContainsKey(AssinaturaCliente))
                        {
                            if (lstAssinaturaCliente[AssinaturaCliente] == acao)
                                adicionar = false;
                            else
                            {
                                lstAssinaturaCliente.Remove(AssinaturaCliente);
                                lstAssinaturaCliente.Add(AssinaturaCliente, acao);
                                adicionar = true;
                            }
                        }
                        else
                        {
                            lstAssinaturaCliente.Add(AssinaturaCliente, acao);
                            adicionar = true;
                        }

                        object valor = tabela.Compute("SUM(" + VALOR + ")", ASSINATURA_CLIENTE_ID + "=" + AssinaturaCliente + " AND " + ACAO + "='" + acao + "'");

                        itemNovo[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;

                        if ((int)linhas[0][PACOTE_ID] != 0)
                        {

                            itemNovo[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            if ((string)linhas[0][CODIGO] != null)
                                itemNovo[CODIGO] = (string)linhas[0][CODIGO];
                        }
                        else
                        {
                            itemNovo[EVENTO_PACOTE] = (string)linhas[0][ASSINATURA];
                            itemNovo[HORARIO] = "--";
                            itemNovo[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            itemNovo[CODIGO] = (string)linhas[0][CODIGO];
                            if ((string)linhas[0][CORTESIA] != "")
                                itemNovo[CORTESIA] = (string)linhas[0][CORTESIA];
                            itemNovo[PRECO] = (string)linhas[0][PRECO];
                        }

                        itemNovo[ACAO] = acao;

                        itemNovo[SENHA] = (string)linhas[0][SENHA];
                        itemNovo[DATAVENDA] = (string)linhas[0][DATAVENDA];

                        itemNovo[CONV] = (int)linhas[0][CONV];
                        itemNovo[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];

                        itemNovo[TOTAL] = (decimal)itemNovo[VALOR] + (decimal)itemNovo[VALOR_CONV];
                        itemNovo[VENDA_BILHETERIA_ID] = (int)linhas[0][VENDA_BILHETERIA_ID];

                        itemNovo[QTDE] = linhas.Length;
                        itemNovo[VALE_INGRESSO] = linhas[0][VALE_INGRESSO];

                        #endregion Pacote Avulso
                    }
                    else
                    {
                        #region itens venda


                        DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                        ingressoID = (int)linhas[0][INGRESSO_ID];

                        object valor = tabela.Compute("SUM(" + VALOR + ")", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                        itemNovo[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;

                        if ((int)linhas[0][PACOTE_ID] != 0)
                        {

                            itemNovo[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            if ((string)linhas[0][CODIGO] != null)
                                itemNovo[CODIGO] = (string)linhas[0][CODIGO];

                        }
                        else
                        {

                            itemNovo[EVENTO_PACOTE] = (string)linhas[0][EVENTO_PACOTE];
                            itemNovo[HORARIO] = (string)linhas[0][HORARIO];
                            itemNovo[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            itemNovo[CODIGO] = (string)linhas[0][CODIGO];
                            if ((string)linhas[0][CORTESIA] != "")
                                itemNovo[CORTESIA] = (string)linhas[0][CORTESIA];
                            itemNovo[PRECO] = (string)linhas[0][PRECO];

                        }

                        acao = (string)linhas[0][ACAO];

                        itemNovo[ACAO] = acao;

                        itemNovo[SENHA] = (string)linhas[0][SENHA];
                        itemNovo[DATAVENDA] = (string)linhas[0][DATAVENDA];

                        itemNovo[CONV] = (int)linhas[0][CONV];
                        itemNovo[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];

                        itemNovo[TOTAL] = (decimal)itemNovo[VALOR] + (decimal)itemNovo[VALOR_CONV];
                        itemNovo[VENDA_BILHETERIA_ID] = (int)linhas[0][VENDA_BILHETERIA_ID];

                        itemNovo[QTDE] = linhas.Length;
                        itemNovo[VALE_INGRESSO] = linhas[0][VALE_INGRESSO];

                        #endregion itens venda
                    }

                    if (adicionar)
                        buffer.Tables["Principal"].Rows.Add(itemNovo);

                }

                DataTable tabelaTmp2 = TabelaMemoria.Distinct(tabela, new string[] { VENDA_BILHETERIA_ID, ENTREGA, TAXA_PROCESSAMENTO_VALOR });

                foreach (DataRow linhaTmp in tabelaTmp2.Rows)
                {

                    int vendaBilheteriaID = (int)linhaTmp[VENDA_BILHETERIA_ID];
                    decimal entrega = (decimal)linhaTmp[ENTREGA];
                    decimal processamento = Convert.ToDecimal(linhaTmp[TAXA_PROCESSAMENTO_VALOR]);

                    DataRow[] linha = buffer.Tables["Principal"].Select(VENDA_BILHETERIA_ID + "=" + vendaBilheteriaID);

                    if (linha.Length > 0)
                    {
                        linha[0][ENTREGA] = entrega;
                        linha[0][TAXA_PROCESSAMENTO_VALOR] = processamento;
                        linha[0][TOTAL] = (decimal)linha[0][TOTAL] + entrega + processamento;
                    }
                }

                buffer.Tables["Principal"].AcceptChanges();
                //buffer.Tables["Principal"].Columns.Remove(VENDA_BILHETERIA_ID);

                decimal valorCancelados = 0;
                int qtdeCancelados = 0;
                decimal convCancelados = 0;
                decimal entrCancelados = 0;
                decimal processamentoCancelados = 0;

                decimal valorVendidos = 0;
                int qtdeVendidos = 0;
                decimal convVendidos = 0;
                decimal entrVendidos = 0;
                decimal processamentoVendidos = 0;

                decimal valorPreReserva = 0;
                int qtdePreReserva = 0;
                decimal convPreReserva = 0;
                decimal entrPreReserva = 0;

                if (buffer.Tables["Principal"].Rows.Count > 0)
                {

                    object OvalorCancelados = buffer.Tables["Principal"].Compute("SUM(" + VALOR + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO) + "' OR " + ACAO + "='Cancelado Pré-Reserva'");
                    object OqtdeCancelados = buffer.Tables["Principal"].Compute("SUM(" + QTDE + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO) + "' OR " + ACAO + "='Cancelado Pré-Reserva'");
                    object OconvCancelados = buffer.Tables["Principal"].Compute("SUM([" + VALOR_CONV + "])", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO) + "' OR " + ACAO + "='Cancelado Pré-Reserva'");
                    object OentrCancelados = buffer.Tables["Principal"].Compute("SUM(" + ENTREGA + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO) + "' OR " + ACAO + "='Cancelado Pré-Reserva'");
                    object OprocessamentoCancelados = buffer.Tables["Principal"].Compute("SUM(" + TAXA_PROCESSAMENTO_VALOR + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.CANCELAMENTO) + "' OR " + ACAO + "='Cancelado Pré-Reserva'");

                    object OvalorVendidos = buffer.Tables["Principal"].Compute("SUM(" + VALOR + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA) + "' OR " + ACAO + "='Vendido Pré-Reserva'");
                    object OqtdeVendidos = buffer.Tables["Principal"].Compute("SUM(" + QTDE + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA) + "' OR " + ACAO + "='Vendido Pré-Reserva'");
                    object OconvVendidos = buffer.Tables["Principal"].Compute("SUM([" + VALOR_CONV + "])", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA) + "' OR " + ACAO + "='Vendido Pré-Reserva'");
                    object OentrVendidos = buffer.Tables["Principal"].Compute("SUM(" + ENTREGA + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA) + "' OR " + ACAO + "='Vendido Pré-Reserva'");
                    object OprocessamentoVendidos = buffer.Tables["Principal"].Compute("SUM(" + TAXA_PROCESSAMENTO_VALOR + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.VENDA) + "' OR " + ACAO + "='Vendido Pré-Reserva'");

                    object OvalorPreReserva = buffer.Tables["Principal"].Compute("SUM(" + VALOR + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.PRE_RESERVA) + "'");
                    object OqtdePreReserva = buffer.Tables["Principal"].Compute("SUM(" + QTDE + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.PRE_RESERVA) + "'");
                    object OconvPreReserva = buffer.Tables["Principal"].Compute("SUM([" + VALOR_CONV + "])", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.PRE_RESERVA) + "'");
                    object OentrPreReserva = buffer.Tables["Principal"].Compute("SUM(" + ENTREGA + ")", ACAO + "='" + VendaBilheteriaItem.AcaoDescritiva(VendaBilheteriaItem.PRE_RESERVA) + "'");

                    valorCancelados = (OvalorCancelados != DBNull.Value) ? Convert.ToDecimal(OvalorCancelados) : 0;
                    qtdeCancelados = (OqtdeCancelados != DBNull.Value) ? Convert.ToInt32(OqtdeCancelados) : 0;
                    convCancelados = (OconvCancelados != DBNull.Value) ? Convert.ToDecimal(OconvCancelados) : 0;
                    entrCancelados = (OentrCancelados != DBNull.Value) ? Convert.ToDecimal(OentrCancelados) : 0;
                    processamentoCancelados = OprocessamentoCancelados != DBNull.Value ? Convert.ToDecimal(OprocessamentoCancelados) : 0;

                    valorVendidos = (OvalorVendidos != DBNull.Value) ? Convert.ToDecimal(OvalorVendidos) : 0;
                    qtdeVendidos = (OqtdeVendidos != DBNull.Value) ? Convert.ToInt32(OqtdeVendidos) : 0;
                    convVendidos = (OconvVendidos != DBNull.Value) ? Convert.ToDecimal(OconvVendidos) : 0;
                    entrVendidos = (OentrVendidos != DBNull.Value) ? Convert.ToDecimal(OentrVendidos) : 0;
                    processamentoVendidos = OprocessamentoVendidos != DBNull.Value ? Convert.ToDecimal(OprocessamentoVendidos) : 0;

                    valorPreReserva = (OvalorPreReserva != DBNull.Value) ? Convert.ToDecimal(OvalorPreReserva) : 0;
                    qtdePreReserva = (OqtdePreReserva != DBNull.Value) ? Convert.ToInt32(OqtdePreReserva) : 0;
                    convPreReserva = (OconvPreReserva != DBNull.Value) ? Convert.ToDecimal(OconvPreReserva) : 0;
                    entrPreReserva = (OentrPreReserva != DBNull.Value) ? Convert.ToDecimal(OentrPreReserva) : 0;
                }

                buffer.Tables["Principal"].Columns.Remove(QTDE);

                //reimpressos

                #region Ingressos Reimpressos
                sql = "SELECT Count(1) AS Qtde, " +
                    "tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tEvento.Nome AS Evento, cast(tIngressoLog.Obs AS varchar(256)) AS Motivo " +
                    "FROM tIngressoLog (NOLOCK) " +
                    "INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngressoLog.VendaBilheteriaID " +
                    "INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                    "WHERE tIngressoLog.Acao = '" + IngressoLog.REIMPRIMIR + "' AND tIngressoLog.CaixaID = " + caixaID + " " +
                    "GROUP BY tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tEvento.Nome, cast(tIngressoLog.Obs AS varchar(256))";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow linha = buffer.Tables["Reimpressao"].NewRow();
                    linha[QTDE] = bd.LerInt("Qtde");
                    linha[STATUS] = VendaBilheteria.StatusDescritivo(bd.LerString("Status"));
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATAVENDA] = bd.LerStringFormatoDataHora("DataVenda");
                    linha[EVENTO] = bd.LerString("Evento");
                    linha[MOTIVO] = bd.LerString("Motivo");
                    buffer.Tables["Reimpressao"].Rows.Add(linha);

                }

                #endregion
                bd.FecharConsulta();

                #region Vale Ingressos Reimpressos
                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(1) AS Qtde, tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, ");
                stbSQL.Append("tValeIngressoTipo.Nome AS ValeIngresso, cast(tValeIngressoLog.Obs AS varchar(256)) AS Motivo ");
                stbSQL.Append("FROM tValeIngressoLog (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngresso (NOLOCK) ON tValeIngresso.ID = tValeIngressoLog.ValeIngressoID ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngressoTipo.ID = tValeIngresso.ValeIngressoTipoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tValeIngressoLog.VendaBilheteriaID ");
                stbSQL.Append("WHERE tValeIngressoLog.Acao ='" + IngressoLog.REIMPRIMIR + "'  AND tValeIngressoLog.CaixaID = " + caixaID);
                stbSQL.Append(" GROUP BY tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tValeIngressoTipo.Nome, cast(tValeIngressoLog.Obs AS varchar(256))");

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow linha = buffer.Tables["Reimpressao"].NewRow();
                    linha[QTDE] = bd.LerInt("Qtde");
                    linha[STATUS] = VendaBilheteria.StatusDescritivo(bd.LerString("Status"));
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATAVENDA] = bd.LerStringFormatoDataHora("DataVenda");
                    linha[EVENTO] = bd.LerString("ValeIngresso");
                    linha[MOTIVO] = bd.LerString("Motivo");
                    buffer.Tables["Reimpressao"].Rows.Add(linha);
                }

                #endregion

                bd.Fechar();

                DataRow total = buffer.Tables["Total"].NewRow();
                total[0] = "Totais Vendidos";
                total[VALOR_DE_TOTAL] = valorVendidos;
                total[QTDE_DE_TOTAL] = qtdeVendidos;
                total[VALOR_CONV_DE_TOTAL] = convVendidos;
                total[ENTREGA_DE_TOTAL] = entrVendidos;
                total[TAXA_PROCESSAMENTO_VALOR] = processamentoVendidos;
                total[TOTAL_DE_TOTAL] = valorVendidos + convVendidos + entrVendidos + processamentoVendidos;
                buffer.Tables["Total"].Rows.Add(total);

                total = buffer.Tables["Total"].NewRow();
                total[0] = "Totais Cancelados";
                total[VALOR_DE_TOTAL] = valorCancelados;
                total[QTDE_DE_TOTAL] = qtdeCancelados;
                total[VALOR_CONV_DE_TOTAL] = convCancelados;
                total[ENTREGA_DE_TOTAL] = entrCancelados;
                total[TAXA_PROCESSAMENTO_VALOR] = processamentoCancelados;
                total[TOTAL_DE_TOTAL] = valorCancelados + convCancelados + entrCancelados + processamentoCancelados;
                buffer.Tables["Total"].Rows.Add(total);

                total = buffer.Tables["Total"].NewRow();
                total[0] = "Totais Pré-Reservados";
                total[VALOR_DE_TOTAL] = valorPreReserva;
                total[QTDE_DE_TOTAL] = qtdePreReserva;
                total[VALOR_CONV_DE_TOTAL] = convPreReserva;
                total[ENTREGA_DE_TOTAL] = entrPreReserva;
                total[TOTAL_DE_TOTAL] = valorPreReserva + convPreReserva + entrPreReserva;
                buffer.Tables["Total"].Rows.Add(total);

                object Oreimpressao = buffer.Tables["Reimpressao"].Compute("SUM(" + QTDE + ")", "1=1");

                total = buffer.Tables["Total"].NewRow();
                total[0] = "Totais Reimpressos";
                total[VALOR_DE_TOTAL] = DBNull.Value;
                total[QTDE_DE_TOTAL] = (Oreimpressao != DBNull.Value) ? Convert.ToInt32(Oreimpressao) : 0; ;
                total[VALOR_CONV_DE_TOTAL] = DBNull.Value;
                total[ENTREGA_DE_TOTAL] = DBNull.Value;
                total[TOTAL_DE_TOTAL] = DBNull.Value;
                buffer.Tables["Total"].Rows.Add(total);

                total = buffer.Tables["Total"].NewRow();
                total[0] = "Resultado Caixa";
                total[VALOR_DE_TOTAL] = valorVendidos - valorCancelados;
                total[QTDE_DE_TOTAL] = qtdePreReserva + qtdeVendidos - qtdeCancelados;
                total[VALOR_CONV_DE_TOTAL] = convVendidos - convCancelados;
                total[ENTREGA_DE_TOTAL] = entrVendidos - entrCancelados;
                total[TAXA_PROCESSAMENTO_VALOR] = processamentoVendidos - processamentoCancelados;
                total[TOTAL_DE_TOTAL] = (valorVendidos + convVendidos + entrVendidos + processamentoVendidos) - (entrCancelados + valorCancelados + convCancelados + processamentoCancelados);
                buffer.Tables["Total"].Rows.Add(total);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ClientObjects.EstruturaCaixaResumo Resumo(int caixaID)
        {
            ClientObjects.EstruturaCaixaResumo caixaresumo = new IRLib.Paralela.ClientObjects.EstruturaCaixaResumo();
            List<ClientObjects.EstruturaCaixaResumoPrincipal> lista;
            try
            {
                // Captura os detalhes do caixa e totais
                DataSet bufferTemp = this.Detalhe(caixaID);
                DataTable dtPrincipal = bufferTemp.Tables["Principal"];
                DataTable dtTotal = bufferTemp.Tables["Total"];

                #region Estrutura de impressão

                // Estrutura de impressão principal
                caixaresumo.RelatorioPrincipal = new DataTable("Principal");
                caixaresumo.RelatorioPrincipal.Columns.Add(ACAO, typeof(string));
                caixaresumo.RelatorioPrincipal.Columns.Add(FORMA_PAGAMENTO, typeof(string));
                caixaresumo.RelatorioPrincipal.Columns.Add(FORMA_PAGAMENTO_ID, typeof(int));
                caixaresumo.RelatorioPrincipal.Columns.Add(VALOR, typeof(decimal));
                caixaresumo.RelatorioPrincipal.Columns.Add(QTDE, typeof(decimal));
                caixaresumo.RelatorioPrincipal.Columns.Add(VALOR_CONV, typeof(decimal));
                caixaresumo.RelatorioPrincipal.Columns.Add(ENTREGA, typeof(decimal)).DefaultValue = 0;
                caixaresumo.RelatorioPrincipal.Columns.Add(TAXA_PROCESSAMENTO_VALOR, typeof(decimal)).DefaultValue = 0;
                caixaresumo.RelatorioPrincipal.Columns.Add(TOTAL, typeof(decimal));
                caixaresumo.RelatorioPrincipal.Columns.Add(VALE_INGRESSO, typeof(bool)).DefaultValue = false;

                // Estrutura de impressão de totais
                caixaresumo.RelatorioTotais = new DataTable("Total");
                caixaresumo.RelatorioTotais.Columns.Add(".", typeof(string));
                caixaresumo.RelatorioTotais.Columns.Add(QTDE_DE_TOTAL, typeof(decimal));
                caixaresumo.RelatorioTotais.Columns.Add(VALOR_DE_TOTAL, typeof(decimal));
                caixaresumo.RelatorioTotais.Columns.Add(VALOR_CONV_DE_TOTAL, typeof(decimal));
                caixaresumo.RelatorioTotais.Columns.Add(TAXA_PROCESSAMENTO_VALOR, typeof(decimal)).DefaultValue = 0;
                caixaresumo.RelatorioTotais.Columns.Add(ENTREGA_DE_TOTAL, typeof(decimal));
                caixaresumo.RelatorioTotais.Columns.Add(TOTAL_DE_TOTAL, typeof(decimal));

                #endregion

                #region Carga de Formas de Pagamento

                caixaresumo.Principal = new List<IRLib.Paralela.ClientObjects.EstruturaCaixaResumoPrincipal>();

                DataRow[] linhaVendidosValeIngresso = dtPrincipal.Select(ACAO + " = 'Vendido' AND " + VALE_INGRESSO + " = " + true);
                if (linhaVendidosValeIngresso.Length > 0)
                {
                    lista = FormaPagamentoValeIngresso(ref linhaVendidosValeIngresso);
                    foreach (EstruturaCaixaResumoPrincipal item in lista)
                        caixaresumo.Principal.Add(item);
                }

                DataRow[] linhaCanceladosValeIngresso = dtPrincipal.Select(ACAO + " = 'Cancelado' AND " + VALE_INGRESSO + " = " + true);
                if (linhaCanceladosValeIngresso.Length > 0)
                {
                    lista = FormaPagamentoValeIngresso(ref linhaCanceladosValeIngresso);
                    foreach (EstruturaCaixaResumoPrincipal item in lista)
                        caixaresumo.Principal.Add(item);
                }

                // Captura as linhas de ingressos vendidos
                DataRow[] linhaVendidos = dtPrincipal.Select(ACAO + " = 'Vendido' AND " + VALE_INGRESSO + " =" + false);
                if (linhaVendidos.Length > 0)
                {
                    lista = FormaPagamento(ref linhaVendidos);
                    foreach (ClientObjects.EstruturaCaixaResumoPrincipal item in lista)
                    {
                        caixaresumo.Principal.Add(item);
                    }
                }


                // Captura as linhas de ingressos cancelados
                DataRow[] linhaCancelados = dtPrincipal.Select(ACAO + " = 'Cancelado' AND " + VALE_INGRESSO + " =" + false);
                if (linhaCancelados.Length > 0)
                {
                    lista = FormaPagamento(ref linhaCancelados);
                    foreach (ClientObjects.EstruturaCaixaResumoPrincipal item in lista)
                    {
                        caixaresumo.Principal.Add(item);
                    }
                }

                // Captura as linhas de ingressos pré-reservados
                DataRow[] linhaPreReservados = dtPrincipal.Select(ACAO + " = 'Pré-reservado' AND " + VALE_INGRESSO + " =" + false);
                if (linhaPreReservados.Length > 0)
                {
                    lista = FormaPagamento(ref linhaPreReservados);
                    foreach (ClientObjects.EstruturaCaixaResumoPrincipal item in lista)
                    {
                        caixaresumo.Principal.Add(item);
                    }
                }

                // Captura as linhas de ingressos vendidos pré-reserva
                DataRow[] linhaVendidosPreReserva = dtPrincipal.Select(ACAO + " = 'Vendido Pré-Reserva' AND " + VALE_INGRESSO + " =" + false);
                if (linhaVendidosPreReserva.Length > 0)
                {
                    lista = FormaPagamento(ref linhaVendidosPreReserva);
                    foreach (ClientObjects.EstruturaCaixaResumoPrincipal item in lista)
                    {
                        caixaresumo.Principal.Add(item);
                    }
                }

                // Captura as linhas de ingressos cancelados pré-reserva
                DataRow[] linhaCanceladosPreReserva = dtPrincipal.Select(ACAO + " = 'Cancelado Pré-Reserva' AND " + VALE_INGRESSO + " =" + false);
                if (linhaCanceladosPreReserva.Length > 0)
                {
                    lista = FormaPagamento(ref linhaCanceladosPreReserva);
                    foreach (ClientObjects.EstruturaCaixaResumoPrincipal item in lista)
                        caixaresumo.Principal.Add(item);
                }
                #endregion

                #region Carga de Totais

                // Faz o loop no datatable de totais, e alimenta a lista de totais
                caixaresumo.Totais = new List<IRLib.Paralela.ClientObjects.EstruturaCaixaResumoTotais>();
                IRLib.Paralela.ClientObjects.EstruturaCaixaResumoTotais resumototal;
                foreach (DataRow linha in dtTotal.Rows)
                {
                    resumototal = new IRLib.Paralela.ClientObjects.EstruturaCaixaResumoTotais();

                    // Tipos de totais
                    resumototal.Acao = Convert.ToString(linha[0]);

                    // Valor total
                    if (linha[VALOR_DE_TOTAL] != DBNull.Value)
                        resumototal.Valor = Convert.ToDecimal(linha[VALOR_DE_TOTAL]);
                    else
                        resumototal.Valor = 0;

                    // Quantidade de ingressos
                    if (linha[QTDE_DE_TOTAL] != DBNull.Value)
                        resumototal.QuantidadeIngressos = Convert.ToInt32(linha[QTDE_DE_TOTAL]);
                    else
                        resumototal.QuantidadeIngressos = 0;

                    // Valor de conveniência
                    if (linha[VALOR_CONV_DE_TOTAL] != DBNull.Value)
                        resumototal.ValorConveniencia = Convert.ToDecimal(linha[VALOR_CONV_DE_TOTAL]);
                    else
                        resumototal.ValorConveniencia = 0;

                    // Valor de entrega
                    if (linha[ENTREGA_DE_TOTAL] != DBNull.Value)
                        resumototal.ValorEntrega = Convert.ToDecimal(linha[ENTREGA_DE_TOTAL]);
                    else
                        resumototal.ValorEntrega = 0;

                    resumototal.ValorTaxaProcessamento = linha[TAXA_PROCESSAMENTO_VALOR] != DBNull.Value ? Convert.ToDecimal(linha[TAXA_PROCESSAMENTO_VALOR]) : 0;

                    // Valor total geral
                    if (linha[TOTAL_DE_TOTAL] != DBNull.Value)
                        resumototal.ValorTotal = Convert.ToDecimal(linha[TOTAL_DE_TOTAL]);
                    else
                        resumototal.ValorTotal = 0;

                    caixaresumo.Totais.Add(resumototal);
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return caixaresumo;

        }

        private List<ClientObjects.EstruturaCaixaResumoPrincipal> FormaPagamento(ref DataRow[] linhas)
        {
            // Objeto de retorno
            List<ClientObjects.EstruturaCaixaResumoPrincipal> retorno = new List<ClientObjects.EstruturaCaixaResumoPrincipal>();
            ClientObjects.EstruturaCaixaResumoPrincipal item;
            string acao = string.Empty;

            // Conexão
            BD FormaPagamentoBD = null;
            // Lista de VendaBilheteriaID
            List<int> listaVendaBilheteriaID = new List<int>();

            try
            {
                // Cria o objeto de conexão
                FormaPagamentoBD = new BD();

                // Loop nas linhas, para capturar os IDs de venda bilheteria
                foreach (DataRow linha in linhas)
                {
                    if (!listaVendaBilheteriaID.Contains(Convert.ToInt32(linha[VENDA_BILHETERIA_ID])))
                        listaVendaBilheteriaID.Add(Convert.ToInt32(linha[VENDA_BILHETERIA_ID]));

                    acao = Convert.ToString(linha[ACAO]);
                }
                StringBuilder stbSQL = new StringBuilder();
                // Se existir algum ID na lista
                if (listaVendaBilheteriaID.Count > 0)
                {
                    // Consulta ao banco de dados

                    stbSQL.Append("SELECT ");
                    stbSQL.Append("   a.Acao, ");
                    stbSQL.Append("   a.FormaPagamento, ");
                    stbSQL.Append("   a.FormaPagamentoID, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("   	WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.Valor) ");
                    stbSQL.Append("	    ELSE SUM(a.Valor) * -1 ");
                    stbSQL.Append("   END AS Valor, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("	    WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.Quantidade) ");
                    stbSQL.Append("	    ELSE SUM(a.Quantidade) * -1 ");
                    stbSQL.Append("   END AS Quantidade, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("	    WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.Conveniencia) ");
                    stbSQL.Append("	    ELSE SUM(a.Conveniencia) * -1 ");
                    stbSQL.Append("   END AS Conveniencia, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("	    WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(DISTINCT a.Entrega) ");
                    stbSQL.Append("	    ELSE SUM(DISTINCT a.Entrega) * -1 ");
                    stbSQL.Append("   END AS Entrega, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("	    WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.TaxaProcessamentoValor) ");
                    stbSQL.Append("	    ELSE SUM(a.TaxaProcessamentoValor) * -1 ");
                    stbSQL.Append("   END AS TaxaProcessamentoValor ");
                    stbSQL.Append("FROM ");
                    stbSQL.Append("   ( ");
                    stbSQL.Append("       SELECT ");
                    stbSQL.Append("           tIngressoLog.Acao, ");
                    stbSQL.Append("           CASE ");
                    stbSQL.Append("	            WHEN tIngressoLog.CortesiaID > 0 THEN 'Cortesia' ");
                    stbSQL.Append("	            ELSE isnull(tFormaPagamento.Nome, 'Cortesia')  ");
                    stbSQL.Append("           END AS FormaPagamento,  ");
                    stbSQL.Append("           CASE ");
                    stbSQL.Append("	            WHEN tIngressoLog.CortesiaID > 0 THEN 0 ");
                    stbSQL.Append("	            ELSE isnull(tFormaPagamento.ID, 0) ");
                    stbSQL.Append("           END AS FormaPagamentoID, ");
                    stbSQL.Append("           SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float), CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100) AS Valor, ");
                    stbSQL.Append("           CASE ");
                    stbSQL.Append("               WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN SUM(1) ");
                    stbSQL.Append("               ELSE SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100) ");
                    stbSQL.Append("           END AS Quantidade, ");
                    stbSQL.Append("           MIN(tVendaBilheteriaItem.TaxaConvenienciaValor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100) AS Conveniencia ");
                    stbSQL.Append("           ,tVendaBilheteria.TaxaEntregaValor AS Entrega, tVendaBilheteria.TaxaProcessamentoValor ");
                    stbSQL.Append("       FROM tIngressoLog (NOLOCK) ");
                    stbSQL.Append("       INNER JOIN tIngresso (NOLOCK) ON tIngressoLog.IngressoID = tIngresso.ID ");
                    stbSQL.Append("       INNER JOIN tCaixa (NOLOCK) ON tIngressoLog.CaixaID = tCaixa.ID ");
                    stbSQL.Append("       INNER JOIN tVendaBilheteria (NOLOCK) ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID ");
                    stbSQL.Append("       INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID ");
                    stbSQL.Append("       LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID ");
                    stbSQL.Append("       LEFT JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID ");
                    stbSQL.Append("       INNER JOIN tPreco (NOLOCK) ON tIngressoLog.PrecoID = tPreco.ID ");
                    stbSQL.Append("       WHERE ");
                    stbSQL.Append("           tVendaBilheteria.ID IN (" + Utilitario.ArrayToString(listaVendaBilheteriaID.ToArray()) + ") ");
                    stbSQL.Append("       GROUP BY ");
                    stbSQL.Append("           tIngressoLog.VendaBilheteriaItemID, ");
                    stbSQL.Append("           tIngressoLog.Acao, ");
                    stbSQL.Append("           tFormaPagamento.Nome, ");
                    stbSQL.Append("           tFormaPagamento.ID, ");
                    stbSQL.Append("           tIngressoLog.CortesiaID,tVendaBilheteria.TaxaEntregaValor, TaxaProcessamentoValor ");
                    stbSQL.Append("   ) AS a ");
                    stbSQL.Append("   WHERE ");
                    stbSQL.Append("       (a.Acao = '" + IngressoLog.CANCELAR + "' OR a.Acao = '" + IngressoLog.VENDER + "') ");
                    stbSQL.Append("GROUP BY ");
                    stbSQL.Append("   a.Acao, ");
                    stbSQL.Append("   a.FormaPagamento, ");
                    stbSQL.Append("   a.FormaPagamentoID ");
                    stbSQL.Append("ORDER BY ");
                    stbSQL.Append("  a.FormaPagamento");

                    FormaPagamentoBD.Consulta(stbSQL.ToString());
                    while (FormaPagamentoBD.Consulta().Read())
                    {
                        item = new IRLib.Paralela.ClientObjects.EstruturaCaixaResumoPrincipal();

                        item.Acao = acao;
                        item.FormaPagamento = FormaPagamentoBD.LerString("FormaPagamento");
                        item.FormaPagamentoID = FormaPagamentoBD.LerInt("FormaPagamentoID");
                        item.Valor = FormaPagamentoBD.LerDecimal("Valor");
                        item.Quantidade = FormaPagamentoBD.LerInt("Quantidade");
                        item.ValorConveniencia = FormaPagamentoBD.LerDecimal("Conveniencia");
                        item.ValorEntrega = FormaPagamentoBD.LerDecimal("Entrega");
                        item.ValorTotal = FormaPagamentoBD.LerDecimal("Valor") + FormaPagamentoBD.LerDecimal("Conveniencia") + FormaPagamentoBD.LerDecimal("Entrega") + FormaPagamentoBD.LerDecimal("TaxaProcessamentoValor");
                        item.ValorTaxaProcessamento = FormaPagamentoBD.LerDecimal("TaxaProcessamentoValor");

                        retorno.Add(item);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                // Fecha a conexão
                FormaPagamentoBD.Fechar();
            }

            return retorno;
        }

        private List<ClientObjects.EstruturaCaixaResumoPrincipal> FormaPagamentoValeIngresso(ref DataRow[] linhas)
        {
            List<ClientObjects.EstruturaCaixaResumoPrincipal> retorno = new List<ClientObjects.EstruturaCaixaResumoPrincipal>();
            ClientObjects.EstruturaCaixaResumoPrincipal item;
            string acao = string.Empty;

            // Conexão
            BD FormaPagamentoBD = null;
            // Lista de VendaBilheteriaID
            List<int> listaVendaBilheteriaID = new List<int>();

            try
            {// Cria o objeto de conexão
                FormaPagamentoBD = new BD();

                // Loop nas linhas, para capturar os IDs de venda bilheteria
                foreach (DataRow linha in linhas)
                {
                    if (!listaVendaBilheteriaID.Contains(Convert.ToInt32(linha[VENDA_BILHETERIA_ID])))
                        listaVendaBilheteriaID.Add(Convert.ToInt32(linha[VENDA_BILHETERIA_ID]));

                    acao = Convert.ToString(linha[ACAO]);
                }
                StringBuilder stbSQL = new StringBuilder();
                // Se existir algum ID na lista
                if (listaVendaBilheteriaID.Count > 0)
                {
                    // Consulta ao banco de dados

                    stbSQL.Append("SELECT ");
                    stbSQL.Append("   a.Acao, ");
                    stbSQL.Append("   a.FormaPagamento, ");
                    stbSQL.Append("   a.FormaPagamentoID, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("   	WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.Valor) ");
                    stbSQL.Append("	    ELSE SUM(a.Valor) * -1 ");
                    stbSQL.Append("   END AS Valor, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("	    WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.Quantidade) ");
                    stbSQL.Append("	    ELSE SUM(a.Quantidade) * -1 ");
                    stbSQL.Append("   END AS Quantidade, ");
                    stbSQL.Append("   CASE ");
                    stbSQL.Append("	    WHEN a.Acao = '" + IngressoLog.VENDER + "' THEN SUM(a.Conveniencia) ");
                    stbSQL.Append("	    ELSE SUM(a.Conveniencia) * -1 ");
                    stbSQL.Append("   END AS Conveniencia ");
                    stbSQL.Append("FROM ");
                    stbSQL.Append("   ( ");
                    stbSQL.Append("       SELECT ");
                    stbSQL.Append("           tValeIngressoLog.Acao, ");
                    //stbSQL.Append("           CASE ");
                    //stbSQL.Append("	            WHEN tIngressoLog.CortesiaID > 0 THEN 'Cortesia' ");
                    //stbSQL.Append("	            ELSE isnull(tFormaPagamento.Nome, 'Cortesia')  ");
                    //stbSQL.Append("           END AS FormaPagamento,  ");
                    //stbSQL.Append("           CASE ");
                    //stbSQL.Append("	            WHEN tIngressoLog.CortesiaID > 0 THEN 0 ");
                    //stbSQL.Append("	            ELSE isnull(tFormaPagamento.ID, 0) ");
                    //stbSQL.Append("           END AS FormaPagamentoID, ");
                    stbSQL.Append("               tFormaPagamento.Nome AS FormaPagamento, ");
                    stbSQL.Append("               tFormaPagamento.ID AS FormaPagamentoID, ");
                    stbSQL.Append("           SUM(tValeIngressoTipo.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float), CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100) AS Valor, ");
                    stbSQL.Append("           CASE ");
                    stbSQL.Append("               WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN SUM(1) ");
                    stbSQL.Append("               ELSE SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100) ");
                    stbSQL.Append("           END AS Quantidade, ");
                    stbSQL.Append("           MIN(tVendaBilheteriaItem.TaxaConvenienciaValor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100) AS Conveniencia ");
                    stbSQL.Append("       FROM tValeIngressoLog (NOLOCK) ");
                    stbSQL.Append("       INNER JOIN tValeIngresso (NOLOCK) ON tValeIngressoLog.ValeIngressoID = tValeIngresso.ID ");
                    stbSQL.Append("       INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngresso.ValeIngressoTipoID = tValeIngressoTipo.ID ");
                    stbSQL.Append("       INNER JOIN tCaixa (NOLOCK) ON tValeIngressoLog.CaixaID = tCaixa.ID ");
                    stbSQL.Append("       INNER JOIN tVendaBilheteria (NOLOCK) ON tValeIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID ");
                    stbSQL.Append("       INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tValeIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID ");
                    stbSQL.Append("       LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID ");
                    stbSQL.Append("       LEFT JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID ");
                    //stbSQL.Append("       INNER JOIN tPreco (NOLOCK) ON tIngressoLog.PrecoID = tPreco.ID ");
                    stbSQL.Append("       WHERE ");
                    stbSQL.Append("           tVendaBilheteria.ID IN (" + Utilitario.ArrayToString(listaVendaBilheteriaID.ToArray()) + ") ");
                    stbSQL.Append("       GROUP BY ");
                    stbSQL.Append("           tValeIngressoLog.VendaBilheteriaItemID, ");
                    stbSQL.Append("           tValeIngressoLog.Acao, ");
                    stbSQL.Append("           tFormaPagamento.Nome, ");
                    stbSQL.Append("           tFormaPagamento.ID ");
                    //stbSQL.Append("           tValeIngressoLog.CortesiaID ");
                    stbSQL.Append("   ) AS a ");
                    stbSQL.Append("   WHERE ");
                    stbSQL.Append("       (a.Acao = '" + IngressoLog.CANCELAR + "' OR a.Acao = '" + IngressoLog.VENDER + "') ");
                    stbSQL.Append("GROUP BY ");
                    stbSQL.Append("   a.Acao, ");
                    stbSQL.Append("   a.FormaPagamento, ");
                    stbSQL.Append("   a.FormaPagamentoID ");
                    stbSQL.Append("ORDER BY ");
                    stbSQL.Append("  a.FormaPagamento");

                    FormaPagamentoBD.Consulta(stbSQL.ToString());
                    while (FormaPagamentoBD.Consulta().Read())
                    {
                        item = new IRLib.Paralela.ClientObjects.EstruturaCaixaResumoPrincipal();

                        item.Acao = acao + " - Vale Ingresso ";
                        item.FormaPagamento = FormaPagamentoBD.LerString("FormaPagamento");
                        item.FormaPagamentoID = FormaPagamentoBD.LerInt("FormaPagamentoID");
                        item.Valor = FormaPagamentoBD.LerDecimal("Valor");
                        item.Quantidade = FormaPagamentoBD.LerInt("Quantidade");
                        item.ValorConveniencia = FormaPagamentoBD.LerDecimal("Conveniencia");
                        item.ValorTotal = FormaPagamentoBD.LerDecimal("Valor") + FormaPagamentoBD.LerDecimal("Conveniencia");

                        retorno.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Fecha a conexão
                FormaPagamentoBD.Fechar();
            }
            return retorno;
        }

        public List<EstruturaCaixaPos> ResumoCaixaPos(string posnumber, string dataInicio, string dataFinal)
        {
            BD bd = new BD();

            try
            {
                EstruturaEstadoCidadeSenhaPos estrutura = new Loja().VerificarExistenciaPOS(posnumber);
                int CaixaID = BilheteriaParalela.VerificaCaixaPos(estrutura.UsuarioID, estrutura.LojaID);

                List<EstruturaCaixaPos> CaixaVenda = new List<EstruturaCaixaPos>();
                List<EstruturaCaixaPos> CaixaCancelamento = new List<EstruturaCaixaPos>();

                string sqlVenda = string.Empty;
                string sqlCancelamento = string.Empty;
                string sqlComplemento = string.Empty;


                if (string.IsNullOrEmpty(dataInicio) && string.IsNullOrEmpty(dataFinal))
                    sqlComplemento = string.Format("AND tIngressoLog.UsuarioID = {0} AND tCaixa.DataAbertura > '{1}%' AND tCaixa.LojaID = {2}", estrutura.UsuarioID, DateTime.Now.AddDays(-30).ToString("yyyyMMdd"), estrutura.LojaID);
                else
                    sqlComplemento = string.Format("AND tIngressoLog.UsuarioID = {0} AND (tCaixa.DataAbertura > '{1}%' AND tCaixa.DataFechamento < '{2}%') AND tCaixa.LojaID = {3}", estrutura.UsuarioID, dataInicio, dataFinal, estrutura.LojaID);


                sqlVenda = string.Format(@"SELECT tIngresso.EventoID, tEvento.Nome, tCaixa.ID AS CaixaID, tCaixa.DataAbertura, SUM(tPreco.Valor) as Total
                FROM tCaixa (NOLOCK)
                INNER JOIN tIngressoLog (nolock) on tCaixa.ID = tIngressoLog.CaixaID
                INNER JOIN tIngresso (nolock) on tIngressoLog.IngressoID = tIngresso.ID
                INNER JOIN tEvento (nolock) on tIngresso.EventoID = tEvento.ID
                INNER JOIN tPreco (nolock) ON tIngressoLog.PrecoID = tPreco.ID 
                WHERE tIngressoLog.Acao='{0}' {1}
                GROUP BY tIngresso.EventoID, tEvento.Nome, tCaixa.DataAbertura, tCaixa.ID
				ORDER BY tCaixa.ID DESC", IngressoLog.VENDER, sqlComplemento);

                sqlCancelamento = string.Format(@"SELECT tIngresso.EventoID, tCaixa.ID AS CaixaID, SUM(tPreco.Valor) as Total
                FROM tCaixa (NOLOCK)
                INNER JOIN tIngressoLog (nolock) on tCaixa.ID = tIngressoLog.CaixaID
                INNER JOIN tIngresso (nolock) on tIngressoLog.IngressoID = tIngresso.ID
                INNER JOIN tEvento (nolock) on tIngresso.EventoID = tEvento.ID
                INNER JOIN tPreco (nolock) ON tIngressoLog.PrecoID = tPreco.ID 
                WHERE tIngressoLog.Acao='{0}' {1}
                GROUP BY tIngresso.EventoID, tEvento.Nome, tCaixa.DataAbertura, tCaixa.ID
                ORDER BY tCaixa.ID DESC", IngressoLog.CANCELAR, sqlComplemento);

                bd.Consulta(sqlVenda);

                while (bd.Consulta().Read())
                {
                    CaixaVenda.Add(new EstruturaCaixaPos()
                    {
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Nome"),
                        CaixaID = bd.LerInt("CaixaID"),
                        DataAberturaCaixa = (DateTime.ParseExact(bd.LerString("DataAbertura"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd/MM"),
                        ValorTotal = bd.LerDecimal("Total"),
                    });
                }

                bd.FecharConsulta();

                bd.Consulta(sqlCancelamento);

                while (bd.Consulta().Read())
                {
                    CaixaCancelamento.Add(new EstruturaCaixaPos()
                    {
                        EventoID = bd.LerInt("EventoID"),
                        CaixaID = bd.LerInt("CaixaID"),
                        ValorTotal = bd.LerDecimal("Total")
                    });
                }

                foreach (EstruturaCaixaPos itemcancelado in CaixaCancelamento)
                    foreach (EstruturaCaixaPos itemvendido in CaixaVenda)
                        if (itemcancelado.EventoID == itemvendido.EventoID && itemcancelado.CaixaID == itemvendido.CaixaID)
                            itemvendido.ValorTotal -= itemcancelado.ValorTotal;

                return CaixaVenda;
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

        public List<EstruturaCaixaPos> ResumoRetiradaPos(string posnumber)
        {
            BD bd = new BD();

            try
            {
                EstruturaEstadoCidadeSenhaPos estrutura = new Loja().VerificarExistenciaPOS(posnumber);
                int CaixaID = BilheteriaParalela.VerificaCaixaPos(estrutura.UsuarioID, estrutura.LojaID);

                List<EstruturaCaixaPos> CaixaVenda = new List<EstruturaCaixaPos>();

                string sqlVenda = string.Empty;

                sqlVenda = string.Format(@"SELECT tSangria.EventoID, tEvento.Nome, tCaixa.ID AS CaixaID, tCaixa.DataAbertura, tVendaBilheteria.ValorTotal AS Total
                FROM tSangria (nolock)
                INNER JOIN tVendaBilheteria (nolock) ON tSangria.VendaBilheteriaID = tVendaBilheteria.ID
                INNER JOIN tCaixa (nolock) ON tVendaBilheteria.CaixaID = tCaixa.ID
                INNER JOIN tEvento (nolock) ON tSangria.EventoID = tEvento.ID
                WHERE tCaixa.ID = {0}", CaixaID);

                bd.Consulta(sqlVenda);

                while (bd.Consulta().Read())
                {
                    CaixaVenda.Add(new EstruturaCaixaPos()
                    {
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Nome"),
                        CaixaID = bd.LerInt("CaixaID"),
                        DataAberturaCaixa = (DateTime.ParseExact(bd.LerString("DataAbertura"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd/MM"),
                        ValorTotal = bd.LerDecimal("Total"),
                    });
                }

                return CaixaVenda;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    [Serializable]
    public class CaixaRelatoriosException : Exception
    {
        public CaixaRelatoriosException() : base() { }

        public CaixaRelatoriosException(string msg) : base(msg) { }

        public CaixaRelatoriosException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}