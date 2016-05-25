using IRCore.Util;
using IRCore.Util.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web
{
    public static class SessionExtensions
    {

        public static T GetModel<T>(this HttpSessionStateBase value) where T : class
        {
            T sessionModel = null;
            string sessionKey = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.sessionModelKey);
            if (value[sessionKey] != null)
            {
                sessionModel = (T)value[sessionKey];
            }
            else
            {
                sessionModel = (T)Activator.CreateInstance(typeof(T));
            }
            return sessionModel;
        }

        public static void SaveModel<T>(this HttpSessionStateBase value, T sessionModel) where T : class
        {
            string sessionKey = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.sessionModelKey);
            value[sessionKey] = sessionModel;
        }

        public static void ClearModel(this HttpSessionStateBase value)
        {
            string sessionKey = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.sessionModelKey);
            value.Remove(sessionKey);
        }

    }
}