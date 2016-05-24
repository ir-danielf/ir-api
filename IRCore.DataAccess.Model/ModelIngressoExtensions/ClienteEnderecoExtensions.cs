namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tClienteEndereco
    {

        public bool EntregaDisponivel { 
            get
            {
                return ((EntregaControles != null) && (EntregaControles.Count > 0));
            } 
        }
        public List<tEntregaControle> EntregaControles { get; set; }

        public List<tEntregaArea> EntregaArea { get; set; }

    }
}