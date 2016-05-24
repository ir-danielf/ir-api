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
    public class IngressoLogADO : MasterADO<dbIngresso>
    {
        public IngressoLogADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public List<tIngressoLog> ConsultarAnulacoesDeImpressao(int vendaBilheteriaID)
        {
            string strSql = @"
                        SELECT 
	                        ID,IngressoID,UsuarioID,TimeStamp,Acao,PrecoID,CortesiaID,BloqueioID,VendaBilheteriaItemID,Obs,EmpresaID,VendaBilheteriaID,CaixaID,LojaID,CanalID,ClienteID,CodigoBarra,CodigoImpressao,MotivoId,SupervisorID,GerenciamentoIngressosID,AssinaturaClienteID
                        FROM 
	                        tIngressoLog
                        WHERE
	                        Acao = 'A' AND VendaBilheteriaID = @VendaBilheteriaID";
            return conIngresso.Query<tIngressoLog>(strSql, new { VendaBilheteriaID = vendaBilheteriaID }).ToList();
        }
    }
}
