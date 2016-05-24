using System.Linq;

namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;

    public partial class ParceiroMidia
    {

        public List<Setor> ApresentacaoSetorIDs { get; set; }

        public List<int> ApresentacaoIDs
        {
            get
            {
                return (ApresentacaoSetorIDs != null) ? ApresentacaoSetorIDs.Select(t => t.ApresentacaoID).Distinct().ToList() : null;
            }
        }

        public List<ParceiroMidiaEntrega> ParceiroMidiaEntrega { get; set; }

    }
}
