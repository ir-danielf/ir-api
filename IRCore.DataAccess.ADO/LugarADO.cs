using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IRCore.DataAccess.ADO
{
    public class LugarADO : MasterADO<dbIngresso>
    {
        public LugarADO(MasterADOBase ado = null) : base(ado) { }


        public tLugar Consultar(int lugarID)
        {
            var result = (from item in dbIngresso.tLugar
                          where item.ID == lugarID
                          select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        public List<tLugar> ListarSetor(int setorID)
        {   
            var result = (from item in dbIngresso.tLugar
                          where item.SetorID == setorID
                          select item);
            return result.AsNoTracking().ToList();
        }
    }
}