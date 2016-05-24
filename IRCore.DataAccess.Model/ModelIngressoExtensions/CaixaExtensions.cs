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
    using System.Globalization;

    public partial class tCaixa
    {
        public DateTime DataAberturaAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(DataAbertura, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { DataAbertura = value.ToString("yyyyMMddHHmmss"); }
        }

        public DateTime DataFechamentoAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(DataFechamento, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            set { DataFechamento = value.ToString("yyyyMMddHHmmss"); }
        }
    }
}
