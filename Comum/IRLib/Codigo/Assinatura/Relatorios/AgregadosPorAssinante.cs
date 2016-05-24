using CTLib;
using IRLib.Assinaturas.Models.Relatorios;
using System.Collections.Generic;


namespace IRLib.Assinaturas.Relatorios
{
    public class AgregadosPorAssinante
    {
        public List<Agregado> BuscarAgregados(int assinaturaTipoID, string ano = null)
        {
            BD bd = new BD();
            try
            {
                string sql = string.Format(@"SELECT 
		                            DISTINCT c.LoginOSESP, g.Nome, g.GrauParentesco, g.DataNascimento, g.Profissao, sp.Situacao AS SituacaoProfissional
	                            FROM tAssinaturaCliente ac (NOLOCK)
                                INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
	                            INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
	                            INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID
	                            INNER JOIN tAgregados g (NOLOCK) ON g.ClienteID = c.ID
	                            INNER JOIN tSituacaoProfissional sp (NOLOCK) ON sp.ID = g.SituacaoProfissionalID
	                            WHERE a.AssinaturaTipoID = {0} AND ac.Acao IN ('R', 'E', 'N') {1}
	                            ORDER BY c.LoginOSESP, g.Nome", assinaturaTipoID, (string.IsNullOrEmpty(ano)?"":" AND an.ano = "+ano));

                bd.Consulta(sql);

                List<Agregado> lista = new List<Agregado>();
                while (bd.Consulta().Read())
                    lista.Add(new Agregado()
                    {
                        Login = bd.LerString("LoginOSESP"),
                        Nome = bd.LerString("Nome"),
                        GrauParentesco = bd.LerInt("GrauParentesco"),
                        DataNascimento = bd.LerDateTime("DataNascimento"),
                        Profissao = bd.LerString("Profissao"),
                        SituacaoProfissional = bd.LerString("SituacaoProfissional"),
                    });

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }
}
