using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCore.DataAccess.ADO.Models;

namespace IRCore.BusinessObject
{
    public class MediaPartnerReportBO : MasterBO<MediaPartnerReportADO>
    {
        public MediaPartnerReportBO(MasterADOBase ado) : base(ado) { }


        /// <summary>
        /// Método que retorna uma lista de ReportClientesVoucher
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
        public List<ReportClientesTrocaVoucher_Result> ListarReportClientesTrocaVoucher(int parceiroMidiaID, int areaID = 0, int tipoID = 0, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0, List<int> classeIDs = null, List<int> pracaIDs = null)
        {
            return ado.ListarReportClientesTrocaVoucher(parceiroMidiaID, areaID, tipoID, dataInicial, dataFinal, localID, eventoID, classeIDs, pracaIDs);
        }

        /// <summary>
        /// Método que retorna uma lista de ReportVoucherTrocas
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
        public List<ReportVoucherTrocas_Result> ListarReportVoucherTrocas(int parceiroMidiaID, int areaID = 0, List<int> classeIDs = null, List<int> pracaIDs = null, DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            return ado.ListarReportVoucherTrocas(parceiroMidiaID, areaID, classeIDs, pracaIDs, dataInicial, dataFinal);
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
            return ado.ListarReportIngressosLiberados(parceiroMidiaID, areaID, tipoID, dataInicial, dataFinal, localID, eventoID, classeIDs, pracaIDs); 
        }

        public List<ReportIngressosDisponiveis_Result> ListarReportIngressosDisponiveis(int parceiroMidiaID, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0, List<int> classeIDs = null)
        {
            return ado.ListarReportIngressosDisponiveis(parceiroMidiaID, dataInicial, dataFinal, localID, eventoID, classeIDs);
        }

        public List<ReportHistoricoIngressos_Result> ListarReportHistoricoIngressos(int parceiroMidiaID, int areaID = 0, int tipoID = 0, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0, List<int> classeIDs = null, List<int> pracaIDs = null)
        {
            return ado.ListarReportHistoricoIngressos(parceiroMidiaID, areaID, tipoID, dataInicial, dataFinal, localID, eventoID, classeIDs, pracaIDs);
        }

        /// <summary>
        /// Método que retorna uma lista de Vouchers Disponibilizados X Vouchers Utilizados
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="localId"></param>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        public List<ReportVoucherUtilizadoDisponibilizado_Result> ListarReportUtilizacaoVoucher(int parceiroMidiaID, DateTime? dataInicial = null, DateTime? dataFinal = null, int localID = 0, int eventoID = 0)
        {
            return ado.ListarReportUtilizacaoVoucher(parceiroMidiaID, dataInicial, dataFinal, localID, eventoID);
        }
    }
}
