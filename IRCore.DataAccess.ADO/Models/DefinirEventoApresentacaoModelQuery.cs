using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class DefinirEventoApresentacaoModelQuery
    {
        public string evento { get; set; }
        public string local { get; set; }
        public string regional { get; set; }
        public string empresa { get; set; }
        public DateTime apresentacao { get; set; }
        public int id { get; set; }
    }

    public class CancelamentoDetalheModelQuery
    {
        public string Usuario { get; set; }
        public DateTime Solicitado { get; set; }
        public string Evento { get; set; }
        public string Local { get; set; }
        public string Regional { get; set; }
        public string Empresa { get; set; }
        public string Motivo { get; set; }
        public string Codigo { get; set; }
    }
}
