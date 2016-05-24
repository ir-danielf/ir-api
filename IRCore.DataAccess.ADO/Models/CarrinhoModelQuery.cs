using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class CarrinhoModelQuery
    {
        public Carrinho carrinho { get; set; }

        public Evento evento { get; set; }

        public Setor setor { get; set; }

        public Apresentacao apresentacao { get; set; }

        public Local local { get; set; }

        public EventoSubtipo subtipo { get; set; }

        public Tipo tipo { get; set; }
    }

    public static class CarrinhoExtensionQuery
    {
        public static Carrinho toCarrinho(this CarrinhoModelQuery carrinhoQuery)
        {
            
            carrinhoQuery.carrinho.SetorObject = carrinhoQuery.setor;
            carrinhoQuery.carrinho.ApresentacaoObject = carrinhoQuery.apresentacao;
            carrinhoQuery.carrinho.EventoObject = carrinhoQuery.evento;
            carrinhoQuery.carrinho.EventoObject.Local = carrinhoQuery.local;
            carrinhoQuery.carrinho.EventoObject.Tipo = carrinhoQuery.tipo;
            carrinhoQuery.carrinho.EventoObject.Subtipo = carrinhoQuery.subtipo;
            carrinhoQuery.carrinho.EventoObject.PrimeiraApresentacao = carrinhoQuery.apresentacao;
            carrinhoQuery.carrinho.EventoObject.QuantidadeApresentacoes = 1;
            return carrinhoQuery.carrinho;
        }

    }

}
