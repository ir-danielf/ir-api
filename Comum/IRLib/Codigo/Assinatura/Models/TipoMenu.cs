
namespace IRLib.Assinaturas.Models
{
    public class TipoMenu
    {
        public Utils.Enums.EnumTipoMenuAssinatura TipoMenuAssinatura { get; set; }
        public Utils.Enums.EnumMenuSelecionado TipoMenuSelecionado { get; set; }
        public bool Operador { get; set; }

        public TipoMenu() { this.Operador = false; }
    }
}
