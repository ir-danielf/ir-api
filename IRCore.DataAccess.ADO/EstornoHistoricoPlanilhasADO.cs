using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO
{
	public class EstornoHistoricoPlanilhasADO : MasterADO<dbIngresso>
	{
		public EstornoHistoricoPlanilhasADO(MasterADOBase ado = null) : base(ado) { }

		public IQueryable<EstornoHistoricoPlanilhas> Listar()
		{
			return dbIngresso.EstornoHistoricoPlanilhas;
		}

		public EstornoHistoricoPlanilhas GetArquivoDownload(int historicoId)
		{
			EstornoHistoricoPlanilhas result = (from item in dbIngresso.EstornoHistoricoPlanilhas
												where item.ID == historicoId
												   select item).FirstOrDefault();

			return result;
		}
	}
}
