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
    public class ParceiroMidiaAreaBO : MasterBO<ParceiroMidiaAreaADO>
    {

        public ParceiroMidiaAreaBO(MasterADOBase ado = null) : base(ado) { }

        
        public ParceiroMidiaArea Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public void Salvar(ParceiroMidiaArea obj)
        {
            ado.Salvar(obj);
        }

        public List<ParceiroMidiaArea> Listar(int parceiroId)
        {
            return ado.Listar(parceiroId);
        }

        public List<ParceiroMidiaArea> ListarByParceiroMidiaAreaId(int ParceiroMidiaAreaId)
        {
            return ado.ListarByParceiroMidiaAreaId(ParceiroMidiaAreaId);
        }

        public RetornoModel VerificarDependencias(ParceiroMidiaArea obj)
        {
            RetornoModel retorno = new RetornoModel();

            VoucherBO voucherBO = new VoucherBO(ado);
            int nroRegistros = voucherBO.ContarAreas(obj.ID);
            if (nroRegistros > 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Esta Área não pode ser excluída pois existem "+nroRegistros+" Vouchers vinculados";
            }
            else
            {
                retorno.Sucesso = true;
            }
            return retorno;
        }

        public RetornoModel Remover(ParceiroMidiaArea obj)
        {
            RetornoModel retornoDependencias = VerificarDependencias(obj);
            if (retornoDependencias.Sucesso)
            {
                ado.Remover(obj);
                retornoDependencias.Mensagem = "Área removida com sucesso";
            }
            return retornoDependencias;
        }

    }
}