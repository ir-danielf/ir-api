using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class PerfisBO : MasterBO<PerfisADO>
    {

        public PerfisBO(MasterADOBase ado) : base(ado) { }
        public List<ProcListaPrefisUsuario_Result> ListarPerfisUsuario(int usuarioID)
        {
            return ado.ListarPerfisUsuario(usuarioID);
        }
        public List<int> ListarEventosIDPorRegional(int regionalID)
        {
            return ado.ListarEventosIDPorRegional(regionalID);
        }
        
        public List<int> ListarEventosIDPorEmpresa(int empresaID)
        {
            return ado.ListarEventosIDPorEmpresa(empresaID);
        }

        public List<int> ListarEventosIDPorLocal(int localID)
        {
            return ado.ListarEventosIDPorLocal(localID);
        }
    }
}
