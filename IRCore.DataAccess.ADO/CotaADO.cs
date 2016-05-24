using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class CotaADO : MasterADO<dbIngresso>
    {
        public CotaADO(MasterADOBase ado = null) : base(ado, false) { }

        /// <summary>
        /// Consulta uma cota através de um CotaItemID
        /// </summary>
        /// <param name="cotaItemID"></param>
        /// <returns></returns>
        public tCota ConsultarPorCotaItem(int cotaItemID)
        {
            var queryStr = @"
                    SELECT TOP 1
	                    ID,Nome,LocalID,Quantidade,QuantidadePorCliente
                    FROM
	                    tCota (NOLOCK)
                    WHERE
	                    ID = @CotaID";
            tCota query = conIngresso.Query<tCota>(queryStr, new { CotaID = cotaItemID }).FirstOrDefault();
            return query;
        }
    }
}
