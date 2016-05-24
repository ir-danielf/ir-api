using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class AssinaturaFaseBO : MasterBO<AssinaturaFaseADO>
    {
        public AssinaturaFaseBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaFaseBO() : base(null) { }

        public List<AssinaturaFaseModel> Lista()
        {
            return ado.Lista();
        }
    }
}
