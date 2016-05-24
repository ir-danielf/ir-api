using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.Sftp
{
    public class Sftp
    {
        public bool Executa()
        {
            DateTime data = DateTime.Now;

            //somente deve ser executado se for no dia configurado
            //de acordo com a solicitação inicial do DSU-382 deve rodar todo dia primeiro
            //mas sabe-se lá né, vai que!
            //por isso o dia está numa key no appSettings ;)
            int teste = Convert.ToInt32(ConfigurationManager.AppSettings["DiaDoMes"]);
            if (data.Day == Convert.ToInt32(ConfigurationManager.AppSettings["DiaDoMes"]))
            {
                string arquivo = GeraEnviaArquivo.GeraArquivo();

                if (!string.IsNullOrEmpty(arquivo))
                {
                    GeraEnviaArquivo.Upload(arquivo);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
