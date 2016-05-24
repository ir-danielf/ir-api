using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class CaixaResumoAssinatura
    {

        BD bd;

        public CaixaResumoAssinatura()
        {
            bd = new BD();
        }

        public IRLib.Paralela.Assinaturas.Models.CaixaResumoAssinatura   BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas, int Canais, int Lojas, string DataInicial, string DataFinal)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();
                var retorno = new Models.CaixaResumoAssinatura();

                DateTime dataInicial = DateTime.MaxValue;
                if (!string.IsNullOrEmpty(DataInicial) && Utilitario.IsDateTime(DataInicial, "dd/MM/yyyy"))
                    dataInicial = DateTime.ParseExact(DataInicial.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime dataFinal = DateTime.MaxValue;
                if (!string.IsNullOrEmpty(DataFinal) && Utilitario.IsDateTime(DataFinal, "dd/MM/yyyy"))
                    dataFinal = DateTime.ParseExact(DataFinal.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);


                string sql = @"
                            SELECT
                            s.ID as SetorID , s.Nome AS SetorNome,a.ID as AssinaturaID, a.Nome as AssinaturaNome,
                            cn.ID as CanalID,cn.Nome as CanalNome, l.ID as LojaID,l.Nome as LojaNome,
                            fp.Nome  as FormaPagamento , pt.Nome AS Preco ,SUM(p.Valor) AS ValorTotal, COUNT(DISTINCT ac.ID) AS QtdAssinatura
                            FROM tAssinaturaCliente ac (NOLOCK)
                            INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = ac.VendaBilheteriaID
                            INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
                            INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
                            INNER JOIN tAssinatura a (NOLOCK) ON ac.AssinaturaID = a.ID                            
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
                            LEFT JOIN tPrecoTipo pt (NOLOCK) ON pt.ID = ac.PrecoTipoID
                            INNER JOIN tCaixa c on c.ID = vb.caixaID
                            INNER JOIN tLoja l on c.LojaID = l.ID
                            INNER JOIN tCanal cn on cn.ID = l.CanalID
                            INNER JOIN tVendaBilheteriaFormaPagamento vbfp on vbfp.VendaBilheteriaID = ac.VendaBilheteriaID
                            INNER JOIN tFormaPagamento fp on fp.ID = vbfp.FormaPagamentoID
                            WHERE a.Ativo = 'T' 
                            AND (( Acao = 'R' ) 
                            OR ( Acao = 'N' AND ac.Status = 'N' ) 
                            OR ( Acao = 'E' AND ac.Status = 'T' )) AND vb.VendaCancelada = 'F'
                            AND a.AssinaturaTipoID =" + AssinaturaTipoID;

                sql += " AND c.DataAbertura > " + dataInicial.ToString("yyyyMMddHHmmss");

                sql += " AND c.DataAbertura < " + dataFinal.ToString("yyyyMMddHHmmss");

                if (Assinaturas > 0)
                    sql += " AND a.ID = " + Assinaturas;

                if (Canais > 0)
                    sql += " AND cn.ID = " + Canais;

                if (Lojas > 0)
                    sql += " AND l.ID = " + Lojas;

                sql += @" GROUP BY a.Nome, s.ID, s.Nome, pt.Nome, fp.Nome, a.ID, a.Nome, cn.ID, cn.Nome, l.ID, l.Nome, fp.Nome
                         ORDER BY SetorID, CanalID, a.Nome, s.Nome, pt.Nome, fp.Nome ";

                bd.Consulta(sql);


                List<int> auxSetorID = new List<int>();

                while (bd.Consulta().Read())
                {

                    retorno.listaTotal.Add(new Models.DetalheCaixaResumo()
                    {
                        AssinaturaID = bd.LerInt("AssinaturaID"),
                        AssinaturaNome = bd.LerString("AssinaturaNome"),
                        CanalID = bd.LerInt("CanalID"),
                        CanalNome = bd.LerString("CanalNome"),
                        LojaID = bd.LerInt("LojaID"),
                        LojaNome = bd.LerString("LojaNome"),
                        QtdAssinatura = bd.LerInt("QtdAssinatura"),
                        ValorTotal = bd.LerDecimal("ValorTotal"),
                        FormaPagamento = bd.LerString("FormaPagamento"),
                        SetorNome = bd.LerString("SetorNome"),
                        SetorID = bd.LerInt("SetorID"),
                    });
                }

                retorno.listaCanal = retorno.listaTotal.Select(c => c.CanalID).Distinct().ToList();
                retorno.listaLoja = retorno.listaTotal.Select(c => c.LojaID).Distinct().ToList();
                retorno.listaAssinatura = retorno.listaTotal.Select(c => c.AssinaturaID).Distinct().ToList();


                return retorno;

            }
            finally
            {
                bd.Fechar();
            }
        }


    }
}
