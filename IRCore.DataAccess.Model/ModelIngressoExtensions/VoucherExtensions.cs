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

    public partial class Voucher
    {
        public enumVoucherStatus StatusAsEnum
        {
            get { return (enumVoucherStatus)Status[0]; }
            set { Status = ((char)value).ToString(); }
        }

        public List<string> Cidades
        {
            get
            {
                return ParceiroMidiaPraca.Select(t => t.CidadeNome).ToList();
            }
        }
    }
}
