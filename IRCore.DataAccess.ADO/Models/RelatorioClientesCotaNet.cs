using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRCore.DataAccess.ADO.Models
{
    public class RelatorioClientesCotaNet
    {
        public string CPFResponsavelCompra { get; set; }
        public string DataApresentacao { get; set; }
        public string DataCompra { get; set; }
        public string Evento { get; set; }
        public string CPFNet { get; set; }
        public string HoraApresentacao { get; set; }
        public string Local { get; set; }
        public string NomeResponsavelCompra { get; set; }
        public int QuantidadeTicket { get; set; }
        public string Setor { get; set; }
        public string ValorTicket { get; set; }
        public string NomeValorTicket { get; set; }
    }
}