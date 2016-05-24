
using CTLib;
using System;
using System.Drawing;
namespace IRLib.Codigo.NovoGerador
{
    [Serializable]
    public class GerarEventoEtapa3Fase2Model
    {
        public string CategoriaPrincipal { get; set; }
        public string GeneroPrincipal { get; set; }
        public string ReleaseEvento { get; set; }
        public Image ImagemDivulgacao { get; set; }

        public string FileImagemDivulgacao { get; set; }

        public Image ImagemTicket { get; set; }

        public string FileImagemTicket { get; set; }
    }
}
