using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using IRCore.Util.Enumerator;
using IRCore.BusinessObject.Enumerator;
using IRCore.Util;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.ADO.Enumerator;
using IRCore.DataAccess.Model.Enumerator;

namespace IRCore.BusinessObject
{
    public class EventoBO : MasterBO<EventoADO>
    {
        public EventoBO(int canalId = 2) : base(null, canalId) { }

        public EventoBO(MasterADOBase ado, int canalId = 2) : base(ado, canalId) { }

        public IPagedList<Evento> ListarVoucher(int pageNumber, int pageSize, Voucher voucher)
        {
            return ado.ListarParceiroNivel(pageNumber, pageSize, voucher.ParceiroMidiaClasse.Nivel, voucher.ParceiroMidiaID, voucher.Cidades);
        }

        public IPagedList<Evento> ListarParceiro(int pageNumber, int pageSize, ParceiroMidia parceiro)
        {
            return ado.ListarInApresentacao(pageNumber, pageSize, parceiro.ApresentacaoIDs);
        }

        public List<tEvento> ListarHistoricoParceiro(ParceiroMidia parceiro)
        {
            //return ado.ListarHistoricoInApresentacao(parceiro.ApresentacaoIDs);

            return ado.ListarHistoricoEventos(parceiro.ID);
        }

        public List<tEvento> ListarEventoDisponivel(ParceiroMidia parceiro)
        {
            return ado.ListarEventoDisponivel(parceiro.ID);
        }

        public List<Evento> ListarParceiro(ParceiroMidia parceiro)
        {
            return ado.ListarInApresentacao(parceiro.ApresentacaoIDs);
        }

        public Evento ConsultarVoucher(int eventoID, Voucher voucher)
        {

            ApresentacaoBO apresentacaoBO = new ApresentacaoBO(ado);
            var evento = ado.ConsultarMediaPartner(eventoID, voucher.ParceiroMidiaID, voucher.ParceiroMidiaClasse.Nivel);
            if (evento != null)
            {
                foreach (var item in evento.Apresentacao)
                {
                    item.MapaEsquematico = apresentacaoBO.ConsultarMapaEsquematico(item.IR_ApresentacaoID);
                }

            }
            return evento;
        }

        public List<tIngresso> ListarMapaSetor(int setorId, int apresentacaoId, int parceiroId = 0)
        {
            return ado.ListarMapaSetor(setorId, apresentacaoId, parceiroId);
        }

        public List<MapaAcentoModel> ListarMapaObject(int setorId, int apresentacaoId, int clienteId, string sessionId)
        {
            var mapaSetor = ado.ListarMapaSetorObject(setorId, apresentacaoId, clienteId, sessionId);
            foreach (var mapa in mapaSetor)
            {
                var caminhoPerspectivaLugar = string.Format("{0}pl{1}.jpg", ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoPerspectivaLugarImagem), Convert.ToInt32(mapa.pl).ToString("000000"));

                mapa.pl = !mapa.pl.Equals("0") ? caminhoPerspectivaLugar : string.Empty;
            }
            return mapaSetor;
        }

        public List<MapaAssentoMesaModel> ListarMapaMesaObject(int setorId, int apresentacaoId, string sessionId)
        {
            var mapaMesaSetor = ado.ListarMapaMesaSetorObject(setorId, apresentacaoId, sessionId);
            foreach (var mesa in mapaMesaSetor)
            {
                var caminhoPerspectivaLugar = string.Format("{0}pl{1}.jpg", ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoPerspectivaLugarImagem), Convert.ToInt32(mesa.pl).ToString("000000"));

                mesa.pl = !mesa.pl.Equals("0") ? caminhoPerspectivaLugar : string.Empty;
            }
            return (List<MapaAssentoMesaModel>)mapaMesaSetor;
        }

        public List<MapaAcentoModel> ListarMapaObjectOSESP(int setorId, int apresentacaoId)
        {
            return ado.ListarMapaSetorObjectOSESP(setorId, apresentacaoId);
        }

        public List<tIngresso> ListarMapaSetorSemJoin(int setorId, int apresentacaoId)
        {
            return ado.ListarMapaSetorSemJoin(setorId, apresentacaoId);
        }

        public List<tLugar> RetornaLugares(List<int> lugaresID)
        {
            return ado.RetornaLugares(lugaresID);
        }

        public Evento ConsultarAdmin(int idEvento)
        {
            Evento evt = ado.Consultar(idEvento);
            if (evt != null)
                evt.EventoMidias = ListarEventoMidiaInEventoAsDictionary(idEvento);
            return evt;
        }

        public Evento Consultar(int eventoId, bool isPOS = false, bool mostrarEstatistica = false)
        {
            var evt = ado.Consultar(eventoId, this.CanalId, isPOS);
            if (evt != null)
            {
                evt.EventoMidias = ListarEventoMidiaInEventoAsDictionary(eventoId);

                if (mostrarEstatistica)
                {
                    ((List<Apresentacao>)evt.Apresentacao).ForEach(a =>
                        a.Estatistica = ado.ConsultarEstatisticaApresentacao(eventoId, a.IR_ApresentacaoID)
                     );
                }
            }
            return evt;
        }

        public InfosObrigatoriasIngresso ListarInfosObrigatoriasIngresso(int idEvento)
        {
            return ado.ListarInfosObrigatoriasIngresso(idEvento);
        }

        public Evento ConsultarOSESP(int idEvento)
        {
            Evento evt = ado.ConsultarOSESP(idEvento);
            if (evt != null)
                evt.EventoMidias = ListarEventoMidiaInEventoAsDictionary(idEvento);
            return evt;
        }

        public IPagedList<Evento> ListarCompleto(int pageNumber, int pageSize, string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, enumTipoPeriodo? tipoPeriodo = null, enumTipoDias tipoDias = enumTipoDias.todosDias, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc, bool multiplasQueries = false)
        {

            int quantidadeDias = (tipoPeriodo == null) ? 0 : tipoPeriodo.Value.ValueAsInt();
            List<int> diasSemana = null;

            if (tipoDias == enumTipoDias.diasUteis)
            {
                diasSemana = ConfiguracaoAppUtil.GetAsListInt(enumConfiguracaoBO.eventoDiasUteis);
            }
            else if (tipoDias == enumTipoDias.finaisDeSemana)
            {
                diasSemana = ConfiguracaoAppUtil.GetAsListInt(enumConfiguracaoBO.eventoFinaisDeSemana);
            }
            IPagedList<Evento> eventos = null;
            if (multiplasQueries)
            {
                var apresentacaoBO = new ApresentacaoBO();
                var precoBO = new PrecoBO();

                eventos = ado.Listar(pageNumber, pageSize, busca, localID, estado, cidade, tipoID, subtipoID, quantidadeDias, diasSemana, latitude, longitude, distancia, ordem);
                foreach (var item in eventos)
                {
                    item.EventoMidias = ListarEventoMidiaInEventoAsDictionary(item.IR_EventoID);
                    item.Apresentacao = apresentacaoBO.Listar(item.IR_EventoID);
                    foreach (var itemAp in item.Apresentacao)
                    {
                        itemAp.MenorPreco = precoBO.ConsultarMaiorMenorPorApresentacao(itemAp.IR_ApresentacaoID);
                        if (itemAp.MenorPreco != null)
                        {
                            itemAp.MaiorPreco = precoBO.ConsultarMaiorMenorPorApresentacao(itemAp.IR_ApresentacaoID, false);
                            if (item.MaiorPreco == null)
                            {
                                item.MaiorPreco = itemAp.MaiorPreco;
                                item.MenorPreco = itemAp.MenorPreco;
                            }
                            else
                            {
                                if (itemAp.MaiorPreco.Valor > item.MaiorPreco.Valor)
                                {
                                    item.MaiorPreco = itemAp.MaiorPreco;
                                }
                                if (itemAp.MenorPreco.Valor < item.MenorPreco.Valor)
                                {
                                    item.MenorPreco = itemAp.MenorPreco;
                                }
                            }

                        }
                        if (item.PrimeiraApresentacao == null)
                        {
                            item.PrimeiraApresentacao = itemAp;
                        }
                        else if (itemAp.CalcHorario < item.PrimeiraApresentacao.CalcHorario)
                        {
                            if (item.UltimaApresentacao == null)
                            {
                                item.UltimaApresentacao = item.PrimeiraApresentacao;
                            }
                            item.PrimeiraApresentacao = itemAp;
                        }
                        else if ((item.UltimaApresentacao == null) || (itemAp.CalcHorario > item.UltimaApresentacao.CalcHorario))
                        {
                            item.UltimaApresentacao = itemAp;
                        }
                        item.QtdeDisponivel += itemAp.QtdeDisponivel;
                    }

                }
            }
            else
            {
                eventos = ado.ListarCompleto(pageNumber, pageSize, busca, localID, estado, cidade, tipoID, subtipoID, quantidadeDias, diasSemana, latitude, longitude, distancia, ordem);
                foreach (var item in eventos)
                {
                    item.EventoMidias = ListarEventoMidiaInEventoAsDictionary(item.IR_EventoID);
                }
            }
            return eventos;
        }

        public IPagedList<Evento> Listar(int pageNumber, int pageSize, string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, enumTipoPeriodo? tipoPeriodo = null, enumTipoDias tipoDias = enumTipoDias.todosDias, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc)
        {

            var quantidadeDias = (tipoPeriodo == null) ? 0 : tipoPeriodo.Value.ValueAsInt();
            List<int> diasSemana = null;

            switch (tipoDias)
            {
                case enumTipoDias.diasUteis:
                    diasSemana = ConfiguracaoAppUtil.GetAsListInt(enumConfiguracaoBO.eventoDiasUteis);
                    break;
                case enumTipoDias.finaisDeSemana:
                    diasSemana = ConfiguracaoAppUtil.GetAsListInt(enumConfiguracaoBO.eventoFinaisDeSemana);
                    break;
            }

            var eventos = ado.Listar(pageNumber, pageSize, busca, localID, estado, cidade, tipoID, subtipoID, quantidadeDias, diasSemana, latitude, longitude, distancia, ordem);

            //Se não encontrar nenhum evento realizar nova busca. Corrigindo o texto de busca
            if (eventos.None() && ConfigurationManager.AppSettings["HabilitarSpelling"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitarSpelling"]))
            {
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    busca = busca.CorrigirTexto();
                    eventos = ado.Listar(pageNumber, pageSize, busca, localID, estado, cidade, tipoID, subtipoID, quantidadeDias, diasSemana, latitude, longitude, distancia, ordem);
                }
            }

            foreach (var item in eventos)
            {
                item.EventoMidias = ListarEventoMidiaInEventoAsDictionary(item.IR_EventoID);
            }
            return eventos;
        }

        public List<Evento> ListarOSESP()
        {
            List<Evento> eventos = ado.ListarOSESP();
            foreach (var item in eventos)
            {
                item.EventoMidias = ListarEventoMidiaInEventoAsDictionary(item.IR_EventoID);
            }
            return eventos;
        }

        /// <summary>
        /// Método que lista os eventos de um calendário
        /// </summary>
        /// <param name="dataInical">Data inicial do calendário</param>
        /// <param name="numeroDias">Número de dias apresentados</param>
        /// <returns></returns>
        public List<KeyValuePair<DateTime, int>> ListarDatas(DateTime dataInicial, int numeroDias)
        {
            return ado.ListarDatas(dataInicial, numeroDias);
        }

        /// <summary>
        /// Método que retorna os Eventos de uma determinada data
        /// </summary>
        /// <param name="data">Data usada na busca</param>
        /// <param name="pagina">Numero da página que está sendo carregada</param>
        /// <param name="numeroItens">Número de itens a serem carregados</param>
        /// <returns></returns>
        public IPagedList<Evento> Listar(DateTime data, int pagina, int numeroItens)
        {
            return ado.Listar(data, pagina, numeroItens);
        }

        /// <summary>
        /// Método que retorna os Eventos de uma determinada data
        /// </summary>
        /// <param name="data">Data usada na busca</param>
        /// <param name="pagina">Numero da página que está sendo carregada</param>
        /// <param name="numeroItens">Número de itens a serem carregados</param>
        /// <returns></returns>
        public List<Evento> ListarTodos()
        {
            return ado.ListarTodos();
        }

        public List<Banner> ListarTodosBanners()
        {
            return ado.ListarTodosBanners();
        }

        #region Métodos Tabela tEventos usada no admin

        public IPagedList<tEvento> ListarAdmin(int pageNumber, int pageSize, string busca, enumAdminFiltroEvento filtro)
        {
            return ado.ListarAdmin(pageNumber, pageSize, busca, filtro);
        }

        public IPagedList<EventoCancelarMassaModelQuery> ListarEventosCancelar(string busca, int pagina, int itens, int diasLimiteCancelamento)
        {
            return ado.ListarEventosCancelar(busca, pagina, itens, diasLimiteCancelamento);
        }

        public tEvento ConsultartEvento(int tEventoId)
        {
            var evento = ado.ConsultarAdmin(tEventoId);
            return evento;
        }

        public void Salvar(tEvento evento, int usuarioLogadoId)
        {
            ado.Salvar(evento, usuarioLogadoId);
        }

        #endregion

        public Dictionary<enumEventoTipoMidiaTipo, Dictionary<string, List<string>>> ListarEventoMidiaInEventoAsDictionary(int eventoID)
        {
            return ado.ListarEventoMidiaInEvento(eventoID).ToDictionary();
        }

        public List<Evento> ListarDestaques(string tipo, int regiaoId, int canalId)
        {
            var eventos = ado.ListarDestaques(tipo, regiaoId, canalId);
            foreach (var item in eventos)
            {
                item.EventoMidias = ListarEventoMidiaInEventoAsDictionary(item.IR_EventoID);
            }
            return eventos;
        }

        public List<DefinirEventoApresentacaoModelQuery> DefinirEventoApresentacao(int eventoID)
        {
            return ado.DefinirEventoApresentacao(eventoID);
        }

        public List<DefinirEventoApresentacaoModelQuery> ListarDisponiveisCancelMassa(int eventoID, int diasLimiteCancelamento)
        {
            return ado.ListarDisponiveisCancelMassa(eventoID, diasLimiteCancelamento);
        }

        public List<CancelamentoModeloMotivo> DefinirEventoMotivoCancelamento()
        {
            return ado.DefinirEventoMotivoCancelamento();
        }

        public IPagedList<CancelamentoMassaModelQuery> ListarEventosCancelados(string busca, int pagina, int itens)
        {
            return ado.ListarEventosCancelados(busca, pagina, itens);
        }

        public CancelamentoDetalheModelQuery ConsultarCancelamento(int cancelamentoID)
        {
            return ado.ConsultarCancelamento(cancelamentoID);
        }

        public void ConsultarDadosRelatorioCancelamento(List<int> apresentacoesID = null, string codigoCancelamento = null, int cancelamentoID = 0)
        {
            CancelamentoRelatorioDadosBasicos dadosBasicos = null;
            if (apresentacoesID != null)
            {
                dadosBasicos = ado.ConsultarCancelamentoRelatorioDadosBasicos(apresentacoesID);
            }
            else
            {
                dadosBasicos = ado.ConsultarCancelamentoRelatorioDadosBasicos(codigoCancelamento, cancelamentoID);
            }
        }

        public InfoLeiMeia ConsultarInfoMeiaEntrada(int eventoId, int canalId)
        {
            return ado.ConsultarInfoMeiaEntrada(eventoId, canalId);
        }
    }
}
