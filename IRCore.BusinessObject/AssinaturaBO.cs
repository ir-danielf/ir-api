using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using System;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class AssinaturaBO : MasterBO<AssinaturaADO>
    {
        public AssinaturaBO(MasterADOBase ado) : base(ado) { }
        public AssinaturaBO() : base(null) { }
        public IRCore.DataAccess.Model.AssinaturaModel Consultar(int id)
        {
            return ado.Consultar(id);
        }
        public List<IRCore.DataAccess.Model.AssinaturaModel> BuscaByAssinaturaTipoeAno(int assinaturaTipoID, int assinaturaAnoID)
        {
            return ado.BuscaByAssinaturaTipoeAno(assinaturaTipoID, assinaturaAnoID);
        }

        public void Executa(IRCore.BusinessObject.Models.AssinaturaModel assinaturaModel)
        {
            try
            {
                var mapeamentos = new List<IRLib.NovaTemporada.MapeamentoInfo>();

                foreach (var item in assinaturaModel.Mapeamento)
                {
                        mapeamentos.Add(new IRLib.NovaTemporada.MapeamentoInfo { AnteriorID = item.AnteriorID, NovoID = item.NovoID});                    
                }

                var novaTemporada = new IRLib.NovaTemporada();
                novaTemporada.TipoID = assinaturaModel.AssinaturaTipoID;
                novaTemporada.Mapeamento = mapeamentos;
                novaTemporada.TemporadaAnterior = assinaturaModel.AssinaturaAnoID;
                novaTemporada.TemporadaNova = assinaturaModel.AssinaturaAnoNovoID;
                novaTemporada.TipoIDNovo = assinaturaModel.AssinaturaTipoNovoID;
                novaTemporada.UsuarioID = assinaturaModel.UsuarioID;

                novaTemporada.GerarNovaTemporada();
            }
            catch (Exception ex)
            {
                //retornar erro para o json
            }
        }
    }
}
