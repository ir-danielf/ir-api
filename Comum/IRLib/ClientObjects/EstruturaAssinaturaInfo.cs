using System;
using System.Collections.Generic;
using System.Drawing;
namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaAssinaturaInfo
    {
        public List<int> ApresentacoesID = new List<int>();
        public int PacoteID;
        public int SetorID;
        public int PrecoID;
        public String CodigoLugar;
    }

    [Serializable]
    public class EstruturaAssinaturaDetalhe
    {
        public String AssinaturaNome;
        public String AssinaturaPreco;
        public String AssinaturaSetor;
        public String AssinaturaFileira;
        public String AssinaturaPoltrona;
    }

    [Serializable]
    public class EstruturaAssinatura
    {
        public int ID { get; set; }
        public String Nome { get; set; }
        public String TipoCancelamento { get; set; }
        public String AssinaturaTipo { get; set; }
        public bool Ativo { get; set; }
        public String Local { get; set; }
        public String Bloqueio { get; set; }
        public String DesistenciaBloqueio { get; set; }


    }

    [Serializable]
    public class EstruturaAssinaturaAno
    {
        public int ID { get; set; }
        public int Ano { get; set; }
        public string AnoInfo { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaEvento
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public bool Incluir { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaApresentacao
    {
        public int ID { get; set; }
        public string Evento { get; set; }
        public string Horario { get; set; }
        public bool Incluir { get; set; }
        public int DiaDaSemana { get; set; }

    }

    [Serializable]
    public class EstruturaAssinaturaSetor
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public bool Incluir { get; set; }
        public int QtdApresentacoes { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaPreco
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public bool Incluir { get; set; }
        public int QtdSetores { get; set; }
        public int QtdApresentacao { get; set; }
        public decimal Valor { get; set; }
        public decimal TaxaConvenienciaValorTotal { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaFormaPagamento
    {
        public int ID { get; set; }
        public int FormaPagamentoID { get; set; }
        public Enumerators.TipoAcaoCanal Acao { get; set; }
        public Bitmap ImagemAcao { get; set; }
        public string FormaPagamento { get; set; }
        public string Bandeira { get; set; }
        public string Tipo { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaCanal
    {
        public int ID { get; set; }
        public Enumerators.TipoAcaoCanal Acao { get; set; }
        public Bitmap ImagemAcao { get; set; }
        public string Regional { get; set; }
        public string Empresa { get; set; }
        public int CanalID { get; set; }
        public string Canal { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaAjuda
    {
        public int ID { get; set; }
        public int AssinaturaTipoID { get; set; }
        public string AssinaturaTipo { get; set; }
        public string NomePagina { get; set; }
        public string Conteudo { get; set; }
    }

    [Serializable]
    public class EstruturaAssinaturaIngresso
    {

        public int IngressoID { get; set; }
        public string Codigo { get; set; }
        public int EventoID { get; set; }
        public int ApresentacaoID { get; set; }
        public string Status { get; set; }
        public int BloqueioID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int SetorID { get; set; }
        public int LugarID { get; set; }
        public int AssinaturaClienteID { get; set; }
        public AssinaturaCliente.EnumAcao Acao { get; set; }
        public int DesistenciaBloqueioID { get; set; }

    }

    [Serializable]
    public class EstruturaAssinaturaBloqueio
    {
        public int AssinaturaClienteID { get; set; }
        public int LugarID { get; set; }
        public int ClienteID { get; set; }
        public int SetorID { get; set; }
        public int AssinaturaID { get; set; }
        public int AssinaturaAnoID { get; set; }
        public int AssinaturaBloqueioID { get; set; }
        public int AssinaturaDesistenciaID { get; set; }
        public int AssinaturaExtintoID { get; set; }
        public string NomeAssinatura { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }
        public string Assinante { get; set; }
        public string CPF { get; set; }
        public string Preco { get; set; }
        public string StatusDescricao
        {
            get
            {
                return Utils.Enums.GetDescription<Assinatura.EnumStatusVisual>(Status);
            }
        }
        public string StatusAssinatura { get; set; }
        public string StatusIngresso { get; set; }
        public int BloqueioID { get; set; }
        public int BloqueioCorID { get; set; }
        public string BloqueioUtilizado { get; set; }
        public bool Selecionar { get; set; }
        public int PosicaoX { get; set; }
        public int PosicaoY { get; set; }
        public Assinatura.EnumStatusVisual Status { get; set; }

    }

    [Serializable]
    public class EstruturaAssinaturaStatus
    {
        public char StatusChar { get; set; }
        public string Status { get; set; }
    }
    [Serializable]
    public class EstruturaAssinaturaValores
    {
        public string Setor { get; set; }
        public string Assinatura { get; set; }
        public string PrecoTipo { get; set; }
        public decimal Preco { get; set; }
        public decimal Soma { get; set; }
        public int Quantidade { get; set; }
    }
}
