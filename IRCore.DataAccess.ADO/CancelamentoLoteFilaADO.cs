using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using IRCore.DataAccess.ADO.Models;
using System.Data.Common;
using System.Data;

namespace IRCore.DataAccess.ADO
{
    public class CancelamentoLoteFilaADO : MasterADO<dbIngresso>
    {
        public CancelamentoLoteFilaADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public bool Atualizar(tCancelamentoLoteFila CancelamentoLoteFila)
        {
            #region query
            string query =
                @"UPDATE 
                    tCancelamentoLoteFila
                  SET
                    CancelamentoLoteID = @CancelamentoLoteID,
                    DataMovimentacao = @DataMovimentacao,
                    VendaBilheteriaID = @VendaBilheteriaID,
                    CanalID = @CanalID,
                    LojaID = @LojaID,
                    Status = @Status,
                    Operacao = @Operacao
                  WHERE
                    ID = @ID";
            #endregion
            var param = new
            {
                ID = CancelamentoLoteFila.ID,
                CancelamentoLoteID = CancelamentoLoteFila.CancelamentoLoteID,
                DataMovimentacao = CancelamentoLoteFila.DataMovimentacao.ToString("yyyyMMddHHmmss"),
                VendaBilheteriaID = CancelamentoLoteFila.VendaBilheteriaID,
                CanalID = CancelamentoLoteFila.CanalID,
                LojaID = CancelamentoLoteFila.LojaID,
                Status = CancelamentoLoteFila.Status,
                Operacao = CancelamentoLoteFila.Operacao
            };
            bool result = conIngresso.Execute(query, param) > 0;
            return result;
        }

        public bool AtualizarSituacaoZero(tCancelamentoLoteFila CancelamentoLoteFila)
        {
            #region query
            string query =
                @"UPDATE 
                    tCancelamentoLoteFila
                  SET
                    CancelamentoLoteID = @CancelamentoLoteID,
                    DataMovimentacao = @DataMovimentacao,
                    VendaBilheteriaID = @VendaBilheteriaID,
                    CanalID = @CanalID,
                    LojaID = @LojaID,
                    Status = @Status
                  WHERE
                    ID = @ID";
            #endregion
            var param = new
            {
                ID = CancelamentoLoteFila.ID,
                CancelamentoLoteID = CancelamentoLoteFila.CancelamentoLoteID,
                DataMovimentacao = CancelamentoLoteFila.DataMovimentacao.ToString("yyyyMMddHHmmss"),
                VendaBilheteriaID = CancelamentoLoteFila.VendaBilheteriaID,
                CanalID = CancelamentoLoteFila.CanalID,
                LojaID = CancelamentoLoteFila.LojaID,
                Status = CancelamentoLoteFila.Status
            };
            bool result = conIngresso.Execute(query, param) > 0;
            return result;
        }

        public List<OperacaoCancelamentoModelQuery> MontarOperacoesCancelamento(List<int> ApresentacoesID)
        {
            #region strQuery
            string strQuery = @"SELECT
                                    iop.VendaBilheteriaID,
                                    loj.CanalID,
                                    loj.ID AS LojaID,
                                    iop.IngressoID,
                                    iop.Operacao
                                FROM
                                    GetIngressosOperacoes(@ApresentacoesID) AS iop
                                    INNER JOIN tCaixa(NOLOCK) AS cai ON cai.ID = iop.CaixaID
                                    INNER JOIN tLoja(NOLOCK) AS loj ON loj.ID = cai.LojaID
                                WHERE
                                    iop.Operacao <> 'G'";
            #endregion

            List<OperacaoCancelamentoModelQuery> result = conIngresso.Query<OperacaoCancelamentoModelQuery>(strQuery, new { ApresentacoesID = String.Join(",", ApresentacoesID) }).ToList();
            return result;
        }

        public List<tCancelamentoLoteFila> ConsultarCancelamentosNaFila(int Quantidade)
        {
            #region querytCancelamentoLoteFila
            string querytCancelamentoLoteFila =
                "SELECT TOP " + Quantidade + @"
                    tCLF.ID, tCLF.CancelamentoLoteID, dbo.StringToDateTime(tCLF.DataMovimentacao) AS DataMovimentacao, tCLF.VendaBilheteriaID, tCLF.CanalID, tCLF.LojaID, tCLF.Status, tCLF.Operacao
                FROM
                    tCancelamentoLoteFila(NOLOCK) AS tCLF
                WHERE
                    tCLF.Status = 'F'
                ORDER BY
                    tCLF.LojaID";
            #endregion
            #region querytCancelamentoLoteFilaIngresso
            string querytCancelamentoLoteFilaIngresso =
                @"SELECT
                    tCLFI.ID, tCLFI.CancelamentoLoteFilaID, tCLFI.IngressoID
                  FROM
                    tCancelamentoLoteFilaIngresso(NOLOCK) AS tCLFI
                    INNER JOIN tIngresso(NOLOCK) AS ing ON ing.ID = tCLFI.IngressoID
                  WHERE
                    tCLFI.CancelamentoLoteFilaID IN @IDS
                    AND ing.Status NOT IN ('D', 'R', 'B')";
            #endregion
            List<tCancelamentoLoteFila> lista;
            lista = conIngresso.Query<tCancelamentoLoteFila>(querytCancelamentoLoteFila).ToList();
            foreach (tCancelamentoLoteFila item in lista)
            {
                item.Ingressos = new List<tCancelamentoLoteFilaIngresso>();
            }
            List<tCancelamentoLoteFilaIngresso> listaIngressos;
            listaIngressos = conIngresso.Query<tCancelamentoLoteFilaIngresso>(querytCancelamentoLoteFilaIngresso, new { IDS = lista.Select(x => x.ID).ToList() }).ToList();
            foreach (tCancelamentoLoteFilaIngresso item in listaIngressos)
            {
                tCancelamentoLoteFila fila = lista.Where(x => x.ID == item.CancelamentoLoteFilaID).Select(x => x).FirstOrDefault();
                fila.Ingressos.Add(item);
            }
            return lista;
        }

        public void PopularCancelamentoFila(List<tCancelamentoLoteFila> CancelamentoFila)
        {
            #region queryInsertLoteFila
            string queryInsertLoteFila = @"INSERT INTO 
                                                tCancelamentoLoteFila
                                                (CancelamentoLoteID, DataMovimentacao, VendaBilheteriaID, CanalID, LojaID, Status, Operacao) 
                                                OUTPUT inserted.ID
                                           VALUES
                                                (@CancelamentoLoteID, @DataMovimentacao, @VendaBilheteriaID, @CanalID, @LojaID, 'F', @Operacao)";
            #endregion
            #region queryInsertLoteFilaIngresso
            string queryInsertLoteFilaIngresso = @"INSERT INTO 
                                                tCancelamentoLoteFilaIngresso
                                                (CancelamentoLoteFilaID, IngressoID) 
                                                OUTPUT inserted.ID
                                           VALUES
                                                (@CancelamentoLoteFilaID, @IngressoID)";
            #endregion

            DbTransaction Transaction = null;
            try
            {
                if (conIngresso.State != ConnectionState.Open)
                {
                    conIngresso.Open();
                }
                Transaction = conIngresso.BeginTransaction();
                foreach (tCancelamentoLoteFila clf in CancelamentoFila)
                {
                    object param = new
                    {
                        CancelamentoLoteID = clf.CancelamentoLoteID,
                        DataMovimentacao = clf.DataMovimentacao.ToString("yyyyMMddHHmmss"),
                        VendaBilheteriaID = clf.VendaBilheteriaID,
                        CanalID = clf.CanalID,
                        LojaID = clf.LojaID,
                        Status = clf.Status,
                        Operacao = clf.Operacao
                    };
                    clf.ID = conIngresso.Query<int>(queryInsertLoteFila, param, transaction: Transaction).FirstOrDefault();
                    foreach (tCancelamentoLoteFilaIngresso clfi in clf.Ingressos)
                    {
                        clfi.CancelamentoLoteFilaID = clf.ID;
                        clfi.ID = conIngresso.Query<int>(queryInsertLoteFilaIngresso, clfi, transaction: Transaction).FirstOrDefault();
                    }

                }
                Transaction.Commit();
            }
            catch (Exception ex)
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (conIngresso.State == ConnectionState.Open)
                {
                    conIngresso.Close();
                }
            }
        }

        public List<CancelamentoLoteMailModel> CarregarCamposEmail(int vendaBilheteriaID)
        {
            return CarregarCamposEmail(null, vendaBilheteriaID);
        }
        public List<CancelamentoLoteMailModel> CarregarCamposEmail(DbTransaction dbTrans, int vendaBilheteriaID)
        {
            string query = @"SELECT TOP 1 tCliente.Nome as Cliente, tCliente.CPF, tCLiente.Email, tVendaBilheteria.Senha, 
			                    tvendaBilheteria.CalcDataVenda as Data, tCanal.Nome as Canal,tFormaPagamento.Nome as Pagamento,tEvento.Nome, tCancelamentoLoteOperacoes.DescricaoEmail
                             FROM tVendaBilheteria(NOLOCK)
                             INNER JOIN tCliente(NOLOCK) ON tCLiente.ID = tVendaBIlheteria.ClienteID
                             INNER JOIN tCaixa(NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                             INNER JOIN tLoja(NOLOCK) ON tLoja.ID = tCaixa.LojaId
                             INNER JOIN tCanal(NOLOCK) ON tCanal.ID = tLoja.CanalID
                             INNER JOIN tVendaBilheteriaFormaPagamento(NOLOCK) ON tVendaBilheteriaFormaPagamento.vendaBilheteriaID = tVendaBilheteria.ID
                             INNER JOIN tFormaPagamento(NOLOCK) ON tFormaPagamento.Id = tVendaBilheteriaFormaPagamento.FormaPagamentoID
                             INNER JOIN tCancelamentoLoteFila(NOLOCK) ON tCancelamentoLoteFila.VendabilheteriaID = tVendaBilheteria.ID
							 INNER JOIN tCancelamentoLote(NOLOCK) ON tCancelamentoLote.ID = tCancelamentoLoteFila.CancelamentoLoteID
							 INNER JOIN tEvento(NOLOCK) ON tEvento.ID = tCancelamentoLote.EventoID
                             INNER JOIN tCancelamentoLoteOperacoes(NOLOCK) ON tCancelamentoLoteOperacoes.Operacao = tCancelamentoLoteFila.Operacao
                             WHERE tVendaBilheteria.ID = @vendaBilheteriaID";

            return conIngresso.Query<CancelamentoLoteMailModel>(query, new { vendaBilheteriaID = vendaBilheteriaID }, dbTrans).ToList();
        }

        public bool TemCancelamentoPendente(DbTransaction dbTrans, int vendaBilheteriaID, List<tIngresso> lstIngressoID)
        {
            

            string query = @"SELECT count(cdpi.ID)
                               FROM tCancelDevolucaoPendente cdp ( NOLOCK )
                              INNER JOIN tCancelDevolucaoPendenteIngresso cdpi ( NOLOCK ) ON cdp.id = cdpi.CancelDevolucaoPendenteID
                              WHERE cdp.VendaBilheteriaIDVenda = @VBID
                                AND cdp.StatusCancel = 'P'
                                AND cdpi.IngressoID in @Ingressos;";

            return conIngresso.Query<int>(query, new { VBID = vendaBilheteriaID, Ingressos = lstIngressoID.Select(x=>x.ID) }, dbTrans).FirstOrDefault() > 0;
        }

        public List<CancelamentoLoteIngressoPendente> ListarIngressosCancelamentoPendente(DbTransaction dbTrans, int vendaBilheteriaID, List<tIngresso> lstIngressoID)
        {
            string query;
            List<CancelamentoLoteIngressoPendente> retorno = new List<CancelamentoLoteIngressoPendente>();

            query = @"SELECT cdp.ID as PendenciaID, cdpi.IngressoID
                        FROM tCancelDevolucaoPendente cdp ( NOLOCK )
                       INNER JOIN tCancelDevolucaoPendenteIngresso cdpi ( NOLOCK ) ON cdp.id = cdpi.CancelDevolucaoPendenteID
                       WHERE cdp.ID IN (SELECT DISTINCT cdp.ID
                                          FROM tCancelDevolucaoPendente cdp ( NOLOCK )
                                         INNER JOIN tCancelDevolucaoPendenteIngresso cdpi ( NOLOCK ) ON cdp.id = cdpi.CancelDevolucaoPendenteID
                                         WHERE cdp.VendaBilheteriaIDVenda = @VBID
                                           AND cdp.StatusCancel = 'P'
                                           AND cdpi.IngressoID IN @Ingressos);";

            retorno = conIngresso.Query<CancelamentoLoteIngressoPendente>(query, new { VBID = vendaBilheteriaID, Ingressos = lstIngressoID.Select(n => n.ID) }, dbTrans).ToList();

            return retorno;
        }

        public IEnumerable<VerificarOperacoesModelQuery> VerificarAlteracaoOperacao(List<tCancelamentoLoteFila> Lista)
        {
            List<int> ingIDS = new List<int>();
            foreach(tCancelamentoLoteFila item in Lista)
            {
                ingIDS.AddRange(item.Ingressos.Select(i => i.IngressoID));
            }
            string queryApresentacoes = @"SELECT DISTINCT
                                                ApresentacaoID
                                            FROM
                                                tIngresso(NOLOCK)
                                            WHERE
                                                ID IN @IIDS";
            var ApsID = conIngresso.Query<int>(queryApresentacoes, new { IIDS = ingIDS });
            string query = @"SELECT DISTINCT
                                VendaBilheteriaID
                                ,Operacao 
                             FROM
                                dbo.GetIngressosOperacoes(@ApsID)
                             WHERE
                                VendaBilheteriaID IN @VbsID;";
            return conIngresso.Query<VerificarOperacoesModelQuery>(query, new { ApsID = String.Join(",", ApsID), VbsID = Lista.Select(i => i.VendaBilheteriaID) });
        }

        public bool EstornarEntrega(DbTransaction dbTrans, int vendaBilheteriaID, int qtdIngressos)
        {
            string query;
            CancelamentoLoteDadosImpressao result = new CancelamentoLoteDadosImpressao();

            query = @"SELECT count (log.IngressoID) AS qtdIngresso,
                             count (DISTINCT ec.id) AS qtdEntrega,
                             sum (CASE WHEN e.PermitirImpressaoInternet = 'T'
                                       THEN 1
                                       ELSE 0 
                                 END)       AS qtdETicket,
                             sum (CASE WHEN i.Status = 'I'
                                       THEN 1
                                       ELSE 0 
                                 END)       AS qtdImpresso
                        FROM tVendaBilheteria vb( NOLOCK )
                       INNER JOIN tIngressoLog log ( NOLOCK ) ON log.VendaBilheteriaID = vb.ID AND log.Acao = 'V'
                       INNER JOIN tIngresso i ( NOLOCK ) ON i.ID = log.IngressoID
                        LEFT JOIN tEntregaControle ec ( NOLOCK ) ON ec.ID = vb.EntregaControleID
                        LEFT JOIN tEntrega e ( NOLOCK ) ON e.ID = ec.EntregaID
                       WHERE vb.id = @VBID;";

            result = conIngresso.Query<CancelamentoLoteDadosImpressao>(query, new { VBID = vendaBilheteriaID }, dbTrans).FirstOrDefault();

            return result.qtdEntrega == 0 ||
                   (result.qtdIngresso == qtdIngressos &&
                    (result.qtdImpresso == 0 ||
                     result.qtdETicket == qtdIngressos)
                   );
        }

        public CancelamentoLoteDadosCliente CarregarDadosCliente(DbTransaction dbTrans, int vendaBilheteriaID)
        {
            string query;
            CancelamentoLoteDadosCliente retorno = new CancelamentoLoteDadosCliente();

            query = @"SELECT CASE WHEN isnull (ct.NomeCartao, '') = ''
                                  THEN c.Nome COLLATE Latin1_General_BIN
                                  ELSE ct.NomeCartao COLLATE Latin1_General_BIN
                             END          AS Nome,
                             c.Email,
                             c.CPF,
                             ct.NroCartao AS Cartao,
                             b.Nome       AS Bandeira
                        FROM tVendaBilheteria vb ( NOLOCK )
                       INNER JOIN tCliente c ( NOLOCK ) ON c.ID = vb.ClienteID
                       INNER JOIN tVendaBilheteriaFormaPagamento vbfp ON vb.ID = vbfp.VendaBilheteriaID
                        LEFT JOIN tCartao ct ( NOLOCK ) ON ct.id = vbfp.CartaoID
                        LEFT JOIN tBandeira b ( NOLOCK ) ON b.ID = ct.BandeiraID
                       WHERE vb.id = @VBID;";

            retorno = conIngresso.Query<CancelamentoLoteDadosCliente>(query, new { VBID = vendaBilheteriaID }, dbTrans).FirstOrDefault();

            return retorno;
        }

        public CancelamentoLoteDadosCancelamento CarregarDadosVendaCancelamento(List<int> ingressosID, DbTransaction dbtrans)
        {
            string sql = @"SELECT TOP 1 tVendaBilheteria.ID as VendaBilheteriaIDVenda, tIngresso.ClienteID, Senha as SenhaVenda, 
                                EntregaAgendaID, EntregaControleID, CASE WHEN tVendabilheteria.Fraude = 1 THEN 'True' ELSE 'False' END Fraude,
                                tVendaBilheteria.ValorSeguro as ValorSeguroTotal, tVendaBilheteria.TaxaEntregaValor as ValorEntregaTotal,
                                tVendaBilheteria.TaxaConvenienciaValorTotal as ValorConvenienciaTotal, 
                                (tVendaBilheteria.ValorTotal - tVendaBilheteria.ValorSeguro - tVendaBilheteria.TaxaEntregaValor - tVendaBilheteria.TaxaConvenienciaValorTotal) as ValorIngressosTotal
                           FROM tIngresso(NOLOCK)
                           INNER JOIN tVendaBilheteria(NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID                               
                           WHERE tIngresso.ID in @ingressosID";
            return conIngresso.Query<CancelamentoLoteDadosCancelamento>(sql, new { ingressosID = ingressosID },dbtrans).FirstOrDefault();
        }

        public CancelamentoLoteValoresPendentes CarregarValoresDadosPendentes(List<int> IngressosID, DbTransaction dbtrans)
        {
            string sql = @"SELECT   EntregaValor,
                                    SeguroValor,
                                    SUM (ConvenienciaValor) AS ConvenienciaValor,
                                    SUM (IngressoValor)     AS IngressoValor
                                FROM (
                                         SELECT DISTINCT
                                             tVendaBilheteria.TaxaEntregaValor          AS EntregaValor,
                                             tVendaBilheteria.ValorSeguro               AS SeguroValor,
                                             tVendaBilheteriaItem.TaxaConvenienciaValor AS ConvenienciaValor,
                                             SUm (tPreco.Valor)                         AS IngressoValor,
                                             tVendabilheteriaItem.ID                    AS VendaBilheteriaItem
                                         FROM tIngresso
                                             ( NOLOCK ) INNER JOIN tIngressoLog
                                             ( NOLOCK ) ON tIngressoLog.ID = (SELECT MAX (ID)
                                                                              FROM tIngressoLog
                                                                              WHERE IngressoID = tIngresso.ID AND Acao = 'V')
                                             INNER JOIN tVendaBilheteriaItem
                                             ( NOLOCK ) ON tIngressoLog.VendaBilheteriaItemID = tVendabilheteriaItem.ID
                                             INNER JOIN tVendaBilheteria
                                             ( NOLOCK ) ON tVendaBilheteria.Id = tIngresso.VendaBilheteriaID
                                             INNER JOIN tPreco
                                             ( NOLOCK ) ON tPreco.ID = tIngresso.PrecoID
                                         WHERE tIngresso.ID IN  @IngressosID
                                         GROUP BY tVendaBilheteria.TaxaEntregaValor, tVendaBilheteria.ValorSeguro,
                                             tVendaBilheteriaItem.TaxaConvenienciaValor, tVendaBilheteriaItem.ID) AS tbl
                                GROUP BY EntregaValor, SeguroValor";

            return conIngresso.Query<CancelamentoLoteValoresPendentes>(sql, new { IngressosID = IngressosID },dbtrans).FirstOrDefault();
        }

        public List<tCancelamentoLoteFila> CarregarCancelamentoManuaisProcessados()
        {
            #region querytCancelamentoLoteFila
            string querytCancelamentoLoteFila =
                @"SELECT
                    tCLF.ID, tCLF.CancelamentoLoteID, dbo.StringToDateTime(tCLF.DataMovimentacao) AS DataMovimentacao, tCLF.VendaBilheteriaID, tCLF.CanalID, tCLF.LojaID, tCLF.Status, tCLF.Operacao
                FROM
                    tCancelamentoLoteFila(NOLOCK) AS tCLF
                WHERE
                    tCLF.Status NOT IN ('F', 'C')
                    AND tCLF.Operacao NOT IN ('D', 'E')
                ORDER BY
                    tCLF.LojaID";
            #endregion
            #region querytCancelamentoLoteFilaIngresso
            string querytCancelamentoLoteFilaIngresso =
                @"SELECT
                    tCLFI.ID, tCLFI.CancelamentoLoteFilaID, tCLFI.IngressoID
                  FROM
                    tCancelamentoLoteFilaIngresso(NOLOCK) AS tCLFI
                    INNER JOIN tIngresso(NOLOCK) AS ing ON ing.ID = tCLFI.IngressoID
                  WHERE
                    tCLFI.CancelamentoLoteFilaID IN (@IDS)
                    AND ing.Status NOT IN ('D', 'R', 'B')";
            #endregion
            List<tCancelamentoLoteFila> lista;
            lista = conIngresso.Query<tCancelamentoLoteFila>(querytCancelamentoLoteFila).ToList();
            foreach (tCancelamentoLoteFila item in lista)
            {
                item.Ingressos = new List<tCancelamentoLoteFilaIngresso>();
            }
            List<tCancelamentoLoteFilaIngresso> listaIngressos;
            querytCancelamentoLoteFilaIngresso = querytCancelamentoLoteFilaIngresso.Replace("@IDS", String.Join(",", lista.Select(x => x.ID).ToList()));
            listaIngressos = conIngresso.Query<tCancelamentoLoteFilaIngresso>(querytCancelamentoLoteFilaIngresso).ToList();
            foreach (tCancelamentoLoteFilaIngresso item in listaIngressos)
            {
                tCancelamentoLoteFila fila = lista.Where(x => x.ID == item.CancelamentoLoteFilaID).Select(x => x).FirstOrDefault();
                fila.Ingressos.Add(item);
            }
            return lista;
        }
    }
}