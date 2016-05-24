using CTLib;
using System;
using System.Data;

namespace IRLib.Paralela 
{
	public enum TiposLinhasParaCaixaConferencia 
	{
		PorEvento,
		PorCanal,
		PorEventoCanal, 
		PorCanalLoja, 
		PorEventoCanalLoja
	}
	
	public class CaixaParaConferencia 
	{
		#region Propriedades Locais
		//		private bool temDados = false;
		private DataTable tabelaLinhas=null;
		private string dataInicial = ""; 
		private string dataFinal = ""; 
		private int formaPagamentoID = 0;
		private int lojaID = 0; 
		private int canalID = 0; 
		private int eventoID = 0; 
		private int empresaID = 0; 
		private bool comCortesias = false; 
		private bool empresaVendeIngressos = false; 
		private bool empresaPromoveEventos= false;
		// Variáveis usadas no agrupamento por FormaPagamento
		private bool teveVendasLojas = false; 
		//private bool teveVendasCanais = false; 
		//private bool teveVendasEventos = false; 
		private decimal quantidadeFormaPagamentoTotal = 0; 
		private decimal ingressosFormaPagamentoTotal = 0; 
		private decimal convenienciaFormaPagamentoTotal = 0;
        private decimal comissaoFormaPagamentoTotal = 0; 
		private decimal entregaFormaPagamentoTotal = 0; 
		private decimal totalFormaPagamentoTotal = 0;
		// Variáveis usadas no PorLoja 
		private decimal quantidadeDasLojas = 0; 
		private decimal ingressosDasLojas = 0; 
		private decimal convenienciaDasLojas = 0;
        private decimal comissaoDasLojas = 0; 
		private decimal entregaDasLojas = 0; 
		private decimal totalDasLojas = 0; 
		// Variáveis usadas no PorCanal 
		private decimal quantidadeDosCanais = 0; 
		private decimal ingressosDosCanais = 0; 
		private decimal convenienciaDosCanais = 0;
        private decimal comissaoDosCanais = 0; 
		private decimal entregaDosCanais = 0; 
		private decimal totalDosCanais = 0; 
		// Variáveis usadas no PorEvento
		private decimal quantidadeDosEventos = 0; 
		private decimal ingressosDosEventos = 0; 
		private decimal convenienciaDosEventos = 0;
        private decimal comissaoDosEventos = 0; 
		private decimal entregaDosEventos = 0; 
		private decimal totalDosEventos = 0; 
		//
		DataTable tabelaTotal= Utilitario.EstruturaCaixaParaConferencia();
		DataTable tabelaEntrega = Utilitario.EstruturaTaxaEntrega();
		//		//
		//		DataTable cortesiasTabela= new DataTable();
		//		private decimal quantidadeCortesias = 0; 
		#endregion
		#region Propriedades Globais
		//		public DataTable CortesiasTabela {
		//			get {
		//				return cortesiasTabela;
		//			}
		//		}
		public DataTable TabelaFinalEntrega 
		{
			get 
			{
				return tabelaEntrega;
			}
		}
			
		public DataTable TabelaFinal 
		{
			get 
			{
				return tabelaTotal;
			}
		}
		public int[] EventosInformados 
		{
			set 
			{
			}
		}
		public string DataInicial 
		{
			set 
			{
				dataInicial = value;
			}
		}
		public string DataFinal 
		{
			set 
			{
				dataFinal = value;
			}
		}
		public int FormaPagamentoID 
		{
			set 
			{
				formaPagamentoID = value;
			}
		}
		public int LojaID 
		{
			set 
			{
				lojaID = value;
			}
		}
		public int CanalID 
		{
			set 
			{
				canalID = value;
			}
		}
		public int EventoID 
		{
			set 
			{
				eventoID = value;
			}
		}
		public int EmpresaID 
		{
			set 
			{
				empresaID = value;
				Empresa empresa =new Empresa();
				empresa.Ler(empresaID);
				empresaVendeIngressos = empresa.EmpresaVende.Valor; 
				empresaPromoveEventos= empresa.EmpresaPromove.Valor;
			}
		}
		public bool ComCortesias 
		{
			set 
			{
				comCortesias = value;
			}
		}
		#endregion 

		// IRLib.Utilitario

        public DataTable VendasPorEvento(string datainicial, string datafinal)
        {
            BD db = new BD();
            string sql = @"CREATE TABLE #Vendas 

                             (     

                             VendaBilheteriaID INT, 

                             TaxaConvenienciaValorTotal DECIMAL(12,2), 

                             ValorTotal DECIMAL(12,2), 

                             ComissaoTotal DECIMAL(12,2), 

                             PagamentoID INT, 

                             PagamentoNome NVARCHAR(50), 

                             PagamentoValor DECIMAL(12,2), 

                             PagamentoPct float, 

                             CanalID INT, 

                             CanalNome NVARCHAR(50), 

                             LojaID INT,

                             LojaNome NVARCHAR(50),

                             TaxaEntregaValor DECIMAL(12,2)

                             ) 

                             

                             CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)          

                        CREATE TABLE #Final

                        (

                             EmpresaID INT,

                             EmpresaNome NVARCHAR(100), 

                             LocalID INT,

                             LocalNome NVARCHAR(100), 

                             EventoID INT,

                             EventoNome NVARCHAR(100), 

                             CanalID INT,

                             CanalNome NVARCHAR(100),

                             LojaID INT,

                             LojaNome NVARCHAR(100), 

                             PagamentoID INT,

                             PagamentoNome NVARCHAR(100), 

                             Quantidade  DECIMAL(12,2),

                             Valor DECIMAL(12,2), 

                             Taxa DECIMAL(12,2), 
					        
                             Comissao DECIMAL(12,2),

                             TaxaEntregaValor DECIMAL(12,2)

                        ) 

 

                             INSERT INTO #Vendas 

                             SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS 

                             PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 

                             tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, TaxaEntregaValor

                             FROM tVendaBilheteria (NOLOCK)

                             INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID 

                             INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID

                             INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID

                             LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID

                             LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID

                             WHERE DataAbertura >= '@Inicial' AND DataAbertura < '@Final'

                             DECLARE @Canais TABLE (ID INT IDENTITY(1,1), CanalID INT)

                             DECLARE @n INT

                             DECLARE @i INT 

                             DECLARE @CanalID INT

 

                             SET @i = 1

                             INSERT INTO @Canais SELECT DISTINCT CanalID FROM #Vendas

 

                             SELECT @n = COUNT(CanalID) FROM @Canais

 

 

                             WHILE @i <= @n

                                   BEGIN

                                         SET @CanalID = (SELECT CanalID FROM @canais WHERE ID = @i)

                                         SET @i = @i + 1

 

 

                                         INSERT INTO #final

                                         SELECT 

                                         info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 

                                         vendas.CanalID,vendas.CanalNome, 

                                         vendas.LojaID,vendas.LojaNome,  

                                         PagamentoID, 

                                         CASE 

                                               WHEN (PagamentoID IS NULL) THEN 'Cortesia' 

                                                ELSE PagamentoNome

                                         END  AS PagamentoNome,

                                         CASE tIngressoLog.Acao 

                                               WHEN 'V' THEN

                                                     CASE 

                                                           WHEN (PagamentoID IS NULL) THEN 

                                                                 SUM(1)

                                                           ELSE 

                                                                 SUM(1 * PagamentoPCT / 100)

                                                     END

                                               ELSE

                                                     CASE        

                                                           WHEN (PagamentoID IS NULL) THEN SUM(-1)

                                                           ELSE SUM(1 * PagamentoPCT / -100)

                                               END

                                         END

                                         AS Quantidade,

                                         CASE tIngressoLog.Acao 

                                               WHEN 'V' THEN

                                                     SUM(tPreco.Valor) * PagamentoPct / 100

                                               ELSE

                                                     SUM(tPreco.Valor) * PagamentoPct / -100 

                                               END

                                         AS Valor,

                                         CASE tIngressoLog.Acao 

                                               WHEN 'V' THEN

                                                     CASE COUNT(tIngresso.ID) 

                                                           WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 

                                                                 SUM(TaxaConvenienciaValor) * PagamentoPct / 100

                                                           ELSE

                                                                 (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)

                                                           END

                                               ELSE  

                                                     CASE COUNT(tIngresso.ID) 

                                                           WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 

                                                                 SUM(TaxaConvenienciaValor) * PagamentoPct / -100

                                                           ELSE

                                                                 (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)

                                                           END

                                               END AS Taxa, 

                                               CASE tIngressoLog.Acao 

                                               WHEN 'V' THEN

                                                     CASE COUNT(tIngresso.ID) 

                                                           WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 

                                                                 SUM(TaxaComissao) * PagamentoPct / 100

                                                           ELSE

                                                                 (TaxaComissao * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)

                                                           END

                                               ELSE  

                                                     CASE COUNT(tIngresso.ID) 

                                                           WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 

                                                                 SUM(TaxaComissao) * PagamentoPct / -100

                                                           ELSE

                                                                 (TaxaComissao * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)

                                                           END

                                               END AS Comissao, 

                                         CASE tIngressoLog.Acao 

                                               WHEN 'V' THEN

                                                     SUM(TaxaEntregaValor) * PagamentoPct / 100

                                               ELSE

                                                     SUM(TaxaEntregaValor) * PagamentoPct / -100 

                                               END

                                         AS TaxaEntregaValor

                                         FROM tIngressoLog

                                         INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = 

tIngressoLog.CanalID 

                                         INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID

                                         INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID

                                         INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID

                                         INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID

                                         WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID  GROUP BY 

                                         info.EmpresaID,info.EmpresaNome, info.LocalID,info.LocalNome, info.EventoID,EventoNome,  vendas.CanalID,CanalNome, 

vendas.LojaID,vendas.LojaNome,  

                                         PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, TaxaEntregaValor,PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao

                                   END

 

                             SELECT 

                             tEmpresa.Nome, 
                                CASE tFormaPagamento.Tipo
                              WHEN 0 THEN Cartao
                              WHEN 1 THEN NaoCartao
                              ELSE 'Não definido'
                        END ComQuemEsta,
tCanalTipo.Nome AS Tipo, PagamentoNome,CanalNome,

                             SUM(Quantidade) AS Quantidade, 

                             SUM(Valor) AS Valor,    

                             SUM(Taxa) AS Conv,

                             SUM(TaxaEntregaValor) AS Taxa, 

							 SUM(vendas.ComissaoTotal) AS Comissao

                             SUM(Taxa) + SUM(TaxaEntregaValor) + SUM(Valor) AS Total

                             FROM #FINAL f

                             INNER JOIN tCanal ON tCanal.ID = CanalID

                             INNER JOIN tCanalTipo ON CanalTipoID = tCanalTipo.ID

                             INNER JOIN tEmpresa ON tEmpresa.ID = tCanal.EmpresaID
                             INNER JOIN tFormaPagamento ON tFormaPagamento.ID = PagamentoID

                             GROUP BY tEmpresa.Nome, CanalNome, Cartao, NaoCartao, tCanalTipo.Nome, PagamentoNome, tFormaPagamento.Tipo
                             HAVING SUM(Valor) <> 0
                             ORDER BY tEmpresa.Nome, CanalNome, tCanalTipo.Nome, PagamentoNome
";
            sql = sql.Replace("@Inicial", datainicial);
            sql = sql.Replace("@Final", datafinal);
            IDataReader dr = db.Consulta(sql);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

		private string ObterCanaisEmFuncaoDoEvento(int eventoID, string canais) 
		{
            BD bd = new BD();

            try
            {
                string strCanal = "";
                if (canais != "")
                {                    
                    string sql;
                    sql =
                        "SELECT DISTINCT tCanalEvento.CanalID, tCanal.Nome " +
                        "FROM         tCanalEvento INNER JOIN " +
                        "tCanal ON tCanalEvento.CanalID = tCanal.ID INNER JOIN " +
                        "tLoja ON tCanal.ID = tLoja.CanalID INNER JOIN " +
                        "tCaixa ON tLoja.ID = tCaixa.LojaID INNER JOIN " +
                        "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID " +
                        "WHERE     (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura < '" + dataFinal + "')  AND (tCanalEvento.EventoID IN (" + eventoID.ToString() + ")) AND " +
                        "(tCanalEvento.CanalID IN (" + canais + "))  " +
                        "ORDER BY tCanal.Nome ";

                    //					"SELECT DISTINCT tIngressoLog.CanalID, tCanal.Nome "+
                    //					"FROM            tCaixa INNER JOIN "+
                    //                         "tIngresso INNER JOIN "+
                    //                         "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID ON tCaixa.ID = tIngressoLog.CaixaID INNER JOIN "+
                    //                         "tCanal ON tIngressoLog.CanalID = tCanal.ID "+
                    //					"WHERE    (tCaixa.DataAbertura >= '"+dataInicial+"') AND (tCaixa.DataAbertura <= '"+dataFinal+"')  AND (tIngressoLog.Acao IN ("+IngressoLog.VendidosCancelados+")) AND (tIngressoLog.CanalID IN ("+canais+")) AND (tIngresso.EventoID IN ("+eventoID.ToString()+")) "+
                    //					"ORDER BY tCanal.Nome ";

                    bd.Consulta(sql);
                    bool primeiraVez = true;
                    while (bd.Consulta().Read())
                    {
                        if (primeiraVez)
                        {
                            strCanal = bd.LerString("CanalID");
                            primeiraVez = false;
                        }
                        else
                        {
                            strCanal = strCanal + "," + bd.LerString("CanalID");
                        }
                    }
                    bd.Fechar();
                }
                return strCanal;
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                bd.Fechar();
            }
		}

		private string ObterLojasEmFuncaoDoCanal(int canalID, string lojas) 
		{
            BD bd = new BD();

            try
            {                
                string sql;
                sql =
                    "SELECT DISTINCT tIngressoLog.LojaID, tLoja.Nome " +
                    "FROM            tCaixa INNER JOIN " +
                    "tIngressoLog ON tCaixa.ID = tIngressoLog.CaixaID INNER JOIN " +
                    "tLoja ON tIngressoLog.LojaID = tLoja.ID " +
                    "WHERE    (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura <= '" + dataFinal + "')  AND (tIngressoLog.Acao IN ('" + IngressoLog.CANCELAR + "'" + "," + "'" + IngressoLog.VENDER + "')) AND (tIngressoLog.CanalID IN (" + canalID.ToString() + ")) AND (tIngressoLog.LojaID IN (" + lojas + ")) " +
                    "ORDER BY tLoja.Nome ";
                bd.Consulta(sql);
                bool primeiraVez = true;
                string strLoja = "";
                while (bd.Consulta().Read())
                {
                    if (primeiraVez)
                    {
                        strLoja = bd.LerString("LojaID");
                        primeiraVez = false;
                    }
                    else
                    {
                        strLoja = strLoja + "," + bd.LerString("LojaID");
                    }
                }
                bd.Fechar();
                return strLoja;
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                bd.Fechar();
            }
		}

		public int QuantidadeCortesiasPorCaixa(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID) 
		{

            BD bdVendidos = new BD();
            BD bdCancelados = new BD();

            try
            {
                int quantidade = 0;
                int quantidadeVendidos = 0;
                int quantidadeCancelados = 0;
                if (dataInicial == "" || dataFinal == "" || eventoID < 0 || canalID < 0)
                    return Int32.MinValue;
                if (eventoID == 0 && canalID == 0)
                    return Int32.MinValue;
                string sql = "";
                string sqlVendidos = "";
                string sqlCancelados = "";
                #region SELECTs em função das condições
                sql =
                    "SELECT  COUNT(tIngressoLog.ID) AS Quantidade " +
                    "FROM    tIngressoLog INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tCaixa ON tIngressoLog.CaixaID = tCaixa.ID " +
                    "WHERE	(tCaixa.DataAbertura < '" + dataFinal + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') " +
                    "AND (tIngressoLog.CortesiaID > 0) ";

                if (canalID > 0 && eventoID == 0)
                {
                    if (lojaID > 0)
                    {
                        // se tem canal tem loja
                        sql = sql + " AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    {
                        // lojas agrupadas (loja =0)
                        sql = sql + " AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                if (canalID == 0 && eventoID > 0)
                {
                    sql = sql + " AND (tIngresso.EventoID = " + eventoID + ") ";
                }
                if (canalID > 0 && eventoID > 0)
                {
                    if (lojaID > 0)
                    {
                        // se tem canal tem loja
                        sql = sql + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                    }
                    else
                    {
                        // lojas agrupadas (loja =0)
                        sql = sql + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") ";
                    }
                }
                #endregion
                // Vendidos
                
                sqlVendidos = sql + " AND (tIngressoLog.Acao = N'" + IngressoLog.VENDER + "') ";
                bdVendidos.Consulta(sqlVendidos);
                if (bdVendidos.Consulta().Read())
                {
                    quantidadeVendidos = bdVendidos.LerInt("Quantidade");
                }
                bdVendidos.Fechar();
                // Cancelados                
                sqlCancelados = sql + " AND (tIngressoLog.Acao = N'" + IngressoLog.CANCELAR + "') ";
                bdCancelados.Consulta(sqlCancelados);
                if (bdCancelados.Consulta().Read())
                {
                    quantidadeCancelados = bdCancelados.LerInt("Quantidade");
                }
                bdCancelados.Fechar();
                quantidade = quantidadeVendidos - quantidadeCancelados;
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                bdVendidos.Fechar();
                bdCancelados.Fechar();

            }
		} // fim de QuantidadeCortesiasPorCaixa

		private DataTable LinhasCaixasPorFormaPagamentoEventoCanalLoja(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID, string titulo, int quantidadeCortesia) 
		{
            BD obterDados = new BD();

            try
            {
                bool temDados = false;
                DataTable tabelaLinhasRecebimentos = Utilitario.EstruturaCaixaParaConferencia();
                if ((dataInicial != "" && dataFinal != "" && eventoID >= 0 && canalID >= 0 && lojaID >= 0)
                    || quantidadeCortesia != 0)
                {
                    #region Obtendo as linhas de Formas de Recebimento
                    if (dataInicial != "" && dataFinal != "" && eventoID >= 0 && canalID >= 0 && lojaID >= 0)
                    {
                        // Obtendo dados através de SQL						
                        string sql =
                            "SELECT DISTINCT tFormaPagamento.ID AS FormaPagamentoID, tFormaPagamento.Nome AS FormaPagamento " +
                            "FROM            tIngresso INNER JOIN " +
                            "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN  " +
                            "tCaixa ON tIngressoLog.CaixaID = tCaixa.ID INNER JOIN " +
                            "tFormaPagamento INNER JOIN " +
                            "tVendaBilheteria INNER JOIN " +
                            "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID ON  " +
                            "tFormaPagamento.ID = tVendaBilheteriaFormaPagamento.FormaPagamentoID ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID " +
                            "WHERE (tCaixa.DataAbertura < '" + dataFinal + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') ";
                        if (canalID > 0 && eventoID == 0)
                        {
                            if (lojaID > 0)
                            {
                                // se tem canal tem loja
                                sql = sql + " AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                            }
                            else
                            {
                                // lojas agrupadas (loja =0)
                                sql = sql + " AND (tIngressoLog.CanalID = " + canalID + ") ";
                            }
                        }
                        if (canalID == 0 && eventoID > 0)
                        {
                            sql = sql + " AND (tIngresso.EventoID = " + eventoID + ") ";
                        }
                        if (canalID > 0 && eventoID > 0)
                        {
                            if (lojaID > 0)
                            {
                                // se tem canal tem loja
                                sql = sql + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") AND (tIngressoLog.LojaID = " + lojaID + ") ";
                            }
                            else
                            {
                                // lojas agrupadas (loja =0)
                                sql = sql + " AND (tIngresso.EventoID = " + eventoID + ") AND (tIngressoLog.CanalID = " + canalID + ") ";
                            }
                        }
                        sql = sql + " ORDER BY tFormaPagamento.Nome ";
                        obterDados.Consulta(sql);
                        while (obterDados.Consulta().Read())
                        {
                            temDados = true;
                            DataRow linha = tabelaLinhasRecebimentos.NewRow();
                            linha["FormaPagamentoID"] = obterDados.LerInt("FormaPagamentoID");
                            linha["Formas de Recebimento"] = obterDados.LerString("FormaPagamento");
                            tabelaLinhasRecebimentos.Rows.Add(linha);
                        }
                        obterDados.Fechar();
                    }
                    #endregion
                    #region Tem dados
                    if (quantidadeCortesia != 0 || temDados)
                    {
                        // signifca que teve dados
                        // Obter detalhes
                        TabelaMemoria tabelaMemoria = new TabelaMemoria();
                        tabelaMemoria.TabelaOrigem = tabelaLinhasRecebimentos.Copy();
                        // Incluir Cabeçalho
                        tabelaMemoria.Campo = "FormaPagamentoID";
                        tabelaMemoria.Conteudo = -1;
                        tabelaMemoria.InserirPrimeiroRegistro();
                        tabelaMemoria.TabelaDestino.Rows[0]["Formas de Recebimento"] = titulo;
                        tabelaLinhasRecebimentos = tabelaMemoria.TabelaDestino.Copy();
                        // Adicionar Cortesia
                        if (quantidadeCortesia != 0)
                        {
                            DataRow linhaCortesia = tabelaLinhasRecebimentos.NewRow();
                            linhaCortesia["FormaPagamentoID"] = 0;
                            linhaCortesia["Formas de Recebimento"] = "Cortesias";
                            linhaCortesia["Quantidade"] = quantidadeCortesia;
                            tabelaLinhasRecebimentos.Rows.Add(linhaCortesia);
                        }
                        // Adicionar subTotal
                        DataRow linhaUm = tabelaLinhasRecebimentos.NewRow();
                        linhaUm["FormaPagamentoID"] = -1;
                        linhaUm["Formas de Recebimento"] = titulo;
                        tabelaLinhasRecebimentos.Rows.Add(linhaUm);
                    }
                    #endregion
                }
                return tabelaLinhasRecebimentos.Copy();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                obterDados.Fechar();
            }
		} // LinhasCaixasPorFormaPagamentoEventoCanalLoja

		private bool ObterCalculoPorFormaRecebimento(string dataInicial, string dataFinal, int eventoID, int canalID, int lojaID) 
		{
			bool teveVendas = false;
			// Para cada forma de pagamento na condição especificada, calcular
			foreach (DataRow linha in tabelaLinhas.Rows) 
			{
				FormaPagamento formaPagamento = new FormaPagamento();
				if (linha["FormaPagamentoID"]!= System.DBNull.Value) 
				{
					formaPagamento.Control.ID = Convert.ToInt32(linha["FormaPagamentoID"]);
					if (formaPagamento.Control.ID > -1) 
					{ 
						// -1 é usado para agrupamento
						if (formaPagamento.Control.ID == 0) 
						{ 
							// se for cortesia só totaliza
							// Totalizando
							quantidadeFormaPagamentoTotal += Convert.ToDecimal(linha["Quantidade"]); 
							// Formato
							linha["Quantidade"]= Convert.ToDecimal(linha["Quantidade"]).ToString(Utilitario.FormatoMoeda);
							linha["R$ Ingressos"]= 0;
							linha["R$ Entrega"]= 0;
							linha["R$ Conveniência"]= 0;
                            linha["R$ Comissão"] = 0;
							linha["R$ Total"]= 0;
						}
						if (formaPagamento.Control.ID > 0) 
						{ 
							// 0 é cortesia logo não soma valores
							#region Quantidade
							// Total (diferença de vendidos com cancelados)
							decimal quantidadeVendidos = formaPagamento.QuantidadeIngressosPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.VENDER);
							decimal quantidadeCancelados = formaPagamento.QuantidadeIngressosPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.CANCELAR);
							if (quantidadeVendidos - quantidadeCancelados !=0)
								teveVendas = true;  
							linha["Quantidade"] = quantidadeVendidos - quantidadeCancelados;
							// Totalizando
							quantidadeFormaPagamentoTotal += Convert.ToDecimal(linha["Quantidade"]); 
							// Formato
							linha["Quantidade"]= Convert.ToDecimal(linha["Quantidade"]).ToString(Utilitario.FormatoMoeda);
							#endregion 
							#region Valor do Ingresso
							// Total (diferença de vendidos com cancelados)
							linha["R$ Ingressos"] = 
								formaPagamento.ValorIngressosPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.VENDER) -
								formaPagamento.ValorIngressosPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.CANCELAR);
							// Totalizando
							ingressosFormaPagamentoTotal += Convert.ToDecimal(linha["R$ Ingressos"]); 
							// Formato
							linha["R$ Ingressos"]= Math.Round(Convert.ToDecimal(linha["R$ Ingressos"]),1).ToString(Utilitario.FormatoMoeda);
							#endregion 
							#region Valor da Conveniência
							// Total (diferença de vendidos com cancelados)
							linha["R$ Conveniência"] = 
								formaPagamento.ValorConvenienciaPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.VENDER) -
								formaPagamento.ValorConvenienciaPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.CANCELAR);
							// Totalizando
							convenienciaFormaPagamentoTotal += Convert.ToDecimal(linha["R$ Conveniência"]); 
							// Formato
							linha["R$ Conveniência"]= Math.Round(Convert.ToDecimal(linha["R$ Conveniência"]),1).ToString(Utilitario.FormatoMoeda);
							#endregion 
                            #region Valor da Comissão
                            comissaoFormaPagamentoTotal += Convert.ToDecimal(linha["R$ Comissão"]);
                            linha["R$ Comissão"] = Math.Round(Convert.ToDecimal(linha["R$ Comissão"]), 1).ToString(Utilitario.FormatoMoeda);
                            #endregion
                            #region Valor da Entrega
                            // Total (diferença de vendidos com cancelados)
							linha["R$ Entrega"] = 
								formaPagamento.ValorEntregaPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.VENDER) -
								formaPagamento.ValorEntregaPorCaixaEventoCanalLoja(dataInicial, dataFinal, eventoID, canalID, lojaID, IngressoLog.CANCELAR);
							// Totalizando
							entregaFormaPagamentoTotal += Convert.ToDecimal(linha["R$ Entrega"]); 
							// Formato
							linha["R$ Entrega"]= Math.Round(Convert.ToDecimal(linha["R$ Entrega"]),1).ToString(Utilitario.FormatoMoeda);
							#endregion 
							#region Valor do Total
							// Total (diferença de vendidos com cancelados)
							linha["R$ Total"] = Convert.ToDecimal(linha["R$ Ingressos"]) + Convert.ToDecimal(linha["R$ Conveniência"])+ Convert.ToDecimal(linha["R$ Entrega"]);
							// Totalizando
							totalFormaPagamentoTotal += Convert.ToDecimal(linha["R$ Total"]); 
							// Formato
							linha["R$ Total"]= Math.Round(Convert.ToDecimal(linha["R$ Total"]),1).ToString(Utilitario.FormatoMoeda);
							#endregion 
						}
					}
				}
			} // laço do FormaPagamento
			return teveVendas;
		} // ObterCalculoPorFormaRecebimento


		public CaixaParaConferencia() 
		{
		}
		public void AdicionarLinhaFinal(TiposLinhasParaCaixaConferencia ultimoAgrupamento) 
		{
			decimal quantidadeTotais = quantidadeDosCanais;
			decimal ingressosTotais = ingressosDosCanais;
			decimal convenienciaTotais = convenienciaDosCanais;
            decimal comissaoTotais = comissaoDosCanais;
			decimal entregaTotais = entregaDosCanais;
			decimal totalTotais = totalDosCanais;
			switch (ultimoAgrupamento) 
			{
				case (TiposLinhasParaCaixaConferencia.PorCanal):
				case (TiposLinhasParaCaixaConferencia.PorCanalLoja):
					quantidadeTotais = quantidadeDosCanais;
					ingressosTotais = ingressosDosCanais;
					convenienciaTotais = convenienciaDosCanais;
					entregaTotais = entregaDosCanais;
					totalTotais = totalDosCanais;
					break;
				case (TiposLinhasParaCaixaConferencia.PorEvento):
				case (TiposLinhasParaCaixaConferencia.PorEventoCanal):
				case (TiposLinhasParaCaixaConferencia.PorEventoCanalLoja):
					quantidadeTotais = quantidadeDosEventos;
					ingressosTotais = ingressosDosEventos;
					convenienciaTotais = convenienciaDosEventos;
                    comissaoTotais = comissaoDosEventos;
					entregaTotais = entregaDosEventos;
					totalTotais = totalDosEventos;
					break;
			}
			if (quantidadeTotais>0) 
			{
				// linha em branco
				DataRow linhaBrancoMais = tabelaTotal.NewRow();
				tabelaTotal.Rows.Add(linhaBrancoMais);
				//
				DataRow linhaTotais = tabelaTotal.NewRow();
				linhaTotais["Formas de Recebimento"]= "Total";
				linhaTotais["Quantidade"]= quantidadeTotais;
				linhaTotais["R$ Ingressos"]= ingressosTotais;
				linhaTotais["R$ Conveniência"]= convenienciaTotais;
                linhaTotais["R$ Comissão"] = comissaoTotais;
				//linhaTotais["R$ Entrega"]= entregaTotais;
				linhaTotais["R$ Total"]= totalTotais;
				// Formato
				linhaTotais["Quantidade"]= Convert.ToDecimal(linhaTotais["Quantidade"]).ToString(Utilitario.FormatoMoeda);
				linhaTotais["R$ Ingressos"]= Convert.ToDecimal(linhaTotais["R$ Ingressos"]).ToString(Utilitario.FormatoMoeda);
				linhaTotais["R$ Conveniência"]= Convert.ToDecimal(linhaTotais["R$ Conveniência"]).ToString(Utilitario.FormatoMoeda);
				//linhaTotais["R$ Entrega"]= Convert.ToDecimal(linhaTotais["R$ Entrega"]).ToString(Utilitario.FormatoMoeda);
				linhaTotais["R$ Total"]= Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
				tabelaTotal.Rows.Add(linhaTotais);
			}
		}

		public void CaixasComLinhasPorEventos(int[] eventosInformados, int[] canaisInformados, int[] lojasInformadas,int localID,bool semTaxas) 
		{
			try 
			{
				int tipoRelatorio = 2;
				DataTable tabela = getData(eventosInformados,canaisInformados,lojasInformadas,localID,tipoRelatorio);
				
				
				int empresaIDAtual = 0;
				int empresaIDAnterior = 0;
				int eventoIDAtual = 0;
				int eventoIDAnterior = 0;
				int formaPagamentoIDAtual = 0;
				int formaPagamentoIDAnterior = 0;

				//variaveis para os calculos
				decimal qtdTotalIngresso = 0;
				decimal valorTotalIngresso = 0;
				decimal valorTotalConveniencia = 0;
                decimal valorTotalComissao = 0;
				//variaveis para a linha de totais 
				decimal somaValorIngressos = 0;
				decimal somaQuantidadeIngressos = 0;
				decimal somaValorConveniencia = 0;
				string nomeAnteriorEmpresa = string.Empty;
				string nomeAnteriorEvento = string.Empty;
				//Total Evento
				decimal totalEventoValorIngresso = 0;
				decimal totalEventoQuantidadeIngresso = 0;
				decimal totalEventoValorConveniencia = 0;
                decimal totalEventoValorComissao = 0;
				//Total Empresa
				decimal totalEmpresaValorIngresso = 0;
				decimal totalEmpresaQuantidadeIngresso = 0;
				decimal totalEmpresaValorConveniencia = 0;
				object valorAuxiliar = 0;

				DataRow novaLinha = null;
				DataRow linha = null;

				for (int i = 0; i <= tabela.Rows.Count - 1; i++) 
				{
					linha = tabela.Rows[i];
					
					empresaIDAtual = (int)linha["EmpresaID"];
					eventoIDAtual = (int)linha["EventoID"];
					formaPagamentoIDAtual = (int)linha["PagamentoID"];
					
					if(i > 0) 
					{	
						if(eventoIDAtual != eventoIDAnterior) 
						{

							//insere a linha total do Evento
							DataRow linhaTotalEvento = tabelaTotal.NewRow();

							linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Evento " + nomeAnteriorEvento + "</div>";
							linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + totalEventoQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
							linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorIngresso) + "</div>";
							if(!semTaxas)
							{
								linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorConveniencia) + "</div>";
                                linhaTotalEvento["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorComissao) + "</div>";
								linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((totalEventoValorIngresso + totalEventoValorConveniencia)) + "<div>";
							}
							tabelaTotal.Rows.Add(linhaTotalEvento);
						
							//zera as variáveis para o proximo Evento
							totalEventoValorIngresso = 0;
							totalEventoQuantidadeIngresso = 0;
							totalEventoValorConveniencia = 0;
                            totalEventoValorComissao = 0;
						}

						if(empresaIDAtual != empresaIDAnterior)
						{
							//insere a linha total do Empresa
							DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

							linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + nomeAnteriorEmpresa+ "</div>";
							linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
							linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso) + "</div>";
							if(!semTaxas)
							{
								linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia) + "</div>";
								linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia)) + "<div>";
							}
							tabelaTotal.Rows.Add(linhaTotalEmpresa);
						
							//zera as variáveis para a próxima empresa
							totalEmpresaValorIngresso = 0;
							totalEmpresaQuantidadeIngresso = 0;
							totalEmpresaValorConveniencia = 0;
						
						}
					}

					//se a empresa mudou
					if(empresaIDAtual != empresaIDAnterior)
					{
						// insere a linha só com o nome da Empresa.
						novaLinha = tabelaTotal.NewRow();

						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linha["EmpresaNome"] + "</div>";
					}


					// Se mudou o evento.
					if (eventoIDAtual != eventoIDAnterior) 
					{

						// insere a linha só com o nome do evento.
						novaLinha = tabelaTotal.NewRow();
						
						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Evento " + linha["EventoNome"] + "</div>";

						// insere a linha só com o nome do Forma de pagamento.		
						novaLinha = tabelaTotal.NewRow();
						tabelaTotal.Rows.Add(novaLinha);


						/******Calculo dos valores de linhas de formas de pagamento*********/

						//qtde total de ingressos vendido 
						valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valor total de ingressos vendido para a forma de pagamento
						valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valorTotalConveniência
						valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                        //valorTotalComissão
                        valorAuxiliar = tabela.Compute("SUM(comissao)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
                        valorTotalComissao = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];

						novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
						novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
						if(!semTaxas)
						{
							novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
							novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
						}

						/**************Fim Calculo dos valores de linhas de formas de pagamento***********/	

					}
					else 
					{
						//se mudou a forma de pagamento
						if(formaPagamentoIDAtual != formaPagamentoIDAnterior) 
						{
									
							// insere a linha só com o nome do Forma de pagamento.		
							novaLinha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(novaLinha);

							/******Calculo dos valores de linhas de formas de pagamento*********/

							//qtde total de ingressos vendido 
							valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


							//valor total de ingressos vendido para a forma de pagamento
							valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valorTotalConveniência
							valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


							novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];
							novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
							novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
							if(!semTaxas)
							{
								novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
								novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
							}

							/**************Fim Calculo dos valores de linhas de formas de pagamento***********/

						}	

					}
						
					/****variaveis de totais de loja,canal,evento - são zeradas conforme são inseridas linhas de loja,canal e evento**/
					
					if (!(eventoIDAtual == eventoIDAnterior && formaPagamentoIDAnterior == formaPagamentoIDAtual)) 
					{	
						//soma de totais
						somaValorIngressos = somaValorIngressos + valorTotalIngresso;
						somaQuantidadeIngressos = somaQuantidadeIngressos + qtdTotalIngresso; 
						somaValorConveniencia = somaValorConveniencia + valorTotalConveniencia;			 
						//Total Evento
						totalEventoValorIngresso = totalEventoValorIngresso + valorTotalIngresso;
						totalEventoQuantidadeIngresso = totalEventoQuantidadeIngresso + qtdTotalIngresso;
						totalEventoValorConveniencia = totalEventoValorConveniencia + valorTotalConveniencia;
                        totalEventoValorComissao = totalEventoValorComissao + valorTotalComissao;
						//Total Empresa
						totalEmpresaValorIngresso = totalEmpresaValorIngresso + valorTotalIngresso;
						totalEmpresaQuantidadeIngresso = totalEmpresaQuantidadeIngresso + qtdTotalIngresso;
						totalEmpresaValorConveniencia = totalEmpresaValorConveniencia + valorTotalConveniencia;
					}
					
					// Define ID atual e anterior para cada linha do laço
					empresaIDAnterior = empresaIDAtual;
					eventoIDAnterior = eventoIDAtual;
					formaPagamentoIDAnterior = formaPagamentoIDAtual;
					//nome do anterior para linha de total
					nomeAnteriorEmpresa = (string) linha["EmpresaNome"];
					nomeAnteriorEvento = (string) linha["EventoNome"];
					//insere os ultimos totais de loja, canal e evento
					if(i == tabela.Rows.Count - 1) 
					{	
						//insere a linha total do Evento
						DataRow linhaTotalEvento = tabelaTotal.NewRow();

						linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Evento " + nomeAnteriorEvento + "</div>";
						linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + totalEventoQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
						linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorIngresso) + "</div>";
						if(!semTaxas)
						{
							linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorConveniencia) + "</div>";
							linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((totalEventoValorIngresso + totalEventoValorConveniencia)) + "<div>";
						}
						tabelaTotal.Rows.Add(linhaTotalEvento);

						//insere a linha total do Empresa
						DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

						linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + nomeAnteriorEmpresa+ "</div>";
						linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
						linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso) + "</div>";
						if(!semTaxas)
						{
							linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia) + "</div>";
							linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia)) + "<div>";
						}
						tabelaTotal.Rows.Add(linhaTotalEmpresa);
					
					}
				}
			
							
				//linha em branco
				DataRow linhaBranco = tabelaTotal.NewRow();
				tabelaTotal.Rows.Add(linhaBranco);

				//insere linha totais
				DataRow linhaTotal = tabelaTotal.NewRow();

				linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
				linhaTotal["Quantidade"] = somaQuantidadeIngressos.ToString(Utilitario.FormatoMoeda);
				linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(somaValorIngressos);
				if(!semTaxas)
				{
					linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(somaValorConveniencia);
					linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"])+ Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
				}
				tabelaTotal.Rows.Add(linhaTotal);
			
			}
		
		
			catch(Exception erro) 
			{	
				throw erro;
			}
		}


		public void CaixasComLinhasPorEventosCanais(int[] eventosInformados, int[] canaisInformados,int[] lojasInformadas,int localID,bool semTaxas ) 
		{
			try 
			{
				int tipoRelatorio = 5;
				DataTable tabela = getData(eventosInformados,canaisInformados,lojasInformadas,localID,tipoRelatorio);

				int empresaIDAtual = 0;
				int empresaIDAnterior = 0;
				int eventoIDAtual = 0;
				int eventoIDAnterior = 0;
				int canalIDAtual = 0;
				int canalIDAnterior = 0;
				int formaPagamentoIDAtual = 0;
				int formaPagamentoIDAnterior = 0;

				//variaveis para os calculos
				decimal qtdTotalIngresso = 0;
				decimal valorTotalIngresso = 0;
				decimal valorTotalConveniencia = 0;
				//variaveis para a linha de totais 
				decimal somaValorIngressos = 0;
				decimal somaQuantidadeIngressos = 0;
				decimal somaValorConveniencia = 0;
				string nomeAnteriorEmpresa = string.Empty;
				string nomeAnteriorEvento = string.Empty;
				string nomeAnteriorCanal = string.Empty;
				string nomeAnteriorLoja = string.Empty;
				//Total Canal
				decimal totalCanalValorIngresso = 0;
				decimal totalCanalQuantidadeIngresso = 0;
				decimal totalCanalValorConveniencia = 0;
				//Total Evento
				decimal totalEventoValorIngresso = 0;
				decimal totalEventoQuantidadeIngresso = 0;
				decimal totalEventoValorConveniencia = 0;
				//Total Empresa
				decimal totalEmpresaValorIngresso = 0;
				decimal totalEmpresaQuantidadeIngresso = 0;
				decimal totalEmpresaValorConveniencia = 0;
				object valorAuxiliar = 0;

				DataRow novaLinha = null;
				DataRow linha = null;

				for (int i = 0; i <= tabela.Rows.Count - 1; i++) 
				{
					linha = tabela.Rows[i];
					
					empresaIDAtual = (int)linha["EmpresaID"];
					eventoIDAtual = (int)linha["EventoID"];
					canalIDAtual  = (int)linha["CanalID"];
					formaPagamentoIDAtual = (int)linha["PagamentoID"];
					
					if(i > 0) 
					{
						if(canalIDAtual != canalIDAnterior || ((canalIDAtual == canalIDAnterior) && (eventoIDAtual != eventoIDAnterior))) 
						{

							//insere a linha total do canal
							DataRow linhaTotalCanal = tabelaTotal.NewRow();

							linhaTotalCanal["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:75px'>Total Canal " + nomeAnteriorCanal + "</div>";
							linhaTotalCanal["Quantidade"] = totalCanalQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
							linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalCanalValorIngresso);
							if(!semTaxas)
							{
								linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalCanalValorConveniencia);
								linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalCanalValorIngresso + totalCanalValorConveniencia));
							}
							tabelaTotal.Rows.Add(linhaTotalCanal);
						
							//zera as variáveis para o proximo canal
							totalCanalValorIngresso = 0;
							totalCanalQuantidadeIngresso = 0;
							//totalCanalValorEntrega = 0;
							totalCanalValorConveniencia = 0;
							//totalCanalSomaTotal = 0;
						}
						if(eventoIDAtual != eventoIDAnterior) 
						{

							//insere a linha total do Evento
							DataRow linhaTotalEvento = tabelaTotal.NewRow();

							linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Evento " + nomeAnteriorEvento + "</div>";
							linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + totalEventoQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
							linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorIngresso) + "</div>";
							if(!semTaxas)
							{
								linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorConveniencia) + "</div>";
								linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((totalEventoValorIngresso + totalEventoValorConveniencia)) + "</div>";
							}
							tabelaTotal.Rows.Add(linhaTotalEvento);
						
							//zera as variáveis para o proximo Evento
							totalEventoValorIngresso = 0;
							totalEventoQuantidadeIngresso = 0;
							//totalEventoValorEntrega = 0;
							totalEventoValorConveniencia = 0;
							//totalEventoSomaTotal = 0;
						}

						if(empresaIDAtual != empresaIDAnterior)
						{
							//insere a linha total do Empresa
							DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

							linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + nomeAnteriorEmpresa+ "</div>";
							linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
							linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso) + "</div>";
							if(!semTaxas)
							{
								linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia) + "</div>";
								linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia)) + "<div>";
							}
							tabelaTotal.Rows.Add(linhaTotalEmpresa);
						
							//zera as variáveis para a próxima empresa
							totalEmpresaValorIngresso = 0;
							totalEmpresaQuantidadeIngresso = 0;
							totalEmpresaValorConveniencia = 0;
						
						}
					}

					//se a empresa mudou
					if(empresaIDAtual != empresaIDAnterior)
					{
						// insere a linha só com o nome da Empresa.
						novaLinha = tabelaTotal.NewRow();

						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linha["EmpresaNome"] + "</div>";
					}	
				
					// Se mudou o evento.
					if (eventoIDAtual != eventoIDAnterior) 
					{

						// insere a linha só com o nome do evento.
						novaLinha = tabelaTotal.NewRow();
						
						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Evento " + linha["EventoNome"] + "</div>";

						// insere a linha só com o nome do Canal.
						novaLinha = tabelaTotal.NewRow();
						
						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:75px'>Canal " + linha["CanalNome"] + "</div>";

						// insere a linha só com o nome do Forma de pagamento.		
						novaLinha = tabelaTotal.NewRow();
						tabelaTotal.Rows.Add(novaLinha);


						/******Calculo dos valores de linhas de formas de pagamento*********/

						//qtde total de ingressos vendido 
						valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


						//valor total de ingressos vendido para a forma de pagamento
						valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valorTotalConveniência
						valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


						novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];

						novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda);
						novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
						if(!semTaxas)
						{
							novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
							novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
						}
						/**************Fim Calculo dos valores de linhas de formas de pagamento***********/
 
						

					}
					else 
					{ // se continua no mesmo evento, procurar canal.
						
						//se mudou o canal
						if(canalIDAtual != canalIDAnterior) 
						{
							// insere a linha só com o nome do Canal.
							novaLinha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(novaLinha);
							novaLinha["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:75px'>Canal " + linha["CanalNome"] + "</div>";

							// insere a linha só com o nome do Forma de pagamento.		
							novaLinha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(novaLinha);

							/******Calculo dos valores de linhas de formas de pagamento*********/

							//qtde total de ingressos vendido 
							valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valor total de ingressos vendido para a forma de pagamento
							valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valorTotalConveniência
							valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


							novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];
							novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
							novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
							if(!semTaxas)
							{
								novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
								novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
							}
							/**************Fim Calculo dos valores de linhas de formas de pagamento***********/
						}
						else 
						{ // se continua no mesmo canal, procurar formapagamento.
							//se mudou a forma de pagamento
							if(formaPagamentoIDAtual != formaPagamentoIDAnterior) 
							{
									
								// insere a linha só com o nome do Forma de pagamento.		
								novaLinha = tabelaTotal.NewRow();
								tabelaTotal.Rows.Add(novaLinha);

								/******Calculo dos valores de linhas de formas de pagamento*********/

								//qtde total de ingressos vendido 
								valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
								qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


								//valor total de ingressos vendido para a forma de pagamento
								valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
								valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

								//valorTotalConveniência
								valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
								valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


								novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];
								novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
								novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
								if(!semTaxas)
								{
									novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
									novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
								}

								/**************Fim Calculo dos valores de linhas de formas de pagamento***********/

							}	

						}
						
					}
					
					/****variaveis de totais de loja,canal,evento - são zeradas conforme são inseridas linhas de loja,canal e evento**/
					
					if (!(eventoIDAtual == eventoIDAnterior && canalIDAnterior == canalIDAtual && formaPagamentoIDAnterior == formaPagamentoIDAtual)) 
					{	
						//soma de totais
						somaValorIngressos = somaValorIngressos + valorTotalIngresso;
						somaQuantidadeIngressos = somaQuantidadeIngressos + qtdTotalIngresso; 
						somaValorConveniencia = somaValorConveniencia + valorTotalConveniencia;			 
						//Total Canal
						totalCanalValorIngresso = totalCanalValorIngresso + valorTotalIngresso;
						totalCanalQuantidadeIngresso = totalCanalQuantidadeIngresso + qtdTotalIngresso;
						totalCanalValorConveniencia = totalCanalValorConveniencia + valorTotalConveniencia;
						//Total Evento
						totalEventoValorIngresso = totalEventoValorIngresso + valorTotalIngresso;
						totalEventoQuantidadeIngresso = totalEventoQuantidadeIngresso + qtdTotalIngresso;
						totalEventoValorConveniencia = totalEventoValorConveniencia + valorTotalConveniencia;
						//Total Empresa
						totalEmpresaValorIngresso = totalEmpresaValorIngresso + valorTotalIngresso;
						totalEmpresaQuantidadeIngresso = totalEmpresaQuantidadeIngresso + qtdTotalIngresso;
						totalEmpresaValorConveniencia = totalEmpresaValorConveniencia + valorTotalConveniencia;
						
					}
					
					// Define ID atual e anterior para cada linha do laço
					empresaIDAnterior = empresaIDAtual;
					eventoIDAnterior = eventoIDAtual;
					canalIDAnterior = canalIDAtual;
					formaPagamentoIDAnterior = formaPagamentoIDAtual;
					//nome do anterior para linha de total
					nomeAnteriorEmpresa = (string) linha["EmpresaNome"];
					nomeAnteriorEvento = (string) linha["EventoNome"];
					nomeAnteriorCanal = (string) linha["CanalNome"];
					//insere os ultimos totais de loja, canal e evento
					if(i == tabela.Rows.Count - 1) 
					{
						//insere a linha total do canal
						DataRow linhaTotalCanal = tabelaTotal.NewRow();

						linhaTotalCanal["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:75px'>Total Canal " + nomeAnteriorCanal + "</div>";
						linhaTotalCanal["Quantidade"] = totalCanalQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
						linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalCanalValorIngresso);
						if(!semTaxas)
						{
							linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalCanalValorConveniencia);
							linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalCanalValorIngresso + totalCanalValorConveniencia));
						}
						tabelaTotal.Rows.Add(linhaTotalCanal);

						//insere a linha total do Evento
						DataRow linhaTotalEvento = tabelaTotal.NewRow();

						linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Evento " + nomeAnteriorEvento + "</div>";
						linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + totalEventoQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
						linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorIngresso) + "</div>";
						if(!semTaxas)
						{
							linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorConveniencia) + "</div>";
							linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC;padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((totalEventoValorIngresso + totalEventoValorConveniencia)) + "</div>";
						}
						tabelaTotal.Rows.Add(linhaTotalEvento);

						//insere a linha total do Empresa
						DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

						linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + nomeAnteriorEmpresa+ "</div>";
						linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
						linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso) + "</div>";
						if(!semTaxas)
						{
							linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia) + "</div>";
							linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia)) + "<div>";
						}
						tabelaTotal.Rows.Add(linhaTotalEmpresa);

					}
				
				}
							
				//linha em branco
				DataRow linhaBranco = tabelaTotal.NewRow();
				tabelaTotal.Rows.Add(linhaBranco);

				//insere linha totais
				DataRow linhaTotal = tabelaTotal.NewRow();

				linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
				linhaTotal["Quantidade"] = somaQuantidadeIngressos.ToString(Utilitario.FormatoMoeda);
				linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(somaValorIngressos);
				if(!semTaxas)
				{
					linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(somaValorConveniencia);
					linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));
				}

				tabelaTotal.Rows.Add(linhaTotal);
			}
			catch(Exception erro) 
			{	
				throw erro;
			}


		}

		public void CaixasComLinhasPorCanais(int[] eventosInformados, int[] canaisInformados, int[] lojasInformadas,int localID,bool semTaxas) 
		{
			string empresa = string.Empty;
			string canal = string.Empty;
			string formaPagamento = string.Empty;
			//variaveis para os calculos
			decimal qtdTotalIngresso = 0;
			decimal valorTotalIngresso = 0;
			decimal valorTotalConveniencia = 0;
			//variaveis para a linha de totais 
			decimal somaValorIngressos = 0;
			decimal somaQuantidadeIngressos = 0;
			decimal somaValorConveniencia = 0;
			decimal totalCanalValorIngresso = 0;
			decimal totalCanalQuantidadeIngresso = 0;
			decimal totalCanalValorConveniencia = 0;
			decimal totalEmpresaValorIngresso = 0;
			decimal totalEmpresaQuantidadeIngresso = 0;
			decimal totalEmpresaValorConveniencia = 0;
			object valorAuxiliar = 0;
			DataRow linha= null;

			try 
			{
				int tipoRelatorio = 3;
				DataTable tabela = getData(eventosInformados,canaisInformados,lojasInformadas,localID,tipoRelatorio);
				

				DataTable dtEmpresa = CTLib.TabelaMemoria.Distinct(tabela,"EmpresaNome");
				foreach(DataRow linhaEmpresa in dtEmpresa.Rows) 
				{
					// insere a linha só com o nome da Empresa.
					linha = tabelaTotal.NewRow();

					tabelaTotal.Rows.Add(linha);
					linha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
					empresa = linhaEmpresa["EmpresaNome"].ToString();
					
					DataTable dtCanal = CTLib.TabelaMemoria.DistinctComFiltro(tabela,"CanalNome","EmpresaNome = '" + empresa + "'");
					foreach(DataRow linhaCanal in dtCanal.Rows)
					{
						// insere a linha só com o nome do Canal.
						linha = tabelaTotal.NewRow();
						tabelaTotal.Rows.Add(linha);
						linha["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:40px'>Canal " + linhaCanal["CanalNome"] + "</div>";
						canal = linhaCanal["CanalNome"].ToString();

						DataTable dtFormaPagamento = CTLib.TabelaMemoria.DistinctComFiltro(tabela,"FormaPagamento","EmpresaNome = '" + empresa + "' and CanalNome = '" + canal + "'");
						foreach(DataRow linhaFormaPagamento in dtFormaPagamento.Rows)
						{
							//linha de forma de pagamento
							linha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(linha);
							formaPagamento = linhaFormaPagamento["FormaPagamento"].ToString();


							/******Calculo dos valores de linhas de formas de pagamento*********/

							//qtde total de ingressos vendido 
							valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "' and FormaPagamento = '" + formaPagamento + "'");
							qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valor total de ingressos vendido para a forma de pagamento
							valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "' and FormaPagamento = '" + formaPagamento + "'");
							valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valorTotalConveniência
							valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "'and CanalNome= '" + canal + "' and FormaPagamento = '" + formaPagamento + "'");
							valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							linha["Formas de Recebimento"] = linhaFormaPagamento["FormaPagamento"];
							linha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
							linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
							if(!semTaxas)
							{
								linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
								linha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linha["R$ Conveniência"]) + Convert.ToDecimal(linha["R$ Ingressos"])));
							}
						}

						//calculo linha total do canal
					
						//qtde total Canal
						valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "'");
						totalCanalQuantidadeIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valor total Canal
						valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "'");
						totalCanalValorIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valor total taxa conveniência Canal
						valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "' AND CanalNome= '" + canal + "'");
						totalCanalValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
							
	
						//insere a linha total do canal
						DataRow linhaTotalCanal = tabelaTotal.NewRow();

						linhaTotalCanal["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:40px'>Total Canal " + canal + "</div>";
						linhaTotalCanal["Quantidade"] =   totalCanalQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
						linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalCanalValorIngresso);
						if(!semTaxas)
						{
							linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalCanalValorConveniencia);
							linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalCanalValorIngresso + totalCanalValorConveniencia));
						}
						tabelaTotal.Rows.Add(linhaTotalCanal);
				
				
					}

					//calculo linha total da Empresa
					
					//qtde total Empresa
					valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "'");
					totalEmpresaQuantidadeIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					//valor total Empresa
					valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "'");
					totalEmpresaValorIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					//valor total taxa conveniência Empresa
					valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "'");
					totalEmpresaValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
							
	
					//insere a linha total da Empresa
					DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

					linhaTotalEmpresa["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Total Empresa " + empresa + "</div>";
					linhaTotalEmpresa["Quantidade"] =   totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
					linhaTotalEmpresa["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso);
					if(!semTaxas)
					{
						linhaTotalEmpresa["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia);
						linhaTotalEmpresa["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia));
					}
					tabelaTotal.Rows.Add(linhaTotalEmpresa);



						

				}

				//insere linha totais
				
				//qtde total 
				valorAuxiliar = tabela.Compute("SUM(Quantidade)","1=1");
				somaQuantidadeIngressos = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

				//valor total
				valorAuxiliar = tabela.Compute("SUM(Valor)","1=1");
				somaValorIngressos = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

				//Taxa Convenência Total
				valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","1=1");
				somaValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


				//linha em branco
				DataRow linhaBranco = tabelaTotal.NewRow();
				tabelaTotal.Rows.Add(linhaBranco);
			
				//linha com totais
				DataRow linhaTotal = tabelaTotal.NewRow();

				linhaTotal["Formas de Recebimento"] = "<div style= 'text-align:left'>Total</div>";
				linhaTotal["Quantidade"] = somaQuantidadeIngressos.ToString(Utilitario.FormatoMoeda);
				linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(somaValorIngressos);
				if(!semTaxas)
				{
					linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(somaValorConveniencia);
					linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));
				}
				tabelaTotal.Rows.Add(linhaTotal);

			}
			catch(Exception erro) 
			{	
				throw erro;
			}
		}

		
		public void CaixasComLinhasPorLojas(int[] lojas) 
		{
			try 
			{

				#region Para cada loja
				foreach (int lojaID in lojas) 
				{
					// Zerar variáveis de Formas de Recebimento
					quantidadeFormaPagamentoTotal = 0; 
					ingressosFormaPagamentoTotal = 0; 
					convenienciaFormaPagamentoTotal = 0; 
					entregaFormaPagamentoTotal = 0; 
					totalFormaPagamentoTotal = 0;
					#region Obter os dados na condição especificada
					// Filtrando as condições
					IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
					Caixa caixa = new Caixa();
					// Parâmetros: dataInicial, dataFinal, ingressoLog.Vendidos +"," +ingressoLog.Cancelados, comCortesia, apresentacaoID, lojaID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasLoja, empresaVendeIngressos, empresaPromoveEventos);
					// Linhas do Grid
					Loja loja = new Loja();
					loja.Ler(lojaID);
					int quantidadeCortesia =0;
					// Obter cortesia
					if (comCortesias)
						quantidadeCortesia = QuantidadeCortesiasPorCaixa(dataInicial, dataFinal, eventoID, canalID, lojaID);
					// Obter linhas por Formas de Recebimento desta Loja (cabeçalho e rodapé está incluso aqui)
					tabelaLinhas = LinhasCaixasPorFormaPagamentoEventoCanalLoja (dataInicial, dataFinal, eventoID, canalID, lojaID, "Loja "+loja.Nome.Valor, quantidadeCortesia); 	
					#endregion
					// Se tiver linhas, obter os valores com ObterCalculoPorFormaPagamento
					if (tabelaLinhas.Rows.Count >0) 
					{
						teveVendasLojas = (
							ObterCalculoPorFormaRecebimento (dataInicial, dataFinal, eventoID, canalID, lojaID)
							|| teveVendasLojas 
							);
						#region Vai somando os agrupamentos
						if (teveVendasLojas) 
						{
							foreach (DataRow linhaParcial in tabelaLinhas.Rows) 
							{
								tabelaTotal.ImportRow(linhaParcial); // tabelaLinhas final
							}
							// Atribuir Formas de Recebimento somadas para esta Loja
							int ultimo = tabelaTotal.Rows.Count-1;
							DataRow linhaPorLoja = tabelaTotal.Rows[ultimo];
							linhaPorLoja["Quantidade"]= quantidadeFormaPagamentoTotal;
							linhaPorLoja["R$ Ingressos"]= ingressosFormaPagamentoTotal;
							linhaPorLoja["R$ Conveniência"]= convenienciaFormaPagamentoTotal;
							linhaPorLoja["R$ Entrega"]= entregaFormaPagamentoTotal;
							linhaPorLoja["R$ Total"]= totalFormaPagamentoTotal;
							// Formato
							linhaPorLoja["Quantidade"]= Convert.ToDecimal(linhaPorLoja["Quantidade"]).ToString(Utilitario.FormatoMoeda);
							linhaPorLoja["R$ Ingressos"]= Convert.ToDecimal(linhaPorLoja["R$ Ingressos"]).ToString(Utilitario.FormatoMoeda);
							linhaPorLoja["R$ Conveniência"]= Convert.ToDecimal(linhaPorLoja["R$ Conveniência"]).ToString(Utilitario.FormatoMoeda);
							linhaPorLoja["R$ Entrega"]= Convert.ToDecimal(linhaPorLoja["R$ Entrega"]).ToString(Utilitario.FormatoMoeda);
							linhaPorLoja["R$ Total"]= Convert.ToDecimal(linhaPorLoja["R$ Total"]).ToString(Utilitario.FormatoMoeda);
							// Somar total de cada Loja (usado fora do laço)
							quantidadeDasLojas = quantidadeDasLojas+ quantidadeFormaPagamentoTotal; 
							ingressosDasLojas = ingressosDasLojas+ ingressosFormaPagamentoTotal; 
							convenienciaDasLojas = convenienciaDasLojas+ convenienciaFormaPagamentoTotal;
                            comissaoDasLojas = comissaoDasLojas + comissaoFormaPagamentoTotal;
							entregaDasLojas = entregaDasLojas+ entregaFormaPagamentoTotal; 
							totalDasLojas = totalDasLojas+ totalFormaPagamentoTotal; 
						}
						#endregion
					}
				} // fim do laço de Lojas
				#endregion
			}
			catch(Exception erro) 
			{	
				throw erro;
			}
		} // CaixasComLinhasPorLojas
		
		public void CaixasComLinhasPorCanaisLojas(int[] eventosInformados, int[] canaisInformados, int[] lojasInformadas,int localID,bool semTaxas) 
		{
			string empresa = string.Empty;
			string canal = string.Empty;
			string loja = string.Empty;
			string formaPagamento = string.Empty;
			//variaveis para os calculos
			decimal qtdTotalIngresso = 0;
			decimal valorTotalIngresso = 0;
			decimal valorTotalConveniencia = 0;
			//variaveis para a linha de totais 
			decimal somaValorIngressos = 0;
			decimal somaQuantidadeIngressos = 0;
			decimal somaValorConveniencia = 0;
			//Total Loja
			decimal totalLojaValorIngresso = 0;
			decimal totalLojaQuantidadeIngresso = 0;  
			decimal totalLojaValorConveniencia = 0; 
			decimal totalCanalValorIngresso = 0;
			decimal totalCanalQuantidadeIngresso = 0;
			decimal totalCanalValorConveniencia = 0;
			decimal totalEmpresaValorIngresso = 0;
			decimal totalEmpresaQuantidadeIngresso = 0;
			decimal totalEmpresaValorConveniencia = 0;
			object valorAuxiliar = 0;
			DataRow linha= null;

			try 
			{
				int tipoRelatorio = 4;
				DataTable tabela = getData(eventosInformados,canaisInformados,lojasInformadas,localID,tipoRelatorio);
				

				DataTable dtEmpresa = CTLib.TabelaMemoria.Distinct(tabela,"EmpresaNome");
				foreach(DataRow linhaEmpresa in dtEmpresa.Rows) 
				{
					// insere a linha só com o nome da Empresa.
					linha = tabelaTotal.NewRow();

					tabelaTotal.Rows.Add(linha);
					linha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
					empresa = linhaEmpresa["EmpresaNome"].ToString();
					
					DataTable dtCanal = CTLib.TabelaMemoria.DistinctComFiltro(tabela,"CanalNome","EmpresaNome = '" + empresa + "'");
					foreach(DataRow linhaCanal in dtCanal.Rows)
					{
						// insere a linha só com o nome do Canal.
						linha = tabelaTotal.NewRow();
						tabelaTotal.Rows.Add(linha);
						linha["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:40px'>Canal " + linhaCanal["CanalNome"] + "</div>";
						canal = linhaCanal["CanalNome"].ToString();

						DataTable dtLoja = CTLib.TabelaMemoria.DistinctComFiltro(tabela,"LojaNome","EmpresaNome = '" + empresa + "' and CanalNome = '" + canal + "'");
						foreach(DataRow linhaLoja in dtLoja.Rows)
						{
							// insere a linha só com o nome da Loja.
							linha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(linha);
							linha["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:75px'>Loja " + linhaLoja["LojaNome"] + "</div>";
							loja = linhaLoja["LojaNome"].ToString();

							DataTable dtFormaPagamento = CTLib.TabelaMemoria.DistinctComFiltro(tabela,"FormaPagamento","EmpresaNome = '" + empresa + "' and CanalNome = '" + canal + "' and LojaNome= '" + loja + "'");
							foreach(DataRow linhaFormaPagamento in dtFormaPagamento.Rows)
							{
								//linha de forma de pagamento
								linha = tabelaTotal.NewRow();
								tabelaTotal.Rows.Add(linha);
								formaPagamento = linhaFormaPagamento["FormaPagamento"].ToString();


								/******Calculo dos valores de linhas de formas de pagamento*********/

								//qtde total de ingressos vendido 
								valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "' and LojaNome= '" + loja + "' and FormaPagamento = '" + formaPagamento + "'");
								qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

								//valor total de ingressos vendido para a forma de pagamento
								valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "' and LojaNome= '" + loja + "' and FormaPagamento = '" + formaPagamento + "'");
								valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

								//valorTotalConveniência
								valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "'and CanalNome= '" + canal + "' and LojaNome= '" + loja + "' and FormaPagamento = '" + formaPagamento + "'");
								valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

								linha["Formas de Recebimento"] = linhaFormaPagamento["FormaPagamento"];
								linha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
								linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
								if(!semTaxas)
								{
									linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
									linha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linha["R$ Conveniência"]) + Convert.ToDecimal(linha["R$ Ingressos"])));
								}
							}

							//calculo linha total da loja
					
							//qtde total loja
							valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "' and LojaNome = '" + loja + "'");
							totalLojaQuantidadeIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valor total loja
							valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "' and LojaNome = '" + loja + "'");
							totalLojaValorIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valor total taxa conveniência loja
							valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "' AND CanalNome= '" + canal + "' and LojaNome = '" + loja + "'");
							totalLojaValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
							
	
							//insere a linha total do loja
							DataRow linhaTotalLoja = tabelaTotal.NewRow();

							linhaTotalLoja["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:75px'>Total Loja " + loja + "</div>";
							linhaTotalLoja["Quantidade"] =   totalLojaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
							linhaTotalLoja["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalLojaValorIngresso);
							if(!semTaxas)
							{
								linhaTotalLoja["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalLojaValorConveniencia);
								linhaTotalLoja["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalLojaValorIngresso + totalLojaValorConveniencia));
							}
							tabelaTotal.Rows.Add(linhaTotalLoja);
							



						}

						//calculo linha total do canal
					
						//qtde total Canal
						valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "'");
						totalCanalQuantidadeIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valor total Canal
						valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "' and CanalNome= '" + canal + "'");
						totalCanalValorIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valor total taxa conveniência Canal
						valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "' AND CanalNome= '" + canal + "'");
						totalCanalValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
							
	
						//insere a linha total do canal
						DataRow linhaTotalCanal = tabelaTotal.NewRow();

						linhaTotalCanal["Formas de Recebimento"] = "<div style= 'text-align:left;margin-left:40px'>Total Canal " + canal + "</div>";
						linhaTotalCanal["Quantidade"] =   totalCanalQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
						linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalCanalValorIngresso);
						if(!semTaxas)
						{
							linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalCanalValorConveniencia);
							linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalCanalValorIngresso + totalCanalValorConveniencia));
						}
						tabelaTotal.Rows.Add(linhaTotalCanal);
				
				
					}

					//calculo linha total da Empresa
					
					//qtde total Empresa
					valorAuxiliar = tabela.Compute("SUM(Quantidade)","EmpresaNome = '" + empresa + "'");
					totalEmpresaQuantidadeIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					//valor total Empresa
					valorAuxiliar = tabela.Compute("SUM(Valor)","EmpresaNome = '" + empresa + "'");
					totalEmpresaValorIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					//valor total taxa conveniência Empresa
					valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","EmpresaNome = '" + empresa + "'");
					totalEmpresaValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
							
	
					//insere a linha total da Empresa
					DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

					linhaTotalEmpresa["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Total Empresa " + empresa + "</div>";
					linhaTotalEmpresa["Quantidade"] =   totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
					linhaTotalEmpresa["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso);
					if(!semTaxas)
					{
						linhaTotalEmpresa["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia);
						linhaTotalEmpresa["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia));
					}
					tabelaTotal.Rows.Add(linhaTotalEmpresa);


						

				}

				//insere linha totais
				
				//qtde total 
				valorAuxiliar = tabela.Compute("SUM(Quantidade)","1=1");
				somaQuantidadeIngressos = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

				//valor total
				valorAuxiliar = tabela.Compute("SUM(Valor)","1=1");
				somaValorIngressos = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

				//Taxa Convenência Total
				valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)","1=1");
				somaValorConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


				//linha em branco
				DataRow linhaBranco = tabelaTotal.NewRow();
				tabelaTotal.Rows.Add(linhaBranco);
			
				//linha com totais
				DataRow linhaTotal = tabelaTotal.NewRow();

				linhaTotal["Formas de Recebimento"] = "<div style= 'text-align:left'>Total</div>";
				linhaTotal["Quantidade"] = somaQuantidadeIngressos.ToString(Utilitario.FormatoMoeda);
				linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(somaValorIngressos);
				if(!semTaxas)
				{
					linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(somaValorConveniencia);
					linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));
				}
				tabelaTotal.Rows.Add(linhaTotal);

			}
			catch(Exception erro) 
			{	
				throw erro;
			}

		}

		public DataTable getData(int[] eventosInformados, int[] canaisInformados, int[] lojasInformadas,int LocalID, int tipoRelatorio) 
		{

            //DataTable que recebe os dados da consulta ao Banco
            DataTable tabela = new DataTable("tabelaGetData");

            //Estrutura da tabela que receberá os dados do select
            tabela.Columns.Add("EmpresaID", typeof(int));
            tabela.Columns.Add("EmpresaNome", typeof(string));
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("EventoNome", typeof(string));
            tabela.Columns.Add("CanalID", typeof(int));
            tabela.Columns.Add("CanalNome", typeof(string));
            tabela.Columns.Add("LojaID", typeof(int));
            tabela.Columns.Add("LojaNome", typeof(string));
            tabela.Columns.Add("PagamentoID", typeof(int));
            tabela.Columns.Add("FormaPagamento", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("Quantidade", typeof(decimal));
            //tabela.Columns.Add("taxaentregavalor",typeof(decimal));//.DefaultValue = 0;
            tabela.Columns.Add("taxaconvenienciavalor", typeof(decimal));

            BD bd = new BD();

            try
            {
                string strCanais = CTLib.Utilitario.VetorInteiroParaString(canaisInformados);
                string strEventos = CTLib.Utilitario.VetorInteiroParaString(eventosInformados);
                string strLojas = CTLib.Utilitario.VetorInteiroParaString(lojasInformadas);

                if (strEventos == "")
                    strEventos = "0";
                if (strCanais == "")
                    strCanais = "0";
                if (strLojas == "")
                    strLojas = "0";

                string sql;

                //se for selecionada 1 empresa e não selecionar canal,loja e evento
                if (empresaID > 0 && canaisInformados.Length > 1 && lojasInformadas.Length > 1 && eventosInformados.Length > 1)
                {
                    sql = @"SELECT tEmpresa.ID as EmpresaID,tEmpresa.Nome as EmpresaNome,tEvento.ID as EventoID,tEvento.Nome as EventoNome,tCanal.ID as CanalID, tCanal.Nome as CanalNome, tLoja.ID as LojaID,tLoja.Nome as LojaNome,
								CASE 
									WHEN tIngressolog.CortesiaID > 0 then 
										0
									ELSE
										isnull(tFormaPagamento.ID,0) 
									END
								AS PagamentoID,
								CASE 
									WHEN tIngressolog.CortesiaID > 0 then 
										'Cortesia'
									ELSE
										isnull(tFormaPagamento.Nome ,'Cortesia') 
									END
								AS FormaPagamento ,                 
								CASE tIngressoLog.Acao 
									WHEN 'V' THEN
										MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
									ELSE
										MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
								END as taxaconvenienciavalor, 
								CASE tIngressoLog.Acao 
									WHEN 'V' THEN 
										SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
									ELSE 
										SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
								END As Valor, 
								CASE tIngressoLog.Acao
									WHEN 'V' THEN
										CASE 
											WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
												sum(1)
											ELSE
												SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
											END
									ELSE 
										CASE 
											WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
												sum(-1)
											ELSE
												SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
											END
								END AS Quantidade
								FROM tIngressolog  
								INNER JOIN tCanal ON tIngressolog.CanalID = tCanal.ID  
								INNER JOIN tCanalEvento  ON tCanal.ID = tCanalEvento.CanalID                                                             
								INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID
								INNER JOIN tEmpresa on tEmpresa.ID = tIngresso.EmpresaID 
								INNER JOIN tApresentacao ON tApresentacao.ID = tIngresso.ApresentacaoID 
								INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID AND tEvento.ID = tCanalEvento.EventoID 
								INNER JOIN tLoja ON tCanal.ID = tLoja.CanalID 
								INNER JOIN tCaixa ON tLoja.ID = tCaixa.LojaID 
								INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID 
								LEFT JOIN tVendaBilheteriaFormaPagamento ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID 
								LEFT JOIN tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID 
								INNER JOIN tvendabilheteriaitem ON tvendabilheteria.ID = tvendabilheteriaitem.vendabilheteriaID and tvendabilheteriaitem.ID = tingressolog.vendabilheteriaitemID 
								INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID
								INNER JOIN tLocal ON tLocal.ID = tEvento.LocalID ";
                    if (LocalID > 0)
                        sql += " AND tLocal.EmpresaID = " + empresaID + " AND tEvento.LocalID =  " + LocalID;



                    sql += " WHERE tIngressolog.acao in ('V','C') AND (tCaixa.DataAbertura >= '" + dataInicial + @"') AND (tCaixa.DataAbertura < '" + dataFinal + @"')
								AND (tLocal.EmpresaID = " + empresaID + " OR tCanal.EmpresaID = " + empresaID + ") ";
                    if (!comCortesias)
                        sql += " AND tingressolog.cortesiaID = 0 ";
                    //								if(LocalID > 0)
                    //									sql += " AND tEvento.LocalID = " + LocalID;
                    if (formaPagamentoID > 0)
                        sql += " AND (tFormaPagamento.ID = " + formaPagamentoID + @")";
                    sql += //@"AND  tApresentacao.DisponivelRelatorio = 'T' 
                        @" GROUP BY tingressolog.vendabilheteriaitemID,tEmpresa.ID,tEmpresa.Nome,tEvento.ID,tEvento.Nome,tCanal.ID, tCanal.Nome, tLoja.ID,tLoja.Nome, tFormaPagamento.ID,tFormaPagamento.Nome,tIngressoLog.Acao, tvendabilheteria.status,tIngressoLog.CortesiaID ";
                    if (tipoRelatorio == 1)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tcanal.nome,tcanal.ID,tloja.nome,Formapagamento";
                    if (tipoRelatorio == 2)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,Formapagamento";
                    if (tipoRelatorio == 3)
                        sql += " ORDER BY tEmpresa.Nome,tcanal.nome,tcanal.ID,Formapagamento";
                    if (tipoRelatorio == 4)
                        sql += " ORDER BY tEmpresa.Nome,tcanal.nome,tcanal.ID,tloja.nome,Formapagamento";
                    if (tipoRelatorio == 5)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tcanal.nome,tcanal.ID,Formapagamento";

                }
                else
                {
                    sql =

                        @"SELECT tEmpresa.ID as EmpresaID,tEmpresa.Nome as EmpresaNome,tEvento.ID as EventoID,tEvento.Nome as EventoNome,tCanal.ID as CanalID, tCanal.Nome as CanalNome, tLoja.ID as LojaID,tLoja.Nome as LojaNome, 
						CASE 
							WHEN tIngressolog.CortesiaID > 0 then 
								0
							ELSE
								isnull(tFormaPagamento.ID,0) 
							END
						AS PagamentoID,
						CASE 
							WHEN tIngressolog.CortesiaID > 0 then 
								'Cortesia'
							ELSE
								isnull(tFormaPagamento.Nome ,'Cortesia') 
							END
						AS FormaPagamento,                
						CASE tIngressoLog.Acao 
							WHEN 'V' THEN
								MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
							ELSE
								MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
						END as taxaconvenienciavalor, 
						CASE tIngressoLog.Acao 
							WHEN 'V' THEN 
								SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
							ELSE 
								SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
						END As Valor, 
						CASE tIngressoLog.Acao
							WHEN 'V' THEN
								CASE 
									WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
										sum(1)
									ELSE
										SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
									END
							ELSE 
								CASE 
									WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
										sum(-1)
									ELSE
										SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
									END
						END AS Quantidade
						FROM tIngressolog  
						INNER JOIN tCanal ON tIngressolog.CanalID = tCanal.ID  
						INNER JOIN tCanalEvento  ON tCanal.ID = tCanalEvento.CanalID                                                                 
						INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID
						INNER JOIN tEmpresa on tEmpresa.ID = tIngresso.EmpresaID 
						INNER JOIN tApresentacao ON tApresentacao.ID = tIngresso.ApresentacaoID 
						INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID AND tEvento.ID = tCanalEvento.EventoID  
						INNER JOIN tLoja ON tCanal.ID = tLoja.CanalID 
						INNER JOIN tCaixa ON tLoja.ID = tCaixa.LojaID 
						INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID 
						LEFT JOIN tVendaBilheteriaFormaPagamento ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID 
						LEFT JOIN tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID 
						INNER JOIN tvendabilheteriaitem ON tvendabilheteria.ID = tvendabilheteriaitem.vendabilheteriaID and tvendabilheteriaitem.ID = tingressolog.vendabilheteriaitemID 
						INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID";
                    if (empresaID > 0)
                    {
                        sql += " INNER JOIN tLocal ON tLocal.ID = tEvento.LocalID ";
                        if (LocalID > 0)
                            sql += " AND tLocal.EmpresaID = " + empresaID + " AND tEvento.LocalID =  " + LocalID;
                    }
                    sql += " WHERE tIngressolog.acao in ('V','C') AND (tCaixa.DataAbertura >= '" + dataInicial + @"') AND (tCaixa.DataAbertura < '" + dataFinal + @"')";
                    if (!comCortesias)
                        sql += " AND tingressolog.cortesiaID = 0 ";
                    if (empresaID > 0)
                        sql += " AND (tLocal.EmpresaID = " + empresaID + " OR tCanal.EmpresaID = " + empresaID + ") ";
                    //					if(LocalID > 0)
                    //						sql += " AND tEvento.LocalID = " + LocalID;
                    if (canaisInformados.Length == 1 && canaisInformados[0] > 0)
                        sql += " AND tIngressoLog.CanalID = " + canaisInformados[0];
                    if (lojasInformadas.Length == 1 && lojasInformadas[0] > 0)
                        sql += " AND tIngressoLog.LojaID = " + lojasInformadas[0];
                    if (eventosInformados.Length == 1 && eventosInformados[0] > 0)
                        sql += " AND tEvento.ID = " + eventosInformados[0];
                    if (formaPagamentoID > 0)
                        sql += " AND (tFormaPagamento.ID = " + formaPagamentoID + @")";
                    //sql += @"AND  tApresentacao.DisponivelRelatorio = 'T' 
                    sql += " GROUP BY tingressolog.vendabilheteriaitemID,tEmpresa.ID,tEmpresa.Nome,tEvento.ID,tEvento.Nome,tCanal.ID, tCanal.Nome, tLoja.ID,tLoja.Nome, tFormaPagamento.ID,tFormaPagamento.Nome,tIngressoLog.Acao, tvendabilheteria.status,tIngressoLog.CortesiaID ";
                    if (tipoRelatorio == 1)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tcanal.nome,tcanal.ID,tloja.nome,Formapagamento";
                    if (tipoRelatorio == 2)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,Formapagamento";
                    if (tipoRelatorio == 3)
                        sql += " ORDER BY tEmpresa.Nome,tcanal.nome,tcanal.ID,Formapagamento";
                    if (tipoRelatorio == 4)
                        sql += " ORDER BY tEmpresa.Nome,tcanal.nome,tcanal.ID,tloja.nome,Formapagamento";
                    if (tipoRelatorio == 5)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tcanal.nome,tcanal.ID,Formapagamento";

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();


                    if (tipoRelatorio != 3)
                    {
                        linhaTabela["EventoID"] = bd.LerInt("EventoID");
                        linhaTabela["EventoNome"] = bd.LerString("EventoNome");
                        linhaTabela["LojaID"] = bd.LerInt("LojaID");
                        linhaTabela["LojaNome"] = bd.LerString("LojaNome");
                    }
                    linhaTabela["EmpresaID"] = bd.LerInt("EmpresaID");
                    linhaTabela["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linhaTabela["CanalID"] = bd.LerInt("CanalID");
                    linhaTabela["CanalNome"] = bd.LerString("CanalNome");
                    linhaTabela["PagamentoID"] = bd.LerInt("PagamentoID");
                    linhaTabela["FormaPagamento"] = bd.LerString("FormaPagamento");
                    linhaTabela["Valor"] = bd.LerDecimal("Valor");
                    linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
                    //linhaTabela["taxaentregavalor"] = bd.LerDecimal("taxaentregavalor");
                    linhaTabela["taxaconvenienciavalor"] = bd.LerDecimal("taxaconvenienciavalor");


                    tabela.Rows.Add(linhaTabela);
                }

                bd.Fechar();

            }
            catch (Exception)
            {
                throw;
                //return this.getData1(eventosInformados,canaisInformados,lojasInformadas,tipoRelatorio);
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;	
		
		}//fim get data

		
		public DataTable getData1(int[] eventosInformados, int[] canaisInformados, int[] lojasInformadas, int tipoRelatorio) 
		{
			string strCanais = CTLib.Utilitario.VetorInteiroParaString(canaisInformados);
			string strEventos = CTLib.Utilitario.VetorInteiroParaString(eventosInformados);
			string strLojas = CTLib.Utilitario.VetorInteiroParaString(lojasInformadas);

			if(strEventos == "")
				strEventos = "0";
			if(strCanais == "")
				strCanais = "0";
			if(strLojas == "")
				strLojas = "0";

			
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabela = new DataTable("tabelaGetData");
			
			//Estrutura da tabela que receberá os dados do select
			tabela.Columns.Add("EmpresaID",typeof(int));
			tabela.Columns.Add("EmpresaNome",typeof(string));
			tabela.Columns.Add("EventoID",typeof(int));
			tabela.Columns.Add("EventoNome",typeof(string));
			tabela.Columns.Add("CanalID",typeof(int));
			tabela.Columns.Add("CanalNome",typeof(string));
			tabela.Columns.Add("LojaID",typeof(int));
			tabela.Columns.Add("LojaNome",typeof(string));
			tabela.Columns.Add("PagamentoID",typeof(int));
			tabela.Columns.Add("FormaPagamento",typeof(string));
			tabela.Columns.Add("Valor",typeof(decimal));
			tabela.Columns.Add("Quantidade",typeof(decimal));
			//tabela.Columns.Add("taxaentregavalor",typeof(decimal));//.DefaultValue = 0;
			tabela.Columns.Add("taxaconvenienciavalor",typeof(decimal));		

			BD bd = new BD();

            try
            {
                string sql;

                if (tipoRelatorio == 3)
                {
                    sql = @"SELECT tEmpresa.ID as EmpresaID,tEmpresa.Nome as EmpresaNome,tCanal.ID as CanalID, tCanal.Nome as CanalNome,isnull(tFormaPagamento.ID,0) as PagamentoID,isnull(tFormaPagamento.Nome ,'Cortesia')as FormaPagamento, 
						CASE tIngressoLog.Acao 
							WHEN 'V' THEN
								MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
							ELSE
								MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
						END as taxaconvenienciavalor, 
						CASE tIngressoLog.Acao 
							WHEN 'V' THEN 
								SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
							ELSE 
								SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
						END As Valor, 
						CASE tIngressoLog.Acao
							WHEN 'V' THEN
								CASE 
									WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
										sum(1)
									ELSE
										SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
									END
							ELSE 
								CASE 
									WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
										sum(-1)
									ELSE
										SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
									END
						END AS Quantidade
						FROM tIngressolog  
						INNER JOIN tCanal ON tIngressolog.CanalID = tCanal.ID                                                               
						INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID
						INNER JOIN tApresentacao ON tApresentacao.ID = tIngresso.ApresentacaoID 
						INNER JOIN tEmpresa on tEmpresa.ID = tIngresso.EmpresaID 
						INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID 
						INNER JOIN tLoja ON tCanal.ID = tLoja.CanalID 
						INNER JOIN tCaixa ON tLoja.ID = tCaixa.LojaID 
						INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID  
						LEFT JOIN tVendaBilheteriaFormaPagamento ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID 
						LEFT JOIN tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID 
						INNER JOIN tvendabilheteriaitem ON tvendabilheteria.ID = tvendabilheteriaitem.vendabilheteriaID and tvendabilheteriaitem.ID = tingressolog.vendabilheteriaitemID 
						INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID
						WHERE tIngressolog.acao in ('V','C') AND (tCaixa.DataAbertura >= '" + dataInicial + @"') AND (tCaixa.DataAbertura < '" + dataFinal + @"')
						AND (tIngresso.EventoID IN (" + strEventos + @")) ";

                    if (!comCortesias)
                        sql += " AND (tingressolog.cortesiaID = 0) ";
                    if (strCanais != "0")
                        sql += " AND (tIngressolog.CanalID IN (" + strCanais + @")) ";
                    if (strLojas != "0")
                        sql += " AND (tIngressolog.LojaID IN (" + strLojas + @")) ";
                    if (formaPagamentoID > 0)
                        sql += " AND (tFormaPagamento.ID = " + formaPagamentoID + @")";
                    sql += @"AND  tApresentacao.DisponivelRelatorio = 'T'
								GROUP BY tingressolog.vendabilheteriaitemID,tEmpresa.ID,tEmpresa.Nome,tCanal.ID, tCanal.Nome,tFormaPagamento.ID,tFormaPagamento.Nome,tIngressoLog.Acao, tvendabilheteria.status 
								ORDER BY tEmpresa.Nome,tcanal.nome,tCanal.ID,tformapagamento.nome ";

                }

                else
                {

                    sql =

                        @"SELECT tEmpresa.ID as EmpresaID,tEmpresa.Nome as EmpresaNome,tEvento.ID as EventoID,tEvento.Nome as EventoNome,tCanal.ID as CanalID, tCanal.Nome as CanalNome, tLoja.ID as LojaID,tLoja.Nome as LojaNome,isnull(tFormaPagamento.ID,0) as PagamentoID,isnull(tFormaPagamento.Nome ,'Cortesia')as FormaPagamento, 
						CASE tIngressoLog.Acao 
							WHEN 'V' THEN
								MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
							ELSE
								MIN(tvendabilheteriaitem.taxaconvenienciavalor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
						END as taxaconvenienciavalor, 
						CASE tIngressoLog.Acao 
							WHEN 'V' THEN 
								SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
							ELSE 
								SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
						END As Valor, 
						CASE tIngressoLog.Acao
							WHEN 'V' THEN
								CASE 
									WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
										sum(1)
									ELSE
										SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
									END
							ELSE 
								CASE 
									WHEN (Count(tVendaBilheteriaFormaPagamento.ID) = 0) THEN
										sum(-1)
									ELSE
										SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
									END
						END AS Quantidade
						FROM tIngressolog  
						INNER JOIN tCanal ON tIngressolog.CanalID = tCanal.ID                                                               
						INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID
						INNER JOIN tEmpresa on tEmpresa.ID = tIngresso.EmpresaID 
						INNER JOIN tApresentacao ON tApresentacao.ID = tIngresso.ApresentacaoID 
						INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID 
						INNER JOIN tLoja ON tCanal.ID = tLoja.CanalID 
						INNER JOIN tCaixa ON tLoja.ID = tCaixa.LojaID 
						INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID  
						LEFT JOIN tVendaBilheteriaFormaPagamento ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID 
						LEFT JOIN tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID 
						INNER JOIN tvendabilheteriaitem ON tvendabilheteria.ID = tvendabilheteriaitem.vendabilheteriaID and tvendabilheteriaitem.ID = tingressolog.vendabilheteriaitemID 
						INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID
						WHERE tIngressolog.acao in ('V','C') AND (tCaixa.DataAbertura >= '" + dataInicial + @"') AND (tCaixa.DataAbertura < '" + dataFinal + @"')
						AND (tIngresso.EventoID IN (" + strEventos + @")) ";
                    if (!comCortesias)
                        sql += " AND (tingressolog.cortesiaID = 0) ";
                    if (strCanais != "0")
                        sql += " AND (tIngressolog.CanalID IN (" + strCanais + @")) ";
                    if (strLojas != "0")
                        sql += " AND (tIngressolog.LojaID IN (" + strLojas + @")) ";

                    if (formaPagamentoID > 0)
                        sql += " AND (tFormaPagamento.ID = " + formaPagamentoID + @")";
                    sql += @"AND  tApresentacao.DisponivelRelatorio = 'T'
						GROUP BY tingressolog.vendabilheteriaitemID,tEmpresa.ID,tEmpresa.Nome,tEvento.ID,tEvento.Nome,tCanal.ID, tCanal.Nome, tLoja.ID,tLoja.Nome, tFormaPagamento.ID,tFormaPagamento.Nome,tIngressoLog.Acao, tvendabilheteria.status";
                    if (tipoRelatorio == 1)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tcanal.nome,tcanal.ID,tloja.nome,tformapagamento.nome";
                    if (tipoRelatorio == 2)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tformapagamento.nome";
                    if (tipoRelatorio == 3)
                        sql += " ORDER BY tEmpresa.Nome,tcanal.nome,tcanal.ID,tformapagamento.nome";
                    if (tipoRelatorio == 4)
                        sql += " ORDER BY tEmpresa.Nome,tcanal.nome,tcanal.ID,tloja.nome,tformapagamento.nome";
                    if (tipoRelatorio == 5)
                        sql += " ORDER BY tEmpresa.nome,tevento.nome,tcanal.nome,tcanal.ID,tformapagamento.nome";
                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();


                    if (tipoRelatorio != 3)
                    {
                        linhaTabela["EventoID"] = bd.LerInt("EventoID");
                        linhaTabela["EventoNome"] = bd.LerString("EventoNome");
                        linhaTabela["LojaID"] = bd.LerInt("LojaID");
                        linhaTabela["LojaNome"] = bd.LerString("LojaNome");
                    }
                    linhaTabela["EmpresaID"] = bd.LerInt("EmpresaID");
                    linhaTabela["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linhaTabela["CanalID"] = bd.LerInt("CanalID");
                    linhaTabela["CanalNome"] = bd.LerString("CanalNome");
                    linhaTabela["PagamentoID"] = bd.LerInt("PagamentoID");
                    linhaTabela["FormaPagamento"] = bd.LerString("FormaPagamento");
                    linhaTabela["Valor"] = bd.LerDecimal("Valor");
                    linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
                    //linhaTabela["taxaentregavalor"] = bd.LerDecimal("taxaentregavalor");
                    linhaTabela["taxaconvenienciavalor"] = bd.LerDecimal("taxaconvenienciavalor");

                    tabela.Rows.Add(linhaTabela);
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                bd.Fechar();
            }
			return tabela;			
		
		}//fim get data

		
		//Obtem os dados para a tabela de taxa de conveniência
		public DataTable getDataTaxaEntrega(int empresaID,int localID,int eventoID,int canalID,int lojaID,int formaPagamentoID,bool comCortesias) 
		{
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabela = new DataTable("tabelaGetDataTaxaEntrega");
			
			//Estrutura da tabela que receberá os dados do select
			tabela.Columns.Add("TaxaEntrega",typeof(string));
			tabela.Columns.Add("TaxaEntregaID",typeof(int));
			tabela.Columns.Add("TaxaEntregaValor",typeof(decimal));
            
			BD bd = new BD();

            try
            {          
    //			string sql =
    // 
    //				@"SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
    //					CASE tvendabilheteria.status
    //						WHEN 'P' THEN 
    //							tvendabilheteria.taxaentregavalor 
    //						ELSE 
    //							tvendabilheteria.taxaentregavalor * (-1)
    //						END 
    //					As TaxaEntregaValor
    //					FROM tIngressolog (NOLOCK) 
    //					INNER JOIN tCanal (NOLOCK) ON tIngressolog.CanalID = tCanal.ID                                                               
    //					INNER JOIN tIngresso (NOLOCK) ON tIngressolog.IngressoID = tIngresso.ID
    //					INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID 
    //					INNER JOIN tEvento (NOLOCK) ON tIngresso.EventoID = tEvento.ID 
    //					INNER JOIN tLoja (NOLOCK) ON tCanal.ID = tLoja.CanalID and tIngressoLog.LojaID = tLoja.ID 
    //					INNER JOIN tCaixa (NOLOCK) ON tLoja.ID = tCaixa.LojaID 
    //					INNER JOIN tVendaBilheteria (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID  
    //					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID 
    //					LEFT JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID 
    //					INNER JOIN tvendabilheteriaitem (NOLOCK) ON tvendabilheteria.ID = tvendabilheteriaitem.vendabilheteriaID and tvendabilheteriaitem.ID = tingressolog.vendabilheteriaitemID 
    //					INNER JOIN tTaxaEntrega (NOLOCK) ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
    //					WHERE tIngressolog.acao in ('V','C') AND (tCaixa.DataAbertura >= '" + dataInicial + @"') AND (tCaixa.DataAbertura < '" + dataFinal + "')";
    //					if(empresaID > 0)
    //						sql += " AND (tIngresso.EmpresaID = " + empresaID + " OR tCanal.EmpresaID = " + empresaID + ") ";
    //					if(localID > 0)
    //						sql += " AND tIngresso.LocalID = " + localID;
    //					if(eventoID > 0)
    //						sql += " AND tIngresso.EventoID = " + eventoID;
    //					if(canalID > 0)
    //						sql += " AND tIngressoLog.CanalID = " + canalID;
    //					if(lojaID > 0)
    //						sql += " AND tIngressoLog.LojaID = " + lojaID;
    //					if(!comCortesias)
    //						sql += " AND tingressolog.cortesiaID = 0 ";
    //					if(formaPagamentoID > 0)
    //						sql += " AND tFormaPagamento.ID = " + formaPagamentoID;
    //					
    //					sql += @" AND tVendaBilheteria.taxaEntregaValor > 0 ";
    //					sql += @"GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
    //					ORDER BY tTaxaEntrega.Nome";

				    string sql = @"
    					
					    CREATE TABLE #Final
					    (
					    EmpresaID INT,
					    EmpresaNome NVARCHAR(100), 
					    LocalID INT,
					    LocalNome NVARCHAR(100), 
					    EventoID INT,
					    EventoNome NVARCHAR(100), 
					    CanalID INT,
					    CanalNome NVARCHAR(100),
					    LojaID INT,
					    LojaNome NVARCHAR(100), 
					    PagamentoID INT,
					    PagamentoNome NVARCHAR(100), 
					    Quantidade  DECIMAL(12,2),
					    Valor DECIMAL(12,2), 
					    Taxa DECIMAL(12,2), 
					    Comissao DECIMAL(12,2)
					    )

					    CREATE TABLE #Vendas 
					    (	
					    VendaBilheteriaID INT, 
					    TaxaConvenienciaValorTotal DECIMAL(12,2), 
					    ValorTotal DECIMAL(12,2), 
                        ComissaoTotal DECIMAL(12,2), 
                        PagamentoID INT, 
					    PagamentoNome NVARCHAR(50), 
					    PagamentoValor DECIMAL(12,2), 
					    PagamentoPct float, 
					    CanalID INT, 
					    CanalNome NVARCHAR(50), 
					    LojaID INT,
					    LojaNome NVARCHAR(50),
					    TaxaEntregaID INT,
					    TaxaEntregaValor DECIMAL(12,2)
					    ) 

					    CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)

    						
					    INSERT INTO #Vendas 
					    SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					    tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome,tVendaBilheteria.TaxaEntregaID,tVendaBilheteria.TaxaEntregaValor
					    FROM tVendaBilheteria (NOLOCK)
					    INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID 
					    INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					    INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					    LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					    LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					    WHERE DataAbertura >= '" +  dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
					    if(canalID > 0)
						    sql += " AND tCanal.ID = " + canalID;
					    if(lojaID > 0)
						    sql += " AND tLoja.ID = " + lojaID;
					    if(formaPagamentoID > 0)
						    sql += " AND tFormaPagamento.ID = " + formaPagamentoID;

    					
						    sql += @"SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
						    CASE tvendabilheteria.status
							    WHEN 'P' THEN 
								    tvendabilheteria.taxaentregavalor 
							    ELSE 
								    tvendabilheteria.taxaentregavalor * (-1)
							    END 
						    As TaxaEntregaValor
						    FROM tIngressoLog
						    INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
						    inner join tVendaBilheteria ON tVendaBilheteria.ID = vendas.VendaBilheteriaID
						    INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressolog.IngressoID
						    INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
						    INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
						    WHERE tIngressoLog.Acao IN ('C','V') and tVendaBilheteria.taxaEntregaValor > 0 
						    group by ttaxaentrega.id,ttaxaentrega.Nome,tVendaBilheteria.ID,tVendaBilheteria.TaxaEntregaValor,tVendaBilheteria.Status
						    ORDER BY TaxaEntrega ";


			    bd.Consulta(sql);

			    while(bd.Consulta().Read()) 
			    {
				    DataRow linhaTabela = tabela.NewRow();

				    linhaTabela["TaxaEntrega"] = bd.LerString("TaxaEntrega");
				    linhaTabela["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
				    linhaTabela["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

				    tabela.Rows.Add(linhaTabela);
			    }

			    bd.Fechar();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                bd.Fechar();
            }	

			return tabela;			
		
		}//fim getDataTaxaEntrega

		public DataTable TabelaFinalTaxaEntrega(DataTable entrega) 
		{
			try 
			{

				//Variáveis
				object valorAuxiliar = 0;
				decimal valorTaxaEntrega = 0;
				decimal valorTaxaEntregaTotal = 0;
				DataTable dtTaxa = null;

				
				DataRow linhaTaxa = null;
				DataTable tabela = entrega;

				dtTaxa = CTLib.TabelaMemoria.DistinctSort(tabela,"TaxaEntrega","1=1","TaxaEntrega");
				
				for(int i=0;i<=dtTaxa.Rows.Count - 1;i++)
				{
					linhaTaxa = dtTaxa.Rows[i];


					//Valor taxa para cada Tipo de Entrega 
					valorAuxiliar = tabela.Compute("SUM(TaxaEntregaValor)", "TaxaEntrega= '" + linhaTaxa["TaxaEntrega"] + "'");
					valorTaxaEntrega = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					DataRow linhaFinal = tabelaEntrega.NewRow();

					linhaFinal["Taxa de Entrega"] = "<div style='text-align:left'>" + linhaTaxa["TaxaEntrega"] + "</div>";
					linhaFinal["Valor"] = Utilitario.AplicaFormatoMoeda(valorTaxaEntrega);

					tabelaEntrega.Rows.Add(linhaFinal);

					//soma para linha de total
					valorTaxaEntregaTotal = valorTaxaEntregaTotal + valorTaxaEntrega;
					

				}

				//linha Branco
				DataRow branco = tabelaEntrega.NewRow();
				tabelaEntrega.Rows.Add(branco);

				//linha Total
				DataRow total = tabelaEntrega.NewRow();
				
				total["Taxa de Entrega"] = "<div style='text-align:left'>Total</div>";
				total["Valor"] = Utilitario.AplicaFormatoMoeda(valorTaxaEntregaTotal);

				tabelaEntrega.Rows.Add(total);


				return tabelaEntrega;

			}
			catch(Exception erro) 
			{
				throw erro;
			}
		}
					

		
		public void CaixasComLinhasPorEventosCanaisLojas(int[] eventosInformados, int[] canaisInformados, int[] lojasInformadas,int localID,bool semTaxas) 
		{
			try 
			{
				
				int tipoRelatorio = 1;
				DataTable tabela = getData(eventosInformados,canaisInformados,lojasInformadas,localID,tipoRelatorio);
				
				int empresaIDAtual = 0;
				int empresaIDAnterior = 0;		//EMPRESA
				int eventoIDAtual = 0;
				int eventoIDAnterior = 0;
				int canalIDAtual = 0;
				int canalIDAnterior = 0;
				int lojaIDAtual = 0;
				int lojaIDAnterior = 0;
				int formaPagamentoIDAtual = 0;
				int formaPagamentoIDAnterior = 0;

				//variaveis para os calculos
				decimal qtdTotalIngresso = 0;
				decimal valorTotalIngresso = 0;
				//decimal valorTotalEntrega = 0;
				decimal valorTotalConveniencia = 0;
				
				//variaveis para a linha de totais 
				decimal somaValorIngressos = 0;
				decimal somaQuantidadeIngressos = 0;
				decimal somaValorConveniencia = 0;
				string nomeAnteriorEmpresa = string.Empty;
				string nomeAnteriorEvento = string.Empty;
				string nomeAnteriorCanal = string.Empty;
				string nomeAnteriorLoja = string.Empty;
				//Total Loja
				decimal totalLojaValorIngresso = 0;
				decimal totalLojaQuantidadeIngresso = 0;  
				decimal totalLojaValorConveniencia = 0; 
				//Total Canal
				decimal totalCanalValorIngresso = 0;
				decimal totalCanalQuantidadeIngresso = 0;
				decimal totalCanalValorEntrega = 0;
				decimal totalCanalValorConveniencia = 0;
				//Total Evento
				decimal totalEventoValorIngresso = 0;
				decimal totalEventoQuantidadeIngresso = 0;
				decimal totalEventoValorConveniencia = 0;
				//Total Empresa
				decimal totalEmpresaValorIngresso = 0;
				decimal totalEmpresaQuantidadeIngresso = 0;
				decimal totalEmpresaValorConveniencia = 0;
				object valorAuxiliar = 0;
				
				DataRow novaLinha = null;
				DataRow linha = null;

				for (int i = 0; i <= tabela.Rows.Count - 1; i++) 
				{
					linha = tabela.Rows[i];
					
					empresaIDAtual = (int)linha["EmpresaID"]; //EMPRESA
					eventoIDAtual = (int)linha["EventoID"];
					canalIDAtual  = (int)linha["CanalID"];
					lojaIDAtual   = (int)linha["LojaID"];
					formaPagamentoIDAtual = (int)linha["PagamentoID"];
					
					if(i > 0) 
					{
						if(lojaIDAtual != lojaIDAnterior || ((lojaIDAtual == lojaIDAnterior) && (eventoIDAtual != eventoIDAnterior))) 
						{
							//insere a linha total da loja
							DataRow linhaTotalLoja = tabelaTotal.NewRow();

							linhaTotalLoja["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Total Loja " + nomeAnteriorLoja + "</div>";
							linhaTotalLoja["Quantidade"] = totalLojaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
							linhaTotalLoja["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalLojaValorIngresso);
							//exibido apenas para perfil financeiro especial
							if(!semTaxas)
							{
								linhaTotalLoja["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalLojaValorConveniencia);
								linhaTotalLoja["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalLojaValorIngresso + totalLojaValorConveniencia)); 
							}

							tabelaTotal.Rows.Add(linhaTotalLoja);
						
							//zera as variáveis para a proxima loja
							totalLojaValorIngresso = 0;
							totalLojaQuantidadeIngresso = 0;  
							//totalLojaValorEntrega = 0;
							totalLojaValorConveniencia = 0; 
							//totalLojaSomaTotal = 0; 
						}
						if(canalIDAtual != canalIDAnterior || ((canalIDAtual == canalIDAnterior) && (eventoIDAtual != eventoIDAnterior))) 
						{

							//insere a linha total do canal
							DataRow linhaTotalCanal = tabelaTotal.NewRow();

							linhaTotalCanal["Formas de Recebimento"] = "<div style='text-align:left;margin-left:75px'>Total Canal " + nomeAnteriorCanal + "</div>";
							linhaTotalCanal["Quantidade"] = totalCanalQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
							linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalCanalValorIngresso);
							//exibido apenas para perfil financeiro especial
							if(!semTaxas)
							{
								linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalCanalValorConveniencia);
								linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalCanalValorIngresso + totalCanalValorEntrega + totalCanalValorConveniencia));
							}
								tabelaTotal.Rows.Add(linhaTotalCanal);
						
							//zera as variáveis para o proximo canal
							totalCanalValorIngresso = 0;
							totalCanalQuantidadeIngresso = 0;
							//totalCanalValorEntrega = 0;
							totalCanalValorConveniencia = 0;
							//totalCanalSomaTotal = 0;
						}
						if(eventoIDAtual != eventoIDAnterior) 
						{

							//insere a linha total do Evento
							DataRow linhaTotalEvento = tabelaTotal.NewRow();

							linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Evento " + nomeAnteriorEvento + "</div>";
							linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + totalEventoQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
							linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorIngresso) + "</div>";
							//exibido apenas para perfil financeiro especial
							if(!semTaxas)
							{
								linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorConveniencia) + "</div>";
								linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((totalEventoValorIngresso + totalEventoValorConveniencia)) + "<div>";
							}
							tabelaTotal.Rows.Add(linhaTotalEvento);
						
							//zera as variáveis para o proximo Evento
							totalEventoValorIngresso = 0;
							totalEventoQuantidadeIngresso = 0;
							totalEventoValorConveniencia = 0;
						}

						if(empresaIDAtual != empresaIDAnterior)
						{
							//insere a linha total do Empresa
							DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

							linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + nomeAnteriorEmpresa+ "</div>";
							linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
							linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso) + "</div>";
							//exibido apenas para perfil financeiro especial
							if(!semTaxas)
							{
								linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia) + "</div>";
								linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia)) + "<div>";
							}
							tabelaTotal.Rows.Add(linhaTotalEmpresa);
						
							//zera as variáveis para a próxima empresa
							totalEmpresaValorIngresso = 0;
							totalEmpresaQuantidadeIngresso = 0;
							totalEmpresaValorConveniencia = 0;
						
						}
						
					}
					
					//se a empresa mudou
					if(empresaIDAtual != empresaIDAnterior)
					{
						// insere a linha só com o nome da Empresa.
						novaLinha = tabelaTotal.NewRow();

						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linha["EmpresaNome"] + "</div>";
					}

					// Se mudou o evento.
					if (eventoIDAtual != eventoIDAnterior) 
					{

						// insere a linha só com o nome do evento.
						novaLinha = tabelaTotal.NewRow();
						
						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Evento " + linha["EventoNome"] + "</div>";


						// insere a linha só com o nome do Canal.
						novaLinha = tabelaTotal.NewRow();
						
						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:75px'>Canal " + linha["CanalNome"] + "</div>";

						// insere a linha só com o nome do Loja.
						novaLinha = tabelaTotal.NewRow();
						
						tabelaTotal.Rows.Add(novaLinha);
						novaLinha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Loja " + linha["LojaNome"] + "</div>";

						// insere a linha só com o nome do Forma de pagamento.		
						novaLinha = tabelaTotal.NewRow();
						tabelaTotal.Rows.Add(novaLinha);


						/******Calculo dos valores de linhas de formas de pagamento*********/

						//qtde total de ingressos vendido 
						valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


						//valor total de ingressos vendido para a forma de pagamento
						valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//valorTotalConveniência
						valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
						valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


						novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];

						novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
						novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
						//exibido apenas para perfil financeiro especial
						if(!semTaxas)
						{
							novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
							novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"])+ Convert.ToDecimal(novaLinha["R$ Ingressos"])));
						}

						/**************Fim Calculo dos valores de linhas de formas de pagamento***********/
 
						

					}
					else 
					{ // se continua no mesmo evento, procurar canal.
						
						//se mudou o canal
						if(canalIDAtual != canalIDAnterior) 
						{
							// insere a linha só com o nome do Canal.
							novaLinha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(novaLinha);
							novaLinha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:75px'>Canal " + linha["CanalNome"] + "</div>";

							// insere a linha só com o nome do Loja.
							novaLinha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(novaLinha);
							novaLinha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Loja " + linha["LojaNome"] + "</div>";

							// insere a linha só com o nome do Forma de pagamento.		
							novaLinha = tabelaTotal.NewRow();
							tabelaTotal.Rows.Add(novaLinha);

							/******Calculo dos valores de linhas de formas de pagamento*********/

							//qtde total de ingressos vendido 
							valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


							//valor total de ingressos vendido para a forma de pagamento
							valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

							//valorTotalConveniência
							valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
							valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


							novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];

							novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda);
							novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
							//exibido apenas para perfil financeiro especial
							if(!semTaxas)
							{
								novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
								novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
							}
							/**************Fim Calculo dos valores de linhas de formas de pagamento***********/
						
						}
						else 
						{ // se continua no mesmo canal, procurar loja.
							//se mudou a loja
							if(lojaIDAtual != lojaIDAnterior) 
							{
								// insere a linha só com o nome do Loja.
								novaLinha = tabelaTotal.NewRow();
								tabelaTotal.Rows.Add(novaLinha);
								novaLinha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Loja " + linha["LojaNome"] + "</div>";

								// insere a linha só com o nome do Forma de pagamento.		
								novaLinha = tabelaTotal.NewRow();
								tabelaTotal.Rows.Add(novaLinha);

								/******Calculo dos valores de linhas de formas de pagamento*********/

								//qtde total de ingressos vendido 
								valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
								qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


								//valor total de ingressos vendido para a forma de pagamento
								valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
								valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

								//valorTotalConveniência
								valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
								valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


								novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];
								novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
								novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
								//exibido apenas para perfil financeiro especial
								if(!semTaxas)
								{
									novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
									novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
								}
								/**************Fim Calculo dos valores de linhas de formas de pagamento***********/
							}
							else 
							{ // se continua a mesma loja, procurar forma pagamento.
								//se mudou a forma de pagamento
								if(formaPagamentoIDAtual != formaPagamentoIDAnterior) 
								{
									
									// insere a linha só com o nome do Forma de pagamento.		
									novaLinha = tabelaTotal.NewRow();
									tabelaTotal.Rows.Add(novaLinha);

									/******Calculo dos valores de linhas de formas de pagamento*********/

									//qtde total de ingressos vendido 
									valorAuxiliar = tabela.Compute("SUM(Quantidade)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
									qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


									//valor total de ingressos vendido para a forma de pagamento
									valorAuxiliar = tabela.Compute("SUM(Valor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
									valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

									//valorTotalConveniência
									valorAuxiliar = tabela.Compute("SUM(taxaconvenienciavalor)", "EventoID=" + eventoIDAtual + " and CanalID=" + canalIDAtual + " and LojaID=" + lojaIDAtual + " and PagamentoID=" + formaPagamentoIDAtual);
									valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


									novaLinha["Formas de Recebimento"] = linha["FormaPagamento"];
									novaLinha["Quantidade"] = qtdTotalIngresso.ToString(Utilitario.FormatoMoeda); 
									novaLinha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalIngresso); 
									//exibido apenas para perfil financeiro especial
									if(!semTaxas)
									{
										novaLinha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(valorTotalConveniencia);
										novaLinha["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(novaLinha["R$ Conveniência"]) + Convert.ToDecimal(novaLinha["R$ Ingressos"])));
									}
									/**************Fim Calculo dos valores de linhas de formas de pagamento***********/

								}
							}
						}
					}
					
					/****variaveis de totais de loja,canal,evento - são zeradas conforme são inseridas linhas de loja,canal e evento**/
					
					if (!(eventoIDAtual == eventoIDAnterior && canalIDAnterior == canalIDAtual && lojaIDAtual == lojaIDAnterior && formaPagamentoIDAnterior == formaPagamentoIDAtual)) 
					{	
						//soma de totais
						somaValorIngressos = somaValorIngressos + valorTotalIngresso;
						somaQuantidadeIngressos = somaQuantidadeIngressos + qtdTotalIngresso; 
						//somaValorEntrega = somaValorEntrega + valorTotalEntrega;
						somaValorConveniencia = somaValorConveniencia + valorTotalConveniencia;			 
						//Total Loja
						totalLojaValorIngresso = totalLojaValorIngresso + valorTotalIngresso;
						totalLojaQuantidadeIngresso = totalLojaQuantidadeIngresso + qtdTotalIngresso;
						//totalLojaValorEntrega = totalLojaValorEntrega + valorTotalEntrega;
						totalLojaValorConveniencia = totalLojaValorConveniencia + valorTotalConveniencia; 
						//Total Canal
						totalCanalValorIngresso = totalCanalValorIngresso + valorTotalIngresso;
						totalCanalQuantidadeIngresso = totalCanalQuantidadeIngresso + qtdTotalIngresso;
						totalCanalValorConveniencia = totalCanalValorConveniencia + valorTotalConveniencia;
						//Total Evento
						totalEventoValorIngresso = totalEventoValorIngresso + valorTotalIngresso;
						totalEventoQuantidadeIngresso = totalEventoQuantidadeIngresso + qtdTotalIngresso;
						totalEventoValorConveniencia = totalEventoValorConveniencia + valorTotalConveniencia;
						//Total Empresa
						totalEmpresaValorIngresso = totalEmpresaValorIngresso + valorTotalIngresso;
						totalEmpresaQuantidadeIngresso = totalEmpresaQuantidadeIngresso + qtdTotalIngresso;
						totalEmpresaValorConveniencia = totalEmpresaValorConveniencia + valorTotalConveniencia;


						
					}
							
					// Define ID atual e anterior para cada linha do laço
					empresaIDAnterior = empresaIDAtual;
					eventoIDAnterior = eventoIDAtual;
					canalIDAnterior = canalIDAtual;
					lojaIDAnterior = lojaIDAtual;
					formaPagamentoIDAnterior = formaPagamentoIDAtual;
					//nome do anterior para linha de total
					nomeAnteriorEmpresa = (string) linha["EmpresaNome"];
					nomeAnteriorEvento = (string) linha["EventoNome"];
					nomeAnteriorCanal = (string) linha["CanalNome"];
					nomeAnteriorLoja = (string) linha["LojaNome"];
					//insere os ultimos totais de loja, canal e evento
					if(i == tabela.Rows.Count - 1) 
					{
						//insere a linha total da loja
						DataRow linhaTotalLoja = tabelaTotal.NewRow();

						linhaTotalLoja["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Total Loja " + nomeAnteriorLoja + "</div>";
						linhaTotalLoja["Quantidade"] = totalLojaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
						linhaTotalLoja["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalLojaValorIngresso);
						//exibido apenas para perfil financeiro especial
						if(!semTaxas)
						{
							linhaTotalLoja["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalLojaValorConveniencia);
							linhaTotalLoja["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalLojaValorIngresso + totalLojaValorConveniencia)); 
						}
						tabelaTotal.Rows.Add(linhaTotalLoja);

						//insere a linha total do canal
						DataRow linhaTotalCanal = tabelaTotal.NewRow();

						linhaTotalCanal["Formas de Recebimento"] = "<div style='text-align:left;margin-left:75px'>Total Canal " + nomeAnteriorCanal + "</div>";
						linhaTotalCanal["Quantidade"] = totalCanalQuantidadeIngresso.ToString(Utilitario.FormatoMoeda);
						linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(totalCanalValorIngresso);
						//exibido apenas para perfil financeiro especial
						if(!semTaxas)
						{
							linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(totalCanalValorConveniencia);
							linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((totalCanalValorIngresso + totalCanalValorEntrega + totalCanalValorConveniencia));
						}
						tabelaTotal.Rows.Add(linhaTotalCanal);

						//insere a linha total do Evento
						DataRow linhaTotalEvento = tabelaTotal.NewRow();

						linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Evento " + nomeAnteriorEvento + "</div>";
						linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + totalEventoQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
						linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorIngresso) + "</div>";
						//exibido apenas para perfil financeiro especial
						if(!semTaxas)
						{
							linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEventoValorConveniencia) + "</div>";
							linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((totalEventoValorIngresso + totalEventoValorConveniencia)) + "<div>";
						}
						tabelaTotal.Rows.Add(linhaTotalEvento);

						//insere a linha total da Empresa
						DataRow linhaTotalEmpresa = tabelaTotal.NewRow();

						linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + nomeAnteriorEmpresa + "</div>";
						linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + totalEmpresaQuantidadeIngresso.ToString(Utilitario.FormatoMoeda) + "</div>";
						linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorIngresso) + "</div>";
						//exibido apenas para perfil financeiro especial
						if(!semTaxas)
						{
							linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(totalEmpresaValorConveniencia) + "</div>";
							linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((totalEmpresaValorIngresso + totalEmpresaValorConveniencia)) + "<div>";
						}
						tabelaTotal.Rows.Add(linhaTotalEmpresa);

					}
				
				
				}
							
				//linha em branco
				DataRow linhaBranco = tabelaTotal.NewRow();
				tabelaTotal.Rows.Add(linhaBranco);

				//insere linha totais
				DataRow linhaTotal = tabelaTotal.NewRow();

				linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
				linhaTotal["Quantidade"] = somaQuantidadeIngressos.ToString(Utilitario.FormatoMoeda);
				linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(somaValorIngressos);
				//exibido apenas para perfil financeiro especial
				if(!semTaxas)
				{
					linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(somaValorConveniencia);
					linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
				}
				tabelaTotal.Rows.Add(linhaTotal);


			}
			catch(Exception erro) 
			{	
				throw erro;
			}
		} 
		public void LinhaEmBranco()
		{
			DataRow linhaBranco = tabelaTotal.NewRow();
			tabelaTotal.Rows.Add(linhaBranco);
		}

		#region NOVOS MÉTODOS
 
		#region Estruturas DataTable

		public DataTable EstruturaEntrega()
		{
			DataTable tabela = new DataTable("entrega");
			
			//Estrutura da tabela que receberá os dados do select
			tabela.Columns.Add("TaxaEntrega",typeof(string));
			tabela.Columns.Add("TaxaEntregaID",typeof(int));
			tabela.Columns.Add("TaxaEntregaValor",typeof(decimal));

			return tabela;
		}

		public DataTable EstruturaRetorno()
		{

			//DataTable de retorno
			DataTable tabela = new DataTable("Retorno");
			tabela.Columns.Add("Formas de Recebimento");
			tabela.Columns.Add("Quantidade");
			tabela.Columns.Add("R$ Ingressos");
			tabela.Columns.Add("R$ Conveniência");
            tabela.Columns.Add("R$ Comissão");
			tabela.Columns.Add("R$ Total");

			return tabela;
		}

		public DataTable EstruturaCalculo()
		{
			//DataTable para cálculos de forma de pagamentos
			DataTable tabela = new DataTable();
			tabela.Columns.Add("EmpresaID", typeof(int));
			tabela.Columns.Add("EmpresaNome", typeof(string));
			tabela.Columns.Add("LocalID", typeof(int));
			tabela.Columns.Add("LocalNome",typeof(string));
			tabela.Columns.Add("EventoID", typeof(int));
			tabela.Columns.Add("EventoNome",typeof(string));
			tabela.Columns.Add("CanalID", typeof(int));
			tabela.Columns.Add("CanalNome",typeof(string));
			tabela.Columns.Add("LojaID", typeof(int));
			tabela.Columns.Add("LojaNome",typeof(string));
			tabela.Columns.Add("PagamentoID", typeof(int));
			tabela.Columns.Add("PagamentoNome",typeof(string));
			tabela.Columns.Add("Total",typeof(decimal));
			tabela.Columns.Add("Valor",typeof(decimal));
            tabela.Columns.Add("Comissao", typeof(decimal));
			tabela.Columns.Add("Taxa",typeof(decimal));

			return tabela;

		}

		#endregion

		public string tabelaFinal()
		{
			string tabelaFinal = @"		
				CREATE TABLE #Final
				(
					EmpresaID INT,
					EmpresaNome NVARCHAR(100), 
					LocalID INT,
					LocalNome NVARCHAR(100), 
					EventoID INT,
					EventoNome NVARCHAR(100), 
					CanalID INT,
					CanalNome NVARCHAR(100),
					LojaID INT,
					LojaNome NVARCHAR(100), 
					PagamentoID INT,
					PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),
					Valor DECIMAL(12,2), 
					Taxa DECIMAL(12,2), 
					Comissao DECIMAL(12,2)
				) ";

			return tabelaFinal;
	
		}
		public string tabelaVendas()
		{
			string tabelaVendas = @" 
				CREATE TABLE #Vendas 
					(	
					VendaBilheteriaID INT, 
					TaxaConvenienciaValorTotal DECIMAL(12,2), 
					ValorTotal DECIMAL(12,2), 
                    ComissaoTotal DECIMAL(12,2), 
                    PagamentoID INT, 
					PagamentoNome NVARCHAR(50), 
					PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, 
					CanalID INT, 
					CanalNome NVARCHAR(50), 
					LojaID INT,
					LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) ";
			return tabelaVendas;
		}

		public DataSet EventosCanaisLojas(int empresaID,int localID,int eventoID,int canalID,int lojaID,int formaPagamentoID,bool comCortesias,bool semTaxas)
		{
            BD bd = new BD();

            try
            {                

                DataTable dtLoja = null;
                DataTable dtCanal = null;
                DataTable dtPagamentos = null;
                DataTable dtEmpresas = null;
                DataTable dtEventos = null;
                DataTable dtLocal = null;

                DataSet dsRetorno = new DataSet();

                DataTable entrega = EstruturaEntrega();

                DataTable retorno = EstruturaRetorno();

                DataTable calculo = EstruturaCalculo();



                //linhas
                DataRow linhaEntrega = null;
                DataRow linha = null;
                DataRow linhaLoja = null;
                DataRow linhaCanal = null;
                DataRow linhaEmpresa = null;
                DataRow linhaLocal = null;
                DataRow linhaEvento = null;

                //variaveis para calculos de totais
                //totais de loja
                decimal qtdTotalLoja = 0;
                decimal valorTotalLoja = 0;
                decimal convenienciaTotalLoja = 0;
                decimal comissaoTotalLoja = 0;
                decimal TotalLoja = 0;
                //totais de canal
                decimal qtdTotalCanal = 0;
                decimal valorTotalCanal = 0;
                decimal convenienciaTotalCanal = 0;
                decimal comissaoTotalCanal = 0;
                decimal TotalCanal = 0;
                //totais de evento
                decimal qtdTotalEvento = 0;
                decimal valorTotalEvento = 0;
                decimal convenienciaTotalEvento = 0;
                decimal comissaoTotalEvento = 0;
                decimal TotalEvento = 0;
                //totais do Local
                decimal qtdTotalLocal = 0;
                decimal valorTotalLocal = 0;
                decimal convenienciaTotalLocal = 0;
                decimal comissaoTotalLocal = 0;
                decimal TotalLocal = 0;
                //totais de empresa
                decimal qtdTotalEmpresa = 0;
                decimal valorTotalEmpresa = 0;
                decimal convenienciaTotalEmpresa = 0;
                decimal comissaoTotalEmpresa = 0;
                decimal TotalEmpresa = 0;
                //total geral
                decimal qtdTotal = 0;
                decimal valorTotal = 0;
                decimal convenienciaTotal = 0;
                decimal comissaoTotal = 0;
                decimal Total = 0;


                string sql = @"INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID 
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
                if (canalID > 0)
                    sql += " AND tCanal.ID = " + canalID;
                if (lojaID > 0)
                    sql += " AND tLoja.ID = " + lojaID;
                if (formaPagamentoID > 0)
                    sql += " AND tFormaPagamento.ID = " + formaPagamentoID;




                sql += @"
					DECLARE @Canais TABLE (ID INT IDENTITY(1,1), CanalID INT)
					DECLARE @n INT
					DECLARE @i INT 
					DECLARE @CanalID INT

					SET @i = 1
					INSERT INTO @Canais SELECT DISTINCT CanalID FROM #Vendas

					SELECT @n = COUNT(CanalID) FROM @Canais


					WHILE @i <= @n
						BEGIN
							SET @CanalID = (SELECT CanalID FROM @canais WHERE ID = @i)
							SET @i = @i + 1


							INSERT INTO #final
							SELECT 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
																					          CASE 
								WHEN ((PagamentoID IS NULL) OR (((SUM(tPreco.Valor) * PagamentoPct / 100) = 0)) AND 
								(CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											(SUM(TaxaConvenienciaValor) * PagamentoPct / 100)
										ELSE
											((TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
										END
							)=0 
								) THEN 0
                            ELSE
							PagamentoID END AS PagamentoID, 
							CASE 
										WHEN ((PagamentoID IS NULL) OR (((SUM(tPreco.Valor) * PagamentoPct / 100) = 0)) AND 
								(CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											(SUM(TaxaConvenienciaValor) * PagamentoPct / 100)
										ELSE
											((TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
										END
							)=0 
								) THEN 'Cortesia' 
								ELSE PagamentoNome
							END  AS PagamentoNome,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE 
										WHEN (PagamentoID IS NULL) THEN 
											SUM(1)
										ELSE 
											SUM(1 * PagamentoPCT / 100)
									END
								ELSE
									CASE 		
										WHEN (PagamentoID IS NULL) THEN SUM(-1)
										ELSE SUM(1 * PagamentoPCT / -100)
								END
							END
							AS Quantidade,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									SUM(tPreco.Valor) * PagamentoPct / 100
								ELSE
									SUM(tPreco.Valor) * PagamentoPct / -100 
								END
							AS Valor,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / -100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Taxa, 
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / 100
										ELSE
											(TaxaComissao * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / -100
										ELSE
											(TaxaComissao * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Comissao 
							FROM tIngressoLog
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID 
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";


                sql += @" GROUP BY 
							info.EmpresaID,info.EmpresaNome, info.LocalID,info.LocalNome, info.EventoID,EventoNome,  vendas.CanalID,CanalNome, vendas.LojaID,vendas.LojaNome,  
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					SELECT 
					EmpresaID,EmpresaNome, LocalID,LocalNome, EventoID,EventoNome, CanalID,CanalNome,LojaID, LojaNome,PagamentoID, PagamentoNome,
					SUM(Quantidade) AS Total, 
					SUM(Valor) AS Valor,	
					SUM(Taxa) AS Taxa, 
				    SUM(Comissao) AS Comissao 
					FROM #FINAL
					GROUP BY EmpresaID,EmpresaNome, LocalID,LocalNome, EventoID,EventoNome, CanalID,CanalNome,LojaID, LojaNome,PagamentoID, PagamentoNome
					ORDER BY EmpresaNome, LocalNome, EventoNome, CanalNome, LojaNome, PagamentoNome;
					
					DROP TABLE #FINAL
					
					SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
						CASE tvendabilheteria.status
							WHEN 'P' THEN 
								tvendabilheteria.taxaentregavalor 
							ELSE 
								tvendabilheteria.taxaentregavalor * (-1)
							END 
						As TaxaEntregaValor
						FROM tIngressoLog
						INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
						inner join tVendaBilheteria ON tVendaBilheteria.ID = vendas.VendaBilheteriaID
						INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressolog.IngressoID
						INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
						INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
						WHERE tIngressoLog.Acao IN ('C','V') and tVendaBilheteria.taxaEntregaValor > 0 ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";

                sql += @"GROUP BY tTaxaEntrega.id,tTaxaEntrega.Nome,tVendaBilheteria.ID,tVendaBilheteria.TaxaEntregaValor,tVendaBilheteria.Status
						ORDER BY TaxaEntrega 

						DROP TABLE #Vendas
					";

                bd.Consulta(tabelaVendas() + tabelaFinal() + sql);

                while (bd.Consulta().Read())
                {
                    linha = calculo.NewRow();

                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["LocalNome"] = bd.LerString("LocalNome");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["EventoNome"] = bd.LerString("EventoNome");
                    linha["CanalID"] = bd.LerInt("CanalID");
                    linha["CanalNome"] = bd.LerString("CanalNome");
                    linha["LojaID"] = bd.LerInt("LojaID");
                    linha["LojaNome"] = bd.LerString("LojaNome");
                    linha["PagamentoID"] = bd.LerInt("PagamentoID");
                    linha["PagamentoNome"] = bd.LerString("PagamentoNome");
                    linha["Total"] = bd.LerDecimal("Total");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Taxa"] = bd.LerDecimal("Taxa");
                    linha["Comissao"] = bd.LerDecimal("Comissao");

                    calculo.Rows.Add(linha);
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {

                    linhaEntrega = entrega.NewRow();

                    linhaEntrega["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaEntrega["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaEntrega["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    entrega.Rows.Add(linhaEntrega);
                }

                bd.Fechar();

                string[] colunasEmpresa = new string[2];

                colunasEmpresa[0] = "EmpresaID";
                colunasEmpresa[1] = "EmpresaNome";

                dtEmpresas = CTLib.TabelaMemoria.DistinctSort(calculo, "EmpresaNome", "1=1", colunasEmpresa);

                for (int i = 0; i <= dtEmpresas.Rows.Count - 1; i++)
                {
                    linhaEmpresa = dtEmpresas.Rows[i];

                    //linha com nome da empresa
                    DataRow linhaRetorno = retorno.NewRow();

                    linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";

                    retorno.Rows.Add(linhaRetorno);

                    string[] colunasLocal = new string[2];

                    colunasLocal[0] = "LocalID";
                    colunasLocal[1] = "LocalNome";

                    dtLocal = CTLib.TabelaMemoria.DistinctSort(calculo, "LocalNome", "EmpresaID = " + linhaEmpresa["EmpresaID"], colunasLocal);

                    for (int n = 0; n <= dtLocal.Rows.Count - 1; n++)
                    {
                        linhaLocal = dtLocal.Rows[n];

                        //linha com nome do Local
                        linhaRetorno = retorno.NewRow();

                        linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Local " + linhaLocal["LocalNome"] + "</div>";

                        retorno.Rows.Add(linhaRetorno);


                        string[] colunasEvento = new string[2];

                        colunasEvento[0] = "EventoID";
                        colunasEvento[1] = "EventoNome";

                        dtEventos = CTLib.TabelaMemoria.DistinctSort(calculo, "EventoNome", "LocalID = '" + linhaLocal["LocalID"] + "'", colunasEvento);


                        for (int j = 0; j <= dtEventos.Rows.Count - 1; j++)
                        {
                            linhaEvento = dtEventos.Rows[j];

                            //linha com o nome do evento
                            linhaRetorno = retorno.NewRow();

                            linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:75px'>Evento " + linhaEvento["EventoNome"] + "</div>";

                            retorno.Rows.Add(linhaRetorno);


                            string[] colunasCanal = new string[2];

                            colunasCanal[0] = "CanalID";
                            colunasCanal[1] = "CanalNome";

                            dtCanal = CTLib.TabelaMemoria.DistinctSort(calculo, "CanalNome", "EventoID = '" + linhaEvento["EventoID"] + "'", colunasCanal);

                            for (int y = 0; y <= dtCanal.Rows.Count - 1; y++)
                            {
                                linhaCanal = dtCanal.Rows[y];

                                linha = retorno.NewRow();

                                //linha com o nome do canal
                                linha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Canal " + linhaCanal["CanalNome"] + "</div>";

                                retorno.Rows.Add(linha);

                                string[] colunasLoja = new string[2];

                                colunasLoja[0] = "LojaID";
                                colunasLoja[1] = "LojaNome";

                                dtLoja = CTLib.TabelaMemoria.DistinctSort(calculo, "LojaNome", "EventoID = '" + linhaEvento["EventoID"] + "' AND CanalID = '" + linhaCanal["CanalID"] + "'", colunasLoja);

                                for (int z = 0; z <= dtLoja.Rows.Count - 1; z++)
                                {
                                    linhaLoja = dtLoja.Rows[z];

                                    linha = retorno.NewRow();

                                    //linha com o nome da loja
                                    linha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:135px'>Loja " + linhaLoja["LojaNome"] + "</div>";

                                    retorno.Rows.Add(linha);


                                    //variáveis para os calculos
                                    decimal qtdTotalIngresso = 0;

                                    string[] colunasPagamento = new string[5];

                                    colunasPagamento[0] = "PagamentoNome";
                                    colunasPagamento[1] = "Total";
                                    colunasPagamento[2] = "Valor";
                                    colunasPagamento[3] = "Taxa";
                                    colunasPagamento[4] = "Comissao";

                                    dtPagamentos = CTLib.TabelaMemoria.DistinctSort(calculo, "PagamentoNome", "EventoID = '" + linhaEvento["EventoID"] + "' AND CanalID = '" + linhaCanal["CanalID"] + "' AND LojaID = '" + linhaLoja["LojaID"] + "'", colunasPagamento);

                                    for (int k = 0; k <= dtPagamentos.Rows.Count - 1; k++)
                                    {
                                        DataRow linhaPagamentos = dtPagamentos.Rows[k];

                                        linha = retorno.NewRow();
                                        linha["Formas de Recebimento"] = linhaPagamentos["PagamentoNome"];

                                        qtdTotalIngresso = (decimal)linhaPagamentos["Total"];
                                        linha["Quantidade"] = qtdTotalIngresso.ToString(IRLib.Paralela.Utilitario.FormatoMoeda);
                                        linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]));
                                        linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Taxa"]));
                                        linha["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Comissao"]));
                                        linha["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));

                                        retorno.Rows.Add(linha);

                                        //totais da loja
                                        qtdTotalLoja = qtdTotalLoja + Convert.ToDecimal(linhaPagamentos["Total"]);
                                        valorTotalLoja = valorTotalLoja + Convert.ToDecimal(linhaPagamentos["Valor"]);
                                        convenienciaTotalLoja = convenienciaTotalLoja + Convert.ToDecimal(linhaPagamentos["Taxa"]);
                                        comissaoTotalLoja = comissaoTotalLoja + Convert.ToDecimal(linhaPagamentos["Comissao"]);
                                        TotalLoja = TotalLoja + (Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));


                                    }//Fim FormaPagamento

                                    //insere a linha total da loja
                                    DataRow linhaTotalLoja = retorno.NewRow();

                                    linhaTotalLoja["Formas de Recebimento"] = "<div style='text-align:left;margin-left:135px'>Total Loja " + linhaLoja["LojaNome"] + "</div>";
                                    linhaTotalLoja["Quantidade"] = Math.Round(qtdTotalLoja).ToString(Utilitario.FormatoMoeda);
                                    linhaTotalLoja["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalLoja);
                                    //exibido apenas para perfil financeiro especial
                                    linhaTotalLoja["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotalLoja);
                                    linhaTotalLoja["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotalLoja);
                                    linhaTotalLoja["R$ Total"] = Utilitario.AplicaFormatoMoeda((valorTotalLoja + convenienciaTotalLoja));
                                    retorno.Rows.Add(linhaTotalLoja);

                                    //totais do Canal
                                    qtdTotalCanal = qtdTotalCanal + qtdTotalLoja;
                                    valorTotalCanal = valorTotalCanal + valorTotalLoja;
                                    convenienciaTotalCanal = convenienciaTotalCanal + convenienciaTotalLoja;
                                    comissaoTotalCanal = comissaoTotalCanal + comissaoTotalLoja;
                                    TotalCanal = TotalCanal + (valorTotalCanal + convenienciaTotalCanal);

                                    //zera os totais de loja
                                    qtdTotalLoja = 0;
                                    valorTotalLoja = 0;
                                    convenienciaTotalLoja = 0;
                                    comissaoTotalLoja = 0;
                                    TotalLoja = 0;


                                }//Fim Loja

                                //insere a linha total do canal
                                DataRow linhaTotalCanal = retorno.NewRow();

                                linhaTotalCanal["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Total Canal " + linhaCanal["CanalNome"] + "</div>";
                                linhaTotalCanal["Quantidade"] = Math.Round(qtdTotalCanal).ToString(Utilitario.FormatoMoeda);
                                linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalCanal);
                                //exibido apenas para perfil financeiro especial
                                linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotalCanal);
                                linhaTotalCanal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotalLocal);
                                linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((valorTotalCanal + convenienciaTotalCanal));
                                retorno.Rows.Add(linhaTotalCanal);

                                //totais dos Eventos
                                qtdTotalEvento = qtdTotalEvento + qtdTotalCanal;
                                valorTotalEvento = valorTotalEvento + valorTotalCanal;
                                convenienciaTotalEvento = convenienciaTotalEvento + convenienciaTotalCanal;
                                comissaoTotalEvento = comissaoTotalEvento + comissaoTotalCanal;
                                TotalEvento = TotalEvento + (valorTotalEvento + convenienciaTotalEvento);

                                //zera os totais de Canal
                                qtdTotalCanal = 0;
                                valorTotalCanal = 0;
                                convenienciaTotalCanal = 0;
                                comissaoTotalCanal = 0;
                                TotalCanal = 0;


                            }//Fim Canal

                            //insere a linha total do Evento
                            DataRow linhaTotalEvento = retorno.NewRow();

                            linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:75px'>Total Evento " + linhaEvento["EventoNome"] + "</div>";
                            linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalEvento).ToString(Utilitario.FormatoMoeda) + "</div>";
                            linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalEvento) + "</div>";
                            //exibido apenas para perfil financeiro especial
                            linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEvento) + "</div>";
                            linhaTotalEvento["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEvento) + "</div>";
                            linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalEvento + convenienciaTotalEvento)) + "<div>";
                            retorno.Rows.Add(linhaTotalEvento);

                            //totais dos Locais
                            qtdTotalLocal = qtdTotalLocal + qtdTotalEvento;
                            valorTotalLocal = valorTotalLocal + valorTotalEvento;
                            convenienciaTotalLocal = convenienciaTotalLocal + convenienciaTotalEvento;
                            comissaoTotalLocal = comissaoTotalLocal + comissaoTotalEvento;
                            TotalLocal = TotalLocal + (valorTotalLocal + convenienciaTotalLocal);

                            //zera os totais de Eventos
                            qtdTotalEvento = 0;
                            valorTotalEvento = 0;
                            convenienciaTotalEvento = 0;
                            comissaoTotalEvento = 0;
                            TotalEvento = 0;

                        }//Fim Evento

                        //insere a linha total do Local
                        DataRow linhaTotalLocal = retorno.NewRow();

                        linhaTotalLocal["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Local " + linhaLocal["LocalNome"] + "</div>";
                        linhaTotalLocal["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalLocal).ToString(Utilitario.FormatoMoeda) + "</div>";
                        linhaTotalLocal["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalLocal) + "</div>";
                        //exibido apenas para perfil financeiro especial
                        linhaTotalLocal["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalLocal + convenienciaTotalLocal)) + "<div>";
                        retorno.Rows.Add(linhaTotalLocal);

                        //totais das Empresas 
                        qtdTotalEmpresa = qtdTotalEmpresa + qtdTotalLocal;
                        valorTotalEmpresa = valorTotalEmpresa + valorTotalLocal;
                        convenienciaTotalEmpresa = convenienciaTotalEmpresa + convenienciaTotalLocal;
                        comissaoTotalEmpresa = comissaoTotalEmpresa + comissaoTotalLocal;
                        TotalEmpresa = TotalEmpresa + (valorTotalEmpresa + convenienciaTotalEmpresa);

                        //zera os totais do Local
                        qtdTotalLocal = 0;
                        valorTotalLocal = 0;
                        convenienciaTotalLocal = 0;
                        comissaoTotalLocal = 0;
                        TotalLocal = 0;


                    }//Fim Local

                    //insere a linha total do Empresa
                    DataRow linhaTotalEmpresa = retorno.NewRow();

                    linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
                    linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Math.Round(qtdTotalEmpresa).ToString(Utilitario.FormatoMoeda) + "</div>";
                    linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(valorTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((valorTotalEmpresa + convenienciaTotalEmpresa)) + "<div>";
                    retorno.Rows.Add(linhaTotalEmpresa);

                    //total geral
                    qtdTotal = qtdTotal + qtdTotalEmpresa;
                    valorTotal = valorTotal + valorTotalEmpresa;
                    convenienciaTotal = convenienciaTotal + convenienciaTotalEmpresa;
                    comissaoTotal = comissaoTotal + comissaoTotalEmpresa;
                    Total = Total + (valorTotal + convenienciaTotal);

                    //zera os totais de Empresas
                    qtdTotalEmpresa = 0;
                    valorTotalEmpresa = 0;
                    convenienciaTotalEmpresa = 0;
                    comissaoTotalEmpresa = 0;
                    TotalEmpresa = 0;

                }//Fim Empresa

                //linha em branco
                DataRow linhaBranco = retorno.NewRow();
                retorno.Rows.Add(linhaBranco);

                //insere linha totais
                DataRow linhaTotal = retorno.NewRow();

                linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
                linhaTotal["Quantidade"] = Math.Round(qtdTotal).ToString(Utilitario.FormatoMoeda);
                linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotal);
                //exibido apenas para perfil financeiro especial
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotal);
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotal);
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
                retorno.Rows.Add(linhaTotal);

                //esconde as taxas para os perfis de empresa que promove
                if (semTaxas)
                {
                    retorno.Columns.Remove("R$ Conveniência");
                    retorno.Columns.Remove("R$ Comissão");
                    retorno.Columns.Remove("R$ Total");
                }

                dsRetorno.Tables.Add(entrega);
                dsRetorno.Tables.Add(retorno);


                return dsRetorno;
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
			
		public DataSet EventosCanais(int empresaID, int localID,int eventoID,int canalID,int formaPagamentoID,bool comCortesias,bool semTaxas)
		{
            BD bd = new BD();

            try
            {

                DataTable dtCanal = null;
                DataTable dtPagamentos = null;
                DataTable dtEmpresas = null;
                DataTable dtEventos = null;
                DataTable dtLocal = null;

                DataSet dsRetorno = new DataSet();

                DataTable entrega = EstruturaEntrega();

                DataTable retorno = EstruturaRetorno();

                DataTable calculo = EstruturaCalculo();

                //linhas
                DataRow linhaEntrega = null;
                DataRow linha = null;
                DataRow linhaCanal = null;
                DataRow linhaEmpresa = null;
                DataRow linhaLocal = null;
                DataRow linhaEvento = null;

                //variaveis para calculos de totais
                //totais de canal
                decimal qtdTotalCanal = 0;
                decimal valorTotalCanal = 0;
                decimal convenienciaTotalCanal = 0;
                decimal comissaoTotalCanal = 0;
                decimal TotalCanal = 0;
                //totais de evento
                decimal qtdTotalEvento = 0;
                decimal valorTotalEvento = 0;
                decimal convenienciaTotalEvento = 0;
                decimal comissaoTotalEvento = 0;
                decimal TotalEvento = 0;
                //totais do Local
                decimal qtdTotalLocal = 0;
                decimal valorTotalLocal = 0;
                decimal convenienciaTotalLocal = 0;
                decimal comissaoTotalLocal = 0;
                decimal TotalLocal = 0;
                //totais de empresa
                decimal qtdTotalEmpresa = 0;
                decimal valorTotalEmpresa = 0;
                decimal convenienciaTotalEmpresa = 0;
                decimal comissaoTotalEmpresa = 0;
                decimal TotalEmpresa = 0;
                //total geral
                decimal qtdTotal = 0;
                decimal valorTotal = 0;
                decimal convenienciaTotal = 0;
                decimal comissaoTotal = 0;
                decimal Total = 0;



                string sql = @"INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
                if (canalID > 0)
                    sql += " AND tCanal.ID = " + canalID;
                if (formaPagamentoID > 0)
                    sql += " AND tFormaPagamento.ID = " + formaPagamentoID;


                sql += @"
					DECLARE @Canais TABLE (ID INT IDENTITY(1,1), CanalID INT)
					DECLARE @n INT
					DECLARE @i INT 
					DECLARE @CanalID INT

					SET @i = 1
					INSERT INTO @Canais SELECT DISTINCT CanalID FROM #Vendas

					SELECT @n = COUNT(CanalID) FROM @Canais


					WHILE @i <= @n
						BEGIN
							SET @CanalID = (SELECT CanalID FROM @canais WHERE ID = @i)
							SET @i = @i + 1


							INSERT INTO #final
							SELECT 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
																											          CASE 
								WHEN ((PagamentoID IS NULL) OR (((SUM(tPreco.Valor) * PagamentoPct / 100) = 0)) AND 
								(CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											(SUM(TaxaConvenienciaValor) * PagamentoPct / 100)
										ELSE
											((TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
										END
							)=0 
								) THEN 0
                            ELSE
							PagamentoID END AS PagamentoID, 
							CASE 
										WHEN ((PagamentoID IS NULL) OR (((SUM(tPreco.Valor) * PagamentoPct / 100) = 0)) AND 
								(CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											(SUM(TaxaConvenienciaValor) * PagamentoPct / 100)
										ELSE
											((TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
										END
							)=0 
								) THEN 'Cortesia' 
								ELSE PagamentoNome
							END  AS PagamentoNome,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE 
										WHEN (PagamentoID IS NULL) THEN 
											SUM(1)
										ELSE 
											SUM(1 * PagamentoPCT / 100)
									END
								ELSE
									CASE 		
										WHEN (PagamentoID IS NULL) THEN SUM(-1)
										ELSE SUM(1 * PagamentoPCT / -100)
								END
							END
							AS Quantidade,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									SUM(tPreco.Valor) * PagamentoPct / 100
								ELSE
									SUM(tPreco.Valor) * PagamentoPct / -100 
								END
							AS Valor,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / -100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Taxa, 
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / 100
										ELSE
											(TaxaComissao * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / -100
										ELSE
											(TaxaComissao * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Comissao 
							FROM tIngressoLog
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";


                sql += @" GROUP BY 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					SELECT 
					EmpresaID,EmpresaNome, LocalID,LocalNome, EventoID,EventoNome, CanalID,CanalNome,PagamentoID, PagamentoNome,
					SUM(Quantidade) AS Total, 
					SUM(Valor) AS Valor,
					SUM(Taxa) AS Taxa, 
				    SUM(Comissao) AS Comissao 
					FROM #FINAL
					GROUP BY EmpresaID,EmpresaNome, LocalID,LocalNome, EventoID,EventoNome, CanalID,CanalNome,PagamentoID, PagamentoNome
					ORDER BY EmpresaNome, LocalNome, EventoNome, CanalNome, PagamentoNome;

					DROP TABLE #Final

					SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
						CASE tvendabilheteria.status
							WHEN 'P' THEN 
								tvendabilheteria.taxaentregavalor 
							ELSE 
								tvendabilheteria.taxaentregavalor * (-1)
							END 
						As TaxaEntregaValor
						FROM tIngressoLog
						INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
						inner join tVendaBilheteria ON tVendaBilheteria.ID = vendas.VendaBilheteriaID
						INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressolog.IngressoID
						INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
						INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
						WHERE tIngressoLog.Acao IN ('C','V') and tVendaBilheteria.taxaEntregaValor > 0 ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";

                sql += @"GROUP BY tTaxaEntrega.id,tTaxaEntrega.Nome,tVendaBilheteria.ID,tVendaBilheteria.TaxaEntregaValor,tVendaBilheteria.Status
								ORDER BY TaxaEntrega 

						DROP TABLE #Vendas
					";

                bd.Consulta(tabelaFinal() + tabelaVendas() + sql);

                while (bd.Consulta().Read())
                {
                    linha = calculo.NewRow();

                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["LocalNome"] = bd.LerString("LocalNome");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["EventoNome"] = bd.LerString("EventoNome");
                    linha["CanalID"] = bd.LerInt("CanalID");
                    linha["CanalNome"] = bd.LerString("CanalNome");
                    linha["PagamentoID"] = bd.LerInt("PagamentoID");
                    linha["PagamentoNome"] = bd.LerString("PagamentoNome");
                    linha["Total"] = bd.LerDecimal("Total");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Taxa"] = bd.LerDecimal("Taxa");
                    linha["Comissao"] = bd.LerDecimal("Comissao");

                    calculo.Rows.Add(linha);

                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {

                    linhaEntrega = entrega.NewRow();

                    linhaEntrega["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaEntrega["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaEntrega["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    entrega.Rows.Add(linhaEntrega);
                }

                bd.Fechar();
                string[] colunasEmpresa = new string[2];

                colunasEmpresa[0] = "EmpresaID";
                colunasEmpresa[1] = "EmpresaNome";

                dtEmpresas = CTLib.TabelaMemoria.DistinctSort(calculo, "EmpresaNome", "1=1", colunasEmpresa);

                for (int i = 0; i <= dtEmpresas.Rows.Count - 1; i++)
                {
                    linhaEmpresa = dtEmpresas.Rows[i];

                    //linha com nome da empresa
                    DataRow linhaRetorno = retorno.NewRow();

                    linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";

                    retorno.Rows.Add(linhaRetorno);

                    string[] colunasLocal = new string[2];

                    colunasLocal[0] = "LocalID";
                    colunasLocal[1] = "LocalNome";

                    dtLocal = CTLib.TabelaMemoria.DistinctSort(calculo, "LocalNome", "EmpresaID = '" + linhaEmpresa["EmpresaID"] + "'", colunasLocal);

                    for (int n = 0; n <= dtLocal.Rows.Count - 1; n++)
                    {
                        linhaLocal = dtLocal.Rows[n];

                        //linha com nome do Local
                        linhaRetorno = retorno.NewRow();

                        linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Local " + linhaLocal["LocalNome"] + "</div>";

                        retorno.Rows.Add(linhaRetorno);

                        string[] colunasEvento = new string[2];

                        colunasEvento[0] = "EventoID";
                        colunasEvento[1] = "EventoNome";


                        dtEventos = CTLib.TabelaMemoria.DistinctSort(calculo, "EventoNome", "LocalID = '" + linhaLocal["LocalID"] + "'", colunasEvento);


                        for (int j = 0; j <= dtEventos.Rows.Count - 1; j++)
                        {
                            linhaEvento = dtEventos.Rows[j];

                            //linha com o nome do evento
                            linhaRetorno = retorno.NewRow();

                            linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:75px'>Evento " + linhaEvento["EventoNome"] + "</div>";

                            retorno.Rows.Add(linhaRetorno);

                            string[] colunasCanal = new string[2];

                            colunasCanal[0] = "CanalID";
                            colunasCanal[1] = "CanalNome";

                            dtCanal = CTLib.TabelaMemoria.DistinctSort(calculo, "CanalNome", "EventoID = '" + linhaEvento["EventoID"] + "'", colunasCanal);

                            for (int y = 0; y <= dtCanal.Rows.Count - 1; y++)
                            {
                                linhaCanal = dtCanal.Rows[y];

                                linha = retorno.NewRow();

                                //linha com o nome do canal
                                linha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Canal " + linhaCanal["CanalNome"] + "</div>";

                                retorno.Rows.Add(linha);


                                //variáveis para os calculos
                                decimal qtdTotalIngresso = 0;

                                string[] colunasPagamento = new string[5];

                                colunasPagamento[0] = "PagamentoNome";
                                colunasPagamento[1] = "Total";
                                colunasPagamento[2] = "Valor";
                                colunasPagamento[3] = "Taxa";
                                colunasPagamento[4] = "Comissao";

                                dtPagamentos = CTLib.TabelaMemoria.DistinctSort(calculo, "PagamentoNome", "EventoID = '" + linhaEvento["EventoID"] + "' AND CanalID = '" + linhaCanal["CanalID"] + "'", colunasPagamento);

                                for (int k = 0; k <= dtPagamentos.Rows.Count - 1; k++)
                                {
                                    DataRow linhaPagamentos = dtPagamentos.Rows[k];

                                    linha = retorno.NewRow();
                                    linha["Formas de Recebimento"] = linhaPagamentos["PagamentoNome"];

                                    qtdTotalIngresso = (decimal)linhaPagamentos["Total"];
                                    linha["Quantidade"] = qtdTotalIngresso.ToString(IRLib.Paralela.Utilitario.FormatoMoeda);
                                    linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]));
                                    linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Taxa"]));
                                    linha["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Comissao"]));
                                    linha["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));

                                    retorno.Rows.Add(linha);

                                    //totais do Canal
                                    qtdTotalCanal = qtdTotalCanal + Convert.ToDecimal(linhaPagamentos["Total"]);
                                    valorTotalCanal = valorTotalCanal + Convert.ToDecimal(linhaPagamentos["Valor"]);
                                    convenienciaTotalCanal = convenienciaTotalCanal + Convert.ToDecimal(linhaPagamentos["Taxa"]);
                                    comissaoTotalCanal = comissaoTotalCanal + Convert.ToDecimal(linhaPagamentos["Comissao"]);
                                    TotalCanal = TotalCanal + (Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));


                                }//Fim FormaPagamento

                                //insere a linha total do canal
                                DataRow linhaTotalCanal = retorno.NewRow();

                                linhaTotalCanal["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Total Canal " + linhaCanal["CanalNome"] + "</div>";
                                linhaTotalCanal["Quantidade"] = Math.Round(qtdTotalCanal, 1).ToString(Utilitario.FormatoMoeda);
                                linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalCanal);
                                //exibido apenas para perfil financeiro especial
                                linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotalCanal);
                                linhaTotalCanal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotalCanal);
                                linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((valorTotalCanal + convenienciaTotalCanal));
                                retorno.Rows.Add(linhaTotalCanal);

                                //totais dos Eventos
                                qtdTotalEvento = qtdTotalEvento + qtdTotalCanal;
                                valorTotalEvento = valorTotalEvento + valorTotalCanal;
                                convenienciaTotalEvento = convenienciaTotalEvento + convenienciaTotalCanal;
                                comissaoTotalEvento = comissaoTotalEvento + comissaoTotalCanal;
                                TotalEvento = TotalEvento + (valorTotalEvento + convenienciaTotalEvento);

                                //zera os totais de Canal
                                qtdTotalCanal = 0;
                                valorTotalCanal = 0;
                                convenienciaTotalCanal = 0;
                                comissaoTotalCanal = 0;
                                TotalCanal = 0;


                            }//Fim Canal

                            //insere a linha total do Evento
                            DataRow linhaTotalEvento = retorno.NewRow();

                            linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:75px'>Total Evento " + linhaEvento["EventoNome"] + "</div>";
                            linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalEvento, 1).ToString(Utilitario.FormatoMoeda) + "</div>";
                            linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalEvento) + "</div>";
                            //exibido apenas para perfil financeiro especial
                            linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEvento) + "</div>";
                            linhaTotalEvento["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEvento) + "</div>";
                            linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalEvento + convenienciaTotalEvento)) + "<div>";
                            retorno.Rows.Add(linhaTotalEvento);

                            //totais dos Locais
                            qtdTotalLocal = qtdTotalLocal + qtdTotalEvento;
                            valorTotalLocal = valorTotalLocal + valorTotalEvento;
                            convenienciaTotalLocal = convenienciaTotalLocal + convenienciaTotalEvento;
                            comissaoTotalLocal = comissaoTotalLocal + comissaoTotalEvento;
                            TotalLocal = TotalLocal + (valorTotalLocal + convenienciaTotalLocal);

                            //zera os totais de Eventos
                            qtdTotalEvento = 0;
                            valorTotalEvento = 0;
                            convenienciaTotalEvento = 0;
                            comissaoTotalEvento = 0;
                            TotalEvento = 0;

                        }//Fim Evento

                        //insere a linha total do Local
                        DataRow linhaTotalLocal = retorno.NewRow();

                        linhaTotalLocal["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Local " + linhaLocal["LocalNome"] + "</div>";
                        linhaTotalLocal["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalLocal, 1).ToString(Utilitario.FormatoMoeda) + "</div>";
                        linhaTotalLocal["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalLocal) + "</div>";
                        //exibido apenas para perfil financeiro especial
                        linhaTotalLocal["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalLocal + convenienciaTotalLocal)) + "<div>";
                        retorno.Rows.Add(linhaTotalLocal);

                        //totais das Empresas
                        qtdTotalEmpresa = qtdTotalEmpresa + qtdTotalLocal;
                        valorTotalEmpresa = valorTotalEmpresa + valorTotalLocal;
                        convenienciaTotalEmpresa = convenienciaTotalEmpresa + convenienciaTotalLocal;
                        comissaoTotalEmpresa = comissaoTotalEmpresa + comissaoTotalLocal;
                        TotalEmpresa = TotalEmpresa + (valorTotalEmpresa + convenienciaTotalEmpresa);

                        //zera os totais do Local
                        qtdTotalLocal = 0;
                        valorTotalLocal = 0;
                        convenienciaTotalLocal = 0;
                        comissaoTotalLocal = 0;
                        TotalLocal = 0;


                    }//Fim Local

                    //insere a linha total do Empresa
                    DataRow linhaTotalEmpresa = retorno.NewRow();

                    linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
                    linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Math.Round(qtdTotalEmpresa, 1).ToString(Utilitario.FormatoMoeda) + "</div>";
                    linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(valorTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((valorTotalEmpresa + convenienciaTotalEmpresa)) + "<div>";
                    retorno.Rows.Add(linhaTotalEmpresa);

                    //total geral
                    qtdTotal = qtdTotal + qtdTotalEmpresa;
                    valorTotal = valorTotal + valorTotalEmpresa;
                    convenienciaTotal = convenienciaTotal + convenienciaTotalEmpresa;
                    comissaoTotal = comissaoTotal + comissaoTotalEmpresa;
                    Total = Total + (valorTotal + convenienciaTotal);

                    //zera os totais de Empresas
                    qtdTotalEmpresa = 0;
                    valorTotalEmpresa = 0;
                    convenienciaTotalEmpresa = 0;
                    comissaoTotalEmpresa = 0;
                    TotalEmpresa = 0;

                }//Fim Empresa

                //linha em branco
                DataRow linhaBranco = retorno.NewRow();
                retorno.Rows.Add(linhaBranco);

                //insere linha totais
                DataRow linhaTotal = retorno.NewRow();

                linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
                linhaTotal["Quantidade"] = Math.Round(qtdTotal).ToString(Utilitario.FormatoMoeda);
                linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotal);
                //exibido apenas para perfil financeiro especial
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotal);
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotal);
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
                retorno.Rows.Add(linhaTotal);

                //esconde as taxas para os perfis de empresa que promove
                if (semTaxas)
                {
                    retorno.Columns.Remove("R$ Conveniência");
                    retorno.Columns.Remove("R$ Comissão");
                    retorno.Columns.Remove("R$ Total");
                }

                dsRetorno.Tables.Add(entrega);
                dsRetorno.Tables.Add(retorno);

                return dsRetorno;

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

		public DataSet Eventos(int empresaID, int localID,int eventoID,int formaPagamentoID,bool comCortesias,bool semTaxas)
		{
            BD bd = new BD();

            try
            {

                DataTable dtPagamentos = null;
                DataTable dtEmpresas = null;
                DataTable dtEventos = null;
                DataTable dtLocal = null;

                DataSet dsRetorno = new DataSet();

                DataTable entrega = EstruturaEntrega();

                DataTable retorno = EstruturaRetorno();

                DataTable calculo = EstruturaCalculo();

                //linhas
                DataRow linhaEntrega = null;
                DataRow linha = null;
                DataRow linhaEmpresa = null;
                DataRow linhaLocal = null;
                DataRow linhaEvento = null;

                //variaveis para calculos de totais
                //totais de evento
                decimal qtdTotalEvento = 0;
                decimal valorTotalEvento = 0;
                decimal convenienciaTotalEvento = 0;
                decimal comissaoTotalEvento = 0;
                decimal TotalEvento = 0;
                //totais do Local
                decimal qtdTotalLocal = 0;
                decimal valorTotalLocal = 0;
                decimal convenienciaTotalLocal = 0;
                decimal comissaoTotalLocal = 0;
                decimal TotalLocal = 0;
                //totais de empresa
                decimal qtdTotalEmpresa = 0;
                decimal valorTotalEmpresa = 0;
                decimal convenienciaTotalEmpresa = 0;
                decimal comissaoTotalEmpresa = 0;
                decimal TotalEmpresa = 0;
                //total geral
                decimal qtdTotal = 0;
                decimal valorTotal = 0;
                decimal convenienciaTotal = 0;
                decimal comissaoTotal = 0;
                decimal Total = 0;


                string sql = @"INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
                if (formaPagamentoID > 0)
                    sql += " AND tFormaPagamento.ID = " + formaPagamentoID;


                sql += @"
					DECLARE @Canais TABLE (ID INT IDENTITY(1,1), CanalID INT)
					DECLARE @n INT
					DECLARE @i INT 
					DECLARE @CanalID INT

					SET @i = 1
					INSERT INTO @Canais SELECT DISTINCT CanalID FROM #Vendas

					SELECT @n = COUNT(CanalID) FROM @Canais


					WHILE @i <= @n
						BEGIN
							SET @CanalID = (SELECT CanalID FROM @canais WHERE ID = @i)
							SET @i = @i + 1


							INSERT INTO #final
							SELECT 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							CASE 
								WHEN ((PagamentoID IS NULL) OR (((SUM(tPreco.Valor) * PagamentoPct / 100) = 0)) AND 
								(CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											(SUM(TaxaConvenienciaValor) * PagamentoPct / 100)
										ELSE
											((TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
										END
							)=0 
								) THEN 0
                            ELSE
							PagamentoID END AS PagamentoID, 
								CASE 
										WHEN ((PagamentoID IS NULL) OR (((SUM(tPreco.Valor) * PagamentoPct / 100) = 0)) AND 
								(CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											(SUM(TaxaConvenienciaValor) * PagamentoPct / 100)
										ELSE
											((TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
										END
							)=0 
								) THEN 'Cortesia' 
								ELSE PagamentoNome
							END  AS PagamentoNome,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE 
										WHEN (PagamentoID IS NULL) THEN 
											SUM(1)
										ELSE 
											SUM(1 * PagamentoPCT / 100)
									END
								ELSE
									CASE 		
										WHEN (PagamentoID IS NULL) THEN SUM(-1)
										ELSE SUM(1 * PagamentoPCT / -100)
								END
							END
							AS Quantidade,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									SUM(tPreco.Valor) * PagamentoPct / 100
								ELSE
									SUM(tPreco.Valor) * PagamentoPct / -100 
								END
							AS Valor,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / -100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Taxa, 
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / 100
										ELSE
											(TaxaComissao * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / -100
										ELSE
											(TaxaComissao * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Comissao  
							FROM tIngressoLog
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";

                sql += @" GROUP BY 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					SELECT 
					EmpresaID,EmpresaNome,LocalID ,LocalNome,EventoID, EventoNome,PagamentoID, PagamentoNome,
					SUM(Quantidade) AS Total, 
					SUM(Valor) AS Valor,
					SUM(Taxa) AS Taxa, 
					SUM(Comissao) AS Comissao 
					FROM #FINAL
					GROUP BY EmpresaID,EmpresaNome,LocalID ,LocalNome,EventoID, EventoNome,PagamentoID, PagamentoNome
					ORDER BY EmpresaNome, LocalNome, EventoNome, PagamentoNome;

					DROP TABLE #Final

					
					SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
						CASE tvendabilheteria.status
							WHEN 'P' THEN 
								tvendabilheteria.taxaentregavalor 
							ELSE 
								tvendabilheteria.taxaentregavalor * (-1)
							END 
						As TaxaEntregaValor
						FROM tIngressoLog
						INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
						inner join tVendaBilheteria ON tVendaBilheteria.ID = vendas.VendaBilheteriaID
						INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressolog.IngressoID
						INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
						INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
						WHERE tIngressoLog.Acao IN ('C','V') and tVendaBilheteria.taxaEntregaValor > 0 ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";

                sql += @"GROUP BY tTaxaEntrega.id,tTaxaEntrega.Nome,tVendaBilheteria.ID,tVendaBilheteria.TaxaEntregaValor,tVendaBilheteria.Status
								ORDER BY TaxaEntrega 
						DROP TABLE #Vendas
					";

                bd.Consulta(tabelaFinal() + tabelaVendas() + sql);

                while (bd.Consulta().Read())
                {
                    linha = calculo.NewRow();

                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["LocalNome"] = bd.LerString("LocalNome");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["EventoNome"] = bd.LerString("EventoNome");
                    linha["PagamentoID"] = bd.LerInt("PagamentoID");
                    linha["PagamentoNome"] = bd.LerString("PagamentoNome");
                    linha["Total"] = bd.LerDecimal("Total");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Taxa"] = bd.LerDecimal("Taxa");
                    linha["Comissao"] = bd.LerDecimal("Comissao");

                    calculo.Rows.Add(linha);

                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {

                    linhaEntrega = entrega.NewRow();

                    linhaEntrega["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaEntrega["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaEntrega["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    entrega.Rows.Add(linhaEntrega);
                }

                bd.Fechar();

                string[] colunasEmpresa = new string[2];

                colunasEmpresa[0] = "EmpresaID";
                colunasEmpresa[1] = "EmpresaNome";

                dtEmpresas = CTLib.TabelaMemoria.DistinctSort(calculo, "EmpresaNome", "1=1", colunasEmpresa);

                for (int i = 0; i <= dtEmpresas.Rows.Count - 1; i++)
                {
                    linhaEmpresa = dtEmpresas.Rows[i];

                    //linha com nome da empresa
                    DataRow linhaRetorno = retorno.NewRow();

                    linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";

                    retorno.Rows.Add(linhaRetorno);

                    string[] colunasLocal = new string[2];

                    colunasLocal[0] = "LocalID";
                    colunasLocal[1] = "LocalNome";


                    dtLocal = CTLib.TabelaMemoria.DistinctSort(calculo, "LocalNome", "EmpresaID = '" + linhaEmpresa["EmpresaID"] + "'", colunasLocal);

                    for (int n = 0; n <= dtLocal.Rows.Count - 1; n++)
                    {
                        linhaLocal = dtLocal.Rows[n];

                        //linha com nome do Local
                        linhaRetorno = retorno.NewRow();

                        linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Local " + linhaLocal["LocalNome"] + "</div>";

                        retorno.Rows.Add(linhaRetorno);

                        string[] colunasEventos = new string[2];

                        colunasEventos[0] = "EventoID";
                        colunasEventos[1] = "EventoNome";


                        dtEventos = CTLib.TabelaMemoria.DistinctSort(calculo, "EventoNome", "LocalID = '" + linhaLocal["LocalID"] + "'", colunasEventos);


                        for (int j = 0; j <= dtEventos.Rows.Count - 1; j++)
                        {
                            linhaEvento = dtEventos.Rows[j];

                            //linha com o nome do evento
                            linhaRetorno = retorno.NewRow();

                            linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:75px'>Evento " + linhaEvento["EventoNome"] + "</div>";

                            retorno.Rows.Add(linhaRetorno);

                            //dtPagamentos = CTLib.TabelaMemoria.DistinctSort(calculo,"PagamentoNome","EventoNome = '" + linhaEvento["EventoNome"] + "'","PagamentoNome");


                            //variáveis para os calculos
                            decimal qtdTotalIngresso = 0;

                            string[] colunasPagamento = new string[5];

                            colunasPagamento[0] = "PagamentoNome";
                            colunasPagamento[1] = "Total";
                            colunasPagamento[2] = "Valor";
                            colunasPagamento[3] = "Taxa";
                            colunasPagamento[4] = "Comissao";

                            dtPagamentos = CTLib.TabelaMemoria.DistinctSort(calculo, "PagamentoNome", "EventoID = '" + linhaEvento["EventoID"] + "'", colunasPagamento);

                            for (int k = 0; k <= dtPagamentos.Rows.Count - 1; k++)
                            {
                                DataRow linhaPagamentos = dtPagamentos.Rows[k];

                                linha = retorno.NewRow();
                                linha["Formas de Recebimento"] = linhaPagamentos["PagamentoNome"];

                                qtdTotalIngresso = (decimal)linhaPagamentos["Total"];
                                linha["Quantidade"] = Math.Round(qtdTotalIngresso, 1).ToString(IRLib.Paralela.Utilitario.FormatoMoeda);
                                linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]));
                                linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Taxa"]));
                                linha["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Comissao"]));
                                linha["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));

                                retorno.Rows.Add(linha);

                                //totais dos Eventos
                                qtdTotalEvento = qtdTotalEvento + Convert.ToDecimal(linhaPagamentos["Total"]);
                                valorTotalEvento = valorTotalEvento + Convert.ToDecimal(linhaPagamentos["Valor"]);
                                convenienciaTotalEvento = convenienciaTotalEvento + Convert.ToDecimal(linhaPagamentos["Taxa"]);
                                comissaoTotalEvento = comissaoTotalEvento + Convert.ToDecimal(linhaPagamentos["Comissao"]);
                                TotalEvento = TotalEvento + (Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));

                            }//Fim FormaPagamento

                            //insere a linha total do Evento
                            DataRow linhaTotalEvento = retorno.NewRow();

                            linhaTotalEvento["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:75px'>Total Evento " + linhaEvento["EventoNome"] + "</div>";
                            linhaTotalEvento["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalEvento, 1).ToString(Utilitario.FormatoMoeda) + "</div>";
                            linhaTotalEvento["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalEvento) + "</div>";
                            //exibido apenas para perfil financeiro especial
                            linhaTotalEvento["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEvento) + "</div>";
                            linhaTotalEvento["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEvento) + "</div>";
                            linhaTotalEvento["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalEvento + convenienciaTotalEvento)) + "<div>";
                            retorno.Rows.Add(linhaTotalEvento);

                            //totais dos Locais
                            qtdTotalLocal = qtdTotalLocal + qtdTotalEvento;
                            valorTotalLocal = valorTotalLocal + valorTotalEvento;
                            convenienciaTotalLocal = convenienciaTotalLocal + convenienciaTotalEvento;
                            comissaoTotalLocal = comissaoTotalLocal + comissaoTotalEvento;
                            TotalLocal = TotalLocal + (valorTotalLocal + convenienciaTotalLocal);

                            //zera os totais de Eventos
                            qtdTotalEvento = 0;
                            valorTotalEvento = 0;
                            convenienciaTotalEvento = 0;
                            comissaoTotalEvento = 0;
                            TotalEvento = 0;

                        }//Fim Evento

                        //insere a linha total do Local
                        DataRow linhaTotalLocal = retorno.NewRow();

                        linhaTotalLocal["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Local " + linhaLocal["LocalNome"] + "</div>";
                        linhaTotalLocal["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalLocal).ToString(Utilitario.FormatoMoeda) + "</div>";
                        linhaTotalLocal["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalLocal) + "</div>";
                        //exibido apenas para perfil financeiro especial
                        linhaTotalLocal["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalLocal + convenienciaTotalLocal)) + "<div>";
                        retorno.Rows.Add(linhaTotalLocal);

                        //totais das Empresas
                        qtdTotalEmpresa = qtdTotalEmpresa + qtdTotalLocal;
                        valorTotalEmpresa = valorTotalEmpresa + valorTotalLocal;
                        convenienciaTotalEmpresa = convenienciaTotalEmpresa + convenienciaTotalLocal;
                        comissaoTotalEmpresa = comissaoTotalEmpresa + comissaoTotalLocal;
                        TotalEmpresa = TotalEmpresa + (valorTotalEmpresa + convenienciaTotalEmpresa);

                        //zera os totais do Local
                        qtdTotalLocal = 0;
                        valorTotalLocal = 0;
                        convenienciaTotalLocal = 0;
                        comissaoTotalLocal = 0;
                        TotalLocal = 0;


                    }//Fim Local

                    //insere a linha total do Empresa
                    DataRow linhaTotalEmpresa = retorno.NewRow();

                    linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
                    linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Math.Round(qtdTotalEmpresa).ToString(Utilitario.FormatoMoeda) + "</div>";
                    linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(valorTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((valorTotalEmpresa + convenienciaTotalEmpresa)) + "<div>";
                    retorno.Rows.Add(linhaTotalEmpresa);

                    //total geral
                    qtdTotal = qtdTotal + qtdTotalEmpresa;
                    valorTotal = valorTotal + valorTotalEmpresa;
                    convenienciaTotal = convenienciaTotal + convenienciaTotalEmpresa;
                    comissaoTotal = comissaoTotal + comissaoTotalEmpresa;
                    Total = Total + (valorTotal + convenienciaTotal);

                    //zera os totais de Empresas
                    qtdTotalEmpresa = 0;
                    valorTotalEmpresa = 0;
                    convenienciaTotalEmpresa = 0;
                    comissaoTotalEmpresa = 0;
                    TotalEmpresa = 0;

                }//Fim Empresa

                //linha em branco
                DataRow linhaBranco = retorno.NewRow();
                retorno.Rows.Add(linhaBranco);

                //insere linha totais
                DataRow linhaTotal = retorno.NewRow();

                linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
                linhaTotal["Quantidade"] = Math.Round(qtdTotal).ToString(Utilitario.FormatoMoeda);
                linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotal);
                //exibido apenas para perfil financeiro especial
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotal);
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotal);
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
                retorno.Rows.Add(linhaTotal);

                //esconde as taxas para os perfis de empresa que promove
                if (semTaxas)
                {
                    retorno.Columns.Remove("R$ Conveniência");
                    retorno.Columns.Remove("R$ Comissão");
                    retorno.Columns.Remove("R$ Total");
                }

                dsRetorno.Tables.Add(entrega);
                dsRetorno.Tables.Add(retorno);

                return dsRetorno;

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

		public DataSet CanaisLojas(int empresaID, int localID,int canalID,int lojaID,int formaPagamentoID, bool comCortesias,bool semTaxas)
		{
            BD bd = new BD();

            try
            {

                DataTable dtLoja = null;
                DataTable dtCanal = null;
                DataTable dtPagamentos = null;
                DataTable dtEmpresas = null;
                DataTable dtLocal = null;

                DataSet dsRetorno = new DataSet();

                DataTable entrega = EstruturaEntrega();

                DataTable retorno = EstruturaRetorno();

                DataTable calculo = EstruturaCalculo();

                //linhas
                DataRow linhaEntrega = null;
                DataRow linha = null;
                DataRow linhaLoja = null;
                DataRow linhaCanal = null;
                DataRow linhaEmpresa = null;
                DataRow linhaLocal = null;

                //variaveis para calculos de totais
                //totais de loja
                decimal qtdTotalLoja = 0;
                decimal valorTotalLoja = 0;
                decimal convenienciaTotalLoja = 0;
                decimal comissaoTotalLoja = 0;
                decimal TotalLoja = 0;
                //totais de canal
                decimal qtdTotalCanal = 0;
                decimal valorTotalCanal = 0;
                decimal convenienciaTotalCanal = 0;
                decimal comissaoTotalCanal = 0;
                decimal TotalCanal = 0;
                //totais do Local
                decimal qtdTotalLocal = 0;
                decimal valorTotalLocal = 0;
                decimal convenienciaTotalLocal = 0;
                decimal comissaoTotalLocal = 0;
                decimal TotalLocal = 0;
                //totais de empresa
                decimal qtdTotalEmpresa = 0;
                decimal valorTotalEmpresa = 0;
                decimal convenienciaTotalEmpresa = 0;
                decimal comissaoTotalEmpresa = 0;
                decimal TotalEmpresa = 0;
                //total geral
                decimal qtdTotal = 0;
                decimal valorTotal = 0;
                decimal convenienciaTotal = 0;
                decimal comissaoTotal = 0;
                decimal Total = 0;


                string sql = @"INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
                if (canalID > 0)
                    sql += " AND tCanal.ID = " + canalID;
                if (lojaID > 0)
                    sql += " AND tLoja.ID = " + lojaID;
                if (formaPagamentoID > 0)
                    sql += " AND tFormaPagamento.ID = " + formaPagamentoID;


                sql += @"
					DECLARE @Canais TABLE (ID INT IDENTITY(1,1), CanalID INT)
					DECLARE @n INT
					DECLARE @i INT 
					DECLARE @CanalID INT

					SET @i = 1
					INSERT INTO @Canais SELECT DISTINCT CanalID FROM #Vendas

					SELECT @n = COUNT(CanalID) FROM @Canais


					WHILE @i <= @n
						BEGIN
							SET @CanalID = (SELECT CanalID FROM @canais WHERE ID = @i)
							SET @i = @i + 1


							INSERT INTO #final
							SELECT 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							PagamentoID, 
							CASE 
								WHEN (PagamentoID IS NULL) THEN 'Cortesia' 
								ELSE PagamentoNome
							END  AS PagamentoNome,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE 
										WHEN (PagamentoID IS NULL) THEN 
											SUM(1)
										ELSE 
											SUM(1 * PagamentoPCT / 100)
									END
								ELSE
									CASE 		
										WHEN (PagamentoID IS NULL) THEN SUM(-1)
										ELSE SUM(1 * PagamentoPCT / -100)
								END
							END
							AS Quantidade,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									SUM(tPreco.Valor) * PagamentoPct / 100
								ELSE
									SUM(tPreco.Valor) * PagamentoPct / -100 
								END
							AS Valor,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / -100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Taxa, 
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / 100
										ELSE
											(TaxaComissao * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / -100
										ELSE
											(TaxaComissao * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Comissao 
							FROM tIngressoLog
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";


                sql += @" GROUP BY 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							PagamentoID, PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					SELECT 
					EmpresaID,EmpresaNome,LocalID, LocalNome, CanalID,CanalNome, LojaID,LojaNome,PagamentoID, PagamentoNome,
					SUM(Quantidade) AS Total, 
					SUM(Valor) AS Valor,
					SUM(Taxa) AS Taxa, 
					SUM(Comissao) AS Comissao 
					FROM #FINAL
					GROUP BY EmpresaID,EmpresaNome,LocalID, LocalNome, CanalID,CanalNome, LojaID,LojaNome,PagamentoID, PagamentoNome
					ORDER BY EmpresaNome, LocalNome, CanalNome, LojaNome, PagamentoNome;

					DROP TABLE #Final

					SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
						CASE tvendabilheteria.status
							WHEN 'P' THEN 
								tvendabilheteria.taxaentregavalor 
							ELSE 
								tvendabilheteria.taxaentregavalor * (-1)
							END 
						As TaxaEntregaValor
						FROM tIngressoLog
						INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
						inner join tVendaBilheteria ON tVendaBilheteria.ID = vendas.VendaBilheteriaID
						INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressolog.IngressoID
						INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
						INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
						WHERE tIngressoLog.Acao IN ('C','V') and tVendaBilheteria.taxaEntregaValor > 0 ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (eventoID > 0)
                    sql += " AND info.EventoID = " + eventoID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";

                sql += @"GROUP BY tTaxaEntrega.id,tTaxaEntrega.Nome,tVendaBilheteria.ID,tVendaBilheteria.TaxaEntregaValor,tVendaBilheteria.Status
								ORDER BY TaxaEntrega 
						DROP TABLE #Vendas
					";

                bd.Consulta(tabelaFinal() + tabelaVendas() + sql);

                while (bd.Consulta().Read())
                {
                    linha = calculo.NewRow();

                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["LocalNome"] = bd.LerString("LocalNome");
                    linha["CanalID"] = bd.LerInt("CanalID");
                    linha["CanalNome"] = bd.LerString("CanalNome");
                    linha["LojaID"] = bd.LerInt("LojaID");
                    linha["LojaNome"] = bd.LerString("LojaNome");
                    linha["PagamentoID"] = bd.LerInt("PagamentoID");
                    linha["PagamentoNome"] = bd.LerString("PagamentoNome");
                    linha["Total"] = bd.LerDecimal("Total");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Taxa"] = bd.LerDecimal("Taxa");
                    linha["Comissao"] = bd.LerDecimal("Comissao");

                    calculo.Rows.Add(linha);

                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {

                    linhaEntrega = entrega.NewRow();

                    linhaEntrega["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaEntrega["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaEntrega["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    entrega.Rows.Add(linhaEntrega);
                }

                bd.Fechar();

                string[] colunasEmpresa = new string[2];

                colunasEmpresa[0] = "EmpresaID";
                colunasEmpresa[1] = "EmpresaNome";

                dtEmpresas = CTLib.TabelaMemoria.DistinctSort(calculo, "EmpresaNome", "1=1", colunasEmpresa);

                for (int i = 0; i <= dtEmpresas.Rows.Count - 1; i++)
                {
                    linhaEmpresa = dtEmpresas.Rows[i];

                    //linha com nome da empresa
                    DataRow linhaRetorno = retorno.NewRow();

                    linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";

                    retorno.Rows.Add(linhaRetorno);

                    string[] colunasLocal = new string[2];

                    colunasLocal[0] = "LocalID";
                    colunasLocal[1] = "LocalNome";

                    dtLocal = CTLib.TabelaMemoria.DistinctSort(calculo, "LocalNome", "EmpresaID = " + linhaEmpresa["EmpresaID"], colunasLocal);

                    for (int n = 0; n <= dtLocal.Rows.Count - 1; n++)
                    {
                        linhaLocal = dtLocal.Rows[n];

                        //linha com nome do Local
                        linhaRetorno = retorno.NewRow();

                        linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Local " + linhaLocal["LocalNome"] + "</div>";

                        retorno.Rows.Add(linhaRetorno);

                        string[] colunasCanal = new string[2];

                        colunasCanal[0] = "CanalID";
                        colunasCanal[1] = "CanalNome";

                        dtCanal = CTLib.TabelaMemoria.DistinctSort(calculo, "CanalNome", "LocalID = '" + linhaLocal["LocalID"] + "'", colunasCanal);

                        for (int y = 0; y <= dtCanal.Rows.Count - 1; y++)
                        {
                            linhaCanal = dtCanal.Rows[y];

                            linha = retorno.NewRow();

                            //linha com o nome do canal
                            linha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Canal " + linhaCanal["CanalNome"] + "</div>";

                            retorno.Rows.Add(linha);

                            string[] colunasLoja = new string[2];

                            colunasLoja[0] = "LojaID";
                            colunasLoja[1] = "LojaNome";

                            dtLoja = CTLib.TabelaMemoria.DistinctSort(calculo, "LojaNome", "LocalID = '" + linhaLocal["LocalID"] + "' AND CanalID = '" + linhaCanal["CanalID"] + "'", colunasLoja);

                            for (int z = 0; z <= dtLoja.Rows.Count - 1; z++)
                            {
                                linhaLoja = dtLoja.Rows[z];

                                linha = retorno.NewRow();

                                //linha com o nome da loja
                                linha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:135px'>Loja " + linhaLoja["LojaNome"] + "</div>";

                                retorno.Rows.Add(linha);


                                //variáveis para os calculos
                                decimal qtdTotalIngresso = 0;

                                string[] colunasPagamento = new string[5];

                                colunasPagamento[0] = "PagamentoNome";
                                colunasPagamento[1] = "Total";
                                colunasPagamento[2] = "Valor";
                                colunasPagamento[3] = "Taxa";
                                colunasPagamento[4] = "Comissao";

                                dtPagamentos = CTLib.TabelaMemoria.DistinctSort(calculo, "PagamentoNome", "LocalNome = '" + linhaLocal["LocalNome"] + "' AND CanalNome = '" + linhaCanal["CanalNome"] + "' AND LojaNome = '" + linhaLoja["LojaNome"] + "'", colunasPagamento);

                                for (int k = 0; k <= dtPagamentos.Rows.Count - 1; k++)
                                {
                                    DataRow linhaPagamentos = dtPagamentos.Rows[k];

                                    linha = retorno.NewRow();
                                    linha["Formas de Recebimento"] = linhaPagamentos["PagamentoNome"];

                                    qtdTotalIngresso = (decimal)linhaPagamentos["Total"];
                                    linha["Quantidade"] = qtdTotalIngresso.ToString(IRLib.Paralela.Utilitario.FormatoMoeda);
                                    linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]));
                                    linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Taxa"]));
                                    linha["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Comissao"]));
                                    linha["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));

                                    retorno.Rows.Add(linha);

                                    //totais da loja
                                    qtdTotalLoja = qtdTotalLoja + Convert.ToDecimal(linhaPagamentos["Total"]);
                                    valorTotalLoja = valorTotalLoja + Convert.ToDecimal(linhaPagamentos["Valor"]);
                                    convenienciaTotalLoja = convenienciaTotalLoja + Convert.ToDecimal(linhaPagamentos["Taxa"]);
                                    comissaoTotalLoja = comissaoTotalLoja + Convert.ToDecimal(linhaPagamentos["Comissao"]);
                                    TotalLoja = TotalLoja + (Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));


                                }//Fim FormaPagamento

                                //insere a linha total da loja
                                DataRow linhaTotalLoja = retorno.NewRow();

                                linhaTotalLoja["Formas de Recebimento"] = "<div style='text-align:left;margin-left:135px'>Total Loja " + linhaLoja["LojaNome"] + "</div>";
                                linhaTotalLoja["Quantidade"] = Math.Round(qtdTotalLoja).ToString(Utilitario.FormatoMoeda);
                                linhaTotalLoja["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalLoja);
                                //exibido apenas para perfil financeiro especial
                                linhaTotalLoja["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotalLoja);
                                linhaTotalLoja["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotalLoja);
                                linhaTotalLoja["R$ Total"] = Utilitario.AplicaFormatoMoeda((valorTotalLoja + convenienciaTotalLoja));
                                retorno.Rows.Add(linhaTotalLoja);

                                //totais do Canal
                                qtdTotalCanal = qtdTotalCanal + qtdTotalLoja;
                                valorTotalCanal = valorTotalCanal + valorTotalLoja;
                                convenienciaTotalCanal = convenienciaTotalCanal + convenienciaTotalLoja;
                                comissaoTotalCanal = comissaoTotalCanal + comissaoTotalLoja;
                                TotalCanal = TotalCanal + (valorTotalCanal + convenienciaTotalCanal);

                                //zera os totais de loja
                                qtdTotalLoja = 0;
                                valorTotalLoja = 0;
                                convenienciaTotalLoja = 0;
                                comissaoTotalLoja = 0;
                                TotalLoja = 0;


                            }//Fim Loja

                            //insere a linha total do canal
                            DataRow linhaTotalCanal = retorno.NewRow();

                            linhaTotalCanal["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Total Canal " + linhaCanal["CanalNome"] + "</div>";
                            linhaTotalCanal["Quantidade"] = Math.Round(qtdTotalCanal).ToString(Utilitario.FormatoMoeda);
                            linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalCanal);
                            //exibido apenas para perfil financeiro especial
                            linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotalCanal);
                            linhaTotalCanal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotalCanal);
                            linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((valorTotalCanal + convenienciaTotalCanal));
                            retorno.Rows.Add(linhaTotalCanal);

                            //totais dos Locais
                            qtdTotalLocal = qtdTotalLocal + qtdTotalCanal;
                            valorTotalLocal = valorTotalLocal + valorTotalCanal;
                            convenienciaTotalLocal = convenienciaTotalLocal + convenienciaTotalCanal;
                            comissaoTotalLocal = comissaoTotalLocal + comissaoTotalCanal;
                            TotalLocal = TotalLocal + (valorTotalLocal + convenienciaTotalLocal);

                            //zera os totais de Canal
                            qtdTotalCanal = 0;
                            valorTotalCanal = 0;
                            convenienciaTotalCanal = 0;
                            comissaoTotalCanal = 0;
                            TotalCanal = 0;


                        }//Fim Canal

                        //insere a linha total do Local
                        DataRow linhaTotalLocal = retorno.NewRow();

                        linhaTotalLocal["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Local " + linhaLocal["LocalNome"] + "</div>";
                        linhaTotalLocal["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Math.Round(qtdTotalLocal).ToString(Utilitario.FormatoMoeda) + "</div>";
                        linhaTotalLocal["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalLocal) + "</div>";
                        //exibido apenas para perfil financeiro especial
                        linhaTotalLocal["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalLocal + convenienciaTotalLocal)) + "<div>";
                        retorno.Rows.Add(linhaTotalLocal);

                        //totais das Empresas
                        qtdTotalEmpresa = qtdTotalEmpresa + qtdTotalLocal;
                        valorTotalEmpresa = valorTotalEmpresa + valorTotalLocal;
                        convenienciaTotalEmpresa = convenienciaTotalEmpresa + convenienciaTotalLocal;
                        comissaoTotalEmpresa = comissaoTotalEmpresa + comissaoTotalLocal;
                        TotalEmpresa = TotalEmpresa + (valorTotalEmpresa + convenienciaTotalEmpresa);

                        //zera os totais do Local
                        qtdTotalLocal = 0;
                        valorTotalLocal = 0;
                        convenienciaTotalLocal = 0;
                        comissaoTotalLocal = 0;
                        TotalLocal = 0;


                    }//Fim Local

                    //insere a linha total do Empresa
                    DataRow linhaTotalEmpresa = retorno.NewRow();

                    linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
                    linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Math.Round(qtdTotalEmpresa).ToString(Utilitario.FormatoMoeda) + "</div>";
                    linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(valorTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((valorTotalEmpresa + convenienciaTotalEmpresa)) + "<div>";
                    retorno.Rows.Add(linhaTotalEmpresa);

                    //total geral
                    qtdTotal = qtdTotal + qtdTotalEmpresa;
                    valorTotal = valorTotal + valorTotalEmpresa;
                    convenienciaTotal = convenienciaTotal + convenienciaTotalEmpresa;
                    comissaoTotal = comissaoTotal + comissaoTotalEmpresa;
                    Total = Total + (valorTotal + convenienciaTotal);

                    //zera os totais de Empresas
                    qtdTotalEmpresa = 0;
                    valorTotalEmpresa = 0;
                    convenienciaTotalEmpresa = 0;
                    comissaoTotalEmpresa = 0;
                    TotalEmpresa = 0;

                }//Fim Empresa

                //linha em branco
                DataRow linhaBranco = retorno.NewRow();
                retorno.Rows.Add(linhaBranco);

                //insere linha totais
                DataRow linhaTotal = retorno.NewRow();

                linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
                linhaTotal["Quantidade"] = Math.Round(qtdTotal).ToString(Utilitario.FormatoMoeda);
                linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotal);
                //exibido apenas para perfil financeiro especial
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotal);
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotal);
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
                retorno.Rows.Add(linhaTotal);

                //esconde as taxas para os perfis de empresa que promove
                if (semTaxas)
                {
                    retorno.Columns.Remove("R$ Conveniência");
                    retorno.Columns.Remove("R$ Comissão");
                    retorno.Columns.Remove("R$ Total");
                }

                dsRetorno.Tables.Add(entrega);
                dsRetorno.Tables.Add(retorno);

                return dsRetorno;
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
			
		public DataSet Canais(int empresaID, int localID,int canalID,int formaPagamentoID, bool comCortesias,bool semTaxas)
		{
            BD bd = new BD();

            try
            {

                DataTable dtCanal = null;
                DataTable dtPagamentos = null;
                DataTable dtEmpresas = null;
                DataTable dtLocal = null;

                DataSet dsRetorno = new DataSet();

                DataTable entrega = EstruturaEntrega();

                DataTable retorno = EstruturaRetorno();

                DataTable calculo = EstruturaCalculo();

                //linhas
                DataRow linhaEntrega = null;
                DataRow linha = null;
                DataRow linhaCanal = null;
                DataRow linhaEmpresa = null;
                DataRow linhaLocal = null;

                //variaveis para calculos de totais
                //totais de canal
                decimal qtdTotalCanal = 0;
                decimal valorTotalCanal = 0;
                decimal convenienciaTotalCanal = 0;
                decimal comissaoTotalCanal = 0;
                decimal TotalCanal = 0;
                //totais do Local
                decimal qtdTotalLocal = 0;
                decimal valorTotalLocal = 0;
                decimal convenienciaTotalLocal = 0;
                decimal comissaoTotalLocal = 0;
                decimal TotalLocal = 0;
                //totais de empresa
                decimal qtdTotalEmpresa = 0;
                decimal valorTotalEmpresa = 0;
                decimal convenienciaTotalEmpresa = 0;
                decimal comissaoTotalEmpresa = 0;
                decimal TotalEmpresa = 0;
                //total geral
                decimal qtdTotal = 0;
                decimal valorTotal = 0;
                decimal convenienciaTotal = 0;
                decimal comissaoTotal = 0;
                decimal Total = 0;


                string sql = @"INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
                if (canalID > 0)
                    sql += " AND tCanal.ID = " + canalID;
                if (formaPagamentoID > 0)
                    sql += " AND tFormaPagamento.ID = " + formaPagamentoID;


                sql += @"
					DECLARE @Canais TABLE (ID INT IDENTITY(1,1), CanalID INT)
					DECLARE @n INT
					DECLARE @i INT 
					DECLARE @CanalID INT

					SET @i = 1
					INSERT INTO @Canais SELECT DISTINCT CanalID FROM #Vendas

					SELECT @n = COUNT(CanalID) FROM @Canais


					WHILE @i <= @n
						BEGIN
							SET @CanalID = (SELECT CanalID FROM @canais WHERE ID = @i)
							SET @i = @i + 1


							INSERT INTO #final
							SELECT 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							PagamentoID, 
							CASE 
								WHEN (PagamentoID IS NULL) THEN 'Cortesia' 
								ELSE PagamentoNome
							END  AS PagamentoNome,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE 
										WHEN (PagamentoID IS NULL) THEN 
											SUM(1)
										ELSE 
											SUM(1 * PagamentoPCT / 100)
									END
								ELSE
									CASE 		
										WHEN (PagamentoID IS NULL) THEN SUM(-1)
										ELSE SUM(1 * PagamentoPCT / -100)
								END
							END
							AS Quantidade,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									SUM(tPreco.Valor) * PagamentoPct / 100
								ELSE
									SUM(tPreco.Valor) * PagamentoPct / -100 
								END
							AS Valor,
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaConvenienciaValor) * PagamentoPct / -100
										ELSE
											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Taxa, 
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / 100
										ELSE
											(TaxaComissao * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											SUM(TaxaComissao) * PagamentoPct / -100
										ELSE
											(TaxaComissao * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
										END
								END AS Comissao 
							FROM tIngressoLog
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";


                sql += @" GROUP BY 
							info.EmpresaID,info.EmpresaNome,info.LocalID, info.LocalNome, info.EventoID,EventoNome, 
							vendas.CanalID,vendas.CanalNome, 
							vendas.LojaID,vendas.LojaNome,  
							PagamentoID, PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					SELECT 
					EmpresaID,EmpresaNome,LocalID, LocalNome, CanalID, CanalNome, PagamentoID,PagamentoNome,
					SUM(Quantidade) AS Total, 
					SUM(Valor) AS Valor,
					SUM(Taxa) AS Taxa, 
					SUM(Comissao) AS Comissao 
					FROM #FINAL
					GROUP BY EmpresaID,EmpresaNome,LocalID, LocalNome, CanalID, CanalNome,PagamentoID, PagamentoNome
					ORDER BY EmpresaNome, LocalNome, CanalNome, PagamentoNome;
					
					DROP TABLE #Final


					SELECT tTaxaEntrega.Nome AS TaxaEntrega,tTaxaEntrega.ID as TaxaEntregaID,
						CASE tvendabilheteria.status
							WHEN 'P' THEN 
								SUM(tvendabilheteria.taxaentregavalor) 
							ELSE 
								SUM(tvendabilheteria.taxaentregavalor) * (-1)
							END 
						As TaxaEntregaValor
						FROM #Vendas vendas
						INNER JOIN tVendaBilheteria ON tVendaBilheteria.ID = vendas.VendaBilheteriaID AND TaxaEntregaValor > 0
						INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID ";
                if (empresaID > 0)
                    sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                if (localID > 0)
                    sql += " AND info.LocalID = " + localID;
                if (!comCortesias)
                    sql += " AND tIngressoLog.CortesiaID = 0";

                sql += @"GROUP BY tTaxaEntrega.id,tTaxaEntrega.Nome,tVendaBilheteria.TaxaEntregaValor,tVendaBilheteria.Status
							ORDER BY TaxaEntrega 

						DROP TABLE #Vendas
					";

                bd.Consulta(tabelaFinal() + tabelaVendas() + sql);

                while (bd.Consulta().Read())
                {
                    linha = calculo.NewRow();

                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["EmpresaNome"] = bd.LerString("EmpresaNome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["LocalNome"] = bd.LerString("LocalNome");
                    linha["CanalID"] = bd.LerInt("CanalID");
                    linha["CanalNome"] = bd.LerString("CanalNome");
                    linha["PagamentoID"] = bd.LerInt("PagamentoID");
                    linha["PagamentoNome"] = bd.LerString("PagamentoNome");
                    linha["Total"] = bd.LerDecimal("Total");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Taxa"] = bd.LerDecimal("Taxa");
                    linha["Comissao"] = bd.LerDecimal("Comissao");

                    calculo.Rows.Add(linha);

                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {
                    linhaEntrega = entrega.NewRow();

                    linhaEntrega["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaEntrega["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaEntrega["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    entrega.Rows.Add(linhaEntrega);
                }

                bd.Fechar();

                string[] colunasEmpresa = new string[2];

                colunasEmpresa[0] = "EmpresaID";
                colunasEmpresa[1] = "EmpresaNome";

                dtEmpresas = CTLib.TabelaMemoria.DistinctSort(calculo, "EmpresaNome", "1=1", colunasEmpresa);

                for (int i = 0; i <= dtEmpresas.Rows.Count - 1; i++)
                {
                    linhaEmpresa = dtEmpresas.Rows[i];

                    //linha com nome da empresa
                    DataRow linhaRetorno = retorno.NewRow();

                    linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold'>Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";

                    retorno.Rows.Add(linhaRetorno);

                    string[] colunasLocal = new string[2];

                    colunasLocal[0] = "LocalID";
                    colunasLocal[1] = "LocalNome";

                    dtLocal = CTLib.TabelaMemoria.DistinctSort(calculo, "LocalNome", "EmpresaID = '" + linhaEmpresa["EmpresaID"] + "'", colunasLocal);

                    for (int n = 0; n <= dtLocal.Rows.Count - 1; n++)
                    {
                        linhaLocal = dtLocal.Rows[n];

                        //linha com nome do Local
                        linhaRetorno = retorno.NewRow();



                        linhaRetorno["Formas de Recebimento"] = "<div style='text-align:left;font-weight:bold;margin-left:40px'>Local " + linhaLocal["LocalNome"] + "</div>";

                        retorno.Rows.Add(linhaRetorno);

                        string[] colunasCanal = new string[2];

                        colunasCanal[0] = "CanalID";
                        colunasCanal[1] = "CanalNome";

                        dtCanal = CTLib.TabelaMemoria.DistinctSort(calculo, "CanalNome", "LocalID = '" + linhaLocal["LocalID"] + "'", colunasCanal);

                        for (int y = 0; y <= dtCanal.Rows.Count - 1; y++)
                        {
                            linhaCanal = dtCanal.Rows[y];

                            linha = retorno.NewRow();

                            //linha com o nome do canal
                            linha["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Canal " + linhaCanal["CanalNome"] + "</div>";

                            retorno.Rows.Add(linha);

                            //variáveis para os calculos
                            decimal qtdTotalIngresso = 0;

                            string[] colunasPagamento = new string[5];

                            colunasPagamento[0] = "PagamentoNome";
                            colunasPagamento[1] = "Total";
                            colunasPagamento[2] = "Valor";
                            colunasPagamento[3] = "Taxa";
                            colunasPagamento[4] = "Comissao";

                            dtPagamentos = CTLib.TabelaMemoria.DistinctSort(calculo, "PagamentoNome", "LocalID = '" + linhaLocal["LocalID"] + "' AND CanalID = '" + linhaCanal["CanalID"] + "'", colunasPagamento);

                            for (int k = 0; k <= dtPagamentos.Rows.Count - 1; k++)
                            {
                                DataRow linhaPagamentos = dtPagamentos.Rows[k];

                                linha = retorno.NewRow();
                                linha["Formas de Recebimento"] = linhaPagamentos["PagamentoNome"];

                                qtdTotalIngresso = (decimal)linhaPagamentos["Total"];
                                linha["Quantidade"] = qtdTotalIngresso.ToString(IRLib.Paralela.Utilitario.FormatoMoeda);
                                linha["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]));
                                linha["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Taxa"]));
                                linha["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Comissao"]));
                                linha["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));

                                retorno.Rows.Add(linha);

                                //totais do Canal
                                qtdTotalCanal = qtdTotalCanal + Convert.ToDecimal(linhaPagamentos["Total"]);
                                valorTotalCanal = valorTotalCanal + Convert.ToDecimal(linhaPagamentos["Valor"]);
                                convenienciaTotalCanal = convenienciaTotalCanal + Convert.ToDecimal(linhaPagamentos["Taxa"]);
                                comissaoTotalCanal = comissaoTotalCanal + Convert.ToDecimal(linhaPagamentos["Comissao"]);
                                TotalCanal = TotalCanal + (Convert.ToDecimal(linhaPagamentos["Valor"]) + Convert.ToDecimal(linhaPagamentos["Taxa"]));


                            }//Fim FormaPagamento

                            //insere a linha total do canal
                            DataRow linhaTotalCanal = retorno.NewRow();

                            linhaTotalCanal["Formas de Recebimento"] = "<div style='text-align:left;margin-left:105px'>Total Canal " + linhaCanal["CanalNome"] + "</div>";
                            linhaTotalCanal["Quantidade"] = Math.Round(qtdTotalCanal).ToString(Utilitario.FormatoMoeda);
                            linhaTotalCanal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotalCanal);
                            //exibido apenas para perfil financeiro especial
                            linhaTotalCanal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotalCanal);
                            linhaTotalCanal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotalCanal);
                            linhaTotalCanal["R$ Total"] = Utilitario.AplicaFormatoMoeda((valorTotalCanal + convenienciaTotalCanal));
                            retorno.Rows.Add(linhaTotalCanal);

                            //totais dos Locais
                            qtdTotalLocal = qtdTotalLocal + qtdTotalCanal;
                            valorTotalLocal = valorTotalLocal + valorTotalCanal;
                            convenienciaTotalLocal = convenienciaTotalLocal + convenienciaTotalCanal;
                            comissaoTotalLocal = comissaoTotalLocal + comissaoTotalCanal;
                            TotalLocal = TotalLocal + (valorTotalLocal + convenienciaTotalLocal);

                            //zera os totais de Canal
                            qtdTotalCanal = 0;
                            valorTotalCanal = 0;
                            convenienciaTotalCanal = 0;
                            comissaoTotalCanal = 0;
                            TotalCanal = 0;


                        }//Fim Canal

                        //insere a linha total do Local
                        DataRow linhaTotalLocal = retorno.NewRow();

                        linhaTotalLocal["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;margin-left:40px'>Total Local " + linhaLocal["LocalNome"] + "</div>";
                        linhaTotalLocal["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + qtdTotalLocal.ToString(Utilitario.FormatoMoeda) + "</div>";
                        linhaTotalLocal["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(valorTotalLocal) + "</div>";
                        //exibido apenas para perfil financeiro especial
                        linhaTotalLocal["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalLocal) + "</div>";
                        linhaTotalLocal["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0'>" + Utilitario.AplicaFormatoMoeda((valorTotalLocal + convenienciaTotalLocal)) + "<div>";
                        retorno.Rows.Add(linhaTotalLocal);

                        //totais das Empresas
                        qtdTotalEmpresa = qtdTotalEmpresa + qtdTotalLocal;
                        valorTotalEmpresa = valorTotalEmpresa + valorTotalLocal;
                        convenienciaTotalEmpresa = convenienciaTotalEmpresa + convenienciaTotalLocal;
                        comissaoTotalEmpresa = comissaoTotalEmpresa + comissaoTotalLocal;
                        TotalEmpresa = TotalEmpresa + (valorTotalEmpresa + convenienciaTotalEmpresa);

                        //zera os totais do Local
                        qtdTotalLocal = 0;
                        valorTotalLocal = 0;
                        convenienciaTotalLocal = 0;
                        comissaoTotalLocal = 0;
                        TotalLocal = 0;


                    }//Fim Local

                    //insere a linha total do Empresa
                    DataRow linhaTotalEmpresa = retorno.NewRow();

                    linhaTotalEmpresa["Formas de Recebimento"] = "<div style='background: #CCCCCC;font-weight:bold; padding: 0 0 0 0;text-align:left;'>Total Empresa " + linhaEmpresa["EmpresaNome"] + "</div>";
                    linhaTotalEmpresa["Quantidade"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Math.Round(qtdTotalEmpresa).ToString(Utilitario.FormatoMoeda) + "</div>";
                    linhaTotalEmpresa["R$ Ingressos"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(valorTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Conveniência"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(convenienciaTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Comissão"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda(comissaoTotalEmpresa) + "</div>";
                    linhaTotalEmpresa["R$ Total"] = "<div style='background: #CCCCCC; padding: 0 0 0 0;text-align:right'>" + Utilitario.AplicaFormatoMoeda((valorTotalEmpresa + convenienciaTotalEmpresa)) + "<div>";
                    retorno.Rows.Add(linhaTotalEmpresa);

                    //total geral
                    qtdTotal = qtdTotal + qtdTotalEmpresa;
                    valorTotal = valorTotal + valorTotalEmpresa;
                    convenienciaTotal = convenienciaTotal + convenienciaTotalEmpresa;
                    comissaoTotal = comissaoTotal + comissaoTotalEmpresa;
                    Total = Total + (valorTotal + convenienciaTotal);

                    //zera os totais de Empresas
                    qtdTotalEmpresa = 0;
                    valorTotalEmpresa = 0;
                    convenienciaTotalEmpresa = 0;
                    comissaoTotalEmpresa = 0;
                    TotalEmpresa = 0;

                }//Fim Empresa

                //linha em branco
                DataRow linhaBranco = retorno.NewRow();
                retorno.Rows.Add(linhaBranco);

                //insere linha totais
                DataRow linhaTotal = retorno.NewRow();

                linhaTotal["Formas de Recebimento"] = "<div style='text-align:left'>Total</div>";
                linhaTotal["Quantidade"] = Math.Round(qtdTotal).ToString(Utilitario.FormatoMoeda);
                linhaTotal["R$ Ingressos"] = Utilitario.AplicaFormatoMoeda(valorTotal);
                //exibido apenas para perfil financeiro especial
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(convenienciaTotal);
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(comissaoTotal);
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda((Convert.ToDecimal(linhaTotal["R$ Ingressos"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"])));
                retorno.Rows.Add(linhaTotal);

                //esconde as taxas para os perfis de empresa que promove
                if (semTaxas)
                {
                    retorno.Columns.Remove("R$ Conveniência");
                    retorno.Columns.Remove("R$ Comissão");
                    retorno.Columns.Remove("R$ Total");
                }

                dsRetorno.Tables.Add(entrega);
                dsRetorno.Tables.Add(retorno);

                return dsRetorno;
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

		#endregion


	} // fim de classe
} // fim de namespace
