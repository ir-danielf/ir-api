using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class AssinaturaFaseADO : MasterADO<dbIngresso>
    {
        public AssinaturaFaseADO(MasterADOBase ado = null) : base(ado) { }

        public List<AssinaturaFaseModel> Lista()
        {
            var queryStr = "select * from tAssinaturaFase (nolock)";

            var query = conIngresso.Query<AssinaturaFaseModel>(queryStr);
            return query.ToList();
        }
    }
}
