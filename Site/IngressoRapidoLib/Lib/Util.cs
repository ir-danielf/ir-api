using CTLib;
using System;
using System.IO;
using System.Net;

namespace IngressoRapido.Lib
{
    public class Util
    {
        public static string ToTitleCase(string texto)
        {

            return texto;
            //return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(texto.ToLower());
        }


        public static void ReturnJS(string script, System.Web.UI.Page pagina)
        {
            pagina.RegisterStartupScript("js", "<script type=\"text/javascript\">" + script + "</script>");
        }

        public static void ReturnAjaxJS(string script, System.Web.UI.UpdatePanel updatePanel)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "js", script, true);
        }

        public static void OpenWindow(string url, int width, int height, System.Web.UI.UpdatePanel updatePanel)
        {
            string script = "window.open('" + url + "', 'window', 'toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,screenX=0,screenY=0,left=0,top=0,width=" + width + ",outerWidth=" + width + ",height=" + height + "');";

            Util.ReturnAjaxJS(script, updatePanel);
        }

        public static void OpenWindow(string url, System.Web.UI.UpdatePanel updatePanel)
        {
            string script = "w = screen.availWidth - 7; h = screen.availHeight - 57; window.open('" + url + "', 'window', 'toolbar=no,location=yes,status=no,menubar=no,scrollbars=yes,resizable=no,left=0,top=0,width='+w+',height='+h);";
            Util.ReturnAjaxJS(script, updatePanel);

        }

        public static void OpenWindow(string url, System.Web.UI.Page pagina)
        {
            string script = "w = screen.availWidth - 7; h = screen.availHeight - 57; window.open('" + url + "', 'window', 'toolbar=no,location=yes,status=no,menubar=no,scrollbars=yes,resizable=no,left=0,top=0,width='+w+',height='+h);";

            Util.ReturnJS(script, pagina);
        }

        public static void OpenModal(string url, System.Web.UI.UpdatePanel updatePanel)
        {
            string script = "window.showModalDialog('" + url + "', window,'','')";

            Util.ReturnAjaxJS(script, updatePanel);

            #region Modal em JS
            //function openModal(pUrl, pWidth, pHeight) {
            //if (window.showModalDialog) {
            //    return window.showModalDialog(pUrl, window,
            //      "dialogWidth:" + pWidth + "px;dialogHeight:" + pHeight + "px");
            //} else {
            //    try {
            //        netscape.security.PrivilegeManager.enablePrivilege(
            //          "UniversalBrowserWrite");
            //        window.open(pUrl, "wndModal", "width=" + pWidth
            //          + ",height=" + pHeight + ",resizable=no,modal=yes");
            //        return true;
            //    }
            //    catch (e) {
            //        alert("Script não confiável, não é possível abrir janela modal.");
            //        return false;
            //        }
            //    }
            //}
            #endregion
        }

        public static void AlertaJs(string msg, System.Web.UI.Page pagina)
        {
            pagina.RegisterStartupScript("alerta", "<script>window.alert('" + msg + "')</script>");
        }



        public static void AjaxAlertJs(System.Web.UI.UpdatePanel updatePanel, string msg)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "alerta", "alert('" + msg + "');", true);
        }

        public static void AjaxAlertJS(System.Web.UI.Page page, string msg)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "alert('" + msg + "');", true);
        }


        public static void RedirectAjaxJS(System.Web.UI.UpdatePanel updatePanel, string url)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "x", "window.location.href = '" + url + "'", true);
        }

        public static void RedirectJS(System.Web.UI.Page pagina, string url)
        {
            //pagina.RegisterStartupScript("x", "<script>window.parent.content.location.href = " + url + "</script>");
            pagina.ClientScript.RegisterStartupScript(pagina.GetType(), "x", "<script>window.location.href = '" + url + "';</script>");
        }

        public static bool IsInt(string number)
        {
            try
            {
                int.Parse(number);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTime(string datetime)
        {
            try
            {
                DateTime.ParseExact(datetime, "dd/MM/yyyy", null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // formata decimal para enviar para a Redecard
        public static string FormataValorRedecard(Decimal valor)
        {
            return valor.ToString("f").Replace(",", "").Replace(".", "");
        }

        public static string HTTPGetPage(string valores)
        {
            /*
             * EXEMPLO DE UTILIZAÇÃO
            string fonte = Util.HTTPGetPage("https://comercio.locaweb.com.br/comercio.comp?merchid=ingressorapido&price=0001&damount=Easasd&tid=73489720018193711001&orderid=1&order=Teste&bin=455187&authenttype=1&visa_antipopup=0&PosicaoDadosVisanet=0");
            Response.Write(fonte);                        
            */

            WebRequest mywebReq = null;
            WebResponse mywebResp = null;
            StreamReader sr = null;
            string strHTML;

            try
            {
                mywebReq = WebRequest.Create(valores);
                mywebResp = mywebReq.GetResponse();
                sr = new StreamReader(mywebResp.GetResponseStream());
                strHTML = sr.ReadToEnd();
                return strHTML;
            }
            catch (WebException ex)
            {
                throw new Exception("no http" + ex.Message);
            }
            finally
            {
                if (mywebResp != null)
                    mywebResp.Close();
                if (sr != null)
                {
                    sr.Close();

                    sr.Dispose();
                }
            }
        }

        public static string HTTPPostPage(string url, string strPost)
        {
            String result = "";
            //String strPost = "x=1&y=2&z=YouPostedOk";
            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (myWriter != null)
                    myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();

                // Close and clean up the StreamReader
                sr.Close();
            }

            return result;
        }

        public static string LimparTitulo(string texto)
        {
            if (texto.StartsWith("'") || texto.StartsWith("!") || texto.StartsWith(".") || texto.StartsWith("_"))
            {
                do
                {
                    texto = texto.Remove(0, 1);
                } while (texto.StartsWith("'") || texto.StartsWith("!") || texto.StartsWith(".") || texto.StartsWith("_"));
            }
            if (texto.EndsWith("'") || texto.EndsWith("."))
            {
                do
                {
                    texto = texto.Remove(texto.Length - 1, 1);
                } while (texto.EndsWith("'") || texto.EndsWith("."));
            }
            //for (int i = 0; i < texto.Length; i++)
            //{
            //    chr = char.Parse(texto.Substring(0, 1));
            //    if (chr == '.' || chr == ' ' || chr == '_' || chr == '-' || chr == '!')
            //        texto = texto.Substring(1);
            //    else
            //        return texto;
            //}
            return texto;
        }

        public static string AnalisaString(string strTemp)
        {
            if (strTemp == null)
                return "";
            else
                return strTemp;
        }

        public static void EfetuarRequest(string request)
        {
            BD bd = new BD();
            string retorno = string.Empty;
            try
            {
                retorno = bd.ConsultaValor(request).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new Exception(retorno);
        }

        public static string StringToBD(string str)
        {
            str = str.Replace("'", "''");

            return str;
        }

        public static string DecimalToBD(Decimal valor)
        {
            return valor.ToString("f").Replace(",", ".");
        }

        public static string AmountToDecimalBD(string str)
        {
            return str.Insert(str.Length - 2, ".");
        }

        public static string TrazerEstadoExtenso(string uf)
        {
            string strEstado = "";

            if (uf.ToUpper() == "GO") strEstado = "Goiás";
            if (uf.ToUpper() == "MT") strEstado = "Mato Grosso";
            if (uf.ToUpper() == "MS") strEstado = "Mato Grosso do Sul";
            if (uf.ToUpper() == "DF") strEstado = "Distrito Federal";
            if (uf.ToUpper() == "AM") strEstado = "Amazonas";
            if (uf.ToUpper() == "AC") strEstado = "Acre";
            if (uf.ToUpper() == "RO") strEstado = "Rondônia";
            if (uf.ToUpper() == "RR") strEstado = "Roraima";
            if (uf.ToUpper() == "AP") strEstado = "Amapá";
            if (uf.ToUpper() == "TO") strEstado = "Tocantins";
            if (uf.ToUpper() == "PA") strEstado = "Pará";
            if (uf.ToUpper() == "MA") strEstado = "Maranhão";
            if (uf.ToUpper() == "PI") strEstado = "Piauí";
            if (uf.ToUpper() == "CE") strEstado = "Ceará";
            if (uf.ToUpper() == "RN") strEstado = "Rio Grande do Norte";
            if (uf.ToUpper() == "PB") strEstado = "Paraíba";
            if (uf.ToUpper() == "PE") strEstado = "Pernambuco";
            if (uf.ToUpper() == "SE") strEstado = "Sergipe";
            if (uf.ToUpper() == "AL") strEstado = "Alagoas";
            if (uf.ToUpper() == "BA") strEstado = "Bahia *";
            if (uf.ToUpper() == "SP") strEstado = "São Paulo *";
            if (uf.ToUpper() == "MG") strEstado = "Minas Gerais";
            if (uf.ToUpper() == "RJ") strEstado = "Rio de Janeiro *";
            if (uf.ToUpper() == "ES") strEstado = "Espírito Santo";
            if (uf.ToUpper() == "PR") strEstado = "Paraná *";
            if (uf.ToUpper() == "SC") strEstado = "Santa Catarina *";
            if (uf.ToUpper() == "RS") strEstado = "Rio Grande do Sul";

            return strEstado;
        }

        public static string CortaTexto(string texto, int tamanho)
        {
            return texto.Substring(0, texto.Length < tamanho ? texto.Length : tamanho);
        }


        public static System.Web.UI.HtmlControls.HtmlGenericControl Dictionary(string Cultura)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl Ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl();

            string linguagem = "Pt";

            //TODO: TIRAR QUANDO FOR INTERNACIONALIZAR
            //switch (Cultura)
            //{
            //    case "en-US":
            //        linguagem = "En";
            //        break;
            //    case "es-ES":
            //        linguagem = "Es";
            //        break;
            //    default:
            //        linguagem = "Pt";
            //        break;
            //}



            Ctrl.TagName = "script";
            Ctrl.ID = "Dictionary";
            Ctrl.Attributes.Add("src", "js/Dictionary/dictionary" + linguagem + ".js");


            return Ctrl;
        }
    }
}



