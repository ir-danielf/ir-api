using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class CanalADO : MasterADO<dbIngresso>
    {   
        public CanalADO(MasterADOBase ado = null) : base(ado) { }

        public tCanal Consultar(int id)
        {
            var queryStr = @"Select Top 1
                         c.ID, c.EmpresaID, c.Nome, c.Comprovante, c.Obs, c.CanalTipoID, c.ClientePresente, c.TaxaConveniencia, c.OpcaoImprimirSemPreco, c.Cartao, c.NaoCartao, c.TaxaMinima, c.TaxaMaxima, c.ObrigaCadastroCliente, c.TaxaComissao, c.ComissaoMinima, c.ComissaoMaxima, c.Comissao, c.ConfirmacaoPorEmail, c.TipoVenda, c.PoliticaTroca, c.ComprovanteQuantidade, c.ObrigatoriedadeID, c.EnviaSms, c.TEFF, c.NroEstabelecimento, c.SiglaTipo, c.SiglaPagamento, c.ResponsabilidadeDinheiroCliente, c.Responsavel, c.Recolhimento, c.Sangria
                    FROM tCanal (nolock) c
                WHERE c.ID = @id";

            var query = conIngresso.Query<tCanal>(queryStr, new { id = id });

            return query.FirstOrDefault();
        }

        public decimal ConsultarTaxaConveniencia(int eventoID, int canalID)
        {
            var querySTR = @"SELECT TOP 1 ISNULL(ce.TaxaConveniencia, 0) AS TaxaConveniencia FROM tCanalEvento (nolock) ce WHERE ce.EventoID = @eventoID AND ce.CanalID = @canalID";

            var query = conIngresso.Query<decimal>(querySTR, new { eventoID, canalID });
            return query.FirstOrDefault();
        }

        public List<ListaCanalPorNome_Result> ListarCanalPorNome(string canal)
        {
            string query = @"SELECT DISTINCT
	                            Can.ID AS CanalID
	                            ,Can.Nome AS CanalNome
	                            ,Emp.Nome AS EmpresaNome
	                            ,Reg.Nome AS RegionalNome
                            FROM
	                            tCanal(NOLOCK) AS Can
	                            INNER JOIN tEmpresa(NOLOCK) AS Emp ON Emp.ID = Can.EmpresaID
	                            INNER JOIN tRegional(NOLOCK) AS Reg ON Reg.ID = Emp.RegionalID
                            WHERE
                                Can.Nome like @canal ";
            return conIngresso.Query<ListaCanalPorNome_Result>(query, new { canal = "%" + canal + "%" }).ToList();
        }

        public bool isPOS(int canalId, int canalTipoId)
        {
            var sql = 
@"SELECT count(*)
FROM tCanal(NOLOCK) AS ca
     INNER JOIN tCanalTipo(NOLOCK) AS ct ON(ca.CanalTipoID = ct.ID)
WHERE ca.Id = @canalId and ct.Id = @canalTipoId;";

            var result = conIngresso.Execute(sql, new { canalId = canalId, canalTipoId = canalTipoId });

            return result > 0;
        }
    }
}