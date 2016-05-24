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

    public partial class tIngresso
    {

        public tBloqueio Bloqueio { get; set; }
        public enumIngressoStatus StatusAsEnum
        {
            get { return (enumIngressoStatus)Status[0]; }
            set { Status = ((char)value).ToString(); }
        }

        public DateTime TimeStampReservaAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(TimeStampReserva, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { TimeStampReserva = value.ToString("yyyyMMddHHmmss"); }
        }
    }
}
