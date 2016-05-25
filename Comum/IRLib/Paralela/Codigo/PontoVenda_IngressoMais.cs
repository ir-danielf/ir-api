/**************************************************
* Arquivo: PontosVenda.cs
* Gerado: 27/11/2007
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib.Paralela
{

    public class PontoVenda_IngressoMais : PontoVenda_IngressoMais_B
    {

        public PontoVenda_IngressoMais() { }

        public PontoVenda_IngressoMais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public PontoVenda_IngressoMais CarregarPDV(string Clausula)
        {
            try
            {

                string sql = "SELECT * FROM tPontoVenda_IngressoMais WHERE " + Clausula;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.Local.ValorBD = bd.LerString("Local");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Numero.ValorBD = bd.LerString("Numero");
                    this.Compl.ValorBD = bd.LerString("Compl");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Referencia.ValorBD = bd.LerString("Referencia");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.PermiteRetirada.ValorBD = bd.LerString("PermiteRetirada");
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();
                return this;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarTodosPDV()
        {
            DataSet ds = new DataSet();

            DataRow dr = null;

            string sql = "";

            DataTable PontoVenda = new DataTable("PontoVenda");
            PontoVenda.Columns.Add("ID", typeof(int));
            PontoVenda.Columns.Add("Local", typeof(string));
            PontoVenda.Columns.Add("Nome", typeof(string));
            PontoVenda.Columns.Add("Endereco", typeof(string));
            PontoVenda.Columns.Add("Numero", typeof(string));
            PontoVenda.Columns.Add("Compl", typeof(string));
            PontoVenda.Columns.Add("Cidade", typeof(string));
            PontoVenda.Columns.Add("Estado", typeof(string));
            PontoVenda.Columns.Add("Bairro", typeof(string));
            PontoVenda.Columns.Add("Obs", typeof(string));
            PontoVenda.Columns.Add("Referencia", typeof(string));
            PontoVenda.Columns.Add("CEP", typeof(string));
            PontoVenda.Columns.Add("PermiteRetirada", typeof(bool));

            DataTable PontoVendaXFormaPgto = new DataTable("PontoVendaXFormaPgto");
            PontoVendaXFormaPgto.Columns.Add("ID", typeof(int));
            PontoVendaXFormaPgto.Columns.Add("PontoVendaID", typeof(int));
            PontoVendaXFormaPgto.Columns.Add("PontoVendaFormaPgtoID", typeof(int));

            DataTable PontoVendaHorario_IngressoMais = new DataTable("PontoVendaHorario_IngressoMais");
            PontoVendaHorario_IngressoMais.Columns.Add("ID", typeof(int));
            PontoVendaHorario_IngressoMais.Columns.Add("PontoVendaID", typeof(int));
            PontoVendaHorario_IngressoMais.Columns.Add("HorarioInicial", typeof(string));
            PontoVendaHorario_IngressoMais.Columns.Add("HorarioFinal", typeof(string));
            PontoVendaHorario_IngressoMais.Columns.Add("DiaSemana", typeof(int));

            DataTable PontoVendaFormaPgto = new DataTable("PontoVendaFormaPgto");
            PontoVendaFormaPgto.Columns.Add("ID", typeof(int));
            PontoVendaFormaPgto.Columns.Add("Nome", typeof(string));

            try
            {
                // Pontos de Venda
                sql = "SELECT * FROM tPontoVenda_IngressoMais ORDER BY Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    dr = PontoVenda.NewRow();
                    PontoVenda.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["Local"] = bd.LerString("Local");
                    dr["Nome"] = bd.LerString("Nome");
                    dr["Endereco"] = bd.LerString("Endereco");
                    dr["Numero"] = bd.LerString("Numero");
                    dr["Compl"] = bd.LerString("Compl");
                    dr["Cidade"] = bd.LerString("Cidade");
                    dr["Estado"] = bd.LerString("Estado");
                    dr["Bairro"] = bd.LerString("Bairro");
                    dr["Obs"] = bd.LerString("Obs");
                    dr["Referencia"] = bd.LerString("Referencia");
                    dr["CEP"] = bd.LerString("CEP");
                    dr["PermiteRetirada"] = bd.LerBoolean("PermiteRetirada");
                }
                bd.Fechar();


                // PontoVendaFormaPgto
                sql = "SELECT * FROM tPontoVenda_IngressoMaisXFormaPgto_IngressoMais_IngressoMais";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    dr = PontoVendaXFormaPgto.NewRow();
                    PontoVendaXFormaPgto.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["PontoVendaID"] = bd.LerInt("PontoVendaID");
                    dr["PontoVendaFormaPgtoID"] = bd.LerInt("PontoVendaFormaPgtoID");
                }
                bd.Fechar();


                // Horario
                sql = "SELECT * FROM tPontoVenda_IngressoMaisHorario_IngressoMais";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    dr = PontoVendaHorario_IngressoMais.NewRow();
                    PontoVendaHorario_IngressoMais.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["PontoVendaID"] = bd.LerInt("PontoVendaID");
                    dr["HorarioInicial"] = bd.LerString("HorarioInicial");
                    dr["HorarioFinal"] = bd.LerString("HorarioFinal");
                    dr["DiaSemana"] = bd.LerInt("DiaSemana");
                }
                bd.Fechar();

                // Formas de Pagamento
                sql = "SELECT * FROM tPontoVenda_IngressoMaisFormaPgto_IngressoMais";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    dr = PontoVendaFormaPgto.NewRow();
                    PontoVendaFormaPgto.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["Nome"] = bd.LerString("Nome");
                }
                bd.Fechar();

                ds.Tables.Add(PontoVenda);
                ds.Tables.Add(PontoVendaXFormaPgto);
                ds.Tables.Add(PontoVendaHorario_IngressoMais);
                ds.Tables.Add(PontoVendaFormaPgto);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CarregarTabelaPDV(List<int> eventosID, string estado, string cidade)
        {
            //if (eventosID.Count == 0)
            //    return CarregarTabelaPDV(estado, cidade);

            DataRow dr = null;

            string sql = "";

            DataTable PontoVenda = new DataTable("PontoVenda");
            PontoVenda.Columns.Add("ID", typeof(int));
            PontoVenda.Columns.Add("Local", typeof(string));
            PontoVenda.Columns.Add("Nome", typeof(string));
            PontoVenda.Columns.Add("Endereco", typeof(string));
            PontoVenda.Columns.Add("Numero", typeof(string));
            PontoVenda.Columns.Add("Compl", typeof(string));
            PontoVenda.Columns.Add("Cidade", typeof(string));
            PontoVenda.Columns.Add("Estado", typeof(string));
            PontoVenda.Columns.Add("Bairro", typeof(string));
            PontoVenda.Columns.Add("Obs", typeof(string));
            PontoVenda.Columns.Add("Referencia", typeof(string));
            PontoVenda.Columns.Add("CEP", typeof(string));
            PontoVenda.Columns.Add("PermiteRetirada", typeof(bool));

            try
            {
                // Pontos de Venda
                if (eventosID.Count != 0)
                    sql =
                        string.Format(@"
                        SELECT 
		                    DISTINCT pv.ID, pv.Local, pv.Nome, pv.Endereco, pv.Numero, Compl, Cidade, Estado, Bairro, pv.Obs, Referencia, CEP, PermiteRetirada,
							                (COUNT(DISTINCT epv.EventoID) + 
							                CASE WHEN ev.HabilitarRetiradaTodosPDV = 'T'
								                THEN 1
								                ELSE 0
							                END) AS Disponivel
			                INTO #tmp
			                FROM tPontoVenda_IngressoMais pv (NOLOCK)
	                        LEFT JOIN tEventoPontoDeVenda epv (NOLOCK) ON pv.ID = epv.PontoDeVendaID AND epv.EventoID IN ({0})
	                        LEFT JOIN tEvento ev (NOLOCK) ON ev.ID IN ({0})
	                        WHERE pv.Estado = '{1}' AND pv.Cidade = '{2}' AND pv.PermiteRetirada = 'T' AND (epv.ID IS NOT NULL OR ev.HabilitarRetiradaTodosPDV = 'T')
	                        GROUP BY pv.ID, pv.Local, pv.Nome, pv.Endereco, pv.Numero, Compl, Cidade, 
					                Estado, Bairro, pv.Obs, Referencia, CEP, PermiteRetirada, ev.HabilitarRetiradaTodosPDV               	        
	                 SELECT 
		                ID, Local, Nome, Endereco, Numero, Compl, Cidade, 
					                Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada
	                FROM #tmp
	                GROUP BY ID, Local, Nome, Endereco, Numero, Compl, Cidade, 
					                Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada
	                HAVING SUM(Disponivel) = {3}
	                DROP TABLE #tmp
                    ", Utilitario.ArrayToString(eventosID.ToArray()), estado, cidade, eventosID.Count);
                else
                    sql = "SELECT * FROM tPontoVenda_IngressoMais WHERE PermiteRetirada = 'T' AND IR = 'T' AND Estado = '" + estado + "' AND Cidade = '" + cidade + "' ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    dr = PontoVenda.NewRow();
                    PontoVenda.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["Local"] = bd.LerString("Local");
                    dr["Nome"] = bd.LerString("Nome");
                    dr["Endereco"] = bd.LerString("Endereco");
                    dr["Numero"] = bd.LerString("Numero");
                    dr["Compl"] = bd.LerString("Compl");
                    dr["Cidade"] = bd.LerString("Cidade");
                    dr["Estado"] = bd.LerString("Estado");
                    dr["Bairro"] = bd.LerString("Bairro");
                    dr["Obs"] = bd.LerString("Obs");
                    dr["Referencia"] = bd.LerString("Referencia");
                    dr["CEP"] = bd.LerString("CEP");
                    dr["PermiteRetirada"] = true;
                }
                bd.Fechar();


                return PontoVenda;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CarregarTabelaPDV(string estado, string cidade, bool Geral)
        {
            DataRow dr = null;

            string sql = "";

            DataTable PontoVenda = new DataTable("PontoVenda");
            PontoVenda.Columns.Add("ID", typeof(int));
            PontoVenda.Columns.Add("Local", typeof(string));
            PontoVenda.Columns.Add("Nome", typeof(string));
            PontoVenda.Columns.Add("Endereco", typeof(string));
            PontoVenda.Columns.Add("Numero", typeof(string));
            PontoVenda.Columns.Add("Compl", typeof(string));
            PontoVenda.Columns.Add("Cidade", typeof(string));
            PontoVenda.Columns.Add("Estado", typeof(string));
            PontoVenda.Columns.Add("Bairro", typeof(string));
            PontoVenda.Columns.Add("Obs", typeof(string));
            PontoVenda.Columns.Add("Referencia", typeof(string));
            PontoVenda.Columns.Add("CEP", typeof(string));
            PontoVenda.Columns.Add("PermiteRetirada", typeof(bool));

            try
            {
                // Pontos de Venda
                sql = "SELECT * FROM tPontoVenda_IngressoMais WHERE  Estado = '" + estado + "' AND Cidade = '" + cidade + "' ORDER BY Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    dr = PontoVenda.NewRow();
                    PontoVenda.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["Local"] = bd.LerString("Local");
                    dr["Nome"] = bd.LerString("Nome");
                    dr["Endereco"] = bd.LerString("Endereco");
                    dr["Numero"] = bd.LerString("Numero");
                    dr["Compl"] = bd.LerString("Compl");
                    dr["Cidade"] = bd.LerString("Cidade");
                    dr["Estado"] = bd.LerString("Estado");
                    dr["Bairro"] = bd.LerString("Bairro");
                    dr["Obs"] = bd.LerString("Obs");
                    dr["Referencia"] = bd.LerString("Referencia");
                    dr["CEP"] = bd.LerString("CEP");
                    dr["PermiteRetirada"] = bd.LerBoolean("PermiteRetirada");
                }
                bd.Fechar();


                return PontoVenda;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string buscarNome(int pdvID)
        {
            try
            {
                string retorno = "-";
                string sql = "SELECT Nome FROM tPontoVenda_IngressoMais WHERE ID = " + pdvID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = bd.LerString("Nome");
                }

                bd.Fechar();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable EstruturaTabela()
        {
            DataTable tabela = new DataTable("Cidade");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("EstadoID", typeof(int));
            tabela.Columns.Add("EstadoSigla", typeof(string));
            return tabela;
        }

        public DataTable TabelaPorEstadocomSiglaComPDV(int estadoID)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta(@"SELECT DISTINCT tCidade.ID, tCidade.Nome, EstadoSigla 
                            FROM tCidade (NOLOCK) 
                            INNER JOIN tPontoVenda_IngressoMais (NOLOCK) ON tCidade.Nome = tPontoVenda_IngressoMais.Cidade 
                            AND tPontoVenda_IngressoMais.Estado = EstadoSigla
                            WHERE EstadoID = " + estadoID + " AND tPontoVenda_IngressoMais.PermiteRetirada = 'T' AND tPontoVenda_IngressoMais.IR = 'T' ORDER BY Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = estadoID;
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); ;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable EstadosComPDV()
        {
            try
            {
                DataTable tabela = new DataTable("Estado");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));

                bd.Consulta(@"SELECT DISTINCT tEstado.ID,tEstado.Sigla FROM tEstado
                               INNER JOIN tPontoVenda_IngressoMais ON tEstado.Sigla = tPontoVenda_IngressoMais.Estado and tPontoVenda_IngressoMais.PermiteRetirada = 'T' AND tPontoVenda_IngressoMais.IR = 'T'");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Sigla"] = bd.LerString("Sigla");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable TabelaPorEstadocomSiglaComPDV(int estadoID, bool Geral)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta(@"SELECT DISTINCT tCidade.ID, tCidade.Nome, EstadoSigla 
                            FROM tCidade (NOLOCK) 
                            INNER JOIN tPontoVenda_IngressoMais (NOLOCK) ON tCidade.Nome = tPontoVenda_IngressoMais.Cidade 
                            AND tPontoVenda_IngressoMais.Estado = EstadoSigla
                            WHERE EstadoID = " + estadoID + " ORDER BY Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = estadoID;
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); ;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable EstadosComPDV(bool Geral)
        {
            try
            {
                DataTable tabela = new DataTable("Estado");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));

                bd.Consulta(@"SELECT DISTINCT tEstado.ID,tEstado.Sigla FROM tEstado
                               INNER JOIN tPontoVenda_IngressoMais ON tEstado.Sigla = tPontoVenda_IngressoMais.Estado ");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Sigla"] = bd.LerString("Sigla");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable EstadosComPDV(List<int> listaEventoID)
        {
            if (listaEventoID.Count == 0)
                return this.EstadosComPDV();

            try
            {
                DataTable tabela = new DataTable("Estado");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));


                string sql =
                    string.Format(@"
                        SELECT 
		                    DISTINCT e.ID, e.Sigla, 
							                    (COUNT(DISTINCT epv.EventoID) + 
							                    CASE WHEN ev.HabilitarRetiradaTodosPDV = 'T'
								                    THEN 1
								                    ELSE 0
							                    END) AS Disponivel
			                    INTO #tmp
			                    FROM tPontoVenda_IngressoMais pv (NOLOCK)
			                    INNER JOIN tEstado e (NOLOCK) ON e.Sigla = pv.Estado
	                            LEFT JOIN tEventoPontoDeVenda epv (NOLOCK) ON pv.ID = epv.PontoDeVendaID AND epv.EventoID IN ({0})
	                            LEFT JOIN tEvento ev (NOLOCK) ON ev.ID IN ({0})
	                            WHERE pv.PermiteRetirada = 'T' AND (epv.ID IS NOT NULL OR ev.HabilitarRetiradaTodosPDV = 'T')
	                            GROUP BY e.ID, e.Sigla, ev.HabilitarRetiradaTodosPDV--, ev.ID
                    	        
	                     SELECT 
		                    ID, Sigla
	                    FROM #tmp
	                    GROUP BY ID, Sigla
	                    HAVING SUM(Disponivel) = {1}
                        ORDER BY Sigla
                    	       
	                    DROP TABLE #tmp
                        ", Utilitario.ArrayToString(listaEventoID.ToArray()), listaEventoID.Count);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Sigla"] = bd.LerString("Sigla");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataTable TabelaPorEstadocomSiglaComPDV(int estadoID, List<int> listaEventoID)
        {
            if (listaEventoID.Count == 0)
                return this.TabelaPorEstadocomSiglaComPDV(estadoID);

            try
            {
                string sql =
                   string.Format(@"
                        SELECT 
		                    DISTINCT c.ID, c.Nome, c.EstadoSigla,
							                    (COUNT(DISTINCT epv.EventoID) + 
							                    CASE WHEN ev.HabilitarRetiradaTodosPDV = 'T'
								                    THEN 1
								                    ELSE 0
							                    END) AS Disponivel
			                    INTO #tmp
			                    FROM tPontoVenda_IngressoMais pv (NOLOCK)
			                    INNER JOIN tCidade c (NOLOCK) ON c.Nome = pv.Cidade
	                            LEFT JOIN tEventoPontoDeVenda epv (NOLOCK) ON pv.ID = epv.PontoDeVendaID AND epv.EventoID IN ({0})
	                            LEFT JOIN tEvento ev (NOLOCK) ON ev.ID IN ({0})
	                            WHERE EstadoID = {1} AND pv.PermiteRetirada = 'T' AND (epv.ID IS NOT NULL OR ev.HabilitarRetiradaTodosPDV = 'T')
	                            GROUP BY c.ID, c.Nome, c.EstadoSigla, ev.HabilitarRetiradaTodosPDV--, ev.ID
                    	        
	                    SELECT 
		                    ID, Nome, EstadoSigla 
	                    FROM #tmp
	                    GROUP BY ID,Nome, EstadoSigla 
	                    HAVING SUM(Disponivel) = {2}
                    	ORDER BY Nome
	                    DROP TABLE #tmp
                        ", Utilitario.ArrayToString(listaEventoID.ToArray()), estadoID, listaEventoID.Count);

                bd.Consulta(sql);
                DataTable dtt = this.EstruturaTabela();
                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = estadoID;
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); ;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override bool Inserir()
        {
            bool inserido = base.Inserir();
            if (!inserido)
                return false;

            if (this.PermiteRetirada.Valor && this.IR.Valor)
            {
                EventoPontoDeVenda epdv = new EventoPontoDeVenda();
                List<int> Eventos = new List<int>();
                bd.Consulta(
                    @"SELECT 
                        DISTINCT tEvento.ID 
                    FROM tEvento (NOLOCK) 
                    INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                    WHERE HabilitarRetiradaTodosPDV = 'F' AND tApresentacao.Horario > '" + DateTime.Now.Date.ToString("yyyyMMddHHmmss") + "'");

                while (bd.Consulta().Read())
                    Eventos.Add(bd.LerInt("ID"));

                foreach (var evento in Eventos)
                {
                    epdv.Limpar();
                    epdv.PontoDeVendaID.Valor = this.Control.ID;
                    epdv.EventoID.Valor = evento;
                    epdv.Inserir();
                }
            }

            return inserido;
        }

    }

    public class PontoVendaLista_IngressoMais : PontoVendaLista_IngressoMais_B //List<PontosVenda>
    {

        public PontoVendaLista_IngressoMais() { }

        public PontoVendaLista_IngressoMais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<PontoVenda_IngressoMais> CarregarNomesPontoVenda()
        {
            List<PontoVenda_IngressoMais> oPontosVendaLista = new List<PontoVenda_IngressoMais>();

            try
            {
                string sql = "SELECT ID, Nome, Local FROM tPontoVenda_IngressoMais ORDER BY Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    PontoVenda_IngressoMais pv = new PontoVenda_IngressoMais();
                    pv.Control.ID = bd.LerInt("ID");
                    pv.Local.Valor = bd.LerString("Local");
                    pv.Nome.Valor = bd.LerString("Nome");

                    oPontosVendaLista.Add(pv);
                }
                bd.Fechar();
                return oPontosVendaLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PontoVenda_IngressoMais> CarregarNomesPontoVendaPorEstado(string estado)
        {
            List<PontoVenda_IngressoMais> oPontosVendaLista = new List<PontoVenda_IngressoMais>();

            try
            {
                string sql = @"SELECT ID, Nome, Local, Endereco, Numero,Compl,Bairro,Obs,Referencia  
                                FROM tPontoVenda_IngressoMais Where Estado = '" + estado + "' ORDER BY Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    PontoVenda_IngressoMais pv = new PontoVenda_IngressoMais();
                    pv.Control.ID = bd.LerInt("ID");
                    pv.Local.Valor = bd.LerString("Local");
                    pv.Nome.Valor = bd.LerString("Nome");
                    pv.Endereco.Valor = bd.LerString("Endereco");
                    pv.Numero.Valor = bd.LerString("Numero");
                    pv.Compl.Valor = bd.LerString("Compl");
                    pv.Bairro.Valor = bd.LerString("Bairro");
                    pv.Obs.Valor = bd.LerString("Obs");
                    pv.Referencia.Valor = bd.LerString("Referencia");

                    oPontosVendaLista.Add(pv);
                }
                bd.Fechar();
                return oPontosVendaLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

}
