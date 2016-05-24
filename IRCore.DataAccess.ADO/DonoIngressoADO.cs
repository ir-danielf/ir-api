using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class DonoIngressoADO : MasterADO<dbIngresso>
    {
        public DonoIngressoADO(MasterADOBase ado = null) : base(ado, false) { }
        
        public tDonoIngresso Consultar(int id)
        {
            var queryStr = @"
                    SELECT TOP 1
	                    ID,Nome,RG,CPF,Email,Telefone,DataNascimento,CPFResponsavel,NomeResponsavel,ClienteID
                    FROM
	                    tDonoIngresso (NOLOCK)
                    WHERE
	                    ID = @DonoIngressoID";
            tDonoIngresso query = conIngresso.Query<tDonoIngresso>(queryStr, new { DonoIngressoID = id }).FirstOrDefault();
            return query;
        }

        public bool Remover(int id)
        {
            var sql = "DELETE FROM tDonoIngresso WHERE ID = @id";

            var linhas = conIngresso.Execute(sql, new {id});

            return linhas > 0;
        }
    }
}
