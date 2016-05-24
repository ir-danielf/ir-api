using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
	public class EstornoDadosDepositoBancarioBO : MasterBO<EstornoDadosDepositoBancarioADO>
	{
		public EstornoDadosDepositoBancarioBO(MasterADOBase ado = null) : base(ado) { }

		public List<EstornoDadosDepositoBancario> Listar()
		{
			return ado.Listar().ToList();
		}

		public EstornoDadosDepositoBancario Consultar(int estornoId)
		{
			return ado.Consultar(estornoId);
		}

		public List<DateTime?> getDatas()
		{
			return ado.getDatas();
		}

		public void Salvar(EstornoDadosDepositoBancario obj)
		{
			ado.Salvar(obj);
		}
	}
}
