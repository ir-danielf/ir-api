using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    /// <summary>
    /// Gerenciador do ReimpressaoGerenciador
    /// </summary>
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ReimpressaoGerenciador : MarshalByRefObject
    {

        public const string TABELA_GRID = "Grid";
        public const string TABELA_ESTRUTURA_IMPRESSAO = "EstruturaImpressao";

        public const string EVENTO = "Evento";
        public const string SETOR_PRODUTO = "Setor/Produto";
        public const string PRECO = "Preço";
        public const string CODIGO = "Cod";
        public const string HORARIO = "Horário";
        public const string CORTESIA = "Cortesia";
        public const string VALOR = "Valor";

        public const string STATUS = "Status";
        public const string STATUS_DETALHADO = "StatusDetalhado";
        //public const string ACAO = "Ação";
        public const string COMPROVANTE_QUANTIDADE = "ComprovanteQuantidade";
        public const string CLIENTE = "Cliente";
        public const string CANAL = "Canal";
        public const string INFO_VENDA = "InfoVenda";
        public const string VENDEDOR = "Vendedor";
        public const string DATA_VENDA = "DataVenda";
        public const string DATA_CAIXA = "DataCaixa";
        public const string SENHA = "Senha";
        public const string STATUS_VENDA = "StatusVenda";
        public const string INGRESSO_ID = "IngressoID";
        public const string VENDA_ID = "VendaBilheteriaID";
        public const string LOG_ID = "LogID";
        public const string VALOR_TOTAL = "ValorTotal";
        public const string TIPO_VENDA = "TipoVenda";
        public const string CLIENTE_STATUS = "ClienteStatus";
        public const string OBS = "Obs";
        public const string VALORSEGURO = "ValorSeguro";
        //private int canalID;
        //private int usuarioID;
        //private int perfilID;

        //private string login;

        //private string canaisLista; //para perfil de supervisor

        public ReimpressaoGerenciador()
        {

            //canalID = 0;
            //usuarioID = 0;
            //perfilID = 0;

            //login = "";

        }

        //public int UsuarioID
        //{
        //    set { usuarioID = value; }
        //}

        //public int CanalID
        //{
        //    set { canalID = value; }
        //}

        ////public string CanaisLista{
        ////	set{ canaisLista = value; }
        ////}

        //public int PerfilID
        //{
        //    set { perfilID = value; }
        //}

        //public string Login
        //{
        //    set { login = value; }
        //}

        #region estruturas
        public static DataTable EstruturaGrid()
        {

            DataTable tGrid = new DataTable(TABELA_GRID);

            tGrid.Columns.Add(INGRESSO_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(LOG_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(EVENTO, typeof(string));
            tGrid.Columns.Add(HORARIO, typeof(string));
            tGrid.Columns.Add(SETOR_PRODUTO, typeof(string));
            tGrid.Columns.Add(CODIGO, typeof(string));
            tGrid.Columns.Add(PRECO, typeof(string));
            tGrid.Columns.Add(CORTESIA, typeof(string));
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(STATUS, typeof(string));
            tGrid.Columns.Add(STATUS_DETALHADO, typeof(string));
            tGrid.Columns.Add(TIPO_VENDA, typeof(string));
            //tGrid.Columns.Add(ACAO, typeof(string));
            //tGrid.Columns.Add(ReimpressaoGerenciador.CONV, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(ReimpressaoGerenciador.VALOR_CONV, typeof(decimal)).DefaultValue = 0;

            return tGrid;

        }

        public static DataTable EstruturaInfoVenda()
        {

            DataTable tGrid = new DataTable(INFO_VENDA);

            tGrid.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));
            tGrid.Columns.Add("TaxaEntregaValor", typeof(decimal));
            tGrid.Columns.Add(VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(STATUS_VENDA, typeof(string));
            tGrid.Columns.Add(CLIENTE, typeof(string));
            tGrid.Columns.Add(CANAL, typeof(string));
            tGrid.Columns.Add(COMPROVANTE_QUANTIDADE, typeof(int));
            tGrid.Columns.Add(VENDEDOR, typeof(string));
            tGrid.Columns.Add(SENHA, typeof(string));
            tGrid.Columns.Add(DATA_VENDA, typeof(DateTime));
            tGrid.Columns.Add(DATA_CAIXA, typeof(DateTime));
            tGrid.Columns.Add(CLIENTE_STATUS, typeof(string)).DefaultValue = string.Empty;
            tGrid.Columns.Add(VALORSEGURO, typeof(decimal)).DefaultValue = 0;
            return tGrid;

        }
        #endregion

        /// <summary>
        /// Pesquisa pelo codigo de barras
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarCodigoBarras(string codigoBarra, int perfilID, int caixaID, int canalID, string login, int empresaID, int localID, int lojaID)
        {

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "(CodigoBarra='" + codigoBarra + "' OR CodigoBarraCliente='" + codigoBarra + "')";
                ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
                ingressoLista.Carregar();

                if (ingressoLista.Tamanho > 0)
                {
                    ingressoLista.Primeiro();

                    string sql;

                    if (perfilID == Perfil.CANAL_BILHETEIRO)
                    {
                        string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro ");
                        sb.Append("FROM tIngressoLog (NOLOCK) ");
                        sb.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario (NOLOCK) u ON  tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("WHERE tLoja.CanalID=tCanal.ID AND tCaixa.ID =" + caixaID + " ");
                        sb.Append("AND tCaixa.DataFechamento='' AND tIngressoLog.Acao='" + IngressoLog.VENDER + "' ");
                        sb.Append("AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else if (perfilID == Perfil.CANAL_SUPERVISOR)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                        sb.Append("FROM tIngressoLog (NOLOCK) ");
                        sb.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario u (NOLOCK) ON  tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("WHERE tLoja.CanalID=tCanal.ID AND tCanal.ID=" + canalID + "");
                        sb.Append("AND tIngressoLog.Acao='" + IngressoLog.VENDER + "' ");
                        sb.Append("AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else if (perfilID == Perfil.LOCAL_SUPERVISOR)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                        sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                        sb.Append("WHERE tIngressoLog.Acao='" + IngressoLog.VENDER + "' ");
                        sb.Append("AND (( tCanal.ID = " + canalID + ") OR ( tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " )) ");
                        sb.Append("AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else if (perfilID == Perfil.SAC_SUPERVISOR ||
                             perfilID == Perfil.SAC_OPERADOR || 
                             perfilID == Perfil.SAC_SUPERVISOR_NOVO ||
                             perfilID == Perfil.SAC_OPERADOR_NOVO)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                        sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                        sb.Append("WHERE tIngressoLog.Acao='" + IngressoLog.VENDER + "' ");
                        sb.Append("AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else
                        throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");

                    BD bd = new BD();

                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {
                        DataRow linhaInfo = info.NewRow();

                        linhaInfo["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linhaInfo["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                        linhaInfo[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                        linhaInfo[STATUS_VENDA] = bd.LerString("Status");

                        if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                            linhaInfo[CLIENTE] = bd.LerString("Cliente");
                        else
                            linhaInfo[CLIENTE] = bd.LerString("NomeFantasia");

                        linhaInfo[CANAL] = bd.LerString("Canal");
                        linhaInfo[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");
                        linhaInfo[VENDEDOR] = bd.LerString("Vendedor");
                        linhaInfo[SENHA] = bd.LerString("Senha");
                        linhaInfo[DATA_VENDA] = bd.LerDateTime("DataVenda");
                        linhaInfo[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                        linhaInfo[CLIENTE_STATUS] = bd.LerString("ClienteStatus");
                        linhaInfo[VALORSEGURO] = bd.LerDecimal("ValorSeguro");

                        info.Rows.Add(linhaInfo);

                        sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + ingressoLista.Ingresso.VendaBilheteriaID + ", " + ingressoLista.Ingresso.Control.ID + ", " + lojaID;

                        bd.Consulta(sql);

                        if (bd.Consulta().Read())
                        {
                            if (bd.LerInt("VendaBilheteriaID") == ingressoLista.Ingresso.VendaBilheteriaID.Valor)
                            {

                                DataRow linha = tabelaImpressao.NewRow();

                                linha["ID"] = bd.LerInt("ID");
                                linha["IngressoID"] = bd.LerInt("IngressoID");
                                linha["Usuario"] = login;
                                linha["Evento"] = bd.LerString("Evento");
                                linha["Pacote"] = bd.LerString("Pacote");
                                linha["EventoID"] = bd.LerInt("EventoID");
                                linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
                                linha["PacoteID"] = bd.LerInt("PacoteID");
                                linha["Horario"] = bd.LerDateTime("Horario");
                                linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                                linha["DataVendaString"] = bd.LerStringFormatoDataHora("DataVenda");
                                linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                                linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
                                linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
                                linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                linha["Setor"] = bd.LerString("Setor");
                                linha["Acesso"] = bd.LerString("Acesso");
                                linha["Produto"] = bd.LerBoolean("Produto");
                                linha["Cortesia"] = bd.LerString("Cortesia");
                                linha["Preco"] = bd.LerString("Preco");
                                linha["Valor"] = bd.LerDecimal("Valor");
                                linha["Codigo"] = bd.LerString("Codigo");
                                linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
                                linha["Loja"] = bd.LerString("Loja");
                                linha["Senha"] = bd.LerString("Senha");
                                linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                                linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                                linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                                linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                                linha["Cliente"] = bd.LerString("Cliente");
                                //linha["ClienteRG"] = bd.LerString("ClienteRG");
                                linha["ClienteCPF"] = bd.LerString("ClienteCPF");
                                linha["ClienteID"] = bd.LerInt("ClienteID");
                                linha["ClienteEnderecoID"] = bd.LerInt("ClienteEnderecoID");
                                linha["Status"] = bd.LerString("Status");
                                linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
                                linha["LocalNome"] = bd.LerString("LocalNome");
                                linha["LocalEndereco"] = bd.LerString("Logradouro") + ", " + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                                linha["LocalPais"] = bd.LerString("Pais");
                                linha["LocalEstado"] = bd.LerString("LocalEstado");
                                linha["LocalCidade"] = bd.LerString("LocalCidade");
                                linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                                linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                                linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                                linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
                                linha["TipoVenda"] = bd.LerString("TipoVenda");
                                linha["TipoImpressao"] = bd.LerString("TipoImpressao");
                                linha["NomeCartao"] = bd.LerString("NomeCartao");

                                linha["Alvara"] = bd.LerString("Alvara");
                                linha["FonteImposto"] = bd.LerString("FonteImposto");
                                linha["AVCB"] = bd.LerString("AVCB");
                                linha["PorcentagemImposto"] = bd.LerDecimal("PorcentagemImposto");

                                linha["Lotacao"] = bd.LerDecimal("Lotacao");
                                linha["DataEmissaoAlvara"] = bd.LerStringFormatoData("DataEmissaoAlvara");
                                linha["DataValidadeAlvara"] = bd.LerStringFormatoData("DataValidadeAlvara");
                                linha["DataEmissaoAvcb"] = bd.LerStringFormatoData("DataEmissaoAvcb");
                                linha["DataValidadeAvcb"] = bd.LerStringFormatoData("DataValidadeAvcb");

                                linha["ApresentacaoCancelada"] = bd.LerString("ApresentacaoCancelada");

                                tabelaImpressao.Rows.Add(linha);

                                DataRow linhaGrid = grid.NewRow();

                                linhaGrid[EVENTO] = (string)linha["Evento"];
                                linhaGrid[HORARIO] = (DateTime)linha["Horario"];
                                linhaGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                                if ((string)linha["Cortesia"] != "")
                                    linhaGrid[CORTESIA] = (string)linha["Cortesia"];
                                linhaGrid[PRECO] = (string)linha["Preco"];
                                linhaGrid[VALOR] = (decimal)linha["Valor"];
                                linhaGrid[CODIGO] = (string)linha["Codigo"];
                                linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));

                                if ((string)linha["ApresentacaoCancelada"] == "T")
                                    linhaGrid[STATUS_DETALHADO] = "Apresentação cancelada";

                                linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                                linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                                linhaGrid[LOG_ID] = (int)linha["ID"];
                                linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];

                                grid.Rows.Add(linhaGrid);

                            }
                        }
                    }
                    bd.Fechar();
                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
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
        public DataSet PesquisarCodigoIngresso(int apresentacaoSetorID, string codigo, int perfilID, int caixaID, int canalID, string login, int empresaID, int localID, int lojaID)
        {

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "ApresentacaoSetorID=" + apresentacaoSetorID;
                ingressoLista.FiltroSQL = "Codigo='" + codigo + "'";
                ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
                ingressoLista.Carregar();

                if (ingressoLista.Tamanho == 1)
                {

                    ingressoLista.Primeiro();

                    string sql;

                    if (perfilID == Perfil.CANAL_BILHETEIRO)
                    {

                        string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus  ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                        sb.Append("FROM tIngressoLog (NOLOCK) ");
                        sb.Append("INNER JOIN tVendaBilheteria (NOLOCK) vb ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                        sb.Append("LEFT JOIN  tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                        sb.Append("WHERE tCaixa.ID=" + caixaID + " ");
                        sb.Append("AND tCaixa.DataFechamento='' AND tIngressoLog.Acao='" + IngressoLog.VENDER + "' ");
                        sb.Append("AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ");
                        sb.Append("ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else if (perfilID == Perfil.CANAL_SUPERVISOR)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                        sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                        sb.Append("WHERE tCanal.ID=" + canalID + " AND tIngressoLog.Acao='" + IngressoLog.VENDER + "' ");
                        sb.Append("AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ");
                        sb.Append("ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else if (perfilID == Perfil.LOCAL_SUPERVISOR)
                    {

                        //TODO: Corrigir permissao
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro ");
                        sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON  vb.ClienteID=c.ID ");
                        sb.Append("WHERE tIngressoLog.Acao='" + IngressoLog.VENDER + "' AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ");
                        sb.Append("AND (( tCanal.ID = " + canalID + ") OR ( tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " )) ");
                        sb.Append("ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else if (perfilID == Perfil.SAC_SUPERVISOR || 
                             perfilID == Perfil.SAC_OPERADOR ||
                             perfilID == Perfil.SAC_SUPERVISOR_NOVO ||
                             perfilID == Perfil.SAC_OPERADOR_NOVO)
                    {
                        //TODO: Corrigir permissao
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT top 1 vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                        sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, ");
                        sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus ,c.CNPJ, c.NomeFantasia, vb.ValorSeguro ");
                        sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                        sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                        sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                        sb.Append("INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID=vb.ID ");
                        sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                        sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                        sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                        sb.Append("LEFT JOIN tCliente c (NOLOCK) ON  vb.ClienteID=c.ID ");
                        sb.Append("WHERE tIngressoLog.Acao='" + IngressoLog.VENDER + "' AND tIngressoLog.IngressoID=" + ingressoLista.Ingresso.Control.ID + " ");
                        sb.Append("ORDER BY tIngressoLog.ID DESC");
                        sql = sb.ToString();
                    }
                    else
                    {
                        throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");
                    }

                    BD bd = new BD();

                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {
                        DataRow linhaInfo = info.NewRow();

                        linhaInfo["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linhaInfo["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                        linhaInfo[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                        linhaInfo[STATUS_VENDA] = bd.LerString("Status");
                        if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                            linhaInfo[CLIENTE] = bd.LerString("Cliente");
                        else
                            linhaInfo[CLIENTE] = bd.LerString("NomeFantasia");
                        linhaInfo[CANAL] = bd.LerString("Canal");
                        linhaInfo[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");
                        linhaInfo[VENDEDOR] = bd.LerString("Vendedor");
                        linhaInfo[SENHA] = bd.LerString("Senha");
                        linhaInfo[DATA_VENDA] = bd.LerDateTime("DataVenda");
                        linhaInfo[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                        linhaInfo[CLIENTE_STATUS] = bd.LerString("ClienteStatus");
                        linhaInfo[VALORSEGURO] = bd.LerDecimal("ValorSeguro");

                        info.Rows.Add(linhaInfo);

                        sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + ingressoLista.Ingresso.VendaBilheteriaID + ", " + ingressoLista.Ingresso.Control.ID + ", " + lojaID;

                        bd.Consulta(sql);

                        if (bd.Consulta().Read())
                        {
                            if (bd.LerInt("VendaBilheteriaID") == ingressoLista.Ingresso.VendaBilheteriaID.Valor)
                            {

                                DataRow linha = tabelaImpressao.NewRow();

                                linha["ID"] = bd.LerInt("ID");
                                linha["IngressoID"] = bd.LerInt("IngressoID");
                                linha["Usuario"] = login;
                                linha["Evento"] = bd.LerString("Evento");
                                linha["Pacote"] = bd.LerString("Pacote");
                                linha["EventoID"] = bd.LerInt("EventoID");
                                linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
                                linha["PacoteID"] = bd.LerInt("PacoteID");
                                linha["Horario"] = bd.LerDateTime("Horario");
                                linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                                linha["DataVendaString"] = bd.LerStringFormatoDataHora("DataVenda");
                                linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                                linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
                                linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
                                linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                linha["Setor"] = bd.LerString("Setor");
                                linha["Acesso"] = bd.LerString("Acesso");
                                linha["Produto"] = bd.LerBoolean("Produto");
                                linha["Cortesia"] = bd.LerString("Cortesia");
                                linha["Preco"] = bd.LerString("Preco");
                                linha["Valor"] = bd.LerDecimal("Valor");
                                linha["Codigo"] = bd.LerString("Codigo");
                                linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
                                linha["Loja"] = bd.LerString("Loja");
                                linha["Senha"] = bd.LerString("Senha");
                                linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                                linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                                linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                                linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                                linha["Cliente"] = bd.LerString("Cliente");
                                //linha["ClienteRG"] = bd.LerString("ClienteRG");
                                linha["ClienteCPF"] = bd.LerString("ClienteCPF");
                                linha["ClienteID"] = bd.LerInt("ClienteID");
                                linha["ClienteEnderecoID"] = bd.LerInt("ClienteEnderecoID");
                                linha["Status"] = bd.LerString("Status");
                                linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
                                linha["LocalNome"] = bd.LerString("LocalNome");
                                linha["LocalEndereco"] = bd.LerString("Logradouro") + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                                linha["LocalCep"] = bd.LerString("LocalCep");
                                linha["LocalPais"] = bd.LerString("Pais");
                                linha["LocalEstado"] = bd.LerString("LocalEstado");
                                linha["LocalCidade"] = bd.LerString("LocalCidade");
                                linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                                linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                                linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                                linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
                                linha["TipoVenda"] = bd.LerString("TipoVenda");
                                linha["TipoImpressao"] = bd.LerString("TipoImpressao");
                                linha["NomeCartao"] = bd.LerString("NomeCartao");

                                linha["Alvara"] = bd.LerString("Alvara");
                                linha["FonteImposto"] = bd.LerString("FonteImposto");
                                linha["AVCB"] = bd.LerString("AVCB");
                                linha["PorcentagemImposto"] = bd.LerDecimal("PorcentagemImposto");

                                linha["Lotacao"] = bd.LerDecimal("Lotacao");
                                linha["DataEmissaoAlvara"] = bd.LerStringFormatoData("DataEmissaoAlvara");
                                linha["DataValidadeAlvara"] = bd.LerStringFormatoData("DataValidadeAlvara");
                                linha["DataEmissaoAvcb"] = bd.LerStringFormatoData("DataEmissaoAvcb");
                                linha["DataValidadeAvcb"] = bd.LerStringFormatoData("DataValidadeAvcb");

                                linha["ApresentacaoCancelada"] = bd.LerString("ApresentacaoCancelada");

                                tabelaImpressao.Rows.Add(linha);

                                DataRow linhaGrid = grid.NewRow();

                                linhaGrid[EVENTO] = (string)linha["Evento"];
                                linhaGrid[HORARIO] = (DateTime)linha["Horario"];
                                linhaGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                                if ((string)linha["Cortesia"] != "")
                                    linhaGrid[CORTESIA] = (string)linha["Cortesia"];
                                linhaGrid[PRECO] = (string)linha["Preco"];
                                linhaGrid[VALOR] = (decimal)linha["Valor"];
                                linhaGrid[CODIGO] = (string)linha["Codigo"];
                                linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));

                                if ((string)linha["ApresentacaoCancelada"] == "T")
                                    linhaGrid[STATUS_DETALHADO] = "Apresentação cancelada";

                                linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                                linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                                linhaGrid[LOG_ID] = (int)linha["ID"];
                                linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];

                                grid.Rows.Add(linhaGrid);

                            }
                        }

                        tabelaImpressao = AssinaturaBancoIngresso.VerificarSeExisteBancoIngresso(bd.LerInt("IngressoID"), tabelaImpressao);
                    }
                    bd.Fechar();

                }
                else if (ingressoLista.Tamanho > 1)
                {
                    throw new ReimpressaoGerenciadorException("Apresentação possui mais de 1 lugar com código " + codigo);
                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
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
        public DataSet PesquisarSenha(string senha, int perfilID, int caixaID, int canalID, string login, int empresaID, int localID, int lojaID)
        {

            BD bd = new BD();

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                int vendaID = 0;

                string sql;

                if (perfilID == Perfil.CANAL_BILHETEIRO)
                {

                    string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, vb.DataVenda, ");
                    sb.Append("c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus , c.CNPJ, c.NomeFantasia, vb.ValorSeguro ");
                    sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE tCaixa.ID = " + caixaID + " ");
                    sb.Append("AND vb.Senha='" + senha + "'");
                    sql = sb.ToString();
                }
                else if (perfilID == Perfil.CANAL_SUPERVISOR)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, vb.DataVenda, ");
                    sb.Append("c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus, c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                    sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON  vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE tCanal.ID=" + canalID + " AND vb.Senha='" + senha + "'");
                    sql = sb.ToString();
                }
                else if (perfilID == Perfil.LOCAL_SUPERVISOR)
                {

                    //TODO: Corrigir permissao
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                    sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                    sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus , c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                    sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE  vb.Senha='" + senha + "' ");
                    sb.Append("AND (( tCanal.ID = " + canalID + ") OR ( tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " )) ");
                    sql = sb.ToString();
                }
                else if (perfilID == Perfil.SAC_SUPERVISOR ||
                         perfilID == Perfil.SAC_OPERADOR || 
                         perfilID == Perfil.SAC_SUPERVISOR_NOVO  ||
                         perfilID == Perfil.SAC_OPERADOR_NOVO)
                {
                    //TODO: Corrigir permissao
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                    sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                    sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus , c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                    sb.Append("FROM tVendaBilheteria (NOLOCK) AS vb ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario (NOLOCK) u ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE  vb.Senha='" + senha + "' ");
                    sql = sb.ToString();
                }
                else
                    throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();

                    vendaID = bd.LerInt("ID");
                    linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                    linha[STATUS_VENDA] = bd.LerString("Status");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        linha[CLIENTE] = bd.LerString("Cliente");
                    else
                        linha[CLIENTE] = bd.LerString("NomeFantasia");
                    linha[CANAL] = bd.LerString("Canal");
                    linha[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");
                    linha[VENDEDOR] = bd.LerString("Vendedor");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                    linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                    linha[CLIENTE_STATUS] = bd.LerString("ClienteStatus");
                    linha[VALORSEGURO] = bd.LerDecimal("ValorSeguro");
                    info.Rows.Add(linha);
                }
                bd.Fechar();

                if (vendaID != 0)
                {

                    sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + vendaID + ", 0, " + lojaID;
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        if (bd.LerInt("VendaBilheteriaID") == vendaID)
                        {

                            DataRow linha = tabelaImpressao.NewRow();

                            linha["ID"] = bd.LerInt("ID");
                            linha["IngressoID"] = bd.LerInt("IngressoID");
                            linha["Usuario"] = login;
                            linha["Evento"] = bd.LerString("Evento");
                            linha["Pacote"] = bd.LerString("Pacote");
                            linha["EventoID"] = bd.LerInt("EventoID");
                            linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
                            linha["PacoteID"] = bd.LerInt("PacoteID");
                            linha["Horario"] = bd.LerDateTime("Horario");
                            linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                            linha["DataVendaString"] = bd.LerStringFormatoDataHora("DataVenda");
                            linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                            linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
                            linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
                            linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                            linha["Setor"] = bd.LerString("Setor");
                            linha["Acesso"] = bd.LerString("Acesso");
                            linha["Produto"] = bd.LerBoolean("Produto");
                            linha["Cortesia"] = bd.LerString("Cortesia");
                            linha["Preco"] = bd.LerString("Preco");
                            linha["Valor"] = bd.LerDecimal("Valor");
                            linha["Codigo"] = bd.LerString("Codigo");
                            linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                            linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
                            linha["Loja"] = bd.LerString("Loja");
                            linha["Senha"] = bd.LerString("Senha");
                            linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                            linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                            linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                            linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                            linha["Cliente"] = bd.LerString("Cliente");
                            //linha["ClienteRG"] = bd.LerString("ClienteRG");
                            linha["ClienteCPF"] = bd.LerString("ClienteCPF");
                            linha["ClienteID"] = bd.LerInt("ClienteID");
                            linha["ClienteEnderecoID"] = bd.LerInt("ClienteEnderecoID");
                            linha["Status"] = bd.LerString("Status");
                            linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
                            linha["LocalNome"] = bd.LerString("LocalNome");
                            linha["LocalEndereco"] = bd.LerString("Logradouro") + ", " + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                            linha["LocalCep"] = bd.LerString("LocalCep");
                            linha["LocalPais"] = bd.LerString("Pais");
                            linha["LocalEstado"] = bd.LerString("LocalEstado");
                            linha["LocalCidade"] = bd.LerString("LocalCidade");
                            linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                            linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                            linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                            linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
                            linha["TipoVenda"] = bd.LerString("TipoVenda");
                            linha["Bin"] = bd.LerString("Bin");
                            linha["EntregaNome"] = bd.LerString("EntregaNome");
                            linha["PeriodoEntrega"] = bd.LerString("PeriodoEntrega");
                            DateTime DataAux = bd.LerDateTime("DataEntrega");
                            linha["DataEntrega"] = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "";
                            linha["AreaEntrega"] = bd.LerString("AreaEntrega");
                            linha["TipoImpressao"] = bd.LerString("TipoImpressao");
                            linha["NomeCartao"] = bd.LerString("NomeCartao");

                            linha["Alvara"] = bd.LerString("Alvara");
                            linha["FonteImposto"] = bd.LerString("FonteImposto");
                            linha["AVCB"] = bd.LerString("AVCB");
                            linha["PorcentagemImposto"] = bd.LerDecimal("PorcentagemImposto");

                            linha["Lotacao"] = bd.LerDecimal("Lotacao");
                            linha["DataEmissaoAlvara"] = bd.LerStringFormatoData("DataEmissaoAlvara");
                            linha["DataValidadeAlvara"] = bd.LerStringFormatoData("DataValidadeAlvara");
                            linha["DataEmissaoAvcb"] = bd.LerStringFormatoData("DataEmissaoAvcb");
                            linha["DataValidadeAvcb"] = bd.LerStringFormatoData("DataValidadeAvcb");

                            linha["ApresentacaoCancelada"] = bd.LerString("ApresentacaoCancelada");

                            tabelaImpressao.Rows.Add(linha);

                            DataRow linhaGrid = grid.NewRow();

                            linhaGrid[EVENTO] = (string)linha["Evento"];
                            linhaGrid[HORARIO] = (DateTime)linha["Horario"];
                            linhaGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                            if ((string)linha["Cortesia"] != "")
                                linhaGrid[CORTESIA] = (string)linha["Cortesia"];
                            linhaGrid[PRECO] = (string)linha["Preco"];
                            linhaGrid[VALOR] = (decimal)linha["Valor"];
                            linhaGrid[CODIGO] = (string)linha["Codigo"];
                            linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));

                            if ((string)linha["ApresentacaoCancelada"] == "T")
                                linhaGrid[STATUS_DETALHADO] = "Apresentação cancelada";

                            linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                            linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                            linhaGrid[LOG_ID] = (int)linha["ID"];
                            linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];

                            grid.Rows.Add(linhaGrid);

                        }
                    }

                    bd.Fechar();
                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
                retorno.Tables.Add(info);

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

        /// <summary>
        /// Pesquisa pela senha de cliente
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarVenda(int vendaID, int perfilID, int caixaID, int canalID, string login, int empresaID, int localID, int lojaID)
        {

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();

                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                string sql;

                if (perfilID == Perfil.CANAL_BILHETEIRO)
                {

                    string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                    sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                    sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus, c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                    sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE tCaixa.ID = " + caixaID + " ");
                    sb.Append("AND tCaixa.DataFechamento='' AND vb.ID=" + vendaID);
                    sql = sb.ToString();
                }
                else if (perfilID == Perfil.CANAL_SUPERVISOR)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                    sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                    sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus, c.CNPJ, c.NomeFantasia, vb.ValorSeguro  ");
                    sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE tCanal.ID=" + canalID + " ");
                    sb.Append("AND vb.ID=" + vendaID);
                    sql = sb.ToString();
                }
                else if (perfilID == Perfil.LOCAL_SUPERVISOR)
                {

                    //TODO: Corrigir permissao
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                    sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                    sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus, c.CNPJ, c.NomeFantasia, vb.ValorSeguro ");
                    sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE vb.ID=" + vendaID);
                    sb.Append("AND (( tCanal.ID = " + canalID + ") OR ( tIngresso.EmpresaID = " + empresaID + " AND tIngresso.LocalID = " + localID + " )) ");
                    sql = sb.ToString();
                }
                else if(perfilID == Perfil.SAC_SUPERVISOR||
                        perfilID == Perfil.SAC_OPERADOR || 
                        perfilID == Perfil.SAC_SUPERVISOR_NOVO ||
                        perfilID == Perfil.SAC_OPERADOR_NOVO)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.ValorTotal, vb.Status, vb.Senha, ");
                    sb.Append("vb.DataVenda, c.Nome AS Cliente, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade,  u.Nome AS Vendedor, ");
                    sb.Append("tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus, c.CNPJ, c.NomeFantasia, vb.ValorSeguro ");
                    sb.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                    sb.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    sb.Append("INNER JOIN tUsuario u (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                    sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    sb.Append("INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID ");
                    sb.Append("LEFT JOIN tCliente c (NOLOCK) ON vb.ClienteID=c.ID ");
                    sb.Append("WHERE vb.ID=" + vendaID);
                    sql = sb.ToString();
                }
                else
                {
                    throw new ReimpressaoGerenciadorException("Perfil nulo ou não permitido.");
                }

                BD bd = new BD();
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();

                    linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                    linha[STATUS_VENDA] = bd.LerString("Status");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        linha[CLIENTE] = bd.LerString("Cliente");
                    else
                        linha[CLIENTE] = bd.LerString("NomeFantasia");
                    linha[CANAL] = bd.LerString("Canal");
                    linha[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");
                    linha[VENDEDOR] = bd.LerString("Vendedor");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                    linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                    linha[CLIENTE_STATUS] = bd.LerString("ClienteStatus");
                    linha[VALORSEGURO] = bd.LerDecimal("ValorSeguro");
                    info.Rows.Add(linha);

                }
                bd.Fechar();

                if (info.Rows.Count > 0)
                {
                    sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + vendaID + ", 0, " + lojaID;
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        if (bd.LerInt("VendaBilheteriaID") == vendaID)
                        {

                            DataRow linha = tabelaImpressao.NewRow();

                            linha["ID"] = bd.LerInt("ID");
                            linha["IngressoID"] = bd.LerInt("IngressoID");
                            linha["Usuario"] = login;
                            linha["Evento"] = bd.LerString("Evento");
                            linha["Pacote"] = bd.LerString("Pacote");
                            linha["EventoID"] = bd.LerInt("EventoID");
                            linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
                            linha["PacoteID"] = bd.LerInt("PacoteID");
                            linha["Horario"] = bd.LerDateTime("Horario");
                            linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                            linha["DataVendaString"] = bd.LerStringFormatoDataHora("DataVenda");
                            linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                            linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
                            linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
                            linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                            linha["Setor"] = bd.LerString("Setor");
                            linha["Acesso"] = bd.LerString("Acesso");
                            linha["Produto"] = bd.LerBoolean("Produto");
                            linha["Cortesia"] = bd.LerString("Cortesia");
                            linha["Preco"] = bd.LerString("Preco");
                            linha["Valor"] = bd.LerDecimal("Valor");
                            linha["Codigo"] = bd.LerString("Codigo");
                            linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                            linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
                            linha["Loja"] = bd.LerString("Loja");
                            linha["Senha"] = bd.LerString("Senha");
                            linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                            linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                            linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                            linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                            linha["Cliente"] = bd.LerString("Cliente");
                            //linha["ClienteRG"] = bd.LerString("ClienteRG");
                            linha["ClienteCPF"] = bd.LerString("ClienteCPF");
                            linha["ClienteID"] = bd.LerInt("ClienteID");
                            linha["ClienteEnderecoID"] = bd.LerInt("ClienteEnderecoID");
                            linha["Status"] = bd.LerString("Status");
                            linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
                            linha["LocalNome"] = bd.LerString("LocalNome");
                            linha["LocalEndereco"] = bd.LerString("Logradouro") + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                            linha["LocalCep"] = bd.LerString("LocalCep");
                            linha["LocalPais"] = bd.LerString("Pais");
                            linha["LocalEstado"] = bd.LerString("LocalEstado");
                            linha["LocalCidade"] = bd.LerString("LocalCidade");
                            linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                            linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                            linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                            linha["TipoVenda"] = bd.LerString("TipoVenda");
                            linha["TipoImpressao"] = bd.LerString("TipoImpressao");
                            linha["NomeCartao"] = bd.LerString("NomeCartao");

                            linha["Alvara"] = bd.LerString("Alvara");
                            linha["FonteImposto"] = bd.LerString("FonteImposto");
                            linha["AVCB"] = bd.LerString("AVCB");
                            linha["PorcentagemImposto"] = bd.LerDecimal("PorcentagemImposto");

                            linha["Lotacao"] = bd.LerDecimal("Lotacao");
                            linha["DataEmissaoAlvara"] = bd.LerStringFormatoData("DataEmissaoAlvara");
                            linha["DataValidadeAlvara"] = bd.LerStringFormatoData("DataValidadeAlvara");
                            linha["DataEmissaoAvcb"] = bd.LerStringFormatoData("DataEmissaoAvcb");
                            linha["DataValidadeAvcb"] = bd.LerStringFormatoData("DataValidadeAvcb");

                            linha["ApresentacaoCancelada"] = bd.LerString("ApresentacaoCancelada");

                            tabelaImpressao.Rows.Add(linha);

                            DataRow linhaGrid = grid.NewRow();

                            linhaGrid[EVENTO] = (string)linha["Evento"];
                            linhaGrid[HORARIO] = (DateTime)linha["Horario"];
                            linhaGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                            if ((string)linha["Cortesia"] != "")
                                linhaGrid[CORTESIA] = (string)linha["Cortesia"];
                            linhaGrid[PRECO] = (string)linha["Preco"];
                            linhaGrid[VALOR] = (decimal)linha["Valor"];
                            linhaGrid[CODIGO] = (string)linha["Codigo"];
                            linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));

                            if ((string)linha["ApresentacaoCancelada"] == "T")
                                linhaGrid[STATUS_DETALHADO] = "Apresentação cancelada";

                            linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                            linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                            linhaGrid[LOG_ID] = (int)linha["ID"];
                            linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];

                            grid.Rows.Add(linhaGrid);

                        }
                    }

                    bd.Fechar();
                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
                retorno.Tables.Add(info);

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EstruturaRetornoVendaValeIngresso PesquisarVendaValeIngresso(int vendaID, int perfilID, int caixaID, int canalID, string login, int empresaID, int localID)
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
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, u.Nome as Vendedor, ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade , c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        //stbSQL.Append("WHERE vb.Senha = '" + senha + "' AND ca.ID =" + caixaID);
                        stbSQL.Append("WHERE ca.ID = " + caixaID + " ");
                        stbSQL.Append("AND ca.DataFechamento='' AND vb.ID=" + vendaID);
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade , c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
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
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade  , c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE vb.ID = '" + vendaID + "' ");
                        stbSQL.Append("AND ( vi.CanalID = " + canalID + " OR l.ID = " + localID + " ) ");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID ,vb.ClienteID,c.Nome AS ClienteNome, vb.Senha, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade  , c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE vb.ID = '" + vendaID + "' ");
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
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    retorno.DataAberturaCaixa = bd.LerDateTime("CaixaAbertura");
                    retorno.ValorTotalVenda = valorTotal;
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.Senha = bd.LerString("Senha");
                    retorno.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");
                }

                bd.FecharConsulta();
                stbSQL = new StringBuilder();

                if (retorno.VendaBilheteriaID != 0)
                {
                    stbSQL.Append("SELECT DISTINCT vi.ID , vit.CodigoTrocaFixo, vi.CodigoTroca, vi.CodigoBarra, vit.SaudacaoNominal, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                    stbSQL.Append("         vit.Valor, vit.ValorTipo ,vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao ");
                    stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                    stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                    stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = viLog.VendaBilheteriaItemID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                    stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND vi.VendaBilheteriaID = " + retorno.VendaBilheteriaID);
                    stbSQL.Append(" ORDER BY vi.ID");

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
                        string valorTipo = bd.LerString("ValorTipo");
                        if (ValeIngressoTipo.EnumValorTipo.Porcentagem == (ValeIngressoTipo.EnumValorTipo)Convert.ToChar(valorTipo))
                        {
                            impressaoItem.ValorExibicao = 0;
                            impressaoItem.Porcentagem = (int)bd.LerDecimal("Valor");
                        }
                        else
                        {
                            impressaoItem.ValorExibicao = bd.LerDecimal("Valor");
                            impressaoItem.Porcentagem = 0;
                        }
                        impressaoItem.Valor = bd.LerDecimal("Valor");
                        impressaoItem.ValorTipo = Convert.ToChar(valorTipo);

                        impressaoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                        impressaoItem.CodigoTroca = impressaoItem.CodigoTrocaFixo ? codigoTrocaFixo : bd.LerString("CodigoTroca");
                        switch (Convert.ToChar(bd.LerString("Status")))
                        {
                            case (char)ValeIngresso.enumStatus.Vendido:
                                impressaoItem.Status = ValeIngresso.enumStatus.Vendido;
                                break;
                            case (char)ValeIngresso.enumStatus.Impresso:
                                impressaoItem.Status = ValeIngresso.enumStatus.Impresso;
                                break;
                            case (char)ValeIngresso.enumStatus.Cancelado:
                                impressaoItem.Status = ValeIngresso.enumStatus.Cancelado;
                                break;
                            case (char)ValeIngresso.enumStatus.Trocado:
                                impressaoItem.Status = ValeIngresso.enumStatus.Trocado;
                                break;
                            case (char)ValeIngresso.enumStatus.Disponivel:
                                impressaoItem.Status = ValeIngresso.enumStatus.Disponivel;
                                break;
                            case (char)ValeIngresso.enumStatus.Reservado:
                                impressaoItem.Status = ValeIngresso.enumStatus.Reservado;
                                break;
                            case (char)ValeIngresso.enumStatus.Entregue:
                                impressaoItem.Status = ValeIngresso.enumStatus.Entregue;
                                break;
                            case (char)ValeIngresso.enumStatus.Aguardando:
                                impressaoItem.Status = ValeIngresso.enumStatus.Aguardando;
                                break;
                            case (char)ValeIngresso.enumStatus.Expirado:
                                impressaoItem.Status = ValeIngresso.enumStatus.Expirado;
                                break;
                        }
                        impressaoItem.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                        impressaoItem.ClientePresenteado = bd.LerString("ClienteNome");
                        impressaoItem.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                        impressaoItem.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                        impressaoItem.ValidadeData = bd.LerDateTime("ValidadeData");
                        impressaoItem.ValidadeEmDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                        impressaoItem.NomeCartao = bd.LerString("NomeCartao");
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

        public EstruturaRetornoVendaValeIngresso PesquisarCodigoBarrasValeIngresso(string codigoBarra, int perfilID, int caixaID, int canalID, string login, int empresaID, int localID)
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
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, u.Nome as Vendedor, ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal ,c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
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
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal ,c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
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
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal ,c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
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
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal ,c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                        stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
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
                    retorno.ComprovanteQuantidade = 0;
                }
                else
                {
                    throw new ReimpressaoGerenciadorException("Não existem ingressos com esse Código de Barra");
                }

                bd.FecharConsulta();
                stbSQL = new StringBuilder();

                stbSQL.Append("SELECT DISTINCT vi.ID , vit.CodigoTrocaFixo, vi.CodigoTroca, vi.CodigoBarra, vit.SaudacaoNominal, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                stbSQL.Append("         vit.Valor, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao, vit.ValorTipo ");
                stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = viLog.VendaBilheteriaItemID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND vi.VendaBilheteriaID = " + retorno.VendaBilheteriaID);
                stbSQL.Append(" ORDER BY vi.ID");

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
                        retornoItem.ValorExibicao = 0;
                        retornoItem.Porcentagem = (int)bd.LerDecimal("Valor");
                    }
                    else
                    {
                        retornoItem.ValorExibicao = bd.LerDecimal("Valor");
                        retornoItem.Porcentagem = 0;
                    }
                    retornoItem.Valor = bd.LerDecimal("Valor");
                    retornoItem.ValorTipo = Convert.ToChar(valorTipo);

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
                        case (char)ValeIngresso.enumStatus.Cancelado:
                            retornoItem.Status = ValeIngresso.enumStatus.Cancelado;
                            break;
                        case (char)ValeIngresso.enumStatus.Trocado:
                            retornoItem.Status = ValeIngresso.enumStatus.Trocado;
                            break;
                        case (char)ValeIngresso.enumStatus.Disponivel:
                            retornoItem.Status = ValeIngresso.enumStatus.Disponivel;
                            break;
                        case (char)ValeIngresso.enumStatus.Reservado:
                            retornoItem.Status = ValeIngresso.enumStatus.Reservado;
                            break;
                        case (char)ValeIngresso.enumStatus.Entregue:
                            retornoItem.Status = ValeIngresso.enumStatus.Entregue;
                            break;
                        case (char)ValeIngresso.enumStatus.Aguardando:
                            retornoItem.Status = ValeIngresso.enumStatus.Aguardando;
                            break;
                        case (char)ValeIngresso.enumStatus.Expirado:
                            retornoItem.Status = ValeIngresso.enumStatus.Expirado;
                            break;
                    }

                    retornoItem.NomeCartao = bd.LerString("NomeCartao");

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
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID, vb.ClienteID, vb.ClienteEnderecoID, c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, u.Nome as Vendedor, vb.DataVenda, ca.DataAbertura AS CaixaAbertura, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade ,c.CNPJ, c.NomeFantasia , ");
                        stbSQL.Append("e.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                        stbSQL.Append("LEFT JOIN tEntrega e(NOLOCK) ON ec.EntregaID = e.ID ");
                        stbSQL.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = ec.EntregaAreaID ");
                        stbSQL.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = ec.PeriodoID ");
                        stbSQL.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' AND ca.ID =" + caixaID + " AND ca.DataFechamento=''");
                        break;
                    case Perfil.CANAL_SUPERVISOR:
                        stbSQL.Append("SELECT vb.ID AS VendaBilheteriaID ,vb.ClienteID, vb.ClienteEnderecoID, c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade ,c.CNPJ, c.NomeFantasia  ");
                        stbSQL.Append(",e.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                        stbSQL.Append("LEFT JOIN tEntrega e(NOLOCK) ON ec.EntregaID = e.ID ");
                        stbSQL.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = ec.EntregaAreaID ");
                        stbSQL.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = ec.PeriodoID ");
                        stbSQL.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' AND tCanal.ID =" + canalID);
                        break;
                    case Perfil.LOCAL_SUPERVISOR:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID , vb.ClienteID, vb.ClienteEnderecoID, c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade ,c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append(",e.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                        stbSQL.Append("LEFT JOIN tEntrega e(NOLOCK) ON ec.EntregaID = e.ID ");
                        stbSQL.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = ec.EntregaAreaID ");
                        stbSQL.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = ec.PeriodoID ");
                        stbSQL.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' ");
                        stbSQL.Append("AND ( vi.CanalID = " + canalID + " OR l.ID = " + localID + " ) ");
                        break;
                    case Perfil.SAC_OPERADOR:
                    case Perfil.SAC_SUPERVISOR:
                    case Perfil.SAC_OPERADOR_NOVO:
                    case Perfil.SAC_SUPERVISOR_NOVO:
                        stbSQL.Append("SELECT DISTINCT vb.ID AS VendaBilheteriaID , vb.ClienteID, vb.ClienteEnderecoID, c.Nome AS ClienteNome, ");
                        stbSQL.Append("vb.ValorTotal, vb.TaxaEntregaValor, vb.DataVenda, ");
                        stbSQL.Append("ca.DataAbertura AS CaixaAbertura, u.Nome as Vendedor, vb.Status, tCanal.Nome AS Canal, tCanal.ComprovanteQuantidade ,c.CNPJ, c.NomeFantasia ");
                        stbSQL.Append(",e.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
                        stbSQL.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                        stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                        stbSQL.Append("INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                        stbSQL.Append("INNER JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                        stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                        stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                        stbSQL.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                        stbSQL.Append("LEFT JOIN tEntrega e(NOLOCK) ON ec.EntregaID = e.ID ");
                        stbSQL.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = ec.EntregaAreaID ");
                        stbSQL.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = ec.PeriodoID ");
                        stbSQL.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");
                        stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID ");
                        stbSQL.Append("WHERE vb.Senha = '" + senha + "' ");
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
                    retorno.ClienteEnderecoID = bd.LerInt("ClienteEnderecoID");

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
                    retorno.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");


                    retorno.EntregaNome = bd.LerString("EntregaNome");
                    retorno.PeriodoEntrega = bd.LerString("PeriodoEntrega");
                    DateTime DataAux = bd.LerDateTime("DataEntrega");
                    retorno.DataEntrega = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "";
                    retorno.AreaEntrega = bd.LerString("AreaEntrega");
                }

                bd.FecharConsulta();
                stbSQL = new StringBuilder();

                if (retorno.VendaBilheteriaID != 0)
                {
                    stbSQL.Append("SELECT DISTINCT vi.ID, vit.CodigoTrocaFixo, vi.CodigoTroca, vi.CodigoBarra, vit.SaudacaoNominal, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                    stbSQL.Append("         vit.Valor, vit.ValorTipo , vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao ");
                    stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                    stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                    stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = viLog.VendaBilheteriaItemID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                    stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND vi.VendaBilheteriaID = " + retorno.VendaBilheteriaID);
                    stbSQL.Append(" ORDER BY vi.ID");


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
                            impressaoItem.ValorExibicao = 0;
                            impressaoItem.Porcentagem = (int)bd.LerDecimal("Valor");
                        }
                        else
                        {
                            impressaoItem.ValorExibicao = bd.LerDecimal("Valor");
                            impressaoItem.Porcentagem = 0;
                        }
                        impressaoItem.Valor = bd.LerDecimal("Valor");
                        impressaoItem.ValorTipo = Convert.ToChar(valorTipo);

                        impressaoItem.NomeCartao = bd.LerString("NomeCartao");
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
    public class ReimpressaoGerenciadorException : Exception
    {

        public ReimpressaoGerenciadorException() : base() { }

        public ReimpressaoGerenciadorException(string msg) : base(msg) { }

        public ReimpressaoGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
