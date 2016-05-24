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
    public class DestaquesLinkRegiaoBO : MasterBO<DestaquesLinkRegiaoADO>
    {

        public DestaquesLinkRegiaoBO(MasterADOBase ado = null) : base(ado) { }


        /// <summary>
        /// Remover Destaque
        /// </summary>
        /// <param name="destaque"></param>
        public void Remover(DestaqueLinkRegiao destaque)
        {
            ado.Remover(destaque);
        }

        /// <summary>
        /// Salvar Destaque 
        /// </summary>
        /// <param name="destaque"></param>
        public void Salvar(DestaqueLinkRegiao destaque,int usuarioLogadoId)
        {
            ado.Salvar(destaque);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regiaoId"></param>
        /// <returns></returns>
        public List<DestaqueLinkRegiao> ListarTodos(int regiaoId = 0)
        {
            TipoBO tBO = new TipoBO(ado);
            List<DestaqueLinkRegiao> lista = ado.ListarTodos(regiaoId).OrderBy(x => x.Titulo).ToList();
            lista.ForEach(x => x.Tipo = tBO.Consultar(x.TipoID));
            return lista;
        }

        /// <summary>
        /// Lista um pagedlist de DestaqueLinRegiao
        /// </summary>
        /// <param name="pageNumber">Numero da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns></returns>
        public IPagedList<DestaqueLinkRegiao> Listar(int pageNumber, int pageSize,int tipoId)
        {
            return ado.Listar(pageNumber, pageSize,tipoId);
        }

        /// <summary>
        /// Lista Destaque Link Região Paginado
        /// </summary>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="tipoIdsNaoCarregaveis">Ids que não são carregados</param>
        /// <returns></returns>
        public IPagedList<DestaqueLinkRegiao> Listar(int pageNumber, int pageSize, List<int> tipoIdsNaoCarregaveis)
        {
            return ado.Listar(pageNumber, pageSize,tipoIdsNaoCarregaveis);
        }

        /// <summary>
        /// Método que retorna um destaque
        /// </summary>
        /// <param name="destaqueLinkRegiaoid"></param>
        /// <returns></returns>
        public DestaqueLinkRegiao Consultar(int destaqueLinkRegiaoid)
        {
            return ado.Consultar(destaqueLinkRegiaoid);
        }
    }
}
