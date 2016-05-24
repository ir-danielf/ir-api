using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    /// <summary>
    /// id = id da mesa
    /// cd = código da mesa
    /// tt = total de assentos
    /// qd = quantidade de assentos disponíveis    
    /// sa = quantidade de assentos reservados pelo usuário (seu assento)
    /// rv = assentos reservados
    /// px = ponto x do local do assento
    /// py = ponto y do local do assento
    /// pl = perspectiva lugar, imagem do setor
    /// </summary>
    public class MapaAssentoMesaModel
    {
        public int id { set; get; }
        public string cd { set; get; }
        public int tt { set; get; }
        public int qd { set; get; }
        public int sa { set; get; }
        public int rv { set; get; }
        public int px { set; get; }
        public int py { set; get; }
        public string pl { set; get; }
    }
}
