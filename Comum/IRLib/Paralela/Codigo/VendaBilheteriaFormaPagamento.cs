/**************************************************
* Arquivo: VendaBilheteriaFormaPagamento.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class VendaBilheteriaFormaPagamento : VendaBilheteriaFormaPagamento_B
    {

        public VendaBilheteriaFormaPagamento() { }

        /// <summary>
        /// Inserir novo(a) VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamento(FormaPagamentoID, VendaBilheteriaID, Valor, Porcentagem,Dias,TaxaAdm,IR, DataDeposito) ");
                sql.Append("VALUES (@001,@002,'@003','@004','@005','@006','@007','@008');SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.FormaPagamentoID.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Porcentagem.ValorBD);
                sql.Replace("@005", this.Dias.ValorBD);
                sql.Replace("@006", this.TaxaAdm.ValorBD);
                sql.Replace("@007", this.IR.ValorBD);
                sql.Replace("@008", this.DataDeposito.ValorBD);

                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = x != null ? Convert.ToInt32(x) : 0;
                bd.Fechar();

                bool result = Convert.ToBoolean(this.Control.ID);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override bool Atualizar()
        {
            try
            {
                return this.Atualizar(bd);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override bool Atualizar(BD bd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE tVendaBilheteriaFormaPagamento SET FormaPagamentoID = @001, VendaBilheteriaID = @002, Valor = '@003', Porcentagem = '@004', Dias = @005, TaxaAdm = '@006', IR = '@007', DataDeposito = '@008', Atualizado = '@009', ValeIngressoID = @010, VendaBilheteriaFormaPagamentoTEFID = @011, MensagemRetorno = '@012', HoraTransacao = '@013', DataTransacao = '@014', CodigoIR = '@015', NumeroAutorizacao = '@016', NSUHost = '@017', NSUSitef = '@018', Cupom = '@019', DadosConfirmacaoVenda = '@020', Rede = '@021', CodigoRespostaTransacao = '@022', CodigoRespostaVenda = '@023', CartaoID = @024 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.FormaPagamentoID.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@003", this.Valor.ValorBD);
            sql.Replace("@004", this.Porcentagem.ValorBD);
            sql.Replace("@005", this.Dias.ValorBD);
            sql.Replace("@006", this.TaxaAdm.ValorBD);
            sql.Replace("@007", this.IR.ValorBD);
            sql.Replace("@008", this.DataDeposito.ValorBD);
            sql.Replace("@009", this.Atualizado.ValorBD);
            sql.Replace("@010", this.ValeIngressoID.ValorBD);
            sql.Replace("@011", this.VendaBilheteriaFormaPagamentoTEFID.ValorBD);
            sql.Replace("@012", this.MensagemRetorno.ValorBD);
            sql.Replace("@013", this.HoraTransacao.ValorBD);
            sql.Replace("@014", this.DataTransacao.ValorBD);
            sql.Replace("@015", this.CodigoIR.ValorBD);
            sql.Replace("@016", this.NumeroAutorizacao.ValorBD);
            sql.Replace("@017", this.NSUHost.ValorBD);
            sql.Replace("@018", this.NSUSitef.ValorBD);
            sql.Replace("@019", this.Cupom.ValorBD);
            sql.Replace("@020", this.DadosConfirmacaoVenda.ValorBD);
            sql.Replace("@021", this.Rede.ValorBD);
            sql.Replace("@022", this.CodigoRespostaTransacao.ValorBD);
            sql.Replace("@023", this.CodigoRespostaVenda.ValorBD);
            sql.Replace("@024", this.CartaoID.ValorBD);

            int x = bd.Executar(sql.ToString());
            return (x == 1);
        }
        private List<DateTime> Feriados(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                List<DateTime> feriados = new List<DateTime>();
                string sql = "SELECT * FROM tFeriado AS f WHERE f.Data >= '" + dataInicial.ToString("yyyyMMddHHmmss") + "' AND f.Data <= '" + dataFinal.ToString("yyyyMMddHHmmss") + "' ";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    feriados.Add(DateTime.ParseExact(bd.LerString("Data"), "yyyyMMddHHmmss", null));
                }
                return feriados;
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
        /// Rotina utilizada apenas uma vez para popular o campo DataDeposito na tVendaBilheteriaFormaPagamento.
        /// Não utilizar. Esse código está aqui para não ser perdido. kim
        /// </summary>
        [Obsolete("Do not use! ", true)]
        private void PopulaDataDeposito()
        {
            //esse código popula a DataDeposito
            string sql = @"SELECT tVendaBilheteriaFormaPagamento.ID,tVendaBilheteria.DataVenda, tVendaBilheteriaFormaPagamento.Dias FROM tVendaBilheteriaFormaPagamento
                            INNER JOIN tVendaBilheteria ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID ORDER BY tVendaBilheteriaFormaPagamento.ID";
            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("DataVenda", typeof(string));
            tabela.Columns.Add("Dias", typeof(int));
            bd.Executar(sql);
            while (bd.Consulta().Read())
            {
                DataRow linha = tabela.NewRow();
                linha["ID"] = bd.LerInt("ID");
                linha["DataVenda"] = bd.LerString("DataVenda");
                linha["Dias"] = bd.LerInt("Dias");
                tabela.Rows.Add(linha);

            }
            DateTime data = new DateTime();
            bd.Fechar();
            bd.IniciarTransacao();
            foreach (DataRow linha in tabela.Rows)
            {
                data = DateTime.ParseExact((string)linha["DataVenda"], "yyyyMMddHHmmss", null);
                data = data.AddDays((int)linha["Dias"]);
                string sql2 = "UPDATE tVendaBilheteriaFormaPagamento SET DataDeposito = '" + data.ToString("yyyyMMddHHmmss") + "' WHERE ID = " + (int)linha["ID"];
                bd.Executar(sql2);
            }


            DataTable tabela2 = new DataTable();
            tabela2.Columns.Add("ID", typeof(int));
            tabela2.Columns.Add("EmpresaID", typeof(int));
            bd.FecharConsulta();
            bd.Consulta(@"SELECT tCaixa.ID, EmpresaID FROM tCaixa
                                                    INNER JOIN tLoja ON tLoja.ID= tCaixa.LojaID
                                                    INNER JOIN tCanal ON tLoja.CanalID= tCanal.ID");
            while (bd.Consulta().Read())
            {
                DataRow linha = tabela2.NewRow();
                linha["ID"] = bd.LerInt("ID");
                linha["EmpresaID"] = bd.LerInt("EmpresaID");
                tabela2.Rows.Add(linha);
            }

            //insere na VendaBilheteriaFormaPagamentoItem
            VendaBilheteriaFormaPagamentoItem vb = new VendaBilheteriaFormaPagamentoItem();
            foreach (DataRow item in tabela2.Rows)
            {
                vb.Inserir((int)item["ID"], (int)item["EmpresaID"], bd);
            }


            bd.FinalizarTransacao();
        }
        public DataTable ConciliacaoBancaria(string empresaID, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                string inicial = dataInicial.ToString("yyyyMMddHHmmss");
                string final = dataFinal.ToString("yyyyMMddHHmmss");
                string filtroEmpresa = "";
                string filtroIR = "";
                //List<DateTime> feriados = Feriados(dataInicial, dataFinal);
                DataTable retorno = new DataTable();
                retorno.Columns.Add("DataDeposito", typeof(string));
                retorno.Columns.Add("FormaPagamentoID", typeof(int));
                retorno.Columns.Add("FormaPagamento", typeof(string));
                retorno.Columns.Add("ValorBruto", typeof(string));
                retorno.Columns.Add("ValorLiquido", typeof(string));

                if (empresaID != "1")
                {
                    filtroEmpresa = "AND EmpresaID = " + empresaID + " ";
                    filtroIR = "AND (vbfp.IR <> 'T' OR IR IS NULL) ";
                }
                else
                    filtroIR = " AND vbfp.IR = 'T' ";

                string filtroDataDeposito = "vbfpi.DataDeposito >= '" + inicial + "' AND vbfpi.DataDeposito <= '" + final + "' ";
                if (inicial == final)
                {
                    filtroDataDeposito = "vbfpi.DataDeposito LIKE '" + inicial.Substring(0, 8) + "%' ";
                }

                string sql = @"SELECT dbo.GetDataDeposito(vbfpi.DataDeposito) AS DataDeposito, fp.Nome AS FormaPagamento,fp.ID AS FormaPagamentoID,SUM(vbfpi.ValorBruto) AS ValorBruto , SUM(vbfpi.ValorBruto - (ValorBruto * (ISNULL(vbfp.TaxaAdm,0) / 100))) AS ValorLiquido
                                FROM
                                tVendaBilheteriaFormaPagamentoItem As vbfpi (NOLOCK)
                                INNER JOIN tVendaBilheteriaFormaPagamento AS vbfp (NOLOCK)  ON vbfp.ID = vbfpi.VendaBilheteriaFormaPagamentoID
                                INNER JOIN tFormaPagamento AS fp (NOLOCK)  ON fp.ID = vbfp.FormaPagamentoID " +
                                " WHERE " + filtroDataDeposito + filtroIR + " " + filtroEmpresa + " GROUP BY dbo.GetDataDeposito(vbfpi.DataDeposito),fp.Nome, fp.ID ORDER BY dbo.GetDataDeposito(vbfpi.DataDeposito), fp.ID";

                bd.Executar(sql);
                DataRow linha;
                IRLib.Paralela.ClientObjects.EstruturaConciliacaoBancaria atual = new IRLib.Paralela.ClientObjects.EstruturaConciliacaoBancaria();
                IRLib.Paralela.ClientObjects.EstruturaConciliacaoBancaria anterior = new IRLib.Paralela.ClientObjects.EstruturaConciliacaoBancaria();
                DateTime dataAtual = new DateTime();
                DateTime dataAnterior = new DateTime();
                decimal subtotalBruto = 0;
                decimal subtotalLiquido = 0;
                decimal totalBruto = 0;
                decimal totalLiquido = 0;

                while (bd.Consulta().Read())
                {
                    dataAtual = DateTime.ParseExact(bd.LerString("DataDeposito").Trim(), "yyyyMMdd", null);

                    atual.DataDeposito = dataAtual.ToString();
                    atual.FormaPagamento = bd.LerString("FormaPagamento");
                    atual.FormaPagamentoID = bd.LerInt("FormaPagamentoID");
                    atual.ValorBruto = bd.LerDecimal("ValorBruto");
                    atual.ValorLiquido = bd.LerDecimal("ValorLiquido");

                    if (dataAtual != DateTime.MinValue && dataAnterior != DateTime.MinValue)
                    {
                        if (dataAtual.Month != dataAnterior.Month)
                        {
                            //adiciona os subtotais
                            linha = retorno.NewRow();
                            linha["DataDeposito"] = "SubTotal:";
                            linha["FormaPagamento"] = "";
                            linha["ValorBruto"] = subtotalBruto.ToString("C");
                            linha["ValorLiquido"] = subtotalLiquido.ToString("C");
                            retorno.Rows.Add(linha);
                            //limpa os subtotais
                            subtotalBruto = 0;
                            subtotalLiquido = 0;
                            //linha em branco
                            linha = retorno.NewRow();
                            retorno.Rows.Add(linha);
                        }
                    }
                    //guarda a data anterior
                    anterior = atual;
                    dataAnterior = dataAtual;

                    subtotalBruto += atual.ValorBruto;
                    subtotalLiquido += atual.ValorLiquido;
                    totalBruto += atual.ValorBruto;
                    totalLiquido += atual.ValorLiquido;

                    //adiciona a ultima linha
                    linha = retorno.NewRow();
                    linha["DataDeposito"] = dataAtual.ToString("dd/MM/yyyy");
                    linha["FormaPagamento"] = atual.FormaPagamento;
                    linha["ValorBruto"] = atual.ValorBruto.ToString("C");
                    linha["ValorLiquido"] = atual.ValorLiquido.ToString("C");
                    retorno.Rows.Add(linha);

                    //dataIncorreta = true;
                }


                //adiciona os subtotais
                linha = retorno.NewRow();
                linha["DataDeposito"] = "SubTotal:";
                linha["FormaPagamento"] = "";
                linha["ValorBruto"] = subtotalBruto.ToString("C");
                linha["ValorLiquido"] = subtotalLiquido.ToString("C");
                retorno.Rows.Add(linha);
                //limpa os subtotais
                subtotalBruto = 0;
                subtotalLiquido = 0;

                //adiciona os totais
                linha = retorno.NewRow();
                linha["DataDeposito"] = "Total:";
                linha["FormaPagamento"] = "";
                linha["ValorBruto"] = totalBruto.ToString("C");
                linha["ValorLiquido"] = totalLiquido.ToString("C");
                retorno.Rows.Add(linha);

                return retorno;
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
        /// Inserir novo(a) VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public string StringInserir()
        {
            try
            {

                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamento(FormaPagamentoID, VendaBilheteriaID, Valor, Porcentagem, Dias, TaxaAdm, IR, DataDeposito, Atualizado, ValeIngressoID, VendaBilheteriaFormaPagamentoTEFID, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CodigoRespostaVenda, CartaoID, TokenPayPal, PayerIDPaypal, CorrelationID, TransactionID, Coeficiente, JurosValor) ");
                sql.Append("VALUES (@001,@002,'@003','@004',@005,'@006','@007','@008','@009',@010,@011,'@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023',@024,'@025','@026','@027','@028', @029, @030);SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.FormaPagamentoID.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Porcentagem.ValorBD);
                sql.Replace("@005", this.Dias.ValorBD);
                sql.Replace("@006", this.TaxaAdm.ValorBD);
                sql.Replace("@007", this.IR.ValorBD);
                sql.Replace("@008", this.DataDeposito.ValorBD);
                sql.Replace("@009", this.Atualizado.ValorBD);
                sql.Replace("@010", this.ValeIngressoID.ValorBD);
                sql.Replace("@011", this.VendaBilheteriaFormaPagamentoTEFID.ValorBD);
                sql.Replace("@012", this.MensagemRetorno.ValorBD);
                sql.Replace("@013", this.HoraTransacao.ValorBD);
                sql.Replace("@014", this.DataTransacao.ValorBD);
                sql.Replace("@015", this.CodigoIR.ValorBD);
                sql.Replace("@016", this.NumeroAutorizacao.ValorBD);
                sql.Replace("@017", this.NSUHost.ValorBD);
                sql.Replace("@018", this.NSUSitef.ValorBD);
                sql.Replace("@019", this.Cupom.ValorBD);
                sql.Replace("@020", this.DadosConfirmacaoVenda.ValorBD);
                sql.Replace("@021", this.Rede.ValorBD);
                sql.Replace("@022", this.CodigoRespostaTransacao.ValorBD);
                sql.Replace("@023", this.CodigoRespostaVenda.ValorBD);
                sql.Replace("@024", this.CartaoID.ValorBD);
                sql.Replace("@025", this.TokenPayPal.ValorBD);
                sql.Replace("@026", this.PayerIDPaypal.ValorBD);
                sql.Replace("@027", this.CorrelationID.ValorBD);
                sql.Replace("@028", this.TransactionID.ValorBD);
                sql.Replace("@029", this.Coeficiente.ValorBD);
                sql.Replace("@030", this.JurosValor.ValorBD);

                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AlterarFormasPagamento(DataTable tPagamento, int vendaBilheteriaID)
        {
            // Verifica estrutura do DataTable.
            if (tPagamento == null ||
                tPagamento.Columns.Count < 2 ||
                tPagamento.Rows.Count == 0 ||
                vendaBilheteriaID == 0)
                throw new VendaBilheteriaFormaPagamentoException("Parâmetros de entrada incorretos!");

            tPagamento.Columns.Add("Porcentagem", typeof(decimal));
            BD bd = new BD();
            try
            {

                // Atribui vendaBilheteriaID. Igual para todas as formas
                this.VendaBilheteriaID.Valor = vendaBilheteriaID;

                /*
                 * 1) Inicia transação.
                 * 2) Calcula porcentagem. Deverá ser igual a 100.
                 * 3) Exclue as formas de pagamento atuais.
                 * 4) Insere as novas.
                 * 5) Commit transaction
                 * :: Caso ocorra algum erro efetuar o rollback transaction.
                 * 
                */

                /// 1)
                bd.IniciarTransacao();
                decimal valorTotalVenda = 0;

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tVendaBilheteria.ValorTotal " +
                    "FROM " +
                    "   tVendaBilheteria (NOLOCK) " +
                    "WHERE " +
                    "   tVendaBilheteria.ID = " + vendaBilheteriaID))
                {
                    if (oDataReader.Read())
                        valorTotalVenda = bd.LerDecimal("ValorTotal");
                }

                decimal porcentagemTotal = 0;
                object val = tPagamento.Compute("SUM(Valor)", "1=1");
                if (val == null || val == System.DBNull.Value || Convert.ToDecimal(val) == 0)
                    throw new VendaBilheteriaFormaPagamentoException("Valor total deve ser maior que zero.");

                if (Convert.ToDecimal(val) != valorTotalVenda)
                    throw new VendaBilheteriaFormaPagamentoException("Valor total deve ser igual ao valor da venda.");

                decimal valorTotal = (decimal)val;
                decimal porc = 0;

                /// 2)
                for (int i = 0; i < tPagamento.Rows.Count; i++)
                {
                    DataRow pagto = tPagamento.Rows[i];
                    //calcular porcentagem
                    porc = Math.Round(((decimal)pagto["Valor"] * 100) / valorTotal, 2);

                    pagto["Porcentagem"] = porc;

                    porcentagemTotal += porc;
                }

                if (Math.Round(porcentagemTotal) != 100)
                    throw new VendaBilheteriaFormaPagamentoException("Falha ao calcular a porcentagem (" + porcentagemTotal + ").");

                /// 3)
                string sql = "DELETE FROM tVendaBilheteriaFormaPagamento WHERE VendaBilheteriaID = " + vendaBilheteriaID;
                int itensExcluidos = bd.Executar(sql);

                if (itensExcluidos == 0)
                    throw new VendaBilheteriaFormaPagamentoException("Falha ao alterar as formas de pagamento. Processo abortado. Por favor, entre em contato com o administrador.");

                /// 4)
                object x = null;
                foreach (DataRow pagto in tPagamento.Rows)
                {
                    if ((decimal)pagto["Valor"] == 0 || (decimal)pagto["Porcentagem"] == 0 || (int)pagto["ID"] == 0)
                        throw new VendaBilheteriaFormaPagamentoException("Valor ou Porcentagem não pode ser igual a zero.");

                    this.FormaPagamentoID.Valor = (int)pagto["ID"];
                    this.Porcentagem.Valor = (decimal)pagto["Porcentagem"];
                    this.Valor.Valor = (decimal)pagto["Valor"];

                    x = bd.Executar(this.StringInserir());

                    if ((int)x < 1)
                        throw new VendaBilheteriaFormaPagamentoException("Falha ao alterar as formas de pagamento. Processo abortado!");

                }
                ///

                /// 5)
                bd.FinalizarTransacao();
                /// 
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                bd.DesfazerTransacao();
                throw new VendaBilheteriaFormaPagamentoException("Ocorreram problemas ao acessar o banco de dados. Por favor, entre em contato com o administrador: \n" + ex.Message);
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw new VendaBilheteriaFormaPagamentoException("Erro não identificado: " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IEnumerable<Assinaturas.Models.Pagamento> ListarPagamento(int clienteID, int assinaturaTipoID, string filtro)
        {
            try
            {

                string sql =
                    string.Format(@"SELECT DISTINCT
                                        b.Valor,
                                        b.TimeStamp,
                                        f.Nome as FormaPagamento ,
                                        b.DataPagamento ,
                                        p.FormaPagamentoID,
                                        b.DataVencimento ,
                                        b.DataConfirmacao 
                                    FROM tAssinaturaCliente ac(NOLOCK)
                                    INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
                                    INNER JOIN tVendaBilheteria v(NOLOCK) ON ac.VendaBilheteriaID = v.ID
                                    INNER JOIN tVendaBilheteriaFormaPagamento p(NOLOCK) ON p.VendaBilheteriaID = v.ID
                                    INNER JOIN tVendaBilheteriaFormaPagamentoBoleto b(NOLOCK)ON b.VendaBilheteriaFormaPagamentoID = p.ID
                                    INNER JOIN tFormaPagamento f(NOLOCK) ON p.FormaPagamentoID = f.ID
                                    WHERE v.ClienteID = {0} {1} AND a.AssinaturaTipoID = {2}  and (ac.status <> 'D' and ac.acao <> 'D') ", clienteID, filtro, assinaturaTipoID);
                bd.Consulta(sql);

                var lista = new List<Assinaturas.Models.Pagamento>();
                FormaPagamento.ESTADO auxEstado = new FormaPagamento.ESTADO();


                if (bd.Consulta().Read())
                {

                    do
                    {
                        auxEstado = FormaPagamento.ESTADO.Aberto;

                        if (bd.LerDateTime("DataVencimento").Date < DateTime.Now.Date)
                        {
                            auxEstado = FormaPagamento.ESTADO.Vencido;
                        }

                        if (bd.LerDateTime("DataPagamento") > DateTime.MinValue)
                        {
                            auxEstado = FormaPagamento.ESTADO.Pago;
                        }

                        lista.Add(new Assinaturas.Models.Pagamento()
                        {
                            EstadoPagamento = auxEstado.ToString(),
                            EstadoPagamentoID = Convert.ToInt32(auxEstado),
                            Valor = bd.LerDecimal("Valor"),
                            TimeStamp = bd.LerDateTime("TimeStamp"),
                            FormaPagamento = bd.LerString("FormaPagamento"),
                            DataPagamento = bd.LerDateTime("DataPagamento"),
                            FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                            DataVencimento = bd.LerDateTime("DataVencimento"),
                            DataConfirmacao = bd.LerDateTime("DataConfirmacao"),
                        });
                    } while (bd.Consulta().Read());
                }

                bd.FecharConsulta();

                string sql2 =
                    string.Format(@"SELECT DISTINCT
                                            p.Valor,
                                            v.DataVenda as TimeStamp,
                                            f.Nome as FormaPagamento ,
                                            v.DataVenda as DataPagamento ,
                                            p.FormaPagamentoID
                                        FROM tAssinaturaCliente ac(NOLOCK)
                                        INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
                                        INNER JOIN tVendaBilheteria v(NOLOCK) ON ac.VendaBilheteriaID = v.ID
                                        INNER JOIN tVendaBilheteriaFormaPagamento p(NOLOCK) ON p.VendaBilheteriaID = v.ID
                                        INNER JOIN tFormaPagamento f(NOLOCK) ON p.FormaPagamentoID = f.ID
                                WHERE f.Tipo <> 3  AND v.ClienteID = {0}  {1}  and (ac.status <> 'D' and ac.acao <> 'D') AND a.AssinaturaTipoID = {2} ", clienteID, filtro, assinaturaTipoID);
                bd.Consulta(sql2);
                if (bd.Consulta().Read())
                {
                    do
                    {
                        lista.Add(new Assinaturas.Models.Pagamento()
                        {
                            EstadoPagamento = FormaPagamento.ESTADO.Pago.ToString(),
                            EstadoPagamentoID = Convert.ToInt32(FormaPagamento.ESTADO.Pago),
                            Valor = bd.LerDecimal("Valor"),
                            TimeStamp = bd.LerDateTime("TimeStamp"),
                            FormaPagamento = bd.LerString("FormaPagamento"),
                            DataPagamento = bd.LerDateTime("DataPagamento"),
                            FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                        });
                    } while (bd.Consulta().Read());
                }

                bd.FecharConsulta();

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class VendaBilheteriaFormaPagamentoLista : VendaBilheteriaFormaPagamentoLista_B
    {

        public VendaBilheteriaFormaPagamentoLista() { }

    }

}
