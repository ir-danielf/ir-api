﻿using COMSITEFMKTLib;
using System;
using System.Configuration;
﻿using IRCore.Util;

namespace IRLib
{
    public class Sitef
    {
        public string terminal { get; set; }

        private static string empresa = ConfigurationManager.AppSettings["EmpresaTEF"].ToString();
        public static string Loja
        {
            get { return empresa; }
            set { empresa = value; }
        }

        const string respostaTEF = "000";
        const string respostaTEF2 = "00";
        const string RESPOSTATIMEOUT = "S0";

        #region Enumeradores
        public enum enumEmpresa
        {
            IngressoRapido = 00000000
        }
        public enum enumBandeira
        {
            Visa = 1,
            Master = 2,
            Diners = 3,
            Amex = 4,
            Aura = 5,
            Credicard = 6,
            Hipercard = 12,
            ValeCultura = 996,
            Elo = 7,
            EloCultura = 8,
            MilagemSmiles = 996, //NAO UTILIZAR, Nao faz parte do TEF, utilizado só para carregar a informacao corretamente na Confirmacao.Tef 
            PayPal = 997, //NAO UTILIZAR, Nao faz parte do TEF, utilizado só para carregar a informacao corretamente na Confirmacao.Tef
            VisaElectron = 998, //NAO UTILIZAR, Nao faz parte do TEF, utilizado só para carregar a informacao corretamente na Confirmacao.Tef
            Itau = 999 //NAO UTILIZAR, Nao faz parte do TEF, utilizado só para carregar a informacao corretamente na Confirmacao.Tef
        }
        public enum enumTipoConfirmacao
        {
            Confirmar = 0,
            Cancelar = 1
        }
        public enum enumTerminal
        {
            SiteIR = 00000001,
            PDV = 00000001,
            CallCenter = 00000001
        }
        public enum enumTipoFinanciamento
        {
            Estabelecimento = 4,
            Administradora = 3
        }
        public enum enumTipoSenha
        {
            //
            NaoColetar = 0,
            Obrigatorio = 1,
            Opcional = 2
        }
        public enum enumTaxaSerivo
        {
            NaoColetar = 0,
            ColetarSeNecessario = 1
        }
        public enum enumTipoDeDeposito
        {
            Dinheiro = 01,
            Cheque = 02,
            DinheiroECheque = 03
        }
        public enum enumTipoTransacao
        {
            Confirma = 0,
            Cancela = 1
        }
        public enum enumFormasPagamento
        {
            Sim = 1,
            Nao = 2
        }
        public enum enumTransacaoComChip
        {
            Nao = 0,
            Sim = 1
        }
        public enum enumCriptografiaCartao
        {
            DES = 1,
            TRIPLEDES = 2,
            DUKPT = 3,
            DUKPTTRIPLEDES = 4
        }
        public enum enumCriptografiaSenhaCartao
        {
            DES = 1,
            TRIPLEDES = 2,
            DUKPT = 3,
            DUKPTTRIPLEDES = 4
        }
        public enum enumTipoVenda
        {
            Credito = 0,
            Debito = 1
        }
        public enum enumTipoOperacao
        {
            CartaoMagnetico = 1,
            CartaoDigitado = 2
        }
        public enumBandeira Bandeira { get; set; }
        #endregion

        #region Enumeradores de Retorno
        public enum enumRetornoSitef
        {
            Ok,
            Invalido,
            ParcialBancoOK,
            ParcialBancoFail
        }

        #endregion

        #region Propriedades
        public enumEmpresa Empresa { get; set; }
        private string IP = ConfigurationManager.AppSettings["IPTEF"].ToString();
        public string Parcelas { get; set; }
        public string RedeRetorno { get; set; }
        public enumTipoConfirmacao TipoConfirmacao { get; set; }
        public enumTerminal Terminal { get; set; }
        public enumTipoFinanciamento TipoFinanciamento { get; set; }
        public int ID { get; set; }
        public string NumeroCartao { get; set; }
        public string DataVencimento { get; set; }
        public string CodigoSeguranca { get; set; }
        public string ValorCompra { get; set; }
        public string ClienteID { get; set; }
        public string DadosConfirmacao { get; set; }
        public string DadosConfirmacaoCancelamento { get; set; }
        public string CodigoRespostaSitefVenda { get; set; }
        public string CodigoRespostaSitefFinaliza { get; set; }
        public string CodigoRespostaIR { get; set; }
        public string DataTransacao { get; set; } // MMDD
        public string HoraTransacao { get; set; } //HHMMSS
        public string NSUHost { get; set; } // Número unico de identificacao da transação
        public string CodigoIR { get; set; } //Codigo da IR
        public string Estabelecimento { get; set; }
        public string NumeroAutorizacao { get; set; } //Numero gerado na compra
        public string NumeroCancelamento { get; set; } //Numero do documento de cancelamento
        public string DataCancelamento { get; set; } //Data de Cancelamento MMDD
        public string HoraCancelamento { get; set; } //HHMMSS
        public string NSUSitef { get; set; } //Numero unico de transacao do Sitef
        public string Rede { get; set; } //ID referente a Bandeira
        public int VendaBilheteriaFormaPagamentoID { get; set; }
        public enumTipoSenha TipoSenha { get; set; }
        public enumTaxaSerivo TaxaServico { get; set; }
        public string NumeroMinimoParcelas = "1";
        public string NumeroMaximoParcelas = "12";
        public enumTransacaoComChip TransacaoComChip { get; set; }
        public string CodigoRoteamento { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoLinhaCredito { get; set; }
        //realizado através de mais de uma forma de pagamento (Ex.: cheque + cartão de débito) 
        public enumFormasPagamento FormasPagamento { get; set; }
        public enumTipoTransacao TipoTransacao { get; set; }
        public string TextoPagamentoNegado { get; set; }
        public string NumeroSeriePinPad { get; set; }
        public string FabricantePin { get; set; }
        public enumTipoDeDeposito TipoDeDeposito { get; set; }
        private enumCriptografiaCartao CriptografiaCartao { get; set; }
        public string TamanhoChaveDoCartao { get; set; } //Tamanho do campo chave + 2 (checksum)
        public string ChaveDoCartao { get; set; }
        private enumCriptografiaSenhaCartao CriptografiaSenhaCartao { get; set; }
        public enumTipoOperacao TipoOperacao { get; set; }
        public string Senha { get; set; } // Deve conter os caracteres digitados pelo cliente, criptografados ou não no PIN PAD.  Em ASCII.
        public string NumeroRecibo { get; set; }
        public string DataFiscal { get; set; } // Data da impressora fiscal (Onde DD=dia, MM=mês e AAAA=ano).
        public string HoraFiscal { get; set; } // Hora da impressora fiscal (Onde HH=hora, MM=minuto e SS=segundo).
        public string CupomFiscal { get; set; } // Informa o número do cupom fiscal.
        public string MensagemVenda { get; set; }
        public string MensagemVendaNegado { get; set; }
        public string MensagemFinaliza { get; set; }
        public string RetornoTEF
        {
            get { return this.CodigoRespostaSitefVenda + " - " + this.MensagemVenda; }
        }
        #endregion

        private SiTefMKTClass oSitef;

        /// <summary>
        /// Inicia o Sitef, Configurando IP, Terminal e Empresa
        /// </summary>
        /// <returns></returns>
        public enumRetornoSitef IniciaSitef()
        {
            LogUtil.Debug(string.Format("##Sitef.IniciandoVenda## CLIENTE {0}", this.ClienteID));

            oSitef = new SiTefMKTClass();
            oSitef.configura(IP, terminal, empresa);

            LogUtil.Debug(string.Format("##Sitef.EfetuandoVenda## CLIENTE {0}", this.ClienteID));
            if (this.EfetuaVenda() != enumRetornoSitef.Ok)
            {
                if ((!string.IsNullOrEmpty(this.CodigoRespostaSitefVenda) && this.CodigoRespostaSitefVenda.Contains("S0")) ||
                    (string.IsNullOrEmpty(this.CodigoRespostaSitefVenda + this.MensagemVenda)))
                    throw new TimeoutException("Houve uma falha na comunicação, por favor, aguarde alguns instantes e tente novamente");
                else
                    throw new Exception(this.CodigoRespostaSitefVenda + " - " + this.MensagemVenda);
            }
            return enumRetornoSitef.Ok;
        }


        public enumRetornoSitef IniciaSitefDebito()
        {
            LogUtil.Debug(string.Format("##Sitef.IniciandoConsultaDebito## CLIENTE {0}", this.ClienteID));

            oSitef = new SiTefMKTClass();
            oSitef.configura(IP, terminal, empresa);

            if (this.ConsultaCartaoDebito() != enumRetornoSitef.Ok)
            {
                if (string.IsNullOrEmpty(this.CodigoRespostaSitefVenda))
                    throw new Exception("Houve uma falha na comunicação, por favor, aguarde alguns instantes e tente novamente");
                else
                    throw new Exception(this.CodigoRespostaSitefVenda + " - " + this.MensagemVenda);
            }


            if (this.EfetuaVendaDebito() != enumRetornoSitef.Ok)
            {
                if ((!string.IsNullOrEmpty(this.CodigoRespostaSitefVenda) && this.CodigoRespostaSitefVenda.Contains("S0")) ||
                    (string.IsNullOrEmpty(this.CodigoRespostaSitefVenda + this.MensagemVenda)))
                    throw new TimeoutException("Houve uma falha na comunicação, por favor, aguarde alguns instantes e tente novamente");
                else
                    throw new Exception(this.CodigoRespostaSitefVenda + " - " + this.MensagemVenda);
            }
            return enumRetornoSitef.Ok;
        }

        /// <summary>
        /// Inicia o Sitef, Configurando IP, Terminal e Empresa
        /// </summary>
        /// <returns></returns>
        public enumRetornoSitef IniciaConsulta()
        {
            LogUtil.Debug(string.Format("##Sitef.IniciandoConsulta## CLIENTE {0}", this.ClienteID));

            oSitef = new SiTefMKTClass();
            oSitef.configura(IP, terminal, empresa);

            //return enumRetornoSitef.Ok;

            LogUtil.Debug(string.Format("##Sitef.ConsultandoCartão## CLIENTE {0}", this.ClienteID));
            if (this.ConsultaCartao() != enumRetornoSitef.Ok)
            {
                if (string.IsNullOrEmpty(this.CodigoRespostaSitefVenda))
                    throw new Exception("Houve uma falha na comunicação, por favor, aguarde alguns instantes e tente novamente");
                else
                    throw new Exception(this.CodigoRespostaSitefVenda + " - " + this.MensagemVenda);
            }
            return enumRetornoSitef.Ok;
        }

        /// <summary>
        /// Faz a consulta do cartao para verificar bandeira, credito disponivel
        /// </summary>
        /// <returns></returns>
        public enumRetornoSitef ConsultaCartao()
        {
            try
            {
                oSitef.iniciaTransacao();

                if (!string.IsNullOrEmpty(this.Rede))
                    oSitef.gravaCampo(eCampo.MKT_REDE, this.Rede);

                oSitef.gravaCampo(eCampo.MKT_NUMERO_CARTAO, this.NumeroCartao);
                oSitef.gravaCampo(eCampo.MKT_DATA_VENCIMENTO, this.DataVencimento);
                if (this.CodigoSeguranca.Length > 0)
                    oSitef.gravaCampo(eCampo.MKT_CODIGO_SEGURANCA, this.CodigoSeguranca);

                oSitef.executa(eAcao.MKT_CONSULTA_CARTAO);

                this.CodigoRespostaSitefVenda = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_SITEF);
                this.MensagemVenda = "Número do cartão inválido"; //Só tem esse tipo de resultado se o código for diferente das respostas 00 ou 0

                var retornoSitef = enumRetornoSitef.Invalido;
                if (!string.IsNullOrEmpty(this.CodigoRespostaSitefVenda) && (this.CodigoRespostaSitefVenda == respostaTEF || this.CodigoRespostaSitefVenda == respostaTEF2))
                    retornoSitef = enumRetornoSitef.Ok;

                LogUtil.Debug(string.Format("##Sitef.IniciandoConsulta.SUCCESS## CLIENTE {0}, MSG {1}", this.ClienteID, retornoSitef));

                return retornoSitef;

            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Sitef.IniciandoConsulta.EXCEPTION## CLIENTE {0}, MSG {1}", this.ClienteID, ex.Message), ex);
                throw ex;
            }
        }

        public enumRetornoSitef ConsultaCartaoDebito()
        {
            try
            {
                oSitef.iniciaTransacao();

                oSitef.gravaCampo(eCampo.MKT_NUMERO_CARTAO, this.NumeroCartao);

                if (this.Bandeira == enumBandeira.ValeCultura)
                    oSitef.gravaCampo(eCampo.MKT_TIPO_VENDA, ((int)enumTipoVenda.Debito).ToString());

                oSitef.executa(eAcao.MKT_CONSULTA_CARTAO);

                this.CodigoRespostaSitefVenda = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_SITEF);

                var retornoSitef = enumRetornoSitef.Invalido;
                if (this.CodigoRespostaSitefVenda == respostaTEF || this.CodigoRespostaSitefVenda == respostaTEF2)
                {
                    this.RedeRetorno = Convert.ToInt32(oSitef.leCampo(eCampo.MKT_REDE)).ToString("00000");

                    if (this.Bandeira == enumBandeira.ValeCultura)
                    {
                        this.DataVencimento = oSitef.leCampo(eCampo.MKT_DATA_VENCIMENTO);
                        this.CodigoRoteamento = oSitef.leCampo(eCampo.MKT_CODIGO_ROTEAMENTO);
                        this.CodigoProduto = oSitef.leCampo(eCampo.MKT_CODIGO_PRODUTO);
                        this.CodigoLinhaCredito = oSitef.leCampo(eCampo.MKT_CODIGO_LINHA_CREDITO);
                    }

                    retornoSitef = enumRetornoSitef.Ok;
                }

                LogUtil.Debug(string.Format("##Sitef.IniciandoConsultaDebito.SUCCESS## CLIENTE {0}, MSG {1}", this.ClienteID, retornoSitef));

                return retornoSitef;


            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Sitef.IniciandoConsultaDebito.EXCEPTION## CLIENTE {0}, MSG {1}", this.ClienteID, ex.Message), ex);

                throw ex;
            }
        }

        /// <summary>
        /// Efetua a venda sem finalizar a transação
        /// </summary>
        /// <returns></returns>
        private enumRetornoSitef EfetuaVenda()
        {
            oSitef.iniciaTransacao();

            if (!string.IsNullOrEmpty(this.Rede))
                oSitef.gravaCampo(eCampo.MKT_REDE, this.Rede);

            oSitef.gravaCampo(eCampo.MKT_NUMERO_CARTAO, this.NumeroCartao);
            oSitef.gravaCampo(eCampo.MKT_NUMERO_PARCELAS, this.Parcelas);
            oSitef.gravaCampo(eCampo.MKT_TIPO_FINANCIAMENTO, Convert.ToString((int)this.TipoFinanciamento));
            oSitef.gravaCampo(eCampo.MKT_DATA_VENCIMENTO, this.DataVencimento);
            if (this.CodigoSeguranca.Length > 0)
                oSitef.gravaCampo(eCampo.MKT_CODIGO_SEGURANCA, this.CodigoSeguranca);
            oSitef.gravaCampo(eCampo.MKT_VALOR, this.ValorCompra.ToString().Replace(",", "").Replace(".", ""));
            oSitef.gravaCampo(eCampo.MKT_CODIGO_CLIENTE, this.ClienteID);

            oSitef.executa(eAcao.MKT_VENDA_SITEF);

            this.CodigoRespostaSitefVenda = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_SITEF);

            this.MensagemVenda = oSitef.leCampo(eCampo.MKT_TEXTO_EXIBICAO);

            var retornoSitef = enumRetornoSitef.Invalido;
            if (this.CodigoRespostaSitefVenda == respostaTEF || this.CodigoRespostaSitefVenda == respostaTEF2)
            {
                string Cupom = string.Empty;

                this.CodigoRespostaIR = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_INSTITUICAO);
                this.HoraTransacao = oSitef.leCampo(eCampo.MKT_HORA);
                this.DataTransacao = oSitef.leCampo(eCampo.MKT_DATA);
                this.CodigoIR = oSitef.leCampo(eCampo.MKT_CODIGO_ESTABELECIMENTO);
                this.NumeroAutorizacao = oSitef.leCampo(eCampo.MKT_NUMERO_AUTORIZACAO);
                this.NSUSitef = oSitef.leCampo(eCampo.MKT_NSU_SITEF);
                this.NSUHost = oSitef.leCampo(eCampo.MKT_NSU_HOST);

                //ENGENHARIA DE OCASIA, NAO ME ORGULHO DISSO. ASS. Gustavo//
                this.RedeRetorno = Convert.ToInt32(oSitef.leCampo(eCampo.MKT_REDE)).ToString("00000");

                while (oSitef.existemMaisElementos(eCampo.MKT_LINHAS_CUPOM))
                    Cupom += oSitef.leCampo(eCampo.MKT_LINHAS_CUPOM).Trim() + Environment.NewLine;

                if (Cupom.Length != 0)
                    this.CupomFiscal = Cupom;

                //MMDD + Cupom + ## -> AP = Aprovado Visa?? 05 = Aprovado MC??
                this.DadosConfirmacao = oSitef.leCampo(eCampo.MKT_DADOS_CONFIRMACAO);

                retornoSitef = enumRetornoSitef.Ok;
            }

            LogUtil.Debug(string.Format("##Sitef.IniciandoConsultaDebito.SUCCESS## CLIENTE {0}, MSG {1}, AUTH {2}", this.ClienteID, retornoSitef, this.NumeroAutorizacao));

            return retornoSitef;
        }

        /// <summary>
        /// Metodo nao existe no sitef
        /// </summary>
        /// <returns></returns>
        public enumRetornoSitef EfetuaVendaDebito()
        {
            try
            {
                oSitef.iniciaTransacao();

                if (this.Bandeira != enumBandeira.ValeCultura && !string.IsNullOrEmpty(this.RedeRetorno))
                    oSitef.gravaCampo(eCampo.MKT_REDE, this.RedeRetorno);

                oSitef.gravaCampo(eCampo.MKT_NUMERO_CARTAO, this.NumeroCartao);

                if (this.CodigoSeguranca.Length > 0)
                    oSitef.gravaCampo(eCampo.MKT_CODIGO_SEGURANCA, this.CodigoSeguranca);

                oSitef.gravaCampo(eCampo.MKT_VALOR, this.ValorCompra.ToString().Replace(",", "").Replace(".", ""));
                oSitef.gravaCampo(eCampo.MKT_CODIGO_CLIENTE, this.ClienteID);
                oSitef.gravaCampo(eCampo.MKT_TIPO_VENDA, ((int)enumTipoVenda.Debito).ToString());
                oSitef.gravaCampo(eCampo.MKT_DATA_VENCIMENTO, this.DataVencimento);

                if (!string.IsNullOrEmpty(this.CodigoRoteamento))
                    oSitef.gravaCampo(eCampo.MKT_CODIGO_ROTEAMENTO, this.CodigoRoteamento);
                if (!string.IsNullOrEmpty(this.CodigoProduto))
                    oSitef.gravaCampo(eCampo.MKT_CODIGO_PRODUTO, this.CodigoProduto);
                if (!string.IsNullOrEmpty(this.CodigoLinhaCredito))
                    oSitef.gravaCampo(eCampo.MKT_CODIGO_LINHA_CREDITO, this.CodigoLinhaCredito);

                oSitef.executa(eAcao.MKT_VENDA_SITEF_DEBITO);

                this.CodigoRespostaSitefVenda = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_SITEF);

                this.MensagemVenda = oSitef.leCampo(eCampo.MKT_TEXTO_EXIBICAO);

                if (this.CodigoRespostaSitefVenda == respostaTEF || this.CodigoRespostaSitefVenda == respostaTEF2)
                {
                    string Cupom = string.Empty;

                    this.CodigoRespostaIR = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_INSTITUICAO);
                    this.HoraTransacao = oSitef.leCampo(eCampo.MKT_HORA);
                    this.DataTransacao = oSitef.leCampo(eCampo.MKT_DATA);
                    this.CodigoIR = oSitef.leCampo(eCampo.MKT_CODIGO_ESTABELECIMENTO);
                    this.NumeroAutorizacao = oSitef.leCampo(eCampo.MKT_NUMERO_AUTORIZACAO);
                    this.NSUSitef = oSitef.leCampo(eCampo.MKT_NSU_SITEF);
                    this.NSUHost = oSitef.leCampo(eCampo.MKT_NSU_HOST);

                    //ENGENHARIA DE OCASIA, NAO ME ORGULHO DISSO. ASS. Gustavo//
                    this.RedeRetorno = Convert.ToInt32(oSitef.leCampo(eCampo.MKT_REDE)).ToString("00000");

                    while (oSitef.existemMaisElementos(eCampo.MKT_LINHAS_CUPOM))
                        Cupom += oSitef.leCampo(eCampo.MKT_LINHAS_CUPOM).Trim() + Environment.NewLine;

                    if (Cupom.Length != 0)
                        this.CupomFiscal = Cupom;

                    //MMDD + Cupom + ## -> AP = Aprovado Visa?? 05 = Aprovado MC??
                    this.DadosConfirmacao = oSitef.leCampo(eCampo.MKT_DADOS_CONFIRMACAO);

                    return enumRetornoSitef.Ok;
                }
                else
                {
                    return enumRetornoSitef.Invalido;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo que finaliza a transacao obtendo o codigo de resposta 
        /// </summary>
        /// <returns></returns>
        public enumRetornoSitef FinalizaTransacao(enumTipoConfirmacao tipoConfirmacao)
        {
            LogUtil.Info(string.Format("##Sitef.FinalizandoTransacao## CLIENTE {0}, TIPO {1}", this.ClienteID, tipoConfirmacao));

            try
            {
                var retornoSitef = enumRetornoSitef.Invalido;

                if (oSitef == null)
                {
                    retornoSitef = enumRetornoSitef.Ok;
                }
                else if (this.DadosConfirmacao == null)
                {
                    retornoSitef = enumRetornoSitef.ParcialBancoOK;
                }
                else
                {
                    oSitef.gravaCampo(eCampo.MKT_DADOS_CONFIRMACAO, this.DadosConfirmacao);
                    oSitef.gravaCampo(eCampo.MKT_TIPO_TRANSACAO, "0"); //Venda = 0 ; ;; pgtoContas = 1 IGNORAR 1!
                    oSitef.gravaCampo(eCampo.MKT_TIPO_CONFIRMACAO, Convert.ToString((int)tipoConfirmacao));
                    oSitef.executa(eAcao.MKT_FINALIZA_TRANSACAO);

                    //131 = Aprovado? //130 = Cancelado?
                    this.CodigoRespostaSitefFinaliza = oSitef.leCampo(eCampo.MKT_CODIGO_RESPOSTA_SITEF);

                    retornoSitef = enumRetornoSitef.Ok;
                }

                LogUtil.Info(string.Format("##Sitef.FinalizandoTransacao.SUCCESS## CLIENTE {0}, TIPO {1}, RETORNO {2}", this.ClienteID, tipoConfirmacao, retornoSitef));

                return retornoSitef;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Sitef.FinalizandoTransacao.EXCEPTION## CLIENTE {0}, TIPO {1}, MSG {2}", this.ClienteID, tipoConfirmacao, ex.Message), ex);

                throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");
            }

        }

        /// <summary>
        /// Atualiza a compra utilizando o valor de retorno do TEF
        /// </summary>
        /// <returns></returns>
        public bool AtualizarRetornoFinaliza()
        {
            try
            {
                //    oTEF.CodigoRespostaTransacao.Valor = Convert.ToInt32(this.CodigoRespostaSitefFinaliza);
                //    oTEF.Control.ID = this.ID;
                //    oTEF.Atualizar();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Meotodo de cancelamento por identificacao (DadosConfirmacao)
        /// </summary>
        /// <returns></returns>
        public enumRetornoSitef CancelamentoPorIdentificacao()
        {
            try
            {
                oSitef.iniciaTransacao();
                oSitef.gravaCampo(eCampo.MKT_DADOS_CONFIRMACAO, this.DadosConfirmacao);
                oSitef.gravaCampo(eCampo.MKT_HORA_FISCAL, this.HoraFiscal);
                oSitef.gravaCampo(eCampo.MKT_DATA_FISCAL, this.DataFiscal);
                oSitef.gravaCampo(eCampo.MKT_CODIGO_CLIENTE, this.ClienteID);

                oSitef.executa(eAcao.MKT_CANCELAMENTO_IDENTIFICADO);

                this.DadosConfirmacaoCancelamento = oSitef.leCampo(eCampo.MKT_DADOS_CONFIRMACAO);
                this.NumeroCancelamento = oSitef.leCampo(eCampo.MKT_NUMERO_AUTORIZACAO);
                return enumRetornoSitef.Ok;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool ConsolidarValorTotal(decimal valorTotal)
        {
            if (Convert.ToDecimal(this.ValorCompra) != valorTotal)
                return false;

            return true;
        }
    }
}