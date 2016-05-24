using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IRAPI.Controllers
{
    [IRAPIAuthorize(enumAPIRele.venda, enumAPIRele.evento)]
    public class TipoController : MasterApiController
    {
        /// <summary>
        /// Método da API que retorna o status da operação, uma mensagem e uma lista de tipos.
        /// URL de acesso: tipos?apenasEventos={false|true}
        /// Método de acesso: GET
        /// </summary>
        /// <param name="apenasEventos">Quando true retorno apenas com eventos ativos</param>
        [Route("tipos")]
        [HttpGet]
        public RetornoModel<List<Tipo>> Get(bool apenasEventos = false)
        {
            RetornoModel<List<Tipo>> retorno =  new RetornoModel<List<Tipo>>();
            List<Tipo> lstTipo = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var tipoBO = new TipoBO(ado);
                    lstTipo = tipoBO.Listar(apenasEventos);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Retorno = lstTipo;
            retorno.Sucesso = true;
            if (lstTipo.Count == 0)
            {
                retorno.Mensagem = "Nenhum tipo encontrado";
				retorno.Sucesso = false;
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
        /// Método da API que retorna o status da operação, uma mensagem e uma lista de subtipos com base no ID do tipo informado.
        /// URL de acesso: /Tipo/{tipoId}/subtipos?apenasEventos={false|true}
        /// Método de acesso: GET
        /// </summary>
        /// <param name="tipoId">ID do tipo</param>
        /// <param name="apenasEventos">Quando true retorno apenas com eventos ativos</param>
        [Route("tipos/{tipoId}/subtipos")]
        [HttpGet]
        public RetornoModel<List<EventoSubtipo>> Get(int tipoId, bool apenasEventos = false)
        {
            RetornoModel<List<EventoSubtipo>> retorno = new RetornoModel<List<EventoSubtipo>>();
            List<EventoSubtipo> lstSubTipo = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var subTipoBO = new SubtipoBO(ado);
                    lstSubTipo = subTipoBO.ListarTipo(tipoId, apenasEventos);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Retorno = lstSubTipo;
            retorno.Sucesso = true;
            if (lstSubTipo.Count == 0)
            {
                retorno.Mensagem = "Nenhum Subtipo encontrado para o tipo informado";
				retorno.Sucesso = false;
				NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            else
            {
                retorno.Mensagem = "OK";
            }
            return ParseRetorno(retorno);
        }
    }
}
