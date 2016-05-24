using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;

namespace IRCore.BusinessObject
{
    public class DestaqueRegiaoBO : MasterBO<DestaqueRegiaoADO>
    {
        public DestaqueRegiaoBO(MasterADOBase ado = null) : base(ado) { }

        public DestaqueRegiao Consultar(int id, bool loadDependencias = true)
        {
            return ado.Consultar(id);
        }

        public void Remover(DestaqueRegiao destaqueRegiao)
        {
            ado.Remover(destaqueRegiao);
        }
        
        /// <summary>
        /// Salvar Destaque
        /// </summary>
        /// <param name="destaque"></param>
        public void Salvar(DestaqueRegiao destaque)
        {
            ado.Salvar(destaque);
        }

    }
}
