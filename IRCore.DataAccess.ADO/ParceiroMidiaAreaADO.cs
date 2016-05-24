using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO
{
    public class ParceiroMidiaAreaADO : MasterADO<dbIngresso>
    {
        public ParceiroMidiaAreaADO(MasterADOBase ado = null) : base(ado, false) { }

        
        public ParceiroMidiaArea Consultar(int id)
        {
            var result = (from item in dbIngresso.ParceiroMidiaArea
                          where item.ID == id
                          select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        
        public List<ParceiroMidiaArea> Listar(int parceiroId)
        {
            return (from item in dbIngresso.ParceiroMidiaArea
                            where item.ParceiroMidiaID == parceiroId     
                            orderby item.Nome
                    select item).AsNoTracking().ToList();
        }
        public List<ParceiroMidiaArea> ListarByParceiroMidiaAreaId(int ParceiroMidiaAreaId)
        {
            return (from item in dbIngresso.ParceiroMidiaArea
                    where item.ID == ParceiroMidiaAreaId
                    select item).AsNoTracking().ToList();
        }
    }
}