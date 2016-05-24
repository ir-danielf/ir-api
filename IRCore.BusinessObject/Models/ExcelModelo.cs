using System;

namespace Admin.Areas.Logistica.Models
{
    public class ExcelModelo
    {
        public string Tipo { get; set; }
        public string DataHoraOcorrencia { get; set; }
        public string CodigoRastreamento { get; set; }
        public string StatusTexto { get; set; }
        public string Senha { get; set; }
        public string GrauParentesco { get; set; }
        public string NomeRecebedor { get; set; }
        public string RG { get; set; }

    }
}