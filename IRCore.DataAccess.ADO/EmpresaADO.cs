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

namespace IRCore.DataAccess.ADO
{
    public class EmpresaADO : MasterADO<dbIngresso>
    {
        public EmpresaADO(MasterADOBase ado = null) : base(ado) { }

        public tEmpresa Consultar(int empresaId)
        {
            tEmpresa result = (from item in dbIngresso.tEmpresa
                               where item.ID == empresaId
                               select item).AsNoTracking().FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Lista
        /// </summary>
        /// <param name="busca"></param>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public List<tEmpresa> Listar(string busca = null, int empresaId = 0)
        {
            return ListarQuery(busca, empresaId).AsNoTracking().ToList();
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public IPagedList<tEmpresa> Listar(int pageNumber, int pageSize, string busca = null, int empresaId = 0)
        {
            return ListarQuery(busca, empresaId).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="busca"></param>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        private IQueryable<tEmpresa> ListarQuery(string busca = null, int empresaId = 0)
        {

            IQueryable<tEmpresa> result = dbIngresso.tEmpresa;
            // Filtrar por busca
            if (!string.IsNullOrEmpty(busca))
            {
                result = result.Where(t => t.ID == empresaId
                                       || (t.Nome.Contains(busca))
                                       || (t.Email.Contains(busca))
                                       || (t.Cidade.Contains(busca)));

            }

            result = result.OrderBy(t => t.Nome).OrderBy(t => t.Nome);
            return result;
        }

    }
}
