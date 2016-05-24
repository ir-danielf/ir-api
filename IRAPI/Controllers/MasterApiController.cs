using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using Newtonsoft.Json;

namespace IRAPI.Controllers
{
    public class MasterApiController : ApiController
    {
        public APIUsuarioToken APIUsuarioToken { get; set; }
        
        public bool AtualizarToken { get; set; }

        public SessionModel SessionModel { get; set; }

        /// <summary>
        /// Desabilita os erros para a API do NewRelic.
        /// </summary>
        public void NewRelicIgnoreTransaction()
        {
            NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            try
            {
                //LogUtil.Debug(string.Format("##Master.ExecuteAsync## SESSION {0}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA"));

                var retorno = base.ExecuteAsync(controllerContext, cancellationToken);

                // TODO: O token é atualizado somente em caso de retorno OK dos métodos HTTP. Então, se pesquisarmos um evento, e o mesmo
                // não existir, e o token ainda não foi salvo em banco, o token terá um valor nulo.
                // Lembrando que todos os métodos HTTP retornam um HttpException em caso de anomalia.
                if ((APIUsuarioToken != null) && (retorno.Result.StatusCode == HttpStatusCode.OK))
                {
                    try
                    {
                        var dadosSerializados = JsonConvert.SerializeObject(SessionModel.CopyTo(new DadosSessionModel()));

                        LogUtil.Debug(string.Format("##Master.ExecuteAsync## SESSION_DATA {0}", dadosSerializados));

                        if ((!APIUsuarioToken.DadosSession.Equals(dadosSerializados)) || (AtualizarToken))
                        {
                            LogUtil.Debug(string.Format("##Master.ExecuteAsync.AtualizarSessionData## SESSION_DATA {0}", dadosSerializados));


                            APIUsuarioToken.DadosSession = dadosSerializados;
                            APIUsuarioToken.ClienteID = SessionModel.ClienteID;

                            using (var usuarioBO = new APIUsuarioBO())
                            {
                                usuarioBO.AtualizarSessao(APIUsuarioToken, AtualizarToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Master.ExecuteAsync.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                        retorno.Result.StatusCode = HttpStatusCode.InternalServerError;
                        //LogUtil.Error(ex);
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Master.ExecuteAsync.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                //LogUtil.Error(ex);
                throw ex;
            }
        }

        protected CompraEstruturaVendaModel GerarEstruturaVenda()
        {
            var EstruturaVenda = new CompraEstruturaVendaModel();
            EstruturaVenda.CanalID = SessionModel.CanalID;
            EstruturaVenda.LojaID = SessionModel.LojaID;
            EstruturaVenda.UsuarioID = SessionModel.UsuarioID;
            return EstruturaVenda;
        }

        protected void CarregarCompra(CompraBOModel bo, bool carregarEntregas = false)
        {
            LogUtil.Debug(string.Format("##CarregarCompra## SESSION {0}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA"));

            CarregarCliente(bo.clienteBO, carregarEntregas);
            try
            {
                SessionModel.Compra = new CompraModel();
                SessionModel.Compra.EstruturaVenda = GerarEstruturaVenda();
                SessionModel.Compra.Login = SessionModel.Login;
                SessionModel.Compra.SessionID = SessionModel.SessionID;
                SessionModel.Compra.CarrinhoItens = bo.carrinhoBO.Listar(SessionModel.SessionID, SessionModel.ClienteID, enumCarrinhoStatus.reservado);
                if ((carregarEntregas || SessionModel.EntregaControleID > 0) && SessionModel.Compra.CarrinhoItens.Count > 0)
                {
                    SessionModel.Compra = bo.entregaBO.Carregar(SessionModel.Compra);

                    var ativarTaxasEntregaDinamica = Convert.ToBoolean(ConfigurationManager.AppSettings["AtivarTaxasEntregaDinamica"]);

                    if (ativarTaxasEntregaDinamica)
                    {
                        var taxasEntregaDinamica = ConfigurationManager.AppSettings["TaxasEntregaDinamica"].Split('|');

                        foreach (var taxaEntregaDinamica in taxasEntregaDinamica)
                        {
                            var configTaxaEntregaDinamica = taxaEntregaDinamica.Split(';');

                            if (configTaxaEntregaDinamica.Length == 3)
                            {
                                var idEntregaControleDinamica = Convert.ToInt32(configTaxaEntregaDinamica[0]);
                                var idEntregaControleCombinar = Convert.ToInt32(configTaxaEntregaDinamica[1]);
                                var valorEntregaControleDinamica = Convert.ToDecimal(configTaxaEntregaDinamica[2], new CultureInfo("pt-BR"));

                                var temIdEntregaControleCombinar = SessionModel.Compra.EntregaControles.Any(a => a.ID == idEntregaControleCombinar);
                                if (temIdEntregaControleCombinar)
                                {
                                    var entregaControleDinamica = SessionModel.Compra.EntregaControles.FirstOrDefault(f => f.ID == idEntregaControleDinamica);

                                    if (entregaControleDinamica != null)
                                        entregaControleDinamica.Valor = valorEntregaControleDinamica;
                                }
                            }
                        }
                    }
                }
                if (SessionModel.Compra.EntregaControles != null && SessionModel.Compra.EntregaControles.Count > 0 && SessionModel.ValesIngressoID != null && SessionModel.ValesIngressoID.Count > 0)
                {
                    SessionModel.Compra.ValeIngressos = bo.valeIngressoBO.Listar(SessionModel.ValesIngressoID);
                }

                SessionModel.Compra.EntregaControleID = SessionModel.EntregaControleID;
                SessionModel.Compra.ClienteEnderecoID = SessionModel.ClienteEnderecoID;
                SessionModel.Compra.PDVID = SessionModel.PDVID;
                SessionModel.Compra = bo.carrinhoBO.CalcularValores(SessionModel.Compra);
                SessionModel.Compra = bo.cotaBO.CarregarCotaInformacao(SessionModel.Compra);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##CarregarCompra.EXCEPTION## SESSION {0} MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                var retorno = new RetornoModel<CompraModel>();
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
        }

        protected void CarregarCliente(ClienteBO clienteBO, bool carregarEnderecos = false)
        {
            LogUtil.Debug(string.Format("##CarregarCliente## SESSION {0}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA"));

            try
            {
                if ((SessionModel.ClienteID > 0) && (SessionModel.Login == null))
                {
                    SessionModel.Login = clienteBO.Consultar(SessionModel.ClienteID, false);
                }
                if ((SessionModel.Login != null) && (SessionModel.Login.Cliente.EnderecoList == null))
                {
                    SessionModel.Login.Cliente = clienteBO.CarregarEnderecos(SessionModel.Login.Cliente);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##CarregarCliente.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                var retorno = new RetornoModel<CompraModel>();
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
        }

        protected void CarregarClienteOSESP(ClienteBO clienteBO, bool carregarEnderecos = false)
        {
            try
            {
                if ((SessionModel.ClienteID > 0) && (SessionModel.Login == null))
                {
                    SessionModel.Login = clienteBO.ConsultarOSESP(SessionModel.ClienteID, true);
                }
                if ((SessionModel.Login != null) && (SessionModel.Login.Cliente.EnderecoList == null))
                {
                    SessionModel.Login.Cliente = clienteBO.CarregarEnderecos(SessionModel.Login.Cliente);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Master.CarregarClienteOSESP.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                var retorno = new RetornoModel<CompraModel>();
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
        }

        protected RetornoModel LimparCompra(CarrinhoBO carrinhoBO)
        {
            SessionModel.EntregaControleID = 0;
            SessionModel.PDVID = 0;
            SessionModel.ClienteEnderecoID = 0;
            SessionModel.ValesIngressoID = null;

            RetornoModel retorno = new RetornoModel();
            retorno.Mensagem = "Ok";
            retorno.Sucesso = true;
            try
            {
                var carrinhoItens = carrinhoBO.Listar(SessionModel.SessionID, SessionModel.ClienteID, enumCarrinhoStatus.reservado);
                carrinhoBO.LimparReserva(SessionModel.SessionID, enumIngressoStatus.disponivel, carrinhoItens);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Master.LimparCompra.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                //LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            return retorno;
        }

        protected Login ParseLogin(Login login)
        {
            if (login != null)
            {
                login.Senha = "";
                login.FaceBookUserInfos = null;
                if (login.Cliente != null)
                {
                    login.Cliente.Senha = null;
                    login.Cliente.Obs = null;
                    login.Cliente.LoginOSESP = null;
                    login.Cliente.CEPEntrega = null;
                    login.Cliente.EnderecoEntrega = null;
                    login.Cliente.NumeroEntrega = null;
                    login.Cliente.CidadeEntrega = null;
                    login.Cliente.EstadoEntrega = null;
                    login.Cliente.ComplementoEntrega = null;
                    login.Cliente.BairroEntrega = null;
                    login.Cliente.DDDTelefoneComercial2 = null;
                    login.Cliente.TelefoneComercial2 = null;
                    login.Cliente.tVendaBilheteria = null;
                    login.Cliente.Voucher = null;
                    login.Cliente.NomeEntrega = null;
                    login.Cliente.CPFEntrega = null;
                    login.Cliente.RGEntrega = null;
                    //login.Cliente.Email = null;
                    //login.Cliente.Profissao = null;
                    //login.Cliente.SituacaoProfissionalID = null;
                    if (string.IsNullOrEmpty(login.Cliente.CNPJ))
                    {
                        login.Cliente.CNPJ = null;
                        login.Cliente.NomeFantasia = null;
                        login.Cliente.RazaoSocial = null;
                        login.Cliente.InscricaoEstadual = null;
                    }
                }
            }
            return login;
        }

        protected Evento ParseEvento(Evento evento)
        {
            if (evento != null)
            {
                evento = ParseEventoDetalhe(evento);
                evento.Release = null;
                evento.Apresentacao = null;
                evento.NewsAssinante = null;
                evento.DestaqueRegiao = null;
                if (evento.Tipo != null)
                {
                    evento.Tipo.DestaqueLinkRegiao = null;
                }
            }
            return evento;
        }

        protected Evento ParseEventoDetalhe(Evento evento)
        {
            if (evento != null)
            {
                if (evento.Apresentacao != null)
                {
                    foreach (var apresentacao in evento.Apresentacao)
                    {
                        apresentacao.Preco = null;
                        if (apresentacao.MaiorPreco != null)
                        {
                            apresentacao.MaiorPreco.Apresentacao = null;
                        }
                        if (apresentacao.MenorPreco != null)
                        {
                            apresentacao.MenorPreco.Apresentacao = null;
                        }

                    }
                }
                if (evento.EventoMidias != null)
                {
                    foreach (var itemChave in evento.EventoMidias.Where(t => t.Key == enumEventoTipoMidiaTipo.arquivo))
                    {

                        foreach (var itemMidia in itemChave.Value)
                        {
                            for (int i = 0; i < itemMidia.Value.Count; i++)
                            {
                                itemMidia.Value[i] = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoEventoMidia) + itemMidia.Value[i];
                            }
                        }
                    }
                }
                evento.Imagem = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoEventoImagens) + evento.Imagem;
            }
            return evento;
        }

        protected List<Carrinho> ParseCarrinhoItens(List<Carrinho> carrinhoItens)
        {
            if (carrinhoItens != null)
            {
                foreach (var carrinho in carrinhoItens)
                {
                    carrinho.EventoObject.PrimeiraApresentacao = null;
                    carrinho.SetorObject.Apresentacao = null;
                    carrinho.ApresentacaoObject.Evento = null;
                    carrinho.ApresentacaoObject.Preco = null;
                    carrinho.ApresentacaoObject.Setor = null;
                    carrinho.EventoObject.Apresentacao = null;
                    carrinho.Precos = null;
                    carrinho.EventoObject = ParseEvento(carrinho.EventoObject);
                    if (carrinho.CotaItemObject != null)
                    {
                        if (!Request.RequestUri.AbsolutePath.Contains("/cotas"))
                        {
                            carrinho.CotaItemObject = null;
                        }
                        else
                        {
                            if (carrinho.CotaItemObject.Parceiro != null)
                            {
                                carrinho.CotaItemObject.Parceiro.Url = null;
                            }
                        }

                    }

                }
            }
            return carrinhoItens;
        }

        protected CompraModel ParseCompra(CompraModel compra)
        {
            if (compra != null)
            {
                compra.Login = ParseLogin(compra.Login);

                if ((compra.EntregaControles != null) && (compra.EntregaControles.Count > 0))
                {
                    if (!((Request.Method == HttpMethod.Get) && (Request.RequestUri.AbsolutePath.Contains("/entregas"))))
                    {
                        if (compra.EntregaControleID > 0)
                        {
                            compra.EntregaControles = compra.EntregaControles.Where(t => t.ID == SessionModel.EntregaControleID).ToList();
                            if ((compra.Login != null) && (compra.Login.Cliente.EnderecoList != null) && (SessionModel.ClienteEnderecoID > 0))
                            {
                                compra.Login.Cliente.EnderecoList = compra.Login.Cliente.EnderecoList.Where(t => t.ID == SessionModel.EntregaControleID).ToList();
                            }
                        }

                    }

                    compra.EntregaControles.RemoveAll(t => t.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaAgendada || t.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaNormal);
                }

                compra.CarrinhoItens = ParseCarrinhoItens(compra.CarrinhoItens);

                if (compra.VendaBilheteria != null)
                {
                    compra.VendaBilheteria.tCliente = null;
                    compra.EntregaControles = null;
                    if (compra.Login != null && compra.Login.Cliente != null)
                        compra.Login.Cliente.EnderecoList = null;
                }

                compra.EstruturaVenda = null;

            }

            return compra;
        }

        protected T ParseRetorno<T>(T retorno)
        {
            if (retorno != null)
            {
                if (retorno.GetType() == typeof(Evento))
                {
                    var retornoTipado = retorno.GetAsType<Evento>();
                    retornoTipado = ParseEventoDetalhe(retornoTipado);
                    retorno = retornoTipado.GetAsType<T>();
                }
                else if (retorno.GetType() == typeof(PagedListModel<Evento>))
                {
                    var retornoTipado = retorno.GetAsType<PagedListModel<Evento>>();
                    if (retornoTipado.Itens != null)
                    {
                        foreach (var evento in retornoTipado.Itens)
                        {
                            ParseEvento(evento);
                        }
                    }
                    retorno = retornoTipado.GetAsType<T>();
                }
                else if (retorno.GetType() == typeof(List<Evento>))
                {
                    var retornoTipado = retorno.GetAsType<List<Evento>>();
                    if (retornoTipado != null)
                    {
                        foreach (var evento in retornoTipado)
                        {
                            ParseEvento(evento);
                        }
                    }
                    retorno = retornoTipado.GetAsType<T>();

                }
                else if (retorno.GetType() == typeof(Login))
                {
                    var retornoTipado = retorno.GetAsType<Login>();
                    retornoTipado = ParseLogin(retornoTipado);
                    retorno = retornoTipado.GetAsType<T>();
                }
                else if (retorno.GetType() == typeof(PagedListModel<CompraModel>))
                {
                    var retornoTipado = retorno.GetAsType<PagedListModel<CompraModel>>();
                    if (retornoTipado.Itens != null)
                    {
                        foreach (var item in retornoTipado.Itens)
                        {
                            ParseCompra(item);
                        }
                    }
                }
                else if (retorno.GetType() == typeof(CompraModel))
                {
                    var retornoTipado = retorno.GetAsType<CompraModel>();
                    retornoTipado = ParseCompra(retornoTipado);
                    retorno = retornoTipado.GetAsType<T>();
                }
                else if (retorno.GetType() == typeof(List<CompraModel>))
                {
                    var retornoTipado = retorno.GetAsType<List<CompraModel>>();
                    foreach (var compra in retornoTipado)
                    {
                        ParseCompra(compra);
                    }
                    retorno = retornoTipado.GetAsType<T>();
                }
                else if (retorno.GetType() == typeof(List<Carrinho>))
                {
                    var retornoTipado = retorno.GetAsType<List<Carrinho>>();
                    retornoTipado = ParseCarrinhoItens(retornoTipado);
                    retorno = retornoTipado.GetAsType<T>();
                }
                else if (retorno.GetType() == typeof(SessionModel))
                {
                    var retornoTipado = retorno.GetAsType<SessionModel>();
                    retornoTipado.Compra = ParseCompra(retornoTipado.Compra);
                    retorno = retornoTipado.GetAsType<T>();
                }
            }
            return retorno;
        }

        protected RetornoModel<T> ParseRetorno<T>(RetornoModel<T> retorno)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString["removeRetorno"] != null)
                {
                    try
                    {
                        retorno.SetPropByName("Retorno", null);
                    }
                    catch { }
                }
                else
                {
                    retorno.Retorno = ParseRetorno(retorno.Retorno);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Master.ParseRetorno<T>.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                //LogUtil.Error(ex);
                retorno.Mensagem = ex.Message;
                return retorno;
            }

        }

        protected RetornoModel<T, TEnum> ParseRetorno<T, TEnum>(RetornoModel<T, TEnum> retorno)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString["removeRetorno"] != null)
                {
                    try
                    {
                        retorno.SetPropByName("Retorno", null);
                    }
                    catch { }
                }
                else
                {
                    retorno.Retorno = ParseRetorno(retorno.Retorno);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Master.ParseRetorno<T, TEnum>.EXCEPTION## SESSION {0}, MSG {1}", (this.SessionModel != null) ? this.SessionModel.SessionID : "NA", ex.Message), ex);

                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }
    }
}