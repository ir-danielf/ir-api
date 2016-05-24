using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace IRCore.DataAccess.ADO
{
	public class ListaBancosADO : MasterADO<dbIngresso>
	{
		public ListaBancosADO(MasterADOBase ado = null) : base(ado) { }

		public List<ListaBancos> Lista()
		{
			{
				string queryString = @"SELECT Codigo, NomeBanco FROM ListaBancos WHERE IRDeposita = 1";

				var query = conIngresso.Query<ListaBancos>(queryString);

				return query.ToList();
			}
		}
	}
}
