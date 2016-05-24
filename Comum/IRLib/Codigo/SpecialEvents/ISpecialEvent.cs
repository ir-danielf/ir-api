using System;
using System.Collections.Generic;

namespace IRLib
{
    public interface IEvento
    {
        int ID { get; set; }
        string Nome { get; set; }
        List<IApresentacao> Apresentacoes { get; set; }
    }

    public interface IApresentacao
    {
        int EventoID { get; set; }
        int ID { get; set; }
        DateTime Horario { get; set; }
        List<IApresentacaoSetor> Setores { get; set; }
    }

    public interface IApresentacaoSetor
    {
        int ID { get; set; }
        int SetorID { get; set; }
        int ApresentacaoID { get; set; }
        string SetorNome { get; set; }
        string Imagem { get; set; }
        List<IPreco> Precos { get; set; }
        IApresentacao Apresentacao { get; set; }
    }

    public interface IPreco
    {
        int ID { get; set; }
        string Nome { get; set; }
        decimal Valor { get; set; }
        int ApresentacaoSetorID { get; set; }
        int QuantidadeDisponivel { get; set; }
        int QuantidadeMaxima { get; set; }
        IApresentacaoSetor ApresentacaoSetor { get; set; }
    }

    public enum enumLoadingType
    {
        QRX,
        Flip,
        Comum
    }
}
