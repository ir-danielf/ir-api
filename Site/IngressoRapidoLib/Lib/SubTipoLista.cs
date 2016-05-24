using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for TipoLista
/// </summary>
namespace IngressoRapido.Lib
{
	public class SubTipoLista : List<SubTipo>
	{
		DAL oDAL = new DAL();
		SubTipo oTipo;

		public SubTipoLista()
		{
			this.Clear();
		}

		/// <summary>
		/// Funcao Interna: Retorna uma Lista de Tipos, 
		/// a partir de uma clausula WHERE 
		/// </summary>
		public SubTipoLista ListaPorTipo(int tipoID, string primeiroRegistro)
		{
			if (primeiroRegistro.Length > 0)
				this.Add(new SubTipo(0) { Descricao = primeiroRegistro });

			string strSql = "SELECT IR_SubTipoID, Descricao FROM EventoSubTipo (NOLOCK) WHERE TipoID = " + tipoID + " ORDER BY Descricao";

			try
			{
				using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
				{
					while (dr.Read())
					{
						oTipo = new SubTipo(Convert.ToInt32(dr["IR_SubTipoID"].ToString()));
						oTipo.Descricao = Util.LimparTitulo(dr["Descricao"].ToString());
						this.Add(oTipo);
					}
				}

				// Fecha conexão da classe DataAccess
				oDAL.ConnClose();
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

		public SubTipoLista ListaPorTipoFiltrada(int tipoID)
		{
            string strSql = @"SELECT DISTINCT es.IR_SubtipoID, es.Descricao
                              FROM EventoSubtipo es (NOLOCK) 
                              INNER JOIN Evento e (NOLOCK) ON e.SubtipoID = es.IR_SubtipoID
                              INNER JOIN Local loc (NOLOCK) ON e.LocalID = loc.IR_LocalID
                              INNER JOIN Apresentacao apr (NOLOCK) ON apr.EventoID = e.IR_EventoID
                              INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = apr.IR_ApresentacaoID
                              WHERE es.TipoID = " + tipoID + "ORDER BY es.Descricao";

			try
			{
				using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
				{
					while (dr.Read())
					{
						this.Add(new SubTipo(dr["IR_SubTipoID"].ToInt32())
						{
							Descricao = Util.LimparTitulo(dr["Descricao"].ToString()),
							tipoID = tipoID
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
	}
}