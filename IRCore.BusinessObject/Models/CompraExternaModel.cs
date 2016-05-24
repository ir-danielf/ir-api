using System.Collections.Generic;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Models;

namespace IRCore.BusinessObject.Models
{
    public class CompraExternaModel
    {
        public List<CarrinhoExterno> Carrinho { get; set; }

        public string SenhaVenda { get; set; }

        public InfosObrigatoriasIngresso InfosObrigatoriasIngresso { get; set; }

        public CompraExternaModel()
        {
            Carrinho = new List<CarrinhoExterno>();
        }
    }
}