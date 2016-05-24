using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCore.DataAccess.ADO.Models;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class MediaPartnerReportADO : MasterADO<dbIngresso>
    {

        public MediaPartnerReportADO(MasterADOBase ado = null) : base(ado, true, true) { }

        /// <summary>
        /// Método que retorna uma lista de ReportClientesVoucher
        /// </summary>
        /// <param name="parceiroMidiaID"></param>
        /// <param name="areaID"></param>
        /// <param name="tipoID"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="localID"></param>
        /// <param name="eventoID"></param>
        /// <param name="classeIDs"></param>
        /// <param name="pracas"></param>
        /// <returns></returns>
        public List<ReportClientesTrocaVoucher_Result> ListarReportClientesTrocaVoucher(int parceiroMidiaID = 0, int areaID = 0, int tipoID = 0, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID=0, List<int> classeIDs = null, List<int> pracaIDs = null)
        {
            RelatoriosReportADO consulta = new RelatoriosReportADO();
            var query = consulta.ReportClientesTrocaVoucher(parceiroMidiaID, tipoID, areaID, dataInicial, dataFinal, localID, eventoID, string.Join(",", classeIDs), string.Join(",", pracaIDs));
            //var query = dbIngresso.ReportClientesTrocaVoucher(parceiroMidiaID, tipoID, areaID, dataInicial, dataFinal, localID, eventoID, string.Join(",", classeIDs), string.Join(",", pracaIDs));
            return query.ToList();
        }

        /// <summary>
        /// Método que retorna uma lista de ReportVoucherTrocas
        /// </summary>
        /// <param name="parceiroMidiaId"></param>
        /// <param name="areaId"></param>
        /// <param name="classesId"></param>
        /// <param name="pracas"></param>
        /// <returns></returns>
        public List<ReportVoucherTrocas_Result> ListarReportVoucherTrocas(int parceiroMidiaID, int areaID = 0, List<int> classeIDs = null, List<int> pracaIDs = null, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            var query = dbIngresso.ReportVoucherTrocas(parceiroMidiaID, areaID, string.Join(",", classeIDs), string.Join(",", pracaIDs), dataInicial, dataFinal);
            return query.ToList();
        }

        /// <summary>
        /// Método que retorna uma lista de ReportIngressosLiberados
        /// </summary>
        /// <param name="parceiroMidiaId"></param>
        /// <param name="areaId"></param>
        /// <param name="tipoId"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="localId"></param>
        /// <param name="eventoId"></param>
        /// <param name="classesId"></param>
        /// <param name="pracas"></param>
        /// <returns></returns>
        public List<ReportIngressosLiberados_Result> ListarReportIngressosLiberados(int parceiroMidiaID, int areaID = 0, int tipoID = 0, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0, List<int> classeIDs = null, List<int> pracaIDs = null)
        {
            var query = dbIngresso.ReportIngressosLiberados(parceiroMidiaID, tipoID, areaID, dataInicial, dataFinal, localID, eventoID, string.Join(",", classeIDs), string.Join(",", pracaIDs));
            return query.ToList();
        }
        public List<ReportIngressosDisponiveis_Result> ListarReportIngressosDisponiveis(int parceiroMidiaID, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0, List<int> classeIDs = null)
        {
            var query = dbIngresso.ReportIngressosDisponiveis(parceiroMidiaID, dataInicial, dataFinal, localID, eventoID, string.Join(",", classeIDs));
            return query.ToList();
        }

        public List<ReportHistoricoIngressos_Result> ListarReportHistoricoIngressos(int parceiroMidiaID, int areaID = 0, int tipoID = 0, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0, List<int> classeIDs = null, List<int> pracaIDs = null)
        {
            var query = dbIngresso.ReportHistoricoIngressos(parceiroMidiaID, tipoID, areaID, dataInicial, dataFinal, localID, eventoID, string.Join(",", classeIDs), string.Join(",", pracaIDs));
            return query.ToList();
        }

        /// <summary>
        /// Método que retorna uma lista de Vouchers Disponibilizados X Vouchers Utilizados
        /// </summary>
        /// <param name="parceiroMidiaId"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="localId"></param>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        public List<ReportVoucherUtilizadoDisponibilizado_Result> ListarReportUtilizacaoVoucher(int parceiroMidiaId, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0)
        {
            string query = "Exec ReportVoucherUtilizadoDisponibilizado @parceiroMidiaId, @periodoDe, @peridodoAte, @localID, @eventoID";

            return conIngresso.Query<ReportVoucherUtilizadoDisponibilizado_Result>(query, new { parceiroMidiaId = parceiroMidiaId, periodoDe = (dataInicial == null ? null : Convert.ToDateTime(dataInicial).ToString("yyyyMMddHHmmss")), peridodoAte = (dataFinal == null ? null : Convert.ToDateTime(dataFinal).ToString("yyyyMMddHHmmss")), localID = localID, eventoID = eventoID }).ToList();
        }
    }
}
