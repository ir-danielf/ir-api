using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class GeoBO : MasterBO<GeoADO>
    {
        public GeoBO(MasterADOBase ado = null) : base(ado) { }

        public List<string> ListarEstadoLocal()
        {
            return ado.ListarEstadoLocal();
        }

        public List<string> ListarCidadeLocal(string uf)
        {
            return ado.ListarCidadeLocal(uf);
        }

        public List<Estado> ListarEstado()
        {
            return ado.ListarEstado();
        }


        public List<Cidade> ListarCidade(string uf)
        {
            return ado.ListarCidade(uf);
        }

        public List<Pais> ListarPais()
        {
            return ado.ListarPais();
        }

        public tCEP BuscarEndereco(string cep)
        {
            return ado.BuscarEndereco(cep);
        }

        public Cidade ConsultarCidade(string nome)
        {
            return ado.ConsultarCidade(nome);
        }
    }
}
