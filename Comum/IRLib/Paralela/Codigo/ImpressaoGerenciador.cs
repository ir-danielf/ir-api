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
    /// Gerenciador do ImpressaoGerenciador
    /// </summary>
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ImpressaoGerenciadorParalela : MarshalByRefObject
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
        //public const string ACAO = "Ação";
        public const string COMPROVANTE_QUANTIDADE = "ComprovanteQuantidade";
        public const string CLIENTE = "Cliente";
        public const string CLIENTE_EMAIL = "ClienteEmail";
        public const string CANAL = "Canal";
        public const string INFO_VENDA = "InfoVenda";
        public const string INFO_TAXAENTREGA = "InfoTaxaEntrega";
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
        public const string DIAS_TRIAGEM = "DiasTriagem";
        public const string STATUS_DETALHADO = "StatusDetalhado";
        public const string CLIENTE_STATUS = "ClienteStatus";
        public const string TAXA_ENTREGA = "TaxaEntrega";
        public const string TAXA_ENTREGA_ID = "TaxaEntregaID";
        public const string TAXA_ENTREGA_TIPO = "TaxaEntregaTipo";
        public const string PAGAMENTO_PROCESSADO = "PagamentoProcessado";

        private bool SoImprimirDataApresentacao = new ConfigGerenciadorParalela().ImprimirSomenteDataApresentacao();

        //private string login;

        public ImpressaoGerenciadorParalela()
        {

            //login = "";

        }

        //public string Login
        //{
        //    set { login = value; }
        //}

        #region estruturas
        public static DataTable EstruturaGrid()
        {

            DataTable tGrid = new DataTable(BilheteriaParalela.TABELA_GRID);

            tGrid.Columns.Add(INGRESSO_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(LOG_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(BilheteriaParalela.EVENTO, typeof(string));
            tGrid.Columns.Add(BilheteriaParalela.HORARIO, typeof(string));
            tGrid.Columns.Add(BilheteriaParalela.SETOR_PRODUTO, typeof(string));
            tGrid.Columns.Add(BilheteriaParalela.CODIGO, typeof(string));
            tGrid.Columns.Add(BilheteriaParalela.PRECO, typeof(string));
            tGrid.Columns.Add(BilheteriaParalela.CORTESIA, typeof(string));
            tGrid.Columns.Add(BilheteriaParalela.VALOR, typeof(decimal));
            tGrid.Columns.Add(STATUS, typeof(string));
            tGrid.Columns.Add(TIPO_VENDA, typeof(string));
            tGrid.Columns.Add(DIAS_TRIAGEM, typeof(int));
            tGrid.Columns.Add(STATUS_DETALHADO, typeof(string));
            //tGrid.Columns.Add(ACAO, typeof(string));
            //tGrid.Columns.Add(Bilheteria.CONV, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(Bilheteria.VALOR_CONV, typeof(decimal)).DefaultValue = 0;

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
            tGrid.Columns.Add(PAGAMENTO_PROCESSADO, typeof(bool)).DefaultValue = true;
            return tGrid;

        }

        public static DataTable EstruturaTaxaEntrega()
        {
            DataTable tTaxaEntrega = new DataTable(INFO_TAXAENTREGA);
            tTaxaEntrega.Columns.Add(TAXA_ENTREGA_ID, typeof(int));
            tTaxaEntrega.Columns.Add(TAXA_ENTREGA, typeof(string));
            tTaxaEntrega.Columns.Add(TAXA_ENTREGA_TIPO, typeof(string));
            tTaxaEntrega.Columns.Add(CLIENTE, typeof(string)).DefaultValue = string.Empty;
            tTaxaEntrega.Columns.Add(CLIENTE_EMAIL, typeof(string)).DefaultValue = string.Empty;
            tTaxaEntrega.Columns.Add(VENDA_ID, typeof(int)).DefaultValue = 0;
            tTaxaEntrega.Columns.Add(SENHA, typeof(string)).DefaultValue = string.Empty;
            return tTaxaEntrega;
        }

        #endregion

        /// <summary>
        /// Pesquisa pelo codigo de barras
        /// </summary>
        /// <returns></returns>
        public DataSet PesquisarCodigoBarras(string codigoBarra, string login, int lojaID)
        {

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();
                DataTable taxaEntrega = EstruturaTaxaEntrega();
                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "(CodigoBarra='" + codigoBarra + "' OR CodigoBarraCliente='" + codigoBarra + "')";
                ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
                ingressoLista.Carregar();

                if (ingressoLista.Tamanho > 0)
                {

                    ingressoLista.Primeiro();

                    BD bd = new BD();

                    string sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + ingressoLista.Ingresso.VendaBilheteriaID + ", " + ingressoLista.Ingresso.Control.ID + ", " + lojaID;

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
                            linha["LocalCidade"] = bd.LerString("LocalCidade");
                            linha["LocalEstado"] = bd.LerString("LocalEstado");
                            linha["LocalPais"] = bd.LerString("Pais");
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

                            int diasTriagem = bd.LerInt("DiasTriagem");
                            DateTime dataVenda = bd.LerDateTime("DataVenda");
                            DateTime dataEvento = bd.LerDateTime("Horario");

                            if (diasTriagem > 0)
                            {
                                if (this.SoImprimirDataApresentacao && dataEvento.Subtract(DateTime.Now).Days <= 0)
                                    linha["DiasTriagem"] = 0;
                                else if (this.SoImprimirDataApresentacao)
                                    linha["DiasTriagem"] = 1;
                                else if (dataVenda.AddDays(diasTriagem) <= DateTime.Now)
                                    linha["DiasTriagem"] = 0;
                                else if (Utilitario.DiasUteisPeriodo(DateTime.Now, dataEvento) <= 2)
                                    linha["DiasTriagem"] = 0;
                                else
                                    linha["DiasTriagem"] = 1;
                            }
                            else
                                linha["DiasTriagem"] = 0;

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

                            linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                            linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhaGrid["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));
                            linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                            linhaGrid[LOG_ID] = (int)linha["ID"];
                            linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];
                            linhaGrid[DIAS_TRIAGEM] = (int)linha["DiasTriagem"];
                            grid.Rows.Add(linhaGrid);

                        }
                    }

                    bd.Fechar();

                    bd.Consulta(this.MontarBuscaIngressos("WHERE vb.ID=" + ingressoLista.Ingresso.VendaBilheteriaID));

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
                        linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                        info.Rows.Add(linha);

                        DataRow linhaTaxaEntrega = taxaEntrega.NewRow();
                        linhaTaxaEntrega[TAXA_ENTREGA] = bd.LerString(TAXA_ENTREGA);
                        linhaTaxaEntrega[TAXA_ENTREGA_ID] = bd.LerInt(TAXA_ENTREGA_ID);
                        linhaTaxaEntrega[VENDA_ID] = bd.LerInt("ID");
                        linhaTaxaEntrega[CLIENTE] = bd.LerString("Cliente");
                        linhaTaxaEntrega[CLIENTE_EMAIL] = bd.LerString("ClienteEmail");
                        linhaTaxaEntrega[TAXA_ENTREGA_TIPO] = bd.LerString("TaxaEntregaTipo");
                        linhaTaxaEntrega[SENHA] = bd.LerString("Senha");
                        taxaEntrega.Rows.Add(linhaTaxaEntrega);

                    }
                    bd.Fechar();

                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
                retorno.Tables.Add(info);
                retorno.Tables.Add(taxaEntrega);
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
        public DataSet PesquisarCodigoIngresso(int apresentacaoSetorID, string codigo, string login, int lojaID)
        {

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();
                DataTable taxaEntrega = EstruturaTaxaEntrega();
                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "ApresentacaoSetorID=" + apresentacaoSetorID;
                ingressoLista.FiltroSQL = "Codigo='" + codigo + "'";
                ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
                ingressoLista.Carregar();

                if (ingressoLista.Tamanho == 1)
                {

                    ingressoLista.Primeiro();

                    BD bd = new BD();


                    string sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + ingressoLista.Ingresso.VendaBilheteriaID + ", " + ingressoLista.Ingresso.Control.ID + ", " + lojaID;

                    bd.Consulta(sql);

                    int diasTriagem = 0;
                    DateTime dataVenda = DateTime.MinValue;
                    DateTime dataEvento = DateTime.MinValue;

                    while (bd.Consulta().Read())
                    {
                        if (bd.LerInt("VendaBilheteriaID") != ingressoLista.Ingresso.VendaBilheteriaID.Valor)
                            continue;

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
                        linha["LocalCidade"] = bd.LerString("LocalCidade");
                        linha["LocalEstado"] = bd.LerString("LocalEstado");
                        linha["LocalPais"] = bd.LerString("Pais");
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

                        diasTriagem = bd.LerInt("DiasTriagem");
                        dataVenda = bd.LerDateTime("DataVenda");
                        dataEvento = bd.LerDateTime("Horario");

                        if (diasTriagem > 0)
                        {
                            if (this.SoImprimirDataApresentacao && dataEvento.Subtract(DateTime.Now).Days <= 0)
                                linha["DiasTriagem"] = 0;
                            else if (this.SoImprimirDataApresentacao)
                                linha["DiasTriagem"] = 1;
                            else if (dataVenda.AddDays(diasTriagem) <= DateTime.Now)
                                linha["DiasTriagem"] = 0;
                            else if (Utilitario.DiasUteisPeriodo(DateTime.Now, dataEvento) <= 2)
                                linha["DiasTriagem"] = 0;
                            else
                                linha["DiasTriagem"] = 1;
                        }
                        else
                            linha["DiasTriagem"] = 0;

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

                        linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                        linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));
                        linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                        linhaGrid[LOG_ID] = (int)linha["ID"];
                        linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];
                        linhaGrid[DIAS_TRIAGEM] = (int)linha["DiasTriagem"];
                        grid.Rows.Add(linhaGrid);
                    }

                    bd.Fechar();


                    bd.Consulta(this.MontarBuscaIngressos("WHERE vb.ID=" + ingressoLista.Ingresso.VendaBilheteriaID));

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
                        linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                        info.Rows.Add(linha);

                        #region Monta as informações de Taxa de Entrega
                        DataRow linhaTaxaEntrega = taxaEntrega.NewRow();
                        linhaTaxaEntrega[TAXA_ENTREGA] = bd.LerString(TAXA_ENTREGA);
                        linhaTaxaEntrega[TAXA_ENTREGA_ID] = bd.LerInt(TAXA_ENTREGA_ID);
                        linhaTaxaEntrega[VENDA_ID] = bd.LerInt("ID");
                        linhaTaxaEntrega[CLIENTE] = bd.LerString("Cliente");
                        linhaTaxaEntrega[CLIENTE_EMAIL] = bd.LerString("ClienteEmail");
                        linhaTaxaEntrega[TAXA_ENTREGA_TIPO] = bd.LerString("TaxaEntregaTipo");
                        linhaTaxaEntrega[SENHA] = bd.LerString("Senha");
                        taxaEntrega.Rows.Add(linhaTaxaEntrega);

                        #endregion

                    }
                    bd.Fechar();

                }
                else if (ingressoLista.Tamanho > 1)
                {
                    throw new ImpressaoGerenciadorException("Apresentação possui mais de 1 lugar com código " + codigo);
                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
                retorno.Tables.Add(info);
                retorno.Tables.Add(taxaEntrega);
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
        public DataSet PesquisarSenha(string senha, string login, int lojaID)
        {

            try
            {

                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();
                DataTable taxaEntrega = EstruturaTaxaEntrega();

                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                int vendaID = 0;

                BD bd = new BD();


                string sql = this.MontarBuscaIngressos("WHERE vb.Senha='" + senha + "'");

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
                    linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);

                    DataRow linhaTaxaEntrega = taxaEntrega.NewRow();
                    linhaTaxaEntrega[TAXA_ENTREGA] = bd.LerString(TAXA_ENTREGA);
                    linhaTaxaEntrega[TAXA_ENTREGA_ID] = bd.LerInt(TAXA_ENTREGA_ID);
                    linhaTaxaEntrega[VENDA_ID] = bd.LerInt("ID");
                    linhaTaxaEntrega[CLIENTE] = bd.LerString("Cliente");
                    linhaTaxaEntrega[CLIENTE_EMAIL] = bd.LerString("ClienteEmail");
                    linhaTaxaEntrega[TAXA_ENTREGA_TIPO] = bd.LerString("TaxaEntregaTipo");
                    linhaTaxaEntrega[SENHA] = bd.LerString("Senha");
                    taxaEntrega.Rows.Add(linhaTaxaEntrega);

                    info.Rows.Add(linha);

                }
                bd.Fechar();

                if (vendaID != 0)
                {

                    sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + vendaID + ", 0 ," + lojaID;

                    bd.Consulta(sql);

                    int diasTriagem = 0;
                    DateTime dataVenda = DateTime.MinValue;
                    DateTime dataEvento = DateTime.MinValue;

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
                            linha["GerenciamentoIngressos"] = bd.LerString("GerenciamentoIngressos");
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
                            linha["ClienteCPF"] = bd.LerString("ClienteCPF");
                            linha["ClienteID"] = bd.LerInt("ClienteID");
                            linha["ClienteEnderecoID"] = bd.LerInt("ClienteEnderecoID");
                            linha["Status"] = bd.LerString("Status");
                            linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
                            linha["LocalNome"] = bd.LerString("LocalNome");
                            linha["LocalEndereco"] = bd.LerString("Logradouro") + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                            linha["LocalCep"] = bd.LerString("LocalCep");
                            linha["LocalCidade"] = bd.LerString("LocalCidade");
                            linha["LocalEstado"] = bd.LerString("LocalEstado");
                            linha["LocalPais"] = bd.LerString("Pais");
                            linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                            linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                            linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                            linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
                            linha["TipoVenda"] = bd.LerString("TipoVenda");
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

                            diasTriagem = bd.LerInt("DiasTriagem");
                            dataVenda = bd.LerDateTime("DataVenda");
                            dataEvento = bd.LerDateTime("Horario");
                            if (diasTriagem > 0)
                            {
                                if (this.SoImprimirDataApresentacao && dataEvento.Subtract(DateTime.Now).Days <= 0)
                                    linha["DiasTriagem"] = 0;
                                else if (this.SoImprimirDataApresentacao)
                                    linha["DiasTriagem"] = 1;
                                else if (dataVenda.AddDays(diasTriagem) <= DateTime.Now)
                                    linha["DiasTriagem"] = 0;
                                else if (Utilitario.DiasUteisPeriodo(DateTime.Now, dataEvento) <= 2)
                                    linha["DiasTriagem"] = 0;
                                else
                                    linha["DiasTriagem"] = 1;
                            }
                            else
                                linha["DiasTriagem"] = 0;

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

                            linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                            linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));
                            linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                            linhaGrid[LOG_ID] = (int)linha["ID"];
                            linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];
                            linhaGrid[DIAS_TRIAGEM] = (int)linha["DiasTriagem"];

                            grid.Rows.Add(linhaGrid);

                        }
                    }
                    bd.Fechar();
                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
                retorno.Tables.Add(info);
                retorno.Tables.Add(taxaEntrega);
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
        public DataSet PesquisarVenda(int vendaID, string login, int lojaID)
        {
            try
            {
                DataSet retorno = new DataSet();
                DataTable grid = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();
                DataTable taxaEntrega = EstruturaTaxaEntrega();

                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                BD bd = new BD();

                string sql = this.MontarBuscaIngressos("WHERE vb.ID=" + vendaID);


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
                    linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);

                    DataRow linhaTaxaEntrega = taxaEntrega.NewRow();
                    linhaTaxaEntrega[TAXA_ENTREGA] = bd.LerString(TAXA_ENTREGA);
                    linhaTaxaEntrega[TAXA_ENTREGA_ID] = bd.LerInt(TAXA_ENTREGA_ID);
                    linhaTaxaEntrega[VENDA_ID] = bd.LerInt("ID");
                    linhaTaxaEntrega[CLIENTE] = bd.LerString("Cliente");
                    linhaTaxaEntrega[CLIENTE_EMAIL] = bd.LerString("ClienteEmail");
                    linhaTaxaEntrega[TAXA_ENTREGA_TIPO] = bd.LerString("TaxaEntregaTipo");
                    linhaTaxaEntrega[SENHA] = bd.LerString("Senha");
                    taxaEntrega.Rows.Add(linhaTaxaEntrega);

                    info.Rows.Add(linha);
                }
                bd.Fechar();

                if (info.Rows.Count > 0)
                {

                    sql = "EXEC Proc_IngressosImpressaoPorVenda4 " + vendaID + ", 0, " + lojaID;

                    bd.Consulta(sql);

                    int diasTriagem = 0;
                    DateTime dataVenda = DateTime.MinValue;
                    DateTime dataEvento = DateTime.MinValue;

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
                            linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                            linha["LocalPais"] = bd.LerString("Pais");
                            linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                            linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                            linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
                            linha["TipoVenda"] = bd.LerString("TipoVenda");
                            linha["DiasTriagem"] = bd.LerInt("DiasTriagem");
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

                            diasTriagem = bd.LerInt("DiasTriagem");
                            dataVenda = bd.LerDateTime("DataVenda");
                            dataEvento = bd.LerDateTime("Horario");

                            if (diasTriagem > 0)
                            {
                                if (this.SoImprimirDataApresentacao && dataEvento.Subtract(DateTime.Now).Days <= 0)
                                    linha["DiasTriagem"] = 0;
                                else if (this.SoImprimirDataApresentacao)
                                    linha["DiasTriagem"] = 1;
                                else if (dataVenda.AddDays(diasTriagem) <= DateTime.Now)
                                    linha["DiasTriagem"] = 0;
                                else if (Utilitario.DiasUteisPeriodo(DateTime.Now, dataEvento) <= 2)
                                    linha["DiasTriagem"] = 0;
                                else
                                    linha["DiasTriagem"] = 1;
                            }
                            else
                                linha["DiasTriagem"] = 0;

                            tabelaImpressao.Rows.Add(linha);

                            DataRow linhaGrid = grid.NewRow();

                            linhaGrid[EVENTO] = (string)linha["Evento"];
                            linhaGrid[HORARIO] = (DateTime)linha["Horario"];
                            linhaGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                            if ((string)linha["Cortesia"] != string.Empty)
                                linhaGrid[CORTESIA] = (string)linha["Cortesia"];
                            linhaGrid[PRECO] = (string)linha["Preco"];
                            linhaGrid[VALOR] = (decimal)linha["Valor"];
                            linhaGrid[CODIGO] = (string)linha["Codigo"];

                            linhaGrid[STATUS] = Ingresso.StatusDescritivo((string)linha["Status"]);
                            linhaGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linha["Status"].ToString(), bd.LerString("TaxaEntregaTipo"));
                            linhaGrid[INGRESSO_ID] = (int)linha["IngressoID"];
                            linhaGrid[LOG_ID] = (int)linha["ID"];
                            linhaGrid[TIPO_VENDA] = (string)linha["TipoVenda"];
                            linhaGrid[DIAS_TRIAGEM] = (int)linha["DiasTriagem"];

                            grid.Rows.Add(linhaGrid);

                        }
                    }

                    bd.Fechar();

                }

                retorno.Tables.Add(grid);
                retorno.Tables.Add(tabelaImpressao);
                retorno.Tables.Add(info);
                retorno.Tables.Add(taxaEntrega);
                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string MontarBuscaIngressos(string clausula)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.TaxaEntregaID, IsNull(e.Tipo, '') AS TaxaEntregaTipo, IsNull(e.Nome, '') AS TaxaEntrega, vb.ValorTotal, ");
            sb.Append("vb.Status, vb.Senha, vb.DataVenda, c.Nome AS Cliente, c.Email AS ClienteEmail, c.CNPJ, c.NomeFantasia, tCanal.Nome AS Canal, ");
            sb.Append("tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, IsNull(c.StatusAtual, '') AS ClienteStatus, vb.PagamentoProcessado ");
            sb.Append("FROM tVendaBilheteria AS vb  (NOLOCK) ");
            sb.Append("INNER JOIN  tCaixa  (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
            sb.Append("INNER JOIN tUsuario (NOLOCK) AS u ON tCaixa.UsuarioID=u.ID ");
            sb.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
            sb.Append("INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
            sb.Append("LEFT JOIN tCliente AS c (NOLOCK) ON vb.ClienteID=c.ID ");
            sb.Append("LEFT JOIN tEntregaControle tc (NOLOCK) ON vb.EntregaControleID = tc.ID ");
            sb.Append("LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID ");
            sb.Append(clausula);

            return sb.ToString();
        }

        public EstruturaRetornoVendaValeIngresso PesquisarCodigoBarrasValeIngresso(string codigoBarra, string login)
        {
            BD bd = new BD();
            try
            {
                EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
                retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                EstruturaImpressaoVir retornoItem;

                bd.Consulta(this.MontarBuscaValeIngressos("WHERE viLog.CodigoBarra ='" + codigoBarra + "'"));

                if (bd.Consulta().Read())
                {
                    retorno.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    retorno.TaxaConvenienciaValorTotal = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    retorno.ValorTotalEntrega = bd.LerDecimal("TaxaEntregaValor");
                    retorno.ValorTotalVenda = bd.LerDecimal("ValorTotal");
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.Senha = bd.LerString("Senha");
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        retorno.ClienteNome = bd.LerString("Cliente");
                    else
                        retorno.ClienteNome = bd.LerString("NomeFantasia");
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.DataAberturaCaixa = bd.LerDateTime("DataCaixa");
                    retorno.ComprovanteQuantidade = 0;
                    retorno.ClienteEnderecoID = bd.LerInt("ClienteEnderecoID");
                    retorno.TaxaEntrega = bd.LerString("TaxaEntrega");
                    retorno.TaxaEntregaTipo = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                }
                bd.FecharConsulta();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT vi.ID , vit.CodigoTrocaFixo, vit.SaudacaoNominal, vi.CodigoBarra, vi.CodigoTroca vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                stbSQL.Append("vit.Valor, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status, IsNull(vb.NomeCartao, '') AS NomeCartao, vit.ValorTipo ");
                stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND viLog.VendaBilheteriaID = " + retorno.VendaBilheteriaID + " ORDER BY viLog.TimeStamp");

                bd.Consulta(stbSQL.ToString());

                string validadeData = string.Empty;
                string codigoTrocaFixo = string.Empty;
                while (bd.Consulta().Read())
                {
                    retornoItem = new EstruturaImpressaoVir();
                    retornoItem.ValeIngressoID = bd.LerInt("ID");
                    codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    retornoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                    retornoItem.CodigoTroca = codigoTrocaFixo.Length > 0 ? codigoTrocaFixo : bd.LerString("CodigoTroca");
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
                        case (char)ValeIngresso.enumStatus.Trocado:
                            retornoItem.Status = ValeIngresso.enumStatus.Trocado;
                            break;
                        case (char)ValeIngresso.enumStatus.Aguardando:
                            retornoItem.Status = ValeIngresso.enumStatus.Aguardando;
                            break;
                        case (char)ValeIngresso.enumStatus.Cancelado:
                            retornoItem.Status = ValeIngresso.enumStatus.Cancelado;
                            break;
                        case (char)ValeIngresso.enumStatus.Entregue:
                            retornoItem.Status = ValeIngresso.enumStatus.Entregue;
                            break;
                        case (char)ValeIngresso.enumStatus.Expirado:
                            retornoItem.Status = ValeIngresso.enumStatus.Expirado;
                            break;
                    }
                    retornoItem.ClientePresenteado = bd.LerString("ClienteNome");
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

        public EstruturaRetornoVendaValeIngresso PesquisarSenhaValeIngresso(string senha, string login)
        {
            BD bd = new BD();
            try
            {
                EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
                retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                EstruturaImpressaoVir retornoItem;

                decimal valorTotal, taxaEntregaValor;

                bd.Consulta(this.MontarBuscaValeIngressos("WHERE vb.Senha='" + senha + "'"));
                if (bd.Consulta().Read())
                {

                    valorTotal = bd.LerDecimal("ValorTotal");
                    taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");

                    retorno.VendaBilheteriaID = bd.LerInt("ID");
                    retorno.ClienteID = bd.LerInt("ClienteID");

                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        retorno.ClienteNome = bd.LerString("Cliente");
                    else
                        retorno.ClienteNome = bd.LerString("NomeFantasia");

                    retorno.ValorTotalEntrega = taxaEntregaValor;
                    retorno.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    retorno.DataAberturaCaixa = bd.LerDateTime("DataCaixa");
                    retorno.ValorTotalVenda = valorTotal;
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");
                    retorno.ClienteEnderecoID = bd.LerInt("ClienteEnderecoID");
                    retorno.TaxaEntrega = bd.LerString("TaxaEntrega");
                    retorno.TaxaEntregaTipo = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                    retorno.EntregaNome = bd.LerString("EntregaNome");
                    retorno.PeriodoEntrega = bd.LerString("PeriodoEntrega");
                    DateTime DataAux = bd.LerDateTime("DataEntrega");
                    retorno.DataEntrega = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "";
                    retorno.AreaEntrega = bd.LerString("AreaEntrega");
                    retorno.ClienteEmail = bd.LerString("ClienteEmail");
                    retorno.Senha = senha;
                }
                bd.FecharConsulta();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT vi.ID , vit.CodigoTrocaFixo, vi.CodigoBarra, vi.CodigoTroca, vit.SaudacaoNominal,  vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                stbSQL.Append("vit.Valor, vit.ValorTipo,vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao ");
                stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND viLog.VendaBilheteriaID = " + retorno.VendaBilheteriaID + " ORDER BY viLog.TimeStamp");

                bd.Consulta(stbSQL.ToString());

                string validadeData = string.Empty;
                string codigoTrocaFixo = string.Empty;
                while (bd.Consulta().Read())
                {
                    retornoItem = new EstruturaImpressaoVir();
                    retornoItem.ValeIngressoID = bd.LerInt("ID");
                    codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    retornoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                    retornoItem.CodigoTroca = codigoTrocaFixo.Length > 0 ? codigoTrocaFixo : bd.LerString("CodigoTroca");
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
                        case (char)ValeIngresso.enumStatus.Trocado:
                            retornoItem.Status = ValeIngresso.enumStatus.Trocado;
                            break;
                        case (char)ValeIngresso.enumStatus.Aguardando:
                            retornoItem.Status = ValeIngresso.enumStatus.Aguardando;
                            break;
                        case (char)ValeIngresso.enumStatus.Cancelado:
                            retornoItem.Status = ValeIngresso.enumStatus.Cancelado;
                            break;
                        case (char)ValeIngresso.enumStatus.Entregue:
                            retornoItem.Status = ValeIngresso.enumStatus.Entregue;
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

        public EstruturaRetornoVendaValeIngresso PesquisarVendaValeIngresso(int vendaID, string login)
        {
            BD bd = new BD();
            try
            {
                EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
                retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                EstruturaImpressaoVir retornoItem;

                bd.Consulta(this.MontarBuscaValeIngressos("WHERE vb.ID=" + vendaID));

                decimal valorTotal, taxaEntregaValor;
                if (bd.Consulta().Read())
                {
                    valorTotal = bd.LerDecimal("ValorTotal");
                    taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");

                    retorno.VendaBilheteriaID = bd.LerInt("ID");
                    retorno.ClienteID = bd.LerInt("ClienteID");
                    if (string.IsNullOrEmpty(bd.LerString("CNPJ")))
                        retorno.ClienteNome = bd.LerString("Cliente");
                    else
                        retorno.ClienteNome = bd.LerString("NomeFantasia");
                    retorno.ValorTotalEntrega = taxaEntregaValor;
                    retorno.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                    retorno.DataVenda = bd.LerDateTime("DataVenda");
                    retorno.DataAberturaCaixa = bd.LerDateTime("DataCaixa");
                    retorno.ValorTotalVenda = valorTotal;
                    retorno.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                    retorno.CanalVenda = bd.LerString("Canal");
                    retorno.Vendedor = bd.LerString("Vendedor");
                    retorno.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");
                    retorno.ClienteEnderecoID = bd.LerInt("ClienteEnderecoID");
                    retorno.TaxaEntrega = bd.LerString("TaxaEntrega");
                    retorno.TaxaEntregaTipo = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                    retorno.Senha = bd.LerString("Senha");
                }
                bd.FecharConsulta();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT vi.ID , vit.CodigoTrocaFixo,vit.SaudacaoNominal, vi.CodigoBarra, vi.CodigoTroca, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                stbSQL.Append("         vit.Valor,vit.ValorTipo , vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao ");
                stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                stbSQL.Append("WHERE viLog.Acao ='" + IngressoLog.VENDER + "' AND viLog.VendaBilheteriaID = " + retorno.VendaBilheteriaID + " ORDER BY viLog.TimeStamp");

                bd.Consulta(stbSQL.ToString());

                string validadeData = string.Empty;
                string codigoTrocaFixo = string.Empty;
                while (bd.Consulta().Read())
                {
                    retornoItem = new EstruturaImpressaoVir();
                    retornoItem.ValeIngressoID = bd.LerInt("ID");
                    codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    retornoItem.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                    retornoItem.CodigoTroca = codigoTrocaFixo.Length > 0 ? codigoTrocaFixo : bd.LerString("CodigoTroca");
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
                        case (char)ValeIngresso.enumStatus.Trocado:
                            retornoItem.Status = ValeIngresso.enumStatus.Trocado;
                            break;
                        case (char)ValeIngresso.enumStatus.Aguardando:
                            retornoItem.Status = ValeIngresso.enumStatus.Aguardando;
                            break;
                        case (char)ValeIngresso.enumStatus.Cancelado:
                            retornoItem.Status = ValeIngresso.enumStatus.Cancelado;
                            break;
                        case (char)ValeIngresso.enumStatus.Entregue:
                            retornoItem.Status = ValeIngresso.enumStatus.Entregue;
                            break;
                        case (char)ValeIngresso.enumStatus.Expirado:
                            retornoItem.Status = ValeIngresso.enumStatus.Expirado;
                            break;
                    }
                    retornoItem.ClientePresenteado = bd.LerString("ClienteNome");
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

        private string MontarBuscaValeIngressos(string clausula)
        {
            StringBuilder stbSQL = new StringBuilder();

            stbSQL.Append("SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.ClienteID, vb.ClienteEnderecoID, vb.TaxaEntregaValor, vb.ValorTotal, ");
            stbSQL.Append("vb.Status, vb.Senha, vb.DataVenda, IsNull(c.Nome, '') AS Cliente,  IsNull(c.Email, '') AS ClienteEmail,c.CNPJ, c.NomeFantasia, tCanal.Nome AS Canal, ");
            stbSQL.Append("tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, tCanal.ComprovanteQuantidade, ");
            stbSQL.Append("IsNULL(e.Tipo, '') AS TaxaEntregaTipo, e.Nome AS TaxaEntrega, ");
            stbSQL.Append("e.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
            stbSQL.Append("FROM tVendaBilheteria AS vb  (NOLOCK) ");
            stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
            stbSQL.Append("INNER JOIN tUsuario AS u  (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
            stbSQL.Append("INNER JOIN tLoja  (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
            stbSQL.Append("INNER JOIN tCanal  (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
            stbSQL.Append("LEFT JOIN tCliente AS c  (NOLOCK) ON vb.ClienteID=c.ID ");
            stbSQL.Append("LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID ");
            stbSQL.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON ec.ID = vb.EntregaControleID ");
            stbSQL.Append("LEFT JOIN tEntrega e (NOLOCK) ON e.ID = ec.EntregaID ");
            stbSQL.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = ec.EntregaAreaID ");
            stbSQL.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = ec.PeriodoID ");
            stbSQL.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");
            stbSQL.Append(clausula);
            return stbSQL.ToString();
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
    public class ImpressaoGerenciadorException : Exception
    {

        public ImpressaoGerenciadorException() : base() { }

        public ImpressaoGerenciadorException(string msg) : base(msg) { }

        public ImpressaoGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
