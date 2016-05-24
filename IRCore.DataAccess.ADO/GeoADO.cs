using Dapper;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.DataAccess.ADO
{
    public class GeoADO : MasterADO<dbSite>
    {
        public GeoADO(MasterADOBase ado = null) : base(ado) { }


        public List<string> ListarEstadoLocal()
        {
            var sql =
@"SELECT DISTINCT
       l.Estado
FROM tLocal AS l(NOLOCK)
     INNER JOIN tEvento AS e(NOLOCK) ON l.ID = e.LocalID
     INNER JOIN tEventoSubtipo AS es(NOLOCK) ON e.EventoSubTipoID = es.ID
     INNER JOIN tEventoTipo AS t(NOLOCK) ON es.eventoTipoID = t.ID
     INNER JOIN tApresentacao AS a(NOLOCK) ON a.EventoID = e.ID
     INNER JOIN tApresentacaoSetor AS aps(NOLOCK) ON aps.ApresentacaoID = a.ID
     INNER JOIN tPreco AS p(NOLOCK) ON p.ApresentacaoSetorID = aps.ID
     INNER JOIN tCanalEvento AS ce(NOLOCK) ON ce.EventoID = e.ID
     INNER JOIN tCanal AS c(NOLOCK) ON c.ID = ce.CanalID
WHERE a.Horario > dbo.GetDateString(GETDATE())
     AND a.DisponivelVenda = 'T'
     AND (p.Valor > 0
          OR (p.Valor = 0
              AND e.DisponivelCortesiaInternet = 'T'))
     AND c.PopulaSiteIR = 'T'";

            var query = conIngresso.Query<string>(sql);

            return query.ToList();
        }

        public List<Estado> ListarEstado()
        {
            string sql = @"SELECT ID, Sigla FROM tEstado(NOLOCK)";

            var result = conIngresso.Query<Estado>(sql);

            return result.ToList();
        }

        public List<string> ListarCidadeLocal(string uf)
        {
            var sql =
@"SELECT DISTINCT
       l.Cidade
FROM tLocal AS l(NOLOCK)
     INNER JOIN tEvento AS e(NOLOCK) ON l.ID = e.LocalID
     INNER JOIN tEventoSubtipo AS es(NOLOCK) ON e.EventoSubTipoID = es.ID
     INNER JOIN tEventoTipo AS t(NOLOCK) ON es.eventoTipoID = t.ID
     INNER JOIN tApresentacao AS a(NOLOCK) ON a.EventoID = e.ID
     INNER JOIN tApresentacaoSetor AS aps(NOLOCK) ON aps.ApresentacaoID = a.ID
     INNER JOIN tPreco AS p(NOLOCK) ON p.ApresentacaoSetorID = aps.ID
     INNER JOIN tCanalEvento AS ce(NOLOCK) ON ce.EventoID = e.ID
     INNER JOIN tCanal AS c(NOLOCK) ON c.ID = ce.CanalID
WHERE a.Horario > dbo.GetDateString(GETDATE())
     AND a.DisponivelVenda = 'T'
     AND (p.Valor > 0
          OR (p.Valor = 0
              AND e.DisponivelCortesiaInternet = 'T'))
     AND c.PopulaSiteIR = 'T'
     AND (@uf = ''
          OR l.Estado = @uf)
ORDER BY l.Cidade";

            var query = conIngresso.Query<string>(sql, new { uf = uf });
            return query.ToList();
        }

        public List<Cidade> ListarCidade(string uf)
        {

            var sql =
@"SELECT c.ID,
       c.Nome,
       c.EstadoID
FROM tCidade AS c(NOLOCK)
     LEFT JOIN tEstado AS e(NOLOCK) ON c.EstadoID = e.ID
WHERE (@uf = ''
          OR e.Sigla = @uf)
ORDER BY c.Nome";

            var query = conIngresso.Query<Cidade>(sql, new { uf = uf });

            return query.ToList();
        }

        public List<Pais> ListarPais()
        {
            string sql = @"SELECT ID,Nome
                           FROM Pais (NOLOCK)";

            var query = conIngresso.Query<Pais>(sql);
            return query.ToList();
        }

        public tCEP BuscarEndereco(string cep)
        {
            string sql = @"SELECT ID,CidadeNome,Endereco,Bairro,CEP,Logradouro,EstadoSigla,EstadoID,CidadeID
                           FROM tCEP (NOLOCK)
                           where cep = @cep";
            var query = conIngresso.Query<tCEP>(sql, new
            {
                cep = cep
            });

            return query.FirstOrDefault();
        }
        public Cidade ConsultarCidade(string nome)
        {
            string sql = @"SELECT *
                           FROM Cidade (NOLOCK)
                           where Nome = @Nome";
            var query = conIngresso.Query<Cidade>(sql, new
            {
                Nome = nome
            });

            return query.FirstOrDefault();
        }

    }
}
