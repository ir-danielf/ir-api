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
    public class EventoTipoMidiaADO : MasterADO<dbIngresso>
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="ado"></param>
        public EventoTipoMidiaADO(MasterADOBase ado = null) : base(ado, false) { }

        /// <summary>
        /// Método que lista todos os tipos de midia de eventos
        /// </summary>
        /// <returns></returns>
        public List<EventoTipoMidia> Listar()
        {
            return (from item in dbIngresso.EventoTipoMidia
                    orderby item.Nome
                    select item).AsNoTracking().ToList();
        }

        /// <summary>
        /// Método que consulta um tipo de midia de eventos
        /// </summary>
        /// <returns></returns>
        public EventoTipoMidia Consultar(int eventoTipoMidiaID)
        {
            return (from item in dbIngresso.EventoTipoMidia
                    where item.ID == eventoTipoMidiaID
                    select item).AsNoTracking().FirstOrDefault();
        }
    }

}
