using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class TipoBO : MasterBO<TipoADO>
    {
        public TipoBO(MasterADOBase ado = null) : base(ado) { }

        public List<Tipo> Listar(bool apenasEventos = false)
        {
            return ado.Listar(apenasEventos);
        }

        public Tipo Consultar(int tipoId)
        {
            return ado.Consultar(tipoId);
        }

        public List<tEventoTipo>ListarHistoricoParceiro(ParceiroMidia parceiro)
        {
            return ado.ListarHistoricoInApresentacao(parceiro.ApresentacaoIDs);
        }
    }
}
