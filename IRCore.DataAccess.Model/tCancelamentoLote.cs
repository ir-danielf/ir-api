using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class tCancelamentoLote
    {
        public int ID {get; set; }
        public string CodigoCancelamento {get; set; }
        public int EventoID {get; set; }
        public int CancelamentoLoteModeloMotivoID {get; set; }
        public string MotivoCancelamento {get; set; }
        public int UsuarioID {get; set; }
        public DateTime DataCancelamento {get; set; }
        public string Status {get; set; }
        public DateTime DataMovimentacao {get; set; }
        public List<tCancelamentoLoteApresentacao> Apresentacoes { get; set; }
    }
}
