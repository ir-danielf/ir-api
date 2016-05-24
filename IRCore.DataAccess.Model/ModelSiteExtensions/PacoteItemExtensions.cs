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
    
    public partial class PacoteItem
    {
        public Pacote Pacote { get; set; }
        public NomenclaturaPacote NomenclaturaPacote { get; set; }

        public Preco Preco { get; set; }
        public Setor Setor { get; set; }
        public Apresentacao Apresentacao { get; set; }
    }
}
