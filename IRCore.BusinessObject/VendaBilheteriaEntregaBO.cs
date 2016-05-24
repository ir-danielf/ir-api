using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class VendaBilheteriaEntregaBO: MasterBO<VendaBilheteriaEntregaADO>
    {
        public VendaBilheteriaEntregaBO(MasterADOBase ado) : base(ado) {}
        public VendaBilheteriaEntregaBO() : base(null) { }

        public bool Cadastrar(tVendaBilheteriaEntrega vendaBilheteriaEntrega)
        {
            return ado.Cadastrar(vendaBilheteriaEntrega);
        }

        public tVendaBilheteriaEntrega Carregar(int id)
        {
            return ado.Carregar(id);
        }

        public List<tVendaBilheteriaEntrega> Consultar(int vendaBilheteriaID)
        {
            return ado.Consultar(vendaBilheteriaID);
        }

        public List<tVendaBilheteriaEntrega> ConsultarManipulacao(int vendaBilheteriaID)
        {
            return ado.ConsultarManipulacao(vendaBilheteriaID);
        }
    }
}
