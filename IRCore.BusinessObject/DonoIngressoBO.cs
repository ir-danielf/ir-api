using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IRCore.BusinessObject
{
    public class DonoIngressoBO : MasterBO<DonoIngressoADO>
    {
        public DonoIngressoBO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Método que consulta um Dono Ingresso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tDonoIngresso Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public bool Remover(int id)
        {
            return ado.Remover(id);
        }
    }
}
