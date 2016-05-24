using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Dapper;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using System.Linq.Expressions;

namespace IRCore.DataAccess.ADO
{
    public class SubtipoADO : MasterADO<dbSite>
    {
        public SubtipoADO(MasterADOBase ado = null) : base(ado) { }

        public List<EventoSubtipo> ListarTipo(int tipoId, bool apenasEventos = false)
        {
            List<EventoSubtipo> resultado = null;
            if (apenasEventos)
            {
                const string sql = @"SELECT DISTINCT subtipo.ID, subtipo.IR_SubtipoID, subtipo.TipoID, subtipo.Descricao FROM Evento(NOLOCK) evento
                                     LEFT JOIN EventoSubtipo(NOLOCK) subtipo ON evento.SubtipoID = subtipo.IR_SubtipoID
                                     WHERE subtipo.TipoID = @tipoId
                                     ORDER BY Descricao";
                resultado = conSite.Query<EventoSubtipo>(sql, new { tipoId }).ToList();
            }
            else
            {
                const string sql = @"SELECT ID, IR_SubtipoID, TipoID, Descricao FROM EventoSubtipo(NOLOCK)
                                    WHERE TipoID = @tipoId
                                    ORDER BY Descricao";
                resultado = conSite.Query<EventoSubtipo>(sql, new { tipoId }).ToList();
            }
            return resultado;
        }

        /// <summary>
        /// Lista os subtipos no menu mais
        /// </summary>
        /// <param name="naoPertencem"> Ids dos Tipos cujos subtipos não serão listados</param>
        /// <returns>Uma lista de subtipos</returns>
        public List<EventoSubtipo> ListarSubtiposMenuMais()
        {
            List<int> naoPertencem = new List<int>();
            naoPertencem.Add(1);
            naoPertencem.Add(2);
            naoPertencem.Add(5);
            List<EventoSubtipo> lista = new List<EventoSubtipo>();
            lista = (from item in dbSite.EventoSubtipo
                     where !naoPertencem.Contains(item.TipoID ?? 0)
                     orderby item.Descricao
                     select item).AsNoTracking().ToList();
            return lista;
        }

        public EventoSubtipo Consultar(int subtipoId)
        {
            return (from item in dbSite.EventoSubtipo
                    where item.ID == subtipoId
                    select item).AsNoTracking().FirstOrDefault();
        }

        public EventoSubtipo ConsultarPorIrSubtipoId(int IR_subtipoId)
        {
            return (from item in dbSite.EventoSubtipo
                    where item.IR_SubtipoID == IR_subtipoId
                    select item).AsNoTracking().FirstOrDefault();
        }
    }
}
