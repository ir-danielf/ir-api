/**************************************************
* Arquivo: Banner.cs
* Gerado: 30/10/2008
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

namespace IRLib.Paralela
{

    public class Banner : Banner_B
    {

        public Banner() { }

        public enum BannerPosicao
        {
            Nenhum = 0,
            Topo = 1,
            Destaque = 2,
            Rodape = 3,
            TopoPrefeitura = 4,
            Cadastro = 5,
            DestaqueMobile = 6
        }

        public Banner(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public int GetMaxID()
        {
            try
            {
                string sql = "SELECT MAX(ID) FROM cBanner";
                object obj = bd.ConsultaValor(sql);
                int bannerID = (obj != null) ? Convert.ToInt32(obj) : 0;
                return ++bannerID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void GerarXmlBanner()
        {
            try
            {

                XmlDocument xml = new XmlDocument();
                //Carrega o XML Atraves da estrutura definida no metodo
                xml.LoadXml(this.GerarEstruturaXML());

                //Cria o elemento Banners com seu atributo
                XmlElement elementoBanners = xml.CreateElement("banners");
                elementoBanners.SetAttribute("showHeader", "true");

                int i = 0; // Variavel utilizada para saber quantos banners existem

                //EstruturaBanners lista = new EstruturaBanners();
                //lista = EstruturaBanners.listaBanners();

                foreach (EstruturaBanners lista in EstruturaBanners.retornarListaBanners())
                {
                    XmlElement elementoBanner = xml.CreateElement("banner");

                    XmlElement elementoName = xml.CreateElement("name");
                    XmlElement elementoBody = xml.CreateElement("body");
                    XmlElement elementoImagePath = xml.CreateElement("imagePath");
                    XmlElement elementoLink = xml.CreateElement("link");
                    XmlElement elementoButton = xml.CreateElement("button");

                    elementoName.InnerText = lista.name.ToString();
                    elementoBody.InnerText = lista.body.ToString();
                    elementoImagePath.InnerText = lista.imagePath.ToString();
                    elementoLink.InnerText = lista.link.ToString();
                    elementoButton.InnerText = lista.link.ToString();

                    //Inclui os Elementos dentro do Elemento Banner
                    elementoBanner.AppendChild(elementoName);
                    elementoBanner.AppendChild(elementoBody);
                    elementoBanner.AppendChild(elementoImagePath);
                    elementoBanner.AppendChild(elementoLink);
                    elementoBanner.AppendChild(elementoButton);


                    //Inclui Elemento Banner dentro do Elemento Banners
                    elementoBanners.AppendChild(elementoBanner);

                    //Soma + 1 para ter a quantidade de banners
                    i++;
                    // Utilizado para validar a quantidade maxima de banners a serem disponibilizados
                    if (i > 5)
                        break;
                }

                //Inclui o elemento Banners no arquivo XML
                xml.DocumentElement.AppendChild(elementoBanners);

                //Encontra a NodeList que tem a tagName para que seja incluido o valor dos banners a serem incluidos
                XmlNodeList listaNos = xml.GetElementsByTagName("numberOfBanners");
                listaNos.Item(0).InnerText = i.ToString();

                xml.Save(ConfigurationManager.AppSettings["PathXMLBanners"].ToString() + "imagens.xml");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GerarEstruturaXML()
        {
            StringBuilder xmlBase = new StringBuilder();
            xmlBase.Append("<?xml version='1.0' encoding='utf-8'?>");
            xmlBase.Append("<rotator isRandom='false'>");
            xmlBase.Append("<bannerTime>5</bannerTime>");
            xmlBase.Append("<numberOfBanners></numberOfBanners>");
            xmlBase.Append("</rotator>");
            return xmlBase.ToString();
        }

        public List<ClientObjects.Banner> CarregarBannersDG()
        {
            List<ClientObjects.Banner> lista = new List<ClientObjects.Banner>();
            ClientObjects.Banner oBanner;

            try
            {

                string sql = "SELECT ID, Nome, Localizacao, Posicao, Descricao FROM tBanner(nolock) ORDER BY Localizacao, Posicao";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    oBanner = new ClientObjects.Banner();
                    //popula objeto
                    oBanner.ID = bd.LerInt("ID");
                    oBanner.Nome = bd.LerString("Nome");
                    oBanner.Localizacao = bd.LerInt("Localizacao");
                    oBanner.LocalizacaoToString = bd.LerString("Localizacao");
                    oBanner.Posicao = bd.LerInt("Posicao");
                    oBanner.Descricao = bd.LerString("Descricao");
                    lista.Add(oBanner);
                }


                return lista;

            }
            catch (Exception ex)
            {
                bd.Fechar();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }


        }
    }

    public class BannerLista : BannerLista_B
    {
        public BannerLista() { }

        public BannerLista(int usuarioIDLogado) : base(usuarioIDLogado) { }


    }

    [Serializable]
    public class BannerLista2 : List<IRLib.Paralela.ClientObjects.Banner>
    {
        public BannerLista2() { }
        public BannerLista2 CarregarBannersDG()
        {
            BD bd = new BD();
            BannerLista2 lista = new BannerLista2();

            ClientObjects.Banner oBanner;

            try
            {

                string sql = "SELECT ID, Nome, Localizacao, Posicao FROM tBanner(nolock) ORDER BY Posicao";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    oBanner = new IRLib.Paralela.ClientObjects.Banner();
                    //popula objeto
                    oBanner.ID = bd.LerInt("ID");
                    oBanner.Nome = bd.LerString("Nome");
                    oBanner.Localizacao = bd.LerInt("Localizacao");
                    oBanner.Posicao = bd.LerInt("Posicao");
                    lista.Add(oBanner);
                }


                return lista;

            }
            catch (Exception ex)
            {
                bd.Fechar();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }


        }
    }

    public class EstruturaBanners
    {
        public string name { get; set; }
        public string body { get; set; }
        public string imagePath { get; set; }
        public string link { get; set; }

        public static List<EstruturaBanners> retornarListaBanners()
        {
            BD oBD = new BD();
            try
            {
                List<EstruturaBanners> listaBanners = new List<EstruturaBanners>();

                StringBuilder stbSql = new StringBuilder();
                stbSql.Append("Select Nome, Img, Url, Descricao ");
                stbSql.Append("FROM tBanner WHERE Localizacao = 2");

                oBD.Consulta(stbSql.ToString());

                while (oBD.Consulta().Read())
                {
                    EstruturaBanners banner = new EstruturaBanners();
                    banner.name = oBD.LerString("Nome");
                    banner.body = oBD.LerString("Descricao");
                    banner.imagePath = Path.GetFileName(oBD.LerString("Img"));
                    banner.link = oBD.LerString("Url");

                    listaBanners.Add(banner);
                }
                return listaBanners;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oBD.FecharConsulta();
            }
        }
    }

}
