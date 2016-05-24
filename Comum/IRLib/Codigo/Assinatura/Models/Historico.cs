using System;

namespace IRLib.Assinaturas.Models
{
    public class Historico
    {
        public int AssinaturaClienteID { get; set; }
        public int AssinaturaID { get; set; }
        public string Assinatura { get; set; }
        public int SetorID { get; set; }
        public string Setor { get; set; }
        public int LugarID { get; set; }
        public string Codigo { get; set; }
        public DateTime Data { get; set; }
        public string Usuario { get; set; }

        public IRLib.AssinaturaCliente.EnumStatus Status { private get; set; }
        public IRLib.AssinaturaCliente.EnumAcao Acao { private get; set; }

        public string AcaoExibicao
        {
            get
            {
                switch (this.Acao)
                {
                    case AssinaturaCliente.EnumAcao.Renovar:
                        return Status == AssinaturaCliente.EnumStatus.Renovado ? "Renovação" : "Renovação sem pagamento";
                    case AssinaturaCliente.EnumAcao.Desisistir:
                        return "Desistência";
                    case AssinaturaCliente.EnumAcao.Trocar:
                        return "Troca sinalizada";
                    case AssinaturaCliente.EnumAcao.EfetivarTroca:
                        return "Troca efetivada";
                    case AssinaturaCliente.EnumAcao.Aquisicao:
                        return "Nova aquisição";
                    default:
                        return string.Empty;
                }
            }
        }

    }
}
