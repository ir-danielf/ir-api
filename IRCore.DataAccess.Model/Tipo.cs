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
    
    public partial class Tipo
    {
        public Tipo()
        {
            this.DestaqueLinkRegiao = new HashSet<DestaqueLinkRegiao>();
        }
    
        public int ID { get; set; }
        public int IR_TipoID { get; set; }
        public string Nome { get; set; }
        public string Obs { get; set; }
    
        public virtual ICollection<DestaqueLinkRegiao> DestaqueLinkRegiao { get; set; }
    }
}
