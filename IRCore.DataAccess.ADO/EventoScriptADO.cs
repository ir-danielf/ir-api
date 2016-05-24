using Dapper;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO
{
    public class EventoScriptADO : MasterADO<dbIngresso>
    {

        public EventoScriptADO(MasterADOBase ado = null) : base(ado, false) { }

        public bool Inserir(int eventoID, string script)
        {
            string sql = @"INSERT INTO EventoScript (EventoID, Script)
                                VALUES (@eventoID, @script)";

            int result = conIngresso.Execute(sql, new
            {
                eventoID = eventoID,
                script = script
            });

            return result > 0;
        }

        public bool Inserir(EventoScriptModel esm)
        {
            string sql = @"INSERT INTO EventoScript (EventoID, Script)
                                VALUES (@eventoID, @script)";

            int result = conIngresso.Execute(sql, new
            {
                eventoID = esm.EventoID,
                script = esm.Script
            });

            return result > 0;
        }

        public bool Alterar(EventoScriptModel esm)
        {
            string sql = @"UPDATE EventoScript SET EventoID = @eventoID, Script = @script WHERE ID = @ID";

            int result = conIngresso.Execute(sql, new
            {
                eventoID = esm.EventoID,
                script = esm.Script,
                ID = esm.ID
            });

            return result > 0;
        }

        public bool Remover(int ID)
        {
            string sql = @"DELETE FROM EventoScript
                           WHERE ID = @ID";

            int result = conIngresso.Execute(sql, new
            {
                ID = ID
            });

            return result > 0;
        }

        public List<EventoScriptModel> ListarScripts(int eventoID)
        {
            var sql = @"SELECT es.ID, e.ID AS EventoID, e.Nome as EventoNome, es.Script
                             FROM tEvento e
                             LEFT JOIN EventoScript (NOLOCK) es ON e.ID = es.EventoID
                             WHERE e.ID = @eventoID";

            return conIngresso.Query<EventoScriptModel>(sql, new
            {
                eventoID = eventoID
            }).ToList();
        }

        public EventoScriptModel Carregar(int id)
        {
            var sql = @"SELECT es.ID, e.ID AS EventoID, e.Nome as EventoNome, es.Script
                             FROM tEvento e
                             LEFT JOIN EventoScript (NOLOCK) es ON e.ID = es.EventoID
                             WHERE es.ID = @id";

            return conIngresso.Query<EventoScriptModel>(sql, new
            {
                id = id
            }).FirstOrDefault();
        }
    }
}
