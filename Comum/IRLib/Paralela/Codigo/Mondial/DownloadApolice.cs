using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace IRLib.Paralela.Codigo.Mondial
{
    public class DownloadApolice
    {
        public string GetApolice()
        {
            try
            {
                string caminho = ConfigurationManager.AppSettings["DownloadPathApolice"];

                string apolice = File.ReadAllText(caminho, Encoding.Default);

                apolice = apolice.Replace(Environment.NewLine, "\r\n");

                return apolice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
