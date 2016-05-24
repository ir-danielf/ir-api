using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using System.Linq.Expressions;

namespace IRCore.DataAccess.ADO
{
    public class DestaquesLinkRegiaoADO : MasterADO<dbSite>
    {
        public DestaquesLinkRegiaoADO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Lista todos DestaquesLinkRegiao em queryable
        /// </summary>
        /// <returns></returns>
        private IQueryable<DestaqueLinkRegiao> ListarQuery(int id = 0,List<int> idsNaoCarregaveis = null)
        {
            if (id != 0)
            {
                return (from dlr in dbSite.DestaqueLinkRegiao
                        where dlr.TipoID == id
                        orderby dlr.ID
                        select dlr);
            }
            else if (idsNaoCarregaveis != null)
            {
                return (from dlr in dbSite.DestaqueLinkRegiao
                        where idsNaoCarregaveis.All(x => x != dlr.TipoID)
                        orderby dlr.ID
                        select dlr);
            }
            else
        {
                return (from dlr in dbSite.DestaqueLinkRegiao
                        orderby dlr.ID
                        select dlr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regiaoId"></param>
        /// <returns></returns>
        public List<DestaqueLinkRegiao> ListarTodos(int regiaoId = 0)
        {
            List<DestaqueLinkRegiao> lista = new List<DestaqueLinkRegiao>();
            lista = (from item in dbSite.DestaqueLinkRegiao
                     join itemRegiao in dbSite.Regiao on item.RegiaoID equals itemRegiao.ID
                     where (regiaoId > 0 && item.RegiaoID == regiaoId)
                            || (regiaoId == 0 && (itemRegiao.IsGeral ?? false))
                     orderby item.Titulo
                     select item).AsNoTracking().ToList();
            return lista;
        }

        public IPagedList<DestaqueLinkRegiao> Listar(int pageNumber, int pageSize, int tipoId)
        {
            return ListarQuery(tipoId).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<DestaqueLinkRegiao> Listar(int pageNumber, int pageSize, List<int> idsNaoCarregaveis)
        {
            return ListarQuery(idsNaoCarregaveis: idsNaoCarregaveis).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        public DestaqueLinkRegiao Consultar(int destaqueRegiaoLinkId)
        {
            return (from dlr in dbSite.DestaqueLinkRegiao
                    where dlr.ID == destaqueRegiaoLinkId
                    select dlr).AsNoTracking().FirstOrDefault();
        }
      
    }
}
