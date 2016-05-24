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
    public class AssinaturaADO : MasterADO<dbIngresso>
    {
        public AssinaturaADO(MasterADOBase ado = null) : base(ado, true, true) { }


        public AssinaturaModel Consultar(int id)
        {
            var queryStr = @"select ass.ID,
                                    ass.Nome,
                                    ass.TipoCancelamento,
                                    ass.AssinaturaTipoID,
                                    ass.Ativo,
                                    ass.LocalID,
                                    ass.BloqueioID,
                                    ass.DesistenciaBloqueioID,
                                    ass.ExtintoBloqueioID,
                                    ass.Sigla,
                                    ass.AlertaAssinante
                               from dbo.tAssinatura ass (nolock)
                              where ass.ID = @ID";

            var query = conIngresso.Query<AssinaturaModel>(queryStr, new
            {
                ID = id
            });

            return query.FirstOrDefault();
        }
        public List<AssinaturaModel> BuscaByAssinaturaTipoeAno(int assinaturaTipoID, int assinaturaAnoID)
        {
            var queryStr = @"select ass.ID,
                                    ass.Nome,
                                    ass.TipoCancelamento,
                                    ass.AssinaturaTipoID,
                                    ass.Ativo,
                                    ass.LocalID,
                                    ass.BloqueioID,
                                    ass.DesistenciaBloqueioID,
                                    ass.ExtintoBloqueioID,
                                    ass.Sigla,
                                    ass.AlertaAssinante
                               FROM tAssinatura ass (NOLOCK)
                              INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = ass.ID
                              WHERE ass.AssinaturaTipoID = @ID
                                AND an.Ano = @AnoID";

            var query = conIngresso.Query<AssinaturaModel>(queryStr, new { ID = assinaturaTipoID, AnoID = assinaturaAnoID });

            return query.ToList();
        }
    }
}
