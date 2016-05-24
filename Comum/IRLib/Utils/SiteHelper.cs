using System;
using System.Configuration;
using System.Web;
using IRAPI.SDK;
using IRAPI.SDK.Model;

namespace IRLib.Utils
{
    public class SiteHelper
    {
        private static SiteHelper _siteHelper;

        public static SiteHelper GetInstanceSingleton()
        {
            if (_siteHelper != null)
                return _siteHelper;

            _siteHelper = new SiteHelper();
            return _siteHelper;
        }

        public static IRAPIClient GetStaticApi()
        {
            var clientAPI = new IRAPIClient("NetPromoterService", ConfigurationManager.AppSettings["usuarioAPI"], ConfigurationManager.AppSettings["senhaAPI"]);
            clientAPI.AccessToken = clientAPI.GetToken();

            return clientAPI;
        }

        public static NPSClient GetStaticApiNPS()
        {
            var clientAPI = new NPSClient("NetPromoterService", ConfigurationManager.AppSettings["usuarioNPS"], ConfigurationManager.AppSettings["senhaNPS"]);

            return clientAPI;
        }
    }
}