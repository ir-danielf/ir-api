using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTLib;
using System.IO;
using System.Windows.Forms;
using IRLib.IRConfigs;
using System.Net;
using System.Xml;
using System.Configuration;
using System.Runtime.Remoting.Lifetime;
namespace IRLib.Codigo.Atualizador
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class AtualizadorSistema : MarshalByRefObject
    {
        string UpdatePath;
        string UpdateFileName;

        public bool BuscarVersao(string versaoSistema)
        {
            try
            {
                UpdatePath = ConfigurationManager.AppSettings["UpdatePath"];
                UpdateFileName = ConfigurationManager.AppSettings["UpdateFileName"];
                string CurrentVersion = LerArquivoInfo(UpdatePath, UpdateFileName).VersaoAtual;

                return versaoSistema != CurrentVersion;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Exception("Houve um erro na tentativa de atualizar o sistema. Contate o suporte ingresso rápido");
            }
        }

        public byte[] Atualizar()
        {
            try
            {
                byte[] arquivo = null;
                UpdatePath = ConfigurationManager.AppSettings["UpdatePath"];
                UpdateFileName = ConfigurationManager.AppSettings["UpdateFileName"];

                string CurrentVersion = LerArquivoInfo(UpdatePath, UpdateFileName).VersaoAtual;

                string upFile = UpdatePath + CurrentVersion + ".zip";
                if (File.Exists(upFile))
                {
                    arquivo = File.ReadAllBytes(upFile);
                }

                return arquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private VersionInfo LerArquivoInfo(string UpdateURL, string UpdateInfoFileName)
        {
            if (!string.IsNullOrEmpty(UpdateURL) && !string.IsNullOrEmpty(UpdateInfoFileName))
            {
                try
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(UpdateURL + UpdateInfoFileName);
                    VersionInfo ver = new VersionInfo();
                    ver.VersaoAtual = xdoc.GetElementsByTagName("VersaoAtual")[0].InnerText;
                    ver.VersaoMinima = xdoc.GetElementsByTagName("VersaoMinima")[0].InnerText;
                    return ver;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                throw new Exception("Não foi possível buscar atualizações para o sistema.");
        }
    }

    class VersionInfo
    {
        public string VersaoMinima { get; set; }
        public string VersaoAtual { get; set; }
    }
}
