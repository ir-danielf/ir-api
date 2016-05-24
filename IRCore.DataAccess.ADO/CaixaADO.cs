using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class CaixaADO : MasterADO<dbIngresso>
    {
        public CaixaADO(MasterADOBase ado = null) : base(ado) { }

        
        public tCaixa ConsultarAberto(int usuarioId)
        {
            var queryStr = @"Select Top 1
                         c.ID, c.UsuarioID, c.LojaID, c.ApresentacaoID, c.SaldoInicial, c.DataAbertura, c.DataFechamento, c.Comissao, c.GerouConciliacao, c.ConciliacaoID
                    FROM tCaixa (nolock) c
                WHERE c.UsuarioID = @usuarioId and (c.DataFechamento is null or c.DataFechamento = '')";

            var query = conIngresso.Query<tCaixa>(queryStr, new { usuarioId = usuarioId });
            return query.FirstOrDefault();
        }

        public bool Salvar(tCaixa caixa, int? UsuarioID)
        {
            #region queries tCaixa
            string checkTCaixa = @"SELECT
                                        CASE WHEN ID IS NULL THEN
                                            0
                                        ELSE
                                            ID
                                        END
                                   FROM
                                        tCaixa(NOLOCK)
                                   WHERE
                                        ID = @ID";
            string selectTcaixa = @"SELECT
                                        ID
                                        ,UsuarioID
                                        ,LojaID
                                        ,ApresentacaoID
                                        ,SaldoInicial
                                        ,DataAbertura
                                        ,DataFechamento
                                        ,Comissao
                                        ,GerouConciliacao
                                        ,ConciliacaoID
                                    FROM
                                        tCaixa(NOLOCK)
                                    WHERE
                                        ID = @ID";
            string insertTCaixa = @"INSERT INTO tCaixa
                                           (ID
                                           ,UsuarioID
                                           ,LojaID
                                           ,ApresentacaoID
                                           ,SaldoInicial
                                           ,DataAbertura
                                           ,DataFechamento
                                           ,Comissao
                                           ,GerouConciliacao
                                           ,ConciliacaoID)
                                           OUTPUT inserted.ID
                                     VALUES
                                           (
                                            (SELECT MAX(ID)+1 FROM cCaixa(NOLOCK))
                                           ,@UsuarioID
                                           ,@LojaID
                                           ,@ApresentacaoID
                                           ,@SaldoInicial
                                           ,@DataAbertura
                                           ,@DataFechamento
                                           ,@Comissao
                                           ,@GerouConciliacao
                                           ,@ConciliacaoID)";
            string updateTCaixa = @"UPDATE
                                        tCaixa
                                    SET 
                                        ID = @ID
                                        ,UsuarioID = @UsuarioID
                                        ,LojaID = @LojaID
                                        ,ApresentacaoID = @ApresentacaoID
                                        ,SaldoInicial = @SaldoInicial
                                        ,DataAbertura = @DataAbertura
                                        ,DataFechamento = @DataFechamento
                                        ,Comissao = @Comissao
                                        ,GerouConciliacao = @GerouConciliacao
                                        ,ConciliacaoID = @ConciliacaoID
                                    WHERE 
                                        ID = @ID";
            #endregion
            #region queries cCaixa
            string insertCCcaixa = @"INSERT INTO cCaixa
                                           (ID
                                           ,Versao
                                           ,Acao
                                           ,TimeStamp
                                           ,UsuarioID)
                                     VALUES
                                           (@ID
                                           ,(SELECT CASE WHEN MAX(Versao) IS NULL THEN 0 ELSE MAX(Versao) + 1 END AS Versao FROM cCaixa(NOLOCK) WHERE ID = @ID)
                                           ,@Acao
                                           ,@TimeStamp
                                           ,@UsuarioID)";
            #endregion
            #region queries xCaixa
            string insertXCaixa = @"INSERT INTO xCaixa
                                           (ID
                                           ,Versao
                                           ,UsuarioID
                                           ,LojaID
                                           ,ApresentacaoID
                                           ,SaldoInicial
                                           ,DataAbertura
                                           ,DataFechamento
                                           ,Comissao
                                           ,ConciliacaoID)
                                     VALUES
                                           (@ID
                                           ,(SELECT CASE WHEN MAX(Versao) IS NULL THEN 0 ELSE MAX(Versao) END AS Versao FROM cCaixa(NOLOCK) WHERE ID = @ID)
                                           ,@UsuarioID
                                           ,@LojaID
                                           ,@ApresentacaoID
                                           ,@SaldoInicial
                                           ,@DataAbertura
                                           ,@DataFechamento
                                           ,@Comissao
                                           ,@ConciliacaoID)";
            #endregion
            bool update = Convert.ToInt32(conIngresso.ExecuteScalar(checkTCaixa, caixa)) > 0;
            bool resultado = false;
            if (update)
            {
                tCaixa caixaO = conIngresso.Query<tCaixa>(selectTcaixa, caixa).FirstOrDefault();
                conIngresso.Execute(insertXCaixa, caixaO);
                resultado = conIngresso.Execute(updateTCaixa, caixa) > 0;
            }
            else
            {
                caixa.ID = conIngresso.Query<int>(insertTCaixa, caixa).FirstOrDefault();
                resultado = caixa.ID > 0;
            }
            conIngresso.Execute(insertCCcaixa, new { ID = caixa.ID, Acao = ((update) ? "U" : "I"), TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"), UsuarioID = UsuarioID });
            return resultado;
        }
    }
}