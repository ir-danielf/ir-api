using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    using System;

    public class ReportCompradores_Result
    {
        public string cpf { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string local { get; set; }
        public string evento { get; set; }
        public DateTime apresentacao { get; set; }
        public string setor { get; set; }
        public int quantidadeIngressos { get; set; }
        public string meioEntrega { get; set; }
    }
}
