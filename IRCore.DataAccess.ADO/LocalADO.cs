using Dapper;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace IRCore.DataAccess.ADO
{
    public class LocalADO : MasterADO<dbIngresso>
    {
        public LocalADO(MasterADOBase ado = null) : base(ado) { }

        public Local Consultar(int id)
        {
            var sql =
@"SELECT DISTINCT
       l.ID,
       l.ID AS                                                                                                   IR_LocalID,
       l.Nome,
       l.Cidade,
       l.Estado,
       CAST(l.Obs AS   VARCHAR(3000)) AS                                                                         Obs,
       ISNULL(l.Logradouro, N'')+ISNULL(', '+CAST(l.Numero AS NVARCHAR(50)), N'')+ISNULL(' - '+l.Bairro, N'') AS Endereco,
       l.CEP,
       l.DDDTelefone,
       l.Telefone,
       l.ComoChegar,
       emp.TaxaMaximaEmpresa,
       CASE
           WHEN emp.BannerPadraoSite = 'F'
           THEN 0
           ELSE 1
       END AS                                                                                                    BannersPadraoSite,
       emp.ID AS                                                                                                 EmpresaID,
       pais.Nome AS                                                                                              Pais,
       l.ImagemInternet AS                                                                                       Imagem,
       l.CodigoPraca,
       l.Latitude,
       l.Longitude,
       NULL AS                                                                                                   LatitudeAsDecimal,
       NULL AS                                                                                                   LongitudeAsDecimal
FROM tLocal AS l(NOLOCK)
     INNER JOIN tEmpresa AS emp(NOLOCK) ON emp.ID = l.EmpresaID
     INNER JOIN tPais AS pais(NOLOCK) ON pais.ID = l.PaisID
     INNER JOIN tEvento AS e(NOLOCK) ON l.ID = e.LocalID
WHERE l.ID = @localId
      AND e.Publicar IN('T', 'S')";

            var result = conIngresso.Query<Local>(sql, new { localId = id }).ToList();
            return result.FirstOrDefault();
        }

        public InfosObrigatoriasIngresso ListarInfosObrigatoriasIngresso(int localId)
        {
            const string sql =
@"SELECT l.Alvara,
       l.AVCB,
       l.FonteImposto,
       l.PorcentagemImposto,
       l.DataEmissaoAlvara,
       l.DataValidadeAlvara,
       l.DataEmissaoAvcb,
       l.DataValidadeAvcb,
       l.Lotacao
FROM tLocal AS l(NOLOCK)
WHERE l.ID = @localId;";

            var result = conIngresso.Query<InfosObrigatoriasIngresso>(sql, new { localId });
            return result.FirstOrDefault();
        }

        public List<Local> Listar(int limit, string busca = null, string estado = null, string cidade = null)
        {
            /* Aqui tem problema com o take, aparentemente ngm usa o metodo */
            return ListarQuery(busca, estado, cidade).Take(limit).ToList();
        }

        public List<Local> Listar(string busca = null, string estado = null, string cidade = null)
        {
            return ListarQuery(busca, estado, cidade);
        }

        private List<Local> ListarQuery(string busca = null, string estado = null, string cidade = null)
        {
            var sql =
@"SELECT DISTINCT
       l.ID,
       l.ID AS                                                                                                   IR_LocalID,
       l.Nome,
       l.Cidade,
       l.Estado,
       CAST(l.Obs AS   VARCHAR(3000)) AS                                                                         Obs,
       ISNULL(l.Logradouro, N'')+ISNULL(', '+CAST(l.Numero AS NVARCHAR(50)), N'')+ISNULL(' - '+l.Bairro, N'') AS Endereco,
       l.CEP,
       l.DDDTelefone,
       l.Telefone,
       l.ComoChegar,
       emp.TaxaMaximaEmpresa,
       CASE
           WHEN emp.BannerPadraoSite = 'F'
           THEN 0
           ELSE 1
       END AS                                                                                                    BannersPadraoSite,
       emp.ID AS                                                                                                 EmpresaID,
       pais.Nome AS                                                                                              Pais,
       l.ImagemInternet AS                                                                                       Imagem,
       l.CodigoPraca,
       l.Latitude,
       l.Longitude,
       NULL AS                                                                                                   LatitudeAsDecimal,
       NULL AS                                                                                                   LongitudeAsDecimal
FROM tLocal AS l(NOLOCK)
     INNER JOIN tEmpresa AS emp(NOLOCK) ON emp.ID = l.EmpresaID
     INNER JOIN tPais AS pais(NOLOCK) ON pais.ID = l.PaisID
     INNER JOIN tEvento AS e(NOLOCK) ON l.ID = e.LocalID
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
     AND e.Publicar IN('T', 'S')
AND (@cidade IS NULL
     OR l.Cidade = @cidade)
AND (@estado IS NULL
     OR l.Estado = @estado)
AND (@busca IS NULL
     OR (LOWER(l.Nome) LIKE '%'+@busca+'%'
         OR LOWER(l.Cidade) LIKE '%'+@busca+'%'
         OR LOWER(l.Estado) LIKE '%'+@busca+'%'))";

            var query = conIngresso.Query<Local>(sql, new { cidade = cidade, estado = estado, busca = busca }).ToList();
            return query;
        }

        public List<Local> ListarTodos()
        {
            return (from item in dbSite.Local
                    select item).AsNoTracking().ToList();
        }

        public List<tLocal> ListarHistoricoInApresentacao(List<int> apresentacaoIDs)
        {
            var query = dbIngresso.tLocal.Where(u => u.tEvento.Any(t => t.tApresentacao.Any(y => apresentacaoIDs.Contains(y.ID))));
            return query.OrderBy(x => x.Nome).AsNoTracking().ToList();
        }
        
        public List<tLocal> ListarHistoricoLocais(int parceiroMidiaID)
        {
            var query = conIngresso.Query<tLocal>(
            @"SELECT     ID, EmpresaID, Nome, Contato, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Obs, Logradouro, Bairro, Numero, Estacionamento, EstacionamentoObs, AcessoNecessidadeEspecial, 
                              AcessoNecessidadeEspecialObs, ArCondicionado, ComoChegar, RetiradaBilheteria, HorariosBilheteria, ComoChegarInternet, Complemento, ContratoID, PaisID, ImagemInternet, CodigoPraca, 
                              Latitude, Longitude, Alvara, AVCB, FonteImposto, PorcentagemImposto, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, Ativo
                    FROM         tLocal
                    WHERE     (ID IN
                    (SELECT     tLocal.ID
                    FROM          tEvento WITH (nolock) INNER JOIN
                                           tLocal ON tEvento.LocalID = tLocal.ID
                    WHERE      (tEvento.ID IN
                       (SELECT DISTINCT tIngresso.EventoID
                         FROM          tIngresso WITH (nolock) INNER JOIN
                                                tEvento AS tEvento_1 ON tIngresso.EventoID = tEvento_1.ID
                         WHERE      (tIngresso.ParceiroMidiaID = @parceiroMidiaID))))) order by Nome",
            new
            {
                parceiroMidiaID
            });

            var result = query.ToList();

            return result;
        }

        public List<tLocal> ListarLocalDisponivel(int parceiroMidiaID)
        {
            var query = conIngresso.Query<tLocal>(
            @"SELECT tl.ID , tl.EmpresaID , tl.Nome , tl.Contato , tl.Endereco , tl.Cidade , tl.Estado , tl.CEP , tl.DDDTelefone , tl.Telefone , tl.Obs , tl.Logradouro , 
                tl.Bairro , tl.Numero , tl.Estacionamento , tl.EstacionamentoObs , tl.AcessoNecessidadeEspecial , tl.AcessoNecessidadeEspecialObs , tl.ArCondicionado , 
                tl.ComoChegar , tl.RetiradaBilheteria , tl.HorariosBilheteria , tl.ComoChegarInternet , tl.Complemento , tl.ContratoID , tl.PaisID , tl.ImagemInternet ,
                tl.CodigoPraca , tl.Latitude , tl.Longitude , tl.Alvara , tl.AVCB , tl.FonteImposto , tl.PorcentagemImposto , tl.DataEmissaoAlvara , tl.DataValidadeAlvara , 
                tl.DataEmissaoAvcb , tl.DataValidadeAvcb , tl.Lotacao , tl.Ativo
            FROM tLocal AS tl
            WHERE tl.id IN ( SELECT DISTINCT
                                    ti.LocalID
                             FROM tingresso AS ti INNER JOIN dbo.tApresentacao AS ta(NOLOCK) ON ta.id = ti.ApresentacaoID
					    INNER JOIN	SiteIR..Apresentacao	 siteApresentaçao (NOLOCK) ON siteApresentaçao.IR_ApresentacaoID = ta.id
                             WHERE ti.ParceiroMidiaID = @parceiroMidiaID
                                   AND
                                   ta.horario > dbo.GetDateTimeString ( DATEADD(hour , 48 , GETDATE())
                                                                      )
                           )
            ORDER BY tl.nome",
            new
            {
                parceiroMidiaID
            });

            var result = query.ToList();

            return result;
        }

        public List<tLocal> ListarLugaresAtivos(int parceiroMidiaID)
        {
            var query = conIngresso.Query<tLocal>(
                string.Format(
                    @"SELECT  ID,
                               Nome,
                              CASE SUM(Ativo) 
                                WHEN 0 THEN 'F'
                                ELSE 'T'
                                END as Ativo 
                        FROM
                        (
                            SELECT DISTINCT
                                   tl.ID ID,
                                   tl.Nome Nome,
                                   Ativo = 0
                            FROM dbo.tEvento te
                                 INNER JOIN dbo.tApresentacao ta(NOLOCK) ON ta.EventoID = te.id
                                 INNER JOIN dbo.tLocal tl ON tl.ID = te.LocalID
                            WHERE ta.Horario > [dbo].[GetDateTimeString](DATEADD(hour, -48, GETDATE()))
                            UNION
                            SELECT DISTINCT
                                   tl.ID ID,
                                   tl.Nome Nome,
                                   Ativo = 1
                            FROM dbo.tLocal tl
                                 INNER JOIN tParceiroMediaLocal pm(NOLOCK) ON pm.LocalID = tl.id
                                                                              AND ({0} = 0
                                                                                   OR (pm.ParceiroMidiaID = {0}))
                        ) AS Locais
                        GROUP BY ID,
                                 Nome
                        ORDER BY nome
            ", parceiroMidiaID));

            var result = query.ToList();

            return result;
        }
    }
}