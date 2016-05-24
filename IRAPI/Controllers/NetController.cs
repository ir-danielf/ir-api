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
    [IRAPIAuthorize(enumAPIRele.net)]
    public class NetController : MasterApiController
    {
        [Route("net/clientes/compra_cota/{cpf}")]
        [HttpGet]
        public RetornoModel<List<ClienteComprasCotaNetModel>> GetClienteComprasCota(string cpf)
        {
            RetornoModel<List<ClienteComprasCotaNetModel>> retorno = new RetornoModel<List<ClienteComprasCotaNetModel>>();
            List<ClienteComprasCotaNetModel> resultado = null;

            try
            {
                using (var ado = new MasterADOBase())
                {
                    ClienteBO clienteBO = new ClienteBO(ado);

                    resultado = clienteBO.ListaComprasCotaNet(cpf);
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
                retorno.Mensagem = "Nenhuma compra com cota de 50% encontrada para o cpf";
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