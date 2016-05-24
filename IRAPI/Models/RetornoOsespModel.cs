using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRAPI.Models
{
    public class RetornoOsespModel
    {
        public int BloqueioID { get; set; }
        public string BloqueioNome { get; set; }
        public string Status { get; set; }
    }
}