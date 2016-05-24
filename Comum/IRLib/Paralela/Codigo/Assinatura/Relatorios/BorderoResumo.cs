using CTLib;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class BorderoResumo
    {

        BD bd;

        public BorderoResumo()
        {
            bd = new BD();
        }

        public IRLib.Paralela.Assinaturas.Models.BorderoResumo BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();
                var retorno = new Models.BorderoResumo();

                int apresentacao = oAssinatura.getApresentacao(Assinaturas, Temporadas);
                oAssinatura.CarregarNome(Assinaturas);

                retorno.Assinatura = oAssinatura.Nome.Valor;
                retorno.Local = oAssinatura.GetLocal(Assinaturas);
                


                string sql = @"SELECT
	                        s.ID as SetorID , s.Nome AS Setor, pt.Nome AS Preco, SUM(p.Valor) AS Faturamento, COUNT(DISTINCT ac.ID) AS Quantidade
	                        FROM tAssinaturaCliente ac (NOLOCK)
	                        INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = ac.VendaBilheteriaID
	                        INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
	                        INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
	                        INNER JOIN tAssinatura a (NOLOCK) ON ac.AssinaturaID = a.ID
	                        INNER JOIN tAssinaturaAno an (NOLOCK) ON ac.AssinaturaAnoID = an.ID
	                        INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
	                        LEFT JOIN tPrecoTipo pt (NOLOCK) ON pt.ID = ac.PrecoTipoID
	                        WHERE a.ID = " + Assinaturas + @" AND (ac.Acao = 'R' OR ac.Acao = 'E' OR ac.Acao = 'N') 
	                        GROUP BY a.Nome,s.ID, s.Nome, pt.Nome
	                        ORDER BY a.Nome, s.Nome, pt.Nome 


                            SELECT
                                s.ID as SetorID,
		                        s.Nome AS Setor,
		                        COUNT(i.ID) AS Lotacao, 
				                        SUM(CASE WHEN ac.ID IS NOT NULL AND i.CortesiaID <= 0
					                        THEN 
						                        CASE WHEN ac.Acao IN ('R', 'E', 'N')
						                        THEN 1 
						                        ELSE 0
						                        END
					                        ELSE
						                        0
					                        END) 
			                         AS Pagantes,
				                        SUM(CASE WHEN ac.ID IS NOT NULL AND i.CortesiaID > 0
					                        THEN 
						                        CASE WHEN ac.Acao IN ('R', 'E', 'N')
						                        THEN 1 
						                        ELSE 0
						                        END
					                        ELSE
						                        0
					                        END) 
			                        AS Cortesia
	                        FROM tIngresso i (NOLOCK)
	                        INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
	                        LEFT JOIN tAssinaturaCliente ac (NOLOCK) ON ac.ID = i.AssinaturaClienteID
	                        WHERE i.ApresentacaoID = " + apresentacao + @"
	                        GROUP BY s.ID,s.Nome
	                        ORDER BY s.ID,s.Nome";

                bd.Consulta(sql);


                List<int> auxSetorID = new List<int>();

                while (bd.Consulta().Read())
                {

                    int setorID = bd.LerInt("SetorID");
                    string setor = bd.LerString("Setor");
                    string preco = bd.LerString("Preco");
                    decimal faturamento = bd.LerDecimal("Faturamento");
                    int quantidade = bd.LerInt("Quantidade");


                    retorno.ListaPagantes.Add(new IRLib.Paralela.Assinaturas.Models.EstruturaPagantes
                    {
                        SetorID = setorID,
                        Setor = setor,
                        Preco = preco,
                        Faturamento = faturamento,
                        Quantidade = quantidade,
                    });

                    if (auxSetorID.Contains(setorID))
                    {
                        retorno.ListaPagantesSub.FirstOrDefault(c => c.SetorID == setorID).Quantidade += quantidade;
                        retorno.ListaPagantesSub.FirstOrDefault(c => c.SetorID == setorID).Faturamento += faturamento;
                    }
                    else
                    {
                        auxSetorID.Add(setorID);
                        retorno.ListaPagantesSub.Add(new IRLib.Paralela.Assinaturas.Models.EstruturaPagantesSubtotal
                        {
                            SetorID = setorID,
                            Quantidade = quantidade,
                            Faturamento = faturamento,
                        });
                    }

                    retorno.TotalPagantes.Quantidade += quantidade;
                    retorno.TotalPagantes.Faturamento += faturamento;
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {
                    retorno.ListaEstatisticaSetor.Add(new IRLib.Paralela.Assinaturas.Models.EstruturaEstatisticaSetor
                    {
                        SetorID = bd.LerInt("SetorID"),
                        Setor = bd.LerString("Setor"),
                        Lotacao = bd.LerInt("Lotacao"),
                        Pagantes = bd.LerInt("Pagantes"),
                        Cortesia = bd.LerInt("Cortesia"),
                    });
                }

                return retorno;

            }
            finally
            {
                bd.Fechar();
            }
        }


    }
}
