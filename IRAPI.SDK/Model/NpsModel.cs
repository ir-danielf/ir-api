using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRAPI.SDK.Model
{
    public class NpsModel
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Delay { get; set; }

        public string Status { get; set; }

        public string Canal { get; set; }

        public Nullable<DateTime> DataInclusao { get; set; }

        public Nullable<DateTime> DataEnvio { get; set; }

    }
}
