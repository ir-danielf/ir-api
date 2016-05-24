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
    public class AssinaturaTextoADO : MasterADO<dbIngresso>
    {
        public AssinaturaTextoADO(MasterADOBase ado = null) : base(ado) { }

        public AssinaturaTextoModel Busca(int assinaturaTipoID, int assinaturaFaseID)
        {
            var queryStr = @"SELECT tAssinaturaTexto.*
                                FROM tAssinaturaTexto (nolock)
                                    INNER JOIN tAssinaturaAno (nolock) ON tAssinaturaTexto.AssinaturaAnoID = tAssinaturaAno.ID
                                 INNER JOIN tAssinatura (nolock) ON tAssinatura.ID = tAssinaturaAno.AssinaturaID
                                 INNER JOIN tAssinaturaTipo (nolock) ON tAssinatura.AssinaturaTipoID = tAssinaturaTipo.ID
                                WHERE tAssinaturaTipo.ID = @AssinaturaTipoID
                                AND tAssinaturaTexto.AssinaturaFaseID = @AssinaturaFaseID";

            var query = conIngresso.Query<AssinaturaTextoModel>(queryStr, new
            {
                AssinaturaTipoID = assinaturaTipoID,
                AssinaturaFaseID = assinaturaFaseID
            });

            return query.FirstOrDefault();
        }
    }
}
