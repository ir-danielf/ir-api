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
    public class RegiaoBO : MasterBO<RegiaoADO>
    {
        public RegiaoBO(MasterADOBase ado = null) : base(ado) { }

        public List<Regiao> Listar()
        {
            return ado.Listar();
        }
        public IPagedList<Regiao> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ado.Listar(pageNumber, pageSize, busca);
        }

        public Regiao Consultar(int regionalId)
        {
            return ado.Consultar(regionalId);
        }

        public Regiao ConsultarGeral()
        {
            return ado.ConsultarGeral();
        }
    }
}
