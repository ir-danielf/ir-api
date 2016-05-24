using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System.Collections.Generic;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.DataAccess.Models;
using IRLib.ClientObjects;

namespace IRCore.BusinessObject.Models
{
    public class CompraModel
    {
        /// <summary>
        /// Guarda o limite total disponivel de itens no carrinho
        /// </summary>
        public int LimiteTotalItensCarrinho { get { return ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.limiteCarrinho); } }

        public tVendaBilheteria VendaBilheteria { get; set; }

        public List<tEntregaControle> EntregaControles { get; set; }

        public bool EntregaComEndereco { get; set; }
        public int EntregaControleID { get; set; }

        public int ClienteEnderecoID { get; set; }

        public int PDVID { get; set; }

        public List<Carrinho> CarrinhoItens { get; set; }

        public string SessionID { get; set; }

        public Login Login { get; set; }

        public int ClienteID { get { return (Login != null) ? Login.ClienteID : 0; } }

        public List<tValeIngresso> ValeIngressos { get; set; }

        public CompraTotalModel Total { get; set; }

        public List<CompraPagamentoModel> Pagamentos { get; set; }

        public CompraEstruturaVendaModel EstruturaVenda { get; set; }

        public CompraModel()
        {
            CarrinhoItens = new List<Carrinho>();
            RetornoPagamento = null;
        }

        public decimal ValorTotalSeguro { get; set; }

        public CotaPendenteModel StatusCotaPendente { get; set; }

        public EstruturaPagamento.enumRetornoPagamento? RetornoPagamento { get; set; }
    }


    public class ReservaQuantidadeRetorno
    {
        public int IngressoID { get; set; }
        public int PrecoID { get; set; }
        public decimal ValorTaxaConveniencia { get; set; }
        public decimal ValorIngressos { get; set; }
        
    }

}

