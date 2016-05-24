using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using IRCore.Util;
using Utils.Web;

namespace IRLib
{
    public class EmailAccertify
    {
        private Cliente cliente { get; set; }

        public EmailAccertify()
        {
            cliente = new Cliente();
        }

        public void EnviarSolicitacaoDocumentos(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarSolicitacaoDocumentacao(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarSolicitacaoDocumentos : " + ex.Message);
            }
        }

        public void EnviarSolicitacaoDocumentos_im(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarSolicitacaoDocumentacao_im(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarSolicitacaoDocumentos_im : " + ex.Message);
            }
        }

        public void EnviarCompraEmAnalise(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarVendaEmAnalise(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(CultureInfo.InvariantCulture), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarConfirmacaoCompra : " + ex.Message);
            }
        }

        public void EnviarConfirmacaoCompra(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarVendaAprovada(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarConfirmacaoCompra : " + ex.Message);
            }
        }

        public void EnviarConfirmacaoCompra_im(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarVendaAprovada_im(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarConfirmacaoCompra_im : " + ex.Message);
            }
        }

        public void EnviarCancelamentoCompra(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarEmailCancelamento(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarCancelamentoCompra : " + ex.Message);
            }
        }

        public void EnviarCancelamentoCompra_im(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarEmailCancelamento_im(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarCancelamentoCompra_im : " + ex.Message);
            }
        }

        public void EnviarConfirmacaoCompraComIngresso(int clienteID, EstruturaTransacoesDetalhes venda, List<IngressoImpressao> listaIngressos, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);

                    string querystring = QueryString.Current
                                        .Add(QueryString.ID, venda.VendaBilheteriaID.ToString())
                                        .Add(QueryString.LOGADO, "true")
                                        .Encrypt(ConfigurationManager.AppSettings["ChaveCriptografiaLogin"]).ToString();

                    ServicoEmail.EnviarVendaAprovada_eTicket_Anexo(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega, ConfigurationManager.AppSettings["URLImpressao"] + querystring);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarConfirmacaoCompraComIngresso : " + ex.Message);
            }
        }

        public void EnviarConfirmacaoCompraComIngresso_im(int clienteID, EstruturaTransacoesDetalhes venda, List<IngressoImpressao> listaIngressos, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);

                    string querystring = QueryString.Current
                                        .Add(QueryString.ID, venda.VendaBilheteriaID.ToString())
                                        .Add(QueryString.LOGADO, "true")
                                        .Encrypt(ConfigurationManager.AppSettings["ChaveCriptografiaLogin"]).ToString();

                    ServicoEmail.EnviarVendaAprovada_eTicket_Anexo_im(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega, ConfigurationManager.AppSettings["URLImpressao"] + querystring);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarConfirmacaoCompraComIngresso_im : " + ex.Message);
            }
        }

        public void EnviarConfirmacaoCompraMobileTicket(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (!string.IsNullOrEmpty(email[0]))
                {
                    string pedido = MontarDetalhes(venda);
                    string entrega = MontarProcedimento(venda);
                    ServicoEmail.EnviarVendaAprovada_Mobile_Ticket(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarConfirmacaoCompraMobileTicket: " + ex.Message);
            }
        }

        public void EnviarCancelamentoFraude(int clienteID, EstruturaTransacoesDetalhes venda, bool debugMail = false)
        {
            try
            {
                var email = BuscaEmailCliente(clienteID);

                if (string.IsNullOrEmpty(email[0])) return;

                var pedido = MontarDetalhes(venda);
                var entrega = MontarProcedimento(venda);
                ServicoEmail.EnviarEmailCancelamentoFraude(email[1], email[0], venda.Senha, pedido, venda.Atendente, venda.DataVenda.ToString(CultureInfo.InvariantCulture), venda.Canal, venda.FormasPagamento(), entrega, venda.TaxaEntrega);
            }
            catch (Exception ex)
            {
                throw new Exception("EnviarCancelamentoCompra : " + ex.Message);
            }
        }

        public string MontarDetalhes(EstruturaTransacoesDetalhes venda)
        {
            try
            {
                StringBuilder tabela = new StringBuilder();

                decimal soma = 0;
                soma = venda.TaxaEntregaValor;

                tabela.Append("<table width='540' border='0' style='font-size: 12px;'>");

                bool replaceMesaFechada = false;
                int LugarAnterior = 0;
                decimal precoMesa = 0, ConvenienciaMesa = 0;

                foreach (var item in venda.ListaIngressos)
                {
                    tabela.Append(@"<tr><td width='242' bgcolor='#EBECEE' style='padding-left: 5px;'>");

                    if (item.LugarID != LugarAnterior)
                    {
                        replaceMesaFechada = false;
                        tabela.Replace("##PrecoMesa##", precoMesa.ToString("c"));
                        tabela.Replace("##ConvenienciaMesa##", ConvenienciaMesa.ToString("c"));
                    }

                    if (item.TipoLugar == "M")
                    {
                        if (item.LugarID != LugarAnterior)
                        {
                            replaceMesaFechada = true;
                            precoMesa = 0;
                            ConvenienciaMesa = 0;
                            LugarAnterior = item.LugarID;
                            tabela.Append("<b>Local: </b>" + item.Local + "<br />");
                            tabela.Append("<b>Evento: </b>" + item.Evento + "<br />");
                            tabela.Append("<b>Setor: </b>" + item.Setor + "<br />");
                            string dateFormat = hasDateTime(item.Setor, new string[] { "HH:mm" }) ? "dd/MM/yy" : "dd/MM/yy HH:mm";
                            tabela.Append("<b>Data: </b>" + (item.Apresentacao != DateTime.MinValue ? item.Apresentacao.ToString(dateFormat) : "-") + "</p></td>");

                            tabela.Append("<td bgcolor='#EBECEE'>");
                            tabela.Append("<b>Ingresso: </b>" + item.Codigo + "<br />");
                            tabela.Append("<b>Tipo: </b>" + item.PrecoNome + "<br />");
                            tabela.Append("<b>Preço: </b>##PrecoMesa##<br />");
                            tabela.Append("<b>Taxa de Conveniência: </b>##ConvenienciaMesa##</td>");

                            precoMesa = item.Valor;
                            ConvenienciaMesa = item.TaxaConveniencia;
                            soma += item.Valor + item.TaxaConveniencia;
                        }
                        else
                        {
                            precoMesa += item.Valor;
                            ConvenienciaMesa += item.TaxaConveniencia;
                            soma += item.Valor + item.TaxaConveniencia;
                        }
                    }
                    else
                    {
                        tabela.Append("<b>Local: </b>" + item.Local + "<br />");
                        tabela.Append("<b>Evento: </b>" + item.Evento + "<br />");
                        tabela.Append("<b>Setor: </b>" + item.Setor + "<br />");
                        string dateFormat = hasDateTime(item.Setor, new string[] { "HH:mm" }) ? "dd/MM/yy" : "dd/MM/yy HH:mm";
                        tabela.Append("<b>Data: </b>" + (item.Apresentacao != DateTime.MinValue ? item.Apresentacao.ToString(dateFormat) : "-") + "</td>");
                        tabela.Append("<td bgcolor='#EBECEE'>");
                        tabela.Append("<b>Ingresso: </b>" + item.Codigo + "<br />");
                        tabela.Append("<b>Tipo: </b>" + item.PrecoNome + "<br />");
                        tabela.Append("<b>Preço: </b>" + item.Valor.ToString("c") + "<br />");
                        tabela.Append("<b>Taxa de Conveniência: </b>" + item.TaxaConveniencia.ToString("c") + " </td>");

                        soma += item.Valor + item.TaxaConveniencia;
                    }

                    tabela.Append(@"</tr>");
                }

                if (replaceMesaFechada)
                {
                    replaceMesaFechada = false;
                    tabela.Replace("##PrecoMesa##", precoMesa.ToString("c"));
                    tabela.Replace("##ConvenienciaMesa##", ConvenienciaMesa.ToString("c"));
                }

                tabela.Append(@"</table>");
                tabela.Append(@"<table width='540' border='0'>");

                if (venda.TaxaEntregaID > 0)
                    tabela.Append("<tr ><td bgcolor='#D2D6D7' align='right'><b>" + venda.TaxaEntrega + ": </b>" + venda.TaxaEntregaValor.ToString("c") + "</td></tr>");
                tabela.Append("<tr ><td bgcolor='#D2D6D7' align='right'><b>" + soma.ToString("c") + "</td></tr>");
                tabela.Append(@"</table>");

                return tabela.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Detalhes : " + ex.Message);
            }
        }

        public string MontarProcedimento(EstruturaTransacoesDetalhes venda)
        {
            try
            {
                List<int> eventos = venda.ListaIngressos.Select(c => c.EventoID).Distinct().ToList();
                List<string> eventosNomes = venda.ListaIngressos.Select(c => c.Evento).Distinct().ToList();

                EventoTaxaEntrega eventoTaxaEntrega = new EventoTaxaEntrega();
                Entrega oEntrega = new Entrega();
                EstruturaEntrega oEE = oEntrega.CarregarEstruturaPeloControleID(venda.EntregaControleID);

                List<string> detalhes = eventoTaxaEntrega.DetalhesEntregaPorEventos(eventos, oEE.ID);

                StringBuilder tabela = new StringBuilder();

                tabela.Append(@"<table width='540' border='0'><tr><td style='font-size: 12px;'>");

                if (detalhes.Count == 0)
                    tabela.Append("<b>Detalhe de Entrega:</b>" + oEE.ProcedimentoEntrega);
                else if (detalhes.Count == eventosNomes.Count)
                {
                    for (int i = 0; i < eventosNomes.Count; i++)
                    {
                        tabela.Append("<b>Evento: </b> " + eventosNomes[i]);
                        string detalheEntrega = eventoTaxaEntrega.DetalheEntregaEvento(eventos[i], oEE.ID);
                        tabela.Append("<br/><br/><b>Detalhe da entrega: </b> " + detalheEntrega + "<br/>");
                    }
                }
                else
                {
                    if (detalhes.Count > 0)
                        tabela.Append("<br/><b>Detalhe da Entrega: </b> ");

                    foreach (string str in detalhes)
                        tabela.Append("<br/> " + str);
                }

                tabela.Append(@"</td></tr></table>");

                return tabela.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Procedimento : " + ex.Message);
            }
        }

        private bool hasDateTime(string dateString, string[] formats)
        {
            try
            {
                string[] str = dateString.Split(new Char[] { ' ' });
                DateTime dateTime;

                foreach (var text in str)
                {
                    foreach (var format in formats)
                    {
                        if (DateTime.TryParseExact(text, format, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateTime))
                            return true;
                    }
                }

                return false;

            }
            catch (Exception)
            {

                return false;
            }
        }

        private string[] BuscaEmailCliente(int clienteID, bool debug = false)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##IRLib.Email.BuscaEmailCliente.Start##");
            var email = new string[2];
            if (debug)
            {
                email[0] = "debugmail@mailinator.com";
                email[1] = "Debug de Email";
            }
            else
            {
                email = cliente.BuscaEmail(clienteID);
            }

            sw.Stop();
            LogUtil.Debug(String.Format("##IRLib.Email.BuscaEmailCliente.Finished## TEMPO_DECORRIDO: {0} ms", sw.ElapsedMilliseconds));
            return email;
        }
    }
}
