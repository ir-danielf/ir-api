using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;

namespace IRCore.BusinessObject
{
    public class LojaBO : MasterBO<LojaADO>
    {
        public LojaBO(MasterADOBase ado) : base(ado) { }
        public LojaBO() : base(null) { }

        public tLoja Consultar(int lojaID)
        {
            return ado.Consultar(lojaID);
        }
    }
}
