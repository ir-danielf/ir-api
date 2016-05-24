using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
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
    public class PracaBO : MasterBO<PracaADO>
    {
        public PracaBO(MasterADOBase ado) : base(ado) { }


        /// <summary>
        /// Método que lista todas Praças
        /// </summary>
        /// <returns></returns>
        public List<ParceiroMidiaPraca> ListarPracas()
        {
            return ado.Listar();
        }

        /// <summary>
        /// Método que consulta uma praça
        /// </summary>
        /// <param name="pracaId">Id da praça consultada</param>
        /// <returns></returns>
        public ParceiroMidiaPraca Consultar(int pracaId)
        {
            return ado.Consultar(pracaId);
        }

        ///<summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ParceiroMidiaPraca> Listar(int pageNumber, int pageSize,int parceiroMidiaID)
        {
            return ado.Listar(pageNumber, pageSize,parceiroMidiaID);
        }

        /// <summary>
        /// Salvar Praça
        /// </summary>
        /// <param name="praca"></param>
        public void Salvar(ParceiroMidiaPraca praca)
        {
            ado.Salvar(praca);
        }


        public RetornoModel VerificarDependencias(ParceiroMidiaPraca obj)
        {
            RetornoModel retorno = new RetornoModel();

            VoucherBO voucherBO = new VoucherBO(ado);
            int nroRegistros = voucherBO.ContarPracas(obj.ID);
            if (nroRegistros > 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Esta Praça não pode ser excluída pois existem "+nroRegistros+" Vouchers vinculados";
            }
            else
            {
                retorno.Sucesso = true;
            }
            return retorno;
        }
        /// <summary>
        /// Remover Praça
        /// </summary>
        /// <param name="praca"></param>
        public RetornoModel Remover(ParceiroMidiaPraca obj)
        {
            RetornoModel retornoDependencias = VerificarDependencias(obj);
            if (retornoDependencias.Sucesso)
            {
                ado.Remover(obj);
                retornoDependencias.Mensagem = "Praça removida com sucesso";
            }
            return retornoDependencias;
        }

        /// <summary>
        /// Listar Praças por Parceiro
        /// </summary>
        /// <param name="parceiroId"></param>
        /// <returns></returns>
        public List<ParceiroMidiaPraca> Listar(int parceiroId)
        {
            return ado.Listar(parceiroId);
        }

        /// <summary>
        /// Listar Praças por mais de um id
        /// </summary>
        /// <param name="pracaIds"></param>
        /// <returns></returns>
        public List<ParceiroMidiaPraca> Listar(List<int> pracaIds)
        {
            return ado.Listar(pracaIds);
        }


    }
}
