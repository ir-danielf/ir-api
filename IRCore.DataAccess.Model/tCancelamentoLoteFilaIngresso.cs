using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class tCancelamentoLoteFilaIngresso
    {
        public int ID { get; set; }
        public int CancelamentoLoteFilaID { get; set; }
        public int IngressoID { get; set; }
    }
}
