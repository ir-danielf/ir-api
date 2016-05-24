using CTLib;
using System;
using System.Data;

namespace IRLib
{
    /// <summary>
    /// Summary description for Genericos.
    /// </summary>
    public class Genericos
    {
        public const string COMBINACAO_SETOR = "Setor";
        public const string COMBINACAO_FORMAPAGAMENTO = "Forma de Pagamento";
        public const string COMBINACAO_LOJA = "Loja";
        public const string COMBINACAO_DIA = "Dia";
        public const string COMBINACAO_EVENTO = "Evento";
        public const string COMBINACAO_CANAL = "Canal";
        public const string COMBINACAO_PRECO = "Preço";

        private BD bd;

        public Genericos()
        {
            bd = new BD();
        }

        #region txEntregaPorEvento


        public DataTable getDataTxEntregaPorEvento(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {

            //DataTable que recebe os dados da consulta ao Banco
            DataTable tabela = new DataTable("tabelaGetDataTaxaEntrega");

            //Estrutura da tabela que receberá os dados do select
            tabela.Columns.Add("TaxaEntrega", typeof(string));
            tabela.Columns.Add("TaxaEntregaID", typeof(int));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

            BD bd = new BD();

            string sql;

            try
            {

                if (localID > 0)
                {

                    sql = @"SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
					CASE tvendabilheteria.status        
						WHEN 'P' THEN          
							tvendabilheteria.taxaentregavalor         
						ELSE          
							tvendabilheteria.taxaentregavalor * (-1)        
						END        
					As TaxaEntregaValor            
					FROM tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID 
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid       
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    "WHERE  Acao IN ('C','V') AND tLocal.EmpresaID = " + empresaID;

                    if (apresentacaoID > 0)
                        sql += @" AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    if (eventoID > 0)
                        sql += @" AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += @" AND tIngressoLog.CortesiaID = 0 ";

                    sql += " AND tApresentacao.DisponivelRelatorio = 'T'  " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura <= '" + dataFinal + "'  " +
                        "AND tLocal.ID =  " + localID +
                        " GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor ";
                }
                else
                {

                    sql = @"SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,
					CASE tvendabilheteria.status        
						WHEN 'P' THEN          
							tvendabilheteria.taxaentregavalor         
						ELSE          
							tvendabilheteria.taxaentregavalor * (-1)        
						END        
					As TaxaEntregaValor            
					FROM tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID 
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid       
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    "WHERE  Acao IN ('C','V') ";

                    if (apresentacaoID > 0)
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    if (empresaID > 0)
                        sql += "AND tLocal.EmpresaID = " + empresaID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        "' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor ";

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();

                    linhaTabela["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaTabela["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaTabela["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    tabela.Rows.Add(linhaTabela);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }//fim getDataTaxaEntregaPorEvento

        #endregion

        #region txEntregaPorApresentacao

        public DataTable getDataTxEntregaPorApresentacao(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {

            //DataTable que recebe os dados da consulta ao Banco
            DataTable tabela = new DataTable("tabelaGetDataTaxaEntrega");

            //Estrutura da tabela que receberá os dados do select
            tabela.Columns.Add("TaxaEntrega", typeof(string));
            tabela.Columns.Add("TaxaEntregaID", typeof(int));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

            BD bd = new BD();

            string sql;

            try
            {

                if (localID > 0)
                {

                    sql = @"SELECT tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID,   
						CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
						FROM tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID  
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCanal ON tIngressoLog.CanalID = tCanal.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID 
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID 
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE   
					Acao IN ('V', 'C')  
					AND tLocal.EmpresaID = " + empresaID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += " AND tApresentacao.DisponivelRelatorio = 'T'  " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura <= '" + dataFinal + "'  " +
                        "AND tLocal.ID =  " + localID +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor 
						ORDER BY tTaxaEntrega.Nome ";
                }
                else
                {

                    sql = @"SELECT  
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					from tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					 Acao IN ('C','V') ";

                    if (empresaID > 0)
                        sql += "AND tLocal.EmpresaID = " + empresaID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();

                    linhaTabela["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaTabela["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaTabela["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    tabela.Rows.Add(linhaTabela);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }//fim getDataTaxaEntregaPorApresentacao

        #endregion

        #region txEntregaPorFormaPagamento

        public DataTable getDataTxEntregaPorFormaPagamento(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            //DataTable que recebe os dados da consulta ao Banco
            DataTable tabela = new DataTable("tabelaGetDataTaxaEntrega");

            //Estrutura da tabela que receberá os dados do select
            tabela.Columns.Add("TaxaEntrega", typeof(string));
            tabela.Columns.Add("TaxaEntregaID", typeof(int));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

            BD bd = new BD();

            string sql;

            try
            {

                if (localID > 0)
                {

                    sql = @"SELECT 
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					FROM tIngressolog  
					INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID  
					INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID
					INNER JOIN tApresentacao ON tApresentacao.ID = ApresentacaoID 
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tCaixa ON tIngressoLog.CaixaID = tCAixa.ID 
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tVendaBilheteria ON tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID    
					INNER JOIN tLocal ON tLocal.ID = tEvento.LocalID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					Acao IN ('C','V') 
					AND tLocal.EmpresaID = " + empresaID;

                    if (eventoID > 0)
                        sql += "AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    if (apresentacaoID > 0)
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;



                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        "' AND tLocal.ID =  " + localID +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";
                }
                else
                {

                    sql = @"SELECT  
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					from tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					Acao IN ('C','V') ";

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (empresaID > 0)
                        sql += " AND tLocal.EmpresaID = " + empresaID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    if (apresentacaoID > 0)
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();

                    linhaTabela["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaTabela["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaTabela["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    tabela.Rows.Add(linhaTabela);


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }//fim getDataTaxaEntregaPorFormaPagamento

        #endregion

        #region txEntregaPorCanal

        public DataTable getDataTxEntregaPorCanal(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            //DataTable que recebe os dados da consulta ao Banco
            DataTable tabela = new DataTable("tabelaGetDataTaxaEntrega");

            //Estrutura da tabela que receberá os dados do select
            tabela.Columns.Add("TaxaEntrega", typeof(string));
            tabela.Columns.Add("TaxaEntregaID", typeof(int));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

            BD bd = new BD();

            string sql;

            try
            {

                if (localID > 0)
                {

                    sql = @"SELECT 
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					FROM tIngressolog  
					INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID  
					INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID
					INNER JOIN tApresentacao ON tApresentacao.ID = ApresentacaoID 
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tCaixa ON tIngressoLog.CaixaID = tCAixa.ID 
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tVendaBilheteria ON tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID    
					INNER JOIN tLocal ON tLocal.ID = tEvento.LocalID " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					Acao IN ('C','V') AND tLocal.EmpresaID = " + empresaID;

                    if (apresentacaoID > 0)
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += " AND tApresentacao.DisponivelRelatorio = 'T'  " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura <= '" + dataFinal + "'  " +
                        "AND tLocal.ID =  " + localID +
                        " Group By tCanal.Nome, tIngressoLog.Acao, tPreco.Valor ORDER BY tCanal.Nome";



                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        "' AND tLocal.ID =  " + localID +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";
                }
                else
                {

                    sql = @"SELECT  
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					from tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID  " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					Acao IN ('C','V') ";

                    if (apresentacaoID > 0)
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    if (empresaID > 0)
                        sql += "AND tLocal.EmpresaID = " + empresaID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();

                    linhaTabela["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaTabela["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaTabela["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    tabela.Rows.Add(linhaTabela);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }//fim getDataTaxaEntregaPorFormaPagamento

        #endregion

        #region getDataTxEntrega

        public DataTable getDataTxEntrega(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, string tipoRelatorio)
        {

            //DataTable que recebe os dados da consulta ao Banco
            DataTable tabela = new DataTable("tabelaGetDataTaxaEntrega");

            //Estrutura da tabela que receberá os dados do select
            tabela.Columns.Add("TaxaEntrega", typeof(string));
            tabela.Columns.Add("TaxaEntregaID", typeof(int));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

            BD bd = new BD();

            string sql;

            try
            {

                if (localID > 0)
                {

                    sql = @"SELECT 
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					FROM tIngressolog  
					INNER JOIN tIngresso ON tIngressolog.IngressoID = tIngresso.ID  
					INNER JOIN tpreco ON tingressolog.precoID = tpreco.ID
					INNER JOIN tApresentacao ON tApresentacao.ID = ApresentacaoID 
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tCaixa ON tIngressoLog.CaixaID = tCAixa.ID 
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tVendaBilheteria ON tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID    
					INNER JOIN tLocal ON tLocal.ID = tEvento.LocalID  " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					Acao IN ('C','V') AND tLocal.EmpresaID = " + empresaID;

                    if (apresentacaoID > 0 && tipoRelatorio != "Apresentacao")
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += "AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        "' AND tLocal.ID =  " + localID +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";
                }
                else
                {

                    sql = @"SELECT  
					tTaxaEntrega.Nome AS TaxaEntrega,tvendabilheteria.ID as TaxaEntregaID, 
							CASE tVendaBilheteria.Status        
							WHEN 'P' THEN          
								tvendabilheteria.taxaentregavalor         
							ELSE          
								tvendabilheteria.taxaentregavalor * (-1)        
							END        
						As TaxaEntregaValor   
					from tIngressoLog 
					INNER JOIN tIngresso ON tIngressoLog.IngressoID = tIngresso.ID 
					INNER JOIN tPreco ON tIngressoLog.PrecoID = tPreco.ID
					INNER JOIN tApresentacao ON tIngresso.ApresentacaoID = tApresentacao.ID 
					INNER JOIN tCaixa ON tCaixa.ID = tIngressoLog.CaixaID
					INNER JOIN tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID and tIngressolog.VendaBilheteriaID = tVendaBilheteria.ID         
					INNER JOIN tTaxaEntrega ON tTaxaEntrega.ID = tvendabilheteria.taxaentregaid
					INNER JOIN tEvento ON tIngresso.EventoID = tEvento.ID  
					INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID  " +
                    ((regionalID > 0) ? "" +
                    "INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " : "") +
                    @"WHERE  
					Acao IN ('C','V') ";

                    if (empresaID > 0)
                        sql += "AND tLocal.EmpresaID = " + empresaID;

                    if (regionalID > 0)
                        sql += @" AND tEmpresa.RegionalID =  " + regionalID;

                    if (apresentacaoID > 0 && tipoRelatorio != "Apresentacao")
                        sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;

                    if (eventoID > 0)
                        sql += " AND tIngresso.EventoID =  " + eventoID;

                    if (!comCortesia)
                        sql += " AND tIngressoLog.CortesiaID = 0 ";

                    sql += "AND tVendaBilheteria.taxaEntregaValor > 0 AND tApresentacao.DisponivelRelatorio = 'T' " +
                        "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                        @"' GROUP BY tvendabilheteria.id,tvendabilheteria.status,tTaxaEntrega.Nome,tvendabilheteria.taxaentregavalor
					ORDER BY tTaxaEntrega.Nome ";

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();

                    linhaTabela["TaxaEntrega"] = bd.LerString("TaxaEntrega");
                    linhaTabela["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                    linhaTabela["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");

                    tabela.Rows.Add(linhaTabela);


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }//fim getDataTaxaEntregaPorFormaPagamento

        #endregion



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

        //tabelas temporárias usadas nos métodos sql

        public string tabelaFinal()
        {
            string tabelaFinal = @"		
				CREATE TABLE #final
				(
					EmpresaNome NVARCHAR(100), 
					LocalNome NVARCHAR(100),
					EventoNome NVARCHAR(100), 
					CanalNome NVARCHAR(100),
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


        /// <summary>
        ///	Método genérico usado em Vendas Por Evento e Vendas Por Canal nos casos que tiverem duas combinações de relatório,
        ///	onde grupo é o tipo de relatório 1 e SubGrupo é o tipo de relatório 2, selecionados no filtro. 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="grupoNome"></param>
        /// <param name="subGrupoNome"></param>
        /// <param name="grupoID"></param>
        /// <param name="subGrupoID"></param>
        /// <param name="labelColuna"></param>
        /// <returns></returns>
        private DataTable MontaRelatorioCombinado(string sql, string grupoNome, string subGrupoNome, string grupoID, string subGrupoID, string labelColuna)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {
                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add(grupoNome, typeof(string));
                dtConsulta.Columns.Add(subGrupoNome, typeof(string));
                dtConsulta.Columns.Add(grupoID, typeof(int));
                dtConsulta.Columns.Add(subGrupoID, typeof(int));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));


                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha[grupoID] = bd.LerInt(grupoID);
                    novaLinha[subGrupoID] = bd.LerInt(subGrupoID);
                    novaLinha[grupoNome] = bd.LerString(grupoNome);
                    novaLinha[subGrupoNome] = bd.LerString(subGrupoNome);
                    if (grupoNome == "PagamentoNome" || subGrupoNome == "PagamentoNome")
                        novaLinha["Quantidade"] = bd.LerDecimal("Total");
                    else
                        novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;


                DataTable dtGrupo = CTLib.TabelaMemoria.DistinctSort(dtConsulta, grupoNome + ", " + grupoID, string.Empty, grupoID, grupoNome);

                string umIgualUm = grupoID + " = " + grupoID; // usado no compute para comparações 1=1

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", umIgualUm)))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", umIgualUm))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", umIgualUm))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", umIgualUm))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linhaGrupo;
                DataRow linhaSubGrupo;
                //foreach(DataRow linha in dt.Rows)
                for (int i = 0; i <= dtGrupo.Rows.Count - 1; i++)
                {
                    linhaGrupo = dtGrupo.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left;font-weight:bold'>" + linhaGrupo[grupoNome].ToString() + "</div>";

                    tabela.Rows.Add(linhaFinal);

                    DataTable dtSubGrupo = CTLib.TabelaMemoria.DistinctSort(dtConsulta, subGrupoNome + ", " + subGrupoID, grupoID + " = '" + linhaGrupo[grupoID] + "'", subGrupoID, subGrupoNome);
                    for (int j = 0; j <= dtSubGrupo.Rows.Count - 1; j++)
                    {
                        linhaSubGrupo = dtSubGrupo.Rows[j];
                        linhaFinal = tabela.NewRow();

                        linhaFinal["VariacaoLinhaID"] = 0;
                        linhaFinal["VariacaoLinha"] = "<div style='text-align:left;margin-left:40px'>" + linhaSubGrupo[subGrupoNome].ToString() + "</div>";

                        // Quantidade e Valor
                        linhaFinal["Qtd Total"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"));
                        linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"))));
                        linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND " + grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"));
                        linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND " + grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"))));
                        linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND " + grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"))));
                        linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND " + grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'")))));
                        linhaFinal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"));
                        linhaFinal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", grupoID + " = '" + linhaGrupo[grupoID] + "' and " + subGrupoID + " = '" + linhaSubGrupo[subGrupoID] + "'"));
                        linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));
                        tabela.Rows.Add(linhaFinal);
                    }

                    // Insere a linha total 
                    DataRow linhaTotalGrupo = tabela.NewRow();

                    linhaTotalGrupo["VariacaoLinhaID"] = 0;
                    linhaTotalGrupo["VariacaoLinha"] = "<div style='text-align:left;font-weight:bold'>Total " + linhaGrupo[grupoNome].ToString() + "</div>";
                    linhaTotalGrupo["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", grupoID + " = '" + linhaGrupo[grupoID] + "'")))));
                    linhaTotalGrupo["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", grupoID + " = '" + linhaGrupo[grupoID] + "'"))));
                    linhaTotalGrupo["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 and " + grupoID + " = '" + linhaGrupo[grupoID] + "'")))));
                    linhaTotalGrupo["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 and " + grupoID + " = '" + linhaGrupo[grupoID] + "'"))));
                    linhaTotalGrupo["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 and " + grupoID + " = '" + linhaGrupo[grupoID] + "'"))));
                    linhaTotalGrupo["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 and " + grupoID + " = '" + linhaGrupo[grupoID] + "'")))));
                    linhaTotalGrupo["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", grupoID + " = '" + linhaGrupo[grupoID] + "'"));
                    linhaTotalGrupo["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", grupoID + " = '" + linhaGrupo[grupoID] + "'"));
                    linhaTotalGrupo["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotalGrupo["R$ Total"]) + Convert.ToDecimal(linhaTotalGrupo["R$ Conveniência"]));

                    tabela.Rows.Add(linhaTotalGrupo);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = labelColuna;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }


        #region Por Canal

        #region Relatórios sem combinação

        public DataTable VendasPorCanal_Evento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {

                //verifica se foi selecionado o filtro secundário em tipo de relatório

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Setor")
                        return VendasPorEventoSetor(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Forma de Pagamento")
                        return VendasPorEventoFormaPagamento(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Loja")
                        return VendasPorEventoLoja(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Dia")
                        return VendasPorEventoDia(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);

                }



                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("EventoID", typeof(int));
                dtConsulta.Columns.Add("EventoNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorCanal_Evento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);

                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["EventoID"] = bd.LerString("EventoID");
                    novaLinha["EventoNome"] = bd.LerString("EventoNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasEvento = new string[2];

                colunasEvento[0] = "EventoID";
                colunasEvento[1] = "EventoNome";

                DataTable dt = CTLib.TabelaMemoria.DistinctSort(dtConsulta, "EventoID", "", colunasEvento);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "EventoNome = EventoNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "EventoNome = EventoNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "EventoNome = EventoNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "EventoNome = EventoNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["EventoNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "EventoID = " + linha["EventoID"]));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "EventoID = " + linha["EventoID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND EventoID = " + linha["EventoID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND EventoID = " + linha["EventoID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND EventoID = " + linha["EventoID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND EventoID = " + linha["EventoID"])))));
                    linhaFinal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "EventoID = " + linha["EventoID"]));
                    linhaFinal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "EventoID = " + linha["EventoID"]));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Eventos";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorCanal_FormaPagamento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {
                //verifica se foi selecionado o filtro secundário em tipo de relatório

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Dia")
                        return VendasPorFormaPagamentoDia(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Evento")
                        return VendasPorFormaPagamentoEvento(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                }

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("FormaPagamentoID", typeof(int));
                dtConsulta.Columns.Add("FormaPagamentoNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorCanal_FormaPagamento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);

                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    novaLinha["FormaPagamentoNome"] = bd.LerString("FormaPagamentoNome");
                    novaLinha["Quantidade"] = bd.LerDecimal("Total");
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasFormaPagamento = new string[2];

                colunasFormaPagamento[0] = "FormaPagamentoID";
                colunasFormaPagamento[1] = "FormaPagamentoNome";


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasFormaPagamento);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "FormaPagamentoNome = FormaPagamentoNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "FormaPagamentoNome = FormaPagamentoNome"))));
                linhaTotal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0"));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "FormaPagamentoNome = FormaPagamentoNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "FormaPagamentoNome = FormaPagamentoNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                foreach (DataRow linha in dt.Rows)
                {
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["FormaPagamentoNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "FormaPagamentoID = " + linha["FormaPagamentoID"])))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "FormaPagamentoID = " + linha["FormaPagamentoID"]))));
                    linhaFinal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"])))));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"])))));
                    linhaFinal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }

                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Formas de Pagamento";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorCanal_Loja(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {

                //verifica se foi selecionado o filtro secundário em tipo de relatório

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Evento")
                        return VendasPorLojaEvento(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Forma de Pagamento")
                        return VendasPorLojaFormaPagamento(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Dia")
                        return VendasPorLojaDia(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                }


                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("LojaID", typeof(int));
                dtConsulta.Columns.Add("LojaNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorCanal_Loja(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);

                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["LojaID"] = bd.LerInt("LojaID");
                    novaLinha["LojaNome"] = bd.LerString("LojaNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasLoja = new string[2];

                colunasLoja[0] = "LojaID";
                colunasLoja[1] = "LojaNome";


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasLoja);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "LojaNome = LojaNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "LojaNome = LojaNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "LojaNome = LojaNome"));
                linhaTotal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "LojaNome = LojaNome"));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["LojaNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "LojaID = " + linha["LojaID"]));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "LojaID = " + linha["LojaID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND LojaID = " + linha["LojaID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND LojaID = " + linha["LojaID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND LojaID = " + linha["LojaID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND LojaID = " + linha["LojaID"])))));
                    linhaFinal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "LojaID = " + linha["LojaID"]));
                    linhaFinal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "LojaID = " + linha["LojaID"]));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Eventos";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorCanal_Dia(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();
            tabela.Columns.Add("Data", typeof(DateTime));

            try
            {

                //verifica se foi selecionado o filtro secundário em tipo de relatório

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Loja")
                        return VendasPorDiaLoja(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                }

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("Data", typeof(DateTime));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorCanal_Dia(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);

                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["Data"] = Convert.ToDateTime(Utilitario.DataHoraBDParaDataHoraLegivel(bd.LerString("Data") + "000000"));
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, "Data");

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Data = Data")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Data = Data"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Data = Data"));
                linhaTotal["R$ Comissão"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Data = Data"));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["Data"].ToString() + "</div>";
                    linhaFinal["Data"] = Convert.ToDateTime(linha["Data"]);

                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Data = '" + linha["Data"] + "'"));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Data = '" + linha["Data"] + "'"))));
                    linhaFinal["Qtd Vend"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND Data = '" + linha["Data"] + "'"));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND Data = '" + linha["Data"] + "'"))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND Data = '" + linha["Data"] + "'"))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND Data = '" + linha["Data"] + "'")))));
                    linhaFinal["R$ Conveniência"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Data = '" + linha["Data"] + "'"));
                    linhaFinal["R$ Comissão"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Data = '" + linha["Data"] + "'"));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }

                //				DataRow[] drsOrdenar = tabela.Select("VariacaoLinha<> ''","Data DESC");
                //				TabelaMemoria oTm = new TabelaMemoria();
                //				oTm.DataRowsParaTabelaDestino(drsOrdenar);
                //				tabela = oTm.TabelaDestino.Copy();


                // Insere a linha de totais.
                linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Data = Data")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Data = Data"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Data = Data"));
                linhaTotal["R$ Comissão"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Data = Data"));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                tabela.Rows.Add(linhaTotal);


                tabela.Columns.RemoveAt(tabela.Columns.Count - 1);

                tabela.Columns["VariacaoLinha"].ColumnName = "Dias";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
            return tabela;
        }


        public DataTable VendasPorCanal_Usuario(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {
                //verifica se foi selecionado o filtro secundário em tipo de relatório

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Forma de Pagamento")
                        return VendasPorUsuarioFormaPagamento(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                    if (combinacao == "Evento")
                        return VendasPorUsuarioEvento(perfilTipoID, dataInicial, dataFinal, comCortesia, localID, canalID, empresaID, regionalID, lojaID, usuarioID, combinacao);
                }

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("UsuarioID", typeof(int));
                dtConsulta.Columns.Add("Usuario", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorCanal_Usuario(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);

                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["UsuarioID"] = bd.LerString("UsuarioID");
                    novaLinha["Usuario"] = bd.LerString("Usuario");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasUsuario = new string[2];

                colunasUsuario[0] = "UsuarioID";
                colunasUsuario[1] = "Usuario";


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasUsuario);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Usuario = Usuario")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Usuario = Usuario"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Usuario = Usuario"));
                linhaTotal["R$ Comissão"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Usuario = Usuario"));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["Usuario"] + "</div>";

                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "UsuarioID = " + linha["UsuarioID"]));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "UsuarioID = " + linha["UsuarioID"]))));
                    linhaFinal["Qtd Vend"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND UsuarioID = " + linha["UsuarioID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND UsuarioID = " + linha["UsuarioID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND UsuarioID = " + linha["UsuarioID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND UsuarioID = " + linha["UsuarioID"])))));
                    linhaFinal["R$ Conveniência"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "UsuarioID = " + linha["UsuarioID"]));
                    linhaFinal["R$ Comissão"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "UsuarioID = " + linha["UsuarioID"]));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }

                DataRow[] drsOrdenar = tabela.Select("VariacaoLinha<> ''", "VariacaoLinha");
                TabelaMemoria oTm = new TabelaMemoria();
                oTm.DataRowsParaTabelaDestino(drsOrdenar);
                tabela = oTm.TabelaDestino.Copy();


                // Insere a linha de totais.
                linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Usuario = Usuario")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Usuario = Usuario"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Usuario = Usuario"));
                linhaTotal["R$ Comissão"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Usuario = Usuario"));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                tabela.Rows.Add(linhaTotal);


                //tabela.Columns.RemoveAt(tabela.Columns.Count -1);

                tabela.Columns["VariacaoLinha"].ColumnName = "Usuário";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        #endregion

        #region combinações de relatório

        //****************************Com Evento**********************************

        //EVENTO E SETOR
        public DataTable VendasPorEventoSetor(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_EventoSetor(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "SetorNome", "EventoID", "SetorID", "Evento/Setor");
        }

        //EVENTO E FORMA DE PAGAMENTO
        public DataTable VendasPorEventoFormaPagamento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_EventoFormaPagamento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "PagamentoNome", "EventoID", "PagamentoID", "Evento/Forma de Pagamento");
        }

        //EVENTO E LOJA
        public DataTable VendasPorEventoLoja(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_EventoLoja(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "lojaNome", "EventoID", "LojaID", "Evento/Loja");
        }

        //EVENTO E DIA
        public DataTable VendasPorEventoDia(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_EventoDia(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "DiaNome", "EventoID", "DataID", "Evento/Dia");
        }

        //****************************Com Forma de Pagamento**********************************

        //FORMA DE PAGAMENTO E DIA
        public DataTable VendasPorFormaPagamentoDia(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_FormaPagamentoDia(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "PagamentoNome", "DiaNome", "PagamentoID", "DataID", "Forma de Pagamento/Dia");
        }
        //FORMA DE PAGAMENTO E EVENTO
        public DataTable VendasPorFormaPagamentoEvento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_FormaPagamentoEvento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "PagamentoNome", "EventoNome", "PagamentoID", "EventoID", "Forma de Pagamento/Evento");
        }

        //****************************Com Loja**********************************

        //LOJA E EVENTO
        public DataTable VendasPorLojaEvento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_LojaEvento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "LojaNome", "EventoNome", "LojaID", "EventoID", "Loja/Evento");
        }

        //LOJA E FORMA DE PAGAMENTO
        public DataTable VendasPorLojaFormaPagamento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_LojaFormaPagamento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "LojaNome", "PagamentoNome", "LojaID", "PagamentoID", "Loja/Forma de Pagamento");
        }

        //LOJA E DIA
        public DataTable VendasPorLojaDia(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_LojaDia(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "LojaNome", "DiaNome", "LojaID", "DataID", "Loja/Dia");
        }

        //****************************Com Dia**********************************

        //DIA E LOJA
        public DataTable VendasPorDiaLoja(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorDiaLoja(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "DiaNome", "LojaNome", "DataID", "LojaID", "Dia/Loja");
        }

        //****************************Com Usuario**********************************

        //USUARIO E FORMA DE PAGAMENTO
        public DataTable VendasPorUsuarioFormaPagamento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_UsuarioFormaPagamento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "UsuarioNome", "PagamentoNome", "UsuarioID", "PagamentoID", "usuario/Forma de Pagamento");
        }

        //USUARIO E EVENTO
        public DataTable VendasPorUsuarioEvento(int perfilTipoID, string dataInicial, string dataFinal, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, int lojaID, int usuarioID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorCanal_UsuarioEvento(perfilTipoID, comCortesia, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, lojaID, usuarioID);
            return this.MontaRelatorioCombinado(sql, "UsuarioNome", "EventoNome", "UsuarioID", "EventoID", "usuario/Evento");
        }

        #endregion


        #region metodos monta SQL sem combinação

        private string MontaSQLVendasPorCanal_Evento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {

            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100), 
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
													SUM(TaxaConvenienciaValor) * PagamentoPct / 100
												ELSE 
													dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
											END 
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
													((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
												ELSE 
													dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
											END 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID, EventoNome, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tIngressoLog.ID, tPreco.Valor, tPreco.ID, tVendaBilheteriaItem.PacoteID
						END
					
					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome
					
					DROP TABLE #Final
					";




            return sql;

        }



        private string MontaSQLVendasPorCanal_FormaPagamento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, tPreco.Valor / 100 * TaxaConveniencia)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
													SUM(TaxaConvenienciaValor) * PagamentoPct / 100
												ELSE 
													dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
											END 
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END        
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
													((TaxaConvenienciaValor) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
												ELSE 
													dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
											END 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID, EventoNome, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END
					
					DROP TABLE #Vendas

					SELECT 
					PagamentoID AS FormaPagamentoID,PagamentoNome AS FormaPagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL				
					ORDER BY PagamentoNome

					DROP TABLE #Final
					";



            return sql;
        }


        private string MontaSQLVendasPorCanal_Loja(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {

            string sql = @"

					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), LojaID INT,LojaNome NVARCHAR(100), 
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 

					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome, 
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,  CanalNome, vendas.LojaID,vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID 
						END

					DROP TABLE #Vendas

					SELECT 
					LojaID,LojaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY LojaNome

					DROP TABLE #Final
					";

            return sql;


        }


        private string MontaSQLVendasPorCanal_Dia(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"CREATE TABLE #final
								(
									EmpresaNome NVARCHAR(100), 
									LocalNome NVARCHAR(100), 
									EventoNome NVARCHAR(100),
									CanalNome NVARCHAR(100),
									LojaNome NVARCHAR(100), 
									DataAbertura NVARCHAR(100),
									PagamentoID INT,
									PagamentoNome NVARCHAR(100), 
									Quantidade  DECIMAL(12,2),
									Valor DECIMAL(12,2), 
									Taxa DECIMAL(12,2), 
                                    Comissao DECIMAL(12,2) 									
								)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;

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
										info.EmpresaNome, info.LocalNome, EventoNome,
										vendas.CanalNome,
										vendas.LojaNome,DataAbertura, 
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
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        														SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                            ELSE
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                                        END 
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE 
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END 
													END
											ELSE	
												CASE COUNT(tIngresso.ID) 
													WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                                SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                             ELSE 
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                                        END 
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE 
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END 
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";
            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND info.RegionalID =  " + regionalID;

            if (localID > 0)
                sql += " AND info.LocalID = " + localID;

            if (!comCortesia)
                sql += " AND tIngressoLog.CortesiaID = 0";

            sql += @" GROUP BY info.EmpresaNome, info.LocalNome, EventoNome,DataAbertura,  CanalNome, vendas.LojaNome,DataAbertura, 
										PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                                        tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
									END

									DROP TABLE #Vendas

									SELECT 
									LEFT(DataAbertura,8) as Data,
									Quantidade AS Total, 
									Valor,
									Taxa, 
                                    Comissao 
									FROM #FINAL
									ORDER BY DataAbertura

									DROP TABLE #Final
									";
            return sql;



        }


        private string MontaSQLVendasPorCanal_Usuario(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),UsuarioID INT,
					UsuarioNome NVARCHAR(100),PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),UsuarioID INT,UsuarioNome NVARCHAR(100)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, tUsuario.ID AS UsuarioID, tUsuario.Nome AS UsuarioNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					INNER JOIN tUsuario (NOLOCK) ON tUsuario.ID = caixas.UsuarioID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
            if (canalID > 0)
                sql += " AND tCanal.ID = " + canalID;
            if (lojaID > 0)
                sql += " AND tLoja.ID = " + lojaID;


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
							info.EmpresaNome, info.LocalNome,vendas.UsuarioID,UsuarioNome,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, vendas.UsuarioID,UsuarioNome, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END
					DROP TABLE #Vendas

					SELECT 
					UsuarioID,UsuarioNome AS Usuario,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY UsuarioNome

					DROP TABLE #Final
					";
            return sql;


        }


        #endregion


        #region metodos monta SQL

        private string MontaSQLVendasPorCanal_EventoSetor(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100),SetorID INT, SetorNome NVARCHAR(100), 
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,tSetor.ID AS SetorID, tSetor.Nome AS SetorNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END                                                 
										ELSE
                                            CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(TaxaConvenienciaValor) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID, EventoNome,tSetor.ID,tSetor.Nome, 
							PagamentoID,PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao, 
                            tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,SetorID,SetorNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome,SetorNome

					DROP TABLE #Final
					";

            return sql;

        }


        private string MontaSQLVendasPorCanal_EventoFormaPagamento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END 
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
											WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID, EventoNome,
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,PagamentoID,PagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome,PagamentoNome

					DROP TABLE #Final
					";
            return sql;

        }

        private string MontaSQLVendasPorCanal_EventoLoja(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100), LojaID INT, LojaNome NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,vendas.LojaID, vendas.LojaNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
        											dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
											WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID, EventoNome,vendas.LojaID,LojaNome,
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,LojaID,LojaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome,LojaNome

					DROP TABLE #Final
					";

            return sql;

        }

        private string MontaSQLVendasPorCanal_EventoDia(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100), DataAbertura NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50), DataAbertura NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,DataAbertura,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        											SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
        											dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END 
										ELSE
                                    		CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE 
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END 
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas (NOLOCK) ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
											WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID, EventoNome,DataAbertura,
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome,DataAbertura

					DROP TABLE #Final
					";
            return sql;

        }

        private string MontaSQLVendasPorCanal_FormaPagamentoDia(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),DataAbertura NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,DataAbertura,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE 
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, 
							PagamentoID, PagamentoNome, DataAbertura, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					PagamentoID,PagamentoNome,LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY PagamentoNome,DataAbertura

					DROP TABLE #Final
					";


            return sql;
        }

        private string MontaSQLVendasPorCanal_FormaPagamentoEvento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),EventoID INT,EventoNome NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, tFormaPagamento.ID AS PagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,info.EventoID,info.EventoNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,info.EventoID,info.EventoNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END
					
					DROP TABLE #Vendas

					SELECT 
					PagamentoID,PagamentoNome,EventoID,EventoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY PagamentoNome,EventoNome

					DROP TABLE #Final
					";


            return sql;
        }

        private string MontaSQLVendasPorCanal_LojaEvento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),EventoID INT,EventoNome NVARCHAR(100),LojaID INT, LojaNome NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,info.EventoID,info.EventoNome,vendas.LojaID,vendas.LojaNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,info.EventoID,info.EventoNome,vendas.LojaID,LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia,  TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID 
						END

					DROP TABLE #Vendas

					SELECT 
					LojaID,LojaNome,EventoID,EventoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY LojaNome,EventoNome

					DROP TABLE #Final
					";
            return sql;

        }

        private string MontaSQLVendasPorCanal_LojaFormaPagamento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),LojaID INT, LojaNome NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,vendas.LojaID,vendas.LojaNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,vendas.LojaID,LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID 
						END

					DROP TABLE #Vendas

					SELECT 
					LojaID,LojaNome,PagamentoID,PagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY LojaNome,PagamentoNome

					DROP TABLE #Final
					";
            return sql;

        }

        private string MontaSQLVendasPorCanal_LojaDia(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),LojaID INT, LojaNome NVARCHAR(100),DataAbertura NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,vendas.LojaID,vendas.LojaNome,DataAbertura,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,vendas.LojaID,LojaNome,DataAbertura,
							PagamentoID,PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID 
						END

					DROP TABLE #Vendas

					SELECT 
					LojaID,LojaNome,LEFT(DataAbertura,8) AS DataID,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY LojaNome,DataAbertura

					DROP TABLE #Final
					";
            return sql;

        }

        private string MontaSQLVendasPorDiaLoja(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),LojaID INT, LojaNome NVARCHAR(100),DataAbertura NVARCHAR(100),
					PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
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
            if (usuarioID > 0)
                sql += " AND caixas.UsuarioID = " + usuarioID;


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
							info.EmpresaNome, info.LocalNome,vendas.LojaID,vendas.LojaNome,DataAbertura,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,vendas.LojaID,LojaNome,DataAbertura,
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					LEFT(DataAbertura,8) AS DataID,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,LojaID,LojaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY DataAbertura,LojaNome

					DROP TABLE #Final
					";
            return sql;


        }

        private string MontaSQLVendasPorCanal_UsuarioFormaPagamento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),UsuarioID INT,
					UsuarioNome NVARCHAR(100),PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),UsuarioID INT ,UsuarioNome NVARCHAR(100)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome,tUsuario.ID AS UsuarioID, tUsuario.Nome AS UsuarioNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					INNER JOIN tUsuario (NOLOCK) ON tUsuario.ID = caixas.UsuarioID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
            if (canalID > 0)
                sql += " AND tCanal.ID = " + canalID;
            if (lojaID > 0)
                sql += " AND tLoja.ID = " + lojaID;


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
							info.EmpresaNome, info.LocalNome,vendas.UsuarioID,UsuarioNome,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,vendas.UsuarioID, UsuarioNome, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					UsuarioID,UsuarioNome,PagamentoID,PagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY UsuarioNome,PagamentoNome

					DROP TABLE #Final
					";

            return sql;

        }

        private string MontaSQLVendasPorCanal_UsuarioEvento(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),UsuarioID INT,
					UsuarioNome NVARCHAR(100),EventoID INT, EventoNome NVARCHAR (100),PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),UsuarioID INT ,UsuarioNome NVARCHAR(100)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome,tUsuario.ID AS UsuarioID, tUsuario.Nome AS UsuarioNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					INNER JOIN tUsuario (NOLOCK) ON tUsuario.ID = caixas.UsuarioID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";
            if (canalID > 0)
                sql += " AND tCanal.ID = " + canalID;
            if (lojaID > 0)
                sql += " AND tLoja.ID = " + lojaID;


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
							info.EmpresaNome, info.LocalNome,vendas.UsuarioID,UsuarioNome,
							info.EventoID AS EventoID,info.EventoNome,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome,info.EventoID,info.EventoNome,vendas.UsuarioID, UsuarioNome, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					UsuarioID,UsuarioNome,EventoID,EventoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY UsuarioNome,EventoNome

					DROP TABLE #Final
					";

            return sql;


        }


        #endregion


        #endregion

        #region Por Evento

        #region Relatórios sem combinação

        public DataTable VendasPorEvento_Evento(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int reginoalID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {

            //verifica se foi selecionado o filtro secundário em tipo de relatório

            if (combinacao != string.Empty)
            {
                if (combinacao == "Canal")
                    return VendasPorEventoCanal(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                if (combinacao == "Forma de Pagamento")
                    return VendasPorEvento_EventoFormaPagamento(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                if (combinacao == "Dia")
                    return VendasPorEvento_EventoDia(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID, combinacao);
            }

            try
            {

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("EventoID", typeof(int));
                dtConsulta.Columns.Add("EventoNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Evento(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);


                DataTable tabela = Utilitario.EstruturaVendasGerenciais();

                bd.Consulta(tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["EventoID"] = bd.LerString("EventoID");
                    novaLinha["EventoNome"] = bd.LerString("EventoNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasEvento = new string[2];

                colunasEvento[0] = "EventoID";
                colunasEvento[1] = "EventoNome";

                DataTable dt = CTLib.TabelaMemoria.DistinctSort(dtConsulta, "EventoNome", "1=1", colunasEvento);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "Totais";  //foi alterado o arredondamento
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "EventoNome = EventoNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "EventoNome = EventoNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "EventoNome = EventoNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "EventoNome = EventoNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = linha["EventoNome"].ToString();
                    // Quantidade e Valor			//foi alterado o arredondamento
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "EventoID = " + linha["EventoID"])))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "EventoID = " + linha["EventoID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND EventoID = " + linha["EventoID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND EventoID = " + linha["EventoID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND EventoID = " + linha["EventoID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND EventoID = " + linha["EventoID"])))));
                    linhaFinal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "EventoID = " + linha["EventoID"]));
                    linhaFinal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "EventoID = " + linha["EventoID"]));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Eventos";
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable VendasPorEvento_Apresentacao(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int regionalID, int localID, int canalID, int empresaID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Setor")
                        return VendasPorEvento_ApresentacaoSetor(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                    if (combinacao == "Forma de Pagamento")
                        return VendasPorEvento_ApresentacaoFormaPagamento(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                    if (combinacao == "Canal")
                        return VendasPorEvento_ApresentacaoCanal(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                    if (combinacao == "Preço")
                        return VendasPorEvento_ApresentacaoPreco(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                    if (combinacao == "Dia")
                        return VendasPorEvento_ApresentacaoDia(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                }


                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("ApresentacaoID", typeof(int));
                dtConsulta.Columns.Add("Horario", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Apresentacao(comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);

                bd.Consulta(tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    novaLinha["Horario"] = bd.LerString("Horario");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasApresentacao = new string[2];

                colunasApresentacao[0] = "ApresentacaoID";
                colunasApresentacao[1] = "Horario";


                DataTable dt = CTLib.TabelaMemoria.DistinctSort(dtConsulta, "Horario", "1=1", colunasApresentacao);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Horario = Horario")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Horario = Horario"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Horario = Horario"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Horario = Horario"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;
                foreach (DataRow linha in dt.Rows)
                {
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + Utilitario.DataHoraBDParaDataHoraLegivel(linha["Horario"].ToString()) + "</div>";
                    // Quantidade e Valor
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "ApresentacaoID = " + linha["ApresentacaoID"])))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "ApresentacaoID = " + linha["ApresentacaoID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND Horario = " + linha["Horario"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND ApresentacaoID = " + linha["ApresentacaoID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND ApresentacaoID = " + linha["ApresentacaoID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND ApresentacaoID = " + linha["ApresentacaoID"])))));
                    linhaFinal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "ApresentacaoID = " + linha["ApresentacaoID"]))));
                    linhaFinal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "ApresentacaoID = " + linha["ApresentacaoID"]))));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Apresentação";

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                bd.Fechar();
            }

            return tabela;

        }


        public DataTable VendasPorEvento_FormaPagamento(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {
                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("FormaPagamentoID", typeof(int));
                dtConsulta.Columns.Add("FormaPagamentoNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLPorEvento_FormaPagamento(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);

                bd.Consulta(tabelaFinal() + tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["FormaPagamentoID"] = bd.LerInt("PagamentoID");
                    novaLinha["FormaPagamentoNome"] = bd.LerString("PagamentoNome");
                    novaLinha["Quantidade"] = bd.LerDecimal("Total");
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasFormaPagamento = new string[2];

                colunasFormaPagamento[0] = "FormaPagamentoID";
                colunasFormaPagamento[1] = "FormaPagamentoNome";


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasFormaPagamento);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "FormaPagamentoNome = FormaPagamentoNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "FormaPagamentoNome = FormaPagamentoNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "FormaPagamentoNome = FormaPagamentoNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "FormaPagamentoNome = FormaPagamentoNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                foreach (DataRow linha in dt.Rows)
                {
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["FormaPagamentoNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["R$ Total"] = VerificaCompute(dtConsulta.Compute("SUM(Valor)", "FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["R$ Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"]))));
                    linhaFinal["R$ Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND FormaPagamentoID = " + linha["FormaPagamentoID"]))));
                    linhaFinal["R$ Conveniência"] = VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["R$ Comissão"] = VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "FormaPagamentoID = " + linha["FormaPagamentoID"]));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Formas de Pagamento";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorEvento_Canal(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            try
            {


                if (combinacao != string.Empty)
                {
                    if (combinacao == "Evento")
                        return VendasPorEvento_CanalEvento(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                    if (combinacao == "Setor")
                        return VendasPorEvento_CanalSetor(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                    if (combinacao == "Forma de Pagamento")
                        return VendasPorEvento_CanalFormaPagamento(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                    if (combinacao == "Preço")
                        return VendasPorEvento_CanalPreco(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                }

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("CanalID", typeof(int));
                dtConsulta.Columns.Add("CanalNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Canal(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);


                DataTable tabela = Utilitario.EstruturaVendasGerenciais();

                bd.Consulta(tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["CanalID"] = bd.LerInt("CanalID");
                    novaLinha["CanalNome"] = bd.LerString("CanalNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasCanal = new string[2];

                colunasCanal[0] = "CanalID";
                colunasCanal[1] = "CanalNome";

                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasCanal);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "CanalNome = CanalNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "CanalNome = CanalNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "CanalNome = CanalNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "CanalNome = CanalNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                foreach (DataRow linha in dt.Rows)
                {
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["CanalNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "CanalID = " + linha["CanalID"])))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "CanalID = " + linha["CanalID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND CanalID = " + linha["CanalID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND CanalID = " + linha["CanalID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND CanalID = " + linha["CanalID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND CanalID = " + linha["CanalID"])))));
                    linhaFinal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "CanalID = " + linha["CanalID"]))));
                    linhaFinal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "CanalID = " + linha["CanalID"]))));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Canais";
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable VendasPorEvento_Loja(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {

                if (combinacao != string.Empty)
                    if (combinacao == "Preço")
                        return VendasPorEvento_LojaPreco(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);


                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("LojaID", typeof(int));
                dtConsulta.Columns.Add("LojaNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Loja(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);

                bd.Consulta(tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["LojaID"] = bd.LerInt("LojaID");
                    novaLinha["LojaNome"] = bd.LerString("LojaNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasLoja = new string[2];

                colunasLoja[0] = "LojaID";
                colunasLoja[1] = "LojaNome";


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasLoja);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "LojaNome = LojaNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "LojaNome = LojaNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "LojaNome = LojaNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "LojaNome = LojaNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                foreach (DataRow linha in dt.Rows)
                {
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["LojaNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "LojaID = '" + linha["LojaID"] + "'")))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "LojaID = " + linha["LojaID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND LojaID = " + linha["LojaID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND LojaID = " + linha["LojaID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND LojaID = " + linha["LojaID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND LojaID = " + linha["LojaID"])))));
                    linhaFinal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "LojaID = " + linha["LojaID"]))));
                    linhaFinal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "LojaID = " + linha["LojaID"]))));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Lojas";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorEvento_Setor(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {

                //verifica se foi selecionado o filtro secundário em tipo de relatório

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Canal")
                        return VendasPorEvento_SetorCanal(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                    if (combinacao == "Preço")
                        return VendasPorEvento_SetorPreco(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                    if (combinacao == "Dia")
                        return VendasPorEvento_SetorDia(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID, combinacao);
                }

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("SetorID", typeof(int));
                dtConsulta.Columns.Add("SetorNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Setor(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);

                bd.Consulta(tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["SetorID"] = bd.LerInt("SetorID");
                    novaLinha["SetorNome"] = bd.LerString("SetorNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasSetor = new string[2];

                colunasSetor[0] = "SetorID";
                colunasSetor[1] = "SetorNome";

                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasSetor);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "SetorNome = SetorNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "SetorNome = SetorNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "SetorNome = SetorNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "SetorNome = SetorNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                foreach (DataRow linha in dt.Rows)
                {
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["SetorNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "SetorID = " + linha["SetorID"])))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "SetorID = " + linha["SetorID"]))));
                    linhaFinal["Qtd Vend"] = VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND SetorID = " + linha["SetorID"]));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND SetorID = " + linha["SetorID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND SetorID = " + linha["SetorID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND SetorID = " + linha["SetorID"])))));
                    linhaFinal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "SetorID = " + linha["SetorID"]))));
                    linhaFinal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "SetorID = " + linha["SetorID"]))));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Setores";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorEvento_Preco(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            DataTable tabela = Utilitario.EstruturaVendasGerenciais();

            try
            {

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("PrecoID", typeof(int));
                dtConsulta.Columns.Add("PrecoNome", typeof(string));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Preco(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);

                bd.Consulta(tabelaVendas() + sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["PrecoID"] = bd.LerInt("PrecoID");
                    novaLinha["PrecoNome"] = Utilitario.DataHoraBDParaDataHoraLegivel(bd.LerString("Horario")) + " - " + bd.LerString("SetorNome") + " - " + bd.LerString("PrecoNome");
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");
                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;

                string[] colunasPreco = new string[2];

                colunasPreco[0] = "PrecoID";
                colunasPreco[1] = "PrecoNome";


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, colunasPreco);

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "PrecoNome = PrecoNome")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "PrecoNome = PrecoNome"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "PrecoNome = PrecoNome"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "PrecoNome = PrecoNome"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["PrecoNome"].ToString() + "</div>";
                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "PrecoID = " + linha["PrecoID"])))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "PrecoID = " + linha["PrecoID"]))));
                    linhaFinal["Qtd Vend"] = Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND PrecoID = " + linha["PrecoID"])));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND PrecoID = " + linha["PrecoID"]))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND PrecoID = " + linha["PrecoID"]))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND PrecoID = " + linha["PrecoID"])))));
                    linhaFinal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "PrecoID = " + linha["PrecoID"]))));
                    linhaFinal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "PrecoID = " + linha["PrecoID"]))));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }


                tabela.Rows.Add(linhaTotal);

                tabela.Columns["VariacaoLinha"].ColumnName = "Setores";

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public DataTable VendasPorEvento_Dia(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            try
            {

                if (combinacao != string.Empty)
                {
                    if (combinacao == "Canal")
                        return VendasPorEvento_DiaCanal(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                    if (combinacao == "Setor")
                        return VendasPorEvento_DiaSetor(dataInicial, dataFinal, comCortesia, apresentacaoID, eventoID, localID, canalID, empresaID, regionalID);
                }

                DataTable dtConsulta = new DataTable("Consulta");
                dtConsulta.Columns.Add("Data", typeof(DateTime));
                dtConsulta.Columns.Add("Quantidade", typeof(decimal));
                dtConsulta.Columns.Add("Valor", typeof(decimal));
                dtConsulta.Columns.Add("Taxa", typeof(decimal));
                dtConsulta.Columns.Add("Comissao", typeof(decimal));

                string sql = this.MontaSQLVendasPorEvento_Dia(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);




                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                tabela.Columns.Add("Data", typeof(DateTime));


                bd.Consulta(sql);

                DataRow novaLinha = null;

                while (bd.Consulta().Read())
                {
                    novaLinha = dtConsulta.NewRow();
                    novaLinha["Data"] = Convert.ToDateTime(Utilitario.DataHoraBDParaDataHoraLegivel(bd.LerString("Data") + "000000"));
                    novaLinha["Quantidade"] = Math.Round(bd.LerDecimal("Total"), 2);
                    novaLinha["Valor"] = bd.LerDecimal("Valor");
                    novaLinha["Taxa"] = bd.LerDecimal("Taxa");
                    novaLinha["Comissao"] = bd.LerDecimal("Comissao");

                    dtConsulta.Rows.Add(novaLinha);
                }

                bd.Fechar();

                if (dtConsulta.Rows.Count == 0)
                    return tabela;


                DataTable dt = CTLib.TabelaMemoria.Distinct(dtConsulta, "Data");

                // Insere a linha de totais.
                DataRow linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Data = Data")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Data = Data"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0")))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Data = Data"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Data = Data"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                DataRow linhaFinal = null;

                DataRow linha;
                //foreach(DataRow linha in dt.Rows)
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    linha = dt.Rows[i];
                    // Cria a linha na table final.
                    linhaFinal = tabela.NewRow();

                    linhaFinal["VariacaoLinhaID"] = 0;
                    linhaFinal["VariacaoLinha"] = "<div style='text-align:left'>" + linha["Data"].ToString() + "</div>";
                    linhaFinal["Data"] = Convert.ToDateTime(linha["Data"]);

                    // Quantidade e Valor
                    linhaFinal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Data = '" + linha["Data"] + "'")))));
                    linhaFinal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Data = '" + linha["Data"] + "'"))));
                    linhaFinal["Qtd Vend"] = Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0 AND Data = '" + linha["Data"] + "'"));
                    linhaFinal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0 AND Data = '" + linha["Data"] + "'")))))));
                    linhaFinal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0 AND Data = '" + linha["Data"] + "'"))));
                    linhaFinal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0 AND Data = '" + linha["Data"] + "'")))));
                    linhaFinal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Data = '" + linha["Data"] + "'"))));
                    linhaFinal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Data = '" + linha["Data"] + "'"))));
                    linhaFinal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaFinal["R$ Total"]) + Convert.ToDecimal(linhaFinal["R$ Conveniência"]));

                    tabela.Rows.Add(linhaFinal);
                }

                // Insere a linha de totais.
                linhaTotal = tabela.NewRow();
                linhaTotal["VariacaoLinhaID"] = 0;
                linhaTotal["VariacaoLinha"] = "<div style='text-align:left'>Totais</div>";
                linhaTotal["Qtd Total"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Data = Data")))));
                linhaTotal["R$ Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Data = Data"))));
                linhaTotal["Qtd Vend"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade >= 0"))))))));
                linhaTotal["R$ Vend"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor >= 0"))));
                linhaTotal["Qtd Canc"] = Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Quantidade)", "Quantidade < 0"))));
                linhaTotal["R$ Canc"] = Utilitario.AplicaFormatoMoeda(Math.Abs(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Valor)", "Valor < 0")))));
                linhaTotal["R$ Conveniência"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Taxa)", "Data = Data"))));
                linhaTotal["R$ Comissão"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(Utilitario.VerificaCompute(dtConsulta.Compute("SUM(Comissao)", "Data = Data"))));
                linhaTotal["Total"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linhaTotal["R$ Total"]) + Convert.ToDecimal(linhaTotal["R$ Conveniência"]));

                tabela.Rows.Add(linhaTotal);


                tabela.Columns.RemoveAt(tabela.Columns.Count - 1);

                tabela.Columns["VariacaoLinha"].ColumnName = "Dias";
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region combinações de relatório

        //****************************Com Evento**********************************

        //EVENTO E CANAL
        public DataTable VendasPorEventoCanal(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEventoCanal(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "CanalNome", "EventoID", "CanalID", "Evento/Canal");
        }
        //EVENTO E FORMA DE PAGAMENTO
        public DataTable VendasPorEvento_EventoFormaPagamento(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_EventoFormaPagamento(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "PagamentoNome", "EventoID", "PagamentoID", "Evento/Forma de Pagamento");
        }
        //EVENTO E DIA
        public DataTable VendasPorEvento_EventoDia(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_EventoDia(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "EventoNome", "DiaNome", "EventoID", "DataID", "Evento/Dia");
        }

        //****************************Com Apresentação**********************************

        //APRESENTACAO E SETOR
        public DataTable VendasPorEvento_ApresentacaoSetor(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_ApresentacaoSetor(comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "ApresentacaoNome", "SetorNome", "ApresentacaoID", "SetorID", "Apresentacão/Setor");
        }
        //APRESENTACAO E FORMA DE PAGAMENTO
        public DataTable VendasPorEvento_ApresentacaoFormaPagamento(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_ApresentacaoFormaPagamento(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID);
            return this.MontaRelatorioCombinado(sql, "ApresentacaoNome", "PagamentoNome", "ApresentacaoID", "PagamentoID", "Apresentacão/Forma de Pagamento");
        }
        //APRESENTACAO E CANAL
        public DataTable VendasPorEvento_ApresentacaoCanal(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_ApresentacaoCanal(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID);
            return this.MontaRelatorioCombinado(sql, "ApresentacaoNome", "CanalNome", "ApresentacaoID", "CanalID", "Apresentacão/Canal");
        }
        //APRESENTACAO E PRECO
        public DataTable VendasPorEvento_ApresentacaoPreco(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_ApresentacaoPreco(comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "ApresentacaoNome", "PrecoNome", "ApresentacaoID", "PrecoID", "Apresentacão/Preco");
        }

        //APRESENTACAO E DIA
        public DataTable VendasPorEvento_ApresentacaoDia(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_ApresentacaoDia(dataInicial, dataFinal, comCortesia, eventoID, localID, canalID, empresaID, regionalID);
            return this.MontaRelatorioCombinado(sql, "ApresentacaoNome", "DiaNome", "ApresentacaoID", "DataID", "Apresentacão/Dia");
        }

        //****************************Com Setor**********************************

        //SETOR E CANAL
        public DataTable VendasPorEvento_SetorCanal(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_SetorCanal(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "SetorNome", "CanalNome", "SetorID", "CanalID", "Setor/Canal");
        }
        //SETOR E PREÇO
        public DataTable VendasPorEvento_SetorPreco(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_SetorPreco(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "SetorNome", "PrecoNome", "SetorID", "PrecoID", "Setor/Preço");
        }
        //SETOR E DIA
        public DataTable VendasPorEvento_SetorDia(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string combinacao)
        {
            string sql = this.MontaSQLVendasPorEvento_SetorDia(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "SetorNome", "DiaNome", "SetorID", "DataID", "Setor/Dia");
        }

        //****************************Com Canal**********************************

        //CANAL E EVENTO
        public DataTable VendasPorEvento_CanalEvento(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_CanalEvento(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "CanalNome", "EventoNome", "CanalID", "EventoID", "Canal/Evento");
        }
        //CANAL E SETOR
        public DataTable VendasPorEvento_CanalSetor(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_CanalSetor(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "CanalNome", "SetorNome", "CanalID", "SetorID", "Canal/Setor");
        }

        //CANAL E FORMA DE PAGAMENTO
        public DataTable VendasPorEvento_CanalFormaPagamento(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_CanalFormaPagamento(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "CanalNome", "PagamentoNome", "CanalID", "PagamentoID", "Canal/Forma de Pagamento");
        }
        //CANAL E PREÇO
        public DataTable VendasPorEvento_CanalPreco(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_CanalPreco(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "CanalNome", "PrecoNome", "CanalID", "PrecoID", "Canal/Preço");
        }

        //****************************Com Loja**********************************

        //LOJA E PRECO
        public DataTable VendasPorEvento_LojaPreco(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_LojaPreco(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "LojaNome", "PrecoNome", "LojaID", "PrecoID", "Loja/Preco");
        }

        //****************************Com Dia**********************************

        //DIA E CANAL
        public DataTable VendasPorEvento_DiaCanal(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_DiaCanal(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "DiaNome", "CanalNome", "DataID", "CanalID", "Dia/Canal");
        }
        //DIA E SETOR
        public DataTable VendasPorEvento_DiaSetor(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = this.MontaSQLVendasPorEvento_DiaSetor(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal);
            return this.MontaRelatorioCombinado(sql, "DiaNome", "SetorNome", "DataID", "SetorID", "Dia/Setor");
        }






        #endregion


        #region metodos monta SQL sem combinação

        private string MontaSQLVendasPorEvento_Evento(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            string sql = @"



					CREATE TABLE #final
					(
						EmpresaNome NVARCHAR(100), 
						LocalNome NVARCHAR(100),
						EventoID INT, 
						EventoNome NVARCHAR(100), 
						CanalNome NVARCHAR(100),
						LojaNome NVARCHAR(100), 
						PagamentoID INT,
						PagamentoNome NVARCHAR(100), 
						Quantidade  DECIMAL(12,2),
						Valor DECIMAL(12,2), 
						Taxa DECIMAL(12,2), 
                        Comissao DECIMAL(12,2) 
					)


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,
							vendas.CanalNome,
							vendas.LojaNome, 
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN                 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN                 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END 
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN                 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN                 
                                                    (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas (NOLOCK) ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";


            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @"
                        GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,  CanalNome, vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID 
						END
					
					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome

					DROP TABLE #Final
					";

            return sql;


        }


        private string MontaSQLVendasPorEvento_Apresentacao(bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
				
				CREATE TABLE #final
				(
					EmpresaNome NVARCHAR(100), 
					LocalNome NVARCHAR(100), 
					EventoNome NVARCHAR(100), 
					ApresentacaoID INT,
					Horario NVARCHAR(100),
					CanalNome NVARCHAR(100),
					LojaNome NVARCHAR(100), 
					PagamentoID INT,
					PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),
					Valor DECIMAL(12,2), 
					Taxa DECIMAL(12,2), 
                    Comissao DECIMAL(12,2) 
				)
		
					INSERT INTO #Vendas 
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
							info.EmpresaNome, info.LocalNome, EventoNome,tApresentacao.ID,Horario,
							vendas.CanalNome,
							vendas.LojaNome, 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                        GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,tApresentacao.ID,Horario, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					ApresentacaoID,Horario,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY Horario

					DROP TABLE #Final
					";

            return sql;


        }


        private string MontaSQLPorEvento_FormaPagamento(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
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
							info.EmpresaNome, info.LocalNome, EventoNome,
							vendas.CanalNome,
							vendas.LojaNome, 
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas (NOLOCK) ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                        GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,  CanalNome, vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					PagamentoID,PagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY PagamentoNome

					DROP TABLE #Final
					";

            return sql;
        }


        private string MontaSQLVendasPorEvento_Canal(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"

					CREATE TABLE #final
					(
						EmpresaNome NVARCHAR(100), 
						LocalNome NVARCHAR(100),
						EventoNome NVARCHAR(100), 
						CanalID INT,
						CanalNome NVARCHAR(100),
						LojaNome NVARCHAR(100), 
						PagamentoID INT,
						PagamentoNome NVARCHAR(100), 
						Quantidade  DECIMAL(12,2),
						Valor DECIMAL(12,2), 
						Taxa DECIMAL(12,2), 
                        Comissao DECIMAL(12,2) 
					)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome, EventoNome,
							vendas.CanalID,vendas.CanalNome,
							vendas.LojaNome, 
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        											(TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
        											dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, (tPreco.Valor / 100 * TaxaConveniencia * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";


            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,  vendas.CanalID,CanalNome, vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					CanalID,CanalNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY CanalNome 
					DROP TABLE #Final
					";

            return sql;

        }

        private string MontaSQLVendasPorEvento_Loja(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"


					CREATE TABLE #final
					(
						EmpresaNome NVARCHAR(100), 
						LocalNome NVARCHAR(100),
						EventoNome NVARCHAR(100), 
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
					
					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome, EventoNome,
							vendas.CanalNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,  CanalNome,vendas.LojaID,vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					LojaID,LojaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY LojaNome

					DROP TABLE #Final
					";

            return sql;

        }


        private string MontaSQLVendasPorEvento_Setor(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
					
					CREATE TABLE #final
					(
						EmpresaNome NVARCHAR(100), 
						LocalNome NVARCHAR(100), 
						EventoNome NVARCHAR(100), 
						SetorID INT,
						SetorNome NVARCHAR(100),
						CanalNome NVARCHAR(100),
						LojaNome NVARCHAR(100), 
						PagamentoID INT,
						PagamentoNome NVARCHAR(100), 
						Quantidade  DECIMAL(12,2),
						Valor DECIMAL(12,2), 
						Taxa DECIMAL(12,2), 
                        Comissao DECIMAL(12,2) 
					)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";

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
							info.EmpresaNome, info.LocalNome, EventoNome,tSetor.ID AS SetorID,tSetor.Nome AS SetorNome,
							vendas.CanalNome,
							vendas.LojaNome, 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,tSetor.ID,tSetor.Nome,  CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					SetorID,SetorNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY SetorNome

					DROP TABLE #Final
					";

            return sql;


        }


        private string MontaSQLVendasPorEvento_Preco(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            string sql = @"
					
					CREATE TABLE #final
					(
						EmpresaNome NVARCHAR(100), 
						LocalNome NVARCHAR(100), 
						EventoNome NVARCHAR(100),
						Horario NVARCHAR(100),
						SetorNome NVARCHAR(100),
						PrecoID INT,
						PrecoNome NVARCHAR(100),
						CanalNome NVARCHAR(100),
						LojaNome NVARCHAR(100), 
						PagamentoID INT,
						PagamentoNome NVARCHAR(100), 
						Quantidade  DECIMAL(12,2),
						Valor DECIMAL(12,2), 
						Taxa DECIMAL(12,2), 
                        Comissao DECIMAL(12,2) 
					)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";

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
							info.EmpresaNome, info.LocalNome, EventoNome,Horario,tSetor.Nome AS SetorNome,tPreco.ID AS PrecoID,tPreco.Nome AS PrecoNome,
							vendas.CanalNome,
							vendas.LojaNome, 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND tIngressoLog.CortesiaID = 0";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,Horario,tSetor.Nome,tPreco.ID,tPreco.Nome, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					Horario,SetorNome,PrecoID,PrecoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY PrecoNome

					DROP TABLE #Final
					";

            return sql;


        }


        private string MontaSQLVendasPorEvento_Dia(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {


            string sql = @"CREATE TABLE #final
								(
									EmpresaNome NVARCHAR(100), 
									LocalNome NVARCHAR(100), 
									EventoNome NVARCHAR(100),
									CanalNome NVARCHAR(100),
									LojaNome NVARCHAR(100), 
									DataAbertura NVARCHAR(100),
									PagamentoID INT,
									PagamentoNome NVARCHAR(100), 
									Quantidade  DECIMAL(12,2),
									Valor DECIMAL(12,2), 
									Taxa DECIMAL(12,2), 
                                    Comissao DECIMAL(12,2) 									
								)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome, EventoNome,
										vendas.CanalNome,
										vendas.LojaNome,DataAbertura, 
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
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        														SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                            ELSE
        														dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
													END
											ELSE	
												CASE COUNT(tIngresso.ID) 
													WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas (NOLOCK) ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";


            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                                    GROUP BY info.EmpresaNome, info.LocalNome, EventoNome,DataAbertura,  CanalNome, vendas.LojaNome,DataAbertura, 
										PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                                        tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
									END

									DROP TABLE #Vendas

									SELECT 
									LEFT(DataAbertura,8) as Data,
									Quantidade AS Total, 
									Valor,
									Taxa, 
                                    Comissao 
									FROM #FINAL
									ORDER BY DataAbertura

									DROP TABLE #Final
									";

            return sql;
        }



        #endregion

        #region metodos monta SQL

        private string MontaSQLVendasPorEventoCanal(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            string sql = @"


					CREATE TABLE #final
					(
					EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100), 
					CanalID INT,CanalNome NVARCHAR(100),LojaNome NVARCHAR(100), PagamentoID INT,PagamentoNome NVARCHAR(100), 
					Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					) 
					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), 
					PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,
							vendas.CanalID,vendas.CanalNome,
							vendas.LojaNome, 
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
        											(TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
        											dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END 
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas (NOLOCK) ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";


            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome, vendas.CanalID,CanalNome, vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome, CanalID,CanalNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome, CanalNome

					DROP TABLE #Final
					";
            return sql;

        }


        private string MontaSQLVendasPorEvento_EventoFormaPagamento(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"


					CREATE TABLE #final
					(
						EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT,EventoNome NVARCHAR(100), PagamentoID INT,
						PagamentoNome NVARCHAR(100),Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
					)

					CREATE TABLE #Vendas
					(	
					VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), 
					PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50)
					) 
					
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID) 

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome,info.EventoID, EventoNome, 
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE 
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END 
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE 
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";


            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome,  CanalNome, vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID 
						END
					
					DROP TABLE #Vendas

					SELECT 
					EventoID,EventoNome,PagamentoID,PagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY EventoNome,PagamentoNome

					DROP TABLE #Final
					";


            return sql;
        }


        private string MontaSQLVendasPorEvento_EventoDia(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {


            string sql = @"CREATE TABLE #final
								(
									EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),EventoID INT, EventoNome NVARCHAR(100),CanalNome NVARCHAR(100),LojaNome NVARCHAR(100), 
									DataAbertura NVARCHAR(100),PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
									Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
								)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome,info.EventoID, EventoNome,
										vendas.CanalNome,
										vendas.LojaNome,DataAbertura, 
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
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE 
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
													END
											ELSE	
												CASE COUNT(tIngresso.ID) 
													WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                                SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                            ELSE 
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END 
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem  (NOLOCK)ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";


            sql += @" 
                                    GROUP BY info.EmpresaNome, info.LocalNome,info.EventoID, EventoNome,DataAbertura,  CanalNome, vendas.LojaNome,DataAbertura, 
										PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                                        tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
									END

									DROP TABLE #Vendas

									SELECT 
									EventoID,EventoNome,LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,
									Quantidade AS Total, 
									Valor,
									Taxa, 
                                    Comissao 
									FROM #FINAL
									ORDER BY EventoNome,DataAbertura

									DROP TABLE #Final
									";
            return sql;
        }


        private string MontaSQLVendasPorEvento_ApresentacaoSetor(bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),ApresentacaoID INT, Horario NVARCHAR(100),SetorID INT,SetorNome NVARCHAR(100), 
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome 
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,tSetor.ID AS SetorID,tSetor.Nome AS SetorNome, 
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY info.EmpresaNome, info.LocalNome,tApresentacao.ID,tApresentacao.Horario,tSetor.ID,tSetor.Nome, CanalNome, vendas.LojaNome,
										PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
									END

									DROP TABLE #Vendas

									SELECT 
									ApresentacaoID,SUBSTRING(Horario, 7,2) + '/'+ SUBSTRING(Horario, 5,2) +'/'+ SUBSTRING(Horario, 1,4) +' '+ SUBSTRING(Horario, 9,2) +':'+ SUBSTRING(Horario, 11,2)  AS ApresentacaoNome,SetorID,SetorNome,
									Quantidade AS Total, 
									Valor,
									Taxa, 
                                    Comissao 
									FROM #FINAL
									ORDER BY Horario,SetorNome
									
									DROP TABLE #Final
									";
            return sql;


        }


        private string MontaSQLVendasPorEvento_ApresentacaoFormaPagamento(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {

            //tApresentacao.ID AS ApresentacaoID,SUBSTRING(Horario, 7,2) + '/'+ SUBSTRING(Horario, 5,2) +'/'+ SUBSTRING(Horario, 1,4) +' '+ SUBSTRING(Horario, 9,2) +':'+ SUBSTRING(Horario, 11,2)  AS ApresentacaoNome,


            string sql = @"CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),ApresentacaoID INT, Horario NVARCHAR(100), 
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome 
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                                        GROUP BY info.EmpresaNome, info.LocalNome,tApresentacao.ID,tApresentacao.Horario,CanalNome, vendas.LojaNome,
												PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
										END

										DROP TABLE #Vendas

										SELECT 
										ApresentacaoID,SUBSTRING(Horario, 7,2) + '/'+ SUBSTRING(Horario, 5,2) +'/'+ SUBSTRING(Horario, 1,4) +' '+ SUBSTRING(Horario, 9,2) +':'+ SUBSTRING(Horario, 11,2)  AS ApresentacaoNome,PagamentoID,PagamentoNome,
										Quantidade AS Total, 
										Valor,
										Taxa, 
                                        Comissao 
										FROM #FINAL
										ORDER BY Horario,PagamentoNome

										DROP TABLE #Final
									";


            return sql;
        }


        private string MontaSQLVendasPorEvento_ApresentacaoCanal(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = @"CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),ApresentacaoID INT, Horario NVARCHAR(100),CanalID INT, CanalNome NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome 
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";

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
										info.EmpresaNome, info.LocalNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,
										vendas.CanalID,Vendas.CanalNome,PagamentoID,
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND tIngressoLog.CortesiaID = 0";

            sql += @" 
                                        GROUP BY info.EmpresaNome, info.LocalNome,tApresentacao.ID,tApresentacao.Horario,vendas.CanalID,CanalNome,vendas.LojaID, vendas.LojaNome,
												PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
										END

										DROP TABLE #Vendas

										SELECT 
										ApresentacaoID,SUBSTRING(Horario, 7,2) + '/'+ SUBSTRING(Horario, 5,2) +'/'+ SUBSTRING(Horario, 1,4) +' '+ SUBSTRING(Horario, 9,2) +':'+ SUBSTRING(Horario, 11,2)  AS ApresentacaoNome,CanalID,CanalNome,
										Quantidade AS Total, 
										Valor,
										Taxa, 
                                        Comissao 
										FROM #FINAL
										ORDER BY Horario,CanalNome

										DROP TABLE #Final
									";
            return sql;

        }

        private string MontaSQLVendasPorEvento_ApresentacaoPreco(bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),ApresentacaoID INT, Horario NVARCHAR(100),PrecoID INT, PrecoNome NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome 
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,
										tPreco.ID AS PrecoID,tPreco.Nome,PagamentoID,
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY info.EmpresaNome, info.LocalNome,tApresentacao.ID,tApresentacao.Horario,tpreco.ID,tPreco.Nome,vendas.CanalID,CanalNome,vendas.LojaID, vendas.LojaNome,
												PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
										END

										DROP TABLE #Vendas

										SELECT 
										ApresentacaoID,SUBSTRING(Horario, 7,2) + '/'+ SUBSTRING(Horario, 5,2) +'/'+ SUBSTRING(Horario, 1,4) +' '+ SUBSTRING(Horario, 9,2) +':'+ SUBSTRING(Horario, 11,2)  AS ApresentacaoNome,PrecoID,PrecoNome,
										Quantidade AS Total, 
										Valor,
										Taxa, 
                                        Comissao 
										FROM #FINAL
										ORDER BY Horario,PrecoNome

										DROP TABLE #Final
									";
            return sql;


        }


        private string MontaSQLVendasPorEvento_ApresentacaoDia(string dataInicial, string dataFinal, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID)
        {
            string sql = @"CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),ApresentacaoID INT, Horario NVARCHAR(100),DataAbertura NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,
										DataAbertura,PagamentoID,
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                                        GROUP BY info.EmpresaNome, info.LocalNome,tApresentacao.ID,tApresentacao.Horario,DataAbertura,vendas.CanalID,CanalNome,vendas.LojaID, vendas.LojaNome,
												PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
										END

										DROP TABLE #Vendas

										SELECT 
										ApresentacaoID,SUBSTRING(Horario, 7,2) + '/'+ SUBSTRING(Horario, 5,2) +'/'+ SUBSTRING(Horario, 1,4) +' '+ SUBSTRING(Horario, 9,2) +':'+ SUBSTRING(Horario, 11,2)  AS ApresentacaoNome,
										LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,
										Quantidade AS Total, 
										Valor,
										Taxa, 
                                        Comissao 
										FROM #FINAL
										ORDER BY Horario,DataAbertura

										DROP TABLE #Final
									";
            return sql;

        }


        private string MontaSQLVendasPorEvento_SetorCanal(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
					
					CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), SetorID INT, SetorNome NVARCHAR(100), CanalID INT, CanalNome NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
			
					CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome 
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";

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
							info.EmpresaNome, info.LocalNome,tSetor.ID AS SetorID,tSetor.Nome AS SetorNome,
							vendas.CanalID AS CanalID,vendas.CanalNome,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                        GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,tSetor.ID,tSetor.Nome, vendas.CanalID, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					SetorID,SetorNome,CanalID,CanalNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY SetorNome,CanalNome

					DROP TABLE #Final
					";

            return sql;


        }


        private string MontaSQLVendasPorEvento_SetorPreco(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
					
					CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), SetorID INT, SetorNome NVARCHAR(100), PrecoID INT, PrecoNome NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
                    
                    CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)			

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";

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
							info.EmpresaNome, info.LocalNome,tSetor.ID AS SetorID, tSetor.Nome AS SetorNome,
							tPreco.ID AS PrecoID, tPreco.Nome AS PrecoNome,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,tSetor.ID,tSetor.Nome,tPreco.ID, tPreco.Nome,  CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao,PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					SetorID,SetorNome,PrecoID,PrecoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY SetorNome,PrecoNome

					DROP TABLE #Final
					";

            return sql;

        }


        private string MontaSQLVendasPorEvento_SetorDia(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {


            string sql = @"
					
					CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), SetorID INT, SetorNome NVARCHAR(100), DataAbertura NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
                    CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
			
					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";

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
							info.EmpresaNome, info.LocalNome,tSetor.ID AS SetorID, tSetor.Nome AS SetorNome,
							DataAbertura,
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                        GROUP BY 
							info.EmpresaNome, info.LocalNome, EventoNome,tSetor.ID,tSetor.Nome,DataAbertura, CanalNome, vendas.LojaNome, 
							PagamentoID,PagamentoNome, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,tIngressoLog.acao
						END

					DROP TABLE #Vendas

					SELECT 
					SetorID,SetorNome,LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY SetorNome,DataAbertura

					DROP TABLE #Final

					";
            return sql;
        }

        private string MontaSQLVendasPorEvento_CanalEvento(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            string sql = @"
					
				CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100), EventoID INT, EventoNome NVARCHAR(100), CanalID INT, CanalNome NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
		


					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome,info.EventoID, EventoNome,
							vendas.CanalID,vendas.CanalNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, info.EventoID,EventoNome, vendas.CanalID, CanalNome, vendas.LojaNome, 
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas


					SELECT 
					CanalID,CanalNome,EventoID,EventoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY CanalNome,EventoNome

					DROP TABLE #Final";
            return sql;


        }


        private string MontaSQLVendasPorEvento_CanalSetor(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
					
				CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),CanalID INT, CanalNome NVARCHAR(100),SetorID INT, SetorNome NVARCHAR(100), 
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
		
                    CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome,
							vendas.CanalID,vendas.CanalNome,tSetor.ID AS SetorID, tSetor.Nome AS SetorNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";


            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, vendas.CanalID, CanalNome, tSetor.ID, tSetor.Nome,  
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID, 
                            tIngressoLog.acao,  tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas


					SELECT 
					CanalID,CanalNome,SetorID,SetorNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY CanalNome,SetorNome
					
					DROP TABLE #Final

				";

            return sql;


        }


        private string MontaSQLVendasPorEvento_CanalFormaPagamento(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
					
				CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),CanalID INT, CanalNome NVARCHAR(100), 
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
		
                    CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome,
							vendas.CanalID,vendas.CanalNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                    SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
                                                    dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" GROUP BY 
							info.EmpresaNome, info.LocalNome, vendas.CanalID, CanalNome,  
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas


					SELECT 
					CanalID,CanalNome,PagamentoID, PagamentoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY CanalNome,PagamentoNome

					DROP TABLE #Final
					";

            return sql;
        }


        private string MontaSQLVendasPorEvento_CanalPreco(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {
            string sql = @"
					
				CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),CanalID INT, CanalNome NVARCHAR(100),PrecoID INT, PrecoNome NVARCHAR(100), 
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


					CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
		
                    CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)

					INSERT INTO #Vendas 
					SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal), 
 					tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome
					FROM tVendaBilheteria (NOLOCK)
					INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
					INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
					INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
					LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
					LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
					WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
							info.EmpresaNome, info.LocalNome,
							vendas.CanalID,vendas.CanalNome,tPreco.ID AS PrecoID, tPreco.Nome AS PrecoNome,
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
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
										END
								ELSE	
									CASE COUNT(tIngresso.ID) 
										WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                            END
										ELSE
											CASE 
												WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                ELSE
											        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                            END
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
							FROM tIngressoLog (NOLOCK)
							INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
							INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
							INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
							INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
							INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
							WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID ";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                        GROUP BY 
							info.EmpresaNome, info.LocalNome, vendas.CanalID, CanalNome, tPreco.ID, tPreco.Nome,  
							PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                            tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
						END

					DROP TABLE #Vendas


					SELECT 
					CanalID,CanalNome,PrecoID,PrecoNome,
					Quantidade AS Total, 
					Valor,
					Taxa, 
                    Comissao 
					FROM #FINAL
					ORDER BY CanalNome,PrecoNome

					DROP TABLE #Final
					";
            return sql;

        }


        private string MontaSQLVendasPorEvento_DiaCanal(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {


            string sql = @"CREATE TABLE #final
								(
								EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),DataAbertura NVARCHAR(100),CanalID INT, CanalNome NVARCHAR(100),
								PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
								Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
								)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
		
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome, DataAbertura,
										vendas.CanalID,vendas.CanalNome,
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
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
													        	SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                            ELSE
													        	dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
													END
											ELSE	
												CASE COUNT(tIngresso.ID) 
													WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                                SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                            ELSE
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";


            sql += @" 
                                    GROUP BY info.EmpresaNome, info.LocalNome,DataAbertura,vendas.CanalID, CanalNome,
										PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                                        tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
									END

									DROP TABLE #Vendas


									SELECT 
									LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,CanalID,CanalNome,
									Quantidade AS Total, 
									Valor,
									Taxa, 
                                    Comissao 
									FROM #FINAL
									ORDER BY DataAbertura, CanalNome

									DROP TABLE #Final
									";
            return sql;
        }


        private string MontaSQLVendasPorEvento_DiaSetor(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {


            string sql = @"CREATE TABLE #final
								(
								EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),DataAbertura NVARCHAR(100),SetorID INT, SetorNome NVARCHAR(100),
								PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
								Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
								)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50),DataAbertura NVARCHAR(50))
		
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome, DataAbertura
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome, DataAbertura,
										tSetor.ID AS SetorID,tSetor.Nome AS SetorNome,
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
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                                        END 
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
													END
											ELSE	
												CASE COUNT(tIngresso.ID) 
													WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.setorID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                                    GROUP BY info.EmpresaNome, info.LocalNome,DataAbertura,tSetor.ID, tSetor.Nome,
										PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao ,PagamentoPCT, tIngressoLog.CortesiaID, 
                                        tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
									END
									
									DROP TABLE #Vendas

									SELECT 
									LEFT(DataAbertura,8) AS DataID ,SUBSTRING(DataAbertura, 7,2) + '/'+ SUBSTRING(DataAbertura, 5,2) +'/'+ SUBSTRING(DataAbertura, 1,4) AS DiaNome,SetorID,SetorNome,
									Quantidade AS Total, 
									Valor,
									Taxa, 
                                    Comissao 
									FROM #FINAL
									ORDER BY DataAbertura, SetorNome

									DROP TABLE #Final
									";
            return sql;
        }

        private string MontaSQLVendasPorEvento_LojaPreco(int apresentacaoID, bool comCortesia, int eventoID, int localID, int canalID, int empresaID, int regionalID, string dataInicial, string dataFinal)
        {

            string sql = @"CREATE TABLE #final
							(
							EmpresaNome NVARCHAR(100), LocalNome NVARCHAR(100),LojaID INT, LojaNome NVARCHAR(100),PrecoID INT, PrecoNome NVARCHAR(100),
							PagamentoID INT,PagamentoNome NVARCHAR(100), Quantidade  DECIMAL(12,2),Valor DECIMAL(12,2), 
							Taxa DECIMAL(12,2), Comissao DECIMAL(12,2)
							)


								CREATE TABLE #Vendas(VendaBilheteriaID INT, TaxaConvenienciaValorTotal DECIMAL(12,2), ValorTotal DECIMAL(12,2), ComissaoTotal DECIMAL(12,2), PagamentoID INT, PagamentoNome NVARCHAR(50), PagamentoValor DECIMAL(12,2), PagamentoPct float, CanalID INT, CanalNome NVARCHAR(50), LojaID INT,LojaNome NVARCHAR(50))
			
								CREATE CLUSTERED INDEX IX_relVEndas ON #Vendas (VendaBilheteriaID)
				
								INSERT INTO #Vendas 
								SELECT tVendaBilheteria.ID AS VendaBilheteriaID , TaxaConvenienciaValorTotal, ValorTotal, ComissaoValorTotal, FormaPagamentoID, tFormaPagamento.Nome AS PagamentoNome, tVendaBilheteriaFormaPagamento.Valor, dbo.Dividir(tVendaBilheteriaFormaPagamento.Valor * 100, ValorTotal),
 								tLoja.CanalID, tCanal.Nome AS CanalNome, caixas.LojaID, tLoja.Nome AS LojaNome 
								FROM tVendaBilheteria (NOLOCK)
								INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
								INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
								INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
								LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
								LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
								WHERE DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal + "'";


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
										info.EmpresaNome, info.LocalNome,vendas.LojaID,vendas.LojaNome,
										tPreco.ID AS PrecoID,tPreco.Nome,PagamentoID,
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
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
											        			SUM(TaxaConvenienciaValor) * PagamentoPct / 100
                                                            ELSE
											        			dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
                                                                (TaxaConvenienciaValor * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
                                                                dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / 100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
													END
											ELSE	
												CASE COUNT(tIngresso.ID) 
													WHEN COUNT(DISTINCT tVendaBilheteriaItem.ID) THEN 
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        SUM(TaxaConvenienciaValor) * PagamentoPct / -100
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, SUM(tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100)
                                                        END
													ELSE
											            CASE 
												            WHEN tVendaBilheteriaItem.PacoteID = 0 THEN 
														        (TaxaConvenienciaValor * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID)
                                                            ELSE
														        dbo.CalculaTaxaConvenienciaPacote (tIngressoLog.Acao, @CanalID, tVendaBilheteriaItem.PacoteID, ((tPreco.Valor / 100 * TaxaConveniencia) * PagamentoPct / -100) * COUNT(DISTINCT tVendaBilheteriaItem.ID))
                                                        END
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
										FROM tIngressoLog (NOLOCK)
										INNER JOIN #Vendas vendas ON vendas.VendaBilheteriaID = tIngressoLog.VendaBilheteriaID AND vendas.CanalID = tIngressoLog.CanalID
										INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
										INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = IngressoID
										INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
										INNER JOIN vwInfoEvento info ON tIngresso.EventoID = info.EventoID
										INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngressoLog.PrecoID
										WHERE tIngressoLog.Acao IN ('C','V') AND tIngressoLog.CanalID = @CanalID";

            if (empresaID > 0)
                sql += " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";

            if (regionalID > 0)
                sql += @" AND (info.RegionalID =  " + regionalID + ")";

            if (localID > 0)
                sql += " AND (info.LocalID = " + localID + ")";

            if (eventoID > 0)
                sql += " AND (info.EventoID = " + eventoID + ")";

            if (apresentacaoID > 0)
                sql += " AND (tIngresso.ApresentacaoID = " + apresentacaoID + ")";

            if (!comCortesia)
                sql += " AND (tIngressoLog.CortesiaID = 0)";

            sql += @" 
                                        GROUP BY info.EmpresaNome, info.LocalNome,vendas.LojaID, vendas.LojaNome,tpreco.ID,tPreco.Nome,
												PagamentoID, PagamentoNome, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, PagamentoPCT, tIngressoLog.CortesiaID,
                                                tIngressoLog.acao, tPreco.Valor, tVendaBilheteriaItem.PacoteID
										END

										DROP TABLE #Vendas

										SELECT 
										LojaID,LojaNome,PrecoID,PrecoNome,
										Quantidade AS Total, 
										Valor,
										Taxa, 
                                        Comissao 
										FROM #FINAL
										ORDER BY LojaNome,PrecoNome

										DROP TABLE #Final
									";
            return sql;


        }




        #endregion



        #endregion


        public DataTable TaxaEntrega(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int canalID, int empresaID, int regionalID, string tipoRelatorio)
        {
            try
            {

                //Variáveis
                object valorAuxiliar = 0;
                decimal valorTaxaEntrega = 0;
                decimal valorTaxaEntregaTotal = 0;
                string nomeTaxaAtual = string.Empty;
                string nomeTaxaAnterior = string.Empty;

                DataTable tabela = getDataTxEntrega(apresentacaoID, comCortesia, eventoID, localID, canalID, empresaID, regionalID, dataInicial, dataFinal, tipoRelatorio);
                DataRow linha = null;
                DataTable tabelaEntrega = Utilitario.EstruturaTaxaEntrega();
                for (int i = 0; i <= tabela.Rows.Count - 1; i++)
                {
                    linha = tabela.Rows[i];
                    nomeTaxaAtual = (string)linha["TaxaEntrega"];

                    if (nomeTaxaAtual != nomeTaxaAnterior)
                    {

                        //Valor taxa para cada Tipo de Entrega 
                        valorAuxiliar = tabela.Compute("SUM(TaxaEntregaValor)", "TaxaEntrega= '" + nomeTaxaAtual + "'");
                        valorTaxaEntrega = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                        DataRow linhaFinal = tabelaEntrega.NewRow();

                        linhaFinal["Taxa de Entrega"] = "<div style='text-align:left'>" + nomeTaxaAtual + "</div>";
                        linhaFinal["Valor"] = Utilitario.AplicaFormatoMoeda(valorTaxaEntrega);

                        tabelaEntrega.Rows.Add(linhaFinal);

                        //soma para linha de total
                        valorTaxaEntregaTotal = valorTaxaEntregaTotal + valorTaxaEntrega;
                    }
                    nomeTaxaAnterior = nomeTaxaAtual;

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
            catch (Exception erro)
            {
                throw erro;
            }



        }
    }
}
