using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.EstornoDadosPlanilhas.Operadoras
{
    public class Generator
    {
        public void GerarPlanilhas(string strPath, string emailEnvioRede,string emailEnvioCielo,string emailEnvioValeCultura,
            string emailEnvioEloCultura,string emailEnvioAmex, string emailEnvioPlanilhasManuais)
        {
            //PlanilhasEstornosManuais.GeraPlanilhaEstornosManuais(strPath, emailEnvioPlanilhasManuais);
            Rede.GeraPlanilhaRede(strPath, emailEnvioRede);
            Cielo.GeraPlanilhaCielo(strPath, emailEnvioCielo);
            ValeCultura.GeraPlanilhaValeCultura(strPath, emailEnvioValeCultura, "Vale Cultura");
            ValeCultura.GeraPlanilhaValeCultura(strPath, emailEnvioEloCultura, "Elo Cultura");
            Amex.GeraPlanilhaAmexAVista(strPath, emailEnvioAmex);
            AmexParcelado.GeraPlanilhaAmexParcelado(strPath, emailEnvioAmex);
        }
        
    }
}
