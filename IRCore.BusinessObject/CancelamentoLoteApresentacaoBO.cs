using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class CancelamentoLoteApresentacaoBO : MasterBO<CancelamentoLoteApresentacaoADO>
    {
        public CancelamentoLoteApresentacaoBO(MasterADOBase ado) : base(ado) { }
        public CancelamentoLoteApresentacaoBO() : base(null) { }

        public tCancelamentoLoteApresentacao Consultar(int ID)
        {
            return ado.Consultar(ID);
        }

        public bool Atualizar(tCancelamentoLoteApresentacao CancelamentoLoteApresentacao)
        {
            return ado.Atualizar(CancelamentoLoteApresentacao);
        }

        public List<tCancelamentoLoteApresentacao> ConsultarPorCancelamentoLote(int CancelamentoLoteID)
        {
            return ado.ConsultarPorCancelamentoLote(CancelamentoLoteID);
        }

        public List<tCancelamentoLoteApresentacao> ConsultarPorCancelamentoLoteSolicitado(int CancelamentoLoteID)
        {
            return ado.ConsultarPorCancelamentoLoteSolicitado(CancelamentoLoteID);
        }
       
    }
}
