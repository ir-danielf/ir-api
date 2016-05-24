using CTLib;
using System.Windows.Forms;
using System.IO;

namespace IRLib 
{
    public class VerificarPermissaoTrocaCancelamento : BaseBD
    {
        BD bd = new BD();

        public bool VerificarSeIngressoBilheteiroECaixaAberto(int idusuario, int idVendaBilheteria)
        {
            var retorno = 0;

            try
            {
                var sql = @"SELECT 
	                            1
                            FROM 
	                            TCAIXA C WITH(NOLOCK) 
	                            INNER JOIN TVENDABILHETERIA VB WITH(NOLOCK) 
		                            ON C.ID = VB.CAIXAID 
                            WHERE 
	                            VB.ID = {1}
                                AND C.USUARIOID = {0}
	                            AND C.DATAFECHAMENTO = ''";

                sql = string.Format(sql, idusuario, idVendaBilheteria);

                retorno = bd.ExecutarScalar(sql);
            }
            finally
            {
                bd.Fechar();
            }

            if (retorno == 1)
                return true;

            return false;
        }

        public bool VerificarSeIngressoSupervisorCanal(int idusuarioSupervisor, int idVendaBilheteria)
        {
            var retorno = 0;

            try
            {
                var sql = @"SELECT 
	                            1
                            FROM 
	                            TVENDABILHETERIA VB WITH(NOLOCK)
	                            INNER JOIN TCAIXA C WITH(NOLOCK)
		                            ON VB.CAIXAID = C.ID
	                            INNER JOIN TLOJA L WITH(NOLOCK)
		                            ON C.LOJAID = L.ID
	                            INNER JOIN TPERFILCANAL PC WITH(NOLOCK)
		                            ON L.CANALID = PC.CANALID
                            WHERE 
	                            VB.ID = {1}
	                            AND PC.USUARIOID = {0}
	                            AND PC.PERFILID = 7";

                sql = string.Format(sql, idusuarioSupervisor, idVendaBilheteria);

                retorno = bd.ExecutarScalar(sql);
            }
            finally
            {
                bd.Fechar();
            }

            if (retorno == 1)
                return true;

            return false;
        }
        public bool VerificarSeIngressoSupervisorLocal(int idusuarioSupervisor, int idVendaBilheteria)
        {
            var retorno = 0;

            try
            {
//                var sql = @"SELECT 
//	                            1
//                            FROM 
//	                            TVENDABILHETERIA VB WITH(NOLOCK)
//	                            INNER JOIN TCAIXA C WITH(NOLOCK) ON VB.CAIXAID = C.ID
//	                            INNER JOIN TLOJA L WITH(NOLOCK) ON C.LOJAID = L.ID
//	                            INNER JOIN TCANAL CN WITH(NOLOCK) ON CN.ID = L.CANALID
//	                            INNER JOIN TLOCAL LC WITH(NOLOCK) ON LC.EMPRESAID = CN.EMPRESAID
//	                            INNER JOIN TPERFILLOCAL PL WITH(NOLOCK) ON PL.LOCALID = LC.ID
//	                            INNER JOIN TPERFILCANAL PC WITH(NOLOCK) ON PC.CANALID = L.CANALID 
//                            WHERE VB.ID = {1}
//	                            AND PC.PERFILID = 7
//	                            AND PC.USUARIOID = {0}
//	                            AND PL.USUARIOID = {0}";

                var sql = @"SELECT 
	                            1
                            FROM 
	                            TVENDABILHETERIA VB WITH(NOLOCK)
	                            INNER JOIN TCAIXA C WITH(NOLOCK) ON VB.CAIXAID = C.ID
	                            INNER JOIN TLOJA L WITH(NOLOCK) ON C.LOJAID = L.ID
	                            INNER JOIN TCANAL CN WITH(NOLOCK) ON CN.ID = L.CANALID
	                            INNER JOIN TLOCAL LC WITH(NOLOCK) ON LC.EMPRESAID = CN.EMPRESAID
	                            INNER JOIN TPERFILLOCAL PL WITH(NOLOCK) ON PL.LOCALID = LC.ID
	                         
                            WHERE VB.ID = {1}
	                            AND PL.PERFILID = 16
	                            AND PL.USUARIOID = {0}";

                sql = string.Format(sql, idusuarioSupervisor, idVendaBilheteria);

                retorno = bd.ExecutarScalar(sql);
            }
            finally
            {
                bd.Fechar();
            }

            if (retorno == 1)
                return true;

            return false;
        }

        public override bool Atualizar()
         {
             throw new System.NotImplementedException();
         }
 
         public override void Desfazer()
         {
             throw new System.NotImplementedException();
         }
 
         public override bool Excluir(int id)
         {
             throw new System.NotImplementedException();
         }
         public override bool Inserir()
         {
             throw new System.NotImplementedException();
         }
 
         public override void Ler(int id)
         {
             throw new System.NotImplementedException();
         }
 
         public override void Limpar()
         {
             throw new System.NotImplementedException();
         }

    }
}