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
    public class ClasseSetorBO : MasterBO<ClasseSetorADO>
    {

        public ClasseSetorBO(MasterADOBase ado = null) : base(ado) { }
        
        /// <summary>
        /// Salva a classe/setor
        /// </summary>
        /// <param name="obj"></param>
        public void Salvar(ParceiroMidiaClasseSetor obj)
        {
            ado.Salvar(obj);
        }

        /// <summary>
        /// Remove a classe/setor
        /// </summary>
        /// <param name="obj"></param>
        public void Remover(ParceiroMidiaClasseSetor obj)
        {
            ado.Remover(obj);
        }

        /// <summary>
        /// Consulta a classe/setor
        /// </summary>
        /// <param name="obj"></param>
        public ParceiroMidiaClasseSetor Consultar(int id)
        {
            return ado.Consultar(id);
        }
    }
}