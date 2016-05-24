using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IRAPI.Controllers
{
    /// <summary>
    /// APIs de locais
    /// </summary>
    [IRAPIAuthorize(enumAPIRele.venda, enumAPIRele.evento)]
    public class LocalController : MasterApiController
    {
        /// <summary>
        /// Consulta local do evento por ID.
        /// </summary>
        /// <param name="id">Id do local</param>
        /// <returns>Detalhes do local</returns>
        [Route("locais/{id}")]
        [HttpGet]
        public RetornoModel<Local> Get(int id)
        {
            var retorno = new RetornoModel<Local>();
            Local local;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var localBO = new LocalBO(ado);
                    local = localBO.Consultar(id);
                }
            }
            catch(Exception ex)
            {
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            if (local != null)
            {
                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
                retorno.Retorno = local;
            }
            else
            {
                retorno.Mensagem = "Nenhum Local encontrado";
                retorno.Sucesso = false;
				NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Retorna uma lista com locais de eventos filtrados por uma string de busca, estado e cidade.
        /// </summary>
        /// <param name="busca">Palavra-chave</param>
        /// <param name="uf">Estado</param>
        /// <param name="cidade">Cidade</param>
        /// <returns>Lista de locais</returns>
        [Route("locais")]
        [HttpGet]
        public RetornoModel<List<Local>> Get(string busca = null, string uf = null, string cidade = null)
        {
            var retorno = new RetornoModel<List<Local>>();
            List<Local> lstLocal;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var localBO = new LocalBO(ado);
                    lstLocal = localBO.Listar(busca, uf, cidade);
                }
            }
            catch(Exception ex)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = ex.Message;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            if (lstLocal.Count > 0)
            {
                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
                retorno.Retorno = lstLocal;
            }
            else
            {
                retorno.Mensagem = "Nenhum local encontrado";
                retorno.Sucesso = false;
				NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }
    }
}
