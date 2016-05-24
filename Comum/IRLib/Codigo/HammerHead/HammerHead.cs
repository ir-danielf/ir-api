using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using CTLib;
using IRCore.Util;
using IRLib.ClientObjects;

namespace IRLib.HammerHead
{
    public class HammerHead
    {
        private VendaBilheteria oVendaBilheteria { get; set; }
        private VendaBilheteriaFormaPagamento oVendaBilheteriaFormaPagamento { get; set; }
        public ProcessamentoPagamentos oProcessamento { get; set; }
        private DateTime TempoAguardando { get; set; }
        private EmailAccertify email { get; set; }
        public IngressoGerenciadorAccertify ingressos { get; set; }
        public SalvarLog.Events salvarlogevent { get; set; }

        public HammerHead()
        {
            LogUtil.Info(string.Format("##HammerHead.Constructor##"));

            oVendaBilheteria = new VendaBilheteria(ConfiguracaoAccertify.Instancia.Chaves.UsuarioID.Valor);
            oVendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
            oProcessamento = new ProcessamentoPagamentos();
            email = new EmailAccertify();
            ingressos = new IngressoGerenciadorAccertify();
        }

        public void ExecutarSingle(string senha, Enumeradores.RetornoAccertify retornoAccertify, int score, string action = "")
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Info(string.Format("##HammerHead.ExecutingSingle## SENHA: {0}, RETORNO_ACCERTIFY: {1}, SCORE: {2}", senha, retornoAccertify, score));

            salvarlogevent = SalvarLog.Events.Listener;

            var vendaBilheteriaID = oVendaBilheteria.BuscaID(senha.Trim());

            LogUtil.Info(string.Format("##HammerHead.ExecutarSingle.VendaBilheteriaIdRetrieved## SENHA: {0}, RETORNO_ACCERTIFY: {1}, SCORE: {2}, TEMPO_DECORRIDO: {3} ms", senha, retornoAccertify, score, sw.ElapsedMilliseconds));

            var vendas = CarregarSingle(vendaBilheteriaID);

            LogUtil.Info(string.Format("##HammerHead.ExecutarSingle.CarregarSingle## SENHA: {0}, RETORNO_ACCERTIFY: {1}, SCORE: {2}, TEMPO_DECORRIDO: {3} ms", senha, retornoAccertify, score, sw.ElapsedMilliseconds));
            oProcessamento.Single = true;
            oProcessamento.RetornoAccertify = retornoAccertify;

            var venda = vendas.FirstOrDefault();

            if (venda == null)
                return;

            venda.Score = score;
            this.EfetuarPagamento(venda, action);

            sw.Stop();
            LogUtil.Info(string.Format("##HammerHead.ExecutarSingle.Finished## SENHA: {0}, RETORNO_ACCERTIFY: {1}, SCORE: {2}, TEMPO_DECORRIDO_TOTAL: {3} ms", senha, retornoAccertify, score, sw.ElapsedMilliseconds));
        }

        private List<EstruturaVenda> CarregarSingle(int vendaBilheteriaID)
        {
            var vendas = new List<EstruturaVenda>
            {
                oVendaBilheteria.CarregarVendaParaPagamento(vendaBilheteriaID)
            };

            return vendas;
        }

        public List<EstruturaVenda> CarregarVendas(Enumeradores.Site site, string modo = "todos")
        {
            var listVendas = new List<EstruturaVenda>();

            try
            {
                listVendas = oVendaBilheteria.CarregarVendasParaPagamento(site, modo);

                LogUtil.Info(string.Format("##HammerHead.CarregandoVenda.SUCCESS## VENDAS_ENCONTRADAS: {0}", listVendas.Count));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##HammerHead.CarregandoVenda.EXCEPTION## MSG: {0}", ex.Message), ex);

                SalvarLog.SalvarErro("Não foi possível carregar a fila.", ex.Message, SalvarLog.Events.Fila);
            }
            finally
            {
                SalvarLog.Salvar("Foram encontrados : " + listVendas.Count + " registros para processar", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.Fila);
            }
            return listVendas;
        }

        public void EfetuarPagamentos(List<EstruturaVenda> listVendas, string action = "")
        {
            try
            {
                LogUtil.Debug(string.Format("##HammerHead.EfetuandoPagamentos##"));

                if (listVendas.Count == 0)
                {
                    LogUtil.Debug(string.Format("##HammerHead.EfetuandoPagamentos.FilaVazia##"));
                    return; // A fila está vazia
                }

                foreach (var venda in listVendas)
                {
                    try
                    {
                        EfetuarPagamento(venda, action);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##HammerHead.EfetuandoPagamentos.EXCEPTION## VENDA: {0}, MSG: {1}", venda.ID, ex.Message), ex);
                        SalvarLog.SalvarErro("Falha ao processar transação: " + venda.Senha, ex.Message, this.salvarlogevent);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##HammerHead.EfetuandoPagamentos.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw;
            }
        }

        public void EfetuarPagamento(EstruturaVenda venda, string action = "")
        {
            LogUtil.Debug(string.Format("##HammerHead.EfetuandoPagamento## VENDA: {0}, ACTION: {1}", venda.ID, action));
            var success = false;

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                this.ProcessarVenda(venda, action);
                success = true;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##HammerHead.EfetuandoPagamento.EXCEPTION## VENDA: {0}, MSG: {1}", venda.ID, ex.Message), ex);
                throw;
            }
            finally
            {
                sw.Stop();
                LogUtil.Debug(string.Format("##HammerHead.EfetuandoPagamento.FINISH## VENDA: {0}, SUCESS: {1}, TEMPO_DECORRIDO_TOTAL: {2} ms", venda.ID, success, sw.ElapsedMilliseconds));
            }
        }

        public void ProcessarVenda(EstruturaVenda venda, string action = "")
        {
            var sw = new Stopwatch();
            sw.Start();

            if (venda.VendaCancelada)
            {
                this.oProcessamento.RetornoAccertify = Enumeradores.RetornoAccertify.VendaJaCancelada;
            }

            var retorno = oProcessamento.ProcessarVenda(venda);

            LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));

            switch (retorno)
            {
                case Enumeradores.RetornoProcessamento.Processado:
                    this.Aprovar(venda);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.Aprovar.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
                case Enumeradores.RetornoProcessamento.CancelarAccertify:
                    this.Fraude(venda);
                    //cancelar ingressos
                    oVendaBilheteria.CancelarVenda(venda, true);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.CancelarVenda.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
                case Enumeradores.RetornoProcessamento.AguardarAccertify:
                case Enumeradores.RetornoProcessamento.Timeout:
                    this.Analisar(venda);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.Analisar.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
                case Enumeradores.RetornoProcessamento.SolicitarDocumentos:
                    this.SolicitarDocumentos(venda);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.SolicitarDocumentos.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
                case Enumeradores.RetornoProcessamento.CancelarSemFraude:
                    this.CancelarSemFraude(venda);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.CancelarSemFraude.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
                case Enumeradores.RetornoProcessamento.CartaoInvalido:
                    LogUtil.Error(string.Format("##HammerHead.ProcessandoVenda.ERROR## VENDA: {0}, RETORNO_ACCERTIFY: {1}", venda.ID, retorno));
                    break;
                case Enumeradores.RetornoProcessamento.Chargeback:
                    this.Chageback(venda);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.Chageback.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
                case Enumeradores.RetornoProcessamento.VendaJaCancelada:
                    var fraude = (action == "REJECT");
                    this.VendaJaCancelada(venda, fraude);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.VendaJaCancelada.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    oVendaBilheteria.CancelarVenda(venda, fraude);
                    break;
                case Enumeradores.RetornoProcessamento.Bypass:
                default:
                    this.AtualizarScore(venda);
                    LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.AtualizarScore.Finish## VENDA: {0}, RETORNO_ACCERTIFY: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, retorno, sw.ElapsedMilliseconds));
                    break;
            }

            var saveLog = ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor;
            LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.BeforeLog## VENDA: {0}, SALVAR_LOG: {1}, TEMPO_DECORRIDO: {2} ms", venda.ID, saveLog, sw.ElapsedMilliseconds));

            if (saveLog)
                SalvarLog.Salvar(string.Format("Venda processada HammerHead. \n Venda: {0} \nScore:{1}\nRecommendation: {2}", venda.Senha, venda.Score.ToString(), venda.RetornoAccertify.ToString()), Enumeradores.TipoEntrada.Sucesso);

            sw.Stop();
            LogUtil.Debug(string.Format("##HammerHead.ProcessandoVenda.FINISH## VENDA: {0}, TEMPO_DECORRIDO_TOTAL: {1} ms", venda.ID, sw.ElapsedMilliseconds));
        }

        private void VendaJaCancelada(EstruturaVenda venda, bool fraude = false)
        {
            try
            {
                LogUtil.Info(string.Format("##HammerHead.VendaJaCancelada## VENDA: {0}", venda.ID));

                var statusAntiFraude = fraude ? VendaBilheteria.StatusAntiFraude.Fraude : VendaBilheteria.StatusAntiFraude.Aprovado;

                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, statusAntiFraude, venda.Score, venda.RetornoAccertify);

                LogUtil.Info(string.Format("##HammerHead.VendaJaCancelada.SUCCESS## VENDA: {0}", venda.ID));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##HammerHead.VendaJaCancelada.EXCEPTION## VENDA: {0}, MSG: {1}", venda.ID, ex.Message), ex);
                SalvarLog.SalvarErro("Erro ao atualizar venda já cancelada: " + venda.ID, ex.Message, SalvarLog.Events.Transacoes);
            }
        }

        private void SolicitarDocumentos(object objVenda)
        {
            if (!(objVenda is EstruturaVenda))
                return;

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            try
            {
                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, VendaBilheteria.StatusAntiFraude.EmAnalise, venda.Score, venda.RetornoAccertify);

                EstruturaTransacoesDetalhes detalheVendas = oVendaBilheteria.AcompanhamentoIngressos(venda.ID);

                if (ConfigurationManager.AppSettings["CanaisImais"].ToString().Contains(detalheVendas.CanalID.ToString()))
                    email.EnviarSolicitacaoDocumentos_im(venda.Cliente.ID, detalheVendas, ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);
                else
                    email.EnviarSolicitacaoDocumentos(venda.Cliente.ID, detalheVendas, ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);
            }
            catch (Exception ex)
            {
                throw new Exception("Solicitar Documentos : " + ex.Message);
            }
        }

        private void Fraude(object objVenda)
        {
            if (!(objVenda is EstruturaVenda))
                return;

            var venda = (EstruturaVenda)objVenda;

            try
            {
                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, VendaBilheteria.StatusAntiFraude.Fraude, venda.Score, venda.RetornoAccertify);

                var detalheVendas = oVendaBilheteria.AcompanhamentoIngressos(venda.ID);

                LogUtil.Debug(string.Format("##HammerHead.Fraude.EnviarEmail## VENDA: {0}, MSG: {1}", venda.ID, "EnviarCancelamentoFraude"));
                email.EnviarCancelamentoFraude(venda.Cliente.ID, detalheVendas, ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);

            }
            catch (Exception ex)
            {
                SalvarLog.SalvarErro("Erro ao cancelar a Venda: " + venda.ID, ex.Message, SalvarLog.Events.Transacoes);
            }
        }

        private void Chageback(EstruturaVenda venda)
        {
            try
            {
                LogUtil.Info(string.Format("##HammerHead.Chageback## VENDA: {0}", venda.ID));

                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, VendaBilheteria.StatusAntiFraude.Aprovado, venda.Score, venda.RetornoAccertify);

                LogUtil.Info(string.Format("##HammerHead.Chageback.SUCCESS## VENDA: {0}", venda.ID));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##HammerHead.Chageback.EXCEPTION## VENDA: {0}, MSG: {1}", venda.ID, ex.Message), ex);
                SalvarLog.SalvarErro("Erro ao atualizar chargeback: " + venda.ID, ex.Message, SalvarLog.Events.Transacoes);
            }
        }

        private void CancelarSemFraude(object objVenda)
        {
            if (!(objVenda is EstruturaVenda))
                return;

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            try
            {
                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, VendaBilheteria.StatusAntiFraude.Cancelado, venda.Score, venda.RetornoAccertify);
            }
            catch (Exception ex)
            {
                SalvarLog.SalvarErro("Erro ao cancelar a Venda: " + venda.ID, ex.Message, SalvarLog.Events.Transacoes);
            }
        }

        private void Analisar(object objVenda)
        {
            if (!(objVenda is EstruturaVenda))
                return;

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            try
            {
                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, VendaBilheteria.StatusAntiFraude.EmAnalise, venda.Score, venda.RetornoAccertify);
            }
            catch (Exception ex)
            {
                throw new Exception("Analisar : " + ex.Message);
            }
        }

        private void Aprovar(object objVenda)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (!(objVenda is EstruturaVenda))
            {
                LogUtil.Debug(string.Format("##HammerHead.Aprovar.ERRO## OBJ_VENDA: {0}", objVenda));
                return;
            }

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            try
            {
                LogUtil.Debug(string.Format("##HammerHead.Aprovar## VENDA: {0}", venda.ID));

                var listaIngressos = new List<IngressoImpressao>();
                var status = VendaBilheteria.StatusAntiFraude.Aprovado;

                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, status, venda.Score, venda.RetornoAccertify);
                LogUtil.Debug(string.Format("##HammerHead.Aprovar.AtualizarStatusAntiFraudeCalled## VENDA: {0}, TEMPO_DECORRIDO: {1} ms", venda.ID, sw.ElapsedMilliseconds));

                EstruturaTransacoesDetalhes detalheVendas = oVendaBilheteria.AcompanhamentoIngressos(venda.ID);
                LogUtil.Debug(string.Format("##HammerHead.Aprovar.AcompanhamentoIngressosCalled## VENDA: {0}, TEMPO_DECORRIDO: {1} ms", venda.ID, sw.ElapsedMilliseconds));

                LogUtil.Debug(string.Format("##HammerHead.Aprovar## VENDA: {0}, IMPRESSÃO_INTERNET: {1}", venda.ID, detalheVendas.PermitirImpressaoInternet));
                if (detalheVendas.PermitirImpressaoInternet)
                {
                    listaIngressos = ingressos.PesquisaVendaBilheteria(detalheVendas.VendaBilheteriaID);
                    LogUtil.Debug(string.Format("##HammerHead.Aprovar.PesquisaVendaBilheteriaCalled## VENDA: {0}, TEMPO_DECORRIDO: {1} ms", venda.ID, sw.ElapsedMilliseconds));
                }

                if (listaIngressos.Count > 0)
                {
                    LogUtil.Debug(string.Format("##HammerHead.Aprovar.EnviarEmail## VENDA: {0}, MSG: {1}", venda.ID, "EnviarConfirmacaoCompraComIngresso"));

                    if (
                        ConfigurationManager.AppSettings["CanaisImais"].ToString()
                            .Contains(detalheVendas.CanalID.ToString()))
                        email.EnviarConfirmacaoCompraComIngresso_im(venda.Cliente.ID, detalheVendas, listaIngressos,
                            ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);
                    else
                        email.EnviarConfirmacaoCompraComIngresso(venda.Cliente.ID, detalheVendas, listaIngressos,
                            ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);
                }
                else
                {
                    LogUtil.Debug(string.Format("##HammerHead.Aprovar.EnviarEmail## VENDA: {0}, MSG: {1}", venda.ID,
                        "EnviarConfirmacaoCompra"));

                    if (
                        ConfigurationManager.AppSettings["CanaisImais"].ToString()
                            .Contains(detalheVendas.CanalID.ToString()))
                        email.EnviarConfirmacaoCompra_im(venda.Cliente.ID, detalheVendas,
                            ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);
                    else
                        email.EnviarConfirmacaoCompra(venda.Cliente.ID, detalheVendas,
                            ConfiguracaoHammerHead.Instancia.Configuracao.AmbienteDeTestes.Valor);
                }

                LogUtil.Debug(string.Format("##HammerHead.Aprovar.EmailConfirmacaoEnviado## VENDA: {0}, TEMPO_DECORRIDO: {1} ms", venda.ID, sw.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                LogUtil.Error(
                    string.Format("##HammerHead.Aprovar.EXCEPTION## VENDA: {0}, MSG: {1}", venda.ID, ex.Message), ex);
                throw new Exception("Erro ao Aprovar : " + ex.Message);
            }
            finally
            {
                sw.Stop();
                LogUtil.Debug(string.Format("##HammerHead.Aprovar.FINISHED## VENDA: {0}, TEMPO_DECORRIDO: {1}", venda.ID, sw.ElapsedMilliseconds));
            }
        }

        private void AtualizarVenda(EstruturaVenda venda)
        {
            BD bd = new BD();
            try
            {
                bd.IniciarTransacao();

                this.AtualizarVendaBilheteria(bd, venda);
                this.AtualizarFormaPagamento(bd, venda);

                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                SalvarLog.SalvarErro("Falha ao atualizar a venda", ex.Message, SalvarLog.Events.Transacoes);
            }
            finally
            {
                bd.Fechar();
            }
        }

        private void AtualizarVendaBilheteria(BD bd, EstruturaVenda venda)
        {
            try
            {
                string nota = string.Empty;
                if (oProcessamento.oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                    nota = AdyenStatic.Fields.NotaFiscal;
                else
                    nota = oProcessamento.oPagamento.oSitef.CupomFiscal;

                oVendaBilheteria.AtualizarPagamentoProcessado(bd, venda.ID, nota);
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar("Pagamento e Nota fiscal Atualizados.", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.Transacoes);
            }
            catch (Exception ex)
            {
                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.SalvarErro("Não foi possível atualizar o Pagamento e Nota Fiscal.", ex.Message, SalvarLog.Events.Transacoes);
                throw ex;
            }

        }

        private void AtualizarFormaPagamento(BD bd, EstruturaVenda venda)
        {
            oVendaBilheteriaFormaPagamento.Ler(venda.FormaPagamento.VendaBilheteriaFormaPagamentoID);

            if (oProcessamento.oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
            {
                oVendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oProcessamento.oPagamento.oAdyen.CodigoReferencia;
                oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oProcessamento.oPagamento.oAdyen.CodigoAutenticacao ?? oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor;
                oVendaBilheteriaFormaPagamento.Cupom.Valor = AdyenStatic.Fields.NotaFiscal;
            }
            else
            {
                oVendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oProcessamento.oPagamento.oSitef.ID;
                oVendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oProcessamento.oPagamento.oSitef.CodigoRespostaSitefVenda;
                oVendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oProcessamento.oPagamento.oSitef.MensagemFinaliza;
                oVendaBilheteriaFormaPagamento.HoraTransacao.Valor = oProcessamento.oPagamento.oSitef.HoraTransacao;
                oVendaBilheteriaFormaPagamento.DataTransacao.Valor = oProcessamento.oPagamento.oSitef.DataTransacao;
                oVendaBilheteriaFormaPagamento.CodigoIR.Valor = oProcessamento.oPagamento.oSitef.CodigoIR;
                oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oProcessamento.oPagamento.oSitef.NumeroAutorizacao;
                oVendaBilheteriaFormaPagamento.NSUHost.Valor = oProcessamento.oPagamento.oSitef.NSUHost;
                oVendaBilheteriaFormaPagamento.NSUSitef.Valor = oProcessamento.oPagamento.oSitef.NSUSitef;
                oVendaBilheteriaFormaPagamento.Cupom.Valor = oProcessamento.oPagamento.oSitef.CupomFiscal;
                oVendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oProcessamento.oPagamento.oSitef.DadosConfirmacao;
                oVendaBilheteriaFormaPagamento.Rede.Valor = oProcessamento.oPagamento.oSitef.RedeRetorno;
                oVendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oProcessamento.oPagamento.oSitef.CodigoRespostaSitefFinaliza;

                if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                    SalvarLog.Salvar("VendaBilheteriaFormaPagamento Atualizada\n NotaFiscal: " + oProcessamento.oPagamento.oSitef.CupomFiscal, Enumeradores.TipoEntrada.Sucesso, SalvarLog.Events.Transacoes);
            }
            oVendaBilheteriaFormaPagamento.Atualizar(bd);
        }

        private void AtualizarScore(object objVenda)
        {
            if (!(objVenda is EstruturaVenda))
                return;

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            BD bd = new BD();
            try
            {
                oVendaBilheteria.AtualizarScore(bd, venda.ID, venda.Score, venda.RetornoAccertify.ToString());
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
    }
}