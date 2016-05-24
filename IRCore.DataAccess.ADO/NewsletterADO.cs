using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO
{
    public class NewsletterADO : MasterADO<dbSite>
    {
        public NewsletterADO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="busca"></param>
        /// <returns></returns>
        private IQueryable<NewsAssinante> ListarQuery(string busca = null, int? eventoId = null)
        {

            IQueryable<NewsAssinante> result = (from item in dbSite.NewsAssinante
                                                     orderby item.Nome
                                                     select item);
            // Filtrar por busca
            if (!string.IsNullOrEmpty(busca))
            {
                result = result.Where(t => (t.Nome.ToLower().Contains(busca.ToLower()))
                                       || (t.Email.ToLower().Contains(busca.ToLower())));

            }
            result = result.Where(t => t.EventoID == eventoId);

            return result;
        }


        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<NewsAssinante> Listar(int pageNumber, int pageSize, string busca = null, int? eventoId = null)
        {
            return ListarQuery(busca, eventoId).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public List<NewsAssinante> Listar(string busca = null, int? eventoId = null)
        {
            return ListarQuery(busca, eventoId).AsNoTracking().ToList();
        }

        /// <summary>
        /// Consulta uma news na base de dados
        /// </summary>
        /// <param name="destaqueId"></param>
        /// <returns></returns>
        public NewsAssinante Consultar(int newsId)
        {
            return (from item in dbSite.NewsAssinante
                    where item.ID == newsId
                    select item).AsNoTracking().FirstOrDefault();
        }

        public NewsAssinante Consultar(string email, int? eventoID)
        {
            return (from item in dbSite.NewsAssinante
                    where item.Email == email && item.EventoID == eventoID
                    select item).AsNoTracking().FirstOrDefault();
        }
    }
}
