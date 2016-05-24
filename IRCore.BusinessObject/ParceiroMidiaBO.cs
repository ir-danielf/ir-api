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
    public class ParceiroMidiaBO : MasterBO<ParceiroMidiaADO>
    {
        public ParceiroMidiaBO(MasterADOBase ado = null) : base(ado) { }


        /// <summary>
        /// Retorna o parceiro através da 
        /// string url de contexto
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ParceiroMidia ConsultarOld(string urlContexto)
        {
            return ado.Consultar(urlContexto);
        }

        public ParceiroMidia Consultar(string urlContexto)
        {
            return ado.Consultar(urlContexto);
        }

        /// <summary>
        /// Retorna o parceiro através da 
        /// string url de contexto
        /// </summary>
        /// <param name="parceiroId"></param>
        /// <returns></returns>
        public ParceiroMidia Consultar(int parceiroId)
        {
            return ado.Consultar(parceiroId);
        }

        /// <summary>
        /// Lista Paginada de Vouchers
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<ParceiroMidia> Listar(int pageNumber, int pageSize, string[] busca = null, int empresaID = 0)
        {
            return ado.Listar(pageNumber, pageSize, busca, empresaID);
        }

        /// <summary>
        /// Lista resultados da busca
        /// </summary>
        /// <param name="capturado"></param>
        /// <param name="valido"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public List<ParceiroMidia> Listar(string[] busca = null)
        {
            return ado.Listar(busca);
        }

        public List<ParceiroMidia> Listar(List<int> parceiroIDs)
        {
            return ado.Listar(parceiroIDs);
        }

        public List<ParceiroMidia> Listar(int empresaID)
        {
            return ado.Listar(empresaID);
        }

        public void Cadastrar(ParceiroMidia parceiro)
        {
            ado.Salvar(parceiro);
        }

    }
}
