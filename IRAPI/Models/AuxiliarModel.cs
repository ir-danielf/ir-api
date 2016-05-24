using IRCore.BusinessObject;
using IRCore.DataAccess.ADO.Estrutura;
using System.Collections.Generic;

namespace IRAPI.Models
{
    public class PagedListModel<T>
    {
        public int FirstItemOnPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public int LastItemOnPage { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public List<T> Itens { get; set; }
    }

    public class CompraBOModel
    {

        public CompraBOModel(MasterADOBase ado): this(1, ado)
        {
        }

        public CompraBOModel(int siteId, MasterADOBase ado)
        {
            carrinhoBO = new CarrinhoBO(ado);
            clienteBO = new ClienteBO(siteId, ado);
            entregaBO = new EntregaBO(ado);
            valeIngressoBO = new ValeIngressoBO(ado);
            cotaBO = new CotaBO(ado);
        }

        public CarrinhoBO carrinhoBO { get; set; }
        public ClienteBO clienteBO { get; set; }
        public ValeIngressoBO valeIngressoBO { get; set; }
        public EntregaBO entregaBO { get; set; }
        public CotaBO cotaBO { get; set; }
    }
}