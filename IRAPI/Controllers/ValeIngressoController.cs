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
using System.Web;
using System.Web.Http;

namespace IRAPI.Controllers
{
     [IRAPIAuthorize(enumAPIRele.venda)]
    public class ValeIngressoController : MasterApiController
    {
        [Route("vales_ingresso/{codigo}")]
        [HttpGet]
        public RetornoModel<tValeIngresso> GetValeIngresso(string codigo)
        {
            RetornoModel<tValeIngresso> retorno = new RetornoModel<tValeIngresso>();
            tValeIngresso valeIngresso = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    ValeIngressoBO valeIngressoBO = new ValeIngressoBO(ado);
                    valeIngresso = valeIngressoBO.ValidarCodigo(codigo);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Retorno = valeIngresso;
            if (valeIngresso != null)
            {
                retorno.Mensagem = "OK";
                retorno.Sucesso = true;
            }
            else
            {
                retorno.Mensagem = "Código do vale ingresso inválido";
				retorno.Sucesso = false;
				NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }
    }
}