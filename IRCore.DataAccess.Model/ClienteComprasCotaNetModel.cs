using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class ClienteComprasCotaNetModel
    {
        public string CPFResponsavelCompra { get; set; }
        public string NomeResponsavelCompra { get; set; }
        public string  Local { get; set; }
        public string Evento { get; set; }
        public string DataApresentacao { get; set; }
        public string HoraApresentacao { get; set; }
        public string Setor { get; set; }
        public string DataCompra { get; set; }
        public string ValorTicket { get; set; }
        public string QuantidadeTicket { get; set; }
        public string Senha { get; set; }
    }
}
