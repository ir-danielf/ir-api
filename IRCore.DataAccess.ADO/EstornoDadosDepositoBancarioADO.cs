using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class EstornoDadosDepositoBancarioADO : MasterADO<dbIngresso>
    {
        public EstornoDadosDepositoBancarioADO(MasterADOBase ado = null) : base(ado, false) { }

        public IQueryable<EstornoDadosDepositoBancario> Listar()
        {
            var queryStr = @"SELECT
                            est.*,
                            vbCancel.Senha ""SenhaCancelamento"",
                            vbVenda.Senha ""SenhaCompra"" 
                            FROM EstornoDadosDepositoBancario est  ( NOLOCK )
                            LEFT JOIN tVendaBilheteria vbCancel WITH ( FORCESEEK, NOLOCK ) ON vbCancel.ID = est.VendaBilheteriaIDCancel
                            LEFT JOIN tVendaBilheteria vbVenda WITH ( FORCESEEK, NOLOCK ) ON vbVenda.ID = est.VendaBilheteriaIDVenda
                            WHERE est.Status IN ('P', 'F')
                            ORDER BY est.Status";

            var query = conIngresso.Query<EstornoDadosDepositoBancario>(queryStr);

            return query.ToList().AsQueryable();
        }

        public EstornoDadosDepositoBancario Consultar(int estornoID)
        {
            var queryStr = @"select est.*, vbCancel.Senha ""SenhaCancelamento"", vbVenda.Senha ""SenhaCompra"" from		EstornoDadosDepositoBancario est
				inner join tVendaBilheteria vbCancel on vbCancel.ID = est.VendaBilheteriaIDCancel
				inner join tVendaBilheteria vbVenda on vbVenda.ID = est.VendaBilheteriaIDVenda
				WHERE est.ID = @ID";

            var query = conIngresso.Query<EstornoDadosDepositoBancario>(queryStr, new { id = estornoID });

            return query.ToList().FirstOrDefault();
        }

        public List<DateTime?> getDatas()
        {
            return (from item in dbIngresso.EstornoDadosDepositoBancario
                    select item.DataEnvio).Distinct().ToList();
        }
    }
}
