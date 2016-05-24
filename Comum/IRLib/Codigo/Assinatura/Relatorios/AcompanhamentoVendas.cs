using CTLib;
using IRLib.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;

namespace IRLib.Assinaturas.Relatorios
{
    public class AcompanhamentoVendas
    {
        private BD bd { get; set; }

        public List<AcompanhamentoVenda> Buscar(int AssinaturaTipoID, int Temporadas, int Assinaturas)
        {
            try
            {
                List<AcompanhamentoVenda> lista = new List<AcompanhamentoVenda>();
                bd = new BD();

                string temporada = "AND an.Ano = '" + Temporadas + "'";
                string assinatura = Assinaturas > 0 ? "AND a.ID = " + Assinaturas : string.Empty;



                bd.Consulta(string.Format(@"
						SELECT
							DISTINCT ai.ApresentacaoID
							into #tmpApresentacoes
							FROM tAssinaturaTipo at (NOLOCK)
							INNER JOIN tAssinatura a (NOLOCK) ON a.AssinaturaTipoID = at.ID
							INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
							INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
							WHERE at.ID = {0} {1} {2}


						
						SELECT DISTINCT CASE WHEN ts.Nome IS NULL
															THEN '-' END AS Nome,
								 e.Nome AS Evento, ap.ID, ap.Horario, 
								 SUM(CASE WHEN i.Status = 'D'
											THEN 1 ELSE 0 END) AS Disponivel,
								 SUM(CASE WHEN i.Status IN ('V', 'I', 'E') AND i.AssinaturaClienteID > 0
											THEN 1 ELSE 0 END) AS Assinatura,			
								 SUM(CASE WHEN i.Status IN ('V', 'I', 'E') AND i.CortesiaID > 0
											THEN 1 ELSE 0 END) AS Cortesia,		
								 SUM(CASE WHEN i.Status IN ('V', 'I', 'E') AND i.SerieID > 0 AND i.AssinaturaClienteID = 0 AND i.CortesiaID = 0
											THEN 1 ELSE 0 END) AS Serie,		
								 SUM(CASE WHEN i.Status = 'B'
											THEN 1 ELSE 0 END) AS Bloqueado,
								 SUM(CASE WHEN i.Status IN ('V', 'I', 'E') AND i.AssinaturaClienteID = 0 AND (i.SerieID IS NULL OR i.SerieID = 0) AND i.CortesiaID = 0
										THEN 1 ELSE 0 END) AS Avulso
								 
							FROM tIngresso i (NOLOCK)
							INNER JOIN #tmpApresentacoes tap ON tap.ApresentacaoID = i.ApresentacaoID
							INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
							INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
                            LEFT JOIN dbo.tSerie ts (NOLOCK) ON i.SerieID = ts.ID
                            WHERE i.SetorID <> 696
							
                            GROUP BY e.Nome, ap.ID, ap.Horario,ts.Nome
                           
							ORDER BY ap.Horario

							", AssinaturaTipoID, temporada, assinatura));

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem registros para serem montados.");

               

                do
                {
                    //if (atual.Horario != bd.LerDateTime("Horario"))
                    //{
                    //    if (atual.Horario != DateTime.MinValue)
                    //        lista.Add(atual);

                    //    atual = new AcompanhamentoVenda()
                    //    {
                    //        Horario = bd.LerDateTime("Horario"),
                    //        Evento = bd.LerString("Evento"),
                    //    };
                    //}

                    //if (bd.LerString("Status") == Ingresso.DISPONIVEL)
                    //    atual.Disponivel++;
                    //else if (bd.LerInt("AssinaturaClienteID") > 0)
                    //    atual.Assinatura++;
                    //else if (bd.LerInt("CortesiaID") > 0)
                    //    atual.Cortesia++;
                    //else if (bd.LerInt("SerieID") > 0)
                    //    atual.Serie++;
                    //else if (bd.LerString("Status") == Ingresso.BLOQUEADO)
                    //    atual.Bloqueado++;
                    //else
                    //    atual.Avulso++;


                    lista.Add(new AcompanhamentoVenda()
                    {
                        Horario = bd.LerDateTime("Horario"),
                        Evento = bd.LerString("Evento"),
                        Disponivel = bd.LerInt("Disponivel"),
                        Assinatura = bd.LerInt("Assinatura"),
                        Avulso = bd.LerInt("Avulso"),
                        Bloqueado = bd.LerInt("Bloqueado"),
                        Cortesia = bd.LerInt("Cortesia"),
                        Serie = bd.LerInt("Serie"),
                        NomeSerie = bd.LerString("Nome")
                    });

                } while (bd.Consulta().Read());               

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}
