using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IRCore.DataAccess.ADO
{
    public class ClasseSetorADO : MasterADO<dbIngresso>
    {
        public ClasseSetorADO(MasterADOBase ado = null) : base(ado, false) { }
       

        /// <summary>
        /// Consulta a classe/setor
        /// </summary>
        /// <param name="obj"></param>
        public ParceiroMidiaClasseSetor Consultar(int id)
        {
            return (from item in dbIngresso.ParceiroMidiaClasseSetor
                    where item.ID == id
                    select item).AsNoTracking().FirstOrDefault();
        }
    }
}
