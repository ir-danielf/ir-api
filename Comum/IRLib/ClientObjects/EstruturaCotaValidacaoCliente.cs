using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotaValidacaoCliente
    {
        public int DonoID { get; set; }
        public string ClienteNome { get; set; }
        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int IngressoID { get; set; }
        public int CotaItemID { get; set; }
        public int CotaItemIDAPS { get; set; }


        public int QuantidadePorClienteCotaItem { get; set; }
        public int QuantidadeCotaItem { get; set; }

        public int QuantidadePorClienteCotaItemAPS { get; set; }
        public int QuantidadeCotaItemAPS { get; set; }

        public int QuantidadePorClienteApresentacao { get; set; }
        public int QuantidadeApresentacao { get; set; }

        public int QuantidadePorClienteApresentacaoSetor { get; set; }
        public int QuantidadeApresentacaoSetor { get; set; }

        public string CodigoPromocional { get; set; }

        public string CPF { get; set; }
        public int ReservaID { get; set; }
        public bool ValidaBin { get; set; }
        public int QuantidadeReservadaCliente { get; set; }
        public bool Nominal { get; set; }
    }
}
