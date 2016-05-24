using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    using System;

    public class ReportPrevisaoPDV_Result
    {
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Nome { get; set; }
        public int Compras { get; set; }
        public int Ingressos { get; set; }
    }
}
