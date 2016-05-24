using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;

namespace IRCore.BusinessObject
{
    public class CortesiaBO : MasterBO<CortesiaADO>
    {
        public CortesiaBO(MasterADOBase ado) : base(ado) { }
        public CortesiaBO() : base(null) { }

        public bool InsereCcortesia(IRCore.DataAccess.Model.CortesiaModel.CortesiaModelInsercao cmi)
        {
            return ado.InsereCCortesia(cmi);
        }

        public bool Insere(IRCore.DataAccess.Model.CortesiaModel.CortesiaModelInsercao cmi)
        {
            return ado.Insere(cmi);
        }

        public int GetID()
        {
            return ado.GetID();
        }
        public CortesiaModel BuscaCortesiaByLocalParceiroMidia(int LocalID, int ParceiroMidiaID)
        {
            return ado.BuscaCortesiaByLocalParceiroMidia(LocalID, ParceiroMidiaID);
        }

        public CortesiaModel BuscaCortesiaByID(int ID)
        {
            return ado.BuscaCortesiaByID(ID);
        }

    }
}
