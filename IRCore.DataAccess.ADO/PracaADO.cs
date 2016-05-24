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
    public class PracaADO : MasterADO<dbIngresso>
    {

        public PracaADO(MasterADOBase ado = null) : base(ado, false) { }



        /// <summary>
        /// Médoto que lista todas as praças
        /// </summary>
        /// <returns></returns>
        public List<ParceiroMidiaPraca> Listar()
        {
            var result = (from item in dbIngresso.ParceiroMidiaPraca
                          select item).OrderBy(t => t.ID).AsNoTracking().ToList();

            return result;
        }

        /// <summary>
        /// Método que retorna uma Praça
        /// </summary>
        /// <param name="pracaId">Id da praça consultada</param>
        /// <returns></returns>
        public ParceiroMidiaPraca Consultar(int pracaId)
        {
            ParceiroMidiaPraca result = (from item in dbIngresso.ParceiroMidiaPraca
                                        where item.ID == pracaId
                                         select item).AsNoTracking().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<ParceiroMidiaPraca> Listar(int pageNumber, int pageSize, int parceiroMidiaID)
        {
            return ListarQuery(parceiroMidiaID).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="busca"></param>
        /// <returns></returns>
        private IQueryable<ParceiroMidiaPraca> ListarQuery(int parceiroMidiaID)
        {
            return (from praca in dbIngresso.ParceiroMidiaPraca
                    where praca.ParceiroMidiaID == parceiroMidiaID
                    orderby praca.ID
                    select praca);


        }

        /// <summary>
        /// Salva uma praça
        /// </summary>
        /// <param name="praca"></param>
        /// <returns></returns>
        public bool Salvar(ParceiroMidiaPraca praca)
        {
            return base.Salvar(praca);
        }

        /// <summary>
        /// Lista as praças por parceiro
        /// </summary>
        /// <param name="parceiroId"></param>
        /// <returns></returns>
        public List<ParceiroMidiaPraca> Listar(int parceiroId)
        {
            return (from item in dbIngresso.ParceiroMidiaPraca
                    where item.ParceiroMidiaID == parceiroId
                    select item).AsNoTracking().ToList();
        }

        /// <summary>
        /// Lista as praças por mais de um id
        /// </summary>
        /// <param name="pracasId"></param>
        /// <returns></returns>
        public List<ParceiroMidiaPraca> Listar(List<int> pracasId)
        {
            return (from item in dbIngresso.ParceiroMidiaPraca
                    where pracasId.Contains(item.ID)
                    select item).AsNoTracking().ToList();
        }
    }
}
