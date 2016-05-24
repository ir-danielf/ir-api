using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IRAPI.Controllers
{
    [IRAPIAuthorize(enumAPIRele.osesp)]
    public class OsespController : MasterApiController
    {

        /// <summary>
        /// Método da API que busca eventos com base nos filtros e retorna com base no numero da pagina e eventos por página. 
        /// URL de acesso: eventos
        /// Método de acesso: GET
        /// </summary>
        /// <param name="busca">String a ser buscada</param>
        /// <param name="localid">ID do local do evento</param>
        /// <param name="uf">Estado do evento</param>
        /// <param name="cidade">Cidade do evento</param>
        /// <param name="tipoid">ID do tipo de evento</param>
        /// <param name="subtipoID">ID do subtipo de evento</param>
        /// <param name="pg">Número da página</param>
        /// <param name="qtdpg">Quantidade eventos por página</param>
        [Route("osesp/eventos")]
        [HttpGet]
        public RetornoModel<List<Evento>> Get()
        {
            RetornoModel<List<Evento>> retorno = new RetornoModel<List<Evento>>();
            try
            {
                using (var eventoBO = new EventoBO())
                {
                    var result = eventoBO.ListarOSESP();
                    if (result.Count > 0)
                    {
                        retorno.Retorno = result.ToList();
                    }
                    retorno.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (retorno.Retorno == null || retorno.Retorno == null || retorno.Retorno.Count == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum evento encontrado";
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
            }
            return ParseRetorno(retorno);
        }


        /// <summary>
        /// Método da API que autentica o Cliente com base no username e password informado e verifica se este cliente tem assinatura no OSESP. 
        /// URL de acesso: clientes/auth
        /// Método de acesso: POST
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso,objeto Login e mensagem informando o erro, quando houver</returns>
        [Route("osesp/clientes/auth")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostAuth([FromBody]ClienteAuthRequestModel obj)
        {
            RetornoModel<Login, enumClienteException> retorno;

            if (obj != null && obj.username != null && obj.password != null)
            {
                using (ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId()))
                {
                    try
                    {
                        retorno = clienteBO.LogarOsesp(obj.username, obj.password);
                    }
                    catch (Exception ex)
                    {
                        retorno = new RetornoModel<Login, enumClienteException>();
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        LogUtil.Error(ex);
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (!retorno.Sucesso)
                    {
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                    else
                    {

                        try
                        {

                            // Guarda o cliente logado na sessao
                            SessionModel.ClienteID = retorno.Retorno.ClienteID;
                            SessionModel.SiteID = retorno.Retorno.SiteID;

                            return ParseRetorno(retorno);
                        }
                        catch (Exception ex)
                        {
                            retorno = new RetornoModel<Login, enumClienteException>();
                            retorno.Mensagem = ex.Message;
                            retorno.Sucesso = false;
                            LogUtil.Error(ex);
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                        }
                    }
                }
            }
            else
            {
                retorno = new RetornoModel<Login, enumClienteException>();
                retorno.Mensagem = "Objeto Auth não encontrado ou mal formatado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }
        }

        [Route("osesp/clientes/auth")]
        [HttpDelete]
        public RetornoModel DeleteAuth()
        {
            SessionModel.ClienteID = 0;
            SessionModel.EntregaControleID = 0;
            SessionModel.ClienteEnderecoID = 0;
            SessionModel.PDVID = 0;

            return ParseRetorno(new RetornoModel()
            {
                Sucesso = true,
                Mensagem = "OK"
            });
        }


        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e uma lista de setores com base no ID do evento e da apresentação.
        /// URL de acesso: eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores
        /// Método de acesso: GET
        /// </summary>
        /// <param name="eventoID">ID do evento</param>
        /// <param name="apresentacaoID">ID da apresentação</param>
        [Route("osesp/eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores")]
        [HttpGet]
        public RetornoModel<List<Setor>> GetSetores(int eventoID, int apresentacaoID, [FromUri]bool comCotaNominal = true, [FromUri]bool comCotaPromocional = true)
        {
            RetornoModel<List<Setor>> retorno = new RetornoModel<List<Setor>>();
            List<Setor> resultado = null;
            try
            {
                using (var setorBO = new SetorBO())
                {
                    resultado = setorBO.ListarOSESP(apresentacaoID, comCotaNominal, comCotaPromocional);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            retorno.Retorno = resultado;
            retorno.Sucesso = true;
            if (resultado == null || resultado.Count == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum setor encontrado par a apresentação ou apresentação não encontrada";
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
            }
            return ParseRetorno(retorno);
        }

        [Route("osesp/eventos/{eventoID}/apresentacoes/{apresentacaoID}/setores/{setorID}/mapa_lugares")]
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
                    ingressos = eventoBO.ListarMapaObjectOSESP(setorID, apresentacaoID);
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
        /// Método da API que retorna o status da operação, uma mensagem e um evento com base no ID informado.
        /// URL de acesso: eventos/{ID}
        /// Método de acesso: GET
        /// </summary>
        /// <param name="id">Id do evento</param>
        [Route("osesp/eventos/{id}")]
        [HttpGet]
        public RetornoModel<Evento> GetEvento(int id)
        {
            RetornoModel<Evento> retorno = new RetornoModel<Evento>();
            try
            {
                using (var ado = new MasterADOBase())
                {
                    EventoBO eventoBO = new EventoBO(ado);
                    var result = eventoBO.ListarOSESP().Where(x => x.IR_EventoID == id).ToList();

                    if (result.Count > 0)
                    {
                        retorno.Retorno = eventoBO.ConsultarOSESP(id);
                    }
                    else
                    {
                        retorno.Sucesso = false;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Sucesso = (retorno.Retorno != null);

            if (!retorno.Sucesso)
            {
                retorno.Retorno = null;
                retorno.Mensagem = "Evento não encontrado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            retorno.Mensagem = "OK";
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método que bloqueia um ingresso
        /// URL de acesso: osesp/ingressos_bloqueados
        /// Método de acesso: POST
        /// </summary>
        /// <param name="id">Id do Ingresso</param>
        [Route("osesp/ingressos_bloqueados")]
        [HttpPost]
        public RetornoModel<RetornoOsespModel> PostBloquearIngresso([FromBody]OsespIngresso req)
        {
            RetornoModel<RetornoOsespModel> retorno = new RetornoModel<RetornoOsespModel>();

            if (req.ingressoID == 0)
            {
                retorno.Mensagem = "Ingresso ID obrigatório";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }

            int UsuarioOSESPID = Convert.ToInt32(ConfigurationManager.AppSettings["UsuarioOSESPID"]);
            int PluID = Convert.ToInt32(ConfigurationManager.AppSettings["OSESPID_PLU"]);
            int PluUtilizadoID = Convert.ToInt32(ConfigurationManager.AppSettings["OSESPID_PLU_UTILIZADO"]);
            int BloqueioPadrao = Convert.ToInt32(ConfigurationManager.AppSettings["BLOQUEIO_PADRAO"]);

            try
            {
                using (var ado = new MasterADOBase())
                {
                    EventoBO eventoBO = new EventoBO(ado);
                    IngressoBO ingressoBO = new IngressoBO(ado);
                    retorno.Sucesso = ingressoBO.BloquearIngressoOSESP(req.ingressoID, UsuarioOSESPID, PluID, PluUtilizadoID, BloqueioPadrao);

                    if (retorno.Sucesso)
                    {
                        tIngresso ingresso = ingressoBO.BuscaRetornoOsesp(req.ingressoID);
                        

                        RetornoOsespModel rom = new RetornoOsespModel();
                        rom.BloqueioID = Convert.ToInt32(ingresso.BloqueioID);
                        rom.Status = "Bloqueado";
                        rom.BloqueioNome = ingressoBO.BuscaNomeBloqueio(Convert.ToInt32(ingresso.BloqueioID));

                        retorno.Retorno = rom;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            if (!retorno.Sucesso)
            {
                retorno.Mensagem = "Ingresso não encontrado ou não disponível para bloqueio";
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
                return ParseRetorno(retorno);
            }
        }

        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e um evento com base no ID informado.
        /// URL de acesso: osesp/ingressos_bloqueados
        /// Método de acesso: GET
        /// </summary>
        /// <param name="id">Id do evento</param>
        [Route("osesp/ingressos_bloqueados/{ingressoID}")]
        [HttpDelete]
        public RetornoModel<RetornoOsespModel> DeleteDesbloquearIngresso(int ingressoID)
        {
            RetornoModel<RetornoOsespModel> retorno = new RetornoModel<RetornoOsespModel>();

            int UsuarioOSESPID = Convert.ToInt32(ConfigurationManager.AppSettings["UsuarioOSESPID"]);
            int PluID = Convert.ToInt32(ConfigurationManager.AppSettings["OSESPID_PLU"]);
            int PluUtilizadoID = Convert.ToInt32(ConfigurationManager.AppSettings["OSESPID_PLU_UTILIZADO"]);
            int BloqueioPadrao = Convert.ToInt32(ConfigurationManager.AppSettings["BLOQUEIO_PADRAO"]);

            try
            {
                using (var ado = new MasterADOBase())
                {
                    IngressoBO ingressoBO = new IngressoBO(ado);
                    retorno.Sucesso = ingressoBO.DesbloquearIngressoOSESP(ingressoID, UsuarioOSESPID, PluID, PluUtilizadoID, BloqueioPadrao);

                    if (retorno.Sucesso)
                    {
                        RetornoOsespModel rom = new RetornoOsespModel();
                        tIngresso ingresso = ingressoBO.BuscaRetornoOsesp(ingressoID);

                        switch (ingresso.Status)
                        {
                            case "B":
                                rom.Status = "Bloqueado";
                                break;
                            case "D":
                                rom.Status = "Desbloqueado";
                                break;
                            default:
                                rom.Status = ingresso.Status;
                                break;
                        }
                                                
                        rom.BloqueioID = Convert.ToInt32(ingresso.BloqueioID);
                        rom.BloqueioNome = ingressoBO.BuscaNomeBloqueio(Convert.ToInt32(ingresso.BloqueioID));

                        retorno.Retorno = rom;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                retorno.Mensagem = "Ingresso não encontrado ou não disponível para desbloqueio";
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
                return ParseRetorno(retorno);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("osesp/clientes/logado")]
        [HttpPut]
        public RetornoModel<Login, enumClienteException> PutLogado([FromBody]Login obj)
        {
            RetornoModel<Login, enumClienteException> retorno;
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId());
                CarregarClienteOSESP(clienteBO, false);
                if (obj != null)
                {
                    if (SessionModel.Login == null)
                    {
                        retorno = new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não existe um cliente logado", Retorno = null, Tipo = enumClienteException.usuarioNaoEncontrado };
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                    Login login = SessionModel.Login;
                    login.Email = obj.Email;
                    login.FaceBookUserID = login.FaceBookUserID;
                    login.FaceBookUserToken = login.FaceBookUserToken;
                    login.FaceBookUserInfos = login.FaceBookUserInfos;
                    login.CPF = obj.CPF.Replace(new string[] { ".", "-" }, "");

                    if (login.Cliente==null)
                        login.Cliente = new tCliente();

                    if (!string.IsNullOrEmpty(obj.Cliente.Senha))
                        login.Cliente.Senha = obj.Cliente.Senha;
                    login.Cliente.RG = obj.Cliente.RG;
                    login.Cliente.ContatoTipoID = obj.Cliente.ContatoTipoID;
                    login.Cliente.Pais = obj.Cliente.Pais;
                    login.Cliente.Nome = obj.Cliente.Nome;
                    login.Cliente.DDDCelular = obj.Cliente.DDDCelular;
                    login.Cliente.Celular = obj.Cliente.Celular;
                    login.Cliente.DDDTelefone = obj.Cliente.DDDTelefone;
                    login.Cliente.Telefone = obj.Cliente.Telefone;
                    login.Cliente.DDDTelefoneComercial = obj.Cliente.DDDTelefoneComercial;
                    login.Cliente.TelefoneComercial = obj.Cliente.DDDTelefoneComercial;
                    login.Cliente.RecebeEmailAsBool = obj.Cliente.RecebeEmailAsBool;
                    login.Cliente.DataNascimento = obj.Cliente.DataNascimento;
                    login.Cliente.Sexo = obj.Cliente.Sexo;
                    login.Cliente.CidadeCliente = obj.Cliente.CidadeCliente;
                    login.Cliente.EstadoCliente = obj.Cliente.EstadoCliente;
                    login.Cliente.EnderecoCliente = obj.Cliente.EnderecoCliente;
                    login.Cliente.NumeroCliente = obj.Cliente.NumeroCliente;
                    if (!string.IsNullOrEmpty(obj.Cliente.CEPCliente))
                        login.Cliente.CEPCliente = obj.Cliente.CEPCliente.Replace("-", "");
                    login.Cliente.ComplementoCliente = obj.Cliente.ComplementoCliente;
                    login.Cliente.BairroCliente = obj.Cliente.BairroCliente;

                    login.Cliente.SituacaoProfissionalID = obj.Cliente.SituacaoProfissionalID;
                    login.Cliente.Profissao = obj.Cliente.Profissao;
                    login.Cliente.Email = obj.Cliente.Email;
                    login.Cliente.Senha = obj.Cliente.Senha;

                    bool salvarConcluido = false;
                    try
                    {
                        salvarConcluido = clienteBO.Salvar(login, SessionModel.UsuarioID);
                    }
                    catch (Exception ex)
                    {
                        retorno = new RetornoModel<Login, enumClienteException>();
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        LogUtil.Error(ex);
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (salvarConcluido)
                    {
                        retorno = new RetornoModel<Login, enumClienteException>();
                        retorno.Mensagem = "OK";
                        retorno.Retorno = login;
                        retorno.Sucesso = true;
                    }
                    else
                    {
                        retorno = new RetornoModel<Login, enumClienteException>();
                        retorno.Mensagem = "Não foi possível completar a solicitação";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                    retorno.Sucesso = salvarConcluido;
                }
                else
                {
                    retorno = new RetornoModel<Login, enumClienteException>();
                    retorno.Mensagem = "Objeto Login não encontrado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna o usuário logado se ele existir
        /// URL de acesso: osesp/clientes/logado
        /// Método de acesso: GET
        /// </summary>
        [Route("osesp/clientes/logado")]
        [HttpGet]
        public RetornoModel<Login, enumClienteException> GetLogado()
        {
            RetornoModel<Login, enumClienteException> retorno;
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId());
                CarregarClienteOSESP(clienteBO, false);

                if (SessionModel.Login == null)
                {
                    retorno = new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não existe um cliente logado", Retorno = null, Tipo = enumClienteException.usuarioNaoEncontrado };
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                else
                {
                    Login login = SessionModel.Login;
                    retorno = new RetornoModel<Login, enumClienteException>();
                    retorno.Sucesso = true;
                    retorno.Retorno = login;
                }
             }
            return ParseRetorno(retorno);
        }
    }
}