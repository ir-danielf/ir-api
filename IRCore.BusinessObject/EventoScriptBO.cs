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
    public class EventoScriptBO : MasterBO<EventoScriptADO>
    {
        public EventoScriptBO(MasterADOBase ado = null) : base(ado) { }

        public List<EventoScriptModel> ListarScripts(int eventoID)
        {
            return ado.ListarScripts(eventoID);
        }

        public bool Salvar(EventoScriptModel esm)
        {
            if (esm.ID > 0)
                return ado.Alterar(esm);
            else
                return ado.Inserir(esm);
        }

        public EventoScriptModel Carregar(int id)
        {
            EventoScriptModel esm = ado.Carregar(id);

            return esm;
        }

        public bool Remover(int id)
        {
            return ado.Remover(id);
        }
    }
}
