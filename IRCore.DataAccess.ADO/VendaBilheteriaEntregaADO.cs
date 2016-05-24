using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class VendaBilheteriaEntregaADO : MasterADO<dbIngresso>
    {
        public VendaBilheteriaEntregaADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public bool Cadastrar(tVendaBilheteriaEntrega vendaBilheteriaEntrega)
        {
            string sql = @"
                           INSERT INTO 
                                tVendaBilheteriaEntrega (VendaBilheteriaID,Tipo,DataHoraOcorrencia,EmailEnviado,CodigoRastreamento,StatusTexto)
                           VALUES 
                                (@VendaBilheteriaID, @Tipo, @DataHoraOcorrencia, @EmailEnviado, @CodigoRastreamento, @StatusTexto)";
            return conIngresso.Execute(sql, vendaBilheteriaEntrega) > 0;
        }

        public tVendaBilheteriaEntrega Carregar(int id)
        {
            string sql = @"
                           SELECT
	                           ID,VendaBilheteriaID,Tipo,DataHoraOcorrencia,EmailEnviado,CodigoRastreamento,StatusTexto
                           FROM
	                           tVendaBilheteriaEntrega (NOLOCK)
                           WHERE
                               id = @ID";
            return conIngresso.Query<tVendaBilheteriaEntrega>(sql, new { ID = id }).FirstOrDefault();
        }

        public List<tVendaBilheteriaEntrega> Consultar(int vendaBilheteriaID)
        {
            string sql = @"
                           SELECT
	                           ID,VendaBilheteriaID,Tipo,DataHoraOcorrencia,EmailEnviado,CodigoRastreamento,StatusTexto
                           FROM
	                           tVendaBilheteriaEntrega (NOLOCK)
                           WHERE
                               VendaBilheteriaID = @VendaBilheteriaID";
            return conIngresso.Query<tVendaBilheteriaEntrega>(sql, new { VendaBilheteriaID = vendaBilheteriaID }).ToList();
        }

        public List<tVendaBilheteriaEntrega> ConsultarManipulacao(int vendaBilheteriaID)
        {
            string sql = @"
                           SELECT
	                           ID,VendaBilheteriaID,Tipo,DataHoraOcorrencia,EmailEnviado,CodigoRastreamento,StatusTexto
                           FROM
	                           tVendaBilheteriaEntrega (NOLOCK)
                           WHERE
                               VendaBilheteriaID = @VendaBilheteriaID AND Tipo IN ('X','L')";
            return conIngresso.Query<tVendaBilheteriaEntrega>(sql, new { VendaBilheteriaID = vendaBilheteriaID }).ToList();
        }
    }
}
