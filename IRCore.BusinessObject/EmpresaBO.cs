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
    public class EmpresaBO : MasterBO<EmpresaADO>
    {
        public EmpresaBO(MasterADOBase ado = null) : base(ado) { }

        public List<tEmpresa> Listar()
        {
            return ado.Listar();
        }
        public IPagedList<tEmpresa> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ado.Listar(pageNumber, pageSize, busca);
        }

        public tEmpresa Consultar(int empresaId)
        {
            return ado.Consultar(empresaId);
        }

        public void Salvar(tEmpresa empresa, int usuarioLogadoId)
        {
            ado.Salvar(empresa, usuarioLogadoId);
        }
    }
}
