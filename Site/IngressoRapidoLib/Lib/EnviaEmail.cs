using IngressoRapido.TemplateParser;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for EnviaEmail
    /// </summary>
    public class EnviaEmail
    {
        public bool EnviarAlerta(string assunto, string corpoEmail)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("noreply@ingressorapido.com.br", "Ingressorapido.com.br - Alerta do Site");
                message.To.Add(new MailAddress("evandro.araujo@celeritas.com.br", "Evandro"));
                message.To.Add(new MailAddress("sac@ingressorapido.com.br"));
                message.To.Add(new MailAddress("internet@ingressorapido.com.br"));
               
                message.Subject = assunto;
                message.BodyEncoding = Encoding.Default;
                message.Body = corpoEmail;
                message.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.ingressorapido.com.br");
                System.Net.NetworkCredential MailCredential = new System.Net.NetworkCredential("noreply@ingressorapido.com.br", "abcd123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = MailCredential;

                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao enviar e-mail: " + ex);
                //return false;
            }
        }

        public bool EnviarAlerta(string assunto, Parser corpoEmail)
        {
            return EnviarAlerta(assunto, corpoEmail.Parse());
        }      
 
        public bool Enviar(MailAddress remetente, MailAddress destinatario, string assunto, string corpoEmail)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = remetente;
                message.To.Add(destinatario);
                message.Bcc.Add(new MailAddress("arquivo@ingressorapido.com.br", "Arquivo do Site"));
                message.Subject = assunto;
                message.BodyEncoding = Encoding.Default;
                message.Body = corpoEmail;
                message.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.ingressorapido.com.br");
                smtp.EnableSsl = false;
                string emailRemetente = ConfigurationManager.AppSettings["EmailRemetente"].ToString();
                string emailSenha = ConfigurationManager.AppSettings["EmailSenha"].ToString();
                smtp.Credentials = new System.Net.NetworkCredential(emailRemetente, emailSenha);
                smtp.Send(message);

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception ("Falha ao enviar e-mail: " + ex);
                //return false;
            }
        }
        
        public bool Enviar(MailAddress remetente, MailAddress destinatario, string assunto, Parser corpoEmail)
        {
            return Enviar(remetente, destinatario, assunto, corpoEmail.Parse());
        }

        public bool Enviar(MailAddress destinatario, string assunto, Parser corpoEmail)
        {
            return Enviar(new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"].ToString(), "Ingresso Rápido", Encoding.UTF8), destinatario, assunto, corpoEmail.Parse());
        }
        
        public bool Enviar(MailAddress destinatario, string assunto, string corpoEmail)
        {
            return Enviar(new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"].ToString(), "IngressoRápido", Encoding.UTF8), destinatario, assunto, corpoEmail);
        }

        public bool EnviarCC(MailAddress remetente, MailAddress destinatario, MailAddressCollection destinatariosCC, string assunto, string corpoEmail)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = remetente;
                message.To.Add(destinatario);
                foreach (MailAddress destinatarioCC in destinatariosCC)
                    message.CC.Add(destinatarioCC);

                message.Subject = assunto;
                message.BodyEncoding = Encoding.Default;
                message.Body = corpoEmail;
                message.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.ingressorapido.com.br");
                smtp.EnableSsl = false;
                string emailRemetente = ConfigurationManager.AppSettings["EmailRemetente"].ToString();
                string emailSenha = ConfigurationManager.AppSettings["EmailSenha"].ToString();
                smtp.Credentials = new System.Net.NetworkCredential(emailRemetente, emailSenha);
                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao enviar e-mail: " + ex);
                //return false;
            }
        }
        
        public bool EnviarCC(MailAddress remetente, MailAddress destinatario, MailAddressCollection destinatariosCC, string assunto, Parser corpoEmail)
        {
            return EnviarCC(remetente, destinatario, destinatariosCC, assunto, corpoEmail.Parse());

        }
        
        public bool EnviarCC(MailAddress destinatario, MailAddressCollection destinatariosCC, string assunto, Parser corpoEmail)
        {
            return EnviarCC(new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"].ToString(), "Ingresso Rápido", Encoding.UTF8), destinatario, destinatariosCC, assunto, corpoEmail.Parse());
        }

        private bool Enviar_OLD(MailAddress from, MailAddress to, string assunto, string corpo, string template, bool isHtml)
        {
            MailMessage msg = new MailMessage();

            msg.IsBodyHtml = isHtml;
            msg.BodyEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            msg.SubjectEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

            //msgMail.From = from;
            msg.From = new MailAddress("fabiano.lira@celeritas.com.br", "Sing It.");
            msg.To.Add(to);

            msg.Subject = assunto;

            // we replace the newline with it's html equivalent in the message
            corpo = corpo.Replace("\r\n", "<br />");

            // then we place our message in the mail template by replacing
            // the 'placemailhere' string by our message if it exists.
            if (template.ToLower().Contains("placemailhere"))
            {
                msg.Body = template.Replace("placemailhere", corpo);
            }
            else
            {
                msg.Body = corpo;
            }

            SmtpClient smtp = new SmtpClient("localhost", 25);
            //smtp.Host = System.Web.Configuration.WebConfigurationManager.AppSettings["smtphost"];
            smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
            smtp.UseDefaultCredentials = true;

            try
            {
                smtp.Send(msg);
            }
            catch (SmtpException ex)
            {
                //Response.Write("Ocorreram problemas no envio do e-mail. Error = " + ex.Message); 
                throw ex;                
            }
            return true;
        }

    }
}