/**************************************************
* Arquivo: EventoTaxaEntrega.cs
* Gerado: 11/11/2008
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib
{

    public class EventoTaxaEntrega : EventoTaxaEntrega_B
    {

        public EventoTaxaEntrega() { }

        public EventoTaxaEntrega(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Sobrecarga do método base Inserir(), inserindo registros sem verificação manual de ID e mantendo as rotinas de auditoria
        /// </summary>
        /// <returns></returns>
        public override bool Inserir()
        {

            try
            {
                BD bd = new BD();

                bd.IniciarTransacao();

                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoTaxaEntrega(EventoID, TaxaEntregaID) ");
                sql.Append("VALUES (@001,@002); SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.TaxaEntregaID.ValorBD);


                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = this.Control.ID > 0;

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        internal bool Inserir(BD bd)
        {

            try
            {

                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoTaxaEntrega(EventoID, TaxaEntregaID) ");
                sql.Append("VALUES (@001,@002); SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.TaxaEntregaID.ValorBD);


                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = this.Control.ID > 0;

                if (result)
                    InserirControle("I", bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cEventoTaxaEntrega (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string GetDetalhesEntrega(int eventoID, int taxaEntregaID)
        {
            try
            {
                string sql = "SELECT TOP 1 DetalhesEntrega FROM tEventoTaxaEntrega(NOLOCK) WHERE EventoID = " + eventoID + " AND TaxaEntregaID = " + taxaEntregaID;

                bd.ConsultaValor(sql);

                string retorno = "";
                if (bd.Consulta().Read())
                {
                    retorno = bd.LerString("DetalhesEntrega");
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

        public bool SalvarDetalhesEntrega(int eventoID, int taxaEntregaID, string detalhesEntrega)
        {
            try
            {
                string sql = "UPDATE tEventoTaxaEntrega SET DetalhesEntrega = '" + detalhesEntrega.Replace("'", "''") + "' WHERE EventoID = " + eventoID + " AND TaxaEntregaID = " + taxaEntregaID;

                return (bd.Executar(sql) == 1);
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

        /// <summary>
        /// Captura os detalhes das taxas de entrega do evento
        /// </summary>
        /// <param name="eventoIDs">Lista de Eventos ID</param>
        /// <param name="taxaEntregaID">TaxaEntrega ID</param>
        /// <returns></returns>
        public List<string> DetalhesEntregaPorEventos(List<int> eventoIDs, int taxaEntregaID)
        {
            List<string> retorno = new List<string>();
            try
            {


                List<int> procedimentos = new List<int>();

                string sql = "";
                if (eventoIDs.Count > 0)
                {
                    sql = @"SELECT DISTINCT
                            CASE WHEN
		                            LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
			                            THEN tEventoEntregaControle.ProcedimentoEntrega
		                            ELSE
			                            CASE WHEN
				                            LEN(tEntregaControle.ProcedimentoEntrega) > 0
				                            THEN tEntregaControle.ProcedimentoEntrega
				                            ELSE tEntrega.ProcedimentoEntrega
			                            END
                            END as ProcedimentoEntrega,
                            CASE WHEN
		                            LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
			                            THEN 3
		                            ELSE
			                            CASE WHEN
				                            LEN(tEntregaControle.ProcedimentoEntrega) > 0
				                            THEN 2
				                            ELSE 1
			                            END
                            END as PrioridadeProcedimento
                            FROM tEventoEntregaControle
                            INNER JOIN tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                            INNER JOIN tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                            WHERE 
                            tEventoEntregaControle.EventoID IN ( " + Utilitario.ArrayToString(eventoIDs.ToArray()) + @") AND
                            tEntrega.ID = " + taxaEntregaID + @"
                            GROUP BY tEntregaControle.ID,tEntrega.ProcedimentoEntrega,tEntregaControle.ProcedimentoEntrega,
                            tEventoEntregaControle.ProcedimentoEntrega,tEventoEntregaControle.EventoID";
                }
                else
                {
                    sql = @"SELECT
			                tEntrega.ProcedimentoEntrega as ProcedimentoEntrega,
			                1 as PrioridadeProcedimento
                            FROM tEntrega 
                            WHERE tEntrega.ID = " + taxaEntregaID + @"";
                }

                    using (IDataReader oDataReader = bd.Consulta(sql))
                    {
                        while (oDataReader.Read())
                            switch (bd.LerInt("PrioridadeProcedimento"))
                            {
                                case 3:
                                    retorno.Add(bd.LerString("ProcedimentoEntrega"));
                                    break;
                                case 2:
                                    if (!procedimentos.Contains(2))
                                    {
                                        procedimentos.Add(2);
                                        retorno.Add(bd.LerString("ProcedimentoEntrega"));
                                    }
                                    break;
                                case 1:
                                    if (!procedimentos.Contains(1))
                                    {
                                        procedimentos.Add(1);
                                        retorno.Add(bd.LerString("ProcedimentoEntrega"));
                                    }
                                    break;
                                default:
                                    break;
                            }
                    }
                

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw new Exception("Procedimento de Entrega: "+ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public string DetalheEntregaEvento(int eventoID, int taxaEntregaID)
        {
            try
            {
                StringBuilder retorno = new StringBuilder();
                string sql = @"SELECT DISTINCT CASE WHEN
                            LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
                                THEN tEventoEntregaControle.ProcedimentoEntrega
                            ELSE
                                CASE WHEN
                                    LEN(tEntregaControle.ProcedimentoEntrega) > 0
                                    THEN tEntregaControle.ProcedimentoEntrega
                                    ELSE tEntrega.ProcedimentoEntrega
                                END
                            END as ProcedimentoEntrega FROM tEventoEntregaControle 
                            INNER JOIN tEntregaControle ON tEntregaControle.ID = tEventoEntregaControle.EntregaControleID
                            INNER JOIN tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                            WHERE EventoID = " + eventoID + " AND tEntrega.ID = " + taxaEntregaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Append(bd.LerString("ProcedimentoEntrega"));
                }
                return retorno.ToString();
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


        /// <summary>
        /// método utilizado na tela de eventos para internet
        /// </summary>
        /// <param name="eventoIDs"></param>
        /// <returns></returns>
        public string DetalhesEntregaPorEventos(int eventoID)
        {
            try
            {
                StringBuilder retorno = new StringBuilder();
                string sql = "SELECT tTaxaEntrega.Nome, tEventoTaxaEntrega.DetalhesEntrega FROM tEventoTaxaEntrega(NOLOCK), tTaxaEntrega (NOLOCK) WHERE tTaxaEntrega.ID = tEventoTaxaEntrega.TaxaEntregaID AND EventoID = " + eventoID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Append(bd.LerString("Nome") + ":" + Environment.NewLine);
                    retorno.Append(bd.LerString("DetalhesEntrega") + Environment.NewLine + Environment.NewLine);
                }
                return retorno.ToString();
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

        public DataTable GetTaxas(int[] eventoID)
        {
            try
            {
                DataTable tabela = new DataTable("EventoTaxaEntrega");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("RegiaoID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Prazo", typeof(int));
                tabela.Columns.Add("Estado", typeof(string));

                string eventosID = string.Empty;

                foreach (int item in eventoID)
                    eventosID += item.ToString() + ",";

                eventosID = eventosID.Remove(eventosID.Length - 1, 1);

                string sql = @"SELECT DISTINCT tTaxaEntrega.ID, tTaxaEntrega.RegiaoID," +
                        "tTaxaEntrega.Nome AS Nome, tTaxaEntrega.Valor, tTaxaEntrega.Prazo, tTaxaEntrega.Estado, tEventoTaxaEntrega.EventoID " +
                        "FROM tTaxaEntrega(NOLOCK) " +
                        "INNER JOIN tRegiao (NOLOCK) ON tTaxaEntrega.RegiaoID = tRegiao.ID  " +
                        "INNER JOIN tEventoTaxaEntrega (NOLOCK) ON tEventoTaxaEntrega.TaxaEntregaID = tTaxaEntrega.ID  " +
                        "WHERE tTaxaEntrega.Disponivel='T'  " +
                        "AND EventoID IN ( " + eventosID + ") AND tRegiao.ID <> " + Regiao.TAXA_WEB.ToString() + " " +
                        "ORDER BY tTaxaEntrega.Nome ";


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["RegiaoID"] = bd.LerInt("RegiaoID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["Prazo"] = bd.LerInt("Prazo");
                    linha["Estado"] = bd.LerString("Estado");
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

        public DataTable GetTaxas(int eventoID)
        {
            return this.GetTaxas(new int[] { eventoID });
        }

        public List<IRLib.ClientObjects.EstruturaEventoTaxaEntrega> CarregarPorEvento(int eventoID)
        {
            try
            {
                List<IRLib.ClientObjects.EstruturaEventoTaxaEntrega> lista = new List<IRLib.ClientObjects.EstruturaEventoTaxaEntrega>();
                string sql =
                        string.Format(@"SELECT te.ID AS TaxaEntregaID, IsNull(ete.ID, 0) AS EventoTaxaEntregaID,
                                te.RegiaoID, r.Nome AS Regiao, te.Nome AS TaxaEntrega, te.Valor,
                                CASE WHEN ete.ID IS NOT NULL
                                    THEN 'T'
                                    ELSE 'F' 
                                END AS Disponivel,
                                CASE WHEN ete.DetalhesEntrega IS NULL OR ete.DetalhesEntrega = ''
                                    THEN 'T'
                                    ELSE 'F'
                                END AS Padrao,
                                IsNull(te.ProcedimentoEntrega, '') AS Procedimento,
                                IsNull(ete.DetalhesEntrega, '') AS Detalhes
                                
                            FROM tTaxaEntrega te (NOLOCK)
                            LEFT JOIN tEventoTaxaEntrega ete (NOLOCK) ON te.ID = ete.TaxaEntregaID AND ete.EventoID = {0}
                            LEFT JOIN tEvento e (NOLOCK) ON ete.EventoID = e.ID AND e.ID = {0}
                            INNER JOIN tRegiao r (NOLOCK) ON r.ID = te.RegiaoID
                                ORDER BY te.Nome, r.Nome", eventoID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaEventoTaxaEntrega()
                    {
                        EventoID = eventoID,
                        TaxaEntregaID = bd.LerInt("TaxaEntregaID"),
                        EventoTaxaEntregaID = bd.LerInt("EventoTaxaEntregaID"),
                        RegiaoID = bd.LerInt("RegiaoID"),
                        Regiao = bd.LerString("Regiao"),
                        TaxaEntrega = bd.LerString("TaxaEntrega"),
                        Valor = bd.LerDecimal("Valor"),
                        Disponivel = bd.LerBoolean("Disponivel"),
                        DisponivelOld = bd.LerBoolean("Disponivel"),
                        Padrao = bd.LerBoolean("Padrao"),
                        Procedimento = bd.LerString("Procedimento"),
                        Detalhes = bd.LerString("Detalhes"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Inserir(IRLib.ClientObjects.EstruturaEventoTaxaEntrega editado)
        {
            this.EventoID.Valor = editado.EventoID;
            this.TaxaEntregaID.Valor = editado.TaxaEntregaID;
            this.DetalhesEntrega.Valor = string.Empty;
            this.Inserir();
        }

        public void Atualizar(IRLib.ClientObjects.EstruturaEventoTaxaEntrega editado)
        {
            this.Control.ID = editado.EventoTaxaEntregaID;
            this.EventoID.Valor = editado.EventoID;
            this.TaxaEntregaID.Valor = editado.TaxaEntregaID;
            this.DetalhesEntrega.Valor = editado.Detalhes;
            this.Atualizar();
        }
    }

    public class EventoTaxaEntregaLista : EventoTaxaEntregaLista_B
    {

        public EventoTaxaEntregaLista() { }

        public EventoTaxaEntregaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
