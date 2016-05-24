using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class AssinaturaAnoBO : MasterBO<AssinaturaAnoADO>
    {
        public AssinaturaAnoBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaAnoBO() : base(null) { }
        public AssinaturaAnoModel Consultar(int id)
        {
            return ado.Consultar(id);
        }
        public List<AssinaturaAnoModel> BuscaByAssinaturaTipo(int assinaturaTipoID)
        {
            return ado.BuscaByAssinaturaTipo(assinaturaTipoID);
        }
    }
}
