using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCotaItem
    {
        public int ID { get; set; }
        public bool Novo { get; set; }
        public bool Excluir { get; set; }
        public List<EstruturaFormaPagamento> FormaPagamentoExcluir { get; set; }
        public List<EstruturaFormaPagamento> FormaPagamentoInserir { get; set; }
        public string precoIniciaCom { get; set; }
        public int quantidade { get; set; }
        public int quantidadePorCliente { get; set; }
        public bool validaBin { get; set; }
        public bool validacaoPadrao { get; set; }
        public int parceiroID { get; set; }
        public string textoValidacao { get; set; }
        public int obrigatoriedadeID { get; set; }
        public EstruturaObrigatoriedade obrigatoriedade { get; set; }
        public bool alterado { get; set; }
        public bool distribuido { get; set; }
        // Só sao utilizados no Pesquisar
        public int CotaID { get; set; }
        public string Nome { get; set; }
        public int QuantidadeCota { get; set; }
        public int QuantidadePorClienteCota { get; set; }
        public bool CPFResponsavel { get; set; }
        public string Termo { get; set; }
        public string TermoSite { get; set; }

        public bool Nominal { get; set; }
        public int QuantidadePorCodigo { get; set; }
    }
}
