using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Web
{
    public static class RequestExtensions
    {

        public static string GetUserIPAddress(this HttpRequestBase request)
        {
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        
        public static List<string> GetClientIPAddress(this HttpRequest request)
        {
            List<string> ipAddress;
            if (!string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',').Select(t => t.Trim()).ToList();
            }
            else
            {
                ipAddress = new List<string>();
            }
            if (!ipAddress.Contains(request.ServerVariables["REMOTE_ADDR"]))
            {
                ipAddress.Add(request.ServerVariables["REMOTE_ADDR"]);
            }
            return ipAddress;
        }

        public static string GetClientReferer(this HttpRequest request)
        {
            if (request.UrlReferrer != null)
            {
                string url = request.UrlReferrer.Scheme + "://" + request.UrlReferrer.Host;
                if (((request.UrlReferrer.Scheme == "http") && (request.UrlReferrer.Port != 80)) || ((request.UrlReferrer.Scheme == "https") && (request.UrlReferrer.Port != 443)))
                {
                    url += ":" + request.UrlReferrer.Port;
                }

            }
            return null;
        }

        public static CultureInfo GetBrowserCulture(this HttpRequestBase request)
        {
            string[] languages = request.UserLanguages;

            if (languages == null || languages.Length == 0)
                return null;

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

    }
}