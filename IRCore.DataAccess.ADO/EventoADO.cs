using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using IRCore.Util.Enumerator;
using System.Data.Entity.SqlServer;
using System.Globalization;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.ADO.Enumerator;
using Dapper;
using IRCore.Util;
using System.Configuration;
using System.Data.SqlClient;

namespace IRCore.DataAccess.ADO
{
    public class EventoADO : MasterADO<dbIngresso>
    {
        public EventoADO(MasterADOBase ado = null) : base(ado, true, true) { }

        private List<Evento> result;

        private int addResult(Evento evento, Local local, EventoSubtipo subtipoEvento, Tipo tipoEvento, Apresentacao apresentacao, Preco precoMenor, Preco precoMaior)
        {
            var resultEvento = result.FirstOrDefault(t => t.IR_EventoID == evento.IR_EventoID);
            if (resultEvento == null)
            {
                resultEvento = evento;
                resultEvento.Local = local;
                resultEvento.Apresentacao = new List<Apresentacao>();
                resultEvento.Tipo = tipoEvento;
                resultEvento.Subtipo = subtipoEvento;
                resultEvento.QuantidadeApresentacoes = 0;
                resultEvento.QtdeDisponivel = 0;
                resultEvento.PrimeiraApresentacao = apresentacao;
                result.Add(resultEvento);
            }

            apresentacao.MenorPreco = precoMenor;
            apresentacao.MaiorPreco = precoMaior;

            resultEvento.Apresentacao.Add(apresentacao);
            resultEvento.QuantidadeApresentacoes++;
            resultEvento.QtdeDisponivel += apresentacao.QtdeDisponivel;

            if (resultEvento.PrimeiraApresentacao.Horario != apresentacao.Horario)
            {
                resultEvento.UltimaApresentacao = apresentacao;
            }

            if ((precoMaior != null) && ((resultEvento.MaiorPreco == null) || (precoMaior.Valor > resultEvento.MaiorPreco.Valor)))
            {
                resultEvento.MaiorPreco = precoMaior;
            }
            if ((precoMenor != null) && ((resultEvento.MenorPreco == null) || (precoMenor.Valor < resultEvento.MenorPreco.Valor)))
            {
                resultEvento.MenorPreco = precoMenor;
            }

            return apresentacao.IR_ApresentacaoID;
        }

        public IPagedList<Evento> ListarInApresentacao(int pageNumber, int pageSize, List<int> apresentacaoIDs, List<string> cidades = null, int parceiroMediaID = 0)
        {
            return ListarInApresentacaoQuery(apresentacaoIDs, cidades, parceiroMediaID).AsNoTracking().ToPagedList(pageNumber, pageSize).SelectPagedList(t => t.toEvento());
        }

        public IPagedList<Evento> ListarParceiroNivel(int pageNumber, int pageSize, int nivel, int parceiroMidiaID, List<string> cidades = null)
        {

            int startRow = ((pageNumber - 1) * pageSize);
            int endRow = (pageNumber * pageSize);

            string whereCidades = "";

            if (cidades != null && cidades.Count > 0)
            {
                whereCidades = " and loc.Cidade in (" + string.Join(",", cidades.Select(t => "'" + t.Trim() + "'")) + ") ";
            }


            string queryCount = @" Select COUNT (DISTINCT ev.ID)
                                    FROM Evento ev (NOLOCK) 
                                    INNER JOIN Apresentacao ap (NOLOCK) on ap.EventoID = ev.IR_EventoID
									INNER JOIN vwParceiroMidiaClasseSetor pmcs (NOLOCK) on pmcs.ApresentacaoID = ap.IR_ApresentacaoID
									INNER JOIN Local loc(NOLOCK) on loc.IR_LocalID = ev.LocalID
                                    WHERE pmcs.ParceiroMidiaID = @parceiroMidiaID and pmcs.nivel <= @nivel " + whereCidades;

            int total = conSite.Query<int>(queryCount, new
            {
                parceiroMidiaID = parceiroMidiaID,
                nivel = nivel
            }).FirstOrDefault();

            string queryString = @" Select ev.ID,ev.IR_EventoID,ev.Nome,ev.LocalID,ev.TipoID,ev.Release,ev.Obs,ev.Imagem,ev.Destaque,ev.Prioridade,ev.EntregaGratuita,ev.RetiradaBilheteria,ev.DisponivelAvulso,ev.Parcelas,ev.PublicarSemVendaMotivo,ev.Publicar,ev.SubtipoID,ev.DataAberturaVenda,ev.LocalImagemMapaID,ev.LocalImagemNome,ev.EscolherLugarMarcado,ev.PalavraChave,ev.ExibeQuantidade,ev.BannersPadraoSite,ev.MenorPeriodoEntrega,ev.FilmeID,ev.PermiteVendaPacote,ev.PossuiTaxaProcessamento,ev.LimiteMaximoIngressosEvento,ev.LimiteMaximoIngressosEstado,ev.ImagemDestaque,  
                                    loc.ID,loc.IR_LocalID,loc.Nome,loc.Cidade,loc.Estado,loc.Obs,loc.Endereco,loc.CEP,loc.DDDTelefone,loc.Telefone,loc.ComoChegar,loc.TaxaMaximaEmpresa,loc.BannersPadraoSite,loc.EmpresaID,loc.Pais,loc.Imagem,loc.CodigoPraca,loc.Latitude,loc.Longitude,loc.LongitudeAsDecimal,loc.LatitudeAsDecimal
                                    FROM (SELECT TOP @endRow e.ID, ROW_NUMBER() OVER (ORDER BY e.ID) AS RowNum  From Evento e INNER JOIN Apresentacao ap (NOLOCK) on ap.EventoID = e.IR_EventoID
										INNER JOIN vwParceiroMidiaClasseSetor pmcs (NOLOCK) on pmcs.ApresentacaoID = ap.IR_ApresentacaoID
                                        INNER JOIN PrecoParceiroMidia ppm (NOLOCK) on ppm.ApresentacaoID = ap.IR_ApresentacaoID		
										INNER JOIN Local loc(NOLOCK) on loc.IR_LocalID = e.LocalID								
                                        WHERE pmcs.ParceiroMidiaID = @parceiroMidiaID and pmcs.nivel <= @nivel " + whereCidades + @"
                                        group by e.ID) as temp
										INNER JOIN Evento ev(NOLOCK) on ev.id = temp.id
										INNER JOIN Local loc(NOLOCK) on loc.IR_LocalID = ev.LocalID
										where RowNum > @startRow";

            queryString = queryString.Replace("@endRow", endRow.ToString());


            List<Evento> result = conSite.Query<Evento, Local, Evento>(queryString, addResultListarParceiroNivel, new
            {
                parceiroMidiaID = parceiroMidiaID,
                nivel = nivel,
                startRow = startRow
            }).ToList();

            return result.ToPagedList(pageNumber, pageSize, total);


        }

        public Evento addResultListarParceiroNivel(Evento evento, Local local)
        {
            evento.Local = local;
            return evento;
        }

        public List<Evento> ListarInApresentacao(List<int> apresentacaoIDs, List<string> cidades = null, int parceiroMediaID = 0)
        {
            return ListarInApresentacaoQuery(apresentacaoIDs, cidades, parceiroMediaID).AsNoTracking().ToList().Select(t => t.toEvento()).ToList();
        }

        public List<tEvento> ListarHistoricoInApresentacao(List<int> apresentacaoIDs)
        {
            var query = dbIngresso.tEvento.Where(t => t.tApresentacao.Any(x => apresentacaoIDs.Contains(x.ID)));

            return query.OrderBy(x => x.Nome).AsNoTracking().ToList();

        }

        public List<tEvento> ListarHistoricoEventos(int ParceiroMidiaID)
        {
            var query = conIngresso.Query<tEvento>(
            @"SELECT     ID, LocalID, EventoTipoID, Nome, DescricaoResumida, DescricaoDetalhada, VersaoImagemIngresso, VersaoImagemVale, Obs, VendaDistribuida, VersaoImagemVale2, VersaoImagemVale3, 
                      ImpressaoCodigoBarra, ObrigaCadastroCliente, CodigoBarraEstruturado, DesabilitaAutomatico, Resenha, Publicar, Destaque, PrioridadeDestaque, ImagemInternet, EntregaGratuita, 
                      RetiradaBilheteria, Parcelas, Financeiro, Atencao, PDVSemConveniencia, RetiradaIngresso, MeiaEntrada, Promocoes, AberturaPortoes, DuracaoEvento, Release, DescricaoPadraoApresentacao, 
                      Censura, EntradaAcompanhada, PublicarSemVendaMotivo, ContratoID, PermitirVendaSemContrato, EventoSubTipoID, LocalImagemMapaID, DataAberturaVenda, ObrigatoriedadeID, 
                      EscolherLugarMarcado, MapaEsquematicoID, PalavraChave, ExibeQuantidade, DisponivelCortesiaInternet, NivelRisco, TaxaDistribuida, MenorPeriodoEntrega, TipoCodigoBarra, TipoImpressao, 
                      ImagemDestaque, FilmeID, PermiteVendaPacote, LimiteMaximoIngressosEvento, HabilitarRetiradaTodosPDV, CodigoPos, BaseCalculo, TipoCalculoDesconto, TipoCalculo, Alvara, AVCB, 
                      VendaSemAlvara, FonteImposto, PorcentagemImposto, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, VenderPos, Ativo
                        FROM         tEvento WITH (nolock)
                        WHERE     (ID IN
                                                  (SELECT DISTINCT tIngresso.EventoID
                                                    FROM          tIngresso WITH (nolock) INNER JOIN
                                                                           tEvento ON tIngresso.EventoID = tEvento.ID
                                                    WHERE      (tIngresso.ParceiroMidiaID = @parceiroMidiaID))) order by Nome",
            new
            {
                ParceiroMidiaID
            });

            var result = query.ToList();

            return result;
        }

        public List<tEvento> ListarEventoDisponivel(int ParceiroMidiaID)
        {
            var query = conIngresso.Query<tEvento>(
            @"
            SELECT ID , LocalID , EventoTipoID , Nome , DescricaoResumida , DescricaoDetalhada , VersaoImagemIngresso , VersaoImagemVale , Obs , VendaDistribuida , VersaoImagemVale2 , 
            VersaoImagemVale3 , ImpressaoCodigoBarra , ObrigaCadastroCliente , CodigoBarraEstruturado , DesabilitaAutomatico , Resenha , Publicar , Destaque , PrioridadeDestaque , 
            ImagemInternet , EntregaGratuita , RetiradaBilheteria , Parcelas , Financeiro , Atencao , PDVSemConveniencia , RetiradaIngresso , MeiaEntrada , Promocoes , AberturaPortoes , 
            DuracaoEvento , Release , DescricaoPadraoApresentacao , Censura , EntradaAcompanhada , PublicarSemVendaMotivo , ContratoID , PermitirVendaSemContrato , EventoSubTipoID , LocalImagemMapaID , 
            DataAberturaVenda , ObrigatoriedadeID , EscolherLugarMarcado , MapaEsquematicoID , PalavraChave , ExibeQuantidade , DisponivelCortesiaInternet , NivelRisco , TaxaDistribuida , MenorPeriodoEntrega , 
            TipoCodigoBarra , TipoImpressao , ImagemDestaque , FilmeID , PermiteVendaPacote , LimiteMaximoIngressosEvento , HabilitarRetiradaTodosPDV , CodigoPos , BaseCalculo , TipoCalculoDesconto , TipoCalculo , 
            Alvara , AVCB , VendaSemAlvara , FonteImposto , PorcentagemImposto , DataEmissaoAlvara , DataValidadeAlvara , DataEmissaoAvcb , DataValidadeAvcb , Lotacao , VenderPos , Ativo
            FROM tEvento WITH (NOLOCK)
            WHERE ID IN ( SELECT DISTINCT
                                 ti.EventoID
                          FROM tIngresso AS ti INNER JOIN tApresentacao AS ta(NOLOCK) ON ti.ApresentacaoID = ta.id
                          INNER JOIN	SiteIR..Apresentacao	 siteApresentaçao (NOLOCK) ON siteApresentaçao.IR_ApresentacaoID = ta.id
					 WHERE ti.ParceiroMidiaID = @parceiroMidiaID AND 
                                ta.horario > dbo.GetDateTimeString ( DATEADD(hour , 48 , GETDATE())
                                                                   ))
            ORDER BY Nome
            ",
            new
            {
                ParceiroMidiaID
            });

            var result = query.ToList();

            return result;
        }

        private IQueryable<EventoModelQuery> ListarInApresentacaoQuery(List<int> apresentacaoIDs, List<string> cidades = null, int parceiroMediaID = 0)
        {

            var query = ConsultaComMapeamento();

            query = query.Where(t => t.apresentacoes.Any(x => apresentacaoIDs.Contains(x.apresentacao.IR_ApresentacaoID)));


            if (parceiroMediaID > 0)
            {
                query = query.Where(t => t.apresentacoes.Any(x => x.apresentacao.PrecoParceiroMidia.Select(y => y.ParceiroMidiaID).Contains(parceiroMediaID)));
            }
            if (cidades != null && cidades.Count > 0)
            {
                query = query.Where(t => cidades.Contains(t.local.Cidade));
            }
            return query.OrderBy(x => x.evento.IR_EventoID);

        }

        public Evento Consultar(int eventoId, int canalId = 2, bool isPos = false)
        {
            var diffHour = isPos ? 12 : 2;

            var sql =
@"SELECT e.ID,
       e.Id AS                                                                                                   IR_EventoID,
       e.Nome,
       e.LocalID,
       CAST(Resenha AS   VARCHAR(5000)) AS                                                                       Release,
       e.ImagemInternet AS                                                                                       Imagem,
       IIF(e.RetiradaBilheteria = 'T', 1, 0) AS                                                                  RetiradaBilheteria,
       1 AS                                                                                                    DisponivelAvulso,
       e.PublicarSemVendaMotivo,
       e.Publicar,
       e.eventoSubtipoId AS                                                                                      SubtipoID,
       e.DataAberturaVenda,
       IIF(e.ExibeQuantidade = 'T', 1, 0) AS                                                                     ExibeQuantidade,
       e.FilmeID,
       IIF(e.PermiteVendaPacote = 'T', 1, 0) AS                                                                  PermiteVendaPacote,
       IIF((ce.TaxaConveniencia = 0
            AND uf.PossuiTaxaProcessamento = 'T')
           OR uf.PossuiTaxaProcessamento = 'F', 0, 1) AS                                                         PossuiTaxaProcessamento,
       e.EntradaFranca,
       e.OcultarHoraApresentacao,
       l.ID,
       l.Id AS                                                                                                   IR_LocalID,
       l.Nome,
       l.Cidade,
       l.Estado,
       ISNULL(l.Logradouro, N'')+ISNULL(', '+CAST(l.Numero AS NVARCHAR(50)), N'')+ISNULL(' - '+l.Bairro, N'') AS Endereco,
       l.CEP,
       l.DDDTelefone,
       l.Telefone,
       l.EmpresaID,
       pais.Nome AS                                                                                              Pais,
       l.ImagemInternet AS                                                                                       Imagem,
       l.Latitude,
       l.Longitude,
       l.LongitudeAsDecimal,
       l.LatitudeAsDecimal,
       st.ID,
       st.Id AS                                                                                                  IR_SubtipoID,
       st.eventoTipoId AS                                                                                        TipoID,
       st.Descricao,
       t.ID,
       t.Id AS                                                                                                   IR_TipoID,
       t.Nome,
       a.ID,
       a.Id AS                                                                                                   IR_ApresentacaoID,
       a.Horario,
       a.EventoID,
       IIF(ISNULL(a.mapaEsquematicoId, 0) > 0
           OR ISNULL(e.mapaEsquematicoId, 0) > 0, 1, 0) AS                                                       UsarEsquematico,
       a.Programacao,
       a.CodigoProgramacao,
       a.CalcDiaDaSemana,
       a.CalcHorario,
       ( SELECT COUNT(i.Id) AS Quantidade
         FROM tApresentacaoSetor AS aps(NOLOCK)
              LEFT JOIN tIngresso AS i(NOLOCK) ON i.apresentacaoSetorId = aps.Id
                                                  AND i.Status = 'D'
         WHERE aps.apresentacaoId = a.Id
         GROUP BY aps.apresentacaoId ) AS                                                                        QtdeDisponivel,
       pmenor.ID,
       pmenor.Id AS                                                                                              IR_PrecoID,
       pmenor.Nome,
       pmenor.Valor,
       a.Id AS                                                                                                   ApresentacaoID,
       0 AS                                                                                                      SetorID,
       pmenor.QuantidadePorCliente,
       0 AS                                                                                                      Pacote,
       0 AS                                                                                                      Serie,
       pmenor.CodigoCinema,
       pmaior.ID,
       pmaior.Id AS                                                                                              IR_PrecoID,
       pmaior.Nome,
       pmaior.Valor,
       a.Id AS                                                                                                   ApresentacaoID,
       0 AS                                                                                                      SetorID,
       pmaior.QuantidadePorCliente,
       0 AS                                                                                                      Pacote,
       0 AS                                                                                                      Serie,
       pmaior.CodigoCinema
FROM tEvento(NOLOCK) AS e
     INNER JOIN tCanalEvento AS ce WITH (NOLOCK) ON ce.eventoId = e.Id AND ce.canalId = @canalId
     INNER JOIN
     ( SELECT DATEDIFF(minute, CalcHorario, GETDATE()) AS DIFF_DATE_MINUTE,
              *
       FROM dbo.tApresentacao(NOLOCK)
       WHERE DisponivelVenda = 'T' ) AS a ON e.Id = a.eventoID
     INNER JOIN tLocal(NOLOCK) AS l ON e.LocalID = l.Id
     INNER JOIN tEventoSubtipo(NOLOCK) AS st ON st.Id = e.eventoSubtipoId
     INNER JOIN tEventoTipo(NOLOCK) AS t ON t.Id = st.eventoTipoId
     LEFT JOIN tPreco(NOLOCK) AS pmenor ON pmenor.Id =
     ( SELECT MAX(p1.Id)
       FROM tPreco AS p1(NOLOCK)
            INNER JOIN tApresentacaoSetor AS aps1(NOLOCK) ON aps1.Id = p1.apresentacaoSetorId
       WHERE aps1.apresentacaoId = a.Id
             AND p1.Valor =
             ( SELECT MIN(p2.Valor)
               FROM tPreco AS p2(NOLOCK)
                    INNER JOIN tApresentacaoSetor AS aps2(NOLOCK) ON aps2.Id = p2.apresentacaoSetorId
                    INNER JOIN tCanalPreco AS cp2 ON cp2.precoId = p2.Id AND cp2.canalId = @canalId
               WHERE aps2.apresentacaoId = a.Id
                     AND (p2.Valor > 0
                          OR (p2.Valor = 0
                              AND e.disponivelCortesiaInternet = 'T'))
                     AND NOT EXISTS
                                    ( SELECT TOP 1 *
                                      FROM tPacoteItem AS pi1(nolock)
                                      WHERE pi1.precoId = p2.id ) ) )
     LEFT JOIN tPreco(nolock) AS pmaior ON pmaior.Id =
     ( SELECT MAX(p1.Id)
       FROM tPreco AS p1(NOLOCK)
            INNER JOIN tApresentacaoSetor AS aps1(NOLOCK) ON aps1.Id = p1.apresentacaoSetorId
       WHERE aps1.apresentacaoId = a.Id
             AND p1.Valor =
             ( SELECT MAX(p2.Valor)
               FROM tPreco AS p2(NOLOCK)
                    INNER JOIN tApresentacaoSetor AS aps2(NOLOCK) ON aps2.Id = p2.apresentacaoSetorId
                    INNER JOIN tCanalPreco AS cp2 ON cp2.precoId = p2.Id
                                                     AND cp2.canalId = @canalId
               WHERE aps2.apresentacaoId = a.Id
                     AND (p2.Valor > 0
                          OR (p2.Valor = 0
                              AND e.disponivelCortesiaInternet = 'T'))
                     AND NOT EXISTS
                                    ( SELECT TOP 1 *
                                      FROM tPacoteItem AS pi1(nolock)
                                      WHERE pi1.precoId = p2.id ) ) )
     LEFT OUTER JOIN tEstado AS uf(NOLOCK) ON uf.Sigla COLLATE Latin1_General_CI_AI = l.Estado
     INNER JOIN tPais AS pais(NOLOCK) ON pais.Id = l.paisId
WHERE e.Id = @eventoId
      AND GETDATE() < DATEADD(hh, {=diffHour}, a.CalcHorario)
ORDER BY a.Horario;";

            result = new List<Evento>();

            var query = conIngresso.Query<Evento, Local, EventoSubtipo, Tipo, Apresentacao, Preco, Preco, int>(sql, addResult, new { eventoId = eventoId, canalId = canalId, diffHour = diffHour });

            return result.FirstOrDefault();
        }

        public InfosObrigatoriasIngresso ListarInfosObrigatoriasIngresso(int idEvento)
        {
            var queryStr =
@"SELECT 
e.Alvara, e.AVCB, e.FonteImposto, e.PorcentagemImposto, e.DataEmissaoAlvara, e.DataValidadeAlvara, e.DataEmissaoAvcb, e.DataValidadeAvcb, e.Lotacao, e.LocalID, eb.ProdutorRazaoSocial, eb.ProdutorCpfCnpj, e.Atencao
FROM tEvento (nolock) e LEFT JOIN tEventoBordero (nolock) eb ON (e.ID = eb.EventoID)
WHERE e.ID = @idEvento";

            var query = conIngresso.Query<InfosObrigatoriasIngresso>(queryStr, new
            {
                idEvento
            });

            return query.FirstOrDefault();
        }

        public Evento ConsultarOSESP(int idEvento)
        {
            var queryStr = @"SELECT 
  e.ID, e.IR_EventoID, e.Nome, e.LocalID, e.Release, e.Imagem, e.RetiradaBilheteria, e.DisponivelAvulso, e.PublicarSemVendaMotivo, e.Publicar, e.SubtipoID, e.DataAberturaVenda, e.ExibeQuantidade, e.FilmeID, e.PermiteVendaPacote, e.PossuiTaxaProcessamento,
  l.ID, l.IR_LocalID, l.Nome, l.Cidade, l.Estado, l.Endereco, l.CEP, l.DDDTelefone, l.Telefone, l.EmpresaID, l.Pais, l.Imagem, l.Latitude, l.Longitude, l.LongitudeAsDecimal, l.LatitudeAsDecimal,
  st.ID, st.IR_SubtipoID, st.TipoID, st.Descricao,
  t.ID, t.IR_TipoID, t.Nome, 
  a.ID, a.IR_ApresentacaoID, a.Horario, a.EventoID, a.UsarEsquematico, a.Programacao, a.CodigoProgramacao, a.CalcDiaDaSemana, a.CalcHorario, (SELECT SUM(QtdeDisponivel) FROM Setor (nolock) WHERE ApresentacaoID = a.IR_ApresentacaoID) as QtdeDisponivel,
  pmenor.ID, pmenor.IR_PrecoID, pmenor.Nome, pmenor.Valor, pmenor.ApresentacaoID, pmenor.SetorID, pmenor.QuantidadePorCliente, pmenor.Pacote, pmenor.Serie, pmenor.CodigoCinema, 
  pmaior.ID, pmaior.IR_PrecoID, pmaior.Nome, pmaior.Valor, pmaior.ApresentacaoID, pmaior.SetorID, pmaior.QuantidadePorCliente, pmaior.Pacote, pmaior.Serie, pmaior.CodigoCinema
--FROM         API_Osesp_Eventos AS eOSESP WITH (nolock)
FROM Apresentacao AS a WITH (nolock)
INNER JOIN Evento AS e WITH (nolock) ON a.EventoID = e.IR_EventoID 
--INNER JOIN API_Osesp_Eventos AS eOSESP WITH (nolock) ON e.IR_EventoID = eOSESP.ID 
INNER JOIN Local AS l WITH (nolock) ON e.LocalID = l.IR_LocalID 
INNER JOIN EventoSubtipo AS st WITH (nolock) ON st.IR_SubtipoID = e.SubtipoID 
INNER JOIN Tipo AS t WITH (nolock) ON t.IR_TipoID = st.TipoID 
--INNER JOIN Apresentacao AS a WITH (nolock) ON a.EventoID = e.IR_EventoID 
LEFT OUTER JOIN Preco AS pmenor WITH (nolock) ON pmenor.ID =
    (SELECT     MAX(ID) AS Expr1
    FROM          Preco AS p1 WITH (nolock)
    WHERE      (ApresentacaoID = a.IR_ApresentacaoID) AND (Valor =
                                (SELECT     MIN(p2.Valor) AS Expr1
                                    FROM          Preco AS p2 WITH (nolock) INNER JOIN
                                                        Setor AS s2 ON p2.SetorID = s2.IR_SetorID AND s2.PrincipalPrecoID = p2.IR_PrecoID
                                    WHERE      (p2.ApresentacaoID = a.IR_ApresentacaoID) AND (p2.Pacote = 0)))) LEFT OUTER JOIN
                      Preco AS pmaior WITH (nolock) ON pmaior.ID =
                          (SELECT     MAX(ID) AS Expr1
                            FROM          Preco AS p1 WITH (nolock)
                            WHERE      (ApresentacaoID = a.IR_ApresentacaoID) AND (Valor =
                                                       (SELECT     MAX(p2.Valor) AS Expr1
                                                         FROM          Preco AS p2 WITH (nolock) INNER JOIN
                                                                                Setor AS s2 ON p2.SetorID = s2.IR_SetorID AND s2.PrincipalPrecoID = p2.IR_PrecoID
                                                         WHERE      (p2.ApresentacaoID = a.IR_ApresentacaoID) AND (p2.Pacote = 0))))
WHERE     (e.IR_EventoID = @eventoID)
ORDER BY a.Horario".Replace("@eventoID", idEvento.ToString());


            result = new List<Evento>();
            var query = conSite.Query<Evento, Local, EventoSubtipo, Tipo, Apresentacao, Preco, Preco, int>(queryStr, addResult, new
            {
                eventoID = idEvento
            });

            return result.FirstOrDefault();
        }

        public Evento ConsultarMediaPartner(int eventoID, int parceiroMidiaID, int nivel)
        {
            string sql = @"Select ev.ID,ev.IR_EventoID,ev.Nome,ev.LocalID,ev.TipoID,ev.Release,ev.Obs,ev.Imagem,ev.Destaque,ev.Prioridade,ev.EntregaGratuita,ev.RetiradaBilheteria,ev.DisponivelAvulso,ev.Parcelas,ev.PublicarSemVendaMotivo,ev.Publicar,ev.SubtipoID,ev.DataAberturaVenda,ev.LocalImagemMapaID,ev.LocalImagemNome,ev.EscolherLugarMarcado,ev.PalavraChave,ev.ExibeQuantidade,ev.BannersPadraoSite,ev.MenorPeriodoEntrega,ev.FilmeID,ev.PermiteVendaPacote,ev.PossuiTaxaProcessamento,ev.LimiteMaximoIngressosEvento,ev.LimiteMaximoIngressosEstado,ev.ImagemDestaque,  
                            loc.ID,loc.IR_LocalID,loc.Nome,loc.Cidade,loc.Estado,loc.Obs,loc.Endereco,loc.CEP,loc.DDDTelefone,loc.Telefone,loc.ComoChegar,loc.TaxaMaximaEmpresa,loc.BannersPadraoSite,loc.EmpresaID,loc.Pais,loc.Imagem,loc.CodigoPraca,loc.Latitude,loc.Longitude,loc.LongitudeAsDecimal,loc.LatitudeAsDecimal,
		                    ap.ID,ap.IR_ApresentacaoID,ap.Horario,ap.EventoID,ap.UsarEsquematico,ap.Programacao,ap.CodigoProgramacao,ap.CalcDiaDaSemana,ap.CalcHorario
                            From Evento ev(NOLOCK) 
		                    INNER JOIN Apresentacao ap (NOLOCK) on ap.EventoID = ev.IR_EventoID
    		                INNER JOIN Setor st (nolock) on st.ApresentacaoID = ap.IR_ApresentacaoID
    		                INNER JOIN vwParceiroMidiaClasseSetor pmcs (NOLOCK) on pmcs.ApresentacaoID = ap.IR_ApresentacaoID and pmcs.SetorID = st.IR_SetorID
		                    INNER JOIN PrecoParceiroMidia ppm (NOLOCK) on ppm.ApresentacaoID = ap.IR_ApresentacaoID and pmcs.ParceiroMidiaID = ppm.ParceiroMidiaID and pmcs.SetorID = ppm.setorID
		                    INNER JOIN Local loc(NOLOCK) on loc.IR_LocalID = ev.LocalID
		                    WHERE ppm.ParceiroMidiaID = @parceiroMidiaID and ev.IR_EventoID = @eventoID and pmcs.nivel <= @nivel
		                    order by ap.calchorario";
            Evento obj = null;
            var result = conSite.Query<Evento, Local, Apresentacao, int>(sql, (evento, local, apresentacao) =>
            {
                if (obj == null)
                {
                    obj = evento;
                    obj.Apresentacao = new List<Apresentacao>();
                    obj.Local = local;
                }
                obj.Apresentacao.Add(apresentacao);

                return apresentacao.IR_ApresentacaoID;
            }, new
            {
                parceiroMidiaID = parceiroMidiaID,
                eventoID = eventoID,
                nivel = nivel
            }).ToList();

            return obj;
        }

        public IQueryable<Evento> ConsultarQueyrable(int idEvento)
        {
            return (from evento in dbSite.Evento.Include(t => t.Apresentacao)
                    join local in dbSite.Local on evento.LocalID equals local.IR_LocalID
                    join subtipo in dbSite.EventoSubtipo on evento.SubtipoID equals subtipo.IR_SubtipoID
                    join tipo in dbSite.Tipo on subtipo.TipoID equals tipo.IR_TipoID
                    where evento.Apresentacao.Count > 0 && local != null && subtipo != null && tipo != null && evento.IR_EventoID == idEvento
                    select evento);
        }

        public List<tIngresso> ListarMapaSetor(int setorId, int apresentacaoId, int parceiroId = 0)
        {

            var query = (from item in dbIngresso.tIngresso.Include(t => t.tLugar)
                         where item.SetorID == setorId
                            && item.ApresentacaoID == apresentacaoId
                         select item);
            if (parceiroId != 0)
                query = query.Where(x => x.ParceiroMidiaID == parceiroId);

            var result = query.AsNoTracking().ToList();

            return result;

        }

        public List<MapaAcentoModel> ListarMapaSetorObject(int setorId, int apresentacaoId, int? clienteId, string sessionId)
        {
            //TODO: Vilmar: Retirar a transação
            //int trans = IniciarTransacao();
            /*Montagem da Query que retorna só os itens necessários 
             * id = id do ingresso
             * cd = código do ingresso
             * st = Status do assento podendo ser D para disponivel, ND para não disponivel e SA para Seu assento
             * tp = tipo de lugar
             * tt = total de assentos
             * rv = assentos reservados
             * px = ponto x do local do assento
             * py = ponto y do local do assento
             * pl = perspectiva lugar, imagem do setor
             */
            var query = conIngresso.Query<MapaAcentoModel>(
                        @"SELECT
                            i.ID,
                            i.Codigo                        AS cd,
                            CASE WHEN i.Status = 'D'
                            THEN 'D'
                            ELSE CASE WHEN i.Status = 'R' AND i.SessionID = @sessionID
                            THEN 'SA'
                                ELSE 'ND' END END           AS st,
                            s.LugarMarcado                  AS tp,
                            l.PNETipo                       AS pne,
                            l.Quantidade                    AS tt,
                            l.QuantidadeBloqueada           AS rv,
                            l.PosicaoX                      AS px,
                            l.PosicaoY                      AS py,
                            IsNull(l.PerspectivaLugarID, 0) AS pl
                        FROM tIngresso i ( NOLOCK )
                            INNER JOIN tSetor s ( NOLOCK ) ON i.SetorID = s.ID
                            LEFT JOIN tLugar l ( NOLOCK ) ON i.LugarID = l.ID
                            LEFT JOIN tPerspectivaLugar pl ( NOLOCK ) ON pl.ID = l.PerspectivaLugarID
                        WHERE i.SetorID = @setorID AND i.ApresentacaoID = @apresentacaoID",
                        new
                        {
                            clienteId,
                            sessionId,
                            setorId,
                            apresentacaoId
                        });

            var result = query.ToList();

            return result;

        }

        public List<MapaAssentoMesaModel> ListarMapaMesaSetorObject(int setorId, int apresentacaoId, string sessionId)
        {
            /*Montagem da Query que retorna só os itens necessários 
             * id = id da mesa
             * cd = código da mesa
             * tt = total de assentos
             * qd = quantidade de assentos disponíveis   
             * sa = quantidade de assentos reservados pelo usuário (seu assento)
             * rv = assentos reservados
             * px = ponto x do local do assento
             * py = ponto y do local do assento
             * pl = perspectiva lugar, imagem do setor
             */
            var query = conIngresso.Query<MapaAssentoMesaModel>(
                        @"SELECT
                              LugarID                   AS id,
                              Codigo                    AS cd,
                              Quantidade                AS tt,
                              SUM(QuantidadeDisponivel) AS qd,
                              SeuAssento                AS sa,
                              Reservado                 AS rv,
                              PosicaoX                  AS px,
                              PosicaoY                  AS py,
                              PerspectivaLugarID        AS pl
                            FROM
                              (SELECT DISTINCT
                                 i.LugarID,
                                 i.ID,
                                 l.Quantidade,
                                 SUM(CASE i.Status
                                     WHEN 'D'
                                       THEN 1
                                     ELSE 0 END)                                                            AS QuantidadeDisponivel,
                                 SUM(CASE WHEN i.Status = 'R' AND i.SessionID = @sessionId
                                   THEN 1
                                     ELSE 0 END)                                                            AS SeuAssento,
                                 l.QuantidadeBloqueada                                                      AS Reservado,
                                 l.PosicaoX,
                                 l.PosicaoY,
                                 substring(i.Codigo, 1, LEN(i.Codigo) - patindex('%-%', reverse(i.Codigo))) AS Codigo,
                                 IsNull(l.PerspectivaLugarID, 0)                                            AS PerspectivaLugarID
                               FROM tIngresso i ( NOLOCK )
                                 INNER JOIN tLugar l ( NOLOCK ) ON i.LugarID = l.ID
                                 INNER JOIN tSetor s ( NOLOCK ) ON i.SetorID = s.ID AND l.SetorID = s.ID
                                 LEFT JOIN tPerspectivaLugar pl ( NOLOCK ) ON pl.ID = l.PerspectivaLugarID
                               WHERE i.Codigo <> '' AND i.ApresentacaoID = @apresentacaoId AND s.ID = @setorId AND s.AprovadoPublicacao = 'T'
                               GROUP BY i.LugarID, i.ID, i.Codigo, l.Quantidade, l.QuantidadeBloqueada, l.PosicaoX, l.PosicaoY, s.LugarMarcado, l.PerspectivaLugarID
                              ) AS a
                            GROUP BY LugarID, Codigo, Quantidade, SeuAssento, Reservado, PosicaoX, PosicaoY, PerspectivaLugarID
                            ORDER BY cd",
                        new
                        {
                            setorId,
                            apresentacaoId,
                            sessionId
                        });

            var result = query.ToList();

            return result;

        }

        public List<MapaAcentoModel> ListarMapaSetorObjectOSESP(int setorId, int apresentacaoId)
        {
            //int trans = IniciarTransacao();
            /*Montagem da Query que retorna só os itens necessários 
             * id = id do ingresso
             * cd = código do ingresso
             * st = Status do assento podendo ser D para disponivel, ND para não disponivel e SA para Seu assento
             * tp = tipo de lugar
             * tt = total de assentos
             * rv = assentos reservados
             * px = ponto x do local do assento
             * py = ponto y do local do assento
             * bq = nome do bloqueio
             */

            var query = conIngresso.Query<MapaAcentoModel>(
            @"select 
	            i.ID, 
	            i.Codigo as cd, 
                i.Status as st,
	            s.LugarMarcado as tp, 
	            l.Quantidade as tt, 
	            l.QuantidadeBloqueada as rv, 
	            l.PosicaoX as px, 
	            l.PosicaoY as py,
	            b.Nome as bn,
	            b.ID as bid
            from tIngresso i (nolock)
            inner join API_Osesp_Eventos osespE (nolock) on osespE.ID = i.EventoID
            inner join tSetor s (nolock) on i.SetorID = s.ID
            inner join tLugar l (nolock) on i.LugarID = l.ID
            left join tBloqueio b (nolock) on b.ID = i.BloqueioID
            where i.SetorID = @setorId and i.ApresentacaoID = @apresentacaoId
            group by i.ID, b.ID, i.Codigo, i.Status, s.LugarMarcado, l.Quantidade, l.QuantidadeBloqueada, l.PosicaoX, l.PosicaoY, b.ID, b.Nome",
            new
            {
                setorId = setorId,
                apresentacaoId = apresentacaoId
            });

            var result = query.ToList();

            //FinalizarTransacao(trans);

            return result;

        }

        public List<tIngresso> ListarMapaSetorSemJoin(int setorId, int apresentacaoId)
        {
            //TODO: Vilmar Refatorar
            //int trans = IniciarTransacao();

            var query = (from item in dbIngresso.tIngresso.Include(x => x.tLugar)
                         where item.SetorID == setorId
                            && item.ApresentacaoID == apresentacaoId
                         select item);

            var result = query.ToList();

            // FinalizarTransacao(trans);

            return result;
        }

        public List<tLugar> RetornaLugares(List<int> setoresID)
        {
            return (from item in dbIngresso.tLugar
                    where setoresID.Contains(item.ID)
                    select item).ToList();
        }

        private IQueryable<EventoModelQuery> ConsultaComMapeamento(double? latitude = null, double? longitude = null)
        {
            IQueryable<EventoModelQuery> query;

            if (latitude != null && longitude != null)
            {
                double pi = System.Math.PI;
                double auxPi = pi / 180;

                query = (from evento in dbSite.Evento.Include(t => t.Apresentacao)
                         join local in dbSite.Local on evento.LocalID equals local.IR_LocalID
                         join subtipo in dbSite.EventoSubtipo on evento.SubtipoID equals subtipo.IR_SubtipoID
                         join tipo in dbSite.Tipo on subtipo.TipoID equals tipo.IR_TipoID
                         where evento.Apresentacao.Count > 0 && local != null && subtipo != null && tipo != null && local.LatitudeAsDecimal != null && local.LongitudeAsDecimal != null
                         select new EventoModelQuery()
                         {
                             evento = evento,
                             local = local,
                             tipo = tipo,
                             subtipo = subtipo,
                             apresentacoes = evento.Apresentacao.OrderBy(t => t.CalcHorario).Select(apresentacao => new ApresentacaoModelQuery()
                             {
                                 apresentacao = apresentacao,
                                 qtdeDisponivel = apresentacao.Setor.Sum(t => t.QtdeDisponivel.Value),
                                 menorPreco = dbSite.Preco.Where(p => p.ApresentacaoID.Value == apresentacao.IR_ApresentacaoID).OrderBy(t => t.Valor).FirstOrDefault(),
                                 maiorPreco = dbSite.Preco.Where(p => p.ApresentacaoID.Value == apresentacao.IR_ApresentacaoID).OrderByDescending(t => t.Valor).FirstOrDefault(),
                             }).ToList(),
                             distancia = (40030 * ((180 / pi) * SqlFunctions.Acos(SqlFunctions.Cos((90 - ((double)local.LatitudeAsDecimal.Value)) * auxPi) * SqlFunctions.Cos((90 - ((double)latitude)) * auxPi) + SqlFunctions.Sin((90 - ((double)local.LatitudeAsDecimal.Value)) * auxPi) * SqlFunctions.Sin((90 - ((double)latitude)) * auxPi) * SqlFunctions.Cos((((double)local.LongitudeAsDecimal.Value) - longitude) * auxPi)))) / 360
                         });
            }
            else
            {
                query = (from evento in dbSite.Evento.Include(t => t.Apresentacao)
                         join local in dbSite.Local on evento.LocalID equals local.IR_LocalID
                         join subtipo in dbSite.EventoSubtipo on evento.SubtipoID equals subtipo.IR_SubtipoID
                         join tipo in dbSite.Tipo on subtipo.TipoID equals tipo.IR_TipoID
                         where evento.Apresentacao.Count > 0 && local != null && subtipo != null && tipo != null
                         select new EventoModelQuery()
                         {
                             evento = evento,
                             local = local,
                             tipo = tipo,
                             subtipo = subtipo,
                             apresentacoes = evento.Apresentacao.OrderBy(t => t.CalcHorario).Select(apresentacao => new ApresentacaoModelQuery()
                             {
                                 apresentacao = apresentacao,
                                 qtdeDisponivel = apresentacao.Setor.Sum(t => t.QtdeDisponivel.Value),
                                 menorPreco = dbSite.Preco.Where(p => p.ApresentacaoID.Value == apresentacao.IR_ApresentacaoID).OrderBy(t => t.Valor).FirstOrDefault(),
                                 maiorPreco = dbSite.Preco.Where(p => p.ApresentacaoID.Value == apresentacao.IR_ApresentacaoID).OrderByDescending(t => t.Valor).FirstOrDefault(),
                             }).ToList(),
                             distancia = null
                         });
            }

            return query;
        }

        private List<Evento> ListarQuery(string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, int quantidadeDias = 0, List<int> diasSemana = null, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc)
        {
            List<string> whereFilter = new List<string>();
            string distanciaCalculo = "(40030 * ((180 / @pi) * ACOS(COS((90 - l.LatitudeAsDecimal) * @auxPi) * COS((90 - @latitude) * @auxPi) + SIN((90 - l.LatitudeAsDecimal) * @auxPi) * SIN((90 - @latitude) * @auxPi) * COS((l.LongitudeAsDecimal - @longitude) * @auxPi)))) / 360";

            double pi = System.Math.PI;
            double auxPi = pi / 180;

            distanciaCalculo = distanciaCalculo
                .Replace("@pi", pi.ToString().Replace(",", "."))
                .Replace("@auxPi", auxPi.ToString().Replace(",", "."))
                .Replace("@latitude", latitude.ToString().Replace(",", "."))
                .Replace("@longitude", longitude.ToString().Replace(",", "."));

            if (subtipoID > 0)
            {
                //Filtra pelo subtipo de evento
                whereFilter.Add("e.SubtipoID = " + subtipoID);
            }
            else if (tipoID > 0)
            {
                //Filtra pelo tipo de evento
                whereFilter.Add("st.TipoID = " + tipoID);
            }


            if (localID > 0)
            {
                //Filtra por Local
                whereFilter.Add("e.LocalID = " + localID);

            }
            else if (!string.IsNullOrEmpty(cidade))
            {
                //Filtra pela cidade
                whereFilter.Add("l.Cidade = " + cidade);
            }
            else if (!string.IsNullOrEmpty(estado))
            {
                //Filtra por estado
                whereFilter.Add("l.Cidade = " + estado);
            }

            if (!string.IsNullOrEmpty(busca))
            {
                foreach (var search in busca.Split(','))
                {
                    whereFilter.Add("(((e.Nome + l.Nome + l.Cidade + l.Estado + t.Nome + st.descricao) COLLATE SQL_Latin1_General_CP1_CI_AI) LIKE '%" + search + "%')");
                }
            }


            if (quantidadeDias > 0)
            {
                //Filtra o periodo
                var dateQuantidadeDias = DateTime.Now.AddDays(quantidadeDias);
                whereFilter.Add("a.CalcHorario <= " + dateQuantidadeDias.ToString("yyyy/MM/dd"));
            }

            if ((diasSemana != null))
            {
                //Filtra os dias da Semana
                whereFilter.Add("a.CalcDiaDaSemana in (" + String.Join(",", diasSemana) + ")");
            }

            if (latitude != null && longitude != null)
            {
                whereFilter.Add("(" + distanciaCalculo + ") <= " + distancia);
            }


            var queryStr = @"SELECT 
                         e.ID, e.IR_EventoID, e.Nome, e.LocalID, e.Release, e.Imagem, e.RetiradaBilheteria, e.DisponivelAvulso, e.PublicarSemVendaMotivo, e.Publicar, e.SubtipoID, e.DataAberturaVenda, e.ExibeQuantidade, e.FilmeID, e.PermiteVendaPacote, e.PossuiTaxaProcessamento,
                         l.ID, l.IR_LocalID, l.Nome, l.Cidade, l.Estado, l.Endereco, l.CEP, l.DDDTelefone, l.Telefone, l.EmpresaID, l.Pais, l.Imagem, l.Latitude, l.Longitude, l.LongitudeAsDecimal, l.LatitudeAsDecimal,"
                         + (
                         (latitude != null && longitude != null) ?
                         (distanciaCalculo + " as Distancia,") :
                         ""
                         ) +
                        @"st.ID, st.IR_SubtipoID, st.TipoID, st.Descricao,
                         t.ID, t.IR_TipoID, t.Nome, 
                         a.ID, a.IR_ApresentacaoID, a.Horario, a.EventoID, a.UsarEsquematico, a.Programacao, a.CodigoProgramacao, a.CalcDiaDaSemana, a.CalcHorario, (SELECT SUM(QtdeDisponivel) FROM Setor (nolock) WHERE ApresentacaoID = a.IR_ApresentacaoID) as QtdeDisponivel,
                         pmenor.ID, pmenor.IR_PrecoID, pmenor.Nome, pmenor.Valor, pmenor.ApresentacaoID, pmenor.SetorID, pmenor.QuantidadePorCliente, pmenor.Pacote, pmenor.Serie, pmenor.CodigoCinema, 
                         pmaior.ID, pmaior.IR_PrecoID, pmaior.Nome, pmaior.Valor, pmaior.ApresentacaoID, pmaior.SetorID, pmaior.QuantidadePorCliente, pmaior.Pacote, pmaior.Serie, pmaior.CodigoCinema
                        FROM Evento (nolock) e
                    INNER JOIN Local (nolock) l ON e.LocalID = l.IR_LocalID
                    INNER JOIN EventoSubtipo (nolock) st ON st.IR_SubtipoID = e.SubtipoID
                    INNER JOIN Tipo (nolock) t ON t.IR_TipoID = st.TipoID
                    INNER JOIN Apresentacao (nolock) a ON a.EventoID = e.IR_EventoID
                    LEFT JOIN Preco (nolock) pmenor ON pmenor.ID = (SELECT max(ID) FROM Preco (nolock) p1 WHERE p1.ApresentacaoID = a.IR_ApresentacaoID and p1.Valor = (SELECT min(Valor) FROM Preco (nolock) p2 INNER JOIN Setor s2 ON p2.SetorID = s2.IR_SetorID AND p2.ApresentacaoID = s2.ApresentacaoID AND s2.PrincipalPrecoID = p2.IR_PrecoID WHERE p2.ApresentacaoID = a.IR_ApresentacaoID))
                    LEFT JOIN Preco (nolock) pmaior ON pmaior.ID = (SELECT max(ID) FROM Preco (nolock) p1 WHERE p1.ApresentacaoID = a.IR_ApresentacaoID and p1.Valor = (SELECT max(Valor) FROM Preco (nolock) p2 INNER JOIN Setor s2 ON p2.SetorID = s2.IR_SetorID AND p2.ApresentacaoID = s2.ApresentacaoID AND s2.PrincipalPrecoID = p2.IR_PrecoID WHERE p2.ApresentacaoID = a.IR_ApresentacaoID))"
                    + ((whereFilter.Count > 0) ? (" WHERE " + String.Join(" and ", whereFilter)) : "")
                    + ((ordem == enumEventoOrdem.dataAsc) ? " ORDER BY a.CalcHorario;" : " ORDER BY Distancia;");

            result = new List<Evento>();
            var query = conSite.Query<Evento, Local, EventoSubtipo, Tipo, Apresentacao, Preco, Preco, int>(queryStr, addResult, new
            {
                auxPi = auxPi,
                latitude = latitude,
                longitude = longitude
            });

            return result;
        }

        private PagedModel<Evento> ListarCompletoQueryPaged(int pageNumber = 0, int pageSize = 0, string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, int quantidadeDias = 0, List<int> diasSemana = null, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc, List<int> eventoIDs = null)
        {
            List<string> whereFilter = new List<string>();
            Dictionary<string, string> whereJoin = new Dictionary<string, string>();

            string joinLocal = " INNER JOIN Local (nolock) l ON e.LocalID = l.IR_LocalID";
            string joinSubtipo = " INNER JOIN EventoSubtipo (nolock) st ON st.IR_SubtipoID = e.SubtipoID";
            string joinTipo = " INNER JOIN Tipo (nolock) t ON t.IR_TipoID = st.TipoID";
            string joinApresentacao = " INNER JOIN Apresentacao (nolock) a ON a.EventoID = e.IR_EventoID";

            string distanciaCalculo = "(40030 * ((180 / @pi) * ACOS(COS((90 - l.LatitudeAsDecimal) * @auxPi) * COS((90 - @latitude) * @auxPi) + SIN((90 - l.LatitudeAsDecimal) * @auxPi) * SIN((90 - @latitude) * @auxPi) * COS((l.LongitudeAsDecimal - @longitude) * @auxPi)))) / 360";

            double pi = System.Math.PI;
            double auxPi = pi / 180;

            distanciaCalculo = distanciaCalculo
                .Replace("@pi", pi.ToString().Replace(",", "."))
                .Replace("@auxPi", auxPi.ToString().Replace(",", "."))
                .Replace("@latitude", latitude.ToString().Replace(",", "."))
                .Replace("@longitude", longitude.ToString().Replace(",", "."));

            result = new List<Evento>();

            if (eventoIDs != null && eventoIDs.Count > 0)
            {
                //Filtra pelos IDs
                whereFilter.Add("e.ID in (" + string.Join(", ", eventoIDs) + ")");
            }
            if (subtipoID > 0)
            {
                //Filtra pelo subtipo de evento
                whereFilter.Add("e.SubtipoID = " + subtipoID);
            }
            else if (tipoID > 0)
            {
                //Filtra pelo tipo de evento
                whereFilter.Add("st.TipoID = " + tipoID);
                whereJoin["st"] = joinSubtipo;
            }

            whereJoin.Select(t => t.Value).ToArray();


            if (localID > 0)
            {
                //Filtra por Local
                whereFilter.Add("e.LocalID = " + localID);

            }
            else if (!string.IsNullOrEmpty(cidade))
            {
                //Filtra pela cidade
                whereFilter.Add("l.Cidade = " + cidade);
                whereJoin["l"] = joinLocal;
            }
            else if (!string.IsNullOrEmpty(estado))
            {
                //Filtra por estado
                whereFilter.Add("l.Cidade = " + estado);
                whereJoin["l"] = joinLocal;
            }

            if (!string.IsNullOrEmpty(busca))
            {
                foreach (var search in busca.Split(','))
                {
                    whereFilter.Add("(((e.Nome + l.Nome + l.Cidade + l.Estado + t.Nome + st.descricao) COLLATE SQL_Latin1_General_CP1_CI_AI) LIKE '%" + search + "%')");
                }
                whereJoin["st"] = joinSubtipo;
                whereJoin["l"] = joinLocal;
                whereJoin["t"] = joinTipo;
            }


            if (quantidadeDias > 0)
            {
                //Filtra o periodo
                var dateQuantidadeDias = DateTime.Now.AddDays(quantidadeDias);
                whereFilter.Add("a.CalcHorario <= " + dateQuantidadeDias.ToString("yyyy/MM/dd"));
                whereJoin["a"] = joinApresentacao;
            }

            if ((diasSemana != null))
            {
                //Filtra os dias da Semana
                whereFilter.Add("a.CalcDiaDaSemana in (" + String.Join(",", diasSemana) + ")");
                whereJoin["a"] = joinApresentacao;
            }

            if (latitude != null && longitude != null && distancia != null)
            {
                whereFilter.Add("(" + distanciaCalculo + ") <= " + distancia);
                whereJoin["l"] = joinLocal;
            }
            if (ordem == enumEventoOrdem.distanciaAsc)
            {
                whereJoin["l"] = joinLocal;
            }
            else
            {
                whereJoin["a"] = joinApresentacao;
            }

            int startRow = ((pageNumber - 1) * pageSize);
            int endRow = (pageNumber * pageSize);

            string subquery = @"
                    SELECT TOP @endRow e.ID, ROW_NUMBER() OVER (ORDER BY min(" + ((ordem == enumEventoOrdem.dataAsc) ? "a.CalcHorario" : distanciaCalculo) + @")) AS RowNum
		            FROM Evento (nolock) e" +
                    string.Join(" ", whereJoin.Select(t => t.Value).ToArray())
                 + ((whereFilter.Count > 0) ? (" WHERE " + String.Join(" and ", whereFilter)) : "")
                 + " GROUP BY e.ID";

            int size = Convert.ToInt32(conSite.ExecuteScalar("SELECT count(distinct e.ID) FROM Evento (nolock) e" +
                            string.Join(" ", whereJoin.Select(t => t.Value).ToArray()) +
                            ((whereFilter.Count > 0) ? (" WHERE " + String.Join(" and ", whereFilter)) : "")
                       ));

            var queryStr = @"SELECT 
              e.ID, e.IR_EventoID, e.Nome, e.LocalID, e.Release, e.Imagem, e.RetiradaBilheteria, e.DisponivelAvulso, e.PublicarSemVendaMotivo, e.Publicar, e.SubtipoID, e.DataAberturaVenda, e.ExibeQuantidade, e.FilmeID, e.PermiteVendaPacote, e.PossuiTaxaProcessamento,
              l.ID, l.IR_LocalID, l.Nome, l.Cidade, l.Estado, l.Endereco, l.CEP, l.DDDTelefone, l.Telefone, l.EmpresaID, l.Pais, l.Imagem, l.Latitude, l.Longitude, l.LongitudeAsDecimal, l.LatitudeAsDecimal,"
                + (
                (latitude != null && longitude != null) ?
                (distanciaCalculo + " as Distancia,") :
                ""
              ) + @"
              st.ID, st.IR_SubtipoID, st.TipoID, st.Descricao,
              t.ID, t.IR_TipoID, t.Nome, 
              a.ID, a.IR_ApresentacaoID, a.Horario, a.EventoID, a.UsarEsquematico, a.Programacao, a.CodigoProgramacao, a.CalcDiaDaSemana, a.CalcHorario, (SELECT SUM(QtdeDisponivel) FROM Setor (nolock) WHERE ApresentacaoID = a.IR_ApresentacaoID) as QtdeDisponivel,
              pmenor.ID, pmenor.IR_PrecoID, pmenor.Nome, pmenor.Valor, pmenor.ApresentacaoID, pmenor.SetorID, pmenor.QuantidadePorCliente, pmenor.Pacote, pmenor.Serie, pmenor.CodigoCinema, 
              pmaior.ID, pmaior.IR_PrecoID, pmaior.Nome, pmaior.Valor, pmaior.ApresentacaoID, pmaior.SetorID, pmaior.QuantidadePorCliente, pmaior.Pacote, pmaior.Serie, pmaior.CodigoCinema
              FROM (" + subquery + @") busca
            INNER JOIN Evento (nolock) e ON e.ID = busca.ID" +
            joinLocal + joinSubtipo + joinTipo + joinApresentacao + @"
            LEFT JOIN Preco (nolock) pmenor ON pmenor.ID = (SELECT max(ID) FROM Preco (nolock) p1 WHERE p1.ApresentacaoID = a.IR_ApresentacaoID and p1.Valor = (SELECT min(Valor) FROM Preco (nolock) p2 INNER JOIN Setor s2 ON p2.SetorID = s2.IR_SetorID AND p2.ApresentacaoID = s2.ApresentacaoID AND s2.PrincipalPrecoID = p2.IR_PrecoID WHERE p2.ApresentacaoID = a.IR_ApresentacaoID))
            LEFT JOIN Preco (nolock) pmaior ON pmaior.ID = (SELECT max(ID) FROM Preco (nolock) p1 WHERE p1.ApresentacaoID = a.IR_ApresentacaoID and p1.Valor = (SELECT max(Valor) FROM Preco (nolock) p2 INNER JOIN Setor s2 ON p2.SetorID = s2.IR_SetorID AND p2.ApresentacaoID = s2.ApresentacaoID AND s2.PrincipalPrecoID = p2.IR_PrecoID WHERE p2.ApresentacaoID = a.IR_ApresentacaoID))
            WHERE busca.RowNum > @startRow
            ORDER BY busca.RowNum, a.CalcHorario";

            queryStr = queryStr.Replace("@startRow", startRow.ToString())
                .Replace("@endRow", endRow.ToString());

            var query = conSite.Query<Evento, Local, EventoSubtipo, Tipo, Apresentacao, Preco, Preco, int>(queryStr, addResult, new
            {
                startRow = startRow,
                endRow = endRow,
                pi = pi,
                auxPi = auxPi,
                latitude = latitude,
                longitude = longitude
            });

            return new PagedModel<Evento>(result, size);
        }

        private PagedModel<Evento> ListarQueryPaged(int pageNumber, int pageSize, string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, int quantidadeDias = 0, List<int> diasSemana = null, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc)
        {
            var distanciaCalculo = "(40030 * ((180 / @pi) * ACOS(COS((90 - l.LatitudeAsDecimal) * @auxPi) * COS((90 - @latitude) * @auxPi) + SIN((90 - l.LatitudeAsDecimal) * @auxPi) * SIN((90 - @latitude) * @auxPi) * COS((l.LongitudeAsDecimal - @longitude) * @auxPi)))) / 360";

            distanciaCalculo = distanciaCalculo
                .Replace("@pi", Math.PI.ToString(CultureInfo.InvariantCulture).Replace(",", "."))
                .Replace("@auxPi", (Math.PI / 180).ToString(CultureInfo.InvariantCulture).Replace(",", "."))
                .Replace("@latitude", latitude.ToString().Replace(",", "."))
                .Replace("@longitude", longitude.ToString().Replace(",", "."));

            result = new List<Evento>();

            var startRow = ((pageNumber - 1) * pageSize);
            var endRow = (pageNumber * pageSize);
            var orderby = (ordem == enumEventoOrdem.dataAsc) ? "a.CalcHorario" : distanciaCalculo;

            var sqlCount =
@"SELECT COUNT(DISTINCT e.ID)
FROM tEvento(nolock) AS e
     INNER JOIN tEventoSubtipo(nolock) AS st ON st.ID = e.EventoSubtipoID
     INNER JOIN tLocal(nolock) AS l ON e.LocalID = l.ID
     INNER JOIN tEventoTipo(nolock) AS t ON t.ID = st.EventoTipoID
     INNER JOIN tApresentacao(nolock) AS a ON a.EventoID = e.ID AND a.DisponivelVenda = 'T'
     INNER JOIN tApresentacaoSetor AS aps(NOLOCK) ON aps.ApresentacaoID = a.ID
     INNER JOIN tPreco AS p(NOLOCK) ON p.ApresentacaoSetorID = aps.ID
     INNER JOIN tCanalEvento AS ce(NOLOCK) ON ce.EventoID = e.ID AND ce.canalId = @canalId
     INNER JOIN tCanal AS c(NOLOCK) ON c.ID = ce.CanalID
WHERE a.Horario > dbo.GetDateString(GETDATE())
AND (p.Valor > 0
     OR (p.Valor = 0
         AND e.DisponivelCortesiaInternet = 'T'))
AND e.Publicar IN('T', 'S')
AND (@subtipoId = 0
     OR e.EventoSubtipoID = @subtipoId)
AND (@tipoId = 0
     OR st.EventoTipoID = @tipoId)
AND (@localId = 0
     OR e.LocalID = @localId)
AND (@cidade IS NULL
     OR l.Cidade = @cidade)
AND (@uf IS NULL
     OR l.Estado = @uf)
AND (@horario = ''
     OR a.CalcHorario <= @horario)
AND (@diaSemana IS NULL OR a.CalcDiaDaSemana IN @diaSemana)
AND (@busca IS NULL
     OR (((e.palavraChave + ' ' + e.Nome + ' ' + l.Nome + ' ' + l.Cidade + ' ' + l.Estado + ' ' + t.Nome + ' ' + st.descricao) COLLATE SQL_Latin1_General_CP1_CI_AI) LIKE @busca))
AND ((@latitude IS NULL
      OR @longitude IS NULL
      OR @distancia = 0)
     OR ((40030 * ((180 / @pi) * ACOS(COS((90 - l.LatitudeAsDecimal) * @auxPi) * COS((90 - @latitude) * @auxPi) + SIN((90 - l.LatitudeAsDecimal) * @auxPi) * SIN((90 - @latitude) * @auxPi) * COS((l.LongitudeAsDecimal - @longitude) * @auxPi)))) / 360) <= @distancia);";

            var size = conIngresso.Query<int>(sqlCount, new
            {
                pi = Math.PI,
                auxpi = (Math.PI / 180),
                latitude = latitude,
                longitude = longitude,
                distancia = distancia,
                subtipoId = subtipoID,
                localId = localID,
                tipoId = tipoID,
                diaSemana = diasSemana ?? new List<int>(),
                busca = "%" + busca + "%",
                cidade = cidade,
                uf = estado,
                horario = quantidadeDias > 0 ? DateTime.Now.AddDays(quantidadeDias).ToString("yyyy/MM/dd") : "",
                canalId = this.CanalId
            });

            var sqlBusca =
@"SELECT e.ID,
       e.Id AS                                                                                                                                                                                                                                            IR_EventoID,
       e.Nome,
       e.LocalID,
       e.Release,
       e.ImagemInternet AS                                                                                                                                                                                                                                Imagem,
       IIF(e.RetiradaBilheteria = 'T', 1, 0) AS RetiradaBilheteria,
       1 AS                                                                                                                                                                                                                                               ASDisponivelAvulso,
       e.PublicarSemVendaMotivo,
       e.Publicar,
       e.eventoSubtipoId,
       e.DataAberturaVenda,
       IIF(e.ExibeQuantidade = 'T', 1, 0) AS ExibeQuantidade,
       1 AS DisponivelAvulso,
       e.FilmeID,
       IIF(e.PermiteVendaPacote = 'T', 1, 0) AS PermiteVendaPacote,
       IIF((ce.TaxaConveniencia = 0
            AND uf.PossuiTaxaProcessamento = 'T')
           OR uf.PossuiTaxaProcessamento = 'F', 0, 1) AS                                                                                                                                                                                                  PossuiTaxaProcessamento,
       ( SELECT MIN(p.Valor)
         FROM tApresentacao AS a(NOLOCK)
              INNER JOIN tApresentacaoSetor AS aps(NOLOCK) ON aps.apresentacaoId = a.Id
              INNER JOIN tPreco AS p(NOLOCK) ON p.apresentacaoSetorId = aps.Id
              INNER JOIN tCanalPreco AS cp ON cp.precoId = p.Id AND cp.canalId = @canalId
         WHERE a.eventoId = e.Id
               AND (p.Valor > 0 OR (p.Valor = 0
                        AND e.disponivelCortesiaInternet = 'T')) ) AS MenorPrecoEvento,
       l.ID,
       l.Id AS                                                                                                                                                                                                                                            IR_LocalID,
       l.Nome,
       l.Cidade,
       l.Estado,
       ISNULL(l.Logradouro, N'')+ISNULL(', '+CAST(l.Numero AS NVARCHAR(50)), N'')+ISNULL(' - '+l.Bairro, N'') AS Endereco,
       l.CEP,
       l.DDDTelefone,
       l.Telefone,
       l.EmpresaID,
       pais.Nome AS                                                                                                                                                                                                                                       Pais,
       l.ImagemInternet AS                                                                                                                                                                                                                                Imagem,
       l.Latitude,
       l.Longitude,
       l.LongitudeAsDecimal,
       l.LatitudeAsDecimal,
       (40030 * ((180 / @pi) * ACOS(COS((90 - l.LatitudeAsDecimal) * @auxPi) * COS((90 - @latitude) * @auxPi) + SIN((90 - l.LatitudeAsDecimal) * @auxPi) * SIN((90 - @latitude) * @auxPi) * COS((l.LongitudeAsDecimal - @longitude) * @auxPi)))) / 360 AS Distancia,
       st.ID,
       st.Id AS                                                                                                                                                                                                                                           IR_SubtipoID,
       st.eventoTipoId,
       st.Descricao,
       t.ID,
       t.Id AS                                                                                                                                                                                                                                            IR_TipoID,
       t.Nome
FROM
     ( SELECT TOP {=rows} e.ID,
                     ROW_NUMBER() OVER(ORDER BY MIN({{orderby}})) AS RowNum
       FROM tEvento(NOLOCK) AS e
            INNER JOIN tCanalEvento AS ce WITH (NOLOCK) ON ce.eventoId = e.Id AND ce.canalId = @canalId
            LEFT JOIN tEventoSubtipo(NOLOCK) AS st ON st.Id = e.eventoSubtipoId
            LEFT JOIN tLocal(NOLOCK) AS l ON e.localId = l.Id
            LEFT JOIN tEventoTipo(NOLOCK) AS t ON t.Id = st.eventoTipoId
            INNER JOIN tApresentacao(NOLOCK) AS a ON a.eventoId = e.Id AND a.DisponivelVenda = 'T'
       WHERE a.Horario > dbo.GetDateString(GETDATE())
AND (@subtipoId = 0
     OR e.eventoSubtipoId = @subtipoId)
AND (@tipoId = 0
     OR st.eventoTipoId = @tipoId)
AND (@localId = 0
     OR e.localId = @localId)
AND (@cidade IS NULL
     OR l.cidade = @cidade)
AND (@uf IS NULL
     OR l.estado = @uf)
AND (@horario = ''
     OR a.calcHorario <= @horario)
AND (@diaSemana IS NULL OR a.CalcDiaDaSemana IN @diaSemana)
AND (@busca IS NULL
     OR (((e.palavraChave + ' ' + e.Nome + ' ' + l.Nome + ' ' + l.Cidade + ' ' + l.Estado + ' ' + t.Nome + ' ' + st.descricao) COLLATE SQL_Latin1_General_CP1_CI_AI) LIKE @busca))
AND ((@latitude IS NULL
      OR @longitude IS NULL
      OR @distancia = 0)
     OR ((40030 * ((180 / @pi) * ACOS(COS((90 - l.LatitudeAsDecimal) * @auxPi) * COS((90 - @latitude) * @auxPi) + SIN((90 - l.LatitudeAsDecimal) * @auxPi) * SIN((90 - @latitude) * @auxPi) * COS((l.LongitudeAsDecimal - @longitude) * @auxPi)))) / 360) <= @distancia)
       GROUP BY e.Id ) AS busca
     INNER JOIN tEvento(NOLOCK) AS e ON e.Id = busca.Id
     INNER JOIN tCanalEvento AS ce (NOLOCK) ON ce.eventoId = e.Id AND ce.canalId = @canalId
     INNER JOIN tLocal(NOLOCK) AS l ON e.localId = l.Id
     INNER JOIN tEventoSubtipo(NOLOCK) AS st ON st.Id = e.eventoSubtipoId
     INNER JOIN tEventoTipo(NOLOCK) AS t ON t.Id = st.eventoTipoId
     LEFT OUTER JOIN tEstado AS uf(NOLOCK) ON uf.Sigla COLLATE Latin1_General_CI_AI = l.Estado
     INNER JOIN tPais AS pais(NOLOCK) ON pais.Id = l.paisId
WHERE busca.RowNum > {=startRow}
ORDER BY busca.RowNum;";

            sqlBusca = sqlBusca.Replace("{{orderby}}", orderby);

            var query = conIngresso.Query<Evento, Local, EventoSubtipo, Tipo, Evento>(sqlBusca, (evento, local, subtipo, tipo) =>
            {
                evento.Local = local;
                evento.Subtipo = subtipo;
                evento.Tipo = tipo;
                return evento;
            }, new
            {
                pi = Math.PI,
                auxpi = (Math.PI / 180),
                latitude = latitude,
                longitude = longitude,
                distancia = distancia,
                subtipoId = subtipoID,
                localId = localID,
                tipoId = tipoID,
                diaSemana = diasSemana ?? new List<int>(),
                busca = "%" + busca + "%",
                cidade = cidade,
                uf = estado,
                horario = quantidadeDias > 0 ? DateTime.Now.AddDays(quantidadeDias).ToString("yyyy/MM/dd") : "",
                rows = endRow,
                startRow = startRow,
                canalId = this.CanalId
            });

            return new PagedModel<Evento>(query.ToList(), size.Single());
        }

        public List<Evento> ListarOSESP()
        {
            List<string> whereFilter = new List<string>();
            Dictionary<string, string> whereJoin = new Dictionary<string, string>();

            result = new List<Evento>();

            var queryStr = @"SELECT 
                                  e.ID
                                , e.IR_EventoID
                                , e.Nome
                                , e.LocalID
                                , e.Release
                                , e.Imagem
                                , e.RetiradaBilheteria
                                , e.DisponivelAvulso
                                , e.PublicarSemVendaMotivo
                                , e.Publicar
                                , e.SubtipoID
                                , e.DataAberturaVenda
                                , e.ExibeQuantidade
                                , e.FilmeID
                                , e.PermiteVendaPacote
                                , e.PossuiTaxaProcessamento
                                , (SELECT 
                                    MIN(p.Valor)
                                    FROM Apresentacao AS a(NOLOCK) 
                                        INNER JOIN Preco AS p(NOLOCK) ON p.ApresentacaoId = a.IR_ApresentacaoId 
                                    WHERE a.EventoId = e.IR_EventoId
                                ) AS MenorPrecoEvento
                                , l.ID
                                , l.IR_LocalID
                                , l.Nome
                                , l.Cidade
                                , l.Estado
                                , l.Endereco
                                , l.CEP
                                , l.DDDTelefone
                                , l.Telefone
                                , l.EmpresaID
                                , l.Pais
                                , l.Imagem
                                , l.Latitude
                                , l.Longitude
                                , l.LongitudeAsDecimal
                                , l.LatitudeAsDecimal
                                , st.ID
                                , st.IR_SubtipoID
                                , st.TipoID
                                , st.Descricao
                                , t.ID
                                , t.Nome
                                , t.IR_TipoID
                                FROM Evento e WITH (NOLOCK)
                                    INNER JOIN Local (NOLOCK) l ON e.LocalID = l.IR_LocalID
                                    INNER JOIN EventoSubtipo (NOLOCK) st ON e.SubtipoID = st.IR_SubtipoID 
                                    INNER JOIN Tipo (NOLOCK) t ON st.TipoID = t.IR_TipoID
                                WHERE (e.IR_EventoID IN (SELECT DISTINCT ID FROM API_Osesp_Eventos WITH (NOLOCK)))";

            var query = conSite.Query<Evento, Local, EventoSubtipo, Tipo, Evento>(queryStr, (evento, local, subtipo, tipo) =>
            {
                evento.Local = local;
                evento.Subtipo = subtipo;
                evento.Tipo = tipo;
                return evento;
            });

            return query.ToList();
        }

        public IPagedList<Evento> ListarCompleto(int pageNumber, int pageSize, string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, int quantidadeDias = 0, List<int> diasSemana = null, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc, List<int> eventoIDs = null)
        {
            PagedModel<Evento> result = ListarCompletoQueryPaged(pageNumber, pageSize, busca, localID, estado, cidade, tipoID, subtipoID, quantidadeDias, diasSemana, latitude, longitude, distancia, ordem, eventoIDs);
            return result.List.ToPagedList(pageNumber, pageSize, result.Count);
        }

        public IPagedList<Evento> Listar(int pageNumber, int pageSize, string busca = null, int localID = 0, string estado = null, string cidade = null, int tipoID = 0, int subtipoID = 0, int quantidadeDias = 0, List<int> diasSemana = null, double? latitude = null, double? longitude = null, double distancia = 0, enumEventoOrdem ordem = enumEventoOrdem.dataAsc)
        {
            var result = ListarQueryPaged(pageNumber, pageSize, busca, localID, estado, cidade, tipoID, subtipoID, quantidadeDias, diasSemana, latitude, longitude, distancia, ordem);
            return result.List.ToPagedList(pageNumber, pageSize, result.Count);
        }

        public List<KeyValuePair<DateTime, int>> ListarDatas(DateTime dataInicial, int numeroDias)
        {
            return (from apr in dbSite.Apresentacao
                    join local in dbSite.Local on apr.Evento.LocalID equals local.IR_LocalID
                    join subtipo in dbSite.EventoSubtipo on apr.Evento.SubtipoID equals subtipo.IR_SubtipoID
                    join tipo in dbSite.Tipo on subtipo.TipoID equals tipo.IR_TipoID
                    where local != null && subtipo != null && tipo != null
                    select apr)
                .Where(apr => apr.CalcHorario != null &&
                    apr.CalcHorario >= dataInicial)
                .GroupBy(apr => DbFunctions.TruncateTime(apr.CalcHorario).Value)
                .OrderBy(x => x.Key)
                .Take(numeroDias)
                .ToList()
                .Select(t => new KeyValuePair<DateTime, int>(t.Key, (from y in t group y by y.EventoID into n select n).Count())).ToList();
        }

        /// <summary>
        /// Método que retorna os Eventos de uma determinada data
        /// </summary>
        /// <param name="data">Data usada na busca</param>
        /// <param name="pagina">Numero da página que está sendo carregada</param>
        /// <param name="numeroItens">Número de itens a serem carregados</param>
        /// <returns></returns>
        public IPagedList<Evento> Listar(DateTime data, int pagina, int numeroItens)
        {
            return Listar(data).ToPagedList(pagina, numeroItens).SelectPagedList(t => t.toEvento());
        }

        private IQueryable<EventoModelQuery> Listar(DateTime data)
        {

            return (from evento in dbSite.Evento.Include(t => t.Apresentacao)
                    join local in dbSite.Local on evento.LocalID equals local.IR_LocalID
                    join subtipo in dbSite.EventoSubtipo on evento.SubtipoID equals subtipo.IR_SubtipoID
                    join tipo in dbSite.Tipo on subtipo.TipoID equals tipo.IR_TipoID
                    where evento.Apresentacao.Count(x => DbFunctions.TruncateTime(x.CalcHorario).Value == data) > 0 && local != null && subtipo != null && tipo != null
                    select new EventoModelQuery()
                    {
                        evento = evento,
                        local = local,
                        tipo = tipo,
                        subtipo = subtipo,
                        apresentacoes = evento.Apresentacao.OrderBy(t => t.CalcHorario).Select(apresentacao => new ApresentacaoModelQuery()
                        {
                            apresentacao = apresentacao,
                            qtdeDisponivel = apresentacao.Setor.Sum(t => t.QtdeDisponivel.Value),
                            menorPreco = dbSite.Preco.Where(p => p.ApresentacaoID.Value == apresentacao.IR_ApresentacaoID).OrderBy(t => t.Valor).FirstOrDefault(),
                            maiorPreco = dbSite.Preco.Where(p => p.ApresentacaoID.Value == apresentacao.IR_ApresentacaoID).OrderByDescending(t => t.Valor).FirstOrDefault(),
                        }).ToList()
                    }).OrderBy(t => t.evento.ID);
        }

        #region Métodos Tabela tEventos usada no admin

        /// <summary>
        /// Método que retorna um IQueryable com os Eventos da tabela tEvento
        /// </summary>
        /// <param name="busca">Texto usado na busca pelo nome do evento</param>
        /// <returns></returns>
        private IQueryable<tEvento> ListarAdminQuery(string busca, enumAdminFiltroEvento filtro)
        {
            IQueryable<tEvento> query = dbIngresso.tEvento.Include(t => t.tLocal).Include(t => t.tApresentacao).Include(t => t.EventoMidia);

            //Filtro de busca superior
            if (!string.IsNullOrEmpty(busca))
            {
                busca = busca.ToLower().RemoveAcentos();
                query = query.Where(item => item.Nome.ToLower().Contains(busca) || (item.tLocal != null && (item.tLocal.Nome.ToLower().Contains(busca)) || item.tLocal.Cidade.ToLower().Contains(busca)) || item.tApresentacao.Any(x => (x.Horario.Substring(6, 2) + "/" + x.Horario.Substring(4, 2)) == busca) || item.tApresentacao.Any(x => x.DisponivelVenda == "T"));
            }

            switch (filtro)
            {
                case enumAdminFiltroEvento.statusPublicado:
                    query = query.Where(t => t.Publicar == "T");
                    break;
                case enumAdminFiltroEvento.statusNãoPublicado:
                    query = query.Where(t => t.Publicar == "F");
                    break;
                case enumAdminFiltroEvento.statusPublicadoSemVenda:
                    query = query.Where(t => t.Publicar == "S");
                    break;
                case enumAdminFiltroEvento.incompletoMobile:
                    query = query.Where(t => t.EventoMidia.Count() < 2);
                    break;
                case enumAdminFiltroEvento.incompletoSite:
                    query = query.Where(t => t.ImagemInternet == null || t.ImagemInternet.Length == 0);
                    break;
            }
            return query.OrderBy(t => t.Nome);
        }

        public IPagedList<tEvento> ListarAdmin(int pageNumber, int pageSize, string busca, enumAdminFiltroEvento filtro)
        {
            return ListarAdminQuery(busca, filtro).ToPagedList(pageNumber, pageSize);
        }

        public tEvento ConsultarAdmin(int tEventoId)
        {
            return (from item in dbIngresso.tEvento.Include(t => t.tApresentacao)
                    where item.ID == tEventoId
                    select item).FirstOrDefault();
        }

        #endregion

        public List<Evento> ListarTodos()
        {
            return (from item in dbSite.Evento
                    orderby item.Nome
                    select item).ToList();
        }

        public List<Banner> ListarTodosBanners()
        {
            return (from item in dbSite.Banner
                    orderby item.Nome
                    select item).ToList();
        }

        public List<EventoMidia> ListarEventoMidiaInEvento(int eventoId)
        {
            var sql =
@"SELECT em.ID,
       em.EventoTipoMidiaID,
       em.Publicado,
       em.EventoID,
       em.Valor,
       etm.ID,
       etm.Chave,
       etm.Nome,
       etm.Instrucao,
       etm.Tipo
FROM EventoMidia(NOLOCK) AS em
     INNER JOIN EventoTipoMidia(NOLOCK) AS etm ON em.EventoTipoMidiaID = etm.ID
WHERE em.EventoID = @eventoId;";

            var query = conIngresso.Query<EventoMidia, EventoTipoMidia, EventoMidia>(sql,
                (eventoMidia, eventoTipoMidia) =>
                {
                    eventoMidia.EventoTipoMidia = eventoTipoMidia;
                    return eventoMidia;
                }, new { eventoId = eventoId });

            return query.ToList();
        }

        public EstatisticaIngressos ConsultarEstatisticaApresentacao(int eventoId, int apresentacaoId)
        {

            var strCon = ConfigurationManager.AppSettings["Conexao"];
            var builder = new SqlConnectionStringBuilder(strCon);
            var database = builder.InitialCatalog;

            var queryStr = string.Format(@"
                SELECT  {2}.dbo.GetQuantidadeIngressos({0},{1},null,null) as TotalIngressos, 
	                    ({2}.dbo.GetQuantidadeIngressos({0},{1},null,'V') + {2}.dbo.GetQuantidadeIngressos({0},{1},null,'I')) as Vendidos, 
		                {2}.dbo.GetQuantidadeIngressos({0},{1},null,'D') as Disponiveis,
		                {2}.dbo.GetQuantidadeIngressos({0},{1},null,'R') as Reservados", eventoId, apresentacaoId, database);

            var query = conIngresso.Query<EstatisticaIngressos>(queryStr);

            return query.FirstOrDefault();
        }

        public List<Evento> ListarDestaques(string tipoDestaque, int regiaoId, int canalId)
        {
            var queryStr =
@"SELECT e.ID,
       e.Id AS                                                                                                   IR_EventoID,
       e.Nome,
       e.LocalID,
       CAST(Resenha AS   VARCHAR(5000)) AS                                                                       Release,
       e.ImagemInternet AS                                                                                       Imagem,
       IIF(e.RetiradaBilheteria = 'T', 1, 0) AS                                                                  RetiradaBilheteria,
       1 AS                                                                                                      DisponivelAvulso,
       e.PublicarSemVendaMotivo,
       e.Publicar,
       e.eventoSubtipoID AS                                                                                      SubtipoID,
       e.DataAberturaVenda,
       IIF(e.ExibeQuantidade = 'T', 1, 0) AS                                                                     ExibeQuantidade,
       e.FilmeID,
       IIF(e.PermiteVendaPacote = 'T', 1, 0) AS                                                                  PermiteVendaPacote,
       IIF((ce.TaxaConveniencia = 0
            AND uf.PossuiTaxaProcessamento = 'T')
           OR uf.PossuiTaxaProcessamento = 'F', 0, 1) AS                                                         PossuiTaxaProcessamento,
       ( SELECT MIN(p.Valor)
         FROM tApresentacao AS a(NOLOCK)
              INNER JOIN tApresentacaoSetor AS aps(NOLOCK) ON aps.apresentacaoId = a.Id
              INNER JOIN tPreco AS p(NOLOCK) ON p.apresentacaoSetorId = aps.Id
              INNER JOIN tCanalPreco AS cp ON cp.precoId = p.Id AND cp.canalId = @canalId
         WHERE a.eventoId = e.Id
               AND (p.Valor > 0 OR (p.Valor = 0
                        AND e.disponivelCortesiaInternet = 'T')) ) AS                                            MenorPrecoEvento,
       de.Ordem,
       l.ID,
       l.Id AS                                                                                                   IR_LocalID,
       l.Nome,
       l.Cidade,
       l.Estado,
       ISNULL(l.Logradouro, N'')+ISNULL(', '+CAST(l.Numero AS NVARCHAR(50)), N'')+ISNULL(' - '+l.Bairro, N'') AS Endereco,
       l.CEP,
       l.DDDTelefone,
       l.Telefone,
       l.EmpresaID,
       pais.Nome AS                                                                                              Pais,
       l.ImagemInternet AS                                                                                       Imagem,
       l.Latitude,
       l.Longitude,
       l.LongitudeAsDecimal,
       l.LatitudeAsDecimal,
       st.ID,
       st.Id AS                                                                                                  IR_SubtipoID,
       st.eventoTipoID AS                                                                                        TipoID,
       st.Descricao,
       t.ID,
       t.Id AS                                                                                                   IR_TipoID,
       t.Nome
FROM tDestaqueRegiao(NOLOCK) AS de
     INNER JOIN tEvento(NOLOCK) AS e ON de.eventoId = e.Id
     INNER JOIN tCanalEvento AS ce WITH (NOLOCK) ON ce.eventoId = e.Id AND ce.canalId = @canalId
     INNER JOIN tLocal(NOLOCK) AS l ON e.localId = l.Id
     INNER JOIN tEventoSubtipo(NOLOCK) AS st ON st.Id = e.eventoSubtipoId
     INNER JOIN tEventoTipo(NOLOCK) AS t ON t.Id = st.eventoTipoId
     LEFT OUTER JOIN tEstado AS uf(NOLOCK) ON uf.Sigla COLLATE Latin1_General_CI_AI = l.Estado
     LEFT JOIN tPais AS pais(NOLOCK) ON pais.Id = l.paisId
WHERE de.tipo = @tipoDestaque
      AND regiaoId = @regiaoId
ORDER BY de.Ordem;";

            var query = conIngresso.Query<Evento, Local, EventoSubtipo, Tipo, Evento>(queryStr, (evento, local, subtipoEvento, tipoEvento) =>
            {
                evento.Local = local;
                evento.Subtipo = subtipoEvento;
                evento.Tipo = tipoEvento;
                return evento;
            }, new
            {
                tipoDestaque = tipoDestaque,
                regiaoId = regiaoId,
                canalId = canalId
            });

            return query.ToList();
        }

        public List<DefinirEventoApresentacaoModelQuery> DefinirEventoApresentacao(int eventoID)
        {
            string query = @"SELECT 
	                            tEvento.Nome AS Evento
	                            ,tLocal.Nome AS Local
	                            ,tEmpresa.Nome AS Empresa
	                            ,tRegional.Nome AS Regional
	                            ,tApresentacao.CalcHorario AS Apresentacao
                                ,tApresentacao.ID AS id
                            FROM 
	                            tEvento (NOLOCK) 
	                            LEFT JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID
	                            LEFT JOIN tEmpresa (NOLOCK) ON tLocal.EmpresaID = tEmpresa.ID
	                            LEFT JOIN tRegional (NOLOCK) ON tEmpresa.RegionalID = tRegional.ID
	                            LEFT JOIN tApresentacao (NOLOCK) ON tEvento.ID = tApresentacao.EventoID
                            WHERE 
	                            tEvento.ID = @eventoID AND tApresentacao.Cancelada = 'F'";
            return conIngresso.Query<DefinirEventoApresentacaoModelQuery>(query, new { eventoID = eventoID }).ToList();
        }

        public List<DefinirEventoApresentacaoModelQuery> ListarDisponiveisCancelMassa(int eventoID, int diasLimiteCancelamento)
        {
            string query = @"SELECT 
	                            tEvento.Nome AS Evento
	                            ,tLocal.Nome AS Local
	                            ,tEmpresa.Nome AS Empresa
	                            ,tRegional.Nome AS Regional
	                            ,tApresentacao.CalcHorario AS Apresentacao
                                ,tApresentacao.ID AS id
                            FROM 
	                            tEvento (NOLOCK) 
	                            LEFT JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID
	                            LEFT JOIN tEmpresa (NOLOCK) ON tLocal.EmpresaID = tEmpresa.ID
	                            LEFT JOIN tRegional (NOLOCK) ON tEmpresa.RegionalID = tRegional.ID
	                            LEFT JOIN tApresentacao (NOLOCK) ON tEvento.ID = tApresentacao.EventoID
                            WHERE 
	                            tEvento.ID = @eventoID 
                                AND tApresentacao.Cancelada = 'F'
                                AND DATEDIFF(DAY, tApresentacao.CalcHorario, GETDATE()) < @diasLimiteCancelamento
                            ORDER BY
                                tApresentacao.Horario asc
                                ";
            return conIngresso.Query<DefinirEventoApresentacaoModelQuery>(query, new { eventoID = eventoID, diasLimiteCancelamento = diasLimiteCancelamento }).ToList();
        }

        public List<CancelamentoModeloMotivo> DefinirEventoMotivoCancelamento()
        {
            string query = @"SELECT
	                            ID
                                ,Modelo
	                            ,Descricao
                            FROM 
	                            tCancelamentoLoteModeloMotivo(NOLOCK)";
            return conIngresso.Query<CancelamentoModeloMotivo>(query).ToList();
        }

        public IPagedList<EventoCancelarMassaModelQuery> ListarEventosCancelar(string busca, int pagina, int itens, int diasLimiteCancelamento)
        {
            string buscaLike = "%" + busca + "%";
            int size = Convert.ToInt32(conIngresso.ExecuteScalar(@"Select COUNT(DISTINCT tEvento.ID) 
		                                                        FROM tEvento(NOLOCK)
		                                                        INNER JOIN tLocal(NOLOCK) ON tEvento.LocalId = tLocal.ID
                                                                INNER JOIN tEmpresa(NOLOCK) ON tLocal.EmpresaId = tEmpresa.ID
		                                                        INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                                                                WHERE (tEvento.Nome Like @buscaLike OR tLocal.Nome Like @buscaLike OR tEmpresa.Nome Like @buscaLike OR tLocal.Cidade Like @buscaLike)
                                                                      AND tApresentacao.Cancelada <> 'T'", new { buscaLike = buscaLike }));

            string query = @"SELECT RowNumber,ID,EventoNome,EmpresaNome,LocalNome,Horario, PodeCancelar FROM
                             ( SELECT ROW_NUMBER() OVER (ORDER BY EventoNome) as RowNumber, 
                                      ID, 
                                      EmpresaNome, 
                                      EventoNome,
                                      LocalNome,
                                      Horario,
                                      CASE WHEN DATEDIFF(DAY, Horario, GETDATE()) >= @diasLimiteCancelamento THEN 0 ELSE 1 END as PodeCancelar
                                FROM
                                (
                                    SELECT Distinct 
                                                tEvento.ID as ID,
                                                tEvento.Nome as EventoNome, 
                                                tEmpresa.Nome as EmpresaNome, 
                                                tLocal.Nome as LocalNome,
                                                (Select TOP 1 CalcHorario FROM tApresentacao(NOLOCK) WHERE EventoID = tEvento.ID ORDER BY CalcHorario DESC) as Horario
		                            FROM tEvento(NOLOCK)
		                            INNER JOIN tLocal(NOLOCK) ON tEvento.LocalId = tLocal.ID
                                    INNER JOIN tEmpresa(NOLOCK) ON tLocal.EmpresaId = tEmpresa.ID
		                            INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.EventoID = tEvento.ID
		                            WHERE (tEvento.Nome Like @buscaLike OR tLocal.Nome Like @buscaLike OR tLocal.Cidade Like @buscaLike OR tEmpresa.Nome Like @buscaLike)
                                          AND tApresentacao.Cancelada <> 'T') as tbl1
                             ) as tbl2   
                        WHERE RowNumber BETWEEN ((@pagina - 1) * @itens + 1) AND (@pagina * @itens)
                        ORDER BY PodeCancelar desc, EventoNome";
            var result = conIngresso.Query<EventoCancelarMassaModelQuery>(query, new { busca = busca, buscaLike = buscaLike, pagina = pagina, itens = itens, diasLimiteCancelamento = diasLimiteCancelamento });
            PagedModel<EventoCancelarMassaModelQuery> pageModel = new PagedModel<EventoCancelarMassaModelQuery>(result.ToList(), size);
            return pageModel.List.ToPagedList(pagina, itens, pageModel.Count);
        }

        public IPagedList<CancelamentoMassaModelQuery> ListarEventosCancelados(string busca, int pagina, int itens)
        {
            string buscaLike = "%" + busca + "%";
            int size = Convert.ToInt32(
                conIngresso.ExecuteScalar(@"
                                            SELECT COUNT(*)
                                            FROM tCancelamentoLote (NOLOCK) cl
                                            INNER JOIN tEvento (NOLOCK) e on e.ID = cl.EventoID
                                            INNER JOIN tLocal (NOLOCK) l on l.ID = e.LocalID
                                            INNER JOIN tEmpresa (NOLOCK) emp on emp.ID = l.EmpresaID
                                            WHERE e.Nome LIKE @buscaLike OR emp.Nome LIKE @buscaLike OR cl.CodigoCancelamento LIKE @buscaLike or l.Nome like @buscaLike
                                            ",
                                            new
                                            {
                                                busca = busca,
                                                buscaLike = buscaLike
                                            }));

            string query = @"SELECT ID, EventoID, DataCancelamento, CodigoCancelamento, Empresa, Evento, Local, DatasCanceladas FROM
                            ( SELECT ROW_NUMBER() OVER (ORDER BY cl.DataCancelamento desc) as RowNumber, e.ID as EventoID, cl.ID, dbo.StringToDateTime(cl.DataCancelamento) as DataCancelamento, cl.CodigoCancelamento, emp.Nome as Empresa, e.Nome as Evento, l.Nome as Local, count(cl.ID) as DatasCanceladas
                                FROM tCancelamentoLote (NOLOCK) cl
                                JOIN tEvento (NOLOCK) e on e.ID = cl.EventoID
                                JOIN tLocal (NOLOCK) l on l.ID = e.LocalID
                                JOIN tEmpresa (NOLOCK) emp on emp.ID = l.EmpresaID
                                JOIN tRegional (NOLOCK) r on r.ID = emp.RegionalID
                                JOIN tCancelamentoLoteApresentacao (NOLOCK) cla on cla.CancelamentoLoteID = cl.ID
                                WHERE e.Nome LIKE @buscaLike OR emp.Nome LIKE @buscaLike OR cl.CodigoCancelamento LIKE @buscaLike or l.Nome like @buscaLike
                                GROUP BY cl.ID, cl.DataCancelamento, cl.CodigoCancelamento, emp.Nome, e.Nome, l.Nome, e.ID
                            ) as tbl2
                            WHERE RowNumber BETWEEN ((@pagina - 1) * @itens + 1) AND (@pagina * @itens)
                            ORDER BY DataCancelamento desc";
            var result = conIngresso.Query<CancelamentoMassaModelQuery>(query, new { busca = busca, buscaLike = buscaLike, pagina = pagina, itens = itens });
            PagedModel<CancelamentoMassaModelQuery> pageModel = new PagedModel<CancelamentoMassaModelQuery>(result.ToList(), size);
            return pageModel.List.ToPagedList(pagina, itens, pageModel.Count);
        }

        public CancelamentoDetalheModelQuery ConsultarCancelamento(int cancelamentoID)
        {
            string query = @"SELECT 
	                            tUsuario.Nome AS Usuario
	                            ,dbo.StringToDateTime(tCancelamentoLote.DataCancelamento) AS Solicitado
	                            ,tEvento.Nome AS Evento
	                            ,tLocal.Nome AS Local
	                            ,tRegional.Nome AS Regional
	                            ,tEmpresa.Nome AS Empresa
	                            ,tCancelamentoLote.MotivoCancelamento AS Motivo
	                            ,tCancelamentoLote.CodigoCancelamento AS Codigo
                            FROM 
	                            tCancelamentoLote
	                            LEFT JOIN tEvento (NOLOCK) ON tCancelamentoLote.EventoID = tEvento.ID
	                            LEFT JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID
	                            LEFT JOIN tEmpresa (NOLOCK) ON tLocal.EmpresaID = tEmpresa.ID
	                            LEFT JOIN tRegional (NOLOCK) ON tEmpresa.RegionalID = tRegional.ID
	                            LEFT JOIN tUsuario (NOLOCK) ON tCancelamentoLote.UsuarioID = tUsuario.ID
                            WHERE 
	                            tCancelamentoLote.ID = @cancelamentoID";
            return conIngresso.Query<CancelamentoDetalheModelQuery>(query, new { cancelamentoID = cancelamentoID }).FirstOrDefault();
        }

        public CancelamentoRelatorioDadosBasicos ConsultarCancelamentoRelatorioDadosBasicos(string codigoCancelamento, int cancelamentoLoteID)
        {
            if (codigoCancelamento == null)
                codigoCancelamento = "";

            string query = @"Select TOP 1 evt.Nome as Evento, reg.nome as Regional,loc.nome as Local, emp.nome as Empresa, canc.DataCancelamento as Data,usu.Nome as Usuario,canc.MotivoCancelamento
                             FROM tCancelamentoLote canc(NOLOCK) 
                             INNER JOIN tEvento evt(NOLOCK) ON canc.EventoID = evt.ID
                             INNER JOIN tLocal loc(NOLOCK) ON evt.LocalId = loc.ID
                             INNER JOIN tEmpresa emp(NOLOCK) ON loc.EmpresaID = emp.ID
                             INNER JOIN tRegional reg(NOLOCK) ON emp.RegionalID = reg.ID
                             INNER JOIN tUsuario usu(NOLOCK) ON canc.UsuarioID = usu.ID
                             WHERE canc.ID = @cancelamentoLoteID OR canc.CodigoCancelamento = @codigoCancelamento";
            return conIngresso.Query<CancelamentoRelatorioDadosBasicos>(query, new
            {
                codigoCancelamento = codigoCancelamento,
                cancelamentoLoteID = cancelamentoLoteID
            }).FirstOrDefault();
        }

        public CancelamentoRelatorioDadosBasicos ConsultarCancelamentoRelatorioDadosBasicos(List<int> apresentacoesID)
        {
            string query = @"Select TOP 1 evt.Nome as Evento, reg.nome as Regional,loc.nome as Local, emp.nome as Empresa
                                FROM tApresentacao apr(NOLOCK) 
                                INNER JOIN tEvento evt(NOLOCK) ON apr.EventoID = evt.ID
                                INNER JOIN tLocal loc(NOLOCK) ON evt.LocalId = loc.ID
                                INNER JOIN tEmpresa emp(NOLOCK) ON loc.EmpresaID = emp.ID
                                INNER JOIN tRegional reg(NOLOCK) ON emp.RegionalID = reg.ID
                                WHERE apr.ID in @apresentacoesID";
            return conIngresso.Query<CancelamentoRelatorioDadosBasicos>(query, new
            {
                apresentacoesID = apresentacoesID
            }).FirstOrDefault();
        }

        public CancelamentoRelatorioDadosTotais ConsultarCancelamentoRelatorioDadosTotalizadores(List<int> apresentacoesID)
        {
            string query = @"SELECT COUNT(ID) as TotalIngressos,
	                           COUNT(DISTINCT apresentacaoID) as TotalApresentacoes,
	                           COUNT(DISTINCT PacoteID) as PacotesDesativados,
	                           SUM(Impresso) as Impressos,
	                           SUM(Vendido) as Vendidos, 
	                           (COUNT(ID) - SUM(Impresso) - SUM(Vendido)) as Disponiveis, 
	                           CASE WHEN SUM(VendaAtiva) > 0 then 'True' ELSE 'False' END VendaAtiva
		                        FROM
		                        (
			                        Select tIngresso.ID, tIngresso.apresentacaoID,tIngresso.PacoteID,
			                        CASE WHEN Status = 'I' THEN 1 ELSE 0 END Impresso,
			                        CASE WHEN Status = 'V' THEN 1 ELSE 0 END Vendido,
			                        CASE WHEN DisponivelVenda = 'T' THEN 1 ELSE 0 END VendaAtiva
			                        FROM tIngresso(NOLOCK)
			                        INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
			                        WHERE apresentacaoID in @apresentacoesID
		                        )tbl";
            return conIngresso.Query<CancelamentoRelatorioDadosTotais>(query, new
            {
                apresentacoesID = apresentacoesID
            }).FirstOrDefault();
        }

        private InfoLeiMeia infoMeia;

        public InfoLeiMeia ConsultarInfoMeiaEntrada(int eventoId, int canalId)
        {
            var eventosSemMeiaEntrada = ConfiguracaoAppUtil.GetAsListInt("EventosSemMeiaEntrada");
            var locaisSemMeiaEntrada = ConfiguracaoAppUtil.GetAsListInt("LocaisSemMeiaEntrada");

            infoMeia = new InfoLeiMeia();

            var infoLeiMeia = this.conIngresso.Query<Evento, Apresentacao, Setor, Local, InfoLeiMeia>("sp_ConsultarInfoMeiaEntrada", mapEventoInfoMeia,
                    new { eventoId = eventoId, canalId = canalId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            if (infoLeiMeia == null) return null;

            if (locaisSemMeiaEntrada.Contains(infoLeiMeia.localId))
            {
                infoLeiMeia.temMeiaEntrada = false;
            }
            else
            {
                infoLeiMeia.temMeiaEntrada = !eventosSemMeiaEntrada.Contains(eventoId);
            }

            return infoLeiMeia;
        }

        private InfoLeiMeia mapEventoInfoMeia(Evento evento, Apresentacao apr, Setor setor, Local local)
        {
            if (string.IsNullOrEmpty(infoMeia.nomeEvento)) infoMeia.nomeEvento = evento.Nome;
            if (infoMeia.localId == 0) infoMeia.localId = local.ID;

            if (infoMeia.apresentacoes == null)
                infoMeia.apresentacoes = new List<InfoLeiMeia.Aprentacao>();

            var apresentacao_ = infoMeia.apresentacoes.FirstOrDefault(f => f.id == apr.ID);
            if (apresentacao_ == null)
            {
                apresentacao_ = new InfoLeiMeia.Aprentacao
                {
                    id = apr.ID,
                    dataApresentacao = apr.CalcHorario.Value,
                    diaSemana = apr.CalcDiaDaSemana ?? 0,
                    cotaId = apr.CotaID,
                    setores = new List<InfoLeiMeia.Setor>()
                };

                infoMeia.apresentacoes.Add(apresentacao_);
            }

            var setor_ = apresentacao_.setores.FirstOrDefault(f => f.id == setor.ID);
            if (setor_ == null)
            {
                setor_ = new InfoLeiMeia.Setor
                {
                    id = setor.ID,
                    nomeSetor = setor.Nome,
                    qtdTotalTickets = setor.QtdeTotal,
                    qtdTotalMeiaIndisp = setor.QtdTotalMeiaIndisp
                };

                infoMeia.apresentacoes.Find(f => f == apresentacao_).setores.Add(setor_);
            }

            return infoMeia;
        }
    }
}