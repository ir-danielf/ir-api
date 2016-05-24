using IRAPI.SDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.NetPromoterService
{
    public class NetPromoterService
    {

        public NetPromoterService()
        {

        }

        public void Executar()
        {
            try
            {
                RetornoModelAPI<List<IRAPI.SDK.Model.NpsModel>> listaNSP = IRLib.Utils.SiteHelper.GetStaticApi().Get<RetornoModelAPI<List<IRAPI.SDK.Model.NpsModel>>>("/nps/listar?Status=Aguardando"); // Realiza a chamada da IRAPI

                if (listaNSP.Sucesso)
                {
                    foreach(var item in listaNSP.Retorno)
                    {
                        object objNPS = new
                        {
                            name = item.Name,
                            email = item.Email,
                            delay = item.Delay,
                            properties = new { canal = item.Canal }
                        };

                        // Envia os dados para a API
                        IRLib.Utils.SiteHelper.GetStaticApiNPS().Post("/v1/people.json", objNPS);

                        // Atualiza para enviado
                        object objIRAPI = new
                        {
                            IdAgendamento = item.ID,
                            Status = "Sucesso"
                        };

                        IRLib.Utils.SiteHelper.GetStaticApi().Post("/nps/atualizar", objIRAPI);
                    }
                }
            }
            catch (Exception)
            {
                ; // Retorna exceção caso algo de errado ao enviar requisição para obter os ID's que serão enviados para a API
            }
        }
    }
}
