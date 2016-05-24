using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class SetorModelQuery
    {
        public Setor setor { get; set; }

        public List<Preco> precos { get; set; }
    }

    public static class SetorExtensionQuery
    {
        public static Setor toSetor(this SetorModelQuery setorQuery)
        {
            setorQuery.setor.Preco = setorQuery.precos;
            return setorQuery.setor;
        }

    }

}
