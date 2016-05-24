using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class AssinaturaConfigBO : MasterBO<AssinaturaConfigADO>
    {
        public AssinaturaConfigBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaConfigBO() : base(null) { }
        public IRCore.DataAccess.Model.AssinaturaConfigModel Consultar(int id)
        {
            return ado.Consultar(id);
        }
        public List<IRCore.DataAccess.Model.AssinaturaConfigModel> BuscaByAssinaturaConfigNome(string assinaturaConfigNome = "")
        {
            return ado.BuscaByAssinaturaConfigNome(assinaturaConfigNome);
        }


        public bool Altera(AssinaturaConfigModel atm)
        {
           return ado.Alterar(atm);
        }

        public int Insere(AssinaturaConfigModel atm)
        {
            return ado.Insere(atm);
        }
    }
}
