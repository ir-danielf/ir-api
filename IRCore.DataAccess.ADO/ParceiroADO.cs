using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class ParceiroADO : MasterADO<dbIngresso>
    {
        public ParceiroADO(MasterADOBase ado = null) : base(ado, false) { }


        /// <summary>
        /// Consulta um parceiro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tParceiro Consultar(int id)
        {
            var queryStr = @"
                    SELECT TOP 1 
	                    ID,Parceiro,Tipo,Url
                    FROM
	                    tParceiro (NOLOCK)
                    WHERE
	                    ID = @ParceiroID";
            tParceiro query = conIngresso.Query<tParceiro>(queryStr, new { ParceiroID = id }).FirstOrDefault();
            return query;
        }

        public List<tBloqueio> BloqueiosParceiro(int ParceiroMediaID ,string LocaisID)
        {
            var query = conIngresso.Query<tBloqueio>(
            string.Format(

            @"
            SELECT ID,
                   Nome,
                   CASE SUM(Ativo)
                       WHEN 0
                       THEN 'F'
                       ELSE 'T'
                   END AS Ativo
            FROM
            (
                SELECT DISTINCT
                       tb.ID,
                       tb.Nome COLLATE Latin1_General_CI_AI+' - '+tl.nome COLLATE Latin1_General_CI_AI Nome,
                       Ativo = 1
                FROM tbloqueio tb(NOLOCK)
                     INNER JOIN tParceiroMediaBloqueio pb(NOLOCK) ON pb.BloqueioID = tb.id
                     INNER JOIN dbo.tLocal tl(NOLOCK) ON tl.ID = tb.LocalID
                WHERE pb.ParceiroMidiaID = 0
                      OR (pb.ParceiroMidiaID = {0})
                      AND tb.LocalID IN ({1})
                UNION
                SELECT DISTINCT
                       tb.ID,
                       tb.Nome COLLATE Latin1_General_CI_AI+' - '+tl.nome COLLATE Latin1_General_CI_AI Nome,
                       Ativo = 0
                FROM tbloqueio tb
                     INNER JOIN dbo.tLocal tl(NOLOCK) ON tl.ID = tb.LocalID
                WHERE tb.LocalID IN({1})
            ) AS bloqueios
            GROUP BY ID,
                     Nome;
            ", ParceiroMediaID, LocaisID));

            var result = query.ToList();

            return result;
        }

        public void InserirLocalParceiroMidia(string locaisIds, int parceiroMediaID, int usuarioID)
        {
            conIngresso.Execute(
                string.Format(
                    @"exec  Pr_InserirLocalParceiroMidia '{0}', {1}, {2}
                    ", locaisIds, parceiroMediaID, usuarioID));
        }

        public void InserirBloqueioParceiroMidia(string bloqueiosIds, int parceiroMediaID, int usuarioID)
        {
            conIngresso.Execute(
                string.Format(
                    @"exec  Pr_InserirBloqueioParceiroMidia '{0}', {1}, {2}
                    ", bloqueiosIds, parceiroMediaID, usuarioID));
        }

    }
}
