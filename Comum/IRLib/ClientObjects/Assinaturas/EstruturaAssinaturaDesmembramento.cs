using System;

namespace IRLib.ClientObjects.Assinaturas
{
    [Serializable]
    public class EstruturaAssinaturaDesmembramento
    {
        public string Assinatura { get; set; }
        public string Temporada { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }
        public string ClienteResponsavel { get; set; }
        public string ClienteNovo { get; set; }
        public string Motivo { get; set; }
        public string Usuario { get; set; }
        public int Notificacao { get; set; }
    }
}
