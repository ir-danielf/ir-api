using System;
using System.Data;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaVendaPos
    {
        public int EventoID { get; set; }
        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int PrecoID { get; set; }
        public string NomePreco { get; set; }
        public string LugarMarcado { get; set; }
        public int LocalID { get; set; }
        public int EmpresaID { get; set; }
        public string CodigoImpressao { get; set; }

    }

    [Serializable]
    public class EstruturaRetornoVendaPos
    {
        public int Contador { get; set; }
        public int EventoID { get; set; }
        public int PrecoID { get; set; }
        public int IngressoID { get; set; }
        public string Senha { get; set; }
        public string CodigoBarra { get; set; }
        public string pg { get; set; }
        public DataSet Reserva { get; set; }
    }
}