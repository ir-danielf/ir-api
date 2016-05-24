using System;

namespace IRCore.DataAccess.Model
{

    public class ReportVoucherUtilizadoDisponibilizado_Result
    {
        public string Local { get; set; }
        public string Evento { get; set; }
        public string Data { get; set; }
        public int IngressosLiberados { get; set; }
        public int VouchersResgatados { get; set; }
        public decimal Utilizacao { get; set; }
    }
}
