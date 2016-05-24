using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class LocalBO : MasterBO<LocalADO>
    {
        
        public LocalBO(MasterADOBase ado = null) : base(ado) { }

        public Local Consultar(int id)
        {
            var local = ado.Consultar(id);
            return local;
        }

        public InfosObrigatoriasIngresso ListarInfosObrigatoriasIngresso(int localId)
        {
            return ado.ListarInfosObrigatoriasIngresso(localId);
        }
        
        public List<Local> ListarTodos()
        {
            return ado.ListarTodos();
        }

        public List<Local> Listar(int limit, string busca = null, string estado = null, string cidade = null)
        {
            return ado.Listar(limit, busca, estado, cidade);
        }

        public List<Local> Listar(string busca = null, string estado = null, string cidade = null)
        {
            return ado.Listar(busca, estado, cidade);
        }

        public List<tLocal> ListarHistoricoParceiro(ParceiroMidia parceiro)
        {
            return ado.ListarHistoricoLocais(parceiro.ID);
        }

        public List<tLocal> ListarLocalDisponivel(ParceiroMidia parceiro)
        {
            return ado.ListarLocalDisponivel(parceiro.ID);
        }

        public List<tLocal> ListarLugaresAtivos(int parceiroMidiaID)
        {
            return ado.ListarLugaresAtivos(parceiroMidiaID);
        }
    }
}