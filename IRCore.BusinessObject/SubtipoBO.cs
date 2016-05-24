using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRCore.BusinessObject
{
    public class SubtipoBO : MasterBO<SubtipoADO>
    {
        public SubtipoBO(MasterADOBase ado = null) : base(ado) { }

        public List<EventoSubtipo> ListarTipo(int tipoId, bool apenasEventos = false)
        {
            return ado.ListarTipo(tipoId, apenasEventos);
        }

        /// <summary>
        /// Lista os subtipos no menu mais
        /// </summary>
        /// <param name="naoPertencem"> Ids dos Tipos cujos subtipos não serão listados</param>
        /// <returns>Uma lista de subtipos</returns>
        public List<EventoSubtipo> ListarSubtiposMenuMais()
        {
            return ado.ListarSubtiposMenuMais();
        }

        public EventoSubtipo Consultar(int subTipoId)
        {
            return ado.Consultar(subTipoId);
        }

        public EventoSubtipo ConsultarPorIrSubtipoId(int IR_subTipoId)
        {
            return ado.ConsultarPorIrSubtipoId(IR_subTipoId);
        }


    }
}
