using System;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public class AgrupamentoVendaSimples
    {
        public AgrupamentoVendaSimples()
        {
            Evento = new EventoSimples();
            Pacotes = new List<PacoteSimples>();
            Camping = new EventoSimples();
        }
        public EventoSimples Camping { get; set; }
        public EventoSimples Evento { get; set; }
        public List<PacoteSimples> Pacotes { get; set; }
    }

    public class EventoSimples
    {
        public EventoSimples()
        {
            this.Apresentacoes = new List<ApresentacaoSimples>();
        }

        public int EventoID { get; set; }
        public string Nome { get; set; }
        public bool DisponivelVenda { get; set; }
        public List<ApresentacaoSimples> Apresentacoes { get; set; }
        public string Release { get; set; }
        public string PrimeiraApresentacao { get; set; }
        public string UltimaApresentacao { get; set; }
    }

    public class ApresentacaoSimples
    {
        public ApresentacaoSimples()
        {
            this.Setores = new List<SetorSimples>();
        }

        public int ApresentacaoID { get; set; }
        public DateTime Horario { get; set; }
        public string HorarioFormatado { get; set; }
        public string Dia { get; set; }
        public List<SetorSimples> Setores { get; set; }
    }

    public class SetorSimples
    {
        public SetorSimples()
        {
            Precos = new List<PrecoSimples>();
        }

        public int SetorID { get; set; }
        public string Nome { get; set; }
        public int QtdeDisponivel { get; set; }
        public string LugarTipo { get; set; }
        public int GrupoID { get; set; }
        public string Horario { get; set; }
        public List<PrecoSimples> Precos { get; set; }

        public string Cabecalho
        {
            get { return string.Format("st{0}.jpg", this.SetorID.ToString("000000")); }
        }
    }

    public class PrecoSimples
    {
        public int PrecoID { get; set; }
        public int SetorID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string ValorFormatado
        {
            get
            {
                return this.Valor.ToString("c");
            }
        }
        public int QtdeDisponivel { get; set; }
    }

    public class PacoteSimples
    {
        public int PacoteID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string ValorFormatado
        {
            get
            {
                return this.Valor.ToString("c");
            }
        }
    }
}