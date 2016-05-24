using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Models
{
    public class AssinaturaModel
    {
        public int AssinaturaTipoID { get; set; }
        public string AssintaturaTipoNomeAnterior { get; set; }
        public int AssinaturaTipoNovoID { get; set; }
        public string AssintaturaTipoNomeNovo { get; set; }
        public int AssinaturaAnoID { get; set; }
        public int AssinaturaAnoNovoID { get; set; }
        public int UsuarioID { get; set; }
        public List<AssinaturaMapeamentoModel> Mapeamento { get; set; }

    }

}
