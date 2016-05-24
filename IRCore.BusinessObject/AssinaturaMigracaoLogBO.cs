using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class AssinaturaMigracaoLogBO : MasterBO<AssinaturaMigracaoLogADO>
    {
        public AssinaturaMigracaoLogBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaMigracaoLogBO() : base(null) { }

        public AssinaturaMigracaoLogModel ConsultaMigrado(int AssinaturaIDOrigem, int AssinaturaIDNovo)
        {
            return ado.ConsultaMigrado(AssinaturaIDOrigem, AssinaturaIDNovo);
        }

        public bool Insere(AssinaturaMigracaoLogModel aml)
        {
            return ado.Insere(aml);
        }
        
        public AssinaturaMigracaoLogModel Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public List<AssinaturaMigracaoLogModel> Busca(string busca)
        {
            return ado.busca(busca);
        }

        public List<AssinaturaMigracaoLogModelRel> RelatorioMigracao(int AssinaturaTipoIDOrigem, int AssinaturaTipoIDNovo, int AssinaturaAnoOrigem, int AssinaturaAnoNovo)
        {
            return ado.RelatorioMigracao(AssinaturaTipoIDOrigem, AssinaturaTipoIDNovo, AssinaturaAnoOrigem, AssinaturaAnoNovo);
        }

        public List<AssinaturaClientesNaoMigradosRel> RelatorioClientesNaoMigrados(int AssinaturaTipoIDOrigem, int AssinaturaTipoIDNovo, int AssinaturaAnoOrigem, int AssinaturaAnoNovo)
        {
            return ado.RelatorioClientesNaoMigrados(AssinaturaTipoIDOrigem, AssinaturaTipoIDNovo, AssinaturaAnoOrigem, AssinaturaAnoNovo);
        }
    }
}
