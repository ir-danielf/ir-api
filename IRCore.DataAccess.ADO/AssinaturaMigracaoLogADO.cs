using System.Collections.Generic;
using System.Linq;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class AssinaturaMigracaoLogADO : MasterADO<dbIngresso>
    {
        public AssinaturaMigracaoLogADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public AssinaturaMigracaoLogModel ConsultaMigrado(int AssinaturaIDOrigem, int AssinaturaIDNovo)
        {
            var queryStr = @"SELECT [ID]
                                  ,[AssinaturaTipoIDOrigem]
                                  ,[AssinaturaAnoOrigem]
                                  ,[AssinaturaTipoIDNovo]
                                  ,[AssinaturaAnoNovo]
                                  ,[AssinaturaIDOrigem]
                                  ,[AssinaturaIDNovo]
                                  ,[DataMigracao]
                                  ,[UsuarioID]
                              FROM [IngressosNovo].[dbo].[tAssinaturaMigracaoLog]
                              where [AssinaturaIDOrigem] = @assinaturaIDOrigem
                              AND [AssinaturaIDNovo] = @assinaturaIDNovo";

            var query = conIngresso.Query<AssinaturaMigracaoLogModel>(queryStr, new { assinaturaIDOrigem = AssinaturaIDOrigem, assinaturaIDNovo = AssinaturaIDNovo });

            return query.FirstOrDefault();
        }

        public bool Insere(AssinaturaMigracaoLogModel aml)
        {
            var result = (conIngresso.Execute(@"insert into tAssinaturaMigracaoLog
                                            (  AssinaturaTipoIDOrigem
                                                , AssinaturaAnoOrigem
                                                , AssinaturaTipoIDNovo
                                                , AssinaturaAnoNovo
                                                , AssinaturaIDOrigem
                                                , AssinaturaIDNovo
                                                , DataMigracao
                                                , UsuarioID)
								            values(
                                                @AssinaturaTipoIDOrigem
                                                ,@AssinaturaAnoOrigem
                                                ,@AssinaturaTipoIDNovo
                                                ,@AssinaturaAnoNovo
                                                ,@AssinaturaIDOrigem
                                                ,@AssinaturaIDNovo
                                                ,@DataMigracao
                                                ,@UsuarioID
                                            ))",
                new
                {
                    AssinaturaTipoIDOrigem = aml.AssinaturaTipoIDOrigem,
                    AssinaturaAnoOrigem = aml.AssinaturaAnoOrigem,
                    AssinaturaTipoIDNovo= aml.AssinaturaTipoIDNovo,
                    AssinaturaAnoNovo = aml.AssinaturaAnoNovo,
                    AssinaturaIDOrigem = aml.AssinaturaIDOrigem,
                    AssinaturaIDNovo = aml.AssinaturaIDNovo,
                    DataMigracao = aml.DataMigracao,
                    UsuarioID = aml.UsuarioID
                }) > 0);

            return result;

        }

        public AssinaturaMigracaoLogModel Consultar(int id)
        {
            var queryStr = @"select aml.ID,
                                    aml.AssinaturaTipoIDOrigem,
                                    aml.AssinaturaAnoIDOrigem,
                                    aml.AssinaturaTipoIDNovo,
                                    aml.AssinaturaAnoIDNovo,
                                    aml.AssinaturaIDOrigem,
                                    aml.AssinaturaIDNovo,
                                    aml.DataMigracao,
                                    aml.UsuarioID
                               from dbo.AssinaturaMigracaoLog aml (nolock)
                              where aml.ID = @ID";

            var query = conIngresso.Query<AssinaturaMigracaoLogModel>(queryStr, new { ID = id });

            return query.FirstOrDefault();
        }
        
        public List<AssinaturaMigracaoLogModel> busca(string busca)
        {

            var queryStr = @"SELECT distinct
                                    aml.AssinaturaTipoIDOrigem,
                                    aml.AssinaturaTipoIDNovo,
                                    dbo.StringToDateTime(aml.DataMigracao) as DataMigracao,
                                    aml.UsuarioID,
                                    ato.Nome as AssinaturaTipoOrigem,
                                    atn.Nome as AssinaturaTipoNovo,
                                    aml.AssinaturaAnoOrigem,
                                    aml.AssinaturaAnoNovo
                                FROM dbo.tAssinaturaMigracaoLog aml ( NOLOCK )
                                    INNER JOIN dbo.tAssinaturaTipo ato ( NOLOCK ) ON ato.id = aml.AssinaturaTipoIDOrigem
                                    INNER JOIN dbo.tAssinaturaTipo atn ( NOLOCK ) ON atn.id = aml.AssinaturaTipoIDNovo
                              where aml.AssinaturaTipoIDOrigem in (SELECT id
                                                                     FROM dbo.tAssinaturaTipo
                                                                    where Nome like '%" + busca + @"%')
                                 or  aml.AssinaturaTipoIDNovo in (SELECT id
                                                                    FROM dbo.tAssinaturaTipo 
                                                                   where Nome like '%" + busca + @"%')";
                        
            var query = conIngresso.Query<AssinaturaMigracaoLogModel>(queryStr);
            return query.ToList();
        }

        public List<AssinaturaMigracaoLogModelRel> RelatorioMigracao(int AssinaturaTipoIDOrigem, int AssinaturaTipoIDNovo, int AssinaturaAnoOrigem, int AssinaturaAnoNovo)
        {

            var queryStr = @"exec Proc_AssinaturaMigracaoLogRel {0}, {1}, {2}, {3};";

            var query = conIngresso.Query<AssinaturaMigracaoLogModelRel>(string.Format(queryStr, AssinaturaTipoIDOrigem, AssinaturaTipoIDNovo, AssinaturaAnoOrigem, AssinaturaAnoNovo));
            return query.ToList();
        }

        public List<AssinaturaClientesNaoMigradosRel> RelatorioClientesNaoMigrados(int AssinaturaTipoIDOrigem, int AssinaturaTipoIDNovo, int AssinaturaAnoOrigem, int AssinaturaAnoNovo)
        {

            var queryStr = @"exec Proc_AssinaturaClientesNaoMigrados {0}, {1}, {2}, {3};";

            var query = conIngresso.Query<AssinaturaClientesNaoMigradosRel>(string.Format(queryStr, AssinaturaTipoIDOrigem, AssinaturaTipoIDNovo, AssinaturaAnoOrigem, AssinaturaAnoNovo));
            return query.ToList();
        }
    }
}
