using System;
using System.Configuration;
using System.Linq;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.Util;
using IRLib.ExpressCheckout;
using IRLib.PayPal;
using IRLib.PayPal.Enums;

namespace IRCore.BusinessObject
{
    public class PayPalBO
    {
        public RetornoModel<SetExpressCheckoutResponseModel> setExpressCheckout(CompraModel compra, string returnUrl, string cancelUrl)
        {
            var retorno = new RetornoModel<SetExpressCheckoutResponseModel>();
            var setExpressCheckoutResponse = new SetExpressCheckoutResponseModel();
            
            if (compra.Login != null && compra.Login.ID > 0)
            {
                if (compra.CarrinhoItens != null && compra.CarrinhoItens.Any())
                {
                    if (compra.EntregaControleID > 0)
                    {
                        using (var ado = new MasterADOBase())
                        {
                            var eventoId = compra.CarrinhoItens.FirstOrDefault().EventoID;

                            var urlPagamentoAprovadoUsuario = !string.IsNullOrEmpty(returnUrl) ? returnUrl : ConfigurationManager.AppSettings["urlPagamentoAprovadoUsuario"];
                            urlPagamentoAprovadoUsuario = string.Format(urlPagamentoAprovadoUsuario, eventoId);

                            var urlPagamentoCanceladoUsuario = !string.IsNullOrEmpty(cancelUrl) ? cancelUrl : ConfigurationManager.AppSettings["urlPagamentoCanceladoUsuario"];
                            urlPagamentoCanceladoUsuario = string.Format(urlPagamentoCanceladoUsuario, eventoId);

                            var clienteADO = new ClienteADO(ado);

                            var cliente = clienteADO.Consultar(compra.ClienteID);

                            LogUtil.Debug(string.Format("##Get.PayPal.Token Consultar cliente## SESSION {0}, CLIENTE ID {1}", compra.SessionID, cliente.ID));

                            var endereco = clienteADO.ConsultarEndereco(compra.ClienteEnderecoID);

                            LogUtil.Debug(string.Format("##Get.PayPal.Token Consultar endereço## SESSION {0}, CLIENTE ID {1}", compra.SessionID, cliente.ID));

                            //Get token

                            LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout## CLIENTE ID {0}, URL PAGAMENTO APROVADO {1}, URL PAGAMENTO CANCELADO {2}", cliente.ID, urlPagamentoAprovadoUsuario, urlPagamentoCanceladoUsuario));

                            var expressCheckoutApi = PayPalApiFactory.instance.ExpressCheckout();
                            var setExpressCheckout = expressCheckoutApi.SetExpressCheckout(urlPagamentoAprovadoUsuario, urlPagamentoCanceladoUsuario);

                            setExpressCheckout.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                            setExpressCheckout.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                            foreach (var item in compra.CarrinhoItens)
                            {
                                var descricao = string.Format("{0} - {1} - {2}", item.ApresentacaoDataHoraAsDateTime.ToString("dd/MM/yyyy"), item.PrecoNome, item.Setor);
                                var preco = Convert.ToDouble(item.PrecoValor) + Convert.ToDouble(item.TaxaConveniencia);
                                setExpressCheckout.PaymentRequest(0).addItem(item.Evento, 1, preco, descricao);

                                LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout## CLIENTE ID {0}, ITEM CARRINHO {1}, PREÇO {2}", cliente.ID, descricao, preco));
                            }

                            var entrega = compra.EntregaControles.FirstOrDefault(x => x.ID == compra.EntregaControleID);

                            LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout## CLIENTE ID {0}, TAXA DE ENTREGA {1}", cliente.ID, entrega.Entrega.Nome));

                            setExpressCheckout.PaymentRequest(0).addItem("Taxa de entrega", 1, Convert.ToDouble(entrega.Valor), entrega.Entrega.Nome);

                            setExpressCheckout.PaymentRequest(0).CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                            if (endereco != null && endereco.ID > 0)
                            {
                                LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout## CLIENTE ID {0}, ENDEREÇO ID {1}", cliente.ID, endereco.ID));

                                setExpressCheckout.AddressOverride = true;

                                var enderecoCompleto = string.Format("{0}, {1} {2} - {3}", endereco.Endereco, endereco.Numero, endereco.Complemento, endereco.Bairro);

                                setExpressCheckout.PaymentRequest(0).ShipToName = endereco.Nome;
                                setExpressCheckout.PaymentRequest(0).ShipToStreet = enderecoCompleto;
                                setExpressCheckout.PaymentRequest(0).ShipToZip = endereco.CEP;
                                setExpressCheckout.PaymentRequest(0).ShipToState = endereco.Estado;
                                setExpressCheckout.PaymentRequest(0).ShipToCity = endereco.Cidade;
                                setExpressCheckout.PaymentRequest(0).ShipToCountryCode = LocaleCode.BRAZIL.ToString();

                                if (!string.IsNullOrEmpty(cliente.DDDCelular) && !string.IsNullOrEmpty(cliente.Celular))
                                {
                                    setExpressCheckout.PaymentRequest(0).ShipToPhoneNum = string.Format("{0}{1}", cliente.DDDCelular, cliente.Celular);
                                }
                            }
                            else
                            {
                                setExpressCheckout.NoShipping = true;
                            }

                            setExpressCheckout.MaxAmount = Convert.ToDouble(compra.Total.ValorTotal);

                            var response = setExpressCheckout.execute();
                            
                            if (response.ResponseNVP.Ack == Ack.SUCCESS)
                            {
                                setExpressCheckoutResponse.token = response.Token;

                                retorno.Sucesso = true;
                                retorno.Mensagem = "OK";

                                LogUtil.Info(string.Format("##Get.PayPal.Token SetExpressCheckout.SUCCESS## SESSION {0}, TOKEN {1}", compra.SessionID, setExpressCheckoutResponse.token));
                            }
                            else
                            {
                                retorno.Sucesso = false;
                                retorno.Mensagem = "Não foi possível efetuar o pagamento com PayPal. Tente novamente ou selecione outra forma de pagamento";

                                LogUtil.Info(string.Format("##Get.PayPal.Token SetExpressCheckout.ERROR## SESSION {0}, MSG {1}", compra.SessionID, response.ResponseNVP.getError(0).LongMessage));
                            }
                            
                        }
                    }
                    else
                    {
                        retorno.Sucesso = false;
                        retorno.Mensagem = "Selecione uma entrega";

                        LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout.ERROR## SESSION {0}, MSG {1}", compra.SessionID, retorno.Mensagem));
                    }
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "Nenhum item encontrado no carrinho";

                    LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout.ERROR## SESSION {0}, MSG {1}", compra.SessionID, retorno.Mensagem));
                }

            }
            else
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum cliente logado";

                LogUtil.Debug(string.Format("##Get.PayPal.Token SetExpressCheckout.ERROR## SESSION {0}, MSG {1}", compra.SessionID, retorno.Mensagem));
            }

            retorno.Retorno = setExpressCheckoutResponse;

            return retorno;
        }
    }
}