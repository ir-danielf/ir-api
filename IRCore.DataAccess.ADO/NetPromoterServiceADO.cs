using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using IRCore.DataAccess.ADO.Models;

namespace IRCore.DataAccess.ADO
{
    public class NetPromoterServiceADO : MasterADO<dbIngresso>
    {
        public NetPromoterServiceADO(MasterADOBase ado = null) : base(ado) { }

        public void AdicionarAgendamento(string Name, string Email, int Delay, string Canal)
        {
            // Adiciona o agendamento na tabela para processamento posterior
            var sql = new StringBuilder();
            sql.Append("INSERT INTO tNetPromoterService (Name, Email, Delay, Canal, Status, DataInclusao) ");
            sql.Append(" VALUES ('@Name', '@Email', @Delay, '@Canal', 'A', GETDATE()) ");
            sql.Replace("@Name", Name);
            sql.Replace("@Email", Email);
            sql.Replace("@Delay", Convert.ToString(Delay));
            sql.Replace("@Canal", Canal);

            conIngresso.Execute(sql.ToString());
        }

        public IEnumerable<dynamic> ObterAgendamentosPorStatus(string status)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ID, Name, Email, Delay, Canal, Status, DataInclusao, DataEnvio FROM tNetPromoterService ");
            sql.Append(" WHERE Status = '@Status' ");
            sql.Replace("@Status", status);

            var lista = conIngresso.Query(sql.ToString());
            return lista;
        }

        public void AtualizarAgendamento(string IdAgendamento, string Status, DateTime? DataEnvio)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE tNetPromoterService SET Status = '@Status', DataEnvio = @DataEnvio");
            sql.Append(" WHERE ID = @IdAgendamento ");
            sql.Replace("@Status", Status);
            sql.Replace("@DataEnvio", DataEnvio.HasValue ? string.Format("'{0}'", DataEnvio.Value.ToString("yyyy-MM-dd HH:mm:ss")) : "NULL");
            sql.Replace("@IdAgendamento", IdAgendamento);

            conIngresso.Execute(sql.ToString());
        }

        public void AtualizarAgendamentoRange(string IdAgendamentoInicial, string IdAgendamentoFinal, string Status, DateTime? DataEnvio)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE tNetPromoterService SET Status = '@Status', DataEnvio = @DataEnvio");
            sql.Append(" WHERE ID BETWEEN @IdAgendamentoInicial AND @IdAgendamentoFinal ");
            sql.Replace("@Status", Status);
            sql.Replace("@DataEnvio", DataEnvio.HasValue ? string.Format("'{0}'", DataEnvio.Value.ToString("yyyy-MM-dd HH:mm:ss")) : "NULL");
            sql.Replace("@IdAgendamentoInicial", IdAgendamentoInicial);
            sql.Replace("@IdAgendamentoFinal", IdAgendamentoFinal);

            conIngresso.Execute(sql.ToString());
        }
    }
}
