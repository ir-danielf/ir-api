/**************************************************
* Arquivo: CEP.cs
* Gerado: 14/11/2008
* Autor: Celeritas Ltda
***************************************************/

using Google.Api.Maps.Service;
using Google.Api.Maps.Service.Geocoding;
using IRLib.ClientObjects;
using System;
using System.Configuration;
using System.Data;
using IRCore.Util;

namespace IRLib
{

    public class CEP : CEP_B
    {

        public CEP() { }

        public CEP(int usuarioIDLogado) : base(usuarioIDLogado) { }
        /// <summary>
        /// Retorna um objeto CEP populado com as informações trazidas a partir do Cep passado. Retorna Null se nao encontrar.
        /// </summary>
        /// <param name="cep"></param>
        /// <returns></returns>
        public CEP Buscar(string cep)
        {
            try
            {
                bool achou = false;
                CEP retorno = new CEP();

                bd.Consulta("SELECT EstadoID,CidadeID,EstadoSigla,CidadeNome,Logradouro,Endereco,Bairro,Cep FROM tCep(NOLOCK) WHERE Cep = '" + cep + "'");

                while (bd.Consulta().Read())
                {
                    retorno.EstadoID.Valor = bd.LerInt("EstadoID");
                    retorno.CidadeID.Valor = bd.LerInt("CidadeID");
                    retorno.EstadoSigla.Valor = bd.LerString("EstadoSigla");
                    retorno.CidadeNome.Valor = bd.LerString("CidadeNome");
                    retorno.Logradouro.Valor = bd.LerString("Logradouro");
                    retorno.Endereco.Valor = bd.LerString("Endereco");
                    retorno.Bairro.Valor = bd.LerString("Bairro");
                    retorno.Cep.Valor = bd.LerString("Cep");
                    achou = true;
                }
                if (achou)
                    return retorno;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaCEP BuscarEstrutura(string cep)
        {
            try
            {
                bd.Consulta("SELECT TOP 1 TLO_TX + ' ' +LOG_NO AS Logradouro, BAI_NO AS Bairro, LOC_NO AS Cidade, UFE_SG AS Estado   FROM  LOG_LOGRADOURO WHERE CEP = '" + cep.ToSafeString() + "'");

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar nenhum endereço com o CEP: " + cep);

                EstruturaCEP estrutura = new EstruturaCEP()
                {
                    Rua = bd.LerString("Logradouro"),
                    Bairro = bd.LerString("Bairro"),
                    Cidade = bd.LerString("Cidade"),
                    Estado = bd.LerString("Estado"),
                };

                return estrutura;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public static GeographicPosition BuscarCoordenadas(string cep)
        {
            var estrutura = BuscarCEP.GetEnderecoEstruturado(cep);

            var request = new GeocodingRequest();
            request.Address = FormatarEstrutra(estrutura);
            request.Sensor = "false";
            var response = GeocodingService.GetResponse(request);
            if (response.Status != Google.Api.Maps.Service.ServiceResponseStatus.Ok)
                throw new Exception("Não foi possível encontrar a sua localização a partir do CEP digitado.");

            return response.Results[0].Geometry.Location;
        }

        public static GeographicPosition BuscarCoordenadas(string cep, string endereco, string cidade, string estado)
        {
            try
            {
                LogUtil.Info(string.Format("##RoboAtzSite.BuscarCoordenadas.Initialize## CEP: {0}", cep));

                var achouCEP = false;
                var estrutura = new EstruturaCEP();

                if (!string.IsNullOrEmpty(cep))
                {
                    try
                    {
                        LogUtil.Debug(string.Format("##RoboAtzSite.BuscarCoordenadas.IniciandoGetEnderecoEstruturado##"));
                        estrutura = BuscarCEP.GetEnderecoEstruturado(cep);
                        achouCEP = true;
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##RoboAtzSite.BuscarCoordenadas.EXCEPTION## MSG: {0}", ex.Message), ex);
                    }
                }

                if (!achouCEP)
                {
                    if (endereco.Contains("-"))
                    {
                        var enderecos = endereco.Split('-');
                        estrutura.Rua = enderecos[0];
                    }
                    else
                    {
                        estrutura.Rua = endereco;
                        estrutura.Cidade = cidade;
                        estrutura.Estado = estado;
                    }
                }

                var request = new GeocodingRequest { Address = FormatarEstrutra(estrutura), Sensor = "false" };

                LogUtil.Info(string.Format("##RoboAtzSite.BuscarCoordenadas## ENDEREÇO_FORMATADO: {0}, LATITUDE/LONGITUDE: {1}", request.Address, request.LatitudeLongitude));

                var response = GeocodingService.GetResponse(request);

                LogUtil.Info(string.Format("##RoboAtzSite.BuscarCoordenadas## RESPONSE_STATUS_GEOCODING_SERVICE: {0}", response.Status));

                if (response.Status != ServiceResponseStatus.Ok)
                {
                    LogUtil.Info(string.Format("##RoboAtzSite.BuscarCoordenadas.ERROR## MSG: {0}", "Não foi possível encontrar a sua localização a partir do CEP digitado"));
                    throw new Exception("Não foi possível encontrar a sua localização a partir do CEP digitado.");
                }

                LogUtil.Info(string.Format("##RoboAtzSite.BuscarCoordenadas.SUCCESS##"));

                return response.Results[0].Geometry.Location;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.BuscarCoordenadas.EXCEPTION## MSG: {0}", ex.Message), ex);
                return null;
            }
        }

        private static string FormatarEstrutra(EstruturaCEP estrutura)
        {
            return string.Format("{0} , {1} - {2}", estrutura.Rua, estrutura.Cidade, estrutura.Estado);
        }

        public static int CalcularDistancia(double latitude, double longitude, double latitudeFinal, double longitudeFinal)
        {
            double auxPi = System.Math.PI / 180;
            double arcoA = (longitudeFinal - longitude) * auxPi;
            double arcoB = (90 - latitudeFinal) * auxPi;
            double arcoC = (90 - latitude) * auxPi;

            double resultado = Math.Cos(arcoB) * Math.Cos(arcoC) + Math.Sin(arcoB) * Math.Sin(arcoC) * Math.Cos(arcoA);

            resultado = (40030 * ((180 / Math.PI) * Math.Acos(resultado))) / 360;

            return (int)Math.Round(resultado);
        }
    }

    public class CEPLista : CEPLista_B
    {
        public CEPLista() { }

        public CEPLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }

    public static class BuscarCEP
    {
        public static DataSet GetEndereco(string cep)
        {
            var usuarioCEP = Configuracao.GetString(Configuracao.Keys.UsuarioCEP);
            var senhaCEP = Configuracao.GetString(Configuracao.Keys.SenhaCEP);

            return new wsCEP.BuscaCEP().GetEndereco(usuarioCEP, senhaCEP, cep);
        }

        public static EstruturaCEP GetEnderecoEstruturado(string cep)
        {
            var usuarioCep = Configuracao.GetString(Configuracao.Keys.UsuarioCEP);
            var senhaCep = Configuracao.GetString(Configuracao.Keys.SenhaCEP);

            LogUtil.Debug(string.Format("##RoboAtzSite.GetEnderecoEstruturado.Initialize## CEP: {0}", cep));

            var estrutura = new wsCEP.BuscaCEP().GetEnderecoEstruturado(usuarioCep, senhaCep, cep);

            LogUtil.Debug(string.Format("##RoboAtzSite.GetEnderecoEstruturado.SUCCESS##"));

            return new EstruturaCEP()
            {
                Rua = estrutura.Rua,
                Bairro = estrutura.Bairro,
                Cidade = estrutura.Cidade,
                Estado = estrutura.Estado,
            };
        }
    }
}
