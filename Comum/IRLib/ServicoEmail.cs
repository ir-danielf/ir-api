using CTLib;
using System;
using System.Diagnostics;
using IRCore.Util;

namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ServicoEmail
    {
        private static Emails.MailServiceSoapClient _oService_Emails;
        public static Emails.MailServiceSoapClient GetInstance()
        {
            if (_oService_Emails == null)
                _oService_Emails = new Emails.MailServiceSoapClient();

            return _oService_Emails;
        }

        #region EnvioSync

        #region IR

        public static void EnviarEmailAssinaturas(string Email, string Assunto, string Corpo)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailAssinaturas(Email, Assunto, Corpo);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailEstorno(string email, string nome, string cpf, decimal valoresPagos)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailEstorno(email, nome, cpf, valoresPagos.ToString("c"));

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarBoletos(string email, string linkBoletos, string assinaturasTabela)
        {
            Emails.Retorno retorno = GetInstance().EnviarBoletos(email, linkBoletos, assinaturasTabela);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarAlertaCodigoBarra(string email, string regional, string empresa, string local, string evento, string horario, string setor, int quantidade, bool criados)
        {
            Emails.Retorno retorno = GetInstance().EnviarAlertaCodigoBarra(email, regional, empresa, local, evento, horario, setor, quantidade, criados);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarAlerta(string email, string corpo)
        {
            Emails.Retorno retorno = GetInstance().EnviarAlerta(email, corpo);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarAlertaDeComprasComPrioridadeDeAtendimento(string email, int quantidade, int dias)
        {
            Emails.Retorno retorno = GetInstance().EnviarAlertaDeComprasComPrioridadeDeAtendimento(email, quantidade, dias);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarAlertaMudancaSetor(string html, string email)
        {
            Emails.Retorno retorno = GetInstance().EnviarAlertaMudancaSetor(email, html);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarNovaSenha(string nome, string senha, string email)
        {
            Emails.Retorno retorno = GetInstance().EnviarNovaSenha(nome, senha, email);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarNovaSenhaMobile(string nome, string senha, string email)
        {
            Emails.Retorno retorno = GetInstance().EnviarNovaSenhaMobile(nome, senha, email);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarIndicacao(string email, string evento, string nome, int eventoID)
        {
            Emails.Retorno retorno = GetInstance().EnviarIndicacao(email, evento, nome, eventoID);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarIndicacaoVale(string email, string evento, string nome, int eventoID)
        {
            Emails.Retorno retorno = GetInstance().EnviarIndicacaoVale(email, evento, nome, eventoID);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarFormContato(string emaildestinatario, string responsavel, string nome, string email, string telefone, string mensagem)
        {
            Emails.Retorno retorno = GetInstance().EnviarFormContato(emaildestinatario, responsavel, nome, email, telefone, mensagem);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviaMensagemExcessao(string email, string Message, string StackTrace)
        {
            Emails.Retorno retorno = GetInstance().EnviaMensagemExcessao(email, Message, StackTrace);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarInseridoCanalEventoLogistica(string email, string evento, string local)
        {
            Emails.Retorno retorno = GetInstance().EnviarInseridoCanalEventoLogistica(email, evento, local);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailAcompanhamentoEntrega(string email, string cliente, string vendaBilheteriaID, string senha, string taxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailAcompanhamentoEntrega(email, cliente, vendaBilheteriaID, senha, taxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailAcompanhamentoRetirada(string email, string cliente, string vendaBilheteriaID, string senha, string taxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailAcompanhamentoRetirada(email, cliente, vendaBilheteriaID, senha, taxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailAprovacaoSetores(string email, string arvore)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailAprovacaoSetores(email, arvore);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailAgruparMapa(string email, string arvore)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailAgruparMapa(email, arvore);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarFeedback(string email, string nome, string senha, string data, int vendaBilheteriaID)
        {
            Emails.Retorno retorno = GetInstance().EnviarFeedback(email, nome, senha, data, vendaBilheteriaID);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarFeedbackCliente(string email, string nome, string senha, string feedback)
        {
            Emails.Retorno retorno = GetInstance().EnviarFeedbackCliente(email, nome, senha, feedback);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaEmAnalise(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            var retorno = GetInstance().EnviarVendaEmAnalise(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovada(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovada_eTicket_Anexo(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa, string link)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada_eTicket_Anexo(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa, link);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovada_eTicket(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada_eTicket(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovada_Mobile_Ticket(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada_Mobile_Ticket(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovadaRisco(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##IRLib.ServicoEmail.EnviarEmailCancelamento.SendingEmail##");
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovadaRisco(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            sw.Stop();
            LogUtil.Debug(String.Format("##IRLib.ServicoEmail.EnviarEmailCancelamento.EmailSent## TEMPO_DECORRIDO: {0} ms", sw.ElapsedMilliseconds));
            
            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovadaVir(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovadaVir(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarConfirmacaoDeEntrega(string email, string cliente, string vendaBilheteriaID, string senha, string taxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarConfirmacaoDeEntrega(email, cliente, vendaBilheteriaID, senha, taxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailCancelamento(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##IRLib.ServicoEmail.EnviarEmailCancelamento.SendingEmail##");
            Emails.Retorno retorno = GetInstance().EnviarEmailCancelamento(nome, email, senha, pedido, atendente, data, canal, pagamento);

            sw.Stop();
            LogUtil.Debug(String.Format("##IRLib.ServicoEmail.EnviarEmailCancelamento.EmailSent## TEMPO_DECORRIDO: {0} ms", sw.ElapsedMilliseconds));

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailCancelamentoFraude(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##IRLib.ServicoEmail.EnviarEmailCancelamentoFraude.SendingEmail##");
            Emails.Retorno retorno = GetInstance().EnviarEmailCancelamentoFraude(nome, email, senha, pedido, atendente, data, canal, pagamento);

            sw.Stop();
            LogUtil.Debug(String.Format("##IRLib.ServicoEmail.EnviarEmailCancelamentoFraude.EmailSent## TEMPO_DECORRIDO: {0} ms", sw.ElapsedMilliseconds));

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarAprovacaoPagamento(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##IRLib.ServicoEmail.EnviarAprovacaoPagamento.SendingEmail##");
            Emails.Retorno retorno = GetInstance().EnviarAprovacaoPagamento(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            sw.Stop();
            LogUtil.Debug(String.Format("##IRLib.ServicoEmail.EnviarAprovacaoPagamento.EmailSent## TEMPO_DECORRIDO: {0} ms", sw.ElapsedMilliseconds));

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarSolicitacaoDocumentacao(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##IRLib.ServicoEmail.EnviarSolicitacaoDocumentacao.SendingEmail##");
            Emails.Retorno retorno = GetInstance().EnviarSolicitacaoDocumentacao(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            sw.Stop();
            LogUtil.Debug(String.Format("##IRLib.ServicoEmail.EnviarSolicitacaoDocumentacao.EmailSent## TEMPO_DECORRIDO: {0} ms", sw.ElapsedMilliseconds));

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        #endregion

        #region IM

        public static void EnviarVendaAprovada_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovada_eTicket_Anexo_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa, string link)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada_eTicket_Anexo_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa, link);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovada_eTicket_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovada_eTicket_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarVendaAprovadaRisco_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovadaRisco_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);

        }

        public static void EnviarVendaAprovadaVir_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarVendaAprovadaVir_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarConfirmacaoDeEntrega_im(string email, string cliente, string vendaBilheteriaID, string senha, string taxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarConfirmacaoDeEntrega_im(email, cliente, vendaBilheteriaID, senha, taxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailCancelamento_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailCancelamento_im(nome, email, senha, pedido, atendente, data, canal, pagamento);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarEmailCancelamentoFraude_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento)
        {
            Emails.Retorno retorno = GetInstance().EnviarEmailCancelamentoFraude_im(nome, email, senha, pedido, atendente, data, canal, pagamento);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarAprovacaoPagamento_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarAprovacaoPagamento_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        public static void EnviarSolicitacaoDocumentacao_im(string nome, string email, string senha, string pedido, string atendente, string data, string canal, string pagamento, string detalhetaxa, string tipotaxa)
        {
            Emails.Retorno retorno = GetInstance().EnviarSolicitacaoDocumentacao_im(nome, email, senha, pedido, atendente, data, canal, pagamento, detalhetaxa, tipotaxa);

            if (!retorno.StatusOperacao)
                throw new Exception(retorno.Mensagem);
        }

        #endregion

        #endregion

    }
}