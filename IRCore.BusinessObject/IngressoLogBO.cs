using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class IngressoLogBO : MasterBO<IngressoLogADO>
    {
        public IngressoLogBO(MasterADOBase ado) : base(ado) { }

        public IngressoLogBO() : base(null) { }

        public List<tIngressoLog> ConsultarAnulacoesDeImpressao(int vendaBilheteriaID)
        {
            return ado.ConsultarAnulacoesDeImpressao(vendaBilheteriaID);
        }
    }
}
