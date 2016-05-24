using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{

    public class EventoSimplificadoModelQuery
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Apresentacao { get; set; }
        public string Status { get; set; }
        public string Local { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string DataCancelamento { get; set; }

    }

    public class EventoModelQuery
    {
        public Evento evento { get; set; }

        public List<ApresentacaoModelQuery> apresentacoes { get; set; }

        public Local local { get; set; }
        public EventoSubtipo subtipo { get; set; }
        public Tipo tipo { get; set; }
        public double? distancia { get; set; }

    }

    public class ApresentacaoModelQuery
    {
        public Apresentacao apresentacao { get; set; }
        public Preco menorPreco { get; set; }
        public Preco maiorPreco { get; set; }
        public int qtdeDisponivel { get; set; }

    }

    public static class EventoExtensionQuery
    {
        public static Evento toEvento(this EventoModelQuery eventoQuery)
        {
            eventoQuery.evento.Local = eventoQuery.local;
            if (eventoQuery.distancia != null)
            {
                eventoQuery.evento.Local.Distancia = eventoQuery.distancia.Value;
            }
            eventoQuery.evento.Tipo = eventoQuery.tipo;
            eventoQuery.evento.Subtipo = eventoQuery.subtipo;

            eventoQuery.evento.QuantidadeApresentacoes = eventoQuery.apresentacoes.Count;
            eventoQuery.evento.Apresentacao = eventoQuery.apresentacoes.Select(t => t.toApresentacao()).ToList();
            eventoQuery.evento.PrimeiraApresentacao = eventoQuery.evento.Apresentacao.FirstOrDefault();
            if (eventoQuery.evento.QuantidadeApresentacoes > 1)
            {
                eventoQuery.evento.UltimaApresentacao = eventoQuery.evento.Apresentacao.LastOrDefault();
            }


            eventoQuery.evento.QtdeDisponivel = eventoQuery.apresentacoes.Sum(t => t.qtdeDisponivel);
            eventoQuery.evento.MaiorPreco = eventoQuery.apresentacoes.Where(t => t.maiorPreco != null).Select(t => t.maiorPreco).OrderByDescending(t => t.Valor).FirstOrDefault();
            eventoQuery.evento.MenorPreco = eventoQuery.apresentacoes.Where(t => t.maiorPreco != null).Select(t => t.menorPreco).OrderBy(t => t.Valor).FirstOrDefault();

            return eventoQuery.evento;
        }

        public static Apresentacao toApresentacao(this ApresentacaoModelQuery apresntacaoQuery)
        {

            apresntacaoQuery.apresentacao.MaiorPreco = apresntacaoQuery.maiorPreco;
            apresntacaoQuery.apresentacao.MenorPreco = apresntacaoQuery.menorPreco;
            apresntacaoQuery.apresentacao.QtdeDisponivel = apresntacaoQuery.qtdeDisponivel;

            return apresntacaoQuery.apresentacao;
        }

    }

    public class EventoCancelarMassaModelQuery
    {
        public int ID { get; set; }
        public string EventoNome { get; set; }
        public string LocalNome { get; set; }
        public string EmpresaNome { get; set; }
        public DateTime Horario { get; set; }
        public bool PodeCancelar { get; set; }
    }

    public class CancelamentoMassaModelQuery
    {
        public int ID { get; set; }
        public DateTime DataCancelamento { get; set; }
        public string CodigoCancelamento { get; set; }
        public string Empresa { get; set; }
        public string Evento { get; set; }
        public string Local { get; set; }
        public int EventoID { get; set; }
        public int DatasCanceladas { get; set; }
    }

    public class InformacaoVendaCancelarMassa
    {

    }


    public class InformacaoVendaBasicasCancelarMassa
    {
        public int Identificados { get; set; }
        public int NaoIdentificados { get; set; }
        public int Presencial { get; set; }
        public int Remoto { get; set; }
        public int Impressos { get; set; }
        public int NaoImpressos { get; set; }

        public string Horario { get; set; }
    }

    public class InformacaoVendaFormasPagamento
    {
        public int Quantidade { get; set; }
        public string FormaPagamento { get; set; }
        public string Horario { get; set; }
    }

    public class InformacaoValoresOperacao
    {
        public string Operacao { get; set; }
        public string Descricao { get; set; }
        public string EstornoVia { get; set; }
        public string ContatoCliente { get; set; }
        public string Cancelamento { get; set; }
        public string AcaoIR { get; set; }
        public int Total { get; set; }
    }

    public class ClienteCancelamentoOperacao
    {
        public string CodigoCancelamento { get; set; }
        public string Senha { get; set; }
        public DateTime DataCompra { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Operacao { get; set; }
        public string Evento { get; set; }
        public string Apresentacoes { get; set; }
    }


    public class ConferenciaCancelamento
    {
        public string CodigoCancelamento { get; set; }
        public DateTime DataCancelamento { get; set; }
        public string Evento { get; set; }
        public DateTime Apresentacao { get; set; }
        public string Operacao { get; set; }
        public string Senha { get; set; }
        public string Setor { get; set; }
        public string IngressoCodigo { get; set; }
        public string StatusAtual { get; set; }
        public string NomeCliente { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
    }

}
