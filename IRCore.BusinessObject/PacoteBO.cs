using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class PacoteBO : MasterBO<PacoteADO>
    {
        public PacoteBO(MasterADOBase ado = null) : base(ado) { }

        public Pacote Consultar(int id)
        {
            var pacoteItens = ListarItens(id);
            var pacote = pacoteItens.FirstOrDefault().Pacote;

            if (pacote != null)
            {
                pacote.PacoteItem = pacoteItens;
            }

            return pacote;
        }

        public List<PacoteRetorno> ListarPorEvento(int eventoID, int canalID = 2)
        {
            return ado.ListarPorEvento(eventoID, canalID);
        }

        public List<PacoteItem> ListarItens(int pacoteID)
        {
            return ado.ListarItens(pacoteID);
        }

        public List<PacoteItem> ListarItensEvento(int eventoID)
        {
            return ado.ListarItensEvento(eventoID);
        }
    }
}
