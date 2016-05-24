using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace IRCore.DataAccess.ADO
{
    public class PlanilhaADO : MasterADO<dbIngresso>
    {
        public PlanilhaADO(MasterADOBase ado = null) : base(ado, false) { }

        public bool Salvar(tVendaBilheteriaEntrega tVendaBilheteriaEntrega)
        {
            string sql = @"
                           INSERT INTO 
                                tVendaBilheteriaEntrega (VendaBilheteriaID, Tipo, DataHoraOcorrencia, EmailEnviado, CodigoRastreamento, StatusTexto)
                           VALUES 
                                (@VendaBilheteriaID, @Tipo, @DataHoraOcorrencia, @EmailEnviado, @CodigoRastreamento, @StatusTexto)";
            return conIngresso.Execute(sql, tVendaBilheteriaEntrega) > 0;
        }

        public int BuscaSedex(string CodigoRastreamento)
        {
            string sql = @"SELECT TOP 1 ID FROM tVendaBilheteriaEntrega 
                            WHERE CodigoRastreamento = @CodigoRastreamento ;";

            var query = conIngresso.Query<int>(sql, new { CodigoRastreamento = CodigoRastreamento });

            return query.FirstOrDefault();
        }

        public int BuscaFlash(string Tipo, DateTime DataHoraOcorrencia, string StatusTexto)
        {
            string sql = @"SELECT TOP 1 ID FROM tVendaBilheteriaEntrega 
                            WHERE Tipo = @Tipo 
                            AND DataHoraOcorrencia = @DataHoraOcorrencia 
                            AND StatusTexto = @StatusTexto ;";

            var query = conIngresso.Query<int>(sql, new { Tipo = Tipo, DataHoraOcorrencia = DataHoraOcorrencia, StatusTexto = StatusTexto });

            return query.FirstOrDefault();
        }
    }
}
