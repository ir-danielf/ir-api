using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaIngressoCotaItem
    {
        public int IngressoID { get; set; }
        public int CotaItemID { get; set; }
        public int QuantidadeCotaItem { get; set; }
        public int QuantidadePorCliente_CotaItem { get; set; }
        public int Quantidade_Apresentacao { get; set; }
        public int QuantidadePorCliente_Apresentacao { get; set; }
        public int Quantidade_ApresentacaoSetor { get; set; }
        public int QuantidadePorCliente_ApresentacaoSetor { get; set; }
        public bool Nominal { get; set; }
    }
}
