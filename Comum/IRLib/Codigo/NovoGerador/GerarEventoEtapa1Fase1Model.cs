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
    public class GerarEventoEtapa1Fase1Model 
    {
        [Display(Name = "Nome do Evento")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage="Nome é obrigatório !")]
        public string Nome { get; set; }
        
        [Display(Name = "Vende Canais IR")]               
        public bool VendeCanaisIR { get; set; }

        [Display(Name = "Lista de Canais Proprios")]
        public List<int> CanaisProprios { get; set; }


        [Display(Name = "ID do Local do evento")]
        public int LocalID{ get; set; }

    }
}
