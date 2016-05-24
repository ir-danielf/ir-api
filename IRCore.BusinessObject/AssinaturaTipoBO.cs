using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class AssinaturaTipoBO : MasterBO<AssinaturaTipoADO>
    {
        public AssinaturaTipoBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaTipoBO() : base(null) { }

        public int getId()
        {
            return ado.getId();
        }

        public int Insere(AssinaturaTipoModel atm, int ID)
        {
            return ado.Insere(atm, ID);
        }

        public bool Altera(AssinaturaTipoModel atm)
        {
            return ado.Altera(atm);
        }

        public AssinaturaTipoModel Consultar(int id)
        {
            AssinaturaTipoModel at = ado.Consultar(id);
            if (at == null)
            {
                at.isPermiteAgregado = false;
                at.isAtivaBancoIngresso = false;
            }
            else
            {
                if (at.PermiteAgregados.Equals("T"))
                {
                    at.isPermiteAgregado = true;
                }
                else
                {
                    at.isPermiteAgregado = false;
                }
                if (at.AtivaBancoIngresso.Equals("T"))
                {
                    at.isAtivaBancoIngresso = true;
                }
                else
                {
                    at.isAtivaBancoIngresso = false;
                }
            }

            return at;
        }
        public List<AssinaturaTipoModel> Busca(string busca)
        {
            return ado.Busca(busca);
        }

        public List<AssinaturaTipoModel> Lista()
        {
            return ado.Lista();
        }

        public List<ListaAssTipoPorNome_Result> ListarAssTipoPorNome(string nome)
        {
            List<ListaAssTipoPorNome_Result> retorno = new List<ListaAssTipoPorNome_Result>();

            foreach (AssinaturaTipoModel item in ado.Busca(nome))
            {
                retorno.Add(new ListaAssTipoPorNome_Result() { ID = item.ID, Nome = item.Nome });
            }

            return retorno;
        }

        public List<ListaAssAnoPorAno_Result> BuscarAnoPorAssinaturaTipoID(int assinaturaTipoID)
        {
            List<ListaAssAnoPorAno_Result> retorno = new List<ListaAssAnoPorAno_Result>();

            foreach (AssinaturaAnoModel item in ado.BuscaListaAno(assinaturaTipoID))
            {
                retorno.Add(new ListaAssAnoPorAno_Result() { Ano = item.ano});
            }

            return retorno;
        }
    }
}
