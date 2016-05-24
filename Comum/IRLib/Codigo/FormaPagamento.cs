using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{
    public class FormaPagamento : FormaPagamento_B
    {
        public const int TIPO_CARTAO_CREDITO = 0;
        public const int TIPO_CARTAO_DEBITO = 2;
        public const int TIPO_CHEQUE_1 = 8;
        public const int TIPO_CHEQUE_3 = 30;
        public const int TIPO_CHEQUE_5 = 148;
        public const int TIPO_CHEQUE_9 = 149;
        public const int DINHEIRO = 1;
        public const int VALE_INGRESSO = 137;
        public const int DESCONTO_ID = 147;
        public const int TIPO_TROCA = 159;

        public enum TIPO
        {
            [Description("Crédito")]
            Credito = 0,
            [Description("Débito")]
            Debito = 2,
            [Description("Dinheiro")]
            Dinheiro = 1,
            [Description("Boleto")]
            Boleto = 3,
            [Description("Cheque")]
            Cheque = 4,
            [Description("Troca")]
            Troca = 29,
        }

        public enum ESTADO
        {
            Aberto = 1,
            Pago = 2,
            Vencido = 3,
        }

        public const string DINHEIRO_NOME = "Dinheiro";
        public const string DEBITO_NOME = "Debito";
        public const string BOLETO_NOME = "Boleto";
        public const string TROCA_NOME = "Troca";

        public FormaPagamento() { }

        public FormaPagamento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public static List<EstruturaIDNome> Tipos(bool comDinheiro)
        {
            var lista = new List<EstruturaIDNome>
			{
				new EstruturaIDNome() {ID = -1, Nome = "Todos"},
				new EstruturaIDNome(){ ID = 0, Nome = "Crédito"},
				new EstruturaIDNome(){ ID = 2, Nome = "Débito"},
			};
            if (comDinheiro)
                lista.Insert(1, new EstruturaIDNome() { ID = 1, Nome = "Dinheiro" });
            return lista;
        }

        public string RetornaBandeira(int formaPagamentoID)
        {
            object retorno = bd.ConsultaValor(@"SELECT tBandeira.Nome FROM tFormaPagamento
			INNER JOIN tBandeira ON tBandeira.ID = tFormaPagamento.BandeiraID 
			WHERE tFormaPagamento.ID = " + formaPagamentoID);
            return (retorno is string) ? (string)retorno : "Outro";
        }

        public string RetornaBandeira()
        {
            return RetornaBandeira(this.Control.ID);
        }

        public List<int> GetFormasPagamentoPadrao()
        {
            List<int> lstFormaPagamento = new List<int>();

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                        "tFormaPagamento.ID " +
                    "FROM " +
                    "   tFormaPagamento (NOLOCK) " +
                    "WHERE " +
                    "   tFormaPagamento.Padrao = 'T'"))
                {
                    while (oDataReader.Read())
                    {
                        lstFormaPagamento.Add(bd.LerInt("ID"));
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return lstFormaPagamento;
        }

        public DataTable TodasBandeiras()
        {
            SqlCommand comando = new SqlCommand("SELECT DISTINCT ID, NOME FROM tBandeira", (SqlConnection)bd.Cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(comando);
            DataTable retorno = new DataTable();
            adapter.Fill(retorno);
            return retorno;
        }

        public DataTable getFormasPagamentoPorEventos(List<int> listaEventos, int cotaID)
        {
            try
            {
                BindingList<EstruturaFormaPagamento> lista = new BindingList<EstruturaFormaPagamento>();
                DataTable dtt = new DataTable();
                dtt.Columns.Add("CotaID", typeof(int));
                dtt.Columns.Add("FormaPagamentoID", typeof(int));
                dtt.Columns.Add("FormaPagamento", typeof(string));
                dtt.Columns.Add("Adicionar", typeof(bool));
                dtt.Columns.Add("AdicionarAntigo", typeof(bool));

                lista.AllowNew = true;
                lista.AllowEdit = true;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT fp.Nome, fp.ID, ISNULL(fpc.CotaID ,0) AS CotaID FROM tFormaPagamento fp (NOLOCK)");
                stbSQL.Append("INNER JOIN tFormaPagamentoEvento fpe (NOLOCK) ON fpe.FormaPagamentoID = fp.ID ");
                stbSQL.Append("LEFT JOIN tCotaFormaPagamento fpc (NOLOCK) ON fpc.FormaPagamentoID = fp.ID ");
                if (cotaID != 0)
                    stbSQL.Append(" AND fpc.CotaID =" + cotaID + " ");
                else
                    stbSQL.Append(" AND fpc.CotaID IS NULL ");
                stbSQL.Append("WHERE fpe.EventoID IN (");
                stbSQL.Append(Utilitario.ArrayToString(listaEventos.ToArray()) + ") AND fp.Nome NOT LIKE 'Itaucard%' ");
                stbSQL.Append("GROUP BY fp.Nome, fp.ID, fpc.CotaID ");
                stbSQL.Append("HAVING Count(Distinct fpe.EventoID) = " + listaEventos.Count);
                stbSQL.Append(" Order by ID");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    DataRow dtr = dtt.NewRow();
                    dtr["CotaID"] = bd.LerInt("CotaID");
                    dtr["FormaPagamentoID"] = bd.LerInt("ID");
                    dtr["FormaPagamento"] = bd.LerString("Nome");
                    if (bd.LerInt("CotaID") != 0)
                    {
                        dtr["Adicionar"] = true;
                        dtr["AdicionarAntigo"] = true;
                    }
                    else
                    {
                        dtr["Adicionar"] = false;
                        dtr["AdicionarAntigo"] = false;
                    }
                    dtt.Rows.Add(dtr);

                }
                return dtt;
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

        public DataTable getFormasPagamento(int cotaItemID)
        {
            try
            {
                DataTable dtt = new DataTable();
                dtt.Columns.Add("CotaItemID", typeof(int));
                dtt.Columns.Add("FormaPagamentoID", typeof(int));
                dtt.Columns.Add("FormaPagamento", typeof(string));
                dtt.Columns.Add("Adicionar", typeof(bool));
                dtt.Columns.Add("AdicionarAntigo", typeof(bool));
                dtt.Columns.Add("Tipo", typeof(int));

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT fp.Nome, fp.Tipo, fp.ID, ISNULL(fipc.CotaItemID ,0) AS CotaItemID FROM tFormaPagamento fp (NOLOCK) ");
                stbSQL.Append("LEFT JOIN tCotaItemFormaPagamento fipc (NOLOCK) ON fipc.FormaPagamentoID = fp.ID ");
                if (cotaItemID == 0)
                    stbSQL.Append("AND fipc.CotaItemID IS NULL ");
                else
                    stbSQL.Append("AND fipc.CotaItemID = " + cotaItemID);

                stbSQL.Append("WHERE fp.Ativo = 'T' ORDER BY fp.Nome");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    DataRow dtr = dtt.NewRow();
                    dtr["CotaItemID"] = bd.LerInt("CotaItemID");
                    dtr["FormaPagamentoID"] = bd.LerInt("ID");
                    dtr["FormaPagamento"] = bd.LerString("Nome");
                    if (bd.LerInt("CotaItemID") != 0)
                    {
                        dtr["Adicionar"] = true;
                        dtr["AdicionarAntigo"] = true;
                    }
                    else
                    {
                        dtr["Adicionar"] = false;
                        dtr["AdicionarAntigo"] = false;
                    }
                    dtr["Tipo"] = bd.LerInt("Tipo");
                    dtt.Rows.Add(dtr);
                }
                return dtt;
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

        public DataTable BuscaFormasPagamento(int idCotaItem)
        {
            try
            {
                string sql = @"SELECT  ID,
		                        CotaItemID,
		                        FormaPagamentoID
                            FROM tCotaItemFormaPagamento (NOLOCK)
                            WHERE CotaItemID = @CotaItemID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@CotaItemID", Value = idCotaItem, DbType = DbType.Int32 });

                bd.Consulta(sql, parametros);

                DataTable table = new DataTable();
                table.Columns.Add("ID");
                table.Columns.Add("CotaItemID");
                table.Columns.Add("FormaPagamentoID");

                DataRow row;
                while (bd.Consulta().Read())
                {
                    row = table.NewRow();
                    row["ID"] = bd.LerInt("ID");
                    row["CotaItemID"] = bd.LerInt("CotaItemID");
                    row["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    table.Rows.Add(row);
                }
                return table;
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

        public DataTable Todas(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("FormaPagamento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql = "SELECT ID,Nome FROM tFormaPagamento ORDER BY Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        public override DataTable Todas()
        {

            DataTable tabela = new DataTable("FormaPagamento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                        "  tFormaPagamento.ID, " +
                        "  tFormaPagamento.Nome " +
                        "FROM " +
                        "   tFormaPagamento (NOLOCK) " +
                        "  ORDER BY " +
                        "  tFormaPagamento.Nome"))
                {
                    DataRow linha;
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();

                return tabela;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public decimal QuantidadeIngressosPorApresentacoesCanais(string apresentacaoesInformados, string canaisInformados, string acao)
        {

            try
            {
                decimal quantidade = 0;
                if (apresentacaoesInformados != "" && canaisInformados != "" && acao != "")
                {
                    string condicaoFormaPagamento = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                    else
                        condicaoFormaPagamento = " "; // todos se for = zero
                    string sqlSenha;
                    // Este SELECT repete as senhas propositalmente, pois para cada registro deve dividir pela quantidade de parcelas
                    sqlSenha =
                        @"
						SELECT SUM(Quantidade) as Quantidade FROM (
						SELECT 
						Count(tIngressoLog.ID) AS Quantidade/*, tVendaBilheteria.Senha */
						FROM tIngressoLog 
						INNER JOIN tVendaBilheteria ON tVendaBilheteria.ID = tIngressoLog.VendaBilheteriaID
						INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID 
						INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID AND tPreco.ID = tIngresso.PrecoID 
						INNER JOIN tVendaBilheteriaFormaPagamento ON tIngressoLog.VendaBilheteriaID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID 
						WHERE
						
						(tIngresso.ApresentacaoID IN (" + apresentacaoesInformados + @")) AND (tIngressoLog.CanalID IN (" + canaisInformados + @")) 
						AND (tIngressoLog.Acao = N'" + acao + @"') " + condicaoFormaPagamento +
                        @" GROUP BY Senha ) Teste";
                    ;
                    bd.Consulta(sqlSenha);

                    if (bd.Consulta().Read())
                        quantidade += bd.LerDecimal("Quantidade");

                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorApresentacoesCanais

        public decimal ValorIngressosPorApresentacoesCanais(string apresentacaoesInformados, string canaisInformados, string acaoInformada)
        {
            try
            {
                decimal valor = 0;
                if (apresentacaoesInformados != "" && canaisInformados != "" && acaoInformada != "")
                {
                    string condicaoFormaPagamento = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoFormaPagamento = " AND (FormaPagamentoID = " + this.Control.ID + ") ";
                    else
                        condicaoFormaPagamento = " "; // todos se for = zero
                    string sqlValor;
                    sqlValor =
                        @"SELECT 
							SUM(tPreco.Valor * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor 

							FROM tVendaBilheteriaFormaPagamento

							INNER JOIN tIngressoLog ON tIngressoLog.VendaBilheteriaID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID 
							INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID 
							INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
						WHERE  (tIngresso.ApresentacaoID IN (" + apresentacaoesInformados + @")) AND (tIngressoLog.CanalID IN (" + canaisInformados + @")) 
						AND (tIngressoLog.Acao = N'" + acaoInformada + @"') " + condicaoFormaPagamento; ;
                    bd.Consulta(sqlValor);
                    if (bd.Consulta().Read())
                    {
                        valor = bd.LerDecimal("Valor");
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorApresentacoesCanais

        public decimal QuantidadeIngressosPorCaixaEventoCanalLoja(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID, string acao)
        {
            try
            {
                decimal quantidade = 0;
                if (dataInicial == "" || dataFinal == "" || eventoID < 0 || canalID < 0)
                    return Int32.MinValue;
                if (eventoID == 0 && canalID == 0)
                    return Int32.MinValue;
                // Obtendo dados
                string condicaoFormaPagamento = "";
                if (this.Control.ID > 0)
                    condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                else
                    condicaoFormaPagamento = " "; // todos se for = zero
                string sqlSenha =
                    "SELECT  DISTINCT tIngressoLog.ID AS IngressoLogID, tVendaBilheteria.Senha " +
                    "FROM     tCaixa INNER JOIN " +
                    "tIngressoLog ON tCaixa.ID = tIngressoLog.CaixaID INNER JOIN " +
                    "tVendaBilheteria INNER JOIN " +
                    "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID " +
                    "WHERE    (tIngressoLog.Acao = '" + acao + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura < '" + dataFinal + "')  " + condicaoFormaPagamento;
                //
                // A cláusula DISTINCT é necessária porque uma venda pode ter sido cancelado pela mesma forma de recebimento em mais de um registro
                //
                if (canalID > 0 && eventoID == 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlSenha = sqlSenha + " AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlSenha = sqlSenha + " AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                if (canalID == 0 && eventoID > 0)
                {
                    sqlSenha = sqlSenha + " AND (tIngresso.EventoID = " + eventoID + ") ";
                }
                if (canalID > 0 && eventoID > 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlSenha = sqlSenha + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlSenha = sqlSenha + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                bd.Consulta(sqlSenha);
                // Obter a senha
                // Para cada senha encontrada, verificar quantidade de ingressos e parcelas de forma de pagamento
                while (bd.Consulta().Read())
                {
                    // Cada ingresso deve ser dividido pela quantidade de parcelas da sua venda
                    VendaBilheteria vendaBilheteria = new VendaBilheteria();
                    vendaBilheteria.Senha.Valor = bd.LerString("Senha");
                    quantidade = quantidade + (1 / Convert.ToDecimal(vendaBilheteria.QuantidadePagamentos()));
                }
                bd.Fechar();
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de 

        public decimal ValorIngressosPorCaixaEventoCanalLoja(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID, string acao)
        {
            try
            {
                decimal valor = 0;
                if (dataInicial == "" || dataFinal == "" || eventoID < 0 || canalID < 0)
                    return Int32.MinValue;
                if (eventoID == 0 && canalID == 0)
                    return Int32.MinValue;
                string condicaoFormaPagamento = "";
                // Obtendo dados
                if (this.Control.ID > 0)
                    condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                else
                    condicaoFormaPagamento = " "; // todos se for = zero
                string sqlValor;
                sqlValor =
                    "SELECT   SUM(tPreco.Valor * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor " +
                    "FROM     tCaixa INNER JOIN " +
                    "tPreco INNER JOIN " +
                    "tIngressoLog ON tPreco.ID = tIngressoLog.PrecoID ON tCaixa.ID = tIngressoLog.CaixaID INNER JOIN " +
                    "tVendaBilheteriaFormaPagamento INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID " +
                    "WHERE    (tIngressoLog.Acao = '" + acao + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura < '" + dataFinal + "')  " + condicaoFormaPagamento;
                if (canalID > 0 && eventoID == 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlValor = sqlValor + " AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlValor = sqlValor + " AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                if (canalID == 0 && eventoID > 0)
                {
                    sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") ";
                }
                if (canalID > 0 && eventoID > 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                bd.Consulta(sqlValor);
                if (bd.Consulta().Read())
                {
                    valor = bd.LerDecimal("Valor");
                }
                bd.Fechar();
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorFormaPagamento

        public decimal ValorConvenienciaPorCaixaEventoCanalLoja(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID, string acao)
        {
            try
            {
                decimal valor = 0;
                if (dataInicial == "" || dataFinal == "" || eventoID < 0 || canalID < 0)
                    return Int32.MinValue;
                if (eventoID == 0 && canalID == 0)
                    return Int32.MinValue;
                string condicaoFormaPagamento = "";
                // Obtendo dados
                if (this.Control.ID > 0)
                    condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                else
                    condicaoFormaPagamento = " "; // todos se for = zero
                string sqlValor;
                sqlValor =
                    "SELECT        SUM(tPreco.Valor * tVendaBilheteriaItem.TaxaConveniencia / 100 * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor " +
                    "FROM            tCaixa INNER JOIN " +
                    "tPreco INNER JOIN " +
                    "tIngressoLog ON tPreco.ID = tIngressoLog.PrecoID ON tCaixa.ID = tIngressoLog.CaixaID INNER JOIN " +
                    "tVendaBilheteriaFormaPagamento INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID " +
                    "WHERE    (tIngressoLog.Acao = '" + acao + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura < '" + dataFinal + "')  " + condicaoFormaPagamento;
                if (canalID > 0 && eventoID == 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlValor = sqlValor + " AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlValor = sqlValor + " AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                if (canalID == 0 && eventoID > 0)
                {
                    sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") ";
                }
                if (canalID > 0 && eventoID > 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                bd.Consulta(sqlValor);
                if (bd.Consulta().Read())
                {
                    valor = bd.LerDecimal("Valor");
                }
                bd.Fechar();
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorFormaPagamento

        public decimal ValorEntregaPorCaixaEventoCanalLoja(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID, string acao)
        {
            try
            {
                decimal valor = 0;
                if (dataInicial == "" || dataFinal == "" || eventoID < 0 || canalID < 0)
                    return Int32.MinValue;
                if (eventoID == 0 && canalID == 0)
                    return Int32.MinValue;
                string condicaoFormaPagamento = "";
                // Obtendo dados
                if (this.Control.ID > 0)
                    condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                else
                    condicaoFormaPagamento = " "; // todos se for = zero
                string sqlValor;
                sqlValor =
                    "SELECT  tIngressoLog.ID AS IngressoLogID, tVendaBilheteria.Senha, tVendaBilheteria.TaxaEntregaValor " +
                    "FROM     tCaixa INNER JOIN " +
                    "tPreco INNER JOIN " +
                    "tIngressoLog ON tPreco.ID = tIngressoLog.PrecoID ON tCaixa.ID = tIngressoLog.CaixaID INNER JOIN " +
                    "tVendaBilheteriaFormaPagamento INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID " +
                    "WHERE    (tIngressoLog.Acao = '" + acao + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura < '" + dataFinal + "')  " + condicaoFormaPagamento;
                if (canalID > 0 && eventoID == 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlValor = sqlValor + " AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlValor = sqlValor + " AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                if (canalID == 0 && eventoID > 0)
                {
                    sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") ";
                }
                if (canalID > 0 && eventoID > 0)
                {
                    if (lojaID > 0)
                    { // se tem canal tem loja
                        sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    { // lojas agrupadas (loja =0)
                        sqlValor = sqlValor + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                bd.Consulta(sqlValor);
                while (bd.Consulta().Read())
                {
                    // Cada ingresso deve ser dividido pela quantidade de parcelas da sua venda
                    VendaBilheteria vendaBilheteria = new VendaBilheteria();
                    vendaBilheteria.Senha.Valor = bd.LerString("Senha");
                    decimal valorEntregaPorSenha = bd.LerDecimal("TaxaEntregaValor");
                    if (valorEntregaPorSenha > 0)
                    {
                        valor = valor +
                            (
                            valorEntregaPorSenha / Convert.ToDecimal(vendaBilheteria.QuantidadeIngressos())
                            / Convert.ToDecimal(vendaBilheteria.QuantidadePagamentos())
                            );
                    }
                }
                bd.Fechar();
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de 

        public override decimal QuantidadeIngressosPorFormaPagamento(string ingressoLogIDs)
        {
            try
            {
                decimal quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoFormaPagamento = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                    else
                        condicaoFormaPagamento = " "; // todos se for = zero
                    string sqlSenha;
                    sqlSenha =
                        "SELECT   tIngressoLog.ID AS IngressoLogID, tVendaBilheteria.Senha " +
                        "FROM     tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoFormaPagamento;
                    //						"SELECT DISTINCT tVendaBilheteria.Senha "+
                    //						"FROM tVendaBilheteria INNER JOIN "+
                    //						"tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN "+
                    //						"tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN "+
                    //						"tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN "+
                    //						"tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN "+
                    //						"tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                    //						"tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                    //						"WHERE (tIngressoLog.ID IN ("+ingressoLogIDs+")) " + condicaoFormaPagamento;
                    bd.Consulta(sqlSenha);
                    // Obter a senha
                    // Para cada senha encontrada, verificar quantidade de ingressos e parcelas de forma de pagamento
                    while (bd.Consulta().Read())
                    {
                        // Cada ingresso deve ser dividido pela quantidade de parcelas da sua venda
                        VendaBilheteria vendaBilheteria = new VendaBilheteria();
                        vendaBilheteria.Senha.Valor = bd.LerString("Senha");
                        quantidade = quantidade + (1 / Convert.ToDecimal(vendaBilheteria.QuantidadePagamentos()));
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de 

        public override decimal ValorIngressosPorFormaPagamento(string ingressoLogIDs)
        {
            try
            {
                decimal valor = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoFormaPagamento = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoFormaPagamento = " AND (tFormaPagamento.ID = " + this.Control.ID + ") ";
                    else
                        condicaoFormaPagamento = " "; // todos se for = zero
                    string sqlValor;
                    sqlValor =
                        "SELECT SUM(tPreco.Valor * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor " +
                        "FROM tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoFormaPagamento;
                    //						"SELECT DISTINCT tVendaBilheteria.Senha "+
                    //						"FROM tVendaBilheteria INNER JOIN "+
                    //						"tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN "+
                    //						"tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN "+
                    //						"tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN "+
                    //						"tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN "+
                    //						"tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                    //						"tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                    //						"WHERE (tIngressoLog.ID IN ("+ingressoLogIDs+")) ";
                    // bd.Consulta(sqlValor);
                    //					// Obter a senha
                    //					string senhaObtida="";
                    //					if (bd.Consulta().Read()) {
                    //						senhaObtida += "'"+bd.LerString("Senha")+"'";
                    //					}
                    //					while (bd.Consulta().Read()) {
                    //						senhaObtida = senhaObtida +","+"'"+bd.LerString("Senha")+"'";
                    //					}
                    //					if (senhaObtida!="") {
                    //						// Com a senha obter valores por forma de pagamento
                    //						string sqlValor;
                    //						// sem taxas
                    //						sqlValor = 
                    //							"SELECT SUM((tVendaBilheteria.ValorTotal - tVendaBilheteria.TaxaEntregaValor - tVendaBilheteria.TaxaConvenienciaValorTotal) * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor "+
                    //							"FROM tVendaBilheteria INNER JOIN "+
                    //                         "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                    //                         "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                    //							"WHERE  (tVendaBilheteria.Senha IN ("+senhaObtida+")) "+ condicaoFormaPagamento; 
                    // com taxas
                    //						sqlValor = 
                    //							"SELECT        SUM(tVendaBilheteriaFormaPagamento.Valor) AS Valor "+
                    //							"FROM          tVendaBilheteria INNER JOIN "+
                    //							"tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                    //							"tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                    //							"WHERE        (tVendaBilheteria.Senha IN ("+senhaObtida+")) "+ condicaoFormaPagamento;
                    bd.Consulta(sqlValor);
                    if (bd.Consulta().Read())
                    {
                        valor = bd.LerDecimal("Valor");
                    }
                    //				}
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorFormaPagamento

        public override decimal ValorConvenienciaPorFormaPagamento(string ingressoLogIDs)
        {
            try
            {
                decimal valor = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoFormaPagamento = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoFormaPagamento = " AND (tFormaPagamento.ID = " + this.Control.ID + ") ";
                    else
                        condicaoFormaPagamento = " "; // todos se for = zero
                    string sqlValor;
                    sqlValor =
                        "SELECT   SUM((tPreco.Valor * tVendaBilheteriaItem.TaxaConveniencia/100) * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor " +
                        "FROM     tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoFormaPagamento;
                    //
                    //					string sqlSenha;
                    //					sqlSenha = 
                    //						"SELECT DISTINCT tVendaBilheteria.Senha "+
                    //						"FROM tVendaBilheteria INNER JOIN "+
                    //						"tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN "+
                    //						"tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN "+
                    //						"tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN "+
                    //						"tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN "+
                    //						"tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                    //						"tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                    //						"WHERE (tIngressoLog.ID IN ("+ingressoLogIDs+")) ";
                    //					bd.Consulta(sqlSenha);
                    //					// Obter a senha
                    //					string senhaObtida="";
                    //					if (bd.Consulta().Read()) {
                    //						senhaObtida += "'"+bd.LerString("Senha")+"'";
                    //					}
                    //					while (bd.Consulta().Read()) {
                    //						senhaObtida = senhaObtida +","+"'"+bd.LerString("Senha")+"'";
                    //					}
                    //					if (senhaObtida!="") {
                    //						// Com a senha obter valores por forma de pagamento
                    //						string sqlValor;
                    //						sqlValor = 
                    //							"SELECT SUM(tVendaBilheteria.TaxaConvenienciaValorTotal* tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Valor "+
                    //							"FROM tVendaBilheteria INNER JOIN "+
                    //							"tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                    //							"tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                    //							"WHERE  (tVendaBilheteria.Senha IN ("+senhaObtida+")) "+ condicaoFormaPagamento; 
                    bd.Consulta(sqlValor);
                    if (bd.Consulta().Read())
                    {
                        valor = bd.LerDecimal("Valor");
                    }
                    //					}
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorConvenienciaPorFormaPagamento

        public override decimal ValorEntregaPorFormaPagamento(string ingressoLogIDs)
        {
            try
            {
                decimal valor = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoFormaPagamento = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoFormaPagamento = " AND (tVendaBilheteriaFormaPagamento.FormaPagamentoID = " + this.Control.ID + ") ";
                    else
                        condicaoFormaPagamento = " "; // todos se for = zero
                    string sql;
                    sql =
                        "SELECT  tIngressoLog.ID AS IngressoLogID, tVendaBilheteria.Senha, tVendaBilheteria.TaxaEntregaValor " +
                        "FROM    tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoFormaPagamento;
                    bd.Consulta(sql);
                    while (bd.Consulta().Read())
                    {
                        // Cada ingresso deve ser dividido pela quantidade de parcelas da sua venda
                        VendaBilheteria vendaBilheteria = new VendaBilheteria();
                        vendaBilheteria.Senha.Valor = bd.LerString("Senha");
                        valor = valor + (bd.LerDecimal("TaxaEntregaValor")
                            / Convert.ToDecimal(vendaBilheteria.QuantidadeIngressos())
                            / Convert.ToDecimal(vendaBilheteria.QuantidadePagamentos())
                            );
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de 

        public IRLib.ClientObjects.EstruturaVendaFormaPagamento getFormaPagamentoVendaAssinatura(string bandeira, int parcelas, int empresaID, int clienteID, int usuarioID)
        {
            try
            {
                string filtro = string.Empty;


                int tipoID = 0;
                if (string.Compare(bandeira, DINHEIRO_NOME, true) == 0)
                    tipoID = (int)TIPO.Dinheiro;
                else if (string.Compare(bandeira, DEBITO_NOME, true) == 0)
                    tipoID = (int)TIPO.Debito;
                else if (string.Compare(bandeira, TROCA_NOME, true) == 0)
                    tipoID = (int)TIPO.Troca;

                if (tipoID > 0 && (usuarioID == 0 || usuarioID == Usuario.INTERNET_USUARIO_ID))
                    throw new Exception("Somente operadores com canais diferentes do canal Internet podem pagar com Dinheiro ou Débito");


                if (tipoID > 0)
                {
                    if (tipoID == (int)TIPO.Troca)
                        filtro = "fp.Tipo = " + (int)TIPO.Dinheiro + " AND FormaPagamentoID = 159 ";
                    else
                        filtro = "fp.Tipo = " + tipoID + " ";
                    parcelas = 1;
                }
                else
                    filtro = "b.Nome = '" + bandeira.Replace("'", string.Empty) + "'";

                string sql = string.Format(
                    @"SELECT
							TOP 1
								fp.ID, fp.Tipo, fp.BandeiraID, fp.FormaPagamentoTipoID
							FROM tAssinaturaAcaoProvisoria acp (NOLOCK)
							INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ac.ID = acp.AssinaturaClienteID
							INNER JOIN tAssinaturaFormaPagamento afp (NOLOCK) ON afp.AssinaturaID = ac.AssinaturaID
							INNER JOIN tFormaPagamento fp (NOLOCK) ON afp.FormaPagamentoID = fp.ID
							INNER JOIN tBandeira b (NOLOCK) ON fp.BandeiraID = b.ID
							WHERE {0} AND fp.Nome NOT LIKE '%Itaucard%' AND fp.Parcelas = {1} AND acp.ClienteID = {2}",
                            filtro, parcelas, clienteID, empresaID);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível localizar a forma de pagamento selecionada, por favor tente novamente.");

                var Retorno = new EstruturaVendaFormaPagamento()
                {
                    FormaPagamentoID = bd.LerInt("ID"),
                    Tipo = bd.LerInt("Tipo"),
                    BandeiraID = bd.LerInt("BandeiraID"),
                    FormaPagamentoTipoID = bd.LerInt("FormaPagamentoTipoID"),
                };

                return Retorno;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IRLib.ClientObjects.EstruturaVendaFormaPagamento getFormaPagamentoVenda(int id, int empresaID)
        {
            try
            {
                IRLib.ClientObjects.EstruturaVendaFormaPagamento Retorno = new IRLib.ClientObjects.EstruturaVendaFormaPagamento();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT fp.ID, fp.Tipo, fp.Parcelas, fp.BandeiraID, efp.TaxaAdm, efp.Dias, fp.Padrao, fp.FormaPagamentoTipoID, efp.IR ");
                stbSQL.Append("FROM tFormaPagamento fp (NOLOCK) ");
                stbSQL.Append("INNER JOIN tEmpresaFormaPagamento (NOLOCK) efp ON efp.FormaPagamentoID = fp.ID ");
                stbSQL.Append("WHERE fp.ID = " + id + " AND EmpresaID = " + empresaID);

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    Retorno.FormaPagamentoID = bd.LerInt("ID");
                    Retorno.Tipo = bd.LerInt("Tipo");
                    Retorno.BandeiraID = bd.LerInt("BandeiraID");
                    Retorno.TaxaAdm = bd.LerDecimal("TaxaAdm");
                    Retorno.Dias = bd.LerInt("Dias");
                    Retorno.Padrao = bd.LerString("Padrao");
                    Retorno.IR = bd.LerBoolean("IR");
                    Retorno.FormaPagamentoTipoID = bd.LerInt("FormaPagamentoTipoID");
                }
                return Retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private decimal CalculaPct(object valor, object valorTotal)
        {
            try
            {
                return Convert.ToDecimal(Convert.ToDecimal((((decimal)valor * 100) / (decimal)valorTotal)).ToString(Utilitario.FormatoPorcentagem1Casa));

            }
            catch
            {
                return 0;
            }
        }

        private string VerificaCompute(object valor)
        {
            try
            {
                return Convert.ToDecimal(valor).ToString(Utilitario.FormatoMoeda); ;
            }
            catch
            {
                return "0";
            }
        }

        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                { // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                decimal quantidadeVendidosTotais = 0;
                decimal quantidadeCanceladosTotais = 0;
                decimal quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os dados na condição especificada
                // Filtrando as condições
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotaisTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsTotaisVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsTotaisCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                //				string ingressoLogIDsTotaisTotais = caixa.VendaSenhaPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos +"," +ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID);
                //				string ingressoLogIDsTotaisVendidos = caixa.VendaSenhaPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID);
                //				string ingressoLogIDsTotaisCancelados = caixa.VendaSenhaPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID);
                // Linhas do Grid
                string ingressoLogIDs = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDs);
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // canal zero pega todos
                decimal totaisVendidos = QuantidadeIngressosPorFormaPagamento(ingressoLogIDsTotaisVendidos);
                decimal totaisCancelados = QuantidadeIngressosPorFormaPagamento(ingressoLogIDsTotaisCancelados);
                decimal totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorFormaPagamento(ingressoLogIDsTotaisVendidos);
                decimal valoresCancelados = ValorIngressosPorFormaPagamento(ingressoLogIDsTotaisCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                #endregion
                // Para cada canal na condição especificada, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorFormaPagamento(ingressoLogIDsTotaisVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorFormaPagamento(ingressoLogIDsTotaisCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToDecimal(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToDecimal(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToDecimal(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorFormaPagamento(ingressoLogIDsTotaisVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorFormaPagamento(ingressoLogIDsTotaisCancelados);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                tabela.Columns["VariacaoLinha"].ColumnName = "FormaPagamento";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tFormaPagamento.ID AS FormaPagamentoID, tFormaPagamento.Nome AS FormaPagamento " +
                        "FROM    tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("FormaPagamentoID");
                        linha["VariacaoLinha"] = obterDados.LerString("FormaPagamento");
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public DataSet CarregarTodos()
        {
            try
            {
                DataSet ds = new DataSet();

                DataTable dttTipo = new DataTable("Tipo");
                dttTipo.Columns.Add("ID", typeof(int));
                dttTipo.Columns.Add("Nome", typeof(string));

                DataTable dttBandeira = new DataTable("Bandeira");
                dttBandeira.Columns.Add("ID", typeof(int));
                dttBandeira.Columns.Add("Nome", typeof(string));

                DataTable dttFormaPagamento = new DataTable("FormaPagamento");
                dttFormaPagamento.Columns.Add("ID", typeof(int));
                dttFormaPagamento.Columns.Add("Nome", typeof(string));
                dttFormaPagamento.Columns.Add("BandeiraID", typeof(int));
                dttFormaPagamento.Columns.Add("Bandeira", typeof(string));
                dttFormaPagamento.Columns.Add("TipoID", typeof(int));
                dttFormaPagamento.Columns.Add("Tipo", typeof(string));
                dttFormaPagamento.Columns.Add("Linha", typeof(string));

                string sql = string.Format(@"SELECT 
								fp.ID AS FormaPagamentoID, fp.Nome AS FormaPagamento, b.ID AS BandeiraID, b.Nome AS Bandeira, 
								t.ID AS TipoID, t.Nome AS Tipo, ROW_NUMBER() OVER (ORDER BY fp.Nome) AS Linha
							   FROM tFormaPagamento fp (NOLOCK)
							   INNER JOIN tBandeira b (NOLOCK) ON b.ID = fp.BandeiraID
							   INNER JOIN tFormaPagamentoTipo t (NOLOCK) ON t.ID = fp.FormaPagamentoTipoID
							   WHERE fp.ID <> {0}
							   ORDER BY fp.Nome ", FormaPagamento.VALE_INGRESSO);

                bd.Consulta(sql);

                int bandeiraID = 0;
                int tipoID = 0;
                DataRow dtr = null;
                while (bd.Consulta().Read())
                {

                    bandeiraID = bd.LerInt("BandeiraID");
                    tipoID = bd.LerInt("TipoID");

                    if (dttBandeira.Select("ID = " + bandeiraID).Length == 0)
                    {
                        dtr = dttBandeira.NewRow();
                        dttBandeira.Rows.Add(dtr);

                        dtr["ID"] = bandeiraID;
                        dtr["Nome"] = bd.LerString("Bandeira");
                    }

                    if (dttTipo.Select("ID = " + tipoID).Length == 0)
                    {
                        dtr = dttTipo.NewRow();
                        dttTipo.Rows.Add(dtr);

                        dtr["ID"] = tipoID;
                        dtr["Nome"] = bd.LerString("Tipo");
                    }

                    dtr = dttFormaPagamento.NewRow();
                    dttFormaPagamento.Rows.Add(dtr);

                    dtr["ID"] = bd.LerInt("FormaPagamentoID");
                    dtr["Nome"] = bd.LerString("FormaPagamento");
                    dtr["BandeiraID"] = bandeiraID;
                    dtr["Bandeira"] = bd.LerString("Bandeira");
                    dtr["TipoID"] = tipoID;
                    dtr["Tipo"] = bd.LerString("Tipo");
                    dtr["Linha"] = bd.LerInt("Linha").ToString("000000");
                }

                dtr = dttFormaPagamento.NewRow();
                dtr["ID"] = 0;
                dtr["Nome"] = "Todos";
                dtr["BandeiraID"] = 0;
                dtr["TipoID"] = 0;
                dtr["Linha"] = "000000";
                dttFormaPagamento.Rows.InsertAt(dtr, 0);

                ds.Tables.Add(dttTipo);
                ds.Tables.Add(dttBandeira);
                ds.Tables.Add(dttFormaPagamento);
                return ds;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataSet CarregarCreditoBoleto()
        {
            try
            {
                DataSet ds = new DataSet();

                DataTable dttTipo = new DataTable("Tipo");
                dttTipo.Columns.Add("ID", typeof(int));
                dttTipo.Columns.Add("Nome", typeof(string));

                DataTable dttBandeira = new DataTable("Bandeira");
                dttBandeira.Columns.Add("ID", typeof(int));
                dttBandeira.Columns.Add("Nome", typeof(string));

                DataTable dttFormaPagamento = new DataTable("FormaPagamento");
                dttFormaPagamento.Columns.Add("ID", typeof(int));
                dttFormaPagamento.Columns.Add("Nome", typeof(string));
                dttFormaPagamento.Columns.Add("BandeiraID", typeof(int));
                dttFormaPagamento.Columns.Add("Bandeira", typeof(string));
                dttFormaPagamento.Columns.Add("TipoID", typeof(int));
                dttFormaPagamento.Columns.Add("Tipo", typeof(string));
                dttFormaPagamento.Columns.Add("Linha", typeof(string));

                string sql = string.Format(@"SELECT 
								fp.ID AS FormaPagamentoID, fp.Nome AS FormaPagamento, b.ID AS BandeiraID, b.Nome AS Bandeira, 
								t.ID AS TipoID, t.Nome AS Tipo, ROW_NUMBER() OVER (ORDER BY fp.Nome) AS Linha
							   FROM tFormaPagamento fp (NOLOCK)
							   INNER JOIN tBandeira b (NOLOCK) ON b.ID = fp.BandeiraID
							   INNER JOIN tFormaPagamentoTipo t (NOLOCK) ON t.ID = fp.FormaPagamentoTipoID
							   WHERE fp.ID <> {0} 
							   ORDER BY fp.Nome ", FormaPagamento.VALE_INGRESSO);

                bd.Consulta(sql);

                int bandeiraID = 0;
                int tipoID = 0;
                DataRow dtr = null;
                while (bd.Consulta().Read())
                {

                    bandeiraID = bd.LerInt("BandeiraID");
                    tipoID = bd.LerInt("TipoID");

                    if (dttBandeira.Select("ID = " + bandeiraID).Length == 0)
                    {
                        dtr = dttBandeira.NewRow();
                        dttBandeira.Rows.Add(dtr);

                        dtr["ID"] = bandeiraID;
                        dtr["Nome"] = bd.LerString("Bandeira");
                    }

                    if (dttTipo.Select("ID = " + tipoID).Length == 0)
                    {
                        dtr = dttTipo.NewRow();
                        dttTipo.Rows.Add(dtr);

                        dtr["ID"] = tipoID;
                        dtr["Nome"] = bd.LerString("Tipo");
                    }

                    dtr = dttFormaPagamento.NewRow();
                    dttFormaPagamento.Rows.Add(dtr);

                    dtr["ID"] = bd.LerInt("FormaPagamentoID");
                    dtr["Nome"] = bd.LerString("FormaPagamento");
                    dtr["BandeiraID"] = bandeiraID;
                    dtr["Bandeira"] = bd.LerString("Bandeira");
                    dtr["TipoID"] = tipoID;
                    dtr["Tipo"] = bd.LerString("Tipo");
                    dtr["Linha"] = bd.LerInt("Linha").ToString("000000");
                }

                dtr = dttFormaPagamento.NewRow();
                dtr["ID"] = 0;
                dtr["Nome"] = "Todos";
                dtr["BandeiraID"] = 0;
                dtr["TipoID"] = 0;
                dtr["Linha"] = "000000";
                dttFormaPagamento.Rows.InsertAt(dtr, 0);

                ds.Tables.Add(dttTipo);
                ds.Tables.Add(dttBandeira);
                ds.Tables.Add(dttFormaPagamento);
                return ds;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string RedePreferencialFormaPagamentoID(int formapagamentoID)
        {
            try
            {
                string sql = string.Format(@"SELECT tfp.RedePreferencial FROM tFormaPagamento tfp WHERE tfp.ID = {0}", formapagamentoID);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return bd.LerString("RedePreferencial");
                else
                    return string.Empty;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string RedePreferencialFormaPagamentoID(int formapagamentoID, string numeroEstabelecimento)
        {
            try
            {
                string sql = string.Format(@"SELECT tfe.RedePreferencial FROM tFormaPagamentoEstabelecimento tfe WHERE FormaPagamentoID = {0} AND NroEstabelecimento = '{1}'", formapagamentoID, numeroEstabelecimento);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return bd.LerString("RedePreferencial");
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int BuscarBandeiraIDDebito(int FormaPagamentoID)
        {
            int bandeiraID = 0;
            string consulta = @"SELECT BandeiraID FROM tFormasPagamentoDebito WHERE FormaPagamentoID = {0}";
            consulta = string.Format(consulta, FormaPagamentoID);

            using (var reader = bd.Consulta(consulta))
            {
                if (reader.Read())
                {
                    bandeiraID = Convert.ToInt32(reader["BandeiraID"]);
                }
            }
            bd.Fechar();

            return bandeiraID;
        }


        public int BuscarTEFIDDebito(int FormaPagamentoID)
        {
            int tefid = 0;
            string consulta = @"SELECT tefid FROM tFormasPagamentoDebito WHERE FormaPagamentoID = {0}";
            consulta = string.Format(consulta, FormaPagamentoID);

            using (var reader = bd.Consulta(consulta))
            {
                if (reader.Read())
                {
                    tefid = Convert.ToInt32(reader["tefid"]);
                }
            }
            bd.Fechar();

            return tefid;
        }
    }

    public class FormaPagamentoLista : FormaPagamentoLista_B
    {
        public FormaPagamentoLista() { }

        public FormaPagamentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable Todos()
        {
            DataTable tabela = new DataTable();

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            bd.Consulta("SELECT ID, Nome FROM tFormaPagamento ORDER BY Nome");

            DataRow linha;
            while (bd.Consulta().Read())
            {
                linha = tabela.NewRow();
                linha["ID"] = bd.LerInt("ID");
                linha["Nome"] = bd.LerString("Nome");
                tabela.Rows.Add(linha);
            }

            return tabela;
        }

        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioFormaPagamento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = formaPagamento.Nome.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tFormaPagamento WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }



    }
}
