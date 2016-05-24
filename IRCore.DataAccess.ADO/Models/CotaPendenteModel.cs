using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRCore.DataAccess.Models
{
    public class CotaPendenteModel
    {
        public bool Nenhuma { get; set; }
        public bool Nominal { get; set; }
        public bool Bin { get; set; }
        public bool NaoBin { get; set; }
        public bool Promocional { get; set; }
        public bool Quantidade { get; set; }

        public bool Outras { 
            get 
            {
                return ((NaoBin) && (!Nominal) && (!Promocional) && (!Quantidade));
            } 
        }
    }
}