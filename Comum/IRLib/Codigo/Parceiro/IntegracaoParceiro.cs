using CTLib;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using IRLib.ServiceNetIntegracao;

namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.CAO)]
    public class IntegracaoParceiro : MarshalByRefObject
    {
        public bool ValidaCliente(string URL, string codigo)
        {
            bool achouCliente = false;

            string retorno = this.HTTPGetPage(URL, codigo, codigo);

            if (!string.IsNullOrEmpty(retorno))
                achouCliente = LerXml(retorno);

            return achouCliente;
        }

        public bool ValidaClienteCodigo(string URL, string codigo, string CPF)
        {
            string retorno = this.HTTPGetPage(URL, codigo, CPF);
            return JsonConvert.DeserializeObject<Resposta>(retorno).Status;
        }

        public bool ValidaClienteNET(string cpf, string usuarioSenha)
        {
            try
            {
                int result = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IngressoRapidoWSEndPointClient NetIntegracao = new IngressoRapidoWSEndPointClient();
                result = NetIntegracao.validaCPF(usuarioSenha.Split(',')[0], usuarioSenha.Split(',')[1], cpf);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                return result == 1;
            }
            catch
            {
                return false;
            }
        }

        private string HTTPGetPage(string URL, string codigo, string CPF)
        {
            try
            {
                WebRequest mywebReq = WebRequest.Create(string.Format(URL, CPF, codigo));

                using (WebResponse mywebResp = mywebReq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(mywebResp.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool LerXml(string Retorno)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(Retorno);

                if (doc.DocumentElement.Name == "erro")
                    return false;
                else
                {
                    XmlElement Elem = doc.DocumentElement;

                    if (Elem.ChildNodes.Count > 0 && Elem.ChildNodes[0] != null && Elem.ChildNodes[0].InnerText.Length > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    [DataContract()]
    public class Resposta
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public Result Result { get; set; }
    }

    [DataContract()]
    public class Result
    {
        [DataMember]
        public string SubscriptionCode { get; set; }
        [DataMember]
        public string CPF { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
