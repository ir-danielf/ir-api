using IRLib.Utils;

namespace IRLib.Assinaturas.Models
{
    public class AssinantesSemManifestacao
    {
        public AssinantesSemManifestacao()
        {

        }
        public int LugarID { get; set; }
        public string Login { get; set; }
        public string Cliente { get; set; }
        public string Assinatura { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }
        public IRLib.Assinatura.EnumStatusVisual Status { get; set; }
        public string StatusDescricao
        {
            get
            {
                return Enums.GetDescription<IRLib.Assinatura.EnumStatusVisual>(Status);
            }
        }
    }

}
