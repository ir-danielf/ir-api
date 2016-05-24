using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class FormaPagamentoADO : MasterADO<dbSite>
    {
        public FormaPagamentoADO(MasterADOBase ado = null) : base(ado) { }

        public List<FormaPagamento> ListarEvento(int eventoID)
        {
            var queryStr = new StringBuilder();
            queryStr.AppendFormat(@"SELECT fp.Nome, fp.Parcelas FROM dbo.FormaPagamento fp (NOLOCK)
                                    	INNER JOIN dbo.FormaPagamentoEvento fpe (NOLOCK) ON fp.IR_FormaPagamentoID = fpe.FormaPagamentoID
                                    WHERE fpe.EventoID = @eventoID
                                    GROUP BY fp.Nome, fp.Parcelas", eventoID);

            var query = conSite.Query<FormaPagamento>(queryStr.ToString(), new
            {
                eventoID = eventoID
            });
            return query.ToList();
        }

        public List<FormaPagamento> ListarEvento(List<int> eventos)
        {

            string queryStr = @"SELECT fp.Nome, Max(fp.Parcelas) as Parcelas
                               FROM FormaPagamento (NOLOCK) fp
                               INNER JOIN FormaPagamentoEvento (NOLOCK) fpe on fp.IR_FormaPagamentoID = fpe.FormaPagamentoID
                               where fp.IR_FormaPagamentoID in (SELECT FormaPagamentoID
							                                    FROM FormaPagamentoEvento(NOLOCK) 
												                where EventoId in (@eventosIDs)
												                Group by FormaPagamentoID
												                Having count(eventoID) = @eventosCount)
                                Group by fp.Nome";
            
            var query = conSite.Query<FormaPagamento>(queryStr, new
            {
                eventosIDs = string.Join(",", eventos),
                eventosCount = eventos.Count
            });
            return query.ToList();
        }


        public List<FormaPagamento> Listar(List<int> ids)
        {
            return this.conSite.Query<FormaPagamento>("SELECT * FROM FormaPagamento (NOLOCK) WHERE IR_FormaPagamentoID IN (@FormaPagamentosID)".Replace("@FormaPagamentosID", String.Join(",", ids))).ToList();
        }

        public FormaPagamento Consultar(int idFormaPagamento)
        {
            var sql = string.Format("SELECT * FROM FormaPagamento (NOLOCK) WHERE IR_FormaPagamentoID = {0}", idFormaPagamento);
            return this.conSite.Query<FormaPagamento>(sql).FirstOrDefault();
        }

    }
}
