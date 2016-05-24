

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "VendaBilheteria_B"

    public abstract class VendaBilheteria_B : BaseBD
    {


        public caixaid CaixaID = new caixaid();

        public clienteid ClienteID = new clienteid();

        public senha Senha = new senha();

        public datavenda DataVenda = new datavenda();

        public status Status = new status();

        public taxaentregaid TaxaEntregaID = new taxaentregaid();

        public taxaconvenienciavalortotal TaxaConvenienciaValorTotal = new taxaconvenienciavalortotal();

        public comissaovalortotal ComissaoValorTotal = new comissaovalortotal();

        public valortotal ValorTotal = new valortotal();

        public obs Obs = new obs();

        public notafiscalcliente NotaFiscalCliente = new notafiscalcliente();

        public notafiscalestabelecimento NotaFiscalEstabelecimento = new notafiscalestabelecimento();

        public indiceinstituicaotransacao IndiceInstituicaoTransacao = new indiceinstituicaotransacao();

        public indicetipocartao IndiceTipoCartao = new indicetipocartao();

        public nsusitef NSUSitef = new nsusitef();

        public nsuhost NSUHost = new nsuhost();

        public codigoautorizacaocredito CodigoAutorizacaoCredito = new codigoautorizacaocredito();

        public bin BIN = new bin();

        public modalidadepagamentocodigo ModalidadePagamentoCodigo = new modalidadepagamentocodigo();

        public modalidadepagamentotexto ModalidadePagamentoTexto = new modalidadepagamentotexto();

        public ddd DDD = new ddd();

        public numerocelular NumeroCelular = new numerocelular();

        public modelidcelular ModelIDCelular = new modelidcelular();

        public fabricantecelular FabricanteCelular = new fabricantecelular();

        public nivelrisco NivelRisco = new nivelrisco();

        public ip IP = new ip();

        public fraude Fraude = new fraude();

        public aprovacaoautomatica AprovacaoAutomatica = new aprovacaoautomatica();

        public quantidadeimpressoesinternet QuantidadeImpressoesInternet = new quantidadeimpressoesinternet();

        public vendacancelada VendaCancelada = new vendacancelada();

        public motivoid MotivoID = new motivoid();

        public clienteenderecoid ClienteEnderecoID = new clienteenderecoid();

        public entregacontroleid EntregaControleID = new entregacontroleid();

        public entregaagendaid EntregaAgendaID = new entregaagendaid();

        public pdvid PdvID = new pdvid();

        public taxaentregavalor TaxaEntregaValor = new taxaentregavalor();

        public pagamentoprocessado PagamentoProcessado = new pagamentoprocessado();

        public nomecartao NomeCartao = new nomecartao();

        public valorseguro ValorSeguro = new valorseguro();

        public taxaprocessamentovalor TaxaProcessamentoValor = new taxaprocessamentovalor();

        public taxaprocessamentocancelada TaxaProcessamentoCancelada = new taxaprocessamentocancelada();

        public score Score = new score();

        public retornoaccertify RetornoAccertify = new retornoaccertify();

        public accertifyforcestatus AccertifyForceStatus = new accertifyforcestatus();

        public vendabilhereriaidtroca VendaBilhereriaIDTroca = new vendabilhereriaidtroca();

        public codigorastreio CodigoRastreio = new codigorastreio();


        public VendaBilheteria_B() { }

        // passar o Usuario logado no sistema
        public VendaBilheteria_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteria
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteria WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();

                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();

                    this.Senha.ValorBD = bd.LerString("Senha");

                    this.DataVenda.ValorBD = bd.LerString("DataVenda");

                    this.Status.ValorBD = bd.LerString("Status");

                    this.TaxaEntregaID.ValorBD = bd.LerInt("TaxaEntregaID").ToString();

                    this.TaxaConvenienciaValorTotal.ValorBD = bd.LerDecimal("TaxaConvenienciaValorTotal").ToString();

                    this.ComissaoValorTotal.ValorBD = bd.LerDecimal("ComissaoValorTotal").ToString();

                    this.ValorTotal.ValorBD = bd.LerDecimal("ValorTotal").ToString();

                    this.Obs.ValorBD = bd.LerString("Obs");

                    this.NotaFiscalCliente.ValorBD = bd.LerString("NotaFiscalCliente");

                    this.NotaFiscalEstabelecimento.ValorBD = bd.LerString("NotaFiscalEstabelecimento");

                    this.IndiceInstituicaoTransacao.ValorBD = bd.LerInt("IndiceInstituicaoTransacao").ToString();

                    this.IndiceTipoCartao.ValorBD = bd.LerInt("IndiceTipoCartao").ToString();

                    this.NSUSitef.ValorBD = bd.LerInt("NSUSitef").ToString();

                    this.NSUHost.ValorBD = bd.LerInt("NSUHost").ToString();

                    this.CodigoAutorizacaoCredito.ValorBD = bd.LerInt("CodigoAutorizacaoCredito").ToString();

                    this.BIN.ValorBD = bd.LerInt("BIN").ToString();

                    this.ModalidadePagamentoCodigo.ValorBD = bd.LerInt("ModalidadePagamentoCodigo").ToString();

                    this.ModalidadePagamentoTexto.ValorBD = bd.LerString("ModalidadePagamentoTexto");

                    this.DDD.ValorBD = bd.LerInt("DDD").ToString();

                    this.NumeroCelular.ValorBD = bd.LerInt("NumeroCelular").ToString();

                    this.ModelIDCelular.ValorBD = bd.LerInt("ModelIDCelular").ToString();

                    this.FabricanteCelular.ValorBD = bd.LerString("FabricanteCelular");

                    this.NivelRisco.ValorBD = bd.LerInt("NivelRisco").ToString();

                    this.IP.ValorBD = bd.LerString("IP");

                    this.Fraude.ValorBD = bd.LerInt("Fraude").ToString();

                    this.AprovacaoAutomatica.ValorBD = bd.LerString("AprovacaoAutomatica");

                    this.QuantidadeImpressoesInternet.ValorBD = bd.LerInt("QuantidadeImpressoesInternet").ToString();

                    this.VendaCancelada.ValorBD = bd.LerString("VendaCancelada");

                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();

                    this.ClienteEnderecoID.ValorBD = bd.LerInt("ClienteEnderecoID").ToString();

                    this.EntregaControleID.ValorBD = bd.LerInt("EntregaControleID").ToString();

                    this.EntregaAgendaID.ValorBD = bd.LerInt("EntregaAgendaID").ToString();

                    this.PdvID.ValorBD = bd.LerInt("PdvID").ToString();

                    this.TaxaEntregaValor.ValorBD = bd.LerDecimal("TaxaEntregaValor").ToString();

                    this.PagamentoProcessado.ValorBD = bd.LerString("PagamentoProcessado");

                    this.NomeCartao.ValorBD = bd.LerString("NomeCartao");

                    this.ValorSeguro.ValorBD = bd.LerDecimal("ValorSeguro").ToString();

                    this.TaxaProcessamentoValor.ValorBD = bd.LerDecimal("TaxaProcessamentoValor").ToString();

                    this.TaxaProcessamentoCancelada.ValorBD = bd.LerString("TaxaProcessamentoCancelada");

                    this.Score.ValorBD = bd.LerInt("Score").ToString();

                    this.RetornoAccertify.ValorBD = bd.LerString("RetornoAccertify");

                    this.AccertifyForceStatus.ValorBD = bd.LerString("AccertifyForceStatus");

                    this.VendaBilhereriaIDTroca.ValorBD = bd.LerInt("VendaBilhereriaIDTroca").ToString();

                    this.CodigoRastreio.ValorBD = bd.LerString("CodigoRastreio");

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
        /// Preenche todos os atributos de VendaBilheteria do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xVendaBilheteria WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();

                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();

                    this.Senha.ValorBD = bd.LerString("Senha");

                    this.DataVenda.ValorBD = bd.LerString("DataVenda");

                    this.Status.ValorBD = bd.LerString("Status");

                    this.TaxaEntregaID.ValorBD = bd.LerInt("TaxaEntregaID").ToString();

                    this.TaxaConvenienciaValorTotal.ValorBD = bd.LerDecimal("TaxaConvenienciaValorTotal").ToString();

                    this.ComissaoValorTotal.ValorBD = bd.LerDecimal("ComissaoValorTotal").ToString();

                    this.ValorTotal.ValorBD = bd.LerDecimal("ValorTotal").ToString();

                    this.Obs.ValorBD = bd.LerString("Obs");

                    this.NotaFiscalCliente.ValorBD = bd.LerString("NotaFiscalCliente");

                    this.NotaFiscalEstabelecimento.ValorBD = bd.LerString("NotaFiscalEstabelecimento");

                    this.IndiceInstituicaoTransacao.ValorBD = bd.LerInt("IndiceInstituicaoTransacao").ToString();

                    this.IndiceTipoCartao.ValorBD = bd.LerInt("IndiceTipoCartao").ToString();

                    this.NSUSitef.ValorBD = bd.LerInt("NSUSitef").ToString();

                    this.NSUHost.ValorBD = bd.LerInt("NSUHost").ToString();

                    this.CodigoAutorizacaoCredito.ValorBD = bd.LerInt("CodigoAutorizacaoCredito").ToString();

                    this.BIN.ValorBD = bd.LerInt("BIN").ToString();

                    this.ModalidadePagamentoCodigo.ValorBD = bd.LerInt("ModalidadePagamentoCodigo").ToString();

                    this.ModalidadePagamentoTexto.ValorBD = bd.LerString("ModalidadePagamentoTexto");

                    this.DDD.ValorBD = bd.LerInt("DDD").ToString();

                    this.NumeroCelular.ValorBD = bd.LerInt("NumeroCelular").ToString();

                    this.ModelIDCelular.ValorBD = bd.LerInt("ModelIDCelular").ToString();

                    this.FabricanteCelular.ValorBD = bd.LerString("FabricanteCelular");

                    this.NivelRisco.ValorBD = bd.LerInt("NivelRisco").ToString();

                    this.IP.ValorBD = bd.LerString("IP");

                    this.Fraude.ValorBD = bd.LerInt("Fraude").ToString();

                    this.AprovacaoAutomatica.ValorBD = bd.LerString("AprovacaoAutomatica");

                    this.QuantidadeImpressoesInternet.ValorBD = bd.LerInt("QuantidadeImpressoesInternet").ToString();

                    this.VendaCancelada.ValorBD = bd.LerString("VendaCancelada");

                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();

                    this.ClienteEnderecoID.ValorBD = bd.LerInt("ClienteEnderecoID").ToString();

                    this.EntregaControleID.ValorBD = bd.LerInt("EntregaControleID").ToString();

                    this.EntregaAgendaID.ValorBD = bd.LerInt("EntregaAgendaID").ToString();

                    this.PdvID.ValorBD = bd.LerInt("PdvID").ToString();

                    this.TaxaEntregaValor.ValorBD = bd.LerDecimal("TaxaEntregaValor").ToString();

                    this.PagamentoProcessado.ValorBD = bd.LerString("PagamentoProcessado");

                    this.NomeCartao.ValorBD = bd.LerString("NomeCartao");

                    this.ValorSeguro.ValorBD = bd.LerDecimal("ValorSeguro").ToString();

                    this.TaxaProcessamentoValor.ValorBD = bd.LerDecimal("TaxaProcessamentoValor").ToString();

                    this.TaxaProcessamentoCancelada.ValorBD = bd.LerString("TaxaProcessamentoCancelada");

                    this.Score.ValorBD = bd.LerInt("Score").ToString();

                    this.RetornoAccertify.ValorBD = bd.LerString("RetornoAccertify");

                    this.AccertifyForceStatus.ValorBD = bd.LerString("AccertifyForceStatus");

                    this.VendaBilhereriaIDTroca.ValorBD = bd.LerInt("VendaBilhereriaIDTroca").ToString();

                    this.CodigoRastreio.ValorBD = bd.LerString("CodigoRastreio");

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

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();


                sql.Append("INSERT INTO xVendaBilheteria (ID, Versao, CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaConvenienciaValorTotal, ComissaoValorTotal, ValorTotal, Obs, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, DDD, NumeroCelular, ModelIDCelular, FabricanteCelular, NivelRisco, IP, Fraude, AprovacaoAutomatica, QuantidadeImpressoesInternet, VendaCancelada, MotivoID, ClienteEnderecoID, EntregaControleID, EntregaAgendaID, PdvID, TaxaEntregaValor, PagamentoProcessado, NomeCartao, ValorSeguro, TaxaProcessamentoValor, TaxaProcessamentoCancelada, Score, RetornoAccertify, AccertifyForceStatus, VendaBilhereriaIDTroca, CodigoRastreio) ");
                sql.Append("SELECT ID, @V, CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaConvenienciaValorTotal, ComissaoValorTotal, ValorTotal, Obs, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, DDD, NumeroCelular, ModelIDCelular, FabricanteCelular, NivelRisco, IP, Fraude, AprovacaoAutomatica, QuantidadeImpressoesInternet, VendaCancelada, MotivoID, ClienteEnderecoID, EntregaControleID, EntregaAgendaID, PdvID, TaxaEntregaValor, PagamentoProcessado, NomeCartao, ValorSeguro, TaxaProcessamentoValor, TaxaProcessamentoCancelada, Score, RetornoAccertify, AccertifyForceStatus, VendaBilhereriaIDTroca, CodigoRastreio FROM tVendaBilheteria WHERE ID = @I");
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
        /// Inserir novo(a) VendaBilheteria
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cVendaBilheteria");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteria(ID, CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaConvenienciaValorTotal, ComissaoValorTotal, ValorTotal, Obs, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, DDD, NumeroCelular, ModelIDCelular, FabricanteCelular, NivelRisco, IP, Fraude, AprovacaoAutomatica, QuantidadeImpressoesInternet, VendaCancelada, MotivoID, ClienteEnderecoID, EntregaControleID, EntregaAgendaID, PdvID, TaxaEntregaValor, PagamentoProcessado, NomeCartao, ValorSeguro, TaxaProcessamentoValor, TaxaProcessamentoCancelada, Score, RetornoAccertify, AccertifyForceStatus, VendaBilhereriaIDTroca, CodigoRastreio) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005',@006,'@007','@008','@009','@010','@011','@012',@013,@014,@015,@016,@017,@018,@019,'@020',@021,@022,@023,'@024',@025,'@026',@027,'@028',@029,'@030',@031,@032,@033,@034,@035,'@036','@037','@038','@039','@040','@041',@042,'@043','@044',@045,'@046')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.CaixaID.ValorBD);

                sql.Replace("@002", this.ClienteID.ValorBD);

                sql.Replace("@003", this.Senha.ValorBD);

                sql.Replace("@004", this.DataVenda.ValorBD);

                sql.Replace("@005", this.Status.ValorBD);

                sql.Replace("@006", this.TaxaEntregaID.ValorBD);

                sql.Replace("@007", this.TaxaConvenienciaValorTotal.ValorBD);

                sql.Replace("@008", this.ComissaoValorTotal.ValorBD);

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

                sql.Replace("@021", this.DDD.ValorBD);

                sql.Replace("@022", this.NumeroCelular.ValorBD);

                sql.Replace("@023", this.ModelIDCelular.ValorBD);

                sql.Replace("@024", this.FabricanteCelular.ValorBD);

                sql.Replace("@025", this.NivelRisco.ValorBD);

                sql.Replace("@026", this.IP.ValorBD);

                sql.Replace("@027", this.Fraude.ValorBD);

                sql.Replace("@028", this.AprovacaoAutomatica.ValorBD);

                sql.Replace("@029", this.QuantidadeImpressoesInternet.ValorBD);

                sql.Replace("@030", this.VendaCancelada.ValorBD);

                sql.Replace("@031", this.MotivoID.ValorBD);

                sql.Replace("@032", this.ClienteEnderecoID.ValorBD);

                sql.Replace("@033", this.EntregaControleID.ValorBD);

                sql.Replace("@034", this.EntregaAgendaID.ValorBD);

                sql.Replace("@035", this.PdvID.ValorBD);

                sql.Replace("@036", this.TaxaEntregaValor.ValorBD);

                sql.Replace("@037", this.PagamentoProcessado.ValorBD);

                sql.Replace("@038", this.NomeCartao.ValorBD);

                sql.Replace("@039", this.ValorSeguro.ValorBD);

                sql.Replace("@040", this.TaxaProcessamentoValor.ValorBD);

                sql.Replace("@041", this.TaxaProcessamentoCancelada.ValorBD);

                sql.Replace("@042", this.Score.ValorBD);

                sql.Replace("@043", this.RetornoAccertify.ValorBD);

                sql.Replace("@044", this.AccertifyForceStatus.ValorBD);

                sql.Replace("@045", this.VendaBilhereriaIDTroca.ValorBD);

                sql.Replace("@046", this.CodigoRastreio.ValorBD);


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
        /// Inserir novo(a) VendaBilheteria
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cVendaBilheteria");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteria(ID, CaixaID, ClienteID, Senha, DataVenda, Status, TaxaEntregaID, TaxaConvenienciaValorTotal, ComissaoValorTotal, ValorTotal, Obs, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, DDD, NumeroCelular, ModelIDCelular, FabricanteCelular, NivelRisco, IP, Fraude, AprovacaoAutomatica, QuantidadeImpressoesInternet, VendaCancelada, MotivoID, ClienteEnderecoID, EntregaControleID, EntregaAgendaID, PdvID, TaxaEntregaValor, PagamentoProcessado, NomeCartao, ValorSeguro, TaxaProcessamentoValor, TaxaProcessamentoCancelada, Score, RetornoAccertify, AccertifyForceStatus, VendaBilhereriaIDTroca, CodigoRastreio) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005',@006,'@007','@008','@009','@010','@011','@012',@013,@014,@015,@016,@017,@018,@019,'@020',@021,@022,@023,'@024',@025,'@026',@027,'@028',@029,'@030',@031,@032,@033,@034,@035,'@036','@037','@038','@039','@040','@041',@042,'@043','@044',@045,'@046')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.CaixaID.ValorBD);

                sql.Replace("@002", this.ClienteID.ValorBD);

                sql.Replace("@003", this.Senha.ValorBD);

                sql.Replace("@004", this.DataVenda.ValorBD);

                sql.Replace("@005", this.Status.ValorBD);

                sql.Replace("@006", this.TaxaEntregaID.ValorBD);

                sql.Replace("@007", this.TaxaConvenienciaValorTotal.ValorBD);

                sql.Replace("@008", this.ComissaoValorTotal.ValorBD);

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

                sql.Replace("@021", this.DDD.ValorBD);

                sql.Replace("@022", this.NumeroCelular.ValorBD);

                sql.Replace("@023", this.ModelIDCelular.ValorBD);

                sql.Replace("@024", this.FabricanteCelular.ValorBD);

                sql.Replace("@025", this.NivelRisco.ValorBD);

                sql.Replace("@026", this.IP.ValorBD);

                sql.Replace("@027", this.Fraude.ValorBD);

                sql.Replace("@028", this.AprovacaoAutomatica.ValorBD);

                sql.Replace("@029", this.QuantidadeImpressoesInternet.ValorBD);

                sql.Replace("@030", this.VendaCancelada.ValorBD);

                sql.Replace("@031", this.MotivoID.ValorBD);

                sql.Replace("@032", this.ClienteEnderecoID.ValorBD);

                sql.Replace("@033", this.EntregaControleID.ValorBD);

                sql.Replace("@034", this.EntregaAgendaID.ValorBD);

                sql.Replace("@035", this.PdvID.ValorBD);

                sql.Replace("@036", this.TaxaEntregaValor.ValorBD);

                sql.Replace("@037", this.PagamentoProcessado.ValorBD);

                sql.Replace("@038", this.NomeCartao.ValorBD);

                sql.Replace("@039", this.ValorSeguro.ValorBD);

                sql.Replace("@040", this.TaxaProcessamentoValor.ValorBD);

                sql.Replace("@041", this.TaxaProcessamentoCancelada.ValorBD);

                sql.Replace("@042", this.Score.ValorBD);

                sql.Replace("@043", this.RetornoAccertify.ValorBD);

                sql.Replace("@044", this.AccertifyForceStatus.ValorBD);

                sql.Replace("@045", this.VendaBilhereriaIDTroca.ValorBD);

                sql.Replace("@046", this.CodigoRastreio.ValorBD);


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
        /// Atualiza VendaBilheteria
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cVendaBilheteria WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tVendaBilheteria SET CaixaID = @001, ClienteID = @002, Senha = '@003', DataVenda = '@004', Status = '@005', TaxaEntregaID = @006, TaxaConvenienciaValorTotal = '@007', ComissaoValorTotal = '@008', ValorTotal = '@009', Obs = '@010', NotaFiscalCliente = '@011', NotaFiscalEstabelecimento = '@012', IndiceInstituicaoTransacao = @013, IndiceTipoCartao = @014, NSUSitef = @015, NSUHost = @016, CodigoAutorizacaoCredito = @017, BIN = @018, ModalidadePagamentoCodigo = @019, ModalidadePagamentoTexto = '@020', DDD = @021, NumeroCelular = @022, ModelIDCelular = @023, FabricanteCelular = '@024', NivelRisco = @025, IP = '@026', Fraude = @027, AprovacaoAutomatica = '@028', QuantidadeImpressoesInternet = @029, VendaCancelada = '@030', MotivoID = @031, ClienteEnderecoID = @032, EntregaControleID = @033, EntregaAgendaID = @034, PdvID = @035, TaxaEntregaValor = '@036', PagamentoProcessado = '@037', NomeCartao = '@038', ValorSeguro = '@039', TaxaProcessamentoValor = '@040', TaxaProcessamentoCancelada = '@041', Score = @042, RetornoAccertify = '@043', AccertifyForceStatus = '@044', VendaBilhereriaIDTroca = @045, CodigoRastreio = '@046' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.CaixaID.ValorBD);

                sql.Replace("@002", this.ClienteID.ValorBD);

                sql.Replace("@003", this.Senha.ValorBD);

                sql.Replace("@004", this.DataVenda.ValorBD);

                sql.Replace("@005", this.Status.ValorBD);

                sql.Replace("@006", this.TaxaEntregaID.ValorBD);

                sql.Replace("@007", this.TaxaConvenienciaValorTotal.ValorBD);

                sql.Replace("@008", this.ComissaoValorTotal.ValorBD);

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

                sql.Replace("@021", this.DDD.ValorBD);

                sql.Replace("@022", this.NumeroCelular.ValorBD);

                sql.Replace("@023", this.ModelIDCelular.ValorBD);

                sql.Replace("@024", this.FabricanteCelular.ValorBD);

                sql.Replace("@025", this.NivelRisco.ValorBD);

                sql.Replace("@026", this.IP.ValorBD);

                sql.Replace("@027", this.Fraude.ValorBD);

                sql.Replace("@028", this.AprovacaoAutomatica.ValorBD);

                sql.Replace("@029", this.QuantidadeImpressoesInternet.ValorBD);

                sql.Replace("@030", this.VendaCancelada.ValorBD);

                sql.Replace("@031", this.MotivoID.ValorBD);

                sql.Replace("@032", this.ClienteEnderecoID.ValorBD);

                sql.Replace("@033", this.EntregaControleID.ValorBD);

                sql.Replace("@034", this.EntregaAgendaID.ValorBD);

                sql.Replace("@035", this.PdvID.ValorBD);

                sql.Replace("@036", this.TaxaEntregaValor.ValorBD);

                sql.Replace("@037", this.PagamentoProcessado.ValorBD);

                sql.Replace("@038", this.NomeCartao.ValorBD);

                sql.Replace("@039", this.ValorSeguro.ValorBD);

                sql.Replace("@040", this.TaxaProcessamentoValor.ValorBD);

                sql.Replace("@041", this.TaxaProcessamentoCancelada.ValorBD);

                sql.Replace("@042", this.Score.ValorBD);

                sql.Replace("@043", this.RetornoAccertify.ValorBD);

                sql.Replace("@044", this.AccertifyForceStatus.ValorBD);

                sql.Replace("@045", this.VendaBilhereriaIDTroca.ValorBD);

                sql.Replace("@046", this.CodigoRastreio.ValorBD);


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
        /// Atualiza VendaBilheteria
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
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

                sql.Append("UPDATE tVendaBilheteria SET CaixaID = @001, ClienteID = @002, Senha = '@003', DataVenda = '@004', Status = '@005', TaxaEntregaID = @006, TaxaConvenienciaValorTotal = '@007', ComissaoValorTotal = '@008', ValorTotal = '@009', Obs = '@010', NotaFiscalCliente = '@011', NotaFiscalEstabelecimento = '@012', IndiceInstituicaoTransacao = @013, IndiceTipoCartao = @014, NSUSitef = @015, NSUHost = @016, CodigoAutorizacaoCredito = @017, BIN = @018, ModalidadePagamentoCodigo = @019, ModalidadePagamentoTexto = '@020', DDD = @021, NumeroCelular = @022, ModelIDCelular = @023, FabricanteCelular = '@024', NivelRisco = @025, IP = '@026', Fraude = @027, AprovacaoAutomatica = '@028', QuantidadeImpressoesInternet = @029, VendaCancelada = '@030', MotivoID = @031, ClienteEnderecoID = @032, EntregaControleID = @033, EntregaAgendaID = @034, PdvID = @035, TaxaEntregaValor = '@036', PagamentoProcessado = '@037', NomeCartao = '@038', ValorSeguro = '@039', TaxaProcessamentoValor = '@040', TaxaProcessamentoCancelada = '@041', Score = @042, RetornoAccertify = '@043', AccertifyForceStatus = '@044', VendaBilhereriaIDTroca = @045, CodigoRastreio = '@046' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.CaixaID.ValorBD);

                sql.Replace("@002", this.ClienteID.ValorBD);

                sql.Replace("@003", this.Senha.ValorBD);

                sql.Replace("@004", this.DataVenda.ValorBD);

                sql.Replace("@005", this.Status.ValorBD);

                sql.Replace("@006", this.TaxaEntregaID.ValorBD);

                sql.Replace("@007", this.TaxaConvenienciaValorTotal.ValorBD);

                sql.Replace("@008", this.ComissaoValorTotal.ValorBD);

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

                sql.Replace("@021", this.DDD.ValorBD);

                sql.Replace("@022", this.NumeroCelular.ValorBD);

                sql.Replace("@023", this.ModelIDCelular.ValorBD);

                sql.Replace("@024", this.FabricanteCelular.ValorBD);

                sql.Replace("@025", this.NivelRisco.ValorBD);

                sql.Replace("@026", this.IP.ValorBD);

                sql.Replace("@027", this.Fraude.ValorBD);

                sql.Replace("@028", this.AprovacaoAutomatica.ValorBD);

                sql.Replace("@029", this.QuantidadeImpressoesInternet.ValorBD);

                sql.Replace("@030", this.VendaCancelada.ValorBD);

                sql.Replace("@031", this.MotivoID.ValorBD);

                sql.Replace("@032", this.ClienteEnderecoID.ValorBD);

                sql.Replace("@033", this.EntregaControleID.ValorBD);

                sql.Replace("@034", this.EntregaAgendaID.ValorBD);

                sql.Replace("@035", this.PdvID.ValorBD);

                sql.Replace("@036", this.TaxaEntregaValor.ValorBD);

                sql.Replace("@037", this.PagamentoProcessado.ValorBD);

                sql.Replace("@038", this.NomeCartao.ValorBD);

                sql.Replace("@039", this.ValorSeguro.ValorBD);

                sql.Replace("@040", this.TaxaProcessamentoValor.ValorBD);

                sql.Replace("@041", this.TaxaProcessamentoCancelada.ValorBD);

                sql.Replace("@042", this.Score.ValorBD);

                sql.Replace("@043", this.RetornoAccertify.ValorBD);

                sql.Replace("@044", this.AccertifyForceStatus.ValorBD);

                sql.Replace("@045", this.VendaBilhereriaIDTroca.ValorBD);

                sql.Replace("@046", this.CodigoRastreio.ValorBD);


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
        /// Exclui VendaBilheteria com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cVendaBilheteria WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tVendaBilheteria WHERE ID=" + id;

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
        /// Exclui VendaBilheteria com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cVendaBilheteria WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tVendaBilheteria WHERE ID=" + id;

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
        /// Exclui VendaBilheteria
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


            this.CaixaID.Limpar();

            this.ClienteID.Limpar();

            this.Senha.Limpar();

            this.DataVenda.Limpar();

            this.Status.Limpar();

            this.TaxaEntregaID.Limpar();

            this.TaxaConvenienciaValorTotal.Limpar();

            this.ComissaoValorTotal.Limpar();

            this.ValorTotal.Limpar();

            this.Obs.Limpar();

            this.NotaFiscalCliente.Limpar();

            this.NotaFiscalEstabelecimento.Limpar();

            this.IndiceInstituicaoTransacao.Limpar();

            this.IndiceTipoCartao.Limpar();

            this.NSUSitef.Limpar();

            this.NSUHost.Limpar();

            this.CodigoAutorizacaoCredito.Limpar();

            this.BIN.Limpar();

            this.ModalidadePagamentoCodigo.Limpar();

            this.ModalidadePagamentoTexto.Limpar();

            this.DDD.Limpar();

            this.NumeroCelular.Limpar();

            this.ModelIDCelular.Limpar();

            this.FabricanteCelular.Limpar();

            this.NivelRisco.Limpar();

            this.IP.Limpar();

            this.Fraude.Limpar();

            this.AprovacaoAutomatica.Limpar();

            this.QuantidadeImpressoesInternet.Limpar();

            this.VendaCancelada.Limpar();

            this.MotivoID.Limpar();

            this.ClienteEnderecoID.Limpar();

            this.EntregaControleID.Limpar();

            this.EntregaAgendaID.Limpar();

            this.PdvID.Limpar();

            this.TaxaEntregaValor.Limpar();

            this.PagamentoProcessado.Limpar();

            this.NomeCartao.Limpar();

            this.ValorSeguro.Limpar();

            this.TaxaProcessamentoValor.Limpar();

            this.TaxaProcessamentoCancelada.Limpar();

            this.Score.Limpar();

            this.RetornoAccertify.Limpar();

            this.AccertifyForceStatus.Limpar();

            this.VendaBilhereriaIDTroca.Limpar();

            this.CodigoRastreio.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.CaixaID.Desfazer();

            this.ClienteID.Desfazer();

            this.Senha.Desfazer();

            this.DataVenda.Desfazer();

            this.Status.Desfazer();

            this.TaxaEntregaID.Desfazer();

            this.TaxaConvenienciaValorTotal.Desfazer();

            this.ComissaoValorTotal.Desfazer();

            this.ValorTotal.Desfazer();

            this.Obs.Desfazer();

            this.NotaFiscalCliente.Desfazer();

            this.NotaFiscalEstabelecimento.Desfazer();

            this.IndiceInstituicaoTransacao.Desfazer();

            this.IndiceTipoCartao.Desfazer();

            this.NSUSitef.Desfazer();

            this.NSUHost.Desfazer();

            this.CodigoAutorizacaoCredito.Desfazer();

            this.BIN.Desfazer();

            this.ModalidadePagamentoCodigo.Desfazer();

            this.ModalidadePagamentoTexto.Desfazer();

            this.DDD.Desfazer();

            this.NumeroCelular.Desfazer();

            this.ModelIDCelular.Desfazer();

            this.FabricanteCelular.Desfazer();

            this.NivelRisco.Desfazer();

            this.IP.Desfazer();

            this.Fraude.Desfazer();

            this.AprovacaoAutomatica.Desfazer();

            this.QuantidadeImpressoesInternet.Desfazer();

            this.VendaCancelada.Desfazer();

            this.MotivoID.Desfazer();

            this.ClienteEnderecoID.Desfazer();

            this.EntregaControleID.Desfazer();

            this.EntregaAgendaID.Desfazer();

            this.PdvID.Desfazer();

            this.TaxaEntregaValor.Desfazer();

            this.PagamentoProcessado.Desfazer();

            this.NomeCartao.Desfazer();

            this.ValorSeguro.Desfazer();

            this.TaxaProcessamentoValor.Desfazer();

            this.TaxaProcessamentoCancelada.Desfazer();

            this.Score.Desfazer();

            this.RetornoAccertify.Desfazer();

            this.AccertifyForceStatus.Desfazer();

            this.VendaBilhereriaIDTroca.Desfazer();

            this.CodigoRastreio.Desfazer();

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


        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
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


        public class senha : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Senha";
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


        public class datavenda : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataVenda";
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


        public class status : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Status";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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


        public class taxaconvenienciavalortotal : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConvenienciaValorTotal";
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


        public class comissaovalortotal : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoValorTotal";
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


        public class obs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Obs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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


        public class notafiscalcliente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NotaFiscalCliente";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 600;
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


        public class notafiscalestabelecimento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NotaFiscalEstabelecimento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 600;
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


        public class indiceinstituicaotransacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "IndiceInstituicaoTransacao";
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


        public class indicetipocartao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "IndiceTipoCartao";
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


        public class nsusitef : IntegerProperty
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


        public class nsuhost : IntegerProperty
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


        public class codigoautorizacaocredito : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoAutorizacaoCredito";
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


        public class bin : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BIN";
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


        public class modalidadepagamentocodigo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ModalidadePagamentoCodigo";
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


        public class modalidadepagamentotexto : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ModalidadePagamentoTexto";
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


        public class ddd : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDD";
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


        public class numerocelular : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroCelular";
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


        public class modelidcelular : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ModelIDCelular";
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


        public class fabricantecelular : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "FabricanteCelular";
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


        public class nivelrisco : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "NivelRisco";
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


        public class ip : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "IP";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 15;
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


        public class fraude : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Fraude";
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


        public class aprovacaoautomatica : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "AprovacaoAutomatica";
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


        public class quantidadeimpressoesinternet : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeImpressoesInternet";
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


        public class vendacancelada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaCancelada";
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


        public class motivoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoID";
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


        public class clienteenderecoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteEnderecoID";
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


        public class entregacontroleid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaControleID";
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


        public class entregaagendaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaAgendaID";
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


        public class pdvid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PdvID";
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


        public class pagamentoprocessado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PagamentoProcessado";
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


        public class nomecartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeCartao";
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


        public class valorseguro : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorSeguro";
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


        public class taxaprocessamentovalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaProcessamentoValor";
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


        public class taxaprocessamentocancelada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaProcessamentoCancelada";
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


        public class score : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Score";
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


        public class retornoaccertify : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "RetornoAccertify";
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


        public class accertifyforcestatus : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AccertifyForceStatus";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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


        public class vendabilhereriaidtroca : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilhereriaIDTroca";
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


        public class codigorastreio : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoRastreio";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
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

                DataTable tabela = new DataTable("VendaBilheteria");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("CaixaID", typeof(int));

                tabela.Columns.Add("ClienteID", typeof(int));

                tabela.Columns.Add("Senha", typeof(string));

                tabela.Columns.Add("DataVenda", typeof(DateTime));

                tabela.Columns.Add("Status", typeof(string));

                tabela.Columns.Add("TaxaEntregaID", typeof(int));

                tabela.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));

                tabela.Columns.Add("ComissaoValorTotal", typeof(decimal));

                tabela.Columns.Add("ValorTotal", typeof(decimal));

                tabela.Columns.Add("Obs", typeof(string));

                tabela.Columns.Add("NotaFiscalCliente", typeof(string));

                tabela.Columns.Add("NotaFiscalEstabelecimento", typeof(string));

                tabela.Columns.Add("IndiceInstituicaoTransacao", typeof(int));

                tabela.Columns.Add("IndiceTipoCartao", typeof(int));

                tabela.Columns.Add("NSUSitef", typeof(int));

                tabela.Columns.Add("NSUHost", typeof(int));

                tabela.Columns.Add("CodigoAutorizacaoCredito", typeof(int));

                tabela.Columns.Add("BIN", typeof(int));

                tabela.Columns.Add("ModalidadePagamentoCodigo", typeof(int));

                tabela.Columns.Add("ModalidadePagamentoTexto", typeof(string));

                tabela.Columns.Add("DDD", typeof(int));

                tabela.Columns.Add("NumeroCelular", typeof(int));

                tabela.Columns.Add("ModelIDCelular", typeof(int));

                tabela.Columns.Add("FabricanteCelular", typeof(string));

                tabela.Columns.Add("NivelRisco", typeof(int));

                tabela.Columns.Add("IP", typeof(string));

                tabela.Columns.Add("Fraude", typeof(int));

                tabela.Columns.Add("AprovacaoAutomatica", typeof(bool));

                tabela.Columns.Add("QuantidadeImpressoesInternet", typeof(int));

                tabela.Columns.Add("VendaCancelada", typeof(bool));

                tabela.Columns.Add("MotivoID", typeof(int));

                tabela.Columns.Add("ClienteEnderecoID", typeof(int));

                tabela.Columns.Add("EntregaControleID", typeof(int));

                tabela.Columns.Add("EntregaAgendaID", typeof(int));

                tabela.Columns.Add("PdvID", typeof(int));

                tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

                tabela.Columns.Add("PagamentoProcessado", typeof(bool));

                tabela.Columns.Add("NomeCartao", typeof(string));

                tabela.Columns.Add("ValorSeguro", typeof(decimal));

                tabela.Columns.Add("TaxaProcessamentoValor", typeof(decimal));

                tabela.Columns.Add("TaxaProcessamentoCancelada", typeof(bool));

                tabela.Columns.Add("Score", typeof(int));

                tabela.Columns.Add("RetornoAccertify", typeof(string));

                tabela.Columns.Add("AccertifyForceStatus", typeof(string));

                tabela.Columns.Add("VendaBilhereriaIDTroca", typeof(int));

                tabela.Columns.Add("CodigoRastreio", typeof(string));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Senhas(int clienteid);
        public abstract string GerarSenha();
        public abstract DataTable Itens();
        public abstract DataTable DadosParaIngresso();
        public abstract DataTable DadosParaIngresso(int[] itens);
        public abstract bool EmitirComprovante();
        public abstract decimal Total();
        public abstract DataTable VendasResumo(int caixaid);
        public abstract int QuantidadeIngressos();
        public abstract int QuantidadePagamentos();
        public abstract DataTable ParcelasPagamento(string registrozero, decimal valortotal);
        public abstract int QuantidadeItens();
        public abstract DataTable FormasPagamento();

    }
    #endregion

    #region "VendaBilheteriaLista_B"


    public abstract class VendaBilheteriaLista_B : BaseLista
    {

        private bool backup = false;
        protected VendaBilheteria vendaBilheteria;

        // passar o Usuario logado no sistema
        public VendaBilheteriaLista_B()
        {
            vendaBilheteria = new VendaBilheteria();
        }

        // passar o Usuario logado no sistema
        public VendaBilheteriaLista_B(int usuarioIDLogado)
        {
            vendaBilheteria = new VendaBilheteria(usuarioIDLogado);
        }

        public VendaBilheteria VendaBilheteria
        {
            get { return vendaBilheteria; }
        }

        /// <summary>
        /// Retorna um IBaseBD de VendaBilheteria especifico
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
                    vendaBilheteria.Ler(id);
                    return vendaBilheteria;
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
                    sql = "SELECT ID FROM tVendaBilheteria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteria";

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
                    sql = "SELECT ID FROM tVendaBilheteria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteria";

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
                    sql = "SELECT ID FROM xVendaBilheteria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xVendaBilheteria";

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
        /// Preenche VendaBilheteria corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    vendaBilheteria.Ler(id);
                else
                    vendaBilheteria.LerBackup(id);

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

                bool ok = vendaBilheteria.Excluir();
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
        /// Inseri novo(a) VendaBilheteria na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = vendaBilheteria.Inserir();
                if (ok)
                {
                    lista.Add(vendaBilheteria.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de VendaBilheteria carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("VendaBilheteria");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("CaixaID", typeof(int));

                tabela.Columns.Add("ClienteID", typeof(int));

                tabela.Columns.Add("Senha", typeof(string));

                tabela.Columns.Add("DataVenda", typeof(DateTime));

                tabela.Columns.Add("Status", typeof(string));

                tabela.Columns.Add("TaxaEntregaID", typeof(int));

                tabela.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));

                tabela.Columns.Add("ComissaoValorTotal", typeof(decimal));

                tabela.Columns.Add("ValorTotal", typeof(decimal));

                tabela.Columns.Add("Obs", typeof(string));

                tabela.Columns.Add("NotaFiscalCliente", typeof(string));

                tabela.Columns.Add("NotaFiscalEstabelecimento", typeof(string));

                tabela.Columns.Add("IndiceInstituicaoTransacao", typeof(int));

                tabela.Columns.Add("IndiceTipoCartao", typeof(int));

                tabela.Columns.Add("NSUSitef", typeof(int));

                tabela.Columns.Add("NSUHost", typeof(int));

                tabela.Columns.Add("CodigoAutorizacaoCredito", typeof(int));

                tabela.Columns.Add("BIN", typeof(int));

                tabela.Columns.Add("ModalidadePagamentoCodigo", typeof(int));

                tabela.Columns.Add("ModalidadePagamentoTexto", typeof(string));

                tabela.Columns.Add("DDD", typeof(int));

                tabela.Columns.Add("NumeroCelular", typeof(int));

                tabela.Columns.Add("ModelIDCelular", typeof(int));

                tabela.Columns.Add("FabricanteCelular", typeof(string));

                tabela.Columns.Add("NivelRisco", typeof(int));

                tabela.Columns.Add("IP", typeof(string));

                tabela.Columns.Add("Fraude", typeof(int));

                tabela.Columns.Add("AprovacaoAutomatica", typeof(bool));

                tabela.Columns.Add("QuantidadeImpressoesInternet", typeof(int));

                tabela.Columns.Add("VendaCancelada", typeof(bool));

                tabela.Columns.Add("MotivoID", typeof(int));

                tabela.Columns.Add("ClienteEnderecoID", typeof(int));

                tabela.Columns.Add("EntregaControleID", typeof(int));

                tabela.Columns.Add("EntregaAgendaID", typeof(int));

                tabela.Columns.Add("PdvID", typeof(int));

                tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

                tabela.Columns.Add("PagamentoProcessado", typeof(bool));

                tabela.Columns.Add("NomeCartao", typeof(string));

                tabela.Columns.Add("ValorSeguro", typeof(decimal));

                tabela.Columns.Add("TaxaProcessamentoValor", typeof(decimal));

                tabela.Columns.Add("TaxaProcessamentoCancelada", typeof(bool));

                tabela.Columns.Add("Score", typeof(int));

                tabela.Columns.Add("RetornoAccertify", typeof(string));

                tabela.Columns.Add("AccertifyForceStatus", typeof(string));

                tabela.Columns.Add("VendaBilhereriaIDTroca", typeof(int));

                tabela.Columns.Add("CodigoRastreio", typeof(string));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = vendaBilheteria.Control.ID;

                        linha["CaixaID"] = vendaBilheteria.CaixaID.Valor;

                        linha["ClienteID"] = vendaBilheteria.ClienteID.Valor;

                        linha["Senha"] = vendaBilheteria.Senha.Valor;

                        linha["DataVenda"] = vendaBilheteria.DataVenda.Valor;

                        linha["Status"] = vendaBilheteria.Status.Valor;

                        linha["TaxaEntregaID"] = vendaBilheteria.TaxaEntregaID.Valor;

                        linha["TaxaConvenienciaValorTotal"] = vendaBilheteria.TaxaConvenienciaValorTotal.Valor;

                        linha["ComissaoValorTotal"] = vendaBilheteria.ComissaoValorTotal.Valor;

                        linha["ValorTotal"] = vendaBilheteria.ValorTotal.Valor;

                        linha["Obs"] = vendaBilheteria.Obs.Valor;

                        linha["NotaFiscalCliente"] = vendaBilheteria.NotaFiscalCliente.Valor;

                        linha["NotaFiscalEstabelecimento"] = vendaBilheteria.NotaFiscalEstabelecimento.Valor;

                        linha["IndiceInstituicaoTransacao"] = vendaBilheteria.IndiceInstituicaoTransacao.Valor;

                        linha["IndiceTipoCartao"] = vendaBilheteria.IndiceTipoCartao.Valor;

                        linha["NSUSitef"] = vendaBilheteria.NSUSitef.Valor;

                        linha["NSUHost"] = vendaBilheteria.NSUHost.Valor;

                        linha["CodigoAutorizacaoCredito"] = vendaBilheteria.CodigoAutorizacaoCredito.Valor;

                        linha["BIN"] = vendaBilheteria.BIN.Valor;

                        linha["ModalidadePagamentoCodigo"] = vendaBilheteria.ModalidadePagamentoCodigo.Valor;

                        linha["ModalidadePagamentoTexto"] = vendaBilheteria.ModalidadePagamentoTexto.Valor;

                        linha["DDD"] = vendaBilheteria.DDD.Valor;

                        linha["NumeroCelular"] = vendaBilheteria.NumeroCelular.Valor;

                        linha["ModelIDCelular"] = vendaBilheteria.ModelIDCelular.Valor;

                        linha["FabricanteCelular"] = vendaBilheteria.FabricanteCelular.Valor;

                        linha["NivelRisco"] = vendaBilheteria.NivelRisco.Valor;

                        linha["IP"] = vendaBilheteria.IP.Valor;

                        linha["Fraude"] = vendaBilheteria.Fraude.Valor;

                        linha["AprovacaoAutomatica"] = vendaBilheteria.AprovacaoAutomatica.Valor;

                        linha["QuantidadeImpressoesInternet"] = vendaBilheteria.QuantidadeImpressoesInternet.Valor;

                        linha["VendaCancelada"] = vendaBilheteria.VendaCancelada.Valor;

                        linha["MotivoID"] = vendaBilheteria.MotivoID.Valor;

                        linha["ClienteEnderecoID"] = vendaBilheteria.ClienteEnderecoID.Valor;

                        linha["EntregaControleID"] = vendaBilheteria.EntregaControleID.Valor;

                        linha["EntregaAgendaID"] = vendaBilheteria.EntregaAgendaID.Valor;

                        linha["PdvID"] = vendaBilheteria.PdvID.Valor;

                        linha["TaxaEntregaValor"] = vendaBilheteria.TaxaEntregaValor.Valor;

                        linha["PagamentoProcessado"] = vendaBilheteria.PagamentoProcessado.Valor;

                        linha["NomeCartao"] = vendaBilheteria.NomeCartao.Valor;

                        linha["ValorSeguro"] = vendaBilheteria.ValorSeguro.Valor;

                        linha["TaxaProcessamentoValor"] = vendaBilheteria.TaxaProcessamentoValor.Valor;

                        linha["TaxaProcessamentoCancelada"] = vendaBilheteria.TaxaProcessamentoCancelada.Valor;

                        linha["Score"] = vendaBilheteria.Score.Valor;

                        linha["RetornoAccertify"] = vendaBilheteria.RetornoAccertify.Valor;

                        linha["AccertifyForceStatus"] = vendaBilheteria.AccertifyForceStatus.Valor;

                        linha["VendaBilhereriaIDTroca"] = vendaBilheteria.VendaBilhereriaIDTroca.Valor;

                        linha["CodigoRastreio"] = vendaBilheteria.CodigoRastreio.Valor;

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

                DataTable tabela = new DataTable("RelatorioVendaBilheteria");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("CaixaID", typeof(int));

                    tabela.Columns.Add("ClienteID", typeof(int));

                    tabela.Columns.Add("Senha", typeof(string));

                    tabela.Columns.Add("DataVenda", typeof(DateTime));

                    tabela.Columns.Add("Status", typeof(string));

                    tabela.Columns.Add("TaxaEntregaID", typeof(int));

                    tabela.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));

                    tabela.Columns.Add("ComissaoValorTotal", typeof(decimal));

                    tabela.Columns.Add("ValorTotal", typeof(decimal));

                    tabela.Columns.Add("Obs", typeof(string));

                    tabela.Columns.Add("NotaFiscalCliente", typeof(string));

                    tabela.Columns.Add("NotaFiscalEstabelecimento", typeof(string));

                    tabela.Columns.Add("IndiceInstituicaoTransacao", typeof(int));

                    tabela.Columns.Add("IndiceTipoCartao", typeof(int));

                    tabela.Columns.Add("NSUSitef", typeof(int));

                    tabela.Columns.Add("NSUHost", typeof(int));

                    tabela.Columns.Add("CodigoAutorizacaoCredito", typeof(int));

                    tabela.Columns.Add("BIN", typeof(int));

                    tabela.Columns.Add("ModalidadePagamentoCodigo", typeof(int));

                    tabela.Columns.Add("ModalidadePagamentoTexto", typeof(string));

                    tabela.Columns.Add("DDD", typeof(int));

                    tabela.Columns.Add("NumeroCelular", typeof(int));

                    tabela.Columns.Add("ModelIDCelular", typeof(int));

                    tabela.Columns.Add("FabricanteCelular", typeof(string));

                    tabela.Columns.Add("NivelRisco", typeof(int));

                    tabela.Columns.Add("IP", typeof(string));

                    tabela.Columns.Add("Fraude", typeof(int));

                    tabela.Columns.Add("AprovacaoAutomatica", typeof(bool));

                    tabela.Columns.Add("QuantidadeImpressoesInternet", typeof(int));

                    tabela.Columns.Add("VendaCancelada", typeof(bool));

                    tabela.Columns.Add("MotivoID", typeof(int));

                    tabela.Columns.Add("ClienteEnderecoID", typeof(int));

                    tabela.Columns.Add("EntregaControleID", typeof(int));

                    tabela.Columns.Add("EntregaAgendaID", typeof(int));

                    tabela.Columns.Add("PdvID", typeof(int));

                    tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));

                    tabela.Columns.Add("PagamentoProcessado", typeof(bool));

                    tabela.Columns.Add("NomeCartao", typeof(string));

                    tabela.Columns.Add("ValorSeguro", typeof(decimal));

                    tabela.Columns.Add("TaxaProcessamentoValor", typeof(decimal));

                    tabela.Columns.Add("TaxaProcessamentoCancelada", typeof(bool));

                    tabela.Columns.Add("Score", typeof(int));

                    tabela.Columns.Add("RetornoAccertify", typeof(string));

                    tabela.Columns.Add("AccertifyForceStatus", typeof(string));

                    tabela.Columns.Add("VendaBilhereriaIDTroca", typeof(int));

                    tabela.Columns.Add("CodigoRastreio", typeof(string));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["CaixaID"] = vendaBilheteria.CaixaID.Valor;

                        linha["ClienteID"] = vendaBilheteria.ClienteID.Valor;

                        linha["Senha"] = vendaBilheteria.Senha.Valor;

                        linha["DataVenda"] = vendaBilheteria.DataVenda.Valor;

                        linha["Status"] = vendaBilheteria.Status.Valor;

                        linha["TaxaEntregaID"] = vendaBilheteria.TaxaEntregaID.Valor;

                        linha["TaxaConvenienciaValorTotal"] = vendaBilheteria.TaxaConvenienciaValorTotal.Valor;

                        linha["ComissaoValorTotal"] = vendaBilheteria.ComissaoValorTotal.Valor;

                        linha["ValorTotal"] = vendaBilheteria.ValorTotal.Valor;

                        linha["Obs"] = vendaBilheteria.Obs.Valor;

                        linha["NotaFiscalCliente"] = vendaBilheteria.NotaFiscalCliente.Valor;

                        linha["NotaFiscalEstabelecimento"] = vendaBilheteria.NotaFiscalEstabelecimento.Valor;

                        linha["IndiceInstituicaoTransacao"] = vendaBilheteria.IndiceInstituicaoTransacao.Valor;

                        linha["IndiceTipoCartao"] = vendaBilheteria.IndiceTipoCartao.Valor;

                        linha["NSUSitef"] = vendaBilheteria.NSUSitef.Valor;

                        linha["NSUHost"] = vendaBilheteria.NSUHost.Valor;

                        linha["CodigoAutorizacaoCredito"] = vendaBilheteria.CodigoAutorizacaoCredito.Valor;

                        linha["BIN"] = vendaBilheteria.BIN.Valor;

                        linha["ModalidadePagamentoCodigo"] = vendaBilheteria.ModalidadePagamentoCodigo.Valor;

                        linha["ModalidadePagamentoTexto"] = vendaBilheteria.ModalidadePagamentoTexto.Valor;

                        linha["DDD"] = vendaBilheteria.DDD.Valor;

                        linha["NumeroCelular"] = vendaBilheteria.NumeroCelular.Valor;

                        linha["ModelIDCelular"] = vendaBilheteria.ModelIDCelular.Valor;

                        linha["FabricanteCelular"] = vendaBilheteria.FabricanteCelular.Valor;

                        linha["NivelRisco"] = vendaBilheteria.NivelRisco.Valor;

                        linha["IP"] = vendaBilheteria.IP.Valor;

                        linha["Fraude"] = vendaBilheteria.Fraude.Valor;

                        linha["AprovacaoAutomatica"] = vendaBilheteria.AprovacaoAutomatica.Valor;

                        linha["QuantidadeImpressoesInternet"] = vendaBilheteria.QuantidadeImpressoesInternet.Valor;

                        linha["VendaCancelada"] = vendaBilheteria.VendaCancelada.Valor;

                        linha["MotivoID"] = vendaBilheteria.MotivoID.Valor;

                        linha["ClienteEnderecoID"] = vendaBilheteria.ClienteEnderecoID.Valor;

                        linha["EntregaControleID"] = vendaBilheteria.EntregaControleID.Valor;

                        linha["EntregaAgendaID"] = vendaBilheteria.EntregaAgendaID.Valor;

                        linha["PdvID"] = vendaBilheteria.PdvID.Valor;

                        linha["TaxaEntregaValor"] = vendaBilheteria.TaxaEntregaValor.Valor;

                        linha["PagamentoProcessado"] = vendaBilheteria.PagamentoProcessado.Valor;

                        linha["NomeCartao"] = vendaBilheteria.NomeCartao.Valor;

                        linha["ValorSeguro"] = vendaBilheteria.ValorSeguro.Valor;

                        linha["TaxaProcessamentoValor"] = vendaBilheteria.TaxaProcessamentoValor.Valor;

                        linha["TaxaProcessamentoCancelada"] = vendaBilheteria.TaxaProcessamentoCancelada.Valor;

                        linha["Score"] = vendaBilheteria.Score.Valor;

                        linha["RetornoAccertify"] = vendaBilheteria.RetornoAccertify.Valor;

                        linha["AccertifyForceStatus"] = vendaBilheteria.AccertifyForceStatus.Valor;

                        linha["VendaBilhereriaIDTroca"] = vendaBilheteria.VendaBilhereriaIDTroca.Valor;

                        linha["CodigoRastreio"] = vendaBilheteria.CodigoRastreio.Valor;

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

                    case "CaixaID":
                        sql = "SELECT ID, CaixaID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY CaixaID";
                        break;

                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;

                    case "Senha":
                        sql = "SELECT ID, Senha FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY Senha";
                        break;

                    case "DataVenda":
                        sql = "SELECT ID, DataVenda FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY DataVenda";
                        break;

                    case "Status":
                        sql = "SELECT ID, Status FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY Status";
                        break;

                    case "TaxaEntregaID":
                        sql = "SELECT ID, TaxaEntregaID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY TaxaEntregaID";
                        break;

                    case "TaxaConvenienciaValorTotal":
                        sql = "SELECT ID, TaxaConvenienciaValorTotal FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY TaxaConvenienciaValorTotal";
                        break;

                    case "ComissaoValorTotal":
                        sql = "SELECT ID, ComissaoValorTotal FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ComissaoValorTotal";
                        break;

                    case "ValorTotal":
                        sql = "SELECT ID, ValorTotal FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ValorTotal";
                        break;

                    case "Obs":
                        sql = "SELECT ID, Obs FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;

                    case "NotaFiscalCliente":
                        sql = "SELECT ID, NotaFiscalCliente FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NotaFiscalCliente";
                        break;

                    case "NotaFiscalEstabelecimento":
                        sql = "SELECT ID, NotaFiscalEstabelecimento FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NotaFiscalEstabelecimento";
                        break;

                    case "IndiceInstituicaoTransacao":
                        sql = "SELECT ID, IndiceInstituicaoTransacao FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY IndiceInstituicaoTransacao";
                        break;

                    case "IndiceTipoCartao":
                        sql = "SELECT ID, IndiceTipoCartao FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY IndiceTipoCartao";
                        break;

                    case "NSUSitef":
                        sql = "SELECT ID, NSUSitef FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NSUSitef";
                        break;

                    case "NSUHost":
                        sql = "SELECT ID, NSUHost FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NSUHost";
                        break;

                    case "CodigoAutorizacaoCredito":
                        sql = "SELECT ID, CodigoAutorizacaoCredito FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY CodigoAutorizacaoCredito";
                        break;

                    case "BIN":
                        sql = "SELECT ID, BIN FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY BIN";
                        break;

                    case "ModalidadePagamentoCodigo":
                        sql = "SELECT ID, ModalidadePagamentoCodigo FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ModalidadePagamentoCodigo";
                        break;

                    case "ModalidadePagamentoTexto":
                        sql = "SELECT ID, ModalidadePagamentoTexto FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ModalidadePagamentoTexto";
                        break;

                    case "DDD":
                        sql = "SELECT ID, DDD FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY DDD";
                        break;

                    case "NumeroCelular":
                        sql = "SELECT ID, NumeroCelular FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NumeroCelular";
                        break;

                    case "ModelIDCelular":
                        sql = "SELECT ID, ModelIDCelular FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ModelIDCelular";
                        break;

                    case "FabricanteCelular":
                        sql = "SELECT ID, FabricanteCelular FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY FabricanteCelular";
                        break;

                    case "NivelRisco":
                        sql = "SELECT ID, NivelRisco FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NivelRisco";
                        break;

                    case "IP":
                        sql = "SELECT ID, IP FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY IP";
                        break;

                    case "Fraude":
                        sql = "SELECT ID, Fraude FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY Fraude";
                        break;

                    case "AprovacaoAutomatica":
                        sql = "SELECT ID, AprovacaoAutomatica FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY AprovacaoAutomatica";
                        break;

                    case "QuantidadeImpressoesInternet":
                        sql = "SELECT ID, QuantidadeImpressoesInternet FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY QuantidadeImpressoesInternet";
                        break;

                    case "VendaCancelada":
                        sql = "SELECT ID, VendaCancelada FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY VendaCancelada";
                        break;

                    case "MotivoID":
                        sql = "SELECT ID, MotivoID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY MotivoID";
                        break;

                    case "ClienteEnderecoID":
                        sql = "SELECT ID, ClienteEnderecoID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ClienteEnderecoID";
                        break;

                    case "EntregaControleID":
                        sql = "SELECT ID, EntregaControleID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY EntregaControleID";
                        break;

                    case "EntregaAgendaID":
                        sql = "SELECT ID, EntregaAgendaID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY EntregaAgendaID";
                        break;

                    case "PdvID":
                        sql = "SELECT ID, PdvID FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY PdvID";
                        break;

                    case "TaxaEntregaValor":
                        sql = "SELECT ID, TaxaEntregaValor FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY TaxaEntregaValor";
                        break;

                    case "PagamentoProcessado":
                        sql = "SELECT ID, PagamentoProcessado FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY PagamentoProcessado";
                        break;

                    case "NomeCartao":
                        sql = "SELECT ID, NomeCartao FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY NomeCartao";
                        break;

                    case "ValorSeguro":
                        sql = "SELECT ID, ValorSeguro FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY ValorSeguro";
                        break;

                    case "TaxaProcessamentoValor":
                        sql = "SELECT ID, TaxaProcessamentoValor FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY TaxaProcessamentoValor";
                        break;

                    case "TaxaProcessamentoCancelada":
                        sql = "SELECT ID, TaxaProcessamentoCancelada FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY TaxaProcessamentoCancelada";
                        break;

                    case "Score":
                        sql = "SELECT ID, Score FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY Score";
                        break;

                    case "RetornoAccertify":
                        sql = "SELECT ID, RetornoAccertify FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY RetornoAccertify";
                        break;

                    case "AccertifyForceStatus":
                        sql = "SELECT ID, AccertifyForceStatus FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY AccertifyForceStatus";
                        break;

                    case "VendaBilhereriaIDTroca":
                        sql = "SELECT ID, VendaBilhereriaIDTroca FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY VendaBilhereriaIDTroca";
                        break;

                    case "CodigoRastreio":
                        sql = "SELECT ID, CodigoRastreio FROM tVendaBilheteria WHERE " + FiltroSQL + " ORDER BY CodigoRastreio";
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

    #region "VendaBilheteriaException"

    [Serializable]
    public class VendaBilheteriaException : Exception
    {

        public VendaBilheteriaException() : base() { }

        public VendaBilheteriaException(string msg) : base(msg) { }

        public VendaBilheteriaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}