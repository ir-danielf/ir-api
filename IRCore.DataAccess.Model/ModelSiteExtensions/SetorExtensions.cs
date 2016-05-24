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
    
    public partial class Setor
    {
        
        public enumLugarMarcado LugarMarcadoAsEnum
        {
            get
            {
                try { return (enumLugarMarcado)LugarMarcado[0]; }
                catch { return enumLugarMarcado.pista; }
            }
            set { LugarMarcado = ((char)value).ToString(); }
        }

       

        public List<Preco> Preco { get; set; }

        public int QtdeTotal { get; set; }

        public int QtdTotalMeiaIndisp { get; set; }

        public EstatisticaIngressos Estatistica { get; set; }
    }
}
