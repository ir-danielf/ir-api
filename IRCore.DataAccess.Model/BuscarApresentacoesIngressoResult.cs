using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class BloqueioResult
    {
        public string NomeEvento { get; set; }
        public string DataApresentacao { get; set; }
        public int ApresentacaoID { get; set; }
        public List<DetalheBloqueio> DetalhesBloqueio { get; set; }
    }

    public class DetalheBloqueio
    {
        public int IngressoID { get; set; }
        public string CodigoIngresso { get; set; }
        public string NomeBloqueio { get; set; }
    }
}
