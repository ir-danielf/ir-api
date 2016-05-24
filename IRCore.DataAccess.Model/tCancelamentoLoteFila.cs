using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class tCancelamentoLoteFila
    {
        public int ID { get; set;}
        public int CancelamentoLoteID  { get; set; }
        public int VendaBilheteriaID { get; set; }
        public int CanalID  { get; set; }
        public int LojaID  { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public char Status  { get; set; }
        public char Operacao { get; set; }
        public List<tCancelamentoLoteFilaIngresso> Ingressos { get; set; }
    }
}
