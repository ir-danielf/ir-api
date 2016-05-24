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
    public class EventoMidiaBO : MasterBO<EventoMidiaADO>
    {
        /// <summary>
        /// Construtor da classe EventosMidiaBO
        /// </summary>
        /// <param name="ado"></param>
         public EventoMidiaBO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Método que lista midias de um evento de forma paginada
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<EventoMidia> Listar(int eventoID, int pageNumber,int pageSize)
        {
            return ado.Listar(eventoID, pageNumber, pageSize);
        }

        /// <summary>
        /// Método que lista midias de um evento
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<EventoMidia> Listar(int eventoID)
        {
            return ado.Listar(eventoID).ToList();
        }

        /// <summary>
        /// Método que consulta uma mídia de evento
        /// </summary>
        /// <param name="eventoMidiaID"></param>
        /// <returns></returns>
        public EventoMidia Consultar(int eventoMidiaID)
        {
            return ado.Consultar(eventoMidiaID);
        }

        /// <summary>
        /// Método que salva uma mídia de evento
        /// </summary>
        /// <param name="eventoMidia"></param>
        public void Salvar(EventoMidia eventoMidia)
        {
            ado.Salvar(eventoMidia);
        }

        /// <summary>
        /// Método que remover uma mídia de evento
        /// </summary>
        /// <param name="eventoMidia"></param>
        public void Remover(EventoMidia eventoMidia)
        {
            ado.Remover(eventoMidia);
        }
    }
}
