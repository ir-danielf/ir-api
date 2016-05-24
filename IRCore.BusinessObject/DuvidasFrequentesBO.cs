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
    public class DuvidasFrequentesBO : MasterBO<DuvidasFrequentesADO>
    {
        public DuvidasFrequentesBO(MasterADOBase ado) : base(ado) { }


        /// <summary>
        /// Método que lista todas Duvidas Frequentes
        /// </summary>
        /// <returns></returns>
        public List<DuvidasFrequentes> Listar()
        {
            return ado.Listar();
        }

        /// <summary>
        /// Método que consulta uma dúvida frequente
        /// </summary>
        /// <param name="duvidaFrequenteId">Id da dúvida frequente consultada</param>
        /// <returns></returns>
        public DuvidasFrequentes Consultar(int duvidaFrequenteId)
        {
            return ado.Consultar(duvidaFrequenteId);
        }

    
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<DuvidasFrequentes> Listar(int pageNumber, int pageSize)
        {
            return ado.Listar(pageNumber, pageSize);
        }

        /// <summary>
        /// Salvar Duvida Frequente
        /// </summary>
        /// <param name="duvidaFrequene"></param>
        public void Salvar(DuvidasFrequentes duvidaFrequente)
        {
            ado.Salvar(duvidaFrequente);
        }

        /// <summary>
        /// Remover Dúvida Frequente
        /// </summary>
        /// <param name="duvidaFrequente"></param>
        public void Remover(DuvidasFrequentes duvidaFrequente)
        {
            ado.Remover(duvidaFrequente);
        }
    }
}
