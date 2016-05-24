using CTLib;
using IRLib.Paralela.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class CrieSuaSerie
    {
        BD bd = new BD();
        public List<AssinanteAssinatura> RelacaoVendas(int assinaturaTipoID, int temporada, int assinaturaID)
        {
            try
            {
                string filtroTemporada = temporada == 0 ? string.Empty : " AND an.Ano = '" + temporada + "' ";
                string filtroAssinatura = assinaturaID == 0 ? string.Empty : " AND a.ID = " + assinaturaID + " ";

                string sql = @"SELECT 
	                                DISTINCT c.LoginOSESP, c.Nome, c.CPF, c.RG, ap.Horario, a.Nome AS Assinatura, s.Nome AS Setor, l.Codigo,
                                    pr.Nome as Preco, pr.Valor, fp.Nome as FormaPagamento
	                            FROM tVendaBilheteria vb (NOLOCK)
	                            INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
	                            INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
	                            INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
	                            INNER JOIN tLugar l (NOLOCK) ON l.ID = i.LugarID
	                            INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
	                            INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.ApresentacaoID = i.ApresentacaoID AND ai.SetorID = i.SetorID
	                            INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ai.AssinaturaAnoID
	                            INNER JOIN tAssinatura a (NOLOCK) ON a.ID = an.AssinaturaID
                                INNER JOIN tPreco (NOLOCK) pr ON i.PrecoID = pr.ID
                                INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) vfp ON vb.ID = vfp.VendaBilheteriaID
                                INNER JOIN tFormaPagamento (NOLOCK) fp ON vfp.FormaPagamentoID = fp.ID
	                            WHERE vb.DataVenda > '20120101000000' AND i.SerieID > 0 AND a.AssinaturaTipoID = " + assinaturaTipoID + filtroTemporada + filtroAssinatura + " ORDER BY c.Nome, ap.Horario, s.Nome, l.Codigo";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem registros de venda do crie sua série para o filtro selecionado.");

                List<AssinanteAssinatura> lista = new List<AssinanteAssinatura>();

                do
                {
                    lista.Add(new AssinanteAssinatura()
                    {
                        Login = bd.LerString("LoginOSESP"),
                        Nome = bd.LerString("Nome"),
                        CPF = bd.LerString("CPF"),
                        DataAcao = bd.LerDateTime("Horario").ToString("dd/MM/yyyy à\\s HH:mm"),
                        Assinatura = bd.LerString("Assinatura"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Codigo"),
                        Preco = bd.LerString("Preco"),
                        ValorPreco = bd.LerString("Valor"),
                        FormaPagamento = bd.LerString("FormaPagamento")
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
