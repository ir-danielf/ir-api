using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using IRCore.DataAccess.ADO.Models;
using System.Data.Common;

namespace IRCore.DataAccess.ADO
{
    public class CancelamentoLoteADO : MasterADO<dbIngresso>
    {
        public CancelamentoLoteADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public tCancelamentoLote Consultar(int ID)
        {
            #region strQuery
            string strQuery = @"SELECT
	                                ID
	                                ,CodigoCancelamento
	                                ,EventoID
	                                ,CancelamentoLoteModeloMotivoID
	                                ,MotivoCancelamento
	                                ,UsuarioID
	                                ,dbo.StringToDateTime(DataCancelamento) AS DataCancelamento
	                                ,Status
	                                ,dbo.StringToDateTime(DataMovimentacao) AS DataMovimentacao
                                FROM
	                                tCancelamentoLote (NOLOCK)
                                WHERE
	                                ID = @ID";
            #endregion

            tCancelamentoLote result = conIngresso.Query<tCancelamentoLote>(strQuery, new { ID = ID }).FirstOrDefault();
            return result;
        }

        public tCancelamentoLote ConsultarComApresentacoes(int ID)
        {
            #region strQuery
            string strQuery = @"SELECT
	                                cl.ID, cl.CodigoCancelamento, cl.EventoID, cl.CancelamentoLoteModeloMotivoID, cl.MotivoCancelamento, cl.UsuarioID, dbo.StringToDateTime(DataCancelamento) AS DataCancelamento, Status, dbo.StringToDateTime(DataMovimentacao) AS DataMovimentacao
                                    ,cla.ID, cla.CancelamentoLoteID, cla.ApresentacaoID, cla.Status
                                FROM
	                                tCancelamentoLote (NOLOCK) AS cl
                                    INNER JOIN tCancelamentoLoteApresentacao (NOLOCK) AS cla ON cl.ID = cla.CancelamentoLoteID
                                WHERE
	                                cl.ID = @ID";
            #endregion
            tCancelamentoLote result = conIngresso.Query<tCancelamentoLote, tCancelamentoLoteApresentacao, tCancelamentoLote>(strQuery, addResultComApresentacoes, new { ID = ID }).FirstOrDefault();
            return result;
        }

        public tCancelamentoLote ConsultarProximoSolicitado()
        {
            #region strQuery
            string strQuery = @"SELECT TOP 1
	                                ID
	                                ,CodigoCancelamento
	                                ,EventoID
	                                ,CancelamentoLoteModeloMotivoID
	                                ,MotivoCancelamento
	                                ,UsuarioID
	                                ,dbo.StringToDateTime(DataCancelamento) AS DataCancelamento
	                                ,Status
	                                ,dbo.StringToDateTime(DataMovimentacao) AS DataMovimentacao
                                FROM
	                                tCancelamentoLote (NOLOCK)
                                WHERE
	                                DataCancelamento = DataMovimentacao
                                ORDER BY
                                    ID";
            #endregion

            tCancelamentoLote result = conIngresso.Query<tCancelamentoLote>(strQuery).FirstOrDefault();
            return result;
        }

        public bool Atualizar(tCancelamentoLote CancelamentoLote)
        {
            #region strQuery
            string strQuery = @"UPDATE 
                                    tCancelamentoLote
                                SET
                                     CodigoCancelamento = @CodigoCancelamento
	                                ,EventoID = @EventoID
	                                ,CancelamentoLoteModeloMotivoID = @CancelamentoLoteModeloMotivoID
	                                ,MotivoCancelamento = @MotivoCancelamento
	                                ,UsuarioID = @UsuarioID
	                                ,DataCancelamento = @DataCancelamento
	                                ,Status = @Status
	                                ,DataMovimentacao = @DataMovimentacao
                                WHERE
	                                ID = @ID";
            #endregion

            object param = new
            {
                ID = CancelamentoLote.ID,
                CodigoCancelamento = CancelamentoLote.CodigoCancelamento,
                EventoID = CancelamentoLote.EventoID,
                CancelamentoLoteModeloMotivoID = CancelamentoLote.CancelamentoLoteModeloMotivoID,
                MotivoCancelamento = CancelamentoLote.MotivoCancelamento,
                UsuarioID = CancelamentoLote.UsuarioID,
                DataCancelamento = CancelamentoLote.DataCancelamento.ToString("yyyyMMddHHmmss"),
                Status = CancelamentoLote.Status,
                DataMovimentacao = CancelamentoLote.DataMovimentacao.ToString("yyyyMMddHHmmss")
            };

            bool result = conIngresso.Execute(strQuery, param) > 0;
            return result;
        }

        public List<OperacaoCancelamentoModelQuery> MontarOperacoesCancelamento(List<int> ApresentacoesID)
        {
            #region strQuery
            string strQuery = @"SELECT
                                  VendaBilheteriaID,
                                  CanalID,
                                  LojaID,
                                  IngressoID,
                                  CASE 1 WHEN OperacaoA THEN
                                    'A'
                                  WHEN OperacaoB THEN
                                    'B'
                                  WHEN OperacaoC THEN
                                    'C'
                                  WHEN OperacaoD THEN
                                    'D'
                                  WHEN OperacaoE THEN
                                    'E'
                                  WHEN OperacaoF THEN
                                    'F'
                                  WHEN OperacaoG THEN
                                    'G'
                                  END AS Operacao
                                FROM
                                  (SELECT
                                    *
                                  FROM
                                    vwOperacoesCancelamento
                                  WHERE
                                    ApresentacaoID IN (@ApresentacoesID)) AS aux
                                    AND OperacaoG = 0
                                WHERE
                                  OperacaoA <> 0
                                  OR OperacaoB <> 0
                                  OR OperacaoC <> 0
                                  OR OperacaoD <> 0
                                  OR OperacaoE <> 0
                                  OR OperacaoF <> 0";
            #endregion

            List<OperacaoCancelamentoModelQuery> result = conIngresso.Query<OperacaoCancelamentoModelQuery>(strQuery, new { ApresentacoesID = ApresentacoesID }).ToList();
            return result;
        }

        public string GerarCancelamento(tCancelamentoLote cancelLote, List<int> apresentacoesID)
        {

            conIngresso.Open();
            DbTransaction Transaction = conIngresso.BeginTransaction();
            
            string query = @"INSERT INTO [dbo].[tCancelamentoLote]
                                ([CodigoCancelamento]
                                ,[EventoID]
                                ,[CancelamentoLoteModeloMotivoID]
                                ,[MotivoCancelamento]
                                ,[UsuarioID]
                                ,[DataCancelamento]
                                ,[Status]
                                ,[DataMovimentacao])
                        OUTPUT inserted.ID
                            VALUES
                               (@codigoCancelamentoTemp
                               ,@eventoID
                               ,@cancelamentoMotivoID
                               ,@motivoCancelamento
                               ,@usuarioID
                               ,@dataCancelamento
                               ,'S'
                               ,@dataMovimentacao)";
            try
            {
                //INSERE o CancelamentoLote usando a data de cancelamento como código temporário
                cancelLote.ID = conIngresso.Query<int>(query, new
                {
                    codigoCancelamentoTemp = cancelLote.CodigoCancelamento,
                    eventoID = cancelLote.EventoID,
                    cancelamentoMotivoID = cancelLote.CancelamentoLoteModeloMotivoID,
                    motivoCancelamento = cancelLote.MotivoCancelamento,
                    usuarioID = cancelLote.UsuarioID,
                    dataCancelamento = cancelLote.DataCancelamento.ToString("yyyyMMddHHmmss"),
                    dataMovimentacao = cancelLote.DataMovimentacao.ToString("yyyyMMddHHmmss")

                },transaction:Transaction).FirstOrDefault();

                if (cancelLote.ID > 0)
                {
                    cancelLote.CodigoCancelamento = string.Format("CM{0}", cancelLote.ID.ToString().PadLeft(10, '0'));

                    query = @"UPDATE [dbo].[tCancelamentoLote]
                                SET [CodigoCancelamento] = @codigoCancelamento
                                WHERE ID = @ID";
                    
                    if (conIngresso.Execute(query, new { codigoCancelamento = cancelLote.CodigoCancelamento, ID = cancelLote.ID },transaction:Transaction) == 1)
                    {
                    query = @"SELECT TOP 1 EmpresaID FROM tLocal(NOLOCK)
                              INNER JOIN tEvento(NOLOCK) ON tLocal.ID = tEvento.LocalID
                               WHERE tEvento.ID = @eventoID";

                    //Pega a empresa das apresentações selecionadas
                    int empresaID = conIngresso.Query<int>(query, new
                    {
                        eventoID = cancelLote.EventoID
                    },transaction:Transaction).FirstOrDefault();

                    query = "";

                    //Percorre as apresentacoes e insere os registros equivalentes na tCancelamentoLoteApresentacao e tCancelamentoLoteRepasse além de fazer update na tApresentacao
                    foreach (var aprID in apresentacoesID)
                    {
                        query += string.Format(@"INSERT INTO [dbo].[tCancelamentoLoteApresentacao]
                                      ([CancelamentoLoteID]
                                      ,[ApresentacaoID]
                                      ,[Status])
                                VALUES
                                      (@cancelamentoLoteID
                                      ,{0}
                                      ,'S');
                               
                               UPDATE [dbo].[tApresentacao]
                               SET [DisponivelVenda] = 'F'
                                  ,[Cancelada] = 'T'
                                   WHERE ID = {0};
                                
                              INSERT INTO [dbo].[tCancelamentoLoteRepasse]
                                  ([CancelamentoLoteID]
                                  ,[EventoID]
                                  ,[ApresentacaoID]
                                  ,[EmpresaID]
                                  ,[DataCancelamento]
                                  ,[Status])
                              VALUES
                                  (@cancelamentoLoteID
                                  ,@eventoID
                                  ,{0}
                                  ,@empresaID
                                  ,@dataCancelamento
                                  ,'B')
                                  ", aprID);
                    }

                    //Persiste os dados nas tabelas tCancelamentoLoteRepasse, tCancelamentoLoteApresentacao e tApresentacao, e ve se o número de linhas afetadas é 3 vezes (número de tabelas afetadas) o número de apresentações
                    if (conIngresso.Execute(query, new { cancelamentoLoteID = cancelLote.ID, eventoID = cancelLote.EventoID, empresaID = empresaID, dataCancelamento = cancelLote.DataCancelamento.ToString("yyyyMMddHHmmss") }, transaction: Transaction) == (apresentacoesID.Count * 3))
                    {
                        Transaction.Commit();
                            return cancelLote.CodigoCancelamento;
                        }
                    }

                }

                Transaction.Rollback();
                return string.Empty;
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw ex;
            }
            finally
            {
                conIngresso.Close();
            }
        }

        private tCancelamentoLote addResultComApresentacoes(tCancelamentoLote canc, tCancelamentoLoteApresentacao apr)
        {
            if(canc.Apresentacoes == null)
            {
                canc.Apresentacoes = new List<tCancelamentoLoteApresentacao>();
            }
            canc.Apresentacoes.Add(apr);
            return canc;
        }

        public CancelamentoRelatorioDadosBasicos ConsultarCancelamentoRelatorioDadosBasicos(List<int> apresentacoesID = null, string codigoCancelamento = null, int cancelamentoID = 0)
        {
            if (apresentacoesID != null)
                return ConsultarCancelamentoRelatorioDadosBasicos(apresentacoesID);
            return ConsultarCancelamentoRelatorioDadosBasicos(codigoCancelamento, cancelamentoID);
        }

        private CancelamentoRelatorioDadosBasicos ConsultarCancelamentoRelatorioDadosBasicos(string codigoCancelamento, int cancelamentoLoteID)
        {
            if (codigoCancelamento == null)
                codigoCancelamento = "";

            string query = @"Select TOP 1 evt.Nome as Evento, reg.nome as Regional,loc.nome as Local, emp.nome as Empresa, dbo.StringToDateTime(canc.DataCancelamento) as Data,usu.Nome as Usuario,canc.MotivoCancelamento AS Motivo
                             FROM tCancelamentoLote canc(NOLOCK) 
                             INNER JOIN tEvento evt(NOLOCK) ON canc.EventoID = evt.ID
                             INNER JOIN tLocal loc(NOLOCK) ON evt.LocalId = loc.ID
                             INNER JOIN tEmpresa emp(NOLOCK) ON loc.EmpresaID = emp.ID
                             INNER JOIN tRegional reg(NOLOCK) ON emp.RegionalID = reg.ID
                             INNER JOIN tUsuario usu(NOLOCK) ON canc.UsuarioID = usu.ID
                             WHERE canc.ID = @cancelamentoLoteID OR canc.CodigoCancelamento = @codigoCancelamento";
            return conIngresso.Query<CancelamentoRelatorioDadosBasicos>(query, new
            {
                codigoCancelamento = codigoCancelamento,
                cancelamentoLoteID = cancelamentoLoteID
            }).FirstOrDefault();
        }

        private CancelamentoRelatorioDadosBasicos ConsultarCancelamentoRelatorioDadosBasicos(List<int> apresentacoesID)
        {
            string query = @"Select TOP 1 evt.Nome as Evento, reg.nome as Regional,loc.nome as Local, emp.nome as Empresa
                                FROM tApresentacao apr(NOLOCK) 
                                INNER JOIN tEvento evt(NOLOCK) ON apr.EventoID = evt.ID
                                INNER JOIN tLocal loc(NOLOCK) ON evt.LocalId = loc.ID
                                INNER JOIN tEmpresa emp(NOLOCK) ON loc.EmpresaID = emp.ID
                                INNER JOIN tRegional reg(NOLOCK) ON emp.RegionalID = reg.ID
                                WHERE apr.ID in @apresentacoesID";
            return conIngresso.Query<CancelamentoRelatorioDadosBasicos>(query, new
            {
                apresentacoesID = apresentacoesID
            }).FirstOrDefault();
        }

        public CancelamentoRelatorioDadosTotais ConsultarCancelamentoRelatorioDadosTotalizadores(List<int> apresentacoesID)
        {
            string query = @"SELECT COUNT(ID) as TotalIngressos,
	                           COUNT(DISTINCT apresentacaoID) as TotalApresentacoes,
	                           COUNT(DISTINCT PacoteID) as PacotesDesativados,
	                           SUM(Impresso) as Impressos,
	                           SUM(Vendido) as Vendidos, 
	                           (COUNT(ID) - SUM(Impresso) - SUM(Vendido)) as Disponiveis, 
	                           CASE WHEN SUM(VendaAtiva) > 0 then 'True' ELSE 'False' END VendaAtiva
		                        FROM
		                        (
			                        Select tIngresso.ID, tIngresso.apresentacaoID,tIngresso.PacoteID,
			                        CASE WHEN Status = 'I' THEN 1 ELSE 0 END Impresso,
			                        CASE WHEN Status = 'V' THEN 1 ELSE 0 END Vendido,
			                        CASE WHEN DisponivelVenda = 'T' THEN 1 ELSE 0 END VendaAtiva
			                        FROM tIngresso(NOLOCK)
			                        INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
			                        WHERE apresentacaoID in @apresentacoesID
		                        )tbl";
            return conIngresso.Query<CancelamentoRelatorioDadosTotais>(query, new
            {
                apresentacoesID = apresentacoesID
            }).FirstOrDefault();
        }

        public List<CancelamentoRelatorioDadosOperacoes> ConsultarCancelamentoRelatorioDadosOperacoes(string codigoCancelamento,int cancelamentoID)
        {
            if (codigoCancelamento == null)
                codigoCancelamento = "";

            string sql = @"SELECT Operacao, COUNT(ID) as Total, SUM(Resolvidos) as Resolvido  FROM (
                                    SELECT 
                                    Operacao,
                                    cnf.ID,
                                    CASE WHEN cnf.Status in ('C','S','P') THEN 1 ELSE 0 END Resolvidos
                                    FROM tCancelamentoLote(NOLOCK) cnc
                                    INNER JOIN tCancelamentoLoteFila(NOLOCK) cnf ON cnf.CancelamentoLoteID = cnc.ID
                                    WHERE cnc.ID = @cancelamentoID OR cnc.CodigoCancelamento = @codigoCancelamento
                                    ) tbl
                          GROUP BY Operacao";

            return conIngresso.Query<CancelamentoRelatorioDadosOperacoes>(sql, new
            {
                codigoCancelamento = codigoCancelamento,
                cancelamentoID = cancelamentoID
            }).ToList();
        }

        public List<DateTime> ConsultarDataApresentacoes(string codigoCancelamento,int cancelamentoID)
        {
            List<CancelamentoRelatorioMatrizApresentacoes> retorno = new List<CancelamentoRelatorioMatrizApresentacoes>();

            string sql = @"SELECT CalcHorario FROM tCancelamentoLoteApresentacao(NOLOCK) cna
                           INNER JOIN tCancelamentoLote(NOLOCK) cnc ON cnc.ID = cna.CancelamentoLoteID
                           INNER JOIN tApresentacao(NOLOCK) apr ON apr.ID = cna.ApresentacaoID
                           WHERE CancelamentoLoteID = @cancelamentoID OR CodigoCancelamento = @codigoCancelamento";

            return conIngresso.Query<DateTime>(sql, new
                {
                    codigoCancelamento = codigoCancelamento,
                    cancelamentoID = cancelamentoID
                }).ToList();
        }

        public string CarregarDescricaoEmailOperacao(string operacao)
        {
            string sql = @"SELECT DescricaoEmail FROM tCancelamentoLoteOperacoes(NOLOCK)
						   WHERE Operacao = @operacao";
            return conIngresso.Query<string>(sql, new
            {
                operacao = operacao
            }).FirstOrDefault();
        }
    }
}
