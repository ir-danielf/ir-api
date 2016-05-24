
namespace IRLib.ClientObjects
{
    public class EstruturaCotasValidacao
    {
        public int ID { get; set; }
        public int IDAPS { get; set; }

        public int PrecoID { get; set; }
        public string PrecoNome { get; set; }
        public string CPF { get; set; }
        public string Codigo { get; set; }
        public string CodigoPromo { get; set; }
        public bool CodigoPromoVisivel { get; set; }
        public bool cotaEncontrada { get; set; }
        public int QuantidadePorCliente { get; set; }
        public int QuantidadePorClienteCota { get; set; }
        public int QuantidadePorClienteCotaAPS { get; set; }

        public int QuantidadePorClienteApresentacao { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeCota { get; set; }
        public int QuantidadeCotaAPS { get; set; }

        public int QuantidadeApresentacao { get; set; }
        public string hiddenInfo { get; set; }
        public string PrecoIniciaCom { get; set; }
        public bool ValidaBin { get; set; }
        public string TextoValidacao { get; set; }
        public int IngressoID { get; set; }
        public int ApresentacaoID { get; set; }
        public int SetorID { get; set; }
        public int ParceiroID { get; set; }
    }
}
