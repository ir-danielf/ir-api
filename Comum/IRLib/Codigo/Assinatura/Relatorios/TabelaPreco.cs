using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Assinaturas.Relatorios
{
    public class TabelaPreco
    {
        const string SERIES = "Séries";
        const string TOTAL = "Total";
        BD bd;

        public TabelaPreco()
        {
            bd = new BD();
        }

        public List<TabelaPrecoVisu> BuscarRelatorio(int AssinaturaTipoID, string ano)
        {
            try
            {

                IRLib.Assinatura oAssinatura = new Assinatura();
                List<Models.TabelaPreco> oTotais = new List<Models.TabelaPreco>();
                List<Models.TabelaPreco> oTotaisValores = new List<Models.TabelaPreco>();
                TabelaPrecoVisu oTabelaPreco = new TabelaPrecoVisu();
                TabelaPrecoVisu oTabelaQuantidade = new TabelaPrecoVisu();
                TabelaPrecoVisu oTabelaSoma = new TabelaPrecoVisu();
                List<TabelaPrecoVisu> listaRetorno = new List<TabelaPrecoVisu>();


                string sqlBusca =
                   string.Format(@"SELECT Assinatura,Setor,Preco as PrecoTipo,Valor as Preco 
                                   from vwAssinaturaPagamento as ap
                                   inner join tassinaturaano(nolock) as aa on ap.AssinaturaAnoID = aa.ID
                                   where AssinaturaTipoID = {0} {1} 
                                   order by Assinatura,Setor,Preco
                                   							   
                                   
                                   
                                   select Count(Distinct ac.ID) as Quantidade, dbo.NumeroFormatado(sum(vwa.Valor)) as Soma,vwa.Assinatura,vwa.Setor,vwa.Preco as PrecoTipo
                                   from tAssinaturaCliente ac(nolock)
                                   inner join vwAssinaturaPagamento vwa on vwa.SetorID = ac.SetorID AND vwa.AssinaturaID = ac.AssinaturaID AND vwa.PrecoTipoID = ac.PrecoTipoID
                                   INNER JOIN tAssinatura a (NOLOCK) ON ac.AssinaturaID = a.ID
                                   inner join tassinaturaano(nolock) as aa on aa.AssinaturaID = a.ID
                                   INNER JOIN tVendaBilheteria tvb ON tvb.ID = ac.VendaBilheteriaID
                                   WHERE a.Ativo = 'T' AND (( Acao = 'R' ) 
                                   OR ( Acao = 'N' AND ac.Status = 'N' ) 
                                   OR ( Acao = 'E' AND ac.Status = 'T' )) And vwa.AssinaturaTipoID = {0} AND tvb.VendaCancelada = 'F' {1}
                                   group by ac.SetorID,ac.AssinaturaID,ac.PrecoTipoID,vwa.Assinatura,vwa.Setor,vwa.Preco
                                   order by vwa.Assinatura,vwa.Setor,PrecoTipo
                                    ", AssinaturaTipoID, (String.IsNullOrEmpty(ano) ? "" : "AND aa.ano = " + ano));

                bd.Consulta(sqlBusca);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar seus ingressos.");

                Models.TabelaPreco aux = new Models.TabelaPreco();
                do
                {
                    aux = new Models.TabelaPreco()
                    {
                        Assinatura = bd.LerString("Assinatura"),
                        PrecoTipo = bd.LerString("PrecoTipo"),
                        Preco = bd.LerDecimal("Preco"),
                        Setor = bd.LerString("Setor")
                    };

                    oTotais.Add(aux);
                } while (bd.Consulta().Read());

                bd.Consulta().NextResult();

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar seus ingressos.");
                do
                {
                    aux = new Models.TabelaPreco()
                    {
                        Assinatura = bd.LerString("Assinatura"),
                        PrecoTipo = bd.LerString("PrecoTipo"),
                        Quantidade = bd.LerInt("Quantidade"),
                        Soma = bd.LerDecimal("Soma"),
                        Setor = bd.LerString("Setor")
                    };

                    oTotaisValores.Add(aux);
                } while (bd.Consulta().Read());



                bd.FecharConsulta();


                List<string> listPrecoTipo = new List<string>();
                listPrecoTipo.AddRange(
                    oTotais.Select(c => c.PrecoTipo).Distinct());
                List<string> listColunas = new List<string>();
                listColunas.AddRange(
                    oTotais.Select(c => c.Setor).Distinct());
                List<string> listLinhas = new List<string>();
                listLinhas.Add(SERIES);
                listLinhas.AddRange(oTotais.Select(c => c.Assinatura).Distinct());

                List<EstruturaIDNome> listTotal = listPrecoTipo.Select(c => new EstruturaIDNome()
                {
                    Nome = c,
                }).Distinct().ToList();

                #region TabelaPreco
                oTabelaPreco.Header.AddRange(
                    listColunas.Select(c => new EstruturaTabelaPrecoVisuHeader()
                    {
                        Coluna = c,
                        Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>(listPrecoTipo.Select(e => new EstruturaTabelaPrecoVisuPrecoTipo()
                        {
                            PrecoTipo = e
                        }).Distinct().ToList())

                    }).Distinct().ToList());

                oTabelaPreco.Linhas.AddRange(
                    listLinhas.Select(c => new EstruturaTabelaPrecoVisuLinha()
                    {
                        Linha = c,
                        Colunas = new List<EstruturaTabelaPrecoVisuHeader>(listColunas.Select(e => new EstruturaTabelaPrecoVisuHeader()
                        {
                            Coluna = e,
                            Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>(listPrecoTipo.Select(d => new EstruturaTabelaPrecoVisuPrecoTipo()
                            {
                                PrecoTipo = d
                            }).Distinct().ToList())
                        }).Distinct().ToList())
                    }).Distinct().ToList());



                foreach (var linhas in oTabelaPreco.Linhas)
                {

                    foreach (var col in linhas.Colunas)
                    {
                        foreach (var preco in col.Preco)
                        {
                            if (linhas.Linha.Equals(SERIES))
                            {
                                preco.Valor = preco.PrecoTipo;
                            }
                            else
                            {
                                decimal valor = oTotais.Where(c => c.PrecoTipo == preco.PrecoTipo && c.Setor == col.Coluna && c.Assinatura == linhas.Linha).Sum(c => c.Preco);
                                preco.Valor = valor.ToString("c");
                            }

                        }

                    }
                }

                listaRetorno.Add(oTabelaPreco);
                #endregion TabelaPreco

                listColunas.Add(TOTAL);

                #region TabelaQuantidade
                oTabelaQuantidade.Header.AddRange(
                    listColunas.Select(c => new EstruturaTabelaPrecoVisuHeader()
                    {
                        Coluna = c,
                        Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>(listPrecoTipo.Select(e => new EstruturaTabelaPrecoVisuPrecoTipo()
                        {
                            PrecoTipo = e
                        }).Distinct().ToList())

                    }).Distinct().ToList());

                oTabelaQuantidade.Linhas.AddRange(
                    listLinhas.Select(c => new EstruturaTabelaPrecoVisuLinha()
                    {
                        Linha = c,
                        Colunas = new List<EstruturaTabelaPrecoVisuHeader>(listColunas.Select(e => new EstruturaTabelaPrecoVisuHeader()
                        {
                            Coluna = e,
                            Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>(listPrecoTipo.Select(d => new EstruturaTabelaPrecoVisuPrecoTipo()
                            {
                                PrecoTipo = d
                            }).Distinct().ToList())
                        }).Distinct().ToList())
                    }).Distinct().ToList());



                foreach (var linhas in oTabelaQuantidade.Linhas)
                {

                    foreach (var col in linhas.Colunas)
                    {

                        foreach (var preco in col.Preco)
                        {
                            if (linhas.Linha.Equals(SERIES))
                                preco.Valor = preco.PrecoTipo;
                            else if (col.Coluna.Equals(TOTAL))
                                preco.Valor = oTotaisValores.Where(c => c.PrecoTipo == preco.PrecoTipo && c.Assinatura == linhas.Linha).Sum(c => c.Quantidade).ToString();
                            else
                                preco.Valor = oTotaisValores.Where(c => c.PrecoTipo == preco.PrecoTipo && c.Setor == col.Coluna && c.Assinatura == linhas.Linha).Sum(c => c.Quantidade).ToString();
                        }

                    }
                }

                listaRetorno.Add(oTabelaQuantidade);
                #endregion TabelaQuantidade

                #region TabelaSoma
                oTabelaSoma.Header.AddRange(
                    listColunas.Select(c => new EstruturaTabelaPrecoVisuHeader()
                    {
                        Coluna = c,
                        Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>(listPrecoTipo.Select(e => new EstruturaTabelaPrecoVisuPrecoTipo()
                        {
                            PrecoTipo = e
                        }).Distinct().ToList())

                    }).Distinct().ToList());

                oTabelaSoma.Linhas.AddRange(
                    listLinhas.Select(c => new EstruturaTabelaPrecoVisuLinha()
                    {
                        Linha = c,
                        Colunas = new List<EstruturaTabelaPrecoVisuHeader>(listColunas.Select(e => new EstruturaTabelaPrecoVisuHeader()
                        {
                            Coluna = e,
                            Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>(listPrecoTipo.Select(d => new EstruturaTabelaPrecoVisuPrecoTipo()
                            {
                                PrecoTipo = d
                            }).Distinct().ToList())
                        }).Distinct().ToList())
                    }).Distinct().ToList());



                foreach (var linhas in oTabelaSoma.Linhas)
                {

                    foreach (var col in linhas.Colunas)
                    {
                        foreach (var preco in col.Preco)
                        {
                            if (linhas.Linha.Equals(SERIES))
                                preco.Valor = preco.PrecoTipo;
                            else if (col.Coluna == TOTAL)
                                preco.Valor = oTotaisValores.Where(c => c.PrecoTipo == preco.PrecoTipo && c.Assinatura == linhas.Linha).Sum(c => c.Soma).ToString("c");
                            else
                                preco.Valor = oTotaisValores.Where(c => c.PrecoTipo == preco.PrecoTipo && c.Setor == col.Coluna && c.Assinatura == linhas.Linha).Sum(c => c.Soma).ToString("c");
                        }

                    }
                }
                listaRetorno.Add(oTabelaSoma);
                #endregion TabelaSoma

                return listaRetorno;
            }
            finally
            {
                bd.Fechar();
            }

        }
    }


    public class TabelaPrecoVisu
    {
        public List<EstruturaTabelaPrecoVisuHeader> Header = new List<EstruturaTabelaPrecoVisuHeader>();
        public List<EstruturaTabelaPrecoVisuLinha> Linhas = new List<EstruturaTabelaPrecoVisuLinha>();
    }


    public class EstruturaTabelaPrecoVisuPrecoTipo
    {
        public string PrecoTipo;
        public string Valor;
    }

    public class EstruturaTabelaPrecoVisuHeader
    {
        public string Coluna;
        public List<EstruturaTabelaPrecoVisuPrecoTipo> Preco = new List<EstruturaTabelaPrecoVisuPrecoTipo>();
    }


    public class EstruturaTabelaPrecoVisuLinha
    {
        public string Linha { get; set; }
        public List<EstruturaTabelaPrecoVisuHeader> Colunas = new List<EstruturaTabelaPrecoVisuHeader>();
    }

}
