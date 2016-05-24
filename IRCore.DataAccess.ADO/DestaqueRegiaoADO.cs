using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using System.Linq.Expressions;
using IRCore.Util;
using IRCore.DataAccess.ADO.Models;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class DestaqueRegiaoADO : MasterADO<dbSite>
    {
        public DestaqueRegiaoADO(MasterADOBase ado = null) : base(ado) { }

        public DestaqueRegiao Consultar(int destaqueId)
        {
            return ConsultarQuery(destaqueId).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<DestaqueRegiao> ConsultarQuery(int destaqueId)
        {
            return (from item in dbSite.DestaqueRegiao
                    where item.ID == destaqueId
                   select item);
        }

        
    }
}
