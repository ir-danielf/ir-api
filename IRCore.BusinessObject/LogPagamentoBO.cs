using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;

namespace IRCore.BusinessObject
{
	public class LogPagamentoBO : MasterBO<LogPagamentoADO>
	{
		public LogPagamentoBO(MasterADOBase ado = null)
			: base(ado)
		{
		}

		public void Adicionar(string mensagem, string source, string stackTrace, string mensagemAmigavel, string dadosAdicionais)
		{
			ado.Adicionar(mensagem, source, stackTrace, mensagemAmigavel, dadosAdicionais);
		}
	}
}
