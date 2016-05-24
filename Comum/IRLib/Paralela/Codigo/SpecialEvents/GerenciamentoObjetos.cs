using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{
    /// <summary>
    /// Estrutura BASICA do Sistema,
    /// Evento
    /// Apresentacao
    /// Setor
    /// Preco
    /// </summary>
    [Serializable]
    public class Gerenciamento
    {
        [Serializable]
        public class Data
        {
            public Data() { this.Precos = new List<Precos>(); }
            public int EventoID { get; set; }
            public int ApresentacaoSetorID { get; set; }
            public string EventoNome { get; set; }
            public int ApresentacaoID { get; set; }
            public int SetorID { get; set; }
            public DateTime DataApresentacao { get; set; }
            public List<Precos> Precos { get; set; }
        }

        [Serializable]
        public class Precos
        {
            public Precos() { }
            public int SetorID { get; set; }
            public int EventoID { get; set; }
            public string Evento { get; set; }
            public int ApresentacaoID { get; set; }
            public DateTime DataApresentacao { get; set; }
            public int PrecoID { get; set; }
            public int PrecoTipoID { get; set; }
            public int QuantidadeDisponivel { get; set; }
            public int QuantidadeMaxima { get; set; }
            public int GerenciamentoIngressoID { get; set; }
            public decimal Valor { get; set; }
            public String Nome { get; set; }
            public String Horario { get; set; }
            public String Label { get; set; }
        }



        /// <summary>
        /// Retorno para verificação dos comboboxes de reserva no caso do sistema
        ///         e inclusao da carrinho no caso do site
        /// </summary>
        [Serializable]
        public class RetornoReserva
        {
            public int EventoID { get; set; }
            public int ApresentacaoID { get; set; }
            public int ApresentacaoSetorID { get; set; }
            public int SetorID { get; set; }
            public int PrecoID { get; set; }
            public int GerenciamentoIngressoID { get; set; }
            public bool Reservado { get; set; }
        }
    }
}
