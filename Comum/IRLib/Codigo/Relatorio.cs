using CTLib;
using System;
using System.Collections.Generic;

namespace IRLib
{
    public class Relatorio
    {
        #region Properties

        DateTime dataVenda;
        public DateTime DataVenda
        {
            get { return dataVenda; }
            set { dataVenda = value; }
        }

        int quantidadeIngressos = 0;
        public int QuantidadeIngressos
        {
            get { return quantidadeIngressos; }
            set { quantidadeIngressos = value; }
        }

        int quantidadeVendas = 0;
        public int QuantidadeVendas
        {
            get { return quantidadeVendas; }
            set { quantidadeVendas = value; }
        }

        decimal taxaConveniencia = 0;
        public decimal TaxaConveniencia
        {
            get { return taxaConveniencia; }
            set { taxaConveniencia = value; }
        }

        decimal comissao = 0;
        public decimal Comissao
        {
            get { return comissao; }
            set { comissao = value; }
        }

        decimal taxaEntrega = 0;
        public decimal TaxaEntrega
        {
            get { return taxaEntrega; }
            set { taxaEntrega = value; }
        }
        decimal total = 0;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        #endregion

        public Relatorio() { }

        
    }

    public class RelatorioCollection : List<Relatorio>
    {
        public RelatorioCollection FaturamentoBruto(DateTime dataInicial, DateTime dataFinal)
        {
            Relatorio oRelatorio;
            RelatorioCollection oRelatorioList = new RelatorioCollection();
            BD bd = new BD();

            try
            {
                string inicial = dataInicial.ToString("yyyyMMdd");
                string final = dataFinal.AddDays(1).ToString("yyyyMMdd");


                string sql = "SELECT " +
                                "DISTINCT vb.ID as VendaBilheteriaID, SUBSTRING(vb.DataVenda,1,8) AS DataVenda, " +
                                "vb.Status, vb.TaxaConvenienciaValorTotal AS TaxaConveniencia,  " +
                                "vb.ComissaoValorTotal AS Comissao, vb.TaxaEntregaValor AS TaxaEntrega, COUNT(i.ID) AS QuantidadeIngressos " +
                                "INTO #FaturamentoBruto " +
                                "FROM tCaixa cxa (NOLOCK) " +
                                "INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.caixaID = cxa.ID " +
                                "INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID " +
                                "INNER JOIN tLoja l (NOLOCK) ON l.ID = cxa.LojaID " +
                                "INNER JOIN tCanal c (NOLOCK) ON c.ID = l.CanalID " +
                                "INNER JOIN tEmpresa e (NOLOCK) ON e.ID = c.EmpresaID " +
                                "WHERE (EmpresaPromove = 'F' AND EmpresaVende = 'T') " +
                                "AND (cxa.DataAbertura BETWEEN '" + inicial + "' AND '" + final + "') " +
                                "AND (vb.Status = 'P' OR vb.Status = 'C') " +
                                "GROUP BY vb.ID, vb.DataVenda, vb.Status, vb.TaxaConvenienciaValorTotal, vb.ComissaoValorTotal, vb.TaxaEntregaValor " +
                                "SELECT " +
                                "DataVenda, COUNT(VendaBilheteriaID) AS QuantidadeVendas, SUM(QuantidadeIngressos) AS QuantidadeIngressos, " +
                                "SUM(TaxaConveniencia) AS TaxaConveniencia, " +
                                "SUM(Comissao) AS Comissao, SUM(TaxaEntrega) AS TaxaEntrega, " +
                                "SUM(TaxaConveniencia) + SUM(Comissao) + SUM(TaxaEntrega) AS Total " +
                                "FROM #FaturamentoBruto " +
                                "GROUP BY DataVenda " +
                                "ORDER BY DataVenda " +
                                "DROP TABLE #FaturamentoBruto";

                bd.Executar(sql);

                while (bd.Consulta().Read())
                {
                    oRelatorio = new Relatorio();
                    oRelatorio.DataVenda = DateTime.ParseExact(bd.LerString("DataVenda"), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    oRelatorio.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                    oRelatorio.QuantidadeVendas = bd.LerInt("QuantidadeVendas");
                    oRelatorio.TaxaConveniencia = bd.LerDecimal("TaxaConveniencia");
                    oRelatorio.Comissao = bd.LerDecimal("Comissao");
                    oRelatorio.TaxaEntrega = bd.LerDecimal("TaxaEntrega");
                    oRelatorio.Total = bd.LerDecimal("Total");

                    oRelatorioList.Add(oRelatorio);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();                
            }
            return oRelatorioList;           
        }
    }
    
}
