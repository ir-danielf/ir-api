using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class EntregaControleModelQuery
    {
        public tEntregaControle entregaControle { get; set; }
        public tEntrega entrega { get; set; }

    }

    public static class EntregaControleExtensionQuery
    {
        public static tEntregaControle toEntregaControle(this EntregaControleModelQuery entregaControleQuery)
        {
            entregaControleQuery.entregaControle.Entrega = entregaControleQuery.entrega;
            return entregaControleQuery.entregaControle;
        }

    }

}
