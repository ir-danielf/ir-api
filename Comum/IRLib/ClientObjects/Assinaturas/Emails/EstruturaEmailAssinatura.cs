
namespace IRLib.ClientObjects
{
    public class EstruturaEmailAssinatura
    {
        public string Remetente { get; set; }
        public string NomeExibicao { get; set; }
        public string Senha { get; set; }
        public string SMTP { get; set; }

        public string ClienteNome { get; set; }
        public string ClienteEmail { get; set; }

        public string Assunto { get; set; }
        public string Corpo { get; set; }


        public int EnvioID { get; set; }
    }
}
