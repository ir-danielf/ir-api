using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Models
{
    public class CompraEstruturaVendaModel
    {
        public int LojaID { get; set; }
        public int CaixaID { get; set; }
        public int CanalID { get; set; }
        public int UsuarioID { get; set; }
        public int CanalTipo { get; set; }
        public bool CanalVerificado { get; set; }
        public bool CaixaErrado { get; set; }
    }
}
