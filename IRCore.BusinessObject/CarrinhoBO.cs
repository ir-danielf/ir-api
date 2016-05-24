using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IRLib;
using Apresentacao = IRCore.DataAccess.Model.Apresentacao;
using Setor = IRCore.DataAccess.Model.Setor;

namespace IRCore.BusinessObject
{
    public class CarrinhoBO : MasterBO<CarrinhoADO>
    {
        public CarrinhoBO(MasterADOBase ado) : base(ado) { }
        public CarrinhoBO() : base(null) { }

        public List<Carrinho> Consulta(string sessionID, int clienteID)
        {
            var carrinhoItens = ado.Consultar(sessionID, clienteID);
            return carrinhoItens;
        }

        public Carrinho ConsultarIngresso(int ingressoID, string sessionID, enumCarrinhoStatus status, bool comObjetosVinculados = true)
        {
            if (comObjetosVinculados)
            {
                var carrinho = ado.ConsultarIngressoComMapeamento(ingressoID, sessionID, status);
                if ((carrinho != null) && (string.IsNullOrEmpty(carrinho.PacoteGrupo)))
                {
                    var precoBO = new PrecoBO(ado);
                    var eventoBO = new EventoBO(ado);
                    carrinho.Precos = precoBO.Listar(carrinho.SetorID.Value, carrinho.ApresentacaoID.Value);
                    carrinho.EventoObject.EventoMidias = eventoBO.ListarEventoMidiaInEventoAsDictionary(carrinho.EventoID ?? 0);
                }
                return carrinho;
            }
            else
            {
                return ado.ConsultarIngresso(ingressoID, sessionID, status);
            }
        }


        public List<Carrinho> Listar(string sessionID, int clienteID, enumCarrinhoStatus status = enumCarrinhoStatus.reservado, bool comObjetosVinculados = true)
        {
            if (comObjetosVinculados)
            {
                var lista = ado.ListarComMapeamento(sessionID, clienteID, status);
                if (lista.Count > 0)
                {
                    var precoBO = new PrecoBO(ado);
                    var eventoBO = new EventoBO(ado);

                    foreach (var carrinho in lista)
                    {
                        carrinho.Precos = precoBO.Listar(carrinho.SetorID.Value, carrinho.ApresentacaoID.Value);
                        carrinho.EventoObject.EventoMidias = eventoBO.ListarEventoMidiaInEventoAsDictionary(carrinho.EventoID ?? 0);
                    }

                }
                return lista;
            }
            else
            {
                var result = ado.Listar(sessionID, clienteID, status);
                return result;
            }
        }

        public List<Carrinho> ListarVendaBilheteria(int vendaBilheteriaId)
        {
            IngressoBO ingressoBO = new IngressoBO(ado);
            var listCarrinho = ado.ListarVendaBilheteria(vendaBilheteriaId);
            var listIngresso = ingressoBO.Listar(listCarrinho.Select(x => x.IngressoID ?? 0).Distinct().ToList());
            foreach (var item in listCarrinho)
            {
                tIngresso ingresso = listIngresso.FirstOrDefault(x => x.ID == item.IngressoID);
                if (ingresso != null)
                {
                    item.ApresentacaoObject = ingresso.tApresentacao.toApresentacao();
                    item.EventoObject = ingresso.tEvento.toEvento();
                    item.SetorObject = ingresso.tSetor.toSetor();
                }
            }
            return listCarrinho;
        }

        public bool Salvar(Carrinho carrinho)
        {
            return ado.Salvar(carrinho);
        }

        public void VincularCliente(int clienteID, string sessionID)
        {
            ado.VincularCliente(clienteID, sessionID);
        }

        public RetornoModel<Carrinho> ReservarVoucherIngresso(Voucher voucher, tIngresso ingresso, string sessionID, int clienteId = 0)
        {

            int usuarioId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.usuarioIdSistema);
            //
            // reservando para Voucher, em um setorId, a Quantidade pedida
            // reserva sera feita para sessionid (presente em voucher) ou em clienteId (caso ja logado)
            //
            // OBS: Todas os ingressos que são enviados para esse método pertencem ao mesmo setor e apresentação
            int setorId = ingresso.SetorID.Value;
            int apresentacaoId = ingresso.ApresentacaoID.Value;

            // obter setor em questao
            SetorBO setorBo = new SetorBO(ado);
            Setor setor = setorBo.Consultar(setorId, apresentacaoId);
            if (setor == null)
            {
                return new RetornoModel<Carrinho>() { Sucesso = false, Mensagem = "Setor não encontrado", Retorno = null };
            }

            // obter preco para aquele setor + parceiro
            PrecoBO precoBo = new PrecoBO(ado);
            PrecoParceiroMidia preco = precoBo.ConsultarParceiro(setorId, apresentacaoId, voucher.ParceiroMidiaID);
            if (preco == null)
            {
                return new RetornoModel<Carrinho>() { Sucesso = false, Mensagem = "Não existe preço cadastrado para Parceiros de Midia", Retorno = null };
            }

            IngressoBO ingBo = new IngressoBO(ado);

            // mudar status do ingresso para reservado
            ingresso.SessionID = sessionID;
            ingresso.ClienteID = clienteId;
            ingresso.UsuarioID = usuarioId;
            ingresso.PrecoID = preco.IR_PrecoID;
            ingresso.StatusAsEnum = enumIngressoStatus.reservado;
            ingresso.TimeStampReservaAsDateTime = DateTime.Now;

            #region Preenchendo objeto de Carrinho

            Carrinho carrinho = new Carrinho();
            carrinho.TimeStampAsDateTime = DateTime.Now;
            carrinho.StatusAsEnum = enumCarrinhoStatus.reservado;
            carrinho.TaxaConveniencia = 0;

            carrinho.VoucherID = voucher.ID;
            carrinho.SessionID = sessionID;
            carrinho.ClienteID = clienteId;
            carrinho.SerieID = null;

            carrinho.EventoID = setor.Apresentacao.EventoID;
            carrinho.Evento = setor.Apresentacao.Evento.Nome;
            carrinho.ApresentacaoID = setor.ApresentacaoID;
            carrinho.ApresentacaoDataHora = setor.Apresentacao.Horario;

            carrinho.SetorID = setor.ID;
            carrinho.Setor = setor.Nome;
            carrinho.LocalID = setor.Apresentacao.Evento.LocalID;
            carrinho.Local = setor.Apresentacao.Evento.Local.Nome;
            carrinho.TipoLugar = setor.LugarMarcado;

            carrinho.PrecoID = preco.IR_PrecoID;
            carrinho.PrecoNome = preco.Nome;
            carrinho.PrecoValor = preco.Valor;

            carrinho.IngressoID = ingresso.ID;
            carrinho.Codigo = ingresso.Codigo;
            carrinho.LugarID = ingresso.LugarID;


            #endregion

            try
            {
                if (ado.ReservarIngresso(carrinho, ingresso, enumIngressoStatus.bloqueado))
                {
                    return new RetornoModel<Carrinho>() { Sucesso = true, Mensagem = "OK", Retorno = carrinho };
                }
                else
                {
                    return new RetornoModel<Carrinho>() { Sucesso = false, Mensagem = "Os ingressos não estão mais disponíveis", Retorno = null };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<Carrinho>() { Sucesso = false, Mensagem = "Erro ao reservar Ingresso", Retorno = null };
            }
        }

        public RetornoModel<List<Carrinho>> ReservaVoucherQuantidade(Voucher voucher, int setorId, int apresentacaoId, int quantidade, string sessionID, int clienteId = 0)
        {
            IngressoBO ingBo = new IngressoBO(ado);

            // obter quantidade de Ingressos na tabela tIngresso
            List<tIngresso> listDisponiveis = ingBo.ListarParceiroStatus(voucher.ParceiroMidiaID, setorId, apresentacaoId, enumIngressoStatus.bloqueado, quantidade);
            if (listDisponiveis.Count < quantidade)
            {
                return new RetornoModel<List<Carrinho>>() { Sucesso = false, Retorno = new List<Carrinho>(), Mensagem = "Não existem ingressos disponiveis para este setor" };
            }
            RetornoModel<List<Carrinho>> retorno = new RetornoModel<List<Carrinho>>();
            retorno.Retorno = new List<Carrinho>();
            retorno.Sucesso = true;
            foreach (var ingresso in listDisponiveis)
            {
                RetornoModel<Carrinho> retornoItem = ReservarVoucherIngresso(voucher, ingresso, sessionID, clienteId);
                if (retornoItem.Sucesso)
                {
                    retorno.Retorno.Add(retornoItem.Retorno);
                }
                else
                {
                    retorno.Mensagem = retornoItem.Mensagem;
                    retorno.Sucesso = false;
                }
            }
            return retorno;
        }

        /// <summary>
        /// Retorna itens do carrinho vinculados a um sessionID que possuem um determinado status
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<Carrinho> Listar(string sessionId, enumCarrinhoStatus status)
        {
            return ado.Listar(sessionId, status);
        }

        /// <summary>
        /// Reserva de Quantidade (Pista)  
        /// </summary>
        /// <param name="carrinho"></param>
        /// <param name="setorID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="precosQuantidade"> int = precoId, int = Quantidade</param>
        /// <param name="sessionID"></param>
        /// <param name="clienteID"></param>
        /// <returns></returns>
        public RetornoModel<List<ReservaQuantidadeRetorno>> ReservarQuantidade(int setorID, int apresentacaoID, List<ReservaQuantidadeModel> precosQuantidade, string sessionID, int clienteID, int lojaID, int canalID, int usuarioID, bool isEntradaFranca)
        {
            try
            {
                //Carrega dados no objetos
                ApresentacaoADO apresentacaoado = new ApresentacaoADO(ado);
                Apresentacao ap = apresentacaoado.Consultar(apresentacaoID);
                //Monta Estrutura para passar para os métodos antigos
                IngressoRapido.Lib.CarrinhoLista lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();
                IngressoRapido.Lib.Preco preco = new IngressoRapido.Lib.Preco();
                foreach (var item in precosQuantidade)
                {
                    lstCarrinho.Add(new IngressoRapido.Lib.Carrinho()
                    {
                        EventoID = ap.EventoID,
                        ApresentacaoID = apresentacaoID,
                        SetorID = setorID,
                        PrecoID = item.PrecoID,
                        Quantidade = item.Quantidade,
                        ClienteID = clienteID,
                        SessionID = sessionID
                    });
                    if (!preco.ValidarPrecoLoteAtivo(item.PrecoID, item.PrecoID))
                    {
                        return new RetornoModel<List<ReservaQuantidadeRetorno>>() { Sucesso = false, Mensagem = "Preço selecionado não esta disponivel.", Retorno = null };
                    }
                }
                lstCarrinho.PreencherInformacoesReserva(string.Empty);
                bool temSerie = lstCarrinho.Where(c => c.SerieID > 0).Count() > 0;

                string msgErro = lstCarrinho.VerificarLimitesPrecoValorQuantidade(1, IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso, temSerie, ado.Contar(sessionID, enumCarrinhoStatus.reservado));
                if (string.IsNullOrEmpty(msgErro))
                {
                    var itens = lstCarrinho.InserirItensNovo(MontarEstruturaReserva(sessionID, clienteID, lojaID, canalID, usuarioID));
                    List<ReservaQuantidadeRetorno> retorno = itens.Select(item => new ReservaQuantidadeRetorno()
                    {
                        PrecoID = item.PrecoID,
                        IngressoID = item.IngressoID,
                        ValorIngressos = item.PrecoValor,
                        ValorTaxaConveniencia = item.TaxaConveniencia
                    }).ToList();

                    return new RetornoModel<List<ReservaQuantidadeRetorno>>() { Sucesso = true, Mensagem = "OK", Retorno = retorno };
                }
                else
                {
                    return new RetornoModel<List<ReservaQuantidadeRetorno>>() { Sucesso = false, Mensagem = msgErro };
                }
            }
            catch (Exception e)
            {
                return new RetornoModel<List<ReservaQuantidadeRetorno>>() { Sucesso = false, Mensagem = e.Message };
            }
        }

        /// <summary>
        /// Reserva de Quantidade (Pista)  
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="setorID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="precosQuantidade"> int = precoId, int = Quantidade</param>
        /// <param name="canalPOS">Tipo do canal</param>
        /// <param name="isEntradaFranca"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> SolicitarReservaQuantidade(CompraModel compra, int setorID, int apresentacaoID, List<ReservaQuantidadeModel> precosQuantidade, bool isEntradaFranca, bool canalPOS = false)
        {
            try
            {
                //Carrega dados no objetos
                var apresentacaoado = new ApresentacaoADO(ado);
                var ap = apresentacaoado.Consultar(apresentacaoID);

                if (ap == null)
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Apresentação não esta mais disponível para reserva", Retorno = compra };
                }

                //Monta Estrutura para passar para os métodos antigos
                var lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();
                var preco = new IngressoRapido.Lib.Preco();

                foreach (var item in precosQuantidade)
                {
                    lstCarrinho.Add(new IngressoRapido.Lib.Carrinho()
                    {
                        EventoID = ap.EventoID,
                        ApresentacaoID = apresentacaoID,
                        SetorID = setorID,
                        PrecoID = item.PrecoID,
                        Quantidade = item.Quantidade,
                        ClienteID = compra.ClienteID,
                        SessionID = compra.SessionID
                    });

                    if (!preco.ValidarPrecoLoteAtivo(item.PrecoID, item.Quantidade))
                    {
                        return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Preço selecionado não esta disponivel.", Retorno = compra };
                    }
                }

                if (lstCarrinho[0].VerificarSeExisteVIRouIngresso(IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso) > 0)
                {
                    LogUtil.Info(string.Format("##Post.AddPacote.ERROR## SESSION {0}, MSG {1}", compra.SessionID, "Não é possível comprar Ingressos e Vale-Ingressos ao mesmo tempo."));
                    return new RetornoModel<CompraModel>
                    {
                        Sucesso = false,
                        Retorno = compra,
                        Mensagem = "Não é possível comprar Ingressos e Vale-Ingressos ao mesmo tempo."
                    };
                }

                lstCarrinho.PreencherInformacoesReserva(string.Empty, canalPOS);
                var temSerie = lstCarrinho.Any(c => c.SerieID > 0);
                var msgErro = lstCarrinho.VerificarLimitesPrecoValorQuantidade(1, IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso, temSerie, compra.CarrinhoItens.Count);

                if (!string.IsNullOrEmpty(msgErro))
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = msgErro, Retorno = compra };

                compra = fromCarrinho(compra, lstCarrinho.InserirItensNovo(MontarEstruturaReservaMobile(compra), isEntradaFranca));

                return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            catch (Exception e)
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = e.Message, Retorno = compra };
            }
        }

        /// <summary>
        /// Solicita Reserva de Lugar Marcado
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="ingressoID"></param>
        /// <param name="precoID"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> SolicitarReservaIngresso(CompraModel compra, int ingressoID, int precoID)
        {
            try
            {
                IngressoRapido.Lib.Carrinho oCarrinho = new IngressoRapido.Lib.Carrinho();

                oCarrinho.SessionID = compra.SessionID;
                oCarrinho.ClienteID = compra.ClienteID;
                oCarrinho.PreencherInformacaoIngresso(ingressoID, 0, precoID, 0, string.Empty);

                IngressoRapido.Lib.CarrinhoLista lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();

                lstCarrinho.Add(oCarrinho);

                if (oCarrinho.PrecoID == 0)
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Preço selecionado não esta disponivel.", Retorno = compra };
                }

                IngressoRapido.Lib.Preco preco = new IngressoRapido.Lib.Preco();
                if (!preco.ValidarPrecoLoteAtivo(lstCarrinho[0].PrecoID, 1))
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Preço selecionado não esta disponivel.", Retorno = compra };
                }

                if (!lstCarrinho.VerificaReservaPNE(ingressoID, compra.SessionID))
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = IRLib.Setor.CADEIRANTE_NAO_RESERVADO, Retorno = compra };
                }
                else
                {
                    string qtdMsgErro = lstCarrinho.VerificarLimitesPrecoValorQuantidade(oCarrinho.PrecoValor, IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso, false, compra.CarrinhoItens.Count);

                    if (string.IsNullOrEmpty(qtdMsgErro))
                    {
                        var carrinhoOld = lstCarrinho.InserirLugarMarcado(MontarEstruturaReservaMobile(compra));
                        compra = AdicionarReserva(compra, ConsultarIngresso(carrinhoOld.IngressoID, compra.SessionID, enumCarrinhoStatus.reservado));

                        return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
                    }
                    else
                    {
                        return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = qtdMsgErro, Retorno = compra };
                    }
                }

            }
            catch (Exception ex)
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = ex.Message, Retorno = compra };
            }
        }

        /// <summary>
        /// Solicita Reserva das Mesas Fechadas
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="lugarID"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> SolicitarReservaMesaFechada(CompraModel compra, int apresentacaoID, int lugarID, int precoID)
        {
            IngressoADO ingressoado = new IngressoADO(ado);
            int quantidade = ingressoado.ObterTotalIngressosDeUmaMesaFechada(apresentacaoID, lugarID);

            return SolicitarReservaMesa(compra, lugarID, apresentacaoID, quantidade, IRLib.Setor.enumLugarMarcado.MesaFechada, precoID);

        }

        /// <summary>
        /// Solicitar Reserva Mesa Aberta
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="lugarID"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> SolicitarReservaMesaAberta(CompraModel compra, int apresentacaoID, int lugarID, int quantidade, int precoID)
        {
            return SolicitarReservaMesa(compra, lugarID, apresentacaoID, quantidade, IRLib.Setor.enumLugarMarcado.MesaAberta, precoID);
        }

        /// <summary>
        /// Metodo que identifica o tipo de mesa
        /// e faz a reserva.
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="lugarID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="quantidade"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private RetornoModel<CompraModel> SolicitarReservaMesa(CompraModel compra, int lugarID, int apresentacaoID, int quantidade, IRLib.Setor.enumLugarMarcado tipo, int precoID)
        {
            try
            {
                string tipoLugarApresentacao = ConsultarTipoLugar(apresentacaoID, lugarID);

                if (!string.IsNullOrEmpty(tipoLugarApresentacao) && tipoLugarApresentacao.Equals(tipo.ValueAsString()))
                {
                    IngressoRapido.Lib.CarrinhoLista lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();

                    lstCarrinho.PreencherInformacaoLugar(lugarID, apresentacaoID, quantidade, tipo, string.Empty, precoID);

                    if (lstCarrinho.Count == 0)
                    {
                        return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Preço selecionado não esta disponivel.", Retorno = compra };
                    }
                    if (precoID == 0)
                    {
                        IngressoRapido.Lib.Preco preco = new IngressoRapido.Lib.Preco();
                        if (!preco.ValidarPrecoLoteAtivo(lstCarrinho[0].PrecoID, quantidade))
                        {
                            return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Preço selecionado não esta disponivel.", Retorno = compra };
                        }
                    }

                    int qtdItensCarrinho = 0;
                    string tipoLugarMesaAberta = IRLib.Setor.enumLugarMarcado.MesaAberta.ValueAsString();
                    string tipolugarMesaFechada = IRLib.Setor.enumLugarMarcado.MesaFechada.ValueAsString();

                    if (tipo.ValueAsString().Equals(tipoLugarMesaAberta))
                    {
                        qtdItensCarrinho += compra.CarrinhoItens.Where(w => w.TipoLugar.Equals(tipoLugarMesaAberta)).ToList().Count;
                    }
                    else
                    {
                        qtdItensCarrinho += compra.CarrinhoItens.Where(w => w.TipoLugar.Equals(tipolugarMesaFechada)).Select(s => s.LugarID).Distinct().ToList().Count;
                    }

                    string qtdMsgErro = lstCarrinho.VerificarLimitesPrecoValorQuantidade(lstCarrinho.Sum(c => c.PrecoValor), IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso, false, qtdItensCarrinho);

                    if (string.IsNullOrEmpty(qtdMsgErro))
                    {
                        foreach (var item in lstCarrinho)
                        {
                            item.SessionID = compra.SessionID;
                            item.ClienteID = compra.ClienteID;
                        }
                        compra = fromCarrinho(compra, lstCarrinho.InserirMesa(MontarEstruturaReservaMobile(compra)));

                        return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
                    }
                    else
                    {
                        return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = qtdMsgErro, Retorno = compra };
                    }
                }
                else
                {
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Mesa selecionada inválida", Retorno = compra };
                }
            }
            catch (Exception ex)
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = ex.Message, Retorno = compra };
            }
        }

        public RetornoModel<CompraModel> SolicitarReservaPacote(CompraModel compra, List<PacoteReservaModel> pacotesQuantidade)
        {
            try
            {
                var lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();

                lstCarrinho.AddRange(pacotesQuantidade.Select(item => new IngressoRapido.Lib.Carrinho
                {
                    PacoteID = item.PacoteID,
                    Quantidade = item.Quantidade,
                    SessionID = compra.SessionID,
                    ClienteID = compra.ClienteID
                }));

                if (lstCarrinho[0].VerificarSeExisteVIRouIngresso(IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso) > 0)
                {
                    LogUtil.Info(string.Format("##Post.AddPacote.ERROR## SESSION {0}, MSG {1}", compra.SessionID, "Não é possível comprar Ingressos e Vale-Ingressos ao mesmo tempo."));
                    return new RetornoModel<CompraModel>
                    {
                        Sucesso = false,
                        Retorno = compra,
                        Mensagem = "Não é possível comprar Ingressos e Vale-Ingressos ao mesmo tempo."
                    };
                }

                lstCarrinho.PreencherInformacoesReservaPacote();

                var temSerie = lstCarrinho.Any(c => c.SerieID > 0);

                var msgRetorno = lstCarrinho.VerificarLimitesPrecoValorQuantidade(0, IngressoRapido.Lib.Carrinho.TipoReserva.Ingresso, temSerie, compra.CarrinhoItens.Count);

                if (!string.IsNullOrEmpty(msgRetorno))
                    return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = msgRetorno, Retorno = compra };

                compra = fromCarrinho(compra, lstCarrinho.InserirPacotesNovo(MontarEstruturaReservaMobile(compra)));

                return new RetornoModel<CompraModel> { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            catch (Exception ex)
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = ex.Message, Retorno = compra };
            }

        }

        public void RemoverReserva(Carrinho item)
        {
            IngressoBO ingressoBO = new IngressoBO(ado);
            ado.Remover(item.ID);
            tIngresso ingresso = ingressoBO.Consultar(item.IngressoID ?? 0);
            if (ingresso != null)
            {
                List<tIngresso> ingressos = new List<tIngresso>();
                ingressos.Add(ingresso);
                ingressoBO.LiberarReserva(ingressos);
            }

        }

        /// <summary>
        /// Método que recalcula os valores de um objeto compra
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        public CompraModel CalcularValores(CompraModel compra)
        {
            LogUtil.Debug(string.Format("##CarrinhoBO.CalculaValores## SESSION {0}", compra.SessionID));

            compra.Total = new CompraTotalModel();
            compra.Total.ValorIngressos = compra.CarrinhoItens.Sum(x => x.PrecoValor ?? 0);
            compra.Total.ValorTaxaConveniencia = compra.CarrinhoItens.Sum(x => x.TaxaConveniencia);
            //Calcula o valor total da forma de entrega
            if (compra.EntregaControleID > 0 && compra.EntregaControles != null && compra.EntregaControles.Count > 0)
            {
                var entregaControle = compra.EntregaControles.Where(x => x.ID == compra.EntregaControleID).FirstOrDefault();
                if (entregaControle != null)
                {
                    compra.Total.ValorTaxaEntrega = entregaControle.Valor;
                }
                else
                {
                    compra.Total.ValorTaxaEntrega = 0;
                }
            }

            //Cria um objeto de compraTotal para controle de descontos iniciada com os mesmo valores da compraTotal vinculada à compra
            CompraTotalModel totalControle = new CompraTotalModel();
            totalControle.CopyFrom(compra.Total);
            //Só faz aqs verificações de VIR se o mesmo tiver algum vir inserido
            if (compra.ValeIngressos != null && compra.ValeIngressos.Count > 0)
            {
                //Separa os vales pela quantidade de valores que eles incidem e os que incidem em mais de um valor fica ordenado por Ingresso e depois por Conveniencia
                var valesIncidenciaUnica = compra.ValeIngressos.Where(x => x.ValeIngressoTipo.NumeroIncidencias == 1);
                var valesIncidenciaDupla = compra.ValeIngressos.Where(x => x.ValeIngressoTipo.NumeroIncidencias == 2).OrderByDescending(x => x.ValeIngressoTipo.TrocaIngressoAsBool).ThenByDescending(x => x.ValeIngressoTipo.TrocaConvenienciaAsBool);
                var valesIncidenciaTripla = compra.ValeIngressos.Where(x => x.ValeIngressoTipo.NumeroIncidencias == 3).OrderByDescending(x => x.ValeIngressoTipo.TrocaIngressoAsBool).ThenByDescending(x => x.ValeIngressoTipo.TrocaConvenienciaAsBool);

                //Diminui os valores de incidencia unica no objeto de controle
                totalControle.ValorIngressos -= valesIncidenciaUnica.Where(x => x.ValeIngressoTipo.TrocaIngressoAsBool).Sum(x => x.ValeIngressoTipo.Valor ?? 0);
                totalControle.ValorTaxaConveniencia -= valesIncidenciaUnica.Where(x => x.ValeIngressoTipo.TrocaConvenienciaAsBool).Sum(x => x.ValeIngressoTipo.Valor ?? 0);
                totalControle.ValorTaxaEntrega -= valesIncidenciaUnica.Where(x => x.ValeIngressoTipo.TrocaConvenienciaAsBool).Sum(x => x.ValeIngressoTipo.Valor ?? 0);

                //Diminui os valores de incidencia dupla e depois de incidencia tripla, e chama o calculo de incidencias a cada VIR
                foreach (var item in valesIncidenciaDupla)
                    totalControle = CalculaMultiplasIncidencias(totalControle, item.ValeIngressoTipo);

                foreach (var item in valesIncidenciaTripla)
                    totalControle = CalculaMultiplasIncidencias(totalControle, item.ValeIngressoTipo);


                //Ajusta os valores de controle para que fiquem 0 se forem negativos
                if (totalControle.ValorIngressos < 0)
                    totalControle.ValorIngressos = 0;
                if (totalControle.ValorTaxaConveniencia < 0)
                    totalControle.ValorTaxaConveniencia = 0;
                if (totalControle.ValorTaxaEntrega < 0)
                    totalControle.ValorTaxaEntrega = 0;
            }
            //Seta o valor total na compra
            compra.Total.ValorTotal = totalControle.ValorIngressos + totalControle.ValorTaxaConveniencia + totalControle.ValorTaxaEntrega;

            //Seta o total de descontos atribuídos
            compra.Total.ValorDesconto = (compra.Total.ValorIngressos + compra.Total.ValorTaxaConveniencia + compra.Total.ValorTaxaEntrega) - compra.Total.ValorTotal;

            LogUtil.Debug(string.Format("##CarrinhoBO.CalculaValores.SUCCESS## SESSION {0}, TOTAL {1}, DESC {2}, ENTR {3}, FEE {4}", compra.SessionID, compra.Total.ValorTotal, compra.Total.ValorDesconto, compra.Total.ValorTaxaEntrega, compra.Total.ValorTaxaConveniencia));

            return compra;
        }

        /// <summary>
        /// Método que recebe um objeto compra e um valeIngressoTipo e retorna a compra com os valores ajustado pelo desconto
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="valeIngressoTipo"></param>
        /// <returns></returns>
        private CompraTotalModel CalculaMultiplasIncidencias(CompraTotalModel compraTotal, tValeIngressoTipo valeIngressoTipo)
        {

            //variavel que vai armazenar quando um vale ingresso não incidir inteiro em um unico valor
            decimal diferenca = 0;

            //Armazena o valor do vale que será usado nos calculos
            decimal valorVale = valeIngressoTipo.Valor ?? 0;

            //Descontar ingressos
            if (valeIngressoTipo.TrocaIngressoAsBool && compraTotal.ValorIngressos > 0 && valorVale > 0)
            {
                //verifica se terá diferença para desconto em outro valor
                if (compraTotal.ValorIngressos < valorVale)
                    diferenca = (valorVale) - compraTotal.ValorIngressos;

                compraTotal.ValorIngressos -= valorVale;
                //muda o valor do VIR pro valor da diferença
                valorVale = diferenca;
            }

            //Descontar Conveniencia
            if (valeIngressoTipo.TrocaConvenienciaAsBool && compraTotal.ValorTaxaConveniencia > 0 && valorVale > 0)
            {
                //verifica se terá diferença para desconto em outro valor
                if (compraTotal.ValorTaxaConveniencia < valorVale)
                    diferenca = valorVale - compraTotal.ValorTaxaConveniencia;

                compraTotal.ValorTaxaConveniencia -= valorVale;
                //muda o valor do VIR pro valor da diferença
                valorVale = diferenca;
            }
            //Descontar Entrega
            if (valeIngressoTipo.TrocaEntregaAsBool && compraTotal.ValorTaxaEntrega > 0 && valorVale > 0)
            {
                //verifica se terá diferença para desconto em outro valor
                if (compraTotal.ValorTaxaEntrega < valorVale)
                    diferenca = valorVale - compraTotal.ValorTaxaEntrega;

                compraTotal.ValorTaxaEntrega -= valorVale;
                //muda o valor do VIR pro valor da diferença
                valorVale = diferenca;
            }
            return compraTotal;
        }

        private CompraModel AdicionarReserva(CompraModel compra, Carrinho item, bool recalcularValores = true)
        {
            compra.CarrinhoItens.Add(item);
            if (recalcularValores)
            {
                compra = CalcularValores(compra);
            }
            return compra;
        }

        private CompraModel RemoverReserva(CompraModel compra, Carrinho item, bool recalcularValores = true)
        {
            RemoverReserva(item);
            compra.CarrinhoItens.Remove(item);
            if (recalcularValores)
            {
                compra = CalcularValores(compra);
            }
            return compra;
        }

        public RetornoModel<CompraModel> RemoverReserva(CompraModel compra, int carrinhoID)
        {
            Carrinho item = compra.CarrinhoItens.FirstOrDefault(t => t.ID == carrinhoID);
            if (item != null)
            {
                compra = RemoverReserva(compra, item);
                return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            else
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Sua reserva já expirou", Retorno = compra };
            }
        }

        public RetornoModel<CompraModel> RemoverReserva(CompraModel compra)
        {
            if (compra.CarrinhoItens.Count > 0)
            {
                foreach (var item in compra.CarrinhoItens.ToList())
                {
                    compra = RemoverReserva(compra, item, false);
                }
                compra = CalcularValores(compra);
                return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            else
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Sua reserva já expirou", Retorno = compra };
            }
        }

        public RetornoModel<CompraModel> RemoverReservaEvento(CompraModel compra, int eventoID)
        {
            List<Carrinho> itens = compra.CarrinhoItens.Where(t => t.EventoID == eventoID && string.IsNullOrEmpty(t.PacoteGrupo) && (t.PacoteID == null || t.PacoteID.Value == 0)).ToList();
            if (itens.Count > 0)
            {
                foreach (var item in itens)
                {
                    compra = RemoverReserva(compra, item, false);
                }
                compra = CalcularValores(compra);
                return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            else
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Sua reserva já expirou", Retorno = compra };
            }
        }

        public RetornoModel<CompraModel> RemoverReservaPacote(CompraModel compra, int pacoteID, string pacoteGrupo)
        {
            List<Carrinho> itens = compra.CarrinhoItens.Where(t => t.PacoteID != null && t.PacoteID.Value == pacoteID && t.PacoteGrupo == pacoteGrupo).ToList();
            if (itens.Count > 0)
            {
                foreach (var item in itens)
                {
                    compra = RemoverReserva(compra, item, false);
                }
                compra = CalcularValores(compra);
                return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            else
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Sua reserva já expirou", Retorno = compra };
            }
        }

        public RetornoModel<CompraModel> RemoverReservaMesa(CompraModel compra, int lugarID)
        {
            List<Carrinho> itens = compra.CarrinhoItens.Where(t => t.LugarID == lugarID).ToList();
            if (itens.Count > 0)
            {
                foreach (var item in itens)
                {
                    compra = RemoverReserva(compra, item, false);
                }
                compra = CalcularValores(compra);
                return new RetornoModel<CompraModel>() { Sucesso = true, Mensagem = "OK", Retorno = compra };
            }
            else
            {
                return new RetornoModel<CompraModel>() { Sucesso = false, Mensagem = "Sua reserva já expirou", Retorno = compra };
            }
        }

        private CompraModel fromCarrinho(CompraModel compra, List<IngressoRapido.Lib.Carrinho> lstCarrinhoOld)
        {
            foreach (var carrinhoOld in lstCarrinhoOld)
            {
                compra = AdicionarReserva(compra, ConsultarIngresso(carrinhoOld.IngressoID, compra.SessionID, enumCarrinhoStatus.reservado), false);
            }
            compra = CalcularValores(compra);
            return compra;
        }



        public Boolean AtualizarTempoCarrinhoIngresso(string sessionID, int clienteID, int minutos)
        {
            var ingressoADO = new IngressoADO(ado);

            var ingressos = ingressoADO.Consultar(sessionID, clienteID);
            var carrionhoItens = ado.Consultar(sessionID, clienteID);

            if (ingressos.Count > 0 && carrionhoItens.Count > 0)
            {
                ingressos.ForEach(i => i.TimeStampReserva = DateTime.ParseExact(i.TimeStampReserva, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).AddMinutes(minutos).ToString("yyyyMMddHHmmss"));
                carrionhoItens.ForEach(i => i.TimeStamp = DateTime.ParseExact(i.TimeStamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).AddMinutes(minutos).ToString("yyyyMMddHHmmss"));
            }

            return ado.Salvar();
        }

        #region metodos utilizados no media partner


        public List<Carrinho> ListarVoucher(int voucherId, string sessionId)
        {
            return ado.ListarVoucher(voucherId, sessionId);
        }

        #endregion


        private RetornoModel<int> AlterarPrecoReserva(Carrinho carrinho, int precoID, CompraEstruturaVendaModel estruturaVenda, string sessionID, int clienteID)
        {
            RetornoModel<int> retorno = new RetornoModel<int>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            if (carrinho == null)
            {

                retorno.Sucesso = false;
                retorno.Mensagem = "Reserva não encontrada";
            }
            else
            {
                try
                {
                    IngressoRapido.Lib.CarrinhoLista lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();

                    retorno.Retorno = carrinho.PrecoID.Value;

                    if (carrinho.TipoLugar.Equals("M") || carrinho.TipoLugar.Equals("A") || carrinho.TipoLugar.Equals("C"))
                    {
                        lstCarrinho.PreencherInformacaoLugar(carrinho.LugarID.Value, carrinho.ApresentacaoID.Value, 0, IRLib.Setor.enumLugarMarcado.MesaFechada, string.Empty);

                        if (lstCarrinho[0].Status == enumCarrinhoStatus.expirado.ToString())
                            throw new Exception("Impossivel alterar preço, reserva expirada!");

                        IngressoRapido.Lib.Preco preco = new IngressoRapido.Lib.Preco();
                        if (!(preco.ValidarMesmoLote(lstCarrinho[0].PrecoAtualID, precoID) || preco.ValidarPrecoLoteAtivo(precoID, lstCarrinho[0].QuantidadeMapa)))
                            throw new Exception("Preço selecionado não esta disponivel.");

                        lstCarrinho.MudarPrecoMobile(precoID, MontarEstruturaReservaMobile(estruturaVenda, sessionID, clienteID));
                    }
                    else
                    {
                        IngressoRapido.Lib.Carrinho oCarrinho = new IngressoRapido.Lib.Carrinho();

                        oCarrinho.GetByID(carrinho.ID);

                        if (oCarrinho.Status == enumCarrinhoStatus.expirado.ToString())
                            throw new Exception("Impossivel alterar preço, reserva expirada!");

                        lstCarrinho.Add(oCarrinho);

                        IngressoRapido.Lib.Preco preco = new IngressoRapido.Lib.Preco();
                        if (!(preco.ValidarMesmoLote(lstCarrinho[0].PrecoAtualID, precoID) || preco.ValidarPrecoLoteAtivo(precoID, 1)))
                            throw new Exception("Preço selecionado não esta disponivel.");

                        lstCarrinho.MudarPrecoMobile(precoID, 1, MontarEstruturaReservaMobile(estruturaVenda, sessionID, clienteID));
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    retorno.Sucesso = false;
                    retorno.Mensagem = ex.Message;
                }
            }

            return retorno;
        }

        public RetornoModel<int> AlterarPrecoIngresso(Carrinho carrinho, int precoID, CompraEstruturaVendaModel estruturaVenda, string sessionID, int clienteID)
        {
            RetornoModel<int> retorno = new RetornoModel<int>();
            try
            {
                IngressoRapido.Lib.CarrinhoLista lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();
                //lstCarrinho.CarregarDadosComListaPrecos(clienteID, sessionID);
                //lstCarrinho.RemoveAll(t => t.IngressoID != ingressoID);

                retorno = AlterarPrecoReserva(carrinho, precoID, estruturaVenda, sessionID, clienteID);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                retorno.Sucesso = false;
                retorno.Mensagem = ex.Message;
            }
            return retorno;


        }

        public RetornoModel<int> AlterarPrecoLugar(Carrinho carrinho, int precoID, CompraEstruturaVendaModel estruturaVenda, string sessionID, int clienteID)
        {

            RetornoModel<int> retorno = new RetornoModel<int>();
            try
            {
                //IngressoRapido.Lib.CarrinhoLista lstCarrinho = new IngressoRapido.Lib.CarrinhoLista();
                //lstCarrinho.CarregarDadosComListaPrecos(clienteID, sessionID);
                //lstCarrinho.RemoveAll(t => t.LugarID != lugarID);

                retorno = AlterarPrecoReserva(carrinho, precoID, estruturaVenda, sessionID, clienteID);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                retorno.Sucesso = false;
                retorno.Mensagem = ex.Message;
            }
            return retorno;
        }

        #region metodos auxiliares
        /// <summary>
        /// Método que retorna um a EstruturaReservaInternet
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        public IRLib.ClientObjects.EstruturaReservaInternet MontarEstruturaReservaMobile(CompraModel compra)
        {
            return MontarEstruturaReservaMobile(compra.EstruturaVenda, compra.SessionID, compra.ClienteID);
        }

        public IRLib.ClientObjects.EstruturaReservaInternet MontarEstruturaReservaMobile(CompraEstruturaVendaModel estruturaVenda, string sessionID, int clienteID)
        {
            if (estruturaVenda == null)
                estruturaVenda = new CompraEstruturaVendaModel();

            return new IRLib.ClientObjects.EstruturaReservaInternet()
            {

                LojaID = estruturaVenda.LojaID != 0 ? estruturaVenda.LojaID : Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InternetLojaID)),
                CaixaID = 0,
                CanalID = estruturaVenda.CanalID != 0 ? estruturaVenda.CanalID : Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.CanalInternet)),
                UsuarioID = estruturaVenda.UsuarioID != 0 ? estruturaVenda.UsuarioID : Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InternetUsuarioID)),
                ClienteID = clienteID,
                SessionID = sessionID,
                GUID = Guid.NewGuid().ToString(),
            };
        }

        /// <summary>
        /// Método que retorna um a EstruturaReservaInternet
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        public IRLib.ClientObjects.EstruturaReservaInternet MontarEstruturaReserva(string sessionID, int clienteID, int lojaID, int canalID, int usuarioID)
        {
            return new IRLib.ClientObjects.EstruturaReservaInternet()
            {

                LojaID = lojaID != 0 ? lojaID : Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InternetLojaID)),
                CaixaID = 0,
                CanalID = canalID != 0 ? canalID : Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.CanalInternet)),
                UsuarioID = usuarioID != 0 ? usuarioID : Convert.ToInt32(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InternetUsuarioID)),
                ClienteID = clienteID,
                SessionID = sessionID,
                GUID = Guid.NewGuid().ToString(),
            };
        }


        public void LimparReserva(string sessionID, enumIngressoStatus statusIngresso, List<Carrinho> itensCarrinho)
        {
            ado.LimparReserva(sessionID, statusIngresso);

            var cotaItemControle = new CotaItemControle();

            foreach (var item in itensCarrinho)
            {
                if (item.CotaItemID != null && item.CotaItemID > 0)
                {
                    var apresentacaoId = item.ApresentacaoID ?? 0;
                    var cotaItemId = item.CotaItemID ?? 0;
                    cotaItemControle.DecrementarControladorApresentacao(apresentacaoId, cotaItemId);
                }
            }

        }

        public void AtualizarStatus(string sessionID, enumCarrinhoStatus status)
        {
            ado.AtualizarStatus(sessionID, status);
        }
        #endregion

        public string ConsultarTipoLugar(int ingressoID)
        {
            return ado.ConsultarTipoLugar(ingressoID);
        }

        public string ConsultarTipoLugar(int apresetacaoID, int lugarID)
        {
            return ado.ConsultarTipoLugar(apresetacaoID, lugarID);
        }

        public bool ValidarCarrinho(List<Carrinho> carrinho)
        {
            return ado.Verificar(carrinho);
        }

        public bool RemoverDadosCota(int id)
        {
            return ado.RemoverDadosCota(id);
        }
    }

}