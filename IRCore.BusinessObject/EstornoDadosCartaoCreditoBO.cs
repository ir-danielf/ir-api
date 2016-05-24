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
	public class EstornoDadosCartaoCreditoBO : MasterBO<EstornoDadosCartaoCreditoADO>
	{
		public EstornoDadosCartaoCreditoBO(MasterADOBase ado = null) : base(ado) { }

		public List<EstornoDadosCartaoCredito> Listar()
		{
			return ado.Listar().ToList();
		}

        public List<EstornoDadosCartaoCredito> ListarManual()
        {
            return ado.ListarEstornosManuais().ToList();
        }

        public EstornoDadosCartaoCredito Consultar(int EstornoID)
        {
            return ado.Consultar(EstornoID);
        }
        public bool Atualiza(EstornoDadosCartaoCredito obj)
        {
            return ado.Atualiza(obj);
        }
	}
}