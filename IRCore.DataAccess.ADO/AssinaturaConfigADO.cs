using System.Collections.Generic;
using System.Linq;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class AssinaturaConfigADO : MasterADO<dbIngresso>
    {
        public AssinaturaConfigADO(MasterADOBase ado = null) : base(ado, true, true) { }


        public AssinaturaConfigModel Consultar(int id)
        {
            var queryStr = @"select acf.ID,
                                    acf.Nome,
                                    acf.AssinaturaTipoID,
                                    ass.Nome as AssinaturaTipoNome,
                                    acf.AnoAtivoAssinatura,
                                    acf.AnoAtivoBancoIngressos,
                                    acf.AnoAtivo
                               from dbo.tAssinaturaConfig acf (nolock)
                         inner join tAssinaturaTipo ass (nolock) on ass.id = acf.AssinaturaTipoID
                              where acf.ID = @ID";

            var query = conIngresso.Query<AssinaturaConfigModel>(queryStr, new { ID = id });

            return query.FirstOrDefault();
        }
        public List<AssinaturaConfigModel> BuscaByAssinaturaConfigNome(string assinaturaConfigNome = "")
        {
            var queryStr = @"select acf.ID,
                                    acf.Nome,
                                    acf.AssinaturaTipoID,
                                    ass.Nome as AssinaturaTipoNome,
                                    acf.AnoAtivoAssinatura,
                                    acf.AnoAtivoBancoIngressos,
                                    acf.AnoAtivo
                               FROM tAssinaturaConfig acf (NOLOCK)
                         inner join tAssinaturaTipo ass (nolock) on ass.id = acf.AssinaturaTipoID ";
            if (!string.IsNullOrWhiteSpace(assinaturaConfigNome))
                queryStr += @"WHERE acf.Nome like '%" + assinaturaConfigNome + "%';";

            var query = conIngresso.Query<AssinaturaConfigModel>(queryStr);

            return query.ToList();
        }

        public bool Alterar(AssinaturaConfigModel atm)
        {
            var result = (conIngresso.Execute(@"update tAssinaturaConfig 
	                                               set Nome = @nome,
                                                       AssinaturaTipoID = @assinaturaTipoID,
		                                               AnoAtivoAssinatura = @anoAtivoAssinatura,
		                                               AnoAtivoBancoIngressos = @anoAtivoBancoIngressos,
		                                               AnoAtivo = @anoAtivo
	                                             where ID = @id",
                new
                {
                    nome = atm.Nome,
                    assinaturaTipoID = atm.AssinaturaTipoID,
                    anoAtivoAssinatura = atm.AnoAtivoAssinatura,
                    anoAtivoBancoIngressos = atm.AnoAtivoBancoIngressos,
                    anoAtivo = atm.AnoAtivo,
                    id = atm.ID
                }) > 0);

            return result;
        }

        public int Insere(AssinaturaConfigModel atm)
        {
            int result = conIngresso.Query<int>(@"insert into tAssinaturaConfig
		                                            ( Nome
		                                            , AssinaturaTipoID
		                                            , AnoAtivoAssinatura
		                                            , AnoAtivoBancoIngressos
		                                            , AnoAtivo) 
                                            OUTPUT Inserted.ID
                                            values  ( @nome
		                                            , @assinaturaTipoID
		                                            , @anoAtivoAssinatura
		                                            , @anoAtivoBancoIngressos
		                                            , @anoAtivo)",
                new
                {
                    nome = atm.Nome,
                    assinaturaTipoID = atm.AssinaturaTipoID,
                    anoAtivoAssinatura = atm.AnoAtivoAssinatura,
                    anoAtivoBancoIngressos = atm.AnoAtivoBancoIngressos,
                    anoAtivo = atm.AnoAtivo
                }).FirstOrDefault();

            return result;
        }
    }
}
