using Dapper;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IRCore.DataAccess.ADO
{
    public class TipoADO : MasterADO<dbIngresso>
    {
        public TipoADO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Lista todos tipos de eventos da base de dados.
        /// </summary>
        /// <returns>Uma lista de tipos.</returns>
        public List<Tipo> Listar(bool apenasEventos = false)
        {
            List<Tipo> query;
            if(apenasEventos)
            {
                /* O metodo utilizava Entity Framework na consulta e realizava o DISTINCT após o ToList(), ou seja, o resultado não era exibido de forma distinta. */
                const string sql = @"SELECT DISTINCT tipo.ID, tipo.IR_TipoID, tipo.Nome, CAST(tipo.Obs AS NVARCHAR(MAX)) Obs FROM EVENTO(NOLOCK) evento
                                        LEFT JOIN EventoSubtipo(NOLOCK) subtipo On evento.SubtipoID = subtipo.IR_SubtipoID
                                        LEFT JOIN Tipo(NOLOCK) tipo on subtipo.TipoID = tipo.IR_TipoID
                                     ORDER BY tipo.Nome";
                query = conSite.Query<Tipo>(sql).ToList();
            }
            else
            {
                const string sql = @"SELECT ID, IR_TipoID, Nome, Obs FROM Tipo(NOLOCK) ORDER BY Nome";
                query = conSite.Query<Tipo>(sql).ToList();
            }
            return query;
        }

        /// <summary>
        /// Consulta um tipo de eventos da base de dados.
        /// </summary>
        /// <returns>Uma objeto tipo.</returns>
        public Tipo Consultar(int tipoId)
        {
            var query = dbSite.Tipo.Where(t => t.IR_TipoID == tipoId).AsNoTracking().FirstOrDefault();
            return query;
        }

        /// <summary>
        /// Método que lista os tEventoTipos com base numa lista de apresentações
        /// </summary>
        /// <param name="apresentacaoIDs"></param>
        /// <returns></returns>
        public List<tEventoTipo> ListarHistoricoInApresentacao(List<int> apresentacaoIDs)
        {
            var ids = (from item in dbIngresso.tEventoTipo
                            join subtipo in dbIngresso.tEventoSubtipo on item.ID equals subtipo.EventoTipoID
                            join evento in dbIngresso.tEvento on subtipo.ID equals evento.EventoSubTipoID
                         where evento.tApresentacao.Any(x => apresentacaoIDs.Contains(x.ID))
                         select item.ID).Distinct().ToList();

            var query = dbIngresso.tEventoTipo.Where(t => ids.Contains(t.ID));

            return query.OrderBy(x => x.Nome).AsNoTracking().ToList();

        }
    }
}
