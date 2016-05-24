//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Voucher
    {
        public Voucher()
        {
            this.ParceiroMidiaPraca = new HashSet<ParceiroMidiaPraca>();
        }
    
        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Status { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> DataUso { get; set; }
        public int ParceiroMidiaID { get; set; }
        public Nullable<int> ClienteID { get; set; }
        public int ParceiroMidiaClasseID { get; set; }
        public int ParceiroMidiaAreaID { get; set; }
        public System.DateTime DataExpiracao { get; set; }
        public Nullable<int> VendaBilheteriaID { get; set; }
    
        public virtual ParceiroMidia ParceiroMidia { get; set; }
        public virtual ParceiroMidiaArea ParceiroMidiaArea { get; set; }
        public virtual ParceiroMidiaClasse ParceiroMidiaClasse { get; set; }
        public virtual tCliente tCliente { get; set; }
        public virtual ICollection<ParceiroMidiaPraca> ParceiroMidiaPraca { get; set; }
        public virtual tVendaBilheteria tVendaBilheteria { get; set; }
    }
}
