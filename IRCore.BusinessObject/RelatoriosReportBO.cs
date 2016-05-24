using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class RelatoriosReportBO : MasterBO<RelatoriosReportADO>
    {
        public RelatoriosReportBO(MasterADOBase ado) : base(ado) { }

        public List<ReportIngressosOffline_Result> IngressosOffline(int eventoID, int apresentacaoID, string statusIngresso)
        {
            return ado.IngressosOffline(eventoID, apresentacaoID, statusIngresso);
        }

        public List<ReportWillCall_Result> WillCall(string eventosID, string status, DateTime? inicial, DateTime? final, int localID)
        {
            return ado.WillCall(eventosID, status, inicial, final, localID);
        }

        public List<ReportListaNegra_Result> ListaNegra(int eventoID)
        {
            return ado.ListaNegra(eventoID);
        }

        public List<ReportRaioX_Result> RaioX(int eventoID, int? apresentacaoID)
        {
            return ado.RaioX(eventoID, apresentacaoID);
        }

        public List<ReportCompradores_Result> Compradores(int eventoID, int? apresentacaoID, string statusIngresso, int? entregaControleID, string statusCompra)
        {
            return ado.Compradores(eventoID, apresentacaoID, statusIngresso, entregaControleID, statusCompra);
        }

        public List<ReportPrevisaoPDV_Result> PrevisaoPDV(string PDV, string estado, string cidade, List<int> eventosIDs)
        {
            return ado.PrevisaoPDV(PDV, estado, cidade, eventosIDs);
        }

        public List<ListaEventosPorNome_Result> ListarEventoPorNome(string evento)
        {
            return ado.ListarEventoPorNome(evento);
        }

        public List<ListaLocalPorNome_Result> ListarLocalPorNome(string local, List<int> eventosID)
        {
            return ado.ListarLocalPorNome(local, eventosID);
        }

        public List<int> ListarLocaisID(List<int> eventosID)
        {
            return ado.ListarLocaisID(eventosID);
        }

        public List<tApresentacao> ListarApresentacaoPorEvento(int evento)
        {
            return ado.ListarApresentacaoPorEvento(evento);
        }

        public List<MeioEntregaVM> ListarMeioEntrega(int evento)
        {
            return ado.ListarMeioEntrega(evento);
        }

        public DateTime ListarPrimeiraApresentacao(List<int> eventosID, int localID)
        {
            return ado.ListarPrimeiraApresentacao(eventosID, localID);
        }
        public List<EstadosVM> ListarEstados()
        {
            return ado.ListarEstados();
        }
        public List<CidadesVM> ListarCidades(string estadoSigla)
        {
            return ado.ListarCidades(estadoSigla);
        }

        public List<RelatorioClientesCotaNet> ListarReportClientesCotaNet(string DataFiltro)
        {
            return ado.ListarReportClientesCotaNet(DataFiltro);
        }
    }
}