using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class SenhaCompraModel
    {
        public int ID { get; set; }
        public string Senha { get; set; }
        public DateTime DataCompra { get; set; }
        public int CanalID { get; set; }
        public string CanalNome { get; set; }
        public int LojaID { get; set; }
        public string LojaNome { get; set; }
        public decimal ValorTotal { get; set; }
        public string StatusCodigo { get; set; }
        public string Status { 
            get {
                if (StatusCodigo == null)
                    return null;
                switch(StatusCodigo)
                {
                    case "A":
                        return "Aguardando Aprovação";
                    case "P":
                        return "Pago";
                    case "C":
                        return "Cancelado";
                    case "E":
                        return "Entregue";
                    case "R":
                        return "Re-impresso";
                    case "M":
                        return "Pré-reservado";
                    case "F":
                        return "Fraude";
                    case "N":
                        return "Em Análise";
                    default:
                        return "";
                }
            }
        }

        public string StatusDevolucaoPendenteCodigo { get; set; }
        public string StatusDevolucaoPendente
        {
            get
            {
                if (StatusDevolucaoPendenteCodigo == null)
                    return null;
                switch (StatusDevolucaoPendenteCodigo)
                {
                    case "A":
                        return "Devolução Automática / Sem Devolução";
                    case "P":
                        return "Pendente";
                    case "C":
                        return "Cancelada";
                    case "D":
                        return "Devolvido";
                    default:
                        return "";
                }
            }
        }

        public string StatusCancelamentoCodigo { get; set; }
        public string StatusCancelamento {
            get
            {
                if (StatusCancelamentoCodigo == "S")
                    return "Sim";
                else
                    return "Não";
            }
        }
        public string PermissaoImprimir { get; set; }
        public string PermissaoCancelarData { get; set; }
        // public int Ingressos { get; set; }
        // public int ValeIngressos { get; set; }
        public string EventosID { get; set; }
    }
}
