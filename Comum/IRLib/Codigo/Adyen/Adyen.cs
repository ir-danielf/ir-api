using IRLib.wsAdyen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using CTLib;
using IRCore.Util;

namespace IRLib
{
    public class Adyen
    {
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; }

        public string NumeroCartao { get; set; }

        public string DataValidadeCartao { get; set; }

        public string CodigoVerificacaoCartao { get; set; }

        public string MerchantAccount { get; set; }

        public string ClienteNome { get; set; }

        public string ClienteEmail { get; set; }

        public string ClienteIP { get; set; }

        public string ClienteSessionID { get; set; }

        public string CodigoAutenticacao { get; set; }

        public string CodigoReferencia { get; set; }

        public string ClienteID { get; set; }

        public int Parcelas { get; set; }

        public bool Refused { get; set; }

        public bool Fraud { get; set; }

        public string Adquirente { get; set; }

        private long ValorFormatado
        {
            get
            {
                return Convert.ToInt64(this.Valor.ToString("#.00").Replace(".", string.Empty).Replace(",", string.Empty));
            }
        }

        private NetworkCredential Credentials
        {
            get
            {
                return new NetworkCredential()
                {
                    UserName = ConfiguracaoAdyen.Instancia.Chaves.Username.Valor,
                    Password = ConfiguracaoAdyen.Instancia.Chaves.Password.Valor
                };
            }
        }

        public bool EfetuarPagamento()
        {
            LogUtil.Debug(string.Format("##Adyen.EfetuandoPagamento## SESSION {0}", this.ClienteSessionID));

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Extensions.ValidateRemoteCertificate);

            try
            {
                if (ConfiguracaoAdyen.Instancia.Chaves.Teste.Valor)
                    return this.PagarSandbox();
                else
                    return this.Pagar();
            }
            catch (Exception ex)
            {
                // Loga a exception original em banco para monitoramento de erros não tratados.
                // Este código deve ser temporário, e removido ao final da identificação de erros.
                try
                {
                    Func<string, string> ifNullOrEmptyUseEmpty = delegate(string value)
                    {
                        return !string.IsNullOrEmpty(value) ? value.Replace("'", "") : "N/A";
                    };

                    Func<string, string> maskCreditCard = delegate(string creditCardNumber)
                    {
                        if (string.IsNullOrEmpty(creditCardNumber) || creditCardNumber.Length < 4)
                        {
                            return "N/A";
                        }

                        return string.Concat("".PadLeft(creditCardNumber.Length - 4, '*'), creditCardNumber.Substring(creditCardNumber.Length - 4));
                    };

                    var sql = new System.Text.StringBuilder();
                    sql.Append("INSERT INTO LogPagamento (Date, Message, Source, StackTrace, FriendlyMessage, [Data]) ");
                    sql.Append("VALUES (GETDATE(), '@MESSAGE','@SOURCE','@STACKTRACE', '@FRIENDLYMESSAGE', '@DATA')");
                    sql.Replace("@MESSAGE", ifNullOrEmptyUseEmpty(ex.Message));
                    sql.Replace("@SOURCE", ifNullOrEmptyUseEmpty(ex.Source));
                    sql.Replace("@STACKTRACE", ifNullOrEmptyUseEmpty(ex.StackTrace));
                    sql.Replace("@FRIENDLYMESSAGE", AdyenStatic.Exceptions.Tratar(ex.Message));

                    var jsonPagamentoLog = "CreditCardName: " + ifNullOrEmptyUseEmpty(this.NomeCartao) + ", " +
                        "NumeroCartao: " + maskCreditCard(this.NumeroCartao) + ", " +
                        "ValidadeCartao: " + ifNullOrEmptyUseEmpty(this.DataValidadeCartao) + ", " +
                        "ShopperEmail: " + ifNullOrEmptyUseEmpty(this.ClienteEmail) + ", " +
                        "ShopperIP: " + ifNullOrEmptyUseEmpty(this.ClienteIP) + ", " +
                        "Reference: " + ifNullOrEmptyUseEmpty(this.ClienteNome) + "," +
                        "SessionId: " + ifNullOrEmptyUseEmpty(this.ClienteSessionID) + "," +
                        "Valor: " + this.ValorFormatado.ToString();

                    sql.Replace("@DATA", jsonPagamentoLog);

                    // Insere os dados no banco.
                    new BD().Executar(sql.ToString());

                    LogUtil.Debug(string.Format("##Adyen.EfetuandoPagamento.ERROR## SESSION {0}, MSG {1}", this.ClienteSessionID, "Erro gravado na tabela LogPagamento"));
                }
                catch (Exception bdError)
                {

                }

                LogUtil.Error(string.Format("##Adyen.EfetuandoPagamento.EXCEPTION## SESSION {0}, MSG {1}", this.ClienteSessionID, ex.Message), ex);

                if (ex.Message.Contains(AdyenStatic.Exceptions.Refused))
                    this.Refused = true;
                else if (ex.Message.Contains(AdyenStatic.Exceptions.Fraud))
                    this.Fraud = true;

                throw new Exception(AdyenStatic.Exceptions.Tratar(ex.Message));
            }
        }

        private bool PagarSandbox()
        {
            var additionalData = new List<wsAdyenSandbox.anyType2anyTypeMapEntry>();

            if (this.Parcelas > 1)
            {
                additionalData.Add(new wsAdyenSandbox.anyType2anyTypeMapEntry()
                {
                    key = AdyenStatic.Fields.Installments,
                    value = this.Parcelas.ToString()
                });
            }

            wsAdyenSandbox.PaymentRequest p = new wsAdyenSandbox.PaymentRequest()
            {
                amount = new wsAdyenSandbox.Amount
                {
                    currency = ConfiguracaoAdyen.Instancia.Chaves.Currency.Valor,
                    value = ValorFormatado,
                },
                card = new wsAdyenSandbox.Card
                {
                    cvc = this.CodigoVerificacaoCartao,
                    expiryMonth = this.DataValidadeCartao.Substring(0, 2),
                    expiryYear = "20" + this.DataValidadeCartao.Substring(2, 2),
                    holderName = this.NomeCartao,
                    number = this.NumeroCartao,
                },
                additionalData = additionalData.ToArray(),
                merchantAccount = ConfiguracaoAdyen.Instancia.Chaves.MerchantAccount.Valor,
                shopperEmail = this.ClienteEmail,
                shopperIP = this.ClienteIP,
                shopperReference = this.ClienteID,
                reference = this.ClienteNome,
                sessionId = this.ClienteSessionID,
            };

            wsAdyenSandbox.Payment px = new wsAdyenSandbox.Payment();
            px.Credentials = this.Credentials;
            var retorno = px.authorise(p);

            bool autorizado = AdyenStatic.Action.Autorizado(retorno.resultCode);

            this.CodigoAutenticacao = retorno.authCode ?? string.Empty;
            this.CodigoReferencia = retorno.pspReference ?? string.Empty;

            if (autorizado)
                return true;

            throw new Exception(retorno.refusalReason);
        }

        private bool Pagar()
        {
            LogUtil.Debug(string.Format("##Adyen.Pagando## SESSION {0}", this.ClienteSessionID));

            var additionalData = new List<anyType2anyTypeMapEntry>();

            if (this.Parcelas > 1)
            {
                additionalData.Add(new anyType2anyTypeMapEntry()
                {
                    key = AdyenStatic.Fields.Installments,
                    value = this.Parcelas.ToString()
                });
            }

            if (!string.IsNullOrEmpty(this.Adquirente))
            {
                switch ((AdyenStatic.Adquirente)Enum.Parse(typeof(AdyenStatic.Adquirente), this.Adquirente))
                {
                    case AdyenStatic.Adquirente.Cielo:
                        {
                            additionalData.Add(new anyType2anyTypeMapEntry()
                            {
                                key = AdyenStatic.Fields.AcquirerCode,
                                value = (AdyenStatic.Adquirente.Cielo).ToString()
                            });

                            additionalData.Add(new anyType2anyTypeMapEntry()
                            {
                                key = AdyenStatic.Fields.AuthorisationMid,
                                value = Utils.Enums.GetDescription<AdyenStatic.Adquirente>(AdyenStatic.Adquirente.Cielo).ToString()
                            });
                        }
                        break;
                    case AdyenStatic.Adquirente.Redecard:
                        {
                            additionalData.Add(new anyType2anyTypeMapEntry()
                            {
                                key = AdyenStatic.Fields.AcquirerCode,
                                value = (AdyenStatic.Adquirente.Redecard).ToString()
                            });

                            additionalData.Add(new anyType2anyTypeMapEntry()
                            {
                                key = AdyenStatic.Fields.AuthorisationMid,
                                value = Utils.Enums.GetDescription<AdyenStatic.Adquirente>(AdyenStatic.Adquirente.Redecard).ToString()
                            });
                        }
                        break;
                    default:
                        break;
                }
            }

            PaymentRequest p = new PaymentRequest()
            {
                amount = new Amount
                {
                    currency = ConfiguracaoAdyen.Instancia.Chaves.Currency.Valor,
                    value = ValorFormatado,
                },
                card = new Card
                {
                    cvc = CodigoVerificacaoCartao,
                    expiryMonth = this.DataValidadeCartao.Substring(0, 2),
                    expiryYear = "20" + this.DataValidadeCartao.Substring(2, 2),
                    holderName = this.NomeCartao,
                    number = this.NumeroCartao,
                },

                additionalData = additionalData.ToArray(),
                merchantAccount = ConfiguracaoAdyen.Instancia.Chaves.MerchantAccount.Valor,
                shopperEmail = this.ClienteEmail,
                shopperIP = this.ClienteIP,
                reference = this.ClienteNome,
                sessionId = this.ClienteSessionID,
            };

            Payment px = new Payment();
            px.Credentials = this.Credentials;
            var retorno = px.authorise(p);

            bool autorizado = AdyenStatic.Action.Autorizado(retorno.resultCode);

            this.CodigoAutenticacao = retorno.authCode ?? string.Empty;
            this.CodigoReferencia = retorno.pspReference ?? string.Empty;

            LogUtil.Debug(string.Format("##Adyen.Pagando.SUCCESS## SESSION {0}, RESULT {1}, AUTH {2}, REFUSAL {3}", this.ClienteSessionID, retorno.resultCode, retorno.authCode, retorno.refusalReason));

            if (autorizado)
                return true;

            throw new Exception(retorno.refusalReason);
        }

        public bool CancelarPagamento()
        {
            LogUtil.Debug(string.Format("##Adyen.Cancelando## SESSION {0}", this.ClienteSessionID));

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Extensions.ValidateRemoteCertificate);

            try
            {
                if (string.IsNullOrEmpty(this.CodigoReferencia))
                {
                    LogUtil.Debug(string.Format("##Adyen.Cancelando.ERROR## SESSION {0}, MSG {1}", this.ClienteSessionID, "CodigoReferencia nulo"));

                    throw new Exception("O código de referência deve ser preenchido!");
                }

                var retorno = ConfiguracaoAdyen.Instancia.Chaves.Teste.Valor ? this.CancelarSandbox() : this.Cancelar();
                LogUtil.Debug(string.Format("##Adyen.Cancelando.SUCCESS## SESSION: {0}, RETORNO: {1}", this.ClienteSessionID, retorno));

                return retorno;
            }
            catch (SoapHeaderException ex)
            {
                LogUtil.Error(string.Format("##Adyen.Cancelando.EXCEPTION## SESSION {0}, MSG {1}", this.ClienteSessionID, ex.Message), ex);

                throw new Exception(AdyenStatic.Exceptions.Tratar(ex.Message));
            }
        }

        private bool Cancelar()
        {
            var payment = new Payment { Credentials = this.Credentials };
            var modificationRequest = new ModificationRequest { originalReference = this.CodigoReferencia, merchantAccount = ConfiguracaoAdyen.Instancia.Chaves.MerchantAccount.Valor };

            var retorno = payment.cancelOrRefund(modificationRequest);

            var autorizado = AdyenStatic.Action.Autorizado(retorno.response);
            this.CodigoReferencia = retorno.pspReference;

            LogUtil.Debug(string.Format("##Adyen.Cancelando.SUCCESS## SESSION {0}, RESPONSE {1}, REFERENCE {2}", this.ClienteSessionID, retorno.response, retorno.pspReference));

            return autorizado;
        }

        private bool CancelarSandbox()
        {
            var payment = new wsAdyenSandbox.Payment { Credentials = this.Credentials };
            var modificationRequest = new wsAdyenSandbox.ModificationRequest { originalReference = this.CodigoReferencia, merchantAccount = ConfiguracaoAdyen.Instancia.Chaves.MerchantAccount.Valor };

            var retorno = payment.cancelOrRefund(modificationRequest);

            var autorizado = AdyenStatic.Action.Autorizado(retorno.response);
            this.CodigoReferencia = retorno.pspReference;

            return autorizado;
        }

        public bool CapturarPagamento()
        {
            LogUtil.Debug(string.Format("##Adyen.CapturandoPagamento## SESSION {0}", this.ClienteSessionID));

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Extensions.ValidateRemoteCertificate);

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                var capturaSucesso = ConfiguracaoAdyen.Instancia.Chaves.Teste.Valor ? this.CapturarSandbox() : this.Capturar();

                LogUtil.Debug(string.Format("##Adyen.CapturandoPagamento.FINISH## SESSION: {0}, TEMPO_DECORRIDO_ADYEN_CAPTURA: {1} ms", this.ClienteSessionID, sw.ElapsedMilliseconds));

                return capturaSucesso;
            }
            catch (SoapHeaderException ex)
            {
                LogUtil.Error(string.Format("##Adyen.CapturandoPagamento.EXCEPTION## SESSION {0}, MSG {1}", this.ClienteSessionID, ex.Message), ex);

                throw new Exception(AdyenStatic.Exceptions.Tratar(ex.Message));
            }
        }

        private bool Capturar()
        {
            var payment = new Payment { Credentials = this.Credentials };
            var modificationRequest = new ModificationRequest
            {
                merchantAccount = ConfiguracaoAdyen.Instancia.Chaves.MerchantAccount.Valor,
                originalReference = this.CodigoReferencia,
                modificationAmount = new Amount
                {
                    currency = ConfiguracaoAdyen.Instancia.Chaves.Currency.Valor,
                    value = ValorFormatado
                }
            };

            var jsSerializer = new JavaScriptSerializer();

            var jsonRequest = jsSerializer.Serialize(modificationRequest);
            LogUtil.Debug(String.Format("##Adyen.Capturar## REQUEST: {0}", jsonRequest));

            var retorno = payment.capture(modificationRequest);

            var jsonResponse = jsSerializer.Serialize(retorno);
            LogUtil.Debug(String.Format("##Adyen.Capturar## RESPONSE: {0}", jsonResponse));

            LogUtil.Debug(string.Format("##Adyen.CapturandoPagamento.SUCCESS## SESSION {0}, RESPONSE {1}", this.ClienteSessionID, retorno.response));

            return AdyenStatic.Action.Autorizado(retorno.response);
        }

        private bool CapturarSandbox()
        {
            var payment = new wsAdyenSandbox.Payment { Credentials = this.Credentials };
            var modificationRequest = new wsAdyenSandbox.ModificationRequest
            {
                merchantAccount = ConfiguracaoAdyen.Instancia.Chaves.MerchantAccount.Valor,
                originalReference = this.CodigoReferencia,
                modificationAmount = new wsAdyenSandbox.Amount
                {
                    currency = ConfiguracaoAdyen.Instancia.Chaves.Currency.Valor,
                    value = ValorFormatado
                }
            };

            var retorno = payment.capture(modificationRequest);

            return AdyenStatic.Action.Autorizado(retorno.response);
        }
    }

    public static class AdyenStatic
    {
        public static class Action
        {
            private const string Authorised = "authorised";
            private const string Refused = "refused";
            private const string Error = "error";
            private const string CancelReceived = "[cancel-received]";
            private const string RefundReceived = "[refund-received]";
            private const string CancelOrRefundReceived = "[cancelorrefund-received]";
            private const string CaptureReceived = "[capture-received]";

            public static bool Autorizado(string resultado)
            {
                switch (resultado.ToLower())
                {
                    case Authorised:
                    case CancelReceived:
                    case RefundReceived:
                    case CancelOrRefundReceived:
                    case CaptureReceived:
                        return true;
                    case Refused:
                        return false;
                    case Error:
                        throw new Exception("Ocorreu um erro ao processar o pagamento, por favor tente novamente. Erro 11 - " + resultado);
                    default:
                        throw new Exception("Ocorreu um erro ao processar o pagamento, por favor tente novamente. Erro 12 - " + resultado);
                }
            }
        }

        public static class Exceptions
        {
            private const string CVC = "103";
            private const string InvalidCard = "101";
            private const string InvalidDate = "140";
            public const string Fraud = "FRAUD";
            public const string Refused = "Refused";

            public static string Tratar(string mensagem)
            {
                if (mensagem.Contains(InvalidCard))
                    return "O número do cartão informado não é valido. Por favor tente novamente.";
                else if (mensagem.Contains(CVC))
                    return "O número de verificação (CVV2) informado não é válido, por favor tente novamente.";
                else if (mensagem.Contains(InvalidDate))
                    return "A data de válidade do cartão informado está incorreta. Por favor tente novamente.";
                else if (mensagem.Contains(Fraud))
                    return ConfiguracaoAdyen.Instancia.Chaves.MensagemFraud.Valor;
                else if (mensagem.Contains(Refused))
                    return ConfiguracaoAdyen.Instancia.Chaves.MensagemRefused.Valor;
                else
                    return ConfiguracaoAdyen.Instancia.Chaves.MensagemDefault.Valor;
            }
        }

        public static class Fields
        {
            public const string Installments = "installments";
            public const string NotaFiscal = "Pagamento efetuado através da Adyen, Não existe nota fiscal! Para estornar o valor total cobrado no Cartão de Crédito, marque a opção 'Cancelar Pagamento' na janela de cancelamento.";
            public const string NotaFiscalPaypal = "Pagamento efetuado através do Paypal, Não existe nota fiscal! Para estornar o valor total cobrado no Cartão de Crédito, marque a opção 'Cancelar Pagamento' na janela de cancelamento.";
            public const string NotaFiscalMilhas = "Pagamento efetuado através de Milhas Smiles, Não existe nota fiscal! Para estornar o valor total cobrado no Cartão, será necessario realiza uma solicitação ao serviço de atendimento Smiles.";
            public const string AcquirerCode = "acquirerCode";
            public const string AuthorisationMid = "authorisationMid";
        }

        public enum Adquirente
        {
            [Description("1037590390")]
            Cielo = 0125,
            [Description("40762882")]
            Redecard = 0005
        }
    }
}
