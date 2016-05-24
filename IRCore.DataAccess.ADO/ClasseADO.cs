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
    public class ClasseADO : MasterADO<dbIngresso>
    {
        public ClasseADO(MasterADOBase ado = null) : base(ado, false) { }

        /// <summary>
        /// Consulta uma classe na base de dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ParceiroMidiaClasse Consultar(int id)
        {
            var result = (from item in dbIngresso.ParceiroMidiaClasse
                          where item.ID == id
                          select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Consulta uma classe por um setor e parceiro
        /// </summary>
        /// <param name="idSetor"></param>
        /// <param name="idParceiro"></param>
        /// <returns></returns>
        public ParceiroMidiaClasseSetor Consultar(int idSetor, int idApresentacao, int idParceiro)
        {
            var result = (from item in dbIngresso.ParceiroMidiaClasseSetor
                          where item.ParceiroMidiaClasse.ParceiroMidiaID == idParceiro
                          && item.tSetor.ID == idSetor && item.ApresentacaoID == idApresentacao
                          select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Retorna Lista de CLasses
        /// </summary>
        /// <param name="parceiroId"></param>
        /// <returns></returns>
        public List<ParceiroMidiaClasse> Listar(int? parceiroId = null)
        {
            var result = (from item in dbIngresso.ParceiroMidiaClasse
                          select item);
            if (parceiroId != null)
            {
                result = (from item in result
                          where parceiroId == item.ParceiroMidiaID
                          select item);
            }
            return result.OrderByDescending(x => x.Nivel).ThenBy(x => x.Nome).AsNoTracking().ToList();
        }

    }
}