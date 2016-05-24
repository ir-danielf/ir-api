using System;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class AssinaturasDesmembradas
    {
        public AssinaturasDesmembradas() { }

        public string Antigo { get; set; }
        public string Atual { get; set; }
        public DateTime Data { get; set; }
        public string Assento { get; set; }
        public string Motivo { get; set; }
    }
}
