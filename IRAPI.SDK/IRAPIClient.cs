using IRAPI.SDK.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRAPI.SDK
{
    public class IRAPIClient
    {
        private string MediaType = "application/json";
        private string TokenType = "Bearer";
        //static public string ApiEndpointURL = "http://201.62.240.3/services/api/v1.2/";
        static public string ApiEndpointURL = "https://www.ingressorapido.com.br/services/api/v1.2/";
        //static public string ApiEndpointURL = "http://ir-prd.homologaprojeto.com.br/services/api/v1.2/";
        //static public string ApiEndpointURL = "http://localhost/irapi/";
        public string ClientInfo { get; set; } 
        public string AccessToken { get; set; } 
        public string ApiUser { get; set; }
        public string ApiPassword { get; set; }

        public IRAPIClient(string client) {
            this.ClientInfo = client;
        }

        public IRAPIClient(string client, string user, string password)
        {
            this.ClientInfo = client;
            this.ApiUser = user;
            this.ApiPassword = password;
        }

        private RestRequest Prepare(Method method, string path, object obj = null)
        {
            var request = new RestRequest(path, method);
            request.AddParameter("ClientInfo", this.ClientInfo, ParameterType.HttpHeader);
            request.AddParameter("Authorization", string.Format("Bearer {0}", this.AccessToken), ParameterType.HttpHeader);
            if (obj != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(obj);
            }
            return request;
        }

        private T Call<T>(Method method, string path, object obj = null) where T : new()
        {
            var client = new RestClient(IRAPIClient.ApiEndpointURL);
            IRestResponse<T> response = client.Execute<T>(Prepare(method,path,obj));
            return response.Data;
        }

        private object Call(Method method, string path, object obj = null)
        {
            var client = new RestClient(IRAPIClient.ApiEndpointURL);
            var response = client.Execute(Prepare(method, path,obj));
            var content = response.Content;
            dynamic returnObj = SimpleJson.DeserializeObject(content);
            return returnObj;
        }

        public T Post<T>(string path, object obj = null) where T : new()
        {
            return Call<T>(Method.POST, path, obj);
        }
        public T Get<T>(string path, object obj = null) where T : new()
        {
            return Call<T>(Method.GET, path, obj);
        }
        public T Delete<T>(string path, object obj = null) where T : new()
        {
            return Call<T>(Method.DELETE, path, obj);
        }
        public T Put<T>(string path, object obj = null) where T : new()
        {
            return Call<T>(Method.PUT, path, obj);
        }

        public object Post(string path, object obj = null)
        {
            return Call(Method.POST, path, obj);
        }
        public object Get(string path, object obj = null) 
        {
            return Call(Method.GET, path, obj);
        }
        public object Delete(string path, object obj = null)
        {
            return Call(Method.DELETE, path, obj);
        }
        public object Put(string path, object obj = null)
        {
            return Call(Method.PUT, path, obj);
        }

        public string GetToken()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                var client = new RestClient(IRAPIClient.ApiEndpointURL);

                var request = new RestRequest("/token", Method.POST);
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", this.ApiUser);
                request.AddParameter("password", this.ApiPassword);
                request.AddHeader("ClientInfo", this.ClientInfo);

                var response = client.Execute(request);
                var content = response.Content;

                dynamic returnObj = SimpleJson.DeserializeObject(content);
                this.AccessToken = returnObj.access_token;
            }
            return this.AccessToken;
        }
    }

    
  
}
