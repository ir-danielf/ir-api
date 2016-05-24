using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace IRCore.Web.MVC
{
    public class MasterControllerBase : Controller
    {
        public JavaScriptResult VariaveisJs()
        {
            return JavaScript(((Controller)this).RenderPartialViewToString("VariaveisJs", null));
        }

        public void SetCulture(string parametro, bool usaNagevador = true, bool guardaCookie = true)
        {
            bool hasCookie = false;
            HttpCookie cultureCookie = null;

            if (guardaCookie)
            {
                cultureCookie = Request.Cookies["cookie_" + parametro];
                hasCookie = (cultureCookie != null);
                if (!hasCookie)
                {
                    cultureCookie = new HttpCookie("cookie_" + parametro);
                }
                cultureCookie.Expires = DateTime.Now.AddYears(1);
            }


            CultureInfo culture = null;

            // Tenta pegar da rota ou do get
            if (ControllerContext.RouteData.Values.ContainsKey(parametro))
            {
                culture = GetCulture((string)ControllerContext.RouteData.Values[parametro]);
            }
            else if (Request.Params[parametro] != null)
            {
                culture = GetCulture(Request.Params[parametro]);
            }

            // Caso não tenha pego a cultura do Get Tenta pegar a cultura do cookie
            if ((culture == null) && (hasCookie) && (guardaCookie))
            {
                culture = GetCulture(cultureCookie.Value);
            }

            // Caso não tenha pego a cultura do Get, Cookie Tenta pegar a cultura do Browser
            if ((culture == null) && (usaNagevador))
            {
                culture = GetCultureFromBrowser();
            }

            // Caso não tenha pego a cultura do Get, Cookie e Browser Tenta pegar a cultura padrão setada no AssembyInfo
            if (culture == null)
            {
                culture = Assembly.GetExecutingAssembly().GetName().CultureInfo;
            }

            // Seta Cultira escolhida na Thred
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            if (guardaCookie)
            {
                // Salva Cultura escolhida na Sessao
                cultureCookie.Value = culture.Name;
                Response.SetCookie(cultureCookie);
            }

        }

        private CultureInfo GetCultureFromBrowser()
        {
            try
            {
                return Request.GetBrowserCulture();
            }
            catch { }
            return null;
        }

        private CultureInfo GetCulture(string cultureName)
        {
            try
            {
                return new CultureInfo(cultureName);
            }
            catch { }
            return null;
        }

    }


}
