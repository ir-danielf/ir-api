using CTLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    [Serializable]
    public class GerarEventoEtapa1Fase2Model
    {
        [Display(Name = "Apresentações")]
        public List<DateTime> Apresentacoes { get; set; }

    }
}
