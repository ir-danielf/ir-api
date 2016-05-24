using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using IRCore.Util.Enumerator;
using System.Globalization;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;
using IRCore.DataAccess.ADO.Models;

namespace IRCore.DataAccess.ADO
{
    public class EventoMidiaADO : MasterADO<dbIngresso>
    {
        public EventoMidiaADO(MasterADOBase ado = null) : base(ado, false) { }

        /// <summary>
        /// Método que lista as Midias de um determinado evento de forma paginada
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<EventoMidia> Listar(int eventoID,int pageNumber, int pageSize)
        {
            return (from item in dbIngresso.EventoMidia.Include(t => t.EventoTipoMidia)
                    where item.EventoID == eventoID
                    orderby item.ID
                    select item).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Método que lista as Midias de um determinado evento
        /// </summary>
        /// <param name="eventoID"></param>
        /// <returns></returns>
        public IQueryable<EventoMidia> Listar(int eventoID)
        {
            return (from item in dbIngresso.EventoMidia.Include(t => t.EventoTipoMidia)
                    where item.EventoID == eventoID
                    orderby item.ID
                    select item).AsNoTracking();
        }

        /// <summary>
        /// Método que consulta uma Midia de evento
        /// </summary>
        /// <param name="eventoMidiaID"></param>
        /// <returns></returns>
        public EventoMidia Consultar(int eventoMidiaID)
        {
            return (from item in dbIngresso.EventoMidia.Include(t=>t.tEvento).Include(t=>t.EventoTipoMidia)
                    where item.ID == eventoMidiaID
                    select item).AsNoTracking().FirstOrDefault();
        }
    }

}
