using IngressoRapido.TemplateParser;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for Template
    /// </summary>
    public class Template
    {
        public Template()
        {

        }

        public static Parser NovaSenha(string email, string senha)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Email", email);
            templateVars.Add("Senha", senha);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/EnviaSenha.htm"), templateVars);

            return parser;
        }

        public static Parser AtivarCadastro(string nome, string email, string ChaveAtivacao)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("ChaveAtivacao", ChaveAtivacao);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/AtivaCadastro.htm"), templateVars);

            return parser;
        }
        public static Parser EnviarIndicacao(string email, string nomeEvento, string nomeIncacao, int eventoID)
        {
            try
            {
                string urlBase = System.Configuration.ConfigurationManager.AppSettings["URLBase"];

                Hashtable templateVars = new Hashtable();
                templateVars.Add("EventoNome", nomeEvento);
                templateVars.Add("Indicado", nomeIncacao);
                templateVars.Add("Link", string.Format("{0}/evento.aspx?ID={1}", urlBase, eventoID.ToString()));
                templateVars.Add("URLBase", urlBase);

                Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Indicacao.htm"), templateVars);

                return parser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static Parser VendaAprovadaRisco(string nome, string email, string senhaPedido, string htmlPedido, bool lugaresSeparados, bool temCamping)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("LugaresSeparados", lugaresSeparados);
            templateVars.Add("VendaBilheteriaID", senhaPedido.Substring(3));
            templateVars.Add("TemCamping", temCamping);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/VendaAprovadaRisco.htm"), templateVars);

            return parser;
        }

        public static Parser ReservaPeanut(string nome, string email, string senhaPedido, string htmlPedido, bool lugaresSeparados, bool temCamping, string atendente, string dataVenda, string canal, string formasPagamento)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("NomeCliente", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);
            templateVars.Add("TemCamping", temCamping);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/novoReservadoAnalise.html"), templateVars);

            return parser;
        }

        public static Parser VendaCancelada(string nome, string email, string numeroPedido, string htmlPedido, bool lugaresSeparados)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("NumeroPedido", numeroPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("LugaresSeparados", lugaresSeparados);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/VendaCancelada.htm"), templateVars);

            return parser;
        }

        public static Parser VendaProcessando(string nome, string email, string senhaPedido, string htmlPedido)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/VendaProcessando.htm"), templateVars);

            return parser;
        }

        public static Parser FormContato(string nome, string email, string ddd, string telefone, string mensagem)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("Telefone", "(" + ddd + ") " + telefone);
            templateVars.Add("Mensagem", mensagem);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/FormContato.htm"), templateVars);

            return parser;
        }

        public static Parser ErroCaptura(string senhaPedido, string htmlPedido)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("SenhaPedido", senhaPedido);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/ErroCaptura.htm"), templateVars);

            return parser;
        }

        public static Parser ErroReserva(int clienteID, string nome, string cpf, string telefone, string celular, string sessionID, string tid, int bandeiraCartao, int formaPagamentoID, string[] msgErro, int parcelas, string codigoAutorizacao, Decimal total, string htmlPedido)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("NomeCliente", nome);
            templateVars.Add("CPF", cpf);
            templateVars.Add("Telefone", telefone);
            templateVars.Add("Celular", celular);
            templateVars.Add("ClienteID", clienteID);
            templateVars.Add("SessionID", sessionID);

            if (bandeiraCartao == Ecommerce.Controle.FP_ITAU)
                templateVars.Add("Bandeira", "Itau");
            else
                templateVars.Add("Bandeira", "Visanet");

            templateVars.Add("FormaPagamentoID", formaPagamentoID);
            templateVars.Add("TID", tid);
            templateVars.Add("Timestamp", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            templateVars.Add("CodigoAutorizacao", codigoAutorizacao);
            templateVars.Add("Parcelas", parcelas);
            templateVars.Add("Total", total.ToString("c"));
            templateVars.Add("MsgErro1", msgErro[0]);
            templateVars.Add("MsgErro2", msgErro[1]);
            templateVars.Add("HtmlPedido", htmlPedido);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/ErroReserva.htm"), templateVars);

            return parser;
        }

        public static Parser Reserva(string nome, string email, string senhaPedido, string htmlPedido, bool lugaresSeparados, bool temCamping, string atendente, string dataVenda, string canal, string formasPagamento)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/novoReservado.html"), templateVars);

            return parser;
        }

        public static Parser VendaAprovada_EntregaNormal(string nome, string email, string senhaPedido, string htmlPedido, string atendente, string dataVenda, string canal, string formasPagamento, EstruturaPagamento oPagamento, List<int> eventos)
        {
            EstruturaClienteEndereco retorno = new ClienteEndereco().LerEstrutura(oPagamento.EnderecoClienteID);

            int diasEntrega = 0;

            if (eventos != null)
                diasEntrega = new IRLib.Entrega().MaiorDiasTriagemEntrega(oPagamento.EntregaControleID, eventos);
            else
                diasEntrega = new IRLib.Entrega().MaiorDiasTriagemEntregaVale(oPagamento.EntregaControleID);

            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);
            templateVars.Add("TipoTaxa", oPagamento.EntregaNome);
            templateVars.Add("Endereco", retorno.Endereco + ", " + retorno.Numero + " - " + retorno.Bairro + " / " + retorno.Cidade + " - " + retorno.Estado);
            templateVars.Add("DataEntrega", DateTime.Now.AddDays(diasEntrega).ToString("dd/MM/yyyy"));

            Parser parser;

            if (oPagamento.EntregaNome.Contains("Sedex"))
                parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Novos_Templates/VendaRealizada_sedex.htm"), templateVars);
            else
                parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Novos_Templates/VendaRealizada_motoboy.htm"), templateVars);

            return parser;
        }

        public static Parser VendaAprovada_EntregaAgendada(string nome, string email, string senhaPedido, string htmlPedido, string atendente, string dataVenda, string canal, string formasPagamento, EstruturaPagamento oPagamento, List<int> eventos)
        {
            EstruturaClienteEndereco retorno = new ClienteEndereco().LerEstrutura(oPagamento.EnderecoClienteID);

            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);
            templateVars.Add("TipoTaxa", oPagamento.EntregaNome);
            templateVars.Add("Endereco", retorno.Endereco + ", " + retorno.Numero + " - " + retorno.Bairro + " / " + retorno.Cidade + " - " + retorno.Estado);
            templateVars.Add("DataEntrega", DateTime.ParseExact(oPagamento.DataSelecionada, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"));

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Novos_Templates/VendaRealizada_motoboyagendado.htm"), templateVars);

            return parser;
        }

        public static Parser VendaAprovada_eTicket(string nome, string email, string senhaPedido, string htmlPedido, string atendente, string dataVenda, string canal, string formasPagamento)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Novos_Templates/VendaRealizada_eTicket.html"), templateVars);

            return parser;
        }

        public static Parser VendaAprovada_Retirada(string nome, string email, string senhaPedido, string htmlPedido, string atendente, string dataVenda, string canal, string formasPagamento, EstruturaPagamento oPagamento, List<int> eventos)
        {
            PontoVenda pv = new PontoVenda();
            pv.Carregar(oPagamento.PdvID);

            int diasEntrega = new IRLib.Entrega().MaiorDiasTriagemEntrega(oPagamento.EntregaControleID, eventos);

            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);
            templateVars.Add("TipoTaxa", oPagamento.EntregaNome);
            templateVars.Add("PDVSelecionado", pv.Nome);
            templateVars.Add("Endereco", pv.Endereco + ", " + pv.Numero + " - " + pv.Compl);
            templateVars.Add("DataEntrega", DateTime.Now.AddDays(diasEntrega).ToString("dd/MM/yyyy"));

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Novos_Templates/VendaRealizada_pontovenda.htm"), templateVars);

            return parser;
        }

        public static Parser VendaAprovada_RetiradaBilheteria(string nome, string email, string senhaPedido, string htmlPedido, string atendente, string dataVenda, string canal, string formasPagamento, EstruturaPagamento oPagamento, List<int> eventos)
        {
            Hashtable templateVars = new Hashtable();
            templateVars.Add("Nome", nome);
            templateVars.Add("Email", email);
            templateVars.Add("SenhaPedido", senhaPedido);
            templateVars.Add("HtmlPedido", htmlPedido);
            templateVars.Add("Atendente", atendente);
            templateVars.Add("DataVenda", dataVenda);
            templateVars.Add("Canal", canal);
            templateVars.Add("FormasPagamento", formasPagamento);
            templateVars.Add("TipoTaxa", oPagamento.EntregaNome);

            Parser parser = new Parser(HttpContext.Current.Server.MapPath("~/MailTemplates/Novos_Templates/VendaRealizada_retirada.htm"), templateVars);

            return parser;
        }

    }
}