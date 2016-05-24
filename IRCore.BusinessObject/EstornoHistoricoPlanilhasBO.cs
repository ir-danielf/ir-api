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
	public class EstornoHistoricoPlanilhasBO : MasterBO<EstornoHistoricoPlanilhasADO>
	{
		public EstornoHistoricoPlanilhasBO(MasterADOBase ado = null) : base(ado) { }

		public List<EstornoHistoricoPlanilhas> Listar()
		{
			return ado.Listar().ToList();
		}
		public EstornoHistoricoPlanilhas GetArquivoDownload(int estornoId)
		{
			return ado.GetArquivoDownload(estornoId);
		}
	}
}
