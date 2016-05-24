/**************************************************
* Arquivo: VendaBilheteria.cs
* Gerado: 14/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;


namespace IRLib.Paralela
{

    partial class VendaBilheteria : VendaBilheteria_B
    {

        //status
        //public const string AGUARDANDOPAGAMENTO	= "A";
        public const string PAGO = "P";
        public const string CANCELADO = "C";
        public const string ENTREGUE = "E";
        public const string REIMPRESSO = "R";
        public const string PRE_RESERVADO = "M";
        public const string AGUARDANDO_APROVACAO = "A";
        public const string FRAUDE = "F";
        public const string EMANALISE = "N";
        //public const string VENDIDO = "V";
        //public const string RECUSADO = "R";

        public VendaBilheteria() { }

        public VendaBilheteria(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        private int[] itensComBaixa;

        public int[] ItensComBaixa
        {
            get { return itensComBaixa; }
            set { itensComBaixa = value; }
        }

        public enum StatusAntiFraude
        {
            Aguardando = 'A',
            Aprovado = 'P',
            Fraude = 'F',
            Todos = 'T',
            Invalido = 'I',
            Cancelado = 'C',
            EmAnalise = 'N'
        }
        public enum enumNivelRisco
        {
            CompraDeRisco = 1,
            SemRisco = 0,
            Fraude = 2,
            FraudeCancelada = 3
        }
        public enum enumIsFraude
        {
            FraudeSim = 1,
            FraudeNao = 0
        }

        public enum enumForceStatus
        {
            Review = 'R',
            Aprovar = 'A'
        }

        public DataTable RetornoRelatorioConsolidado(DateTime Inicio, DateTime Fim)
        {
            BD banco = new BD();
            Fim = Fim.AddDays(1);
            IDataReader dr = banco.Consulta(@"SELECT DISTINCT vb.ID VendaBilheteriaID, fp.ID FormaPagamentoID, fp.Parcelas, fp.Tipo, vb.DataVenda, vbf.Valor
                FROM tVendaBilheteria vb
                INNER JOIN tVendaBilheteriaFormaPagamento vbf ON vb.ID = vbf.VendaBilheteriaID
                INNER JOIN tFormaPagamento fp ON fp.ID = vbf.FormaPagamentoID
                INNER JOIN tIngressoLog il ON il.VendaBilheteriaID = vb.ID
                INNER JOIN tIngresso i ON i.ID = il.IngressoID
                INNER JOIN tEvento e ON e.ID = i.EventoID
                WHERE vb.Status = 'P' AND 
                e.Financeiro='T' AND 
                vb.DataVenda >= '" + Inicio.ToString("yyyyMMdd") + "' AND vb.DataVenda < '" + Fim.ToString("yyyyMMdd") + "'");
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            banco.FecharConsulta();
            DataTable dtRetorno = new DataTable();
            dtRetorno.Columns.Add("DataRecebimento", typeof(string));
            dtRetorno.Columns.Add("Valor", typeof(decimal));
            foreach (DataRow drow in dt.Rows)
            {
                try
                {
                    DataRow drNew = dtRetorno.NewRow();
                    DateTime dtRecebe = DateTime.ParseExact((string)drow["DataVenda"], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    int formaPagamentoID = drow["FormaPagamentoID"] == DBNull.Value ? -1 : (int)drow["FormaPagamentoID"];
                    int parcelas = (int)drow["Parcelas"]; //== DBNull.Value ? -1 : (int)drow["Parcelas"];
                    int tipo = drow["Tipo"] == DBNull.Value ? -1 : (int)drow["Tipo"];
                    dtRecebe = dtRecebe.AddDays(RetornaDiasFormaPagamento(formaPagamentoID, parcelas, tipo));
                    if (dtRecebe.DayOfWeek == DayOfWeek.Saturday)
                        dtRecebe = dtRecebe.AddDays(2);
                    if (dtRecebe.DayOfWeek == DayOfWeek.Sunday)
                        dtRecebe = dtRecebe.AddDays(1);
                    drNew["DataRecebimento"] = dtRecebe.ToString("yyyyMMdd");
                    drNew["Valor"] = drow["Valor"];
                    dtRetorno.Rows.Add(drNew);
                }
                catch { continue; }
            }

            dt = dtRetorno.Clone();

            //faz o distinct do datatable através da data
            DataTable distinct = CTLib.TabelaMemoria.Distinct(dtRetorno, "DataRecebimento");
            DataTable aux = dtRetorno.Clone();

            //faz o group by através da data
            foreach (DataRow linha in distinct.Rows)
            {
                DataRow novalinha = aux.NewRow();
                novalinha["DataRecebimento"] = linha["DataRecebimento"];
                novalinha["Valor"] = dtRetorno.Compute("SUM(Valor)", "DataRecebimento='" + (string)linha["DataRecebimento"] + "'");
                aux.Rows.Add(novalinha);
            }

            //ordena e transforma em datatable
            foreach (DataRow linha in aux.Select("1=1", "DataRecebimento ASC"))
                dt.ImportRow(linha);
            //transforma as datas em um formato padrão
            foreach (DataRow linha in dt.Rows)
            {
                DateTime data = DateTime.ParseExact(linha["DataRecebimento"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                linha["DataRecebimento"] = data.ToString("dd/MM/yyyy");
            }

            return dt;
        }

        public bool IsValeIngresso(string senha)
        {
            try
            {
                bool valeIngresso = false, IngressoNormal = false;

                string sql = @"SELECT 
                                CASE WHEN (i.VendaBilheteriaID IS NULL) THEN 'F' ELSE 'T' END AS Ingresso, 
                                CASE WHEN (vi.VendaBilheteriaID IS NULL) THEN 'F' ELSE 'T' END AS ValeIngresso 
                                FROM tVendaBilheteria vb(NOLOCK)
                                LEFT JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                                LEFT JOIN tValeIngresso vi (NOLOCK) ON vi.VendaBilheteriaID = vb.ID
                                WHERE vb.Senha = '" + senha + "'" +
                                " GROUP BY i.VendaBilheteriaID, vi.VendaBilheteriaID";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    valeIngresso = bd.LerBoolean("ValeIngresso");
                    IngressoNormal = bd.LerBoolean("Ingresso");
                }
                if (!valeIngresso && !IngressoNormal)
                    throw new Exception("Senha não contêm ingressos ou vale ingressos");
                else
                    return valeIngresso;

            }
            catch (Exception)
            {

                throw;
            }
            finally { bd.Fechar(); }
        }

        /// <summary>
        /// Função utilizada pela RetornoRelatorioConsolidado para calcular
        /// quantos dias demoram após o pagamento para que a Ingresso Rápido receba.
        /// </summary>
        /// <param name="formaPagamentoID">ID da Forma de Pagamento utilizada.</param>
        /// <param name="parcelas">Número de parcelas da forma de pagamento</param>
        /// <returns>Número de dias para receber o valor</returns>
        private int RetornaDiasFormaPagamento(int formaPagamentoID, int parcelas, int Tipo)
        {
            switch (Tipo)
            {
                case 0:
                    switch (formaPagamentoID)
                    {
                        //se for Visa
                        case 2:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 76:
                        case 77:
                        case 78:
                        case 79:
                        case 80:
                        case 81:
                        case 82:
                        case 83:
                        case 84:
                        case 85:
                            if (parcelas <= 10)
                                return parcelas * 15;
                            else
                                throw new Exception("O número de parcelas é superior ao permitido para esta bandeira!");
                        //se for RedeCard
                        case 3:
                        case 7:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                        case 90:
                        case 91:
                        case 92:
                        case 93:
                        case 94:
                        case 95:
                            if (parcelas <= 6)
                                return parcelas * 18;
                            else
                                throw new Exception("O número de parcelas é superior ao permitido para esta bandeira!");
                        //se for Amex
                        case 4:
                        case 64:
                        case 65:
                        case 70:
                        case 71:
                        case 72:
                        case 96:
                        case 97:
                        case 98:
                        case 99:
                        case 100:
                        case 101:
                        case 102:
                        case 103:
                        case 104:
                        case 105:
                            if (parcelas <= 6)
                                return parcelas * 30;
                            else
                                throw new Exception("O número de parcelas é superior ao permitido para esta bandeira!");
                        default:
                            throw new Exception("A forma de pagamento marcada como cartão de crédito não está definida para nenhuma bandeira!");

                    }
                case 1:
                    return 0;
                case 2:
                    return 1;
                default:
                    throw new Exception("O tipo de forma de pagamento não está compreendido no intervalo de tipos válidos!");
            }
        }

        /// <summary>
        /// Inserir novo(a) VendaBilheteria
        /// </summary>
        /// <returns></returns>	
        public string StringInserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                //Random rnd = new Random((int)System.DateTime.Now.Ticks);
                //int n = rnd.Next(1, 9999);
                //A SENHA POSSUI 14 DIGITOS !!!!!
                //string senha = n.ToString("0000") + this.Control.ID.ToString("000000");

                sql = new StringBuilder();
                sql.EnsureCapacity(800000);
                sql.Append("INSERT INTO tVendaBilheteria(CaixaID, ClienteID, DataVenda, Status, " +
                    "TaxaEntregaID, TaxaEntregaValor, TaxaConvenienciaValorTotal, ValorTotal, Obs, " +
                    "NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, " +
                    "NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, ComissaoValorTotal, " +
                    "DDD, NumeroCelular, ModelIDCelular, FabricanteCelular , NivelRisco, IP, AprovacaoAutomatica, QuantidadeImpressoesInternet,ClienteEnderecoID,EntregaControleID, " +
                    "EntregaAgendaID,PdvID, PagamentoProcessado, NomeCartao, ValorSeguro, TaxaProcessamentoValor, AccertifyForceStatus,VendaBilhereriaIDTroca) ");

                sql.Append("VALUES (@001,@002,'@004','@005',@006,'@007','@008','@009','@010','@011','@012'," +
                    "'@013',@014,'@015','@016','@017','@018','@019','@020','@021', @022, @023, @024, '@025', @026, '@027', '@028', @029,@030,@031,@032,@033, '@034', '@035', @036, @037, '@038',@039);SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.CaixaID.ValorBD);
                sql.Replace("@002", this.ClienteID.ValorBD);
                sql.Replace("@004", this.DataVenda.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.TaxaEntregaID.ValorBD);
                sql.Replace("@007", this.TaxaEntregaValor.ValorBD);
                sql.Replace("@008", this.TaxaConvenienciaValorTotal.ValorBD);
                sql.Replace("@009", this.ValorTotal.ValorBD);
                sql.Replace("@010", this.Obs.ValorBD);
                sql.Replace("@011", this.NotaFiscalCliente.ValorBD);
                sql.Replace("@012", this.NotaFiscalEstabelecimento.ValorBD);
                sql.Replace("@013", this.IndiceInstituicaoTransacao.ValorBD);
                sql.Replace("@014", this.IndiceTipoCartao.ValorBD);
                sql.Replace("@015", this.NSUSitef.ValorBD);
                sql.Replace("@016", this.NSUHost.ValorBD);
                sql.Replace("@017", this.CodigoAutorizacaoCredito.ValorBD);
                sql.Replace("@018", this.BIN.ValorBD);
                sql.Replace("@019", this.ModalidadePagamentoCodigo.ValorBD);
                sql.Replace("@020", this.ModalidadePagamentoTexto.ValorBD);
                sql.Replace("@021", this.ComissaoValorTotal.ValorBD);
                sql.Replace("@022", this.DDD.ValorBD);
                sql.Replace("@023", this.NumeroCelular.ValorBD);
                sql.Replace("@024", this.ModelIDCelular.ValorBD);
                sql.Replace("@025", this.FabricanteCelular.ValorBD);
                sql.Replace("@026", this.NivelRisco.ValorBD);
                sql.Replace("@027", this.IP.ValorBD);
                sql.Replace("@028", this.AprovacaoAutomatica.ValorBD);
                sql.Replace("@029", this.QuantidadeImpressoesInternet.ValorBD);
                sql.Replace("@030", this.ClienteEnderecoID.ValorBD);
                sql.Replace("@031", this.EntregaControleID.ValorBD);
                sql.Replace("@032", this.EntregaAgendaID.ValorBD);
                sql.Replace("@033", this.PdvID.ValorBD);
                sql.Replace("@034", this.PagamentoProcessado.ValorBD);
                sql.Replace("@035", this.NomeCartao.ValorBD);
                sql.Replace("@036", this.ValorSeguro.ValorBD);
                sql.Replace("@037", this.TaxaProcessamentoValor.ValorBD);
                sql.Replace("@038", this.AccertifyForceStatus.ValorBD);
                sql.Replace("@039", this.VendaBilhereriaIDTroca.ValorBD);
                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Atualiza o NivelRisco da Venda Bilheteria para aparecer no Modulo Anti Fraude
        /// Desta forma será possível visualizar Fraudes canceladas sem modificar o novo registro de cancelamento.
        /// </summary>
        /// <param name="vendaBilheteriaID"></param>
        /// <returns></returns>
        public string StringAtualizarNivelRisco(int vendaBilheteriaID)
        {
            return string.Format(@"UPDATE tVendaBilheteria SET NivelRisco = 3 WHERE ID = {0}", vendaBilheteriaID);
        }

        /// <summary>
        /// Inserir novo(a) VendaBilheteria
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                //				sql.Append("SELECT MAX(ID) AS ID FROM tVendaBilheteria");
                //				object obj = bd.ConsultaValor(sql);
                //				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
                //
                //				this.Control.ID = ++id;

                Random rnd = new Random((int)System.DateTime.Now.Ticks);
                int n = rnd.Next(1, 9999);
                //A SENHA POSSUI 10 DIGITOS !!!!!
                string senha = n.ToString("0000") + this.Control.ID.ToString("000000");

                this.Senha.Valor = senha;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteria(CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaEntregaValor, TaxaConvenienciaValorTotal, ValorTotal, Obs, QuantidadeImpressoesInternet) ");
                sql.Append("VALUES (@001,@002,'@003','@004','@005',@006,'@007','@008','@009','@010','@011', @012); SELECT SCOPE_IDENTITY()");

                //sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CaixaID.ValorBD);
                sql.Replace("@002", this.ClienteID.ValorBD);
                sql.Replace("@003", this.Senha.ValorBD);
                sql.Replace("@004", this.DataVenda.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.TaxaEntregaID.ValorBD);
                sql.Replace("@007", this.TaxaEntregaValor.ValorBD);
                sql.Replace("@008", this.TaxaConvenienciaValorTotal.ValorBD);
                sql.Replace("@009", this.ValorTotal.ValorBD);
                sql.Replace("@010", this.Obs.ValorBD);
                sql.Replace("@011", this.ComissaoValorTotal.ValorBD);
                sql.Replace("@012", this.QuantidadeImpressoesInternet.ValorBD);
                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bd.Fechar();

                bool result = this.Control.ID > 0;

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna a descricao do Status a partir da letra convencionada
        /// </summary>
        /// <returns></returns>
        public static string StatusDescritivo(string letraStatus)
        {
            string resultado = "";
            switch (letraStatus)
            {
                case PAGO:
                    resultado = "Pago";
                    break;
                //				case "A":
                //					resultado = "Aguardando";
                //					break;
                case ENTREGUE:
                    resultado = "Entregue";
                    break;
                //				case "R":
                //					resultado = "Recusado";
                //					break;
                case CANCELADO:
                    resultado = "Cancelado";
                    break;
                default:
                    resultado = letraStatus;
                    break;
            }
            return resultado;
        }

        /// <summary>
        /// Obtendo um DataTable dos dados para Impressao de Ingressos em funcao dos Itens de Venda
        /// </summary>
        public override DataTable DadosParaIngresso(int[] itens)
        {
            DataTable tabela = Ingresso.EstruturaImpressao();
            string sql;
            try
            {
                // Checar para cada item 
                for (int conta = 0; conta < itens.Length; conta++)
                {
                    if (itens[conta] > 0)
                    { // itens vem do Grid, se nao foi selecionado eh-1
                        // Obtendo dados atraves de SQL
                        sql =
                            "SELECT DISTINCT " +
                                    "tIngressoLog.IngressoID, tIngresso.LocalID, tVendaBilheteria.Senha, tCliente.Nome AS Cliente, tCliente.ID AS ClienteID, tIngresso.Codigo, tPreco.Nome AS Preco, tPreco.Valor, tCortesia.Nome AS Cortesia,  " +
                                    "tIngresso.ID, tIngresso.CodigoBarra, tIngresso.CodigoBarraCliente, tSetor.Produto, tApresentacao.Horario, tSetor.Nome AS Setor, tEvento.Nome AS Evento, tApresentacaoSetor.ApresentacaoID, tIngresso.ApresentacaoSetorID,  " +
                                    "tApresentacao.EventoID, tUsuario.Login AS Usuario, tLoja.Nome AS Loja, tVendaBilheteria.TaxaConvenienciaValorTotal, tVendaBilheteria.TaxaEntregaValor,  " +
                                    "tVendaBilheteriaItem.PacoteID " +
                            "FROM tCaixa INNER JOIN " +
                         "tVendaBilheteria INNER JOIN " +
                         "tCliente ON tVendaBilheteria.ClienteID = tCliente.ID INNER JOIN " +
                         "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                         "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                         "tLoja ON tCaixa.LojaID = tLoja.ID RIGHT OUTER JOIN " +
                         "tEvento INNER JOIN " +
                         "tApresentacaoSetor INNER JOIN " +
                         "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                         "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID ON tEvento.ID = tApresentacao.EventoID RIGHT OUTER JOIN " +
                         "tPreco RIGHT OUTER JOIN " +
                         "tIngresso ON tPreco.ID = tIngresso.PrecoID ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID ON tIngressoLog.IngressoID = tIngresso.ID LEFT OUTER JOIN " +
                         "tUsuario ON tIngresso.UsuarioID = tUsuario.ID LEFT OUTER JOIN " +
                         "tCortesia ON tIngresso.CortesiaID = tCortesia.ID " +
                            "WHERE     (tVendaBilheteriaItem.ID = N'" + itens[conta] + "') " +
                            "ORDER BY tPreco.Nome ";
                        bd.Consulta(sql);
                        // Alimentando DataTable (cada item pode ter varios ingressos quando eh Pacote)
                        int primeiraVez = 0;
                        //						bool soUmaVez =false;
                        while (bd.Consulta().Read())
                        {
                            //							if (!soUmaVez) {
                            DataRow linha = tabela.NewRow();
                            primeiraVez++;
                            if (primeiraVez == 1)
                            {
                                linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                                linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                            }
                            else
                            {
                                linha["TaxaConvenienciaValorTotal"] = 0;
                                linha["TaxaEntregaValor"] = 0;
                            }
                            linha["ID"] = bd.LerInt("ID");
                            linha["LocalID"] = bd.LerInt("LocalID");
                            linha["Usuario"] = bd.LerString("Usuario");  //nome do Usuario
                            linha["Evento"] = bd.LerString("Evento");  //nome do Evento
                            linha["Horario"] = bd.LerDateTime("Horario");  //horario da Apresentacao
                            linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                            linha["Setor"] = bd.LerString("Setor");  //nome do Setor
                            linha["Produto"] = bd.LerBoolean("Produto");  //se o setor eh produto ou nao
                            linha["Cortesia"] = bd.LerString("Cortesia");  //nome da Cortesia
                            linha["Preco"] = bd.LerString("Preco");  //nome do preco
                            linha["Valor"] = bd.LerDecimal("Valor");  //valor do Ingresso
                            linha["Codigo"] = bd.LerString("Codigo");  //codigo do Ingresso
                            linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");  //codigo de barra do Ingresso
                            linha["CodigoBarra"] = bd.LerString("CodigoBarra");  //codigo de barra do Ingresso
                            linha["Loja"] = bd.LerString("Loja");  // Loja da venda
                            linha["Senha"] = bd.LerString("Senha");  //senha da venda
                            linha["Cliente"] = bd.LerString("Cliente"); //cliente que comprou o ingresso
                            linha["ClienteID"] = bd.LerInt("ClienteID"); // ID cliente que comprou o ingresso
                            // Campos usados para as imagens
                            linha["EventoID"] = bd.LerInt("EventoID");  //id do Evento
                            linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");  //id da Apresentacao
                            linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                            tabela.Rows.Add(linha);
                            //							}
                            //							if (bd.LerInt("PacoteID")>0 ){
                            //								soUmaVez = false;
                            //							}else { // evita de ler Log de ingresso
                            //								soUmaVez = true;
                            //							}
                        } // fim do while			
                        bd.Fechar();
                    }
                }
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        } // fim de DadosParaIngresso		

        /// <summary>
        /// Obtendo um DataTable dos dados para Impressao de Ingressos em funcao da Senha
        /// </summary>
        public override DataTable DadosParaIngresso()
        {
            DataTable tabela = Ingresso.EstruturaImpressao();
            try
            {
                // Obtendo dados atraves de SQL
                string sql =
                "SELECT        tVendaBilheteria.Senha, tCliente.Nome AS Cliente, tCliente.ID AS ClienteID, tIngresso.Codigo, tPreco.Nome AS Preco, tPreco.Valor, tCortesia.Nome AS Cortesia, tIngresso.ID, tSetor.Produto, " +
                    "tIngresso.LocalID, tApresentacao.Horario, tIngresso.CodigoBarra, tIngresso.CodigoBarraCliente, tSetor.Nome AS Setor, tEvento.Nome AS Evento, tApresentacaoSetor.ApresentacaoID, tIngresso.ApresentacaoSetorID, tApresentacao.EventoID,  " +
                    "tUsuario.Login AS Usuario, tLoja.Nome AS Loja, tVendaBilheteria.TaxaConvenienciaValorTotal, tVendaBilheteria.TaxaEntregaValor " +
                    "FROM            tCaixa INNER JOIN " +
                    "tVendaBilheteria INNER JOIN " +
                    "tCliente ON tVendaBilheteria.ClienteID = tCliente.ID INNER JOIN " +
                    "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                    "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                    "tLoja ON tCaixa.LojaID = tLoja.ID RIGHT OUTER JOIN " +
                    "tEvento INNER JOIN " +
                    "tApresentacaoSetor INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID ON tEvento.ID = tApresentacao.EventoID RIGHT OUTER JOIN " +
                    "tPreco RIGHT OUTER JOIN " +
                    "tIngresso ON tPreco.ID = tIngresso.PrecoID ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID ON tIngressoLog.IngressoID = tIngresso.ID LEFT OUTER JOIN " +
                    "tUsuario ON tIngresso.UsuarioID = tUsuario.ID LEFT OUTER JOIN " +
                    "tCortesia ON tIngresso.CortesiaID = tCortesia.ID " +
                "WHERE     (tVendaBilheteria.Senha = N'" + this.Senha.Valor + "') " +
                    "ORDER BY tPreco.Nome";
                bd.Consulta(sql);
                int primeiraVez = 0;
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    primeiraVez++;
                    if (primeiraVez == 1)
                    {
                        linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                    }
                    else
                    {
                        linha["TaxaConvenienciaValorTotal"] = 0;
                        linha["TaxaEntregaValor"] = 0;
                    }
                    linha["ID"] = bd.LerInt("ID");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["Usuario"] = bd.LerString("Usuario");  //nome do Usuario
                    linha["Evento"] = bd.LerString("Evento");  //nome do Evento
                    linha["Horario"] = bd.LerDateTime("Horario");  //horario da Apresentacao
                    linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                    linha["Setor"] = bd.LerString("Setor");  //nome do Setor
                    linha["Produto"] = bd.LerBoolean("Produto");  //se o setor eh produto ou nao
                    linha["Cortesia"] = bd.LerString("Cortesia");  //nome da Cortesia
                    linha["Preco"] = bd.LerString("Preco");  //nome do preco
                    linha["Valor"] = bd.LerDecimal("Valor");  //valor do Ingresso
                    linha["Codigo"] = bd.LerString("Codigo");  //codigo do Ingresso
                    linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");  //codigo de barra do Ingresso
                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");  //codigo de barra do Ingresso
                    linha["Senha"] = bd.LerString("Senha");  //senha da venda
                    linha["Cliente"] = bd.LerString("Cliente"); //cliente que comprou o ingresso
                    linha["ClienteID"] = bd.LerInt("ClienteID"); // ID do cliente que comprou o ingresso
                    linha["Loja"] = bd.LerString("Loja"); //cliente que comprou o ingresso
                    // Campos usados para as imagens
                    linha["EventoID"] = bd.LerInt("EventoID");  //id do Evento
                    linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");  //id da Apresentacao
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        } // fim de DadosParaIngresso		

        /// <summary>
        /// Saber se deve Emitir Comprovante pelo flag do Canal em que esta Venda foi efetuada
        /// </summary>
        public override bool EmitirComprovante()
        {
            bool emitirComprovante = false;
            try
            {
                //	Descobir o Canal desta venda
                string sql =
                    "SELECT       tCanal.Comprovante, tVendaBilheteria.Senha, tLoja.CanalID, tCaixa.LojaID " +
                    "FROM         tVendaBilheteria INNER JOIN " +
                                    "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                                    "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                                    "tCanal ON tLoja.CanalID = tCanal.ID " +
                    "WHERE        (tVendaBilheteria.Senha = N'" + this.Senha.Valor + "') ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    //	Saber se deve ou não Emitir Comprovante
                    emitirComprovante = (bool)bd.LerBoolean("Comprovante");
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return emitirComprovante;
        } // fim de EmitirComprovante

        /// <summary>		
        /// Gerar uma senha para essa venda
        /// </summary>
        /// <returns></returns>
        public override string GerarSenha()
        {

            try
            {

                if (this.Control.ID == 0)
                    throw new Exception("ID eh zero. Objeto nao foi carregado ou salvo.");

                Random rnd = new Random((int)System.DateTime.Now.Ticks);

                int x = rnd.Next(1, 9999);

                //A SENHA POSSUI 10 DIGITOS !!!!!

                string senha = x.ToString("0000") + this.Control.ID.ToString("000000");

                this.Senha.Valor = senha;

                //				this.Atualizar();

                return senha;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter os itens dessa venda
        /// </summary>
        /// <returns></returns>
        public override DataTable Itens()
        {
            try
            {
                DataTable tabela = new DataTable("Senhas");
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("Pagamento", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                string sql =
                    "SELECT     tVendaBilheteriaFormaPagamento.FormaPagamentoID, tFormaPagamento.Nome AS Pagamento, tVendaBilheteriaFormaPagamento.Valor " +
                    "FROM       tVendaBilheteria INNER JOIN " +
                                    "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                                    "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE     (tVendaBilheteria.ID = " + this.Control.ID + ")";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    linha["Pagamento"] = bd.LerString("Pagamento");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Itens

        /// <summary>		
        /// Obter as senhas de um cliente (não incluir cancelados)
        /// </summary>
        public DataTable SenhasSoPagos(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Senhas");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Senha", typeof(string));
                string sql = "SELECT vb.ID, vb.Senha FROM tVendaBilheteria as vb " +
                    "WHERE vb.ClienteID=" + this.ClienteID.Valor + " AND vb.Status = '" + PAGO + "'";
                bd.Consulta(sql);
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Senha"] = bd.LerString("Senha");
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
        /// Obter as senhas de um cliente
        /// </summary>
        /// <returns></returns>
        public override DataTable Senhas(int clienteid)
        {

            try
            {

                DataTable tabela = new DataTable("Senhas");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Senha", typeof(string));

                string sql = "SELECT ID, Senha FROM tVendaBilheteria (NOLOCK) " +
                    "WHERE Status IN('" + PAGO + "','" + EMANALISE + "', '" + AGUARDANDO_APROVACAO + "') AND ClienteID=" + clienteid;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Senha"] = bd.LerString("Senha");
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
        /// Obter as senhas de um cliente
        /// </summary>
        /// <param name="registroZero">Um string que vai conter a descricao do registro zero</param>
        /// <returns></returns>
        public DataTable Senhas(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Senhas");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Senha", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql =
                    "SELECT        ID, Senha, Status " +
                    "FROM            tVendaBilheteria " +
                    "WHERE        (ClienteID = " + this.ClienteID.Valor + ") AND (Status <> N'R')";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Senha"] = bd.LerString("Senha");
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
        /// Obter as forma de pagamento de uma venda
        /// </summary>
        public override DataTable FormasPagamento()
        {
            try
            {
                DataTable tabela = new DataTable("FormaPagamento");
                string consulta = @"SELECT tFormaPagamento.ID , tFormaPagamento.Nome AS FormaPagamento, 
                    tVendaBilheteriaFormaPagamento.Valor, tFormaPagamento.BandeiraID, tVendaBilheteriaFormaPagamento.Rede,
                    tFormaPagamento.Tipo as TipoPagamento
                    FROM tVendaBilheteria (NOLOCK)
                    INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID 
                    INNER JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID 
                    WHERE tVendaBilheteriaFormaPagamento.VendaBilheteriaID={0}";
                consulta = string.Format(consulta, this.Control.ID);

                tabela.Load(bd.Consulta(consulta));
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter as parcelas de uma Venda em função da % do valor total informado
        /// </summary>
        public override DataTable ParcelasPagamento(string registroZero, decimal valorTotal)
        {
            try
            {
                decimal porcentagem = 0;
                DataTable tabela = new DataTable("ItensPagamentoTabela");
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("Pagamento", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql =
                    "SELECT   tFormaPagamento.Nome, tVendaBilheteriaFormaPagamento.Valor, tVendaBilheteriaFormaPagamento.Porcentagem, tVendaBilheteriaFormaPagamento.FormaPagamentoID, " +
                         "tVendaBilheteriaFormaPagamento.ID AS VendaBilheteriaFormaPagamentoID " +
                        "FROM       tVendaBilheteria INNER JOIN " +
                                    "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                                    "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE      (tVendaBilheteria.ID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                decimal valorParcial = 0;
                bool leuPeloMenosUm = false;
                if (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    linha["Pagamento"] = bd.LerString("Nome");
                    porcentagem = bd.LerDecimal("Porcentagem");
                    linha["Valor"] = Math.Round(valorTotal * porcentagem / 100, 2);
                    tabela.Rows.Add(linha);
                    leuPeloMenosUm = true;
                }
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    linha["Pagamento"] = bd.LerString("Nome");
                    porcentagem = bd.LerDecimal("Porcentagem");
                    linha["Valor"] = Math.Round(valorTotal * porcentagem / 100, 2);
                    tabela.Rows.Add(linha);
                    // 
                    valorParcial += Convert.ToDecimal(linha["Valor"]);
                }
                // Evita problema de arredondamento, modificando a priemira parcela
                if (leuPeloMenosUm)
                {
                    valorParcial = valorTotal - valorParcial;
                    tabela.Rows[0]["Valor"] = valorParcial;
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable EstruturaVendasResumo()
        {
            DataTable tabela = new DataTable("VendasResumo");
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("FormaPagamento", typeof(string));
            tabela.Columns.Add("Ingresso", typeof(decimal));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("Entrega", typeof(decimal));
            tabela.Columns.Add("Conveniência", typeof(decimal));
            tabela.Columns.Add("Total", typeof(decimal));
            return tabela;
        }

        public override DataTable VendasResumo(int caixaID)
        {
            try
            {
                string obterTotalVendidos = "";
                string obterTotalCancelados = "";
                DataRow linhaVendido = null;
                DataRow linhaCancelado = null;
                bool temCancelado = false;
                bool temVendido = false;
                DataTable tabela = EstruturaVendasResumo();
                // 
                Caixa caixa = new Caixa();
                caixa.Ler(caixaID);
                //				string  pagamentosDoCanalDoCaixa = caixa.FormaPagamentos(); 
                //				não posso porque pode estornar por um pagamento que não seja deste canal
                // Para cada forma de pagamento
                FormaPagamentoLista formaPagamentoLista = new FormaPagamentoLista();
                //				formaPagamentoLista.FiltroSQL = "ID IN ("+pagamentosDoCanalDoCaixa+")";
                formaPagamentoLista.OrdemSQL = "Nome";
                formaPagamentoLista.Carregar();
                formaPagamentoLista.Primeiro();
                for (int contador = 0; contador < formaPagamentoLista.Tamanho; contador++)
                {
                    #region Obter Soma dos Vendidos (inclui Entregues)
                    BD bancoVendidos = new BD();
                    // sem taxas
                    obterTotalVendidos =
                    "SELECT        tFormaPagamento.Nome, SUM(tVendaBilheteriaFormaPagamento.Valor) AS ValorIngressos, " +
                        "SUM(tVendaBilheteria.TaxaEntregaValor * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Entrega,  " +
                        "SUM(tVendaBilheteria.TaxaConvenienciaValorTotal * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Conveniencia, tVendaBilheteria.CaixaID " +
                        "FROM            tVendaBilheteria INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE        (tVendaBilheteria.Status = N'P' OR " +
                        "tVendaBilheteria.Status = N'E') AND (tFormaPagamento.Nome = '" + formaPagamentoLista.FormaPagamento.Nome.Valor + "') " +
                        "GROUP BY tFormaPagamento.Nome, tVendaBilheteria.CaixaID " +
                        "HAVING        (tVendaBilheteria.CaixaID = " + caixaID + ") " +
                        "ORDER BY tFormaPagamento.Nome ";
                    bancoVendidos.Consulta(obterTotalVendidos);
                    // Obter Soma dos Cancelados
                    BD bancoCancelados = new BD();
                    obterTotalCancelados =
                        "SELECT        tFormaPagamento.Nome, SUM(tVendaBilheteriaFormaPagamento.Valor) AS ValorIngressos, " +
                        "SUM(tVendaBilheteria.TaxaEntregaValor * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Entrega,  " +
                        "SUM(tVendaBilheteria.TaxaConvenienciaValorTotal * tVendaBilheteriaFormaPagamento.Porcentagem / 100) AS Conveniencia, tVendaBilheteria.CaixaID " +
                        "FROM       tVendaBilheteria INNER JOIN " +
                        "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                        "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE      (tVendaBilheteria.Status = N'C') AND (tFormaPagamento.Nome = '" + formaPagamentoLista.FormaPagamento.Nome.Valor + "') " +
                        "GROUP BY tFormaPagamento.Nome, tVendaBilheteria.CaixaID " +
                        "HAVING     (tVendaBilheteria.CaixaID = " + caixaID + ") " +
                        "ORDER BY tFormaPagamento.Nome ";
                    bancoCancelados.Consulta(obterTotalCancelados);
                    #endregion
                    #region Atribuindo da tabela do banco para linha do DataTable
                    if (bancoVendidos.Consulta().Read())
                    {
                        linhaVendido = tabela.NewRow();
                        linhaVendido["FormaPagamento"] = bancoVendidos.LerString("Nome");
                        string sqlIngressoLogID =
                            "SELECT   tIngressoLog.ID AS IngressoLogID " +
                            "FROM     tCaixa INNER JOIN " +
                            "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                            "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                            "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                            "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                            "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                            "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                            "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                            "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                            "tCanal ON tLoja.CanalID = tCanal.ID INNER JOIN " +
                            "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                            "WHERE    (tCaixa.ID = " + caixaID + ") AND (tIngressoLog.Acao IN ('" + IngressoLog.VENDER + "')) ";
                        //						BD obterDados = new BD();
                        //						obterDados.Consulta(sqlIngressoLogID);
                        //						if(obterDados.Consulta().Read())
                        linhaVendido["Ingresso"] = formaPagamentoLista.FormaPagamento.QuantidadeIngressosPorFormaPagamento(sqlIngressoLogID);
                        //						else
                        //							linhaVendido["Ingresso"] = 0;
                        linhaVendido["Entrega"] = bancoVendidos.LerDecimal("Entrega");
                        linhaVendido["Conveniência"] = bancoVendidos.LerDecimal("Conveniencia");
                        linhaVendido["Total"] = bancoVendidos.LerDecimal("ValorIngressos");  // já vem com taxas quando pega pela forma pagamento 
                        // nome deste campo é muito infeliz
                        linhaVendido["Valor"] = Convert.ToDecimal(linhaVendido["Total"]) - Convert.ToDecimal(linhaVendido["Entrega"]) - Convert.ToDecimal(linhaVendido["Conveniência"]);
                        linhaVendido["Status"] = "Vendidos";
                        temVendido = true;
                        // formato
                        linhaVendido["Entrega"] = Convert.ToDecimal(linhaVendido["Entrega"]).ToString(Utilitario.FormatoMoeda);
                        linhaVendido["Conveniência"] = Convert.ToDecimal(linhaVendido["Conveniência"]).ToString(Utilitario.FormatoMoeda);
                        linhaVendido["Total"] = Convert.ToDecimal(linhaVendido["Total"]).ToString(Utilitario.FormatoMoeda);
                        linhaVendido["Valor"] = Convert.ToDecimal(linhaVendido["Valor"]).ToString(Utilitario.FormatoMoeda);
                    }
                    else
                    {
                        temVendido = false;
                    }
                    if (bancoCancelados.Consulta().Read())
                    {
                        linhaCancelado = tabela.NewRow();
                        linhaCancelado["FormaPagamento"] = bancoCancelados.LerString("Nome");
                        string sqlIngressoLogID =
                            "SELECT   tIngressoLog.ID AS IngressoLogID " +
                            "FROM     tCaixa INNER JOIN " +
                            "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                            "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                            "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                            "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                            "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                            "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                            "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                            "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                            "tCanal ON tLoja.CanalID = tCanal.ID INNER JOIN " +
                            "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                            "WHERE    (tCaixa.ID = " + caixaID + ") AND (tIngressoLog.Acao IN ('" + IngressoLog.CANCELAR + "')) ";
                        //						BD obterDados = new BD();
                        //						obterDados.Consulta(sqlIngressoLogID);
                        //						if(obterDados.Consulta().Read())
                        linhaCancelado["Ingresso"] = formaPagamentoLista.FormaPagamento.QuantidadeIngressosPorFormaPagamento(sqlIngressoLogID);
                        //						else
                        //							linhaCancelado["Ingresso"] = 0;
                        linhaCancelado["Entrega"] = bancoCancelados.LerDecimal("Entrega");
                        linhaCancelado["Conveniência"] = bancoCancelados.LerDecimal("Conveniencia");
                        linhaCancelado["Total"] = bancoCancelados.LerDecimal("ValorIngressos");  // já vem com taxas quando pega pela forma pagamento 
                        linhaCancelado["Valor"] = Convert.ToDecimal(linhaCancelado["Total"]) - Convert.ToDecimal(linhaCancelado["Entrega"]) - Convert.ToDecimal(linhaCancelado["Conveniência"]);
                        linhaCancelado["Status"] = "Cancelados";
                        temCancelado = true;
                        // formato
                        linhaCancelado["Entrega"] = Convert.ToDecimal(linhaCancelado["Entrega"]).ToString(Utilitario.FormatoMoeda);
                        linhaCancelado["Conveniência"] = Convert.ToDecimal(linhaCancelado["Conveniência"]).ToString(Utilitario.FormatoMoeda);
                        linhaCancelado["Total"] = Convert.ToDecimal(linhaCancelado["Total"]).ToString(Utilitario.FormatoMoeda);
                        linhaCancelado["Valor"] = Convert.ToDecimal(linhaCancelado["Valor"]).ToString(Utilitario.FormatoMoeda);
                    }
                    else
                    {
                        temCancelado = false;
                    }
                    #endregion
                    #region Linha do Pagamento não existe se não tiver valores
                    if (temVendido && temCancelado)
                    {
                        // Adiciona linha se ambos não forem zero
                        tabela.Rows.Add(linhaVendido);
                        tabela.Rows.Add(linhaCancelado);
                    }
                    // Se não tiver uma das somas atribui zero 
                    if (temVendido && !temCancelado)
                    {
                        tabela.Rows.Add(linhaVendido);
                        //
                        linhaCancelado = tabela.NewRow();
                        linhaCancelado["FormaPagamento"] = bancoVendidos.LerString("Nome");
                        linhaCancelado["Ingresso"] = 0;
                        linhaCancelado["Valor"] = 0;
                        linhaCancelado["Entrega"] = 0;
                        linhaCancelado["Conveniência"] = 0;
                        linhaCancelado["Total"] = 0;
                        linhaCancelado["Status"] = "Cancelados";
                        tabela.Rows.Add(linhaCancelado);
                    }
                    if (temCancelado && !temVendido)
                    {
                        linhaVendido = tabela.NewRow();
                        linhaVendido["FormaPagamento"] = bancoCancelados.LerString("Nome");
                        linhaVendido["Ingresso"] = 0;
                        linhaVendido["Valor"] = 0;
                        linhaVendido["Entrega"] = 0;
                        linhaVendido["Conveniência"] = 0;
                        linhaVendido["Total"] = 0;
                        linhaVendido["Status"] = "Vendidos";
                        tabela.Rows.Add(linhaVendido);
                        //
                        tabela.Rows.Add(linhaCancelado);
                    }
                    #endregion
                    bancoVendidos.Fechar();
                    bancoCancelados.Fechar();
                    formaPagamentoLista.Proximo();
                }
                #region Se tiver cortesia neste Caixa, incluir tb
                #region Obter Soma dos Vendidos (inclui Entregues) e Cancelados
                BD bancoCortesiasVendidos = new BD();
                // sem taxas
                obterTotalVendidos =
                    "SELECT        tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor AS TaxaEntregaValor, tVendaBilheteria.TaxaConvenienciaValorTotal AS TaxaConvenienciaValorTotal,  " +
                                        "tVendaBilheteria.ValorTotal AS ValorTotal, COUNT(tIngressoLog.Acao) AS Quantidade " +
                    "FROM            tIngresso INNER JOIN " +
                                        "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "WHERE        (tIngressoLog.CortesiaID > 0) " +
                    "GROUP BY tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor, tVendaBilheteria.TaxaConvenienciaValorTotal, tVendaBilheteria.ValorTotal, tIngressoLog.Acao " +
                    "HAVING        (tIngressoLog.Acao = N'V') AND (tVendaBilheteria.CaixaID = " + caixaID + ") ";
                bancoCortesiasVendidos.Consulta(obterTotalVendidos);
                // Obter Soma dos Cancelados
                BD bancoCortesiasCancelados = new BD();
                obterTotalCancelados =
                    "SELECT        tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor AS TaxaEntregaValor, tVendaBilheteria.TaxaConvenienciaValorTotal AS TaxaConvenienciaValorTotal,  " +
                    "tVendaBilheteria.ValorTotal AS ValorTotal, COUNT(tIngressoLog.Acao) AS Quantidade " +
                    "FROM            tIngresso INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "WHERE        (tIngressoLog.CortesiaID > 0) " +
                    "GROUP BY tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor, tVendaBilheteria.TaxaConvenienciaValorTotal, tVendaBilheteria.ValorTotal, tIngressoLog.Acao " +
                    "HAVING        (tIngressoLog.Acao = N'C') AND (tVendaBilheteria.CaixaID = " + caixaID + ") ";
                bancoCortesiasVendidos.Consulta(obterTotalVendidos);
                bancoCortesiasCancelados.Consulta(obterTotalCancelados);
                #endregion
                #region Atribuindo da tabela do bancoCortesias para linha do DataTable
                if (bancoCortesiasVendidos.Consulta().Read())
                {
                    linhaVendido = tabela.NewRow();
                    linhaVendido["FormaPagamento"] = "Cortesias";
                    linhaVendido["Ingresso"] = bancoCortesiasVendidos.LerInt("Quantidade");
                    linhaVendido["Entrega"] = bancoCortesiasVendidos.LerDecimal("TaxaEntregaValor");
                    linhaVendido["Conveniência"] = bancoCortesiasVendidos.LerDecimal("TaxaConvenienciaValorTotal");
                    linhaVendido["Total"] = 0;
                    linhaVendido["Valor"] = 0;
                    linhaVendido["Status"] = "Vendidos";
                    temVendido = true;
                }
                else
                {
                    temVendido = false;
                }
                if (bancoCortesiasCancelados.Consulta().Read())
                {
                    linhaCancelado = tabela.NewRow();
                    linhaCancelado["FormaPagamento"] = "Cortesias";
                    linhaCancelado["Ingresso"] = bancoCortesiasCancelados.LerInt("Quantidade");
                    linhaCancelado["Entrega"] = bancoCortesiasCancelados.LerDecimal("TaxaEntregaValor");
                    linhaCancelado["Conveniência"] = bancoCortesiasCancelados.LerDecimal("TaxaConvenienciaValorTotal");
                    linhaCancelado["Total"] = 0;
                    linhaCancelado["Valor"] = 0;
                    linhaCancelado["Status"] = "Cancelados";
                    temCancelado = true;
                }
                else
                {
                    temCancelado = false;
                }
                #endregion
                #region Linha das Cortesias não existe se não tiver valores
                if (temVendido && temCancelado)
                {
                    // Adiciona linha se ambos não forem zero
                    tabela.Rows.Add(linhaVendido);
                    tabela.Rows.Add(linhaCancelado);
                }
                // Se não tiver uma das somas atribui zero 
                if (temVendido && !temCancelado)
                {
                    tabela.Rows.Add(linhaVendido);
                    //
                    linhaCancelado = tabela.NewRow();
                    linhaCancelado["FormaPagamento"] = "Cortesias";
                    linhaCancelado["Ingresso"] = 0;
                    linhaCancelado["Valor"] = 0;
                    linhaCancelado["Entrega"] = 0;
                    linhaCancelado["Conveniência"] = 0;
                    linhaCancelado["Total"] = 0;
                    linhaCancelado["Status"] = "Cancelados";
                    tabela.Rows.Add(linhaCancelado);
                }
                if (temCancelado && !temVendido)
                {
                    linhaVendido = tabela.NewRow();
                    linhaVendido["FormaPagamento"] = "Cortesias";
                    linhaVendido["Ingresso"] = 0;
                    linhaVendido["Valor"] = 0;
                    linhaVendido["Entrega"] = 0;
                    linhaVendido["Conveniência"] = 0;
                    linhaVendido["Total"] = 0;
                    linhaVendido["Status"] = "Vendidos";
                    tabela.Rows.Add(linhaVendido);
                    //
                    tabela.Rows.Add(linhaCancelado);
                }
                #endregion
                bancoCortesiasVendidos.Fechar();
                bancoCortesiasCancelados.Fechar();
                #endregion
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
        } // fim de VendasResumo		

        public override decimal Total()
        {
            decimal total = 0;
            try
            {
                // Obter total das vendas
                string sql =
                    "SELECT       SUM(tVendaBilheteriaFormaPagamento.Valor) AS Total, tVendaBilheteria.Senha, tVendaBilheteria.ID, tVendaBilheteria.TaxaEntregaValor " +
                    "FROM         tVendaBilheteria INNER JOIN " +
                                    "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID " +
                    "GROUP BY tVendaBilheteria.Senha, tVendaBilheteria.ID, tVendaBilheteria.TaxaEntregaValor " +
                    "HAVING        (tVendaBilheteria.ID = " + this.Control.ID + ")";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (bd.Consulta().Read())
                {
                    total = bd.LerDecimal("Total") - bd.LerDecimal("TaxaEntregaValor");
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return total;
        } // fim de Total	

        public override int QuantidadePagamentos()
        {
            int quantidade = 0;
            try
            {

                //return quantidade;
                // Obter quantidade das parcelas
                // COUNT DISTINCT nencessário para resolver o problema de dois registros com mesmo nome de pagamento!!
                string sql =
                    "SELECT COUNT(DISTINCT tVendaBilheteriaFormaPagamento.FormaPagamentoID) AS Quantidade " +
                    "FROM  tVendaBilheteria INNER JOIN " +
                      "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                      "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE  (tVendaBilheteria.Senha = N'" + this.Senha.Valor + "') ";

                //					"SELECT DISTINCT tFormaPagamento.Nome, COUNT(tVendaBilheteriaFormaPagamento.FormaPagamentoID) AS Quantidade "+
                //					"FROM    tVendaBilheteria INNER JOIN "+
                //                      "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                //                      "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                //					"WHERE   (tVendaBilheteria.Senha = N'"+ this.Senha.Valor+"') "+
                //					"GROUP BY tFormaPagamento.Nome ";
                //
                //					"SELECT   tFormaPagamento.Nome "+
                //					"FROM     tVendaBilheteria INNER JOIN "+
                //                         "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
                //                         "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
                //					"WHERE    (tVendaBilheteria.Senha = N'"+ this.Senha.Valor+"') "+
                //					"GROUP BY tFormaPagamento.Nome ";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (bd.Consulta().Read())
                {
                    quantidade = bd.LerInt("Quantidade");
                }

                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return quantidade;
        } // fim de QuantidadePagamentos	

        public override int QuantidadeIngressos()
        {
            int quantidade = 0;
            try
            {
                // Obter quantidade dos ingressos
                string sql =
                    "SELECT     COUNT(tIngressoLog.IngressoID) AS Quantidade " +
                    "FROM       tVendaBilheteria INNER JOIN " +
                                    "tIngressoLog ON tVendaBilheteria.ID = tIngressoLog.VendaBilheteriaID " +
                    "GROUP BY tVendaBilheteria.Senha " +
                    "HAVING        (tVendaBilheteria.Senha = N'" + this.Senha.Valor + "')";
                //					"SELECT        tVendaBilheteria.Senha, tIngressoLog.IngressoID "+
                //					"FROM            tVendaBilheteria INNER JOIN "+
                //													"tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN "+
                //													"tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN "+
                //													"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID "+
                //					"GROUP BY tVendaBilheteria.Senha, tIngressoLog.IngressoID "+
                //					"HAVING        (tVendaBilheteria.Senha = N'"+ this.Senha.Valor+"')";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (bd.Consulta().Read())
                {
                    quantidade = bd.LerInt("Quantidade");
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return quantidade;
        } // fim de QuantidadeIngressos	
        /// <summary>
        /// Obter quantidade de itens por Venda
        /// </summary>
        /// <returns>Quantidade de itens por Venda</returns>
        public override int QuantidadeItens()
        {
            int quantidade = 0;
            try
            {
                // Obter quantidade de itens
                string sql =
                    "SELECT        tVendaBilheteriaItem.VendaBilheteriaID, COUNT(tVendaBilheteriaItem.ID) AS Quantidade " +
                    "FROM          tVendaBilheteria INNER JOIN " +
                               "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID " +
                    "GROUP BY tVendaBilheteriaItem.VendaBilheteriaID " +
                    "HAVING        (tVendaBilheteriaItem.VendaBilheteriaID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                // Alimentando DataTable
                while (bd.Consulta().Read())
                {
                    quantidade = bd.LerInt("Quantidade");
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return quantidade;
        } // fim de QuantidadeItens	

        public void AtribuirIDPelaSenha()
        {
            try
            {
                int ID = 0;
                string sql =
                    "SELECT        ID, Senha " +
                    "FROM            tVendaBilheteria " +
                    "WHERE        (Senha = N'" + this.Senha.Valor + "') ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    ID = bd.LerInt("ID");
                }
                bd.Fechar();
                this.Control.ID = ID;
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
        } // fim de AtribuirIDPelaSenha	

        public string MotivoReimpressao()
        {
            try
            {
                string motivo = "";
                string sql =
                    "SELECT        tVendaBilheteria.Senha, tIngressoLog.Acao, tIngressoLog.Obs AS Motivo, tIngressoLog.ID " +
                    "FROM            tVendaBilheteria INNER JOIN " +
                    "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                    "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID " +
                    "WHERE        (tVendaBilheteria.Senha = N'" + this.Senha.Valor + "') AND (tIngressoLog.Acao = N'" + IngressoLog.REIMPRIMIR + "') " +
                    "ORDER BY tIngressoLog.ID DESC ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    motivo = bd.LerString("Motivo");
                }
                bd.Fechar();
                return motivo;
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
        } // fim de MotivoReimpressao

        public DataTable FormasPagamento(int vendaBilheteriaID)
        {
            try
            {
                DataTable tabela = new DataTable("FormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("FormaPagamento", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Porcentagem", typeof(decimal));

                string sql = "SELECT tFormaPagamento.ID AS FormaPagamentoID, tFormaPagamento.Nome AS FormaPagamento, tVendaBilheteriaFormaPagamento.Valor, tVendaBilheteriaFormaPagamento.Porcentagem " +
                    "FROM tVendaBilheteria " +
                    "INNER JOIN tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID " +
                    "INNER JOIN tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE tVendaBilheteriaFormaPagamento.VendaBilheteriaID=" + vendaBilheteriaID;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("FormaPagamentoID");
                    linha["FormaPagamento"] = bd.LerString("FormaPagamento");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Porcentagem"] = bd.LerDecimal("Porcentagem");
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

        public string FormasPagametoString(int vendaBilheteriaID)
        {
            try
            {
                string retorno = "";
                string sql = @"SELECT tVendaBilheteria.ID,tFormaPagamento.Nome
                FROM tVendaBilheteria 
                INNER JOIN tVendaBilheteriaFormaPagamento ON (tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID)
                INNER JOIN tFormaPagamento ON (FormaPagamentoID=tFormaPagamento.ID)
                WHERE tVendaBilheteria.ID = " + vendaBilheteriaID + @"
                GROUP BY tVendaBilheteria.ID,tFormaPagamento.Nome";

                while (bd.Consulta(sql).Read())
                {
                    if (retorno.Length > 0)
                    {
                        retorno += ", " + bd.LerString("Nome");
                    }
                    else
                    {
                        retorno += bd.LerString("Nome");
                    }

                }


                return retorno;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void TaxaEntregaInfo(int vendaBilheteriaID, out int taxaEntregaID, out decimal taxaEntregaValor, BD bd)
        {
            string sql = @"SELECT taxaEntregaID, taxaEntregaValor FROM tVendaBilheteria WHERE ID = " + vendaBilheteriaID;
            if (bd.Consulta(sql).Read())
            {
                taxaEntregaID = bd.LerInt("taxaEntregaID");
                taxaEntregaValor = bd.LerDecimal("taxaEntregaValor");
                bd.Consulta().Close();
            }
            else
                throw new Exception("VendaBilheteria não encontrado! (TaxaEntregaInfo)");
        }

        public void AtualizarStatusAntiFraude(BD bd, bool aprovacaoAutomatica, int id, StatusAntiFraude status)
        {
            try
            {
                this.Ler(id);
                this.Status.Valor = Convert.ToString((char)status);
                this.AprovacaoAutomatica.Valor = aprovacaoAutomatica;
                this.Atualizar(bd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AtualizarStatusAntiFraude(int id, StatusAntiFraude status)
        {
            AtualizarStatusAntiFraude(id, status, 0, IRLib.Paralela.HammerHead.Enumeradores.RetornoAccertify.Indefinido);
        }

        public void AtualizarStatusAntiFraude(int id, StatusAntiFraude status, int score, HammerHead.Enumeradores.RetornoAccertify retornoAccertify)
        {
            try
            {
                Cartao oCartao = new Cartao();

                char statusCliente = ' ';
                this.Ler(id);
                this.Status.Valor = Convert.ToString((char)status);

                Cliente cliente = new Cliente(this.Control.UsuarioID);
                cliente.Ler(this.ClienteID.Valor);

                if (retornoAccertify != HammerHead.Enumeradores.RetornoAccertify.Indefinido)
                    this.RetornoAccertify.Valor = retornoAccertify.ToString();

                this.Score.Valor = score;

                switch (status)
                {
                    case StatusAntiFraude.Fraude:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeSim;
                        this.NivelRisco.Valor = (int)enumNivelRisco.Fraude;
                        this.MotivoID.ValorBD = DBNull.Value.ToString();
                        statusCliente = (char)Cliente.StatusClienteChar.Bloqueado;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Bloqueado, bd);
                        break;
                    case StatusAntiFraude.Cancelado:
                    case StatusAntiFraude.Aprovado:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeNao;
                        this.NivelRisco.Valor = (int)enumNivelRisco.SemRisco;
                        this.MotivoID.ValorBD = DBNull.Value.ToString();
                        statusCliente = (char)Cliente.StatusClienteChar.Liberado;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Liberado, bd);
                        break;
                    case StatusAntiFraude.Aguardando:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeNao;
                        this.NivelRisco.Valor = (int)enumNivelRisco.CompraDeRisco;
                        this.MotivoID.ValorBD = DBNull.Value.ToString();
                        statusCliente = (char)Cliente.StatusClienteChar.Liberado;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Liberado, bd);
                        break;
                    case StatusAntiFraude.EmAnalise:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeNao;
                        this.NivelRisco.Valor = (int)enumNivelRisco.CompraDeRisco;
                        statusCliente = (char)Cliente.StatusClienteChar.Liberado;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Liberado, bd);
                        break;
                }
                this.AtualizarSemLog(bd);

                cliente.StatusAtual.Valor = statusCliente.ToString();
                cliente.Atualizar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool AtualizarSemLog(BD bd)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteria SET Status = '@005', NivelRisco = @025, Fraude = @027, MotivoID = @031, Score = @042, RetornoAccertify = '@043' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@025", this.NivelRisco.ValorBD);
                sql.Replace("@027", this.Fraude.ValorBD);
                sql.Replace("@031", this.MotivoID.ValorBD);
                sql.Replace("@042", this.Score.ValorBD);
                sql.Replace("@043", this.RetornoAccertify.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AtualizarStatusAntiFraude(int id, StatusAntiFraude status, int MotivoID, string Obs)
        {

            try
            {
                IRLib.Paralela.Cartao oCartao = new Cartao();

                bd.IniciarTransacao();

                char statusCliente = ' ';
                this.Ler(id);
                this.Status.Valor = Convert.ToString((char)status);

                Cliente cliente = new Cliente(this.Control.UsuarioID);
                cliente.Ler(this.ClienteID.Valor);

                switch (status)
                {
                    case StatusAntiFraude.Fraude:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeSim;
                        this.NivelRisco.Valor = (int)enumNivelRisco.Fraude;
                        statusCliente = (char)Cliente.StatusClienteChar.Bloqueado;
                        this.MotivoID.Valor = MotivoID;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Bloqueado, bd);
                        break;
                    case StatusAntiFraude.Aprovado:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeNao;
                        this.NivelRisco.Valor = (int)enumNivelRisco.CompraDeRisco;
                        statusCliente = (char)Cliente.StatusClienteChar.Liberado;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Liberado, bd);

                        this.MotivoID.Valor = MotivoID;
                        break;
                    case StatusAntiFraude.Aguardando:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeNao;
                        this.NivelRisco.Valor = (int)enumNivelRisco.CompraDeRisco;
                        statusCliente = (char)Cliente.StatusClienteChar.Liberado;
                        this.MotivoID.Valor = MotivoID;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Liberado, bd);
                        break;
                    case StatusAntiFraude.EmAnalise:
                        this.Fraude.Valor = (int)enumIsFraude.FraudeNao;
                        this.NivelRisco.Valor = (int)enumNivelRisco.CompraDeRisco;
                        this.MotivoID.Valor = MotivoID;
                        statusCliente = (char)Cliente.StatusClienteChar.Liberado;
                        oCartao.AtualizarPorVendabilheteriaID(id, Cartao.enumStatusCartao.Liberado, bd);
                        break;
                }

                SalvaHistoricoDeVenda(id, this.Control.UsuarioID, statusCliente, MotivoID, Obs, bd);

                this.Atualizar(bd);

                cliente.StatusAtual.Valor = statusCliente.ToString();
                cliente.Atualizar();

                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        protected void InserirControle(BD bd, string acao)
        {

            try
            {
                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cVendaBilheteria (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xVendaBilheteria (ID, Versao, CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaEntregaValor, TaxaConvenienciaValorTotal, ComissaoValorTotal, ValorTotal, Obs, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, DDD, NumeroCelular, ModelIDCelular, FabricanteCelular, NivelRisco, IP, Fraude) ");
                sql.Append("SELECT ID, @V, CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaEntregaValor, TaxaConvenienciaValorTotal, ComissaoValorTotal, ValorTotal, Obs, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, DDD, NumeroCelular, ModelIDCelular, FabricanteCelular, NivelRisco, IP, Fraude FROM tVendaBilheteria WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool TrocarCliente(int novoClienteID)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cVendaBilheteria WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteria SET  ClienteID = @001");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", novoClienteID.ToString());
                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EstruturaTransacoesDetalhes getTransacoesDetalhes()
        {
            try
            {
                EstruturaTransacoesDetalhes oDetalhes = new EstruturaTransacoesDetalhes();
                string status = string.Empty;

                bd.Consulta("EXEC sp_getDetalhesTransacao " + this.Control.ID);
                if (bd.Consulta().Read())
                {
                    EstruturaTransacoesCliente oCliente = new EstruturaTransacoesCliente();
                    oDetalhes.Senha = bd.LerString("Senha");
                    oCliente.ID = bd.LerInt("ClienteID");
                    oCliente.Nome = bd.LerString("Nome");
                    oCliente.Telefone = bd.LerString("Telefone");
                    oCliente.Celular = bd.LerString("Celular");
                    oCliente.CPF = bd.LerString("CPF");
                    oCliente.RG = bd.LerString("RG");
                    oCliente.IP = bd.LerString("IP");
                    oCliente.Endereco = bd.LerString("Endereco");
                    oCliente.Email = bd.LerString("Email");
                    bool cancelada = bd.LerBoolean("VendaCancelada");
                    status = cancelada ? "C" : bd.LerString("Status");
                    switch (status)
                    {
                        case "A":
                            oDetalhes.Status = StatusAntiFraude.Aguardando;
                            break;
                        case "F":
                            oDetalhes.Status = StatusAntiFraude.Fraude;
                            break;
                        case "P":
                            oDetalhes.Status = StatusAntiFraude.Aprovado;
                            break;
                        case "C":
                            oDetalhes.Status = StatusAntiFraude.Cancelado;
                            break;
                        case "N":
                            oDetalhes.Status = StatusAntiFraude.EmAnalise;
                            break;
                    }
                    oDetalhes.NivelRisco = bd.LerInt("NivelRisco");
                    oDetalhes.TaxaEntrega = bd.LerString("TaxaEntrega");
                    oDetalhes.TaxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");
                    oDetalhes.DataVenda = bd.LerDateTime("DataVenda");
                    oDetalhes.ValorTotal = bd.LerDecimal("ValorTotal");
                    oDetalhes.Cliente = oCliente;
                    oDetalhes.Canal = bd.LerString("Canal");
                    oDetalhes.MotivoID = bd.LerInt("MotivoID");
                }

                bd.Consulta().NextResult();
                List<EstruturaFormaPagamento> lstFormaPagamento = new List<EstruturaFormaPagamento>();
                int FormaPagamentoIDValeIngresso = new IRLib.Paralela.ConfigGerenciadorParalela().getFormaPagamentoIDValeIngresso();
                int FormaPagamentoID = 0;
                while (bd.Consulta().Read())
                {
                    FormaPagamentoID = bd.LerInt("FormaPagamentoID");
                    lstFormaPagamento.Add(new EstruturaFormaPagamento
                    {

                        FormaPagamentoID = FormaPagamentoID,
                        FormaPagamento = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("Valor"),
                        CartaoID = bd.LerInt("CartaoID"),
                        Cartao = bd.LerString("NroCartao"),
                        ComprasCartaoVisible = bd.LerInt("CartaoID") > 0 ? true : false
                    });

                    if (FormaPagamentoID != FormaPagamentoIDValeIngresso) //VIR
                    {
                        oDetalhes.NSU = bd.LerString("NSU");
                        oDetalhes.NumeroAutorizacao = bd.LerString("NumeroAutorizacao");
                    }
                }
                oDetalhes.DadosCompra = lstFormaPagamento;


                oDetalhes.MotivoRisco = "";
                bd.Consulta().NextResult();
                while (bd.Consulta().Read())
                {
                    if (oDetalhes.MotivoRisco.Length > 0)
                    {
                        oDetalhes.MotivoRisco += ", " + bd.LerString("Motivo");
                    }
                    else
                    {
                        oDetalhes.MotivoRisco = bd.LerString("Motivo");
                    }
                }

                if (oDetalhes.MotivoRisco.Length <= 0)
                {
                    oDetalhes.MotivoRisco = "Não Informado";
                }

                return oDetalhes;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<EstruturaTransacoesIngressosComprados> getTrasacoesDetalhesIngressos()
        {
            try
            {
                string IngressoStatus = string.Empty;
                List<EstruturaTransacoesIngressosComprados> lstIngressos = new List<EstruturaTransacoesIngressosComprados>();
                bd.Consulta("EXEC sp_getDetalhesTransacaoIngressos " + this.Control.ID);
                while (bd.Consulta().Read())
                {
                    IngressoStatus = Ingresso.StatusDetalhado(bd.LerString("Status"), bd.LerInt("PrazoEntrega"));
                    lstIngressos.Add(new EstruturaTransacoesIngressosComprados
                    {
                        ID = bd.LerInt("ID"),
                        Local = bd.LerString("Local"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = bd.LerDateTime("Horario"),
                        Setor = bd.LerString("Setor"),
                        PrecoNome = bd.LerString("PrecoNome"),
                        Valor = bd.LerDecimal("Valor"),
                        Codigo = bd.LerString("Codigo"),
                        Status = IngressoStatus,
                    });
                }
                return lstIngressos;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool AtivaAntiFraude(int CanalID, IRLib.Paralela.Loja.TEFTipos tipoLoja)
        {
            IRLib.Paralela.ConfigGerenciadorParalela config = new ConfigGerenciadorParalela();

            bool retorno = false;
            string CanaisID = config.getAntiFraudeCanais();

            string[] strings = CanaisID.Split(',');
            int[] Canais = new int[strings.Length];

            for (int i = 0; i < strings.Length; i++)
            {
                Canais[i] = int.Parse(strings[i]);
                if (CanalID == Canais[i] && tipoLoja == Loja.TEFTipos.Digitado)
                    retorno = true;
            }
            return retorno;
        }

        public List<int> VerificarComprasMesmaApresentacao(DataTable dttBulk, int clienteID)
        {
            try
            {
                List<int> ids = new List<int>();
                bd.BulkInsert(dttBulk, "#ApresentacoesTemp", false, true);
                bd.Consulta("EXEC sp_getApresentacoesDistintasJaCompradas " + clienteID);

                while (bd.Consulta().Read())
                    ids.Add(bd.LerInt("ApresentacaoID"));

                return ids;
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

        public void AtualizaVendaCancelada(string senha)
        {

            VendaBilheteria vb = new VendaBilheteria();
            vb.Senha.Valor = senha;
            vb.AtribuirIDPelaSenha();
            vb.Ler(vb.Control.ID);
            if (!vb.VendaCancelada.Valor)
            {
                vb.VendaCancelada.Valor = true;
                vb.Atualizar();
            }

        }

        public void SalvaHistoricoDeVenda(int vendaBilheteriaID, int UsuarioID, char StatusNovo, int MotivoID, string Obs, BD bd)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                StringBuilder sql = new StringBuilder();

                string StatusAnterior = string.Empty;

                sql.Append("SELECT Status FROM tVendaBilheteria (NOLOCK) WHERE ID = " + vendaBilheteriaID);

                if (bd.Consulta(sql).Read())
                {
                    StatusAnterior = bd.LerString("Status");
                    bd.Consulta().Close();
                }

                sql = new StringBuilder();

                sql.Append("INSERT INTO tHistoricoMudancaAcao (TimeStamp, UsuarioID, StatusAnterior, StatusNovo, MotivoID, Obs , vendaBilheteriaID) VALUES ( ");
                sql.Append(timeStamp + "," + UsuarioID + ",'" + StatusAnterior + "','" + StatusNovo + "'," + MotivoID + ",'" + Obs + "'," + vendaBilheteriaID + ")");

                bd.Executar(sql.ToString());

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

        public List<int> ProcurarVendasPorArvore(int eventoid, string apresentacao, string setor, string preco, ref int precoid, ref decimal valor)
        {
            BD bd = new BD();
            try
            {



                bd.Consulta(
                        string.Format(@"SELECT 
                                    TOP 1 p.ID, p.Valor
                                FROM tEvento e (NOLOCK)
                                INNER JOIN tApresentacao ap (NOLOCK) ON ap.EventoID = e.ID
                                INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ap.ID
                                INNER JOIN tSetor s (NOLOCK) ON s.ID = aps.SetorID
                                INNER JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = aps.ID
                                    WHERE
                                        e.ID = {0} AND
                                        Horario = '{1}' AND 
                                        s.Nome LIKE '%{2}%' AND 
                                        p.Nome LIKE  '%{3}%'", eventoid, apresentacao, setor, preco));

                if (bd.Consulta().Read())
                {
                    precoid = bd.LerInt("ID");
                    valor = bd.LerDecimal("Valor");
                }

                bd.FecharConsulta();

                List<int> vendas = new List<int>();

                bd.Consulta(@"SELECT 
                                DISTINCT(vb.ID) AS ID FROM
	                        tVendaBilheteria vb (NOLOCK)
	                        INNER JOIN tIngressoLog log (NOLOCK) ON log.VendaBilheteriaID = vb.ID
	                            WHERE 
		                        log.PrecoID = " + precoid);

                while (bd.Consulta().Read())
                    vendas.Add(bd.LerInt("ID"));

                return vendas;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void MudarPrecosPorVenda(int precoID, decimal valor, List<int> vendasID)
        {
            BD bd = new BD();
            try
            {
                var oFormaPagamento = new { ID = 0, Porcentagem = Convert.ToDecimal(0) };
                var ListaFormaPagamento = VendaBilheteria.ToAnonymousList(oFormaPagamento);

                bd.IniciarTransacao();

                bd.Executar("UPDATE tPreco SET Valor = '" + valor.ToString().Replace(",", ".") + "' WHERE ID = " + precoID);

                decimal ValorTotal = 0;

                foreach (int VendaBilheteriaID in vendasID)
                {
                    ListaFormaPagamento.Clear();

                    ValorTotal =
                        Convert.ToDecimal(
                            bd.ConsultaValor(
                                string.Format(
                                    @"SELECT 
	                                    SUM(p.Valor) + vb.TaxaEntregaValor + TaxaConvenienciaValorTotal AS ValorTotal
	                                FROM tVendaBilheteria vb
	                                INNER JOIN tIngressoLog log ON log.VendaBilheteriaID = vb.ID
	                                INNER JOIN tPreco p ON p.ID = log.PrecoID
	                                    WHERE 
		                                vb.ID = {0} AND Acao IN ('V', 'C')
	                                GROUP BY 
                                        vb.TaxaEntregaValor, vb.TaxaConvenienciaValorTotal", VendaBilheteriaID)));

                    bd.FecharConsulta();

                    bd.Consulta(
                        @"SELECT ID, Porcentagem FROM tVendaBilheteriaFormaPagamento WHERE VendaBilheteriaID = " + VendaBilheteriaID);

                    while (bd.Consulta().Read())
                        ListaFormaPagamento.Add(new
                        {
                            ID = bd.LerInt("ID"),
                            Porcentagem = bd.LerDecimal("Porcentagem"),
                        });

                    bd.FecharConsulta();

                    bd.Consulta().Close();

                    bd.Executar("UPDATE tVendaBilheteria SET ValorTotal = '" + ValorTotal.ToString().Replace(",", ".") + "' WHERE ID = " + VendaBilheteriaID);

                    foreach (var formaPagamento in ListaFormaPagamento)
                    {
                        bd.Executar(
                            string.Format(@"UPDATE tVendaBilheteriaFormaPagamento
                                SET Valor = '{0}' WHERE ID = {1}",
                                Convert.ToDecimal((ValorTotal * formaPagamento.Porcentagem) / 100).ToString().Replace(",", "."), formaPagamento.ID));
                    }
                }
                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaTransacoesDetalhes AcompanhamentoIngressos(int vendaBilheteriaID)
        {
            try
            {
                EstruturaTransacoesDetalhes venda = new EstruturaTransacoesDetalhes();

                bd.Consulta("EXEC proc_AcompanhamentoVenda " + vendaBilheteriaID);

                if (bd.Consulta().Read())
                {
                    venda.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    venda.Senha = bd.LerString("Senha");
                    venda.DataVenda = bd.LerDateTime("DataVenda");
                    venda.Atendente = bd.LerString("Atendente");
                    venda.CanalID = bd.LerInt("CanalID");
                    venda.Canal = bd.LerString("Canal");
                    venda.Cliente = new EstruturaTransacoesCliente()
                    {
                        Nome = bd.LerString("Cliente"),
                        Email = bd.LerString("ClienteEmail")
                    };
                    venda.EntregaControleID = bd.LerInt("EntregaControleID");
                    venda.TaxaEntregaID = bd.LerInt("TaxaEntregaID");
                    venda.TaxaEntrega = bd.LerString("TaxaEntrega");
                    venda.TaxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");
                    venda.TipoTaxa = bd.LerString("TipoEntrega");
                    venda.PermitirImpressaoInternet = bd.LerBoolean("PermitirImpressaoInternet");

                }

                bd.Consulta().NextResult();

                venda.DadosCompra = new List<EstruturaFormaPagamento>();

                while (bd.Consulta().Read())
                {
                    venda.DadosCompra.Add(new EstruturaFormaPagamento()
                    {
                        FormaPagamento = bd.LerString("Nome")
                    });
                }

                bd.Consulta().NextResult();

                venda.ListaIngressos = new List<EstruturaTransacoesIngressosComprados>();

                while (bd.Consulta().Read())
                {
                    venda.ListaIngressos.Add(new EstruturaTransacoesIngressosComprados()
                    {
                        Local = bd.LerString("Local"),
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = bd.LerDateTime("Horario"),
                        Setor = bd.LerString("Setor"),
                        PrecoNome = bd.LerString("Preco"),
                        Valor = bd.LerDecimal("PrecoValor"),
                        TaxaConveniencia = bd.LerDecimal("TaxaConveniencia"),
                        Codigo = bd.LerString("Codigo"),
                        TipoLugar = bd.LerString("LugarMarcado"),
                        LugarID = bd.LerInt("LugarID"),
                    });
                }

                return venda;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private string GetProcedimento(int vendaBilheteriaID)
        {
            try
            {

                List<EstruturaEntregaAgenda> listaEntrega = new List<EstruturaEntregaAgenda>();
                string sql = @"select distinct CASE WHEN
		                                LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
			                                THEN tEventoEntregaControle.ProcedimentoEntrega
		                                ELSE
			                                CASE WHEN
				                                LEN(tEntregaControle.ProcedimentoEntrega) > 0
				                                THEN tEntregaControle.ProcedimentoEntrega
				                                ELSE tEntrega.ProcedimentoEntrega
			                                END
                                END as ProcedimentoEntrega,
                                CASE WHEN
		                                LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
			                                THEN 3
		                                ELSE
			                                CASE WHEN
				                                LEN(tEntregaControle.ProcedimentoEntrega) > 0
				                                THEN 2
				                                ELSE 1
			                                END
                                END as PrioridadeProcedimento
                                FROM tEventoEntregaControle (NOLOCK)
                                INNER JOIN tEntregaControle(NOLOCK) on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                                INNER JOIN tEntrega (NOLOCK) on tEntregaControle.EntregaID = tEntrega.ID
                                INNER JOIN tVendaBilheteria (NOLOCK) on tVendaBilheteria.EntregaControleID = tEntregaControle.ID
                                WHERE tVendaBilheteria.ID =" + vendaBilheteriaID;


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    listaEntrega.Add(new EstruturaEntregaAgenda
                    {

                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega").Replace("<br/>", "\n\r").Replace("<br />", "\n\r"),
                        PrioridadeProcedimento = bd.LerInt("PrioridadeProcedimento")
                    });
                }

                string procedimento = "Procedimento de entrega :</b> ";
                List<int> procedimentos = new List<int>();
                foreach (EstruturaEntregaAgenda item in listaEntrega)
                {
                    switch (item.PrioridadeProcedimento)
                    {
                        case 3:
                            procedimentos.Add(3);
                            procedimento += "<br /><br />" + item.ProcedimentoEntrega;
                            break;
                        case 2:
                            if (!procedimentos.Contains(2))
                            {
                                procedimentos.Add(2);
                                procedimento += "<br /><br />" + item.ProcedimentoEntrega;
                            }
                            break;
                        case 1:
                            if (!procedimentos.Contains(1))
                            {
                                procedimentos.Add(1);
                                procedimento += "<br /><br />" + item.ProcedimentoEntrega;
                            }
                            break;
                        default:
                            break;
                    }
                }


                return procedimento;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<T> ToAnonymousList<T>(T ItemType)
        {
            return new List<T>();
        }

        public static bool DeveImprimir(string status, bool imprimir)
        {
            if (!imprimir)
                throw new Exception("Esta venda não pode ser impressa.");

            switch (status)
            {
                case EMANALISE:
                case AGUARDANDO_APROVACAO:
                    return true;
                case FRAUDE:
                    throw new Exception("A senha está marcada como fraude.\nNão será possível efetuar a impressão");
                default:
                    return false;
            }
        }

        public List<HammerHead.EstruturaVenda> CarregarVendasParaPagamento()
        {
            try
            {
                List<HammerHead.EstruturaVenda> vendas = new List<HammerHead.EstruturaVenda>();

                string sql = "EXEC GetDataHammerHead";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    vendas.Add(new HammerHead.EstruturaVenda()
                    {
                        ID = bd.LerInt("VendaBilheteriaID"),
                        Senha = bd.LerString("Senha"),
                        ValorTotal = bd.LerDecimal("ValorTotal"), //Precisa ignorar o valor total, não pode cobrar o VIR!
                        DataVenda = bd.LerDateTime("DataVenda"),
                        IP = bd.LerString("IP"),
                        NumeroCelular = bd.LerString("Celular"),
                        Score = bd.LerInt("Score"),
                        AccertifyForceStatus = bd.LerString("AccertifyForceStatus"),

                        Cliente = new HammerHead.EstruturaCliente()
                        {
                            ID = bd.LerInt("ClienteID"),
                            Nome = bd.LerString("ClienteNome"),
                            Email = bd.LerString("ClienteEmail"),
                        },
                        FormaPagamento = new HammerHead.EstruturaFormaPagamento()
                        {
                            NomeCartao = bd.LerString("NomeCartao"),
                            VendaBilheteriaFormaPagamentoID = bd.LerInt("VendaBilheteriaFormaPagamentoID"),
                            NotaFiscal = bd.LerString("Cupom"),
                            ID = bd.LerInt("FormaPagamentoID"),
                            BandeiraID = bd.LerInt("BandeiraID"),
                            Parcelas = bd.LerInt("Parcelas"),
                            CodigoResposta = bd.LerString("CodigoRespostaVenda"),
                            Valor = bd.LerDecimal("Valor"),
                            Bandeira = bd.LerString("Bandeira"),
                        },
                        Cartao = new HammerHead.EstruturaCartao()
                        {
                            NumeroCartao = bd.LerString("NumeroCartao"),
                            DataVencimento = bd.LerString("DataVencimento"),
                            CodigoSeguranca = bd.LerString("CodigoSeguranca"),
                        }
                    });
                }
                return vendas;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public HammerHead.EstruturaVenda CarregarVendaParaPagamento(int vendaBilheteriaID)
        {

            try
            {
                bd.Consulta(string.Format(@"SELECT
	                    TOP 1
	                        vb.ID AS VendaBilheteriaID, vb.Senha, vb.DataVenda, vb.ValorTotal,
	                        c.ID AS ClienteID, c.Nome AS ClienteNome, c.Email AS ClienteEmail,
	                        vbfp.FormaPagamentoID, fp.BandeiraID, fp.Parcelas,
                            ct.CartaoCr AS NumeroCartao, ct.DataCr AS DataVencimento, ct.CVVCr AS CodigoSeguranca,
                            vbfp.ID AS VendaBilheteriaFormaPagamentoID, vbfp.Cupom, vbfp.CodigoRespostaVenda, vbfp.Valor, vb.IP, vb.NomeCartao, vb.DDD + vb.NumeroCelular AS Celular, b.Nome AS Bandeira,
                            vb.Score, VendaCancelada
	                    FROM tVendaBilheteria vb (NOLOCK)
	                    INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
	                    INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID
	                    INNER JOIN tFormaPagamento fp (NOLOCK) ON vbfp.FormaPagamentoID = fp.ID
                        INNER JOIN tBandeira b (NOLOCK) ON fp.BandeiraID = b.ID
                        LEFT JOIN tCartao ct (NOLOCK) ON ct.ID = vbfp.CartaoID
	                    WHERE 
                            vb.ID = {0} 
	                    ORDER BY DataVenda ASC", vendaBilheteriaID));

                //Já houve processamento
                if (!bd.Consulta().Read() || bd.LerBoolean("VendaCancelada"))
                    throw new VendaCanceladaException(vendaBilheteriaID);

                return new HammerHead.EstruturaVenda()
                {
                    ID = bd.LerInt("VendaBilheteriaID"),
                    Senha = bd.LerString("Senha"),
                    ValorTotal = bd.LerDecimal("ValorTotal"), //Precisa ignorar o valor total, não pode cobrar o VIR!
                    DataVenda = bd.LerDateTime("DataVenda"),
                    IP = bd.LerString("IP"),
                    NumeroCelular = bd.LerString("Celular"),
                    Score = bd.LerInt("Score"),
                    Cliente = new HammerHead.EstruturaCliente()
                    {
                        ID = bd.LerInt("ClienteID"),
                        Nome = bd.LerString("ClienteNome"),
                        Email = bd.LerString("ClienteEmail"),
                    },
                    FormaPagamento = new HammerHead.EstruturaFormaPagamento()
                    {
                        NomeCartao = bd.LerString("NomeCartao"),
                        VendaBilheteriaFormaPagamentoID = bd.LerInt("VendaBilheteriaFormaPagamentoID"),
                        NotaFiscal = bd.LerString("Cupom"),
                        ID = bd.LerInt("FormaPagamentoID"),
                        BandeiraID = bd.LerInt("BandeiraID"),
                        Parcelas = bd.LerInt("Parcelas"),
                        CodigoResposta = bd.LerString("CodigoRespostaVenda"),
                        Valor = bd.LerDecimal("Valor"),
                        Bandeira = bd.LerString("Bandeira"),
                    },
                    Cartao = new HammerHead.EstruturaCartao()
                    {
                        NumeroCartao = bd.LerString("NumeroCartao"),
                        DataVencimento = bd.LerString("DataVencimento"),
                        CodigoSeguranca = bd.LerString("CodigoSeguranca"),
                    }
                };
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void CancelarVenda(HammerHead.EstruturaVenda venda)
        {
            try
            {
                BilheteriaParalela oBilheteria = new BilheteriaParalela();
                ApoliceMondial oApolice = new ApoliceMondial();

                int caixaID = oBilheteria.VerificaCaixaInternet();
                string ApoliceMondial = string.Empty;

                CancelamentoGerenciador cancelamentoGerenciador = new CancelamentoGerenciador();
                cancelamentoGerenciador.UsuarioID = Usuario.INTERNET_USUARIO_ID;
                cancelamentoGerenciador.CaixaID = caixaID;
                cancelamentoGerenciador.CanalID = Canal.CANAL_INTERNET;
                cancelamentoGerenciador.PerfilID = Perfil.SAC_OPERADOR;
                DataSet ds = cancelamentoGerenciador.PesquisarVenda(venda.ID);

                DataTable itensReservados = ds.Tables[CancelamentoGerenciador.TABELA_GRID];
                DataTable tReserva = ds.Tables[CancelamentoGerenciador.TABELA_RESERVA];
                DataTable tInfoVenda = ds.Tables[CancelamentoGerenciador.INFO_VENDA];

                int entregaControleID = Convert.ToInt32(tInfoVenda.Rows[0][CancelamentoGerenciador.ENTREGA_CONTROLE_ID]);
                int entregaAgendaID = Convert.ToInt32(tInfoVenda.Rows[0][CancelamentoGerenciador.ENTREGA_AGENDA_ID]);
                decimal valorEntrega = Convert.ToDecimal(tInfoVenda.Rows[0][CancelamentoGerenciador.TAXA_ENTREGA_VALOR]);
                decimal valorTotalConveniencia = Convert.ToDecimal(tInfoVenda.Rows[0][CancelamentoGerenciador.TAXA_CONV_VALOR_TOTAL]);
                decimal valorTotal = Convert.ToDecimal(tInfoVenda.Rows[0][CancelamentoGerenciador.VALOR_TOTAL]);
                int motivoIDCartaoInvalido = Convert.ToInt32(ConfigurationManager.AppSettings["MotivoIDCartaoInvalido"]);
                //string taxaEntrega = tInfoVenda.Rows[0]["TaxaEntrega"].ToString();

                DataTable dtPagamento = cancelamentoGerenciador.FormasPagamento(Canal.CANAL_INTERNET, venda.ID, Empresa.INTERNET_EMPRESA_ID);
                dtPagamento.Columns.Add("Valor", typeof(decimal)).DefaultValue = 0;

                DataTable tPagamento = dtPagamento.Clone();
                foreach (DataRow dtr in dtPagamento.Select("ID = " + venda.FormaPagamento.ID))
                {
                    dtr["Valor"] = valorTotal;
                    tPagamento.ImportRow(dtr);
                }

                decimal valorSeguro = Convert.ToInt32(tInfoVenda.Rows[0][CancelamentoGerenciador.VALORSEGURO]);

                if (valorSeguro > 0)
                    ApoliceMondial = oApolice.BuscaApolice(venda.ID);

                string senhaTmp = oBilheteria.Cancelar(itensReservados, tReserva, tPagamento, caixaID, Loja.INTERNET_LOJA_ID, Canal.CANAL_INTERNET, Empresa.INTERNET_EMPRESA_ID,
                                    venda.Cliente.ID, entregaControleID, valorEntrega, valorTotal, Usuario.INTERNET_USUARIO_ID,
                                    string.Empty, string.Empty, true,
                                    entregaControleID > 0, "",
                                    false, venda.ID, motivoIDCartaoInvalido, Usuario.INTERNET_USUARIO_ID, entregaAgendaID,
                                    venda.TaxaProcessamento > 0, venda.TaxaProcessamento, false, ApoliceMondial, false, false, string.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AtualizarPagamentoProcessado(BD bd, int id, string cupomFiscal)
        {
            bd.Executar(
                string.Format(@"UPDATE tVendaBilheteria SET PagamentoProcessado = 'T', 
                    NotaFiscalCliente = '{0}', NotaFiscalEstabelecimento = '{0}' WHERE ID = {1}",
                    cupomFiscal.Replace("'", string.Empty).Replace("--", string.Empty), id));
        }


        public void AtualizarScore(BD bd, int id, int Score)
        {
            AtualizarScore(bd, id, 0, string.Empty);
        }

        public void AtualizarScore(BD bd, int id, int score, string retornoAccertify)
        {
            bd.Executar(string.Format(@"UPDATE tVendaBilheteria SET Score = {0}, RetornoAccertify = '{1}' WHERE ID = {2}", score, retornoAccertify, id));
        }

        public List<EstruturaRetornoIngresso> RetornoIngressosCamping(int VendaBilheteriaID)
        {
            try
            {
                List<EstruturaRetornoIngresso> vendas = new List<EstruturaRetornoIngresso>();
                string sql = string.Empty;

                sql = @"SELECT tIngresso.ID AS IngressoID, tSetor.Nome AS Lote, tPreco.Nome AS Preco, ISNULL(COUNT(tCampingSwu.ID), 0) AS Quantidade 
                FROM tVendaBilheteria (NOLOCK)
                INNER JOIN tIngresso (NOLOCK) ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID
                INNER JOIN tSetor (NOLOCK) ON tIngresso.SetorID = tSetor.ID
                INNER JOIN tPreco (NOLOCK) ON tIngresso.PrecoID = tPreco.ID
                INNER JOIN tQuantidadeCamping (NOLOCK) ON tQuantidadeCamping.PrecoID = tPreco.ID
                LEFT JOIN tCampingSwu (NOLOCK) ON tCampingSwu.IngressoID = tIngresso.ID
                WHERE tVendaBilheteria.ID = " + VendaBilheteriaID + @" GROUP BY tIngresso.ID, tSetor.Nome, tPreco.Nome, tQuantidadeCamping.CampingQuantidade";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    vendas.Add(new EstruturaRetornoIngresso()
                    {
                        IngressoID = bd.LerInt("IngressoID"),
                        Lote = bd.LerString("Lote"),
                        Preco = bd.LerString("Preco"),
                        QuantidadeIntegrantes = bd.LerInt("Quantidade")
                    });
                }

                return vendas;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string tipoEntrega(object vendaBilheteriaID)
        {
            string retorno = "";

            string sql = string.Empty;

            sql = @"SELECT  tEntrega.tipo as TipoEntrega
                    FROM tVendabilheteria (NOLOCK)
                    INNER JOIN tEntregacontrole (NOLOCK) ON tVendabilheteria.EntregaControleID  = tEntregacontrole.ID
                    INNER JOIN tEntrega (NOLOCK) ON tEntregacontrole.EntregaID  = tEntrega.ID
                    WHERE tVendabilheteria.ID = " + vendaBilheteriaID;

            bd.Consulta(sql);

            if (!bd.Consulta().Read())
                throw new Exception("Nenhum tipo de entrega");

            retorno = bd.LerString("TipoEntrega");

            return retorno;
        }

        public EstruturaGoogleAnalytics RetornaEstruturaGoogle(string SenhaVendaBilheteria, bool ValeIngresso)
        {
            try
            {
                EstruturaGoogleAnalytics retorno = new EstruturaGoogleAnalytics();

                string sql = string.Empty;

                sql = @"SELECT DISTINCT tVendaBilheteria.ID as VendaBilheteriaID, tVendaBilheteria.ValorTotal, tVendaBilheteria.TaxaEntregaValor, 
                tVendaBilheteria.TaxaConvenienciaValorTotal, 
                 CASE WHEN LEN(tCliente.EstadoCliente) > 0
                    THEN tCliente.EstadoCliente COLLATE Latin1_General_CI_AI 
                    ELSE tCliente.Estado COLLATE Latin1_General_CI_AI 
	                END AS Estado
                , CASE WHEN LEN(tCliente.CidadeCliente) > 0
                    THEN tCliente.CidadeCliente COLLATE Latin1_General_CI_AI 
                    ELSE tCliente.Cidade COLLATE Latin1_General_CI_AI 
	                END AS Cidade
                , tCliente.Pais
                FROM tVendaBilheteria (NOLOCK) 
                INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID
                WHERE tVendaBilheteria.Senha = '" + SenhaVendaBilheteria + "'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    decimal valorTotal = bd.LerDecimal("ValorTotal");
                    retorno.ValorTotal = valorTotal.ToString("c");
                    decimal valorEntrega = bd.LerDecimal("TaxaEntregaValor");
                    retorno.ValorEntrega = valorEntrega.ToString("c");
                    decimal valorTaxa = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    retorno.ValorTaxa = valorTaxa.ToString("c");
                    retorno.Estado = bd.LerString("Estado");
                    retorno.Cidade = bd.LerString("Cidade");
                    retorno.Pais = bd.LerString("Pais");

                }

                if (!ValeIngresso)
                {
                    sql = @"SELECT DISTINCT tIngresso.ID, tEvento.ID AS EventoID, tEvento.Nome AS EventoNome ,
                    CASE WHEN tIngresso.PacoteID > 0
                        THEN 'Pacote'
                        ELSE 'Ingresso'
                    END AS IngressoTipo, tPreco.Valor AS ValorIngresso
                    FROM tVendaBilheteria (NOLOCK) 
                    LEFT JOIN tIngresso (NOLOCK) ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID
                    LEFT JOIN tIngressoLog (NOLOCK) ON tIngressoLog.IngressoID = tIngresso.ID AND tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID AND tIngressolog.Acao = 'V'
                    LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
                    LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                    WHERE Senha = '" + SenhaVendaBilheteria + "'";

                    retorno.ItensIngresso = new List<EstruturaItemIngresso>();

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        decimal valorIngresso = bd.LerDecimal("ValorIngresso");

                        retorno.ItensIngresso.Add(new EstruturaItemIngresso()
                        {
                            IngressoID = bd.LerInt("ID"),
                            EventoID = bd.LerInt("EventoID"),
                            EventoNome = bd.LerString("EventoNome"),
                            IngressoTipo = bd.LerString("IngressoTipo"),
                            ValorIngresso = valorIngresso.ToString("c")
                        });
                    }
                }
                else
                {
                    sql = @"SELECT DISTINCT  tValeIngressoLog.ID, tValeIngressoTipo.ValorPagamento AS ValorValeIngresso
                    FROM tVendaBilheteria (NOLOCK) 
                    INNER JOIN tValeIngresso (NOLOCK) ON tValeIngresso.VendaBilheteriaID = tVendaBilheteria.ID
                    INNER JOIN tValeIngressoLog (NOLOCK) ON tValeIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID
                    INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngressoTipo.ID = tValeIngresso.ValeIngressoTipoID
                    WHERE Senha = '" + SenhaVendaBilheteria + "' AND Acao = 'V'";

                    retorno.ItensVale = new List<EstruturaItemValeIngresso>();

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        decimal valorValeIngresso = bd.LerDecimal("ValorValeIngresso");

                        retorno.ItensVale.Add(new EstruturaItemValeIngresso()
                        {
                            ValeIngressoID = bd.LerInt("ID"),
                            ValeIngressoTipo = "Vale Ingresso",
                            ValorValeIngresso = valorValeIngresso.ToString("c")
                        });
                    }
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        internal int FormasPagamentoID(int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tVendaBilheteriaFormaPagamento.ID 
                                        FROM  tVendaBilheteriaFormaPagamento (NOLOCK)
                                        INNER JOIN tFormaPagamento (nolock) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.id
                                        WHERE VendaBilheteriaID = {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar a forma de pagamento da venda");
                return bd.LerInt("ID");
            }
            finally
            {
                bd.Fechar();
            }
        }

        internal int Parcelas(int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tFormaPagamento.Parcelas 
                        FROM  tVendaBilheteriaFormaPagamento (NOLOCK)
                        INNER JOIN tFormaPagamento (NOLOCK) on tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.id
                        WHERE VendaBilheteriaID = {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar as parcelas da forma de pagamento da venda");
                return bd.LerInt("Parcelas");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LerFormaPagamento(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteria(nolock) WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.ValorTotal.ValorBD = bd.LerDecimal("ValorTotal").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public decimal BoletosPagos(int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"select SUM(ValorPago) as valorPago
                        from  tVendaBilheteriaFormaPagamento (nolock)
                        inner join tFormaPagamento (nolock) on tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.id
                        inner join tVendaBilheteriaFormaPagamentoBoleto (nolock) on tVendaBilheteriaFormaPagamento.ID = tVendaBilheteriaFormaPagamentoBoleto.VendaBilheteriaFormaPagamentoID
                        where VendaBilheteriaID =  {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar as parcelas da forma de pagamento da venda");
                return bd.LerDecimal("valorPago");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizaPagamentoConcluido(int boletoID)
        {
            try
            {
                string sql =
                    string.Format(@"UPDATE vb SET vb.PagamentoProcessado = 'T' 
                                    FROM tVendaBilheteriaFormaPagamentoBoleto vbb(NOLOCK) 
                                    INNER JOIN tVendaBilheteriaFormaPagamento vfp (NOLOCK) 
                                    ON vbb.VendaBilheteriaFormaPagamentoID = vfp.ID
                                    INNER JOIN tVendaBilheteria vb (NOLOCK) 
                                    ON vfp.VendaBilheteriaID = vb.ID
                                    WHERE vbb.ID = {0} ", boletoID);
                bd.Executar(sql);
            }
            catch
            {
                throw new Exception("Erro ao atualizar pagamento da venda.");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizaPagamentoConcluido(BD bd, int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"UPDATE VendaBilheteriaID SET PagamentoProcessado = 'T' 
                                    WHERE ID = {0} ", vendaBilheteriaID);
                bd.Executar(sql);
            }
            catch
            {
                throw new Exception("Erro ao atualizar pagamento da venda.");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizarEntrega(int vendaBilheteriaID, int clienteEnderecoSelecionadoID, EstruturaEntregaAgenda entregaSelecionada, int pdvSelecionadoID, DataTable tGrid, int usuarioID, int empresaID)
        {
            try
            {
                bd.IniciarTransacao();

                int entregaControleID = 0;
                int entregaAgendaID = 0;
                int pdvID = pdvSelecionadoID;
                int clienteEnderecoID = clienteEnderecoSelecionadoID;

                EntregaAgenda oEA = new EntregaAgenda();

                if (oEA.PodeSerAgendado(entregaSelecionada))
                {
                    object verifAgendado = null;

                    verifAgendado = bd.Executar(oEA.String());

                    if (verifAgendado == null)
                        throw new BilheteriaException("Nova entrega não foi gerada. Verificar a Data de Entrega");
                    else
                        if (Convert.ToInt32(verifAgendado) <= 0)
                            throw new BilheteriaException("Nova entrega não foi gerada. Verificar a Data de Entrega");
                    entregaControleID = entregaSelecionada.dataPeriodoSelecionado.EntregaControleID;
                    entregaAgendaID = oEA.Control.ID;
                }
                else
                {
                    switch (entregaSelecionada.Tipo)
                    {
                        case "A":
                            throw new BilheteriaException("Nova entrega não foi gerada.Verificar a Entrega");
                        default:
                            entregaControleID = entregaSelecionada.EntregaControleID;
                            break;
                    }
                }

                this.CancelarImpressao(bd, tGrid, usuarioID, empresaID);

                this.AtualizarEntregaVenda(bd, vendaBilheteriaID, clienteEnderecoID, entregaControleID, entregaAgendaID, pdvID);

                bd.FinalizarTransacao();

            }
            catch (Exception)
            {
                bd.DesfazerTransacao();

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private void CancelarImpressao(BD bd, DataTable tGrid, int usuarioID, int empresaID)
        {
            List<IRLib.Paralela.ClientObjects.IngressoImpressaoCancelar> listaIngressoImpressaoCancelar = new List<IRLib.Paralela.ClientObjects.IngressoImpressaoCancelar>();

            BilheteriaParalela bilheteria = new BilheteriaParalela();
            string motivo = "Nova entrega.";
            DataRow[] tabelaAux = tGrid.Select("Ação='Imprimir' OR Ação='Reimprimir' OR Ação='Cancelar' OR Ação='Anular Impressão'", "IngressoID,IngressoLogId DESC");

            int ingressoIDAux = 0;

            IRLib.Paralela.ClientObjects.IngressoImpressaoCancelar ingressoImpressaoCancelar;

            foreach (DataRow linha in tabelaAux)
            {
                if (ingressoIDAux != Convert.ToInt32(linha["IngressoID"]))
                {
                    if ((string)linha["Ação"] != "Cancelar" && (string)linha["Ação"] != "Anular Impressão")
                    {                        //preenche o objeto com as informações da ultima impressão.
                        ingressoImpressaoCancelar = new IRLib.Paralela.ClientObjects.IngressoImpressaoCancelar();
                        ingressoImpressaoCancelar.IngressoID = Convert.ToInt32(linha["IngressoID"]);
                        ingressoImpressaoCancelar.VendaBilheteriaID = Convert.ToInt32(linha["VendaBilheteriaID"]);
                        ingressoImpressaoCancelar.PrecoID = Convert.ToInt32(linha["PrecoID"]);
                        ingressoImpressaoCancelar.EventoID = Convert.ToInt32(linha["EventoID"]);
                        ingressoImpressaoCancelar.CodigoBarras = Convert.ToString(linha["CodigoBarra"]);
                        ingressoImpressaoCancelar.BlackList = true;
                        listaIngressoImpressaoCancelar.Add(ingressoImpressaoCancelar);
                    }
                    ingressoIDAux = Convert.ToInt32(linha["IngressoID"]);
                }
            }

            if (listaIngressoImpressaoCancelar.Count == 0)
            {
                return;
            }

            bilheteria.CancelarImpressao(bd, listaIngressoImpressaoCancelar, usuarioID, empresaID, motivo, usuarioID);
        }

        private void AtualizarEntregaVenda(BD bd, int vendaBilheteriaID, int clienteEnderecoID, int entregaControleID, int entregaAgendaID, int pdvID)
        {
            this.ClienteEnderecoID.Valor = clienteEnderecoID;
            this.EntregaControleID.Valor = entregaControleID;
            this.PdvID.Valor = pdvID;
            this.Atualizar(bd);
        }

        private string BuscaSenha(int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tVendaBilheteria.Senha 
                        FROM  tVendaBilheteria (NOLOCK)
                        WHERE ID = {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar a Senha");
                return bd.LerString("Senha");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int BuscaID(string Senha)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tVendaBilheteria.ID 
                        FROM  tVendaBilheteria (NOLOCK)
                        WHERE Senha = '{0}'", Senha);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar a Senha");

                return bd.LerInt("ID");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool InserirSangria(BD _bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteria(CaixaID, DataVenda, Status , ValorTotal) ");
                sql.Append("VALUES (@001,'@002','@003',@004); SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.CaixaID.ValorBD);
                sql.Replace("@002", this.DataVenda.ValorBD);
                sql.Replace("@003", this.Status.ValorBD);
                sql.Replace("@004", this.ValorTotal.ValorBD);

                object x = _bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = this.Control.ID > 0;

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<EstruturaSangriaImpressao> SangriaSupervisorCaixa(EstruturaTelaSangria estSangria, int eventoID, int motivoID, int usuarioID, string nome, string identificacao, int caixaID)
        {
            BD _bd = new BD();
            _bd.IniciarTransacao();

            try
            {
                List<EstruturaSangriaImpressao> lstRetorno = new List<EstruturaSangriaImpressao>();

                Sangria oSangria = new Sangria();
                Motivo oMotivo = new Motivo();
                Caixa oCaixa = new Caixa();

                string[] evento = new Evento().EventoLocalNome(eventoID);

                string motivo = oMotivo.BuscarMotivo(motivoID);
                decimal valorTotal = estSangria.Lista.Sum(c => c.Sangria);

                foreach (var item in estSangria.Lista)
                {
                    this.Limpar();
                    if (item.Valor < item.Sangria)
                        throw new Exception("Informe o valor de sangria corretamente");

                    if (item.Sangria == 0)
                        continue;

                    this.Status.Valor = "S";
                    this.ValorTotal.Valor = item.Sangria;
                    this.CaixaID.Valor = item.CaixaID;
                    this.DataVenda.Valor = DateTime.Now;
                    this.InserirSangria(_bd);

                    oSangria.RegistrarSangria(_bd, this.Control.ID, motivoID, eventoID, nome, identificacao);

                    item.VendaBilheteriaID = this.Control.ID;

                    lstRetorno.Add(new EstruturaSangriaImpressao()
                    {
                        Data = DateTime.Now,
                        Identificacao = identificacao,
                        Motivo = motivo,
                        Evento = evento[1],
                        Responsavel = nome,
                        Senha = this.BuscaSenha(item.VendaBilheteriaID),
                        Usuario = oCaixa.BuscaUsuario(caixaID),
                        Valor = this.ValorTotal.Valor
                    });
                }

                _bd.FinalizarTransacao();

                return lstRetorno;
            }
            catch (Exception)
            {
                _bd.DesfazerTransacao();
                throw;
            }
        }

        public EstruturaSangriaImpressao SangriaCaixa(EstruturaTelaSangria estSangria, int eventoID, int motivoID, int usuarioID, string nome, string identificacao)
        {
            try
            {
                Sangria oSangria = new Sangria();
                Motivo oMotivo = new Motivo();
                Caixa oCaixa = new Caixa();

                Evento oEvento = new Evento();
                string[] evento = oEvento.EventoLocalNome(eventoID);

                bd.IniciarTransacao();

                foreach (var item in estSangria.Lista)
                {
                    this.Limpar();
                    if (item.Valor < item.Sangria)
                        throw new Exception("Informe o valor de sangria corretamente");

                    this.Status.Valor = "S";
                    this.ValorTotal.Valor = item.Sangria;
                    this.CaixaID.Valor = item.CaixaID;
                    this.DataVenda.Valor = DateTime.Now;
                    this.InserirSangria(bd);

                    oSangria.RegistrarSangria(bd, this.Control.ID, motivoID, eventoID, nome, identificacao);
                }

                bd.FinalizarTransacao();

                EstruturaSangriaImpressao imp = new EstruturaSangriaImpressao();

                imp.Data = this.DataVenda.Valor;
                imp.Identificacao = identificacao;
                imp.Motivo = oMotivo.BuscarMotivo(motivoID);
                imp.Responsavel = nome;
                imp.Evento = evento[1];
                imp.Senha = this.BuscaSenha(this.Control.ID);
                imp.Usuario = oCaixa.BuscaUsuario(this.CaixaID.Valor);
                imp.Valor = this.ValorTotal.Valor;

                return imp;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class VendaBilheteriaLista : VendaBilheteriaLista_B
    {
        public VendaBilheteriaLista() { }
    }
}
