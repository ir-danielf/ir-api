using IRAPI.Models;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO.Estrutura;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace IRAPI.Controllers
{
    [IRAPIAuthorize]
    public class VariavelController : MasterApiController
    {
        [Route("variaveis/sessao")]
        [HttpGet]
        public RetornoModel<SessionModel> GetSessao(bool completo = false)
        {
            if (completo)
            {
                using (var ado = new MasterADOBase())
                {
                    var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);
                    CarregarCompra(bo, true);
                }
            }
            RetornoModel<SessionModel> retorno = new RetornoModel<SessionModel>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            retorno.Retorno = SessionModel;
            return ParseRetorno(retorno);
        }

        [IRAPIAuthorize(enumAPIRele.master)]
        [Route("variaveis/sessao")]
        [HttpPut]
        public RetornoModel<SessionModel> PutSessao([FromBody]DadosSessionModel rq, bool zerarValoresNaoInformados = false, bool retornoCompleto = false)
        {
            if (zerarValoresNaoInformados)
            {
                SessionModel.CopyFrom(rq);
            }
            else
            {
                if (rq.CanalID > 0)
                {
                    SessionModel.CanalID = rq.CanalID;
                }
                if (rq.ClienteEnderecoID > 0)
                {
                    SessionModel.ClienteEnderecoID = rq.ClienteEnderecoID;
                }
                if (rq.ClienteID > 0)
                {
                    SessionModel.ClienteID = rq.ClienteID;
                }
                if (!string.IsNullOrEmpty(rq.SessionID))
                {
                    SessionModel.SessionID = rq.SessionID;
                }
                if (rq.UsuarioID > 0)
                {
                    SessionModel.UsuarioID = rq.UsuarioID;
                }
                if (rq.ValesIngressoID != null)
                {
                    SessionModel.ValesIngressoID = rq.ValesIngressoID;
                }
            }
            RetornoModel<SessionModel> retorno = new RetornoModel<SessionModel>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            retorno.Retorno = SessionModel;
            return ParseRetorno(retorno);
        }

        [IRAPIAuthorize(enumAPIRele.master)]
        [Route("variaveis/servidor_infos")]
        [HttpGet]
        public RetornoModel<Dictionary<string, string>> GetServidorInfos()
        {

            RetornoModel<Dictionary<string, string>> retorno = new RetornoModel<Dictionary<string, string>>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            retorno.Retorno = new Dictionary<string, string>();
            retorno.Retorno["machine_name"] = Environment.MachineName;

            foreach (var key in HttpContext.Current.Request.ServerVariables.AllKeys)
            {
                retorno.Retorno[key.ToLower()] = HttpContext.Current.Request.ServerVariables[key];
            }

            return ParseRetorno(retorno);
        }

        [Route("variaveis/guid")]
        [HttpGet]
        public RetornoModel<string> GetGuid()
        {
            SessionModel.Guid = Guid.NewGuid().ToString();

            var retorno = new RetornoModel<string>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            retorno.Retorno = SessionModel.Guid;

            return ParseRetorno(retorno);
        }
    }
}
