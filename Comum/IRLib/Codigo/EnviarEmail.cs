using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class EnviarEmail : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        private string PARA = (string)System.Configuration.ConfigurationManager.AppSettings["EmailDestinoGeral"];

        private string PARA_ABRIL = ConfigurationManager.AppSettings["EmailDestinoAbril"];

        private string DESTINATARIOS_MAPA = ConfigurationManager.AppSettings["DestinatariosMapa"];

        private string DESTINATARIO_SAC = ConfigurationManager.AppSettings["EmailSAC"];

        private string DESTINATARIO_APROV_SETOR = ConfigurationManager.AppSettings["EmailAprovacaoSetores"];

        public void EnviaMensagemExcessao(Exception excesao)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTeste"]))
                return;

            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["MensagemExcessaoAtivado"] != null && Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MensagemExcessaoAtivado"]) == "true")
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["MensagemExcessaoEmailDestinatario"] == null)
                        throw new Exception("A chave MensagemExcessaoEmailDestinatario não está devidamente configurada.");

                    string emailDestinatario = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MensagemExcessaoEmailDestinatario"]);

                    ServicoEmail.EnviaMensagemExcessao(emailDestinatario, excesao.Message, excesao.StackTrace);

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao enviar e-mail: " + ex);
            }
        }

        public void InseridoCanalEventoLogistica(string eventoID, bool filme)
        {
            if (filme)
            {
                InseridoCanalEventoCinema(eventoID);
                return;
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTeste"]))
                return;

            try
            {
                if (string.IsNullOrEmpty(PARA))
                    PARA = string.Empty;
                else if (string.IsNullOrEmpty(PARA_ABRIL))
                    PARA_ABRIL = string.Empty;

                string[] eventoLocalNome = new Evento().EventoLocalNome(Convert.ToInt32(eventoID));

                //monta o e-mail                
                foreach (string destino in PARA.Split(','))
                {
                    if (destino.Length == 0)
                        continue;

                    ServicoEmail.EnviarInseridoCanalEventoLogistica(destino, eventoLocalNome[1], eventoLocalNome[0]);
                }

                foreach (string destino in PARA_ABRIL.Split(','))
                {
                    if (destino.Length == 0)
                        continue;

                    ServicoEmail.EnviarInseridoCanalEventoLogistica(destino, eventoLocalNome[1], eventoLocalNome[0]);
                }

            }
            catch (Exception)
            {

            }
        }

        public void InseridoCanalEventoLogistica(string eventoID)
        {
            this.InseridoCanalEventoLogistica(eventoID, false);
        }

        public void InseridoCanalEventoCinema(string eventoID)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTeste"]))
                return;

            Evento evento = new Evento();

            try
            {
                string[] eventoLocalNome = evento.EventoLocalNome(Convert.ToInt32(eventoID));

                ServicoEmail.EnviarInseridoCanalEventoLogistica(ConfiguracaoCinema.Instancia.Email.Web.Valor, eventoLocalNome[1], eventoLocalNome[0]);
            }
            catch (Exception)
            {

            }
        }

        [OneWay]
        public void EnviarAlertaMudancaSetor(object corpoEmail)
        {

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTeste"]))
                return;

            string destinatarios = DESTINATARIOS_MAPA;
            string corpo = corpoEmail.ToString();


            foreach (string dest in destinatarios.Split(','))
            {
                if (dest.Length > 0)
                {
                    ServicoEmail.EnviarAlertaMudancaSetor(corpo, dest);
                }
            }


        }

        [OneWay]
        public void ConfirmacaoDeCompra(string nomeCliente, string emailCliente, string senhaVenda, string tabelaItensComprados, string formasPagamento, string tipoTaxa, string dataVenda, string atendente, string canal, string detalhesTaxaEntrega)
        {
            try
            {
                ServicoEmail.EnviarVendaAprovada(nomeCliente, emailCliente, senhaVenda, tabelaItensComprados, atendente, dataVenda, canal, formasPagamento, detalhesTaxaEntrega, tipoTaxa);
            }
            catch (Exception ex)
            {
                throw new Exception("ConfirmacaoDeCompra - Falha ao enviar e-mail: " + ex);
            }
        }

        [OneWay]
        public void ConfirmacaoDeCompraVir(string nomeCliente, string emailCliente, string senhaVenda, string tabelaItensComprados, string formasPagamento, string tipoTaxa, string dataVenda, string atendente, string canal, string detalhesTaxaEntrega)
        {
            try
            {
                ServicoEmail.EnviarVendaAprovadaVir(nomeCliente, emailCliente, senhaVenda, tabelaItensComprados, atendente, dataVenda, canal, formasPagamento, detalhesTaxaEntrega, tipoTaxa);

            }
            catch (Exception ex)
            {
                throw new Exception("ConfirmacaoDeCompra - Falha ao enviar e-mail: " + ex);
            }
        }


        private void EnviarAlerta(List<EstruturaAlertaApresentacao> listaApresentacoes, int dias, List<string> destinatarios)
        {
            if (listaApresentacoes.Count == 0)
                return;

            if (destinatarios == null)
                return;

            StringBuilder mensagem = new StringBuilder();

            mensagem.Append("<span style='font-size:18px; color: Red;'>");
            mensagem.Append("<b>Atenção!</b>As apresentações abaixo possuem 20 ou mais senhas para impressão em bilheteria!<br />");
            mensagem.Append("</span>");
            mensagem.Append("<table style='width:548px; text-align:center;'>");
            mensagem.Append("<tr>");
            mensagem.Append("<th>Regional</th>");
            mensagem.Append("<th>Empresa</th>");
            mensagem.Append("<th>Local</th>");
            mensagem.Append("<th>Evento</th>");
            mensagem.Append("<th>Horário</th>");
            mensagem.Append("<th>Quantidade</th>");
            foreach (EstruturaAlertaApresentacao alerta in listaApresentacoes)
            {
                mensagem.Append("<tr>");
                mensagem.AppendFormat("<td>{0}</td>", alerta.Regional);
                mensagem.AppendFormat("<td>{0}</td>", alerta.Empresa);
                mensagem.AppendFormat("<td>{0}</td>", alerta.Local);
                mensagem.AppendFormat("<td>{0}</td>", alerta.Evento);
                mensagem.AppendFormat("<td>{0}</td>", alerta.Horario);
                mensagem.AppendFormat("<td>{0}</td>", alerta.Quantidade);
                mensagem.Append("</tr>");
            }

            mensagem.Append("</table>");

            foreach (var item in destinatarios)
                ServicoEmail.EnviarAlerta(item, mensagem.ToString());
        }

        public void EnviarEmailAgregado(string agregadoNome, string agregadoCpf, string agregadoTelefone, string agregadoEmail, string NomeCliente, string EmailCliente, string senha, string NomeAtendente, string NomeCanal, string DataVenda)
        {
            StringBuilder tabela = new StringBuilder();

            tabela.Append("<table width='548' border='0' cellspacing='1' cellpadding='2' style='font: normal 12px/16px Trebuchet MS , Times New Roman , Arial;'>");
            tabela.Append("<tr>");
            tabela.Append("<td align='left' bgcolor='#dfdfdf' colspan='4'>NOVO AGREGADO CADASTRADO.</td>");
            tabela.Append("<td align='left' bgcolor='#dfdfdf' colspan='4'>O seguinte agregado foi cadastrado:</td>");
            tabela.Append("</tr>");

            tabela.Append("<tr bgcolor='#dfdfdf'>");
            tabela.Append("<td>Nome: </td>");
            tabela.Append(string.Format("<td>{0}</td>", agregadoNome));
            tabela.Append("<td>CPF: </td>");
            tabela.Append(string.Format("<td>{0}</td>", agregadoCpf));
            tabela.Append("</tr>");

            tabela.Append("<tr bgcolor='#dfdfdf'>");
            tabela.Append("<td>Telefone: </td>");
            tabela.Append(string.Format("<td>{0}</td>", agregadoTelefone));
            tabela.Append("<td>Email: </td>");
            tabela.Append(string.Format("<td>{0}</td>", agregadoEmail));
            tabela.Append("</tr>");

            tabela.Append("<tr>");
            tabela.Append("<td align='left' bgcolor='#dfdfdf' colspan='4'>Dados da venda: </td>");
            tabela.Append("</tr>");

            tabela.Append("<tr bgcolor='#dfdfdf'>");
            tabela.Append("<td>Cliente: </td>");
            tabela.Append(string.Format("<td>{0}</td>", NomeCliente));
            tabela.Append("<td>Email: </td>");
            tabela.Append(string.Format("<td>{0}</td>", EmailCliente));
            tabela.Append("</tr>");

            tabela.Append("<tr bgcolor='#dfdfdf'>");
            tabela.Append("<td>Senha: </td>");
            tabela.Append(string.Format("<td>{0}</td>", senha));
            tabela.Append("<td>Atendente: </td>");
            tabela.Append(string.Format("<td>{0}</td>", NomeAtendente));
            tabela.Append("</tr>");

            tabela.Append("<tr bgcolor='#dfdfdf'>");
            tabela.Append("<td>Canal: </td>");
            tabela.Append(string.Format("<td>{0}</td>", NomeCanal));
            tabela.Append("<td>Data venda: </td>");
            tabela.Append(string.Format("<td>{0}</td>", DataVenda));
            tabela.Append("</tr>");

            tabela.Append("</table>");
            ServicoEmail.EnviarAlerta(EmailCliente, tabela.ToString());
        }

        public void EnviarAlerta(List<EstruturaAlertaApresentacao> listaApresentacoes, int dias)
        {
            List<string> destinatariosAtendimento = new List<string>();
            foreach (string destinatario in ConfigurationManager.AppSettings["EmailDestinatariosAlertaApresentacao"].Split(';'))
                destinatariosAtendimento.Add(destinatario);

            EnviarAlerta(listaApresentacoes, dias, destinatariosAtendimento);
        }

        public void EnviarAlerta(List<EstruturaAlertaApresentacao> listaApresentacoes, int dias, string Email, string Regional)
        {
            List<string> destinatario = new List<string>();

            destinatario.Add(Email);
            EnviarAlerta(listaApresentacoes, dias, destinatario);
        }

        public bool EnviarEmailAprovacaoSetores(string Arvore)
        {
            try
            {
                ServicoEmail.EnviarEmailAprovacaoSetores(this.DESTINATARIO_APROV_SETOR, Arvore);
                return true;
            }
            catch (Exception ex)
            {
                this.EnviaMensagemExcessao(ex);
                return false;
            }
        }

        public bool EnviarEmailAgruparMapa(string Arvore)
        {
            try
            {
                ServicoEmail.EnviarEmailAgruparMapa(this.DESTINATARIO_APROV_SETOR, Arvore);

                return true;
            }
            catch (Exception ex)
            {
                this.EnviaMensagemExcessao(ex);
                return false;
            }
        }

        public bool AlertaDeComprasComPrioridadeDeAtendimento(int quantidade_compras, int dias, string EmailsAlertaPeanut)
        {
            try
            {
                string[] listaEmail = EmailsAlertaPeanut.Split(';');

                for (int cont = 0; cont < listaEmail.Length; cont++)
                    ServicoEmail.EnviarAlertaDeComprasComPrioridadeDeAtendimento(listaEmail[cont], quantidade_compras, dias);

                return true;
            }
            catch (Exception ex)
            {
                this.EnviaMensagemExcessao(ex);
                return false;
            }
        }

        public void EnviarFeedback(string corpoEmail, EstruturaFeedback feedBack)
        {
            ServicoEmail.EnviarFeedback(feedBack.ClienteEmail, feedBack.ClienteNome, feedBack.SenhaVenda, feedBack.DataVenda.ToShortDateString(), feedBack.VendaBilheteriaID);
        }

        public void EnviarFeedbackCliente(string nomeCliente, string senhaVenda, string feedback)
        {
            ServicoEmail.EnviarFeedbackCliente(DESTINATARIO_SAC, nomeCliente, senhaVenda, feedback);
        }

        public void EnviarAlertaCodigoBarra(EstruturaQuantidadeCodigosListaBranca item, bool criados)
        {
            ServicoEmail.EnviarAlertaCodigoBarra(item.Email, item.Regional, item.Empresa, item.Local, item.Evento, item.Horario.ToShortDateString(), item.Setor, item.Quantidade, criados);
        }

        public void EnviarBoletos(Cliente cliente, string linksBoleto, string assinaturasTabela)
        {
            ServicoEmail.EnviarBoletos(cliente.Email.Valor, linksBoleto, assinaturasTabela);
        }

        public void EnviarEmailAssinaturas(EstruturaEmailAssinatura envio)
        {
            ServicoEmail.EnviarEmailAssinaturas(envio.ClienteEmail, envio.Assunto, envio.Corpo);
        }

        internal void EnviarEmailEstorno(Cliente cliente, decimal valoresPagos, string s)
        {
            ServicoEmail.EnviarEmailEstorno(cliente.Email.Valor, cliente.Nome.Valor, cliente.CPF.Valor, valoresPagos);
        }
    }
}

