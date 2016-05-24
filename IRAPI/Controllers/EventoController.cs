using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO.Enumerator;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Evento = IRCore.DataAccess.Model.Evento;
using FormaPagamento = IRCore.DataAccess.Model.FormaPagamento;
using Setor = IRCore.DataAccess.Model.Setor;

namespace IRAPI.Controllers
{
    /// <summary>
    /// Controller de eventos
    /// </summary>
    [IRAPIAuthorize(enumAPIRele.venda, enumAPIRele.evento)]
    public class EventoController : MasterApiController
    {
        /// <summary>
        /// Método da API que busca eventos com base nos filtros e retorna com base no numero da pagina e eventos por página. 
        /// URL de acesso: eventos
        /// Método de acesso: GET
        /// </summary>
        /// <param name="busca">Texto a ser buscado</param>
        /// <param name="localid">Id do local do evento</param>
        /// <param name="uf">Estado do evento</param>
        /// <param name="cidade">Cidade do evento</param>
        /// <param name="tipoID">Id da categoria do evento</param>
        /// <param name="subtipoID">Id da subcategoria do evento</param>
        /// <param name="latitude">Latitude do usuário</param>
        /// <param name="longitude">Longitude do usuário</param>
        /// <param name="distancia">Distância entre o usuário e o local do evento</param>
        /// <param name="pg">Número da página a ser exibida</param>
        /// <param name="qtdpg">Quantidade de ítens por página</param>
        /// <param name="ordem">Ordenação</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("eventos_completo")]
        [HttpGet]
        public RetornoModel<PagedListModel<Evento>> GetCompleto(string busca = null, int localid = 0, string uf = null, string cidade = null, int tipoID = 0, int subtipoID = 0, double? latitude = null, double? longitude = null, double distancia = 0, int pg = 1, int qtdpg = 1, enumEventoOrdem ordem = enumEventoOrdem.dataAsc)
        {
            RetornoModel<PagedListModel<Evento>> retorno = new RetornoModel<PagedListModel<Evento>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                using (var eventoBO = new EventoBO())
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca = busca.Replace("'", "''");

                    var result = eventoBO.ListarCompleto(pg, qtdpg, busca, localid, uf, cidade, tipoID, subtipoID, null, enumTipoDias.todosDias, latitude, longitude, distancia, ordem);
                    if (result.Count > 0)
                    {
                        retorno.Retorno = new PagedListModel<Evento>().CopyFrom(result);
                        retorno.Retorno.Itens = result.ToList();
                    }
                    retorno.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                retorno.Mensagem = ex.Message;
                LogUtil.Error(string.Format("##GET.Eventos.eventos_completo.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            stopwatch.Stop();

            if (retorno.Retorno == null || retorno.Retorno.Itens == null || retorno.Retorno.Itens.Count == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum evento encontrado";
                LogUtil.Info(string.Format("##GET.Eventos.eventos_completo## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
            }
            LogUtil.Info(string.Format("##GET.Eventos.eventos_completo## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que busca eventos com base nos filtros e retorna com base no numero da pagina e eventos por página. 
        /// URL de acesso: eventos
        /// Método de acesso: GET
        /// </summary>
        /// <param name="busca">String a ser buscada</param>
        /// <param name="localid">ID do local do evento</param>
        /// <param name="uf">Estado do evento</param>
        /// <param name="cidade">Cidade do evento</param>
        /// <param name="tipoID"></param>
        /// <param name="subtipoID">ID do subtipo de evento</param>
        /// <param name="distancia"></param>
        /// <param name="pg">Número da página</param>
        /// <param name="qtdpg">Quantidade eventos por página</param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="ordem"></param>
        [Route("eventos")]
        [HttpGet]
        public RetornoModel<PagedListModel<Evento>> Get(string busca = null, int localid = 0, string uf = null, string cidade = null, int tipoID = 0, int subtipoID = 0, double? latitude = null, double? longitude = null, double distancia = 0, int pg = 1, int qtdpg = 1, enumEventoOrdem ordem = enumEventoOrdem.dataAsc)
        {
            var retorno = new RetornoModel<PagedListModel<Evento>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                using (var eventoBO = new EventoBO())
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca = busca.Replace("'", "''");

                    var result = eventoBO.Listar(pg, qtdpg, busca, localid, uf, cidade, tipoID, subtipoID, null, enumTipoDias.todosDias, latitude, longitude, distancia, ordem);

                    if (result.Count > 0)
                    {
                        retorno.Retorno = new PagedListModel<Evento>().CopyFrom(result);
                        retorno.Retorno.Itens = result.ToList();
                    }
                    retorno.Sucesso = true;
                }
            }

            catch (Exception ex)
            {
                stopwatch.Stop();
                retorno.Mensagem = ex.Message;
                LogUtil.Error(string.Format("##GET.Eventos.eventos.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            stopwatch.Stop();

            if (retorno.Retorno == null || retorno.Retorno.Itens == null || retorno.Retorno.Itens.Count == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum evento encontrado";
                LogUtil.Info(string.Format("##GET.Eventos.eventos## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
            }

            LogUtil.Info(string.Format("##GET.Eventos.eventos## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e um evento com base no ID informado.
        /// URL de acesso: eventos/{ID}
        /// Método de acesso: GET
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <param name="mostrarEstatistica"></param>
        [Route("eventos/{id}")]
        [HttpGet]
        public RetornoModel<Evento> GetEvento(int id, int mostrarEstatistica = 0)
        {
            var retorno = new RetornoModel<Evento>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                using (var ado = new MasterADOBase())
                {
                    var eventoBO = new EventoBO(ado);

                    var canalTipoPOS = ConfiguracaoAppUtil.GetAsInt("CanalTipoPOS");
                    var isPOS = new CanalBO(ado).isPOS(SessionModel.CanalID, canalTipoPOS);

                    retorno.Retorno = eventoBO.Consultar(id, isPOS, Convert.ToBoolean(mostrarEstatistica));

                    var pacoteBO = new PacoteBO(ado);

                    if (retorno.Retorno != null)
                    {
                        retorno.Retorno.Pacote = pacoteBO.ListarPorEvento(id, SessionModel.CanalID);

                        if (retorno.Retorno.Pacote.Any())
                        {
                            foreach (var pacote in retorno.Retorno.Pacote)
                            {
                                pacote.PacoteItem = pacoteBO.ListarItens(pacote.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                retorno.Mensagem = ex.Message;
                LogUtil.Error(string.Format("##GET.Eventos.eventos/id.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            stopwatch.Stop();

            retorno.Sucesso = (retorno.Retorno != null);

            if (!retorno.Sucesso)
            {
                retorno.Mensagem = "Evento não encontrado";
                LogUtil.Info(string.Format("##GET.Eventos.eventos/id## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            retorno.Mensagem = "OK";

            LogUtil.Info(string.Format("##GET.Eventos.eventos/id## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Esse método retorna os destaques Nível 2, o parametro tipo é obrigatório e o região é opcional, 
        /// caso não for passado deve retornar eventos da região que é marcada como Geral. Esse método também 
        /// tem que retornar o maior e menor preço.
        /// RL de acesso: eventos/destaques?tipo={tipo}&amp;regiao={regiao}
        /// Método de acesso: GET
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="regiao"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("eventos/destaques")]
        [HttpGet]
        public RetornoModel<List<Evento>> GetDestaques(string tipo, int regiao = 1)
        {
            var retorno = new RetornoModel<List<Evento>>();
            List<Evento> eventos;

            try
            {
                using (var eventoBO = new EventoBO())
                {
                    eventos = eventoBO.ListarDestaques(tipo, regiao, SessionModel.CanalID);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Retorno = eventos;
            retorno.Sucesso = true;
            if (eventos == null || eventos.Count == 0)
            {
                retorno.Mensagem = "Destaques não encontrados";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }

            retorno.Mensagem = "OK";
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e uma lista de pactotes com base no ID do evento.
        /// URL de acesso: eventos/{ID}/pacotes
        /// Método de acesso: GET
        /// </summary>
        /// <param name="id">ID do evento</param>
        [Route("eventos/{id}/pacotes")]
        [HttpGet]
        public RetornoModel<List<PacoteRetorno>> GetPacotesEvento(int id)
        {
            var retorno = new RetornoModel<List<PacoteRetorno>>();
            List<PacoteRetorno> pacotes;

            try
            {
                using (var ado = new MasterADOBase())
                {
                    var pacoteBO = new PacoteBO(ado);
                    pacotes = pacoteBO.ListarPorEvento(id, SessionModel.CanalID);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            retorno.Retorno = pacotes;
            retorno.Sucesso = true;

            if (pacotes == null || pacotes.Count == 0)
            {
                retorno.Mensagem = "Evento ou pacotes não encontrados";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }

            retorno.Mensagem = "OK";

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e uma lista de setores com base no ID do evento e da apresentação.
        /// URL de acesso: eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores
        /// Método de acesso: GET
        /// </summary>
        /// <param name="eventoID">ID do evento</param>
        /// <param name="apresentacaoID">ID da apresentação</param>
        /// <param name="comCotaNominal"></param>
        /// <param name="comCotaPromocional"></param>
        [Route("eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores/")]
        [HttpGet]
        public RetornoModel<List<Setor>> GetSetores(int eventoID, int apresentacaoID, [FromUri]bool comCotaNominal = true, [FromUri]bool comCotaPromocional = true, int mostrarEstatistica = 0)
        {
            var retorno = new RetornoModel<List<Setor>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Setor> resultado;
            try
            {
                using (var setorBO = new SetorBO())
                {
                    var ado = new MasterADOBase();

                    var canalTipoPOS = ConfiguracaoAppUtil.GetAsInt("CanalTipoPOS");
                    var isPOS = new CanalBO(ado).isPOS(SessionModel.CanalID, canalTipoPOS);

                    resultado = setorBO.Listar(apresentacaoID, eventoID, SessionModel.CanalID, comCotaNominal, comCotaPromocional, isPOS, SessionModel.SessionID, Convert.ToBoolean(mostrarEstatistica));
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                retorno.Mensagem = ex.Message;
                LogUtil.Error(string.Format("##GET.Eventos/id/apresentacao/id/setores.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            retorno.Retorno = resultado;
            retorno.Sucesso = true;

            stopwatch.Stop();

            if (resultado == null || resultado.Count == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum setor encontrado par a apresentação ou apresentação não encontrada";
                LogUtil.Info(string.Format("##GET.Eventos/id/apresentacao/id/setores## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }

            retorno.Mensagem = "OK";

            LogUtil.Info(string.Format("##GET.Eventos/id/apresentacao/id/setores## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores/{setorID}/mapa_lugares")]
        [HttpGet]
        public RetornoModel<GetMapaAssentosRequestModel> GetMapaAssentos(int eventoID, int apresentacaoID, int setorID)
        {
            RetornoModel<GetMapaAssentosRequestModel> retorno = new RetornoModel<GetMapaAssentosRequestModel>();
            GetMapaAssentosRequestModel mapaAssentos = new GetMapaAssentosRequestModel();
            List<MapaAcentoModel> ingressos = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    EventoBO eventoBO = new EventoBO(ado);
                    ingressos = eventoBO.ListarMapaObject(setorID, apresentacaoID, SessionModel.ClienteID, SessionModel.SessionID);
                    mapaAssentos.mp = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoSetorFundo) + "s" + setorID.ToString("000000") + ".gif";
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Sucesso = true;
            if (ingressos.Count > 0)
            {
                mapaAssentos.lugares = ingressos;
                retorno.Retorno = mapaAssentos;
                retorno.Mensagem = "OK";
            }
            else
            {
                retorno.Mensagem = "Não Encontrado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores/{setorID}/mapa_lugares_mesa")]
        [HttpGet]
        public RetornoModel<GetMapaAssentosRequestModel> GetMapaAssentosMesa(int eventoID, int apresentacaoID, int setorID)
        {
            RetornoModel<GetMapaAssentosRequestModel> retorno = new RetornoModel<GetMapaAssentosRequestModel>();
            GetMapaAssentosRequestModel mapaAssentos = new GetMapaAssentosRequestModel();
            List<MapaAssentoMesaModel> ingressos = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    EventoBO eventoBO = new EventoBO(ado);
                    ingressos = eventoBO.ListarMapaMesaObject(setorID, apresentacaoID, SessionModel.SessionID);
                    mapaAssentos.mp = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoSetorFundo) + "s" + setorID.ToString("000000") + ".gif";
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Sucesso = true;
            if (ingressos.Count > 0)
            {
                mapaAssentos.lugares = ingressos;
                retorno.Retorno = mapaAssentos;
                retorno.Mensagem = "OK";
            }
            else
            {
                retorno.Mensagem = "Não Encontrado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e um mapa esquemático com base no ID do evento e da apresentação.
        /// URL de acesso: eventos/{eventoID}/apresentacoes/{apresentacaoID}/mapa_esquematico
        /// Método de acesso: GET
        /// </summary>
        /// <param name="eventoID">ID do evento</param>
        /// <param name="apresentacaoID">ID da apresentação</param>
        [Route("eventos/{eventoID:int}/apresentacoes/{apresentacaoID:int}/mapa_esquematico")]
        [HttpGet]
        public RetornoModel<GetMapaEsquematicoResponseModel> GetMapaEsquematico(int eventoID, int apresentacaoID)
        {
            RetornoModel<GetMapaEsquematicoResponseModel> retorno = new RetornoModel<GetMapaEsquematicoResponseModel>();
            tMapaEsquematico mapa = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    ApresentacaoBO aprbo = new ApresentacaoBO(ado);
                    mapa = aprbo.ConsultarMapaEsquematico(apresentacaoID);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (mapa != null)
            {
                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
                retorno.Retorno = new GetMapaEsquematicoResponseModel();
                retorno.Retorno.urlMapa = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoMapaEsquematico) + "me" + mapa.ID.ToString("D6") + ".gif";
                retorno.Retorno.mapaSetor = mapa.tMapaEsquematicoSetor.Select(x => new GetMapaEsquematicoSetorModel() { setorID = x.SetorID, coordenadas = x.Coordenadas }).ToList();
            }
            else
            {
                retorno.Mensagem = "Apresentação não possui mapa esquemático";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Retorna as formas de pagamento do evento.
        /// </summary>
        /// <param name="eventoID"></param>
        /// <returns></returns>
        [Route("eventos/{eventoID}/formas_pagamento")]
        [HttpGet]
        public RetornoModel<List<FormaPagamento>> GetFormasPagamento(int eventoID)
        {
            RetornoModel<List<FormaPagamento>> retorno = new RetornoModel<List<FormaPagamento>>();
            try
            {
                using (var ado = new MasterADOBase())
                {
                    FormaPagamentoBO fpbo = new FormaPagamentoBO(ado);
                    List<FormaPagamento> resultado = fpbo.ListarEvento(eventoID);
                    retorno.Retorno = resultado;
                    if (resultado == null || resultado.Count == 0)
                    {
                        retorno.Sucesso = false;
                        retorno.Mensagem = "Nenhuma forma de pagamento disponivel para esse evento";
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                    }
                    else
                    {
                        retorno.Sucesso = true;
                        retorno.Mensagem = "OK";
                        return ParseRetorno(retorno);
                    }
                }

            }
            catch (Exception ex)
            {
                retorno.Retorno = null;
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
        }

        /// <summary>
        /// Retorna as informações do evento sobre a leia de meia entrada.
        /// </summary>
        /// <param name="canalId"></param>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        [Route("eventos/info_meia_entrada/{canalId}/{eventoId}/")]
        [HttpGet]
        [AllowAnonymous]
        public RetornoModel<InfoLeiMeia> GetInfoLeiMeia(int canalId, int eventoId)
        {
            RetornoModel<InfoLeiMeia> retorno;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var eventoBO = new EventoBO(ado);
                    var info = eventoBO.ConsultarInfoMeiaEntrada(eventoId, canalId);

                    if (info != null)
                    {
                        retorno = new RetornoModel<InfoLeiMeia>()
                        {
                            Retorno = info,
                            Sucesso = true,
                            Mensagem = "OK"
                        };

                        return ParseRetorno(retorno);
                    }

                    retorno = new RetornoModel<InfoLeiMeia>()
                    {
                        Retorno = null,
                        Sucesso = false,
                        Mensagem = "Não encontrado"
                    };

                    return ParseRetorno(retorno);
                }
            }
            catch (Exception ex)
            {
                retorno = new RetornoModel<InfoLeiMeia>()
                {
                    Retorno = null,
                    Sucesso = false,
                    Mensagem = ex.Message
                };

                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
        }

        /// <summary>
        /// Método da API que retorna o google markup
        /// URL de acesso: eventos/google_markup/{ID}
        /// Método de acesso: GET
        /// </summary>
        /// <param name="id">Id do evento</param>
        [Route("eventos/google_markup/{id}")]
        [HttpGet]
        public RetornoModel<List<GoogleMarkup>> GetGoogleMarkup(int id)
        {
            var retorno = new RetornoModel<List<GoogleMarkup>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var googleBO = new GoogleBO();
                retorno.Retorno = googleBO.GetMarkups(id);
                retorno.Sucesso = (retorno.Retorno != null);
                retorno.Mensagem = "OK";
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Eventos.GoogleMarkup.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);
                stopwatch.Stop();

                retorno.Sucesso = false;
                retorno.Mensagem = ex.Message;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                LogUtil.Debug(string.Format("##Eventos.GoogleMarkup.ERRO## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));

                retorno.Mensagem = "Evento não encontrado. Markup não gerada.";
                retorno.Sucesso = false;

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }

            LogUtil.Debug(string.Format("##Eventos.GoogleMarkup.SUCCESS## SESSION {0}, TEMPO DECORRIDO {1}, MSG {2}", this.SessionModel.SessionID, stopwatch.Elapsed, retorno.Mensagem));

            stopwatch.Stop();
            return ParseRetorno(retorno);
        }
    }
}