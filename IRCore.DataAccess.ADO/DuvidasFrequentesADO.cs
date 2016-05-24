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
    public class DuvidasFrequentesADO : MasterADO<dbSite>
    {

        public DuvidasFrequentesADO(MasterADOBase ado = null) : base(ado) { }



        /// <summary>
        /// Médoto que lista todas as duvidas frequentes
        /// </summary>
        /// <returns></returns>
        public List<DuvidasFrequentes> Listar()
        {
            var result = (from item in dbSite.DuvidasFrequentes
                          select item).OrderBy(t => t.ID).AsNoTracking().ToList();

            return result;
        }

        /// <summary>
        /// Método que retorna uma Duvida Frequente
        /// </summary>
        /// <param name="duvidaFrequenteId">Id da dúvida frequente consultada</param>
        /// <returns></returns>
        public DuvidasFrequentes Consultar(int duvidaFrequenteId)
        {
            DuvidasFrequentes result = (from item in dbSite.DuvidasFrequentes
                                        where item.ID == duvidaFrequenteId
                                        select item).AsNoTracking().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<DuvidasFrequentes> Listar(int pageNumber, int pageSize)
        {
            return ListarQuery().AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="busca"></param>
        /// <returns></returns>
        private IQueryable<DuvidasFrequentes> ListarQuery()
        {
            return (from df in dbSite.DuvidasFrequentes
                    orderby df.ID
                    select df);


        }

    }
}
