using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for TipoLista
/// </summary>
namespace IngressoRapido.Lib
{
    public class TipoLista : List<Tipo>
    {
        DAL oDAL = new DAL();
        Tipo oTipo;

        public TipoLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Tipos, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        private TipoLista CarregarLista(string clausula)
        {
            string strSql = string.Empty;

            if (clausula == string.Empty)
            {

                strSql = @"SELECT DISTINCT IR_TipoID, Nome FROM Tipo, EventoSubTipo WHERE IR_TipoID = TipoID ORDER BY Nome";
            }
            else
            {
                return this;
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oTipo = new Tipo(dr["IR_TipoID"].ToInt32());
                        oTipo.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.Add(oTipo);
                    }
                }

                return this;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public TipoLista CarregarListaFiltrada()
        {
            try
            {
                string strSql = @"SELECT DISTINCT t.IR_TipoID, t.Nome
                                  FROM Tipo t (NOLOCK)
                                  INNER JOIN EventoSubtipo es (NOLOCK) ON es.TipoID = t.IR_TipoID
                                  INNER JOIN Evento e (NOLOCK) ON e.SubtipoID = es.IR_SubtipoID
                                  INNER JOIN Local loc (NOLOCK) ON e.LocalID = loc.IR_LocalID
                                  INNER JOIN Apresentacao apr (NOLOCK) ON apr.EventoID = e.IR_EventoID
                                  INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = apr.IR_ApresentacaoID
                                  ORDER BY t.Nome";

                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        this.Add(new Tipo(dr["IR_TipoID"].ToInt32())
                        {
                            Nome = Util.LimparTitulo(dr["Nome"].ToString())
                        });
                    }
                }

                return this;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }


        /// <summary>
        /// Retorna a Lista de todos os Tipos de Eventos
        /// </summary>
        public TipoLista CarregarTipos(string primeiroRegistro)
        {
            if (primeiroRegistro.Length > 0)
                this.Add(new Tipo(0) { Nome = primeiroRegistro });
            return CarregarLista("");
        }
    }
}