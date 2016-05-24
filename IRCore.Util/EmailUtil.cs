using IRCore.Util.Enumerator;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace IRCore.Util
{
    public static class EmailUtil
    {

        //public static void Send(string from, string to, string subject, string message)
        //{
        //    MailMessage mail = new MailMessage(from, to, subject, message);
        //    mail.IsBodyHtml = true;
        //    EmailUtil.Send(mail);
        //}
        //public static void Send(string to, string subject, string message)
        //{
        //    EmailUtil.Send(ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.emailDefaultSender), to, subject, message);
        //}

        //public static void Send(MailMessage mail)
        //{
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.emailHost);
        //    smtp.Port = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoGeral.emailPort);
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    if (string.IsNullOrWhiteSpace(ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.emailUser)) || string.IsNullOrWhiteSpace(ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.emailPass)))
        //    {
        //        smtp.UseDefaultCredentials = true;
        //    }
        //    else
        //    {
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.emailUser), ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.emailPass));

        //    }
        //    smtp.EnableSsl = ConfiguracaoAppUtil.GetAsBool(enumConfiguracaoGeral.emailSSL);
        //    smtp.Send(mail);
        //}
    }
}