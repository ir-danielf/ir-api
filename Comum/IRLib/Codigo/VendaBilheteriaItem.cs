/**************************************************
* Arquivo: VendaBilheteriaItem.cs
* Gerado: 14/07/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace IRLib
{

    public class VendaBilheteriaItem : VendaBilheteriaItem_B
    {

        //Status
        public const string VENDA = "V";
        public const string CANCELAMENTO = "C";
        public const string ENTREGA = "E";
        public const string REIMPRESSAO = "R";
        public const string PRE_RESERVA = "M";

        public VendaBilheteriaItem() { }

        public VendaBilheteriaItem(int usuarioIDLogado) { }

        public DataTable DetalhesItem(int VendaBilheteriaItemID)
        {
            DataTable tabela = new DataTable("DetalhesItem");

            try
            {

                tabela.Columns.Add("PacoteNome", typeof(string));
                tabela.Columns.Add("LocalNome", typeof(string));
                tabela.Columns.Add("EventoNome", typeof(string));
                tabela.Columns.Add("Horario", typeof(string));
                tabela.Columns.Add("SetorNome", typeof(string));
                tabela.Columns.Add("PrecoNome", typeof(string));
                tabela.Columns.Add("PrecoValor", typeof(decimal));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Codigo", typeof(string));

                bd.Consulta("" +
                    "SELECT " +
                    "	tPacote.Nome AS PacoteNome, " +
                    "	tLocal.Nome AS LocalNome, " +
                    "	tEvento.Nome AS EventoNome, " +
                    "	tApresentacao.Horario, " +
                    "	tSetor.Nome AS SetorNome, " +
                    "	tPreco.Nome AS PrecoNome, " +
                    "	tPreco.Valor AS PrecoValor, " +
                    "	tIngresso.Status, " +
                    "	tIngresso.Codigo " +
                    "FROM " +
                    "	tVendaBilheteriaItem " +
                    "INNER JOIN " +
                    "	tIngressoLog ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID " +
                    "INNER JOIN " +
                    "	tIngresso ON tIngresso.ID = tIngressoLog.IngressoID " +
                    "INNER JOIN " +
                    "	tLocal ON tLocal.ID = tIngresso.LocalID " +
                    "INNER JOIN " +
                    "	tEvento ON tEvento.ID = tIngresso.EventoID " +
                    "INNER JOIN " +
                    "	tApresentacao ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN " +
                    "	tSetor ON tSetor.ID = tIngresso.SetorID " +
                    "INNER JOIN " +
                    "	tPreco ON tPreco.ID = tIngresso.PrecoID " +
                    "LEFT OUTER JOIN " +
                    "	tPacote ON tPacote.ID = tIngresso.PacoteID " +
                    "WHERE " +
                    "	tVendaBilheteriaItem.ID = " + VendaBilheteriaItemID + " " +
                    "");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["PacoteNome"] = bd.LerString("PacoteNome");
                    linha["LocalNome"] = bd.LerString("LocalNome");
                    linha["EventoNome"] = bd.LerString("EventoNome");
                    linha["Horario"] = bd.LerDateTime("Horario");
                    linha["SetorNome"] = bd.LerString("SetorNome");
                    linha["PrecoNome"] = bd.LerString("PrecoNome");
                    linha["PrecoValor"] = bd.LerDecimal("PrecoValor");
                    linha["Status"] = Ingresso.StatusDescritivo(bd.LerString("Status"));
                    linha["Codigo"] = bd.LerString("Codigo");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tabela;
        }

        /// <summary>
        /// Inserir novo(a) VendaBilheteriaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaItem(VendaBilheteriaID, PacoteID, Acao, TaxaConveniencia, TaxaConvenienciaValor,TaxaComissao,ComissaoValor) ");
                sql.Append("VALUES (@001,@002,'@003',@004,'@005','@006','@007');SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaConvenienciaValor.ValorBD);
                sql.Replace("@006", this.TaxaComissao.ValorBD);
                sql.Replace("@007", this.ComissaoValor.ValorBD);

                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bd.Fechar();

                bool result = Convert.ToBoolean(this.Control.ID);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) VendaBilheteriaItem
        /// </summary>
        /// <returns></returns>	
        public string StringInserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaItem(VendaBilheteriaID, PacoteID, Acao, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, ComissaoValor,PacoteGrupo) ");
                sql.Append("VALUES (@001,@002,'@003',@004,'@005','@006','@007','@008');SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaConvenienciaValor.ValorBD);
                sql.Replace("@006", this.TaxaComissao.ValorBD);
                sql.Replace("@007", this.ComissaoValor.ValorBD);
                sql.Replace("@008", this.PacoteGrupo.ValorBD);
                //sql.Replace("@006", this.Tipo.ValorBD);	

                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Verifica Ultima acao do Ingresso Log
        /// </summary>
        /// <returns></returns>
        public override bool UltimaAcaoIgualCancelar()
        {
            int ingressoID = 0;
            int ingressoLogID = 0;
            bool sim = true;
            try
            {
                BD banco = new BD();
                // Obtendo o IngressoLog em funcao do Venda Bilheteria Item
                string sql =
                    "SELECT     MAX(ID) AS IngressoLogID, IngressoID " +
                    "FROM         tIngressoLog " +
                    "WHERE VendaBilheteriaItemID= (" + this.Control.ID + ") " +
                    "GROUP BY IngressoID ";
                banco.Consulta(sql);
                if (banco.Consulta().Read())
                {
                    ingressoLogID = banco.LerInt("IngressoLogID");
                    ingressoID = banco.LerInt("IngressoID");
                }
                banco.Fechar();
                // Obtendo ultima acao
                sql =
                    "SELECT COUNT(tIngressoLog.ID)AS Total " +
                    "FROM tIngressoLog " +
                    "WHERE tIngressoLog.IngressoID = " + ingressoID + " AND tIngressoLog.ID> " + ingressoLogID + " AND tIngressoLog.Acao IN ('C') ";
                banco.Consulta(sql);
                if (banco.Consulta().Read())
                {
                    if (banco.LerInt("Total") > 0)
                        sim = false;
                }
                banco.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return sim;
        } // fim de UltimaAcaoIgualCancelar

        /// <summary>
        /// Checa os ingressos de um item de venda
        /// </summary>
        /// <returns></returns>
        public override bool ChecarIngressoStatusCancelar()
        {
            Ingresso ingresso = new Ingresso();
            bool statusCancelar = true;
            try
            {
                // Obtendo dados atravehs de SQL
                string sql =
                    "SELECT     tIngressoLog.IngressoID, tIngressoLog.VendaBilheteriaItemID " +
                    "FROM       tIngressoLog INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID " +
                    "WHERE     (tIngressoLog.VendaBilheteriaItemID = " + this.Control.ID + ")";
                bd.Consulta(sql);
                while (bd.Consulta().Read() && statusCancelar)
                {
                    ingresso.Ler(bd.LerInt("IngressoID"));
                    statusCancelar = ingresso.ChecarStatusCancelar();
                }
                bd.Fechar();
                return statusCancelar;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ChecarIngressoStatusCancelar			

        public static string AcaoDescritiva(string acao)
        {
            string retorno;
            switch (acao)
            {
                case CANCELAMENTO:
                    retorno = "Cancelado";
                    break;
                case VENDA:
                    retorno = "Vendido";
                    break;
                case PRE_RESERVA:
                    retorno = "Pré-reservado";
                    break;
                default:
                    retorno = acao;
                    break;
            }
            return retorno;
        }

        public static string AcaoDescritiva(string acao, string acaoIngressoLog)
        {
            string retorno;
            switch (acao)
            {
                case PRE_RESERVA:
                    retorno = "Pré-reservado";
                    break;
                case CANCELAMENTO:
                    switch (acaoIngressoLog)
                    {
                        case VendaBilheteriaItem.PRE_RESERVA:
                            return "Cancelado Pré-Reserva";
                        default:
                            return "Cancelado";
                    }
                case VENDA:
                    switch (acaoIngressoLog)
                    {
                        case VendaBilheteriaItem.PRE_RESERVA:
                            return "Vendido Pré-Reserva";
                        default:
                            return "Vendido";
                    }
                default:
                    retorno = acao;
                    break;
            }
            return retorno;
        }

        public int[] ObterItensPorVenda(int vendaID)
        {
            try
            {
                ArrayList resultado = new ArrayList();
                int[] itensVetor = new int[0];
                // Obtendo dados atraves de SQL
                BD banco = new BD();
                string sql =
                    "SELECT        PacoteID, VendaBilheteriaID, ID AS VendaBilheteriaItemID " +
                    "FROM            tVendaBilheteriaItem " +
                    "WHERE        (VendaBilheteriaID = " + vendaID + ") ";
                banco.Consulta(sql);
                while (banco.Consulta().Read())
                {
                    resultado.Add(banco.LerInt("VendaBilheteriaItemID"));
                }
                banco.Fechar();
                itensVetor = (int[])resultado.ToArray(typeof(int));
                return itensVetor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }// fim de ObterItensPorVenda
        public int ObterItemPorIngresso(int ingressoID)
        {
            int resultado = -1;
            try
            {
                // Obtendo dados atraves de SQL
                BD banco = new BD();
                string sql =
                    "SELECT        tVendaBilheteriaItem.PacoteID, tVendaBilheteriaItem.VendaBilheteriaID, tVendaBilheteriaItem.ID AS VendaBilheteriaItemID, tIngressoLog.IngressoID " +
                    "FROM            tVendaBilheteriaItem INNER JOIN " +
                                                    "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID " +
                    "WHERE        (tIngressoLog.IngressoID = " + ingressoID + ") ";
                banco.Consulta(sql);
                if (banco.Consulta().Read())
                {
                    resultado = banco.LerInt("VendaBilheteriaItemID");
                }
                banco.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return resultado;
        }// fim de ObterPorIngresso
        public override string IngressoIDPorItem()
        {
            string resultado = "";
            try
            {
                BD banco = new BD();
                // Obtendo dados atraves de SQL
                string sql =
                    "SELECT DISTINCT  tIngressoLog.IngressoID, tIngressoLog.VendaBilheteriaItemID " +
                    "FROM       tIngressoLog INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID " +
                    "WHERE     (tIngressoLog.VendaBilheteriaItemID =" + this.Control.ID + ")";
                banco.Consulta(sql);
                if (banco.Consulta().Read())
                    resultado = banco.LerInt("IngressoID").ToString();
                while (banco.Consulta().Read())
                {
                    resultado += "," + banco.LerInt("IngressoID").ToString();
                }
                //
                banco.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return resultado;
        }// fim de IngressosPorItens
        public override int PrimeiroIngressoID()
        {
            string[] ingressoIDsVetor;
            int primeiroIngressoID;
            try
            {
                ingressoIDsVetor = this.IngressoIDPorItem().Split(',');
                primeiroIngressoID = Convert.ToInt32(ingressoIDsVetor[0].Trim());
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return primeiroIngressoID;
        }// fim de IngressosPorItens
        public DataTable EstruturaEventoApresentacaoSetorPreco()
        {
            DataTable tabela = new DataTable();
            tabela.Columns.Add("ItemID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentacao", typeof(string));
            tabela.Columns.Add("Setor/Produto", typeof(string));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            return tabela;
        }
        /// <summary>
        ///  Obtém Evento, Apresentacao e Setor de cada item
        /// </summary>
        /// <returns></returns>
        public override DataTable EventoApresentacaoSetorPreco(string ItensID, string status)
        {
            int ingressoID;
            DataTable tabelaIngresso;
            DataTable tabelaDados;
            tabelaDados = EstruturaEventoApresentacaoSetorPreco();
            DataRow linha;
            try
            {
                string[] itensIDVetor = ItensID.Split(',');

                int[] itensIDVetorInt = CTLib.Utilitario.VetorStringParaInteiro(itensIDVetor);
                foreach (int itemID in itensIDVetorInt)
                {
                    // Obter o primeiro IngressoID de cada item
                    this.Control.ID = itemID;
                    ingressoID = this.PrimeiroIngressoID();
                    // Obter Evento, Apresentação, Setor, Preço deste ingresso (conforme o status)
                    IngressoLista ingressoLista = new IngressoLista(); // precisa estar aqui dentro senão os filtros vão se somando
                    ingressoLista.FiltroSQL = "ID =" + ingressoID;
                    ingressoLista.FiltroSQL = "Status in (" + status + ")";
                    ingressoLista.Carregar();
                    ingressoLista.Primeiro();
                    if (ingressoLista.ToString() != "")
                    {
                        tabelaIngresso = ingressoLista.Ingresso.EventoApresentacaoSetor();
                        linha = tabelaDados.NewRow();
                        linha["ItemID"] = this.Control.ID;
                        linha["Evento"] = tabelaIngresso.Rows[0]["Evento"];
                        linha["Apresentacao"] = tabelaIngresso.Rows[0]["Apresentacao"];
                        linha["Setor/Produto"] = tabelaIngresso.Rows[0]["Setor/Produto"];
                        linha["Preco"] = tabelaIngresso.Rows[0]["Preco"];
                        this.Ler(this.Control.ID);
                        if (this.PacoteID.Valor <= 0)
                        {
                            linha["Valor"] = tabelaIngresso.Rows[0]["Valor"];
                        }
                        else
                        {
                            Pacote pacote = new Pacote();
                            pacote.Ler(this.PacoteID.Valor);
                            linha["Valor"] = pacote.Valor();
                        }
                        tabelaDados.Rows.Add(linha);
                    }
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return tabelaDados;
        }// fim de IngressosPorItens

        /// <summary>
        /// Obter o valor de ingresso do item de venda, um item pode ser um ingresso ou um pacote
        /// </summary>
        /// <returns>Valor do ingresso por item de venda</returns>
        public bool ItemMesaFechada()
        {
            bool mesaFechada = false;
            try
            {
                // Obtendo dados através de SQL
                BD banco = new BD();
                string sql =
                    "SELECT        tSetor.LugarMarcado, tIngressoLog.VendaBilheteriaItemID, tLugar.Codigo " +
                    "FROM            tIngressoLog INNER JOIN " +
                                                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                                                    "tLugar ON tIngresso.LugarID = tLugar.ID INNER JOIN " +
                                                    "tSetor ON tLugar.SetorID = tSetor.ID " +
                    "GROUP BY tSetor.LugarMarcado, tIngressoLog.VendaBilheteriaItemID, tLugar.Codigo  " +
                    "HAVING        (tIngressoLog.VendaBilheteriaItemID = " + this.Control.ID + ") ";
                banco.Consulta(sql);
                if (banco.Consulta().Read())
                {
                    // Verificando se é mesa fechada
                    string lugarMarcado = banco.LerString("LugarMarcado");
                    if (Setor.MesaFechada == lugarMarcado)
                        mesaFechada = true;
                }
                banco.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return mesaFechada;
        }// fim de ItemMesaFechada
        /// <summary>
        /// Obter o valor de ingresso do item de venda, um item pode ser um ingresso ou um pacote
        /// </summary>
        /// <returns>Valor do ingresso por item de venda</returns>
        public override decimal ValorIngresso()
        {
            decimal valor = 0;
            try
            {
                if (this.PacoteID.Valor > 0)
                {
                    // Valor por pacote
                    Pacote pacote = new Pacote();
                    pacote.Ler(this.PacoteID.Valor);
                    valor = pacote.Valor();
                }
                else
                {
                    // Obtendo dados através de SQL
                    BD banco = new BD();
                    string sql;
                    // Verificar se o ingresso pertence a uma mesa fechada
                    if (ItemMesaFechada())
                    {
                        // Valor de todos ingresso da mesa 
                        sql =
                            "SELECT        tIngressoLog.VendaBilheteriaItemID, SUM(tPreco.Valor) AS Valor " +
                            "FROM            tIngressoLog INNER JOIN " +
                            "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                            "tPreco ON tIngresso.PrecoID = tPreco.ID " +
                            "GROUP BY tIngressoLog.VendaBilheteriaItemID " +
                            "HAVING        (tIngressoLog.VendaBilheteriaItemID = " + this.Control.ID + ") ";
                    }
                    else
                    {
                        // Valor por ingresso
                        sql =
                            "SELECT DISTINCT tIngressoLog.IngressoID, tIngressoLog.VendaBilheteriaItemID, tIngresso.Codigo, tPreco.Nome, tPreco.Valor " +
                            "FROM tVendaBilheteriaItem INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                            "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                            "tPreco ON tIngresso.PrecoID = tPreco.ID " +
                            "WHERE     (tIngressoLog.VendaBilheteriaItemID =" + this.Control.ID + ") " +
                            "ORDER BY tIngressoLog.IngressoID ";
                    }
                    banco.Consulta(sql);
                    // Lendo o valor
                    if (banco.Consulta().Read())
                    {
                        valor = banco.LerDecimal("Valor");
                    }
                    banco.Fechar();
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return valor;
        }// fim de ValorIngresso
        /// <summary>
        /// Obter o valor da entrega do item de venda, é obtido pelo valor da Venda
        /// </summary>
        /// <returns>Valor do ingresso por item de venda</returns>
        public override decimal ValorEntrega()
        {
            decimal valor = 0;
            try
            {
                VendaBilheteria vendaBilheteria = new VendaBilheteria();
                vendaBilheteria.Ler(this.VendaBilheteriaID.Valor);
                valor = vendaBilheteria.TaxaEntregaValor.Valor / vendaBilheteria.QuantidadeItens();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return valor;
        }// fim de ValorEntrega

        public bool VendaTEF(int vendaBilheteriaItemID)
        {
            BD bd = new BD();
            try
            {



                object valorRetorno = bd.ConsultaValor(@"SELECT NotaFiscalCliente FROM tVendaBilheteria vb (NOLOCK)
                INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.VendaBilheteriaID = vb.ID
                WHERE vbi.ID = " + vendaBilheteriaItemID);

                if (valorRetorno is string)
                    if ((string)valorRetorno == string.Empty)
                        return false;
                    else //se for uma string e não for vazio está OK
                        return true;
                else
                    return false;
            }
            finally
            {

            }
        }

        public IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento TipoPagamento(int vendaBilheteriaItemID)
        {
            BD bd = new BD();
            try
            {
                string sql = @"SELECT 
                                    IsNull(NotaFiscalCliente, '') AS NotaFiscalCliente
                                    FROM tVendaBilheteria vb (NOLOCK)
                                    INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.VendaBilheteriaID = vb.ID
                                    WHERE vbi.ID = " + vendaBilheteriaItemID;

                string notaFiscal = bd.ConsultaValor(sql).ToString();
                switch (notaFiscal)
                {
                    case AdyenStatic.Fields.NotaFiscal:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen;
                    case AdyenStatic.Fields.NotaFiscalPaypal:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Paypal;
                    case AdyenStatic.Fields.NotaFiscalMilhas:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Milhas;
                    case "":
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Nenhum;
                    default:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.TEF;
                }
            }
            finally
            {
                bd.Fechar();
            }


        }

        public IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento TipoPagamentoBilheteria(int vendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {
                string sql = @"SELECT 
                                    IsNull(NotaFiscalCliente, '')
                                    FROM tVendaBilheteria vb (NOLOCK)
                                    WHERE vb.ID = " + vendaBilheteriaID;

                string notaFiscal = bd.ConsultaValor(sql).ToString();
                switch (notaFiscal)
                {
                    case AdyenStatic.Fields.NotaFiscal:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen;
                    case AdyenStatic.Fields.NotaFiscalPaypal:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Paypal;
                    case "":
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Nenhum;
                    default:
                        return IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.TEF;
                }
            }
            finally
            {
                bd.Fechar();
            }


        }



        public bool VendaTEFVendaBilheteria(int vendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {
                object valorRetorno = bd.ConsultaValor(@"SELECT NotaFiscalCliente FROM tVendaBilheteria vb (NOLOCK)
                WHERE vb.ID = " + vendaBilheteriaID);

                if (valorRetorno is string)
                    if ((string)valorRetorno == string.Empty)
                        return false;
                    else //se for uma string e não for vazio está OK
                        return true;
                else
                    return false;

            }
            finally
            {
                bd.Fechar();
            }
        }


        public string CupomTEF(int vendaBilheteriaItemID)
        {
            BD bd = new BD();
            try
            {


                object valorRetorno = bd.ConsultaValor(@"SELECT NotaFiscalCliente FROM tVendaBilheteria vb (NOLOCK)
                INNER JOIN tVendaBilheteriaItem vbi  (NOLOCK) ON vbi.VendaBilheteriaID = vb.ID
                WHERE vbi.ID = " + vendaBilheteriaItemID);

                if (valorRetorno is string)
                    return (string)valorRetorno;
                else
                    return string.Empty;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string CupomTEFVendaBilheteria(int vendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {
                object valorRetorno = bd.ConsultaValor(@"SELECT NotaFiscalCliente FROM tVendaBilheteria vb  (NOLOCK)
                WHERE vb.ID = " + vendaBilheteriaID);

                if (valorRetorno is string)
                    return (string)valorRetorno;
                else
                    return string.Empty;

            }
            finally
            {
                bd.Fechar();
            }
        }

    } // fim da classe

    public class VendaBilheteriaItemLista : VendaBilheteriaItemLista_B
    {

        public VendaBilheteriaItemLista() { }


    }

}
