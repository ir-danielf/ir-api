using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace IRLib
{
    public class APIHuman : MarshalByRefObject
    {
        private string Codigo { get; set; }
        private string Senha { get; set; }
        private string Link { get; set; }
        private string From { get; set; }
        public string CorpoEmail { get; set; }
        public bool Ativo { get; set; }

        public APIHuman()
        {
            Senha = ConfiguracaoSMS.Instancia.Chaves.Senha.Valor;
            Ativo = ConfiguracaoSMS.Instancia.Chaves.Status.Valor;
            CorpoEmail = ConfiguracaoSMS.Instancia.Chaves.Corpo.Valor;
            Link = ConfiguracaoSMS.Instancia.Chaves.Link.Valor;
            Codigo = ConfiguracaoSMS.Instancia.Chaves.Login.Valor;
            From = ConfiguracaoSMS.Instancia.Chaves.From.Valor;
        }

        public void EnviarSms(string numero)
        {
            try
            {
                string[] info = new string[2];

                info[0] = Link;
                info[1] = "dispatch=send&account=" + Codigo + "&code=" + Senha + "&msg=" + CorpoEmail + "&to=55" + numero + "&from=" + From;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.InicarPage), info);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void EnviarSmsSync(string numero)
        {
            try
            {
                string[] info = new string[2];

                info[0] = Link;
                info[1] = "dispatch=send&account=" + Codigo + "&code=" + Senha + "&msg=" + CorpoEmail + "&to=55" + numero + "&from=" + From;

                this.InicarPage(info);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        private void InicarPage(object info)
        {
            try
            {
                string[] infos = new string[2];

                if (info is Array)
                    infos = (string[])info;

                GetPage(infos);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static string GetPage(string[] info)
        {
            // Declarações necessárias 
            Stream requestStream = null;
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                WebRequest request = WebRequest.Create(info[0]);
                request.Method = WebRequestMethods.Http.Post;

                // Neste ponto, você está setando a propriedade ContentType da página 
                // para urlencoded para que o comando POST seja enviado corretamente 
                request.ContentType = "application/x-www-form-urlencoded";

                StringBuilder urlEncoded = new StringBuilder();

                // Separando cada parâmetro 
                Char[] reserved = { '?', '=', '&' };

                // alocando o bytebuffer 
                byte[] byteBuffer = null;

                // caso a URL seja preenchida 
                if (info[1] != null)
                {
                    int i = 0, j;
                    // percorre cada caractere da url atraz das palavras reservadas para separação 
                    // dos parâmetros 
                    while (i < info[1].Length)
                    {
                        j = info[1].IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            urlEncoded.Append(info[1].Substring(i, info[1].Length - i));
                            break;
                        }
                        urlEncoded.Append(info[1].Substring(i, j - i));
                        urlEncoded.Append(info[1].Substring(j, 1));
                        i = j + 1;
                    }
                    // codificando em UTF8 (evita que sejam mostrados códigos malucos em caracteres especiais 
                    byteBuffer = Encoding.Default.GetBytes(urlEncoded.ToString());

                    request.ContentLength = byteBuffer.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(byteBuffer, 0, byteBuffer.Length);
                    requestStream.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }

                // Dados recebidos 
                response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                // Codifica os caracteres especiais para que possam ser exibidos corretamente 
                System.Text.Encoding encoding = System.Text.Encoding.Default;

                // Preenche o reader 
                reader = new StreamReader(responseStream, encoding);

                Char[] charBuffer = new Char[256];
                int count = reader.Read(charBuffer, 0, charBuffer.Length);

                String Dados = "";

                // Lê cada byte para preencher minha string
                while (count > 0)
                {
                    Dados += new String(charBuffer, 0, count);
                    count = reader.Read(charBuffer, 0, charBuffer.Length);
                }

                return Dados;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                // Fecha tudo 
                if (requestStream != null) requestStream.Close();
                if (response != null) response.Close();
                if (reader != null) reader.Close();
            }
        }
    }
}
