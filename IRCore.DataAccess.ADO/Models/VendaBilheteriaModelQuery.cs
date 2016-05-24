using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class VendaBilheteriaModelQuery
    {
        public tVendaBilheteria VendaBilheteria { get; set; }
        public tEntregaControle EntregaControle { get; set; }
        public tEntrega Entrega { get; set; }
        public List<VendaBilheteriaFormaPagamentoModelQuery> VendaBilheteriaPagamento { get; set; }
        public List<IngressoModelQuery> Ingressos { get; set; }
        
    }

    public class IngressoModelQuery
    {
        public tIngresso Ingresso { get; set; }
        public tEvento Evento { get; set; }
        public tApresentacao Apresentacao { get; set; }
        public tSetor Setor { get; set; }
        public tPreco Preco { get; set; }
        public tLocal Local { get; set; }
        public List<EventoMidiaModelQuery> EventosMidias { get; set; }
    }

    public class VendaBilheteriaFormaPagamentoModelQuery
    {
        public tVendaBilheteriaFormaPagamento FormaPagamento { get; set; }
        public tCartao Cartao { get; set; }
        public ValeIngressoModelQuery ValeIngresso { get; set; }
    }

    public class EventoMidiaModelQuery
    {
        public EventoMidia EventoMidia { get; set; }
        public EventoTipoMidia EventoTipoMidia { get; set; }
    }


    public static class VendaBilheteriaExtensionQuery
    {
        public static tVendaBilheteria toVendaBilheteria(this VendaBilheteriaModelQuery vendaBilheteriaModelQuery)
        {
            vendaBilheteriaModelQuery.VendaBilheteria.EntregaControle = vendaBilheteriaModelQuery.EntregaControle;
            vendaBilheteriaModelQuery.VendaBilheteria.EntregaControle.Entrega = vendaBilheteriaModelQuery.Entrega;
            vendaBilheteriaModelQuery.VendaBilheteria.tVendaBilheteriaFormaPagamento = vendaBilheteriaModelQuery.VendaBilheteriaPagamento.Select(x=>x.toVendaBilheteriaFormaPagamento()).ToList();
            vendaBilheteriaModelQuery.VendaBilheteria.tIngresso = vendaBilheteriaModelQuery.Ingressos.Select(x=>x.toIngresso()).ToList();
            
            return vendaBilheteriaModelQuery.VendaBilheteria;
        }

        public static tIngresso toIngresso(this IngressoModelQuery ingressoModelQuery)
        {
            ingressoModelQuery.Ingresso.tEvento = ingressoModelQuery.Evento;
            ingressoModelQuery.Ingresso.tEvento.EventoMidia = ingressoModelQuery.EventosMidias.Select(x=>x.toEventoMidia()).ToList();
            ingressoModelQuery.Ingresso.tEvento.tLocal = ingressoModelQuery.Local;
            ingressoModelQuery.Ingresso.tApresentacao = ingressoModelQuery.Apresentacao;
            ingressoModelQuery.Ingresso.tSetor = ingressoModelQuery.Setor;
            ingressoModelQuery.Ingresso.tPreco = ingressoModelQuery.Preco;
            ingressoModelQuery.Ingresso.tLocal = ingressoModelQuery.Local;
            return ingressoModelQuery.Ingresso;
        }

        public static tVendaBilheteriaFormaPagamento toVendaBilheteriaFormaPagamento(this VendaBilheteriaFormaPagamentoModelQuery formaPagamentoModelQuery)
        {
            formaPagamentoModelQuery.FormaPagamento = formaPagamentoModelQuery.FormaPagamento;
            formaPagamentoModelQuery.FormaPagamento.tCartao = formaPagamentoModelQuery.Cartao;
            formaPagamentoModelQuery.FormaPagamento.tValeIngresso = formaPagamentoModelQuery.ValeIngresso != null ? formaPagamentoModelQuery.ValeIngresso.toValeIngresso():null;
            return formaPagamentoModelQuery.FormaPagamento;
        }

        public static EventoMidia toEventoMidia(this EventoMidiaModelQuery eventoMidiaModelQuery)
        {
            eventoMidiaModelQuery.EventoMidia.EventoTipoMidia = eventoMidiaModelQuery.EventoTipoMidia;
            return eventoMidiaModelQuery.EventoMidia;
        }
    }

}
