using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class LugarBO : MasterBO<LugarADO>
    {

        public LugarBO(MasterADOBase ado = null) : base(ado) { }

        public tLugar Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public List<tLugar> ListarTodos(int setorID)
        {
            return ado.ListarSetor(setorID);
        }

    }
}