
namespace IRLib.Paralela.Assinaturas.Models
{
    public class AcaoProvisoria
    {
        public int AssinaturaClienteID { get; set; }
        public string Assinatura { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }
        public IRLib.Paralela.AssinaturaCliente.EnumAcao Acao { get; set; }
        public IRLib.Paralela.AssinaturaCliente.EnumStatus Status { get; set; }
        public decimal Valor { get; set; }

        public int LugarID { get; set; }
        public int ClienteID { get; set; }
        public int AssinaturaID { get; set; }
        public int AssinaturaAnoID { get; set; }
        public int SetorID { get; set; }

        public int PrecoTipoID { get; set; }
        public string PrecoTipo { get; set; }

        public int EntregaID { get; set; }
        public string Entrega { get; set; }
        public string AcaoImportar { get; set; }
        public int IndiceImportar { get; set; }
        public string Data { get; set; }

        public string Senha { get; set; }

        public int AgregadoID { get; set; }
    }
}