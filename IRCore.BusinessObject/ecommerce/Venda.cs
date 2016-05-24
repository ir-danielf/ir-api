using IngressoRapido.Lib;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.Util;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using IRCore.DataAccess.Model.Enumerator;
using Utils.Web;
using Cliente = IRLib.Cliente;

namespace IRCore.BusinessObject.ecommerce
{
    public static class Venda
    {
        public static RetornoModel<VendaRetornoModel> EfetuaVendaSistemaTEF(EstruturaPagamento oPagamento, List<int> valeIngresso, string celular, decimal ValorTotalSeguro, CarrinhoLista oCarrinhoLista, CompraModel compra, CompraPagamentoModel compraPagamento)
        {
            LogUtil.Debug(string.Format("##Venda.EfetuaVendaSistemaTEF## SESSION {0}", compra.SessionID));

            Carrinho oCarrinho = new Carrinho();
            IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
            string[] retornoVenda = new string[4];
            string retornoMSG = "Ok";
            try
            {
                int clienteID = compra.ClienteID;
                string SessionID = compra.SessionID;
                List<EstruturaDonoIngresso> listaDonoIngresso = new List<EstruturaDonoIngresso>();

                foreach (Carrinho carrinho in oCarrinhoLista.Where(c => c.isCota.Length > 0))
                {
                    listaDonoIngresso.Add(new EstruturaDonoIngresso()
                    {
                        CodigoPromocional = carrinho.CotaItem.CodigoPromocional,
                        DonoID = carrinho.CotaItem.DonoID,
                        CotaItemID = carrinho.CotaItemID,
                        CotaItemIDAPS = carrinho.CotaItemIDAPS,
                        CPF = carrinho.CotaItem.DonoCPF,
                        IngressoID = carrinho.CotaItem.IngressoID,
                        Nominal = carrinho.CotaItem.Nominal,
                        UsarCPFResponsavel = false,
                    });
                }

                IRLib.Cliente oCliente = new IRLib.Cliente();
                oCliente.Ler(clienteID);
                 
                RetornoModel<EstruturaReservaInternet> reservaInternet = MontarEstruturaReserva(compraPagamento, compra);
                if (!reservaInternet.Sucesso)
                {
                    return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = reservaInternet.Mensagem };
                }

                if (reservaInternet.Retorno.CaixaID == 0)
                {
                    reservaInternet.Retorno.CaixaID = bilheteria.VerificaCaixaInternet(reservaInternet.Retorno.UsuarioID, reservaInternet.Retorno.LojaID);
                }

                retornoVenda = bilheteria.VenderWeb(oPagamento, oCarrinho.IngressosTaxasCarrinho(clienteID, SessionID), SessionID, valeIngresso, celular, listaDonoIngresso, reservaInternet.Retorno,
                     ValorTotalSeguro, oCarrinhoLista.TotalTaxaProcessamento(), oCliente);

                VendaRetornoModel retorno = new VendaRetornoModel();
                string tipoEntrega = string.Empty;
                bool eTicket = false;

                IRLib.Entrega entrega = new IRLib.Entrega();

                if (oPagamento.EntregaControleID > 0)
                {
                    tipoEntrega = entrega.BuscaTipo(oPagamento.EntregaControleID);
                    eTicket = entrega.VerificaeTicket(oPagamento.EntregaControleID);
                }

                //ErroIndefinido = -1, FalhaVenda = 0, Sucesso = 1, ClienteInexistente = 2, ReservaInexistente = 3, TaxaEntregaInexistente = 4, FormaPagamentoNaoCadastrada = 5, ReservasCanceladas = 6, ErroSeguro = 7
                switch (retornoVenda[0])
                {
                    case "1": //Sucesso
                        retorno.MsgCodigo = 1;
                        if (retornoVenda.Length > 1)
                        {
                            retorno.Senha = retornoVenda[1];
                        }
                        if (retornoVenda.Length > 2)
                        {
                            retorno.VendaBilheteriaID = Convert.ToInt32(retornoVenda[2]);
                        }
                        if (retornoVenda.Length > 3)
                        {
                            retorno.MsgCelular = retornoVenda[3];
                        }
                        if (retornoVenda.Length > 4)
                        {
                            retorno.Risco = retornoVenda[4];
                        }

                        retornoMSG = TratarEnvioEmailPagamento(oPagamento, oCarrinhoLista, oCliente, clienteID, SessionID, oCarrinho, eTicket, retornoVenda, tipoEntrega, retornoMSG);

                        TratarEnvioNPS(oCliente);

                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = true, Mensagem = "OK", Retorno = retorno };
                    case "7":
                        oCarrinho.SetStatusCarrinhoVV(clienteID, SessionID, "VV", Convert.ToInt64(retornoVenda[2]));

                        if (retornoVenda[0] == "7")
                            retornoMSG = "Sua compra foi efetuada com sucesso, no entanto não foi possivel realizar a compra do Serviço de Seguro Mondial.";

                        retornoMSG = TratarEnvioEmailPagamento(oPagamento, oCarrinhoLista, oCliente, clienteID, SessionID, oCarrinho, eTicket, retornoVenda, tipoEntrega, retornoMSG);

                        TratarEnvioNPS(oCliente);

                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = true, Mensagem = retornoMSG , Retorno = retorno};

                    case "0": // falha ao inserir registro na tVendaBilheteria
                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Falha ao efetuar o pedido. A venda não pôde ser gerada.", Retorno = retorno};
                    case "2": // falha ao inserir registro na tVendaBilheteria
                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Cliente Inexistente. A venda não pôde ser gerada.", Retorno = retorno};
                    case "3":
                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Reserva Inexistente. A venda não pôde ser gerada.", Retorno = retorno};
                    case "4":
                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Taxa de Entrega Inexistente. A venda não pôde ser gerada.", Retorno = retorno};
                    case "5":  
                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento) Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = retornoVenda[1], Retorno = retorno};
                    case "6":
                        retorno.RetornoPagamento = (EstruturaPagamento.enumRetornoPagamento)Convert.ToInt32(retornoVenda[0]);
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Reservas Canceladas. A venda não pôde ser gerada.", Retorno = retorno};
                    default:
                        retorno.RetornoPagamento = EstruturaPagamento.enumRetornoPagamento.PAGAMENTO_NAO_REALIZADO;
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = retornoVenda[1], Retorno = retorno};
                }
            }
            catch (ApplicationException ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<VendaRetornoModel> { Sucesso = false, Mensagem = ex.Message };
            }
        }

        public static RetornoModel<string[]> EfetuaVendaExterna(CompraModel compra, CompraPagamentoExternoModel compraPagamento)
        {
            var oCarrinho = new Carrinho();
            var bilheteria = new IRLib.Bilheteria();

            var retornoMSG = "Ok";
            try
            {
                int clienteId = compra.ClienteID;
                string sessionId = compra.SessionID;

                var oCliente = new Cliente();
                oCliente.Ler(clienteId);

                RetornoModel<EstruturaReservaInternet> reservaInternet = MontarEstruturaReservaExterno(compraPagamento, compra);
                if (!reservaInternet.Sucesso)
                {
                    LogUtil.Error(string.Format("##EfetuaVendaExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, reservaInternet.Mensagem));
                    return new RetornoModel<string[]>() { Sucesso = false, Mensagem = reservaInternet.Mensagem };
                }

                if (reservaInternet.Retorno.CaixaID == 0)
                {
                    reservaInternet.Retorno.CaixaID = bilheteria.VerificaCaixaInternet(reservaInternet.Retorno.UsuarioID, reservaInternet.Retorno.LojaID);
        }

                var oPagamento = EstruturaPagamentoExterno.Montar(compraPagamento.formaPagamento.IR_FormaPagamentoID, compraPagamento.valor, compraPagamento.formaPagamento.Parcelas, compraPagamento.codigoAutorizacao, compraPagamento.NSU, compraPagamento.IP, compraPagamento.formaPagamento.Nome);
                var retornoVenda = bilheteria.VenderExterno(oPagamento, oCarrinho.IngressosTaxasCarrinho(clienteId, sessionId), clienteId, sessionId, reservaInternet.Retorno);

                LogUtil.Info(string.Format("##EfetuaVendaExterna.SUCCESS## SESSION {0}, SENHA {1}", compra.SessionID, retornoVenda));
                return new RetornoModel<string[]>() { Sucesso = true, Mensagem = retornoMSG, Retorno = retornoVenda };
            }
            catch (ApplicationException ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<string[]> { Sucesso = false, Mensagem = ex.Message };
            }
        }

        public static RetornoModel<VendaRetornoModel> EfetuaVendaSistemaTEFCortesia(EstruturaPagamento oPagamento, string celular, CarrinhoLista oCarrinhoLista, CompraModel compra, CompraPagamentoModel compraPagamento)
        {
            Carrinho oCarrinho = new Carrinho();
            IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
            string[] retornoVenda = new string[4];
            string retornoMSG = "Ok";
            try
            {
                int clienteID = compra.ClienteID;
                string SessionID = compra.SessionID;
                List<EstruturaDonoIngresso> listaDonoIngresso = new List<EstruturaDonoIngresso>();

                foreach (Carrinho carrinho in oCarrinhoLista.Where(c => c.isCota.Length > 0))
                {
                    listaDonoIngresso.Add(new EstruturaDonoIngresso()
                    {
                        CodigoPromocional = carrinho.CotaItem.CodigoPromocional,
                        DonoID = carrinho.DonoID,
                        CotaItemID = carrinho.CotaItemID,
                        CotaItemIDAPS = carrinho.CotaItemIDAPS,
                        CPF = carrinho.CotaItem.DonoCPF,
                        IngressoID = carrinho.CotaItem.IngressoID,
                        Nominal = carrinho.CotaItem.Nominal,
                        UsarCPFResponsavel = false,
                    });
                }

                IRLib.Cliente oCliente = new IRLib.Cliente();
                oCliente.Ler(clienteID);

                RetornoModel<EstruturaReservaInternet> reservaInternet = MontarEstruturaReserva(compraPagamento, compra);
                if (!reservaInternet.Sucesso)
                {
                    return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = reservaInternet.Mensagem };
                }

                if (reservaInternet.Retorno.CaixaID == 0)
                {
                    reservaInternet.Retorno.CaixaID = bilheteria.VerificaCaixaInternet(
                        reservaInternet.Retorno.UsuarioID, reservaInternet.Retorno.LojaID);
                }

                retornoVenda = bilheteria.VenderWebCortesia(oPagamento,
                    oCarrinho.IngressosTaxasCarrinho(clienteID, SessionID), SessionID, celular, listaDonoIngresso,
                    reservaInternet.Retorno,
                    oCarrinhoLista.TotalTaxaProcessamento(), oCliente, clienteID);

                VendaRetornoModel retorno = new VendaRetornoModel();
                string tipoEntrega = string.Empty;
                bool eTicket = false;

                IRLib.Entrega entrega = new IRLib.Entrega();

                if (oPagamento.EntregaControleID > 0)
                {
                    tipoEntrega = entrega.BuscaTipo(oPagamento.EntregaControleID);
                    eTicket = entrega.VerificaeTicket(oPagamento.EntregaControleID);
                }

                //ErroIndefinido = -1, FalhaVenda = 0, Sucesso = 1, ClienteInexistente = 2, ReservaInexistente = 3, TaxaEntregaInexistente = 4, FormaPagamentoNaoCadastrada = 5, ReservasCanceladas = 6, 
                switch (retornoVenda[0])
                {
                    case "1": //Sucesso
                        retorno.MsgCodigo = 1;
                        if (retornoVenda.Length > 1)
                        {
                            retorno.Senha = retornoVenda[1];
                        }
                        if (retornoVenda.Length > 2)
                        {
                            retorno.VendaBilheteriaID = Convert.ToInt32(retornoVenda[2]);
                        }
                        if (retornoVenda.Length > 3)
                        {
                            retorno.MsgCelular = retornoVenda[3];
                        }
                        if (retornoVenda.Length > 4)
                        {
                            retorno.Risco = retornoVenda[4];
                        }

                        retornoMSG = TratarEnvioEmailPagamento(oPagamento, oCarrinhoLista, oCliente, clienteID, SessionID, oCarrinho, eTicket, retornoVenda, tipoEntrega, retornoMSG, true);

                        TratarEnvioNPS(oCliente);

                        return new RetornoModel<VendaRetornoModel>()
                        {
                            Sucesso = true,
                            Mensagem = "OK",
                            Retorno = retorno
                        };
                    case "7":
                        oCarrinho.SetStatusCarrinhoVV(clienteID, SessionID, "VV", Convert.ToInt64(retornoVenda[2]));

                        if (retornoVenda[0] == "7")
                            retornoMSG =
                                "Sua compra foi efetuada com sucesso, no entanto não foi possivel realizar a compra do Serviço de Seguro Mondial.";

                        retornoMSG = TratarEnvioEmailPagamento(oPagamento, oCarrinhoLista, oCliente, clienteID, SessionID, oCarrinho, eTicket, retornoVenda, tipoEntrega, retornoMSG);

                        TratarEnvioNPS(oCliente);

                        return new RetornoModel<VendaRetornoModel>() { Sucesso = true, Mensagem = retornoMSG };
                    case "0": // falha ao inserir registro na tVendaBilheteria                        
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Falha ao efetuar o pedido. A venda não pôde ser gerada." };
                    case "2": // falha ao inserir registro na tVendaBilheteria                        
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Cliente Inexistente. A venda não pôde ser gerada." };
                    case "3":
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Reserva Inexistente. A venda não pôde ser gerada." };
                    case "4":
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Taxa de Entrega Inexistente. A venda não pôde ser gerada." };
                    case "6":
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = "Reservas Canceladas. A venda não pôde ser gerada." };
                    default:
                        return new RetornoModel<VendaRetornoModel>() { Sucesso = false, Mensagem = retornoVenda[1] };
                }
            }
            catch (ApplicationException ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<VendaRetornoModel> { Sucesso = false, Mensagem = ex.Message };
            }
        }

        public static RetornoModel<string[]> EfetuaVendaSistemaTEFCortesiaExterna(CompraModel compra, CompraPagamentoExternoModel compraPagamento)
        {
            var oCarrinho = new Carrinho();
            var bilheteria = new IRLib.Bilheteria();

            var retornoMSG = "Ok";

            try
            {
                int clienteID = compra.ClienteID;
                string SessionID = compra.SessionID;

                var oCliente = new Cliente();
                oCliente.Ler(clienteID);

                RetornoModel<EstruturaReservaInternet> reservaInternet = MontarEstruturaReservaExterno(compraPagamento, compra);
                if (!reservaInternet.Sucesso)
                {
                    return new RetornoModel<string[]>() { Sucesso = false, Mensagem = reservaInternet.Mensagem };
            }

                if (reservaInternet.Retorno.CaixaID == 0)
                {
                    reservaInternet.Retorno.CaixaID = bilheteria.VerificaCaixaInternet(
                        reservaInternet.Retorno.UsuarioID, reservaInternet.Retorno.LojaID);
        }

                var oPagamento = EstruturaPagamentoExterno.Montar(compraPagamento.formaPagamento.IR_FormaPagamentoID, compraPagamento.valor, compraPagamento.formaPagamento.Parcelas, compraPagamento.codigoAutorizacao, compraPagamento.NSU, compraPagamento.IP, compraPagamento.formaPagamento.Nome);
                var retornoVenda = bilheteria.VenderInternetCortesiaExterno(oPagamento, oCarrinho.IngressosTaxasCarrinho(clienteID, SessionID), SessionID, reservaInternet.Retorno, oCliente, clienteID);

                return new RetornoModel<string[]>() { Sucesso = true, Mensagem = retornoMSG, Retorno = retornoVenda };
            }
            catch (ApplicationException ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<string[]> { Sucesso = false, Mensagem = ex.Message };
            }
        }

        private static string TratarEnvioEmailPagamento(EstruturaPagamento oPagamento, CarrinhoLista oCarrinhoLista,
            Cliente oCliente, int clienteID, string SessionID, Carrinho oCarrinho, bool eTicket, string[] retornoVenda,
            string tipoEntrega, string retornoMSG, bool EntradaFranca = false)
        {
            try
            {
                string clienteNome = string.Empty;
                int vendaBilheteriaID = Convert.ToInt32(retornoVenda[2]);

                if (!string.IsNullOrEmpty(oCliente.CNPJ.Valor))
                    clienteNome = oCliente.NomeFantasia.Valor;
                else
                    clienteNome = oCliente.Nome.Valor;

                if (!string.IsNullOrEmpty(oCliente.Email.Valor))
                {
                    var parser = new IngressoRapido.TemplateParser.Parser();

                    string formpagamento = string.Empty;
                    if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Nenhum)
                        formpagamento = "Cortesia";
                    else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.VIR)
                        formpagamento = "Vale Ingresso";
                    else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Milhas)
                        formpagamento = "Milhas Smiles";
                    else
                        formpagamento = oPagamento.BandeiraNome.ToString() + " - " + oPagamento.Parcelas + "X";

                    oCarrinhoLista.CarregarDadosPorClienteID(clienteID, SessionID, CarrinhoLista.Status.Reservado, 0);

                    oCarrinho.SetStatusCarrinho(clienteID, SessionID, "V");

                    /*Os robôs são responsáveis por enviar os e-mails de eticket e tef*/

                    var oVendaBilheteria = new IRLib.VendaBilheteria();

                    var detalhesVenda = oVendaBilheteria.AcompanhamentoIngressos(vendaBilheteriaID);

                    var email = new IRLib.EmailAccertify();

                    if (EntradaFranca && oPagamento.EntregaControleID == new IRLib.ConfigGerenciador().getEntregaControleIDMobileTicket())
                    {
                        email.EnviarConfirmacaoCompraMobileTicket(clienteID, detalhesVenda);
                    }
                    else
                    {
                        email.EnviarCompraEmAnalise(clienteID, detalhesVenda);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                retornoMSG = "Sua compra foi efetuada com sucesso, no entanto não foi possivel realizar o envio do email";
            }
            return retornoMSG;
        }

        private static void TratarEnvioNPS(Cliente oCliente)
        {
            bool desabilitar = Convert.ToBoolean(ConfigurationManager.AppSettings["desabilitarNPS"]);
            if (!desabilitar)
            {
                int delay = Convert.ToInt32(ConfigurationManager.AppSettings["delayNPS"]);
                string canalNPS = Convert.ToString(ConfigurationManager.AppSettings["canalNPS"]);

                try
                {
                    using (var bo = new NetPromoterServiceBO())
                    {
                        bo.SendDataNPS(oCliente.Nome.Valor, oCliente.Email.Valor, delay, canalNPS);
                    }
                }
                catch (Exception)
                {
                    using (var bo = new NetPromoterServiceBO())
                    {
                        bo.AdicionarAgendamento(oCliente.Nome.Valor, oCliente.Email.Valor, delay, canalNPS);
                    }
                }
            }
        }

        public static RetornoModel<string> EfetuaVendaSistemaComprarVIR(EstruturaPagamento oPagamento, EstruturaReservaInternet estruturaReservaInternet, string celular, CarrinhoLista oCarrinhoLista, CompraModel compra)
        {
            Carrinho oCarrinho = new Carrinho();
            IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
            string[] retornoVenda = new string[3];

            string senha = string.Empty;
            string msgRetorno = "Ok";

            try
            {
                int clienteID = compra.ClienteID;
                string SessionID = compra.SessionID;

                IRLib.Cliente oCliente = new IRLib.Cliente();
                oCliente.Ler(clienteID);

                if (oCliente.Control.ID == 0)
                    return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Não foi possível localizar o seu cadastro, por favor, tente novamento." };
                
                retornoVenda = bilheteria.VenderVIRWeb(oPagamento, oCarrinho.getValeIngressoID(clienteID, SessionID).ToArray(), SessionID, new List<EstruturaVirNomePresenteado>(), estruturaReservaInternet, celular, oCliente);

                //ErroIndefinido = -1, FalhaVenda = 0, Sucesso = 1, ClienteInexistente = 2, ReservaInexistente = 3, TaxaEntregaInexistente = 4, FormaPagamentoNaoCadastrada = 5, ReservasCanceladas = 6,
                switch (retornoVenda[0])
                {
                    case "1": //Sucesso
                        oCarrinho.SetStatusCarrinho(clienteID, SessionID, "VV");

                        oCarrinhoLista.CarregarDadosVIRporClienteIDSessionID(clienteID, SessionID, CarrinhoLista.Status.VendidoEmail);

                        senha = retornoVenda[1];

                        IngressoRapido.Lib.Entrega oEntrega = new IngressoRapido.Lib.Entrega();
                        EstruturaEntrega oEE = oEntrega.CarregarEstruturaPeloControleID(oPagamento.EntregaControleID);

                        string formpagamento = string.Empty;
                        if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Nenhum)
                            formpagamento = "Cortesia";
                        else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.VIR)
                            formpagamento = "Vale Ingresso";
                        else
                            formpagamento = oPagamento.BandeiraNome.ToString() + " - " + oPagamento.Parcelas + "X";

                        try
                        {
                            if (!string.IsNullOrEmpty(oCliente.CNPJ.Valor))
                                IRLib.ServicoEmail.EnviarVendaAprovadaVir(oCliente.NomeFantasia.Valor, oCliente.Email.Valor, retornoVenda[1], string.Empty, "Site", DateTime.Now.ToString(), "Site", formpagamento, string.Empty, oEE.Nome);
                            else
                                IRLib.ServicoEmail.EnviarVendaAprovadaVir(oCliente.Nome.Valor, oCliente.Email.Valor, retornoVenda[1], string.Empty, "Site", DateTime.Now.ToString(), "Site", formpagamento, string.Empty, oEE.Nome);
                        }
                        catch
                        {
                            msgRetorno = "Sua compra foi efetuada com sucesso no entando o envio para o Email informado gerou um erro, por favor entre em contato com a Ingresso Rápido informando a senha de compra";
                        }

                        oCarrinho.SetStatusCarrinho(clienteID, SessionID, "V");

                        return new RetornoModel<string>() { Sucesso = true, Retorno = senha, Mensagem = msgRetorno };
                    case "0": // falha ao inserir registro na tVendaBilheteria
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Falha ao efetuar o pedido. A venda não pôde ser gerada." };
                    case "2": // falha ao inserir registro na tVendaBilheteria                        
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Cliente Inexistente. A venda não pôde ser gerada." };
                    case "3":
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Reserva Inexistente. A venda não pôde ser gerada." };
                    case "4":
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Taxa de Entrega Inexistente. A venda não pôde ser gerada." };
                    case "6":
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Reservas Canceladas. A venda não pôde ser gerada." };
                    case "-1":
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = retornoVenda[1] };
                    default:
                        return new RetornoModel<string>() { Sucesso = false, Retorno = senha, Mensagem = "Falha ao efetuar o pedido. Erro indefinido." };
                }
            }
            catch (ApplicationException ex)
            {
                return new RetornoModel<string> { Sucesso = false, Retorno = senha, Mensagem = ex.Message };
            }
        }
        
        public static RetornoModel<EstruturaReservaInternet> MontarEstruturaReserva(CompraPagamentoModel pagamento, CompraModel compra)
        {
            if (compra.EstruturaVenda == null)
                compra.EstruturaVenda = new CompraEstruturaVendaModel();

            if (compra.EstruturaVenda.UsuarioID == 0 || compra.EstruturaVenda.UsuarioID == IRLib.Usuario.INTERNET_USUARIO_ID)
            {
                compra.EstruturaVenda.CaixaID = 0;
                compra.EstruturaVenda.LojaID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InternetLojaID));
                compra.EstruturaVenda.UsuarioID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InternetUsuarioID));
                compra.EstruturaVenda.CanalID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.CanalInternet));
                compra.EstruturaVenda.CanalTipo = 1;
            }
            else
            {
                if (compra.EstruturaVenda.CaixaErrado)
                    return new RetornoModel<EstruturaReservaInternet>() { Sucesso = false, Mensagem = "Esse caixa não pode verder na internet. Abra outro caixa!", Retorno = null };
                else if (!compra.EstruturaVenda.CanalVerificado)
                {
                    var canaisSplit = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.CanaisInternet);

                    if (!canaisSplit.Contains(compra.EstruturaVenda.CanalID.ToString()) || !new IRLib.Loja().ValidarLojaCanal(compra.EstruturaVenda.LojaID, new List<string>() { compra.EstruturaVenda.CanalID.ToString() }))
                    {
                        compra.EstruturaVenda.CaixaErrado = true;
                        return new RetornoModel<EstruturaReservaInternet>() { Sucesso = false, Mensagem = "Esse caixa não pode verder na internet. Abra outro caixa!", Retorno = null };
                    }
                    compra.EstruturaVenda.CanalVerificado = true;
                }
            }

            EstruturaReservaInternet estrutura = new EstruturaReservaInternet()
            {
                CaixaID = compra.EstruturaVenda.CaixaID,
                LojaID = compra.EstruturaVenda.LojaID,
                UsuarioID = compra.EstruturaVenda.UsuarioID,
                CanalID = compra.EstruturaVenda.CanalID,
                ClienteID = compra.ClienteID,
                SessionID = compra.SessionID,
                GUID = Guid.NewGuid().ToString(),
            };

            return new RetornoModel<EstruturaReservaInternet>() { Mensagem = "OK", Sucesso = true, Retorno = estrutura };
        }

        public static RetornoModel<EstruturaReservaInternet> MontarEstruturaReservaExterno(CompraPagamentoExternoModel pagamento, CompraModel compra)
        {
            if (compra.EstruturaVenda == null)
                compra.EstruturaVenda = new CompraEstruturaVendaModel();

            if (compra.EstruturaVenda.CanalID == 0 || compra.EstruturaVenda.LojaID == 0 || compra.EstruturaVenda.UsuarioID == 0)
            {
                int canalTipoPOS;
                int.TryParse(ConfiguracaoAppUtil.Get("CanalTipoPOS"), out canalTipoPOS);
                compra.EstruturaVenda.LojaID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.POSLojaID));
                compra.EstruturaVenda.UsuarioID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.POSUsuarioID));
                compra.EstruturaVenda.CanalID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.CanalPOS));
                compra.EstruturaVenda.CanalTipo = canalTipoPOS;
            }

            var estrutura = new EstruturaReservaInternet()
            {
                CaixaID = compra.EstruturaVenda.CaixaID,
                LojaID = compra.EstruturaVenda.LojaID,
                UsuarioID = compra.EstruturaVenda.UsuarioID,
                CanalID = compra.EstruturaVenda.CanalID,
                ClienteID = compra.ClienteID,
                SessionID = compra.SessionID,
                GUID = Guid.NewGuid().ToString(),
            };

            return new RetornoModel<EstruturaReservaInternet>() { Mensagem = "OK", Sucesso = true, Retorno = estrutura };
        }
    }

    public class VendaRetornoModel
    {
        public int MsgCodigo { get; set; }
        public string Senha { get; set; }
        public int VendaBilheteriaID { get; set; }
        public string MsgCelular { get; set; }
        public string Risco { get; set; }
        public EstruturaPagamento.enumRetornoPagamento RetornoPagamento { get; set; }
    }
}
