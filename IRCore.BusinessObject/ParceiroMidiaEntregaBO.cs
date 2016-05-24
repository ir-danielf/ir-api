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
    public class ParceiroMidiaEntregaBO : MasterBO<ParceiroMidiaEntregaADO>
    {

        public ParceiroMidiaEntregaBO(MasterADOBase ado = null) : base(ado) { }

        public RetornoModel Salvar(ParceiroMidiaEntrega obj)
        {
            RetornoModel retorno = new RetornoModel();

            if (obj.ID > 0)
                retorno.Sucesso = ado.Alterar(obj);
            else
                retorno.Sucesso = ado.Incluir(obj);

            //retorno.Sucesso = ado.Salvar(obj.ParceiroMidiaID, obj.EntregaID, obj.Ativo, obj.Texto);
            if (retorno.Sucesso)
                retorno.Mensagem = retorno.Sucesso ? "Ok" : "Erro ao salvar";
            return retorno;
        }
        public RetornoModel Remover(ParceiroMidiaEntrega obj)
        {
            RetornoModel retorno = new RetornoModel();
            retorno.Sucesso = ado.Remover(obj.ParceiroMidiaID, obj.EntregaID);
            if (retorno.Sucesso)
                retorno.Mensagem = retorno.Sucesso ? "Ok" : "Erro ao remover";
            return retorno;
        }

        public List<tEntrega> ListarEntregas(ParceiroMidia obj)
        {
            return ado.ListarEntregas(obj.ID);
        }

        public List<int> ListarEntregaRestricao(ParceiroMidia parceiro)
        {
            return ado.ListarEntregaRestricao(parceiro.ID);
        }

        public ParceiroMidiaEntrega Consultar(int ID)
        {
            return ado.Consultar(ID);
        }

        public ParceiroMidiaEntrega Buscar(int parceiroMidiaID, int entregaID)
        {
            return ado.Buscar(parceiroMidiaID, entregaID);
        }
    }
}