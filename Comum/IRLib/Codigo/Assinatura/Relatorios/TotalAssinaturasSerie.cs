using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Assinaturas.Relatorios
{
    public class TotalAssinaturasSerie
    {

        BD bd;

        public TotalAssinaturasSerie()
        {
            bd = new BD();
        }

        public TabelaTotalAssinaturasSerie BuscarRelatorio(int AssinaturaTipoID, string ano)
        {
            try
            {
                IRLib.Assinatura oAssinatura = new Assinatura();
                List<Models.TotalAssinaturasSerie> oTotais = new List<Models.TotalAssinaturasSerie>();

                #region old
                string sqlBusca =
                   string.Format(@"Exec sp_getRelatorioTotalAssinaturasSerie2  {0} , {1}", AssinaturaTipoID, ano);

                bd.Consulta(sqlBusca);
                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar seus ingressos.");

                Models.TotalAssinaturasSerie aux = new Models.TotalAssinaturasSerie();
                do
                {
                    aux = new Models.TotalAssinaturasSerie()
                    {
                        Assinatura = bd.LerString("Assinatura"),
                        Quantidade = bd.LerInt("Qtd"),
                        Status = bd.LerString("StatusIngresso") == "" && bd.LerString("StatusAssinatura") == "" && bd.LerString("AcaoAssinatura") == "" ? IRLib.Assinatura.EnumStatusVisual.Indisponivel : oAssinatura.VerificaStatusVisual(bd.LerString("StatusIngresso"), bd.LerString("StatusAssinatura"), bd.LerString("AcaoAssinatura"), bd.LerInt("IngressoBloqueioID"), bd.LerInt("AssinaturaBloqueioID"), bd.LerInt("AssinaturaExtintoBloqueioID"), bd.LerInt("AssinaturaDesistenciaBloqueioID")),
                        Bloqueio = bd.LerString("BloqueioUtilizado"),
                        BloqueioID = bd.LerInt("IngressoBloqueioID")
                    };

                    oTotais.Add(aux);
                } while (bd.Consulta().Read());


                bd.FecharConsulta();
                #endregion old

                TabelaTotalAssinaturasSerie oTabelaTotalAssinaturasSerie = new TabelaTotalAssinaturasSerie();

                List<string> listColunas = new List<string>();

                listColunas.AddRange(
                    oTotais.Select(c => c.Assinatura).Distinct());

                oTabelaTotalAssinaturasSerie.Header.AddRange(
                    listColunas.Select(c => new EstruturaTotalAssinaturasSerieHeader()
                    {
                        Coluna = c
                    }).Distinct().ToList());

                List<EstruturaIDNome> listLinhas = new List<EstruturaIDNome>();

                var naoBloqueados = oTotais.Where(c => c.Status != Assinatura.EnumStatusVisual.Bloqueado).Select(c => new EstruturaIDNome()
                    {
                        Nome = c.StatusDescricao
                    }).Distinct();

                List<string> _colunas = new List<string>();
                foreach (var item in naoBloqueados)
                {
                    if (!_colunas.Contains(item.Nome))
                    {
                        _colunas.Add(item.Nome);
                        listLinhas.Add(item);
                    }
                }

                var bloqueados = oTotais.Where(c => c.Status == Assinatura.EnumStatusVisual.Bloqueado).Select(c => new EstruturaIDNome()
                    {
                        ID = c.BloqueioID,
                        Nome = c.Bloqueio
                    }).Distinct();

                foreach (var item in bloqueados)
                {
                    if (!_colunas.Contains(item.Nome))
                    {
                        _colunas.Add(item.Nome);
                        listLinhas.Add(item);
                    }
                }

                oTabelaTotalAssinaturasSerie.Linhas.AddRange(
                    listLinhas.Select(c => new EstruturaTotalAssinaturasSerieLinha()
                    {
                        BloqueioID = c.ID,
                        Linha = c.Nome,
                        Colunas = new List<EstruturaTotalAssinaturasSerieHeader>(listColunas.Select(e => new EstruturaTotalAssinaturasSerieHeader()
                        {
                            Coluna = e
                        }).Distinct().ToList())
                    }).Distinct().ToList());

                foreach (var linhas in oTabelaTotalAssinaturasSerie.Linhas)
                {
                    foreach (var col in linhas.Colunas)
                    {
                        string valor = oTotais.Where(c => c.Status != Assinatura.EnumStatusVisual.Bloqueado && c.Assinatura == col.Coluna && c.StatusDescricao.ToString() == linhas.Linha).Sum(c => c.Quantidade).ToString();
                        if (valor == null || valor.Length <= 0 || valor.Equals("0"))
                        {
                            valor = oTotais.Where(c => c.Status == Assinatura.EnumStatusVisual.Bloqueado && c.Assinatura == col.Coluna && c.Bloqueio == linhas.Linha && c.BloqueioID == linhas.BloqueioID).Select(c => c.Quantidade.ToString()).FirstOrDefault();
                        }
                        if (valor == null || valor.Length <= 0 || valor.Equals("0"))
                        {
                            valor = "0";
                        }

                        col.Valor = valor;
                    }
                }

                return oTabelaTotalAssinaturasSerie;
            }
            finally
            {
                bd.Fechar();
            }

        }
    }


    public class TabelaTotalAssinaturasSerie
    {
        public List<EstruturaTotalAssinaturasSerieHeader> Header = new List<EstruturaTotalAssinaturasSerieHeader>();
        public List<EstruturaTotalAssinaturasSerieLinha> Linhas = new List<EstruturaTotalAssinaturasSerieLinha>();
    }


    public class EstruturaTotalAssinaturasSerieHeader
    {
        public string Coluna;
        public string Valor;
    }


    public class EstruturaTotalAssinaturasSerieLinha
    {
        public int BloqueioID { get; set; }
        public string Linha { get; set; }
        public List<EstruturaTotalAssinaturasSerieHeader> Colunas = new List<EstruturaTotalAssinaturasSerieHeader>();
    }
}
