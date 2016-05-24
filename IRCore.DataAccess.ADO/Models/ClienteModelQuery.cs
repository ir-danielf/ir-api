using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCore.DataAccess.Model;

namespace IRCore.DataAccess.ADO.Models
{
    public class ClienteModelQuery
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string RG { get; set; }
        public string CPF {get; set;}
        public string Telefone {get; set;}
        public string Evento {get; set;}
        public string Senha { get; set; }
        public string DataVenda {get; set;}

        
    }
}
                          