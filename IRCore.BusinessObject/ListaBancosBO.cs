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
	public class ListaBancosBO : MasterBO<ListaBancosADO>
	{
		public ListaBancosBO(MasterADOBase ado = null) : base(ado) { }

		public List<ListaBancos> Lista()
		{
			return ado.Lista();
		}
	}
}
