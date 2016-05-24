using CTLib;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;

namespace IRLib
{

    /// <summary>
    /// Criado por : Caio Maganha
    /// Data: 14/09/2009
    /// Utilização: Nova Forma de UPLOAD de Arquivos, utilizando .Net Remoting
    /// </summary>
    [ObjectType(ObjectType.RemotingType.CAO)]
    public class ImagensGerenciarServidor : MarshalByRefObject, ISponsoredObject
    {
        public ImagensGerenciarServidor()
        { }
        public enum Tipo
        {
            Evento,
            EventoPos,
            Ingresso,
            IngressoImpressor,
            Mapa,
            Banner,
            ValeIngresso,
            ValeIngressoInternet,
            BackgroundSetor,
            PerspectivaLugar,
            MapaEsquematico,
            Serie,
            LocalParaInternet,
            AssinaturaProgramacao,
            Destaque,
            DestaqueMobile,
            Filme,
            ElementoMapa
        }

        const string KEY_UPLOAD_PATH_EVENTOS = "PathEventos";
        readonly string PATH_EVENTOS = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_EVENTOS];

        const string KEY_UPLOAD_PATH_EVENTOS_POS = "PathEventosPos";
        readonly string PATH_EVENTOS_POS = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_EVENTOS_POS];

        const string KEY_UPLOAD_PATH_BANNERS = "PathBanners";
        readonly string PATH_BANNERS = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_BANNERS];

        const string KEY_UPLOAD_PATH_INGRESSOS = "PathIngressos";
        readonly string PATH_INGRESSOS = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_INGRESSOS];

        const string KEY_UPLOAD_PATH_MAPAS = "PathMapas";
        readonly string PATH_MAPAS = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_MAPAS];

        const string KEY_UPLOAD_PATH_VIR = "PathValeIngresso";
        readonly string PATH_VIR = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_VIR];

        const string KEY_UPLOAD_PATH_VIR_INTERNET = "PathValeIngressoInternet";
        readonly string PATH_VIR_INTERNET = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_VIR_INTERNET];

        const string KEY_UPLOAD_PATH_BACKGROUND_SETOR = "PathBackgroundSetor";
        readonly string PATH_BACKGROUND_SETOR = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_BACKGROUND_SETOR];

        const string KEY_UPLOAD_PATH_PERSPECTIVA_LUGAR = "PathPerspectivaLugar";
        readonly string PATH_PERSPECTIVA_LUGAR = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_PERSPECTIVA_LUGAR];

        const string KEY_UPLOAD_PATH_MAPA_ESQUEMATICO = "PathMapaEsquematico";
        readonly string PATH_MAPA_ESQUEMATICO = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_MAPA_ESQUEMATICO];

        const string KEY_UPLOAD_PATH_SERIE = "PathSerie";
        readonly string PATH_SERIE = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_SERIE];

        const string KEY_UPLOAD_PATH_LOCAL = "PathLocal";
        readonly string PATH_LOCAL = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_LOCAL];

        const string KEY_UPLOAD_PATH_ASSINATURA_PROGRAMACAO = "PathAssinaturaProgramacao";
        readonly string PATH_ASSINATURA_PROGRAMACAO = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_ASSINATURA_PROGRAMACAO];

        const string KEY_UPLOAD_PATH_DESTAQUE = "PathDestaque";
        readonly string PATH_DESTAQUE = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_DESTAQUE];

        const string KEY_UPLOAD_PATH_DESTAQUE_MOBILE = "PathDestaqueMobile";
        readonly string PATH_DESTAQUE_MOBILE = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_DESTAQUE_MOBILE];

        const string KEY_UPLOAD_PATH_FILME = "PathFilme";
        readonly string PATH_FILME = ConfigurationManager.AppSettings[KEY_UPLOAD_PATH_FILME];

        public void Upload(byte[] Arquivo, string NomeImagem, string Extensao, Tipo tipoUpload)
        {
            if (Extensao == ".exe" || Extensao == ".scr" || Extensao == ".msi")
                throw new Exception("Ooops, Extensão inválida");


            NomeImagem = Path.GetFileNameWithoutExtension(NomeImagem);
            MemoryStream mStream = null;
            FileStream fStream = null;
            try
            {
                mStream = new MemoryStream(Arquivo);

                string PATH_UPLOAD = string.Empty;

                switch (tipoUpload)
                {
                    case Tipo.Evento:
                        PATH_UPLOAD = PATH_EVENTOS;
                        break;
                    case Tipo.EventoPos:
                        PATH_UPLOAD = PATH_EVENTOS_POS;
                        break;
                    case Tipo.Banner:
                        PATH_UPLOAD = PATH_BANNERS;
                        break;
                    case Tipo.Ingresso:
                        PATH_UPLOAD = PATH_INGRESSOS;
                        break;
                    case Tipo.Mapa:
                        PATH_UPLOAD = PATH_MAPAS;
                        break;
                    case Tipo.ValeIngresso:
                        PATH_UPLOAD = PATH_VIR;
                        break;
                    case Tipo.ValeIngressoInternet:
                        PATH_UPLOAD = PATH_VIR_INTERNET;
                        break;
                    case Tipo.BackgroundSetor:
                        PATH_UPLOAD = PATH_BACKGROUND_SETOR;
                        break;
                    case Tipo.PerspectivaLugar:
                        PATH_UPLOAD = PATH_PERSPECTIVA_LUGAR;
                        break;
                    case Tipo.MapaEsquematico:
                        PATH_UPLOAD = PATH_MAPA_ESQUEMATICO;
                        break;
                    case Tipo.Serie:
                        PATH_UPLOAD = PATH_SERIE;
                        break;
                    case Tipo.LocalParaInternet:
                        PATH_UPLOAD = PATH_LOCAL;
                        break;
                    case Tipo.AssinaturaProgramacao:
                        PATH_UPLOAD = PATH_ASSINATURA_PROGRAMACAO;
                        break;
                    case Tipo.Destaque:
                        PATH_UPLOAD = PATH_DESTAQUE;
                        break;
                    case Tipo.DestaqueMobile:
                        PATH_UPLOAD = PATH_DESTAQUE_MOBILE;
                        break;
                    case Tipo.Filme:
                        PATH_UPLOAD = PATH_FILME;
                        break;
                }

                if (!Directory.Exists(PATH_UPLOAD))
                    Directory.CreateDirectory(PATH_UPLOAD);

                fStream = new FileStream(PATH_UPLOAD + "/" + NomeImagem + Extensao, FileMode.Create, FileAccess.Write);

                mStream.WriteTo(fStream);

                mStream.Close();
                fStream.Close();

                switch (tipoUpload)
                {
                    case Tipo.Evento:
                        Image thumb = null;

                        Image img = Image.FromFile(PATH_EVENTOS + "/" + NomeImagem + Extensao);
                        thumb = img.GetThumbnailImage(80, 80, null, IntPtr.Zero);

                        string thumbFile = (PATH_EVENTOS + "/" + NomeImagem + "thumb" + Extensao);
                        thumb.Save(thumbFile);
                        break;
                    case Tipo.ValeIngressoInternet:
                        Image thumbVir = null;
                        Image imgVir = Image.FromFile(PATH_VIR_INTERNET + "/" + NomeImagem + Extensao);

                        thumbVir = imgVir.GetThumbnailImage(80, 80, null, IntPtr.Zero);

                        string thumbFileVir = (PATH_VIR_INTERNET + "/" + NomeImagem + "thumb" + Extensao);
                        thumbVir.Save(thumbFileVir);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mStream != null)
                    mStream.Close();
                if (fStream != null)
                    fStream.Close();
            }
        }

        public void Remover(string NomeImagem, string Extensao, Tipo tipo)
        {
            try
            {
                NomeImagem = Path.GetFileNameWithoutExtension(NomeImagem);
                string caminho = string.Empty;
                switch (tipo)
                {
                    case Tipo.Evento:
                        caminho = PATH_EVENTOS + NomeImagem + Extensao;
                        break;
                    case Tipo.EventoPos:
                        caminho = PATH_EVENTOS_POS + NomeImagem + Extensao;
                        break;
                    case Tipo.Banner:
                        caminho = PATH_BANNERS + NomeImagem + Extensao;
                        break;
                    case Tipo.Ingresso:
                        caminho = PATH_INGRESSOS + NomeImagem + Extensao;
                        break;
                    case Tipo.Mapa:
                        caminho = PATH_MAPAS + NomeImagem + Extensao;
                        break;
                    case Tipo.ValeIngresso:
                        caminho = PATH_VIR + NomeImagem + Extensao;
                        break;
                    case Tipo.ValeIngressoInternet:
                        caminho = PATH_VIR_INTERNET + NomeImagem + Extensao;
                        break;
                    case Tipo.BackgroundSetor:
                        caminho = PATH_BACKGROUND_SETOR + NomeImagem + Extensao;
                        break;
                    case Tipo.PerspectivaLugar:
                        caminho = PATH_PERSPECTIVA_LUGAR + NomeImagem + Extensao;
                        break;
                    case Tipo.MapaEsquematico:
                        caminho = PATH_MAPA_ESQUEMATICO + NomeImagem + Extensao;
                        break;
                    case Tipo.Serie:
                        caminho = PATH_SERIE + NomeImagem + Extensao;
                        break;
                    case Tipo.LocalParaInternet:
                        caminho = PATH_LOCAL + NomeImagem + Extensao;
                        break;
                    case Tipo.Filme:
                        caminho = PATH_FILME + NomeImagem + Extensao;
                        break;
                }

                if (!File.Exists(caminho))
                    return;

                File.Delete(caminho);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void EnviarFTP(string nomeArquivo, string extensao, string path, Tipo tipo, bool thumb)
        {
            try
            {
                string FTPAddress = "ftp://200.234.196.3/";
                string pastaftp = string.Empty;
                if (tipo == Tipo.Evento)
                    pastaftp = "web/imgsite";

                else if (tipo == Tipo.Banner)
                    pastaftp = "web/imgsite/banners";

                string login = "ingressorapido";
                string senha = "hk73b894x!";
                string arquivo = nomeArquivo + extensao;

                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FTPAddress + pastaftp + "/" + arquivo);

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(login, senha);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                //Load the file
                FileStream stream = File.OpenRead(path + arquivo);
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                //Upload file
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();

                if (tipo == Tipo.Evento && thumb == false)
                {
                    this.EnviarFTP(nomeArquivo + "thumb", extensao, path, Tipo.Evento, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar a imagem por FTP: " + ex.Message);
            }
        }
    }
}
