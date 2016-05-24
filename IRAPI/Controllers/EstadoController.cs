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
    /// <summary>
    /// APIs de estados, cidades e pontos de venda
    /// </summary>
    [IRAPIAuthorize(enumAPIRele.venda, enumAPIRele.evento)]
    public class EstadoController : MasterApiController
    {

        /// <summary>
        /// Api que retorna as cidades do estado informado
        /// </summary>
        /// <param name="uf">Estado</param>
        /// <param name="apenasEventos">Indica se deve retornar apenas cidades que possui eventos</param>
        /// <returns>Lista de cidades</returns>
        [Route("estados/{uf}/cidades")]
        [HttpGet]
        public RetornoModel<List<string>> GetCidades(string uf, bool apenasEventos = true)
        {
            var retorno = new RetornoModel<List<string>>();
            List<string> cidades = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var geobo = new GeoBO(ado);
                    cidades = apenasEventos ? geobo.ListarCidadeLocal(uf) : geobo.ListarCidade(uf).Select(c => c.Nome).ToList();
                }
            }
            catch (Exception e)
            {
                retorno.Mensagem = e.Message;
                LogUtil.Error(e);
            }
            retorno.Sucesso = true;
            retorno.Retorno = cidades;
            if (cidades == null || cidades.Count == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhuma cidade encontrada";
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
        /// Api que retorna os estados
        /// </summary>
        /// <param name="apenasEventos">Indica se deve retornar apenas cidades que possui eventos</param>
        /// <returns>Lista de estados</returns>
        [Route("estados")]
        [HttpGet]
        public RetornoModel<List<string>> Get(bool apenasEventos = true)
        {
            var retorno = new RetornoModel<List<string>>();
            List<string> cidades;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var geobo = new GeoBO(ado);
                    cidades = apenasEventos ? geobo.ListarEstadoLocal() : geobo.ListarEstado().Select(t => t.Sigla).ToList();
                }
            }
            catch (Exception e)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = e.Message;
                LogUtil.Error(e);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Sucesso = true;
            retorno.Retorno = cidades;
            if (cidades.Count == 0)
            {
                retorno.Mensagem = "Nenhum estado encontrado";
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
        /// Api que retorna os pontos de venda da cidade informada
        /// </summary>
        /// <param name="uf">Estado</param>
        /// <param name="cidade">Cidade</param>
        /// <returns>Lista de pontos de venda</returns>
        [Route("estados/{uf}/cidades/{cidade}/pontos_venda")]
        [HttpGet]
        public RetornoModel<List<PontoVenda>> GetPontosVenda(string uf, string cidade)
        {
            var retorno = new RetornoModel<List<PontoVenda>>();
            List<PontoVenda> lstPontoVenda;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var pontoVendaBO = new PontoVendaBO(ado);
                    lstPontoVenda = pontoVendaBO.ListarPdvPermiteRetirada(cidade, uf);
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                LogUtil.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }
            retorno.Retorno = lstPontoVenda;
            retorno.Sucesso = true;
            if (lstPontoVenda.Count == 0)
            {
                retorno.Mensagem = "Nenhum ponto de venda encontrado";
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