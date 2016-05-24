using System.Web.Mvc;
using IRCore.BusinessObject.ecommerce;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using IRLib.CancelamentoIngresso;
using IRLib.ClientObjects;
using IRLib.Codigo.Brainiac;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using IngressoRapido.Lib;
using IRLib;
using IRLib.Emails;
using IRLib.PayPal;
using CancelaPayPal = IRLib.Paralela.PayPal.CancelaPayPal;
using Carrinho = IRCore.DataAccess.Model.Carrinho;
using Login = IRCore.DataAccess.Model.Login;
using Venda = IRCore.BusinessObject.ecommerce.Venda;
using Utils.Web;

namespace IRCore.BusinessObject
{
    public class VendaBilheteriaBO : MasterBO<VendaBilheteriaADO>
    {
        public VendaBilheteriaBO(MasterADOBase ado) : base(ado) { }

        public VendaBilheteriaBO() : base(null) { }

        public RetornoModel<tVendaBilheteria> VenderVoucher(Voucher voucher, Login login, List<Carrinho> carrinhoList, tEntregaControle entregaControleEscolhido, string ipAddress, int pdvId, tClienteEndereco enderecoEscolhido, CortesiaModel cortesiaModel)
        {

            int usuarioId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.usuarioIdSistema);
            int lojaId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.lojaIdSistema);
            int empresaId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.empresaIdSistema);


            if ((usuarioId == 0) || (lojaId == 0) || (empresaId == 0))
            {
                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = null, Mensagem = "É necessário ter configurado no config usuarioIdSistema, lojaIdSistema e empresaIdSistema" };
            }

            if ((carrinhoList == null) || (carrinhoList.Count == 0))
            {
                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = null, Mensagem = "Não é possível realizar a venda sem itens no carrinho" };
            }

            LojaADO lojaADO = new LojaADO(ado);
            tLoja loja = lojaADO.Consultar(lojaId);

            if (loja == null)
            {
                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível encontrar a lojaIdSistema configurada com " + lojaId };
            }

            CanalADO canalADO = new CanalADO(ado);
            tCanal canal = canalADO.Consultar(loja.CanalID.Value);

            if (canal == null)
            {
                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível encontrar um canal para a lojaIdSistema configurada com " + lojaId };
            }
            else if (canal.Comissao == null)
            {
                canal.Comissao = 0;
            }

            CaixaBO caixaBO = new CaixaBO(ado);


            tCaixa caixa = caixaBO.ConsultarCaixaInternet(usuarioId, loja, canal).Retorno;

            if (caixa == null)
            {
                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível encontrar ou abrir um caixa" };
            }

            tVendaBilheteria vendaBilheteria = null;

            IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();

            int entregaControleID = entregaControleEscolhido.ID;
            int clienteEnderecoID = 0;
            if (enderecoEscolhido != null)
            {
                clienteEnderecoID = enderecoEscolhido.ID;
            }

            Dictionary<int, decimal> ingressosID = new Dictionary<int, decimal>();
            foreach (var carrinho in carrinhoList)
            {
                ingressosID[carrinho.IngressoID.Value] = 0;
            }

            EstruturaReservaInternet estruturaReservaInternet = new EstruturaReservaInternet();
            estruturaReservaInternet.ClienteID = login.ClienteID;
            estruturaReservaInternet.UsuarioID = usuarioId;
            estruturaReservaInternet.CaixaID = caixa.ID;
            estruturaReservaInternet.SessionID = voucher.SessionID;
            estruturaReservaInternet.LojaID = loja.ID;
            estruturaReservaInternet.CanalID = canal.ID;
            estruturaReservaInternet.EmpresaID = empresaId;
            estruturaReservaInternet.GUID = Guid.NewGuid().ToString();

            VoucherBO voucherBO = new VoucherBO(ado);
            CarrinhoBO carrinhoBO = new CarrinhoBO(ado);

            voucher.StatusAsEnum = enumVoucherStatus.capturado;
            if (!voucherBO.AtualizarStatus(voucher, enumVoucherStatus.disponivel))
            {
                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = vendaBilheteria, Mensagem = "Voucher Indisponivel" };
            }

            int id = 0;

            try
            {
                EstruturaCortesia ec = new EstruturaCortesia();
                ec.CorID = cortesiaModel.CorID;
                ec.ID = cortesiaModel.ID;
                ec.LocalID = cortesiaModel.LocalID;
                ec.Nome = cortesiaModel.Nome;
                ec.Obs = cortesiaModel.Obs;
                ec.Padrao = cortesiaModel.Padrao;
                ec.ParceiroMidiaID = cortesiaModel.ParceiroMidiaID;

                id = bilheteria.VenderInternet(ingressosID, voucher.SessionID, estruturaReservaInternet, entregaControleID, pdvId, clienteEnderecoID, login.ClienteID, ipAddress, ec);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }

            if (id != 0)
            {
                vendaBilheteria = ado.Consultar(id);
            }

            if (vendaBilheteria == null)
            {
                voucher.VendaBilheteriaID = null;
                voucher.DataUso = null;
                voucher.ClienteID = null;
                voucher.SessionID = null;
                voucher.StatusAsEnum = enumVoucherStatus.disponivel;

                voucherBO.AtualizarStatus(voucher, enumVoucherStatus.capturado);
                carrinhoBO.LimparReserva(voucher.SessionID, enumIngressoStatus.bloqueado, carrinhoList);

                return new RetornoModel<tVendaBilheteria>() { Sucesso = false, Retorno = vendaBilheteria, Mensagem = "Erro no processamento da Compra" };
            }
            else
            {
                // Troca Status do Carrinho
                carrinhoBO.AtualizarStatus(voucher.SessionID, enumCarrinhoStatus.vendido);
                carrinhoList.ForEach(t => t.StatusAsEnum = enumCarrinhoStatus.vendido);

                voucher.VendaBilheteriaID = id;
                voucher.DataUso = DateTime.Now;
                voucherBO.AtualizarStatus(voucher, enumVoucherStatus.capturado);

                try
                {
                    var conteudoEmail = VenderVoucherConteudoEmail(voucher, login, carrinhoList, entregaControleEscolhido, vendaBilheteria);

                    try
                    {
                        Mail.EnviarMediaPartnerConfirmacao(login.Email, voucher.ParceiroMidia.Nome, conteudoEmail, voucher.ParceiroMidia.UrlContexto.ToLower());
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                    }
                }
                catch (Exception ex1)
                {
                    LogUtil.Error(ex1);
                }
            }

            return new RetornoModel<tVendaBilheteria>() { Sucesso = true, Retorno = vendaBilheteria, Mensagem = "OK" };

        }

        public RetornoModel<byte[]> GerarPDFErro()
        {
            RetornoModel<byte[]> retorno = new RetornoModel<byte[]>();
            retorno.Sucesso = false;
            retorno.Mensagem = "Erro ao Gerar Eticket";

            return retorno;
        }

        public RetornoModel<byte[]> GerarPDF()
        {
            RetornoModel<byte[]> retorno = new RetornoModel<byte[]>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";

            return retorno;
        }

        public RetornoModel<byte[]> GerarPDF(int ID)
        {
            RetornoModel<byte[]> retorno = new RetornoModel<byte[]>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";

            try
            {
                IRLib.IngressoGerenciadorAccertify ingressos = new IRLib.IngressoGerenciadorAccertify();
                List<IRLib.IngressoImpressao> listaIngressos = ingressos.PesquisaVendaBilheteria(ID);

                if (listaIngressos.Count == 0)
                {
                    retorno.Mensagem = "Não existem ingressos nesta compra!";
                    retorno.Sucesso = false;
                }
                else
                {
                    using (IRLib.GeradorPDFeTicket gerador = new IRLib.GeradorPDFeTicket())
                    {
                        using (MemoryStream ms = gerador.LereGerarHtml(listaIngressos))
                        {
                            retorno.Retorno = new byte[ms.Length];
                            ms.Read(retorno.Retorno, 0, (int)ms.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Erro ao Gerar Eticket";
                LogUtil.Error(ex);
            }
            return retorno;
        }


        /// <summary>
        /// Método que monta o conteúdo do email
        /// </summary>
        /// <param name="ingresso"></param>
        /// <returns></returns>
        private EnviarMediaPartnerConfirmacaoConteudoEmailModel VenderVoucherConteudoEmail(Voucher voucher, Login login, List<Carrinho> carrinhoList, tEntregaControle entregaControleEscolhido, tVendaBilheteria vendaBilheteria)
        {

            EnviarMediaPartnerConfirmacaoConteudoEmailModel retorno = new EnviarMediaPartnerConfirmacaoConteudoEmailModel();

            retorno.nomeCliente = login.Cliente.Nome;
            retorno.compraData = vendaBilheteria.DataVendaAsDateTime;
            retorno.senha = vendaBilheteria.Senha;
            retorno.entrega = entregaControleEscolhido.Entrega.Nome;
            retorno.codigoVoucher = voucher.Codigo;
            retorno.parceiroMidia = voucher.ParceiroMidia.Nome;

            List<EnviarMediaPartnerConfirmacaoConteudoEmaiEventoModel> listaEvento = new List<EnviarMediaPartnerConfirmacaoConteudoEmaiEventoModel>();
            EnviarMediaPartnerConfirmacaoConteudoEmaiEventoModel EventoMail = new EnviarMediaPartnerConfirmacaoConteudoEmaiEventoModel();

            EventoBO eventoBO = new EventoBO(ado);

            foreach (int eventoID in carrinhoList.Select(t => t.EventoID).Distinct())
            {

                var evento = eventoBO.Consultar(eventoID);

                EventoMail.cidade = evento.Local.Cidade;
                EventoMail.endereco = evento.Local.Endereco;
                EventoMail.estado = evento.Local.Estado;
                EventoMail.evento = evento.Nome;
                EventoMail.EventoID = eventoID;
                EventoMail.local = evento.Local.Nome;

                List<EnviarMediaPartnerConfirmacaoConteudoEmaiLugarModel> ListaLugar = new List<EnviarMediaPartnerConfirmacaoConteudoEmaiLugarModel>();
                foreach (Carrinho item in carrinhoList.Where(t => t.EventoID == eventoID))
                {
                    EnviarMediaPartnerConfirmacaoConteudoEmaiLugarModel lugar = new EnviarMediaPartnerConfirmacaoConteudoEmaiLugarModel();

                    lugar.dataApresentacao = item.ApresentacaoDataHoraAsDateTime;
                    lugar.setor = item.Setor;
                    lugar.lugar = item.Codigo;

                    ListaLugar.Add(lugar);
                }
                EventoMail.listaEmailLugar = ListaLugar.ToArray();
                listaEvento.Add(EventoMail);
            }
            retorno.listaEmailEvento = listaEvento.ToArray();

            return retorno;
        }

        /// <summary>
        /// Lista as compras de um cliente
        /// </summary>
        /// <param name="login"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public IPagedList<tVendaBilheteria> ListarCliente(int pageNumber, int pageSize, Login login)
        {
            var listVendaBilheteira = ado.ListarCliente(pageNumber, pageSize, login.ClienteID);
            foreach (var listItem in listVendaBilheteira)
            {
                if (listItem.StatusAsEnum == enumVendaBilheteriaStatus.cancelado)
                    listItem.VendaBilheteriaOrigem = Consultar(Convert.ToInt32(listItem.VendaBilheteriaIDOrigem));
            }
            return listVendaBilheteira;

        }

        /// <summary>
        /// Retorna as compras do usuário logado.
        /// </summary>
        /// <param name="pageNumber">Página</param>
        /// <param name="pageSize">Quantidade de página</param>
        /// <param name="clienteID">Cliente ID</param>
        /// <param name="canalID">Canal ID</param>
        /// <returns>Lista de compras</returns>
        public IPagedList<MeuIngresso> GetMeusIngressos(int pageNumber, int pageSize, int clienteID, int canalID)
        {
            var meusIngressos = ado.GetMeusIngressos(pageNumber, pageSize, clienteID, canalID);

            //Add url eticket
            foreach (var compra in meusIngressos.Where(compra => compra.Entrega != null && compra.Entrega.PermitirImpressaoInternetAsBool))
            {
                var urlImpressao = ConfigurationManager.AppSettings["URLImpressao"];
                var senhaImpressao = QueryString.Current.Add(QueryString.ID, compra.ID.ToString()).Add(QueryString.LOGADO, "true").Encrypt(ConfigurationManager.AppSettings["ChaveCriptografiaLogin"]);

                compra.Entrega.ETicketURL = string.Format("{0}{1}", urlImpressao, senhaImpressao);
            }


            return meusIngressos;
        }

        /// <summary>
        /// Método que consulta uma venda bilheteria
        /// </summary>
        /// <param name="vendaBilheteria"></param>
        /// <returns></returns>
        public tVendaBilheteria Consultar(int vendaBilheteriaID)
        {
            return ado.Consultar(vendaBilheteriaID);
        }

        public tVendaBilheteria Consultar(string senha)
        {
            return ado.Consultar(senha);
        }

        public tVendaBilheteria ConsultarComIngressosResumido(int vendaBilheteriaID)
        {
            return ado.ConsultarComIngressosResumido(vendaBilheteriaID);
        }

        /// <summary>
        /// Carrega os valeIngressos de um venda bilheteria
        /// </summary>
        /// <param name="vendaFormasPagamento"></param>
        /// <returns></returns>
        private List<tVendaBilheteriaFormaPagamento> CarregarValeIngressos(List<tVendaBilheteriaFormaPagamento> vendaFormasPagamento)
        {
            if (vendaFormasPagamento != null)
            {
                ValeIngressoBO virBO = new ValeIngressoBO(ado);
                foreach (var item in vendaFormasPagamento)
                {
                    if (item.ValeIngressoID != null && item.ValeIngressoID.Value > 0)
                    {
                        tValeIngresso valeIngresso = virBO.Consultar(item.ValeIngressoID.Value);
                        if (valeIngresso != null)
                            item.tValeIngresso = valeIngresso;
                    }
                }
            }
            return vendaFormasPagamento;
        }

        #region Métodos do Checkout

        /// <summary>
        /// Método que finaliza a compra
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="pagamentos"></param>
        /// <param name="entradaFranca"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> FinalizarCompra(CompraModel compra, List<CompraPagamentoModel> pagamentos, bool entradaFranca)
        {
            LogUtil.Debug(string.Format("##VendaBilheteriaBO.FinalizandoCompra## SESSION {0}", compra.SessionID));

            try
            {
                var carrinhoBO = new CarrinhoBO(ado);

                //Calcula os valores do carrinho
                compra = carrinhoBO.CalcularValores(compra);

                if (pagamentos.Sum(x => x.Valor) != compra.Total.ValorTotal)
                {
                    return new RetornoModel<CompraModel> { Sucesso = false, Retorno = compra, Mensagem = "O valor total do carrinho é diferente da soma dos valores pagos pelos cartões" };
                }

                //Seta apenas o primeiro pagamento para usar por enquanto. Quando for fazer mais de um cartão necessário ajustar
                var pagamento = pagamentos.FirstOrDefault();

                Sitef.enumBandeira? bandeira = null;
                var VSBIN = !entradaFranca && !string.IsNullOrEmpty(pagamento.NumeroCartao) ? pagamento.NumeroCartao.Substring(0, 6) : string.Empty;
                var SessionBIN = VSBIN;

                #region AntiFraude - BlackList IP (Bloqueia a Venda caso o IP esteja na BlackList)

                LogUtil.Debug(string.Format("##VendaBilheteriaBO.FinalizandoCompra.VerificandoBlackList## SESSION {0}", compra.SessionID));
                if (IRLib.BlackList.BlackListIpIsBlock(pagamento.IP))
                {
                    new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "O IP informado está na Black List" };
                }
                #endregion

                //Instancia um carrinho do Proxy e depois carrega com o clienteID e Session ID
                var oCarrinhoLista = new CarrinhoLista();
                oCarrinhoLista.CarregarDadosPorClienteID(compra.ClienteID, compra.SessionID, CarrinhoLista.Status.Reservado, 0);

                //decimal taxaEntregaValor = 0; //TODO: Procurar quem criou essa variável, e entender porque ela não é instanciada
                var formaPagamentoId = 0;

                //Verifica se a compra é totalmente paga com os Vale-Ingressos
                var SomenteVirs = compra.Total.ValorTotal == 0;

                if (!SomenteVirs && !entradaFranca)
                {
                    if (pagamento.formaPagamento != null
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.VisaCredito
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.RedecardCredito
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.Amex
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.Diners
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.HSBCAVista
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.HSBCTransferencia
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.HSBCParcelado
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.Aura
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.Hipercard
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.ValeCultura
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.Elo
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.EloCultura
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.VisaElectron
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.ItauShopline
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.PayPal
                            && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.MilagemSmiles)
                    {
                        return new RetornoModel<CompraModel> { Sucesso = false, Retorno = compra, Mensagem = "Forma de pagamento não selecionada" };
                    }

                    if (pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.VisaElectron
                        && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.ItauShopline
                        && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.PayPal
                        && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.MilagemSmiles
                        && !VerificarBIN(VSBIN, pagamento.formaPagamento.NomeAsEnum))
                    {
                        return new RetornoModel<CompraModel> { Sucesso = false, Retorno = compra, Mensagem = "O cartão informado não é válido" };
                    }
                }

                if (compra.LimiteTotalItensCarrinho < compra.CarrinhoItens.Count() && !entradaFranca && (pagamento.formaPagamento == null || pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.VisaElectron))
                    return new RetornoModel<CompraModel> { Sucesso = false, Retorno = compra, Mensagem = "Limite excedido" };

                //verifica se esta compra esta sendo efetuada atraves do IB-HOM
                if (!entradaFranca && pagamento.ParceiroID != null && !SomenteVirs && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.ItauShopline)
                {
                    if (pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.HSBCTransferencia)
                    {
                        if (CheckBin(VSBIN, pagamento.formaPagamento.NomeAsEnum))
                        {
                            //Verifica se o BIN digitado eh HSBC
                            IRLib.Parceria oParceria = new IRLib.Parceria();
                            if (oParceria.ValidaBin(VSBIN.Replace("'", "")) != IRLib.Parceria.Parceiro.HSBC)
                            {
                                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "O cartão HSBC informado não é válido" };
                            }
                            SessionBIN = pagamento.NumeroCartao.Substring(0, 6);
                        }
                        else
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "O cartão informado não é válido" };
                    }
                    else
                        SessionBIN = string.Empty;
                }
                else
                {
                    if (!entradaFranca && !SomenteVirs && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.ItauShopline
                        && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.PayPal
                        && pagamento.formaPagamento.NomeAsEnum != enumFormaPagamento.MilagemSmiles)
                    {
                        if (!CheckBin(VSBIN, pagamento.formaPagamento.NomeAsEnum))
                            return new RetornoModel<CompraModel> { Sucesso = false, Retorno = compra, Mensagem = "O cartão informado não é válido" };
                    }
                    if (!entradaFranca && pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.ItauShopline
                            && pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.PayPal
                            && pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.MilagemSmiles)
                    {
                        SessionBIN = string.Empty;
                    }
                }

                IRLib.Ingresso.AumentarTempoReservasInternet(compra.ClienteID, compra.SessionID);

                if (!entradaFranca && !SomenteVirs)
                {
                    switch (pagamento.formaPagamento.NomeAsEnum)
                    {
                        case enumFormaPagamento.VisaCredito:
                            bandeira = IRLib.Sitef.enumBandeira.Visa;
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("VisaCredito", pagamento.formaPagamento.Parcelas);
                            break;
                        case enumFormaPagamento.RedecardCredito:
                            bandeira = IRLib.Sitef.enumBandeira.Master;
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("RedecardCredito", pagamento.formaPagamento.Parcelas);
                            break;
                        case enumFormaPagamento.Diners:
                            bandeira = IRLib.Sitef.enumBandeira.Diners;
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("RedecardCredito", pagamento.formaPagamento.Parcelas);
                            break;
                        case enumFormaPagamento.VisaElectron:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("VisaElectron", 1);
                            bandeira = IRLib.Sitef.enumBandeira.VisaElectron;
                            break;
                        case enumFormaPagamento.Amex:
                            bandeira = IRLib.Sitef.enumBandeira.Amex;
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("Amex", pagamento.formaPagamento.Parcelas);
                            break;
                        case enumFormaPagamento.HSBCAVista:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("HSBCCredito", 1);
                            break;
                        case enumFormaPagamento.HSBCParcelado:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("HSBCCredito", 2);
                            break;
                        case enumFormaPagamento.HSBCTransferencia:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("HSBCDebito", 1);
                            break;
                        case enumFormaPagamento.ItauShopline:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("ItauDebito", 1);
                            bandeira = IRLib.Sitef.enumBandeira.Itau;
                            break;
                        case enumFormaPagamento.Aura:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("Aura", pagamento.formaPagamento.Parcelas);
                            bandeira = IRLib.Sitef.enumBandeira.Aura;
                            break;
                        case enumFormaPagamento.Hipercard:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("HiperCard", pagamento.formaPagamento.Parcelas);
                            bandeira = IRLib.Sitef.enumBandeira.Hipercard;
                            break;
                        case enumFormaPagamento.PayPal:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("PayPal", 1);
                            bandeira = IRLib.Sitef.enumBandeira.PayPal;
                            break;
                        case enumFormaPagamento.ValeCultura:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("ValeCultura", pagamento.formaPagamento.Parcelas);
                            bandeira = IRLib.Sitef.enumBandeira.ValeCultura;
                            break;
                        case enumFormaPagamento.Elo:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("Elo", pagamento.formaPagamento.Parcelas);
                            bandeira = IRLib.Sitef.enumBandeira.Elo;
                            break;
                        case enumFormaPagamento.EloCultura:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("EloCultura", pagamento.formaPagamento.Parcelas);
                            bandeira = IRLib.Sitef.enumBandeira.EloCultura;
                            break;
                        case enumFormaPagamento.MilagemSmiles:
                            formaPagamentoId = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("MilagemSmiles", 1);
                            bandeira = IRLib.Sitef.enumBandeira.MilagemSmiles;
                            break;
                        default:
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Forma pagamento não cadastrada" };
                    }

                    if (!BinCartaoValido(VSBIN, pagamento.formaPagamento.NomeAsEnum))
                    {
                        return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Os digitos do cartão não correspondem a bandeira informada." };
                    }

                }

                if (entradaFranca)
                {
                    var formaPagamentoBO = new FormaPagamentoBO(ado);
                    var idFormaPagamentoCortesia = new IRLib.ConfigGerenciador().getFormaPagamentoIDCortesia();
                    var formaPagamentoCortesia = formaPagamentoBO.Consultar(idFormaPagamentoCortesia);

                    pagamento.formaPagamento = formaPagamentoCortesia;
                }
                else
                {
                    //Seta o id na forma de pagamento do cartão
                    pagamento.formaPagamento.IR_FormaPagamentoID = formaPagamentoId;
                }

                string[] msgCota = new string[2];

                if (compra.CarrinhoItens.Any(a => a.CotaItemObject != null))
                {
                    int BIN = 0;
                    if (string.IsNullOrWhiteSpace(SessionBIN))
                        BIN = 0;
                    else
                        BIN = Convert.ToInt32(SessionBIN);

                    oCarrinhoLista.Clear();
                    oCarrinhoLista.CarregarDadosPorClienteID(compra.ClienteID, compra.SessionID, IngressoRapido.Lib.CarrinhoLista.Status.Reservado, 0);
                    msgCota = oCarrinhoLista.ValidarCotas(compra.ClienteID, BIN, formaPagamentoId, SomenteVirs, SomenteVirs);
                    IngressoRapido.Lib.Carrinho oCarrinho = new IngressoRapido.Lib.Carrinho();
                    switch (msgCota[0])
                    {
                        case "1": //Erro Validar Bin
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Cartão não permitido para a compra. Esta apresentação possui restrição de compra por bandeira ou instituição do cartão de crédito, verifique no regulamento qual é a regra aplicada." };

                        case "2": //Erro Forma Pagamento
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "A forma de pagamento selecionada não está disponível. Escolha outra forma de pagamento e tente novamente" };

                        case "3": //Excedeu Limite Cota Setor
                            oCarrinho.ExpirarTodasReservas(compra.ClienteID, compra.SessionID);
                            compra.CarrinhoItens = new List<Carrinho>();
                            compra.Total = new CompraTotalModel();
                            compra.EntregaControleID = 0;
                            compra.EntregaControles = new List<tEntregaControle>();
                            compra.PDVID = 0;
                            compra.ValeIngressos = new List<tValeIngresso>();
                            compra.VendaBilheteria = new tVendaBilheteria();
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "O limite de venda para o preço escolhido foi excedido, por favor, selecione outro preço" };

                        case "4": //Excedeu Limite Cota Apresentacao
                            oCarrinho.ExpirarTodasReservas(compra.ClienteID, compra.SessionID);
                            compra.CarrinhoItens = new List<Carrinho>();
                            compra.Total = new CompraTotalModel();
                            compra.EntregaControleID = 0;
                            compra.EntregaControles = new List<tEntregaControle>();
                            compra.PDVID = 0;
                            compra.ValeIngressos = new List<tValeIngresso>();
                            compra.VendaBilheteria = new tVendaBilheteria();
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "O limite de venda para o preço escolhido foi excedido, por favor, selecione outro preço" };
                        default:
                            break;
                    }
                }

                var msgErroVenda = new string[2];

                //Parte Final - Tipos de Pagamento
                RetornoModel<CompraModel> retorno = null;
                try
                {
                    if (entradaFranca)
                    {
                        #region Entrada Gratuíta

                        IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

                        oCompraTemporaria.ClienteID.Valor = compra.ClienteID;
                        oCompraTemporaria.SessionID.Valor = compra.SessionID;

                        IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = new IRLib.ConfigGerenciador().getFormaPagamentoIDCortesia();
                        oCompraTemporaria.EntregaValor.Valor = 0;
                        oCompraTemporaria.Parcelas.Valor = 0;
                        oCompraTemporaria.ValorTotal.Valor = 0;
                        oCompraTemporaria.Bandeira.Valor = "99";
                        oCompraTemporaria.BIN.Valor = 0;
                        oCompraTemporaria.EntregaControleIDSelecionado.Valor = compra.EntregaControleID;
                        oCompraTemporaria.EnderecoID.Valor = 0;
                        oCompraTemporaria.SomenteCortesias.Valor = true;
                        oCompraTemporaria.SomenteVir.Valor = false;

                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoTEFCortesia(compra, pagamento, oCompraTemporaria);

                        #endregion
                    }
                    else if (SomenteVirs)
                    {
                        #region Somente VIR

                        IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

                        oCompraTemporaria.ClienteID.Valor = compra.ClienteID;
                        oCompraTemporaria.SessionID.Valor = compra.SessionID;

                        IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = new IRLib.ConfigGerenciador().getFormaPagamentoIDValeIngresso();
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;
                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = "99";
                        oCompraTemporaria.BIN.Valor = 0;
                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;
                        oCompraTemporaria.SomenteVir.Valor = true;
                        oCompraTemporaria.SomenteCortesias.Valor = false;

                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoVIR(compra, oCompraTemporaria);
                        #endregion
                    }
                    else if (pagamento.ParceiroID != null)
                    {
                        #region HSBC

                        IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

                        oCompraTemporaria.ClienteID.Valor = compra.ClienteID;
                        oCompraTemporaria.SessionID.Valor = compra.SessionID;

                        IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = formaPagamentoId;
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;

                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = Convert.ToString((int)bandeira);
                        oCompraTemporaria.BIN.Valor = 000000;
                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;
                        oCompraTemporaria.SomenteCortesias.Valor = false;
                        oCompraTemporaria.SomenteVir.Valor = false;
                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoTEF(compra, pagamento, oCompraTemporaria);

                        #endregion
                    }
                    else if (pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.ItauShopline)
                    {
                        #region Itau

                        IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

                        oCompraTemporaria.ClienteID.Valor = compra.ClienteID;
                        oCompraTemporaria.SessionID.Valor = compra.SessionID;

                        IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = formaPagamentoId;
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;
                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = Convert.ToString((int)bandeira);
                        oCompraTemporaria.BIN.Valor = 000000;

                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;

                        oCompraTemporaria.SomenteCortesias.Valor = false;
                        oCompraTemporaria.SomenteVir.Valor = false;

                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoTEF(compra, pagamento, oCompraTemporaria);

                        #endregion
                    }
                    else if (pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.VisaElectron)
                    {
                        #region VisaElectron

                        IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

                        oCompraTemporaria.ClienteID.Valor = compra.ClienteID;
                        oCompraTemporaria.SessionID.Valor = compra.SessionID;

                        IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = formaPagamentoId;
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;
                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = Convert.ToString((int)bandeira);
                        oCompraTemporaria.BIN.Valor = 000000;
                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;

                        oCompraTemporaria.SomenteCortesias.Valor = false;
                        oCompraTemporaria.SomenteVir.Valor = false;
                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoTEF(compra, pagamento, oCompraTemporaria);

                        #endregion
                    }
                    else if (pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.PayPal)
                    {
                        #region PayPal

                        var oCompraTemporaria = new CompraTemporaria
                        {
                            ClienteID = { Valor = compra.ClienteID },
                            SessionID = { Valor = compra.SessionID }
                        };

                        var oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = formaPagamentoId;
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;
                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = Convert.ToString((int)bandeira);
                        oCompraTemporaria.EntregaControleIDSelecionado.Valor = compra.EntregaControleID;
                        oCompraTemporaria.EnderecoID.Valor = compra.ClienteEnderecoID;
                        oCompraTemporaria.PDVSelecionado.Valor = compra.PDVID;
                        oCompraTemporaria.DataSelecionada.Valor = null;
                        oCompraTemporaria.BIN.Valor = 000000;

                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;

                        oCompraTemporaria.SomenteCortesias.Valor = false;
                        oCompraTemporaria.SomenteVir.Valor = false;

                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoPayPal(compra, pagamento);

                        #endregion
                    }
                    else if (pagamento.formaPagamento.NomeAsEnum == enumFormaPagamento.MilagemSmiles)
                    {
                        #region SMILES

                        IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

                        oCompraTemporaria.ClienteID.Valor = compra.ClienteID;
                        oCompraTemporaria.SessionID.Valor = compra.SessionID;

                        IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();


                        oCompraTemporaria.FormaPagamentoID.Valor = formaPagamentoId;
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;
                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = Convert.ToString((int)bandeira);
                        oCompraTemporaria.BIN.Valor = 000000;

                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;

                        oCompraTemporaria.SomenteCortesias.Valor = false;
                        oCompraTemporaria.SomenteVir.Valor = false;

                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        #endregion
                    }
                    else
                    {
                        #region TEF

                        var oCompraTemporaria = new CompraTemporaria
                        {
                            ClienteID = { Valor = compra.ClienteID },
                            SessionID = { Valor = compra.SessionID }
                        };

                        var oEstrutura = oCompraTemporaria.ConsultarSeExiste();

                        oCompraTemporaria.FormaPagamentoID.Valor = formaPagamentoId;
                        oCompraTemporaria.EntregaValor.Valor = compra.Total.ValorTaxaEntrega;
                        oCompraTemporaria.Parcelas.Valor = pagamento.formaPagamento.Parcelas;
                        oCompraTemporaria.ValorTotal.Valor = compra.Total.ValorTotal;
                        oCompraTemporaria.Bandeira.Valor = Convert.ToString((int)bandeira);
                        oCompraTemporaria.BIN.Valor = Convert.ToInt32(pagamento.NumeroCartao.Substring(0, 6));
                        oCompraTemporaria.EntregaControleIDSelecionado.Valor = compra.EntregaControleID;
                        oCompraTemporaria.EnderecoID.Valor = compra.ClienteEnderecoID;
                        oCompraTemporaria.PDVSelecionado.Valor = compra.PDVID;
                        oCompraTemporaria.DataSelecionada.Valor = null;

                        if (!string.IsNullOrWhiteSpace(pagamento.CodigoTrocaFixo))
                            oCompraTemporaria.CodigoTrocaFixo.Valor = pagamento.CodigoTrocaFixo;

                        oCompraTemporaria.SomenteCortesias.Valor = false;
                        oCompraTemporaria.SomenteVir.Valor = false;

                        if (oEstrutura.Encontrado)
                        {
                            oCompraTemporaria.Control.ID = oEstrutura.ID;
                            oCompraTemporaria.AtualizaFormaPagamento();
                        }
                        else
                            oCompraTemporaria.Inserir();

                        retorno = EfetuarPagamentoTEF(compra, pagamento, oCompraTemporaria);
                        #endregion
                    }

                    return retorno ?? new RetornoModel<CompraModel>() { Sucesso = true, Retorno = compra, Mensagem = "OK" };
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##VendaBilheteriaBO.FinalizarCompra.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message), ex);
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = null, Mensagem = ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##VendaBilheteriaBO.FinalizarCompra.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message), ex);
                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = null, Mensagem = ex.Message };
            }
        }

        /// <summary>
        /// Método que finaliza a compra externa
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="pagamentos"></param>
        /// <param name="entradaFranca"></param>
        /// <returns></returns>
        public RetornoModel<CompraExternaModel> FinalizarCompraExterna(CompraModel compra, List<CompraPagamentoExternoModel> pagamentos, bool entradaFranca)
        {
            LogUtil.Info(string.Format("##FinalizarCompraExterna## PGTO {0}, VALOR {1}, COD_AUT {2}, NSU {3}", pagamentos[0].formaPagamento == null ? "NA" : pagamentos[0].formaPagamento.Nome ?? "NA", pagamentos[0].valor, pagamentos[0].codigoAutorizacao ?? "NA", pagamentos[0].NSU ?? "NA"));

            try
            {
                if (pagamentos.Sum(x => x.valor) != compra.Total.ValorTotal)
                {
                    LogUtil.Info(string.Format("##FinalizarCompraExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, "O valor total do carrinho é diferente da soma dos valores pagos pelos cartões"));
                    return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = "O valor total do carrinho é diferente da soma dos valores pagos pelos cartões" };
                }

                //Seta apenas o primeiro pagamento para usar por enquanto. Quando for fazer mais de um cartão necessário ajustar
                var pagamento = pagamentos.FirstOrDefault();

                //Instancia um carrinho do Proxy e depois carrega com o clienteID e Session ID
                var oCarrinhoLista = new CarrinhoLista();
                oCarrinhoLista.CarregarDadosPorClienteID(compra.ClienteID, compra.SessionID, CarrinhoLista.Status.Reservado, 0);

                if (compra.LimiteTotalItensCarrinho <= compra.CarrinhoItens.Count() && pagamento.formaPagamento == null)
                {
                    LogUtil.Info(string.Format("##FinalizarCompraExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, "Limite de itens no carrinho excedido."));
                    return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = "Limite excedido." };
                }

                IRLib.Ingresso.AumentarTempoReservasInternet(compra.ClienteID, compra.SessionID);

                int formaPagamentoID;
                switch (pagamento.formaPagamento.NomeAsEnum)
                {
                    case enumFormaPagamento.VisaCredito:
                        formaPagamentoID = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("VisaCredito", pagamento.formaPagamento.Parcelas);
                        break;
                    case enumFormaPagamento.RedecardCredito:
                        formaPagamentoID = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("RedecardCredito", pagamento.formaPagamento.Parcelas);
                        break;
                    case enumFormaPagamento.VisaElectron:
                        formaPagamentoID = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("VisaElectron", 1);
                        break;
                    case enumFormaPagamento.Redeshop:
                        formaPagamentoID = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("Redeshop", 1);
                        break;
                    case enumFormaPagamento.Dinheiro:
                        formaPagamentoID = IngressoRapido.Lib.FormaPagamento.GetFormaPagamentoID("Dinheiro", 1);
                        break;
                    default:
                        LogUtil.Error(string.Format("##FinalizarCompraExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, "Forma de pagamento escolhida não cadastrada."));
                        return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = "Forma de pagamento não cadastrada." };
                }

                if (entradaFranca)
                {
                    var formaPagamentoBO = new FormaPagamentoBO(ado);
                    var idFormaPagamentoCortesia = new ConfigGerenciador().getFormaPagamentoIDCortesia();
                    var formaPagamentoCortesia = formaPagamentoBO.Consultar(idFormaPagamentoCortesia);

                    pagamento.formaPagamento = formaPagamentoCortesia;
                }
                else
                {
                    //Seta o id na forma de pagamento do cartão
                    pagamento.formaPagamento.IR_FormaPagamentoID = formaPagamentoID;
                }

                // Verificação da existência de cota.
                if (compra.CarrinhoItens[0].CotaItemObject != null)
                {
                    LogUtil.Error(string.Format("##FinalizarCompraExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, "Venda por cota desabilitada."));
                    return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = "Venda por cota desabilitada." };
                }

                //Parte Final - Tipos de pagamento e geração do código de barras.
                try
                {
                    var retorno = EfetuarPagamentoExternoTEF(compra, pagamento);

                    if (retorno.Retorno == null)
                    {
                        LogUtil.Error(string.Format("##FinalizarCompraExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, retorno.Mensagem));
                        return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = "A venda não foi finalizada." };
                    }

                    int lojaID;
                    int usuarioID;
                    int canalID;

                    if (compra.EstruturaVenda.CanalID == 0 || compra.EstruturaVenda.LojaID == 0 || compra.EstruturaVenda.UsuarioID == 0)
                    {
                        lojaID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.POSLojaID));
                        usuarioID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.POSUsuarioID));
                        canalID = Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.CanalPOS));
                    }
                    else
                    {
                        lojaID = compra.EstruturaVenda.LojaID;
                        usuarioID = compra.EstruturaVenda.UsuarioID;
                        canalID = compra.EstruturaVenda.CanalID;
                    }

                    try
                    {
                        var ingressoBO = new IngressoBO(ado);
                        ingressoBO.AtualizarCodigoBarraIngressos(compra.CarrinhoItens, usuarioID, Convert.ToInt32(retorno.Retorno[3]), canalID, lojaID);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error("Geração de código de barra falhou. Fazendo segunda tentativa.\n\r" + ex.Message, "FinalizarCompra");
                        System.Threading.Thread.Sleep(1000);
                        try
                        {
                            var ingressoBO = new IngressoBO(ado);
                            ingressoBO.AtualizarCodigoBarraIngressos(compra.CarrinhoItens, usuarioID, Convert.ToInt32(retorno.Retorno[3]), canalID, lojaID);
                        }
                        catch (Exception exception)
                        {
                            LogUtil.Error(string.Format("Geração de código de barra falhou na segunda tentativa.\n\r {0}", exception.Message), "FinalizarCompra");
                        }
                    }

                    // Criação de model para compra externa, de modo que serão utilizadas muito menos propriedades do que o modelo de compra padrão.
                    var retornoSimplificado = new CompraExternaModel();

                    // Laço que preenche os itens do carrinho.
                    foreach (var item in compra.CarrinhoItens)
                    {
                        var carrinho = new CarrinhoExterno
                        {
                            CodigoBarra = item.CodigoBarra
                        };
                        retornoSimplificado.Carrinho.Add(carrinho);
                    }

                    retornoSimplificado.SenhaVenda = retorno.Retorno[1];

                    LogUtil.Info(string.Format("##FinalizarCompraExterna.SUCCESS## SESSION {0}, PGTO {1}, VALOR {2}, COD_AUT {3}, NSU {4}", compra.SessionID, pagamentos[0].formaPagamento == null ? "NA" : pagamentos[0].formaPagamento.Nome ?? "NA", pagamentos[0].valor, pagamentos[0].codigoAutorizacao ?? "NA", pagamentos[0].NSU ?? "NA"));
                    return new RetornoModel<CompraExternaModel>() { Sucesso = true, Retorno = retornoSimplificado, Mensagem = "OK" };
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##FinalizarCompraExterna.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message));
                    return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##FinalizarCompraExterna.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message));
                return new RetornoModel<CompraExternaModel>() { Sucesso = false, Retorno = null, Mensagem = ex.Message };
            }
        }

        /// <summary>
        /// Método que efetua a compra com PayPal
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> EfetuarPagamentoPayPal(CompraModel compra, CompraPagamentoModel pagamento)
        {
            LogUtil.Debug(string.Format("##VendaBilheteriaBO.EfetuarPagamentoPayPal## SESSION {0}", compra.SessionID));

            try
            {
                var clienteID = compra.ClienteID;
                var sessionID = compra.SessionID;

                var oCompraTemporaria = new CompraTemporaria
                {
                    ClienteID = { Valor = clienteID },
                    SessionID = { Valor = sessionID }
                };

                var compraTemporaria = oCompraTemporaria.RetornarCompraPorClienteSessionID();

                if (compraTemporaria.Encontrado == false)
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Compra não encontrada" };
                }

                var detalhes = PayPalApiFactory.instance.ExpressCheckout().GetExpressCheckoutDetails(pagamento.PayPal.Token).execute();

                var celular = string.Empty;

                if (!string.IsNullOrEmpty(compra.Login.Cliente.DDDCelular) && !string.IsNullOrEmpty(compra.Login.Cliente.Celular))
                    celular = compra.Login.Cliente.DDDCelular + compra.Login.Cliente.Celular;

                var estruturaPagamento = EstruturaPagamento.Montar(EstruturaPagamento.enumTipoPagamento.Paypal, compraTemporaria, 0, string.Empty, string.Empty, string.Empty, string.Empty, 0, false, detalhes.ResponseNVP.PayerID, detalhes.Token, HttpContext.Current.Request.UserHostAddress, 0, 0);

                var valorTotalSeguro = compra.ValorTotalSeguro;

                var carrinhoLista = new CarrinhoLista();

                carrinhoLista.CarregarDadosPorClienteID(clienteID, sessionID, CarrinhoLista.Status.Reservado, 0);

                try
                {
                    return EfetuaVendaSistema(estruturaPagamento, new List<int>(), celular, valorTotalSeguro, carrinhoLista, compraTemporaria, oCompraTemporaria, compra, new CompraPagamentoModel());

                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Erro dentro da venda:" + ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Erro dentro da venda:" + ex.Message };
            }
        }

        /// <summary>
        /// Método que realiza o pagamento com TEF
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="pagamento"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> EfetuarPagamentoTEF(CompraModel compra, CompraPagamentoModel pagamento, IRLib.CompraTemporaria oCompraTemporaria)
        {
            LogUtil.Debug(string.Format("##VendaBilheteriaBO.EfetuarPagamentoTEF## SESSION {0}", compra.SessionID));

            try
            {
                var codigoBIN = pagamento.NumeroCartao.Substring(0, 6);
                var numeroCartao = pagamento.NumeroCartao.Substring(6);
                var formaPagamentoId = pagamento.formaPagamento.IR_FormaPagamentoID;
                var validarData = formaPagamentoId != 160;

                if (!string.IsNullOrWhiteSpace(pagamento.NomeCartao) && (pagamento.NomeCartao.Trim().Length < 6 || pagamento.NomeCartao.Trim().IndexOf(' ') < 0))
                {
                    new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Nome do cartão inválido" };
                }
                if (codigoBIN.Length != 6 || codigoBIN.Contains("'"))
                {
                    new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Código BIN inválido" };
                }
                if (numeroCartao.Length == 0 || numeroCartao.Contains("'"))
                {
                    new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Número do cartão Inválido" };
                }
                if (validarData)
                {
                    if (pagamento.MesValidade > 0 && pagamento.MesValidade < 13)
                    {
                        DateTime hoje = DateTime.Today;
                        DateTime dataValidar = new DateTime(pagamento.AnoValidade + 2000, pagamento.MesValidade, 1);

                        if (dataValidar.AddMonths(1) < hoje)
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Mês ou ano de validade inválidos" };
                    }
                    else
                    {
                        return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Mês ou ano de validade inválidos" };
                    }
                }
                if (!IRLib.Utilitario.IsCartaoValido(pagamento.NumeroCartao.Trim()))
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Número do cartão inválido" };
                }

                var clienteId = compra.ClienteID;
                var sessionId = compra.SessionID;

                oCompraTemporaria = new CompraTemporaria
                {
                    ClienteID = { Valor = clienteId },
                    SessionID = { Valor = sessionId }
                };
                var oRetorno = oCompraTemporaria.RetornarCompraPorClienteSessionID();

                if (oRetorno.Encontrado == false)
                {
                    return new RetornoModel<CompraModel> { Sucesso = false, Retorno = compra, Mensagem = "Compra não encontrada" };
                }

                var valorTotalVIR = compra.Total.ValorDesconto;
                var VirIDs = new List<int>();

                if (compra.ValeIngressos != null)
                {
                    VirIDs = compra.ValeIngressos.Select(x => x.ID).ToList();

                    IRLib.ValeIngresso oValeIngresso = new IRLib.ValeIngresso();

                    decimal valorEntrega = compra.Total.ValorTaxaEntrega;
                    decimal valorTotalConveniencia = compra.Total.ValorTaxaConveniencia;
                    decimal valorTotalIngressos = compra.Total.ValorIngressos;

                    List<IRLib.ClientObjects.EstruturaTrocaValeIngresso> virs = new List<IRLib.ClientObjects.EstruturaTrocaValeIngresso>();

                    if (oRetorno.CodigoTrocaFixo.Length > 0)
                    {
                        IRLib.ClientObjects.EstruturaTrocaValeIngresso oTrocaFixa = new IRLib.ClientObjects.EstruturaTrocaValeIngresso();

                        oTrocaFixa = oValeIngresso.ValorIDVIR(oRetorno.CodigoTrocaFixo);
                        if (!oTrocaFixa.Encontrado)
                            return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "A Quantidade de um dos Vale Ingressos foi excedida." };

                        virs = new List<IRLib.ClientObjects.EstruturaTrocaValeIngresso>() { oTrocaFixa };
                    }
                    else
                        virs = oValeIngresso.ValidarTrocaValeIngresso(VirIDs);

                    valorTotalVIR = compra.Total.ValorDesconto;
                }

                var celular = string.Empty;

                if (!string.IsNullOrEmpty(compra.Login.Cliente.DDDCelular) && !string.IsNullOrEmpty(compra.Login.Cliente.Celular))
                    celular = compra.Login.Cliente.DDDCelular + compra.Login.Cliente.Celular;

                var cartaoIdRetorno = new Cartao().ValidaCartao(pagamento.NumeroCartao.Trim(), clienteId);

                var cartaoOutraPessoa = cartaoIdRetorno[1] == ((char)Cartao.enumStatusCartao.Bloqueado).ToString();

                var cartaoId = Convert.ToInt32(cartaoIdRetorno[0]);

                EstruturaPagamento.enumTipoPagamento ambiente;

                if (Convert.ToInt32(oRetorno.Bandeira) == Convert.ToInt32(Sitef.enumBandeira.ValeCultura) || Convert.ToInt32(oRetorno.Bandeira) == Convert.ToInt32(Sitef.enumBandeira.EloCultura))
                {
                    ambiente = EstruturaPagamento.enumTipoPagamento.TEFDebito;
                }
                else if (ConfiguracaoAdyen.Instancia.Chaves.Ativo.Valor)
                {
                    ambiente = EstruturaPagamento.enumTipoPagamento.Adyen;
                }
                else
                {
                    ambiente = EstruturaPagamento.enumTipoPagamento.TEF;
                }

                string IPCliente = pagamento.IP;

                string dataValidade = string.Format("{0:00}{1}", pagamento.MesValidade, pagamento.AnoValidade);

                string nomeCartao = !string.IsNullOrWhiteSpace(pagamento.NomeCartao) ? pagamento.NomeCartao.Trim().ToUpper() : string.Empty;

                IRLib.ClientObjects.EstruturaPagamento oPagamento = IRLib.ClientObjects.EstruturaPagamento.Montar(ambiente, oRetorno, valorTotalVIR, pagamento.NumeroCartao.Trim(), dataValidade, pagamento.CodigoSeguranca, nomeCartao, cartaoId, cartaoOutraPessoa, string.Empty, string.Empty, IPCliente, 0, 0);

                decimal ValorTotalSeguro = compra.ValorTotalSeguro;

                IngressoRapido.Lib.CarrinhoLista oCarrinhoLista = new IngressoRapido.Lib.CarrinhoLista();

                oCarrinhoLista.CarregarDadosPorClienteID(clienteId, sessionId, IngressoRapido.Lib.CarrinhoLista.Status.Reservado, 0);

                try
                {
                    return EfetuaVendaSistema(oPagamento, VirIDs, celular, ValorTotalSeguro, oCarrinhoLista, oRetorno, oCompraTemporaria, compra, pagamento);

                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Erro dentro da venda:" + ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Erro dentro da venda:" + ex.Message };
            }
        }

        /// <summary>
        /// Método que realiza o pagamento com TEF
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="pagamento"></param>
        /// <returns></returns>
        private RetornoModel<string[]> EfetuarPagamentoExternoTEF(CompraModel compra, CompraPagamentoExternoModel pagamento)
        {
            try
            {
                try
                {
                    return EfetuaVendaExterna(compra, pagamento);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##EfetuarPagamentoExternoTEF.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message));
                    return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = "Erro dentro da venda:" + ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##EfetuarPagamentoExternoTEF.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message));
                return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = "Erro dentro da venda:" + ex.Message };
            }
        }

        /// <summary>
        /// Método que realiza o pagamento de cortesia
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="pagamento"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> EfetuarPagamentoTEFCortesia(CompraModel compra, CompraPagamentoModel pagamento, IRLib.CompraTemporaria oCompraTemporaria)
        {
            try
            {
                int formaPagamentoID = pagamento.formaPagamento.IR_FormaPagamentoID;

                int ClienteID = compra.ClienteID;
                string SessionID = compra.SessionID;

                IRLib.ClientObjects.EstruturaCompraTemporaria oRetorno = oCompraTemporaria.RetornarCompraPorClienteSessionID();

                if (oRetorno.Encontrado == false)
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Compra não encontrada" };
                }

                Carrinho oCarrinho = new Carrinho();
                oCarrinho.SessionID = SessionID;
                oCarrinho.ClienteID = ClienteID;

                decimal valor = compra.Total.ValorTotal;

                string celular = string.Empty;

                if (!string.IsNullOrEmpty(compra.Login.Cliente.DDDCelular) && !string.IsNullOrEmpty(compra.Login.Cliente.Celular))
                    celular = compra.Login.Cliente.DDDCelular + compra.Login.Cliente.Celular;

                string[] msgErro = new string[4];

                bool CartaoOutraPessoa = false;

                IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento ambiente = IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Nenhum;

                string IPCliente = pagamento.IP;

                IRLib.ClientObjects.EstruturaPagamento oPagamento = IRLib.ClientObjects.EstruturaPagamento.Montar(ambiente, oRetorno, 0, null, null, null, null, 0, false, string.Empty, string.Empty, IPCliente, 0, 0);

                IngressoRapido.Lib.CarrinhoLista oCarrinhoLista = new IngressoRapido.Lib.CarrinhoLista();

                oCarrinhoLista.CarregarDadosPorClienteID(ClienteID, SessionID, IngressoRapido.Lib.CarrinhoLista.Status.Reservado, 0);

                try
                {
                    return EfetuaVendaSistemaCortesia(oPagamento, celular, oCarrinhoLista, oRetorno, oCompraTemporaria, compra, pagamento);

                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Erro dentro da venda:" + ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = "Erro dentro da venda:" + ex.Message };
            }
        }

        /// <summary>
        /// Método que realiza o pagamento de cortesia
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="pagamento"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <returns></returns>
        private RetornoModel<string[]> EfetuarPagamentoTEFCortesiaExterno(CompraModel compra, CompraPagamentoExternoModel pagamento)
        {
            try
            {
                try
                {
                    return EfetuaVendaSistemaCortesiaExterno(compra, pagamento);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = "Erro dentro da venda:" + ex.Message };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = "Erro dentro da venda:" + ex.Message };
            }
        }

        /// <summary>
        /// Método que executa o pagamento com Vale Ingresso
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> EfetuarPagamentoVIR(CompraModel compra, IRLib.CompraTemporaria oCompraTemporaria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método verifica se o bin é correspondente a bandeira informada
        /// </summary>
        /// <param name="codigoBIN"></param>
        /// <param name="formaPagamento"></param>
        /// <returns></returns>
        public static bool BinCartaoValido(string codigoBIN, enumFormaPagamento formaPagamento)
        {
            try
            {
                switch (formaPagamento)
                {
                    case enumFormaPagamento.Amex:
                        return Regex.IsMatch(codigoBIN, "^(34|37)");

                    case enumFormaPagamento.Aura:
                        return Regex.IsMatch(codigoBIN, "^(50)");

                    case enumFormaPagamento.Diners:
                        return Regex.IsMatch(codigoBIN, "^(300|301|302|303|304|305|36|38|2014|2149)");

                    case enumFormaPagamento.Elo:
                        return Regex.IsMatch(codigoBIN, "^(401178|401179|431274|438935|451416|457393|457631|457632|504175|627780|636297|636368|506699|5067[0-7][0-8]|509[0-9][0-9][0-9]|65003[1-3]|6500[3-4][0-5]|65005[0-1]|65040[5-9]|6504[1-3][0-9]|65048[5-9]|65049[0-9]|6505[1-2][0-9]|65053[0-8]|6505[4-8][1-9]|65059[0-8]|65070[0-9]|65071[0-8]|65072[0-7]|6509[0-1][0-9]|650920|6516[5-6][2-9]|65167[0-9]|6550[0-1][0-9]|6550[2-4][1-9]|65505[0-8])");

                    case enumFormaPagamento.EloCultura:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.Hipercard:
                        return Regex.IsMatch(codigoBIN, "^(38|60)");

                    case enumFormaPagamento.ItauShopline:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.RedecardCredito:
                        return Regex.IsMatch(codigoBIN, "^(51|52|53|54|55)");

                    case enumFormaPagamento.MilagemSmiles:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.PayPal:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.ValeCultura:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.VisaCredito:
                        return Regex.IsMatch(codigoBIN, "^(4)");

                    case enumFormaPagamento.VisaElectron:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.HSBCAVista:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.HSBCParcelado:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.HSBCTransferencia:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.Cortesia:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.VisaElectron_DESATIVADO:
                        return true; //Sem identificação do cartão

                    case enumFormaPagamento.Nenhuma:
                        return true; //Sem identificação do cartão
                }

                return false;
            }
            catch (Exception)
            {
                return true; //Mesmo com erro ainda faz o usuário realizar o pagamento.
            }
        }

        /// <summary>
        /// Método que checa a validade de um BIN
        /// </summary>
        /// <param name="codigoBIN"></param>
        /// <param name="formaPagamento"></param>
        /// <returns></returns>
        private bool CheckBin(string codigoBIN, enumFormaPagamento formaPagamento)
        {
            try
            {
                if (formaPagamento == enumFormaPagamento.VisaElectron || formaPagamento == enumFormaPagamento.HSBCParcelado
                        || formaPagamento == enumFormaPagamento.ItauShopline)
                    return true;

                if (codigoBIN.Contains("'"))
                {
                    return false;
                }


                if (codigoBIN.Length != 6)
                {
                    return false;
                }
                else
                {
                    foreach (Char c in codigoBIN)
                        if (!Char.IsNumber(c))
                            return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que verifica um bin
        /// </summary>
        /// <param name="codigoBIN"></param>
        /// <param name="formaPagamento"></param>
        /// <returns></returns>
        protected bool VerificarBIN(string codigoBIN, enumFormaPagamento formaPagamento)
        {
            try
            {

                if (!CheckBin(codigoBIN, formaPagamento))
                {
                    return false;
                }

                if ((formaPagamento == enumFormaPagamento.HSBCAVista || formaPagamento == enumFormaPagamento.HSBCParcelado
                        || formaPagamento == enumFormaPagamento.HSBCTransferencia) && formaPagamento == enumFormaPagamento.Bradesco)
                {
                    return false;
                }

                if (formaPagamento == enumFormaPagamento.HSBCAVista || formaPagamento == enumFormaPagamento.HSBCParcelado
                        || formaPagamento == enumFormaPagamento.HSBCTransferencia)
                {
                    if (formaPagamento == enumFormaPagamento.RedecardCredito || formaPagamento == enumFormaPagamento.VisaCredito
                        || formaPagamento == enumFormaPagamento.Amex || formaPagamento == enumFormaPagamento.HSBCAVista
                        || formaPagamento == enumFormaPagamento.HSBCParcelado)
                    {
                        IRLib.Parceria oParceria = new IRLib.Parceria();
                        try
                        {
                            if (oParceria.ValidaBin(codigoBIN) != IRLib.Parceria.Parceiro.HSBC)
                            {
                                return false;
                            }
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (formaPagamento == enumFormaPagamento.Bradesco)
                {
                    IRLib.Parceria oParceria = new IRLib.Parceria();
                    try
                    {
                        if (oParceria.ValidaBin(codigoBIN) != IRLib.Parceria.Parceiro.Bradesco)
                        {
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que efetua a venda no sistema
        /// </summary>
        /// <param name="oPagamento"></param>
        /// <param name="VirIDs"></param>
        /// <param name="celular"></param>
        /// <param name="ValorTotalSeguro"></param>
        /// <param name="oCarrinhoLista"></param>
        /// <param name="oRetorno"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <param name="compra"></param>
        /// <param name="compraPagamento"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> EfetuaVendaSistema(IRLib.ClientObjects.EstruturaPagamento oPagamento, List<int> VirIDs, string celular, decimal ValorTotalSeguro, IngressoRapido.Lib.CarrinhoLista oCarrinhoLista, IRLib.ClientObjects.EstruturaCompraTemporaria oRetorno, IRLib.CompraTemporaria oCompraTemporaria, CompraModel compra, CompraPagamentoModel compraPagamento)
        {
            LogUtil.Debug(string.Format("##VendaBilheteriaBO.EfetuandoVendaSistema## SESSION {0}", compra.SessionID));

            RetornoModel<VendaRetornoModel> retorno = Venda.EfetuaVendaSistemaTEF(oPagamento, VirIDs, celular, ValorTotalSeguro, oCarrinhoLista, compra, compraPagamento);
            try
            {
                if (oRetorno.ID > 0)
                {
                    oCompraTemporaria.Excluir(oRetorno.ID);
                }

                if (!retorno.Sucesso)
                {
                    compra.RetornoPagamento = retorno.Retorno.RetornoPagamento;
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = retorno.Mensagem };
                }

                compra.Pagamentos = new List<CompraPagamentoModel>();
                compra.Pagamentos.Add(compraPagamento);
                compra.VendaBilheteria = Consultar(retorno.Retorno.VendaBilheteriaID);
                return new RetornoModel<CompraModel>() { Sucesso = true, Retorno = compra, Mensagem = "Ok" };
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                //Ver o que precisa ajustar no carrinho antes de retornar.
                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = retorno.Mensagem };
            }
        }

        /// <summary>
        /// Método que efetua a venda no sistema
        /// </summary>
        /// <param name="oPagamento"></param>
        /// <param name="compra"></param>
        /// <param name="compraPagamento"></param>
        /// <returns></returns>
        private RetornoModel<string[]> EfetuaVendaExterna(CompraModel compra, CompraPagamentoExternoModel compraPagamento)
        {
            RetornoModel<string[]> retorno = Venda.EfetuaVendaExterna(compra, compraPagamento);
            try
            {
                if (!retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##EfetuaVendaExterna.ERROR## SESSION {0}, MSG {1}", compra.SessionID, retorno.Mensagem));
                    return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = retorno.Mensagem };
                }

                LogUtil.Info(string.Format("##Post.EfetuaVendaExterna.SUCCESS## SESSION {0}, PGTO {1}, VALOR {2}, COD_AUT {3}, NSU {4}, MSG {5}", compra.SessionID, compraPagamento.formaPagamento == null ? "NA" : compraPagamento.formaPagamento.Nome ?? "NA", compraPagamento.valor, compraPagamento.codigoAutorizacao ?? "NA", compraPagamento.NSU ?? "NA", retorno.Mensagem));
                return new RetornoModel<string[]>() { Sucesso = true, Retorno = retorno.Retorno, Mensagem = "Ok" };
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##EfetuaVendaExterna.EXCEPTION## SESSION {0}, MSG {1}", compra.SessionID, ex.Message));
                //Ver o que precisa ajustar no carrinho antes de retornar.
                return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = retorno.Mensagem };
            }
        }

        /// <summary>
        /// Método que efetua a venda cortesia no sistema
        /// </summary>
        /// <param name="oPagamento"></param>
        /// <param name="VirIDs"></param>
        /// <param name="celular"></param>
        /// <param name="ValorTotalSeguro"></param>
        /// <param name="oCarrinhoLista"></param>
        /// <param name="oRetorno"></param>
        /// <param name="oCompraTemporaria"></param>
        /// <param name="compra"></param>
        /// <param name="compraPagamento"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> EfetuaVendaSistemaCortesia(EstruturaPagamento oPagamento, string celular, IngressoRapido.Lib.CarrinhoLista oCarrinhoLista, IRLib.ClientObjects.EstruturaCompraTemporaria oRetorno, IRLib.CompraTemporaria oCompraTemporaria, CompraModel compra, CompraPagamentoModel compraPagamento)
        {
            RetornoModel<VendaRetornoModel> retorno = ecommerce.Venda.EfetuaVendaSistemaTEFCortesia(oPagamento, celular, oCarrinhoLista, compra, compraPagamento);
            try
            {
                if (oRetorno.ID > 0)
                {
                    oCompraTemporaria.Excluir(oRetorno.ID);
                }

                if (!retorno.Sucesso)
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = retorno.Mensagem };
                }

                compra.Pagamentos = new List<CompraPagamentoModel>();
                compra.Pagamentos.Add(compraPagamento);
                compra.VendaBilheteria = Consultar(retorno.Retorno.VendaBilheteriaID);
                return new RetornoModel<CompraModel>() { Sucesso = true, Retorno = compra, Mensagem = "Ok" };
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                //Ver o que precisa ajustar no carrinho antes de retornar.
                return new RetornoModel<CompraModel>() { Sucesso = false, Retorno = compra, Mensagem = retorno.Mensagem };
            }
        }

        /// <summary>
        /// Método que efetua a venda cortesia no sistema
        /// </summary>
        /// <param name="oPagamento"></param>
        /// <param name="compra"></param>
        /// <param name="compraPagamento"></param>
        /// <returns></returns>
        private RetornoModel<string[]> EfetuaVendaSistemaCortesiaExterno(CompraModel compra, CompraPagamentoExternoModel compraPagamento)
        {
            RetornoModel<string[]> retorno = Venda.EfetuaVendaSistemaTEFCortesiaExterna(compra, compraPagamento);
            try
            {
                if (!retorno.Sucesso)
                {
                    return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = retorno.Mensagem };
                }

                return new RetornoModel<string[]>() { Sucesso = true, Retorno = retorno.Retorno, Mensagem = "Ok" };
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                //Ver o que precisa ajustar no carrinho antes de retornar.
                return new RetornoModel<string[]>() { Sucesso = false, Retorno = null, Mensagem = retorno.Mensagem };
            }
        }

        #endregion

        #region Relatórios
        public List<RelatorioModel> ConsultarRelatorioFixoDayDream(int eventoID)
        {
            return ado.ConsultarRelatorioFixoDayDream(eventoID);
        }

        public List<RelatorioFilarmonicaModel> ConsultarRelatorioFilarmonica(int AssinaturaTipoID)
        {
            return ado.ConsultarRelatorioFilarmonica(AssinaturaTipoID);
        }

        public List<RelatorioRio> ConsultarRelatorioRio(string MesAno)
        {
            return ado.ConsultarRelatorioRio(MesAno);
        }

        public List<RelatorioGlobo> ConsultarRelatorioGlobo(string MesAno)
        {
            return ado.ConsultarRelatorioGlobo(MesAno);
        }

        public List<RelatorioPortoSeguro> ConsultarRelatorioPortoSeguro(string MesAno)
        {
            return ado.ConsultarRelatorioPortoSeguro(MesAno);
        }
        public List<RelatorioVendasCanal> ConsultarRelatorioVendasCanal(string MesAno)
        {
            return ado.ConsultarRelatorioVendasCanal(MesAno);
        }
        public List<RelatorioValeIngressoTroca> ConsultarRelatorioValeIngressoTroca(string MesAno)
        {
            return ado.ConsultarRelatorioValeIngressoTroca(MesAno);
        }
        public List<RelatorioValeIngressoVenda> ConsultarRelatorioValeIngressoVenda(string MesAno)
        {
            return ado.ConsultarRelatorioValeIngressoVenda(MesAno);
        }
        public List<RelatorioTaxaEntrega> ConsultarRelatorioTaxaEntrega(string MesAno)
        {
            return ado.ConsultarRelatorioTaxaEntrega(MesAno);
        }
        public List<RelatorioValeIngresso> ConsultarRelatorioValeIngresso(string MesAno)
        {
            return ado.ConsultarRelatorioValeIngresso(MesAno);
        }
        #endregion

        public List<AgregadoModel> ConsultarAgregados(int vendaBilheteriaID)
        {
            return ado.ConsultarAgregados(vendaBilheteriaID);
        }

        public String PutAgregado(int vendabilheteriaID, string nome, string cpf, string email, string telefone)
        {
            return ado.PutAgregado(vendabilheteriaID, nome, cpf, email, telefone);
        }

        public string DeleteAgregado(int vendabilheteriaID)
        {
            return ado.DeleteAgregado(vendabilheteriaID);
        }

        public IPagedList<SenhaCompraModel> ConsultarSenhasCompra(int pageNumber, int pageSize, Login login)
        {
            return ado.ConsultarSenhasCompra(login.ClienteID, pageNumber, pageSize);
        }

        public SenhaCompraDetalhe ConsultarSenhasCompraDetalhe(int vendabilheteriaID)
        {
            return ado.ConsultarSenhasCompraDetalhe(vendabilheteriaID);
        }

        public SenhaCompraDetalheCancelamento ConsultarSenhasCompraDetalheCancelamento(int vendabilheteriaID, int canalID)
        {
            return ado.ConsultarSenhasCompraDetalheCancelamento(vendabilheteriaID, canalID);
        }

        /// <summary>
        /// Lista as senha de cancelamento de um cliente
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public IPagedList<SenhaCancelamentoModel> ConsultarSenhasCancelamento(int pageNumber, int pageSize, Login login)
        {
            return ado.ConsultarSenhasCancelamento(pageNumber, pageSize, login.ClienteID);
        }

        public SenhaCancelamentoDetalhe ConsultarSenhasCancelamentoDetalhe(int vendabilheteriaID)
        {
            return ado.ConsultarSenhasCancelamentoDetalhe(vendabilheteriaID);
        }

        public int CheckSenhaCancelamento(string SenhaCancelamento)
        {
            return ado.CheckSenhaCancelamento(SenhaCancelamento);
        }

        public int GetSenhaCompra(int idSenhaCancel)
        {
            return ado.GetSenhaCompra(idSenhaCancel);
        }

        public CancelamentoAPIRetornoModel SolicitarCancelamento(CancelamentoAPIModel cancelamentoAPIModel, SenhaCompraDetalheCancelamento senhaCompraDetalheCancelamento,
            int clienteID, int usuarioID, int canalID, int lojaID, int caixaID, int empresaID, int vendaID, string clienteEmail)
        {
            IRLib.Venda venda = new IRLib.Venda();
            EstruturaCancelamento estruturaCancelamento = new EstruturaCancelamento();

            estruturaCancelamento.CaixaID = caixaID;
            estruturaCancelamento.LojaID = lojaID;
            estruturaCancelamento.CanalID = canalID;
            estruturaCancelamento.UsuarioID = usuarioID;
            estruturaCancelamento.EmpresaID = empresaID;
            estruturaCancelamento.ClienteID = clienteID;
            estruturaCancelamento.LocalID = 0;

            estruturaCancelamento.NumeroChamado = null;//Chamado do JIRA, quando o Supervisor o coloca
            estruturaCancelamento.NotaFiscalCliente = null;//Vem do Tef de cancelamento
            estruturaCancelamento.NotaFiscalEstabelecimento = null;//Vem do TEF de cancelamento

            tVendaBilheteria vendabilheteria = Consultar(senhaCompraDetalheCancelamento.CompraID);
            estruturaCancelamento.EntregaControleID = vendabilheteria.EntregaControleID ?? 0;
            estruturaCancelamento.EntregaAgendaID = vendabilheteria.EntregaAgendaID ?? 0;
            estruturaCancelamento.ValorConvenienciaEstornada = 0;
            estruturaCancelamento.ValorIngressosEstornado = senhaCompraDetalheCancelamento.Ingressos.Where(I => cancelamentoAPIModel.Ingressos.Contains(I.ID)).Sum(I => I.PrecoValor);
            estruturaCancelamento.ValorEntregaEstornado = (cancelamentoAPIModel.CancelaEntrega) ? senhaCompraDetalheCancelamento.TotalCompra.ValorTaxaEntrega : 0;
            estruturaCancelamento.ValorSeguroEstornado = (cancelamentoAPIModel.CancelaSeguro) ? senhaCompraDetalheCancelamento.ValorSeguro : 0;

            estruturaCancelamento.TipoCancelamento = IRLib.CancelamentoIngresso.EstruturaCancelamento.enuTipoCancelamento.Normal;
            estruturaCancelamento.CancelamentoFraude = false;

            estruturaCancelamento.VendaBilheteriaIDVenda = vendaID;
            IngressoDetalheCancelamento ingresso = senhaCompraDetalheCancelamento.Ingressos.FirstOrDefault(I => I.TemDevolucao && !I.Cancelado && cancelamentoAPIModel.Ingressos.Contains(I.ID));
            estruturaCancelamento.TemDevolucao = (ingresso != null);
            estruturaCancelamento.EhCanalPresente = false;
            estruturaCancelamento.SupervisorID = 0;

            estruturaCancelamento.MotivoCancelamento = 1;
            estruturaCancelamento.SubMotivoCancelamento = 0;

            System.Data.DataSet dados = venda.MontaDadosItensEIngressosVendidos(cancelamentoAPIModel.Ingressos);

            estruturaCancelamento.DadosItensVendidos = dados.Tables["Grid"];
            estruturaCancelamento.DadosIngressosVendidos = dados.Tables["Reserva"];

            if (cancelamentoAPIModel.FormaDevolucao == "D")
            {
                estruturaCancelamento.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.Deposito;
                estruturaCancelamento.DadosDeposito = new EstruturaCancelamento.EstruturaDevolucaoDeposito();
                estruturaCancelamento.DadosDeposito.Agencia = cancelamentoAPIModel.DadosDeposito.Agencia;
                estruturaCancelamento.DadosDeposito.Banco = cancelamentoAPIModel.DadosDeposito.Banco;
                estruturaCancelamento.DadosDeposito.Conta = cancelamentoAPIModel.DadosDeposito.Conta;
                estruturaCancelamento.DadosDeposito.CPF = cancelamentoAPIModel.DadosDeposito.CPF;
                estruturaCancelamento.DadosDeposito.Correntista = cancelamentoAPIModel.DadosDeposito.Nome;
                estruturaCancelamento.DadosDeposito.IsContaCorrente = (cancelamentoAPIModel.DadosDeposito.Poupanca != "T");
                estruturaCancelamento.DadosDeposito.Email = clienteEmail;
                estruturaCancelamento.DadosDeposito.Digito = cancelamentoAPIModel.DadosDeposito.DV;
            }
            else if (cancelamentoAPIModel.FormaDevolucao == "C")
            {
                PagamentoDetalheCancelamento pagamento = senhaCompraDetalheCancelamento.Pagamentos.FirstOrDefault();
                if (pagamento == null)
                {
                    return new CancelamentoAPIRetornoModel()
                    {
                        Erro = "Informações de estorno não encontradas",
                        Sucesso = false
                    };
                }
                estruturaCancelamento.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.EstornoCC;
                estruturaCancelamento.DadosEstornoCC = new EstruturaCancelamento.EstruturaDevolucaoEstornoCC();
                estruturaCancelamento.DadosEstornoCC.Bandeira = pagamento.Bandeira;
                estruturaCancelamento.DadosEstornoCC.NumeroCartao = pagamento.NumeroCartao;
                estruturaCancelamento.DadosEstornoCC.TitularCartao = (cancelamentoAPIModel.CartaoProprio) ? pagamento.NomeTitular : cancelamentoAPIModel.DadosCartao.TitularNome;
                estruturaCancelamento.DadosEstornoCC.TitularCPF = (cancelamentoAPIModel.CartaoProprio) ? pagamento.CPFTitular : cancelamentoAPIModel.DadosCartao.TitularCPF;
                estruturaCancelamento.DadosEstornoCC.Email = clienteEmail;
            }
            else if (cancelamentoAPIModel.FormaDevolucao == "P")
            {
                PagamentoDetalheCancelamento pagamento = senhaCompraDetalheCancelamento.Pagamentos.FirstOrDefault();
                if (pagamento == null || String.IsNullOrEmpty(pagamento.TransactionID))
                {
                    return new CancelamentoAPIRetornoModel()
                    {
                        Erro = "Informações de pagamento não encontradas",
                        Sucesso = false
                    };
                }
                CancelaPayPal cancelaPayPal = new CancelaPayPal();
                if (cancelaPayPal.RefundPayPal(pagamento.TransactionID))
                {
                    estruturaCancelamento.EstornoEfetuado = true;
                    estruturaCancelamento.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.EstornoCC;
                    estruturaCancelamento.DadosEstornoCC = new EstruturaCancelamento.EstruturaDevolucaoEstornoCC();
                    estruturaCancelamento.DadosEstornoCC.Bandeira = pagamento.Bandeira;
                    estruturaCancelamento.DadosEstornoCC.NumeroCartao = pagamento.NumeroCartao;
                    estruturaCancelamento.DadosEstornoCC.TitularCartao = (cancelamentoAPIModel.CartaoProprio) ? pagamento.NomeTitular : cancelamentoAPIModel.DadosCartao.TitularNome;
                    estruturaCancelamento.DadosEstornoCC.TitularCPF = (cancelamentoAPIModel.CartaoProprio) ? pagamento.CPFTitular : cancelamentoAPIModel.DadosCartao.TitularCPF;
                    estruturaCancelamento.DadosEstornoCC.Email = clienteEmail;
                }
                else
                {
                    return new CancelamentoAPIRetornoModel()
                    {
                        Erro = "Não foi possível estornar o pagamento do PayPal",
                        Sucesso = false
                    };
                }
            }

            CancelamentoIngressos cancelamentoIngressos = new CancelamentoIngressos();
            try
            {
                estruturaCancelamento = cancelamentoIngressos.SolicitarCancelamento(estruturaCancelamento);
                return new CancelamentoAPIRetornoModel()
                {
                    SenhaCancelamento = estruturaCancelamento.SenhaCancelamento,
                    Sucesso = true
                };
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new CancelamentoAPIRetornoModel()
                {
                    Erro = "Algum problema ocorreu",
                    Sucesso = false
                };
            }
        }
    }
}
