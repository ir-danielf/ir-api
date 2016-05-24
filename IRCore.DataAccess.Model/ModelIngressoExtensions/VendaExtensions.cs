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

    public partial class tVendaBilheteria
    {
        public DateTime DataVendaAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(DataVenda, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { DataVenda = value.ToString("yyyyMMddHHmmss"); }
        }

        public enumVendaBilheteriaStatus StatusAsEnum
        {
            get { return (enumVendaBilheteriaStatus)Status[0]; }
            set { Status = ((char)value).ToString(); }
        }

        public tEntregaControle EntregaControle { get; set; }

        public tVendaBilheteria VendaBilheteriaOrigem { get; set; }
    }
}
