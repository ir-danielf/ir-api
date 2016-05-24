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
    public class RegionalBO : MasterBO<RegionalADO>
    {
        public RegionalBO(MasterADOBase ado = null) : base(ado) { }

        public List<tRegional> Listar()
        {
            return ado.Listar();
        }
        public IPagedList<tRegional> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ado.Listar(pageNumber, pageSize, busca);
        }

        public tRegional Consultar(int regionalId) {
            return ado.Consultar(regionalId);
        }
    }
}
