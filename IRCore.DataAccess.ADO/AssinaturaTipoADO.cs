using System.Collections.Generic;
using System.Linq;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class AssinaturaTipoADO : MasterADO<dbIngresso>
    {
        public AssinaturaTipoADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public int getId()
        {
            var queryStr = @"select max(id)+1 from tAssinaturaTipo (nolock)";

            int id = conIngresso.Query<int>(queryStr).FirstOrDefault();

            return id;
        }

        public int Insere(AssinaturaTipoModel atm, int ID)
        {
            var result = (conIngresso.Execute(@"insert into [tAssinaturaTipo] 
		                                                ( ID
                                                        , Nome
		                                                , RenovacaoFim
		                                                , TrocaPrioritariaInicio
		                                                , TrocaPrioritariaFim
		                                                , TrocaInicio
		                                                , TrocaFim
		                                                , NovaAquisicaoInicio
		                                                , NovaAquisicaoFim
		                                                , RenovacaoInicio
		                                                , LocalID
		                                                , Programacao
		                                                , Precos
		                                                , Termos
		                                                , PaginaPrincipal
		                                                , PermiteAgregados
		                                                , CanalAcessoID
		                                                , Logo
                                                        , AtivaBancoIngresso) 
                                                values  ( @Id
                                                        , @Nome
		                                                , @RenovacaoFim
		                                                , @TrocaPioritariaInicio
		                                                , @TrocaPrioritariaFim
		                                                , @TrocaInicio
		                                                , @TrocaFim
		                                                , @NovaAquisicaoInicio
		                                                , @NovaAquisicaoFim
		                                                , @RenovacaoInicio
		                                                , @LocalID
		                                                , @Programacao
		                                                , @Precos
		                                                , @Termos
		                                                , @PaginaPrincipal
		                                                , @PermiteAgregados
		                                                , @CanalAcessoID
		                                                , @Logo
                                                        , @ativaBancoIngresso)",
                new
                {
                    Nome = atm.Nome,
                    RenovacaoFim = atm.RenovacaoFim,
                    TrocaPioritariaInicio = atm.TrocaPrioritariaInicio,
                    TrocaPrioritariaFim = atm.TrocaPrioritariaFim,
                    TrocaInicio = atm.TrocaInicio,
                    TrocaFim = atm.TrocaFim,
                    NovaAquisicaoInicio = atm.NovaAquisicaoInicio,
                    NovaAquisicaoFim = atm.NovaAquisicaoFim,
                    RenovacaoInicio = atm.RenovacaoInicio,
                    LocalID = atm.LocalID,
                    Programacao = atm.Programacao,
                    Precos = atm.Precos,
                    Termos = atm.Termos,
                    PaginaPrincipal = atm.PaginaPrincipal,
                    PermiteAgregados = atm.PermiteAgregados,
                    CanalAcessoID = atm.CanalAcessoID,
                    Logo = atm.Logo,
                    ativaBancoIngresso = atm.AtivaBancoIngresso,
                    Id = ID
                }) > 0);

            return ID;

        }

        public bool Altera(AssinaturaTipoModel atm)
        {
            var result = (conIngresso.Execute(@"update tAssinaturaTipo 
	                                                set Nome = @nome,
                                                        RenovacaoInicio = @renovacaoInicio,
		                                                RenovacaoFim = @renovacaoFim,
		                                                TrocaPrioritariaInicio = @trocaPrioritariaInicio,
		                                                TrocaPrioritariaFim = @trocaPrioritariaFim,
		                                                TrocaInicio = @trocaInicio,
		                                                TrocaFim = @trocaFim,
		                                                NovaAquisicaoInicio = @novaAquisicaoInicio,
		                                                NovaAquisicaoFim = @novaAquisicaoFim,
		                                                LocalID = @localId,
		                                                PermiteAgregados = @permiteAgregados,
                                                        Precos = @preco,
                                                        Logo = @logo,
                                                        AtivaBancoIngresso = @ativaBancoIngresso,
                                                        Programacao = @programacao,
                                                        CanalAcessoID = @canalAcessoID
	                                                where ID = @id",
                new
                {
                    nome = atm.Nome,
                    renovacaoInicio = atm.RenovacaoInicio,
                    renovacaoFim = atm.RenovacaoFim,
                    trocaPrioritariaInicio = atm.TrocaPrioritariaInicio,
                    trocaPrioritariaFim = atm.TrocaPrioritariaFim,
                    trocaInicio = atm.TrocaInicio,
                    trocaFim = atm.TrocaFim,
                    novaAquisicaoInicio = atm.NovaAquisicaoInicio,
                    novaAquisicaoFim = atm.NovaAquisicaoFim,
                    localId = atm.LocalID,
                    permiteAgregados = atm.PermiteAgregados,
                    id = atm.ID,
                    preco = atm.Precos,
                    logo = atm.Logo,
                    ativaBancoIngresso = atm.AtivaBancoIngresso,
                    programacao = atm.Programacao,
                    canalAcessoID = atm.CanalAcessoID
                }) > 0);

            return result;
        }

        public AssinaturaTipoModel Consultar(int id)
        {
            var queryStr = @"select
                              at.ID,
                              at.Nome,
                              at.RenovacaoFim,
                              at.TrocaPrioritariaInicio,
                              at.TrocaPrioritariaFim,
                              at.TrocaInicio,
                              at.TrocaFim,
                              at.NovaAquisicaoInicio,
                              at.NovaAquisicaoFim,
                              at.RenovacaoInicio,
                              at.LocalID,
                              at.Programacao,
                              at.Precos,
                              at.Termos,
                              at.PaginaPrincipal,
                              at.PermiteAgregados,
                              at.Layout,
                              at.PaginaLogin,
                              at.PaginaRodape,
                              at.CanalAcessoID,
                              tc.Nome as CanalAcessoNome,
                              at.RetiradaBilheteria,
                              at.ValorEntregaFixo,
                              at.EntregaID,
                              at.ValorEntrega,
                              at.Logo,
                              isNull(at.AtivaBancoIngresso, 'F') as AtivaBancoIngresso,
                              tl.ID as LocalID,
                              tl.Nome as LocalNome,
                              te.ID as EmpresaID,
                              te.Nome as EmpresaNome,
                              tr.ID as RegionalID,
                              tr.Nome as RegionalNome
                            from
                              dbo.tAssinaturaTipo at (nolock)
                              INNER JOIN dbo.tLocal tl (nolock) on tl.ID = at.LocalID
                              inner JOIN dbo.tEmpresa te (NOLOCK ) on te.ID = tl.EmpresaID
                              inner join dbo.tRegional tr (nolock) on te.RegionalID = tr.ID
                              left outer join dbo.tCanal tc (nolock) on tc.ID = at.CanalAcessoID
                            where at.ID = @ID";

            var query = conIngresso.Query<AssinaturaTipoModel>(queryStr, new
            {
                ID = id
            });

            return query.FirstOrDefault();
        }

        public List<AssinaturaTipoModel> Busca(string busca)
        {
            var queryStr = @"SELECT a.[ID]
                                ,a.[Nome]
                                ,a.[RenovacaoFim]
                                ,a.[TrocaPrioritariaInicio]
                                ,a.[TrocaPrioritariaFim]
                                ,a.[TrocaInicio]
                                ,a.[TrocaFim]
                                ,a.[NovaAquisicaoInicio]
                                ,a.[NovaAquisicaoFim]
                                ,a.[RenovacaoInicio]
                                ,a.[LocalID]
                                ,a.[Programacao]
                                ,a.[Precos]
                                ,a.[Termos]
                                ,a.[PaginaPrincipal]
                                ,a.[PermiteAgregados]
                                ,a.[Layout]
                                ,a.[PaginaLogin]
                                ,a.[PaginaRodape]
                                ,a.[CanalAcessoID]
                                ,a.[RetiradaBilheteria]
                                ,a.[ValorEntregaFixo]
                                ,a.[EntregaID]
                                ,a.[ValorEntrega]
                                ,a.[Logo]
                                ,tL.nome as LocalNome
                        FROM [dbo].[tAssinaturaTipo] a (nolock)
	                    inner join dbo.tLocal (NOLOCK) tL ON tL.ID = a.LocalID  
                        where a.[Nome] like '%" + busca + "%'";
            var query = conIngresso.Query<AssinaturaTipoModel>(queryStr);
            return query.ToList();
        }

        public List<AssinaturaTipoModel> Lista()
        {
            var queryStr = @"SELECT ROW_NUMBER() OVER (ORDER BY T.ID, T.Ano) AS TipoAno, * FROM (
                            SELECT 
	                            DISTINCT at.ID
	                            , at.Nome
	                            , at.RenovacaoFim
	                            , at.TrocaPrioritariaInicio
	                            , at.TrocaPrioritariaFim
	                            , at.TrocaInicio
	                            , at.TrocaFim
	                            , at.NovaAquisicaoInicio
	                            , at.NovaAquisicaoFim
	                            , at.RenovacaoInicio
	                            , at.LocalID
	                            , at.Programacao
	                            , at.Precos
	                            , at.Termos
	                            , at.PaginaPrincipal
	                            , at.PermiteAgregados
	                            , at.Layout
	                            , at.PaginaLogin
	                            , at.PaginaRodape
	                            , at.CanalAcessoID
	                            , at.RetiradaBilheteria
	                            , at.ValorEntregaFixo
	                            , at.EntregaID
	                            , at.ValorEntrega
	                            , at.Logo
	                            , at.AtivaBancoIngresso
	                            , an.Ano 
                            FROM 
	                            tAssinatura ass (NOLOCK)
	                            INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = ass.ID
	                            INNER JOIN tAssinaturaTipo at (nolock) on at.ID = ass.AssinaturaTipoID) AS T";
            var query = conIngresso.Query<AssinaturaTipoModel>(queryStr);
            return query.ToList();
        }

        public IEnumerable<AssinaturaAnoModel> BuscaListaAno(int assinaturaTipoID)
        {
            var queryStr = @"SELECT DISTINCT
                                  an.Ano
                                FROM
                                  tAssinatura ass ( NOLOCK )
                                  INNER JOIN tAssinaturaAno an ( NOLOCK ) ON an.AssinaturaID = ass.ID
                                  INNER JOIN tAssinaturaTipo at ( NOLOCK ) ON at.ID = ass.AssinaturaTipoID
                                where at.id = @ID;";
            var query = conIngresso.Query<AssinaturaAnoModel>(queryStr, new
            {
                ID = assinaturaTipoID
            });
            return query.ToList();
        }
    }
}
