using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace IRLib.Paralela.HammerHead
{
    public class HammerHead
    {
        private VendaBilheteria oVendaBilheteria { get; set; }
        private VendaBilheteriaFormaPagamento oVendaBilheteriaFormaPagamento { get; set; }
        public ProcessamentoPagamentos oProcessamento { get; set; }
        private List<EstruturaVenda> Vendas { get; set; }
        private DateTime TempoAguardando { get; set; }
        private EmailAccertify email { get; set; }
        public IngressoGerenciadorAccertify ingressos { get; set; }
        public SalvarLog.Events salvarlogevent { get; set; }

        public HammerHead()
        {
            oVendaBilheteria = new VendaBilheteria(ConfiguracaoAccertify.Instancia.Chaves.UsuarioID.Valor);
            oVendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
            oProcessamento = new ProcessamentoPagamentos();
            Vendas = new List<EstruturaVenda>();
            email = new EmailAccertify();
            ingressos = new IngressoGerenciadorAccertify();
        }

        public void Executar()
        {
            if (!this.CarregarFila())
                return;

            this.salvarlogevent = SalvarLog.Events.HammerHead;

            var transacoes = this.Vendas.Count;
            var inicio = DateTime.Now;

            this.EfetuarPagamentos();
            var tempoExecucao = DateTime.Now - inicio;

            SalvarLog.Salvar("Fila de " + transacoes + " processada em " + tempoExecucao.TotalSeconds + ".", Enumeradores.TipoEntrada.Informacao);
        }

        public void ExecutarSingle(string senha, Enumeradores.RetornoAccertify retornoAccertify, int score)
        {
            this.salvarlogevent = SalvarLog.Events.Listener;

            int vendaBilheteriaID = oVendaBilheteria.BuscaID(senha.Trim());

            this.CarregarSingle(vendaBilheteriaID);
            this.oProcessamento.Single = true;
            this.oProcessamento.RetornoAccertify = retornoAccertify;
            this.Vendas.FirstOrDefault().Score = score;
            this.EfetuarPagamentos();
        }

        private void CarregarSingle(int vendaBilheteriaID)
        {
            Vendas = new List<EstruturaVenda>()
            { 
                oVendaBilheteria.CarregarVendaParaPagamento(vendaBilheteriaID) 
            };
        }

        private bool CarregarFila()
        {
            try
            {
                if (Vendas.Count > 0)
                {
                    SalvarLog.Salvar("A fila não está vazia!!!!", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.HammerHead);
                    return false;
                }

                Vendas = oVendaBilheteria.CarregarVendasParaPagamento();
                return true;
            }
            catch (Exception ex)
            {
                SalvarLog.SalvarErro("Não foi possível carregar a fila.", ex.Message, SalvarLog.Events.Fila);
                return false;
            }
            finally
            {
                SalvarLog.Salvar("Foram encontrados : " + Vendas.Count + " registros para processar", Enumeradores.TipoEntrada.Informacao, SalvarLog.Events.Fila);
            }
        }

        private void EfetuarPagamentos()
        {
            try
            {
                if (Vendas.Count == 0)
                    return; // A fila está vazia

                while (Vendas.Count > 0)
                {
                    EstruturaVenda venda = Vendas[0];

                    try
                    {
                        Enumeradores.RetornoProcessamento retorno = oProcessamento.ProcessarVenda(venda);

                        switch (retorno)
                        {
                            case Enumeradores.RetornoProcessamento.Processado:
                                this.Aprovar(venda);
                                break;
                            case Enumeradores.RetornoProcessamento.CancelarAccertify:
                                this.Fraude(venda);
                                break;
                            case Enumeradores.RetornoProcessamento.AguardarAccertify:
                            case Enumeradores.RetornoProcessamento.Timeout:
                                this.Analisar(venda);
                                break;
                            case Enumeradores.RetornoProcessamento.SolicitarDocumentos:
                                this.SolicitarDocumentos(venda);
                                break;
                            case Enumeradores.RetornoProcessamento.CancelarSemFraude:
                                this.CancelarSemFraude(venda);
                                break;
                            default:
                                this.AtualizarScore(venda);
                                break;
                        }

                        if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLog.Valor)
                            SalvarLog.Salvar(string.Format("Venda processada HammerHead. \n Venda: {0} \nScore:{1}\nRecommendation: {2}", venda.Senha, venda.Score.ToString(), venda.RetornoAccertify.ToString()), Enumeradores.TipoEntrada.Sucesso);
                    }
                    catch (Exception ex)
                    {
                        SalvarLog.SalvarErro("Falha ao processar transação: " + venda.Senha, ex.Message, this.salvarlogevent);
                    }
                    finally
                    {
                        Vendas.Remove(venda);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Aguardar()
        {
            if (oProcessamento.TimeOut)
                Thread.Sleep(5000);
            else
                Thread.Sleep(1000);
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
                    email.EnviarSolicitacaoDocumentos_im(venda.Cliente.ID, detalheVendas);
                else
                    email.EnviarSolicitacaoDocumentos(venda.Cliente.ID, detalheVendas);
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

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            try
            {
                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, VendaBilheteria.StatusAntiFraude.Fraude, venda.Score, venda.RetornoAccertify);
            }
            catch (Exception ex)
            {
                SalvarLog.SalvarErro("Erro ao cancelar a Venda: " + venda.ID, ex.Message, SalvarLog.Events.Transacoes);
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
            if (!(objVenda is EstruturaVenda))
                return;

            EstruturaVenda venda = (EstruturaVenda)objVenda;

            try
            {
                List<IngressoImpressao> listaIngressos = new List<IngressoImpressao>();
                var status = VendaBilheteria.StatusAntiFraude.Aprovado;

                oVendaBilheteria.Score.Valor = venda.Score;
                oVendaBilheteria.AtualizarStatusAntiFraude(venda.ID, status, venda.Score, venda.RetornoAccertify);

                EstruturaTransacoesDetalhes detalheVendas = oVendaBilheteria.AcompanhamentoIngressos(venda.ID);

                if (detalheVendas.PermitirImpressaoInternet)
                    listaIngressos = ingressos.PesquisaVendaBilheteria(detalheVendas.VendaBilheteriaID);

                if (listaIngressos.Count > 0)
                {
                    if (ConfigurationManager.AppSettings["CanaisImais"].ToString().Contains(detalheVendas.CanalID.ToString()))
                        email.EnviarConfirmacaoCompraComIngresso_im(venda.Cliente.ID, detalheVendas, listaIngressos);
                    else
                        email.EnviarConfirmacaoCompraComIngresso(venda.Cliente.ID, detalheVendas, listaIngressos);
                }
                else
                {
                    if (ConfigurationManager.AppSettings["CanaisImais"].ToString().Contains(detalheVendas.CanalID.ToString()))
                        email.EnviarConfirmacaoCompra_im(venda.Cliente.ID, detalheVendas);
                    else
                        email.EnviarConfirmacaoCompra(venda.Cliente.ID, detalheVendas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Aprovar : " + ex.Message);
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
                if (oProcessamento.oPagamento.TipoPagamento == ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen)
                    nota = IRLib.AdyenStatic.Fields.NotaFiscal;
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

            if (oProcessamento.oPagamento.TipoPagamento == ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen)
            {
                oVendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oProcessamento.oPagamento.oAdyen.CodigoReferencia;
                oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oProcessamento.oPagamento.oAdyen.CodigoAutenticacao ?? oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor;
                oVendaBilheteriaFormaPagamento.Cupom.Valor = IRLib.AdyenStatic.Fields.NotaFiscal;
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
