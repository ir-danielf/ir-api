using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRCore.BusinessObject
{
    public class DestaqueBO : MasterBO<DestaqueADO>
    {

        public DestaqueBO(MasterADOBase ado = null) : base(ado) { }

        public DestaqueConceitual Consultar(int id, bool loadDependencias = true)
        {
            return ado.Consultar(id);
        }

        /// <summary>
        /// Remover Destaque
        /// </summary>
        /// <param name="usuario"></param>
        public void Remover(DestaqueConceitual destaque, int usuarioLogadoId)
        {
            ado.Remover(destaque, usuarioLogadoId);
        }

        public void Remover(DestaqueConceitualRegiao destaqueRegiao)
        {
            ado.Remover(destaqueRegiao);
        }

        public void Remover(DestaqueConceitual destaque, bool updateDataBase = true, bool withException = false)
        {
            ado.Remover(destaque, updateDataBase, withException);
        }

        public void Remover(DestaqueConceitualRegiao destaque, bool updateDataBase = true, bool withException = false)
        {
            ado.Remover(destaque, updateDataBase, withException);
        }

        /// <summary>
        /// Salvar Destaque
        /// </summary>
        /// <param name="destaque"></param>
        public void Salvar(DestaqueConceitual destaque)
        {
            ado.Salvar(destaque);
        }

        public bool AlterarOrdem(string[] IDs, int regiaoID)
        {
            return ado.AlterarOrdem(IDs, regiaoID);
        }
        public List<DestaqueConceitual> Listar(int regiaoID)
        {
            return ado.Listar(regiaoID);
        }
        public List<DestaqueConceitual> Listar(int regiaoID,bool publicados)
        {
            return ado.Listar(regiaoID,publicados);
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<DestaqueConceitual> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ado.Listar(pageNumber, pageSize, busca, 0);
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <param name="destaqueId"></param>
        /// <returns></returns>
        public IPagedList<DestaqueConceitual> Listar(int pageNumber, int pageSize, string busca = null, int destaqueId = 0)
        {
            return ado.Listar(pageNumber, pageSize, busca, destaqueId);
        }

        public int getNewOrdem(int regiaoID)
        {
            return ado.getNewOrdem(regiaoID);
        }
    }
}
