using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using IRAPI.Common.Exception;
using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using Evento = IRCore.DataAccess.Model.Evento;
using FormaPagamento = IRCore.DataAccess.Model.FormaPagamento;
using PontoVenda = IRCore.DataAccess.Model.PontoVenda;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace IRAPI.Controllers
{
    /// <summary>
    /// API responsável por realizar as reservas.
    /// </summary>
    [IRAPIAuthorize(enumAPIRele.venda)]
    public class ReservaController : MasterApiController
    {
        /// <summary>
        /// Método da API que remove limpa toda a reserva
        /// URL de acesso: reservas
        /// Método de acesso: DELETE
        /// </summary>
        /// <returns></returns>
        [Route("reservas/atual")]
        [HttpDelete]
        public RetornoModel Delete()
        {
            LogUtil.Info(string.Format("##Delete.Reservas## SESSION {0}", this.SessionModel.SessionID));

            using (var bo = new CarrinhoBO())
            {
                return ParseRetorno(LimparCompra(bo));
            }
        }

        /// <summary>
        /// Método da API que remove um evento do rq.
        /// URL de acesso: rq/ingressos_por_evento/{eventoID}
        /// Método de acesso: DELETE
        /// </summary>
        /// <param name="eventoID"></param>
        /// <returns></returns>
        [Route("reservas/atual/eventos/{eventoID}")]
        [HttpDelete]
        public RetornoModel<CompraModel> DeleteRemoveEvento(int eventoID)
        {
            LogUtil.Info(string.Format("##Delete.Reservas.Evento## SESSION {0}, EVENTO {1}", this.SessionModel.SessionID, eventoID));

            RetornoModel<CompraModel> retorno = null;

            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);
                    retorno = bo.carrinhoBO.RemoverReservaEvento(SessionModel.Compra, eventoID);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Delete.Reservas.Evento.EXCEPTION## SESSION {0}, EVENTO {1}, MSG {2}", this.SessionModel.SessionID, eventoID, ex.Message), ex);

                retorno = new RetornoModel<CompraModel>();
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            if (!retorno.Sucesso)
            {
                LogUtil.Info(string.Format("##Delete.Reservas.Evento.ERROR## SESSION {0}, EVENTO {1}", this.SessionModel.SessionID, eventoID));

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Delete.Reservas.Evento.SUCCESS## SESSION {0}, EVENTO {1}", this.SessionModel.SessionID, eventoID));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que remove um ingresso ao rq.
        /// URL de acesso: reservas/atual/ingressos/{ingressoId}
        /// Método de acesso: DELETE
        /// </summary>
        /// <param name="ingressoId"></param>
        /// <returns></returns>
        [Route("reservas/atual/ingressos/{ingressoID}")]
        [HttpDelete]
        public RetornoModel<CompraModel> DeleteRemoveIngressorq(int ingressoId)
        {
            LogUtil.Info(string.Format("##Delete.Reservas.Ingresso## SESSION {0}, INGRESSO {1}", this.SessionModel.SessionID, ingressoId));

            RetornoModel<CompraModel> retorno = null;

            using (var ado = new MasterADOBase())
            {
                try
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);
                    if (SessionModel.Compra != null)
                    {
                        Carrinho carrinho = SessionModel.Compra.CarrinhoItens.Where(c => c.IngressoID == ingressoId).FirstOrDefault();
                        if (carrinho != null)
                            retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                        else
                        {
                            retorno = new RetornoModel<CompraModel>();
                            retorno.Mensagem = "Ingresso informado não está no carrinho";
                            retorno.Sucesso = false;
                        }
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Delete.Reservas.Ingresso.ERROR## SESSION {0}, INGRESSO {1}, MSG {2}", this.SessionModel.SessionID, ingressoId, "carrinho = null"));

                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = "Objeto compra não encontrado";
                        retorno.Sucesso = false;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Delete.Reservas.Ingresso.EXCEPTION## SESSION {0}, INGRESSO {1}, MSG {2}", this.SessionModel.SessionID, ingressoId, ex.Message), ex);

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    //LogUtil.Error(ex);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Delete.Reservas.Ingresso.ERROR## SESSION {0}, INGRESSO {1}", this.SessionModel.SessionID, ingressoId));

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }

            LogUtil.Info(string.Format("##Delete.Reservas.Ingresso.SUCCESS## SESSION {0}, INGRESSO {1}", this.SessionModel.SessionID, ingressoId));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Deleta uma mesa do SessionCompra
        /// </summary>
        /// <param name="lugarID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns></returns>
        [Route("reservas/atual/mesas/{lugarID}")]
        [HttpDelete]
        public RetornoModel<CompraModel> DeleteRemoveMesa(int lugarID)
        {
            LogUtil.Info(string.Format("##Delete.Reservas.Lugar## SESSION {0}, LUGAR {1}", this.SessionModel.SessionID, lugarID));

            RetornoModel<CompraModel> retorno = null;

            using (var ado = new MasterADOBase())
            {
                try
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);

                    if (SessionModel.Compra == null)
                    {
                        LogUtil.Info(string.Format("##Delete.Reservas.Lugar.ERROR## SESSION {0}, LUGAR {1}, MSG {2}", this.SessionModel.SessionID, lugarID, "Compra == null"));
                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = "Não existe compra cadastrada.";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    else
                    {
                        retorno = bo.carrinhoBO.RemoverReservaMesa(SessionModel.Compra, lugarID);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Delete.Reservas.Lugar.EXCEPTION## SESSION {0}, LUGAR {1}, MSG {2}", this.SessionModel.SessionID, lugarID, ex.Message), ex);
                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Delete.Reservas.Lugar.ERROR## SESSION {0}, LUGAR {1}", this.SessionModel.SessionID, lugarID));
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }

            LogUtil.Info(string.Format("##Delete.Reservas.Lugar.SUCCESS## SESSION {0}, LUGAR {1}", this.SessionModel.SessionID, lugarID));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que remove um pacote ao rq.
        /// URL de acesso: rq/reserva/pacotes/{pacoteID}/{pacoteGrupo}
        /// Método de acesso: DELETE
        /// </summary>
        /// <param name="pacoteID"></param>
        /// <param name="pacoteGrupo"></param>
        /// <returns></returns>
        [Route("reservas/atual/pacotes/{pacoteID}/{pacoteGrupo}")]
        [HttpDelete]
        public RetornoModel<CompraModel> DeleteRemovePacote(int pacoteID, string pacoteGrupo)
        {
            LogUtil.Info(string.Format("##Delete.Reservas.Pacote## SESSION {0}, PACOTE {1}", this.SessionModel.SessionID, pacoteID));

            RetornoModel<CompraModel> retorno = null;

            if (SessionModel.Compra == null)
            {
                LogUtil.Info(string.Format("##Delete.Reservas.Pacote.ERROR## SESSION {0}, PACOTE {1}, MSG '{2}'", this.SessionModel.SessionID, pacoteID, "SessionModel.Compra == null"));

                retorno = new RetornoModel<CompraModel>();
                retorno.Mensagem = "Não existe compra cadastrada.";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }
            else
            {
                try
                {
                    using (var ado = new MasterADOBase())
                    {
                        var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                        CarregarCompra(bo);
                        retorno = bo.carrinhoBO.RemoverReservaPacote(SessionModel.Compra, pacoteID, pacoteGrupo);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Delete.Reservas.Pacote.EXCEPTION## SESSION {0}, PACOTE {1}, MSG {2}", this.SessionModel.SessionID, pacoteID, ex.Message), ex);

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    //LogUtil.Error(ex);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Delete.Reservas.Pacote.ERROR## SESSION {0}, PACOTE {1}", this.SessionModel.SessionID, pacoteID));

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }

            LogUtil.Info(string.Format("##Delete.Reservas.Pacote.SUCCESS## SESSION {0}, PACOTE {1}", this.SessionModel.SessionID, pacoteID));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que remove um vale ingresso do rq.
        /// URL de acesso: rq/vale_ingresso
        /// Método de acesso: DELETE
        /// </summary>
        /// <param name="rq">rq Model onde o vale ingresso será adicionado</param>
        [Route("reservas/atual/vale_ingresso/{codigo}")]
        [HttpDelete]
        public RetornoModel<CompraModel> DeleteRemoveValeIngresso(string codigo)
        {
            LogUtil.Info(string.Format("##Delete.Reservas.ValeIngresso## SESSION {0}, CODIGO {1}", this.SessionModel.SessionID, codigo));

            RetornoModel<CompraModel> retorno = null;
            tValeIngresso valeIngresso = SessionModel.Compra.ValeIngressos.FirstOrDefault(t => t.CodigoTroca == codigo);
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);
                if (valeIngresso == null)
                {
                    LogUtil.Info(string.Format("##Delete.Reservas.ValeIngresso.ERRO## SESSION {0}, CODIGO {1}, MSG '{2}'", this.SessionModel.SessionID, codigo, "valeIngresso == null"));

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = "Parametro não infromado ou mal formatado!";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
                else
                {
                    if (SessionModel.Compra.EntregaControles == null || SessionModel.Compra.EntregaControles.Count == 0 || SessionModel.Compra.EntregaControleID == 0)
                    {
                        LogUtil.Info(string.Format("##Delete.Reservas.ValeIngresso.ERRO## SESSION {0}, CODIGO {1}, MSG '{2}'", this.SessionModel.SessionID, codigo, "EntregaControles == null OR EntregaControles.Count == 0 OR EntregaControleID == 0"));

                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = "Entregas invalidas para objeto compra";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    retorno = new RetornoModel<CompraModel>();
                    CompraModel compra = bo.valeIngressoBO.RemoverValeIngresso(SessionModel.Compra, valeIngresso);
                    retorno.Retorno = compra;

                    retorno.Mensagem = "OK";
                    retorno.Sucesso = true;

                    SessionModel.ValesIngressoID.Remove(valeIngresso.ID);
                }
            }

            LogUtil.Info(string.Format("##Delete.Reservas.ValeIngresso.SUCCESS## SESSION {0}, CODIGO {1}", this.SessionModel.SessionID, codigo));

            return ParseRetorno(retorno);
        }

        [Route("reservas/ingresso/codigobarras/{idIngresso:int}")]
        [HttpGet]
        public RetornoModel<String> GetBarCodeTicket(int idIngresso)
        {
            LogUtil.Info(string.Format("##Get.CodigoBarras## SESSION {0}, INGRESSO {1}", this.SessionModel.SessionID, idIngresso));

            //todo: atualizar documentacao api-helper
            //select CodigoBarraCliente from tIngresso where ID = 9880069   --id int, CodigoBarraCliente nvarchar(30)
            RetornoModel<String> retorno = new RetornoModel<String>();
            Random rnd = new Random();
            int iRnd1 = rnd.Next(1000000, 9999999);
            int iRnd2 = rnd.Next(1000000, 9999999);
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            retorno.Retorno = iRnd1.ToString() + iRnd2.ToString(); //concatenando 2 inteiros de 7 digitos para gerar 14 digitos randomicos

            LogUtil.Info(string.Format("##Get.CodigoBarras.SUCCESS## SESSION {0}, INGRESSO {1}, MSG {2}", this.SessionModel.SessionID, idIngresso, iRnd1.ToString() + iRnd2.ToString()));

            return ParseRetorno(retorno);
        }

        [Route("reservas/atual/cotas")]
        public RetornoModel<CompraModel> GetConsultarCotas()
        {
            LogUtil.Info(string.Format("##Get.Cotas## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                CarregarCompra(bo);
                retorno.Retorno = SessionModel.Compra;
                if (!SessionModel.Compra.StatusCotaPendente.Nominal && !SessionModel.Compra.StatusCotaPendente.Promocional)
                {
                    LogUtil.Info(string.Format("##Get.Cotas.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Nâo existem itens com cota"));

                    retorno.Sucesso = false;
                    retorno.Mensagem = "Nâo existem itens com cota Promocional ou Nominal pendentes no carrinho";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Pega as entregas da compra
        /// </summary>
        /// <returns></returns>
        [Route("reservas/atual/entregas")]
        [HttpGet]
        public RetornoModel<CompraModel> GetEntregas()
        {
            LogUtil.Info(string.Format("##Get.Entregas## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();

            retorno.Sucesso = true;
            retorno.Mensagem = "OK";

            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);

                retorno.Retorno = SessionModel.Compra;

                if (SessionModel.Compra.CarrinhoItens == null || SessionModel.Compra.CarrinhoItens.Count == 0)
                {
                    LogUtil.Info(string.Format("##Get.Entregas.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "CarrinhoItens == null // CarrinhoItens.Count == 0"));

                    retorno.Sucesso = false;
                    retorno.Mensagem = "Não existem itens no carrinho.";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }

                if (SessionModel.Compra.EntregaControles == null || SessionModel.Compra.EntregaControles.Count == 0)
                {
                    LogUtil.Info(string.Format("##Get.Entregas.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "EntregaControles == null // EntregaControles.Count == 0"));

                    retorno.Sucesso = false;
                    retorno.Mensagem = "Não existem entregas para esta compra.";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
            }

            LogUtil.Info(string.Format("##Get.Entregas.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

            return ParseRetorno(retorno);
        }

        [Route("reservas/atual/entregas/cep/{cep}")]
        [HttpGet]
        public RetornoModel<ReservaCepModel> GetEntregas(string cep)
        {
            LogUtil.Info(string.Format("##Get.Entregas.CEP## SESSION {0}, CEP {1}", this.SessionModel.SessionID, cep));

            RetornoModel<ReservaCepModel> retorno = new RetornoModel<ReservaCepModel>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);

                if (SessionModel.Compra.CarrinhoItens == null || SessionModel.Compra.CarrinhoItens.Count == 0)
                {
                    LogUtil.Info(string.Format("##Get.Entregas.CEP.ERROR## SESSION {0}, CEP {1}, MSG {2}", this.SessionModel.SessionID, cep, "CarrinhoItens == null // CarrinhoItens.Count == 0"));

                    retorno.Sucesso = false;
                    retorno.Mensagem = "Não existem itens no carrinho.";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }

                if (SessionModel.Compra.EntregaControles == null || SessionModel.Compra.EntregaControles.Count == 0)
                {
                    LogUtil.Info(string.Format("##Get.Entregas.CEP.ERROR## SESSION {0}, CEP {1}, MSG {2}", this.SessionModel.SessionID, cep, "EntregaControles == null // EntregaControles.Count == 0"));

                    retorno.Sucesso = false;
                    retorno.Mensagem = "Não existem entregas para esta compra.";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
                EntregaBO eBO = new EntregaBO(ado);
                GeoBO geoBO = new GeoBO(ado);
                tCEP tcep = geoBO.BuscarEndereco(cep);
                retorno.Retorno = new ReservaCepModel()
                {
                    entregaControles = eBO.FiltrarEntregasPorCEP(SessionModel.Compra.EntregaControles, cep),
                    enderecoTemp = (tcep != null) ? new EnderecoTemp(tcep) : null
                };
            }

            LogUtil.Info(string.Format("##Get.Entregas.CEP.SUCCESS## SESSION {0}, CEP {1}", this.SessionModel.SessionID, cep));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Retorna lista das formas de pagamento
        /// </summary>
        /// <returns></returns>
        [Route("reservas/atual/formas_pagamento")]
        [HttpGet]
        public RetornoModel<List<FormaPagamento>> GetFormasPagamento()
        {
            LogUtil.Info(string.Format("##Get.FormasPagamento## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<List<FormaPagamento>> retorno = new RetornoModel<List<FormaPagamento>>();
            List<FormaPagamento> resultado = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);

                    FormaPagamentoBO fpbo = new FormaPagamentoBO(ado);
                    //resultado = fpbo.ListarEvento(SessionModel.Compra.CarrinhoItens.Select(t => t.EventoID.Value).ToList(), SessionModel.Compra.Total.ValorTotal);
                    resultado = fpbo.ListarEvento(SessionModel.ClienteID, SessionModel.SessionID, SessionModel.Compra.Total.ValorTotal);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Get.FormasPagamento.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (resultado == null || resultado.Count == 0)
            {
                LogUtil.Info(string.Format("##Get.FormasPagamento.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Nenhuma forma de pagamento disponivel"));

                retorno.Mensagem = "Nenhuma forma de pagamento disponivel para essa compra";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
                retorno.Retorno = resultado;
                retorno.Sucesso = true;
            }

            LogUtil.Info(string.Format("##Get.FormasPagamento.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

            return ParseRetorno(retorno);
        }

        [Route("reservas/atual/ingressos")]
        [HttpGet]
        public RetornoModel<List<Carrinho>> GetItensReserva()
        {
            LogUtil.Info(string.Format("##Get.ItensReserva## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<List<Carrinho>> retorno = new RetornoModel<List<Carrinho>>();
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo);
                retorno.Retorno = SessionModel.Compra.CarrinhoItens;

                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
            }
            return ParseRetorno(retorno);
        }

        [Route("reservas/atual")]
        [HttpGet]
        public RetornoModel<CompraModel> GetReserva()
        {
            LogUtil.Info(string.Format("##Get.ReservaAtual## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo);
                retorno.Retorno = SessionModel.Compra;

                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna o token para início do pagamento com PayPal
        /// URL de acesso: reserva/atual/paypal/token
        /// Método de acesso: GET
        /// </summary>
        /// <returns>Token</returns>
        [Route("reservas/atual/paypal/token")]
        [HttpGet]
        public RetornoModel<SetExpressCheckoutResponseModel> GetPayPalToken(string returnUrl = "", string cancelUrl = "")
        {
            LogUtil.Info(string.Format("##Get.PayPal.Token## SESSION {0}", this.SessionModel.SessionID));

            var retorno = new RetornoModel<SetExpressCheckoutResponseModel>();
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);

                if (!bo.carrinhoBO.ValidarCarrinho(SessionModel.Compra.CarrinhoItens))
                {
                    LogUtil.Error(string.Format("##Get.PayPal.Token.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Algumas apresentações não estão mais disponíveis"));

                    retorno.Sucesso = false;
                    retorno.Retorno = new SetExpressCheckoutResponseModel();
                    retorno.Mensagem = "Algumas apresentações não estão mais disponíveis";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }

                try
                {
                    LogUtil.Debug(string.Format("##Get.PayPal.Token PayPalBO.setExpressCheckout## SESSION {0}", this.SessionModel.SessionID));

                    var payPal = new PayPalBO();
                    retorno = payPal.setExpressCheckout(SessionModel.Compra, returnUrl, cancelUrl);
                    SessionModel.TokenPayPal = retorno.Retorno.token;

                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Get.PayPal.Token.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                    retorno.Sucesso = false;
                    retorno.Retorno = new SetExpressCheckoutResponseModel();
                    retorno.Mensagem = ex.Message;

                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
            }

            return ParseRetorno(retorno);

        }

        /// <summary>
        /// Método da API que insere um ingresso ao rq.
        /// URL de acesso: reservas/atual/ingressos/
        /// Método de acesso: POST
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [Route("reservas/atual/ingressos")]
        [HttpPost]
        public RetornoModel<CompraModel> PostAddIngressorq(ReservaIngressoRequestModel rq)
        {
            LogUtil.Info(string.Format("##Post.AddIngresso## SESSION {0}, INGRESSO {1}, PREÇO {2}", this.SessionModel.SessionID, rq.ingressoId, rq.precoId));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);

                    if (bo.carrinhoBO.ConsultarTipoLugar(rq.ingressoId) == "M")
                    {
                        LogUtil.Info(string.Format("##Post.AddIngresso.ERROR## SESSION {0}, INGRESSO {1}, PREÇO {2}, MSG {3}", this.SessionModel.SessionID, rq.ingressoId, rq.precoId, "Este método não reserva mesa fechada"));

                        retorno.Mensagem = "Este método não reserva mesa fechada";
                        retorno.Sucesso = false;
                    }
                    else
                    {
                        retorno = bo.carrinhoBO.SolicitarReservaIngresso(SessionModel.Compra, rq.ingressoId, rq.precoId);

                        if (retorno.Sucesso)
                        {
                            SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                            var carrinhoItensValidarCota = SessionModel.Compra.CarrinhoItens.Where(w => w.IngressoID == rq.ingressoId).ToList();

                            var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(retorno.Retorno, carrinhoItensValidarCota);

                            if (!validacaoLimiteQuantidadeCotas.Sucesso)
                            {
                                Carrinho carrinho = SessionModel.Compra.CarrinhoItens.Where(c => c.IngressoID == rq.ingressoId).FirstOrDefault();
                                if (carrinho != null)
                                {
                                    retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                }
                                retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                                retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;

                                LogUtil.Info(string.Format("##Post.AddIngresso.ERROR## SESSION {0}, INGRESSO {1}, PREÇO {2}, MSG {3}", this.SessionModel.SessionID, rq.ingressoId, rq.precoId, "COTA: Você excedeu o limite de itens"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Post.AddIngresso.EXCEPTION## SESSION {0}, INGRESSO {1}, PREÇO {2}, MSG {3}", this.SessionModel.SessionID, rq.ingressoId, rq.precoId, ex.Message));

                retorno = new RetornoModel<CompraModel>();
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                LogUtil.Info(string.Format("##Post.AddIngresso.ERROR## SESSION {0}, INGRESSO {1}, PREÇO {2}, MSG {3}", this.SessionModel.SessionID, rq.ingressoId, rq.precoId, "Sucesso = false"));

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Post.AddIngresso.SUCCESS## SESSION {0}, INGRESSO {1}, PREÇO {2}", this.SessionModel.SessionID, rq.ingressoId, rq.precoId));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que adiciona um pacote ao rq.
        /// URL de acesso: rq/reserva/pacotes/{pacoteID}/{quantidade}
        /// Método de acesso: POST
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [Route("reservas/atual/pacotes")]
        [HttpPost]
        public RetornoModel<CompraModel> PostAddPacote([FromBody] ReservaPacoteRequestModel rq)
        {
            LogUtil.Info(string.Format("##Post.AddPacote## SESSION {0}", SessionModel.SessionID));

            var retorno = new RetornoModel<CompraModel> { Mensagem = "OK", Sucesso = true };

            try
            {
                if (rq != null)
                {
                    if (rq.pacotes.Any(p => p.pacoteID <= 0))
                    {
                        foreach (var pacote in rq.pacotes)
                        {
                            LogUtil.Info(string.Format("##Post.AddPacote.ERROR## SESSION {0}, QTD {1}, PACOTE {2}, MSG: Pacote não existe.", SessionModel.SessionID, pacote.qtd, pacote.pacoteID));

                            retorno.Mensagem = "Pacote não existe.";
                        }

                        retorno.Sucesso = false;
                    }
                    else
                    {
                        using (var ado = new MasterADOBase())
                        {
                            var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                            var carrinhoItens = new List<Carrinho>();

                            // Limpa as reservas anteriores.
                            if (rq.limparReservaAnterior)
                            {
                                LimparCompra(bo.carrinhoBO);
                                CarregarCliente(bo.clienteBO);

                                SessionModel.Compra = new CompraModel
                                {
                                    EstruturaVenda = new CompraEstruturaVendaModel
                                    {
                                        CanalID = SessionModel.CanalID,
                                        LojaID = SessionModel.LojaID,
                                        UsuarioID = SessionModel.UsuarioID
                                    },
                                    Login = SessionModel.Login,
                                    SessionID = SessionModel.SessionID
                                };
                            }
                            else
                            {
                                CarregarCompra(bo);
                                carrinhoItens = new List<Carrinho>(SessionModel.Compra.CarrinhoItens);
                            }

                            if (rq.pacotes != null)
                            {
                                var pacotes = rq.pacotes.Select(t => new PacoteReservaModel() { PacoteID = t.pacoteID, Quantidade = t.qtd }).ToList();

                                retorno = bo.carrinhoBO.SolicitarReservaPacote(SessionModel.Compra, pacotes);

                                var eventosApresentacoesCarrinho = retorno.Retorno.CarrinhoItens.Where(w => !carrinhoItens.Contains(w)).Select(s => new { s.EventoID, s.ApresentacaoID }).Distinct().ToList();

                                var restricoesIngressos = RestricoesQuantidadeIngressosEvento(ado, eventosApresentacoesCarrinho);

                                if (!restricoesIngressos.Sucesso)
                                {
                                    var carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.PacoteID != null && rq.pacotes.Select(s => s.pacoteID).ToList().Contains(c.PacoteID.Value)).ToList();

                                    foreach (var carrinho in carrinhos)
                                    {
                                        retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                    }

                                    LogUtil.Error(string.Format("##Post.AddPacote.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, restricoesIngressos.Mensagem));

                                    throw new CompraException(restricoesIngressos.Mensagem);
                                }

                                SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                                var carrinhoItensValidar = SessionModel.Compra.CarrinhoItens.Where(w => !carrinhoItens.Contains(w)).ToList();

                                var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(retorno.Retorno, carrinhoItensValidar);

                                if (!validacaoLimiteQuantidadeCotas.Sucesso)
                                {
                                    var carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.PacoteID != null && rq.pacotes.Select(s => s.pacoteID).ToList().Contains(c.PacoteID.Value)).ToList();

                                    foreach (var carrinho in carrinhos)
                                    {
                                        retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                    }

                                    LogUtil.Info(string.Format("##Post.AddPacote.ERROR## SESSION {0}, MSG {1}", SessionModel.SessionID, "COTA: Você excedeu o limite de itens"));
                                    retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;
                                    retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                                }

                                foreach (var pacote in rq.pacotes)
                                {
                                    LogUtil.Info(string.Format("##Post.AddPacote.SUCCESS## SESSION {0}, QTD {1}, PACOTE {2}", SessionModel.SessionID, pacote.qtd, pacote.pacoteID));
                                }
                            }
                            else
                            {
                                retorno.Sucesso = false;
                                retorno.Mensagem = "Não foram encontrados pacotes com esta solicitação. Tente novamente.";
                            }
                        }
                    }
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "Solicitação de pacote inválida. Tente novamente.";
                }
            }
            catch (Exception ex)
            {
                if (rq != null && rq.pacotes != null)
                {
                    foreach (var pacote in rq.pacotes)
                    {
                        LogUtil.Error(string.Format("##Post.AddPacote.EXCEPTION## SESSION {0}, QTD {1}, PACOTE {2}, MSG {3}", SessionModel.SessionID, pacote.qtd, pacote.pacoteID, ex.Message));
                    }
                }

                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                if (rq != null && rq.pacotes != null)
                {
                    foreach (var pacote in rq.pacotes)
                    {
                        LogUtil.Info(string.Format("##Post.AddPacote.ERROR## SESSION {0}, QTD {1}, PACOTE {2}, MSG {3}", SessionModel.SessionID, pacote.qtd, pacote.pacoteID, "Sucesso = false"));
                    }
                }

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            if (rq == null || rq.pacotes == null)
                return ParseRetorno(retorno);

            foreach (var pacote in rq.pacotes)
            {
                LogUtil.Info(string.Format("##Post.AddPacote.SUCCESS## SESSION {0}, QTD {1}, PACOTE {2}", SessionModel.SessionID, pacote.qtd, pacote.pacoteID));
            }

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que adiciona um vale ingresso ao rq.
        /// URL de acesso: rq/vale_ingresso
        /// Método de acesso: POST
        /// </summary>
        /// <param name="request">rq Model onde o vale ingresso será adicionado</param>
        /// reservas/atual/vale_ingressos (codigo, Compra+com entrega)
        [Route("reservas/atual/vale_ingressos")]
        [HttpPost]
        public RetornoModel<CompraModel> PostAddValeIngresso([FromBody]ReservaValeIngressoRequestModel request)
        {
            LogUtil.Info(string.Format("##Post.AddValeIngresso## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<CompraModel> retorno = null;
            tValeIngresso valeIngresso = null;
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);
                try
                {
                    valeIngresso = bo.valeIngressoBO.ValidarCodigo(request.codigo);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Post.AddValeIngresso.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    //LogUtil.Error(ex);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
                if (request == null || valeIngresso == null)
                {
                    LogUtil.Info(string.Format("##Post.AddValeIngresso.ERROR## SESSION {0}, CODIGO {1}, MSG {2}", this.SessionModel.SessionID, request.codigo, "valeIngresso == null"));

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = "Compra não informada ou não existe vale ingresso.";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
                else
                {
                    if (SessionModel.Compra.EntregaControles == null || SessionModel.Compra.EntregaControles.Count == 0)
                    {
                        LogUtil.Info(string.Format("##Post.AddValeIngresso.ERROR## SESSION {0}, CODIGO {1}, MSG {2}", this.SessionModel.SessionID, request.codigo, "EntregaControles == null // EntregaControles.Count == 0"));

                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = "Não existem entregas para esta compra.";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    if (SessionModel.Compra.StatusCotaPendente.NaoBin)
                    {
                        LogUtil.Info(string.Format("##Post.AddValeIngresso.ERROR## SESSION {0}, CODIGO {1}, MSG {2}", this.SessionModel.SessionID, request.codigo, "StatusCotaPendente.NaoBin"));

                        retorno.Sucesso = false;
                        retorno.Mensagem = "Existem cotas pendentes de validação no carrinho";
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    try
                    {
                        retorno = bo.valeIngressoBO.InserirValeIngresso(SessionModel.Compra, valeIngresso);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Post.AddValeIngresso.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        //LogUtil.Error(ex);
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                    if (!retorno.Sucesso)
                    {
                        LogUtil.Info(string.Format("##Post.AddValeIngresso.ERROR## SESSION {0}, CODIGO {1}, MSG {2}", this.SessionModel.SessionID, request.codigo, "Sucesso = false"));

                        retorno.Mensagem = "Vale ingresso não existe.";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                    else
                    {
                        if (SessionModel.ValesIngressoID == null)
                        {
                            SessionModel.ValesIngressoID = new List<int>();
                        }
                        SessionModel.ValesIngressoID.Add(valeIngresso.ID);
                    }
                }
            }

            LogUtil.Info(string.Format("##Post.AddValeIngresso.SUCCESS## SESSION {0}, CODIGO {1}", this.SessionModel.SessionID, request.codigo));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Vincula um endereço de entrega a uma compra.
        /// </summary>
        /// <param name="entrega"></param>
        /// <returns></returns>
        [Route("reservas/atual/entregas")]
        [HttpPost]
        public RetornoModel<CompraModel> PostEntrega([FromBody]ReservaEntregaRequestModel entrega)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();

            if (entrega != null)
            {
                LogUtil.Info(string.Format("##Post.SelecionarEntrega## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}", this.SessionModel.SessionID, entrega.entregaControleID, entrega.clienteEnderecoID, entrega.pdvID));

                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    if (entrega.entregaControleID == 0)
                    {
                        LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "entregaControleID == 0"));

                        retorno.Mensagem = "EntregaControleID não informado";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    CarregarCompra(bo, true);

                    if (SessionModel.Compra.EntregaControles == null)
                    {
                        LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "EntregaControles == null"));

                        retorno.Mensagem = "Não existem entregas disponível para seu carrinho";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                    }

                    var entregaControle = SessionModel.Compra.EntregaControles.FirstOrDefault(e => e.ID == entrega.entregaControleID);
                    if (entregaControle != null)
                    {
                        SessionModel.EntregaControleID = entrega.entregaControleID;
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Entrega inválida para o usuário"));

                        retorno.Mensagem = "Entrega inválida para o usuário";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                    if ((entregaControle.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaAgendada) || (entregaControle.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaNormal))
                    {
                        if (entrega.clienteEnderecoID == 0)
                        {
                            LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "clienteEnderecoID == 0"));

                            SessionModel.EntregaControleID = 0;
                            retorno.Mensagem = "clienteEnderecoID não informado";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                        }
                        else
                        {
                            tClienteEndereco Endereco = SessionModel.Login.Cliente.EnderecoList.FirstOrDefault(e => e.ID == entrega.clienteEnderecoID);
                            if (Endereco != null && Endereco.EntregaControles.Any(t => t.ID == SessionModel.EntregaControleID))
                            {
                                SessionModel.ClienteEnderecoID = entrega.clienteEnderecoID;
                            }
                            else
                            {
                                LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Endereço inválido para a entrega"));

                                SessionModel.EntregaControleID = 0;
                                retorno.Mensagem = "Endereço inválido para a entrega";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                            }
                        }
                    }
                    else if (entregaControle.Entrega.TipoAsEnum == enumEntregaTipo.retiradaPDV)
                    {
                        if (entrega.pdvID == 0)
                        {
                            LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "pdvID não informado"));

                            SessionModel.EntregaControleID = 0;
                            retorno.Mensagem = "pdvID não informado";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                        }
                        else
                        {
                            PontoVenda pontoVenda = null;
                            try
                            {
                                PontoVendaBO pontoVendaBO = new PontoVendaBO(ado);
                                pontoVenda = pontoVendaBO.Consultar(entrega.pdvID);
                            }
                            catch (Exception ex)
                            {
                                retorno.Mensagem = ex.Message;
                                retorno.Sucesso = false;
                                //LogUtil.Error(ex);
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                            }
                            if (pontoVenda != null)
                            {
                                SessionModel.PDVID = entrega.pdvID;
                            }
                            else
                            {
                                LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Ponto de Venda inválido"));

                                retorno.Mensagem = "Ponto de Venda inválido";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                            }
                        }
                    }

                    SessionModel.Compra.EntregaControleID = SessionModel.EntregaControleID;
                    SessionModel.Compra = bo.carrinhoBO.CalcularValores(SessionModel.Compra);
                }
                retorno.Retorno = SessionModel.Compra;
                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
            }
            else
            {
                LogUtil.Info(string.Format("##Post.SelecionarEntrega.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

                retorno.Mensagem = "Objeto de entrega não informado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Post.SelecionarEntrega.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

            return ParseRetorno(retorno);
        }

        [Route("reservas/atual/entregas/temp")]
        [HttpPost]
        public RetornoModel PostEntregaTemporaria([FromBody]EntregaTempModel entregaModel)
        {
            LogUtil.Info(string.Format("##Post.EntregaTemporaria## SESSION {0}, ENTREGA {1}, PDV {2}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.pdvID));

            RetornoModel retorno = new RetornoModel();
            retorno.Sucesso = true;

            using (var ado = new MasterADOBase())
            {
                CompraBOModel compraModelBO = new CompraBOModel(SessionModel.GetSiteId(), ado);

                if (entregaModel.entregaControleID == 0)
                {
                    LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "entregaControleID == 0"));

                    retorno.Mensagem = "EntregaControleID não informado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
                CarregarCompra(compraModelBO, true);

                if (SessionModel.Compra.EntregaControles == null)
                {
                    LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, PDV {2}, MSG {3}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.pdvID, "EntregaControles == null"));

                    retorno.Mensagem = "Não existem entregas disponível para seu carrinho";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }

                var entregaControle = SessionModel.Compra.EntregaControles.FirstOrDefault(e => e.ID == entregaModel.entregaControleID);
                if (entregaControle != null)
                {
                    SessionModel.EntregaControleID = entregaModel.entregaControleID;
                }
                else
                {
                    LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "Entrega inválida para o usuário"));

                    retorno.Mensagem = "Entrega inválida para o usuário";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }

                if ((entregaControle.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaAgendada) || (entregaControle.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaNormal))
                {
                    if (entregaModel.enderecoTemp == null || !entregaModel.enderecoTemp.Validar())
                    {
                        LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "Endereço não informado"));

                        SessionModel.EntregaControleID = 0;
                        retorno.Mensagem = "Endereço não informado";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    else
                    {
                        if (!entregaModel.enderecoTemp.TamanhoValidado())
                        {
                            LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "Endereço não informado"));

                            SessionModel.EntregaControleID = 0;
                            retorno.Mensagem = "Endereço inválido, a quantidade de caracteres inseridas nos campos excedeu o limite.";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                        }

                        var entregasPorCEP = compraModelBO.entregaBO.FiltrarEntregasPorCEP(SessionModel.Compra.EntregaControles, entregaModel.enderecoTemp.CEP);
                        if (entregasPorCEP.All(x => x.ID != entregaModel.entregaControleID))
                        {
                            LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGACONTROLE {1}, ENDERECO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "Endereço inválido para a entrega"));

                            SessionModel.EntregaControleID = 0;
                            retorno.Mensagem = "Endereço inválido para a entrega";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                        }

                        if (SessionModel.ClienteID != 0)
                        {
                            LogUtil.Info(string.Format("##Post.EntregaTemporaria.Logado## SESSION {0}, CLIENTE {1}", this.SessionModel.SessionID, SessionModel.ClienteID));


                            ClienteBO cBO = compraModelBO.clienteBO;
                            tClienteEndereco clienteEndereco = cBO.ConsultarEndereco(entregaModel.enderecoTemp.CEP, entregaModel.enderecoTemp.Endereco, entregaModel.enderecoTemp.Numero, entregaModel.enderecoTemp.Complemento, SessionModel.ClienteID);
                            if (clienteEndereco != null)
                            {
                                LogUtil.Info(string.Format("##Post.EntregaTemporaria.EnderecoEncontrado## SESSION {0}, CLIENTE {1}, ENDERECO {2}", this.SessionModel.SessionID, SessionModel.ClienteID, clienteEndereco.ID));

                                SessionModel.ClienteEnderecoID = clienteEndereco.ID;
                                SessionModel.EntregaControleID = entregaModel.entregaControleID;
                                SessionModel.PDVID = entregaModel.pdvID;
                            }
                            else
                            {
                                try
                                {
                                    tCliente cliente = SessionModel.Login.Cliente;
                                    clienteEndereco = new tClienteEndereco()
                                    {
                                        CEP = entregaModel.enderecoTemp.CEP,
                                        Endereco = entregaModel.enderecoTemp.Endereco,
                                        Numero = entregaModel.enderecoTemp.Numero,
                                        Cidade = entregaModel.enderecoTemp.Cidade,
                                        Estado = entregaModel.enderecoTemp.Estado,
                                        Complemento = entregaModel.enderecoTemp.Complemento,
                                        Bairro = entregaModel.enderecoTemp.Bairro,
                                        Nome = cliente.Nome,
                                        CPF = cliente.CPF,
                                        RG = cliente.RG,
                                        ClienteID = cliente.ID,
                                        EnderecoTipoID = 1,
                                        EnderecoPrincipal = "F",
                                        StatusConsulta = 0
                                    };

                                    //Inserir Endereco retornando Id do mesmo
                                    LogUtil.Info(string.Format("##Post.EntregaTemporaria.NovoEndereco## SESSION {0}, CLIENTE {1}, ENDERECO {2}", this.SessionModel.SessionID, SessionModel.ClienteID, clienteEndereco.Endereco));

                                    int enderecoId = cBO.InserirEndereco(clienteEndereco);
                                    if (enderecoId > 0)
                                    {
                                        LogUtil.Info(string.Format("##Post.EntregaTemporaria.NovoEndereco.SUCCESS## SESSION {0}, CLIENTE {1}, ENTREGACONTROLE {2}, ENDERECO {3}", this.SessionModel.SessionID, this.SessionModel.ClienteID, entregaModel.entregaControleID, enderecoId));

                                        SessionModel.ClienteEnderecoID = enderecoId;
                                        SessionModel.EntregaControleID = entregaModel.entregaControleID;
                                        SessionModel.PDVID = entregaModel.pdvID;
                                    }
                                    else
                                    {
                                        LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, CLIENTE {1}, ENTREGACONTROLE {2}, ENDERECO {3}, MSG {4}", this.SessionModel.SessionID, this.SessionModel.ClienteID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, "Endereço não cadastrado"));

                                        SessionModel.EntregaControleID = 0;
                                        SessionModel.EnderecoTemp = null;
                                        NewRelicIgnoreTransaction();
                                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel { Mensagem = "Endereço não cadastrado, tente novamente.", Sucesso = false })));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogUtil.Error(string.Format("##Post.EntregaTemporaria.NovoEndereço.EXCEPTION## SESSION {0}, CLIENTE {1}, MSG {2}", this.SessionModel.SessionID, this.SessionModel.ClienteID, ex.Message), ex);


                                    SessionModel.EntregaControleID = 0;
                                    SessionModel.EnderecoTemp = null;
                                    NewRelicIgnoreTransaction();
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel { Mensagem = "Não foi possivel cadastrar o endereço, tente novamente.", Sucesso = false })));
                                }
                            }
                        }

                        else
                        {
                            LogUtil.Info(string.Format("##Post.EntregaTemporaria.NãoLogado## SESSION {0}, ENTREGACONTROLE {1}", this.SessionModel.SessionID, entregaModel.entregaControleID));

                            SessionModel.EnderecoTemp = entregaModel.enderecoTemp;
                            SessionModel.EntregaControleID = entregaModel.entregaControleID;
                            SessionModel.PDVID = entregaModel.pdvID;
                        }
                    }
                }
                else if (entregaControle.Entrega.TipoAsEnum == enumEntregaTipo.retiradaPDV)
                {
                    if (entregaModel.pdvID < 0)
                    {
                        LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "pdvID não informado"));

                        SessionModel.EntregaControleID = 0;
                        SessionModel.EnderecoTemp = null;
                        retorno.Mensagem = "pdvID não informado";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                    }
                    else if (entregaModel.pdvID == 0) //Impressão em casa
                    {
                        SessionModel.PDVID = 0;
                        SessionModel.EnderecoTemp = null;
                    }
                    else
                    {
                        PontoVenda pontoVenda = null;
                        try
                        {
                            PontoVendaBO pontoVendaBO = new PontoVendaBO(ado);
                            pontoVenda = pontoVendaBO.Consultar(entregaModel.pdvID);
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, ex.Message), ex);

                            retorno.Mensagem = ex.Message;
                            retorno.Sucesso = false;
                            //LogUtil.Error(ex);
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                        }
                        if (pontoVenda != null)
                        {
                            SessionModel.PDVID = entregaModel.pdvID;
                            SessionModel.EnderecoTemp = null;
                        }
                        else
                        {
                            LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "Ponto de Venda inválido"));

                            retorno.Mensagem = "Ponto de Venda inválido";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                }
            }

            if (retorno.Sucesso)
            {
                LogUtil.Info(string.Format("##Post.EntregaTemporaria.SUCCESS## SESSION {0}, ENTREGA {1}, PDV {2}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.pdvID));
                retorno.Mensagem = "OK";
            }
            else
            {
                LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, ENTREGA {1}, ENDEREÇO {2}, PDV {3}, MSG {4}", this.SessionModel.SessionID, entregaModel.entregaControleID, entregaModel.enderecoTemp.Endereco, entregaModel.pdvID, "Sucesso = false"));
                retorno.Mensagem = "Erro";
            }

            return retorno;
        }

        /// <summary>
        /// Finaliza compra
        /// </summary>
        /// <param name="pagamentos"></param>
        /// <returns></returns>
        [Route("reservas/atual/pagamentos")]
        [HttpPost]
        public RetornoModel<CompraModel> PostFinalizarCompra([FromBody]List<CompraPagamentoModel> pagamentos)
        {
            if (pagamentos != null && pagamentos.Any())
                LogUtil.Info(string.Format("##Post.FinalizarCompra## SESSION {0}", this.SessionModel.SessionID));
            else
                LogUtil.Info(string.Format("##Post.FinalizarCompra.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);

                if (!bo.carrinhoBO.ValidarCarrinho(SessionModel.Compra.CarrinhoItens))
                {
                    LogUtil.Info(string.Format("##Post.FinalizarCompra.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Algumas das apresentações não estão mais disponiveis"));

                    retorno = new RetornoModel<CompraModel>
                    {
                        Mensagem = "Algumas das apresentações não estão mais disponiveis",
                        Sucesso = false
                    };
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }

                if (pagamentos != null && pagamentos.Any(a => a.formaPagamento != null && a.formaPagamento.NomeAsEnum == enumFormaPagamento.PayPal))
                {
                    var pagamentosPayPal = pagamentos.Where(w => w.formaPagamento.NomeAsEnum == enumFormaPagamento.PayPal);

                    if (pagamentosPayPal.Any(a => a.PayPal == null || a.PayPal.Token != SessionModel.TokenPayPal))
                    {
                        retorno = new RetornoModel<CompraModel> { Mensagem = "Token PayPal inválido", Sucesso = false };
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }

                // Se o evento for entrada franca, é necessário realizar validação de quantidade de ingressos.
                var evento = RecuperaEventoComValidacaoFinalizarCompra(pagamentos, retorno, ado);
                if (evento.EntradaFranca)
                {
                    try
                    {
                        new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(evento.IR_EventoID, SessionModel.ClienteID, SessionModel.Compra.CarrinhoItens.First().ApresentacaoID.Value, SessionModel.SessionID);
                    }
                    catch (InvalidOperationException ex)
                    {
                        LogUtil.Error(string.Format("##Post.FinalizarCompra.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        retorno = new RetornoModel<CompraModel> { Mensagem = ex.Message, Sucesso = false };
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }

                // Restrições Tim Burton
                int eventoTimBurtonID = Convert.ToInt32(ConfigurationManager.AppSettings["EventoTimBurton"]);
                if (evento.IR_EventoID == eventoTimBurtonID)
                {
                    int limite = Convert.ToInt32((Convert.ToInt32(ConfigurationManager.AppSettings["EventoTimBurtonPrimeiraApresentacao"]) == SessionModel.Compra.CarrinhoItens.First().ApresentacaoID.Value ?
                        ConfigurationManager.AppSettings["EventoTimBurtonPrimeiraApresentacaoLimite"] : ConfigurationManager.AppSettings["EventoTimBurtonLimite"]));
                    try
                    {
                        new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(evento.IR_EventoID, SessionModel.ClienteID, SessionModel.Compra.CarrinhoItens.First().ApresentacaoID.Value, SessionModel.SessionID, Int32.MaxValue, limite, Int32.MaxValue);
                    }
                    catch (InvalidOperationException ex)
                    {
                        LogUtil.Error(string.Format("##Post.FinalizarCompra.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }

                var eventosApresentacoesCarrinho = SessionModel.Compra.CarrinhoItens.Select(s => new { s.EventoID, s.ApresentacaoID }).Distinct().ToList();

                var restricoesIngressos = RestricoesQuantidadeIngressosEvento(ado, eventosApresentacoesCarrinho);

                if (!restricoesIngressos.Sucesso)
                {
                    LogUtil.Error(string.Format("##Post.FinalizarCompra.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, restricoesIngressos.Mensagem));

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = restricoesIngressos.Mensagem;
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }

                try
                {
                    VendaBilheteriaBO vendaBO = new VendaBilheteriaBO(ado);

                    // Incluindo informação do IP da maquina cliente, que sera encaminhado para a Accertify (DES-610)
                    pagamentos[0].IP = HttpContext.Current.Request.UserHostAddress;

                    var entradaFranca = SessionModel.Compra.Total.ValorTotal == 0;

                    retorno = vendaBO.FinalizarCompra(SessionModel.Compra, pagamentos, entradaFranca);

                    if (retorno.Sucesso)
                    {
                        bo.carrinhoBO.AtualizarStatus(SessionModel.SessionID, enumCarrinhoStatus.vendido);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Post.FinalizarCompra.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                    retorno = new RetornoModel<CompraModel> { Mensagem = ex.Message, Sucesso = false };
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }

                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Post.FinalizarCompra.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                    // Seleciona todos itens do carrinho com cota verificada que são do tipo nominal ou promocional
                    var carrinhoItensCota = SessionModel.Compra.CarrinhoItens.Where(w => (w.CotaVerificada ?? false) && w.CotaItemObject != null && (w.CotaItemObject.NominalAsBool || w.CotaItemObject.ValidaCodigoPromocional)).ToList();

                    foreach (var item in carrinhoItensCota)
                    {
                        var donoId = item.DonoID ?? 0;

                        if (donoId > 0)
                        {
                            var donoIngressoBO = new DonoIngressoBO(ado);
                            donoIngressoBO.Remover(donoId);
                        }

                        var carrinhoBO = new CarrinhoBO(ado);
                        carrinhoBO.RemoverDadosCota(item.ID);

                    }

                    AdicionarLogPagamento(pagamentos, new CompraException(retorno.Mensagem));
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                else
                {
                    SessionModel.EntregaControleID = 0;
                    SessionModel.PDVID = 0;
                    SessionModel.ClienteEnderecoID = 0;
                }

                retorno.Mensagem = "OK";
                retorno.Sucesso = true;
            }

            LogUtil.Info(string.Format("##Post.FinalizarCompra.SUCCESS## SESSION {0}", this.SessionModel.SessionID));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Finaliza compra externa
        /// </summary>
        /// <param name="pagamentos"></param>
        /// <returns></returns>
        [Route("reservas/atual/pagamentos_externo")]
        [HttpPost]
        public RetornoModel<CompraExternaModel> PostFinalizarCompraExterna([FromBody] List<CompraPagamentoExternoModel> pagamentos)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (pagamentos != null && pagamentos.Any())
                LogUtil.Info(string.Format("##Post.FinalizarCompraExterna## SESSION {0}, PGTO {1}, VALOR {2}, COD_AUT {3}, NSU {4}", this.SessionModel.SessionID, pagamentos[0].formaPagamento == null ? "NA" : pagamentos[0].formaPagamento.Nome ?? "NA", pagamentos[0].valor, pagamentos[0].codigoAutorizacao ?? "NA", pagamentos[0].NSU ?? "NA"));
            else
                LogUtil.Info(string.Format("##Post.FinalizarCompraExterna.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

            // Verifica no AppSettings se está ativa a verificação de IPs WhiteList.
            if (new IRLib.ConfigGerenciador().getAtivaIPsWhiteList())
            {
                // Incluindo informação do IP da máquina cliente, que será verificado e validado com os IPs liberadores para efetuar pagamento externo.
                pagamentos[0].IP = HttpContext.Current.Request.Headers["CF-Connecting-IP"];

                // Caso o CloudFlare esteja desabilitado, o header CF-Connecting-IP virá como vazio ou nulo e a API deve utilizar o UserHostAddress para buscar o IP de origem.
                if (String.IsNullOrEmpty(pagamentos[0].IP))
                {
                    pagamentos[0].IP = HttpContext.Current.Request.UserHostAddress;
                }

                // Caso IP ainda seja nulo ou não esteja na WhiteList, a API recusa a solicitação.
                if (pagamentos[0].IP == null || !new IRLib.ConfigGerenciador().getIPsWhiteListPagamentoExterno().Contains(pagamentos[0].IP))
                {
                    stopwatch.Stop();
                    LogUtil.Info(string.Format("##Post.FinalizarCompraExterna## SESSION {0}, IP {1}, MSG {2}, TEMPO DECORRIDO {3}", this.SessionModel.SessionID, pagamentos[0].IP ?? "NA", "Não foi possível identificar o IP da solicitação ou o mesmo não consta na White List.", stopwatch.Elapsed));
                    return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = new CompraExternaModel(), Mensagem = "Não foi possível identificar o IP da solicitação ou o mesmo não consta na White List." };
                }
            }

            LogUtil.Info(string.Format("##Post.FinalizarCompraExterna## SESSION {0}, IP {1}", this.SessionModel.SessionID, pagamentos[0].IP ?? "NA"));

            var retorno = new RetornoModel<CompraModel>();
            var compraExterna = new RetornoModel<CompraExternaModel>();

            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                CarregarCompra(bo, true);

                // Se o evento for entrada franca, é necessário realizar validação de quantidade de ingressos.
                var evento = RecuperaEventoComValidacaoFinalizarCompraExterna(retorno, ado);
                if (evento.EntradaFranca)
                {
                    try
                    {
                        new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(evento.IR_EventoID, SessionModel.ClienteID, SessionModel.Compra.CarrinhoItens.First().ApresentacaoID.Value, SessionModel.SessionID);
                    }
                    catch (InvalidOperationException ex)
                    {
                        stopwatch.Stop();
                        LogUtil.Error(string.Format("##Post.FinalizarCompraExterna.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);

                        retorno = new RetornoModel<CompraModel>();
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }

                try
                {
                    var vendaBO = new VendaBilheteriaBO(ado);

                    compraExterna = vendaBO.FinalizarCompraExterna(SessionModel.Compra, pagamentos, evento.EntradaFranca);

                    if (compraExterna.Sucesso)
                    {
                        bo.carrinhoBO.AtualizarStatus(SessionModel.SessionID, enumCarrinhoStatus.vendido);

                        var retornoInfosObrigatorias = InformacoesObrigatoriasIngresso(evento.IR_EventoID, ado);
                        compraExterna.Retorno.InfosObrigatoriasIngresso = retornoInfosObrigatorias;
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    LogUtil.Error(string.Format("##Post.FinalizarCompraExterna.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);

                    compraExterna.Mensagem = ex.Message;
                    compraExterna.Sucesso = false;
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(compraExterna)));
                }

                stopwatch.Stop();

                if (!compraExterna.Sucesso)
                {
                    LogUtil.Error(string.Format("##Post.FinalizarCompraExterna.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, compraExterna.Mensagem, stopwatch.Elapsed));

                    // Seleciona todos itens do carrinho com cota verificada que são do tipo nominal ou promocional
                    var carrinhoItensCota = SessionModel.Compra.CarrinhoItens.Where(w => (w.CotaVerificada ?? false) && w.CotaItemObject != null && (w.CotaItemObject.NominalAsBool || w.CotaItemObject.ValidaCodigoPromocional)).ToList();

                    foreach (var item in carrinhoItensCota)
                    {
                        var donoId = item.DonoID ?? 0;

                        if (donoId > 0)
                        {
                            var donoIngressoBO = new DonoIngressoBO(ado);
                            donoIngressoBO.Remover(donoId);
                        }

                        var carrinhoBO = new CarrinhoBO(ado);
                        carrinhoBO.RemoverDadosCota(item.ID);

                    }

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(compraExterna)));
                }

                SessionModel.EntregaControleID = 0;
                SessionModel.PDVID = 0;
                SessionModel.ClienteEnderecoID = 0;

                compraExterna.Mensagem = "OK";
                compraExterna.Sucesso = true;
            }

            LogUtil.Info(string.Format("##Post.FinalizarCompraExterna.SUCCESS## SESSION {0}, SENHA {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, compraExterna.Retorno.SenhaVenda, stopwatch.Elapsed));
            return ParseRetorno(compraExterna);
        }

        [Route("reservas/atual/mesas/qtd")]
        [HttpPost]
        public RetornoModel<CompraModel> PostMesaAberta([FromBody]ReservaMesaAbertaListRequestModel mesaAbertaRequest)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();

            if (mesaAbertaRequest != null)
            {
                LogUtil.Info(string.Format("##Post.AddMesaAberta## SESSION {0}", this.SessionModel.SessionID));
                try
                {
                    using (var ado = new MasterADOBase())
                    {
                        var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                        var carrinhoItens = new List<Carrinho>();

                        if (mesaAbertaRequest.limparReservaAnterior)
                        {
                            LimparCompra(bo.carrinhoBO);
                            CarregarCliente(bo.clienteBO);

                            SessionModel.Compra = new CompraModel();
                            SessionModel.Compra.EstruturaVenda = new CompraEstruturaVendaModel();
                            SessionModel.Compra.EstruturaVenda.CanalID = SessionModel.CanalID;
                            SessionModel.Compra.EstruturaVenda.LojaID = SessionModel.LojaID;
                            SessionModel.Compra.EstruturaVenda.UsuarioID = SessionModel.UsuarioID;
                            SessionModel.Compra.Login = SessionModel.Login;
                            SessionModel.Compra.SessionID = SessionModel.SessionID;
                        }
                        else
                        {
                            CarregarCompra(bo);
                            carrinhoItens = new List<Carrinho>(SessionModel.Compra.CarrinhoItens);
                        }

                        foreach (var mesa in mesaAbertaRequest.mesas)
                        {
                            retorno = bo.carrinhoBO.SolicitarReservaMesaAberta(SessionModel.Compra, mesa.apresentacaoID, mesa.lugarID, mesa.quantidade, mesa.precoID);
                            if (!retorno.Sucesso)
                                break;

                            SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                            carrinhoItens = SessionModel.Compra.CarrinhoItens.Where(w => !carrinhoItens.Contains(w)).ToList();

                            var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(retorno.Retorno, carrinhoItens);

                            if (!validacaoLimiteQuantidadeCotas.Sucesso)
                            {
                                List<Carrinho> carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.LugarID == mesa.lugarID).ToList();
                                foreach (var carrinho in carrinhos)
                                {
                                    if (carrinho != null)
                                    {
                                        retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                    }
                                }
                                retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;
                                retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Post.AddMesaAberta.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);
                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    LogUtil.Error(ex);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Post.AddMesaAberta.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            else
            {
                LogUtil.Info(string.Format("##Post.AddMesaAberta.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));
                retorno.Mensagem = "Objeto de Mesa Aberta não informado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Reserva uma mesa fechada.
        /// </summary>
        /// <param name="mesaFechadaRequest"></param>
        /// <returns></returns>
        [Route("reservas/atual/mesas")]
        [HttpPost]
        public RetornoModel<CompraModel> PostMesaFechada([FromBody]ReservaMesaFechadaListRequestModel mesaFechadaRequest)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            if (mesaFechadaRequest != null)
            {
                LogUtil.Info(string.Format("##Post.AddMesaFechada## SESSION {0}", this.SessionModel.SessionID));

                try
                {
                    using (var ado = new MasterADOBase())
                    {
                        var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                        var carrinhoItens = new List<Carrinho>();

                        if (mesaFechadaRequest.limparReservaAnterior)
                        {
                            LimparCompra(bo.carrinhoBO);
                            CarregarCliente(bo.clienteBO);

                            SessionModel.Compra = new CompraModel();
                            SessionModel.Compra.EstruturaVenda = new CompraEstruturaVendaModel();
                            SessionModel.Compra.EstruturaVenda.CanalID = SessionModel.CanalID;
                            SessionModel.Compra.EstruturaVenda.LojaID = SessionModel.LojaID;
                            SessionModel.Compra.EstruturaVenda.UsuarioID = SessionModel.UsuarioID;
                            SessionModel.Compra.Login = SessionModel.Login;
                            SessionModel.Compra.SessionID = SessionModel.SessionID;
                        }
                        else
                        {
                            CarregarCompra(bo);
                            carrinhoItens = new List<Carrinho>(SessionModel.Compra.CarrinhoItens);
                        }

                        foreach (var mesa in mesaFechadaRequest.mesas)
                        {
                            retorno = bo.carrinhoBO.SolicitarReservaMesaFechada(SessionModel.Compra, mesa.apresentacaoID, mesa.lugarID, mesa.precoID);
                            if (!retorno.Sucesso)
                                break;
                            SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                            carrinhoItens = SessionModel.Compra.CarrinhoItens.Where(w => !carrinhoItens.Contains(w)).ToList();

                            var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(retorno.Retorno, carrinhoItens);

                            if (!validacaoLimiteQuantidadeCotas.Sucesso)
                            {
                                List<Carrinho> carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.LugarID == mesa.lugarID).ToList();
                                foreach (var carrinho in carrinhos)
                                {
                                    if (carrinho != null)
                                    {
                                        retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                    }
                                }
                                retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;
                                retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                                LogUtil.Info(string.Format("##Post.AddMesaFechada.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "COTA: Você excedeu o limite de itens"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Post.AddMesaFechada.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                    retorno = new RetornoModel<CompraModel>();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    //LogUtil.Error(ex);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Post.AddMesaFechada.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            else
            {
                LogUtil.Info(string.Format("##Post.AddMesaFechada.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

                retorno.Mensagem = "Objeto de Mesa Fechada não informado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Post.AddMesaFechada.SUCCESS## SESSION {0}", this.SessionModel.SessionID));
            return ParseRetorno(retorno);
        }

        [Route("reservas/atual/cotas")]
        [HttpPost]
        public RetornoModel<CompraModel> PostValidarCotas([FromBody]List<ReservaCotaRequestModel> request)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                    CarregarCompra(bo);
                    if (request != null)
                    {
                        LogUtil.Info(string.Format("##Post.ValidarCotas## SESSION {0}, CARRINHO {1}, CODIGO {2}, DONO_CPF {3}, DONO_EMAIL {4}", this.SessionModel.SessionID, request[0].CarrinhoID, request[0].CodigoPromocional, request[0].DonoIngresso.CPF, request[0].DonoIngresso.Email));

                        foreach (var item in SessionModel.Compra.CarrinhoItens)
                        {
                            ReservaCotaRequestModel cotaModel = request.Where(x => x.CarrinhoID == item.ID).FirstOrDefault();
                            if (cotaModel != null && item.CotaItemObject != null)
                            {
                                if (cotaModel.DonoIngresso != null)
                                {
                                    item.CotaItemObject.DonoIngresso = new tDonoIngresso()
                                    {
                                        ClienteID = cotaModel.DonoIngresso.ClienteID,
                                        CPF = cotaModel.DonoIngresso.CPF,
                                        CPFResponsavel = cotaModel.DonoIngresso.CPFResponsavel,
                                        DataNascimento = cotaModel.DonoIngresso.DataNascimento,
                                        DataNascimentoAsDateTime = cotaModel.DonoIngresso.DataNascimentoAsDateTime,
                                        Email = cotaModel.DonoIngresso.Email,
                                        ID = cotaModel.DonoIngresso.ID,
                                        Nome = cotaModel.DonoIngresso.Nome,
                                        NomeResponsavel = cotaModel.DonoIngresso.NomeResponsavel,
                                        RG = cotaModel.DonoIngresso.RG,
                                        Telefone = cotaModel.DonoIngresso.Telefone
                                    };
                                }
                                item.CotaItemObject.CodigoPromocional = (cotaModel.CodigoPromocional ?? "");
                            }
                        }
                    }
                    retorno = bo.cotaBO.ValidarCotas(SessionModel.Compra);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Post.ValidarCotas.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                retorno.Mensagem = ex.Message;
                retorno.Retorno = SessionModel.Compra;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                LogUtil.Info(string.Format("##Post.ValidarCotas.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Post.ValidarCotas.SUCCESS## SESSION {0}", this.SessionModel.SessionID));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Altera preço
        /// </summary>
        /// <param name="IngressoID"></param>
        /// <param name="precoID"></param>
        /// <returns></returns>
        [Route("reservas/atual/ingressos/{ingressoID}/troca_preco")]
        [HttpPut]
        public RetornoModel<CompraModel> PutAlteraPreco(int IngressoID, [FromBody]ReservaTrocaPrecoRequestModel rq)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            LogUtil.Info(string.Format("##Put.AlteraPreço## SESSION {0}, INGRESSO {1}, PREÇO {2}", this.SessionModel.SessionID, IngressoID, rq.precoID));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);
                    Carrinho carrinho = SessionModel.Compra.CarrinhoItens.Where(x => x.IngressoID == IngressoID).FirstOrDefault();

                    var retornoAlteracao = bo.carrinhoBO.AlterarPrecoIngresso(carrinho, rq.precoID, GerarEstruturaVenda(), SessionModel.SessionID, SessionModel.ClienteID);

                    CarregarCompra(bo);

                    if (retornoAlteracao.Sucesso)
                    {
                        SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                        var carrinhoItensValidar = SessionModel.Compra.CarrinhoItens.Where(w => w.IngressoID == IngressoID).ToList();

                        var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(SessionModel.Compra, carrinhoItensValidar);

                        if (!validacaoLimiteQuantidadeCotas.Sucesso)
                        {
                            bo.carrinhoBO.AlterarPrecoIngresso(carrinho, retornoAlteracao.Retorno, GerarEstruturaVenda(), SessionModel.SessionID, SessionModel.ClienteID);

                            CarregarCompra(bo);

                            retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                            retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;
                            LogUtil.Info(string.Format("##Put.AlteraPreço.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "COTA: Você excedeu o limite de itens com este preço"));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = retornoAlteracao.Mensagem;
                        retorno.Sucesso = false;
                    }
                    retorno.Retorno = SessionModel.Compra;
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogUtil.Error(string.Format("##Put.AlteraPreço.EXCEPTION## SESSION {0}, MSG {1} TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);

                retorno.Mensagem = ex.Message;
                retorno.Retorno = null;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            stopwatch.Stop();

            if (!retorno.Sucesso)
            {
                LogUtil.Info(string.Format("##Put.AlteraPreço.ERROR## SESSION {0}, MSG {1} TEMPO DECORRIDO {2}", this.SessionModel.SessionID, "Sucesso = false", stopwatch.Elapsed));

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Put.AlteraPreço.SUCCESS## SESSION {0}, INGRESSO {1}, PREÇO {2} TEMPO DECORRIDO {3}", this.SessionModel.SessionID, IngressoID, rq.precoID, stopwatch.Elapsed));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Esse método deve atualizar no banco de dados na tabela ingresso da base IngressosNovo
        /// e na tabela rq da base SiteIR o tempo de expiração da reserva,
        /// adicionando o tempo passado como parâmetro ao tempo que estava la.
        /// </summary>
        /// <param name="rq"></param>
        /// <returns>Esse método retorna o objeto Compra, com os itens do rq com sua reserva atualizada.</returns>
        [Route("reservas/atual/aumenta_tempo")]
        [HttpPut]
        public RetornoModel<CompraModel> PutAumentaTempo([FromBody]ReservaAumentaTempoRequestModel rq)
        {
            LogUtil.Info(string.Format("##Put.AumentaTempo## SESSION {0}, TEMPO {1}", this.SessionModel.SessionID, rq.minutos));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();

            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);
                    if (SessionModel.Compra.CarrinhoItens.Count > 0)
                    {
                        if (bo.carrinhoBO.AtualizarTempoCarrinhoIngresso(SessionModel.SessionID, SessionModel.ClienteID, rq.minutos))
                        {
                            retorno.Retorno = SessionModel.Compra;

                            retorno.Mensagem = "OK";
                            retorno.Sucesso = true;
                        }
                        else
                        {
                            LogUtil.Info(string.Format("##Put.AumentaTempo.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Nenhum ingresso atualizado"));

                            retorno.Sucesso = false;
                            retorno.Mensagem = "Nenhum ingresso atualizado.";
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Put.AumentaTempo.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                retorno.Mensagem = ex.Message;
                retorno.Retorno = null;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Put.AumentaTempo.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Reserva uma mesa aberta
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <summary>
        /// Realiza a troca de preço de uma mesa
        /// </summary>
        /// <param name="precoID"></param>
        /// <param name="mesa"></param>
        /// <returns></returns>
        [Route("reservas/atual/mesas/{lugarID}/troca_preco")]
        [HttpPut]
        public RetornoModel<CompraModel> PutTrocaPrecoMesa(int lugarID, [FromBody]ReservaTrocaPrecoRequestModel rq)
        {
            LogUtil.Info(string.Format("##Put.TrocaPrecoMesa## SESSION {0}, LUGAR {1}, PREÇO {2}", this.SessionModel.SessionID, lugarID, rq.precoID));

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo);
                    Carrinho carrinho = SessionModel.Compra.CarrinhoItens.Where(x => x.LugarID == lugarID).FirstOrDefault();
                    var retornoAlteracao = bo.carrinhoBO.AlterarPrecoLugar(carrinho, rq.precoID, GerarEstruturaVenda(), SessionModel.SessionID, SessionModel.ClienteID);

                    CarregarCompra(bo);

                    if (retornoAlteracao.Sucesso)
                    {
                        SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                        var carrinhoItensValidar = SessionModel.Compra.CarrinhoItens.Where(w => w.LugarID == lugarID).ToList();

                        var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(SessionModel.Compra, carrinhoItensValidar);

                        if (!validacaoLimiteQuantidadeCotas.Sucesso)
                        {
                            bo.carrinhoBO.AlterarPrecoLugar(carrinho, retornoAlteracao.Retorno, GerarEstruturaVenda(), SessionModel.SessionID, SessionModel.ClienteID);

                            CarregarCompra(bo);

                            retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                            retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;
                            LogUtil.Info(string.Format("##Put.TrocaPrecoMesa.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "COTA: Você excedeu o limite de itens com este preço"));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = retornoAlteracao.Mensagem;
                        retorno.Sucesso = false;
                    }
                    retorno.Retorno = SessionModel.Compra;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Put.TrocaPrecoMesa.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                retorno.Mensagem = ex.Message;
                retorno.Retorno = null;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            if (!retorno.Sucesso)
            {
                LogUtil.Info(string.Format("##Put.TrocaPrecoMesa.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Put.TrocaPrecoMesa.SUCCESS## SESSION {0}", this.SessionModel.SessionID));
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Insere itens no rq
        /// </summary>
        /// <param name="reserva"></param>
        /// <returns></returns>
        [Route("reservas/atual/ingressos/qtd")]
        [HttpPost]
        public RetornoModel<CompraModel> ReservaIngressoPorQuantidade([FromBody]ReservaIngressosQtdRquestModel reserva)
        {
            if (reserva == null || !reserva.Validar())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Um ou mais parâmetros obrigatórios não foram informados ou são inválidos."));
            }

            LogUtil.Info(string.Format("##Post.AddIngressoQTD## SESSION {0}", this.SessionModel.SessionID));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            using (var ado = new MasterADOBase())
            {
                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                var carrinhoItens = new List<Carrinho>();

                // Limpa as reservas anteriores.
                if (reserva.limparReservaAnterior)
                {
                    var resultado = LimparCompra(bo.carrinhoBO);
                    CarregarCliente(bo.clienteBO);

                    SessionModel.Compra = new CompraModel();
                    SessionModel.Compra.EstruturaVenda = new CompraEstruturaVendaModel();
                    SessionModel.Compra.EstruturaVenda.CanalID = SessionModel.CanalID;
                    SessionModel.Compra.EstruturaVenda.LojaID = SessionModel.LojaID;
                    SessionModel.Compra.EstruturaVenda.UsuarioID = SessionModel.UsuarioID;
                    SessionModel.Compra.Login = SessionModel.Login;
                    SessionModel.Compra.SessionID = SessionModel.SessionID;
                }
                else
                {
                    CarregarCompra(bo);
                    carrinhoItens = new List<Carrinho>(SessionModel.Compra.CarrinhoItens);
                }

                // Consulta a apresentação para retornar o código do evento.
                var apresentacao = new ApresentacaoBO(ado, canalId: SessionModel.CanalID).Consultar(reserva.apresentacaoID);
                if (apresentacao == null)
                {
                    stopwatch.Stop();
                    LogUtil.Info(string.Format("##Post.AddIngressoQTD.ERROR## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, "apresentacao == null", stopwatch.Elapsed));

                    GerarErroHttp(retorno, new Exception("A apresentação não existe."), (HttpStatusCode)422, false, null);
                }


                var canalTipoPOS = ConfiguracaoAppUtil.GetAsInt("CanalTipoPOS");
                var isPOS = new CanalBO(ado).isPOS(SessionModel.CanalID, canalTipoPOS);

                // Consulta o evento.
                var evento = new EventoBO(ado, canalId: SessionModel.CanalID).Consultar(apresentacao.EventoID, isPOS);
                if (evento == null)
                {
                    stopwatch.Stop();
                    LogUtil.Info(string.Format("##Post.AddIngressoQTD.ERROR## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, "evento == null", stopwatch.Elapsed));
                    GerarErroHttp(retorno, new Exception("O evento não existe."), (HttpStatusCode)422, false, null);
                }

                if (reserva.setorID > 0 && reserva.apresentacaoID > 0 && reserva.precos.Count > 0)
                {
                    LogUtil.Info(string.Format("##Post.AddIngressoQTD.Dados## SESSION {0}, SETOR {1}, APR {2}, PREÇO {3}, QTD {4}", this.SessionModel.SessionID, reserva.setorID, reserva.apresentacaoID, reserva.precos[0].precoID, reserva.precos[0].quantidade));

                    try
                    {
                        var precosQuantidade = reserva.precos.Select(t => new ReservaQuantidadeModel() { PrecoID = t.precoID, Quantidade = t.quantidade }).ToList();

                        // Solicita a reserva no banco de dados, na entidade tIngresso.
                        retorno = bo.carrinhoBO.SolicitarReservaQuantidade(SessionModel.Compra, reserva.setorID, reserva.apresentacaoID, precosQuantidade, evento.EntradaFranca, isPOS);

                        var eventosApresentacoesCarrinho = retorno.Retorno.CarrinhoItens.Where(w => !carrinhoItens.Contains(w)).Select(s => new { s.EventoID, s.ApresentacaoID }).Distinct().ToList();

                        var restricoesIngressos = RestricoesQuantidadeIngressosEvento(ado, eventosApresentacoesCarrinho);

                        if (!restricoesIngressos.Sucesso)
                        {
                            foreach (var preco in reserva.precos)
                            {
                                var carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.PrecoID == preco.precoID).ToList();

                                foreach (var carrinho in carrinhos)
                                {
                                    retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                }
                            }

                            LogUtil.Error(string.Format("##Post.AddIngressoQTD.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, restricoesIngressos.Mensagem));

                            throw new CompraException(restricoesIngressos.Mensagem);
                        }

                        // Se o evento for entrada franca, é necessário realizar validação de quantidade de ingressos.
                        if (evento.EntradaFranca)
                        {
                            try
                            {
                                new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(evento.IR_EventoID, SessionModel.ClienteID, apresentacao.IR_ApresentacaoID, SessionModel.SessionID);
                            }
                            catch (InvalidOperationException erro)
                            {
                                foreach (var preco in reserva.precos)
                                {
                                    List<Carrinho> carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.PrecoID == preco.precoID).ToList();
                                    foreach (var carrinho in carrinhos)
                                    {
                                        if (carrinho != null)
                                        {
                                            retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                        }
                                    }
                                }
                                stopwatch.Stop();
                                LogUtil.Error(string.Format("##Post.AddIngressoQTD.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, erro.Message, stopwatch.Elapsed), erro);
                                throw new CompraException(erro.Message);
                            }
                        }

                        // Restrições Tim Burton
                        int eventoTimBurtonID = Convert.ToInt32(ConfigurationManager.AppSettings["EventoTimBurton"]);
                        if (evento.IR_EventoID == eventoTimBurtonID)
                        {
                            int limite = Convert.ToInt32((Convert.ToInt32(ConfigurationManager.AppSettings["EventoTimBurtonPrimeiraApresentacao"]) == reserva.apresentacaoID ?
                                ConfigurationManager.AppSettings["EventoTimBurtonPrimeiraApresentacaoLimite"] : ConfigurationManager.AppSettings["EventoTimBurtonLimite"]));
                            try
                            {
                                new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(evento.IR_EventoID, SessionModel.ClienteID, apresentacao.IR_ApresentacaoID, SessionModel.SessionID, Int32.MaxValue, limite, Int32.MaxValue);
                            }
                            catch (InvalidOperationException erro)
                            {
                                foreach (var preco in reserva.precos)
                                {
                                    List<Carrinho> carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.PrecoID == preco.precoID).ToList();
                                    foreach (var carrinho in carrinhos)
                                    {
                                        if (carrinho != null)
                                        {
                                            retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                        }
                                    }
                                }
                                stopwatch.Stop();
                                LogUtil.Error(string.Format("##Post.AddIngressoQTD.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, erro.Message, stopwatch.Elapsed), erro);
                                throw new CompraException(erro.Message);
                            }
                        }

                        SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);

                        carrinhoItens = SessionModel.Compra.CarrinhoItens.Where(w => !carrinhoItens.Contains(w)).ToList();

                        if (isPOS && SessionModel.Compra.CarrinhoItens.Any(a => a.CotaItemObject != null))
                        {
                            stopwatch.Stop();
                            LogUtil.Error(string.Format("##Post.AddIngressoQTD.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, "Eventos com cota estão desabilitados para venda via POS", stopwatch.Elapsed));
                            throw new Exception("Eventos com cota estão desabilitados para venda via POS.");
                        }

                        var validacaoLimiteQuantidadeCotas = bo.cotaBO.VerificarLimite(retorno.Retorno, carrinhoItens);

                        if (!validacaoLimiteQuantidadeCotas.Sucesso)
                        {
                            foreach (var preco in reserva.precos)
                            {
                                List<Carrinho> carrinhos = SessionModel.Compra.CarrinhoItens.Where(c => c.PrecoID == preco.precoID).ToList();
                                foreach (var carrinho in carrinhos)
                                {
                                    if (carrinho != null)
                                    {
                                        retorno = bo.carrinhoBO.RemoverReserva(SessionModel.Compra, carrinho.ID);
                                    }
                                }
                            }
                            retorno.Sucesso = validacaoLimiteQuantidadeCotas.Sucesso;
                            retorno.Mensagem = validacaoLimiteQuantidadeCotas.Mensagem;
                            LogUtil.Info(string.Format("##Post.AddIngressoQTD.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, validacaoLimiteQuantidadeCotas.Mensagem));
                        }
                    }
                    catch (CompraException erro)
                    {
                        stopwatch.Stop();
                        LogUtil.Error(string.Format("##Post.AddIngressoQTD.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, erro.Message, stopwatch.Elapsed), erro);
                        GerarErroHttp(retorno, erro, (HttpStatusCode)422, false, null);
                    }
                    catch (Exception ex)
                    {
                        stopwatch.Stop();
                        LogUtil.Error(string.Format("##Post.AddIngressoQTD.EXCEPTION## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, ex.Message, stopwatch.Elapsed), ex);
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        //LogUtil.Error(ex);
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (!retorno.Sucesso)
                    {
                        stopwatch.Stop();
                        LogUtil.Info(string.Format("##Post.AddIngressoQTD.ERROR## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, "Sucesso = false", stopwatch.Elapsed));
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }

                else
                {
                    stopwatch.Stop();
                    LogUtil.Info(string.Format("##Post.AddIngressoQTD.ERROR## SESSION {0}, MSG {1}, TEMPO DECORRIDO {2}", this.SessionModel.SessionID, "Parâmetros não informados", stopwatch.Elapsed));

                    retorno.Mensagem = "Parametro faltando ou mal formatado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
            }

            stopwatch.Stop();

            LogUtil.Info(string.Format("##Post.AddIngressoQTD.SUCCESS## SESSION {0}, {1}", this.SessionModel.SessionID, stopwatch.Elapsed));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Insere itens no rq
        /// </summary>
        /// <param name="reserva"></param>
        /// <returns></returns>
        [Route("reservas/atual/ingressos/qtd2")]
        [HttpPost]
        public RetornoModel<List<ReservaQuantidadeRetorno>> ReservaIngressoPorQuantidade2([FromBody]ReservaIngressosQtdRquestModel reserva)
        {
            LogUtil.Info(string.Format("##Post.AddIngressoQTD_2## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<List<ReservaQuantidadeRetorno>> retorno = new RetornoModel<List<ReservaQuantidadeRetorno>>();
            using (var bo = new CarrinhoBO())
            {
                if (reserva != null && reserva.setorID > 0 && reserva.apresentacaoID > 0 && reserva.precos.Count > 0)
                {
                    LogUtil.Info(string.Format("##Post.AddIngressoQTD_2.Dados## SESSION {0}, SETOR {1}, APR {2}, PREÇO {3}, QTD {4}", this.SessionModel.SessionID, reserva.setorID, reserva.apresentacaoID, reserva.precos[0].precoID, reserva.precos[0].quantidade));

                    if (reserva.limparReservaAnterior)
                    {
                        LimparCompra(bo);
                    }
                    try
                    {
                        List<ReservaQuantidadeModel> precosQuantidade = reserva.precos.Select(t => new ReservaQuantidadeModel() { PrecoID = t.precoID, Quantidade = t.quantidade }).ToList();
                        retorno = bo.ReservarQuantidade(reserva.setorID, reserva.apresentacaoID, precosQuantidade, SessionModel.SessionID, SessionModel.ClienteID, SessionModel.LojaID, SessionModel.CanalID, SessionModel.UsuarioID, false);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Post.AddIngressoQTD_2.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        //LogUtil.Error(ex);
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (!retorno.Sucesso)
                    {
                        LogUtil.Info(string.Format("##Post.AddIngressoQTD_2.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    LogUtil.Info(string.Format("##Post.AddIngressoQTD_2.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

                    retorno.Mensagem = "Parametro faltando ou mal formatado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
            }

            LogUtil.Info(string.Format("##Post.AddIngressoQTD_2.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Adiciona log de pagamento em caso de erro no momento de efetuar a compra.
        /// </summary>
        /// <param name="pagamentos">Coleção de meios de pagamento do usuário.</param>
        /// <param name="erro">Erro ao validar o processo de compra.</param>
        protected void AdicionarLogPagamento(List<CompraPagamentoModel> pagamentos, CompraException erro)
        {
            // TODO: Funções auxiliares, utilizadas apenas neste método, caso seja necessário um reaproveitamento, mover para um pacote de extensions.
            Func<string, string> ifNullOrEmptyUseEmpty = delegate(string value)
            {
                return !string.IsNullOrEmpty(value) ? value.Replace("'", "") : "N/A";
            };

            // TODO: Funções auxiliares, utilizadas apenas neste método, caso seja necessário um reaproveitamento, mover para um pacote de extensions.
            Func<string, string> maskCreditCard = delegate(string creditCardNumber)
            {
                if (string.IsNullOrEmpty(creditCardNumber) || creditCardNumber.Length < 4)
                {
                    return "N/A";
                }

                return string.Concat("".PadLeft(creditCardNumber.Length - 4, '*'), creditCardNumber.Substring(creditCardNumber.Length - 4));
            };

            // Define a template para coletar dados adicionais, se existirem.
            var dadosAdicionais = "CreditCardName: {0}, NumeroCartao: {1}, ValidadeCartao: {2}, ShopperEmail: {3}, ShopperIP: {4}, Reference: {5}, SessionId: {6}, Valor: {7}";
            var nomeCartao = string.Empty;
            var numeroCartao = string.Empty;
            var validadeCartao = string.Empty;
            var shopperEmail = string.Empty;
            var shopperIp = string.Empty;
            var reference = string.Empty;
            var valor = string.Empty;

            // Adicionado try catch em caso de erro nos objetos de sessao.
            try
            {
                // Recupera dados de pagamento, caso exista.
                if (pagamentos != null && pagamentos.Any())
                {
                    var pagamento = pagamentos.First();
                    nomeCartao = ifNullOrEmptyUseEmpty(pagamento.NomeCartao);
                    numeroCartao = maskCreditCard(pagamento.NumeroCartao);
                    validadeCartao = string.Format("{0}/{1}", ifNullOrEmptyUseEmpty(pagamento.MesValidade.ToString()), ifNullOrEmptyUseEmpty(pagamento.AnoValidade.ToString()));
                }

                // Recupera o valor da compra.
                if (SessionModel.Compra != null && SessionModel.Compra.Total != null)
                {
                    valor = SessionModel.Compra.Total.ValorTotal.ToString();
                }

                // Recupera dados do usuário.
                if (SessionModel.Login != null)
                {
                    shopperEmail = ifNullOrEmptyUseEmpty(SessionModel.Login.Email);
                }

                // Substitui os dados adicionais na template.
                dadosAdicionais = string.Format(dadosAdicionais, nomeCartao, numeroCartao, validadeCartao, shopperEmail, shopperIp, reference, SessionModel.SessionID.ToString(), valor);

                // Insere log de pagamento.
                new LogPagamentoBO().Adicionar(ifNullOrEmptyUseEmpty(erro.Message), ifNullOrEmptyUseEmpty(erro.Source), ifNullOrEmptyUseEmpty(erro.StackTrace), ifNullOrEmptyUseEmpty(erro.Message), dadosAdicionais);
            }
            catch (Exception exception)
            {
                LogUtil.Error(exception);
            }
        }

        /// <summary>
        /// Gera erro HTTP para o modelo compra model.
        /// </summary>
        /// <param name="compra">Modelo de retorno.</param>
        /// <param name="erro">Exception com detalhes do erro.</param>
        /// <param name="codigoHttp">Erro HTTP.</param>
        /// <param name="gravarLogPagamento">Indicativo para gravação de log de pagamento.</param>
        /// <param name="pagamentos">Informações de pagamento</param>
        protected virtual void GerarErroHttp(RetornoModel<CompraModel> compra, Exception erro, HttpStatusCode codigoHttp, bool gravarLogPagamento, List<CompraPagamentoModel> pagamentos)
        {
            if (gravarLogPagamento)
            {
                AdicionarLogPagamento(pagamentos, new CompraException(erro.Message));
            }

            NewRelicIgnoreTransaction();
            compra.Mensagem = erro.Message;
            compra.Sucesso = false;
            throw new HttpResponseException(Request.CreateResponse(codigoHttp, ParseRetorno(compra)));
        }

        /// <summary>
        /// Valida a compra no momento do pagamento.
        /// </summary>
        /// <param name="pagamentos">Informações de pagamento.</param>
        /// <param name="retorno">Modelo de retorno.</param>
        /// <param name="ado">Contexto de banco.</param>
        /// <returns>Detalhes do evento.</returns>
        private Evento RecuperaEventoComValidacaoFinalizarCompra(List<CompraPagamentoModel> pagamentos, RetornoModel<CompraModel> retorno, MasterADOBase ado)
        {
            var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
            if (!bo.carrinhoBO.ValidarCarrinho(SessionModel.Compra.CarrinhoItens))
            {
                throw new CompraException("Algumas das apresentações não está mais disponível, refaça sua compra.");
            }

            if (SessionModel.Compra == null)
            {
                GerarErroHttp(retorno, new CompraException("Nenhuma compra encontrada"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            if (SessionModel.EntregaControleID <= 0)
            {
                GerarErroHttp(retorno, new CompraException("Nenhuma entrega encontrada"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            if (SessionModel.Compra.CarrinhoItens == null || !SessionModel.Compra.CarrinhoItens.Any())
            {
                GerarErroHttp(retorno, new CompraException("Nenhum item encontrado no carrinho"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            if (SessionModel.Compra.StatusCotaPendente != null && (SessionModel.Compra.StatusCotaPendente.Nominal || SessionModel.Compra.StatusCotaPendente.Promocional))
            {
                GerarErroHttp(retorno, new CompraException("Não foi possível validar a compra. Faça o fluxo de compra novamente"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            if (SessionModel.Compra.EntregaControles == null || SessionModel.Compra.EntregaControles.All(x => x.ID != SessionModel.Compra.EntregaControleID))
            {
                GerarErroHttp(retorno, new CompraException("A entrega selecionada não é válida para esta compra"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            var evento = new EventoBO(ado, canalId: SessionModel.CanalID).Consultar(SessionModel.Compra.CarrinhoItens.First().EventoID.Value);
            if (evento == null)
            {
                GerarErroHttp(retorno, new CompraException("O evento não existe"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            if (SessionModel.Compra.Total.ValorTotal > 0 && (pagamentos == null || pagamentos.Count == 0))
            {
                GerarErroHttp(retorno, new CompraException("Nenhuma forma de pagamento informada"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            if (SessionModel.Compra.Total.ValorTotal > 0 && (pagamentos != null && pagamentos.Where(w => w.formaPagamento != null && w.formaPagamento.NomeAsEnum != enumFormaPagamento.PayPal).Any(pagamento => pagamento.NumeroCartao == null || pagamento.NumeroCartao.Length < 6)))
            {
                GerarErroHttp(retorno, new CompraException("Número do cartão inválido"), HttpStatusCode.BadRequest, true, pagamentos);
            }

            return evento;
        }

        /// <summary>
        /// Valida a compra externa no momento do pagamento.
        /// </summary>
        /// <param name="retorno">Modelo de retorno.</param>
        /// <param name="ado">Contexto de banco.</param>
        /// <returns>Detalhes do evento.</returns>
        private Evento RecuperaEventoComValidacaoFinalizarCompraExterna(RetornoModel<CompraModel> retorno, MasterADOBase ado)
        {
            if (SessionModel.Compra == null)
            {
                GerarErroHttp(retorno, new CompraException("Nenhuma compra encontrada"), HttpStatusCode.BadRequest, false, null);
            }

            if (SessionModel.Compra.CarrinhoItens == null || !SessionModel.Compra.CarrinhoItens.Any())
            {
                GerarErroHttp(retorno, new CompraException("Nenhum item encontrado no carrinho"), HttpStatusCode.BadRequest, false, null);
            }

            if (SessionModel.Compra.StatusCotaPendente != null && (SessionModel.Compra.StatusCotaPendente.Nominal || SessionModel.Compra.StatusCotaPendente.Promocional))
            {
                GerarErroHttp(retorno, new CompraException("Não foi possível validar a compra. Faça o fluxo de compra novamente"), HttpStatusCode.BadRequest, false, null);
            }

            var evento = new EventoBO(ado, canalId: SessionModel.CanalID).Consultar(SessionModel.Compra.CarrinhoItens.First().EventoID.Value, true);
            if (evento == null)
            {
                GerarErroHttp(retorno, new CompraException("O evento não existe"), HttpStatusCode.BadRequest, false, null);
            }

            return evento;
        }

        private InfosObrigatoriasIngresso InformacoesObrigatoriasIngresso(int eventoId, MasterADOBase ado)
        {
            var apresentacao = new ApresentacaoBO(ado).ListarInfosObrigatoriasIngresso(eventoId);
            var evento = new EventoBO(ado).ListarInfosObrigatoriasIngresso(eventoId);
            var local = new LocalBO(ado).ListarInfosObrigatoriasIngresso(evento.localId ?? 0);

            var retorno = new InfosObrigatoriasIngresso
            {
                alvara = !string.IsNullOrEmpty(apresentacao.alvara) ? apresentacao.alvara : !string.IsNullOrEmpty(evento.alvara) ? evento.alvara : !string.IsNullOrEmpty(local.alvara) ? local.alvara : string.Empty,
                AVCB = !string.IsNullOrEmpty(apresentacao.AVCB) ? apresentacao.AVCB : !string.IsNullOrEmpty(evento.AVCB) ? evento.AVCB : !string.IsNullOrEmpty(local.AVCB) ? local.AVCB : string.Empty,
                fonteImposto = !string.IsNullOrEmpty(apresentacao.fonteImposto) ? apresentacao.fonteImposto : !string.IsNullOrEmpty(evento.fonteImposto) ? evento.fonteImposto : !string.IsNullOrEmpty(local.fonteImposto) ? local.fonteImposto : string.Empty,
                porcentagemImposto = !string.IsNullOrEmpty(apresentacao.porcentagemImposto) ? apresentacao.porcentagemImposto : !string.IsNullOrEmpty(evento.porcentagemImposto) ? evento.porcentagemImposto : !string.IsNullOrEmpty(local.porcentagemImposto) ? local.porcentagemImposto : string.Empty,
                dataEmissaoAlvara = !string.IsNullOrEmpty(apresentacao.dataEmissaoAlvara) ? apresentacao.dataEmissaoAlvara : !string.IsNullOrEmpty(evento.dataEmissaoAlvara) ? evento.dataEmissaoAlvara : !string.IsNullOrEmpty(local.dataEmissaoAlvara) ? local.dataEmissaoAlvara : string.Empty,
                dataValidadeAlvara = !string.IsNullOrEmpty(apresentacao.dataValidadeAlvara) ? apresentacao.dataValidadeAlvara : !string.IsNullOrEmpty(evento.dataValidadeAlvara) ? evento.dataValidadeAlvara : !string.IsNullOrEmpty(local.dataValidadeAlvara) ? local.dataValidadeAlvara : string.Empty,
                dataEmissaoAvcb = !string.IsNullOrEmpty(apresentacao.dataEmissaoAvcb) ? apresentacao.dataEmissaoAvcb : !string.IsNullOrEmpty(evento.dataEmissaoAvcb) ? evento.dataEmissaoAvcb : !string.IsNullOrEmpty(local.dataEmissaoAvcb) ? local.dataEmissaoAvcb : string.Empty,
                dataValidadeAvcb = !string.IsNullOrEmpty(apresentacao.dataValidadeAvcb) ? apresentacao.dataValidadeAvcb : !string.IsNullOrEmpty(evento.dataValidadeAvcb) ? evento.dataValidadeAvcb : !string.IsNullOrEmpty(local.dataValidadeAvcb) ? local.dataValidadeAvcb : string.Empty,
                lotacao = !string.IsNullOrEmpty(apresentacao.lotacao) ? apresentacao.lotacao : !string.IsNullOrEmpty(evento.lotacao) ? evento.lotacao : !string.IsNullOrEmpty(local.lotacao) ? local.lotacao : string.Empty,
                produtorRazaoSocial = !string.IsNullOrEmpty(evento.produtorRazaoSocial) ? evento.produtorRazaoSocial : string.Empty,
                produtorCpfCnpj = !string.IsNullOrEmpty(evento.produtorCpfCnpj) ? evento.produtorCpfCnpj : string.Empty,
                atencao = !string.IsNullOrEmpty(evento.atencao) ? evento.atencao : string.Empty,
            };

            return retorno;
        }

        private RetornoModel RestricoesQuantidadeIngressosEvento(MasterADOBase ado, dynamic eventosApresentacoesCarrinho)
        {
            var retorno = new RetornoModel { Sucesso = true, Mensagem = "OK" };

            var ativarRestricoesIngressosEvento = Convert.ToBoolean(ConfigurationManager.AppSettings["AtivarRestricoesQuantidadeIngressosEvento"]);

            if (ativarRestricoesIngressosEvento)
            {
                var eventosApresentacoesRestricao = ConfigurationManager.AppSettings["RestricoesQuantidadeIngressosEvento"].Split('|');

                foreach (var eventoApresentacaoCarrinho in eventosApresentacoesCarrinho)
                {
                    foreach (var configEventoApresentacaoRestricao in eventosApresentacoesRestricao.Select(eventoApresentacaoRestricao => eventoApresentacaoRestricao.Split(';').Select(t => t.Split(':')).ToArray()))
                    {
                        try
                        {
                            var eventoID = eventoApresentacaoCarrinho.EventoID ?? 0;
                            var apresentacaoID = eventoApresentacaoCarrinho.ApresentacaoID ?? 0;

                            var eventoIDRestricao = Convert.ToInt32(configEventoApresentacaoRestricao[0][0]);
                            var eventoQuantidadeRestricao = Convert.ToInt32(configEventoApresentacaoRestricao[0][1]) > 0 ? Convert.ToInt32(configEventoApresentacaoRestricao[0][1]) : Int32.MaxValue;
                            var textoTermoValidacao = configEventoApresentacaoRestricao[0].Length == 3 && !string.IsNullOrEmpty(configEventoApresentacaoRestricao[0][2]) ? configEventoApresentacaoRestricao[0][2] : "ingressos";

                            if (eventoID != eventoIDRestricao) continue;

                            var apresentacoesRestricao = configEventoApresentacaoRestricao.Where(w => w != configEventoApresentacaoRestricao[0]).ToArray();

                            if (apresentacoesRestricao.Length > 0)
                            {
                                foreach (var apresentacaoRestricao in apresentacoesRestricao)
                                {
                                    var apresentacaoIDRestricao = Convert.ToInt32(apresentacaoRestricao[0]);
                                    var apresentacaoQuantidadeRestricao = Convert.ToInt32(apresentacaoRestricao[1]);
                                    if (apresentacaoID == apresentacaoIDRestricao)
                                    {
                                        new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(eventoID, SessionModel.ClienteID, apresentacaoID, SessionModel.SessionID, eventoQuantidadeRestricao, apresentacaoQuantidadeRestricao, Int32.MaxValue, textoTermoValidacao);
                                    }
                                }
                            }
                            else
                            {
                                new IngressoBO(ado).ConsultarTotalIngressoPorClienteApresentacao(eventoID, SessionModel.ClienteID, apresentacaoID, SessionModel.SessionID, eventoQuantidadeRestricao, Int32.MaxValue, Int32.MaxValue, textoTermoValidacao);
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            retorno.Sucesso = false;
                            retorno.Mensagem = ex.Message;
                            return retorno;
                        }
                    }
                }
            }

            return retorno;
        }
    }
}