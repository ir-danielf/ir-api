using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IRLib
{
    public class GerenciarEmailsAssinatura
    {
        private List<EstruturaEmailAssinatura> ListaEnvios { get; set; }
        private BD bd = new BD();

        public GerenciarEmailsAssinatura()
        {
            ListaEnvios = new List<EstruturaEmailAssinatura>();
        }
        public void Enviar()
        {
            this.Buscar();

            if (ListaEnvios.Count == 0)
                return;

            int c = 0;
            while (ListaEnvios.Count > 0)
            {
                var envio = ListaEnvios.FirstOrDefault();
                try
                {
                    ServicoEmail.EnviarEmailAssinaturas(envio.ClienteEmail, envio.Assunto, envio.Corpo);

                    bd.Executar(
                        string.Format("UPDATE tAssinaturaEmailEnviar SET DataEnvio = '{0}', Enviado = '{1}' WHERE ID = {2}", DateTime.Now.ToString("yyyyMMddHHmmss"), "T", envio.EnvioID));

                    ListaEnvios.Remove(envio);
                }
                catch (Exception ex)
                {
                    ListaEnvios.Remove(envio);
                    try
                    {
                        bd.Executar(
                            string.Format("UPDATE tAssinaturaEmailEnviar SET DataEnvio = '{0}', Erro = '{1}' WHERE ID = {2}", DateTime.Now.ToString("yyyyMMddHHmmss"), ex.Message.ToSafeString(), envio.EnvioID));
                    }
                    catch
                    {
                        ListaEnvios.Remove(envio);
                    }
                }

                c++;

                //Respiro
                if (c % 10 == 0)
                    Thread.Sleep(Temporizador.Instancia.EmailAssinaturas.Respiro.Valor);
            }

        }

        private void Buscar()
        {
            try
            {
                if (ListaEnvios.Count > 0)
                    return;

                string sql =
                    @"
                        SELECT
	                        aee.ID AS EnvioID, aer.Remetente, aer.NomeExibicao, aer.Senha, aer.SMTP, c.Nome AS ClienteNome, c.Email AS ClienteEmail, aem.Assunto, aem.Corpo
	                    FROM tAssinaturaEmailEnviar aee (NOLOCK)
	                    INNER JOIN tAssinaturaEmailModelo aem (NOLOCK) ON aee.AssinaturaEmailModeloID = aem.ID
	                    INNER JOIN tAssinaturaEmailRemetente aer (NOLOCK) ON aer.ID = aem.AssinautraEmailRemetenteID
	                    INNER JOIN tCliente c (NOLOCK) ON c.ID = aee.ClienteID
	                    WHERE aee.Enviado = 'F'
                    ";

                if (!bd.Consulta(sql).Read())
                    return;

                do
                {
                    ListaEnvios.Add(new EstruturaEmailAssinatura()
                    {
                        EnvioID = bd.LerInt("EnvioID"),
                        Remetente = bd.LerString("Remetente"),
                        NomeExibicao = bd.LerString("NomeExibicao"),
                        Senha = bd.LerString("Senha"),
                        SMTP = bd.LerString("SMTP"),
                        ClienteNome = bd.LerString("ClienteEmail"),
                        ClienteEmail = bd.LerString("ClienteEmail"),
                        Corpo = bd.LerString("Corpo"),
                        Assunto = bd.LerString("Assunto"),
                    });
                } while (bd.Consulta().Read());
            }
            finally
            {
                bd.Fechar();
            }
        }

    }
}
