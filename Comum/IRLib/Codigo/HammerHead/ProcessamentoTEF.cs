using IRLib.ClientObjects;
using System;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using IRCore.Util;

namespace IRLib.HammerHead
{
    public class ProcessamentoPagamentos
    {
        public EstruturaPagamento oPagamento { get; set; }

        private EstruturaVenda Venda { get; set; }

        public ProcessamentoPagamentos()
        {
            oPagamento = new EstruturaPagamento();
        }

        public bool Single { get; set; }

        public Enumeradores.RetornoAccertify RetornoAccertify { get; set; }

        public Enumeradores.RetornoProcessamento ProcessarVenda(EstruturaVenda venda)
        {
            LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda## VENDA: {0}", venda));

            this.Venda = venda;
            this.MontarPagamento();

            var retorno = this.Processar();
            venda.RetornoAccertify = this.RetornoAccertify;

            return retorno;
        }

        private Enumeradores.RetornoProcessamento Processar()
        {
            try
            {
                LogUtil.Debug(string.Format("##ProcessamentoTEF.ProcessandoPagamentoEAnalise## CLIENTE: {0}", this.oPagamento.ClienteID));

                if (ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor)
                {
                    SalvarLog.Salvar("Ambiente de testes ativo!", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.AmbienteTesteAtivo);

                    var rnd = new Random();
                    Thread.Sleep(rnd.Next(2000));
                    var processamento = rnd.Next() % 2 == 0 ? Enumeradores.RetornoProcessamento.Processado : Enumeradores.RetornoProcessamento.CancelarAccertify;

                    LogUtil.Debug(string.Format("##ProcessamentoTEF.ProcessandoPagamentoEAnalise.AmbienteDeTestes## CLIENTE: {0}, RETORNO: {1}, GATEWAY: {2}", this.oPagamento.ClienteID, processamento, this.oPagamento.TipoPagamento));

                    if (this.oPagamento.TipoPagamento != EstruturaPagamento.enumTipoPagamento.Adyen) return processamento;

                    if (processamento == Enumeradores.RetornoProcessamento.Processado && this.oPagamento.FormaPagamentoOrigem == 0)
                    {
                        LogUtil.Debug(string.Format("##ProcessamentoTEF.ProcessandoPagamentoEAnalise.AmbienteDeTestes.PROCESSADO## CLIENTE: {0}, RETORNO: {1}, MSG: {2}", this.oPagamento.ClienteID, processamento, "Capturando pagamento na Adyen"));
                        oPagamento.oAdyen.CapturarPagamento();
                    }
                    else
                    {
                        LogUtil.Debug(string.Format("##ProcessamentoTEF.ProcessandoPagamentoEAnalise.AmbienteDeTestes.RECUSADO## CLIENTE: {0}, RETORNO: {1}, MSG: {2}", this.oPagamento.ClienteID, processamento, "Cancelando pagamento na Adyen"));
                        oPagamento.oAdyen.CancelarPagamento();
                    }

                    return processamento;
                }
                else
                {
                    switch (this.oPagamento.TipoPagamento)
                    {
                        case EstruturaPagamento.enumTipoPagamento.TEF:
                            return this.ProcessarTEF();
                        case EstruturaPagamento.enumTipoPagamento.Adyen:
                            return this.ProcessarAdyen();
                        case EstruturaPagamento.enumTipoPagamento.Paypal:
                            RetornoAccertify = Enumeradores.RetornoAccertify.Bypass;
                            return Enumeradores.RetornoProcessamento.Bypass;
                        default:
                            return Enumeradores.RetornoProcessamento.Processado;
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoAdyen.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message), ex);
                throw;
            }
            catch (System.Web.Services.Protocols.SoapHeaderException ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoAdyen.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message), ex);
                throw;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoAdyen.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message), ex);
                throw;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoPagamentoEAnalise.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message), ex);

                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Transação Inválida.", "Venda: " + Venda.ID + "\nException: " + ex.Message, SalvarLog.Events.Transacoes);

                return Enumeradores.RetornoProcessamento.CartaoInvalido;
            }
        }

        private Enumeradores.RetornoProcessamento ProcessarAdyen()
        {
            try
            {
                var pagamentoEfetuado = Venda.FormaPagamento.NotaFiscal.Contains(AdyenStatic.Fields.NotaFiscal);

                LogUtil.Debug(string.Format("##ProcessamentoTEF.ProcessandoAdyen## CLIENTE: {0}, PAGAMENTO_REALIZADO: {1}", this.oPagamento.ClienteID, pagamentoEfetuado));

                if (!pagamentoEfetuado)
                    oPagamento.oAdyen.EfetuarPagamento();
                else
                    oPagamento.oAdyen.CodigoReferencia = Venda.FormaPagamento.CodigoResposta;

                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar("Cobrança Adyen (Inicial) HammerHead OK", Enumeradores.TipoEntrada.Sucesso, SalvarLog.Events.Transacoes);

                this.ConsultarAccertify();

                Enumeradores.RetornoProcessamento retornoProcessamento = 0;

                LogUtil.Info(string.Format("##ProcessamentoTEF.ProcessandoAdyen.RETORNO## CLIENTE: {0}, RETORNO_ACCERTIFY: {1}", this.oPagamento.ClienteID, RetornoAccertify));
                switch (RetornoAccertify)
                {
                    case Enumeradores.RetornoAccertify.Bypass:
                    case Enumeradores.RetornoAccertify.Aceitar:
                        if (this.oPagamento.FormaPagamentoOrigem == 0) oPagamento.oAdyen.CapturarPagamento();
                        retornoProcessamento = Enumeradores.RetornoProcessamento.Processado;
                        break;
                    case Enumeradores.RetornoAccertify.AcompanhamentoComCliente:
                        retornoProcessamento = Enumeradores.RetornoProcessamento.SolicitarDocumentos;
                        break;
                    case Enumeradores.RetornoAccertify.AguardarReview:
                        retornoProcessamento = Enumeradores.RetornoProcessamento.AguardarAccertify;
                        break;
                    case Enumeradores.RetornoAccertify.Indefinido:
                        LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoAdyen.ERROR## CLIENTE: {0}, RETORNO_ACCERTIFY: {1}", this.oPagamento.ClienteID, RetornoAccertify));
                        break;
                    case Enumeradores.RetornoAccertify.CancelarSemFraude:
                        retornoProcessamento = Enumeradores.RetornoProcessamento.CancelarSemFraude;
                        break;
                    case Enumeradores.RetornoAccertify.CancelarAltoRisco:
                    case Enumeradores.RetornoAccertify.CancelarTempoLimiteExcedido:
                    case Enumeradores.RetornoAccertify.CancelarVendaInvalida:
                        oPagamento.oAdyen.CancelarPagamento();
                        retornoProcessamento = Enumeradores.RetornoProcessamento.CancelarAccertify;
                        break;
                    case Enumeradores.RetornoAccertify.Chargeback:
                        retornoProcessamento = Enumeradores.RetornoProcessamento.Chargeback;
                        break;
                    case Enumeradores.RetornoAccertify.VendaJaCancelada:
                        oPagamento.oAdyen.CancelarPagamento();
                        retornoProcessamento = Enumeradores.RetornoProcessamento.VendaJaCancelada;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return retornoProcessamento;

            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoAdyen.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message), ex);

                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Pagamento Adyen", "Não foi possível processar o pagamento.\nVenda: " + Venda.ID + "\nMotivo: " + ex.Message, SalvarLog.Events.Transacoes);

                return Enumeradores.RetornoProcessamento.CartaoInvalido;
            }
        }

        private Enumeradores.RetornoProcessamento ProcessarTEF()
        {
            try
            {
                LogUtil.Debug(string.Format("##ProcessamentoTEF.ProcessandoTEF## CLIENTE: {0}", this.oPagamento.ClienteID));

                this.ConsultarAccertify();

                switch (RetornoAccertify)
                {
                    case Enumeradores.RetornoAccertify.Bypass:
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
                    case Enumeradores.RetornoAccertify.Chargeback:
                        return Enumeradores.RetornoProcessamento.Chargeback;
                    case Enumeradores.RetornoAccertify.VendaJaCancelada:
                        return Enumeradores.RetornoProcessamento.VendaJaCancelada;
                    default:
                        return Enumeradores.RetornoProcessamento.Processado;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ProcessandoTEF.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message), ex);

                if (ex is VendaCanceladaException)
                {
                    if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                        SalvarLog.Salvar(ex.Message, Enumeradores.TipoEntrada.Alerta, SalvarLog.Events.Listener);

                    return Enumeradores.RetornoProcessamento.VendaCancelada;
                }

                if (ex is TimeoutException)
                {
                    if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                        SalvarLog.SalvarErro("Ocorreu um timeout na execução do HammerHead",
                            oPagamento.oSitef.RetornoTEF, SalvarLog.Events.Transacoes);

                    return Enumeradores.RetornoProcessamento.Timeout;
                }

                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Transação Inválida.", oPagamento.oSitef.RetornoTEF + "\nException: " + ex.Message, SalvarLog.Events.Transacoes);

                return Enumeradores.RetornoProcessamento.CartaoInvalido;
            }
        }

        private void ConsultarAccertify()
        {
            LogUtil.Debug(string.Format("##ProcessamentoTEF.ConsultandoAccertify## CLIENTE: {0}, RETURN_SINGLE: {1}, PAGAMENTO_ORIGEM: {2}", this.oPagamento.ClienteID, this.Single, this.oPagamento.FormaPagamentoOrigem));

            if (Single || this.oPagamento.FormaPagamentoOrigem > 0 || this.Venda.VendaCancelada) //Já foi preenchido!
            {
                if (this.oPagamento.FormaPagamentoOrigem > 0) RetornoAccertify = Enumeradores.RetornoAccertify.Bypass;
                return;
            }

            if (!ConfiguracaoAccertify.Instancia.Chaves.Ativo.Valor)
            {
                LogUtil.Debug(string.Format("##ProcessamentoTEF.ConsultarAccertify.AccertifyDesativado## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, "Aceitar automático"));

                RetornoAccertify = Enumeradores.RetornoAccertify.Aceitar;
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar("Accertify inativa!. Aceitar automático!", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.Accertify);

                return;
            }

            if (Venda.DataVenda.Date >= DateTime.Now.AddDays(ConfiguracaoAdyen.Instancia.Chaves.DiasProcessamento.Valor))
            {
                if (Venda.Score == 0)
                    RetornoAccertify = Enumeradores.RetornoAccertify.CancelarVendaInvalida; //Aconteceu erro ao processar a venda na accertify (provavel nem ter sido enviado), se passar muito tempo, cancela por venda inválida
                else if (Venda.Score <= ConfiguracaoAccertify.Instancia.Chaves.ScoreAceitarTempoExcedido.Valor)
                    RetornoAccertify = Enumeradores.RetornoAccertify.Aceitar;
                else
                    RetornoAccertify = Enumeradores.RetornoAccertify.CancelarTempoLimiteExcedido;

                LogUtil.Debug(string.Format("##ProcessamentoTEF.ConsultandoAccertify.ValidacaoData## CLIENTE: {0}, DATA_VENDA: {1} >= DATA_DIAS_PROC: {2}, RETORNO_ACCERTIFY: {3}", this.oPagamento.ClienteID, Venda.DataVenda.Date, DateTime.Now.AddDays(ConfiguracaoAdyen.Instancia.Chaves.DiasProcessamento.Valor), RetornoAccertify));

                return;
            }

            var sw = new Stopwatch();
            sw.Start();

            var xmlRetorno = new XmlDocument();

            try
            {
                LogUtil.Debug(string.Format("##ProcessamentoTEF.ConsultarAccertify.GerandoXMLVenda## CLIENTE: {0}", this.oPagamento.ClienteID));
                var xml = Accertify.GerarXMLVenda(Venda);

                LogUtil.Debug(string.Format("##ProcessamentoTEF.ConsultarAccertify.ChamandoWSAccertify## CLIENTE: {0}", this.oPagamento.ClienteID));

                var stringXml = Utilitario.HTTPPostXML(ConfiguracaoAccertify.Instancia.Chaves.URL.Valor, xml.InnerXml, ConfiguracaoAccertify.Instancia.Chaves.Usuario.Valor, ConfiguracaoAccertify.Instancia.Chaves.Senha.Valor);

                xmlRetorno.LoadXml(stringXml);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ProcessamentoTEF.ConsultarAccertify.EXCEPTION## CLIENTE: {0}, MSG: {1}", this.oPagamento.ClienteID, ex.Message));
                throw ex;
            }

            var score = Convert.ToInt32(xmlRetorno.GetElementsByTagName("total-score")[0].InnerText);
            var recommendation = xmlRetorno.GetElementsByTagName("recommendation-code")[0].InnerText;

            Venda.Score = score;

            Venda.RetornoAccertify = Accertify.ParseRetorno(recommendation);// RetornoAccertify;
            RetornoAccertify = Venda.RetornoAccertify;

            LogUtil.Debug(string.Format("##ProcessamentoTEF.ConsultarAccertify.SUCCESS## CLIENTE: {0}, SCORE: {1}, RECOMMENDATION: {2}, TEMPO_DECORRIDO_ACCERTIFY: {3} ms", this.oPagamento.ClienteID, score, recommendation, sw.ElapsedMilliseconds));
        }

        private void MontarPagamento()
        {
            try
            {
                EstruturaPagamento.enumTipoPagamento ambiente;

                if (ConfiguracaoAdyen.Instancia.Chaves.Ativo.Valor && Venda.FormaPagamento.NotaFiscal.Contains(AdyenStatic.Fields.NotaFiscal))
                {
                    ambiente = EstruturaPagamento.enumTipoPagamento.Adyen;
                }
                else if (Venda.FormaPagamento.BandeiraID == Convert.ToInt32(Cartao.enumBandeira.PayPal))
                {
                    ambiente = EstruturaPagamento.enumTipoPagamento.Paypal;
                }
                else
                {
                    ambiente = EstruturaPagamento.enumTipoPagamento.TEF;
                }

                try
                {
                    if (Venda.Cartao.NumeroCartao.Length > 0)
                        Venda.Cartao.NumeroCartao = Criptografia.Crypto.Decriptografar(Venda.Cartao.NumeroCartao, ConfiguracaoCriptografia.Instancia.Chaves.Cartao.Valor);

                    if (Venda.Cartao.DataVencimento.Length > 0)
                        Venda.Cartao.DataVencimento = Criptografia.Crypto.Decriptografar(Venda.Cartao.DataVencimento, ConfiguracaoCriptografia.Instancia.Chaves.Data.Valor);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##HammerHead.MontarPagamento.CriptografiaCartao.EXCEPTION## VENDA: {0}, MSG: {1}", Venda.ID, ex.Message), ex);
                }

                oPagamento = new EstruturaPagamento
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
                    FormaPagamentoOrigem = Venda.FormaPagamento.VendaBilheteriaFormaPagamentoIDOrigem
                };

                if (ambiente == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    oPagamento.oAdyen.CodigoReferencia = Venda.FormaPagamento.CodigoResposta;
                }

                LogUtil.Debug(string.Format("##HammerHead.MontarPagamento.SUCCESS## VENDA: {0}, CLIENTE: {1}", Venda.ID, Venda.Cliente.ID));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##HammerHead.MontarPagamento.EXCEPTION## VENDA: {0}, MSG: {1}", Venda.ID, ex.Message), ex);
                throw new Exception("Erro ao MontarPagamento: " + ex.Message);
            }
        }
    }
}
