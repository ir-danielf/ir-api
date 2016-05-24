using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class PontoVendaADO : MasterADO<dbIngresso>
    {
        public PontoVendaADO(MasterADOBase ado = null) : base(ado) { }

        public List<PontoVenda> ListarPdvPermiteRetirada(string cidade, string uf)
        {
            var sql =
@"SELECT p.ID AS              ID,
       p.ID AS              IR_PontoVendaID,
       p.Local AS           Local,
       p.Nome AS            Nome,
       p.Endereco AS        Endereco,
       p.Numero AS          Numero,
       p.Compl AS           Compl,
       p.Cidade AS          Cidade,
       p.Estado AS          Estado,
       p.Bairro AS          Bairro,
       p.Obs AS             Obs,
       p.Referencia AS      Referencia,
       p.CEP AS             CEP,
       NULL AS              Latitude,
       NULL AS              Longitude,
       p.PermiteRetirada AS PermiteRetirada
FROM tPontoVenda AS p(Nolock)
WHERE p.Cidade = @cidade
      AND p.Estado = @uf
      AND p.PermiteRetirada = 'T'
ORDER BY p.Nome";

            var result = conIngresso.Query<PontoVenda>(sql, new { cidade = cidade, uf = uf });

            return result.ToList();
        }

        public List<PontoVenda> ListarCidade(string cidade, string uf)
        {
            string sql = @"SELECT ID,IR_PontoVendaID,Local,Nome,Endereco,Numero,Compl,Cidade,Estado,Bairro,Obs,Referencia,CEP,Latitude,Longitude
                            FROM PontoVenda
                            where Cidade = @cidade AND Estado = @uf
                            order by Nome";
            var query = conSite.Query<PontoVenda>(sql, new
            {
                cidade = cidade,
                uf = uf
            });

            return query.ToList();
        }

        public PontoVenda Consultar(int id)
        {

            var sql = @"SELECT * FROM tPontoVenda AS pv(NOLOCK) WHERE pv.Id = @id";

            var result = conIngresso.Query<PontoVenda>(sql, new { id = id });

            return result.FirstOrDefault();
        }

    }
}