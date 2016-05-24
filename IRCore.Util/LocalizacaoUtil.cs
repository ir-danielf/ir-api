using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using IRCore.Util.Models;

namespace IRCore.Util
{
    public static class LocalizacaoUtil
    {
        public const string GoogleUrlLatLng = "http://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&sensor=true&language=pt-BR";
        public const string GoogleUrlAddress = "http://maps.googleapis.com/maps/api/geocode/json?address={0},{1}&sensor=true&language=pt-BR";

        public static string GetAddress(double latitude, double longitude)
        {
            string latitudeStr = latitude.ToString();
            string longitudeStr = longitude.ToString();

            WebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(LocalizacaoUtil.GoogleUrlLatLng,
                    latitudeStr.ToCoordinate(),
                    longitudeStr.ToCoordinate()));
                request.Method = "GET";
                response = request.GetResponse();
                if (response != null)
                {
                    string str = null;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            str = streamReader.ReadToEnd();
                        }
                    }

                    return str;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }

            return null;
        }

        public static string GetAddress(string cidade, string estado)
        {

            WebResponse response = null;

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(LocalizacaoUtil.GoogleUrlAddress,
                    cidade,
                    estado));
                request.Method = "GET";
                response = request.GetResponse();
                if (response != null)
                {
                    string str = null;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            str = streamReader.ReadToEnd();
                        }
                    }

                    return str;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }

            return null;
        }

        public static Localizacao GetFromJson(string address)
        {
            Localizacao localizacao = new Localizacao();
            try{
                dynamic result = new JavaScriptSerializer().Deserialize<object>(address);
                string numero = "";
                foreach (var item in result["results"][0]["address_components"])
                {
                    if (item["types"][0] == "street_number")
                    {
                        numero = item["long_name"];
                    }
                    else if (item["types"][0] == "route")
                    {
                        localizacao.Logradouro = item["long_name"];
                    }
                    else if (item["types"][0] == "neighborhood")
                    {
                        localizacao.Bairro = item["long_name"];
                    }
                    else if (item["types"][0] == "locality")
                    {
                        localizacao.Cidade = item["long_name"];
                        localizacao.CidadePhonetized = localizacao.Cidade.Phonetized();

                    }
                    else if (item["types"][0] == "administrative_area_level_1")
                    {
                        localizacao.Estado = item["short_name"];
                    }
                    else if (item["types"][0] == "administrative_area_level_1")
                    {
                        localizacao.Estado = item["short_name"];
                    }
                    else if (item["types"][0] == "country")
                    {
                        if (item["short_name"] == "BR")
                        {
                            localizacao.Pais = "Brasil";
                        }
                        else
                        {
                            localizacao.Pais = item["long_name"];
                        }
                    }
                }
                if (!string.IsNullOrEmpty(numero))
                {
                    localizacao.Logradouro += ", " + numero;
                }
                localizacao.EnderecoCompleto = "";
                if (!string.IsNullOrEmpty(localizacao.Logradouro))
                {
                    localizacao.EnderecoCompleto += localizacao.Logradouro + ", ";
                }
                if (!string.IsNullOrEmpty(localizacao.Bairro))
                {
                    localizacao.EnderecoCompleto += localizacao.Bairro + ", ";
                }
                localizacao.EnderecoCompleto += localizacao.Cidade + ", " + localizacao.Estado;
                localizacao.Latitude = Convert.ToDouble(result["results"][0]["geometry"]["location"]["lat"]);
                localizacao.Longitude = Convert.ToDouble(result["results"][0]["geometry"]["location"]["lng"]);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }
            
            return localizacao;
        }
        

        public static Localizacao Get(double latitude, double longitude)
        {
            Localizacao localizacao = GetFromJson(LocalizacaoUtil.GetAddress(latitude, longitude));
            localizacao.Latitude = latitude;
            localizacao.Longitude = longitude;
            localizacao.Valido = true;
            return localizacao;
        }

        public static Localizacao Get(string cidade, string estado)
        {
            Localizacao localizacao = GetFromJson(LocalizacaoUtil.GetAddress(estado, cidade));
            localizacao.Estado = estado;
            localizacao.Cidade = cidade;
            localizacao.CidadePhonetized = cidade.Phonetized();
            localizacao.Valido = true;
            return localizacao;
        }

        public static int CalcDistancia(double latitude, double longitude, double latitudeFinal, double longitudeFinal)
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
}