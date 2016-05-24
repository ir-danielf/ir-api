/* ---------------------------------------------------------------
-- Arquivo Loja.cs
-- Gerado em: segunda-feira, 28 de março de 2005
-- Autor: Celeritas Ltda
---------------------------------------------------------------- */

using CTLib;
using IRLib.ClientObjects;
using IRLib.ClientObjects.Arvore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace IRLib
{

    public class Loja : Loja_B
    {
        public static int INTERNET_LOJA_ID
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["InternetLojaID"]))
                    return 2;
                else
                    return Convert.ToInt32(ConfigurationManager.AppSettings["InternetLojaID"]);

            }
        }

        public enum TEFTipos
        {
            [System.ComponentModel.Description("TEF Digitado")]
            Digitado = 'D',
            [System.ComponentModel.Description("TEF Com Cartão")]
            CartaoPresente = 'C',
            [System.ComponentModel.Description("TEF Com Cartão - Auttar")]
            CartaoPresenteAuttar = 'A',
            [System.ComponentModel.Description("Sem TEF")]
            SemTef = 'S'
        }
        public Loja() { }

        public Loja(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		s0
        /// Obter todas as lojas
        /// </summary>
        /// <returns></returns>
        public DataTable Todas()
        {

            try
            {

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tLoja.ID,tLoja.Nome,tEmpresa.Nome AS Empresa " +
                    "FROM tLoja " +
                    "INNER JOIN tCanal ON tLoja.CanalID=tCanal.ID " +
                    "INNER JOIN tEmpresa ON tCanal.EmpresaID=tEmpresa.ID " +
                    "ORDER BY tEmpresa.Nome,tLoja.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Empresa") + ", " + bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        ///Obtem papeis para essa loja
        /// </summary>
        /// <returns></returns>
        public override DataTable Papeis()
        {
            return null;
        }

        /// <summary>
        /// Obtem qtde de ingressos que esta loja possui
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressos()
        {

            try
            {

                int qtde;

                string sql = "SELECT COUNT(I.ID) AS Qtde FROM tIngresso AS I " +
                    "INNER JOIN tApresentacao AS A ON I.ApresentacaoID = A.ID " +
                    "WHERE I.Status = '" + Ingresso.PREIMPRESSO + "' AND LojaID = " + this.Control.ID + " AND A.DisponivelAjuste = 'T'";

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new SetorException("Apresentação/setor não existe.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem qtde de ingressos que esta loja possui dada uma determinada Apresentacao e Setor
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressos(int apresentacaosetorid)
        {

            try
            {

                int qtde;

                string sql = "SELECT Count(*) AS Qtde " +
                    "FROM tIngresso " +
                    "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + this.Control.ID + " AND ApresentacaoSetorID=" + apresentacaosetorid;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new SetorException("Apresentação/setor não existe.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem qtde de ingressos que esta loja possui dada uma determinada Apresentacao e Setor e Preço
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressos(int apresentacaosetorid, int precoid)
        {

            try
            {

                int qtde;

                string sql = "SELECT Count(*) AS Qtde " +
                    "FROM tIngresso " +
                    "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + this.Control.ID + " AND ApresentacaoSetorID=" + apresentacaosetorid + " AND PrecoID=" + precoid;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new SetorException("Apresentação/setor não existe.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int QtdeIngressosPreImpresso(int apresentacaosetorid, int precoid)
        {

            try
            {

                int qtde;

                string sql = "SELECT " +
                             "CASE(tSetor.LugarMarcado) " +
                             "   WHEN 'M' THEN COUNT(DISTINCT LugarID) " +
                             "   ELSE COUNT(*) " +
                             "END AS Qtde " +
                             "FROM tIngresso " +
                             "INNER JOIN tSetor ON tSetor.ID = tIngresso.SetorID " +
                             "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + this.Control.ID +
                             " AND ApresentacaoSetorID=" + apresentacaosetorid + " AND PrecoID=" + precoid +
                             " AND (PacoteID Is null  or PacoteID = 0) " +
                             "GROUP BY tSetor.LugarMarcado";

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    qtde = 0;

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public decimal carregaValorPreço(int precoid)
        {
            try
            {
                decimal precoValor;

                string sql = "SELECT Valor FROM tPreco WHERE id = " + precoid;

                precoValor = Convert.ToDecimal(bd.ConsultaValor(sql));

                bd.Fechar();

                return precoValor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int carregaQuantidadePorPacote(int PacoteID, int LojaID)
        {
            try
            {
                int quantidade;

                string sql = "SELECT COUNT(I.ID) AS Qtde FROM tIngresso AS I " +
                    "INNER JOIN tApresentacao AS A ON I.ApresentacaoID = A.ID " +
                    "WHERE I.Status = '" + Ingresso.PREIMPRESSO + "' AND PacoteID = " + PacoteID + " AND LojaID = " + LojaID +
                    " AND A.DisponivelAjuste = 'T'  GROUP BY I.EventoID";

                quantidade = Convert.ToInt32(bd.ConsultaValor(sql));

                bd.Fechar();

                return quantidade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int QtdeIngressosPreImpresso(int apresentacaosetorid)
        {
            try
            {
                int qtde;

                string sql = "SELECT " +
                             "CASE(tSetor.LugarMarcado) " +
                             "   WHEN 'M' THEN COUNT(DISTINCT LugarID) " +
                             "   ELSE COUNT(tIngresso.ID) " +
                             "END AS Qtde " +
                             "FROM tIngresso " +
                             "INNER JOIN tSetor ON tSetor.ID = tIngresso.SetorID " +
                             "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + this.Control.ID + " AND ApresentacaoSetorID=" + apresentacaosetorid +
                             " GROUP BY tSetor.LugarMarcado";

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    qtde = 0;

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override DataTable Caixas(string registroZero)
        {
            // Criando DataTable
            DataTable tabela = new DataTable("");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("DataAbertura", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT tCaixa.ID, tCaixa.DataAbertura, tUsuario.Nome, tCaixa.UsuarioID, tCaixa.DataFechamento " +
                    "FROM tCaixa INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE (tCaixa.LojaID = " + this.Control.ID + ") " +
                    "ORDER BY tCaixa.DataAbertura DESC ";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    //					// Somente os caixas fechados
                    //					if (bd.LerStringFormatoDataHora("DataFechamento")!= "  /  /       :  ") {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["DataAbertura"] = bd.LerStringFormatoDataHora("DataAbertura");
                    tabela.Rows.Add(linha);
                    //					}
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        }

        /// <summary>
        /// Caixas em função do Usuário
        /// </summary>
        public override DataTable Caixas(string registroZero, int usuarioID)
        {
            // Criando DataTable
            DataTable tabela = new DataTable("");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("DataAbertura", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT tCaixa.ID, tCaixa.DataAbertura, tUsuario.Nome, tCaixa.UsuarioID, tCaixa.DataFechamento " +
                    "FROM tCaixa INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE (tCaixa.LojaID = " + this.Control.ID + ") AND (tCaixa.UsuarioID = " + usuarioID + ") " +
                    "ORDER BY tCaixa.DataAbertura DESC ";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DataAbertura"] = bd.LerString("Nome") + " - " + bd.LerStringFormatoDataHora("DataAbertura");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        }
        public override DataTable Usuarios(string registroZero)
        {
            // Criando DataTable
            DataTable tabela = new DataTable("");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT DISTINCT tUsuario.ID, tUsuario.Nome " +
                    "FROM            tLoja INNER JOIN " +
                    "tCaixa ON tLoja.ID = tCaixa.LojaID INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE        (tLoja.ID = " + this.Control.ID + ") " +
                    "ORDER BY tUsuario.Nome ";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        }

        private decimal CalculaPct(object valor, object valorTotal)
        {
            try
            {
                return Convert.ToDecimal(Convert.ToDecimal((((decimal)valor * 100) / (decimal)valorTotal)).ToString(Utilitario.FormatoPorcentagem1Casa));

            }
            catch
            {
                return 0;
            }
        }

        private string VerificaCompute(object valor)
        {
            try
            {
                return Convert.ToDecimal(valor).ToString(Utilitario.FormatoMoeda); ;
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// Vendas Gerenciais por evento com Quantidade e Valores dos Ingressos dos Vendidos e Cancelados e Total, com porcentagem
        /// Se for por Canal o parâmetro 
        /// apresentacaoID corresponde a usuarioID
        /// eventoID corresponde a lojaID
        /// localID corresponde a canalID
        /// </summary>
        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia,
            int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                { // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                int quantidadeVendidosTotais = 0;
                int quantidadeCanceladosTotais = 0;
                int quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os Caixas nos intervalos especificados
                // Filtrando as condições
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                // Linhas do Grid
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDsTotais);
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // loja zero pega todos
                int totaisVendidos = QuantidadeIngressosPorLoja(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorLoja(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorLoja(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorLoja(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                # endregion
                // Para cada evento no período especificado, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorLoja(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorLoja(ingressoLogIDsCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToInt32(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToInt32(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToInt32(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorLoja(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorLoja(ingressoLogIDsCancelados);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                tabela.Columns["VariacaoLinha"].ColumnName = "Loja";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Eventos por período do Caixa e situacao dos ingressos
        /// </summary>
        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tLoja.ID, tLoja.Nome " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("ID");
                        linha["VariacaoLinha"] = obterDados.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Obtem quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeIngressosPorLoja(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoLoja = "";
                    if (this.Control.ID > 0)
                        condicaoLoja = "AND (tCaixa.LojaID = " + this.Control.ID + ") ";
                    else
                        condicaoLoja = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   COUNT(tLoja.ID) AS QuantidadeIngressos " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tUsuario ON tCaixa.UsuarioID = tUsuario.ID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoLoja;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de evento
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos eventos
                        while (bd.Consulta().Read())
                        {
                            quantidade += bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorLoja
        /// <summary>
        /// Obtem valor de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override decimal ValorIngressosPorLoja(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoLoja = "";
                    if (this.Control.ID > 0)
                        condicaoLoja = "AND (tCaixa.LojaID = " + this.Control.ID + ") ";
                    else
                        condicaoLoja = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   SUM(tPreco.Valor) AS Valor " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tUsuario ON tCaixa.UsuarioID = tUsuario.ID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoLoja;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do evento
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos eventos
                        while (bd.Consulta().Read())
                        {
                            valor += bd.LerInt("Valor");
                        }
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorLoja

        public DataTable Listagem(int empresaID, int canalID)
        {
            try
            {
                DataTable tabela = new DataTable("LojaListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Loja", typeof(string));
                tabela.Columns.Add("Estoque", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0 && canalID > 0)
                { // um canal de uma empresa
                    condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ")";
                }
                if (empresaID > 0 && canalID == 0)
                { // todos canais de uma empresa
                    condicao = "WHERE      (tEmpresa.ID = " + empresaID + ") ";
                }
                if (empresaID == 0 && canalID == 0)
                { // todos canais de todas as empresas
                    condicao = " ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja, tLoja.ID, tEstoque.Nome AS Estoque, tLoja.Endereco, tLoja.Cidade, tLoja.Estado, tLoja.CEP, tLoja.DDDTelefone,  " +
                    "tLoja.Telefone, tLoja.Email, tLoja.Obs " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID LEFT OUTER JOIN " +
                    "tEstoque ON tLoja.EstoqueID = tEstoque.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome, tLoja.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["Estoque"] = bd.LerString("Estoque");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public DataTable ListagemEmpresaAtiva(int empresaID, int canalID)
        {
            try
            {
                DataTable tabela = new DataTable("LojaListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Loja", typeof(string));
                tabela.Columns.Add("Estoque", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0 && canalID > 0)
                { // um canal de uma empresa
                    condicao = "AND (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ") ";
                }
                if (empresaID > 0 && canalID == 0)
                { // todos canais de uma empresa
                    condicao = "AND      (tEmpresa.ID = " + empresaID + ") ";
                }
                if (empresaID == 0 && canalID == 0)
                { // todos canais de todas as empresas
                    condicao = " ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja, tLoja.ID, tEstoque.Nome AS Estoque, tLoja.Endereco, tLoja.Cidade, tLoja.Estado, tLoja.CEP, tLoja.DDDTelefone,  " +
                    "tLoja.Telefone, tLoja.Email, tLoja.Obs " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID LEFT OUTER JOIN " +
                    "tEstoque ON tLoja.EstoqueID = tEstoque.ID WHERE tEmpresa.Ativo = 'T' AND tCanal.Ativo = 'T' " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome, tLoja.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["Estoque"] = bd.LerString("Estoque");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public List<IRLib.ClientObjects.EstruturaIDNome> ListaLojasInternet(int empresaID, int canalID)
        {
            List<IRLib.ClientObjects.EstruturaIDNome> lista = new List<ClientObjects.EstruturaIDNome>();
            foreach (DataRow linha in this.ListagemInternet(empresaID, canalID).Rows)
            {
                lista.Add(new ClientObjects.EstruturaIDNome()
                {
                    ID = Convert.ToInt32(linha["ID"]),
                    Nome = linha["Loja"].ToString(),
                });
            }
            return lista;
        }

        public DataTable ListagemInternet(int empresaID, int canalID)
        {
            try
            {
                DataTable tabela = new DataTable("LojaListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Loja", typeof(string));
                tabela.Columns.Add("Estoque", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0 && canalID > 0)
                { // um canal de uma empresa
                    condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ") AND tLoja.TEFTipo = 'D' ";
                }
                if (empresaID > 0 && canalID == 0)
                { // todos canais de uma empresa
                    condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND tLoja.TEFTipo = 'D' ";
                }
                if (empresaID == 0 && canalID == 0)
                { // todos canais de todas as empresas
                    condicao = "WHERE tLoja.TEFTipo = 'D' ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja, tLoja.ID, tEstoque.Nome AS Estoque, tLoja.Endereco, tLoja.Cidade, tLoja.Estado, tLoja.CEP, tLoja.DDDTelefone,  " +
                    "tLoja.Telefone, tLoja.Email, tLoja.Obs " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID LEFT OUTER JOIN " +
                    "tEstoque ON tLoja.EstoqueID = tEstoque.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome, tLoja.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["Estoque"] = bd.LerString("Estoque");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public DataTable ListarInternet(int empresaID, int canalID)
        {
            try
            {
                DataTable tabela = new DataTable("LojaListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Loja", typeof(string));
                tabela.Columns.Add("Estoque", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0 && canalID > 0)
                { // um canal de uma empresa                    
                    if (canalID != 2)
                        condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ") AND TEFTipo = 'D' ";
                    else
                        condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ") ";
                }
                if (empresaID > 0 && canalID == 0)
                { // todos canais de uma empresa
                    condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND TEFTipo = 'D' ";
                }
                if (empresaID == 0 && canalID == 0)
                { // todos canais de todas as empresas
                    condicao = "WHERE TEFTipo = 'D' ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja, tLoja.ID, tEstoque.Nome AS Estoque, tLoja.Endereco, tLoja.Cidade, tLoja.Estado, tLoja.CEP, tLoja.DDDTelefone,  " +
                    "tLoja.Telefone, tLoja.Email, tLoja.Obs " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID LEFT OUTER JOIN " +
                    "tEstoque ON tLoja.EstoqueID = tEstoque.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome, tLoja.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["Estoque"] = bd.LerString("Estoque");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public DataTable ListagemPorRegionalID(int empresaID, int canalID, int regionalID)
        {
            try
            {
                DataTable tabela = new DataTable("LojaListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Loja", typeof(string));
                tabela.Columns.Add("Estoque", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0 && canalID > 0)
                { // um canal de uma empresa
                    condicao = "WHERE (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ") AND (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                if (empresaID > 0 && canalID == 0)
                { // todos canais de uma empresa
                    condicao = "WHERE      (tEmpresa.ID = " + empresaID + ") AND (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                if (empresaID == 0 && canalID == 0)
                { // todos canais de todas as empresas
                    condicao = "WHERE      (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja, tLoja.ID, tEstoque.Nome AS Estoque, tLoja.Endereco, tLoja.Cidade, tLoja.Estado, tLoja.CEP, tLoja.DDDTelefone,  " +
                    "tLoja.Telefone, tLoja.Email, tLoja.Obs " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID LEFT OUTER JOIN " +
                    "tEstoque ON tLoja.EstoqueID = tEstoque.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome, tLoja.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["Estoque"] = bd.LerString("Estoque");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public DataTable ListagemEmpresaAtivaPorRegionalID(int empresaID, int canalID, int regionalID)
        {
            try
            {
                DataTable tabela = new DataTable("LojaListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Loja", typeof(string));
                tabela.Columns.Add("Estoque", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0 && canalID > 0)
                { // um canal de uma empresa
                    condicao = "AND (tEmpresa.ID = " + empresaID + ") AND (tCanal.ID = " + canalID + ") AND (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                if (empresaID > 0 && canalID == 0)
                { // todos canais de uma empresa
                    condicao = "AND      (tEmpresa.ID = " + empresaID + ") AND (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                if (empresaID == 0 && canalID == 0)
                { // todos canais de todas as empresas
                    condicao = "AND      (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja, tLoja.ID, tEstoque.Nome AS Estoque, tLoja.Endereco, tLoja.Cidade, tLoja.Estado, tLoja.CEP, tLoja.DDDTelefone,  " +
                    "tLoja.Telefone, tLoja.Email, tLoja.Obs " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID LEFT OUTER JOIN " +
                    "tEstoque ON tLoja.EstoqueID = tEstoque.ID WHERE tEmpresa.Ativo='T' AND tCanal.Ativo = 'T' " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome, tLoja.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["Estoque"] = bd.LerString("Estoque");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public List<EstruturaSelecaoArvoreLoja> Buscar(int empresaID, int canalID, bool ativos = false)
        {
            try
            {
                List<EstruturaSelecaoArvoreLoja> lista = new List<EstruturaSelecaoArvoreLoja>();
                string filtro = string.Empty;

                if (canalID > 0)
                    filtro += " AND c.ID = " + canalID;

                if (empresaID > 0)
                    filtro += " AND em.ID = " + empresaID;

                if (ativos)
                    filtro += " AND c.Ativo = 'T' AND em.Ativo = 'T' ";

                string consulta =
                    string.Format(
                    @"
                    SELECT
                        em.ID AS EmpresaID, em.Nome AS Empresa,
                        c.ID AS CanalID, c.Nome AS Canal,
                        l.ID AS LojaID, l.Nome as Loja
                    FROM tEmpresa em (NOLOCK)
                    INNER JOIN tCanal c (NOLOCK) ON c.EmpresaID = em.ID
                    INNER JOIN tLoja l (NOLOCK) ON l.CanalID = c.ID
                    WHERE 1=1 {0}
                    ", filtro.Length > 0 ? filtro : string.Empty);

                bd.Consulta(consulta);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaSelecaoArvoreLoja()
                    {
                        EmpresaID = bd.LerInt("EmpresaID"),
                        Empresa = bd.LerString("Empresa"),
                        CanalID = bd.LerInt("CanalID"),
                        Canal = bd.LerString("Canal"),
                        LojaID = bd.LerInt("LojaID"),
                        Loja = bd.LerString("Loja"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable BuscarInfoLoja(int id)
        {
            try
            {
                DataTable dttLoja = new DataTable();
                dttLoja.Columns.Add("ID");
                dttLoja.Columns.Add("Nome");

                string nome = bd.ConsultaValor("SELECT Nome FROM tLoja (NOLOCK) WHERE ID = " + id).ToString();

                DataRow dtr = dttLoja.NewRow();
                dtr["ID"] = id;
                dtr["Nome"] = nome;
                dttLoja.Rows.Add(dtr);
                return dttLoja;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool ValidarLojaCanal(int LojaID, List<string> canaisID)
        {
            return ValidarLojaCanal(LojaID, canaisID.ToArray());
        }

        public bool ValidarLojaCanal(int LojaID, string[] canais)
        {
            try
            {
                bool validou = false;

                string consulta = string.Format(@"SELECT ID FROM tLoja (NOLOCK)
                WHERE ID = {0} AND CanalID IN ({1}) AND TEFTipo = '" + (char)TEFTipos.Digitado + "'", LojaID, Utilitario.ArrayToString(canais));

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                    validou = true;

                return validou;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool VerificarExistenciaPos(string numeroPOS)
        {
            try
            {
                int retorno = 0;

                string consulta = "SELECT ID from tloja where numeroPOS = '" + numeroPOS + "'";

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                    retorno = bd.LerInt("ID");

                return retorno > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaEstadoCidadeSenhaPos VerificarExistenciaPOS(string numeroPOS)
        {
            try
            {
                EstruturaEstadoCidadeSenhaPos retorno = new EstruturaEstadoCidadeSenhaPos();

                string consulta = @"SELECT tloja.Estado, tloja.Cidade, tloja.ID, CanalID, tEmpresa.ID AS EmpresaID, tLoja.UsuarioPosID, tUsuario.Nome
                FROM tloja (NOLOCK)
                INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID = tCanal.ID
                INNER JOIN tEmpresa (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID
                INNER JOIN tUsuario (NOLOCK) ON tLoja.UsuarioPosID = tUsuario.ID
                WHERE numeroPOS = '" + numeroPOS + "'";

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                {
                    retorno.Estado = bd.LerString("Estado");
                    retorno.Cidade = bd.LerString("Cidade");
                    retorno.Senha = gerarHash(numeroPOS);
                    retorno.LojaID = bd.LerInt("ID");
                    retorno.CanalID = bd.LerInt("CanalID");
                    retorno.EmpresaID = bd.LerInt("EmpresaID");
                    retorno.UsuarioID = bd.LerInt("UsuarioPosID");
                    retorno.UsuarioNome = bd.LerString("Nome");
                }
                else
                    throw new Exception("Pos nao encontrado.");

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public static bool ValidaPos(string numeroPOS, string Senha)
        {
            try
            {
                string hashGerado = gerarHash(numeroPOS);

                return string.Compare(Senha, hashGerado) == 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string gerarHash(string numeroPOS)
        {
            string hashgerado = string.Empty;

            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                byte[] pwordData = Encoding.Default.GetBytes(numeroPOS);
                byte[] hash = hashAlg.ComputeHash(pwordData);
                hashgerado = Convert.ToBase64String(hash);
            }

            return hashgerado;
        }


        public DataTable BuscaFormaDePagamento(int EmpresaID, int CanalID, decimal Valor)
        {
            try
            {
                DataTable tPagamento = new DataTable("Pagamento");
                tPagamento.Columns.Add("ID", typeof(int));
                tPagamento.Columns.Add("FormaPagamento", typeof(string));
                tPagamento.Columns.Add("Valor", typeof(decimal));
                tPagamento.Columns.Add("Dias", typeof(int));
                tPagamento.Columns.Add("TaxaAdm", typeof(decimal));
                tPagamento.Columns.Add("IR", typeof(string));
                tPagamento.Columns.Add("VirID", typeof(int));
                tPagamento.Columns.Add("CartaoID", typeof(int)).DefaultValue = 0;
                tPagamento.Columns.Add("NumeroCartao", typeof(string)).DefaultValue = string.Empty;

                string sql = string.Format(@"EXEC bilheteria_FormaPagamentoCanalPos {0}, {1}", EmpresaID, CanalID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow pagto = tPagamento.NewRow();

                    pagto["ID"] = bd.LerInt("ID"); ;
                    pagto["FormaPagamento"] = bd.LerString("Nome");
                    pagto["Valor"] = Valor;
                    pagto["Dias"] = bd.LerString("Dias");
                    pagto["TaxaAdm"] = bd.LerDecimal("TaxaAdm");
                    pagto["IR"] = bd.LerString("IR");
                    pagto["BandeiraNome"] = bd.LerString("BandeiraNome");

                    tPagamento.Rows.Add(pagto);
                }

                return tPagamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }


        }

        public List<EstruturaIDNome> BuscaUsuariosPos(int CanalID)
        {
            try
            {
                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();

                retorno.Add(new EstruturaIDNome()
                {
                    ID = 0,
                    Nome = "Selecione"
                });

                string sql = string.Format(@"SELECT DISTINCT tUsuario.ID, tUsuario.Nome AS Usuario, tUsuario.Login
                FROM tUsuario (NOLOCK)
                INNER JOIN tPerfilCanal (NOLOCK) ON tPerfilCanal.UsuarioID = tUsuario.ID
                INNER JOIN tPerfil (NOLOCK) ON tPerfilCanal.PerfilID = tPerfil.ID
                WHERE tPerfilCanal.CanalID = {0} AND tPerfil.ID <> 6
                ORDER BY tUsuario.Nome", CanalID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Usuario") + " - " + bd.LerString("Login")
                    });
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable CarregarEmpresaCanalLoja(string filtro, int paginaAtual, int totItemPorPagina, int empresaID, int canalId, out int totalRegistros, bool Ativos = false)
        {
            string filtroIDs = "";

            if (empresaID > 0 && canalId.Equals(0))
                filtroIDs = "(tEmpresa.ID = @IdEmpresa) AND (tCanal.Nome like '%' + @filtro + '%' OR tLoja.Nome  like '%' + @filtro + '%')";
            else if(empresaID.Equals(0) && canalId.Equals(0))
                filtroIDs = "(tEmpresa.Nome like '%' + @filtro + '%' OR tCanal.Nome like '%' + @filtro + '%' OR tLoja.Nome  like '%' + @filtro + '%')";
            else
                filtroIDs = "(tCanal.ID = @canalId)";

            if (canalId > 0)
            {
                filtroIDs += "AND tCanal.ID = @canalId";
            }

			if (Ativos)
				filtroIDs += " AND tEmpresa.Ativo = 'T' AND tCanal.Ativo = 'T' ";

            string query = @"SELECT * FROM (
                                     SELECT ROW_NUMBER() OVER(ORDER BY tLoja.ID) AS RowNumber,
                                                                tEmpresa.ID AS IdEmpresa,
		                                                        tEmpresa.Nome AS NomeEmpresa,
		                                                        tEmpresa.RegionalID AS IdRegionalEmpresa,
		                                                        tCanal.ID AS IdCanal,
		                                                        tCanal.Nome AS NomeCanal,
		                                                        tLoja.ID AS IdLoja,
		                                                        tLoja.Nome AS NomeLoja
                                                        FROM tEmpresa (NOLOCK)
                                                        INNER JOIN tCanal (NOLOCK) ON tEmpresa.ID = tCanal.EmpresaID
                                                        INNER JOIN tLoja (NOLOCK) ON tCanal.ID = tLoja.CanalID
                                                        where " + filtroIDs + @") AS TBL
                                        WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)
                                        ORDER BY IdRegionalEmpresa;
                                        SELECT COUNT(tEmpresa.ID) as totalRegistros FROM tEmpresa (NOLOCK)
                                                        INNER JOIN tCanal (NOLOCK) ON tEmpresa.ID = tCanal.EmpresaID
                                                        INNER JOIN tLoja (NOLOCK) ON tCanal.ID = tLoja.CanalID
                                                        where" + filtroIDs + @";";

            CTLib.BD bd = new BD();
            //SqlParameterCollection parametros = new SqlParameterCollection();
            List<SqlParameter> parametros = new List<SqlParameter>();
            //    parametros.a("@nomeReginal", filtro);

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                ParameterName = "@filtro",
                Value = filtro
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@PageNumber",
                Value = paginaAtual
            });


            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@RowspPage",
                Value = totItemPorPagina
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@totalRegistros",
                Value = filtro
            });

            if (empresaID != 0)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@IdEmpresa",
                    Value = empresaID
                });

            if (canalId != 0)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@canalId",
                    Value = canalId
                });

            DataSet result = bd.QueryToDataSet(query, parametros);
            totalRegistros = Convert.ToInt32(result.Tables[1].Rows[0][0]);

            return result.Tables[0];
        }

    }

    public class LojaLista : LojaLista_B
    {

        public LojaLista() { }

        public LojaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela de todos os campos de loja carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            DataTable tabela = new DataTable("Loja");

            try
            {

                if (this.Primeiro())
                {
                    tabela.Columns.Add("Empresa", typeof(string));
                    tabela.Columns.Add("Canal", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        Canal canal = new Canal();
                        canal.Ler(loja.CanalID.Valor);
                        linha["Canal"] = canal.Nome.Valor;
                        Empresa e = new Empresa();
                        e.Ler(canal.EmpresaID.Valor);
                        linha["Empresa"] = e.Nome.Valor;
                        linha["Nome"] = loja.Nome.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

            }
            catch
            {
                tabela = null;
            }

            return tabela;

        }

        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tLoja WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY CanalID, Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }



        /// <summary>
        /// Carrega a lista
        /// </summary>
        public void CarregarLojaPorEmpresaID(int empresaID)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT tLoja.ID FROM tLoja " +
                        "INNER JOIN tCanal ON tCanal.ID = tLoja.CanalID " +
                        "WHERE tCanal.EmpresaID=" + empresaID;
                else
                    sql = "SELECT top " + tamanhoMax + " tLoja.ID FROM tLoja" +
                        "INNER JOIN tCanal ON tCanal.ID = tLoja.CanalID " +
                        "WHERE tCanal.EmpresaID=" + empresaID;

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}

