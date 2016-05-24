using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;

    public partial class EstatisticaIngressos
    {
        public int TotalIngressos { get; set; }
        public int Disponiveis { get; set; }
        public int Vendidos { get; set; }
        public int Reservados { get; set; }
    }
}
