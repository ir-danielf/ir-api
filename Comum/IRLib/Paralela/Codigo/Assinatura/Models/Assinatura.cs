using IRLib.Paralela.ClientObjects;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class Assinatura
    {
        public Assinatura()
        {
            acaoCliente = IRLib.Paralela.AssinaturaCliente.AcoesCliente();
        }

        public int AssinaturaClienteID { get; set; }
        public int AssinaturaID { get; set; }
        public string Nome { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }
        public int SetorID { get; set; }
        public int LugarID { get; set; }
        public IRLib.Paralela.AssinaturaCliente.EnumStatus Status { get; set; }
        public IRLib.Paralela.AssinaturaCliente.EnumAcao Acao { get; set; }
        public List<IRLib.Paralela.ClientObjects.Assinaturas.EstruturaAssinaturaPreco> Precos { get; set; }


        private List<Agregados> _agregados;
        public List<Agregados> Agregados
        {
            get
            {

                _agregados = new IRLib.Paralela.Agregados().ListarTodos(this.AssinaturaClienteID);

                return _agregados;
            }
        }

        private IEnumerable<EstruturaIDNome> acaoCliente { get; set; }
        public IEnumerable<EstruturaIDNome> AcaoCliente
        {
            get
            {
                return acaoCliente;
            }
        }
        public int RelacionadoAssinaturaClienteID { get; set; }
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
        public decimal Valor { get; set; }

        public int PrecoTipoID { get; set; }
    }
}