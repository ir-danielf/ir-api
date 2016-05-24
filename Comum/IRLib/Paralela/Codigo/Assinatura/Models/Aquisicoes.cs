
namespace IRLib.Paralela.Assinaturas.Models
{
    public class Aqusicao
    {
        public int AssinaturaID { get; set; }
        public string Assinatura { get; set; }
        public int SetorID { get; set; }
        public string Setor { get; set; }
        public int LugarID { get; set; }
        public string PrecoTipo { get; set; }
        public string Programacao { get; set; }
        public decimal Valor { get; set; }
        public AssinaturaCliente.EnumAcao Acao { get; set; }
        public AssinaturaCliente.EnumStatus Status { get; set; }
        public int AgregadoID { get; set; }
        public string AgregadoNome { get; set; }
        public string AgregadoEmail { get; set; }

        public int AssinaturaClienteID { get; set; }

        public string Lugar { get; set; }

        public string AcaoExibicao
        {
            get
            {
                switch (this.Acao)
                {
                    case AssinaturaCliente.EnumAcao.Renovar:
                        return "Renovado";
                    case AssinaturaCliente.EnumAcao.Desisistir:
                        return "Desistência";
                    case AssinaturaCliente.EnumAcao.Trocar:
                        return "Troca sinalizada";
                    case AssinaturaCliente.EnumAcao.EfetivarTroca:
                        return "Trocado";
                    case AssinaturaCliente.EnumAcao.Aquisicao:
                        return "Nova aquisição";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
