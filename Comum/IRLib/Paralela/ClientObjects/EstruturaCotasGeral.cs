using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCotasGeral
    {
        public List<EstruturaSetores> listaSetores { get; set; }
        public bool Novo { get; set; }
        public bool Excluir { get; set; }
        public int CotaID { get; set; }
        public List<int> listaEventoID { get; set; }
        public List<int> listaApresentacoesID { get; set; }
        public List<EstruturaFormaPagamento> FormaPagamentoExcluir { get; set; }
        public List<EstruturaFormaPagamento> FormaPagamentoInserir { get; set; }
        public string precoIniciaCom { get; set; }
        public int total { get; set; }
        public int totalPorCadastro { get; set; }
        public int quantidade { get; set; }
        public int quantidadePorCliente { get; set; }
        public bool validaBin { get; set; }
        public bool validacaoPadrao { get; set; }
        public int parceiroID { get; set; }
        public string textoValidacao { get; set; }
        public string codigoPromocional { get; set; }
        public int obrigatoriedadeID { get; set; }
        public EstruturaObrigatoriedade obrigatoriedade { get; set; }


    }
}
