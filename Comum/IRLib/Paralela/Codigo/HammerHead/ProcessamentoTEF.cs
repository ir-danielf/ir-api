using IRLib.Paralela.ClientObjects;
using System;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace IRLib.Paralela.HammerHead
{

    public class ProcessamentoPagamentos
    {
        public EstruturaPagamento oPagamento { get; set; }

        private double LimiteUtilizacao { get { return ConfiguracaoHammerHead.Instancia.Configuracao.LimiteUtilizacaoCanais.Valor; } }

        private int IntervaloRequisicoes { get { return ConfiguracaoHammerHead.Instancia.Configuracao.IntervaloRequisicoes.Valor; } } //Tempo a esperar entre requisicões

        private int IntervaloRequisicoesTimeout { get { return ConfiguracaoHammerHead.Instancia.Configuracao.IntervaloRequisicoesTimeOu.Valor; } } //Tempo a esperar depois de um timeout

        private int TempoEstimadoExecucaoMS { get { return ConfiguracaoHammerHead.Instancia.Configuracao.TempoEstimadoExecucaoMS.Valor; } } //Este é o tempo estimado da execução das chamadas ao TEF, caso seja SUPERIOR a X, será considerado timeout

        private double UtilizacaoCanal { get; set; }

        private DateTime UltimaRequisicao { get; set; }

        public bool TimeOut { get { return this.UtilizacaoCanal > 100; } }

        private EstruturaVenda Venda { get; set; }

        public ProcessamentoPagamentos()
        {
            oPagamento = new EstruturaPagamento();
        }

        public bool Single { get; set; }

        public Enumeradores.RetornoAccertify RetornoAccertify { get; set; }

        public bool PodeProcessar()
        {
            try
            {
                if (Single)
                    return true;

                if (TimeOut && DateTime.Now.Subtract(UltimaRequisicao).Seconds < IntervaloRequisicoesTimeout)
                    return false;

                if (UtilizacaoCanal >= LimiteUtilizacao && UltimaRequisicao != DateTime.MinValue && DateTime.Now.Subtract(UltimaRequisicao).Seconds < IntervaloRequisicoes)
                    return false;

                UltimaRequisicao = DateTime.Now;

                return true;
            }
            catch (Exception ex)
            {
                SalvarLog.SalvarErro("Não foi possível determinar a utilização do link.", ex.Message, SalvarLog.Events.UtilizacaoLink);
                return false;
            }
        }

        public Enumeradores.RetornoProcessamento ProcessarVenda(EstruturaVenda venda)
        {
            this.Venda = venda;

            this.MontarPagamento();

            venda.RetornoAccertify = this.RetornoAccertify;
            return this.Processar();
        }

        private Enumeradores.RetornoProcessamento Processar()
        {
            DateTime inicio = DateTime.Now;
            try
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor)
                {
                    SalvarLog.Salvar("Ambiente de testes ativo!", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.AmbienteTesteAtivo);

                    Random rnd = new Random();
                    Thread.Sleep(rnd.Next(2000));
                    if (rnd.Next() % 2 == 0)
                        return Enumeradores.RetornoProcessamento.Processado;
                    else
                        return Enumeradores.RetornoProcessamento.CartaoInvalido;
                }
                else
                {
                    switch (this.oPagamento.TipoPagamento)
                    {
                        case EstruturaPagamento.enumTipoPagamento.TEF:
                            return this.ProcessarTEF();
                        case EstruturaPagamento.enumTipoPagamento.Adyen:
                            return this.ProcessarAdyen();
                        default:
                            return Enumeradores.RetornoProcessamento.Processado;
                    }
                }
            }

            catch (Exception ex)
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Transação Inválida.", "Venda: " + Venda.ID + "\nException: " + ex.Message, SalvarLog.Events.Transacoes);

                return Enumeradores.RetornoProcessamento.CartaoInvalido;
            }
            finally
            {
                DateTime fim = DateTime.Now;
                this.AtribuirProcessamento(fim.Subtract(inicio).TotalMilliseconds);
            }
        }

        private Enumeradores.RetornoProcessamento ProcessarAdyen()
        {
            try
            {
                if (string.Compare(Venda.FormaPagamento.NotaFiscal, IRLib.AdyenStatic.Fields.NotaFiscal) != 0)
                    oPagamento.oAdyen.EfetuarPagamento();
                else
                    oPagamento.oAdyen.CodigoReferencia = Venda.FormaPagamento.CodigoResposta;

                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar("Cobrança Adyen (Inicial) HammerHead OK", Enumeradores.TipoEntrada.Sucesso, SalvarLog.Events.Transacoes);


                this.ConsultarAccertify();

                oPagamento.oAdyen.CapturarPagamento();
                return Enumeradores.RetornoProcessamento.Processado;

            }
            catch (Exception ex)
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Pagamento Adyen", "Não foi possível processar o pagamento.\nVenda: " + Venda.ID + "\nMotivo: " + ex.Message, SalvarLog.Events.Transacoes);

                if (string.Compare(Venda.FormaPagamento.NotaFiscal, IRLib.AdyenStatic.Fields.NotaFiscal) == 0)
                    oPagamento.oAdyen.CancelarPagamento();

                return Enumeradores.RetornoProcessamento.CartaoInvalido;
            }
        }

        private Enumeradores.RetornoProcessamento ProcessarTEF()
        {
            try
            {
                this.ConsultarAccertify();

                switch (RetornoAccertify)
                {
                    case Enumeradores.RetornoAccertify.Aceitar:
                        return Enumeradores.RetornoProcessamento.Processado;
                    case Enumeradores.RetornoAccertify.AguardarReview:
                        return Enumeradores.RetornoProcessamento.AguardarAccertify;
                    case Enumeradores.RetornoAccertify.CancelarAltoRisco:
                    case Enumeradores.RetornoAccertify.CancelarTempoLimiteExcedido:
                    case Enumeradores.RetornoAccertify.CancelarVendaInvalida:
                        return Enumeradores.RetornoProcessamento.CancelarAccertify;
                    case Enumeradores.RetornoAccertify.AcompanhamentoComCliente:
                        return Enumeradores.RetornoProcessamento.SolicitarDocumentos;
                    case Enumeradores.RetornoAccertify.CancelarSemFraude:
                        return Enumeradores.RetornoProcessamento.CancelarSemFraude;
                    default:
                        return Enumeradores.RetornoProcessamento.Processado;
                }
            }
            catch (VendaCanceladaException ex)
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar(ex.Message, Enumeradores.TipoEntrada.Alerta, SalvarLog.Events.Listener);

                return Enumeradores.RetornoProcessamento.VendaCancelada;
            }
            catch (TimeoutException)
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Ocorreu um timeout na execução do HammerHead", oPagamento.oSitef.RetornoTEF, SalvarLog.Events.Transacoes);

                return Enumeradores.RetornoProcessamento.Timeout;
            }
            catch (Exception ex)
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Transação Inválida.", oPagamento.oSitef.RetornoTEF + "\nException: " + ex.Message, SalvarLog.Events.Transacoes);

                return Enumeradores.RetornoProcessamento.CartaoInvalido;
            }
        }

        private void ConsultarAccertify()
        {
            if (Single) //Já foi preenchido!
                return;

            if (!ConfiguracaoAccertify.Instancia.Chaves.Ativo.Valor)
            {
                RetornoAccertify = Enumeradores.RetornoAccertify.Aceitar;
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar("Accertify inativa!. Aceitar automático!", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.Accertify);

                return;
            }

            if (Venda.DataVenda.Date >= DateTime.Now.AddDays(IRLib.ConfiguracaoAdyen.Instancia.Chaves.DiasProcessamento.Valor))
            {
                if (Venda.Score == 0)
                    RetornoAccertify = Enumeradores.RetornoAccertify.CancelarVendaInvalida; //Aconteceu erro ao processar a venda na accertify (provavel nem ter sido enviado), se passar muito tempo, cancela por venda inválida
                else if (Venda.Score <= ConfiguracaoAccertify.Instancia.Chaves.ScoreAceitarTempoExcedido.Valor)
                    RetornoAccertify = Enumeradores.RetornoAccertify.Aceitar;
                else
                    RetornoAccertify = Enumeradores.RetornoAccertify.CancelarTempoLimiteExcedido;

                return;
            }

            XmlDocument xml = null;
            XmlDocument xmlRetorno = new XmlDocument();

            try
            {
                xml = Accertify.GerarXMLVenda(Venda);

                xmlRetorno.LoadXml(Utilitario.HTTPPostXML(ConfiguracaoAccertify.Instancia.Chaves.URL.Valor, xml.InnerXml, ConfiguracaoAccertify.Instancia.Chaves.Usuario.Valor, ConfiguracaoAccertify.Instancia.Chaves.Senha.Valor));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            int score = Convert.ToInt32(xmlRetorno.GetElementsByTagName("total-score")[0].InnerText);
            string recomendation = xmlRetorno.GetElementsByTagName("recommendation-code")[0].InnerText;

            Venda.Score = score;

            var retorno = (IRLib.Paralela.RetornoAccertify.Recommendation)Enum.Parse(typeof(IRLib.Paralela.RetornoAccertify.Recommendation), recomendation);

            Venda.RetornoAccertify = Accertify.ParseRetorno(recomendation);// RetornoAccertify;
            RetornoAccertify = Venda.RetornoAccertify;
        }

        private void AtribuirProcessamento(double tempoEmMS)
        {
            this.UtilizacaoCanal = (tempoEmMS * 100) / TempoEstimadoExecucaoMS;
        }

        private void MontarPagamento()
        {
            try
            {
                var ambiente = IRLib.ConfiguracaoAdyen.Instancia.Chaves.Ativo.Valor && 
                    (Venda.FormaPagamento.BandeiraID != Convert.ToInt32(Sitef.enumBandeira.ValeCultura) &&
                    Venda.FormaPagamento.BandeiraID != Convert.ToInt32(Sitef.enumBandeira.EloCultura))
                    ? EstruturaPagamento.enumTipoPagamento.Adyen 
                    : EstruturaPagamento.enumTipoPagamento.TEF;

                try
                {
                    if (Venda.Cartao.NumeroCartao.Length > 0)
                        Venda.Cartao.NumeroCartao = Criptografia.Crypto.Decriptografar(Venda.Cartao.NumeroCartao, ConfiguracaoCriptografia.Instancia.Chaves.Cartao.Valor);

                    if (Venda.Cartao.DataVencimento.Length > 0)
                        Venda.Cartao.DataVencimento = Criptografia.Crypto.Decriptografar(Venda.Cartao.DataVencimento, ConfiguracaoCriptografia.Instancia.Chaves.Data.Valor);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                oPagamento = new EstruturaPagamento()
                {
                    TipoPagamento = ambiente,
                    ClienteID = Venda.Cliente.ID,
                    ClienteNome = Venda.Cliente.Nome,
                    ClienteEmail = Venda.Cliente.Email,
                    ValorTotal = Venda.ValorTotal,
                    NumeroCartao = Venda.Cartao.NumeroCartao,
                    DataVencimento = Venda.Cartao.DataVencimento,
                    CodigoSeguranca = Venda.Cartao.CodigoSeguranca,
                    NomeCartao = Venda.FormaPagamento.NomeCartao,
                    Parcelas = Venda.FormaPagamento.Parcelas,
                    SessionID = Guid.NewGuid().ToString(),
                    IP = Venda.IP,
                    Bandeira = Venda.FormaPagamento.BandeiraID.ToString(),
                    FormaPagamentoID = Venda.FormaPagamento.ID,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
