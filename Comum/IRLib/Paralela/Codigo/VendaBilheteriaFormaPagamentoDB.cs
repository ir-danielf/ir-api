/******************************************************
* Arquivo VendaBilheteriaFormaPagamentoDB.cs
* Gerado em: 05/12/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "VendaBilheteriaFormaPagamento_B"

    public abstract class VendaBilheteriaFormaPagamento_B : BaseBD
    {

        public formapagamentoid FormaPagamentoID = new formapagamentoid();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public valor Valor = new valor();
        public porcentagem Porcentagem = new porcentagem();
        public dias Dias = new dias();
        public taxaadm TaxaAdm = new taxaadm();
        public ir IR = new ir();
        public datadeposito DataDeposito = new datadeposito();
        public atualizado Atualizado = new atualizado();
        public valeingressoid ValeIngressoID = new valeingressoid();
        public vendabilheteriaformapagamentotefid VendaBilheteriaFormaPagamentoTEFID = new vendabilheteriaformapagamentotefid();
        public mensagemretorno MensagemRetorno = new mensagemretorno();
        public horatransacao HoraTransacao = new horatransacao();
        public datatransacao DataTransacao = new datatransacao();
        public codigoir CodigoIR = new codigoir();
        public numeroautorizacao NumeroAutorizacao = new numeroautorizacao();
        public nsuhost NSUHost = new nsuhost();
        public nsusitef NSUSitef = new nsusitef();
        public cupom Cupom = new cupom();
        public dadosconfirmacaovenda DadosConfirmacaoVenda = new dadosconfirmacaovenda();
        public rede Rede = new rede();
        public codigorespostatransacao CodigoRespostaTransacao = new codigorespostatransacao();
        public codigorespostavenda CodigoRespostaVenda = new codigorespostavenda();
        public cartaoid CartaoID = new cartaoid();
        public tokenpaypal TokenPayPal = new tokenpaypal();
        public payeridpaypal PayerIDPaypal = new payeridpaypal();
        public correlationid CorrelationID = new correlationid();
        public transactionid TransactionID = new transactionid();
        public coeficiente Coeficiente = new coeficiente();
        public jurosvalor JurosValor = new jurosvalor();

        public VendaBilheteriaFormaPagamento_B() { }

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteriaFormaPagamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteriaFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Porcentagem.ValorBD = bd.LerDecimal("Porcentagem").ToString();
                    this.Dias.ValorBD = bd.LerInt("Dias").ToString();
                    this.TaxaAdm.ValorBD = bd.LerDecimal("TaxaAdm").ToString();
                    this.IR.ValorBD = bd.LerString("IR");
                    this.DataDeposito.ValorBD = bd.LerString("DataDeposito");
                    this.ValeIngressoID.ValorBD = bd.LerInt("ValeIngressoID").ToString();
                    this.VendaBilheteriaFormaPagamentoTEFID.ValorBD = bd.LerInt("VendaBilheteriaFormaPagamentoTEFID").ToString();
                    this.MensagemRetorno.ValorBD = bd.LerString("MensagemRetorno");
                    this.HoraTransacao.ValorBD = bd.LerString("HoraTransacao");
                    this.DataTransacao.ValorBD = bd.LerString("DataTransacao");
                    this.CodigoIR.ValorBD = bd.LerString("CodigoIR");
                    this.NumeroAutorizacao.ValorBD = bd.LerString("NumeroAutorizacao");
                    this.NSUHost.ValorBD = bd.LerString("NSUHost");
                    this.NSUSitef.ValorBD = bd.LerString("NSUSitef");
                    this.Cupom.ValorBD = bd.LerString("Cupom");
                    this.DadosConfirmacaoVenda.ValorBD = bd.LerString("DadosConfirmacaoVenda");
                    this.Rede.ValorBD = bd.LerString("Rede");
                    this.CodigoRespostaTransacao.ValorBD = bd.LerString("CodigoRespostaTransacao");
                    this.CodigoRespostaVenda.ValorBD = bd.LerString("CodigoRespostaVenda");
                    this.CartaoID.ValorBD = bd.LerInt("CartaoID").ToString();
                    this.TokenPayPal.ValorBD = bd.LerString("TokenPayPal");
                    this.PayerIDPaypal.ValorBD = bd.LerString("PayerIDPaypal");
                    this.CorrelationID.ValorBD = bd.LerString("CorrelationID");
                    this.TransactionID.ValorBD = bd.LerString("TransactionID");
                    this.Coeficiente.ValorBD = bd.LerDecimal("Coeficiente").ToString();
                    this.JurosValor.ValorBD = bd.LerDecimal("JurosValor").ToString();
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

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteriaFormaPagamento do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xVendaBilheteriaFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Porcentagem.ValorBD = bd.LerDecimal("Porcentagem").ToString();
                    this.Dias.ValorBD = bd.LerInt("Dias").ToString();
                    this.TaxaAdm.ValorBD = bd.LerDecimal("TaxaAdm").ToString();
                    this.IR.ValorBD = bd.LerString("IR");
                    this.DataDeposito.ValorBD = bd.LerString("DataDeposito");
                    this.ValeIngressoID.ValorBD = bd.LerInt("ValeIngressoID").ToString();
                    this.VendaBilheteriaFormaPagamentoTEFID.ValorBD = bd.LerInt("VendaBilheteriaFormaPagamentoTEFID").ToString();
                    this.MensagemRetorno.ValorBD = bd.LerString("MensagemRetorno");
                    this.HoraTransacao.ValorBD = bd.LerString("HoraTransacao");
                    this.DataTransacao.ValorBD = bd.LerString("DataTransacao");
                    this.CodigoIR.ValorBD = bd.LerString("CodigoIR");
                    this.NumeroAutorizacao.ValorBD = bd.LerString("NumeroAutorizacao");
                    this.NSUHost.ValorBD = bd.LerString("NSUHost");
                    this.NSUSitef.ValorBD = bd.LerString("NSUSitef");
                    this.Cupom.ValorBD = bd.LerString("Cupom");
                    this.DadosConfirmacaoVenda.ValorBD = bd.LerString("DadosConfirmacaoVenda");
                    this.Rede.ValorBD = bd.LerString("Rede");
                    this.CodigoRespostaTransacao.ValorBD = bd.LerString("CodigoRespostaTransacao");
                    this.CodigoRespostaVenda.ValorBD = bd.LerString("CodigoRespostaVenda");
                    this.CartaoID.ValorBD = bd.LerInt("CartaoID").ToString();
                    this.TokenPayPal.ValorBD = bd.LerString("TokenPayPal");
                    this.PayerIDPaypal.ValorBD = bd.LerString("PayerIDPaypal");
                    this.CorrelationID.ValorBD = bd.LerString("CorrelationID");
                    this.TransactionID.ValorBD = bd.LerString("TransactionID");
                    this.Coeficiente.ValorBD = bd.LerDecimal("Coeficiente").ToString();
                    this.JurosValor.ValorBD = bd.LerDecimal("JurosValor").ToString();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cVendaBilheteriaFormaPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xVendaBilheteriaFormaPagamento (ID, Versao, FormaPagamentoID, VendaBilheteriaID, Valor, Porcentagem, Dias, TaxaAdm, IR, DataDeposito, Atualizado, ValeIngressoID, VendaBilheteriaFormaPagamentoTEFID, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CodigoRespostaVenda, CartaoID, TokenPayPal, PayerIDPaypal, CorrelationID, TransactionID, Coeficiente, JurosValor) ");
                sql.Append("SELECT ID, @V, FormaPagamentoID, VendaBilheteriaID, Valor, Porcentagem, Dias, TaxaAdm, IR, DataDeposito, Atualizado, ValeIngressoID, VendaBilheteriaFormaPagamentoTEFID, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CodigoRespostaVenda, CartaoID, TokenPayPal, PayerIDPaypal, CorrelationID, TransactionID, Coeficiente, JurosValor FROM tVendaBilheteriaFormaPagamento WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cVendaBilheteriaFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamento(ID, FormaPagamentoID, VendaBilheteriaID, Valor, Porcentagem, Dias, TaxaAdm, IR, DataDeposito, Atualizado, ValeIngressoID, VendaBilheteriaFormaPagamentoTEFID, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CodigoRespostaVenda, CartaoID, TokenPayPal, PayerIDPaypal, CorrelationID, TransactionID, Coeficiente, JurosValor) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004',@005,'@006','@007','@008','@009',@010,@011,'@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023',@024,'@025','@026','@027','@028','@029','@030')");

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
                sql.Replace("@025", this.TokenPayPal.ValorBD);
                sql.Replace("@026", this.PayerIDPaypal.ValorBD);
                sql.Replace("@027", this.CorrelationID.ValorBD);
                sql.Replace("@028", this.TransactionID.ValorBD);
                sql.Replace("@029", this.Coeficiente.ValorBD);
                sql.Replace("@030", this.JurosValor.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return result;

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

        /// <summary>
        /// Inserir novo(a) VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cVendaBilheteriaFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamento(ID, FormaPagamentoID, VendaBilheteriaID, Valor, Porcentagem, Dias, TaxaAdm, IR, DataDeposito, Atualizado, ValeIngressoID, VendaBilheteriaFormaPagamentoTEFID, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CodigoRespostaVenda, CartaoID, TokenPayPal, PayerIDPaypal, CorrelationID, TransactionID, Coeficiente, JurosValor) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004',@005,'@006','@007','@008','@009',@010,@011,'@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023',@024,'@025','@026','@027','@028','@029','@030')");

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
                sql.Replace("@025", this.TokenPayPal.ValorBD);
                sql.Replace("@026", this.PayerIDPaypal.ValorBD);
                sql.Replace("@027", this.CorrelationID.ValorBD);
                sql.Replace("@028", this.TransactionID.ValorBD);
                sql.Replace("@029", this.Coeficiente.ValorBD);
                sql.Replace("@030", this.JurosValor.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cVendaBilheteriaFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaFormaPagamento SET FormaPagamentoID = @001, VendaBilheteriaID = @002, Valor = '@003', Porcentagem = '@004', Dias = @005, TaxaAdm = '@006', IR = '@007', DataDeposito = '@008', Atualizado = '@009', ValeIngressoID = @010, VendaBilheteriaFormaPagamentoTEFID = @011, MensagemRetorno = '@012', HoraTransacao = '@013', DataTransacao = '@014', CodigoIR = '@015', NumeroAutorizacao = '@016', NSUHost = '@017', NSUSitef = '@018', Cupom = '@019', DadosConfirmacaoVenda = '@020', Rede = '@021', CodigoRespostaTransacao = '@022', CodigoRespostaVenda = '@023', CartaoID = @024, TokenPayPal = '@025', PayerIDPaypal = '@026', CorrelationID = '@027', TransactionID = '@028', Coeficiente = '@029', JurosValor = '@030' ");
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
                sql.Replace("@025", this.TokenPayPal.ValorBD);
                sql.Replace("@026", this.PayerIDPaypal.ValorBD);
                sql.Replace("@027", this.CorrelationID.ValorBD);
                sql.Replace("@028", this.TransactionID.ValorBD);
                sql.Replace("@029", this.Coeficiente.ValorBD);
                sql.Replace("@030", this.JurosValor.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

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

        /// <summary>
        /// Atualiza VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cVendaBilheteriaFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaFormaPagamento SET FormaPagamentoID = @001, VendaBilheteriaID = @002, Valor = '@003', Porcentagem = '@004', Dias = @005, TaxaAdm = '@006', IR = '@007', DataDeposito = '@008', Atualizado = '@009', ValeIngressoID = @010, VendaBilheteriaFormaPagamentoTEFID = @011, MensagemRetorno = '@012', HoraTransacao = '@013', DataTransacao = '@014', CodigoIR = '@015', NumeroAutorizacao = '@016', NSUHost = '@017', NSUSitef = '@018', Cupom = '@019', DadosConfirmacaoVenda = '@020', Rede = '@021', CodigoRespostaTransacao = '@022', CodigoRespostaVenda = '@023', CartaoID = @024, TokenPayPal = '@025', PayerIDPaypal = '@026', CorrelationID = '@027', TransactionID = '@028', Coeficiente = '@029', JurosValor = '@030' ");
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
                sql.Replace("@025", this.TokenPayPal.ValorBD);
                sql.Replace("@026", this.PayerIDPaypal.ValorBD);
                sql.Replace("@027", this.CorrelationID.ValorBD);
                sql.Replace("@028", this.TransactionID.ValorBD);
                sql.Replace("@029", this.Coeficiente.ValorBD);
                sql.Replace("@030", this.JurosValor.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui VendaBilheteriaFormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cVendaBilheteriaFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

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

        /// <summary>
        /// Exclui VendaBilheteriaFormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cVendaBilheteriaFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override void Limpar()
        {

            this.FormaPagamentoID.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.Valor.Limpar();
            this.Porcentagem.Limpar();
            this.Dias.Limpar();
            this.TaxaAdm.Limpar();
            this.IR.Limpar();
            this.DataDeposito.Limpar();
            this.Atualizado.Limpar();
            this.ValeIngressoID.Limpar();
            this.VendaBilheteriaFormaPagamentoTEFID.Limpar();
            this.MensagemRetorno.Limpar();
            this.HoraTransacao.Limpar();
            this.DataTransacao.Limpar();
            this.CodigoIR.Limpar();
            this.NumeroAutorizacao.Limpar();
            this.NSUHost.Limpar();
            this.NSUSitef.Limpar();
            this.Cupom.Limpar();
            this.DadosConfirmacaoVenda.Limpar();
            this.Rede.Limpar();
            this.CodigoRespostaTransacao.Limpar();
            this.CodigoRespostaVenda.Limpar();
            this.CartaoID.Limpar();
            this.TokenPayPal.Limpar();
            this.PayerIDPaypal.Limpar();
            this.CorrelationID.Limpar();
            this.TransactionID.Limpar();
            this.Coeficiente.Limpar();
            this.JurosValor.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.FormaPagamentoID.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.Valor.Desfazer();
            this.Porcentagem.Desfazer();
            this.Dias.Desfazer();
            this.TaxaAdm.Desfazer();
            this.IR.Desfazer();
            this.DataDeposito.Desfazer();
            this.Atualizado.Desfazer();
            this.ValeIngressoID.Desfazer();
            this.VendaBilheteriaFormaPagamentoTEFID.Desfazer();
            this.MensagemRetorno.Desfazer();
            this.HoraTransacao.Desfazer();
            this.DataTransacao.Desfazer();
            this.CodigoIR.Desfazer();
            this.NumeroAutorizacao.Desfazer();
            this.NSUHost.Desfazer();
            this.NSUSitef.Desfazer();
            this.Cupom.Desfazer();
            this.DadosConfirmacaoVenda.Desfazer();
            this.Rede.Desfazer();
            this.CodigoRespostaTransacao.Desfazer();
            this.CodigoRespostaVenda.Desfazer();
            this.CartaoID.Desfazer();
            this.TokenPayPal.Desfazer();
            this.PayerIDPaypal.Desfazer();
            this.CorrelationID.Desfazer();
            this.TransactionID.Desfazer();
            this.Coeficiente.Desfazer();
            this.JurosValor.Desfazer();
        }

        public class formapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class vendabilheteriaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class valor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Valor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class porcentagem : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Porcentagem";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 5;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class dias : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Dias";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class taxaadm : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaAdm";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class ir : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "IR";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class datadeposito : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataDeposito";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class atualizado : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Atualizado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class valeingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValeIngressoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class vendabilheteriaformapagamentotefid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaFormaPagamentoTEFID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class mensagemretorno : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "MensagemRetorno";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class horatransacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "HoraTransacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 6;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class datatransacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataTransacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 6;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class codigoir : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoIR";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 25;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class numeroautorizacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroAutorizacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 25;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class nsuhost : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NSUHost";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class nsusitef : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NSUSitef";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class cupom : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cupom";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 300;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class dadosconfirmacaovenda : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DadosConfirmacaoVenda";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 40;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class rede : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Rede";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class codigorespostatransacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoRespostaTransacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class codigorespostavenda : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoRespostaVenda";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class cartaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CartaoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class tokenpaypal : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TokenPayPal";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class payeridpaypal : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PayerIDPaypal";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class correlationid : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CorrelationID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class transactionid : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TransactionID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class coeficiente : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Coeficiente";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class jurosvalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "JurosValor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Porcentagem", typeof(decimal));
                tabela.Columns.Add("Dias", typeof(int));
                tabela.Columns.Add("TaxaAdm", typeof(decimal));
                tabela.Columns.Add("IR", typeof(bool));
                tabela.Columns.Add("DataDeposito", typeof(DateTime));
                tabela.Columns.Add("Atualizado", typeof(int));
                tabela.Columns.Add("ValeIngressoID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoTEFID", typeof(int));
                tabela.Columns.Add("MensagemRetorno", typeof(string));
                tabela.Columns.Add("HoraTransacao", typeof(string));
                tabela.Columns.Add("DataTransacao", typeof(string));
                tabela.Columns.Add("CodigoIR", typeof(string));
                tabela.Columns.Add("NumeroAutorizacao", typeof(string));
                tabela.Columns.Add("NSUHost", typeof(string));
                tabela.Columns.Add("NSUSitef", typeof(string));
                tabela.Columns.Add("Cupom", typeof(string));
                tabela.Columns.Add("DadosConfirmacaoVenda", typeof(string));
                tabela.Columns.Add("Rede", typeof(string));
                tabela.Columns.Add("CodigoRespostaTransacao", typeof(string));
                tabela.Columns.Add("CodigoRespostaVenda", typeof(string));
                tabela.Columns.Add("CartaoID", typeof(int));
                tabela.Columns.Add("TokenPayPal", typeof(string));
                tabela.Columns.Add("PayerIDPaypal", typeof(string));
                tabela.Columns.Add("CorrelationID", typeof(string));
                tabela.Columns.Add("TransactionID", typeof(string));
                tabela.Columns.Add("Coeficiente", typeof(decimal));
                tabela.Columns.Add("JurosValor", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "VendaBilheteriaFormaPagamentoLista_B"

    public abstract class VendaBilheteriaFormaPagamentoLista_B : BaseLista
    {

        private bool backup = false;
        protected VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento;

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamentoLista_B()
        {
            vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
        }

        public VendaBilheteriaFormaPagamento VendaBilheteriaFormaPagamento
        {
            get { return vendaBilheteriaFormaPagamento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de VendaBilheteriaFormaPagamento especifico
        /// </summary>
        public override IBaseBD this[int indice]
        {
            get
            {
                if (indice < 0 || indice >= lista.Count)
                {
                    return null;
                }
                else
                {
                    int id = (int)lista[indice];
                    vendaBilheteriaFormaPagamento.Ler(id);
                    return vendaBilheteriaFormaPagamento;
                }
            }
        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamento";

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

        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamento";

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

        /// <summary>
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xVendaBilheteriaFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xVendaBilheteriaFormaPagamento";

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

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche VendaBilheteriaFormaPagamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    vendaBilheteriaFormaPagamento.Ler(id);
                else
                    vendaBilheteriaFormaPagamento.LerBackup(id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui o item corrente da lista
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = vendaBilheteriaFormaPagamento.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {

            try
            {
                if (lista.Count == 0)
                    Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = false;

                if (lista.Count > 0)
                { //verifica se tem itens

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inseri novo(a) VendaBilheteriaFormaPagamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = vendaBilheteriaFormaPagamento.Inserir();
                if (ok)
                {
                    lista.Add(vendaBilheteriaFormaPagamento.Control.ID);
                    Indice = lista.Count - 1;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Obtem uma tabela de todos os campos de VendaBilheteriaFormaPagamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Porcentagem", typeof(decimal));
                tabela.Columns.Add("Dias", typeof(int));
                tabela.Columns.Add("TaxaAdm", typeof(decimal));
                tabela.Columns.Add("IR", typeof(bool));
                tabela.Columns.Add("DataDeposito", typeof(DateTime));
                tabela.Columns.Add("Atualizado", typeof(int));
                tabela.Columns.Add("ValeIngressoID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoTEFID", typeof(int));
                tabela.Columns.Add("MensagemRetorno", typeof(string));
                tabela.Columns.Add("HoraTransacao", typeof(string));
                tabela.Columns.Add("DataTransacao", typeof(string));
                tabela.Columns.Add("CodigoIR", typeof(string));
                tabela.Columns.Add("NumeroAutorizacao", typeof(string));
                tabela.Columns.Add("NSUHost", typeof(string));
                tabela.Columns.Add("NSUSitef", typeof(string));
                tabela.Columns.Add("Cupom", typeof(string));
                tabela.Columns.Add("DadosConfirmacaoVenda", typeof(string));
                tabela.Columns.Add("Rede", typeof(string));
                tabela.Columns.Add("CodigoRespostaTransacao", typeof(string));
                tabela.Columns.Add("CodigoRespostaVenda", typeof(string));
                tabela.Columns.Add("CartaoID", typeof(int));
                tabela.Columns.Add("TokenPayPal", typeof(string));
                tabela.Columns.Add("PayerIDPaypal", typeof(string));
                tabela.Columns.Add("CorrelationID", typeof(string));
                tabela.Columns.Add("TransactionID", typeof(string));
                tabela.Columns.Add("Coeficiente", typeof(decimal));
                tabela.Columns.Add("JurosValor", typeof(decimal));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = vendaBilheteriaFormaPagamento.Control.ID;
                        linha["FormaPagamentoID"] = vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor;
                        linha["VendaBilheteriaID"] = vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor;
                        linha["Valor"] = vendaBilheteriaFormaPagamento.Valor.Valor;
                        linha["Porcentagem"] = vendaBilheteriaFormaPagamento.Porcentagem.Valor;
                        linha["Dias"] = vendaBilheteriaFormaPagamento.Dias.Valor;
                        linha["TaxaAdm"] = vendaBilheteriaFormaPagamento.TaxaAdm.Valor;
                        linha["IR"] = vendaBilheteriaFormaPagamento.IR.Valor;
                        linha["DataDeposito"] = vendaBilheteriaFormaPagamento.DataDeposito.Valor;
                        linha["Atualizado"] = vendaBilheteriaFormaPagamento.Atualizado.Valor;
                        linha["ValeIngressoID"] = vendaBilheteriaFormaPagamento.ValeIngressoID.Valor;
                        linha["VendaBilheteriaFormaPagamentoTEFID"] = vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor;
                        linha["MensagemRetorno"] = vendaBilheteriaFormaPagamento.MensagemRetorno.Valor;
                        linha["HoraTransacao"] = vendaBilheteriaFormaPagamento.HoraTransacao.Valor;
                        linha["DataTransacao"] = vendaBilheteriaFormaPagamento.DataTransacao.Valor;
                        linha["CodigoIR"] = vendaBilheteriaFormaPagamento.CodigoIR.Valor;
                        linha["NumeroAutorizacao"] = vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor;
                        linha["NSUHost"] = vendaBilheteriaFormaPagamento.NSUHost.Valor;
                        linha["NSUSitef"] = vendaBilheteriaFormaPagamento.NSUSitef.Valor;
                        linha["Cupom"] = vendaBilheteriaFormaPagamento.Cupom.Valor;
                        linha["DadosConfirmacaoVenda"] = vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor;
                        linha["Rede"] = vendaBilheteriaFormaPagamento.Rede.Valor;
                        linha["CodigoRespostaTransacao"] = vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor;
                        linha["CodigoRespostaVenda"] = vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor;
                        linha["CartaoID"] = vendaBilheteriaFormaPagamento.CartaoID.Valor;
                        linha["TokenPayPal"] = vendaBilheteriaFormaPagamento.TokenPayPal.Valor;
                        linha["PayerIDPaypal"] = vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor;
                        linha["CorrelationID"] = vendaBilheteriaFormaPagamento.CorrelationID.Valor;
                        linha["TransactionID"] = vendaBilheteriaFormaPagamento.TransactionID.Valor;
                        linha["Coeficiente"] = vendaBilheteriaFormaPagamento.Coeficiente.Valor;
                        linha["JurosValor"] = vendaBilheteriaFormaPagamento.JurosValor.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioVendaBilheteriaFormaPagamento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("FormaPagamentoID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("Porcentagem", typeof(decimal));
                    tabela.Columns.Add("Dias", typeof(int));
                    tabela.Columns.Add("TaxaAdm", typeof(decimal));
                    tabela.Columns.Add("IR", typeof(bool));
                    tabela.Columns.Add("DataDeposito", typeof(DateTime));
                    tabela.Columns.Add("Atualizado", typeof(int));
                    tabela.Columns.Add("ValeIngressoID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaFormaPagamentoTEFID", typeof(int));
                    tabela.Columns.Add("MensagemRetorno", typeof(string));
                    tabela.Columns.Add("HoraTransacao", typeof(string));
                    tabela.Columns.Add("DataTransacao", typeof(string));
                    tabela.Columns.Add("CodigoIR", typeof(string));
                    tabela.Columns.Add("NumeroAutorizacao", typeof(string));
                    tabela.Columns.Add("NSUHost", typeof(string));
                    tabela.Columns.Add("NSUSitef", typeof(string));
                    tabela.Columns.Add("Cupom", typeof(string));
                    tabela.Columns.Add("DadosConfirmacaoVenda", typeof(string));
                    tabela.Columns.Add("Rede", typeof(string));
                    tabela.Columns.Add("CodigoRespostaTransacao", typeof(string));
                    tabela.Columns.Add("CodigoRespostaVenda", typeof(string));
                    tabela.Columns.Add("CartaoID", typeof(int));
                    tabela.Columns.Add("TokenPayPal", typeof(string));
                    tabela.Columns.Add("PayerIDPaypal", typeof(string));
                    tabela.Columns.Add("CorrelationID", typeof(string));
                    tabela.Columns.Add("TransactionID", typeof(string));
                    tabela.Columns.Add("Coeficiente", typeof(decimal));
                    tabela.Columns.Add("JurosValor", typeof(decimal));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["FormaPagamentoID"] = vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor;
                        linha["VendaBilheteriaID"] = vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor;
                        linha["Valor"] = vendaBilheteriaFormaPagamento.Valor.Valor;
                        linha["Porcentagem"] = vendaBilheteriaFormaPagamento.Porcentagem.Valor;
                        linha["Dias"] = vendaBilheteriaFormaPagamento.Dias.Valor;
                        linha["TaxaAdm"] = vendaBilheteriaFormaPagamento.TaxaAdm.Valor;
                        linha["IR"] = vendaBilheteriaFormaPagamento.IR.Valor;
                        linha["DataDeposito"] = vendaBilheteriaFormaPagamento.DataDeposito.Valor;
                        linha["Atualizado"] = vendaBilheteriaFormaPagamento.Atualizado.Valor;
                        linha["ValeIngressoID"] = vendaBilheteriaFormaPagamento.ValeIngressoID.Valor;
                        linha["VendaBilheteriaFormaPagamentoTEFID"] = vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor;
                        linha["MensagemRetorno"] = vendaBilheteriaFormaPagamento.MensagemRetorno.Valor;
                        linha["HoraTransacao"] = vendaBilheteriaFormaPagamento.HoraTransacao.Valor;
                        linha["DataTransacao"] = vendaBilheteriaFormaPagamento.DataTransacao.Valor;
                        linha["CodigoIR"] = vendaBilheteriaFormaPagamento.CodigoIR.Valor;
                        linha["NumeroAutorizacao"] = vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor;
                        linha["NSUHost"] = vendaBilheteriaFormaPagamento.NSUHost.Valor;
                        linha["NSUSitef"] = vendaBilheteriaFormaPagamento.NSUSitef.Valor;
                        linha["Cupom"] = vendaBilheteriaFormaPagamento.Cupom.Valor;
                        linha["DadosConfirmacaoVenda"] = vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor;
                        linha["Rede"] = vendaBilheteriaFormaPagamento.Rede.Valor;
                        linha["CodigoRespostaTransacao"] = vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor;
                        linha["CodigoRespostaVenda"] = vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor;
                        linha["CartaoID"] = vendaBilheteriaFormaPagamento.CartaoID.Valor;
                        linha["TokenPayPal"] = vendaBilheteriaFormaPagamento.TokenPayPal.Valor;
                        linha["PayerIDPaypal"] = vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor;
                        linha["CorrelationID"] = vendaBilheteriaFormaPagamento.CorrelationID.Valor;
                        linha["TransactionID"] = vendaBilheteriaFormaPagamento.TransactionID.Valor;
                        linha["Coeficiente"] = vendaBilheteriaFormaPagamento.Coeficiente.Valor;
                        linha["JurosValor"] = vendaBilheteriaFormaPagamento.JurosValor.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna um IDataReader com ID e o Campo.
        /// </summary>
        /// <param name="campo">Informe o campo. Exemplo: Nome</param>
        /// <returns></returns>
        public override IDataReader ListaPropriedade(string campo)
        {

            try
            {
                string sql;
                switch (campo)
                {
                    case "FormaPagamentoID":
                        sql = "SELECT ID, FormaPagamentoID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY FormaPagamentoID";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "Porcentagem":
                        sql = "SELECT ID, Porcentagem FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Porcentagem";
                        break;
                    case "Dias":
                        sql = "SELECT ID, Dias FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Dias";
                        break;
                    case "TaxaAdm":
                        sql = "SELECT ID, TaxaAdm FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY TaxaAdm";
                        break;
                    case "IR":
                        sql = "SELECT ID, IR FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY IR";
                        break;
                    case "DataDeposito":
                        sql = "SELECT ID, DataDeposito FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY DataDeposito";
                        break;
                    case "Atualizado":
                        sql = "SELECT ID, Atualizado FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Atualizado";
                        break;
                    case "ValeIngressoID":
                        sql = "SELECT ID, ValeIngressoID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY ValeIngressoID";
                        break;
                    case "VendaBilheteriaFormaPagamentoTEFID":
                        sql = "SELECT ID, VendaBilheteriaFormaPagamentoTEFID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaFormaPagamentoTEFID";
                        break;
                    case "MensagemRetorno":
                        sql = "SELECT ID, MensagemRetorno FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY MensagemRetorno";
                        break;
                    case "HoraTransacao":
                        sql = "SELECT ID, HoraTransacao FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY HoraTransacao";
                        break;
                    case "DataTransacao":
                        sql = "SELECT ID, DataTransacao FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY DataTransacao";
                        break;
                    case "CodigoIR":
                        sql = "SELECT ID, CodigoIR FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY CodigoIR";
                        break;
                    case "NumeroAutorizacao":
                        sql = "SELECT ID, NumeroAutorizacao FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY NumeroAutorizacao";
                        break;
                    case "NSUHost":
                        sql = "SELECT ID, NSUHost FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY NSUHost";
                        break;
                    case "NSUSitef":
                        sql = "SELECT ID, NSUSitef FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY NSUSitef";
                        break;
                    case "Cupom":
                        sql = "SELECT ID, Cupom FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Cupom";
                        break;
                    case "DadosConfirmacaoVenda":
                        sql = "SELECT ID, DadosConfirmacaoVenda FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY DadosConfirmacaoVenda";
                        break;
                    case "Rede":
                        sql = "SELECT ID, Rede FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Rede";
                        break;
                    case "CodigoRespostaTransacao":
                        sql = "SELECT ID, CodigoRespostaTransacao FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY CodigoRespostaTransacao";
                        break;
                    case "CodigoRespostaVenda":
                        sql = "SELECT ID, CodigoRespostaVenda FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY CodigoRespostaVenda";
                        break;
                    case "CartaoID":
                        sql = "SELECT ID, CartaoID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY CartaoID";
                        break;
                    case "TokenPayPal":
                        sql = "SELECT ID, TokenPayPal FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY TokenPayPal";
                        break;
                    case "PayerIDPaypal":
                        sql = "SELECT ID, PayerIDPaypal FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY PayerIDPaypal";
                        break;
                    case "CorrelationID":
                        sql = "SELECT ID, CorrelationID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY CorrelationID";
                        break;
                    case "TransactionID":
                        sql = "SELECT ID, TransactionID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY TransactionID";
                        break;
                    case "Coeficiente":
                        sql = "SELECT ID, Coeficiente FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Coeficiente";
                        break;
                    case "JurosValor":
                        sql = "SELECT ID, JurosValor FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY JurosValor";
                        break;
                    default:
                        sql = null;
                        break;
                }

                IDataReader dataReader = bd.Consulta(sql);

                bd.Fechar();

                return dataReader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve um array dos IDs que compoem a lista
        /// </summary>
        /// <returns></returns>		
        public override int[] ToArray()
        {

            try
            {

                int[] a = (int[])lista.ToArray(typeof(int));

                return a;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve uma string dos IDs que compoem a lista concatenada por virgula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            try
            {

                StringBuilder idsBuffer = new StringBuilder();

                int n = lista.Count;
                for (int i = 0; i < n; i++)
                {
                    int id = (int)lista[i];
                    idsBuffer.Append(id + ",");
                }

                string ids = "";

                if (idsBuffer.Length > 0)
                {
                    ids = idsBuffer.ToString();
                    ids = ids.Substring(0, ids.Length - 1);
                }

                return ids;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "VendaBilheteriaFormaPagamentoException"

    [Serializable]
    public class VendaBilheteriaFormaPagamentoException : Exception
    {

        public VendaBilheteriaFormaPagamentoException() : base() { }

        public VendaBilheteriaFormaPagamentoException(string msg) : base(msg) { }

        public VendaBilheteriaFormaPagamentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}