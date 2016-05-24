using System;
using System.IO;
using System.Web;
using IRCore.Util;

namespace IRAPI.Util
{
    public static class LogHttpUtil
    {
        public static void LogRequest()
        {
            TryLogUserName();
            TryLogUrl();
            TryLogRequestContent();
        }

        public static bool TryLogUserName()
        {
            try
            {
                LogUserName();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryLogUrl()
        {
            try
            {
                LogUrl();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryLogRequestContent()
        {
            try
            {
                LogRequestContent();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void LogUserName()
        {
            var user = HttpContext.Current.User.Identity.Name;
            LogUtil.Error(string.Format("User {0}", user));
        }

        private static void LogUrl()
        {
            var url = string.Format("Url {0} {1}", HttpContext.Current.Request.CurrentExecutionFilePath, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            LogUtil.Error(url);
        }

        private static void LogRequestContent()
        {
            var streamReader = new StreamReader(HttpContext.Current.Request.InputStream);
            var requestContent = streamReader.ReadToEnd();

            if (!string.IsNullOrEmpty(requestContent))
                LogUtil.Error(string.Format("Request Content: {0}", requestContent));
        }
    }
}