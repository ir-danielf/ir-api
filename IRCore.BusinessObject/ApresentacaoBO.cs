using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class ApresentacaoBO : MasterBO<ApresentacaoADO>
    {
        public ApresentacaoBO(MasterADOBase ado, int canalId = 2) : base(ado, canalId) { }
        public ApresentacaoBO(int canalId = 2) : base(null, canalId) { }

        public List<Apresentacao> ListarParceiro(ParceiroMidia parceiro, int eventoID)
        {
            return ado.ListarIn(parceiro.ApresentacaoIDs, eventoID);
        }

        public Apresentacao Consultar(int apresentacaoId)
        {
            return ado.Consultar(apresentacaoId);
        }

        public InfosObrigatoriasIngresso ListarInfosObrigatoriasIngresso(int eventoId)
        {
            return ado.ListarInfosObrigatoriasIngresso(eventoId);
        }

        public tMapaEsquematico ConsultarMapaEsquematico(int apresentacaoId)
        {
            return ado.ConsultarMapaEsquematico(apresentacaoId);
        }

        public Apresentacao ConsultarPorEvento(int eventoID, bool ordemAsc = true)
        {
            return ado.ConsultarPorEvento(eventoID, ordemAsc);
        }

        public List<Apresentacao> Listar(int eventoID)
        {
            return ado.Listar(eventoID);
        }

        public List<InformacaoVendaBasicasCancelarMassa> CarregarInformacoesVenda(List<int> apresentacoesID)
        {
            return ado.CarregarInformacoesVenda(apresentacoesID);
        }

        public List<InformacaoVendaFormasPagamento> CarregarInformacoesVendaFormasPagamento(List<int> apresentacoesID)
        {
            return ado.CarregarInformacoesVendaFormasPagamento(apresentacoesID);
        }

        public List<InformacaoValoresOperacao> CarregarInformacoesVendaPorOperacao(List<int> apresentacoesID)
        {
            return ado.CarregarInformacoesVendaPorOperacao(apresentacoesID);
        }

        public List<CancelamentoRelatorioDadosOperacoes> CarregarInformacoesVendaPorOperacao(int cancelamentoID, string codigoCancelamento = null)
        {
            return ado.CarregarInformacoesVendaPorOperacao(cancelamentoID, codigoCancelamento);
        }

        public void RemoverApresentacoesCanceladasSite(List<int> apresentacoesID)
        {
            ado.RemoverApresentacoesCanceladasSite(apresentacoesID);
        }

        public List<DateTime> ApresentacoesCanceladas(int cancelamentoID)
        {
            return ado.ApresentacoesCanceladas(cancelamentoID);
        }

        public List<ClienteCancelamentoOperacao> ClienteCancelamentoManual(int cancelamentoID)
        {
            return ado.ClienteCancelamentoManual(cancelamentoID);
        }

        public List<DateTime> ConsultarDataApresentacoes(List<int> apresentacoesID)
        {
            return ado.ConsultarDataApresentacoes(apresentacoesID);
        }
        public List<ConferenciaCancelamento> ConferenciaCancelamentoLote(int cancelamentoID)
        {
            return ado.ConferenciaCancelamentoLote(cancelamentoID);
        }
    }
}
