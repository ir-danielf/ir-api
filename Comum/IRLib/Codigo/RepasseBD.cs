/******************************************************
* Arquivo RepasseDB.cs
* Gerado em: 31/08/2009
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Repasse_B"

    public abstract class Repasse_B : BaseBD
    {

        public tipo Tipo = new tipo();
        public contratoid ContratoID = new contratoid();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public dataaberturacaixa DataAberturaCaixa = new dataaberturacaixa();
        public datafechamentocaixa DataFechamentoCaixa = new datafechamentocaixa();
        public empresaidcaixa EmpresaIDCaixa = new empresaidcaixa();
        public canalid CanalID = new canalid();
        public lojaid LojaID = new lojaid();
        public usuarioid UsuarioID = new usuarioid();
        public caixaid CaixaID = new caixaid();
        public empresaidevento EmpresaIDEvento = new empresaidevento();
        public localid LocalID = new localid();
        public eventoid EventoID = new eventoid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public setorid SetorID = new setorid();
        public precoid PrecoID = new precoid();
        public precovalor PrecoValor = new precovalor();
        public taxaconveniencia TaxaConveniencia = new taxaconveniencia();
        public taxaconvenienciavalor TaxaConvenienciaValor = new taxaconvenienciavalor();
        public retercomissao ReterComissao = new retercomissao();
        public comissao Comissao = new comissao();
        public comissaovalor ComissaoValor = new comissaovalor();
        public taxaentregaid TaxaEntregaID = new taxaentregaid();
        public taxaentregavalor TaxaEntregaValor = new taxaentregavalor();
        public valortotal ValorTotal = new valortotal();
        public timestamp TimeStamp = new timestamp();
        public bandeiraid BandeiraID = new bandeiraid();
        public formapagamentoid FormaPagamentoID = new formapagamentoid();
        public pctformapagamento PctFormaPagamento = new pctformapagamento();
        public valorformapagamento ValorFormaPagamento = new valorformapagamento();
        public parcelas Parcelas = new parcelas();
        public parcelaatual ParcelaAtual = new parcelaatual();
        public prazodias PrazoDias = new prazodias();
        public valorparcela ValorParcela = new valorparcela();
        public datatransacao DataTransacao = new datatransacao();
        public datarecebimento DataRecebimento = new datarecebimento();
        public diasrepasse DiasRepasse = new diasrepasse();
        public datacompetencia DataCompetencia = new datacompetencia();
        public dataregimecaixa DataRegimeCaixa = new dataregimecaixa();
        public observacao Observacao = new observacao();
        public taxaadm TaxaADM = new taxaadm();
        public ir IR = new ir();
        public valorparcelarecebimento ValorParcelaRecebimento = new valorparcelarecebimento();
        public quantidadepapel QuantidadePapel = new quantidadepapel();
        public valorpapelunitario ValorPapelUnitario = new valorpapelunitario();
        public tipoimpressao TipoImpressao = new tipoimpressao();
        public tipoingresso TipoIngresso = new tipoingresso();
        public tipocobrancapapel TipoCobrancaPapel = new tipocobrancapapel();
        public tipocanal TipoCanal = new tipocanal();
        public contaid ContaID = new contaid();

        public Repasse_B() { }

        // passar o Usuario logado no sistema
        public Repasse_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Repasse
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tRepasse WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Tipo.ValorBD = bd.LerInt("Tipo").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.DataAberturaCaixa.ValorBD = bd.LerString("DataAberturaCaixa");
                    this.DataFechamentoCaixa.ValorBD = bd.LerString("DataFechamentoCaixa");
                    this.EmpresaIDCaixa.ValorBD = bd.LerInt("EmpresaIDCaixa").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.EmpresaIDEvento.ValorBD = bd.LerInt("EmpresaIDEvento").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.PrecoValor.ValorBD = bd.LerDecimal("PrecoValor").ToString();
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaConvenienciaValor.ValorBD = bd.LerDecimal("TaxaConvenienciaValor").ToString();
                    this.ReterComissao.ValorBD = bd.LerString("ReterComissao");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();
                    this.ComissaoValor.ValorBD = bd.LerDecimal("ComissaoValor").ToString();
                    this.TaxaEntregaID.ValorBD = bd.LerInt("TaxaEntregaID").ToString();
                    this.TaxaEntregaValor.ValorBD = bd.LerDecimal("TaxaEntregaValor").ToString();
                    this.ValorTotal.ValorBD = bd.LerDecimal("ValorTotal").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.BandeiraID.ValorBD = bd.LerInt("BandeiraID").ToString();
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.PctFormaPagamento.ValorBD = bd.LerInt("PctFormaPagamento").ToString();
                    this.ValorFormaPagamento.ValorBD = bd.LerDecimal("ValorFormaPagamento").ToString();
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.ParcelaAtual.ValorBD = bd.LerInt("ParcelaAtual").ToString();
                    this.PrazoDias.ValorBD = bd.LerInt("PrazoDias").ToString();
                    this.ValorParcela.ValorBD = bd.LerDecimal("ValorParcela").ToString();
                    this.DataTransacao.ValorBD = bd.LerString("DataTransacao");
                    this.DataRecebimento.ValorBD = bd.LerString("DataRecebimento");
                    this.DiasRepasse.ValorBD = bd.LerInt("DiasRepasse").ToString();
                    this.DataCompetencia.ValorBD = bd.LerString("DataCompetencia");
                    this.DataRegimeCaixa.ValorBD = bd.LerString("DataRegimeCaixa");
                    this.Observacao.ValorBD = bd.LerString("Observacao");
                    this.TaxaADM.ValorBD = bd.LerDecimal("TaxaADM").ToString();
                    this.IR.ValorBD = bd.LerString("IR");
                    this.ValorParcelaRecebimento.ValorBD = bd.LerDecimal("ValorParcelaRecebimento").ToString();
                    this.QuantidadePapel.ValorBD = bd.LerInt("QuantidadePapel").ToString();
                    this.ValorPapelUnitario.ValorBD = bd.LerDecimal("ValorPapelUnitario").ToString();
                    this.TipoCanal.ValorBD = bd.LerInt("TipoCanal").ToString();
                    this.ContaID.ValorBD = bd.LerInt("ContaID").ToString();
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
        /// Preenche todos os atributos de Repasse do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xRepasse WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Tipo.ValorBD = bd.LerInt("Tipo").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.DataAberturaCaixa.ValorBD = bd.LerString("DataAberturaCaixa");
                    this.DataFechamentoCaixa.ValorBD = bd.LerString("DataFechamentoCaixa");
                    this.EmpresaIDCaixa.ValorBD = bd.LerInt("EmpresaIDCaixa").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.EmpresaIDEvento.ValorBD = bd.LerInt("EmpresaIDEvento").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.PrecoValor.ValorBD = bd.LerDecimal("PrecoValor").ToString();
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaConvenienciaValor.ValorBD = bd.LerDecimal("TaxaConvenienciaValor").ToString();
                    this.ReterComissao.ValorBD = bd.LerString("ReterComissao");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();
                    this.ComissaoValor.ValorBD = bd.LerDecimal("ComissaoValor").ToString();
                    this.TaxaEntregaID.ValorBD = bd.LerInt("TaxaEntregaID").ToString();
                    this.TaxaEntregaValor.ValorBD = bd.LerDecimal("TaxaEntregaValor").ToString();
                    this.ValorTotal.ValorBD = bd.LerDecimal("ValorTotal").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.BandeiraID.ValorBD = bd.LerInt("BandeiraID").ToString();
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.PctFormaPagamento.ValorBD = bd.LerInt("PctFormaPagamento").ToString();
                    this.ValorFormaPagamento.ValorBD = bd.LerDecimal("ValorFormaPagamento").ToString();
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.ParcelaAtual.ValorBD = bd.LerInt("ParcelaAtual").ToString();
                    this.PrazoDias.ValorBD = bd.LerInt("PrazoDias").ToString();
                    this.ValorParcela.ValorBD = bd.LerDecimal("ValorParcela").ToString();
                    this.DataTransacao.ValorBD = bd.LerString("DataTransacao");
                    this.DataRecebimento.ValorBD = bd.LerString("DataRecebimento");
                    this.DiasRepasse.ValorBD = bd.LerInt("DiasRepasse").ToString();
                    this.DataCompetencia.ValorBD = bd.LerString("DataCompetencia");
                    this.DataRegimeCaixa.ValorBD = bd.LerString("DataRegimeCaixa");
                    this.Observacao.ValorBD = bd.LerString("Observacao");
                    this.TaxaADM.ValorBD = bd.LerDecimal("TaxaADM").ToString();
                    this.IR.ValorBD = bd.LerString("IR");
                    this.ValorParcelaRecebimento.ValorBD = bd.LerDecimal("ValorParcelaRecebimento").ToString();
                    this.QuantidadePapel.ValorBD = bd.LerInt("QuantidadePapel").ToString();
                    this.ValorPapelUnitario.ValorBD = bd.LerDecimal("ValorPapelUnitario").ToString();
                    this.TipoCanal.ValorBD = bd.LerInt("TipoCanal").ToString();
                    this.ContaID.ValorBD = bd.LerInt("ContaID").ToString();
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

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xRepasse (ID, Versao, Tipo, ContratoID, VendaBilheteriaID, DataAberturaCaixa, DataFechamentoCaixa, EmpresaIDCaixa, CanalID, LojaID, UsuarioID, CaixaID, EmpresaIDEvento, LocalID, EventoID, ApresentacaoID, SetorID, PrecoID, PrecoValor, TaxaConveniencia, TaxaConvenienciaValor, ReterComissao, Comissao, ComissaoValor, TaxaEntregaID, TaxaEntregaValor, ValorTotal, TimeStamp, BandeiraID, FormaPagamentoID, PctFormaPagamento, ValorFormaPagamento, Parcelas, ParcelaAtual, PrazoDias, ValorParcela, DataTransacao, DataRecebimento, DiasRepasse, DataCompetencia, DataRegimeCaixa, Observacao, TaxaADM, IR, ValorParcelaRecebimento, QuantidadePapel, ValorPapelUnitario, TipoImpressao, TipoIngresso, TipoCobrancaPapel, TipoCanal, ContaID) ");
                sql.Append("SELECT ID, @V, Tipo, ContratoID, VendaBilheteriaID, DataAberturaCaixa, DataFechamentoCaixa, EmpresaIDCaixa, CanalID, LojaID, UsuarioID, CaixaID, EmpresaIDEvento, LocalID, EventoID, ApresentacaoID, SetorID, PrecoID, PrecoValor, TaxaConveniencia, TaxaConvenienciaValor, ReterComissao, Comissao, ComissaoValor, TaxaEntregaID, TaxaEntregaValor, ValorTotal, TimeStamp, BandeiraID, FormaPagamentoID, PctFormaPagamento, ValorFormaPagamento, Parcelas, ParcelaAtual, PrazoDias, ValorParcela, DataTransacao, DataRecebimento, DiasRepasse, DataCompetencia, DataRegimeCaixa, Observacao, TaxaADM, IR, ValorParcelaRecebimento, QuantidadePapel, ValorPapelUnitario, TipoImpressao, TipoIngresso, TipoCobrancaPapel, TipoCanal, ContaID FROM tRepasse WHERE ID = @I");
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
        /// Inserir novo(a) Repasse
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cRepasse");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tRepasse(ID, Tipo, ContratoID, VendaBilheteriaID, DataAberturaCaixa, DataFechamentoCaixa, EmpresaIDCaixa, CanalID, LojaID, UsuarioID, CaixaID, EmpresaIDEvento, LocalID, EventoID, ApresentacaoID, SetorID, PrecoID, PrecoValor, TaxaConveniencia, TaxaConvenienciaValor, ReterComissao, Comissao, ComissaoValor, TaxaEntregaID, TaxaEntregaValor, ValorTotal, TimeStamp, BandeiraID, FormaPagamentoID, PctFormaPagamento, ValorFormaPagamento, Parcelas, ParcelaAtual, PrazoDias, ValorParcela, DataTransacao, DataRecebimento, DiasRepasse, DataCompetencia, DataRegimeCaixa, Observacao, TaxaADM, IR, ValorParcelaRecebimento, QuantidadePapel, ValorPapelUnitario, TipoImpressao, TipoIngresso, TipoCobrancaPapel, TipoCanal, ContaID) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005',@006,@007,@008,@009,@010,@011,@012,@013,@014,@015,@016,'@017',@018,'@019','@020',@021,'@022',@023,'@024','@025','@026',@027,@028,@029,'@030',@031,@032,@033,'@034','@035','@036',@037,'@038','@039','@040','@041','@042','@043',@044,'@045','@046','@047','@048',@049,@050)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Tipo.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@004", this.DataAberturaCaixa.ValorBD);
                sql.Replace("@005", this.DataFechamentoCaixa.ValorBD);
                sql.Replace("@006", this.EmpresaIDCaixa.ValorBD);
                sql.Replace("@007", this.CanalID.ValorBD);
                sql.Replace("@008", this.LojaID.ValorBD);
                sql.Replace("@009", this.UsuarioID.ValorBD);
                sql.Replace("@010", this.CaixaID.ValorBD);
                sql.Replace("@011", this.EmpresaIDEvento.ValorBD);
                sql.Replace("@012", this.LocalID.ValorBD);
                sql.Replace("@013", this.EventoID.ValorBD);
                sql.Replace("@014", this.ApresentacaoID.ValorBD);
                sql.Replace("@015", this.SetorID.ValorBD);
                sql.Replace("@016", this.PrecoID.ValorBD);
                sql.Replace("@017", this.PrecoValor.ValorBD);
                sql.Replace("@018", this.TaxaConveniencia.ValorBD);
                sql.Replace("@019", this.TaxaConvenienciaValor.ValorBD);
                sql.Replace("@020", this.ReterComissao.ValorBD);
                sql.Replace("@021", this.Comissao.ValorBD);
                sql.Replace("@022", this.ComissaoValor.ValorBD);
                sql.Replace("@023", this.TaxaEntregaID.ValorBD);
                sql.Replace("@024", this.TaxaEntregaValor.ValorBD);
                sql.Replace("@025", this.ValorTotal.ValorBD);
                sql.Replace("@026", this.TimeStamp.ValorBD);
                sql.Replace("@027", this.BandeiraID.ValorBD);
                sql.Replace("@028", this.FormaPagamentoID.ValorBD);
                sql.Replace("@029", this.PctFormaPagamento.ValorBD);
                sql.Replace("@030", this.ValorFormaPagamento.ValorBD);
                sql.Replace("@031", this.Parcelas.ValorBD);
                sql.Replace("@032", this.ParcelaAtual.ValorBD);
                sql.Replace("@033", this.PrazoDias.ValorBD);
                sql.Replace("@034", this.ValorParcela.ValorBD);
                sql.Replace("@035", this.DataTransacao.ValorBD);
                sql.Replace("@036", this.DataRecebimento.ValorBD);
                sql.Replace("@037", this.DiasRepasse.ValorBD);
                sql.Replace("@038", this.DataCompetencia.ValorBD);
                sql.Replace("@039", this.DataRegimeCaixa.ValorBD);
                sql.Replace("@040", this.Observacao.ValorBD);
                sql.Replace("@041", this.TaxaADM.ValorBD);
                sql.Replace("@042", this.IR.ValorBD);
                sql.Replace("@043", this.ValorParcelaRecebimento.ValorBD);
                sql.Replace("@044", this.QuantidadePapel.ValorBD);
                sql.Replace("@045", this.ValorPapelUnitario.ValorBD);
                sql.Replace("@046", this.TipoImpressao.ValorBD);
                sql.Replace("@047", this.TipoIngresso.ValorBD);
                sql.Replace("@048", this.TipoCobrancaPapel.ValorBD);
                sql.Replace("@049", this.TipoCanal.ValorBD);
                sql.Replace("@050", this.ContaID.ValorBD);

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
        /// Atualiza Repasse
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cRepasse WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tRepasse SET Tipo = @001, ContratoID = @002, VendaBilheteriaID = @003, DataAberturaCaixa = '@004', DataFechamentoCaixa = '@005', EmpresaIDCaixa = @006, CanalID = @007, LojaID = @008, UsuarioID = @009, CaixaID = @010, EmpresaIDEvento = @011, LocalID = @012, EventoID = @013, ApresentacaoID = @014, SetorID = @015, PrecoID = @016, PrecoValor = '@017', TaxaConveniencia = @018, TaxaConvenienciaValor = '@019', ReterComissao = '@020', Comissao = @021, ComissaoValor = '@022', TaxaEntregaID = @023, TaxaEntregaValor = '@024', ValorTotal = '@025', TimeStamp = '@026', BandeiraID = @027, FormaPagamentoID = @028, PctFormaPagamento = @029, ValorFormaPagamento = '@030', Parcelas = @031, ParcelaAtual = @032, PrazoDias = @033, ValorParcela = '@034', DataTransacao = '@035', DataRecebimento = '@036', DiasRepasse = @037, DataCompetencia = '@038', DataRegimeCaixa = '@039', Observacao = '@040', TaxaADM = '@041', IR = '@042', ValorParcelaRecebimento = '@043', QuantidadePapel = @044, ValorPapelUnitario = '@045', TipoImpressao = '@046', TipoIngresso = '@047', TipoCobrancaPapel = '@048', TipoCanal = @049, ContaID = @050 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Tipo.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@004", this.DataAberturaCaixa.ValorBD);
                sql.Replace("@005", this.DataFechamentoCaixa.ValorBD);
                sql.Replace("@006", this.EmpresaIDCaixa.ValorBD);
                sql.Replace("@007", this.CanalID.ValorBD);
                sql.Replace("@008", this.LojaID.ValorBD);
                sql.Replace("@009", this.UsuarioID.ValorBD);
                sql.Replace("@010", this.CaixaID.ValorBD);
                sql.Replace("@011", this.EmpresaIDEvento.ValorBD);
                sql.Replace("@012", this.LocalID.ValorBD);
                sql.Replace("@013", this.EventoID.ValorBD);
                sql.Replace("@014", this.ApresentacaoID.ValorBD);
                sql.Replace("@015", this.SetorID.ValorBD);
                sql.Replace("@016", this.PrecoID.ValorBD);
                sql.Replace("@017", this.PrecoValor.ValorBD);
                sql.Replace("@018", this.TaxaConveniencia.ValorBD);
                sql.Replace("@019", this.TaxaConvenienciaValor.ValorBD);
                sql.Replace("@020", this.ReterComissao.ValorBD);
                sql.Replace("@021", this.Comissao.ValorBD);
                sql.Replace("@022", this.ComissaoValor.ValorBD);
                sql.Replace("@023", this.TaxaEntregaID.ValorBD);
                sql.Replace("@024", this.TaxaEntregaValor.ValorBD);
                sql.Replace("@025", this.ValorTotal.ValorBD);
                sql.Replace("@026", this.TimeStamp.ValorBD);
                sql.Replace("@027", this.BandeiraID.ValorBD);
                sql.Replace("@028", this.FormaPagamentoID.ValorBD);
                sql.Replace("@029", this.PctFormaPagamento.ValorBD);
                sql.Replace("@030", this.ValorFormaPagamento.ValorBD);
                sql.Replace("@031", this.Parcelas.ValorBD);
                sql.Replace("@032", this.ParcelaAtual.ValorBD);
                sql.Replace("@033", this.PrazoDias.ValorBD);
                sql.Replace("@034", this.ValorParcela.ValorBD);
                sql.Replace("@035", this.DataTransacao.ValorBD);
                sql.Replace("@036", this.DataRecebimento.ValorBD);
                sql.Replace("@037", this.DiasRepasse.ValorBD);
                sql.Replace("@038", this.DataCompetencia.ValorBD);
                sql.Replace("@039", this.DataRegimeCaixa.ValorBD);
                sql.Replace("@040", this.Observacao.ValorBD);
                sql.Replace("@041", this.TaxaADM.ValorBD);
                sql.Replace("@042", this.IR.ValorBD);
                sql.Replace("@043", this.ValorParcelaRecebimento.ValorBD);
                sql.Replace("@044", this.QuantidadePapel.ValorBD);
                sql.Replace("@045", this.ValorPapelUnitario.ValorBD);
                sql.Replace("@046", this.TipoImpressao.ValorBD);
                sql.Replace("@047", this.TipoIngresso.ValorBD);
                sql.Replace("@048", this.TipoCobrancaPapel.ValorBD);
                sql.Replace("@049", this.TipoCanal.ValorBD);
                sql.Replace("@050", this.ContaID.ValorBD);

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
        /// Exclui Repasse com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cRepasse WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tRepasse WHERE ID=" + id;

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
        /// Exclui Repasse
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

            this.Tipo.Limpar();
            this.ContratoID.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.DataAberturaCaixa.Limpar();
            this.DataFechamentoCaixa.Limpar();
            this.EmpresaIDCaixa.Limpar();
            this.CanalID.Limpar();
            this.LojaID.Limpar();
            this.UsuarioID.Limpar();
            this.CaixaID.Limpar();
            this.EmpresaIDEvento.Limpar();
            this.LocalID.Limpar();
            this.EventoID.Limpar();
            this.ApresentacaoID.Limpar();
            this.SetorID.Limpar();
            this.PrecoID.Limpar();
            this.PrecoValor.Limpar();
            this.TaxaConveniencia.Limpar();
            this.TaxaConvenienciaValor.Limpar();
            this.ReterComissao.Limpar();
            this.Comissao.Limpar();
            this.ComissaoValor.Limpar();
            this.TaxaEntregaID.Limpar();
            this.TaxaEntregaValor.Limpar();
            this.ValorTotal.Limpar();
            this.TimeStamp.Limpar();
            this.BandeiraID.Limpar();
            this.FormaPagamentoID.Limpar();
            this.PctFormaPagamento.Limpar();
            this.ValorFormaPagamento.Limpar();
            this.Parcelas.Limpar();
            this.ParcelaAtual.Limpar();
            this.PrazoDias.Limpar();
            this.ValorParcela.Limpar();
            this.DataTransacao.Limpar();
            this.DataRecebimento.Limpar();
            this.DiasRepasse.Limpar();
            this.DataCompetencia.Limpar();
            this.DataRegimeCaixa.Limpar();
            this.Observacao.Limpar();
            this.TaxaADM.Limpar();
            this.IR.Limpar();
            this.ValorParcelaRecebimento.Limpar();
            this.QuantidadePapel.Limpar();
            this.ValorPapelUnitario.Limpar();
            this.TipoImpressao.Limpar();
            this.TipoIngresso.Limpar();
            this.TipoCobrancaPapel.Limpar();
            this.TipoCanal.Limpar();
            this.ContaID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Tipo.Desfazer();
            this.ContratoID.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.DataAberturaCaixa.Desfazer();
            this.DataFechamentoCaixa.Desfazer();
            this.EmpresaIDCaixa.Desfazer();
            this.CanalID.Desfazer();
            this.LojaID.Desfazer();
            this.UsuarioID.Desfazer();
            this.CaixaID.Desfazer();
            this.EmpresaIDEvento.Desfazer();
            this.LocalID.Desfazer();
            this.EventoID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.SetorID.Desfazer();
            this.PrecoID.Desfazer();
            this.PrecoValor.Desfazer();
            this.TaxaConveniencia.Desfazer();
            this.TaxaConvenienciaValor.Desfazer();
            this.ReterComissao.Desfazer();
            this.Comissao.Desfazer();
            this.ComissaoValor.Desfazer();
            this.TaxaEntregaID.Desfazer();
            this.TaxaEntregaValor.Desfazer();
            this.ValorTotal.Desfazer();
            this.TimeStamp.Desfazer();
            this.BandeiraID.Desfazer();
            this.FormaPagamentoID.Desfazer();
            this.PctFormaPagamento.Desfazer();
            this.ValorFormaPagamento.Desfazer();
            this.Parcelas.Desfazer();
            this.ParcelaAtual.Desfazer();
            this.PrazoDias.Desfazer();
            this.ValorParcela.Desfazer();
            this.DataTransacao.Desfazer();
            this.DataRecebimento.Desfazer();
            this.DiasRepasse.Desfazer();
            this.DataCompetencia.Desfazer();
            this.DataRegimeCaixa.Desfazer();
            this.Observacao.Desfazer();
            this.TaxaADM.Desfazer();
            this.IR.Desfazer();
            this.ValorParcelaRecebimento.Desfazer();
            this.QuantidadePapel.Desfazer();
            this.ValorPapelUnitario.Desfazer();
            this.TipoImpressao.Desfazer();
            this.TipoIngresso.Desfazer();
            this.TipoCobrancaPapel.Desfazer();
            this.TipoCanal.Desfazer();
            this.ContaID.Desfazer();
        }

        public class tipo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Tipo";
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

        public class contratoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContratoID";
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

        public class dataaberturacaixa : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataAberturaCaixa";
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

        public class datafechamentocaixa : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataFechamentoCaixa";
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

        public class empresaidcaixa : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaIDCaixa";
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

        public class canalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalID";
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

        public class lojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaID";
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

        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
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

        public class caixaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CaixaID";
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

        public class empresaidevento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaIDEvento";
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

        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
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

        public class eventoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoID";
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

        public class apresentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoID";
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

        public class setorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SetorID";
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

        public class precoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoID";
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

        public class precovalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoValor";
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

        public class taxaconveniencia : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConveniencia";
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

        public class taxaconvenienciavalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConvenienciaValor";
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

        public class retercomissao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ReterComissao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

        public class comissao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Comissao";
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

        public class comissaovalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoValor";
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

        public class taxaentregaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaEntregaID";
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

        public class taxaentregavalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaEntregaValor";
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

        public class valortotal : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorTotal";
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

        public class timestamp : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TimeStamp";
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

        public class bandeiraid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BandeiraID";
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

        public class pctformapagamento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PctFormaPagamento";
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

        public class valorformapagamento : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorFormaPagamento";
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

        public class parcelas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Parcelas";
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

        public class parcelaatual : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ParcelaAtual";
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

        public class prazodias : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrazoDias";
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

        public class valorparcela : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorParcela";
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

        public class datatransacao : DateTimeProperty
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

        public class datarecebimento : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataRecebimento";
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

        public class diasrepasse : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DiasRepasse";
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

        public class datacompetencia : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataCompetencia";
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

        public class dataregimecaixa : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataRegimeCaixa";
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

        public class observacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Observacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 500;
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

        public class taxaadm : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaADM";
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
                    return 0;
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

        public class valorparcelarecebimento : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorParcelaRecebimento";
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

        public class quantidadepapel : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadePapel";
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

        public class valorpapelunitario : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorPapelUnitario";
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

        public class tipoimpressao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoImpressao";
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

        public class tipoingresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoIngresso";
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

        public class tipocobrancapapel : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCobrancaPapel";
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

        public class tipocanal : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCanal";
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

        public class contaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContaID";
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

                DataTable tabela = new DataTable("Repasse");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Tipo", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("DataAberturaCaixa", typeof(DateTime));
                tabela.Columns.Add("DataFechamentoCaixa", typeof(DateTime));
                tabela.Columns.Add("EmpresaIDCaixa", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CaixaID", typeof(int));
                tabela.Columns.Add("EmpresaIDEvento", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("PrecoValor", typeof(decimal));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
                tabela.Columns.Add("ReterComissao", typeof(bool));
                tabela.Columns.Add("Comissao", typeof(int));
                tabela.Columns.Add("ComissaoValor", typeof(decimal));
                tabela.Columns.Add("TaxaEntregaID", typeof(int));
                tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));
                tabela.Columns.Add("ValorTotal", typeof(decimal));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("BandeiraID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("PctFormaPagamento", typeof(int));
                tabela.Columns.Add("ValorFormaPagamento", typeof(decimal));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("ParcelaAtual", typeof(int));
                tabela.Columns.Add("PrazoDias", typeof(int));
                tabela.Columns.Add("ValorParcela", typeof(decimal));
                tabela.Columns.Add("DataTransacao", typeof(DateTime));
                tabela.Columns.Add("DataRecebimento", typeof(DateTime));
                tabela.Columns.Add("DiasRepasse", typeof(int));
                tabela.Columns.Add("DataCompetencia", typeof(DateTime));
                tabela.Columns.Add("DataRegimeCaixa", typeof(DateTime));
                tabela.Columns.Add("Observacao", typeof(string));
                tabela.Columns.Add("TaxaADM", typeof(decimal));
                tabela.Columns.Add("IR", typeof(bool));
                tabela.Columns.Add("ValorParcelaRecebimento", typeof(decimal));
                tabela.Columns.Add("QuantidadePapel", typeof(int));
                tabela.Columns.Add("ValorPapelUnitario", typeof(decimal));
                tabela.Columns.Add("TipoImpressao", typeof(int));
                tabela.Columns.Add("TipoIngresso", typeof(int));
                tabela.Columns.Add("TipoCobrancaPapel", typeof(int));
                tabela.Columns.Add("TipoCanal", typeof(int));
                tabela.Columns.Add("ContaID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "RepasseLista_B"

    public abstract class RepasseLista_B : BaseLista
    {

        private bool backup = false;
        protected Repasse repasse;

        // passar o Usuario logado no sistema
        public RepasseLista_B()
        {
            repasse = new Repasse();
        }

        // passar o Usuario logado no sistema
        public RepasseLista_B(int usuarioIDLogado)
        {
            repasse = new Repasse(usuarioIDLogado);
        }

        public Repasse Repasse
        {
            get { return repasse; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Repasse especifico
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
                    repasse.Ler(id);
                    return repasse;
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
                    sql = "SELECT ID FROM tRepasse";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRepasse";

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
                    sql = "SELECT ID FROM tRepasse";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRepasse";

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
                    sql = "SELECT ID FROM xRepasse";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xRepasse";

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
        /// Preenche Repasse corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    repasse.Ler(id);
                else
                    repasse.LerBackup(id);

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

                bool ok = repasse.Excluir();
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
        /// Inseri novo(a) Repasse na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = repasse.Inserir();
                if (ok)
                {
                    lista.Add(repasse.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Repasse carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Repasse");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Tipo", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("DataAberturaCaixa", typeof(DateTime));
                tabela.Columns.Add("DataFechamentoCaixa", typeof(DateTime));
                tabela.Columns.Add("EmpresaIDCaixa", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CaixaID", typeof(int));
                tabela.Columns.Add("EmpresaIDEvento", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("PrecoValor", typeof(decimal));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
                tabela.Columns.Add("ReterComissao", typeof(bool));
                tabela.Columns.Add("Comissao", typeof(int));
                tabela.Columns.Add("ComissaoValor", typeof(decimal));
                tabela.Columns.Add("TaxaEntregaID", typeof(int));
                tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));
                tabela.Columns.Add("ValorTotal", typeof(decimal));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("BandeiraID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("PctFormaPagamento", typeof(int));
                tabela.Columns.Add("ValorFormaPagamento", typeof(decimal));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("ParcelaAtual", typeof(int));
                tabela.Columns.Add("PrazoDias", typeof(int));
                tabela.Columns.Add("ValorParcela", typeof(decimal));
                tabela.Columns.Add("DataTransacao", typeof(DateTime));
                tabela.Columns.Add("DataRecebimento", typeof(DateTime));
                tabela.Columns.Add("DiasRepasse", typeof(int));
                tabela.Columns.Add("DataCompetencia", typeof(DateTime));
                tabela.Columns.Add("DataRegimeCaixa", typeof(DateTime));
                tabela.Columns.Add("Observacao", typeof(string));
                tabela.Columns.Add("TaxaADM", typeof(decimal));
                tabela.Columns.Add("IR", typeof(bool));
                tabela.Columns.Add("ValorParcelaRecebimento", typeof(decimal));
                tabela.Columns.Add("QuantidadePapel", typeof(int));
                tabela.Columns.Add("ValorPapelUnitario", typeof(decimal));
                tabela.Columns.Add("TipoImpressao", typeof(int));
                tabela.Columns.Add("TipoIngresso", typeof(int));
                tabela.Columns.Add("TipoCobrancaPapel", typeof(int));
                tabela.Columns.Add("TipoCanal", typeof(int));
                tabela.Columns.Add("ContaID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = repasse.Control.ID;
                        linha["Tipo"] = repasse.Tipo.Valor;
                        linha["ContratoID"] = repasse.ContratoID.Valor;
                        linha["VendaBilheteriaID"] = repasse.VendaBilheteriaID.Valor;
                        linha["DataAberturaCaixa"] = repasse.DataAberturaCaixa.Valor;
                        linha["DataFechamentoCaixa"] = repasse.DataFechamentoCaixa.Valor;
                        linha["EmpresaIDCaixa"] = repasse.EmpresaIDCaixa.Valor;
                        linha["CanalID"] = repasse.CanalID.Valor;
                        linha["LojaID"] = repasse.LojaID.Valor;
                        linha["UsuarioID"] = repasse.UsuarioID.Valor;
                        linha["CaixaID"] = repasse.CaixaID.Valor;
                        linha["EmpresaIDEvento"] = repasse.EmpresaIDEvento.Valor;
                        linha["LocalID"] = repasse.LocalID.Valor;
                        linha["EventoID"] = repasse.EventoID.Valor;
                        linha["ApresentacaoID"] = repasse.ApresentacaoID.Valor;
                        linha["SetorID"] = repasse.SetorID.Valor;
                        linha["PrecoID"] = repasse.PrecoID.Valor;
                        linha["PrecoValor"] = repasse.PrecoValor.Valor;
                        linha["TaxaConveniencia"] = repasse.TaxaConveniencia.Valor;
                        linha["TaxaConvenienciaValor"] = repasse.TaxaConvenienciaValor.Valor;
                        linha["ReterComissao"] = repasse.ReterComissao.Valor;
                        linha["Comissao"] = repasse.Comissao.Valor;
                        linha["ComissaoValor"] = repasse.ComissaoValor.Valor;
                        linha["TaxaEntregaID"] = repasse.TaxaEntregaID.Valor;
                        linha["TaxaEntregaValor"] = repasse.TaxaEntregaValor.Valor;
                        linha["ValorTotal"] = repasse.ValorTotal.Valor;
                        linha["TimeStamp"] = repasse.TimeStamp.Valor;
                        linha["BandeiraID"] = repasse.BandeiraID.Valor;
                        linha["FormaPagamentoID"] = repasse.FormaPagamentoID.Valor;
                        linha["PctFormaPagamento"] = repasse.PctFormaPagamento.Valor;
                        linha["ValorFormaPagamento"] = repasse.ValorFormaPagamento.Valor;
                        linha["Parcelas"] = repasse.Parcelas.Valor;
                        linha["ParcelaAtual"] = repasse.ParcelaAtual.Valor;
                        linha["PrazoDias"] = repasse.PrazoDias.Valor;
                        linha["ValorParcela"] = repasse.ValorParcela.Valor;
                        linha["DataTransacao"] = repasse.DataTransacao.Valor;
                        linha["DataRecebimento"] = repasse.DataRecebimento.Valor;
                        linha["DiasRepasse"] = repasse.DiasRepasse.Valor;
                        linha["DataCompetencia"] = repasse.DataCompetencia.Valor;
                        linha["DataRegimeCaixa"] = repasse.DataRegimeCaixa.Valor;
                        linha["Observacao"] = repasse.Observacao.Valor;
                        linha["TaxaADM"] = repasse.TaxaADM.Valor;
                        linha["IR"] = repasse.IR.Valor;
                        linha["ValorParcelaRecebimento"] = repasse.ValorParcelaRecebimento.Valor;
                        linha["QuantidadePapel"] = repasse.QuantidadePapel.Valor;
                        linha["ValorPapelUnitario"] = repasse.ValorPapelUnitario.Valor;
                        linha["TipoImpressao"] = repasse.TipoImpressao.Valor;
                        linha["TipoIngresso"] = repasse.TipoIngresso.Valor;
                        linha["TipoCobrancaPapel"] = repasse.TipoCobrancaPapel.Valor;
                        linha["TipoCanal"] = repasse.TipoCanal.Valor;
                        linha["ContaID"] = repasse.ContaID.Valor;
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

                DataTable tabela = new DataTable("RelatorioRepasse");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Tipo", typeof(int));
                    tabela.Columns.Add("ContratoID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("DataAberturaCaixa", typeof(DateTime));
                    tabela.Columns.Add("DataFechamentoCaixa", typeof(DateTime));
                    tabela.Columns.Add("EmpresaIDCaixa", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("LojaID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("CaixaID", typeof(int));
                    tabela.Columns.Add("EmpresaIDEvento", typeof(int));
                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("PrecoID", typeof(int));
                    tabela.Columns.Add("PrecoValor", typeof(decimal));
                    tabela.Columns.Add("TaxaConveniencia", typeof(int));
                    tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
                    tabela.Columns.Add("ReterComissao", typeof(bool));
                    tabela.Columns.Add("Comissao", typeof(int));
                    tabela.Columns.Add("ComissaoValor", typeof(decimal));
                    tabela.Columns.Add("TaxaEntregaID", typeof(int));
                    tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));
                    tabela.Columns.Add("ValorTotal", typeof(decimal));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("BandeiraID", typeof(int));
                    tabela.Columns.Add("FormaPagamentoID", typeof(int));
                    tabela.Columns.Add("PctFormaPagamento", typeof(int));
                    tabela.Columns.Add("ValorFormaPagamento", typeof(decimal));
                    tabela.Columns.Add("Parcelas", typeof(int));
                    tabela.Columns.Add("ParcelaAtual", typeof(int));
                    tabela.Columns.Add("PrazoDias", typeof(int));
                    tabela.Columns.Add("ValorParcela", typeof(decimal));
                    tabela.Columns.Add("DataTransacao", typeof(DateTime));
                    tabela.Columns.Add("DataRecebimento", typeof(DateTime));
                    tabela.Columns.Add("DiasRepasse", typeof(int));
                    tabela.Columns.Add("DataCompetencia", typeof(DateTime));
                    tabela.Columns.Add("DataRegimeCaixa", typeof(DateTime));
                    tabela.Columns.Add("Observacao", typeof(string));
                    tabela.Columns.Add("TaxaADM", typeof(decimal));
                    tabela.Columns.Add("IR", typeof(bool));
                    tabela.Columns.Add("ValorParcelaRecebimento", typeof(decimal));
                    tabela.Columns.Add("QuantidadePapel", typeof(int));
                    tabela.Columns.Add("ValorPapelUnitario", typeof(decimal));
                    tabela.Columns.Add("TipoImpressao", typeof(int));
                    tabela.Columns.Add("TipoIngresso", typeof(int));
                    tabela.Columns.Add("TipoCobrancaPapel", typeof(int));
                    tabela.Columns.Add("TipoCanal", typeof(int));
                    tabela.Columns.Add("ContaID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Tipo"] = repasse.Tipo.Valor;
                        linha["ContratoID"] = repasse.ContratoID.Valor;
                        linha["VendaBilheteriaID"] = repasse.VendaBilheteriaID.Valor;
                        linha["DataAberturaCaixa"] = repasse.DataAberturaCaixa.Valor;
                        linha["DataFechamentoCaixa"] = repasse.DataFechamentoCaixa.Valor;
                        linha["EmpresaIDCaixa"] = repasse.EmpresaIDCaixa.Valor;
                        linha["CanalID"] = repasse.CanalID.Valor;
                        linha["LojaID"] = repasse.LojaID.Valor;
                        linha["UsuarioID"] = repasse.UsuarioID.Valor;
                        linha["CaixaID"] = repasse.CaixaID.Valor;
                        linha["EmpresaIDEvento"] = repasse.EmpresaIDEvento.Valor;
                        linha["LocalID"] = repasse.LocalID.Valor;
                        linha["EventoID"] = repasse.EventoID.Valor;
                        linha["ApresentacaoID"] = repasse.ApresentacaoID.Valor;
                        linha["SetorID"] = repasse.SetorID.Valor;
                        linha["PrecoID"] = repasse.PrecoID.Valor;
                        linha["PrecoValor"] = repasse.PrecoValor.Valor;
                        linha["TaxaConveniencia"] = repasse.TaxaConveniencia.Valor;
                        linha["TaxaConvenienciaValor"] = repasse.TaxaConvenienciaValor.Valor;
                        linha["ReterComissao"] = repasse.ReterComissao.Valor;
                        linha["Comissao"] = repasse.Comissao.Valor;
                        linha["ComissaoValor"] = repasse.ComissaoValor.Valor;
                        linha["TaxaEntregaID"] = repasse.TaxaEntregaID.Valor;
                        linha["TaxaEntregaValor"] = repasse.TaxaEntregaValor.Valor;
                        linha["ValorTotal"] = repasse.ValorTotal.Valor;
                        linha["TimeStamp"] = repasse.TimeStamp.Valor;
                        linha["BandeiraID"] = repasse.BandeiraID.Valor;
                        linha["FormaPagamentoID"] = repasse.FormaPagamentoID.Valor;
                        linha["PctFormaPagamento"] = repasse.PctFormaPagamento.Valor;
                        linha["ValorFormaPagamento"] = repasse.ValorFormaPagamento.Valor;
                        linha["Parcelas"] = repasse.Parcelas.Valor;
                        linha["ParcelaAtual"] = repasse.ParcelaAtual.Valor;
                        linha["PrazoDias"] = repasse.PrazoDias.Valor;
                        linha["ValorParcela"] = repasse.ValorParcela.Valor;
                        linha["DataTransacao"] = repasse.DataTransacao.Valor;
                        linha["DataRecebimento"] = repasse.DataRecebimento.Valor;
                        linha["DiasRepasse"] = repasse.DiasRepasse.Valor;
                        linha["DataCompetencia"] = repasse.DataCompetencia.Valor;
                        linha["DataRegimeCaixa"] = repasse.DataRegimeCaixa.Valor;
                        linha["Observacao"] = repasse.Observacao.Valor;
                        linha["TaxaADM"] = repasse.TaxaADM.Valor;
                        linha["IR"] = repasse.IR.Valor;
                        linha["ValorParcelaRecebimento"] = repasse.ValorParcelaRecebimento.Valor;
                        linha["QuantidadePapel"] = repasse.QuantidadePapel.Valor;
                        linha["ValorPapelUnitario"] = repasse.ValorPapelUnitario.Valor;
                        linha["TipoImpressao"] = repasse.TipoImpressao.Valor;
                        linha["TipoIngresso"] = repasse.TipoIngresso.Valor;
                        linha["TipoCobrancaPapel"] = repasse.TipoCobrancaPapel.Valor;
                        linha["TipoCanal"] = repasse.TipoCanal.Valor;
                        linha["ContaID"] = repasse.ContaID.Valor;
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
                    case "Tipo":
                        sql = "SELECT ID, Tipo FROM tRepasse WHERE " + FiltroSQL + " ORDER BY Tipo";
                        break;
                    case "ContratoID":
                        sql = "SELECT ID, ContratoID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ContratoID";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "DataAberturaCaixa":
                        sql = "SELECT ID, DataAberturaCaixa FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DataAberturaCaixa";
                        break;
                    case "DataFechamentoCaixa":
                        sql = "SELECT ID, DataFechamentoCaixa FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DataFechamentoCaixa";
                        break;
                    case "EmpresaIDCaixa":
                        sql = "SELECT ID, EmpresaIDCaixa FROM tRepasse WHERE " + FiltroSQL + " ORDER BY EmpresaIDCaixa";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "LojaID":
                        sql = "SELECT ID, LojaID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY LojaID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "CaixaID":
                        sql = "SELECT ID, CaixaID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY CaixaID";
                        break;
                    case "EmpresaIDEvento":
                        sql = "SELECT ID, EmpresaIDEvento FROM tRepasse WHERE " + FiltroSQL + " ORDER BY EmpresaIDEvento";
                        break;
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "PrecoID":
                        sql = "SELECT ID, PrecoID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY PrecoID";
                        break;
                    case "PrecoValor":
                        sql = "SELECT ID, PrecoValor FROM tRepasse WHERE " + FiltroSQL + " ORDER BY PrecoValor";
                        break;
                    case "TaxaConveniencia":
                        sql = "SELECT ID, TaxaConveniencia FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TaxaConveniencia";
                        break;
                    case "TaxaConvenienciaValor":
                        sql = "SELECT ID, TaxaConvenienciaValor FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TaxaConvenienciaValor";
                        break;
                    case "ReterComissao":
                        sql = "SELECT ID, ReterComissao FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ReterComissao";
                        break;
                    case "Comissao":
                        sql = "SELECT ID, Comissao FROM tRepasse WHERE " + FiltroSQL + " ORDER BY Comissao";
                        break;
                    case "ComissaoValor":
                        sql = "SELECT ID, ComissaoValor FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ComissaoValor";
                        break;
                    case "TaxaEntregaID":
                        sql = "SELECT ID, TaxaEntregaID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TaxaEntregaID";
                        break;
                    case "TaxaEntregaValor":
                        sql = "SELECT ID, TaxaEntregaValor FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TaxaEntregaValor";
                        break;
                    case "ValorTotal":
                        sql = "SELECT ID, ValorTotal FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ValorTotal";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "BandeiraID":
                        sql = "SELECT ID, BandeiraID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY BandeiraID";
                        break;
                    case "FormaPagamentoID":
                        sql = "SELECT ID, FormaPagamentoID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY FormaPagamentoID";
                        break;
                    case "PctFormaPagamento":
                        sql = "SELECT ID, PctFormaPagamento FROM tRepasse WHERE " + FiltroSQL + " ORDER BY PctFormaPagamento";
                        break;
                    case "ValorFormaPagamento":
                        sql = "SELECT ID, ValorFormaPagamento FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ValorFormaPagamento";
                        break;
                    case "Parcelas":
                        sql = "SELECT ID, Parcelas FROM tRepasse WHERE " + FiltroSQL + " ORDER BY Parcelas";
                        break;
                    case "ParcelaAtual":
                        sql = "SELECT ID, ParcelaAtual FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ParcelaAtual";
                        break;
                    case "PrazoDias":
                        sql = "SELECT ID, PrazoDias FROM tRepasse WHERE " + FiltroSQL + " ORDER BY PrazoDias";
                        break;
                    case "ValorParcela":
                        sql = "SELECT ID, ValorParcela FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ValorParcela";
                        break;
                    case "DataTransacao":
                        sql = "SELECT ID, DataTransacao FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DataTransacao";
                        break;
                    case "DataRecebimento":
                        sql = "SELECT ID, DataRecebimento FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DataRecebimento";
                        break;
                    case "DiasRepasse":
                        sql = "SELECT ID, DiasRepasse FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DiasRepasse";
                        break;
                    case "DataCompetencia":
                        sql = "SELECT ID, DataCompetencia FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DataCompetencia";
                        break;
                    case "DataRegimeCaixa":
                        sql = "SELECT ID, DataRegimeCaixa FROM tRepasse WHERE " + FiltroSQL + " ORDER BY DataRegimeCaixa";
                        break;
                    case "Observacao":
                        sql = "SELECT ID, Observacao FROM tRepasse WHERE " + FiltroSQL + " ORDER BY Observacao";
                        break;
                    case "TaxaADM":
                        sql = "SELECT ID, TaxaADM FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TaxaADM";
                        break;
                    case "IR":
                        sql = "SELECT ID, IR FROM tRepasse WHERE " + FiltroSQL + " ORDER BY IR";
                        break;
                    case "ValorParcelaRecebimento":
                        sql = "SELECT ID, ValorParcelaRecebimento FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ValorParcelaRecebimento";
                        break;
                    case "QuantidadePapel":
                        sql = "SELECT ID, QuantidadePapel FROM tRepasse WHERE " + FiltroSQL + " ORDER BY QuantidadePapel";
                        break;
                    case "ValorPapelUnitario":
                        sql = "SELECT ID, ValorPapelUnitario FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ValorPapelUnitario";
                        break;
                    case "TipoImpressao":
                        sql = "SELECT ID, TipoImpressao FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TipoImpressao";
                        break;
                    case "TipoIngresso":
                        sql = "SELECT ID, TipoIngresso FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TipoIngresso";
                        break;
                    case "TipoCobrancaPapel":
                        sql = "SELECT ID, TipoCobrancaPapel FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TipoCobrancaPapel";
                        break;
                    case "TipoCanal":
                        sql = "SELECT ID, TipoCanal FROM tRepasse WHERE " + FiltroSQL + " ORDER BY TipoCanal";
                        break;
                    case "ContaID":
                        sql = "SELECT ID, ContaID FROM tRepasse WHERE " + FiltroSQL + " ORDER BY ContaID";
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

    #region "RepasseException"

    [Serializable]
    public class RepasseException : Exception
    {

        public RepasseException() : base() { }

        public RepasseException(string msg) : base(msg) { }

        public RepasseException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}