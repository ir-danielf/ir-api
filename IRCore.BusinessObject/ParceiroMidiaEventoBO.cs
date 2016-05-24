using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class ParceiroMidiaEventoBO : MasterBO<ParceiroMidiaEventoADO>
    {
        public ParceiroMidiaEventoBO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Retorna o ParceiroMidiaEvento através do evento e parceiro
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="parceiroID"></param>
        /// <returns></returns>
        public ParceiroMidiaEvento Consultar(int eventoID, int parceiroID)
        {
            return ado.Consultar(eventoID, parceiroID);
        }



        public void Salvar(ParceiroMidiaEvento parceiroEvento)
        {
            ado.Salvar(parceiroEvento);
        }

    }
}
