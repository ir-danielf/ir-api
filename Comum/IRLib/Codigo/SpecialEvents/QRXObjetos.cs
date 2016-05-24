using System;
using System.Collections.Generic;

namespace IRLib
{
    /// <summary>
    /// Estrutura BASICA do Sistema,
    /// Evento
    /// Apresentacao
    /// Setor
    /// Preco
    /// </summary>
    [Serializable]
    public class QRX
    {
        [Serializable]
        public class Tipo
        {
            public Tipo() { this.Apresentacoes = new List<DataHora>(); }
            public int ID { get; set; }
            public string Nome { get; set; }
            public List<DataHora> Apresentacoes { get; set; }
        }

        [Serializable]
        public class DataHora
        {
            public DataHora() { this.Setores = new List<Montadora>(); }
            public int EventoID { get; set; }
            public int ID { get; set; }
            public DateTime Horario { get; set; }
            public List<Montadora> Setores { get; set; }
        }

        [Serializable]
        public class Montadora
        {
            public Montadora() { this.Precos = new List<Carro>(); }
            public int ID { get; set; }
            public int SetorID { get; set; }
            public int ApresentacaoID { get; set; }
            public string SetorNome { get; set; }
            public string Imagem { get; set; }
            public List<Carro> Precos { get; set; }
            public DataHora Apresentacao { get; set; }
        }

        [Serializable]
        public class Carro
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public decimal Valor { get; set; }
            public int ApresentacaoSetorID { get; set; }
            public int QuantidadeDisponivel { get; set; }
            public int QuantidadeMaxima { get; set; }
            public Montadora ApresentacaoSetor { get; set; }
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
            public bool Reservado { get; set; }
        }
    }
}
