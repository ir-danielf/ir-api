using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Dapper;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Models;

namespace IRCore.DataAccess.ADO
{
    public class ApresentacaoADO : MasterADO<dbIngresso>
    {

        List<tMapaEsquematicoSetor> mapaApresentacaoSetor;

        public ApresentacaoADO(MasterADOBase ado = null) : base(ado, true, true) { }




        public List<Apresentacao> ListarIn(List<int> apresentacaoIDs, int eventoID, int parceiroMediaID = 0)
        {
            var query = (from item in dbSite.Apresentacao
                         where item.EventoID == eventoID && apresentacaoIDs.Contains(item.IR_ApresentacaoID)
                         select item);

            if (parceiroMediaID > 0)
            {
                query = query.Where(x => x.PrecoParceiroMidia.Select(y => y.ParceiroMidiaID).Contains(parceiroMediaID));
            }

            return query.OrderBy(t => t.Horario).AsNoTracking().ToList();
        }

        public Apresentacao Consultar(int apresentacaoId)
        {
            string queryStr = @"SELECT 
	                                ID,IR_ApresentacaoID,Horario,EventoID,UsarEsquematico,Programacao,CodigoProgramacao,CalcDiaDaSemana,CalcHorario
                                FROM 
	                                Apresentacao (NOLOCK)
                                WHERE
	                                IR_ApresentacaoID = @ApresentacaoID";
            return conSite.Query<Apresentacao>(queryStr, new { ApresentacaoID = apresentacaoId }).FirstOrDefault();
        }

        public tMapaEsquematico ConsultarMapaEsquematico(int apresentacaoId)
        {
            mapaApresentacaoSetor = new List<tMapaEsquematicoSetor>();

            string sql = @"SELECT me.ID,me.LocalID,me.Nome,
	                        mes.ID,mes.MapaID,mes.SetorID,mes.Coordenadas
	                        FROM tMapaEsquematico me(NOLOCK)
	                        INNER JOIN tApresentacao ap(NOLOCK) ON me.ID = ap.MapaEsquematicoID
	                        INNER JOIN tMapaEsquematicoSetor mes(NOLOCK) ON me.ID = mes.MapaID
                            Where ap.ID = @apresentacaoID";
            var result = conIngresso.Query<tMapaEsquematico, tMapaEsquematicoSetor, tMapaEsquematico>(sql, addMapaEsqematico, new
            {
                apresentacaoId = apresentacaoId
            }).FirstOrDefault();

            if (result == null)
            {
                sql = @"SELECT me.ID,me.LocalID,me.Nome,
	                        mes.ID,mes.MapaID,mes.SetorID,mes.Coordenadas
	                        FROM tMapaEsquematico me(NOLOCK)
	                        INNER JOIN tEvento ev(NOLOCK) ON ev.MapaEsquematicoID = me.ID
	                        INNER JOIN tApresentacao ap(NOLOCK) ON ev.ID = ap.EventoID
	                        INNER JOIN tMapaEsquematicoSetor mes(NOLOCK) ON me.ID = mes.MapaID
                            where ap.ID = @apresentacaoID";

                // se nao encontra, ve se tem mapa do evento
                result = conIngresso.Query<tMapaEsquematico, tMapaEsquematicoSetor, tMapaEsquematico>(sql, addMapaEsqematico, new
                {
                    apresentacaoId = apresentacaoId
                }).FirstOrDefault();
            }
            return result;
        }

        private tMapaEsquematico addMapaEsqematico(tMapaEsquematico me, tMapaEsquematicoSetor mes)
        {
            mapaApresentacaoSetor.Add(mes);
            me.tMapaEsquematicoSetor = mapaApresentacaoSetor;
            return me;
        }



        public Apresentacao ConsultarPorEvento(int eventoID, bool ordemAsc = true)
        {
            var queryStr = @"
                Select Top 1 a.ID, a.IR_ApresentacaoID, a.Horario, a.EventoID, a.UsarEsquematico, a.Programacao, a.CodigoProgramacao, a.CalcDiaDaSemana, a.CalcHorario, (SELECT SUM(QtdeDisponivel) FROM Setor (nolock) WHERE ApresentacaoID = a.IR_ApresentacaoID) as QtdeDisponivel
                  FROM Apresentacao (nolock) a
                WHERE a.EventoID = @eventoID 
                ORDER BY a.CalcHorario " + (ordemAsc ? "asc" : "desc");
            var query = conSite.Query<Apresentacao>(queryStr, new
            {
                eventoID = eventoID
            });
            return query.FirstOrDefault();
        }

        public List<Apresentacao> Listar(int eventoID)
        {
            var queryStr = @"
                Select a.ID, a.IR_ApresentacaoID, a.Horario, a.EventoID, a.UsarEsquematico, a.Programacao, a.CodigoProgramacao, a.CalcDiaDaSemana, a.CalcHorario, (SELECT SUM(QtdeDisponivel) FROM Setor (nolock) WHERE ApresentacaoID = a.IR_ApresentacaoID) as QtdeDisponivel
                  FROM Apresentacao (nolock) a
                WHERE a.EventoID = @eventoID 
                ORDER BY a.CalcHorario";
            var query = conSite.Query<Apresentacao>(queryStr, new
            {
                eventoID = eventoID
            });
            return query.ToList();
        }

        public InfosObrigatoriasIngresso ListarInfosObrigatoriasIngresso(int eventoID)
        {
            var queryStr = @"
                Select a.Alvara, a.AVCB, a.DataEmissaoAlvara, a.DataValidadeAlvara, a.DataEmissaoAvcb, a.DataValidadeAvcb, a.Lotacao
                  FROM tApresentacao (nolock) a
                WHERE a.EventoID = @eventoID";
            var query = conIngresso.Query<InfosObrigatoriasIngresso>(queryStr, new
            {
                eventoID
            });
            return query.FirstOrDefault();
        }

        public List<InformacaoVendaBasicasCancelarMassa> CarregarInformacoesVenda(List<int> apresentacoesID)
        {
            string query = string.Format(@"SELECT Identificados, (Total - Identificados) AS NaoIdentificados, Presencial, (Total - Presencial) AS Remoto, Impressos, (Total - Impressos) AS NaoImpressos, Horario 
                           FROM
                                (
                                   SELECT COUNT(tIngresso.ID) AS Total,
                                          SUM(CASE WHEN tCliente.Email is not null THEN 1 ELSE 0 END) Identificados,
                                          SUM(CASE WHEN tCanal.TipoVenda = 'T' THEN 1 ELSE 0 END) Presencial,    
                                          SUM(CASE WHEN tIngresso.Status = 'I' THEN 1 ELSE 0 END) Impressos,
										  tApresentacao.CalcHorario AS Horario
                                          FROM tIngresso(NOLOCK)
                                          LEFT JOIN tCliente(NOLOCK) ON tCliente.ID = tIngresso.ClienteID
                                          INNER JOIN tVendaBilheteria(NOLOCK) ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID
                                          INNER JOIN tCaixa(NOLOCK) ON tCAixa.ID = tVendaBilheteria.CaixaID
                                          INNER JOIN tLoja(NOLOCK) ON tLoja.ID = tCaixa.LojaID
                                          INNER JOIN tCanal(NOLOCK) ON tLoja.CanalID = tCanal.ID
										  INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
                                          WHERE tIngresso.ApresentacaoID IN ({0})
										  GROUP BY tApresentacao.CalcHorario) as tbl", string.Join(",", apresentacoesID));

            return conIngresso.Query<InformacaoVendaBasicasCancelarMassa>(query).ToList();




        }

        public List<InformacaoVendaFormasPagamento> CarregarInformacoesVendaFormasPagamento(List<int> apresentacoesID)
        {
            string query = string.Format(@"SELECT COUNT(ID) AS Quantidade,FormaPagamento,Horario FROM
                                    (SELECT Distinct tIngresso.ID as ID,
                                    tApresentacao.CalcHorario as Horario,
                                    CASE WHEN (Select COUNT(vendaBilheteriaID) FROM tVendaBilheteriaFormaPagamento(NOLOCK) WHERE vendaBilheteriaID = tVendaBilheteria.ID) = 1 THEN tFormaPagamento.Nome
										 WHEN (Select COUNT(vendaBilheteriaID) FROM tVendaBilheteriaFormaPagamento(NOLOCK) WHERE vendaBilheteriaID = tVendaBilheteria.ID) = 0 THEN 'Sem Pagamento'
										 ELSE 'Múltiplas' END FormaPagamento
                                    FROM tIngresso(NOLOCK)
                                    INNER JOIN tVendaBilheteria(NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
                                    LEFT JOIN tVendaBilheteriaFormaPagamento(NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
                                    LEFT JOIN tFormaPagamento(NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID
                                    INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
                                    where tIngresso.ApresentacaoID IN ({0})) tbl
                                    Group BY FormaPagamento,Horario", string.Join(",", apresentacoesID));

            return conIngresso.Query<InformacaoVendaFormasPagamento>(query).ToList();
        }


        public List<InformacaoValoresOperacao> CarregarInformacoesVendaPorOperacao(List<int> apresentacoesID)
        {
            string query = string.Format(@"	SELECT tCancelamentoLoteOperacoes.Operacao, tCancelamentoLoteOperacoes.Descricao,tCancelamentoLoteOperacoes.EstornoVia, tCancelamentoLoteOperacoes.ContatoCliente,
		                                           tCancelamentoLoteOperacoes.Cancelamento,tCancelamentoLoteOperacoes.AcaoIR, Count(tbl.Operacao) as Total 
												   FROM(SELECT * FROM GetIngressosOperacoes('{0}')) tbl
												   RIGHT JOIN tCancelamentoLoteOperacoes ON tbl.Operacao = tCancelamentoLoteOperacoes.Operacao
												   GROUP BY tCancelamentoLoteOperacoes.Operacao, tCancelamentoLoteOperacoes.Descricao,tCancelamentoLoteOperacoes.EstornoVia, tCancelamentoLoteOperacoes.ContatoCliente,tCancelamentoLoteOperacoes.Cancelamento,tCancelamentoLoteOperacoes.AcaoIR", string.Join(",", apresentacoesID));

            return conIngresso.Query<InformacaoValoresOperacao>(query).ToList();

        }

        public List<CancelamentoRelatorioDadosOperacoes> CarregarInformacoesVendaPorOperacao(int cancelamentoID, string codigocancelamento)
        {
            if (codigocancelamento == null)
                codigocancelamento = "";
            
            string query = @"SELECT Operacao, Descricao, EstornoVia, ContatoCliente, Cancelamento, AcaoIR, COUNT(ID) as Total, SUM(Resolvidos) as Resolvido  FROM (
                                                SELECT 
                                                clo.Operacao,
									            clo.Descricao,
									            clo.EstornoVia,
									            clo.ContatoCliente,
									            clo.Cancelamento,
									            clo.AcaoIR,
                                                cfi.ID,
                                                CASE WHEN cnf.Status in ('C','S','P') THEN 1 ELSE 0 END Resolvidos
									            FROM tCancelamentoLote(NOLOCK) cnc
                                                INNER JOIN tCancelamentoLoteFila(NOLOCK) cnf ON cnf.CancelamentoLoteID = cnc.ID AND (cnc.ID = @cancelamentoID OR cnc.CodigoCancelamento = @codigocancelamento)
									            INNER JOIN tCancelamentoLoteFilaIngresso(NOLOCK) cfi ON cfi.CancelamentoLoteFilaID = cnf.ID
									            RIGHT JOIN tCancelamentoLoteOperacoes(NOLOCK) clo ON clo.Operacao = cnf.Operacao
                                                ) tbl
                          GROUP BY Operacao, Descricao, EstornoVia, ContatoCliente, Cancelamento, AcaoIR";

            return conIngresso.Query<CancelamentoRelatorioDadosOperacoes>(query, new { cancelamentoID = cancelamentoID, codigocancelamento = codigocancelamento }).ToList();

        }

        public void RemoverApresentacoesCanceladasSite(List<int>apresentacoesID)
        {
            string query = string.Format(@"DELETE FROM Preco WHERE ApresentacaoID in ({0});
                                           DELETE FROM Setor WHERE ApresentacaoID in ({0}); 
                                           DELETE FROM Apresentacao WHERE IR_ApresentacaoID in ({0}); 
                                          ",string.Join(",",apresentacoesID));

            conSite.Execute(query);
        }


        public List<DateTime> ApresentacoesCanceladas(int cancelamentoID)
        {
            string query = @"SELECT 
	                            tApresentacao.CalcHorario AS Apresentacao
                            FROM 
	                            tCancelamentoLote
	                            LEFT JOIN tCancelamentoLoteApresentacao (NOLOCK) on tCancelamentoLoteApresentacao.CancelamentoLoteID = tCancelamentoLote.ID
	                            LEFT JOIN tApresentacao (NOLOCK) ON tCancelamentoLoteApresentacao.ApresentacaoID = tApresentacao.ID
                            WHERE 
	                            tCancelamentoLote.ID = @cancelamentoID";
            return conIngresso.Query<DateTime>(query, new { cancelamentoID = cancelamentoID }).ToList();
        }

        public List<ClienteCancelamentoOperacao> ClienteCancelamentoManual(int cancelamentoID)
        {
            string query = "exec Result_ComprasCancelamentoManual @cancelamentoID;";
            return conIngresso.Query<ClienteCancelamentoOperacao>(query, new { cancelamentoID = cancelamentoID }).ToList();
        }

        public List<DateTime> ConsultarDataApresentacoes(List<int>apresentacoesID)
        {
            string sql = @"SELECT CalcHorario FROM tApresentacao(NOLOCK)
                           WHERE ID in @apresentacoesID";

            return conIngresso.Query<DateTime>(sql, new
            {
                apresentacoesID = apresentacoesID
            }).ToList();
        }
        public List<ConferenciaCancelamento> ConferenciaCancelamentoLote(int cancelamentoID)
        {
            string query = "exec Result_ConferenciaCancelamentoLote @cancelamentoID;";
            return conIngresso.Query<ConferenciaCancelamento>(query, new { cancelamentoID = cancelamentoID }).ToList();
        }

    }

}
