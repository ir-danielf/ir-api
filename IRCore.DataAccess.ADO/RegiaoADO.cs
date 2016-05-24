using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IRCore.DataAccess.ADO
{
    public class RegiaoADO : MasterADO<dbSite>
    {
        public RegiaoADO(MasterADOBase ado = null) : base(ado, false) { }

        public List<Regiao> Listar()
        {
            return ListarQuery().AsNoTracking().ToList();
        }

        public IPagedList<Regiao> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ListarQuery(busca).ToPagedList(pageNumber, pageSize);
        }

        public Regiao ConsultarGeral()
        {
            IQueryable<Regiao> result = (from item in dbSite.Regiao
                                         where item.IsGeral.Value
                                         select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        public Regiao Consultar(int regionalId)
        {
            IQueryable<Regiao> result = (from item in dbSite.Regiao
                                        where item.ID == regionalId
                                        select item);
            return result.FirstOrDefault();
        }

        private IQueryable<Regiao> ListarQuery(string busca = null)
        {

            IQueryable<Regiao> result = dbSite.Regiao;
            // Filtrar por busca
            if (busca != null)
            {
                result = result.Where(t => t.Nome.Contains(busca));

            }
            result = result.OrderBy(t => t.Nome).OrderByDescending(t => t.IsGeral);
            return result;
        }
    }
}
