using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class ObrigatoriedadeADO : MasterADO<dbIngresso>
    {
        public ObrigatoriedadeADO(MasterADOBase ado = null) : base(ado, false) { }
        
        /// <summary>
        /// Consulta uma obrigatoriedade
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tObrigatoriedade Consultar(int id)
        {
            var queryStr = @"
                    SELECT TOP 1
	                    ID,Nome,RG,CPF,Telefone,TelefoneComercial,Celular,DataNascimento,Email,CEPEntrega,EnderecoEntrega,NumeroEntrega,ComplementoEntrega,BairroEntrega,CidadeEntrega,EstadoEntrega,CEPCliente,EnderecoCliente,NumeroCliente,ComplementoCliente,BairroCliente,CidadeCliente,EstadoCliente,NomeEntrega,CPFEntrega,RGEntrega,CPFResponsavel,NomeResponsavel
                    FROM
	                    tObrigatoriedade (NOLOCK)
                    WHERE
	                    ID = @ObrigatoriedadeID";
            tObrigatoriedade query = conIngresso.Query<tObrigatoriedade>(queryStr, new { ObrigatoriedadeID = id }).FirstOrDefault();
            return query;
        }
    }
}
