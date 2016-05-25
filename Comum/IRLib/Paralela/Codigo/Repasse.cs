/**************************************************
* Arquivo: Repasse.cs
* Gerado: 23/04/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace IRLib.Paralela.ClientObjects
{
    public class EstruturaExtratoRepasse
    {

        private Repasse.eTipo tipo;
        public Repasse.eTipo Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private int tipoid;
        public int TipoID
        {
            get { return tipoid; }
            set { tipoid = value; }
        }        


        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private string data;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        private string datasemformatacao;
        public string DataSemFormatacao
        {
            get { return datasemformatacao; }
            set { datasemformatacao = value; }
        }

        private bool subtotal;
        public bool SubTotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }
    }

    public class EstruturaRepasseConta
    {

        private int repasseid;
        public int RepasseID
        {
            get { return repasseid; }
            set { repasseid = value; }
        }

        private int contaid;
        public int ContaID
        {
            get { return contaid; }
            set { contaid = value; }
        }

        private string beneficiario;
        public string Beneficiario
        {
            get { return beneficiario; }
            set { beneficiario = value; }
        }

        private string banco;
        public string Banco
        {
            get { return banco; }
            set { banco = value; }
        }

        private string agencia;
        public string Agencia
        {
            get { return agencia; }
            set { agencia = value; }
        }

        private string conta;
        public string Conta
        {
            get { return conta; }
            set { conta = value; }
        }

        private decimal valortotal;
        public decimal ValorTotal
        {
            get { return valortotal; }
            set { valortotal = value; }
        }

        private int custobancarioid;
        public int CustoBancarioID
        {
            get { return custobancarioid; }
            set { custobancarioid = value; }
        }

        private decimal valorcustobancario;
        public decimal ValorCustoBancario
        {
            get { return valorcustobancario; }
            set { valorcustobancario = value; }
        }

    }
}

namespace IRLib.Paralela
{

    public class Repasse : Repasse_B
    {

        public Repasse() { }

        public Repasse(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Tipo de impressão
        /// </summary>
        public enum eTipoImpressao
        {
            [System.ComponentModel.Description("Impressão")]
            Impressao = 0,
            [System.ComponentModel.Description("Reimpressão")]
            Reimpressao = 1,
            [System.ComponentModel.Description("Protocolo")]
            Protocolo = 2
        }

        /// <summary>
        /// Tipo de ingresso
        /// </summary>
        public enum eTipoIngresso
        {
            [System.ComponentModel.Description("Ingresso Normal")]
            IngressoNormal = 0,
            [System.ComponentModel.Description("Pré-Impresso")]
            PreImpresso = 1,
            [System.ComponentModel.Description("Cortesia")]
            Cortesia = 2
        }

        /// <summary>
        /// Tipo do repasse
        /// </summary>
        public enum eTipo
        {
            [System.ComponentModel.Description("Não definida")]
            NaoDefinida = -1,
            [System.ComponentModel.Description("Crédito por venda")]
            CreditoVenda = 0,
            [System.ComponentModel.Description("Débito por cancelamento de venda")]
            DebitoCancelamentoVenda = 1,
            [System.ComponentModel.Description("Débito por papel")]
            DebitoPapel = 2,
            [System.ComponentModel.Description("Repasse efetuado")]
            RepasseEfetuado = 3,
            [System.ComponentModel.Description("Antecipação de valores")]
            AntecipacaoValores = 4,
            [System.ComponentModel.Description("Débito por custo bancário")]
            DebitoCustoBancario = 5,
            [System.ComponentModel.Description("Crédito operação")]
            CreditoOperacao = 6,
            [System.ComponentModel.Description("Débito por acordo comercial")]
            DebitoAcordoComercial = 7,
            [System.ComponentModel.Description("Taxa de Antecipação")]
            TaxaAtencipacao = 8
        }

        #region Métodos de Manipulação do Repasse

        #region Controle e Log

        internal void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cRepasse (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        internal void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xRepasse (ID, Versao, Tipo, ContratoID, VendaBilheteriaID, DataAberturaCaixa, DataFechamentoCaixa, EmpresaIDCaixa, CanalID, LojaID, UsuarioID, CaixaID, EmpresaIDEvento, LocalID, EventoID, ApresentacaoID, SetorID, PrecoID, PrecoValor, TaxaConveniencia, TaxaConvenienciaValor, ReterComissao, Comissao, ComissaoValor, TaxaEntregaID, TaxaEntregaValor, ValorTotal, TimeStamp, BandeiraID, FormaPagamentoID, PctFormaPagamento, ValorFormaPagamento, Parcelas, ParcelaAtual, PrazoDias, ValorParcela, DataTransacao, DataRecebimento, DiasRepasse, DataCompetencia, Observacao, TaxaADM, IR, ValorParcelaRecebimento, QuantidadePapel, ValorPapelUnitario, TipoImpressao, TipoIngresso, TipoCobrancaPapel, TipoCanal) ");
                sql.Append("SELECT ID, @V, Tipo, ContratoID, VendaBilheteriaID, DataAberturaCaixa, DataFechamentoCaixa, EmpresaIDCaixa, CanalID, LojaID, UsuarioID, CaixaID, EmpresaIDEvento, LocalID, EventoID, ApresentacaoID, SetorID, PrecoID, PrecoValor, TaxaConveniencia, TaxaConvenienciaValor, ReterComissao, Comissao, ComissaoValor, TaxaEntregaID, TaxaEntregaValor, ValorTotal, TimeStamp, BandeiraID, FormaPagamentoID, PctFormaPagamento, ValorFormaPagamento, Parcelas, ParcelaAtual, PrazoDias, ValorParcela, DataTransacao, DataRecebimento, DiasRepasse, DataCompetencia, Observacao, TaxaADM, IR, ValorParcelaRecebimento, QuantidadePapel, ValorPapelUnitario, TipoImpressao, TipoIngresso, TipoCobrancaPapel, TipoCanal FROM tRepasse WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Gravar
        /// <summary>
        /// Inserir novo(a) Repasse
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        /// <returns></returns>
        internal bool Gravar(ref BD bd)
        {

            string sql = string.Empty;

            try
            {
                bool novo = false;                

                System.Threading.Monitor.Enter(bd);

                // Novo
                if (this.Control.ID == 0)
                {
                    this.Control.Versao = 0;
                    novo = true;
                }
                else
                {
                    sql = "SELECT MAX(Versao) FROM cRepasse (NOLOCK) WHERE ID=" + this.Control.ID;
                    object obj = bd.ConsultaValor(sql);
                    int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                    this.Control.Versao = versao;                    
                }

                System.Threading.Monitor.Exit(bd);

                sql = "" +
                    "EXECUTE " +
                    "   salvar_Repasse " +
                    "   @Novo = " + (novo ? 1 : 0) + ", " +
                    "   @ID = " + this.Control.ID + ", " +
                    "   @Tipo = " + this.Tipo.ValorBD + ", " +
                    "   @ContratoID = " + this.ContratoID.ValorBD + ", " +
                    "   @VendaBilheteriaID = " + this.VendaBilheteriaID.ValorBD + ", " +
                    "   @DataAberturaCaixa = '" + this.DataAberturaCaixa.ValorBD + "', " +
                    "   @DataFechamentoCaixa = '" + this.DataFechamentoCaixa.ValorBD + "', " +
                    "   @EmpresaIDCaixa = " + this.EmpresaIDCaixa.ValorBD + ", " +
                    "   @CanalID = " + this.CanalID.ValorBD + ", " +
                    "   @LojaID = " + this.LojaID.ValorBD + ", " +
                    "   @UsuarioID = " + this.UsuarioID.ValorBD + ", " +
                    "   @CaixaID = " + this.CaixaID.ValorBD + ", " +
                    "   @EmpresaIDEvento = " + this.EmpresaIDEvento.ValorBD + ", " +
                    "   @LocalID = " + this.LocalID.ValorBD + ", " +
                    "   @EventoID = " + this.EventoID.ValorBD + ", " +
                    "   @ApresentacaoID = " + this.ApresentacaoID.ValorBD + ", " +
                    "   @SetorID = " + this.SetorID.ValorBD + ", " +
                    "   @PrecoID = " + this.PrecoID.ValorBD + ", " +
                    "   @PrecoValor = " + this.PrecoValor.ValorBD + ", " +
                    "   @TaxaConveniencia = " + this.TaxaConveniencia.ValorBD + ", " +
                    "   @TaxaConvenienciaValor = " + this.TaxaConvenienciaValor.ValorBD + ", " +
                    "   @ReterComissao = '" + this.ReterComissao.ValorBD + "', " +
                    "   @Comissao = " + this.Comissao.ValorBD + ", " +
                    "   @ComissaoValor = " + this.ComissaoValor.ValorBD + ", " +
                    "   @TaxaEntregaID = " + this.TaxaEntregaID.ValorBD + ", " +
                    "   @TaxaEntregaValor = " + this.TaxaEntregaValor.ValorBD + ", " +
                    "   @ValorTotal = " + this.ValorTotal.ValorBD + ", " +
                    "   @TimeStamp = '" + this.TimeStamp.ValorBD + "', " +
                    "   @BandeiraID = " + this.BandeiraID.ValorBD + ", " +
                    "   @FormaPagamentoID = " + this.FormaPagamentoID.ValorBD + ", " +
                    "   @PctFormaPagamento = " + this.PctFormaPagamento.ValorBD + ", " +
                    "   @ValorFormaPagamento = " + this.ValorFormaPagamento.ValorBD + ", " +
                    "   @Parcelas = " + this.Parcelas.ValorBD + ", " +
                    "   @ParcelaAtual = " + this.ParcelaAtual.ValorBD + ", " +
                    "   @PrazoDias = " + this.PrazoDias.ValorBD + ", " +
                    "   @ValorParcela = " + this.ValorParcela.ValorBD + ", " +
                    "   @DataTransacao = '" + this.DataTransacao.ValorBD + "', " +
                    "   @DataRecebimento = '" + this.DataRecebimento.ValorBD + "', " +
                    "   @DiasRepasse = " + this.DiasRepasse.ValorBD + ", " +
                    "   @DataCompetencia = '" + this.DataCompetencia.ValorBD + "', " +
                    "   @DataRegimeCaixa = '" + this.DataRegimeCaixa.ValorBD + "', " +
                    "   @Observacao = '" + this.Observacao.ValorBD + "', " +
                    "   @TaxaADM = " + this.TaxaADM.ValorBD + ", " +
                    "   @IR = '" + this.IR.ValorBD + "', " +
                    "   @ValorParcelaRecebimento = " + this.ValorParcelaRecebimento.ValorBD + ", " +
                    "   @TipoCobrancaPapel = " + this.TipoCobrancaPapel.ValorBD + ", " +
                    "   @QuantidadePapel = " + this.QuantidadePapel.ValorBD + ", " +
                    "   @TipoCanal = " + this.TipoCanal.ValorBD + ", " +
                    "   @ContaID = " + this.ContaID.ValorBD;

                object x = bd.ConsultaValor(sql);

                this.Control.ID = Convert.ToInt32(x);

                InserirControle(((novo) ? "I" : "U"), bd);

                if (!novo)
                    InserirLog(bd);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - SQL: " + sql);
            }

        }
        #endregion
                
        #region Excluir

        /// <summary>
        /// Exclui Repasse com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// /// <param name="id">Objeto de Conexão</param>
        /// <returns></returns>
        internal bool Excluir(int id, BD bd)
        {

            try
            {

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cRepasse WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D", bd);
                InserirLog(bd);

                string sqlDelete = "apagar_Repasse @RepasseID = " + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #endregion

        /// <summary>
        /// Alimenta o repasse das vendas e cancelamentos do caixa
        /// </summary>
        /// <param name="caixaID">ID do caixa</param>
        public void PopulaRepasseAlimentarCaixa(List<int> caixasID)
        {
            BD bdLeitura = new BD();
            try
            {
                bd.IniciarTransacao();

                foreach (int caixaID in caixasID)
                {
                    AlimentaVendaCancelamento(bd, bdLeitura, caixaID);
                    AlimentaPapel(bd, bdLeitura, caixaID);
                }

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

        /// <summary>
        /// Alimenta o repasse das vendas e cancelamentos do caixa
        /// </summary>
        /// <param name="bd">Banco que está utilizando transação</param>
        /// <param name="bdLeitura">Banco para leitura de dados</param>
        /// <param name="caixaID">ID do caixa</param>
        internal void AlimentaVendaCancelamento(BD bd, BD bdLeitura, int caixaID)
        {

            try
            {
                // Job 265 - LP

                DateTime dataapresentacao;
                string strAcao;                

                using (IDataReader oDataReader = bdLeitura.Consulta("" +
                    "EXECUTE monta_RepasseVenda " + caixaID))
                {
                    while (oDataReader.Read())
                    {
                        this.Control.ID = 0;
                        this.TimeStamp.Valor = DateTime.Now;                        
                        strAcao = bdLeitura.LerString("Acao");
                        switch (strAcao)
                        {
                            case IngressoLog.VENDER:
                                this.Tipo.Valor = Convert.ToInt32(eTipo.CreditoVenda);
                                break;
                            case IngressoLog.CANCELAR:
                                this.Tipo.Valor = Convert.ToInt32(eTipo.DebitoCancelamentoVenda);
                                break;
                        }
                        this.TaxaADM.Valor = bdLeitura.LerDecimal("TaxaAdm");

                        // Reter comissão ?
                        this.ReterComissao.Valor = bdLeitura.LerInt("TipoComissao") == Convert.ToInt32(Contrato.TipoDeComissao.ReterNoRepasse);

                        this.ComissaoValor.Valor = bdLeitura.LerDecimal("ComissaoValor") * (strAcao == VendaBilheteria.CANCELADO ? -1 : 1);
                        this.TaxaEntregaValor.Valor = bdLeitura.LerDecimal("TaxaEntregaValor") * (strAcao == VendaBilheteria.CANCELADO ? -1 : 1);
                        this.ValorTotal.Valor = (bdLeitura.LerDecimal("Valor") + bdLeitura.LerDecimal("TaxaEntregaValor") + bdLeitura.LerDecimal("TaxaConvenienciaValor")) * (strAcao == VendaBilheteria.CANCELADO ? -1 : 1);
                        this.ValorFormaPagamento.Valor = bdLeitura.LerDecimal("ValorFormaPagamento") * (strAcao == VendaBilheteria.CANCELADO ? -1 : 1);
                        this.Parcelas.Valor = bdLeitura.LerInt("Parcelas");
                        if (this.Parcelas.Valor == 0)
                            this.Parcelas.Valor = 1;

                        // Parcela atual
                        this.ParcelaAtual.Valor = bdLeitura.LerInt("ParcelaAtual");
                        if (this.ParcelaAtual.Valor == 0)
                            this.ParcelaAtual.Valor = 1;

                        this.ValorParcela.Valor = (bdLeitura.LerDecimal("ValorFormaPagamento") / this.Parcelas.Valor) * (strAcao == VendaBilheteria.CANCELADO ? -1 : 1);
                        // Verifica se ficou algum saldo para receber na última parcela, devido a divisão
                        if (this.Parcelas.Valor > 1 && this.ParcelaAtual.Valor == this.Parcelas.Valor)
                            if ((this.ValorParcela.Valor * (this.Parcelas.Valor - 1)) + this.ValorParcela.Valor != this.ValorFormaPagamento.Valor)
                                this.ValorParcela.Valor = this.ValorFormaPagamento.Valor - (this.ValorParcela.Valor * (this.Parcelas.Valor - 1));

                        this.ValorParcelaRecebimento.Valor = ((bdLeitura.LerDecimal("ValorFormaPagamento") - this.TaxaADM.Valor) / this.Parcelas.Valor) * (strAcao == VendaBilheteria.CANCELADO ? -1 : 1);

                        // Data recebimento - Calculada com base no vínculo entre Empresa e FormaPagamento (tEmpresaFormaPagamento) 
                        this.DataTransacao.Valor = bdLeitura.LerDateTime("DataTransacao");
                        if (this.ParcelaAtual.Valor == 1)
                        {
                            // Primeira parcela - Data da venda + Dias para recebimento (tEmpresaFormaPagamento).
                            this.DataRecebimento.Valor = this.DataTransacao.Valor.AddDays(bdLeitura.LerInt("DiasRecebimento"));
                        }
                        else
                        {
                            // Demais parcelas - Data da venda + Dias para recebimento + (Parcela atual * 30).
                            this.DataRecebimento.Valor = this.DataTransacao.Valor.AddDays(bdLeitura.LerInt("DiasRecebimento") + (this.ParcelaAtual.Valor * 30));
                        }

                        this.DiasRepasse.Valor = bdLeitura.LerInt("DiasRepasse");

                        // Data de repasse
                        //DataCompetencia
                        /*
                        Repasse por data de apresentação:
                            Caso 1 – Data de recebimento da parcela < Data da apresentação: 
                                Data da apresentação + Dias para repasse (tEmpresaFormaPagamento) + Dias necessários para a próxima quinta-feira.
                            Caso 2 – Data de recebimento da parcela > Data da apresentação: 
                                Data do recebimento + Dias para repasse (tEmpresaFormaPagamento) + Dias necessários para a próxima quinta-feira.
                        */
                        /*
                        Repasse por data de recebimento: 
                            Data de recebimento + Dias para repasse (tFormaPagamento) + Dias necessários para a próxima quinta-feira.
                        */

                        dataapresentacao = bdLeitura.LerDateTime("DataApresentacao");
                        if (bdLeitura.LerInt("TipoRepasse") == Convert.ToInt32(Contrato.TipoDeRepasse.DataApresentacao) && this.DataRecebimento.Valor < dataapresentacao)
                        {
                            this.DataCompetencia.Valor = dataapresentacao.AddDays(this.DiasRepasse.Valor);
                        }
                        else
                        {
                            this.DataCompetencia.Valor = this.DataRecebimento.Valor.AddDays(this.DiasRepasse.Valor);
                        }
                        this.DataCompetencia.Valor = Utilitario.ProximoDiaDeSemana(this.DataCompetencia.Valor, DayOfWeek.Thursday);

                        this.Observacao.Valor = "";
                        this.ContratoID.Valor = bdLeitura.LerInt("ContratoID");
                        this.VendaBilheteriaID.Valor = bdLeitura.LerInt("VendaBilheteriaID");
                        this.DataAberturaCaixa.Valor = bdLeitura.LerDateTime("DataAberturaCaixa");
                        this.DataFechamentoCaixa.Valor = bdLeitura.LerDateTime("DataFechamentoCaixa");
                        this.EmpresaIDCaixa.Valor = bdLeitura.LerInt("EmpresaIDCaixa");
                        this.CanalID.Valor = bdLeitura.LerInt("CanalID");
                        this.LojaID.Valor = bdLeitura.LerInt("LojaID");
                        this.UsuarioID.Valor = bdLeitura.LerInt("UsuarioID");
                        this.CaixaID.Valor = bdLeitura.LerInt("CaixaID");
                        this.EmpresaIDEvento.Valor = bdLeitura.LerInt("EmpresaIDEvento");
                        this.LocalID.Valor = bdLeitura.LerInt("LocalID");
                        this.EventoID.Valor = bdLeitura.LerInt("EventoID");
                        this.ApresentacaoID.Valor = bdLeitura.LerInt("ApresentacaoID");
                        this.SetorID.Valor = bdLeitura.LerInt("SetorID");
                        this.PrecoID.Valor = bdLeitura.LerInt("PrecoID");
                        this.PrecoValor.Valor = bdLeitura.LerDecimal("PrecoValor");
                        this.TaxaConveniencia.Valor = bdLeitura.LerInt("TaxaConveniencia");
                        this.TaxaConvenienciaValor.Valor = bdLeitura.LerDecimal("TaxaConvenienciaValor");
                        this.Comissao.Valor = bdLeitura.LerInt("Comissao");
                        this.TaxaEntregaID.Valor = bdLeitura.LerInt("TaxaEntregaID");
                        this.BandeiraID.Valor = bdLeitura.LerInt("BandeiraID");
                        this.FormaPagamentoID.Valor = bdLeitura.LerInt("FormaPagamentoID");
                        this.PctFormaPagamento.Valor = bdLeitura.LerInt("PctFormaPagamento");
                        this.PrazoDias.Valor = bdLeitura.LerInt("PrazoDias");
                        this.DataTransacao.Valor = bdLeitura.LerDateTime("DataTransacao");
                        this.IR.Valor = bdLeitura.LerBoolean("IR");
                        this.TipoCanal.Valor = bdLeitura.LerInt("CanalTipoID");

                        if (!Gravar(ref bd))
                            throw new CaixaException("Não foi possível alimentar os dados de repasse.");

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Alimenta o repasse do papel utilizado
        /// </summary>
        /// <param name="bd">Banco que está utilizando transação</param>
        /// <param name="bdLeitura">Banco para leitura de dados</param>
        /// <param name="caixaID">ID do caixa</param>
        internal void AlimentaPapel(BD bd, BD bdLeitura, int caixaID)
        {
            try
            {
                // Job 266 - LP


                DateTime datacompetencia;
                DateTime dataprimeirorepasse;                

                this.Tipo.Valor = Convert.ToInt32(eTipo.DebitoPapel);

                using (IDataReader oDataReader = bdLeitura.Consulta("" +
                    "EXECUTE monta_RepassePapeis @CaixaID = " + caixaID))
                {
                    while (oDataReader.Read())
                    {
                        this.Control.ID = 0;

                        this.TimeStamp.Valor = DateTime.Now;
                        
                        dataprimeirorepasse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dataprimeirorepasse = Utilitario.ProximoDiaDeSemana(dataprimeirorepasse, DayOfWeek.Thursday);

                        datacompetencia = bdLeitura.LerDateTime("DataTransacao");

                        if (datacompetencia >= dataprimeirorepasse)
                        {
                            dataprimeirorepasse = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
                            dataprimeirorepasse = Utilitario.ProximoDiaDeSemana(dataprimeirorepasse, DayOfWeek.Thursday);
                        }
                        this.DataCompetencia.Valor = new DateTime(dataprimeirorepasse.Year, dataprimeirorepasse.Month, dataprimeirorepasse.Day, datacompetencia.Hour, datacompetencia.Minute, datacompetencia.Second, datacompetencia.Millisecond);
                        this.ValorTotal.Valor = bdLeitura.LerDecimal("ValorTotal");
                        this.ContratoID.Valor = bdLeitura.LerInt("ContratoID");
                        this.DataAberturaCaixa.Valor = bdLeitura.LerDateTime("DataAberturaCaixa");
                        this.DataFechamentoCaixa.Valor = bdLeitura.LerDateTime("DataFechamentoCaixa");
                        this.EmpresaIDCaixa.Valor = bdLeitura.LerInt("EmpresaIDCaixa");
                        this.CanalID.Valor = bdLeitura.LerInt("CanalID");
                        this.LojaID.Valor = bdLeitura.LerInt("LojaID");
                        this.UsuarioID.Valor = bdLeitura.LerInt("UsuarioID");
                        this.CaixaID.Valor = bdLeitura.LerInt("CaixaID");
                        this.EmpresaIDEvento.Valor = bdLeitura.LerInt("EmpresaIDEvento");
                        this.LocalID.Valor = bdLeitura.LerInt("LocalID");
                        this.EventoID.Valor = bdLeitura.LerInt("EventoID");
                        this.DataTransacao.Valor = bdLeitura.LerDateTime("DataTransacao");
                        this.QuantidadePapel.Valor = bdLeitura.LerInt("QuantidadePapel");
                        this.ValorPapelUnitario.Valor = bdLeitura.LerDecimal("ValorPapelUnitario");
                        this.TipoImpressao.Valor = bdLeitura.LerInt("TipoImpressao");
                        this.TipoIngresso.Valor = bdLeitura.LerInt("TipoIngresso");
                        this.TipoCobrancaPapel.Valor =  (bdLeitura.LerBoolean("PapelCobrancaUtilizacao") ? Convert.ToInt32(Contrato.TipoDeCobrancaPapel.PorUtilizacao) : Convert.ToInt32(Contrato.TipoDeCobrancaPapel.PorEnvio));
                        this.TipoCanal.Valor = bdLeitura.LerInt("CanalTipoID");

                        if (!this.Gravar(ref bd))
                            throw new CaixaException("Não foi possível alimentar os dados de repasse.");

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Captura os débitos alimentados manualmente para o evento
        /// </summary>
        /// <param name="eventoID">ID do Evento</param>
        /// <param name="dtInicial">Data Inicial</param>
        /// <param name="dtFinal">Data Final</param>
        /// <returns></returns>
        public List<EstruturaListaRepasse> ListarDebitos(int eventoID, DateTime dtInicial, DateTime dtFinal)
        {
            BD bd = new BD();
            List<EstruturaListaRepasse> lstDebitos = new List<EstruturaListaRepasse>();
            EstruturaListaRepasse item;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "CarregaDebitosRepasse " +
                    "       @EventoID = " + eventoID + ", " +
                    "       @DataInicial = '" + dtInicial.ToString("yyyyMMdd") + "', " +
                    "       @DataFinal = '" + dtFinal.ToString("yyyyMMdd") + "' "))
                {
                    while (oDataReader.Read())
                    {
                        item = new EstruturaListaRepasse();
                        item.ID = bd.LerInt("ID");
                        item.Tipo = (eTipo)bd.LerInt("Tipo");
                        item.DataCompetencia = bd.LerDateTime("DataCompetencia");
                        item.DataCriacao = bd.LerDateTime("DataCriacao");
                        item.ValorTotal = bd.LerDecimal("ValorTotal");
                        item.Observacoes = bd.LerString("Observacao");
                        lstDebitos.Add(item);
                    }
                }

                bd.Fechar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }


            return lstDebitos;
        }      

        /// <summary>
        /// Captura os débitos alimentados manualmente para o evento
        /// </summary>
        /// <param name="localID">ID do Local</param>
        /// <param name="dtRepasse">Data Inicial</param>
        /// <param name="dtFinal">Data Final</param>
        /// <returns></returns>
        public List<EstruturaExtratoRepasse> ListarExtratoRepasse(int localID, DateTime dtRepasseInicial, DateTime dtRepasseFinal)
        {
            BD bd = new BD();
            List<EstruturaExtratoRepasse> lstRetorno = new List<EstruturaExtratoRepasse>();
            EstruturaExtratoRepasse item;
            string dataControle = string.Empty;
            decimal subtotal = 0;
            decimal totalgeral = 0;
            bool blnSubTotal = false;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tRepasse.Tipo, " +
                    "   SUBSTRING(tRepasse.DataCompetencia, 1, 8) AS DataCompetencia, " + 
                    "	CASE " +
                    "		WHEN tRepasse.Tipo = 0 OR tRepasse.Tipo = 1 THEN SUM(tRepasse.ValorParcela) " +
                    "		ELSE SUM(tRepasse.ValorTotal) " +
                    "	END AS Valor " +
                    "FROM " +
                    "	tRepasse (NOLOCK) " +
                    "WHERE " +
                    "	(SUBSTRING(tRepasse.DataCompetencia, 1, 8) >= '" + dtRepasseInicial.ToString("yyyyMMdd") + "') " +
                    "AND " +
                    "	(SUBSTRING(tRepasse.DataCompetencia, 1, 8) <= '" + dtRepasseFinal.ToString("yyyyMMdd") + "') " +
                    "AND " +
                    "	(tRepasse.LocalID = " + localID + ") " +
                    "AND " +
                    "	(" +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.RepasseEfetuado) + " " +
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.AntecipacaoValores) + " " + 
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.CreditoOperacao) + " " +
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.CreditoVenda) + " " +
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.DebitoAcordoComercial) + " " +
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.DebitoCancelamentoVenda) + " " +
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.DebitoPapel) + " " +
                        "OR " +
                            "tRepasse.Tipo = " + Convert.ToInt32(eTipo.TaxaAtencipacao) + " " +
                    "   ) " +
                    "GROUP BY " +
                    "   SUBSTRING(tRepasse.DataCompetencia, 1, 8), " + 
                    "	tRepasse.Tipo " + 
                    "ORDER BY " +
                    "   SUBSTRING(tRepasse.DataCompetencia, 1, 8), " +
                    "	tRepasse.Tipo"))
                {
                    while (oDataReader.Read())
                    {
                        if (dataControle != bd.LerString("DataCompetencia"))
                        {
                            if (dataControle != string.Empty)
                            {
                                item = new EstruturaExtratoRepasse();
                                item.Tipo = eTipo.NaoDefinida;
                                item.TipoID = 0;
                                item.Valor = subtotal;
                                item.Data = "";
                                item.DataSemFormatacao = "";
                                item.SubTotal = true;
                                lstRetorno.Add(item);

                                blnSubTotal = true;
                            }
                            dataControle = bd.LerString("DataCompetencia");
                            subtotal = 0;
                        }
                        item = new EstruturaExtratoRepasse();
                        item.Tipo = (eTipo)bd.LerInt("Tipo");
                        item.TipoID = bd.LerInt("Tipo");
                        item.Valor = bd.LerDecimal("Valor");
                        item.Data = bd.LerDateTime("DataCompetencia").ToString("dd/MM/yyyy");
                        item.DataSemFormatacao = bd.LerDateTime("DataCompetencia").ToString("yyyyMMdd");
                        item.SubTotal = false;
                        lstRetorno.Add(item);

                        subtotal += bd.LerDecimal("Valor");
                        totalgeral += subtotal;
                    }

                    if (blnSubTotal)
                    {
                        item = new EstruturaExtratoRepasse();
                        item.Tipo = eTipo.NaoDefinida;
                        item.Valor = subtotal;
                        item.Data = "";
                        item.DataSemFormatacao = "";
                        item.SubTotal = true;
                        lstRetorno.Add(item);
                    }
                }

                bd.Fechar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }


            return lstRetorno;
        }

        /// <summary>
        /// Captura as datas de repasse do local
        /// </summary>
        /// <param name="localID">ID do Local</param>
        /// <returns></returns>
        public List<DateTime> ListarDataLancamentos(int localID)
        {
            BD bd = new BD();
            List<DateTime> lstRetorno = new List<DateTime>();

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "	SUBSTRING(tRepasse.DataCompetencia, 1, 8) + '0000000' AS DataCompetencia " + 
                    "FROM " + 
                    "	tRepasse (NOLOCK) " + 
                    "WHERE " + 
                    "	(tRepasse.LocalID = " + localID + ")"))
                {
                    while (oDataReader.Read())
                    {
                        lstRetorno.Add(bd.LerDateTime("DataCompetencia"));
                    }
                }

                bd.Fechar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }


            return lstRetorno;
        }

        /// <summary>
        /// Insere e Atualiza os débitos alimentados manualmente para o evento
        /// </summary>
        /// <param name="item">Item de repasse</param>
        /// <returns></returns>
        public int GravarDebitos(EstruturaListaRepasse item)
        {
            BD bd = new BD();
            int intGravarDebitos = 0;
            DateTime data = new DateTime(item.DataCompetencia.Year, item.DataCompetencia.Month, item.DataCompetencia.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            try
            {
                this.Control.ID = item.ID;
                this.EmpresaIDEvento.Valor = item.EmpresaID;
                this.LocalID.Valor = item.LocalID;
                this.EventoID.Valor = item.EventoID;
                this.Tipo.Valor = Convert.ToInt32(item.Tipo);
                this.ValorTotal.Valor = item.ValorTotal;
                this.DataTransacao.Valor = data;
                this.DataCompetencia.Valor = item.DataCompetencia;
                this.DataRegimeCaixa.Valor = item.DataRegimeCaixa;
                this.TimeStamp.Valor = data;
                this.Observacao.Valor = item.Observacoes;

                if (!this.Gravar(ref bd))
                    throw new Exception("Não foi possível atualizar a transação de repasse.");

                intGravarDebitos = this.Control.ID;

                bd.Fechar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }


            return intGravarDebitos;
        }

        /// <summary>
        /// Insere e Atualiza os lançamentos de repasse nas contas bancárias e custos de depósito
        /// </summary>
        /// <param name="lista">Lista de contas</param>
        /// <returns></returns>
        public List<EstruturaRepasseConta> GravarLancamentoRepasse(List<EstruturaRepasseConta> lista, int empresaID, int localID, DateTime dataCompetencia)
        {
            BD bdGravar = new BD();
            DateTime data = new DateTime();

            bdGravar.IniciarTransacao();

            try
            {

                for (int contador = 0; contador < lista.Count; contador++)
                {
                    EstruturaRepasseConta item = lista[contador];

                    #region Gravação das contas

                    this.Control.ID = item.RepasseID;

                    if (item.ValorTotal > 0)
                    {
                        this.Tipo.Valor = Convert.ToInt32(eTipo.RepasseEfetuado);
                        this.EmpresaIDEvento.Valor = empresaID;
                        this.LocalID.Valor = localID;
                        this.TimeStamp.Valor = data;
                        this.DataCompetencia.Valor = dataCompetencia;
                        this.ValorTotal.Valor = item.ValorTotal * -1;
                        this.ContaID.Valor = item.ContaID;
                        this.UsuarioID.Valor = this.Control.UsuarioID;
                        if (!this.Gravar(ref bdGravar))
                            throw new RepasseException("Não foi possível gravar o repasse em conta.");

                        item.RepasseID = this.Control.ID;
                    }
                    else
                    {
                        if (this.Control.ID != 0)
                            if (!this.Excluir(this.Control.ID, bdGravar))
                                throw new RepasseException("Não foi possível excluir o repasse em conta.");

                        item.RepasseID = 0;
                    }                    

                    #endregion

                    #region Gravação dos custos bancários

                    this.Control.ID = item.CustoBancarioID;

                    if (item.ValorCustoBancario > 0)
                    {
                        this.Tipo.Valor = Convert.ToInt32(eTipo.DebitoCustoBancario);
                        this.EmpresaIDEvento.Valor = empresaID;
                        this.LocalID.Valor = localID;
                        this.TimeStamp.Valor = data;
                        this.DataCompetencia.Valor = dataCompetencia;
                        this.ValorTotal.Valor = item.ValorCustoBancario * -1;
                        this.ContaID.Valor = item.ContaID;
                        this.UsuarioID.Valor = this.Control.UsuarioID;
                        if (!this.Gravar(ref bdGravar))
                            throw new RepasseException("Não foi possível gravar o custo bancário.");
                    }
                    else
                    {
                        if (this.Control.ID != 0)
                            if (!this.Excluir(this.Control.ID, bdGravar))
                                throw new RepasseException("Não foi possível excluir o custo bancário.");
                    }
                    
                    #endregion

                    lista[contador] = item;                    
                }

                bdGravar.FinalizarTransacao();
                bdGravar.Fechar();

            }
            catch 
            {
                bdGravar.DesfazerTransacao();
                throw;
            }
            finally
            {
                bdGravar.Fechar();
            }

            return lista;
        }

        public void ApagarDebitos(int RepasseID)
        {

            try
            {
                if (!Excluir(RepasseID, bd))
                    throw new Exception("Não foi possível apagar a transação.");

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

        public List<int> PopulaRepasseGetCaixas() 
        {
            List<int> caixasID = new List<int>();

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tIngressoLog.CaixaID " + 
                    "FROM tIngressoLog (NOLOCK) " +
                    "WHERE " + 
                    "   (tIngressoLog.Acao = '" + IngressoLog.IMPRIMIR + "' OR tIngressoLog.Acao = '" + IngressoLog.REIMPRIMIR + "' OR tIngressoLog.Acao = '" + IngressoLog.EMISSAO_PREIMPRESSO + "' OR tIngressoLog.Acao = '" + IngressoLog.VENDER + "' OR tIngressoLog.Acao = '" + IngressoLog.CANCELAR + "') " + 
                    "EXCEPT " +
                    "SELECT " +
                    "   tRepasse.CaixaID " + 
                    "FROM " + 
                    "   tRepasse (NOLOCK) " +                     
                    "ORDER BY " +
                    "   tIngressoLog.CaixaID"))
                {
                    while (oDataReader.Read())
                    {
                        caixasID.Add(bd.LerInt("CaixaID"));
                    }
                }

                bd.Fechar();
                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return caixasID;
        }

        public List<EstruturaRepasseConta> ListarExtraContas(int empresaID, string data)
        {
            List<EstruturaRepasseConta> repasseConta = new List<EstruturaRepasseConta>();
            EstruturaRepasseConta item;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   ISNULL(tRepasse.ID, 0) AS RepasseID, " +
                    "	tEmpresaConta.ID AS ContaID, " +
                    "	tEmpresaConta.Beneficiario, " +
                    "	tEmpresaConta.Banco, " +
                    "	tEmpresaConta.Agencia, " +
                    "	tEmpresaConta.Conta, " +
                    "	ISNULL(tRepasse.ValorTotal, 0) AS ValorTotal, " +
                    "	ISNULL(tRepasseCustoBancario.ValorTotal, 0) AS ValorCustoBancario, " +
                    "	ISNULL(tRepasseCustoBancario.ID, 0) AS CustoBancarioID " +
                    "FROM tEmpresaConta (NOLOCK) " +
                    "LEFT JOIN tRepasse (NOLOCK) ON tRepasse.ContaID = tEmpresaConta.ID AND SUBSTRING(tRepasse.DataCompetencia, 1, 8) = '" + data + "' AND tRepasse.Tipo = " + Convert.ToInt32(eTipo.RepasseEfetuado) + " " +
                    "LEFT JOIN tRepasse AS tRepasseCustoBancario (NOLOCK) ON tRepasseCustoBancario.ContaID = tRepasse.ContaID AND SUBSTRING(tRepasse.DataCompetencia, 1, 8) = '" + data + "' AND tRepasseCustoBancario.Tipo = " + Convert.ToInt32(eTipo.DebitoCustoBancario) + " " +
                    "WHERE " +
                    "	tEmpresaConta.EmpresaID = " + empresaID))
                {
                    while (oDataReader.Read())
                    {
                        item = new EstruturaRepasseConta();
                        item.RepasseID = bd.LerInt("RepasseID");
                        item.CustoBancarioID = bd.LerInt("CustoBancarioID");
                        item.Beneficiario = bd.LerString("Beneficiario");
                        item.Banco = bd.LerString("Banco");
                        item.Agencia = bd.LerString("Agencia");
                        item.Conta = bd.LerString("Conta");
                        item.ContaID = bd.LerInt("ContaID");
                        item.ValorTotal = bd.LerDecimal("ValorTotal") * -1;
                        item.ValorCustoBancario = bd.LerDecimal("ValorCustoBancario") * -1;
                        repasseConta.Add(item);
                    }
                }

                bd.Fechar();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return repasseConta;
        }


        public DataTable DetalheLancamento(int LocalID, int TipoID, string Data)
        {
            DataTable tabela = new DataTable("Lancamento");
            DataRow linha;

            try
            {
                switch (TipoID)
                {
                    case (int)eTipo.CreditoVenda :
                    case (int)eTipo.DebitoCancelamentoVenda :

                        tabela.Columns.Clear();
                        tabela.Columns.Add("LocalNome", typeof(string));
                        tabela.Columns.Add("EventoNome", typeof(string));
                        tabela.Columns.Add("ApresentacaoHorario", typeof(string));
                        tabela.Columns.Add("SetorNome", typeof(string));
                        tabela.Columns.Add("PrecoNome", typeof(string));
                        tabela.Columns.Add("Quantidade", typeof(string));
                        tabela.Columns.Add("Valor", typeof(decimal));

                        using (IDataReader oDataReader = bd.Consulta("" +
                            "SELECT " +
                            "	tLocal.Nome AS LocalNome, " +
                            "	tEvento.Nome AS EventoNome, " +
                            "	tApresentacao.Horario AS ApresentacaoHorario, " +
                            "	tSetor.Nome AS SetorNome, " +
                            "	tPreco.Nome AS PrecoNome, " +
                            "	COUNT(*) AS Quantidade, " +
                            "	CASE " +
                            "		WHEN tRepasse.Tipo = 0 OR tRepasse.Tipo = 1 THEN SUM(tRepasse.ValorParcela)	" +
                            "		ELSE SUM(tRepasse.ValorTotal) " +
                            "	END AS Valor " +
                            "FROM tRepasse (NOLOCK) " +
                            "LEFT JOIN tLocal (NOLOCK) ON tLocal.ID = tRepasse.LocalID " +
                            "LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tRepasse.EventoID " +
                            "LEFT JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tRepasse.ApresentacaoID " +
                            "LEFT JOIN tSetor (NOLOCK) ON tSetor.ID = tRepasse.SetorID " +
                            "LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = tRepasse.PrecoID " +
                            "WHERE " +
                            "	(SUBSTRING(tRepasse.DataCompetencia, 1, 8) = '" + Data + "') " +
                            "AND " +
                            "	(tRepasse.LocalID = " + LocalID + ") " +
                            "AND " +
                            "	(tRepasse.Tipo = " + TipoID + ") " +
                            "GROUP BY " +
                            "	tLocal.ID, " +
                            "	tLocal.Nome, " +
                            "	tEvento.ID, " +
                            "	tEvento.Nome, " +
                            "	tApresentacao.ID, " +
                            "	tApresentacao.Horario, " +
                            "	tSetor.ID, " +
                            "	tSetor.Nome, " +
                            "	tPreco.ID, " +
                            "	tPreco.Nome, " +
                            "	tRepasse.Tipo"))
                        {
                            while (oDataReader.Read())
                            {
                                linha = tabela.NewRow();
                                linha["LocalNome"] = bd.LerString("LocalNome");
                                linha["EventoNome"] = bd.LerString("EventoNome");
                                linha["ApresentacaoHorario"] = bd.LerDateTime("ApresentacaoHorario").ToString("dd/MM/yyyy HH:mm");
                                linha["SetorNome"] = bd.LerString("SetorNome");
                                linha["PrecoNome"] = bd.LerString("PrecoNome");
                                linha["Quantidade"] = bd.LerInt("Quantidade");
                                linha["Valor"] = bd.LerDecimal("Valor");
                                tabela.Rows.Add(linha);
                            }
                        }

                        break;
                    case (int)eTipo.DebitoPapel :

                        tabela.Columns.Clear();
                        tabela.Columns.Add("EventoNome", typeof(string));
                        tabela.Columns.Add("TipoImpressao", typeof(string));
                        tabela.Columns.Add("QuantidadePapel", typeof(int));

                        using (IDataReader oDataReader = bd.Consulta("" +                        
                            "SELECT " + 
                            "	tEvento.Nome AS EventoNome, " + 
                            "	tRepasse.TipoImpressao, " + 
                            "	SUM(tRepasse.QuantidadePapel) AS QuantidadePapel " + 
                            "FROM tRepasse (NOLOCK) " + 
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tRepasse.EventoID " + 
                            "WHERE " +
                            "	(SUBSTRING(tRepasse.DataCompetencia, 1, 8) = '" + Data + "') " + 
                            "AND " +
                            "	(tRepasse.LocalID = " + LocalID + ") " + 
                            "AND " +
                            "	(tRepasse.Tipo = " + TipoID + ") " + 
                            "GROUP BY " + 
                            "	tEvento.Nome, " + 
                            "	tRepasse.TipoImpressao "))
                        {
                            string tipoImpressaoDescricao = string.Empty;                            

                            while (oDataReader.Read())
                            {
                                Repasse.eTipoImpressao oTipoImpressao = (Repasse.eTipoImpressao)bd.LerInt("TipoImpressao");
                                DescriptionAttribute[] da = (DescriptionAttribute[])(typeof(Repasse.eTipoImpressao).GetField(oTipoImpressao.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false));
                                tipoImpressaoDescricao = da.Length > 0 ? da[0].Description : oTipoImpressao.ToString();

                                linha = tabela.NewRow();
                                linha["EventoNome"] = bd.LerString("EventoNome");
                                linha["TipoImpressao"] = tipoImpressaoDescricao;
                                linha["QuantidadePapel"] = bd.LerInt("QuantidadePapel");
                                tabela.Rows.Add(linha);
                            }
                        }

                        break;
                    case (int)eTipo.RepasseEfetuado:                    

                        tabela.Columns.Clear();
                        tabela.Columns.Add("Beneficiario", typeof(string));
                        tabela.Columns.Add("Banco", typeof(string));
                        tabela.Columns.Add("Agencia", typeof(string));
                        tabela.Columns.Add("Conta", typeof(string));
                        tabela.Columns.Add("CPFCNPJ", typeof(string));
                        tabela.Columns.Add("ValorTotal", typeof(string));

                        using (IDataReader oDataReader = bd.Consulta("" +
                            "SELECT " +
                            "	tEmpresaConta.ID, " +
                            "	tEmpresaConta.Beneficiario, " +
                            "	tEmpresaConta.Banco, " +
                            "	tEmpresaConta.Agencia, " +
                            "	tEmpresaConta.Conta, " +
                            "	tEmpresaConta.CPFCNPJ, " +
                            "	SUM(tRepasse.ValorTotal) AS ValorTotal " +
                            "FROM tRepasse (NOLOCK) " +
                            "INNER JOIN tEmpresaConta (NOLOCK) ON tEmpresaConta.ID = tRepasse.ContaID " +
                            "WHERE " +
                            "	(SUBSTRING(tRepasse.DataCompetencia, 1, 8) = '" + Data + "') " +
                            "AND " +
                            "	(tRepasse.LocalID = " + LocalID + ") " +
                            "AND " +
                            "	(tRepasse.Tipo = " + TipoID + ") " +
                            "GROUP BY " +
                            "	tEmpresaConta.ID, " +
                            "	tEmpresaConta.Beneficiario, " +
                            "	tEmpresaConta.Banco, " +
                            "	tEmpresaConta.Agencia, " +
                            "	tEmpresaConta.Conta, " +
                            "	tEmpresaConta.CPFCNPJ, " +
                            "	tRepasse.Tipo"))
                        {
                            while (oDataReader.Read())
                            {
                                linha = tabela.NewRow();
                                linha["Beneficiario"] = bd.LerString("Beneficiario");
                                linha["Banco"] = bd.LerString("Banco");
                                linha["Agencia"] = bd.LerString("Agencia");
                                linha["Conta"] = bd.LerString("Conta");
                                linha["CPFCNPJ"] = bd.LerString("CPFCNPJ");
                                linha["ValorTotal"] = bd.LerDecimal("ValorTotal");
                                tabela.Rows.Add(linha);
                            }
                        }

                        break;

                    case (int)eTipo.AntecipacaoValores:
                    case (int)eTipo.CreditoOperacao:
                    case (int)eTipo.DebitoAcordoComercial:
                    case (int)eTipo.TaxaAtencipacao:
                    case (int)eTipo.DebitoCustoBancario :

                        tabela.Columns.Clear();
                        tabela.Columns.Add("EventoNome", typeof(string));
                        tabela.Columns.Add("DataRegimeCaixa", typeof(string));
                        tabela.Columns.Add("Observacao", typeof(string));
                        tabela.Columns.Add("ValorTotal", typeof(decimal));

                        using (IDataReader oDataReader = bd.Consulta("" +
                            "SELECT " +
                            "	tEvento.Nome AS EventoNome, " +
                            "	tRepasse.DataRegimeCaixa, " +
                            "	tRepasse.Observacao, " +
                            "	SUM(tRepasse.ValorTotal) AS ValorTotal " +
                            "FROM tRepasse (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tRepasse.EventoID " +
                            "WHERE " +
                            "	(SUBSTRING(tRepasse.DataCompetencia, 1, 8) = '" + Data + "') " +
                            "AND " +
                            "	(tRepasse.LocalID = " + LocalID + ") " +
                            "AND " +
                            "	(tRepasse.Tipo = " + TipoID + ") " +
                            "GROUP BY " +
                            "	tEvento.Nome, " +
                            "	tRepasse.DataRegimeCaixa, " +
                            "	tRepasse.Observacao"))
                        {
                            while (oDataReader.Read())
                            {
                                linha = tabela.NewRow();
                                linha["EventoNome"] = bd.LerString("EventoNome");
                                linha["DataRegimeCaixa"] = bd.LerDateTime("DataRegimeCaixa").ToString("dd/MM/yyyy");
                                linha["Observacao"] = bd.LerString("Observacao");
                                linha["ValorTotal"] = bd.LerDecimal("ValorTotal");
                                tabela.Rows.Add(linha);
                            }
                        }

                        break;
                }
                

                bd.Fechar();

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }
    }



    public class RepasseLista : RepasseLista_B
    {

        public RepasseLista() { }

        public RepasseLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
