using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class PrecoADO : MasterADO<dbIngresso>
    {

        public PrecoADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public PrecoParceiroMidia ConsultarParceiro(int setorId, int apresentacaoId, int parceiroId)
        {
            var sql = @"SELECT ID,IR_PrecoID,Nome,Valor,ApresentacaoID,SetorID,QuantidadePorCliente,Pacote,Serie,CodigoCinema,ParceiroMidiaID
                        FROM PrecoParceiroMidia(NOLOCK)
                        WHERE ParceiroMidiaID = @parceiroId AND SetorID = @setorId
                        AND ApresentacaoID = @apresentacaoID";

            var result = conSite.Query<PrecoParceiroMidia>(sql, new
            {
                parceiroId = parceiroId,
                setorId = setorId,
                apresentacaoId = apresentacaoId
            }).FirstOrDefault();

           return result;
        }


        public tCanalPreco ConsultarCanal(int precoID, int canalID)
        {
            var result = (from item in dbIngresso.tCanalPreco
                          where item.CanalID.Value == canalID && item.PrecoID.Value == precoID
                          select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        public List<Preco> Listar(int setorId, int apresentacaoId)
        {
            string queryString = @"
                    SELECT
	                    ID,IR_PrecoID,Nome,Valor,ApresentacaoID,SetorID,QuantidadePorCliente,Pacote,Serie,CodigoCinema
                    FROM 
	                    Preco (NOLOCK)
                    WHERE
	                    SetorID = @SetorID AND ApresentacaoID = @ApresentacaoID";
            List<Preco> precos = conSite.Query<Preco>(queryString, new
            {
                SetorID = setorId,
                ApresentacaoID = apresentacaoId
            }).ToList();
            return precos;
        }

        public tPreco ConsultarPreco(int setorId, int apresentacaoId, int parceiroId)
        {
            var result = (from item in dbIngresso.tPreco
                          where item.ParceiroMidiaID == parceiroId
                             && item.tApresentacaoSetor.SetorID == setorId
                             && item.tApresentacaoSetor.ApresentacaoID == apresentacaoId
                          select item);
            return result.AsNoTracking().FirstOrDefault();
        }

        public Preco ConsultarMaiorMenorPorApresentacao(int apresentacaoID, bool maior = true)
        {
            var queryStr = @"
                SELECT TOP 1 
                    p.ID, p.IR_PrecoID, p.Nome, p.Valor, p.ApresentacaoID, p.SetorID, p.QuantidadePorCliente, p.Pacote, p.Serie, p.CodigoCinema
                FROM
                    Preco (nolock) p
                WHERE
                    p.ApresentacaoID = @apresentacaoID 
                ORDER BY 
                    p.Valor " + (maior ? "asc" : "desc");
            var query = conSite.Query<Preco>(queryStr, new
            {
                apresentacaoID = apresentacaoID
            });
            return query.FirstOrDefault();
        }
    }
}
