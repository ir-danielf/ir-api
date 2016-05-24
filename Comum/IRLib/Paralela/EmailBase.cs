using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace IRLib.Paralela
{
    public class EmailBase : MarshalByRefObject
    {
        public string Remetente { get; set; }
        public string Para { get; set; }
        public List<string> Destinatarios { get; set; }
        public List<string> CC { get; set; }
        public List<string> CCO { get; set; }
        public string Smtp { get; set; }
        public string LoginSmtp { get; set; }
        public string SenhaSmtp { get; set; }
        public string CorpoEmail { get; set; }
        public string AssuntoEmail { get; set; }
        public bool HabilitarSsl { get; set; }
        public string ReplyTo { get; set; }
        public string NomeExibicao { get; set; }
        public Attachment Anexos { get; set; }

        public EmailBase()
        {
            Destinatarios = new List<string>();
            CC = new List<string>();
            CCO = new List<string>();
        }

        public void Enviar()
        {
            Enviar(false);
        }

        public void EnviarAsync()
        {
            Enviar(true);
        }

        private void Enviar(bool Async)
        {
            try
            {
                // valida o email
                bool bValidaEmail = ValidarConfiguracao();

                if (bValidaEmail)
                {
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(LoginSmtp, NomeExibicao);

                    if (!string.IsNullOrEmpty(this.ReplyTo))
                        message.ReplyTo = new MailAddress(ReplyTo, NomeExibicao);

                    message.To.Add(Para);

                    for (int i = 0; i < CCO.Count; i++)
                        message.Bcc.Add(new MailAddress(CCO[i]));

                    for (int i = 0; i < CC.Count; i++)
                        message.CC.Add(new MailAddress(CC[i]));

                    message.Subject = AssuntoEmail;
                    message.BodyEncoding = Encoding.UTF8;
                    message.Body = CorpoEmail;
                    message.IsBodyHtml = true;

                    if (Anexos != null)
                        message.Attachments.Add(Anexos);

                    SmtpClient smtp = new SmtpClient(Smtp);
                    System.Net.NetworkCredential MailCredential = new System.Net.NetworkCredential(LoginSmtp, SenhaSmtp);
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = MailCredential;

                    if (Async)
                        smtp.SendAsync(message, null);
                    else
                        smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidarConfiguracao()
        {
            Regex oRegex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            bool validado = true;
            try
            {
                for (int i = 0; i < Destinatarios.Count; i++)
                {
                    if (!oRegex.IsMatch(Destinatarios[i]))
                    {
                        validado = false;
                        throw new Exception("O email do cliente está em um formato inválido.");
                    }
                }

                for (int i = 0; i < CC.Count; i++)
                {
                    if (!oRegex.IsMatch(CC[i]))
                    {
                        validado = false;
                        throw new Exception("O email de cópia está em um formato inválido.");
                    }
                }

                for (int i = 0; i < CCO.Count; i++)
                {
                    if (!oRegex.IsMatch(CCO[i]))
                    {
                        validado = false;
                        throw new Exception("O email de cópia oculta está em um formato inválido.");
                    }
                }

                if (Smtp == null || Smtp == "")
                {
                    validado = false;
                    throw new Exception("Endereço do SMTP vazio.");
                }

                if (CorpoEmail == null || CorpoEmail == "")
                {
                    validado = false;
                    throw new Exception("Arquivo do email não encontrado.");
                }

                return validado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
