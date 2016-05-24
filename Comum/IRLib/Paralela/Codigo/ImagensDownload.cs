using CTLib;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace IRLib.Paralela
{

    [ObjectType(ObjectType.RemotingType.SingleCall)]
    [JirayaObject]
    public class ImagensDownloadParalela : MarshalByRefObject
    {

        /// <summary>
        /// Criado por Caio Maganha Rosa
        /// Data: 21/09/2009
        /// Metodo que obtem por .net remoting o path das imagens para download
        /// </summary>
        /// <param name="tipoDownload"></param>
        /// <returns></returns>
        public string GetPath(ImagensGerenciarServidor.Tipo tipoDownload)
        {
            try
            {
                string Path = string.Empty;

                switch (tipoDownload)
                {
                    case ImagensGerenciarServidor.Tipo.Banner:
                        Path = ConfigurationManager.AppSettings["DownloadPathBanners"];
                        break;
                    case ImagensGerenciarServidor.Tipo.Evento:
                        Path = ConfigurationManager.AppSettings["DownloadPathEventos"];
                        break;
                    case ImagensGerenciarServidor.Tipo.Ingresso:
                        Path = ConfigurationManager.AppSettings["DownloadPathIngressos"];
                        break;
                    case ImagensGerenciarServidor.Tipo.IngressoImpressor:
                        Path = ConfigurationManager.AppSettings["DownloadPathIngressos"];
                        break;
                    case ImagensGerenciarServidor.Tipo.Mapa:
                        Path = ConfigurationManager.AppSettings["DownloadPathMapas"];
                        break;
                    case ImagensGerenciarServidor.Tipo.ValeIngresso:
                        Path = ConfigurationManager.AppSettings["DownloadPathValeIngresso"];
                        break;
                    case ImagensGerenciarServidor.Tipo.ValeIngressoInternet:
                        Path = ConfigurationManager.AppSettings["DownloadPathValeIngressoInternet"];
                        break;
                    case ImagensGerenciarServidor.Tipo.BackgroundSetor:
                        Path = ConfigurationManager.AppSettings["DownloadPathBackgroundSetor"];
                        break;
                    case ImagensGerenciarServidor.Tipo.PerspectivaLugar:
                        Path = ConfigurationManager.AppSettings["DownloadPathPerspectivaLugar"];
                        break;
                    case ImagensGerenciarServidor.Tipo.MapaEsquematico:
                        Path = ConfigurationManager.AppSettings["DownloadPathMapaEsquematico"];
                        break;
                    case ImagensGerenciarServidor.Tipo.Serie:
                        Path = ConfigurationManager.AppSettings["DownloadPathSerie"];
                        break;
                    case ImagensGerenciarServidor.Tipo.LocalParaInternet:
                        Path = ConfigurationManager.AppSettings["DownloadPathLocal"];
                        break;
                    case ImagensGerenciarServidor.Tipo.Filme:
                        Path = ConfigurationManager.AppSettings["DownloadPathFilme"];
                        break;
                    case ImagensGerenciarServidor.Tipo.EventoPos:
                        Path = ConfigurationManager.AppSettings["DownloadPathEventosPos"];
                        break;                        
                }

                if (!string.IsNullOrEmpty(Path) && !Path.EndsWith("/") && !Path.EndsWith("\\"))
                    Path += "/";               

                return Path;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel encontrar o Caminho de Download das Imagen" + ex.Message);
            }
        }

        public static void Download(ImagensGerenciarServidor.Tipo tipoDownload, string dirTemp, string nomeArquivo, string extensao, string pastaDestinoLocal)
        {
            try
            {
                //string caminho = string.Empty;
                nomeArquivo = Path.GetFileNameWithoutExtension(nomeArquivo);

                WebClient clienteWeb = new WebClient();

                ImagensDownloadParalela getPath = new ImagensDownloadParalela();

                //Busca o caminho correto no metodo GetPath - .NetRemoting(SingleCall)
                var caminho = getPath.GetPath(tipoDownload);

                if (!caminho.EndsWith("/") && !caminho.EndsWith("\\"))
                    caminho += "/";

                //TODO: Alterar no metodo que chama o download para que a extensao jpg seja passada corretamente
                if (tipoDownload == ImagensGerenciarServidor.Tipo.Evento || tipoDownload == ImagensGerenciarServidor.Tipo.ValeIngressoInternet || tipoDownload == ImagensGerenciarServidor.Tipo.Serie || tipoDownload == ImagensGerenciarServidor.Tipo.LocalParaInternet)
                    extensao = ".jpg";
                else if (tipoDownload == ImagensGerenciarServidor.Tipo.BackgroundSetor)
                    extensao = ".gif";

                if (File.Exists(dirTemp + nomeArquivo + extensao))
                    File.Delete(dirTemp + nomeArquivo + extensao);

                try
                {
                    clienteWeb.DownloadFile(caminho + nomeArquivo + extensao, dirTemp + nomeArquivo + extensao);
                    File.Copy(dirTemp + nomeArquivo + extensao, Application.StartupPath + "\\Imagens\\" + nomeArquivo + extensao, true);
                }
                catch
                {

                }
            }
            catch (WebException)
            {
                if (File.Exists(Application.StartupPath + "\\Imagens\\" + nomeArquivo + extensao))
                    File.Delete(Application.StartupPath + "\\Imagens\\" + nomeArquivo + extensao);

                throw new WebException("Arquivo nao existe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
