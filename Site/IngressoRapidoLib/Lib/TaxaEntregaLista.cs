using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for ApresentacaoLista
    /// </summary>
    public class TaxaEntregaLista : List<TaxaEntrega>
    {
        private DAL oDAL { get; set; }
        TaxaEntrega oTaxaEntrega { get; set; }

        public TaxaEntregaLista()
        {
            this.oDAL = new DAL();
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Apresentações do tipo Apresentação, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        public TaxaEntregaLista TaxasDisponiveis(int clienteID, string estado, string sessionID, bool entregaGratuita)
        {
            string strSql = string.Empty;

            int celularTaxaEntregaID = Convert.ToInt32(ConfigurationManager.AppSettings["EntregaCelularTaxaID"]);

            Carrinho oCarrinho = new Carrinho();
            DateTime menorApresentacao = oCarrinho.DataMaisProxima(clienteID, sessionID);

            //strSql = "SELECT ID, IR_TaxaEntregaID AS IR_ID, Nome, Valor, PrazoEntrega " +
            //         "FROM TaxaEntrega " +
            //         "WHERE (Estado = '" + estado.ToUpper() + "' AND '" + DateTime.Now.ToString("yyyyMMdd") + 
            //         "' + PrazoEntrega < '" + menorApresentacao.ToString("yyyyMMdd") + "') OR PrazoEntrega = -1";           


            //        sp_GetTaxasEntrega @ClienteID, '@SessionID', @SetEntregaGratuita, '@MenorApresentacao', '@DataAtual','@Estado'"
            strSql = "sp_GetTaxasEntrega1 " + clienteID + ", '" + sessionID + "', " + (entregaGratuita ? 1 : 0) + ", '" + menorApresentacao.ToString("yyyyMMdd") + "', '" + DateTime.Now.ToString("yyyyMMdd") + "','" + estado.ToUpper() + "'" + "," + celularTaxaEntregaID;

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql, CommandType.StoredProcedure))
                {
                    while (dr.Read())
                    {
                        oTaxaEntrega = new TaxaEntrega(Convert.ToInt32(dr["ID"]));
                        oTaxaEntrega.IR_ID = Convert.ToInt32(dr["IR_ID"]);
                        oTaxaEntrega.Valor = Convert.ToDecimal(dr["Valor"]);
                        oTaxaEntrega.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        oTaxaEntrega.PrazoEntrega = Convert.ToInt32(dr["PrazoEntrega"]);
                        oTaxaEntrega.ProcedimentoEntrega = Convert.ToString(dr["ProcedimentoEntrega"]) + Environment.NewLine + Convert.ToString(dr["DetalhesEntrega"]);

                        this.Add(oTaxaEntrega);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public TaxaEntregaLista GetByEventoApresentacao(int EventoID, int ApresentacaoID)
        {
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(
                        string.Format(@"SELECT te.IR_TaxaEntregaID, te.Nome, te.Estado, te.Valor, 
                                        te.ProcedimentoEntrega, te.PrazoEntrega, IsNull(ete.DetalhesEntrega, '') AS DetalhesEntrega
                        FROM Evento e (NOLOCK)
                        INNER JOIN EventoTaxaEntrega ete (NOLOCK) ON ete.EventoID = e.IR_EventoID 
                        INNER JOIN TaxaEntrega te (NOLOCK) ON te.IR_TaxaEntregaID = ete.TaxaEntregaID
                        INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID
                        WHERE e.IR_EventoID = {0} AND ap.IR_ApresentacaoID = {1} AND
                        (te.PrazoEntrega = -1 OR 
                        CONVERT(VARCHAR, DATEADD(day, te.PrazoEntrega, CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring(ap.Horario,0,9), 112))
                        ORDER BY Nome", EventoID, ApresentacaoID)))
                {
                    while (dr.Read())
                    {
                        string Procedimento = Convert.ToString(dr["ProcedimentoEntrega"]);
                        string Detalhes = Convert.ToString(dr["DetalhesEntrega"]);


                        this.Add(new TaxaEntrega()
                        {
                            ID = Convert.ToInt32(dr["IR_TaxaEntregaID"]),
                            Valor = Convert.ToDecimal(dr["Valor"]),
                            Nome = Util.LimparTitulo(dr["Nome"].ToString()),
                            Estado = dr["Estado"].ToString(),
                            PrazoEntrega = Convert.ToInt32(dr["PrazoEntrega"]),
                            ProcedimentoEntrega = string.IsNullOrEmpty(Detalhes) ? Procedimento : Detalhes,
                        });
                    }
                };

                if (this.Count == 0)
                    throw new Exception("Não existem taxas de entrega disponíveis para esta apresentação.");


                return this;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public TaxaEntregaLista GetByEventoSomentePacotes(int EventoID)
        {
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(
                        string.Format(@"
                       SELECT COUNT(ete.TaxaEntregaID) AS qtd, te.IR_TaxaEntregaID, 
		                    te.Nome,te.Estado,te.Valor, te.ProcedimentoEntrega, te.PrazoEntrega, 
		                    IsNull(ete.DetalhesEntrega, '') AS DetalhesEntrega
	                    FROM eventotaxaentrega ete(NOLOCK)
	                    INNER JOIN TaxaEntrega te (NOLOCK) ON te.IR_TaxaEntregaID = ete.TaxaEntregaID
	                    WHERE ete.EventoID in ( SELECT DISTINCT it.EventoID
							                    FROM pacoteitem it(NOLOCK)
							                    INNER JOIN pacoteitem pit(NOLOCK)ON(it.pacoteid=pit.pacoteid)
							                    WHERE pit.EventoId = {0}
							                    )
	                    GROUP BY ete.TaxaEntregaID,	te.IR_TaxaEntregaID, te.Nome,te.Estado,	te.Valor, 
		                    te.ProcedimentoEntrega,	te.PrazoEntrega, ete.DetalhesEntrega
	                    HAVING COUNT(ete.TaxaEntregaID)>=(SELECT  COUNT(DISTINCT it.EventoID)
										                    FROM pacoteitem it(NOLOCK)
										                    INNER JOIN pacoteitem pit(NOLOCK)ON(it.pacoteid=pit.pacoteid)
										                    WHERE pit.EventoId = {0}
										                    )
                    ", EventoID

                                      )))
                {
                    while (dr.Read())
                    {
                        string Procedimento = Convert.ToString(dr["ProcedimentoEntrega"]);
                        string Detalhes = Convert.ToString(dr["DetalhesEntrega"]);


                        this.Add(new TaxaEntrega()
                        {
                            ID = Convert.ToInt32(dr["IR_TaxaEntregaID"]),
                            Valor = Convert.ToDecimal(dr["Valor"]),
                            Nome = Util.LimparTitulo(dr["Nome"].ToString()),
                            Estado = dr["Estado"].ToString(),
                            PrazoEntrega = Convert.ToInt32(dr["PrazoEntrega"]),
                            ProcedimentoEntrega = string.IsNullOrEmpty(Detalhes) ? Procedimento : Detalhes,
                        });
                    }
                };

                if (this.Count == 0)
                    throw new Exception("Não existem taxas de entrega disponíveis para esta apresentação.");


                return this;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public TaxaEntregaLista TodasTaxasVIR(string Estado)
        {
            try
            {
                int celularTaxaEntregaID = Convert.ToInt32(ConfigurationManager.AppSettings["EntregaCelularTaxaID"]);

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ");
                stbSQL.Append("ID, IR_TaxaEntregaID, Nome, Estado, ");
                stbSQL.Append("Valor, PrazoEntrega, ProcedimentoEntrega ");
                stbSQL.Append("FROM TaxaEntrega (NOLOCK) ");
                stbSQL.Append("WHERE Nome <> 'Retirada Bilheteria' ");
                stbSQL.Append("AND Estado = '" + Estado + "' AND IR_TaxaEntregaID <> " + celularTaxaEntregaID);
                stbSQL.Append("ORDER BY Estado, Nome");
                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {
                    while (dr.Read())
                    {
                        oTaxaEntrega = new TaxaEntrega(Convert.ToInt32(dr["ID"]));
                        oTaxaEntrega.IR_ID = Convert.ToInt32(dr["IR_TaxaEntregaID"]);
                        oTaxaEntrega.Valor = Convert.ToDecimal(dr["Valor"]);

                        //if (oTaxaEntrega.IR_ID == 4 || oTaxaEntrega.IR_ID == 5 || oTaxaEntrega.IR_ID == 14 || oTaxaEntrega.IR_ID == 17 || oTaxaEntrega.IR_ID == 19 ||
                        //    oTaxaEntrega.IR_ID == 57 || oTaxaEntrega.IR_ID == 65 || oTaxaEntrega.IR_ID == 66)
                        //    oTaxaEntrega.Nome = Util.LimparTitulo(dr["Nome"].ToString()) + " - " + Util.LimparTitulo(dr["Estado"].ToString());
                        //else
                        oTaxaEntrega.Nome = Util.LimparTitulo(dr["Nome"].ToString());

                        oTaxaEntrega.PrazoEntrega = Convert.ToInt32(dr["PrazoEntrega"]);
                        oTaxaEntrega.ProcedimentoEntrega = Convert.ToString(dr["ProcedimentoEntrega"]);

                        this.Add(oTaxaEntrega);
                    }
                }

                return this;
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

    }

}