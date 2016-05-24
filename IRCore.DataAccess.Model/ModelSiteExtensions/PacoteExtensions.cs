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
    
    public partial class Pacote
    {
        public NomenclaturaPacote NomenclaturaPacote { get; set; }

        public List<PacoteItem> PacoteItem { get; set; }
    }
}
