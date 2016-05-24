using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class CancelamentoLoteApresentacaoADO : MasterADO<dbIngresso>
    {
        public CancelamentoLoteApresentacaoADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public tCancelamentoLoteApresentacao Consultar(int ID)
        {
            #region strQuery
            string strQuery = @"SELECT
	                                ID
	                                ,CancelamentoLoteID
	                                ,ApresentacaoID
	                                ,Status
                                FROM
	                                tCancelamentoLoteApresentacao (NOLOCK)
                                WHERE
	                                ID = @ID";
            #endregion

            tCancelamentoLoteApresentacao result = conIngresso.Query<tCancelamentoLoteApresentacao>(strQuery, new { ID = ID }).FirstOrDefault();
            return result;
        }

        public bool Atualizar(tCancelamentoLoteApresentacao CancelamentoLoteApresentacao)
        {
            #region strQuery
            string strQuery = @"UPDATE 
                                    tCancelamentoLoteApresentacao
                                SET
                                     CancelamentoLoteID = @CancelamentoLoteID
	                                ,ApresentacaoID = @ApresentacaoID
	                                ,Status = @Status
                                WHERE
	                                ID = @ID";
            #endregion

            bool result = conIngresso.Execute(strQuery, CancelamentoLoteApresentacao) > 0;
            return result;
        }

        public List<tCancelamentoLoteApresentacao> ConsultarPorCancelamentoLote(int CancelamentoLoteID)
        {
            #region strQuery
            string strQuery = @"SELECT
	                                ID
	                                ,CancelamentoLoteID
	                                ,ApresentacaoID
	                                ,Status
                                FROM
	                                tCancelamentoLoteApresentacao (NOLOCK)
                                WHERE
	                                CancelamentoLoteID = @CancelamentoLoteID";
            #endregion

            List<tCancelamentoLoteApresentacao> result = conIngresso.Query<tCancelamentoLoteApresentacao>(strQuery, new { CancelamentoLoteID = CancelamentoLoteID }).ToList();

            return result;
        }

        public List<tCancelamentoLoteApresentacao> ConsultarPorCancelamentoLoteSolicitado(int CancelamentoLoteID)
        {
            #region strQuery
            string strQuery = @"SELECT
	                                ID
	                                ,CancelamentoLoteID
	                                ,ApresentacaoID
	                                ,Status
                                FROM
	                                tCancelamentoLoteApresentacao (NOLOCK)
                                WHERE
                                    Status = 'S'
	                                AND CancelamentoLoteID = @CancelamentoLoteID";
            #endregion
            List<tCancelamentoLoteApresentacao> result = conIngresso.Query<tCancelamentoLoteApresentacao>(strQuery, new { CancelamentoLoteID = CancelamentoLoteID }).ToList();
            return result;
        }
    }
}
