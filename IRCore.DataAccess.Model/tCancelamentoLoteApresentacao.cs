using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class tCancelamentoLoteApresentacao
    {
        public int ID {get; set; }
        public int CancelamentoLoteID {get; set; }
        public int ApresentacaoID { get; set; }
        public string Status {get; set; }
    }
}
