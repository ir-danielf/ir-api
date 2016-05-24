using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IRCore.DataAccess.Model;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class ParceiroMidiaADO : MasterADO<dbIngresso>
    {
        public ParceiroMidiaADO(MasterADOBase ado = null) : base(ado, false) { }


        /// <summary>
        /// Retorna o parceiro atraves de uma url de contexto
        /// </summary>
        /// <param name="urlContexto"></param>
        /// <returns></returns>
        public ParceiroMidia Consultar(string urlContexto)
        {

            string queryString = @"SELECT p.ID, p.Nome, p.UrlContexto, p.EmpresaID, p.PrazoLiberacao, p.Expiracao, p.PaginaTitulo, p.PaginaTexto, p.PaginaRodape, p.ImagemLogo, p.IngressosPorVoucher,p.PoliticaTroca
                                     FROM ParceiroMidia (NOLOCK) p 
                                    WHERE p.UrlContexto = @urlContexto";

            var query = conIngresso.Query<ParceiroMidia>(queryString, new
            {
                @urlContexto = urlContexto
            });
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Retorna o parceiro atraves de uma url de contexto
        /// </summary>
        /// <param name="parceiroId"></param>
        /// <returns></returns>
        public ParceiroMidia Consultar(int parceiroId)
        {

            string queryString = @"SELECT p.ID, p.Nome, p.UrlContexto, p.EmpresaID, p.PrazoLiberacao, p.Expiracao, p.PaginaTitulo, p.PaginaTexto, p.PaginaRodape, p.ImagemLogo, p.IngressosPorVoucher,p.PoliticaTroca,
                                          e.ID, e.Nome, e.ContatoNome, e.ContatoCargo, e.Endereco, e.Cidade, e.Estado, e.CEP, e.DDDTelefone, e.Telefone, e.DDDFax, e.Fax, e.Email, e.Website, e.Obs, e.EmpresaVende, e.EmpresaPromove, e.RegionalID, e.TaxaMaximaEmpresa, e.BannerPadraoSite
                                     FROM ParceiroMidia (NOLOCK) p 
                               INNER JOIN tEmpresa (NOLOCK) e ON p.EmpresaID = e.ID
                                    WHERE p.ID = @parceiroId";

            var query = conIngresso.Query<ParceiroMidia, tEmpresa, ParceiroMidia>(queryString, (parceiro, empresa) =>
            {
                parceiro.tEmpresa = empresa;
                return parceiro;
            }, new
            {
                @parceiroId = parceiroId
            });
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<ParceiroMidia> Listar(int pageNumber, int pageSize, string[] busca = null, int empresaID = 0)
        {
			return ListarQuery(busca, empresaID).AsNoTracking().ToPagedList(pageNumber, pageSize).SelectPagedList(t => t.toParceiroMidia());
        }

        /// <summary>
        /// Lista
        /// </summary>
        /// <param name="busca"></param>
        /// <returns></returns>
        public List<ParceiroMidia> Listar(string[] busca = null)
        {
            return ListarQuery(busca).AsNoTracking().ToList().Select(t => t.toParceiroMidia()).ToList();
        }

        public List<ParceiroMidia> Listar(List<int> parceiroIDs)
        {
            IQueryable<ParceiroMidiaModelQuery> result = ConsultarComMapeamento();

            result = result.Where(t => parceiroIDs.Contains(t.parceiroMedia.ID));
            result = result.OrderBy(t => t.parceiroMedia.Nome);
            return result.AsNoTracking().ToList().Select(t => t.toParceiroMidia()).ToList();
        }

        public List<ParceiroMidia> Listar(int empresaID)
        {
            IQueryable<ParceiroMidiaModelQuery> result = ConsultarComMapeamento();

            result = result.Where(t => t.parceiroMedia.EmpresaID == empresaID);
            result = result.OrderBy(t => t.parceiroMedia.Nome);

            return result.AsNoTracking().ToList().Select(t => t.toParceiroMidia()).ToList();
		}

        private IQueryable<ParceiroMidiaModelQuery> ConsultarComMapeamento()
        {
            IQueryable<ParceiroMidiaModelQuery> result = (from e in dbIngresso.ParceiroMidia.Include(i => i.tEmpresa)
                                                select new ParceiroMidiaModelQuery()
                                                {
                                                    parceiroMedia = e,
                                                    empresa = e.tEmpresa,
                                                    apresentacaoSetorIDs = e.tIngresso.Where(t => t.Status == "B").Select(t => new ParceiroMidiaApresentacaoSetorModelQuery() { setorID = t.SetorID.Value, apresentacaoID = t.ApresentacaoID.Value }).Distinct().ToList()
                                                   
                                                });

            return result;

        }

        private IQueryable<ParceiroMidiaModelQuery> ListarQuery(string[] buscas = null, int empresaID = 0)
        {

            IQueryable<ParceiroMidiaModelQuery> result = ConsultarComMapeamento();


            // Filtrar por busca
            
            if (buscas!=null && buscas.Length>0)
            {
                foreach(string busca in buscas){
                    result = result.Where(t => t.parceiroMedia.Nome.Contains(busca)
                                       || (t.parceiroMedia.tEmpresa != null && t.parceiroMedia.tEmpresa.Nome.Contains(busca)) && (t.parceiroMedia.EmpresaID == empresaID || empresaID == 0));
                }

            }
            else
            {
                result = result.Where(t => t.parceiroMedia.EmpresaID == empresaID || empresaID == 0);
            }
            result = result.OrderBy(t => t.parceiroMedia.Nome);
            return result;
        }
    }
}
