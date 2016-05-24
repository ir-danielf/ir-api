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
    public class ClasseBO : MasterBO<ClasseADO>
    {

        public ClasseBO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Consulta uma classe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ParceiroMidiaClasse Consultar(int id)
        {
            return ado.Consultar(id);
        }

        /// <summary>
        /// Consulta uma classe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ParceiroMidiaClasseSetor Consultar(int idSetor, int idApresentacao, int idParceiro)
        {
            return ado.Consultar(idSetor, idApresentacao, idParceiro);
        }

        /// <summary>
        /// Salva a classe
        /// </summary>
        /// <param name="obj"></param>
        public void Salvar(ParceiroMidiaClasse obj)
        {
            ado.Salvar(obj);
        }

        /// <summary>
        /// Lista as classes pelo parceiro ou pelo admin
        /// </summary>
        /// <param name="parceiroId"></param>
        /// <returns></returns>
        public List<ParceiroMidiaClasse> Listar(int? parceiroId = null)
        {
            return ado.Listar(parceiroId);
        }


        public RetornoModel VerificarDependencias(ParceiroMidiaClasse obj)
        {
            RetornoModel retorno = new RetornoModel();

            VoucherBO voucherBO = new VoucherBO(ado);
            int nroRegistros = voucherBO.ContarClasses(obj.ID);
            if (nroRegistros > 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Esta Classe não pode ser excluída pois existem " + nroRegistros + " Vouchers vinculados";
            }
            else
            {
                retorno.Sucesso = true;
            }
            return retorno;
        }

        public RetornoModel Remover(ParceiroMidiaClasse obj)
        {
            RetornoModel retornoDependencias = VerificarDependencias(obj);
            if (retornoDependencias.Sucesso)
            {
                ado.Remover(obj);
                retornoDependencias.Mensagem = "Classe removida com sucesso";
            }
            return retornoDependencias;
        }


    }
}