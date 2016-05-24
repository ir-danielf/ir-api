using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;

namespace IRCore.BusinessObject
{
    public class AssinaturaTextoBO : MasterBO<AssinaturaTextoADO>
    {

        public AssinaturaTextoBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaTextoBO() : base(null) { }

        public AssinaturaTextoModel Busca(int assinaturaTipoID, int assinaturaFaseID)
        {
            return ado.Busca(assinaturaTipoID, assinaturaFaseID);
        }
    }
}
