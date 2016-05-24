using System;

namespace IRLib.Assinaturas.Models
{
    public class MusicaCabecaInscito
    {
        public string ID { get; set; }
        public int ClienteID { get; set; }
        public int MusicaCabecaID { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public bool Presente { get; set; }
        public string PresenteValor
        {
            get
            {
                return Presente ? "Sim" : "Não";
            }
        }
        public bool Assinante { get; set; }
        public string AssinanteValor
        {
            get
            {
                return Assinante ? "Sim" : "Não";
            }
        }
        public bool Titular { get; set; }
        public string TitularValor
        {
            get
            {
                return Titular ? "Sim" : "Não";
            }
        }
        public DateTime DataInscricao { get; set; }

    }
}
