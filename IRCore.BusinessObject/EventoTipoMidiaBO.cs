using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Linq.Expressions;
using IRCore.Util.Enumerator;
using IRCore.BusinessObject.Enumerator;
using IRCore.Util;

namespace IRCore.BusinessObject
{
    public class EventoTipoMidiaBO : MasterBO<EventoTipoMidiaADO>
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="ado"></param>
         public EventoTipoMidiaBO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Método que lista todos os tipos de midia de eventos
        /// </summary>
        /// <returns></returns>
        public List<EventoTipoMidia> Listar()
        {
            return ado.Listar();
        }

        /// <summary>
        /// Método que consulta um tipo de midia de eventos
        /// </summary>
        /// <returns></returns>
        public EventoTipoMidia Consultar(int eventoTipoMidiaID)
        {
            return ado.Consultar(eventoTipoMidiaID);
        }
    }
}
