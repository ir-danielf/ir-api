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
    public class RegionalADO : MasterADO<dbIngresso>
    {
        public RegionalADO(MasterADOBase ado = null) : base(ado) { }

        public List<tRegional> Listar()
        {
            return ListarQuery().AsNoTracking().ToList();
        }

        public IPagedList<tRegional> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ListarQuery(busca).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        public tRegional Consultar(int regionalId)
        {
            IQueryable<tRegional> result = (from item in dbIngresso.tRegional
                                        where item.ID == regionalId
                                        select item);
            result = result.OrderBy(t => t.ID);
            return result.AsNoTracking().FirstOrDefault();
        }

        private IQueryable<tRegional> ListarQuery(string busca = null)
        {

            IQueryable<tRegional> result = dbIngresso.tRegional;
            // Filtrar por busca
            if (busca != null)
            {
                result = result.Where(t => t.Nome.Contains(busca));

            }
            result = result.OrderBy(t => t.ID);
            return result;
        }
    }
}
