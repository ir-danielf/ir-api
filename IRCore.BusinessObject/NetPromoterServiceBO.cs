using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using System;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class NetPromoterServiceBO : MasterBO<NetPromoterServiceADO>
    {
        public NetPromoterServiceBO(MasterADOBase ado) : base(ado) {}
        public NetPromoterServiceBO() : base(null) { }

        public void AdicionarAgendamento(string Name, string Email, int Delay, string Canal)
        {
            ado.AdicionarAgendamento(Name, Email, Delay, Canal);
        }

        public IEnumerable<dynamic> ObterAgendamentosPorStatus(string Status)
        {
            return ado.ObterAgendamentosPorStatus(Status);
        }

        public void AtualizarAgendamento(string IdAgendamento, string Status, DateTime? DataEnvio)
        {
            ado.AtualizarAgendamento(IdAgendamento, Status, DataEnvio);
        }

        public void AtualizarAgendamentoRange(string IdAgendamentoInicial, string IdAgendamentoFinal, string Status, DateTime? DataEnvio)
        {
            ado.AtualizarAgendamentoRange(IdAgendamentoInicial, IdAgendamentoFinal, Status, DataEnvio);
        }

        public void SendDataNPS(string Name, string Email, int Delay, string Canal)
        {
            object objNPS = new
            {
                name = Name,
                email = Email,
                delay = Delay,
                properties = new { canal = Canal }
            };

            IRLib.Utils.SiteHelper.GetStaticApiNPS().Post("/v1/people.json", objNPS);
        }

    }
}
