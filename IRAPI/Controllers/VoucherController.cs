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
    [IRAPIAuthorize(enumAPIRele.net)]
    public class VoucherController : MasterApiController
    {
        [Route("clientes/vouchers/{cpf}")]
        [HttpGet]
        public RetornoModel<List<VoucherClienteRetorno>> GetClienteListaVouchers(string cpf)
        {
            RetornoModel<List<VoucherClienteRetorno>> retorno = new RetornoModel<List<VoucherClienteRetorno>>();

            List<VoucherClienteRetorno> resultado = null;

            try
            {
                using (var ado = new MasterADOBase())
                {
                    VoucherBO voucherBO = new VoucherBO(ado);

                    resultado = voucherBO.Consultar(cpf);
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
                retorno.Mensagem = "Não existem voucher trocados por este cliente";
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