
namespace IRCore.DataAccess.ADO
{
	using IRCore.DataAccess.ADO.Estrutura;
	using IRCore.DataAccess.Model;
	using IRCore.DataAccess.Model.Enumerator;
	using PagedList;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using IRCore.DataAccess.ADO.Models;
	using System.Data.Entity;
	using Dapper;

	public class LogPagamentoADO : MasterADO<dbIngresso>
	{
		public LogPagamentoADO(MasterADOBase ado = null)
			: base(ado)
		{
		}

		public void Adicionar(string mensagem, string source, string stackTrace, string mensagemAmigavel, string dadosAdicionais)
		{
			var sql = new StringBuilder();
			sql.Append("INSERT INTO LogPagamento (Date, Message, Source, StackTrace, FriendlyMessage, [Data]) ");
			sql.Append("VALUES (GETDATE(), '@MESSAGE','@SOURCE','@STACKTRACE', '@FRIENDLYMESSAGE', '@DATA')");
			sql.Replace("@MESSAGE", mensagem);
			sql.Replace("@SOURCE", source);
			sql.Replace("@STACKTRACE", stackTrace);
			sql.Replace("@FRIENDLYMESSAGE", mensagemAmigavel);
			sql.Replace("@DATA", dadosAdicionais);

			conIngresso.Execute(sql.ToString());
		}
	}
}
