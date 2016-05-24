/**************************************************
* Arquivo: Venda.cs
* Gerado: 16/05/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Codigo.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using IRLib.Codigo.TransporteModels;
using IRLib.CancelamentoIngresso;
using IRLib.Codigo.TrocaIngresso;
using IRLib.ClientObjects;

namespace IRLib
{

    public class Venda : Venda_B
    {
        public Venda() { }

        public Venda(int usuarioIDLogado) : base(usuarioIDLogado) { }


        /// <summary>		
        /// Obtehm o total das vendas nao pagas de um ingresso (de todos caixas)
        /// </summary>
        /// <returns></returns>
        public override decimal TotalIngresso(int ingresso)
        {
            decimal total = -1;
            try
            {
                BD bd = new BD();
                string sql =
                    "SELECT        SUM(tComandaItem.PrecoVenda * tComandaItem.Quantidade) AS Soma, tVenda.Pago, tVenda.IngressoID " +
                    "FROM          tComanda INNER JOIN " +
                                        "tComandaItem ON tComanda.ID = tComandaItem.ComandaID INNER JOIN " +
                                        "tVenda ON tComanda.VendaID = tVenda.ID " +
                    "GROUP BY tVenda.Pago, tVenda.IngressoID " +
                    "HAVING        (tVenda.Pago = 'F') AND (tVenda.IngressoID = " + ingresso + ")";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerDecimal("Soma");
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Falha ao obter valor total por Ingresso (conta em aberto)!");
            }
            return total;
        }
        /// <summary>		
        /// Obtehm o total da comanda nao paga 
        /// </summary>
        /// <returns></returns>
        public override decimal TotalComanda(int comandaID)
        {
            decimal total = 0;
            try
            {
                BD bd = new BD();
                string sql = "SELECT SUM(tVendaPagamento.Valor) AS Soma " +
                    "FROM tComanda INNER JOIN tVenda ON tComanda.VendaID = tVenda.ID INNER JOIN " +
                    "tVendaPagamento ON tVenda.ID = tVendaPagamento.VendaID " +
                    "WHERE (tComanda.ID = " + comandaID + ") AND (tVenda.Pago = 'F')";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerDecimal("Soma");
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Falha ao obter Total de Venda por Comanda!");
            }
            return total;
        } // fim de TotalComanda

        /// <summary>		
        /// Cria um Datatable com os campos de Itens de Pagamentos
        /// </summary>
        /// <returns></returns>
        public DataTable EstruturaItensPagamento()
        {
            DataTable tabela = new DataTable("ItensPagamento");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("FormaPagamento", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
            }
            catch
            {
                tabela = null;
            }
            return tabela;
        }

        /// <summary>		
        /// Devolve todas as venda pagamentos de um ingresso (de todos caixas)
        /// </summary>
        /// <returns></returns>
        public override DataTable Pagamentos()
        {

            try
            {

                DataTable tabela = EstruturaItensPagamento();

                string sql = "SELECT f.ID, f.Nome, vp.Valor FROM tFormaPagamento as f, tVendaPagamento as vp, " +
                    "tvenda as v WHERE f.ID=vp.FormaPagamentoID AND v.ID=vp.VendaID AND " +
                    "v.Ingresso='" + this.IngressoID.Valor + " ORDER BY f.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = this.Control.ID;
                    linha["FormaPagamentoID"] = bd.LerInt("ID");
                    linha["FormaPagamento"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("Valor");
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
        /// Devolve todas as venda pagamentos de um ingresso
        /// </summary>
        /// <returns></returns>
        public override DataTable PagamentosPorIngresso(int ingressoInformado)
        {
            DataTable tabela = EstruturaItensPagamento();
            try
            {
                BD bd = new BD();
                string sql =
                    "SELECT      tVendaPagamento.Valor, tVenda.ID, tVendaPagamento.FormaPagamentoID, tFormaPagamento.Nome " +
                    "FROM        tVenda INNER JOIN " +
                                    "tVendaPagamento ON tVenda.ID = tVendaPagamento.VendaID INNER JOIN " +
                                    "tFormaPagamento ON tVendaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE        (tVenda.Pago = 'F') AND (tVenda.IngressoID = '" + ingressoInformado + "')";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = this.Control.ID;
                    linha["FormaPagamentoID"] = bd.LerInt("ID");
                    linha["FormaPagamento"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Falha ao obter Pagamentos por Comanda!!");
            }
            return tabela;
        } // fim de PagamentosPorComanda


        //Cargas das novas telas de Historico de entrega
        public DataTable CarregaVendaBilheteria(string filtro, bool porSenha, int paginaAtual, int totItemPorPagina, out int totalRegistros)
        {
            if (!String.IsNullOrEmpty(filtro))
            {
                try
                {
                    Validacoes _Validacoes = new Validacoes();

                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("SELECT * FROM (");
                    strBuilder.Append("SELECT ROW_NUMBER() OVER(ORDER BY vb.DataVenda Desc) AS RowNumber, ");
                    strBuilder.Append("vb.ID AS IdVendaBilheteria, ");
                    strBuilder.Append("vb.Senha, ");
                    strBuilder.Append("vb.DataVenda, ");
                    strBuilder.Append("vb.Status, ");
                    strBuilder.Append("vb.PagamentoProcessado, ");
                    strBuilder.Append("tCanal.Nome AS Canal, ");
                    strBuilder.Append("vb.ClienteID AS IdCliente, ");
                    strBuilder.Append("c.Nome AS ClienteNome, ");
                    strBuilder.Append("ec.EntregaID AS IdEntrega ");
                    strBuilder.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                    //strBuilder.Append("LEFT JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                    strBuilder.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                    strBuilder.Append("LEFT JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                    strBuilder.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                    //strBuilder.Append("LEFT JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                    strBuilder.Append("LEFT JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                    strBuilder.Append("LEFT JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                    strBuilder.Append("WHERE 1=1 ");
                    strBuilder.Append(porSenha ? " AND vb.Senha LIKE @Senha + '%' AND vb.Status <> 'C' " : " AND vb.Status <> 'C' AND vb.ClienteID = @IdCliente ");
                    strBuilder.Append(") AS TBL ");
                    strBuilder.Append("WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage) ");
                    strBuilder.Append("SELECT COUNT(vb.Senha) as totalRegistros ");
                    strBuilder.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                    //strBuilder.Append("LEFT JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID ");
                    strBuilder.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                    strBuilder.Append("LEFT JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID ");
                    strBuilder.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                    //strBuilder.Append("LEFT JOIN tUsuario u (NOLOCK) ON ca.UsuarioID = u.ID ");
                    strBuilder.Append("LEFT JOIN tLoja l (NOLOCK) ON l.ID = ca.LojaID ");
                    strBuilder.Append("LEFT JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID WHERE 1=1 ");
                    strBuilder.Append(porSenha ? " AND vb.Senha LIKE @Senha + '%' AND vb.Status <> 'C' " : " AND vb.Status <> 'C' AND vb.ClienteID = @IdCliente ");

                    CTLib.BD bd = new BD();

                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter()
                    {
                        DbType = porSenha ? DbType.String : DbType.Int32,
                        ParameterName = porSenha ? "@Senha" : "@IdCliente",
                        Value = porSenha ? _Validacoes.removeCaracteres(filtro) : filtro
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

                    DataSet result = bd.QueryToDataSet(strBuilder.ToString(), parametros);
                    totalRegistros = Convert.ToInt32(result.Tables[1].Rows[0][0]);

                    return result.Tables[0];
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    totalRegistros = 0;
                    return null;

                }
            }
            else
            {
                totalRegistros = 0;
                return null;
            }
        }

        public DataTable BuscaVendaBilheteria(int idVendaBilheteria)
        {
            if (idVendaBilheteria > 0)
            {
                try
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("SELECT TOP 1 ");
                    strBuilder.Append("tVendaBilheteria.ID AS IdBilheteria, ");
                    strBuilder.Append("tVendaBilheteria.Senha, ");
                    strBuilder.Append("tVendaBilheteria.DataVenda, ");
                    strBuilder.Append("tVendaBilheteria.ClienteID AS IdCliente, ");
                    strBuilder.Append("tVendaBilheteria.ValorTotal, ");
                    strBuilder.Append("tVendaBilheteria.DataTransacao AS DataCaixa, ");
                    strBuilder.Append("tVendaBilheteria.PagamentoProcessado AS Processamento, ");
                    strBuilder.Append("tVendaBilheteria.TaxaEntregaValor AS ValorEntrega, ");
                    strBuilder.Append("tVendaBilheteria.CaixaID AS IdCaixa, ");
                    strBuilder.Append("tVendaBilheteria.TaxaConvenienciaValorTotal AS Conveniencia, ");
                    strBuilder.Append("tVendaBilheteria.Status AS StatusVenda, ");
                    strBuilder.Append("tVendaBilheteria.ValorSeguro, ");
                    strBuilder.Append("tCliente.Nome AS Cliente, ");
                    strBuilder.Append("tCliente.RG AS RgCliente, ");
                    strBuilder.Append("tCliente.CPF AS CpfCliente, ");
                    strBuilder.Append("tCliente.Email AS EmailCliente, ");
                    strBuilder.Append("tCaixa.DataAbertura AS DataAberturaCaixa, ");
                    strBuilder.Append("tCaixa.UsuarioID AS IdUsuario, ");
                    strBuilder.Append("tUsuario.Nome AS Vendedor, ");
                    strBuilder.Append("tUsuario.Sexo AS SexoUsuario, ");
                    strBuilder.Append("tLoja.Nome AS NomeLoja, ");
                    strBuilder.Append("tLoja.Endereco AS EnderecoLoja, ");
                    strBuilder.Append("tLoja.Cidade AS CidadeLoja, ");
                    strBuilder.Append("tLoja.CanalID AS idCanal, ");
                    strBuilder.Append("tCanal.Nome AS Canal ");
                    strBuilder.Append("FROM tVendaBilheteria (NOLOCK) ");
                    strBuilder.Append("LEFT JOIN tCliente (NOLOCK) ON tVendaBilheteria.ClienteID = tCliente.ID ");
                    strBuilder.Append("LEFT JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID ");
                    strBuilder.Append("LEFT JOIN tUsuario (NOLOCK) ON tCaixa.UsuarioID = tUsuario.ID ");
                    strBuilder.Append("LEFT JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                    strBuilder.Append("LEFT JOIN tCanal (NOLOCK) ON tLoja.CanalID = tCanal.ID ");
                    strBuilder.Append("WHERE tVendaBilheteria.ID = @IdVendaBilheteria");

                    CTLib.BD bd = new BD();

                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter()
                    {
                        DbType = DbType.Int32,
                        ParameterName = "@IdVendaBilheteria",
                        Value = idVendaBilheteria
                    });

                    return bd.QueryToTable(strBuilder.ToString(), parametros);
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                return null;
            }
        }

        public enum enumDetalhaIngresso
        {
            HistoricoEntrega,
            HistoricoCancelamento
        }

        public int CountIngressosPorSenha(int idVendaBilheteria)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(@"SELECT COUNT(ID) FROM tIngressoLog(NOLOCK) ");
            strBuilder.Append("WHERE VendaBilheteriaID = @IdVendaBilheteria and Acao = 'V' ");
            CTLib.BD bd = new BD();

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@IdVendaBilheteria",
                Value = idVendaBilheteria
            });

            DataSet result = bd.QueryToDataSet(strBuilder.ToString(), parametros);
            return Convert.ToInt32(result.Tables[0].Rows[0][0]);
        }

        public DataTable DetalhaIngressos(int idVendaBilheteria, int paginaAtual, int totItemPorPagina, out int totalRegistros, string Status, enumDetalhaIngresso enumDetalhaIngresso)
        {
            if (idVendaBilheteria > 0)
            {
                try
                {
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    string complementoPesquisa = string.Empty;
                    switch (enumDetalhaIngresso)
                    {
                        case Venda.enumDetalhaIngresso.HistoricoEntrega:
                            //complementoPesquisa = "AND (il.Acao = @AcaoUm OR il.Acao = @AcaoDois AND i.Status != @StatusUm)";

                            //parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@AcaoUm", Value = IngressoLog.VENDER });
                            //parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@AcaoDois", Value = IngressoLog.PRE_RESERVA });
                            //parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@StatusUm", Value = VendaBilheteria.CANCELADO });

                            break;
                        case Venda.enumDetalhaIngresso.HistoricoCancelamento:
                            complementoPesquisa = "AND (il.Acao = @AcaoUm OR i.Status = @StatusUm)";

                            parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@AcaoUm", Value = IngressoLog.CANCELAR });
                            parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@StatusUm", Value = VendaBilheteria.CANCELADO });
                            break;
                    }

                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY i.ID) AS RowNumber, ");

                    strBuilder.Append("i.ID AS IngressoID, ");
                    strBuilder.Append("i.ApresentacaoSetorID, ");
                    strBuilder.Append("il.VendaBilheteriaID, ");
                    strBuilder.Append("il.ID, ");
                    strBuilder.Append("il.[TimeStamp], ");
                    //strBuilder.Append("il.Acao, ");
                    strBuilder.Append("CASE il.Acao ");
                    strBuilder.Append("WHEN 'D' THEN 'Desbloquear' ");
                    strBuilder.Append("WHEN 'B' THEN 'Bloquear' ");
                    strBuilder.Append("WHEN 'V' THEN 'Vender' ");
                    strBuilder.Append("WHEN 'C' THEN 'Cancelar' ");
                    strBuilder.Append("WHEN 'K' THEN 'Kancelamento' ");
                    strBuilder.Append("WHEN 'I' THEN 'Imprimir' ");
                    strBuilder.Append("WHEN 'P' THEN 'Pré-Impresso' ");
                    strBuilder.Append("WHEN 'T' THEN 'Tranferência' ");
                    strBuilder.Append("WHEN 'R' THEN 'Reimprimir' ");
                    strBuilder.Append("WHEN 'E' THEN 'Entregar' ");
                    strBuilder.Append("WHEN 'A' THEN 'Anular Impressão' ");
                    strBuilder.Append("WHEN 'O' THEN 'Imprimir Voucher' ");
                    strBuilder.Append("WHEN 'U' THEN 'Reimprimir Voucher' ");
                    strBuilder.Append("WHEN 'M' THEN 'Pré-reserva' ");
                    strBuilder.Append("WHEN 'S' THEN 'Estorno' ");
                    strBuilder.Append("END as \"Ação\", ");
                    strBuilder.Append("il.Obs, ");
                    strBuilder.Append("il.CodigoBarra, ");
                    strBuilder.Append("i.Codigo, ");
                    strBuilder.Append("vb.ClienteID, ");
                    strBuilder.Append("vb.NotaFiscalEstabelecimento, ");
                    strBuilder.Append("vb.Senha, ");
                    strBuilder.Append("vb.TaxaEntregaValor, ");
                    //strBuilder.Append("vb.TaxaConvenienciaValorTotal, ");
                    strBuilder.Append("ISNULL(vbi.TaxaConvenienciaValor,0) AS TaxaConvenienciaValor, ");
                    strBuilder.Append("ISNULL(vb.ClienteEnderecoID, 0) AS EnderecoEntrega, ");
                    strBuilder.Append("u.Nome AS Usuario, ");
                    strBuilder.Append("IsNull(us.Nome, ' - ') as  Supervisor, ");
                    strBuilder.Append("p.Nome AS Preco, ");
                    strBuilder.Append("p.Valor, ");
                    strBuilder.Append("p.ID AS PrecoID, ");
                    strBuilder.Append("i.Status AS StatusDet, ");
                    strBuilder.Append("ev.TipoCodigoBarra, ");
                    strBuilder.Append("ev.ID EventoID, ");
                    strBuilder.Append("ev.Nome Evento, ");
                    strBuilder.Append("l.Nome AS Local, ");
                    strBuilder.Append("l.Cidade, ");
                    strBuilder.Append("s.Nome AS Setor, ");
                    strBuilder.Append("a.Horario AS Apresentacao, ");
                    strBuilder.Append("a.ID AS ApresentacaoID, ");
                    strBuilder.Append("lo.Nome AS Loja, ");
                    strBuilder.Append("cl.Nome AS Canal, ");
                    strBuilder.Append("ci.Nome AS Cliente, ");
                    strBuilder.Append("ci.Email, ");
                    strBuilder.Append("c.Nome AS Cortesia, ");
                    strBuilder.Append("b.Nome AS Bloqueio, ");
                    strBuilder.Append("en.Tipo AS TaxaEntregaTipo, ");
                    strBuilder.Append("en.Nome AS EntregaNome, ");
                    strBuilder.Append("en.ID AS EntregaID, ");
                    strBuilder.Append("ep.Nome AS PeriodoAgenda, ");
                    strBuilder.Append("ear.Nome AS AreaEntrega, ");
                    strBuilder.Append("ea.Data AS DataAgenda, ");
                    strBuilder.Append("pdv.Nome as PDVEntrega ");
                    strBuilder.Append("FROM tIngressoLog il (NOLOCK) ");
                    strBuilder.Append("LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = il.VendaBilheteriaID ");
                    strBuilder.Append("LEFT JOIN tVendaBilheteriaItem vbi (NOLOCK) ON il.VendaBilheteriaItemID = vbi.ID ");
                    strBuilder.Append("INNER JOIN tUsuario u (NOLOCK) ON u.ID = il.UsuarioID ");
                    strBuilder.Append("LEFT JOIN tUsuario us (NOLOCK) ON us.ID = il.SupervisorID ");
                    strBuilder.Append("LEFT OUTER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID ");
                    strBuilder.Append("INNER JOIN tIngresso i (NOLOCK) ON i.ID = IngressoID ");
                    strBuilder.Append("INNER JOIN tEvento ev (NOLOCK) ON ev.ID = i.EventoID ");
                    strBuilder.Append("INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID ");
                    strBuilder.Append("INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID ");
                    strBuilder.Append("INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID ");
                    strBuilder.Append("LEFT JOIN tLoja lo (NOLOCK) ON lo.ID = il.LojaID ");
                    strBuilder.Append("LEFT JOIN tCanal cl (NOLOCK) ON cl.ID = il.CanalID ");
                    strBuilder.Append("LEFT JOIN tCliente ci (NOLOCK) ON ci.ID = vb.ClienteID ");
                    strBuilder.Append("LEFT JOIN tCortesia c (NOLOCK) ON c.ID = il.CortesiaID ");
                    strBuilder.Append("LEFT JOIN tBloqueio b (NOLOCK) ON b.ID = il.BloqueioID ");
                    strBuilder.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                    strBuilder.Append("LEFT JOIN tEntrega en (NOLOCK) ON ec.EntregaID = en.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ec.PeriodoID = ep.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ec.EntregaAreaID = ear.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON vb.EntregaAgendaID = ea.ID ");
                    strBuilder.Append("LEFT JOIN tPontoVenda pdv (NOLOCK) ON vb.PdvID = pdv.ID ");
                    strBuilder.Append("WHERE (il.IngressoID IN (SELECT IngressoID FROM tIngressoLog (NOLOCK) WHERE (tIngressoLog.VendaBilheteriaID = @IdVendaBilheteria))) ");
                    strBuilder.Append(complementoPesquisa);

                    strBuilder.Append(") AS TBL ");
                    strBuilder.Append("WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage) ORDER BY TBL.Codigo, TBL.ID ");
                    strBuilder.Append("SELECT COUNT(i.ID) as totalRegistros ");

                    strBuilder.Append("FROM tIngressoLog il (NOLOCK) ");
                    strBuilder.Append("LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = il.VendaBilheteriaID ");
                    strBuilder.Append("INNER JOIN tUsuario u (NOLOCK) ON u.ID = il.UsuarioID ");
                    strBuilder.Append("LEFT JOIN tUsuario us (NOLOCK) ON us.ID = il.SupervisorID ");
                    strBuilder.Append("LEFT OUTER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID ");
                    strBuilder.Append("INNER JOIN tIngresso i (NOLOCK) ON i.ID = IngressoID ");
                    strBuilder.Append("INNER JOIN tEvento ev (NOLOCK) ON ev.ID = i.EventoID ");
                    strBuilder.Append("INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID ");
                    strBuilder.Append("INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID ");
                    strBuilder.Append("INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID ");
                    strBuilder.Append("LEFT JOIN tLoja lo (NOLOCK) ON lo.ID = il.LojaID ");
                    strBuilder.Append("LEFT JOIN tCanal cl (NOLOCK) ON cl.ID = il.CanalID ");
                    strBuilder.Append("LEFT JOIN tCliente ci (NOLOCK) ON ci.ID = vb.ClienteID ");
                    strBuilder.Append("LEFT JOIN tCortesia c (NOLOCK) ON c.ID = il.CortesiaID ");
                    strBuilder.Append("LEFT JOIN tBloqueio b (NOLOCK) ON b.ID = il.BloqueioID ");
                    strBuilder.Append("LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                    strBuilder.Append("LEFT JOIN tEntrega en (NOLOCK) ON ec.EntregaID = en.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ec.PeriodoID = ep.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ec.EntregaAreaID = ear.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON vb.EntregaAgendaID = ea.ID ");
                    strBuilder.Append("LEFT JOIN tPontoVenda pdv (NOLOCK) ON vb.PdvID = pdv.ID ");
                    strBuilder.Append("WHERE (il.IngressoID IN (SELECT IngressoID FROM tIngressoLog (NOLOCK) WHERE (tIngressoLog.VendaBilheteriaID = @IdVendaBilheteria))) ");
                    strBuilder.Append(complementoPesquisa);

                    CTLib.BD bd = new BD();

                    parametros.Add(new SqlParameter()
                    {
                        DbType = DbType.Int32,
                        ParameterName = "@IdVendaBilheteria",
                        Value = idVendaBilheteria
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

                    DataSet result = bd.QueryToDataSet(strBuilder.ToString(), parametros);
                    totalRegistros = Convert.ToInt32(result.Tables[1].Rows[0][0]);

                    return result.Tables[0];
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    totalRegistros = 0;
                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                totalRegistros = 0;
                return null;
            }
        }

        public DataTable DetalhaIngressos(List<int> ingressosID)
        {
            if (ingressosID.Count > 0)
            {
                try
                {
                    string strQuery = string.Format(@"SELECT  
	                                                    tEvento.Id AS 'EventoID'
	                                                    ,tEvento.Nome AS 'Evento'
	                                                    ,tLocal.ID AS 'LocalID'
	                                                    ,tLocal.Nome AS 'Local'
	                                                    ,tApresentacao.CotaID AS 'CotaID'
	                                                    ,tApresentacao.ID AS 'ApresentacaoID'
	                                                    ,tApresentacao.CalcHorario AS 'Horario'
	                                                    ,tSetor.ID AS 'SetorID'
	                                                    ,tSetor.Nome AS 'Setor'
	                                                    ,tIngresso.Codigo AS 'Codigo'
	                                                    ,tIngresso.ID AS 'IngressoID'
	                                                    ,tPreco.ID AS 'PrecoID'
	                                                    ,tPreco.Nome AS 'Preco'
	                                                    ,tPreco.Valor AS 'Valor'
	                                                    ,tVendaBilheteriaItem.TaxaConvenienciaValor AS 'Conveniencia'
	                                                    ,(tVendaBilheteria.ValorSeguro + tVendaBilheteria.TaxaEntregaValor) AS 'SeguroEntrega'
                                                    FROM 
	                                                    tIngressoLog (NOLOCK) 
                                                        INNER JOIN tIngresso(NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID 
                                                        INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID 
                                                        INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID AND tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID
                                                        INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID 
                                                        INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID 
                                                        INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID 
                                                        INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                                                        INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                                                    WHERE 
	                                        tIngresso.ID IN ({0})", string.Join(",", ingressosID.Distinct().ToList()));

                    CTLib.BD bd = new BD();

                    return bd.QueryToTable(strQuery);
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));
                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                return null;
            }
        }

        public DataTable ValidacaoIngressosSelecionados(List<int> ingressosID, int vendaBilheteriaID)
        {
            if (ingressosID != null && ingressosID.Count > 0)
            {
                try
                {
                    string parametros = string.Empty;
                    foreach (var item in ingressosID)
                    {
                        if (String.IsNullOrEmpty(parametros))
                            parametros = item.ToString();
                        else
                            parametros += String.Format(", {0}", item.ToString());
                    }

                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("SELECT ing.status, ");
                    strBuilder.Append("et.EntregaID, ");
                    strBuilder.Append("vbp.FormaPagamentoID, ");
                    strBuilder.Append("pr.Valor, ");
                    strBuilder.Append("ap.CalcHorario AS Horario, ");
                    strBuilder.Append("dbo.StringToDateTime(vb.DataVenda) AS DataVenda, ");
                    strBuilder.Append("ev.Nome AS nomeEvento, ");
                    strBuilder.Append("ing.PacoteID, ");
                    strBuilder.Append("ing.AssinaturaClienteID, ");

                    strBuilder.Append("Case when (select top 1 dp.StatusCancel ");
                    strBuilder.Append("from tCancelDevolucaoPendente dp (nolock) ");
                    strBuilder.Append("inner join tCancelDevolucaoPendenteIngresso dpi (nolock) on dpi.CancelDevolucaoPendenteID = dp.id ");
                    strBuilder.Append("where dp.vendabilheteriaIDVenda = ing.VendaBilheteriaId ");
                    strBuilder.Append("and dpi.IngressoID = ing.ID order by dpi.ID desc) = 'P' then 1 else 0 end as CancelPendente, ");

                    strBuilder.Append("case when (SELECT max(lm.id) ");
                    strBuilder.Append("FROM tIngressoLog lm ");
                    strBuilder.Append("where lm.acao = 'S' and lm.ingressoid = ing.ID) > ");
                    strBuilder.Append("(SELECT max(lm.id) ");
                    strBuilder.Append("FROM tIngressoLog lm ");
                    strBuilder.Append("where lm.acao = 'V' and lm.ingressoid = ing.ID) then 1 else 0 end as ValorEstornado, ");
                    strBuilder.Append("CASE ISNULL(vb.VendaBilheteriaIDOrigem, 0) ");
                    strBuilder.Append("WHEN 0 THEN 'F' ");
                    strBuilder.Append("ELSE 'T' END AS Troca, ");
                    strBuilder.Append("st.LugarMarcado as TipoLugar, ");
                    strBuilder.Append("ing.ID as IngressoID, ");
                    strBuilder.Append("ap.Cancelada as ApresentacaoCancelada ");


                    strBuilder.Append("FROM tIngresso ing(NOLOCK) ");
                    strBuilder.Append("INNER JOIN tVendaBilheteria vb(NOLOCK) on ing.VendaBIlheteriaID = vb.ID ");
                    strBuilder.Append("LEFT JOIN tEntregaControle et (NOLOCK) on vb.EntregaControleID = et.ID ");
                    strBuilder.Append("INNER JOIN tvendabilheteriaFormaPagamento vbp(NOLOCK) on vbp.VendaBIlheteriaID = vb.ID ");
                    strBuilder.Append("INNER JOIN tPreco pr(NOLOCK) on pr.ID = ing.PrecoID ");
                    strBuilder.Append("INNER JOIN tApresentacao ap(NOLOCK) on ing.apresentacaoID = ap.ID ");
                    strBuilder.Append("INNER JOIN tSetor st(NOLOCK) on ing.SetorID = st.ID ");
                    strBuilder.Append("INNER JOIN tEvento ev(NOLOCK) on ing.EventoID = ev.ID ");

                    if (!String.IsNullOrEmpty(parametros))
                        strBuilder.Append(string.Format(" WHERE ing.ID IN ({0}) AND ing.VendaBilheteriaId = @VendaBilheteriaId ", parametros));

                    List<SqlParameter> listaParametros = new List<SqlParameter>();
                    listaParametros.Add(new SqlParameter() { ParameterName = "@VendaBilheteriaId", Value = vendaBilheteriaID, DbType = DbType.Int32 });

                    CTLib.BD bd = new BD();
                    return bd.QueryToTable(strBuilder.ToString(), listaParametros);
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
                return null;
        }

        public DataTable ValidacaoPacotesIngresso(List<int> pacotesId)
        {
            if (pacotesId != null && pacotesId.Count > 0)
            {
                try
                {
                    string parametros = string.Empty;
                    foreach (var item in pacotesId)
                    {
                        if (String.IsNullOrEmpty(parametros))
                            parametros = item.ToString();
                        else
                            parametros += String.Format(", {0}", item.ToString());
                    }

                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("SELECT PermitirCancelamentoAvulso ");
                    strBuilder.Append("FROM tPacote ");

                    if (!String.IsNullOrEmpty(parametros))
                        strBuilder.Append(string.Format("WHERE ID IN ({0})", parametros));

                    CTLib.BD bd = new BD();
                    return bd.QueryToTable(strBuilder.ToString());
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
                return null;
        }

        public DataTable FormaPagamento(int VendaBilheteriaID)
        {
            if (VendaBilheteriaID > 0)
            {
                try
                {
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    string SQL = @"
                        SELECT 
                            Valor 
	                        ,tFormaPagamento.Nome AS FormaPagamento 
                        FROM 
                            tVendaBilheteriaFormaPagamento (NOLOCK) 
                        JOIN 
                            tFormaPagamento (NOLOCK) ON FormaPagamentoID = tFormaPagamento.ID 
                        JOIN 
                            tBandeira (NOLOCK) ON tFormaPagamento.BandeiraID = tBandeira.ID 
                        WHERE 
                            VendaBilheteriaID = @VendaBilheteriaID";

                    CTLib.BD bd = new BD();

                    parametros.Add(new SqlParameter()
                    {
                        DbType = DbType.Int32,
                        ParameterName = "@VendaBilheteriaID",
                        Value = VendaBilheteriaID
                    });

                    DataSet result = bd.QueryToDataSet(SQL, parametros);

                    return result.Tables[0];
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                return null;
            }
        }

        public List<CancelamentosCompraModel> ListaCancelamentos(int idVendaBilheteria)
        {
            if (idVendaBilheteria > 0)
            {
                  
                try
                {
                  
                    //var listaRetorno = new List<CancelamentosCompraModel>(); 
                    var listaRetorno = ListaCancelamentosAntigos(idVendaBilheteria);
                        var parametros = new List<SqlParameter>();
                        StringBuilder strBuilder = new StringBuilder();
                        strBuilder.Append("SELECT tCancelDevolucaoPendente.ID, ");
                        strBuilder.Append("case when tCancelDevolucaoPendente.StatusCancel = 'N' then '' else tVendaBilheteria.Senha end Senha, ");
                        strBuilder.Append("tCancelDevolucaoPendente.DataInsert, ");
                        strBuilder.Append("tCanal.Nome AS NomeCanal, ");
                        strBuilder.Append("tUsuario.Nome AS NomeUsuario, ");
                        strBuilder.Append("tCancelDevolucaoPendente.NumeroChamado, ");
                        strBuilder.Append("tCancelDevolucaoPendente.StatusCancel, tVendaBilheteria.ConvenienciaCancelada,tVendaBilheteria.EntregaCancelada, tVendaBilheteria.VendaCancelada ");
                        strBuilder.Append("FROM tCancelDevolucaoPendente (NOLOCK) ");
                        strBuilder.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tCancelDevolucaoPendente.VendaBilheteriaIDCancel = tVendaBilheteria.ID ");
                        strBuilder.Append("INNER JOIN tCanal (NOLOCK) ON tCancelDevolucaoPendente.CanalID = tCanal.ID ");
                        strBuilder.Append("INNER JOIN tUsuario (NOLOCK) ON tCancelDevolucaoPendente.UsuarioID = tUsuario.ID ");
                        strBuilder.Append("WHERE tCancelDevolucaoPendente.VendaBilheteriaIDVenda = @VendaBilheteriaID OR ");
                        strBuilder.Append("tCancelDevolucaoPendente.VendaBilheteriaIDCancel = @VendaBilheteriaID");

                        var bd = new BD();

                        parametros.Add(new SqlParameter()
                        {
                            DbType = DbType.Int32,
                            ParameterName = "@VendaBilheteriaID",
                            Value = idVendaBilheteria
                        });

                        

                        DataTable result = bd.QueryToTable(strBuilder.ToString(), parametros);

                        CancelamentosCompraModel _CancelamentosCompraModel;
                        
                        for (int i = 0; i < result.Rows.Count; i++)
                        {
                            _CancelamentosCompraModel = new CancelamentosCompraModel();
                            _CancelamentosCompraModel.ID = Convert.ToInt32(result.Rows[i]["ID"]);
                            _CancelamentosCompraModel.Senha = result.Rows[i]["Senha"].ToString();
                            _CancelamentosCompraModel.DataSolicitacao = Convert.ToDateTime(result.Rows[i]["DataInsert"]);
                            _CancelamentosCompraModel.Canal = result.Rows[i]["NomeCanal"].ToString();
                            _CancelamentosCompraModel.usuario = result.Rows[i]["NomeUsuario"].ToString();
                            _CancelamentosCompraModel.Chamado = result.Rows[i]["NumeroChamado"].ToString();
                            _CancelamentosCompraModel.Status = result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_PENDENTE
                                                                ? "Aguardando Devolução"
                                                                : result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_AUTOMATICO
                                                                ? "Devolução Automática"
                                                                : result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_CANCELADO
                                                                ? "Solicitação Cancelada"
                                                                : result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_NAO_AUTORIZADO
                                                                ? "Não Autorizado"
                                                                : "Devolução Efetuada";
                            /*
                            if (result.Rows[i]["VendaCancelada"].ToString() == "T")
                                _CancelamentosCompraModel.stringCancel += "\nVenda Cancelada";

                            if (result.Rows[i]["ConvenienciaCancelada"].ToString() == "T")
                                _CancelamentosCompraModel.stringCancel += "\nTaxa de Convêniencia Cancelada.";

                            if (result.Rows[i]["EntregaCancelada"].ToString() == "T")
                                _CancelamentosCompraModel.stringCancel += "\nTaxa de Entrega Cancelada";
                            */
                            listaRetorno.Add(_CancelamentosCompraModel);
                        }
                    return listaRetorno;
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
                return null;
        }

        private List<CancelamentosCompraModel> ListaCancelamentosAntigos(int vendaBilheteriaID)
        {
            try
            {
                CTLib.BD bd = new BD();
                var listaRetorno = new List<CancelamentosCompraModel>();

                string sql = @"SELECT 
	                            0 AS 'ID'
	                            ,tVendaBilheteria.Senha AS 'Senha'
	                            ,tVendaBilheteria.CalcDataVenda AS 'DataInsert'
	                            ,tCanal.Nome AS 'NomeCanal'
	                            ,tUsuario.Nome AS 'NomeUsuario'
	                            ,'' AS 'NumeroChamado'
	                            ,'D' AS 'StatusCancel', vbc.ConvenienciaCancelada, vbc.EntregaCancelada, vbc.VendaCancelada
                                FROM 
	                                tVendaBilheteria (NOLOCK) 
	                                INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID
	                                INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID
	                                INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID = tCanal.ID
	                                INNER JOIN (SELECT DISTINCT
					                                Min(Ingl2.VendaBilheteriaID) AS 'VendaBilheteriaID'
				                            FROM 
					                            tIngressoLog Ingl1 (NOLOCK) 
					                            INNER JOIN tIngressoLog Ingl2 (NOLOCK) ON Ingl1.IngressoID = Ingl2.IngressoID AND Ingl1.Acao = 'V' AND Ingl1.VendaBilheteriaID = @vendaBilheteriaID AND Ingl2.ID >= Ingl1.ID AND Ingl2.Acao = 'C'
				                            GROUP BY Ingl1.ID) Cancel ON tVendaBilheteria.ID = Cancel.VendaBilheteriaID
	                            INNER JOIN tUsuario (NOLOCK) ON (SELECT TOP 1 UsuarioID FROM tIngressoLog (NOLOCK) WHERE VendaBilheteriaID = Cancel.VendaBilheteriaID AND Acao = 'C') = tUsuario.ID
	                            LEFT JOIN tCancelDevolucaoPendente (NOLOCK) ON tVendaBilheteria.ID = tCancelDevolucaoPendente.VendaBilheteriaIDCancel
                                left join tVendaBilheteria  as vbc (NOLOCK) on vbc.id = @vendaBilheteriaID
                            WHERE
	                            tCancelDevolucaoPendente.ID IS NULL";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@vendaBilheteriaID",
                    Value = vendaBilheteriaID
                });


                DataTable result = bd.QueryToTable(sql, parametros);
                CancelamentosCompraModel _CancelamentosCompraModel;


                for (int i = 0; i < result.Rows.Count; i++)
                {
                    _CancelamentosCompraModel = new CancelamentosCompraModel();
                    _CancelamentosCompraModel.ID = Convert.ToInt32(result.Rows[i]["ID"]);
                    _CancelamentosCompraModel.Senha = result.Rows[i]["Senha"].ToString();
                    _CancelamentosCompraModel.DataSolicitacao = Convert.ToDateTime(result.Rows[i]["DataInsert"]);
                    _CancelamentosCompraModel.Canal = result.Rows[i]["NomeCanal"].ToString();
                    _CancelamentosCompraModel.usuario = result.Rows[i]["NomeUsuario"].ToString();
                    _CancelamentosCompraModel.Chamado = result.Rows[i]["NumeroChamado"].ToString();
                    _CancelamentosCompraModel.Status = result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_PENDENTE
                                                        ? "Aguardando Devolução"
                                                        : result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_AUTOMATICO
                                                        ? "Devolução Automática"
                                                        : result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_CANCELADO
                                                        ? "Solicitação Cancelada"
                                                        : result.Rows[i]["StatusCancel"].ToString().Trim() == CancelDevolucaoPendente.STATUS_CANCEL_NAO_AUTORIZADO
                                                        ? "Não Autorizado"
                                                        : "Devolução Efetuada";

                    if (result.Rows[i]["VendaCancelada"].ToString() == "T")
                        _CancelamentosCompraModel.stringCancel += "\nVenda Cancelada";

                    if (result.Rows[i]["ConvenienciaCancelada"].ToString() == "T")
                        _CancelamentosCompraModel.stringCancel += "\nTaxa de Convêniencia Cancelada.";

                    if (result.Rows[i]["EntregaCancelada"].ToString() == "T")
                        _CancelamentosCompraModel.stringCancel += "\nTaxa de Entrega Cancelada";                    

                    listaRetorno.Add(_CancelamentosCompraModel);
                }

                return listaRetorno;
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

        #region Historico de cancelamento
        public Dictionary<string, DateTime> ProtocoloCancelamento(int IdCancelamento, DateTime data, string canal, string obs)
        {
            var StatusRetorno = new Dictionary<string, DateTime>();
            string strItensCancelados = string.Empty;
            DataTable dtPendencia = dadosDetalheCancelamento(IdCancelamento);
            int tipo = 0;
            string StatusCancel = string.Empty;
            string chamado = string.Empty;

            if (dtPendencia != null && dtPendencia.Rows.Count > 0)
            {
                if (Convert.ToDecimal(dtPendencia.Rows[0]["VlrIngressoEstornado"]) > decimal.Zero)
                    strItensCancelados = strItensCancelados == string.Empty ? "Ingresso" : strItensCancelados + " / Ingresso";
                if (Convert.ToDecimal(dtPendencia.Rows[0]["VlrTxConvenienciaEstornado"]) > decimal.Zero)
                    strItensCancelados = strItensCancelados == string.Empty ? "Taxa Conveniência" : strItensCancelados + " / Taxa Conveniência";
                if (Convert.ToDecimal(dtPendencia.Rows[0]["VlrSeguroEstornado"]) > decimal.Zero)
                    strItensCancelados = strItensCancelados == string.Empty ? "Seguro" : strItensCancelados + " / Seguro";
                if (Convert.ToDecimal(dtPendencia.Rows[0]["VlrTxEntregaEstornado"]) > decimal.Zero)
                    strItensCancelados = strItensCancelados == string.Empty ? "Taxa Entrega" : strItensCancelados + " / Taxa Entrega";

                tipo = Convert.ToInt32(dtPendencia.Rows[0]["TipoCancelamento"]);
                StatusCancel = dtPendencia.Rows[0]["StatusCancel"].ToString();
                chamado = dtPendencia.Rows[0]["NumeroChamado"].ToString();
            }

            if (StatusCancel == IRLib.CancelamentoIngresso.CancelDevolucaoPendente.STATUS_CANCEL_NAO_AUTORIZADO)
                StatusRetorno.Add(String.Format("Solicitação de cancelamento aguardando autorização. Canal de Cancelamento: {0}" + Environment.NewLine + "Número do Chamado: " + chamado, canal), data);
            else
            if (tipo == (int)IRLib.CancelamentoIngresso.EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento)
                StatusRetorno.Add(String.Format("Solicitação de estorno. Canal de Cancelamento: {0}" + Environment.NewLine + "Itens Estornados: " + strItensCancelados, canal), data);
            else
                StatusRetorno.Add(String.Format("Solicitação de cancelamento. Canal de Cancelamento: {0}" + Environment.NewLine + "Itens Cancelados: " + strItensCancelados, canal), data);

            bool Continuar;
            var dataEtapaUm = ProtocoloCancelamentoEtapaUm(IdCancelamento, data, canal, out Continuar);
            if (dataEtapaUm > DateTime.MinValue)
                StatusRetorno.Add("Devolução Física de Ingresso feita a ingresso rápido.", dataEtapaUm);

            if (Continuar)
            {
                if (dataEtapaUm.Equals(DateTime.MinValue))
                {
                    //verificar se cancelamento e de taxa ou entrega.
                }

                //verificar o reembolso: Select nas tabelas estornoDadosDinheiro, estornoDadosDepositoBancario, estornoDadosCartaoCredito

                int vendaBilheteriaCancelID = VerificarVendaBilheteriaCancel(IdCancelamento);

                var reembolso = VerificaReembolsoEtapaTres(vendaBilheteriaCancelID, DateTime.MinValue);
                if (reembolso != null)
                {
                    string menssagem;
                    if (reembolso.ListaCartaoCredito != null && reembolso.ListaCartaoCredito.Count > 0)
                    {
                        menssagem =
                            string.Format("Início de Processo de Devolução Financeira do Ingresso. Forma de Devolução: Estorno de Crédito junto a operadora {0}.{1}{2}.",
                            reembolso.ListaCartaoCredito.FirstOrDefault().Bandeira, Environment.NewLine, definicaoStatusEstorno(reembolso.ListaCartaoCredito.FirstOrDefault().Status));

                        StatusRetorno.Add(menssagem, reembolso.ListaCartaoCredito.FirstOrDefault().DataCadastro);
                    }

                    if (reembolso.ListaDepositoBancario != null && reembolso.ListaDepositoBancario.Count > 0)
                    {
                        menssagem =
                            string.Format("Início de Processo de Devolução Financeira do Ingresso. Forma de Devolução: Depósito bancário junto ao banco {0}, Agência {1}, Conta Corrente {2}.",
                            reembolso.ListaDepositoBancario.FirstOrDefault().Banco, reembolso.ListaDepositoBancario.FirstOrDefault().Agencia, reembolso.ListaDepositoBancario.FirstOrDefault().Conta);

                        StatusRetorno.Add(menssagem, reembolso.ListaDepositoBancario.FirstOrDefault().DataCadastro);
                    }

                    if (reembolso.ListaEstornoDinheiro != null && reembolso.ListaEstornoDinheiro.Count > 0)
                    {
                        menssagem =
                            string.Format("Início de Processo de Devolução Financeira do Ingresso. Forma de Devolução: Espécies direto ao cliente {0}.",
                            reembolso.ListaEstornoDinheiro.FirstOrDefault().Cliente);

                        StatusRetorno.Add(menssagem, reembolso.ListaEstornoDinheiro.FirstOrDefault().DataCadastro);
                    }
                }
            }

            if (!string.IsNullOrEmpty(obs))
                StatusRetorno.Add(obs, DateTime.MinValue);

            if (StatusCancel == IRLib.CancelamentoIngresso.CancelDevolucaoPendente.STATUS_CANCEL_CANCELADO)
                StatusRetorno.Add("Solicitação de cancelamento foi cancelada.", DateTime.MinValue);

            return StatusRetorno;
        }

        private int VerificarVendaBilheteriaCancel(int IdCancelamento)
        {
            try
            {
                var parametros = new List<SqlParameter>();

                string query = @"SELECT vendabilheteriaIdCancel
                                    FROM tCancelDevolucaoPendente (NOLOCK)
                                    WHERE tCancelDevolucaoPendente.ID = @IDCancelDevolucao";

                var bd = new BD();

                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@IDCancelDevolucao",
                    Value = IdCancelamento
                });

                DataTable _DataTable = bd.QueryToTable(query, parametros);

                if (_DataTable != null && _DataTable.Rows.Count > 0)
                {
                    return _DataTable.AsEnumerable().FirstOrDefault().Field<int>("vendabilheteriaIdCancel");
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                    MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                return 0;

            }
            finally
            {
                bd.Fechar();
            }
        }

        private DataTable dadosDetalheCancelamento(int IdCancelamento)
        {
            DataTable retorno = null;
            try
            {
                var parametros = new List<SqlParameter>();

                string query = @"SELECT TipoCancelamento, VlrIngressoEstornado, VlrSeguroEstornado, VlrTxConvenienciaEstornado, VlrSeguroEstornado, VlrTxEntregaEstornado, StatusCancel, NumeroChamado
                                   FROM tCancelDevolucaoPendente (NOLOCK)
                                  WHERE ID =  @IDCancelDevolucao  ";

                var bd = new BD();

                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@IDCancelDevolucao",
                    Value = IdCancelamento
                });

                retorno = bd.QueryToTable(query, parametros);

            }
            catch (Exception ex)
            {
                if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                    MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        private DateTime ProtocoloCancelamentoEtapaUm(int IdCancelamento, DateTime data, string canal, out bool Continuar)
        {
            try
            {
                var parametros = new List<SqlParameter>();

                string query = @"SELECT tCancelDevolucaoPendente.ID,
                                		    tIngressoLog.IngressoID,
                                		    ([dbo].[StringToDateTime](tIngressoLog.TimeStamp)) AS TimeStamp,
                                		    tIngressoLog.Acao
                                    FROM tCancelDevolucaoPendente (NOLOCK)
                                    JOIN tCancelDevolucaoPendenteIngresso (NOLOCK) on tCancelDevolucaoPendente.ID = tCancelDevolucaoPendenteIngresso.CancelDevolucaoPendenteID
                                    JOIN tIngressoLog (NOLOCK) on tCancelDevolucaoPendenteIngresso.IngressoID = tIngressoLog.IngressoID AND (tCancelDevolucaoPendente.VendaBilheteriaIDvenda = tIngressoLog.VendaBilheteriaID or tCancelDevolucaoPendente.VendaBilheteriaIDCancel = tIngressoLog.VendaBilheteriaID)
                                    WHERE tCancelDevolucaoPendente.ID = @IDCancelDevolucao AND tIngressoLog.Acao IN ('C','I') ";

                var bd = new BD();

                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@IDCancelDevolucao",
                    Value = IdCancelamento
                });

                DataTable _DataTable = bd.QueryToTable(query, parametros);

                if (_DataTable != null && _DataTable.Rows.Count > 0)
                {
                    if (_DataTable.AsEnumerable().Any(o => o.Field<string>("Acao").Equals("I")) &&
                        _DataTable.AsEnumerable().Any(o => o.Field<string>("Acao").Equals("C")))
                    {
                        Continuar = true;
                        return _DataTable.AsEnumerable().FirstOrDefault(o => o.Field<string>("Acao").Equals("C")).Field<DateTime>("TimeStamp");
                    }
                    else if (_DataTable.AsEnumerable().Any(o => o.Field<string>("Acao").Equals("C")))
                    {
                        Continuar = true;
                        return DateTime.MinValue;
                    }
                    else
                    {
                        Continuar = false;
                        return DateTime.MinValue;
                    }
                }
                else
                {
                    Continuar = true;
                    return DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                    MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                Continuar = false;
                return DateTime.MinValue;

            }
            finally
            {
                bd.Fechar();
            }
        }

        private ReembolsoModel VerificaReembolsoEtapaTres(int IdCancelamento, DateTime data)
        {
            try
            {
                var parametros = new List<SqlParameter>();

                parametros.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@IDCancelDevolucao", Value = IdCancelamento });

                string parametroAdicional = string.Empty;
                if (data > DateTime.MinValue)
                {
                    parametros.Add(new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@DataInsercao", Value = data });

                    parametroAdicional = "AND DataInsert = @DataInsercao; ";
                }

                string query = @"SELECT	ID,
		                            VendaBilheteriaIDVenda,
		                            Bandeira,
		                            Cartao,
		                            Valor,
		                            Cliente,
		                            CPFCliente,
		                            CancelamentoPor,
		                            DataInsert,
		                            DataEnvio,
		                            Email,
                                    Status
                                 FROM EstornoDadosCartaoCredito (NOLOCK) AS CartaoCredito
                                 WHERE VendaBilheteriaIDCancel = @IDCancelDevolucao " + parametroAdicional +
                                 @"SELECT ID,
		                            VendaBilheteriaIDVenda,
		                            DataDeposito,
		                            Banco,
		                            Agencia,
		                            Conta,
		                            Valor,
		                            CPFCorrentista,
		                            NomeCorrentista,
		                            DataInsert,
		                            Email,
		                            CancelamentoPor,
                                    Status
                                 FROM EstornoDadosDepositoBancario (NOLOCK) AS DepositoBancario
                                 WHERE VendaBilheteriaIDCancel = @IDCancelDevolucao " + parametroAdicional +
                                 @"SELECT ID,
		                             VendaBilheteriaIDVenda,
		                             Cliente,
		                             CancelamentoPor,
		                             Email,
		                             DataInsert
                                 FROM EstornoDadosDinheiro (NOLOCK) AS Dinheiro
                                 WHERE VendaBilheteriaIDCancel = @IDCancelDevolucao " + parametroAdicional;

                var bd = new BD();

                DataSet _DataSet = bd.QueryToDataSet(query, parametros);

                if (_DataSet != null)
                {
                    var _ReembolsoModel = new ReembolsoModel();
                    DateTime datas;
                    if (_DataSet.Tables[0] != null && _DataSet.Tables[0].Rows.Count > 0)
                    {
                        _ReembolsoModel.ListaCartaoCredito = new List<IRLib.Codigo.TransporteModels.EstornoDadosCartaoCredito>();
                        IRLib.Codigo.TransporteModels.EstornoDadosCartaoCredito _EstornoDadosCartaoCredito;
                        for (int i = 0; i < _DataSet.Tables[0].Rows.Count; i++)
                        {
                            _EstornoDadosCartaoCredito = new IRLib.Codigo.TransporteModels.EstornoDadosCartaoCredito();
                            _EstornoDadosCartaoCredito.ID = Convert.ToInt32(_DataSet.Tables[0].Rows[i]["ID"].ToString());
                            _EstornoDadosCartaoCredito.IdVenda = Convert.ToInt32(_DataSet.Tables[0].Rows[i]["VendaBilheteriaIDVenda"].ToString());
                            _EstornoDadosCartaoCredito.Bandeira = _DataSet.Tables[0].Rows[i]["Bandeira"].ToString();
                            _EstornoDadosCartaoCredito.Cartao = _DataSet.Tables[0].Rows[i]["Cartao"].ToString();
                            _EstornoDadosCartaoCredito.Valor = Convert.ToDecimal(_DataSet.Tables[0].Rows[i]["Valor"].ToString());
                            _EstornoDadosCartaoCredito.Cliente = _DataSet.Tables[0].Rows[i]["Cliente"].ToString();
                            _EstornoDadosCartaoCredito.CPF = _DataSet.Tables[0].Rows[i]["CPFCliente"].ToString();
                            _EstornoDadosCartaoCredito.MotivoCancelamento = _DataSet.Tables[0].Rows[i]["CancelamentoPor"].ToString();

                            if (DateTime.TryParse(_DataSet.Tables[0].Rows[i]["DataInsert"].ToString(), out datas))
                                _EstornoDadosCartaoCredito.DataCadastro = datas;
                            if (DateTime.TryParse(_DataSet.Tables[0].Rows[i]["DataEnvio"].ToString(), out datas))
                                _EstornoDadosCartaoCredito.DataEnvio = datas;

                            _EstornoDadosCartaoCredito.Email = _DataSet.Tables[0].Rows[i]["Email"].ToString();
                            _EstornoDadosCartaoCredito.Status = _DataSet.Tables[0].Rows[i]["Status"].ToString();
                            _ReembolsoModel.ListaCartaoCredito.Add(_EstornoDadosCartaoCredito);
                        }
                    }
                    if (_DataSet.Tables[1] != null && _DataSet.Tables[1].Rows.Count > 0)
                    {
                        _ReembolsoModel.ListaDepositoBancario = new List<IRLib.Codigo.TransporteModels.EstornoDadosDepositoBancario>();
                        IRLib.Codigo.TransporteModels.EstornoDadosDepositoBancario _EstornoDadosDepositoBancario;
                        for (int i = 0; i < _DataSet.Tables[1].Rows.Count; i++)
                        {
                            _EstornoDadosDepositoBancario = new IRLib.Codigo.TransporteModels.EstornoDadosDepositoBancario();
                            _EstornoDadosDepositoBancario.ID = Convert.ToInt32(_DataSet.Tables[1].Rows[i]["ID"].ToString());
                            _EstornoDadosDepositoBancario.IdVenda = Convert.ToInt32(_DataSet.Tables[1].Rows[i]["VendaBilheteriaIDVenda"].ToString());
                            _EstornoDadosDepositoBancario.Banco = _DataSet.Tables[1].Rows[i]["Banco"].ToString();
                            _EstornoDadosDepositoBancario.Agencia = _DataSet.Tables[1].Rows[i]["Agencia"].ToString();
                            _EstornoDadosDepositoBancario.Conta = Convert.ToInt32(_DataSet.Tables[1].Rows[i]["Conta"].ToString());
                            _EstornoDadosDepositoBancario.Valor = Convert.ToDecimal(_DataSet.Tables[1].Rows[i]["Valor"].ToString());
                            _EstornoDadosDepositoBancario.CPF = _DataSet.Tables[1].Rows[i]["CPFCorrentista"].ToString();
                            _EstornoDadosDepositoBancario.Cliente = _DataSet.Tables[1].Rows[i]["NomeCorrentista"].ToString();

                            if (DateTime.TryParse(_DataSet.Tables[1].Rows[i]["DataDeposito"].ToString(), out datas))
                                _EstornoDadosDepositoBancario.DataDeposito = datas;
                            if (DateTime.TryParse(_DataSet.Tables[1].Rows[i]["DataInsert"].ToString(), out datas))
                                _EstornoDadosDepositoBancario.DataCadastro = datas;

                            _EstornoDadosDepositoBancario.Email = _DataSet.Tables[1].Rows[i]["Email"].ToString();
                            _EstornoDadosDepositoBancario.MotivoCancelamento = _DataSet.Tables[1].Rows[i]["CancelamentoPor"].ToString();
                            _EstornoDadosDepositoBancario.Status = _DataSet.Tables[1].Rows[i]["Status"].ToString();
                            _ReembolsoModel.ListaDepositoBancario.Add(_EstornoDadosDepositoBancario);
                        }
                    }
                    if (_DataSet.Tables[2] != null && _DataSet.Tables[2].Rows.Count > 0)
                    {
                        _ReembolsoModel.ListaEstornoDinheiro = new List<IRLib.Codigo.TransporteModels.EstornoDadosDinheiro>();
                        IRLib.Codigo.TransporteModels.EstornoDadosDinheiro _EstornoDadosDinheiro;
                        for (int i = 0; i < _DataSet.Tables[2].Rows.Count; i++)
                        {
                            _EstornoDadosDinheiro = new IRLib.Codigo.TransporteModels.EstornoDadosDinheiro();
                            _EstornoDadosDinheiro.ID = Convert.ToInt32(_DataSet.Tables[2].Rows[i]["ID"].ToString());
                            _EstornoDadosDinheiro.IdVenda = Convert.ToInt32(_DataSet.Tables[2].Rows[i]["VendaBilheteriaIDVenda"].ToString());
                            _EstornoDadosDinheiro.Cliente = _DataSet.Tables[2].Rows[i]["Cliente"].ToString();
                            _EstornoDadosDinheiro.MotivoCancelamento = _DataSet.Tables[2].Rows[i]["CancelamentoPor"].ToString();
                            _EstornoDadosDinheiro.Email = _DataSet.Tables[2].Rows[i]["Email"].ToString();

                            if (DateTime.TryParse(_DataSet.Tables[2].Rows[i]["DataInsert"].ToString(), out datas))
                                _EstornoDadosDinheiro.DataCadastro = datas;

                            _ReembolsoModel.ListaEstornoDinheiro.Add(_EstornoDadosDinheiro);
                        }
                    }
                    return _ReembolsoModel;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                    MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                return null;

            }
            finally
            {
                bd.Fechar();
            }
        }

        private string definicaoStatusEstorno(string Status)
        {
            switch (Status)
            {
                case EstruturaCancelamento.STATUS_ESTORNO_AUTOMATICO:
                    return "Estorno automático";
                case EstruturaCancelamento.STATUS_ESTORNO_SOLICITADO:
                    return "Estorno solicitado aguardando devolução de Ingressos";
                case EstruturaCancelamento.STATUS_ESTORNO_PENDENTE:
                    return "Estorno pendente";
                case EstruturaCancelamento.STATUS_ESTORNO_CANCELADO:
                    return "Solicitação de estorno foi cancelada";
                default:
                    return string.Empty;
            }
        }

        #endregion

        public DataTable DetalhaRastramentoVendaBilheteria(int idVendaBilheteria, int paginaAtual, int totItemPorPagina, out int totalRegistros)
        {
            if (idVendaBilheteria > 0)
            {
                try
                {
                    CTLib.BD bd = new BD();

                    List<SqlParameter> parametrosEntrega = new List<SqlParameter>();

                    parametrosEntrega.Add(new SqlParameter()
                    {
                        DbType = DbType.Int32,
                        ParameterName = "@VendaBilheteriaID",
                        Value = idVendaBilheteria
                    });

                    string sqlQuery = @"
                        SELECT
                            i.Codigo AS IngressoCodigo,
	                        i.Status AS IngressoStatus,
	                        CASE vb.EntregaControleID 
		                        WHEN 0 THEN 
			                        'B' 
		                        ELSE
			                        e.Tipo
	                        END AS EntregaTipo,
	                        pv.Nome,
                            pv.Endereco,
                            pv.Bairro,
                            pv.Cidade,
                            pv.Estado,
                            c.Nome AS 'Canal',
                            i.ID AS IngressoID,
	                        dbo.StringToDateTime(il.TimeStamp) AS DateTimeAcao,
	                        u.Nome AS NomeUsuario
                        FROM
	                        tIngresso (NOLOCK) i INNER JOIN
	                        tVendaBilheteria (NOLOCK) vb ON i.VendabilheteriaID = vb.ID LEFT JOIN
	                        tEntregaControle (NOLOCK) ec ON vb.EntregaControleID = ec.ID LEFT JOIN
	                        tPontoVenda (NOLOCK) pv ON vb.PdvID = pv.ID LEFT JOIN 
	                        tEntrega (NOLOCK) e ON ec.EntregaID = e.ID LEFT JOIN
	                        tIngressoLog (NOLOCK) il ON il.IngressoID = i.ID AND il.VendabilheteriaID = vb.ID AND il.Acao = 'I' AND il.ID = (Select MAX(ID) from tIngressoLog(NOLOCK) where IngressoID = i.ID  AND Acao = 'I') LEFT JOIN
	                        tCanal (NOLOCK) c ON c.ID = il.CanalID LEFT JOIN
	                        tCaixa (NOLOCK) cx ON cx.ID = vb.CaixaID LEFT JOIN
	                        tUsuario (NOLOCK) u ON u.ID = cx.UsuarioID
                        WHERE
	                        vb.ID = @VendaBilheteriaID";

                    List<EntregaAuxiliar> entregaInfo = new List<EntregaAuxiliar>();
                    using (IDataReader dr = bd.Consulta(sqlQuery, parametrosEntrega))
                    {
                        while (dr.Read())
                        {
                            EntregaAuxiliar ea = new EntregaAuxiliar();
                            ea.IngressoCodigo = dr.GetString(0);
                            ea.IngressoStatus = dr.GetString(1);
                            ea.EntregaTipo = dr.GetString(2);
                            ea.Nome = string.Format("{0} Endereço: {1} Bairro: {2} {3}-{4}",
                                                        (dr.IsDBNull(3) ? null : dr.GetString(3)), (dr.IsDBNull(4) ? null : dr.GetString(4)), (dr.IsDBNull(5) ? null : dr.GetString(5)), (dr.IsDBNull(6) ? null : dr.GetString(6)), (dr.IsDBNull(7) ? null : dr.GetString(7)));// dr.IsDBNull(3) ? null : dr.GetString(3);
                            ea.Canal = dr.IsDBNull(8) ? "" : dr.GetString(8);
                            ea.IngressoID = dr.GetInt32(9);
                            ea.DateTimeAcao = dr.GetDateTime(10);
                            ea.NomeUsuario = dr.GetString(11);
                            entregaInfo.Add(ea);
                        }
                    }

                    string tiposSemrastreamento = "BR";
                    if (entregaInfo.Count > 0 && tiposSemrastreamento.Contains(entregaInfo.FirstOrDefault().EntregaTipo))
                    {
                        DataTable dataTable = new DataTable();

                        dataTable.Columns.Add("RowNumber");
                        dataTable.Columns.Add("ID");
                        dataTable.Columns.Add("VendaBilheteriaID");
                        dataTable.Columns.Add("Tipo");
                        dataTable.Columns.Add("EmailEnviado");
                        dataTable.Columns.Add("DataHoraOcorrencia");
                        dataTable.Columns.Add("CodigoRastreamento");
                        dataTable.Columns.Add("StatusTexto", typeof(string));

                        foreach (EntregaAuxiliar ea in entregaInfo)
                        {
                            string status = null;
                            if (ea.EntregaTipo == "B")
                            {
                                if (ea.IngressoStatus == "I")
                                {
                                    status = "Ingresso " + ea.IngressoCodigo + " impresso em " + ea.Canal + " em " + ea.DateTimeAcao.ToString("dd/MM/yyyy") + " e " + ea.DateTimeAcao.ToString("HH:mm:ss") + ", pelo usuário " + ea.NomeUsuario;
                                }
                                else if (ea.IngressoStatus == "V")
                                {
                                    status = "Ingresso " + ea.IngressoCodigo + " aguardando impressão";
                                }
                            }
                            else
                            {
                                if (ea.IngressoStatus == "I")
                                {
                                    status = "Seu ingresso " + ea.IngressoCodigo + " impresso em " + ea.Nome + " em " + ea.DateTimeAcao.ToString("dd/MM/yyyy") + " e " + ea.DateTimeAcao.ToString("HH:mm:ss") + ", pelo usuário " + ea.NomeUsuario;
                                }
                                else if (ea.IngressoStatus == "V")
                                {
                                    status = "Seu ingresso " + ea.IngressoCodigo + " aguardando impressão";
                                }
                            }
                            DataRow row = dataTable.NewRow();
                            row["StatusTexto"] = status;
                            List<SqlParameter> paramEntrega = new List<SqlParameter>();
                            paramEntrega.Add(new SqlParameter()
                            {
                                DbType = DbType.Int32,
                                ParameterName = "@IngressoID",
                                Value = ea.IngressoID
                            });
                            paramEntrega.Add(new SqlParameter()
                            {
                                DbType = DbType.String,
                                ParameterName = "@Acao",
                                Value = ea.IngressoStatus
                            });
                            using (IDataReader dr = bd.Consulta("SELECT TOP 1 dbo.StringToDateTime(TimeStamp) FROM tIngressoLog(NOLOCK) WHERE IngressoID = @IngressoID AND Acao = @Acao ORDER BY ID DESC;", paramEntrega))
                            {
                                if (dr.Read())
                                {
                                    row["DataHoraOcorrencia"] = dr.GetDateTime(0);
                                }
                            }
                            dataTable.Rows.Add(row);
                        }
                        totalRegistros = dataTable.Rows.Count;
                        return dataTable;
                    }
                    else
                    {
                        StringBuilder strBuilder = new StringBuilder();
                        strBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY tVendaBilheteriaEntrega.ID) AS RowNumber, ");
                        strBuilder.Append("ID, ");
                        strBuilder.Append("VendaBilheteriaID, ");
                        strBuilder.Append("Tipo, ");
                        strBuilder.Append("DataHoraOcorrencia, ");
                        strBuilder.Append("EmailEnviado, ");
                        strBuilder.Append("CodigoRastreamento, ");
                        strBuilder.Append("StatusTexto ");
                        strBuilder.Append("FROM tVendaBilheteriaEntrega (NOLOCK) ");
                        strBuilder.Append("WHERE (VendaBilheteriaID = @VendaBilheteriaID)) AS TBL ");
                        strBuilder.Append("WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage) ");
                        strBuilder.Append("SELECT COUNT(tVendaBilheteriaEntrega.ID) as totalRegistros ");
                        strBuilder.Append("FROM tVendaBilheteriaEntrega (NOLOCK) ");
                        strBuilder.Append("WHERE(VendaBilheteriaID = @VendaBilheteriaID); ");
                        List<SqlParameter> parametros = new List<SqlParameter>();
                        parametros.Add(new SqlParameter()
                        {
                            DbType = DbType.Int32,
                            ParameterName = "@VendaBilheteriaID",
                            Value = idVendaBilheteria
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
                        DataSet result = bd.QueryToDataSet(strBuilder.ToString(), parametros);
                        totalRegistros = Convert.ToInt32(result.Tables[1].Rows[0][0]);
                        return result.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    totalRegistros = 0;
                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                totalRegistros = 0;
                return null;
            }
        }

        public DataTable DetalhaAgregados(int idVendaBilheteria, int paginaAtual, int totItemPorPagina, out int totalRegistros)
        {
            if (idVendaBilheteria > 0)
            {
                try
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY tVendaBilheteriaAgregados.ID) AS RowNumber, ");
                    strBuilder.Append("ID, ");
                    strBuilder.Append("VendaBilheteriaID, ");
                    strBuilder.Append("Nome, ");
                    strBuilder.Append("CPF, ");
                    strBuilder.Append("Email, ");
                    strBuilder.Append("UsuarioId, ");
                    strBuilder.Append("Data, ");
                    strBuilder.Append("Telefone ");
                    strBuilder.Append("FROM tVendaBilheteriaAgregados (NOLOCK) ");
                    strBuilder.Append("WHERE (VendaBilheteriaID = @VendaBilheteriaID)) AS TBL ");
                    strBuilder.Append("WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage) ");
                    strBuilder.Append("SELECT COUNT(tVendaBilheteriaAgregados.ID) as totalRegistros ");
                    strBuilder.Append("FROM tVendaBilheteriaAgregados (NOLOCK) ");
                    strBuilder.Append("WHERE(VendaBilheteriaID = @VendaBilheteriaID); ");

                    CTLib.BD bd = new BD();

                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter()
                    {
                        DbType = DbType.Int32,
                        ParameterName = "@VendaBilheteriaID",
                        Value = idVendaBilheteria
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

                    DataSet result = bd.QueryToDataSet(strBuilder.ToString(), parametros);
                    totalRegistros = Convert.ToInt32(result.Tables[1].Rows[0][0]);

                    return result.Tables[0];
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    totalRegistros = 0;
                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                totalRegistros = 0;
                return null;
            }
        }

        public bool CadastraAgregados(int id, int idVendaBilheteria, string Nome, string CPF, string Email, int UsuarioId, string Telefone, string Data)
        {
            if (idVendaBilheteria > 0)
            {
                try
                {
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    StringBuilder strBuilder = new StringBuilder();
                    if (id.Equals(0))
                    {
                        strBuilder.Append("INSERT INTO tVendaBilheteriaAgregados (VendaBilheteriaId, Nome, CPF, Email, UsuarioId, Data, Telefone)");
                        strBuilder.Append("VALUES(@VendaBilheteriaId, @Nome, @CPF, @Email, @UsuarioId, @Data, @Telefone)");
                    }
                    else
                    {
                        strBuilder.Append("UPDATE tVendaBilheteriaAgregados SET ");
                        strBuilder.Append("VendaBilheteriaId = @VendaBilheteriaId, ");
                        strBuilder.Append("Nome = @Nome, ");
                        strBuilder.Append("CPF = @CPF, ");
                        strBuilder.Append("Email = @Email, ");
                        strBuilder.Append("UsuarioId = @UsuarioId, ");
                        strBuilder.Append("Data = @Data, ");
                        strBuilder.Append("Telefone = @Telefone ");
                        strBuilder.Append("WHERE ID = @ID");

                        parametros.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@ID", Value = id });
                    }

                    CTLib.BD bd = new BD();

                    parametros.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@VendaBilheteriaId", Value = idVendaBilheteria });
                    parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@Nome", Value = Nome });
                    parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@CPF", Value = CPF });
                    parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@Email", Value = Email });
                    parametros.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@UsuarioId", Value = UsuarioId });
                    parametros.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@Telefone", Value = Telefone });
                    parametros.Add(new SqlParameter() { DbType = DbType.DateTime, ParameterName = "@Data", Value = Data });

                    return bd.Executar(strBuilder.ToString(), parametros) > 0;

                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return false;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                return false;
            }
        }

        public bool ExcluiAgregado(int id)
        {
            if (id > 0)
            {
                try
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("DELETE FROM tVendaBilheteriaAgregados ");
                    strBuilder.Append("WHERE ID = @ID");

                    List<SqlParameter> parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@ID", Value = id });

                    CTLib.BD bd = new BD();
                    return bd.Executar(strBuilder.ToString(), parametros) > 0;
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));

                    return false;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                return false;
            }
        }


        public DataSet MontaDadosItensEIngressosVendidos(List<int> ingressoIDs)
        {

            int vendaID = 0;

            if (ingressoIDs != null && ingressoIDs.Count > 0)
            {
                BD bd = null;

                DataSet retorno = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();



                string sql = string.Format(@"SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor,
                                vb.EntregaControleID, vb.EntregaAgendaID, vb.ValorTotal, vb.Status, vb.Senha, vb.DataVenda, vb.Obs,
                                c.Nome AS Cliente, vb.ClienteID , c.Email, tCanal.Nome AS Canal, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, vb.NivelRisco,
                                IsNull(e.Tipo, '') AS TaxaEntregaTipo , c.CNPJ , c.NomeFantasia, IsNull(vb.PagamentoProcessado, 'T') AS PagamentoProcessado, TaxaProcessamentoValor, TaxaProcessamentoCancelada, ISNULL(vb.ValorSeguro, 0) AS ValorSeguro, ISNULL(TransactionID,'') as TransactionID
                                FROM tVendaBilheteria AS vb (NOLOCK)
                                LEFT JOIN tVendaBilheteriaFormaPagamento AS vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID
                                INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID
                                INNER JOIN tUsuario (NOLOCK) AS u ON u.ID = tCaixa.UsuarioID
                                INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID
                                INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID
                                INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID
                                LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
                                LEFT JOIN tEntregaControle ec (NOLOCK) ON ec.ID = vb.EntregaControleID
                                LEFT JOIN tEntrega e (NOLOCK) ON e.ID = ec.EntregaID
                                WHERE tIngresso.ID in ({0})", string.Join(",", ingressoIDs));

                try
                {
                    bd = new BD();
                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {
                        DataRow linha = info.NewRow();
                        vendaID = bd.LerInt("ID");
                        linha["ID"] = vendaID;
                        linha[TAXA_CONV_VALOR_TOTAL] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linha[TAXA_ENTREGA_VALOR] = bd.LerDecimal("TaxaEntregaValor");
                        linha[ENTREGA_CONTROLE_ID] = bd.LerInt("EntregaControleID");
                        linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                        linha[STATUS_VENDA] = bd.LerString("Status");
                        String nomeCliente = bd.LerString("Cliente");
                        if (!String.IsNullOrEmpty(nomeCliente))
                            linha[CLIENTE] = bd.LerString("Cliente");
                        else
                            linha[CLIENTE] = bd.LerString("NomeFantasia");

                        linha[CLIENTEID] = bd.LerInt(CLIENTEID);
                        linha[EMAIL] = bd.LerString("Email");
                        linha[CANAL] = bd.LerString("Canal");
                        linha[VENDEDOR] = bd.LerString("Vendedor");
                        linha[SENHA] = bd.LerString("Senha");
                        linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                        linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                        linha[OBSERVACAO] = bd.LerString("Obs");
                        linha[NIVELRISCO] = bd.LerInt("NivelRisco");
                        linha[TAXA_ENTREGA_TIPO] = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                        linha[ENTREGA_AGENDA_ID] = bd.LerInt("EntregaAgendaID");
                        linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                        linha[TAXAPROCESSAMENTOCANCELADA] = bd.LerBoolean(TAXAPROCESSAMENTOCANCELADA);
                        linha[TAXAPROCESSAMENTOVALOR] = bd.LerDecimal(TAXAPROCESSAMENTOVALOR);
                        linha[VALORSEGURO] = bd.LerDecimal(VALORSEGURO);
                        linha[TRANSACTION_ID] = bd.LerString(TRANSACTION_ID);

                        info.Rows.Add(linha);

                    }
                    bd.Fechar();

                    string sqlIngressos = string.Format(@"SELECT tIngresso.VendaBilheteriaID, tIngresso.Status, LOG.ID, LOG.IngressoID, LOG.VendaBilheteriaItemID, tVendaBilheteria.Obs,
                                            tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacao.DisponivelAjuste, tSetor.Nome AS Setor, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tPreco.Nome AS Preco, tPreco.Valor, tIngresso.Codigo, tCortesia.Nome AS Cortesia, tBloqueio.Nome AS Bloqueio, tPacote.ID AS PacoteID, tPacote.Nome AS Pacote, tVendaBilheteriaItem.TaxaConveniencia, tVendaBilheteriaItem.TaxaConvenienciaValor, tVendaBilheteriaItem.TaxaComissao, tVendaBilheteriaItem.ComissaoValor, tVendaBilheteriaItem.PacoteGrupo,
                                            tEvento.TipoCodigoBarra, tIngresso.CodigoBarra, tIngresso.ApresentacaoSetorID, tIngresso.EventoID , tIngresso.AssinaturaClienteID, ISNULL(tAssinatura.Nome, '') as AssinaturaNome
                                            FROM tIngressolog AS LOG (NOLOCK)
                                            INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = LOG.IngressoID
                                            INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
                                            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
                                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                                            LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID
                                            LEFT JOIN tCortesia (NOLOCK) ON tCortesia.ID = tIngresso.CortesiaID
                                            LEFT JOIN tBloqueio (NOLOCK) ON tBloqueio.ID = tIngresso.BloqueioID
                                            INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID
                                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID
                                            LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID
                                            LEFT JOIN tAssinaturaCliente (NOLOCK) ON tAssinaturaCliente.ID = tIngresso.AssinaturaClienteID
                                            LEFT JOIN tAssinatura (NOLOCK) ON tAssinatura.ID = tAssinaturaCliente.AssinaturaID
                                            WHERE LOG.IngressoID in ({0}) AND LOG.VendaBilheteriaID = {1}", string.Join(",", ingressoIDs), vendaID);

                    bd.Consulta(sqlIngressos);

                    DataTable tabela = EstruturaTabela();

                    while (bd.Consulta().Read())
                    {
                        if (bd.LerInt("VendaBilheteriaID") == vendaID)
                        {

                            DataRow linha = tabela.NewRow();

                            linha[VENDA_BILHETERIA_ID] = vendaID;

                            linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                            linha[INGRESSOLOG_ID] = bd.LerInt("ID");
                            linha[DISPONIVEL_AJUSTE] = bd.LerString("DisponivelAjuste");
                            linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");

                            if (bd.LerInt("AssinaturaClienteID") > 0)
                            {
                                linha[EVENTO] = bd.LerString("AssinaturaNome");
                                linha[HORARIO] = "-";
                                linha[BLOQUEIO] = "-";
                                linha[ASSINATURA_CLIENTE_ID] = bd.LerInt("AssinaturaClienteID");
                            }
                            else
                            {
                                linha[ASSINATURA_CLIENTE_ID] = 0;
                                linha[EVENTO] = bd.LerString("Evento");
                                linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                                if (bd.LerString("Bloqueio") != "")
                                    linha[BLOQUEIO] = bd.LerString("Bloqueio");
                            }

                            linha[PACOTE] = bd.LerString("Pacote");
                            linha[PACOTEID] = bd.LerInt("PacoteID");

                            linha[SETOR_PRODUTO] = bd.LerString("Setor");
                            linha[CODIGO] = bd.LerString("Codigo");
                            linha[PRECO] = bd.LerString("Preco");
                            if (bd.LerString("Cortesia") != "")
                                linha[CORTESIA] = bd.LerString("Cortesia");
                            linha[PRECOID] = bd.LerInt("PrecoID");
                            linha[CORTESIAID] = bd.LerInt("CortesiaID");
                            linha[BLOQUEIOID] = bd.LerInt("BloqueioID");
                            linha[VALOR] = bd.LerDecimal("Valor");
                            linha[STATUS] = bd.LerString("Status");
                            linha[CONV] = bd.LerInt("TaxaConveniencia");
                            linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                            linha[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                            linha[COMISSAO_VALOR] = bd.LerDecimal("ComissaoValor");
                            linha[OBSERVACAO] = bd.LerString("Obs");
                            linha[PACOTE_GRUPO] = bd.LerInt("PacoteGrupo");
                            linha[TIPO_CODIGO_BARRA] = bd.LerString("TipoCodigoBarra");
                            linha[CODIGO_BARRA] = bd.LerString("CodigoBarra");
                            linha[EVENTO_ID] = bd.LerInt(EVENTO_ID);
                            linha[APRESENTACAO_SETOR_ID] = bd.LerInt(APRESENTACAO_SETOR_ID);
                            tabela.Rows.Add(linha);
                        }
                    }

                    DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);

                    int pacoteIdAtual = 0;
                    int pacoteReservaID = 0;
                    int pacoteGrupo = 0;
                    int vBItemId = 0;
                    int assinaturaClienteID = 0;
                    List<int> lstAssinaturaClienteID = new List<int>();

                    foreach (DataRow linha in tabelaTmp.Rows)
                    {
                        int vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                        DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                        DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();
                        DataRow novoInfoPacote = retorno.Tables[TABELA_INFO_PACOTE].NewRow();

                        novoInfoPacote[PACOTE_GRUPO] = 0;

                        vBItemId = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = vBItemId;

                        assinaturaClienteID = (int)linhas[0][ASSINATURA_CLIENTE_ID];

                        object valor = DBNull.Value;
                        object valorIndividual = DBNull.Value;
                        if (assinaturaClienteID > 0)
                        {
                            //soma valor Assinatura
                            valor = tabela.Compute("SUM(Valor)", ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                            novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                            valorIndividual = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                        }
                        else
                        {
                            //somar o valor
                            valor = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                            novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                        }

                        Pacote oP = new Pacote();

                        if (oP.CancelamentoAvulso((int)linhas[0][PACOTEID]))
                        {
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            novoItemGrid[TIPO] = TIPO_PACOTE;

                            novoInfoPacote[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                            novoInfoPacote[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                            novoInfoPacote[TIPO] = TIPO_INGRESSO;
                            if (linhas.Length > 1)
                            {
                                novoInfoPacote[TIPO_LUGAR] = Setor.MesaFechada;
                            }
                            else
                            {
                                novoInfoPacote[TIPO_LUGAR] = Setor.Pista;
                            }

                            novoInfoPacote[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                            novoInfoPacote[HORARIO] = (string)linhas[0][HORARIO];
                            novoInfoPacote[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            novoInfoPacote[CODIGO] = (string)linhas[0][CODIGO];
                            if (linhas[0][CORTESIA] != DBNull.Value)
                                novoInfoPacote[CORTESIA] = (string)linhas[0][CORTESIA];
                            if (linhas[0][BLOQUEIO] != DBNull.Value)
                                novoInfoPacote[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                            novoInfoPacote[PRECO] = (string)linhas[0][PRECO];
                            novoInfoPacote[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoInfoPacote[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoInfoPacote[CONV] = (int)linhas[0][CONV];
                            novoInfoPacote[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                            novoInfoPacote[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoInfoPacote[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                            novoInfoPacote[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                            novoInfoPacote[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];

                            BD bdAux = new BD();

                            string sqlValor = string.Format(@"SELECT SUM(tPreco.Valor) as Valor,
                                                        tVendaBilheteriaItem.TaxaConveniencia,
                                                        SUM(tVendaBilheteriaItem.TaxaConvenienciaValor) as TaxaConvenienciaValor,
                                                        tVendaBilheteriaItem.TaxaComissao,
                                                        SUM(tVendaBilheteriaItem.ComissaoValor) as ComissaoValor
                                                        FROM tIngressolog AS LOG (NOLOCK)
                                                        LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID
                                                        INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID
                                                        INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID
                                                        LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID
                                                        WHERE LOG.IngressoID in ({0}) AND LOG.VendaBilheteriaID = {1} AND  tVendaBilheteriaItem.PacoteID = {2} AND tVendaBilheteriaItem.PacoteGrupo = {3}
                                                        group by tVendaBilheteriaItem.TaxaConveniencia, tVendaBilheteriaItem.TaxaComissao", string.Join(",", ingressoIDs), vendaID, (int)linhas[0][PACOTEID], (int)linhas[0][PACOTE_GRUPO]);

                            bdAux.Consulta(sqlValor);

                            while (bdAux.Consulta().Read())
                            {
                                novoItemGrid[CONV] = bdAux.LerInt("TaxaConveniencia");
                                novoItemGrid[VALOR_CONV] = bdAux.LerDecimal("TaxaConvenienciaValor");
                                novoItemGrid[TAXA_COMISSAO] = bdAux.LerInt("TaxaComissao");
                                novoItemGrid[COMISSAO_VALOR] = bdAux.LerDecimal("ComissaoValor");
                                novoItemGrid[VALOR] = bdAux.LerDecimal("Valor");
                            }

                            bdAux.Fechar();

                            novoItemGrid[PACOTE_COMPLETO] = true;
                            novoItemGrid[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];
                            novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoItemGrid[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;
                        }
                        else
                        {
                            if ((int)linhas[0][PACOTEID] != 0)
                            {
                                novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                                novoItemGrid[TIPO] = TIPO_PACOTE;
                            }
                            else
                            {
                                novoItemGrid[TIPO] = TIPO_INGRESSO;

                                if (linhas.Length > 1)
                                    novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                                else
                                    novoItemGrid[TIPO_LUGAR] = Setor.Pista;

                                novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                                novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                                novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                                novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                                if (linhas[0][CORTESIA] != DBNull.Value)
                                    novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                                if (linhas[0][BLOQUEIO] != DBNull.Value)
                                    novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                                novoItemGrid[PRECO] = (string)linhas[0][PRECO];
                            }
                            novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoItemGrid[CONV] = (int)linhas[0][CONV];
                            novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                            novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoItemGrid[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                            novoItemGrid[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                            novoItemGrid[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;

                        }

                        if (pacoteIdAtual != oP.Control.ID)
                        {
                            pacoteGrupo = 0;
                        }

                        if (!lstAssinaturaClienteID.Contains(assinaturaClienteID) && (!oP.PermitirCancelamentoAvulso.Valor || pacoteGrupo != (int)novoInfoPacote[PACOTE_GRUPO]))
                        {
                            retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                            pacoteIdAtual = oP.Control.ID;
                            pacoteReservaID = (int)novoItemGrid[RESERVAID];
                            pacoteGrupo = (int)novoInfoPacote[PACOTE_GRUPO];

                            if (assinaturaClienteID > 0)
                                lstAssinaturaClienteID.Add(assinaturaClienteID);
                        }

                        if (assinaturaClienteID > 0)
                        {
                            DataRow[] dr = retorno.Tables[TABELA_RESERVA].Select(ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                            if (dr.Length > 0)
                            {
                                int possuiReservaID = (int)dr[0][RESERVAID];
                                if (possuiReservaID > 0)
                                    pacoteReservaID = possuiReservaID;
                            }

                            DataRow novoAssinatura = retorno.Tables[TABELA_INFO_ASSINATURA].NewRow();
                            for (int i = 0; i < novoItemGrid.ItemArray.Length; i++)
                            {
                                novoAssinatura[i] = novoItemGrid[i];
                            }

                            novoAssinatura[VALOR] = (valorIndividual != DBNull.Value) ? Convert.ToDecimal(valorIndividual) : 0;
                            novoAssinatura[RESERVAID] = pacoteReservaID;

                            retorno.Tables[TABELA_INFO_ASSINATURA].Rows.Add(novoAssinatura);
                        }

                        for (int i = 0; i < linhas.Length; i++)
                        {
                            DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                            novoItemReserva[RESERVAID] = oP.PermitirCancelamentoAvulso.Valor || assinaturaClienteID > 0 ? pacoteReservaID : (int)novoItemGrid[RESERVAID];
                            novoItemReserva[VENDA_BILHETERIA_ITEM_ID] = vBItemId;
                            novoItemReserva[INGRESSOID] = (int)linhas[i][INGRESSO_ID];
                            novoItemReserva[PACOTEID] = (int)linhas[i][PACOTEID];
                            novoItemReserva[PRECOID] = (int)linhas[i][PRECOID];
                            novoItemReserva[CORTESIAID] = (int)linhas[i][CORTESIAID];
                            novoItemReserva[BLOQUEIOID] = (int)linhas[i][BLOQUEIOID];
                            novoItemReserva[TIPO_CODIGO_BARRA] = linhas[i][TIPO_CODIGO_BARRA];
                            novoItemReserva[CODIGO_BARRA] = linhas[i][CODIGO_BARRA];
                            novoItemReserva[EVENTO_ID] = linhas[i][EVENTO_ID];
                            novoItemReserva[APRESENTACAO_SETOR_ID] = linhas[i][APRESENTACAO_SETOR_ID];
                            novoItemReserva[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;

                            retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                        }


                        if (oP.PermitirCancelamentoAvulso.Valor)
                        {
                            novoInfoPacote[RESERVAID] = pacoteReservaID;
                            retorno.Tables[TABELA_INFO_PACOTE].Rows.Add(novoInfoPacote);
                        }
                    }

                    retorno.Tables.Add(info);

                    return retorno;



                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
                return null;

        }
		


        public DataTable MontaDadosItensVendidos(List<int> ingressosID)
        {
            var sql = string.Format(@"Select CAST(ROW_NUMBER()over(order by vbi.ID) as int) as 'ReservaID', vbi.ID as 'VendaBilheteriaItemID', apr.DisponivelAjuste as 'DisponivelAjuste',
	                        ing.status as 'Status', '' as 'StatusDetalhado', 
 	                        vbi.TaxaConveniencia as 'Conv', vbi.TaxaConvenienciaValor as 'ValorConv', 'I' as 'Tipo',
 	                        sto.LugarMarcado as 'TipoLugar', vbi.TaxaComissao as 'TaxaComissao', vbi.ComissaoValor as 'ComissaoValor',
	                        vb.Obs as 'Obs', vbi.PacoteGrupo as 'PacoteGrupo' , ing.AssinaturaClienteID as 'AssinaturaClienteID',
                            et.Tipo as 'TipoEntrega'
	                    FROM tIngresso ing(NOLOCK)
	                    INNER JOIN tIngressoLog ingl(NOLOCK) ON ingl.ingressoID = ing.ID
	                    INNER JOIN tApresentacao apr(NOLOCK) ON apr.ID = ing.apresentacaoID
                        INNER JOIN tVendaBilheteria vb(NOLOCK) ON vb.ID = ing.vendabilheteriaID	                    
                        INNER JOIN tVendaBilheteriaItem vbi(nolock) ON vbi.ID = ingl.VendaBilheteriaItemID AND vb.ID = vbi.VendaBilheteriaID
	                    INNER JOIN tSetor sto(NOLOCK) ON sto.ID = ing.SetorID
	                    LEFT JOIN tEntregaControle etc(NOLOCK) ON vb.EntregaControleID = etc.ID
	                    LEFT JOIN tEntrega et(NOLOCK) ON et.ID = etc.EntregaID
                        where ing.id in ({0})", string.Join(",", ingressosID));

            CTLib.BD bd = new BD();
            try
            {
                DataTable dados = new DataTable("DadosItensVendidos");
                dados = bd.QueryToTable(sql);
                if (dados != null)
                {
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        string status = dados.Rows[i]["Status"].ToString();
                        string tipoEntrega = dados.Rows[i]["TipoEntrega"].ToString();
                        dados.Rows[i]["StatusDetalhado"] = Ingresso.StatusDetalhado(status, tipoEntrega);
                        dados.Rows[i]["Status"] = Ingresso.StatusDescritivo(status);
                    }
                    dados.Columns.Remove("TipoEntrega");
                }
                return dados;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataTable MontaDadosIngressosVendidos(List<int> ingressosID)
        {
            var sql = string.Format(@"Select CAST(ROW_NUMBER()over(order by vbi.ID) as int)as 'ReservaID', vbi.ID as 'VendaBilheteriaItemID', ing.ID as 'IngressoID'
	                                      ,ing.PacoteID as 'PacoteID', ing.PrecoID as 'PrecoID', ing.CortesiaID as 'CortesiaID', ing.BloqueioID as 'BloqueioID'
	                                      ,evt.TipoCodigoBarra as 'TipoCodigoBarra', ing.CodigoBarra as 'CodigoBarra', ing.eventoID as 'EventoID'
	                                      ,ing.apresentacaoSetorID as 'ApresentacaoSetorID', ing.AssinaturaClienteID as 'AssinaturaClienteID'
	                                      ,vbi.TaxaConvenienciaValor as 'ValorConv'
	                                  FROM tIngresso ing(NOLOCK)
	                                  INNER JOIN tIngressoLog ingl(NOLOCK) ON ingl.ingressoID = ing.ID
	                                  INNER JOIN tVendaBilheteriaItem vbi(nolock) ON vbi.ID = ingl.VendaBilheteriaItemID AND vbi.VendaBilheteriaID = ing.VendaBilheteriaID
	                                  INNER JOIN tEvento evt(NOLOCK) ON evt.ID = ing.EventoID
	                                  WHERE ing.ID in ({0})", string.Join(",", ingressosID));

            CTLib.BD bd = new BD();
            try
            {
                DataTable dados = new DataTable("DadosIngressoVendido");
                dados = bd.QueryToTable(sql);
                return dados;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bd.Fechar();
            }

        }


        #region Estruturas Tables Cancelamento

        #region COLUNAS
        //colunas da GRID
        public const string ID = "ID";
        public const string TABELA_GRID = "Grid";
        public const string TABELA_RESERVA = "Reserva";
        public const string TABELA_INFO_PACOTE = "TabelaInfoPacote";
        public const string TABELA_INFO_ASSINATURA = "TabelaInfoAssinatura";

        public const string PRECOID = "PrecoID";
        public const string CORTESIAID = "CortesiaID";
        public const string BLOQUEIOID = "BloqueioID";

        public const string INGRESSOID = "IngressoID";
        public const string RESERVAID = "ReservaID";
        public const string EVENTO_PACOTE = "Evento/Pacote/Vale Ingresso";
        public const string EVENTO = "Evento";
        public const string PACOTE = "Pacote";
        public const string PACOTEID = "PacoteID";
        public const string HORARIO = "Horário";
        public const string SETOR_PRODUTO = "Setor/Produto";
        public const string CODIGO = "Cod";
        public const string PRECO = "Preço";
        public const string CORTESIA = "Cortesia";
        public const string BLOQUEIO = "Bloqueio";
        public const string VALOR = "Valor";
        public const string CONV = "Conv";
        public const string VALOR_CONV = "Valor Conv";
        public const string STATUS = "Status";
        public const string STATUS_DETALHADO = "StatusDetalhado";
        public const string TIPO = "Tipo";
        public const string TIPO_LUGAR = "TipoLugar";
        public const string TAXA_COMISSAO = "TaxaComissao";
        public const string COMISSAO_VALOR = "ComissaoValor";
        public const string PACOTE_GRUPO = "PacoteGrupo";
        public const string PACOTE_COMPLETO = "PacoteCompleto";
        public const string OBSERVACAO = "Obs";
        public const string NIVELRISCO = "NivelRisco";
        public const string CANCELADO = "Cancelado";
        public const string ENTREGA_AGENDA_ID = "EntregaAgendaID";
        public const string PAGAMENTO_PROCESSADO = "PagamentoProcessado";
        public const char TIPO_PACOTE = 'P'; //tipos de reserva
        public const char TIPO_INGRESSO = 'I';
        public const string TIPO_CODIGO_BARRA = "TipoCodigoBarra";
        public const string CODIGO_BARRA = "CodigoBarra";
        public const string EVENTO_ID = "EventoID";
        public const string APRESENTACAO_SETOR_ID = "ApresentacaoSetorID";
        public const string ASSINATURA_CLIENTE_ID = "AssinaturaClienteID";
        public const string TAXAPROCESSAMENTOVALOR = "TaxaProcessamentoValor";
        public const string TAXAPROCESSAMENTOCANCELADA = "TaxaProcessamentoCancelada";
        public const string VALORSEGURO = "ValorSeguro";
        //public const string ACAO = "Ação";
        public const string CLIENTE = "Cliente";
        public const string CLIENTEID = "ClienteID";

        public const string EMAIL = "Email";
        public const string CANAL = "Canal";
        public const string INFO_VENDA = "InfoVenda";
        public const string VENDEDOR = "Vendedor";
        public const string DATA_VENDA = "DataVenda";
        public const string DATA_CAIXA = "DataCaixa";
        public const string SENHA = "Senha";
        public const string STATUS_VENDA = "StatusVenda";
        public const string INGRESSO_ID = "IngressoID";
        public const string VENDA_BILHETERIA_ID = "VendaBilheteriaID";
        public const string VENDA_BILHETERIA_ITEM_ID = "VendaBilheteriaItemID";
        public const string INGRESSOLOG_ID = "IngressoLogID";
        public const string VALOR_TOTAL = "ValorTotal";
        public const string DISPONIVEL_AJUSTE = "DisponivelAjuste";
        public const string TAXA_ENTREGA_VALOR = "TaxaEntregaValor";
        public const string ENTREGA_CONTROLE_ID = "EntregaControleID";
        public const string TAXA_CONV_VALOR_TOTAL = "TaxaConvenienciaValorTotal";
        public const string TAXA_ENTREGA_TIPO = "TaxaEntregaTipo";
        public const string TRANSACTION_ID = "TransactionID";
        #endregion

        public static DataSet EstruturaGrid()
        {

            DataSet ds = new DataSet("Reservas");

            DataTable tGrid = new DataTable(TABELA_GRID);
            DataTable tInfoPacote = new DataTable(TABELA_INFO_PACOTE);
            DataTable tReserva = new DataTable(TABELA_RESERVA);
            DataTable tInfoAssinatura = new DataTable(TABELA_INFO_ASSINATURA);

            DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
            colGridReservaID.Unique = true;
            colGridReservaID.AutoIncrement = true;
            colGridReservaID.AutoIncrementStep = 1;
            colGridReservaID.AutoIncrementSeed = 1;

            tGrid.Columns.Add(colGridReservaID);
            //tGrid.Columns.Add(INGRESSO_ID, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(INGRESSOLOG_ID, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(VENDA_BILHETERIA_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TIPO, typeof(char)).DefaultValue = "-"; //guarda o tipo de reserva (ingresso ou pacote)
            tGrid.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-";//guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
            tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tGrid.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(PACOTE_GRUPO, typeof(int));
            tGrid.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));


            tInfoPacote.Columns.Add(RESERVAID, typeof(int));
            tInfoPacote.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoPacote.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoPacote.Columns.Add(TIPO, typeof(char)).DefaultValue = "-"; //guarda o tipo de reserva (ingresso ou pacote)
            tInfoPacote.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-"; //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tInfoPacote.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoPacote.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoPacote.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            tInfoAssinatura.Columns.Add(RESERVAID, typeof(int));
            tInfoAssinatura.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(VALOR, typeof(decimal));
            tInfoAssinatura.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoAssinatura.Columns.Add(TIPO, typeof(char)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoAssinatura.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoAssinatura.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoAssinatura.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;
            tInfoAssinatura.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            //TABELA_RESERVA
            tReserva.Columns.Add(RESERVAID, typeof(int));
            tReserva.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
            tReserva.Columns.Add(INGRESSOID, typeof(int));
            tReserva.Columns.Add(PACOTEID, typeof(int)); //Se for Pacote eh o ID do Pacote.
            tReserva.Columns.Add(PRECOID, typeof(int));
            tReserva.Columns.Add(CORTESIAID, typeof(int));
            tReserva.Columns.Add(BLOQUEIOID, typeof(int));
            tReserva.Columns.Add(TIPO_CODIGO_BARRA, typeof(string));
            tReserva.Columns.Add(CODIGO_BARRA, typeof(string));
            tReserva.Columns.Add(EVENTO_ID, typeof(int));
            tReserva.Columns.Add(APRESENTACAO_SETOR_ID, typeof(int));
            tReserva.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            ds.Tables.Add(tGrid);
            ds.Tables.Add(tReserva);
            ds.Tables.Add(tInfoPacote);
            ds.Tables.Add(tInfoAssinatura);
            //Grid com a Reserva
            DataColumn colReservaID = tReserva.Columns[RESERVAID];
            DataRelation dr1 = new DataRelation("GridXReserva", colGridReservaID, colReservaID, true);

            ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colGridReservaID, colReservaID);
            idKeyRestraint1.DeleteRule = Rule.Cascade;
            tReserva.Constraints.Add(idKeyRestraint1);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr1);

            //Grid com a InfoPacote
            DataColumn colInfoPacoteID = tInfoPacote.Columns[RESERVAID];
            DataRelation dr2 = new DataRelation("GridXInfoPacote", colGridReservaID, colInfoPacoteID, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colGridReservaID, colInfoPacoteID);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tInfoPacote.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr2);

            //Grid com a InfoAssinatura
            DataColumn colInfoAssinaturaID = tInfoAssinatura.Columns[RESERVAID];
            DataRelation dr3 = new DataRelation("GridXInfoAssinatura", colGridReservaID, colInfoAssinaturaID, true);

            ForeignKeyConstraint idKeyRestraint3 = new ForeignKeyConstraint(colGridReservaID, colInfoAssinaturaID);
            idKeyRestraint3.DeleteRule = Rule.Cascade;
            tInfoAssinatura.Constraints.Add(idKeyRestraint3);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr3);

            return ds;
        }

        public static DataTable EstruturaInfoVenda()
        {

            DataTable tGrid = new DataTable(INFO_VENDA);

            tGrid.Columns.Add("ID", typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(TAXA_CONV_VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(TAXA_ENTREGA_VALOR, typeof(decimal));
            tGrid.Columns.Add(ENTREGA_CONTROLE_ID, typeof(int));
            tGrid.Columns.Add(TAXA_ENTREGA_TIPO, typeof(string));
            tGrid.Columns.Add(VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(STATUS_VENDA, typeof(string));
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tGrid.Columns.Add(CLIENTE, typeof(string));
            tGrid.Columns.Add(CLIENTEID, typeof(int));
            tGrid.Columns.Add(EMAIL, typeof(string));
            tGrid.Columns.Add(CANAL, typeof(string));
            tGrid.Columns.Add(VENDEDOR, typeof(string));
            tGrid.Columns.Add(SENHA, typeof(string));
            tGrid.Columns.Add(DATA_VENDA, typeof(DateTime));
            tGrid.Columns.Add(DATA_CAIXA, typeof(DateTime));
            tGrid.Columns.Add(OBSERVACAO, typeof(string));
            tGrid.Columns.Add(NIVELRISCO, typeof(int));
            tGrid.Columns.Add(ENTREGA_AGENDA_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(PAGAMENTO_PROCESSADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TAXAPROCESSAMENTOVALOR, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TAXAPROCESSAMENTOCANCELADA, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(VALORSEGURO, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TRANSACTION_ID, typeof(string)).DefaultValue = string.Empty;

            return tGrid;
        }


        private DataTable EstruturaTabela()
        { //auxiliar

            DataTable tabela = new DataTable("Tabela");

            tabela.Columns.Add(INGRESSO_ID, typeof(int));
            tabela.Columns.Add(INGRESSOLOG_ID, typeof(int));
            tabela.Columns.Add(VENDA_BILHETERIA_ID, typeof(int));
            tabela.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
            tabela.Columns.Add(EVENTO, typeof(string));
            tabela.Columns.Add(PACOTE, typeof(string));
            tabela.Columns.Add(PACOTEID, typeof(int));
            tabela.Columns.Add(HORARIO, typeof(string));
            tabela.Columns.Add(SETOR_PRODUTO, typeof(string));
            tabela.Columns.Add(CODIGO, typeof(string));
            tabela.Columns.Add(PRECO, typeof(string));
            tabela.Columns.Add(CORTESIA, typeof(string));
            tabela.Columns.Add(BLOQUEIO, typeof(string));
            tabela.Columns.Add(VALOR, typeof(decimal));
            tabela.Columns.Add(STATUS, typeof(string));
            tabela.Columns.Add(STATUS_DETALHADO, typeof(string));
            tabela.Columns.Add(CONV, typeof(int));
            tabela.Columns.Add(VALOR_CONV, typeof(decimal));
            tabela.Columns.Add(PRECOID, typeof(int));
            tabela.Columns.Add(CORTESIAID, typeof(int));
            tabela.Columns.Add(BLOQUEIOID, typeof(int));
            tabela.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tabela.Columns.Add(TAXA_COMISSAO, typeof(int));
            tabela.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tabela.Columns.Add(OBSERVACAO, typeof(string));
            tabela.Columns.Add(PACOTE_GRUPO, typeof(int));
            tabela.Columns.Add(TIPO_CODIGO_BARRA, typeof(string));
            tabela.Columns.Add(CODIGO_BARRA, typeof(string));
            tabela.Columns.Add(EVENTO_ID, typeof(int));
            tabela.Columns.Add(APRESENTACAO_SETOR_ID, typeof(int));
            tabela.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));
            return tabela;
        }

        #endregion

    }

    public class VendaLista : VendaLista_B
    {

        public VendaLista() { }

        public VendaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

    class EntregaAuxiliar
    {
        public int IngressoID { get; set; }
        public string IngressoCodigo { get; set; }
        public string IngressoStatus { get; set; }
        public string EntregaTipo { get; set; }
        public string Nome { get; set; }
        public string Canal { get; set; }
        public DateTime DateTimeAcao { get; set; }
        public string NomeUsuario { get; set; }
    }

}
