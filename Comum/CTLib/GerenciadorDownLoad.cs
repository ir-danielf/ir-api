using System;
using System.IO;
using System.Net;

namespace CTLib
{

    public class GerenciadorDownLoad
    {
        public void Download(string dirTemp, string nomeArquivo, string extensao, string localServidor, string novoLocal, string nomeNovo)
        {
            bool atualizado = false;
            try
            {

                nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(nomeArquivo);

                WebClient clienteWeb = new WebClient();
                if (File.Exists(dirTemp + nomeArquivo + extensao))
                    File.Delete(dirTemp + nomeArquivo + extensao);
                try
                {
                    clienteWeb.DownloadFile(localServidor + nomeArquivo + extensao, dirTemp + nomeArquivo + extensao);
                    atualizado = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possivel encontrar o arquivo: " + ex.Message);
                }
                try
                {
                    if (atualizado)
                    {
                        File.Copy(dirTemp + nomeArquivo + extensao, novoLocal + nomeNovo + extensao, true);
                    }
                    System.Diagnostics.Process.Start(novoLocal + nomeNovo + extensao);
                }
                catch(Exception)
                {
                    throw new Exception("Não foi possível Salvar o arquivo");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DownloadHtml(string dirTemp, string nomeArquivo, string extensao, string localServidor, string novoLocal, string nomeNovo)
        {
            bool atualizado = false;
            try
            {
                nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(nomeArquivo);

                WebClient clienteWeb = new WebClient();
                if (File.Exists(dirTemp + nomeArquivo + extensao))
                    File.Delete(dirTemp + nomeArquivo + extensao);
                try
                {
                    clienteWeb.DownloadFile(localServidor + nomeArquivo + extensao, dirTemp + nomeArquivo + extensao);
                    atualizado = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possivel encontrar o arquivo: " + ex.Message);
                }
                try
                {
                    if (atualizado)
                    {
                        File.Copy(dirTemp + nomeArquivo + extensao, novoLocal + nomeNovo + extensao, true);
                    }
                }
                catch
                {
                    throw new Exception("Não foi possível Salvar o arquivo");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
