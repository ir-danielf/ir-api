using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO
{
    public class PerfisADO : MasterADO<dbIngresso>
    {
        public PerfisADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public List<ProcListaPrefisUsuario_Result> ListarPerfisUsuario(int usuarioID)
        {
            return dbIngresso.ProcListaPrefisUsuario(usuarioID).ToList();
        }

        public List<int> ListarEventosIDPorRegional(int regionalID)
        {
            List<int?> idsnull = dbIngresso.ProcListaEventoIDPorRegional(regionalID).ToList();
            List<int> ids = new List<int>();
            foreach(int? i in idsnull)
            {
                if(i != null)
                {
                    ids.Add(i??0);
                }
            }
            return ids;
        }

        public List<int> ListarEventosIDPorEmpresa(int empresaID)
        {
            return (from evento in dbIngresso.tEvento
                    where evento.tLocal.EmpresaID == empresaID
                    select evento.ID).ToList();
        }

        public List<int> ListarEventosIDPorLocal(int localID)
        {
            return (from evento in dbIngresso.tEvento
                    where evento.LocalID == localID
                    select evento.ID).ToList();
        }
    }
}
