using IRLib.ClientObjects;

namespace IngressoRapido.Lib
{
    public class CotaItemValidar
    {
        public int IngressoID { get; set; }
        public bool Nominal { get; set; }
        public bool Quantidade { get; set; }
        public int DonoID { get; set; }
        public string EncDonoID
        {
            get
            {
                if (DonoID == 0)
                    return "0";
                return new Crypto.Crypt().Encrypt(DonoID.ToString());
            }
        }
        public string Pais { get; set; }
        public string Identificacao { get; set; }
        public bool TemParceiro { get; set; }
        public string Codigo { get; set; }
        //public EstruturaCliente Cliente { get; set; }
        public EstruturaObrigatoriedadeSite Obrigatoriedade { get; set; }
        public EstruturaCodigoPromoValidacao codigoPromoValidacao { get; set; }
        public bool ClienteExiste { get; set; }
        public string Mensagem { get; set; }
        public int TipoRetorno { get; set; }
        public EstruturaDonoIngressoSite Dono { get; set; }
    }
}
