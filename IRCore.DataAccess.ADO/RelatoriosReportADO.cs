using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.Entity.Core.Objects;

namespace IRCore.DataAccess.ADO
{
    public class RelatoriosReportADO : MasterADO<dbIngresso>
    {
        public RelatoriosReportADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public List<ReportClientesTrocaVoucher_Result> ReportClientesTrocaVoucher(Nullable<int> parceiroMidiaID, Nullable<int> tipoID, Nullable<int> areaID, Nullable<System.DateTime> periodoDe, Nullable<System.DateTime> peridodoAte, Nullable<int> localID, Nullable<int> eventoID, string classesIDs, string pracasIDs)
        {

            string query = "Exec ReportClientesTrocaVoucher @parceiroMidiaID, @tipoID,@areaID,@periodoDe,@peridodoAte,@localID,@eventoID,@classesIDs, @pracasIDs ";
            return conIngresso.Query<ReportClientesTrocaVoucher_Result>(query, new 
            {
                parceiroMidiaID = parceiroMidiaID == null ? null : parceiroMidiaID,
                tipoID = tipoID == null ? null : tipoID,
                areaID = areaID == null ? null : areaID,
                periodoDe = periodoDe == null ? null : periodoDe,
                peridodoAte = peridodoAte == null ? null : peridodoAte,
                localID = localID == null ? null : localID,
                eventoID = eventoID == null ? null : eventoID,
                classesIDs = string.IsNullOrEmpty(classesIDs) ? null : classesIDs,
                pracasIDs = string.IsNullOrEmpty(pracasIDs) ? null : pracasIDs 
            }).ToList();
        }

        public List<ReportIngressosOffline_Result> IngressosOffline(int eventoID, int apresentacaoID, string statusIngresso)
        {
            string query = "Exec ReportIngressosOffline @eventoID,@apresentacaoID,@statusIngresso";
            return conIngresso.Query<ReportIngressosOffline_Result>(query, new { eventoID = eventoID, apresentacaoID = apresentacaoID, statusIngresso = statusIngresso }).ToList();
        }

        public List<ListaEventosPorNome_Result> ListarEventoPorNome(string evento)
        {
            var query = dbIngresso.ListaEventosPorNome(evento);
            return query.ToList();
        }
        public List<ListaLocalPorNome_Result> ListarLocalPorNome(string local, List<int> eventosID)
        {
            if (eventosID == null)
                return new List<ListaLocalPorNome_Result>();
            string query = @"SELECT DISTINCT
	                            Loc.ID AS LocalID
	                            ,Loc.Nome AS LocalNome
	                            ,Emp.Nome AS EmpresaNome
	                            ,Reg.Nome AS RegionalNome
                            FROM
	                            tEvento(NOLOCK) AS Eve
	                            INNER JOIN tLocal(NOLOCK) AS Loc ON Loc.ID = Eve.LocalID
	                            INNER JOIN tEmpresa(NOLOCK) AS Emp ON Emp.ID = Loc.EmpresaID
	                            INNER JOIN tRegional(NOLOCK) AS Reg ON Reg.ID = Emp.RegionalID
                            WHERE
                                Loc.Nome like @local ";
            if(eventosID.Count != 0)
            {
                query += "AND Eve.ID IN (" + String.Join(",", eventosID) + ")";
            }
            return conIngresso.Query<ListaLocalPorNome_Result>(query, new { local = "%"+local+"%" }).ToList();
        }

        public List<int> ListarLocaisID(List<int> eventosID)
        {
            if (eventosID == null)
                return new List<int>();
            string query = @"SELECT DISTINCT
	                            Loc.ID
                            FROM
	                            tEvento(NOLOCK) AS Eve
	                            INNER JOIN tLocal(NOLOCK) AS Loc ON Loc.ID = Eve.LocalID";
            if (eventosID.Count != 0)
            {
                query += "WHERE Eve.ID IN (" + String.Join(",", eventosID) + ")";
        }
            return conIngresso.Query<int>(query).ToList();
        }

        public DateTime ListarPrimeiraApresentacao(List<int> eventosID, int localID)
        {
            if (eventosID == null)
                return DateTime.MinValue;
            string query = @"SELECT DISTINCT
	                            Apr.CalcHorario
                            FROM
	                            tEvento(NOLOCK) AS Eve
	                            INNER JOIN tLocal(NOLOCK) AS Loc ON Loc.ID = Eve.LocalID
	                            INNER JOIN tApresentacao(NOLOCK) AS Apr ON Apr.EventoID = Eve.ID
                            WHERE
                                Apr.DisponivelVenda = 'T'
                                ";
            if (eventosID.Count != 0)
            {
                string eventos = String.Join(",", eventosID);
                query += "AND Eve.ID IN (" + eventos + ")";
            }
            if(localID != 0)
            {
                query += " AND Loc.ID = " + localID;
            }
            return conIngresso.Query<DateTime>(query).FirstOrDefault();
        }

        public List<ReportWillCall_Result> WillCall(string eventosID, string status, DateTime? inicial, DateTime? final, int localID)
        {
            string query = "Exec ReportWillCall @eventosID,@status,@inicial,@final,@localID";
            return conIngresso.Query<ReportWillCall_Result>(query, new { eventosID = eventosID, status = status, inicial = (inicial == DateTime.MinValue ? null : inicial), final = (final == DateTime.MinValue ? null : final), localID = localID }).ToList();
        }

        public List<ReportListaNegra_Result> ListaNegra(int eventoID)
        {
            string query = "Exec ReportListaNegra @eventoID";
            return conIngresso.Query<ReportListaNegra_Result>(query, new { eventoID = eventoID}).ToList();
        }

        public List<ReportRaioX_Result> RaioX(int eventoID, int? apresentacaoID)
        {
            var query = dbIngresso.ReportRaioX(eventoID, apresentacaoID);
            return query.ToList();
        }

        public List<ReportCompradores_Result> Compradores(int eventoID, int? apresentacaoID, string statusIngresso, int? entregaControleID, string statusCompra)
        {
            string query = "Exec ReportCompradores @eventoID,@apresentacaoID,@statusIngresso,@entregaControleID,@statusCompra";
            return conIngresso.Query<ReportCompradores_Result>(query, new { eventoID = eventoID, apresentacaoID = apresentacaoID, statusIngresso = statusIngresso, entregaControleID = entregaControleID, statusCompra = statusCompra}).ToList();
        }

        public List<ReportPrevisaoPDV_Result> PrevisaoPDV(string PDV, string estado, string cidade, List<int> eventosIDs)
        {
            string query = @"SELECT 
	                            tPontoVenda.Estado
	                            ,tPontoVenda.Cidade
	                            ,tPontoVenda.Nome
	                            ,COUNT(DISTINCT tVendaBilheteria.ID) AS Compras
	                            ,COUNT(tIngresso.ID) AS Ingressos
                            FROM
	                            tPontoVenda (NOLOCK)
	                            INNER JOIN tVendaBilheteria (NOLOCK) ON tPontoVenda.ID = tVendaBilheteria.PdvID
	                            INNER JOIN tIngresso (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
	                            INNER JOIN tApresentacao (NOLOCK) ON tIngresso.ApresentacaoID = tApresentacao.ID
                            WHERE
	                            tApresentacao.CalcHorario >= GETDATE()
	                            AND tIngresso.Status = 'V'
                                AND (tPontoVenda.Estado = @estado OR @estado IS NULL)
                                AND (tPontoVenda.Cidade = @cidade OR @cidade IS NULL)
                                AND (tPontoVenda.Nome LIKE @PDV OR @PDV IS NULL)";
                            if (eventosIDs.Count != 0)
                            {
                                query += " AND tIngresso.EventoID IN (" + String.Join(",",eventosIDs) + ")";
                            }
                    query += @"GROUP BY 
	                            tPontovenda.ID
	                            ,tPontoVenda.Nome
	                            ,tPontoVenda.Cidade
	                            ,tPontoVenda.Estado
                            ORDER BY 
	                            tPontoVenda.Estado
	                            ,tPontoVenda.Cidade
	                            ,tPontoVenda.Nome";
            return conIngresso.Query<ReportPrevisaoPDV_Result>(query, new { PDV = (PDV != null ? "%" + PDV + "%" : PDV), estado = estado, cidade = cidade }).ToList();
        }

        public List<tApresentacao> ListarApresentacaoPorEvento(int evento)
        {
            var query = (from item in dbIngresso.tApresentacao
                         where item.EventoID == evento && item.DisponivelVenda == "T"
                         select item); 
            return query.ToList();
        }

        public List<MeioEntregaVM> ListarMeioEntrega(int eventoID)
        {
            var query = @"SELECT DISTINCT 
	                        tEntrega.ID AS ID, 
	                        tEntrega.Nome AS label
                        FROM 
	                        tEventoEntregaControle 
	                        INNER JOIN tEntregaControle ON tEventoEntregaControle.EntregaControleID = tEntregaControle.ID 
	                        INNER JOIN tEntrega ON tEntregaControle.EntregaID = tEntrega.ID
                        WHERE
	                        eventoID = @eventoID";
            return conIngresso.Query<MeioEntregaVM>(query, new { eventoID = eventoID }).ToList();
        }

        public List<EstadosVM> ListarEstados()
        {
            var query = @"SELECT 
	                        tEstado.Sigla AS estado 
                        FROM 
	                        tEstado";
            return conIngresso.Query<EstadosVM>(query).ToList();
        }

        public List<CidadesVM> ListarCidades(string estadoSigla)
        {
            var query = @"SELECT DISTINCT 
	                        tCidade.Nome AS cidade 
                        FROM 
	                        tCidade 
                        WHERE 
	                        tCidade.EstadoSigla = @estadoSigla";
            return conIngresso.Query<CidadesVM>(query, new { estadoSigla = estadoSigla }).ToList();
        }

        public List<RelatorioClientesCotaNet> ListarReportClientesCotaNet(string DataFiltro)
        {
            string query = @"SELECT 
                             c.CPF AS CPFResponsavelCompra
                             ,c.Nome AS NomeResponsavelCompra
                             ,l.Nome AS Local
                             ,e.Nome AS Evento
                             ,(SUBSTRING(a.Horario,7,2) + '/' + SUBSTRING(a.Horario,5,2) + '/' + SUBSTRING(a.Horario,1,4)) AS DataApresentacao
                             ,(SUBSTRING(a.Horario,9,2) + ':' + SUBSTRING(a.Horario,11,2) + ':' + SUBSTRING(a.Horario,13,2)) AS HoraApresentacao
                             ,s.Nome AS Setor
                             ,(SUBSTRING(vb.DataVenda,7,2) + '/' + SUBSTRING(vb.DataVenda,5,2) + '/' + SUBSTRING(vb.DataVenda,1,4) + ' ' + SUBSTRING(vb.DataVenda,9,2) + ':' + SUBSTRING(vb.DataVenda,11,2) + ':' + SUBSTRING(vb.DataVenda,13,2)) AS DataCompra 
                             ,p.Valor AS ValorTicket
                             ,p.Nome AS NomeValorTicket
                             ,COUNT(*) AS QuantidadeTicket
                             , CASE 
                              WHEN DBO.StringToDateTime(vb.DataVenda) > '2015-08-21 23:59:59.999'
                              THEN 
                               ic.CodigoPromocional
                              ELSE
                               ''
                              END AS CPFNET
                            FROM 
                             tIngresso i(NOLOCK) 
                             LEFT JOIN tVendaBilheteria vb (NOLOCK) on vb.ID = i.VendaBilheteriaID
                             INNER JOIN tIngressoCliente ic (NOLOCK) on ic.IngressoID = i.ID
                             INNER JOIN tCliente c (NOLOCK) on c.ID = i.ClienteID
                             INNER JOIN tCotaItem cti (NOLOCK) on cti.ID = ic.CotaItemID
                             INNER JOIN tLocal l (NOLOCK) on l.ID = i.LocalID 
                             INNER JOIN tEvento e (NOLOCK) on e.ID = i.EventoID
                             INNER JOIN tApresentacao a (NOLOCK) on a.ID = i.ApresentacaoID
                             INNER JOIN tSetor s (NOLOCK) on s.ID = i.SetorID
                             INNER JOIN tPreco p (NOLOCK) on p.ID = i.PrecoID
                            WHERE 
                             vb.DataVenda LIKE @DataFiltro
                             and l.ID in (3225, 2044, 3373)
                            GROUP BY 
                             c.CPF
                             ,c.Nome
                             ,l.Nome
                             ,e.Nome
                             ,a.Horario
                             ,s.Nome
                             ,vb.DataVenda
                             ,p.Valor
                             ,p.Nome
                             ,ic.CodigoPromocional
                            ORDER BY 
                             l.Nome,e.Nome,a.Horario,s.Nome";

            return conIngresso.Query<RelatorioClientesCotaNet>(query, new { DataFiltro = (DataFiltro != null ? DataFiltro + "%" : DataFiltro) }).ToList();
        }
    }
}