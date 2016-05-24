using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using Dapper;
using System.Data.Entity.Core.Objects;

namespace IRCore.DataAccess.ADO
{
    public class AssinaturaAnoADO : MasterADO<dbIngresso>
    {
        public AssinaturaAnoADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public AssinaturaAnoModel Consultar(int id)
        {
            var queryStr = @"select an.ID,
                                    an.assinaturaID,
                                    an.Ano,
                                    an.informacoes
                               from dbo.tAssinaturaAno an (nolock)
                              where an.ID = @ID";

            var query = conIngresso.Query<AssinaturaAnoModel>(queryStr, new { ID = id });

            return query.FirstOrDefault();
        }

        public List<AssinaturaAnoModel> BuscaByAssinaturaTipo(int assinaturaTipoID)
        {
            var queryStr = @"select an.ID,
                                    an.assinaturaID,
                                    an.Ano,
                                    an.informacoes
                               FROM tAssinatura ass (NOLOCK)
                              INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = ass.ID
                              WHERE ass.AssinaturaTipoID = @ID";

            var query = conIngresso.Query<AssinaturaAnoModel>(queryStr, new { ID = assinaturaTipoID });

            return query.ToList();
        }
    }
}
