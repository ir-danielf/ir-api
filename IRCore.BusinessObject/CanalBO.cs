using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class CanalBO : MasterBO<CanalADO>
    {
        public CanalBO(MasterADOBase ado = null) : base(ado) { }

        public tCanal Consultar(int canal)
        {
            return ado.Consultar(canal);
        }

        public decimal ConsultarTaxaConveniencia(int eventoID, int canalID)
        {
            return ado.ConsultarTaxaConveniencia(eventoID, canalID);
        }

        public bool isPOS(int canalId, int canalTipoId)
        {
            return ado.isPOS(canalId, canalTipoId);
        }

        public List<ListaCanalPorNome_Result> ListarCanalPorNome(string canal)
        {
            return ado.ListarCanalPorNome(canal);
        }
    }
}
