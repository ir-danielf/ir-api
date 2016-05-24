using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotasSetor
    {
        public int CotaID { get; set; }
        public bool Novo { get; set; }
        public bool Excluir { get; set; }
        public List<int> listaEventoID { get; set; }
        public List<int> listaApresentacoesID { get; set; }
        public int setorID { get; set; }
        public List<EstruturaFormaPagamento> FormaPagamentoExcluir { get; set; }
        public List<EstruturaFormaPagamento> FormaPagamentoInserir { get; set; }
        public string precoIniciaCom { get; set; }
        public int total { get; set; }
        public int totalCadastro { get; set; }
        public int quantidade { get; set; }
        public int quantidadePorCliente { get; set; }
        public bool validaBin { get; set; }
        public bool validacaoPadrao { get; set; }
        public int parceiroID { get; set; }
        public string codigoPromocional { get; set; }
        public string textoValidacao { get; set; }
        public int obrigatoriedadeID { get; set; }
        public EstruturaObrigatoriedade obrigatoriedade { get; set; }

    }
}
