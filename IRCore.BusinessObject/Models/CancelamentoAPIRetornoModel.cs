using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Models
{
    public class CancelamentoAPIRetornoModel
    {
        public string SenhaCancelamento { get; set; }
        public bool Sucesso { get; set; }
        public string Erro { get; set; }
    }
}
