using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class ListaCanalPorNome_Result
    {
        public int CanalID { get; set; }
        public string CanalNome { get; set; }
        public string EmpresaNome { get; set; }
        public string RegionalNome { get; set; }
    }
}
