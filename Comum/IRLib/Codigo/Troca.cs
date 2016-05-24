using CTLib;
using IRLib.ClientObjects;
using IRLib.Codigo.TrocaIngresso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRLib.Codigo
{
    public class Troca : MarshalByRefObject, ISponsoredObject
    {
        /// <summary>
        /// Método que realiza a troca fazendo o cancelamento e nova venda Transacionados
        /// </summary>
        /// <param name="estruturaTrocaPreco"></param>
        /// <returns></returns>
        public EstruturaTrocaIngressoRetorno TrocarIngresso(EstruturaTrocaIngressoPreco estruturaTrocaPreco = null, EstruturaTrocaIngressoCredito estruturaTrocaCredito = null)
        {
            EstruturaTrocaIngressoRetorno retorno = new EstruturaTrocaIngressoRetorno();
            EstruturaTrocaIngressoCompra dadosCompra = null;
            DataTable ingressosTroca = null;
            DataTable itensTroca = null;

            BD bd = null;
            try
            {
                bd = new BD();
                bd.IniciarTransacao();

                //PARTE QUE CHAMA O CANCELAMENTO
                var cancelaIngressos = new IRLib.CancelamentoIngresso.CancelamentoIngressos();

                var dadosCancelamento = estruturaTrocaPreco != null ? estruturaTrocaPreco.DadosCancelamento : estruturaTrocaCredito.DadosCancelamento;

                IRLib.CancelamentoIngresso.EstruturaCancelamento DadosRetorno = cancelaIngressos.SolicitarCancelamento(dadosCancelamento, bd, true, new List<int>());
                ////

                if (DadosRetorno != null)
                {
                    bool prosseguir = true;

                    if (estruturaTrocaCredito != null && estruturaTrocaCredito.TrocaConvNaoAutorizada)
                    {
                        retorno.Mensagem = "Aguarde a validação do supervisor. A troca foi abortada.";
                        prosseguir = false;
                    }
                    else if (dadosCancelamento.FormaDevolucao == IRLib.CancelamentoIngresso.EstruturaCancelamento.enuFormaDevolucao.Deposito)
                    {
                        if (dadosCancelamento.Autorizado)
                        {
                            if (string.IsNullOrEmpty(dadosCancelamento.NumeroChamado))
                                prosseguir = true;
                            else
                            {
                                retorno.Mensagem = "Aguarde a validação do supervisor. A troca foi abortada.";
                                prosseguir = false;
                            }
                        }
                    }
                    else if (dadosCancelamento.FormaDevolucao == IRLib.CancelamentoIngresso.EstruturaCancelamento.enuFormaDevolucao.PayPal)
                    {
                        try
                        {
                            VendaBilheteria vb = new VendaBilheteria();
                            VendaBilheteriaFormaPagamento vbfp = new VendaBilheteriaFormaPagamento();
                            //vbi.Ler(DadosCancelamento.VendaBilheteriaIDVenda);
                            vb.LerSenhaCompra(bd, dadosCancelamento.SenhaVenda);
                            vbfp.LerPorVendaBilheteriaID(bd, vb.Control.ID);

                            if (vb.TipoPagamento(vb.Control.ID) == IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Paypal ||
                                 !string.IsNullOrEmpty(vbfp.TransactionID.Valor))
                            {
                                IRLib.PayPal.CancelaPayPal paypal = new IRLib.PayPal.CancelaPayPal();

                                if (paypal.RefundPartial(vbfp.TransactionID.Valor, dadosCancelamento.ValorEstornoparcial))
                                    prosseguir = true;
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            throw new Exception("Erro estorno paypal: " + exc.Message);
                            
                        }
                    }

                    if (prosseguir)
                    {
                        //Se for Troca de preço tem que tentar efetuar o método que simula a reserva, se for crédito é só seguir adiante pois os ingressos já estão reservados
                        prosseguir = (estruturaTrocaPreco != null ? ReservarIngressosTroca(estruturaTrocaPreco, bd) : true);

                        if (prosseguir)
                        {
                            //Chama a venda
                            if (estruturaTrocaPreco != null)
                            {
                                dadosCompra = estruturaTrocaPreco.DadosTrocaingressoCompra;
                                itensTroca = ingressosTroca = estruturaTrocaPreco.DataTableIngressosTroca;
                            }
                            else
                            {
                                dadosCompra = estruturaTrocaCredito.DadosTrocaingressoCompra;
                                ingressosTroca = estruturaTrocaCredito.DataTableIngressosTroca;
                                itensTroca = estruturaTrocaCredito.DataTableItensTroca;
                            }

                            retorno = SolicitaVendaTroca(dadosCompra, ingressosTroca, bd, DadosRetorno.VendaBilheteriaIDCancelamento, itensTroca);

                            if (retorno != null)
                            {
                                retorno.TrocaComSucesso = true;
                                retorno.Mensagem = "troca efetuada com sucesso.";
                            }
                            else
                            {
                                throw new Exception("Falha ao efetuar troca.");
                            }
                        }
                        else
                            throw new Exception("Falha ao efetuar troca.");
                    }
                }
                else
                    throw new Exception("Falha ao efetuar troca.");

                bd.FinalizarTransacao();

                if (retorno.TrocaComSucesso)
                {
                    /// - Modo para carregar dados da impressao
                    retorno.tImpressao = Ingresso.EstruturaImpressao();
                    ImpressaoGerenciador impressaoGerenciador = new ImpressaoGerenciador();
                    DataSet ds = impressaoGerenciador.PesquisarVenda(retorno.VendaBilheteriaIDTroca, dadosCompra.UsuarioLogin, dadosCompra.lojaID);
                    retorno.tImpressao = ds.Tables[Bilheteria.TABELA_ESTRUTURA_IMPRESSAO];
                }

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
            return retorno;
        }

        /// <summary>
        /// Método que faz a venda dos ingressos que estão sendo cancelados
        /// </summary>
        /// <param name="estruturaCompra"></param>
        /// <param name="dadosVendaIngressos"></param>
        /// <param name="bd"></param>
        /// <returns></returns>
        public EstruturaTrocaIngressoRetorno SolicitaVendaTroca(EstruturaTrocaIngressoCompra estruturaCompra, DataTable dadosVendaIngressos, BD bd, int vendaBilheteriaIdOrigem, DataTable dadosVendaItens)
        {

            EstruturaTrocaIngressoRetorno retorno = new EstruturaTrocaIngressoRetorno();


            if (estruturaCompra.caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo.");

            if (estruturaCompra.lojaID == 0)
                throw new BilheteriaException("Loja não pode ser nula.");

            if (estruturaCompra.canalID == 0)
                throw new BilheteriaException("Canal não pode ser nulo.");

            if (estruturaCompra.usuarioID == 0)
                throw new BilheteriaException("Usuário não pode ser nulo.");

            if (estruturaCompra.empresaID == 0)
                throw new BilheteriaException("Empresa não pode ser nula.");

            string statusFinal = string.Empty;

          
            statusFinal = Ingresso.VENDIDO;

            object idTemp = null;
            string codigoBarra = string.Empty;

            StringBuilder stbSQL = new StringBuilder();

            int entregaControleID = estruturaCompra.entregaControleID;
            int entregaAgendaID = estruturaCompra.entregaAgendaID;
            int pdvID = estruturaCompra.pdvIdSelecionado;
            int clienteEnderecoID = estruturaCompra.clienteEnderecoId;

            #region CADASTRAR VENDA BILHETERIA

            VendaBilheteria vendaBilheteria = new VendaBilheteria();

            string vendaBilheteriaStatus = VendaBilheteria.PAGO;

            vendaBilheteria.ClienteID.Valor = estruturaCompra.clienteID;
            vendaBilheteria.CaixaID.Valor = estruturaCompra.caixaID;
            vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
            vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;
            vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
            vendaBilheteria.TaxaEntregaID.Valor = estruturaCompra.taxaEntregaID;
            vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
            vendaBilheteria.EntregaControleID.Valor = entregaControleID;
            vendaBilheteria.EntregaAgendaID.Valor = entregaAgendaID;
            vendaBilheteria.PdvID.Valor = pdvID;
            vendaBilheteria.TaxaEntregaValor.Valor = estruturaCompra.valorEntrega;
            vendaBilheteria.ValorTotal.Valor = estruturaCompra.valorTotal;
            vendaBilheteria.NotaFiscalCliente.Valor = estruturaCompra.NotaFiscalCliente;
            vendaBilheteria.NotaFiscalEstabelecimento.Valor = estruturaCompra.NotaFiscalEstabelecimento;
            vendaBilheteria.IndiceInstituicaoTransacao.Valor = estruturaCompra.IndiceInstituicaoTransacao;
            vendaBilheteria.IndiceTipoCartao.Valor = estruturaCompra.IndiceTipoCartao;
            vendaBilheteria.NSUSitef.Valor = estruturaCompra.NSUSitef;
            vendaBilheteria.NSUHost.Valor = estruturaCompra.NSUHost;
            vendaBilheteria.CodigoAutorizacaoCredito.Valor = estruturaCompra.CodigoAutorizacaoCredito;
            vendaBilheteria.BIN.Valor = estruturaCompra.BIN;
            vendaBilheteria.ModalidadePagamentoCodigo.Valor = estruturaCompra.ModalidadePagamentoCodigo;
            vendaBilheteria.ModalidadePagamentoTexto.Valor = estruturaCompra.ModalidadePagamentoTexto.Replace("\0", "");
            vendaBilheteria.QuantidadeImpressoesInternet.Valor = 0;
            vendaBilheteria.ComissaoValorTotal.Valor = 0;
            vendaBilheteria.TaxaConvenienciaValorTotal.Valor = estruturaCompra.taxaConvenienciaValorTotal;
            vendaBilheteria.PagamentoProcessado.Valor = true;
            vendaBilheteria.NomeCartao.Valor = estruturaCompra.NomeCartao;
            vendaBilheteria.ValorSeguro.Valor = 0;
            vendaBilheteria.VendaBilheteriaIDOrigem.Valor = vendaBilheteriaIdOrigem;
            if (estruturaCompra.celular != string.Empty)
            {
                string DDD = estruturaCompra.celular.Substring(0, 2);
                string numeroCelular = estruturaCompra.celular.Substring(2, estruturaCompra.celular.Length - 2);

                vendaBilheteria.NumeroCelular.Valor = Convert.ToInt32(numeroCelular);
                vendaBilheteria.DDD.Valor = Convert.ToInt32(DDD);
            }

            idTemp = bd.ConsultaValor(vendaBilheteria.StringInserir());

            if (idTemp == null)
                throw new BilheteriaException("Venda não foi gerada.");
            else
                if (Convert.ToInt32(idTemp) <= 0)
                    throw new BilheteriaException("Venda não foi gerada.");

            vendaBilheteria.Control.ID = Convert.ToInt32(idTemp);

            retorno.VendaBilheteriaIDTroca = vendaBilheteria.Control.ID;
            #endregion


            ////Divisão de ingressos por tipo. Manipulação variável.
            DataRow[] itensNormais = dadosVendaItens.Select(EstruturaTrocaIngressoPreco.TIPO_PACOTE_INGRESSO + "= 'I' AND " + EstruturaTrocaIngressoPreco.TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
            DataRow[] itensMesaFechada = dadosVendaItens.Select(EstruturaTrocaIngressoPreco.TIPO_PACOTE_INGRESSO + "= 'I' AND " + EstruturaTrocaIngressoPreco.TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
            DataRow[] itensPacote = dadosVendaItens.Select(EstruturaTrocaIngressoPreco.TIPO_PACOTE_INGRESSO + "= 'P'");

            CotaItemControle oCotaItemControle = new CotaItemControle();
            IngressoCliente oIngressoCliente = new IngressoCliente();
            CodigoBarra oCodigoBarra = new CodigoBarra();
            //CodigosBarra = new Dictionary<int, string>();

            #region Cadeiras, mesa aberta e pista
            foreach (DataRow item in itensNormais)
            {
                DataRow[] ingressos = dadosVendaIngressos.Select(EstruturaTrocaIngressoPreco.RESERVA_ID + " = '" + (int)item[EstruturaTrocaIngressoPreco.RESERVA_ID] + "'");

                #region Cadastro de VendaBilheteriaItem

                VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                vendaBilheteriaItem.PacoteID.Valor = 0;
                vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                if (item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA] == DBNull.Value)
                    item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA] = 0;
                if (item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA] == DBNull.Value)
                    item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA] = 0;

                vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA];
                vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA];
                vendaBilheteriaItem.TaxaComissao.Valor = 0;
                vendaBilheteriaItem.ComissaoValor.Valor = 0;

                idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                if (idTemp == null)
                    throw new BilheteriaException("Item de Venda não foi gerada.");
                else
                    if (Convert.ToInt32(idTemp) <= 0)
                        throw new BilheteriaException("Item de Venda não foi gerada.");

                vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);
                #endregion

                int clienteIngresso = 0;
                

                #region Código de Barras

                codigoBarra = ingressos[0][EstruturaTrocaIngressoPreco.CODIGO_BARRA].ToString(); // Se não for imprimir, codigo de barras igual ao que está no ingresso -- pode ser lista branca!          

                // Registra a venda. Se existir o cliente selecionado, atualiza com o ID do Cliente
                string SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
                                            @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + estruturaCompra.lojaID + ", @VendaBilheteriaID = " +
                                            vendaBilheteria.Control.ID +
                                            " , @Status = '" + statusFinal + "', @ClienteID = " + estruturaCompra.clienteID + ", @UsuarioID = " + estruturaCompra.usuarioID +
                                            " , @IngressoID = " + (int)ingressos[0][EstruturaTrocaIngressoPreco.INGRESSO_ID] + " , @GerenciamentoIngressosID = " + (int)ingressos[0][EstruturaTrocaIngressoPreco.GERENCIAMENTO_INGRESSOS_ID];

                int codigoImpressao = (int)bd.ConsultaValor(SQL);

                #endregion


                if (codigoImpressao == -1)
                    throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas.(" + (int)ingressos[0][EstruturaTrocaIngressoPreco.INGRESSO_ID] + "-" + estruturaCompra.usuarioID + ")");
                else
                {
                    #region Ingressos Log

                    //inserir na Log
                    IngressoLog ingressoLog = new IngressoLog();
                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                    ingressoLog.IngressoID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.INGRESSO_ID];
                    ingressoLog.UsuarioID.Valor = estruturaCompra.usuarioID;
                    ingressoLog.BloqueioID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.BLOQUEIO_ID];
                    ingressoLog.CortesiaID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.CORTESIA_ID];
                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    ingressoLog.CaixaID.Valor = estruturaCompra.caixaID;
                    ingressoLog.LojaID.Valor = estruturaCompra.lojaID;
                    ingressoLog.CanalID.Valor = estruturaCompra.canalID;
                    ingressoLog.EmpresaID.Valor = estruturaCompra.empresaID;
                    ingressoLog.ClienteID.Valor = estruturaCompra.clienteID;
                    ingressoLog.PrecoID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.PRECO_ID];
                    ingressoLog.Acao.Valor = IngressoLog.VENDER;
                    ingressoLog.GerenciamentoIngressosID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.GERENCIAMENTO_INGRESSOS_ID];

                    if (bd.Executar(ingressoLog.StringInserir()) != 1)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                }

                    #endregion

                #region Cota

                //Incrementa Quantidades Vendidas tCotaItemControle e tIngressoCliente
                if ((int)ingressos[0][EstruturaTrocaIngressoPreco.COTA_ITEM_ID] > 0 || (int)ingressos[0][EstruturaTrocaIngressoPreco.COTA_ITEM_ID_APS] > 0)
                {
                    EstruturaDonoIngresso dono = estruturaCompra.ListaDonoIngresso.Where(c => c.IngressoID == (int)ingressos[0][EstruturaTrocaIngressoPreco.INGRESSO_ID]).FirstOrDefault();

                    if (dono != null)
                    {
                        oIngressoCliente.ApresentacaoID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.APRESENTACAO_ID];
                        oIngressoCliente.ApresentacaoSetorID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                        oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                        oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                        oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;

                        oIngressoCliente.DonoID.Valor = dono.DonoID;
                        oIngressoCliente.CPF.Valor = dono.CPF;

                        bd.Executar(oIngressoCliente.StringInserir());

                        //Preenche os Objs
                        oCotaItemControle.ApresentacaoSetorID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                        oCotaItemControle.ApresentacaoID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.APRESENTACAO_ID];


                        //Incrementa a quantidade da ApresentacaoSetor
                        if (dono.CotaItemIDAPS > 0)
                        {
                            oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                            bd.Executar(oCotaItemControle.StringAtualizarAPS());

                            //Atualizou agora verifica a Quantidade
                            stbSQL = new StringBuilder();
                            stbSQL.Append("SELECT ");
                            stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                            stbSQL.Append("THEN 0 ");
                            stbSQL.Append("ELSE 1 ");
                            stbSQL.Append("END AS Valido ");
                            stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                            stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                            stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, ingressos[0][EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID]);

                            if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                        }
                        if (dono.CotaItemID > 0)
                        {
                            oCotaItemControle.CotaItemID.Valor = dono.CotaItemID;

                            //Incrementa a quantidade da Apresentacao
                            BD bd2 = new BD();
                            bd2.Executar(oCotaItemControle.StringAtualizarAP());

                            //Atualizou agora verifica a Quantidade
                            stbSQL = new StringBuilder();
                            stbSQL.Append("SELECT ");
                            stbSQL.Append("CASE WHEN tApresentacao.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacao.Quantidade ");
                            stbSQL.Append("THEN 0 ");
                            stbSQL.Append("ELSE 1 ");
                            stbSQL.Append("END AS Valido ");
                            stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                            stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                            stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, ingressos[0][EstruturaTrocaIngressoPreco.APRESENTACAO_ID]);

                            if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentacao.");
                        }
                    }
                }
                #endregion

            }
            #endregion

            #region Mesa Fechada
            foreach (DataRow item in itensMesaFechada)
                {
                    DataRow[] ingressos = dadosVendaIngressos.Select(EstruturaTrocaIngressoPreco.RESERVA_ID + "='" + (int)item[EstruturaTrocaIngressoPreco.RESERVA_ID] + "'");

                    #region Cadastro de VendaBilheteria Item
                    
                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                    if (item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA] == DBNull.Value)
                        item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA] = 0;
                    if (item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA] == DBNull.Value)
                        item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA] = 0;

                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA];
                    vendaBilheteriaItem.TaxaComissao.Valor = 0;
                    vendaBilheteriaItem.ComissaoValor.Valor = 0;

                    idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                    if (idTemp == null)
                        throw new BilheteriaException("Item de Venda não foi gerada.");
                    else
                        if (Convert.ToInt32(idTemp) <= 0)
                            throw new BilheteriaException("Item de Venda não foi gerada.");

                    vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);
                    
                    #endregion

                    int clienteIngresso = 0;

                    foreach (DataRow ingresso in ingressos)
                    {
                        #region Código de Barras
                        codigoBarra = ingresso[EstruturaTrocaIngressoPreco.CODIGO_BARRA].ToString();
                        //Registra o ingresso em questão para o cliente e devolve o CodigoImpressao Atualizado
                           string SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
                                               @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + estruturaCompra.lojaID + ", @VendaBilheteriaID = " +
                                                vendaBilheteria.Control.ID +
                                                " , @Status = '" + statusFinal + "', @ClienteID = " + estruturaCompra.clienteID + ", @UsuarioID = " + estruturaCompra.usuarioID +
                                                " , @IngressoID = " + (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID] + ", @GerenciamentoIngressosID = " + (int)ingresso[EstruturaTrocaIngressoPreco.GERENCIAMENTO_INGRESSOS_ID];
                        

                        int codigoImpressao = Convert.ToInt32(bd.ConsultaValor(SQL));
                        

                        #endregion

                        #region IngressoLOG

                        if (codigoImpressao == -1)
                            throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas. (" + (int)ingressos[0][EstruturaTrocaIngressoPreco.INGRESSO_ID] + "-" + estruturaCompra.usuarioID + ")");
                        else
                        {
                            //inserir na Log
                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID];
                            ingressoLog.UsuarioID.Valor = estruturaCompra.usuarioID;
                            ingressoLog.BloqueioID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.BLOQUEIO_ID];
                            ingressoLog.CortesiaID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.CORTESIA_ID];
                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            ingressoLog.CaixaID.Valor = estruturaCompra.caixaID;
                            ingressoLog.LojaID.Valor = estruturaCompra.lojaID;
                            ingressoLog.CanalID.Valor = estruturaCompra.canalID;
                            ingressoLog.EmpresaID.Valor = estruturaCompra.canalID;
                            ingressoLog.ClienteID.Valor = estruturaCompra.clienteID;
                            ingressoLog.PrecoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.PRECO_ID];
                            ingressoLog.Acao.Valor = IngressoLog.VENDER;
                            ingressoLog.GerenciamentoIngressosID.Valor = (int)ingressos[0][EstruturaTrocaIngressoPreco.GERENCIAMENTO_INGRESSOS_ID];

                            if (bd.Executar(ingressoLog.StringInserir()) != 1)
                                throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                        }

                        #endregion

                        #region Cota

                        //Incrementa Quantidades Vendidas tCotaItemControle e tIngressoCliente
                        if ((int)ingresso[EstruturaTrocaIngressoPreco.COTA_ITEM_ID] > 0 || (int)ingresso[EstruturaTrocaIngressoPreco.COTA_ITEM_ID_APS] > 0)
                        {
                            EstruturaDonoIngresso dono = estruturaCompra.ListaDonoIngresso.Where(c => c.IngressoID == (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID]).FirstOrDefault();

                            if (dono != null)
                            {
                                oIngressoCliente.ApresentacaoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_ID];
                                oIngressoCliente.ApresentacaoSetorID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                                oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                                oIngressoCliente.DonoID.Valor = dono.DonoID;
                                oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                                oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                                oIngressoCliente.CPF.Valor = dono.CPF;

                                bd.Executar(oIngressoCliente.StringInserir());

                                //Preenche os Objs
                                oCotaItemControle.ApresentacaoSetorID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                                oCotaItemControle.ApresentacaoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                                
                                //Incrementa a quantidade da ApresentacaoSetor
                                if (dono.CotaItemIDAPS > 0)
                                {
                                    oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                    bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                    //Atualizou agora verifica a Quantidade
                                    stbSQL = new StringBuilder();
                                    stbSQL.Append("SELECT ");
                                    stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                    stbSQL.Append("THEN 0 ");
                                    stbSQL.Append("ELSE 1 ");
                                    stbSQL.Append("END AS Valido ");
                                    stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                    stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                    stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID]);


                                    if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                        throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                                }
                                if (dono.CotaItemID > 0)
                                {
                                    oCotaItemControle.CotaItemID.Valor = dono.CotaItemID;

                                    //Incrementa a quantidade da Apresentacao
                                    bd.Executar(oCotaItemControle.StringAtualizarAP());

                                    //Atualizou agora verifica a Quantidade
                                    stbSQL = new StringBuilder();
                                    stbSQL.Append("SELECT ");
                                    stbSQL.Append("CASE WHEN tApresentacao.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacao.Quantidade ");
                                    stbSQL.Append("THEN 0 ");
                                    stbSQL.Append("ELSE 1 ");
                                    stbSQL.Append("END AS Valido ");
                                    stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                    stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                    stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_ID]);

                                    if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                        throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentacao.");
                                }
                            }
                        }
                        #endregion
                    }

                }
                #endregion

            #region Pacotes

            int PacoteGrupo = 0;
            int pacoteIDAtual = 0;
            foreach (DataRow item in itensPacote)
            {
                DataRow[] ingressosPacote = dadosVendaIngressos.Select(EstruturaTrocaIngressoPreco.RESERVA_ID + "='" + (int)item[EstruturaTrocaIngressoPreco.RESERVA_ID] + "'");
                
                

                VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                vendaBilheteriaItem.PacoteID.Valor = (int)ingressosPacote[0][EstruturaTrocaIngressoCredito.PACOTE_ID];
                vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                if (pacoteIDAtual != vendaBilheteriaItem.PacoteID.Valor)
                {
                    PacoteGrupo = 0;
                    pacoteIDAtual = vendaBilheteriaItem.PacoteID.Valor;
                }
                PacoteGrupo++;

                vendaBilheteriaItem.PacoteGrupo.Valor = PacoteGrupo;

                if (item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA] == DBNull.Value)
                    item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA] = 0;
                if (item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA] == DBNull.Value)
                    item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA] = 0;



                int conv = (int)item[EstruturaTrocaIngressoPreco.TAXA_CONVENIENCIA];
                decimal valorConv = (decimal)item[EstruturaTrocaIngressoPreco.VALOR_CONVENIENCIA];
                int taxaComissao = 0;
                decimal comissaovalor = 0;


                if (!(bool)item[EstruturaTrocaIngressoCredito.PERMITIR_CANCELAMENTO_AVULSO])// "PermitirCancelamentoAvulso"])
                {
                    vendaBilheteriaItem.TaxaConveniencia.Valor = conv;
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = valorConv;
                    vendaBilheteriaItem.TaxaComissao.Valor = taxaComissao;
                    vendaBilheteriaItem.ComissaoValor.Valor = comissaovalor;

                    idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                    if (idTemp == null)
                        throw new BilheteriaException("Item de Venda não foi gerada.");
                    else
                        if (Convert.ToInt32(idTemp) <= 0)
                            throw new BilheteriaException("Item de Venda não foi gerada.");

                    vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);
                }


                foreach (DataRow ingresso in ingressosPacote)
                {
                    if ((bool)item[EstruturaTrocaIngressoCredito.PERMITIR_CANCELAMENTO_AVULSO])// "PermitirCancelamentoAvulso"])
                    {
                        CanalPacote canalPacote = new CanalPacote();

                        //busca as taxas de conveniencia e comissão e popula as variaveis
                        DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(estruturaCompra.canalID, vendaBilheteriaItem.PacoteID.Valor);
                        Preco precoIngresso = new Preco();
                        precoIngresso.Ler((int)ingresso[EstruturaTrocaIngressoPreco.PRECO_ID]);

                        int taxaConvenienciaPacote = (int)taxasPacote.Rows[0]["TaxaConveniencia"];
                        decimal taxaMinPacote = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMinima"]);
                        decimal taxaMaxPacote = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMaxima"]);
                        int taxaComissaoPacote = (int)taxasPacote.Rows[0]["TaxaComissao"];
                        decimal comissaoMinPacote = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMinima"]);
                        decimal comissaoMaxPacote = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMaxima"]);
                        decimal comissaoValorTotal = 0;
                        decimal taxaValorTotal = 0;

                        if (comissaovalor >= taxaComissaoPacote && taxaComissaoPacote != 0)
                            comissaoValorTotal = comissaovalor / ingressosPacote.Count();
                        else
                        {

                            if (taxaComissaoPacote > 0 || comissaoMinPacote > 0)
                            {
                                decimal dAux = (taxaComissaoPacote / 100m) * precoIngresso.Valor.Valor;

                                if (dAux < comissaoMinPacote)
                                    comissaoValorTotal += Decimal.Round(comissaoMinPacote, 2);
                                else if (dAux > comissaoMaxPacote && comissaoMaxPacote > 0)
                                    comissaoValorTotal += Decimal.Round(comissaoMaxPacote, 2);
                                else
                                    comissaoValorTotal += Decimal.Round(dAux, 2);
                            }
                        }
                        vendaBilheteriaItem.TaxaComissao.Valor = taxaComissaoPacote;

                        vendaBilheteriaItem.ComissaoValor.Valor = comissaoValorTotal;
                        //popula a conveniencia de acordo com a taxa minima e maxima
                        if (valorConv >= taxaMaxPacote && taxaMaxPacote != 0)
                            taxaValorTotal = valorConv / ingressosPacote.Count();
                        else
                        {
                            if (taxaConvenienciaPacote > 0 || taxaMinPacote > 0)
                            {
                                decimal dAux = (taxaConvenienciaPacote / 100m) * precoIngresso.Valor.Valor;

                                if (dAux < taxaMinPacote)
                                    taxaValorTotal += Decimal.Round(taxaMinPacote, 2);
                                else if (dAux > taxaMaxPacote && taxaMaxPacote > 0)
                                    taxaValorTotal += Decimal.Round(taxaMaxPacote, 2);
                                else
                                    taxaValorTotal += Decimal.Round(dAux, 2);
                            }
                        }

                        vendaBilheteriaItem.TaxaConveniencia.Valor = taxaConvenienciaPacote;
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = taxaValorTotal;

                        idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                        if (idTemp == null)
                            throw new BilheteriaException("Item de Venda não foi gerada.");
                        else
                            if (Convert.ToInt32(idTemp) <= 0)
                                throw new BilheteriaException("Item de Venda não foi gerada.");

                        vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);
                    }
                    
                    codigoBarra = ingresso[EstruturaTrocaIngressoPreco.CODIGO_BARRA].ToString();


                    string SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
                                           @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + estruturaCompra.lojaID + ", @VendaBilheteriaID = " +
                                        vendaBilheteria.Control.ID +
                                        " , @Status = '" + statusFinal + "', @ClienteID = " + estruturaCompra.clienteID + ", @UsuarioID = " + estruturaCompra.usuarioID +
                                        " , @IngressoID = " + (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID] + ", @GerenciamentoIngressosID = " + (int)ingresso[EstruturaTrocaIngressoPreco.GERENCIAMENTO_INGRESSOS_ID];

                    int codigoImpressao = (int)bd.ConsultaValor(SQL);

                    if (codigoImpressao == -1)
                        throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas.(" + (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID] + "-" + estruturaCompra.usuarioID + ")");
                    else
                    {
                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID];
                        ingressoLog.UsuarioID.Valor = estruturaCompra.usuarioID;
                        ingressoLog.BloqueioID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.BLOQUEIO_ID];
                        ingressoLog.CortesiaID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.CORTESIA_ID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = estruturaCompra.caixaID;
                        ingressoLog.LojaID.Valor = estruturaCompra.lojaID;
                        ingressoLog.CanalID.Valor = estruturaCompra.canalID;
                        ingressoLog.EmpresaID.Valor = estruturaCompra.empresaID;
                        ingressoLog.ClienteID.Valor = estruturaCompra.clienteID;
                        ingressoLog.PrecoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.PRECO_ID];
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;

                        if (bd.Executar(ingressoLog.StringInserir()) != 1)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                    }

                    //Incrementa Quantidades Vendidas tCotaItemControle e tIngressoCliente
                    if ((int)ingresso[EstruturaTrocaIngressoPreco.COTA_ITEM_ID] > 0 || (int)ingresso[EstruturaTrocaIngressoPreco.COTA_ITEM_ID_APS] > 0)
                    {
                        EstruturaDonoIngresso dono = estruturaCompra.ListaDonoIngresso.Where(c => c.IngressoID == (int)ingresso[EstruturaTrocaIngressoPreco.INGRESSO_ID]).FirstOrDefault();
                        if (dono != null)
                        {
                            //Preenche os Objs
                            oCotaItemControle.ApresentacaoSetorID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                            oCotaItemControle.ApresentacaoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_ID];
                            oCotaItemControle.CotaItemID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.COTA_ITEM_ID];
                            oIngressoCliente.ApresentacaoID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_ID];
                            oIngressoCliente.ApresentacaoSetorID.Valor = (int)ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID];
                            oIngressoCliente.CotaItemID.Valor = dono.CotaItemID;
                            oIngressoCliente.DonoID.Valor = dono.DonoID;
                            oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                            oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                            oIngressoCliente.CPF.Valor = dono.CPF;

                            bd.Executar(oIngressoCliente.StringInserir());

                            //Incrementa a quantidade da ApresentacaoSetor								
                            if (bd.Executar(oCotaItemControle.StringAtualizarAPS()) > 0)
                            {
                                //Atualizou agora verifica a Quantidade
                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_SETOR_ID]);

                                if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                            }
                            else
                            {
                                //Incrementa a quantidade da Apresentacao

                                if (bd.Executar(oCotaItemControle.StringAtualizarAP()) > 0)
                                {
                                    //Atualizou agora verifica a Quantidade
                                    stbSQL = new StringBuilder();
                                    stbSQL.Append("SELECT ");
                                    stbSQL.Append("CASE WHEN tApresentacao.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacao.Quantidade ");
                                    stbSQL.Append("THEN 0 ");
                                    stbSQL.Append("ELSE 1 ");
                                    stbSQL.Append("END AS Valido ");
                                    stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                    stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                    stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, ingresso[EstruturaTrocaIngressoPreco.APRESENTACAO_ID]);

                                    if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                        throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentacao.");
                                }
                            }
                        }
                    }
                }

            }
            #endregion

            #region Manipulação de formas de pagamento
            //VendaBilheteriaFormaPagamento
            //dividir o valorTotal nas formas de pagamento
            decimal porcentagemTotal = 0;
            decimal valorAtual = estruturaCompra.valorTotal;

            estruturaCompra.troco = Math.Abs(estruturaCompra.troco);

                
            for (int i = 0; i < estruturaCompra.pagamentos.Rows.Count; i++)
            {
                DataRow pagto = estruturaCompra.pagamentos.Rows[i];
                VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];
                //Dinheiro
                if (1 == (int)pagto["ID"] && !Convert.ToBoolean(pagto["FormaPagamentoCredito"]))
                    vendaBilheteriaFormaPagamento.Valor.Valor = ((decimal)pagto["Valor"] - estruturaCompra.troco);
                else
                    vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto["Valor"];

                valorAtual = valorAtual - vendaBilheteriaFormaPagamento.Valor.Valor;

                //calcular porcentagem
                decimal porc = estruturaCompra.valorTotal > 0 ? (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / estruturaCompra.valorTotal: 0;
                decimal porcentagem = Math.Round(porc, 2);

                //a porcentagem final tem q dar 100%
                if (i != (estruturaCompra.pagamentos.Rows.Count - 1))
                {
                    porcentagemTotal += porcentagem;
                }
                else
                {
                    //eh a ultima parcela
                    decimal totalTmp = porcentagemTotal + porcentagem;
                    if (totalTmp != 100)
                    {
                        porcentagem = 100 - porcentagemTotal;
                        porcentagemTotal += porcentagem;
                    }
                }

                int CartaoID = Convert.ToInt32(pagto["CartaoID"]);

                #region BlackList - Insere Cartão na tBlackList

                Cartao oCartao = new Cartao();

                if (CartaoID == 0 && estruturaCompra.AntiFraude && !Convert.ToBoolean(pagto["FormaPagamentoCredito"]))
                    if (pagto["NumeroCartao"].ToString().Length > 0 && Convert.ToInt32(pagto["ID"]) > 0 && estruturaCompra.clienteID > 0)
                        CartaoID = oCartao.InserirCartao(pagto["NumeroCartao"].ToString(), Convert.ToInt32(pagto["ID"]), estruturaCompra.clienteID, estruturaCompra.NomeCartao);

                #endregion

                vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                vendaBilheteriaFormaPagamento.Dias.Valor = (int)pagto["Dias"];
                vendaBilheteriaFormaPagamento.TaxaAdm.Valor = (decimal)pagto["TaxaAdm"];
                vendaBilheteriaFormaPagamento.IR.Valor = (string)pagto["IR"] == "T" ? true : false;
                vendaBilheteriaFormaPagamento.CartaoID.Valor = CartaoID;

                if (Convert.ToBoolean(pagto["FormaPagamentoCredito"]))
                {

                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = Convert.ToDateTime(pagto["DataDeposito"]);
                    vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = "";
                    vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = pagto["MensagemRetorno"].ToString();
                    vendaBilheteriaFormaPagamento.HoraTransacao.Valor = pagto["HoraTransacao"].ToString();
                    vendaBilheteriaFormaPagamento.DataTransacao.Valor = pagto["DataTransacao"].ToString();
                    vendaBilheteriaFormaPagamento.CodigoIR.Valor = pagto["CodigoIR"].ToString();
                    vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = "";
                    vendaBilheteriaFormaPagamento.NSUHost.Valor = "";
                    vendaBilheteriaFormaPagamento.NSUSitef.Valor = "";
                    vendaBilheteriaFormaPagamento.Cupom.Valor = pagto["Cupom"].ToString();
                    vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = pagto["DadosConfirmacaoVenda"].ToString();
                    vendaBilheteriaFormaPagamento.Rede.Valor = pagto["Rede"].ToString();
                    vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = "";
                    vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoIdOrigem.Valor = Convert.ToInt32(pagto["VendaBilheteriaFormaPagamentoIDOrigem"]);
                }
                else
                {
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                    vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = estruturaCompra.CodigoAutorizacaoCredito.ToString();
                    vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = estruturaCompra.MensagemRetorno;
                    vendaBilheteriaFormaPagamento.HoraTransacao.Valor = estruturaCompra.HoraTransacao;
                    vendaBilheteriaFormaPagamento.DataTransacao.Valor = estruturaCompra.HoraTransacao;
                    vendaBilheteriaFormaPagamento.CodigoIR.Valor = estruturaCompra.CodigoIR;
                    vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = estruturaCompra.NumeroAutorizacao;
                    vendaBilheteriaFormaPagamento.NSUHost.Valor = estruturaCompra.NSUHost.ToString();
                    vendaBilheteriaFormaPagamento.NSUSitef.Valor = estruturaCompra.NSUSitef.ToString();
                    vendaBilheteriaFormaPagamento.Cupom.Valor = estruturaCompra.NotaFiscalCliente;
                    vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = estruturaCompra.DadosConfirmacaoVenda;
                    vendaBilheteriaFormaPagamento.Rede.Valor = estruturaCompra.Rede;
                    vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = estruturaCompra.CodigoRespostaTransacao;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoIdOrigem.Valor = 0;
                }
                    
                
                vendaBilheteriaFormaPagamento.ValeIngressoID.Valor = 0;

                if (bd.Executar(vendaBilheteriaFormaPagamento.StringInserir()) < 1)
                    throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
            }

            #endregion

            if (estruturaCompra.celular.Length > 0 && vendaBilheteriaStatus == (string)VendaBilheteria.PAGO)
            {
                new IRLib.EnviaSMS().EnviaSms(false, vendaBilheteria.Control.ID, estruturaCompra.celular);
            }

            string SQLvenda = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
            idTemp = bd.ConsultaValor(SQLvenda);

            retorno.SenhaTroca = (idTemp != null) ? idTemp.ToString() : null;
            retorno.TrocaComSucesso = !string.IsNullOrEmpty(retorno.SenhaTroca);

            return retorno;
        }

        /// <summary>
        /// Lista os ingressos que podem ser trocados para o Grid principal da tela de troca
        /// </summary>
        /// <param name="ingressosID"></param>
        /// <returns></returns>
        public DataTable ListaIngressosTroca(List<int> ingressosID)
        {
            BD bd;
            if (ingressosID.Count > 0)
            {
                bd = new BD();

                try
                {
                    string strQuery = string.Format(@"SELECT 
                                                        Sum(tPreco.Valor) AS 'Valor',
                                                        SUM(tVendaBilheteriaItem.TaxaConvenienciaValor) AS 'Conveniencia'
                                                        ,tVendaBilheteria.TaxaEntregaValor AS 'Entrega'
                                                        ,tVendaBilheteria.ValorSeguro AS 'Seguro'
                                                        ,tFormaPagamento.FormaPagamentoTipoID
                                                        ,dbo.StringToDateTime(tVendaBilheteria.DataVenda) AS 'DataVenda'
                                                    FROM 
	                                                    tIngresso (NOLOCK) 
	                                                    INNER JOIN tVendaBilheteria (NOLOCK) ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID
	                                                    INNER JOIN tIngressoLog (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID
	                                                    INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID AND tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID 
	                                                    INNER JOIN tPreco (NOLOCK) ON tIngresso.PrecoID = tPreco.ID
                                                        INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBIlheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
											            INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = tVendaBilheteriaFormaPagamento.FormaPagamentoID
                                                    WHERE tIngresso.ID IN ({0})
											        GROUP BY tVendaBilheteria.TaxaEntregaValor, tVendaBilheteria.ValorSeguro, tFormaPagamento.FormaPagamentoTipoID, tVendaBilheteria.DataVenda"
                                                    , string.Join(",", ingressosID));



                    return bd.QueryToTable(strQuery);
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Trim().Contains("O timeout esgotou antes da conclusão da operação ou o servidor não está respondendo."))
                        MessageBox.Show(string.Format("O tempo limite de busca foi atingido.{0}Tente refinar melhor a busca.", Environment.NewLine));
                    return null;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método que faz a consulta que monta o DataTable de estrutura dos grids de troca de ingresso assim como base para preencher as cotas e usar nas venndas.
        /// Monta também uma tabela de preços novos usados na troca.
        /// </summary>
        /// <param name="ingressosPrecosIDs"></param>
        /// <param name="canalID"></param>
        /// <param name="clienteID"></param>
        /// <returns></returns>
        public DataSet MontaDadosIngressosTrocar(Dictionary<int, int> ingressosPrecosIDs, int canalID, int clienteID)
        {

            BD bd = null;

            //Query para pegar Base de dados do grid
            var sql = string.Format(@"Select Cast(ROW_NUMBER()over(order by ing.ID) as int) as 'ReservaID', 
	                                        ing.ID as IngressoID,
	                                        evt.Nome as Evento,
	                                        evt.ID as 'EventoID',
	                                        apr.CalcHorario as Horario,
	                                        sto.Nome as Setor,
	                                        sto.LugarMarcado as TipoLugar,
	                                        ing.codigo as Cod,
	                                        '' as Preco,
	                                        '-' as Cortesia,
	                                        0.0 as 'Valor R$',
	                                        ISNULL(cev.TaxaConveniencia, 0) as 'TX %',
	                                        0.0 as 'Conv R$',
	                                        ISNULL(cev.TaxaMinima, 0) as 'TaxaMin(R$)',
	                                        ISNULL(cev.TaxaMaxima, 0) as 'TaxaMax(R$)',
	                                        ISNULL(apr.CotaID,0) as 'ApresentacaoCotaID',
											ISNULL(aprs.CotaID,0) as 'ApresentacaoSetorCotaID',
											'' as Cota,
	                                        ISNULL(cli.Nome, '') as 'Cliente',
	                                        ISNULL(cli.CPF, '') as CPF,
	                                        ISNULL(cli.email, '') as 'Email',
	                                        '-' as 'Serie',
	                                        0 as 'CotaItemID',
	                                        0 as 'CotaItemIDAPS',
	                                        0 as 'DonoID',
	                                        'False' as 'Nominal',
	                                        'False' as 'ValidaBin',
                                            'False' as 'ValidaBinAPS',
	                                        0 as 'StatusCodigoPromo',
	                                        0 as 'ParceiroID',
                                            0 as 'ParceiroIDAPS',
                                            0 as 'QuantidadePorCodigo',
	                                        0 as 'QuantidadeCota',
	                                        0 as 'QuantidadePorClienteCota',
	                                        0 as 'QuantidadeCotaAPS',
	                                        0 as 'QuantidadePorClienteCotaAPS',
	                                        0 as 'QuantidadeApresentacao', 
	                                        0 as 'QuantidadePorClienteApresentacao',
	                                        0 as 'QuantidadeApresentacaoSetor',
	                                        0 as 'QuantidadePorClienteApresentacaoSetor',
	                                        '' as 'CodigoPromo',
	                                        ing.ApresentacaoID as 'ApresentacaoID',
	                                        ing.ApresentacaoSetorID as 'ApresentacaoSetorID',
	                                        '' as 'PrecoIniciaCom',
	                                        0 as 'ObrigatoriedadeID',
	                                        evt.TipoCodigoBarra as TipoCodigoBarra,
	                                        0 as PrecoID,
	                                        ISNULL(ing.GerenciamentoIngressosID ,0) as GerenciamentoIngressosID,
	                                        0 as BloqueioID,
	                                        0 as CortesiaID,
	                                        ing.codigoSequencial as CodigoSequencial,
	                                        '' as CodigoBarra,
                                            l.Nome as Local,
                                            p.Nome as PrecoOriginal,
                                            p.Valor as ValorOriginal,
                                            0 as CotaItemIDOriginal,
                                            0 as CotaItemIDAPSOriginal,
                                            'I' as TipoPacoteIngresso
	                                   from tIngresso ing(nolock)
	                             INNER JOIN tEvento evt(nolock) ON evt.ID = ing.eventoID
	                             INNER JOIN tApresentacao apr(nolock) ON apr.ID = ing.apresentacaoID
	                             INNER JOIN tSetor sto(nolock) ON sto.ID = ing.setorID
                                 INNER JOIN tpreco p (nolock) on p.id = ing.precoid
                                 INNER JOIN tApresentacaoSetor aprs(nolock) ON aprs.ID = ing.ApresentacaoSetorID
	                             INNER JOIN tLocal l (nolock) ON l.ID = ing.localID
                                  LEFT JOIN tCanalEvento cev(nolock) ON cev.ID in (SELECT max(cev2.ID) 
                                                                                     FROM tCanalEvento cev2(nolock) 
                                                                                    WHERE cev2.EventoID = evt.ID 
                                                                                      AND cev2.canalID = {0})
	                              LEFT JOIN tCliente cli(NOLOCK) ON cli.ID = {1}
	                                  where ing.ID in ({2});", canalID, clienteID, string.Join(",", ingressosPrecosIDs.Select(x => x.Key).ToList()));
            //Query para pegar os Preços Novos
            sql += string.Format(@"Select ID,Nome,Valor from tPreco(nolock)
                     Where ID in ({0})", string.Join(",", ingressosPrecosIDs.Select(x => x.Value).ToList()));

            try
            {
                bd = new BD();
                DataSet dados = new DataSet("Dados");
                dados = bd.QueryToDataSet(sql);
                return dados;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataTable BuscarIngressosOriginais(int vendaBilheteriaID, int canalID, int clienteID)
        {
            BD bd = null;

            //Query para pegar Base de dados do grid
            var sql = string.Format(@"Select Cast(ROW_NUMBER()over(order by ing.ID) as int) as 'ReservaID', 
	                                        ing.ID as IngressoID,
	                                        evt.Nome as Evento,
	                                        evt.ID as 'EventoID',
	                                        apr.CalcHorario as Horario,
	                                        sto.Nome as Setor,
	                                        sto.LugarMarcado as TipoLugar,
	                                        ing.codigo as Cod,
	                                        '' as Preco,
	                                        '-' as Cortesia,
	                                        0.0 as 'Valor R$',
	                                        ISNULL(cev.TaxaConveniencia, 0) as 'TX %',
	                                        0.0 as 'Conv R$',
	                                        ISNULL(cev.TaxaMinima, 0) as 'TaxaMin(R$)',
	                                        ISNULL(cev.TaxaMaxima, 0) as 'TaxaMax(R$)',
	                                        apr.CotaID,'' as Cota,
	                                        ISNULL(cli.Nome, '') as 'Cliente',
	                                        ISNULL(cli.CPF, '') as CPF,
	                                        ISNULL(cli.email, '') as 'Email',
	                                        '-' as 'Serie',
	                                        0 as 'CotaItemID',
	                                        0 as 'CotaItemIDAPS',
	                                        0 as 'DonoID',
	                                        'False' as 'Nominal',
	                                        'False' as 'ValidaBin',
	                                        'True' as 'ValidaBinAPS',
	                                        0 as 'StatusCodigoPromo',
	                                        0 as 'ParceiroID',
	                                        0 as 'QuantidadeCota',
	                                        0 as 'QuantidadePorClienteCota',
	                                        0 as 'QuantidadeCotaAPS',
	                                        0 as 'QuantidadePorClienteCotaAPS',
	                                        0 as 'QuantidadeApresentacao', 
	                                        0 as 'QuantidadePorClienteApresentacao',
	                                        0 as 'QuantidadeApresentacaoSetor',
	                                        0 as 'QuantidadePorClienteApresentacaoSetor',
	                                        '' as 'CodigoPromo',
	                                        ing.ApresentacaoID as 'ApresentacaoID',
	                                        ing.ApresentacaoSetorID as 'ApresentacaoSetorID',
	                                        '' as 'PrecoIniciaCom',
	                                        0 as 'ObrigatoriedadeID',
	                                        evt.TipoCodigoBarra as TipoCodigoBarra,
	                                        0 as PrecoID,
	                                        ISNULL(ing.GerenciamentoIngressosID ,0) as GerenciamentoIngressosID,
	                                        0 as BloqueioID,
	                                        0 as CortesiaID,
	                                        ing.codigoSequencial as CodigoSequencial,
	                                        '' as CodigoBarra,
                                            l.Nome as Local,
                                            p.Nome as PrecoOriginal,
                                            p.Valor as ValorOriginal,
                                            cli.Nome as NomeCliente
	                                   from tIngresso ing(nolock)
	                             INNER JOIN tEvento evt(nolock) ON evt.ID = ing.eventoID
	                             INNER JOIN tApresentacao apr(nolock) ON apr.ID = ing.apresentacaoID
	                             INNER JOIN tSetor sto(nolock) ON sto.ID = ing.setorID
	                             INNER JOIN tLocal l (nolock) ON l.ID = ing.localID
                                 INNER JOIN tpreco p (nolock) on p.id = ing.precoid
	                              LEFT JOIN tCanalEvento cev(nolock) ON cev.EventoID = evt.ID AND cev.canalID = {0}
	                              LEFT JOIN tCliente cli(NOLOCK) ON cli.ID = {1}
	                                  where ing.VendaBilheteriaID in ({2});", canalID, clienteID, vendaBilheteriaID);

            try
            {
                bd = new BD();
                DataTable dados = new DataTable("Dados");
                dados = bd.QueryToTable(sql);
                return dados;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bd.Fechar();
            }

        }




        //TODO: Método que não da suporte para troca de mesa fechada. Se for usar mesa fechada pra troca de preço, deverá usar o método MontaDadosItensEIngressosVendidosTrocaCredito
        /// <summary>
        /// Monta tabela de Itens vendidos para usar na parte de cancelamento da troca de Preço, pois não da suporte a mesa fechada.
        /// </summary>
        /// <param name="ingressoIDs"></param>
        /// <returns></returns>
        public DataSet MontaDadosItensEIngressosVendidos(List<int> ingressoIDs)
        {
            if (ingressoIDs != null && ingressoIDs.Count > 0)
            {
                var sql = string.Format(@"Select Cast(ROW_NUMBER()over(order by vbi.ID) as int) as 'ReservaID', vbi.ID as 'VendaBilheteriaItemID', apr.DisponivelAjuste as 'DisponivelAjuste',
	                        ing.status as 'Status', '' as 'StatusDetalhado', 
 	                        vbi.TaxaConveniencia as 'Conv', vbi.TaxaConvenienciaValor as 'Valor Conv', 'I' as 'Tipo',
 	                        sto.LugarMarcado as 'TipoLugar', vbi.TaxaComissao as 'TaxaComissao', vbi.ComissaoValor as 'ComissaoValor',
	                        vb.Obs as 'Obs', vbi.PacoteGrupo as 'PacoteGrupo' , ing.AssinaturaClienteID as 'AssinaturaClienteID',
                            et.Tipo as 'TipoEntrega'
	                    FROM tIngresso ing(NOLOCK)
	                    INNER JOIN tIngressoLog ingl(NOLOCK) ON ingl.ingressoID = ing.ID
	                    INNER JOIN tApresentacao apr(NOLOCK) ON apr.ID = ing.apresentacaoID
                        INNER JOIN tVendaBilheteria vb(NOLOCK) ON vb.ID = ing.vendabilheteriaID	                    
                        INNER JOIN tVendaBilheteriaItem vbi(nolock) ON vbi.ID = ingl.VendaBilheteriaItemID AND vb.ID = vbi.VendaBilheteriaID
	                    INNER JOIN tSetor sto(NOLOCK) ON sto.ID = ing.SetorID
	                    LEFT JOIN tEntregaControle etc(NOLOCK) ON vb.EntregaControleID = etc.ID
	                    LEFT JOIN tEntrega et(NOLOCK) ON et.ID = etc.EntregaID
                        where ing.ID in ({0}); 

                        Select Cast(ROW_NUMBER()over(order by vbi.ID) as int) as 'ReservaID', vbi.ID as 'VendaBilheteriaItemID', ing.ID as 'IngressoID'
	                                      ,ing.PacoteID as 'PacoteID', ing.PrecoID as 'PrecoID', ing.CortesiaID as 'CortesiaID', ing.BloqueioID as 'BloqueioID'
	                                      ,evt.TipoCodigoBarra as 'TipoCodigoBarra', ing.CodigoBarra as 'CodigoBarra', ing.eventoID as 'EventoID'
	                                      ,ing.apresentacaoSetorID as 'ApresentacaoSetorID', ing.AssinaturaClienteID as 'AssinaturaClienteID'
	                                      ,vbi.TaxaConvenienciaValor as 'Valor Conv'
	                                  FROM tIngresso ing(NOLOCK)
	                                  INNER JOIN tIngressoLog ingl(NOLOCK) ON ingl.ingressoID = ing.ID
	                                  INNER JOIN tVendaBilheteriaItem vbi(nolock) ON vbi.ID = ingl.VendaBilheteriaItemID AND vbi.VendaBilheteriaID = ing.VendaBilheteriaID
	                                  INNER JOIN tEvento evt(NOLOCK) ON evt.ID = ing.EventoID
	                                  WHERE ing.ID in ({0})", string.Join(",", ingressoIDs));

                BD bd = null;
                try
                {
                    bd = new BD();
                    DataSet dataSet = new DataSet();
                    dataSet = bd.QueryToDataSet(sql);
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        //Ajusta a tabela de Itens Vendidos
                        if (dataSet.Tables[0] != null)
                        {
                            dataSet.Tables[0].TableName = "DadosItensVendidos";

                            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                            {
                                string status = dataSet.Tables[0].Rows[i]["Status"].ToString();
                                string tipoEntrega = dataSet.Tables[0].Rows[i]["TipoEntrega"].ToString();
                                dataSet.Tables[0].Rows[i]["StatusDetalhado"] = Ingresso.StatusDetalhado(status, tipoEntrega);
                                dataSet.Tables[0].Rows[i]["Status"] = Ingresso.StatusDescritivo(status);
                            }

                            dataSet.Tables[0].Columns.Remove("TipoEntrega");
                        }
                        if (dataSet.Tables[1] != null)
                            dataSet.Tables[1].TableName = "DadosIngressoVendido";

                    }
                    return dataSet;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
                return null;

        }

        public DataSet MontaDadosItensEIngressosVendidosTrocaCredito(List<int> ingressoIDs,BD bd = null)
        {

            int vendaID = 0;
            bool conexaoExterna = bd != null;

            if (ingressoIDs != null && ingressoIDs.Count > 0)
            {
                

                DataSet retorno = EstruturaGrid();
                DataTable info = EstruturaInfoVenda();



                string sql = string.Format(@"SELECT vb.ID, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor,
                                vb.EntregaControleID, vb.EntregaAgendaID, vb.ValorTotal, vb.Status, vb.Senha, vb.DataVenda, vb.Obs,
                                c.Nome AS Cliente, vb.ClienteID , c.Email, tCanal.Nome AS Canal, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, vb.NivelRisco,
                                IsNull(e.Tipo, '') AS TaxaEntregaTipo , c.CNPJ , c.NomeFantasia, IsNull(vb.PagamentoProcessado, 'T') AS PagamentoProcessado, TaxaProcessamentoValor, TaxaProcessamentoCancelada, ISNULL(vb.ValorSeguro, 0) AS ValorSeguro, ISNULL(TransactionID,'') as TransactionID
                                FROM tVendaBilheteria AS vb (NOLOCK)
                                LEFT JOIN tVendaBilheteriaFormaPagamento AS vbfp (NOLOCK) ON vb.ID = vbfp.VendaBilheteriaID
                                INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID
                                INNER JOIN tUsuario (NOLOCK) AS u ON u.ID = tCaixa.UsuarioID
                                INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID=tLoja.ID
                                INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID=tCanal.ID
                                INNER JOIN tIngresso (NOLOCK) ON vb.ID=tIngresso.VendaBilheteriaID
                                LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
                                LEFT JOIN tEntregaControle ec (NOLOCK) ON ec.ID = vb.EntregaControleID
                                LEFT JOIN tEntrega e (NOLOCK) ON e.ID = ec.EntregaID
                                WHERE tIngresso.ID in ({0})", string.Join(",", ingressoIDs));

                try
                {
                    if(bd == null)
                        bd = new BD();

                    bd.QueryToTable(sql);
                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {
                        DataRow linha = info.NewRow();
                        vendaID = bd.LerInt("ID");
                        linha["ID"] = vendaID;
                        linha[TAXA_CONV_VALOR_TOTAL] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linha[TAXA_ENTREGA_VALOR] = bd.LerDecimal("TaxaEntregaValor");
                        linha[ENTREGA_CONTROLE_ID] = bd.LerInt("EntregaControleID");
                        linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal");
                        linha[STATUS_VENDA] = bd.LerString("Status");
                        String nomeCliente = bd.LerString("Cliente");
                        if (!String.IsNullOrEmpty(nomeCliente))
                            linha[CLIENTE] = bd.LerString("Cliente");
                        else
                            linha[CLIENTE] = bd.LerString("NomeFantasia");

                        linha[CLIENTEID] = bd.LerInt(CLIENTEID);
                        linha[EMAIL] = bd.LerString("Email");
                        linha[CANAL] = bd.LerString("Canal");
                        linha[VENDEDOR] = bd.LerString("Vendedor");
                        linha[SENHA] = bd.LerString("Senha");
                        linha[DATA_VENDA] = bd.LerDateTime("DataVenda");
                        linha[DATA_CAIXA] = bd.LerDateTime("DataCaixa");
                        linha[OBSERVACAO] = bd.LerString("Obs");
                        linha[NIVELRISCO] = bd.LerInt("NivelRisco");
                        linha[TAXA_ENTREGA_TIPO] = Entrega.TipoToEnum(bd.LerString("TaxaEntregaTipo"));
                        linha[ENTREGA_AGENDA_ID] = bd.LerInt("EntregaAgendaID");
                        linha[PAGAMENTO_PROCESSADO] = bd.LerBoolean(PAGAMENTO_PROCESSADO);
                        linha[TAXAPROCESSAMENTOCANCELADA] = bd.LerBoolean(TAXAPROCESSAMENTOCANCELADA);
                        linha[TAXAPROCESSAMENTOVALOR] = bd.LerDecimal(TAXAPROCESSAMENTOVALOR);
                        linha[VALORSEGURO] = bd.LerDecimal(VALORSEGURO);
                        linha[TRANSACTION_ID] = bd.LerString(TRANSACTION_ID);

                        info.Rows.Add(linha);

                    }
                    if(!conexaoExterna)
                        bd.Fechar();

                    string sqlIngressos = string.Format(@"SELECT tIngresso.VendaBilheteriaID, tIngresso.Status, LOG.ID, LOG.IngressoID, LOG.VendaBilheteriaItemID, tVendaBilheteria.Obs,
                                            tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacao.DisponivelAjuste, tSetor.Nome AS Setor, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tPreco.Nome AS Preco, tPreco.Valor, tIngresso.Codigo, tCortesia.Nome AS Cortesia, tBloqueio.Nome AS Bloqueio, tPacote.ID AS PacoteID, tPacote.Nome AS Pacote, tVendaBilheteriaItem.TaxaConveniencia, tVendaBilheteriaItem.TaxaConvenienciaValor, tVendaBilheteriaItem.TaxaComissao, tVendaBilheteriaItem.ComissaoValor, tVendaBilheteriaItem.PacoteGrupo,
                                            tEvento.TipoCodigoBarra, tIngresso.CodigoBarra, tIngresso.ApresentacaoSetorID, tIngresso.EventoID , tIngresso.AssinaturaClienteID, ISNULL(tAssinatura.Nome, '') as AssinaturaNome
                                            FROM tIngressolog AS LOG (NOLOCK)
                                            INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = LOG.IngressoID
                                            INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
                                            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
                                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                                            LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID
                                            LEFT JOIN tCortesia (NOLOCK) ON tCortesia.ID = tIngresso.CortesiaID
                                            LEFT JOIN tBloqueio (NOLOCK) ON tBloqueio.ID = tIngresso.BloqueioID
                                            INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID
                                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID
                                            LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID
                                            LEFT JOIN tAssinaturaCliente (NOLOCK) ON tAssinaturaCliente.ID = tIngresso.AssinaturaClienteID
                                            LEFT JOIN tAssinatura (NOLOCK) ON tAssinatura.ID = tAssinaturaCliente.AssinaturaID
                                            WHERE LOG.IngressoID in ({0}) AND LOG.VendaBilheteriaID = {1}", string.Join(",", ingressoIDs),vendaID);

                    bd.Consulta(sqlIngressos);

                    DataTable tabela = EstruturaTabela();

                    while (bd.Consulta().Read())
                    {
                        if (bd.LerInt("VendaBilheteriaID") == vendaID)
                        {

                            DataRow linha = tabela.NewRow();

                            linha[VENDA_BILHETERIA_ID] = vendaID;

                            linha[INGRESSO_ID] = bd.LerInt("IngressoID");
                            linha[INGRESSOLOG_ID] = bd.LerInt("ID");
                            linha[DISPONIVEL_AJUSTE] = bd.LerString("DisponivelAjuste");
                            linha[VENDA_BILHETERIA_ITEM_ID] = bd.LerInt("VendaBilheteriaItemID");

                            if (bd.LerInt("AssinaturaClienteID") > 0)
                            {
                                linha[EVENTO] = bd.LerString("AssinaturaNome");
                                linha[HORARIO] = "-";
                                linha[BLOQUEIO] = "-";
                                linha[ASSINATURA_CLIENTE_ID] = bd.LerInt("AssinaturaClienteID");
                            }
                            else
                            {
                                linha[ASSINATURA_CLIENTE_ID] = 0;
                                linha[EVENTO] = bd.LerString("Evento");
                                linha[HORARIO] = bd.LerStringFormatoDataHora("Horario");
                                if (bd.LerString("Bloqueio") != "")
                                    linha[BLOQUEIO] = bd.LerString("Bloqueio");
                            }

                            linha[PACOTE] = bd.LerString("Pacote");
                            linha[PACOTEID] = bd.LerInt("PacoteID");

                            linha[SETOR_PRODUTO] = bd.LerString("Setor");
                            linha[CODIGO] = bd.LerString("Codigo");
                            linha[PRECO] = bd.LerString("Preco");
                            if (bd.LerString("Cortesia") != "")
                                linha[CORTESIA] = bd.LerString("Cortesia");
                            linha[PRECOID] = bd.LerInt("PrecoID");
                            linha[CORTESIAID] = bd.LerInt("CortesiaID");
                            linha[BLOQUEIOID] = bd.LerInt("BloqueioID");
                            linha[VALOR] = bd.LerDecimal("Valor");
                            linha[STATUS] = bd.LerString("Status");
                            linha[CONV] = bd.LerInt("TaxaConveniencia");
                            linha[VALOR_CONV] = bd.LerDecimal("TaxaConvenienciaValor");
                            linha[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                            linha[COMISSAO_VALOR] = bd.LerDecimal("ComissaoValor");
                            linha[OBSERVACAO] = bd.LerString("Obs");
                            linha[PACOTE_GRUPO] = bd.LerInt("PacoteGrupo");
                            linha[TIPO_CODIGO_BARRA] = bd.LerString("TipoCodigoBarra");
                            linha[CODIGO_BARRA] = bd.LerString("CodigoBarra");
                            linha[EVENTO_ID] = bd.LerInt(EVENTO_ID);
                            linha[APRESENTACAO_SETOR_ID] = bd.LerInt(APRESENTACAO_SETOR_ID);
                            tabela.Rows.Add(linha);
                        }
                    }

                    DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(tabela, VENDA_BILHETERIA_ITEM_ID);

                    int pacoteIdAtual = 0;
                    int pacoteReservaID = 0;
                    int pacoteGrupo = 0;
                    int vBItemId = 0;
                    int assinaturaClienteID = 0;
                    List<int> lstAssinaturaClienteID = new List<int>();

                    foreach (DataRow linha in tabelaTmp.Rows)
                    {
                        int vendaBilheteriaItemID = (int)linha[VENDA_BILHETERIA_ITEM_ID];

                        DataRow[] linhas = tabela.Select(VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);

                        DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();
                        DataRow novoInfoPacote = retorno.Tables[TABELA_INFO_PACOTE].NewRow();

                        novoInfoPacote[PACOTE_GRUPO] = 0;

                        vBItemId = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                        novoItemGrid[VENDA_BILHETERIA_ITEM_ID] = vBItemId;

                        assinaturaClienteID = (int)linhas[0][ASSINATURA_CLIENTE_ID];

                        object valor = DBNull.Value;
                        object valorIndividual = DBNull.Value;
                        if (assinaturaClienteID > 0)
                        {
                            //soma valor Assinatura
                            valor = tabela.Compute("SUM(Valor)", ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                            novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                            valorIndividual = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                        }
                        else
                        {
                            //somar o valor
                            valor = tabela.Compute("SUM(Valor)", VENDA_BILHETERIA_ITEM_ID + "=" + vendaBilheteriaItemID);
                            novoItemGrid[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                        }

                        Pacote oP = new Pacote();

                        if (oP.CancelamentoAvulso((int)linhas[0][PACOTEID]))
                        {
                            novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                            novoItemGrid[TIPO] = TIPO_PACOTE;

                            novoInfoPacote[VALOR] = (valor != DBNull.Value) ? Convert.ToDecimal(valor) : 0;
                            novoInfoPacote[VENDA_BILHETERIA_ITEM_ID] = (int)linhas[0][VENDA_BILHETERIA_ITEM_ID];
                            novoInfoPacote[TIPO] = TIPO_INGRESSO;
                            if (linhas.Length > 1)
                            {
                                novoInfoPacote[TIPO_LUGAR] = Setor.MesaFechada;
                            }
                            else
                            {
                                novoInfoPacote[TIPO_LUGAR] = Setor.Pista;
                            }

                            novoInfoPacote[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                            novoInfoPacote[HORARIO] = (string)linhas[0][HORARIO];
                            novoInfoPacote[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                            novoInfoPacote[CODIGO] = (string)linhas[0][CODIGO];
                            if (linhas[0][CORTESIA] != DBNull.Value)
                                novoInfoPacote[CORTESIA] = (string)linhas[0][CORTESIA];
                            if (linhas[0][BLOQUEIO] != DBNull.Value)
                                novoInfoPacote[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                            novoInfoPacote[PRECO] = (string)linhas[0][PRECO];
                            novoInfoPacote[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoInfoPacote[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoInfoPacote[CONV] = (int)linhas[0][CONV];
                            novoInfoPacote[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                            novoInfoPacote[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoInfoPacote[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                            novoInfoPacote[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                            novoInfoPacote[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];

                            BD bdAux = new BD();

                            string sqlValor = string.Format(@"SELECT SUM(tPreco.Valor) as Valor,
                                                        tVendaBilheteriaItem.TaxaConveniencia,
                                                        SUM(tVendaBilheteriaItem.TaxaConvenienciaValor) as TaxaConvenienciaValor,
                                                        tVendaBilheteriaItem.TaxaComissao,
                                                        SUM(tVendaBilheteriaItem.ComissaoValor) as ComissaoValor
                                                        FROM tIngressolog AS LOG (NOLOCK)
                                                        LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = LOG.PrecoID
                                                        INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tVendaBilheteriaItem.ID = LOG.VendaBilheteriaItemID
                                                        INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID
                                                        LEFT JOIN tPacote (NOLOCK) ON tPacote.ID = tVendaBilheteriaItem.PacoteID
                                                        WHERE LOG.IngressoID in ({0}) AND LOG.VendaBilheteriaID = {1} AND  tVendaBilheteriaItem.PacoteID = {2} AND tVendaBilheteriaItem.PacoteGrupo = {3}
                                                        group by tVendaBilheteriaItem.TaxaConveniencia, tVendaBilheteriaItem.TaxaComissao", string.Join(",", ingressoIDs), vendaID, (int)linhas[0][PACOTEID], (int)linhas[0][PACOTE_GRUPO]);
                                
                            bdAux.Consulta(sqlValor);

                            while (bdAux.Consulta().Read())
                            {
                                novoItemGrid[CONV] = bdAux.LerInt("TaxaConveniencia");
                                novoItemGrid[VALOR_CONV] = bdAux.LerDecimal("TaxaConvenienciaValor");
                                novoItemGrid[TAXA_COMISSAO] = bdAux.LerInt("TaxaComissao");
                                novoItemGrid[COMISSAO_VALOR] = bdAux.LerDecimal("ComissaoValor");
                                novoItemGrid[VALOR] = bdAux.LerDecimal("Valor");
                            }

                            bdAux.Fechar();

                            novoItemGrid[PACOTE_COMPLETO] = true;
                            novoItemGrid[PACOTE_GRUPO] = (int)linhas[0][PACOTE_GRUPO];
                            novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoItemGrid[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;
                        }
                        else
                        {
                            if ((int)linhas[0][PACOTEID] != 0)
                            {
                                novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][PACOTE];
                                novoItemGrid[TIPO] = TIPO_PACOTE;
                            }
                            else
                            {
                                novoItemGrid[TIPO] = TIPO_INGRESSO;

                                if (linhas.Length > 1)
                                    novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                                else
                                    novoItemGrid[TIPO_LUGAR] = Setor.Pista;

                                novoItemGrid[EVENTO_PACOTE] = (string)linhas[0][EVENTO];
                                novoItemGrid[HORARIO] = (string)linhas[0][HORARIO];
                                novoItemGrid[SETOR_PRODUTO] = (string)linhas[0][SETOR_PRODUTO];
                                novoItemGrid[CODIGO] = (string)linhas[0][CODIGO];
                                if (linhas[0][CORTESIA] != DBNull.Value)
                                    novoItemGrid[CORTESIA] = (string)linhas[0][CORTESIA];
                                if (linhas[0][BLOQUEIO] != DBNull.Value)
                                    novoItemGrid[BLOQUEIO] = (string)linhas[0][BLOQUEIO];
                                novoItemGrid[PRECO] = (string)linhas[0][PRECO];
                            }
                            novoItemGrid[STATUS] = Ingresso.StatusDescritivo((string)linhas[0][STATUS]);
                            novoItemGrid[STATUS_DETALHADO] = Ingresso.StatusDetalhado(linhas[0][STATUS].ToString(), Convert.ToString(info.Rows[0][TAXA_ENTREGA_TIPO]));
                            novoItemGrid[CONV] = (int)linhas[0][CONV];
                            novoItemGrid[VALOR_CONV] = (decimal)linhas[0][VALOR_CONV];
                            novoItemGrid[DISPONIVEL_AJUSTE] = (string)linhas[0][DISPONIVEL_AJUSTE];
                            novoItemGrid[TAXA_COMISSAO] = (int)linhas[0][TAXA_COMISSAO];
                            novoItemGrid[COMISSAO_VALOR] = Convert.ToDecimal(linhas[0][COMISSAO_VALOR]);
                            novoItemGrid[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;

                        }
                        
                        if (pacoteIdAtual != oP.Control.ID)
                        {
                            pacoteGrupo = 0;
                        }

                        if (!lstAssinaturaClienteID.Contains(assinaturaClienteID) && (!oP.PermitirCancelamentoAvulso.Valor || pacoteGrupo != (int)novoInfoPacote[PACOTE_GRUPO]))
                        {
                            retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                            pacoteIdAtual = oP.Control.ID;
                            pacoteReservaID = (int)novoItemGrid[RESERVAID];
                            pacoteGrupo = (int)novoInfoPacote[PACOTE_GRUPO];

                            if (assinaturaClienteID > 0)
                                lstAssinaturaClienteID.Add(assinaturaClienteID);
                        }

                        if (assinaturaClienteID > 0)
                        {
                            DataRow[] dr = retorno.Tables[TABELA_RESERVA].Select(ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);
                            if (dr.Length > 0)
                            {
                                int possuiReservaID = (int)dr[0][RESERVAID];
                                if (possuiReservaID > 0)
                                    pacoteReservaID = possuiReservaID;
                            }

                            DataRow novoAssinatura = retorno.Tables[TABELA_INFO_ASSINATURA].NewRow();
                            for (int i = 0; i < novoItemGrid.ItemArray.Length; i++)
                            {
                                novoAssinatura[i] = novoItemGrid[i];
                            }

                            novoAssinatura[VALOR] = (valorIndividual != DBNull.Value) ? Convert.ToDecimal(valorIndividual) : 0;
                            novoAssinatura[RESERVAID] = pacoteReservaID;

                            retorno.Tables[TABELA_INFO_ASSINATURA].Rows.Add(novoAssinatura);
                        }

                        for (int i = 0; i < linhas.Length; i++)
                        {
                            DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                            novoItemReserva[RESERVAID] = oP.PermitirCancelamentoAvulso.Valor || assinaturaClienteID > 0 ? pacoteReservaID : (int)novoItemGrid[RESERVAID];
                            novoItemReserva[VENDA_BILHETERIA_ITEM_ID] = vBItemId;
                            novoItemReserva[INGRESSOID] = (int)linhas[i][INGRESSO_ID];
                            novoItemReserva[PACOTEID] = (int)linhas[i][PACOTEID];
                            novoItemReserva[PRECOID] = (int)linhas[i][PRECOID];
                            novoItemReserva[CORTESIAID] = (int)linhas[i][CORTESIAID];
                            novoItemReserva[BLOQUEIOID] = (int)linhas[i][BLOQUEIOID];
                            novoItemReserva[TIPO_CODIGO_BARRA] = linhas[i][TIPO_CODIGO_BARRA];
                            novoItemReserva[CODIGO_BARRA] = linhas[i][CODIGO_BARRA];
                            novoItemReserva[EVENTO_ID] = linhas[i][EVENTO_ID];
                            novoItemReserva[APRESENTACAO_SETOR_ID] = linhas[i][APRESENTACAO_SETOR_ID];
                            novoItemReserva[ASSINATURA_CLIENTE_ID] = assinaturaClienteID;

                            retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                        }


                        if (oP.PermitirCancelamentoAvulso.Valor)
                        {
                            novoInfoPacote[RESERVAID] = pacoteReservaID;
                            retorno.Tables[TABELA_INFO_PACOTE].Rows.Add(novoInfoPacote);
                        }
                    }

                    retorno.Tables.Add(info);

                    return retorno;



                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if(!conexaoExterna)
                        bd.Fechar();
                }
            }
            else
                return null;

        }

        /// <summary>
        /// Método que verifica se alguma aparesentacao do eveneto do pacote informado, j´passou.
        /// </summary>
        /// <param name="pacoteid"></param>
        /// <returns></returns>
        public bool VerificaEventoPassadoNoPacote(int pacoteid)
        {
            BD bd = null;

            try
            {
                bd = new BD();
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("select i.id from tingresso i (nolock) ");
                strBuilder.Append("inner join tApresentacao a (nolock) on a.id = i.ApresentacaoID ");
                strBuilder.Append(string.Format("where i.pacoteid = {0} ", pacoteid));
                strBuilder.Append("and dbo.StringToDateTime(a.horario) < getDate() ");

                return bd.QueryToTable(strBuilder.ToString()).Rows.Count > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable CarregaFormasPagamentoTroca(int vendaBilheteriaID)
        {
            string sql = string.Format(@"SELECT tVendaBilheteriaFormaPagamento.ID as VendaBilheteriaFormaPagamentoIDOrigem,
                                               FormaPagamentoID as ID, 
                                               Porcentagem,
	                                           Valor, 
	                                           CartaoID,
	                                           Dias,
	                                           TaxaAdm, 
	                                           IR,
	                                           dbo.StringToDateTime(DataDeposito) as DataDeposito,
	                                           CodigoRespostaVenda, 
	                                           MensagemRetorno,
	                                           HoraTransacao, 
	                                           DataTransacao, 
	                                           CodigoIR, 
	                                           NumeroAutorizacao,
	                                           NSUHost, 
	                                           NSUSitef,
	                                           Cupom,
	                                           DadosConfirmacaoVenda, 
	                                           Rede,
	                                           CodigoRespostaTransacao,
	                                           '' as FormaPagamento, 
	                                           ISNULL(tCartao.NroCartao, '') as NumeroCartao,
	                                           'True' as FormaPagamentoCredito,
		                                       ISNULL(tCartao.NomeCartao, '') AS NomeCartao,
		                                       ISNULL(tBandeira.Nome, '') AS Bandeira
  	                                      FROM tVendaBilheteriaFormaPagamento(NOLOCK)
                                     LEFT JOIN tCartao (NOLOCK) ON tCartao.ID = tVendaBilheteriaFormaPagamento.CartaoID
                                     LEFT JOIN tBandeira (NOLOCK) ON tBandeira.ID = tCartao.BandeiraID
                                         WHERE VendaBilheteriaID = {0}", vendaBilheteriaID);

            BD bd = null;
            try
            {
                bd = new BD();
                DataTable dados = new DataTable();
                dados = bd.QueryToTable(sql);
                return dados;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bd.Fechar();
            }

        }

        private bool ReservarIngressosTroca(EstruturaTrocaIngressoPreco estrutura, BD bd)
        {
            string sql = "";
            foreach (var item in estrutura.IngressosPrecos)
            {
                sql += string.Format(@"UPDATE tIngresso SET PrecoID = {0}, UsuarioID = {1}, Status = 'R', LojaID = {2}, ClienteID = {3} Where ID = {4} AND 1 = (SELECT CASE WHEN CAST(dbo.PrecoMesmoLote((SELECT TOP 1 tIngressoLog.PrecoID FROM tIngressoLog WHERE tIngressoLog.IngressoID = {4} AND tIngressoLog.Acao = 'V' ORDER BY ID DESC),{0}) AS INT) + CAST(dbo.PrecoLoteAtivo({0},1) AS INT) > 0 THEN 1 ELSE 0 END)"
                                        , item.Value
                                        , estrutura.DadosTrocaingressoCompra.usuarioID
                                        , estrutura.DadosTrocaingressoCompra.lojaID
                                        , estrutura.DadosTrocaingressoCompra.clienteID
                                        , item.Key);
            }

            try
            {
                int registros = bd.Executar(sql);
                return registros == estrutura.IngressosPrecos.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<EstruturaAssinaturaPreco> ListaPrecoPorApresentacao(int setorID, int apresentacaoID, int precoID,int canalID)
        {
            if (setorID > 0 && apresentacaoID > 0)
            {
                BD bd = null;
                try
                {
                    bd = new BD();
                    string strQuery = @"SELECT 
	                                        tPreco.ID
	                                        ,tPreco.Nome
	                                        ,tPreco.Valor
                                        FROM 
	                                        tPreco (NOLOCK)
	                                        LEFT JOIN tLote (NOLOCK) ON tPreco.LoteID = tLote.ID
	                                        INNER JOIN tCanalPreco (NOLOCK) ON tPreco.ID = tCanalPreco.PrecoID
                                        WHERE 
	                                        tPreco.ApresentacaoSetorID IN (SELECT 
										                                        ID 
									                                        FROM 
										                                        tApresentacaoSetor (NOLOCK) 
									                                        WHERE 
										                                        SetorID = @setorID 
										                                        AND ApresentacaoID = @apresentacaoID)
	                                        AND tpreco.ID != @precoID
	                                        AND tCanalPreco.CanalID = @canalID
	                                        AND (tPreco.LoteID IS NULL 
	                                        OR (tLote.Status = 'A' 
		                                        AND (tLote.DataLimite IS NULL OR tLote.DataLimite > GETDATE()) 
		                                        AND (tLote.Quantidade IS NULL OR tLote.Quantidade > (SELECT COUNT(ID) FROM tIngresso(NOLOCK) WHERE PrecoID = tPreco.ID AND Status IN ('V', 'R', 'I')))))";

                    List<SqlParameter> parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter() { ParameterName = "@SetorID", Value = setorID, DbType = DbType.Int32 });
                    parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoID", Value = apresentacaoID, DbType = DbType.Int32 });
                    parametros.Add(new SqlParameter() { ParameterName = "@precoID", Value = precoID, DbType = DbType.Int32 });
                    parametros.Add(new SqlParameter() { ParameterName = "@canalID", Value = canalID, DbType = DbType.Int32 });
                    

                    

                    var dtRetorno = bd.QueryToTable(strQuery, parametros);

                    List<EstruturaAssinaturaPreco> retorno = new List<EstruturaAssinaturaPreco>();
                    EstruturaAssinaturaPreco _EstruturaAssinaturaPreco;
                    if (dtRetorno != null && dtRetorno.Rows.Count > 0)
                        for (int i = 0; i < dtRetorno.Rows.Count; i++)
                        {
                            decimal conversao = 0;
                            _EstruturaAssinaturaPreco = new EstruturaAssinaturaPreco();
                            _EstruturaAssinaturaPreco.ID = dtRetorno.Rows[i]["ID"].ToString() != null ? int.Parse(dtRetorno.Rows[i]["ID"].ToString()) : 0;
                            _EstruturaAssinaturaPreco.Nome = dtRetorno.Rows[i]["Nome"].ToString();
                            _EstruturaAssinaturaPreco.Valor = decimal.TryParse(dtRetorno.Rows[i]["Valor"].ToString(), out conversao) ? conversao : 0;

                            retorno.Add(_EstruturaAssinaturaPreco);
                        }

                    return retorno;
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                finally
                {
                    bd.Fechar();
                }
            }
            else
                return null;
        }

        public DataSet ListaComboBancosBandeiras()
        {
            DataSet retorno = null;
            BD bd = null;

            try
            {
                bd = new BD();
                string sql = @"SELECT ID, Codigo, NomeBanco, IRDeposita
                                 FROM ListaBancos(NOLOCK)
                                WHERE IRDeposita = 1 
                             ORDER BY NomeBanco ASC;
                              
                              SELECT * FROM tBandeira(nolock)
                              WHERE TEFID > 0
                              ORDER BY NOME ASC;";
                
                retorno = bd.QueryToDataSet(sql);
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
            return retorno;
        }

        public DataTable CalculaValoresTrocaIngresso(List<int> listIngressos, int vendaBilheteriaID, BD bd = null)
        {
            DataTable retorno = null;
            bool conexaoExterna = bd != null;
            try
            {
                if(bd == null)
                    bd = new BD();
                string sql = string.Format(@"SELECT 
	                                            SUM(tPreco.Valor - (CASE WHEN Cancel.IngressoEstornado = 1 THEN tPreco.Valor ELSE 0 END)) AS 'ValorIngressos'
	                                            ,(Case when tSetor.LugarMarcado = 'M' 
                                                       then tVendaBilheteriaItem.TaxaConvenienciaValor 
	                                                   else SUM(tVendaBilheteriaItem.TaxaConvenienciaValor - (CASE WHEN (Cancel.ConvenienciaEstornado + Cancel.IngressoEstornado) = 1 
																                                                   THEN tVendaBilheteriaItem.TaxaConvenienciaValor 
																                                                   ELSE 0 
															                                                  END))
                                                 end) AS 'ValorConveniencia'
	                                            ,tVendaBilheteria.ValorSeguro - (CASE WHEN MAX(Cancel.SeguroEstornado) = 1 THEN tVendaBilheteria.ValorSeguro ELSE 0 END) AS 'ValorSeguro'
	                                            ,tVendaBilheteria.TaxaEntregaValor - (CASE WHEN MAX(Cancel.EntregaEstornado) = 1 THEN tVendaBilheteria.TaxaEntregaValor ELSE 0 END) AS 'ValorEntrega'
	                                            ,(CASE WHEN (SELECT COUNT(Ing.ID) FROM tIngresso Ing (NOLOCK) WHERE Ing.VendaBilheteriaID = {1} AND Ing.ID NOT IN (SELECT Item FROM SplitString ('{0}', ','))) = 0 THEN 1 ELSE 0 END) AS TrocouTodos
	                                            ,(CASE WHEN (SELECT COUNT(ID) FROM tIngresso Ing (NOLOCK) WHERE Ing.VendaBilheteriaID = {1} AND Ing.ID IN ({0}) AND Ing.Status = 'I') > 0 THEN 1 ELSE 0 END) AS TemIngressoImpresso
                                            FROM 
	                                            tIngressoLog (NOLOCK) 
	                                            INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID
	                                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
	                                            INNER JOIN tVendaBilheteriaItem (NOLOCK) ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID AND tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID
	                                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                                                inner join tSetor (nolock) on tSetor.ID = tIngresso.SetorID
	                                            LEFT JOIN (SELECT 
					                                            IL.IngressoID
					                                            ,MAX(CASE WHEN CDP.VlrIngressoEstornado > 0 THEN 1 ELSE 0 END) AS IngressoEstornado
					                                            ,(CASE WHEN (SELECT COUNT(TCDP.ID) FROM tCancelDevolucaoPendente TCDP (NOLOCK) WHERE TCDP.VendaBilheteriaIDVenda = IL.VendaBilheteriaID AND TCDP.VlrTxEntregaEstornado > 0) > 0 THEN 1 ELSE 0 END) AS EntregaEstornado
					                                            ,MAX(CASE WHEN CDP.VlrTxConvenienciaEstornado > 0 THEN 1 ELSE 0 END) AS ConvenienciaEstornado
					                                            ,(CASE WHEN (SELECT COUNT(TCDP.ID) FROM tCancelDevolucaoPendente TCDP (NOLOCK) WHERE TCDP.VendaBilheteriaIDVenda = IL.VendaBilheteriaID AND TCDP.VlrSeguroEstornado > 0) > 0 THEN 1 ELSE 0 END) AS SeguroEstornado
				                                            FROM 
					                                            tIngressoLog IL (NOLOCK)
					                                            LEFT JOIN tCancelDevolucaoPendenteIngresso CDPI (NOLOCK) ON IL.IngressoID = CDPI.IngressoID
					                                            LEFT JOIN tCancelDevolucaoPendente CDP (NOLOCK) ON CDPI.CancelDevolucaoPendenteID = CDP.ID AND IL.VendaBilheteriaID = CDP.VendaBilheteriaIDVenda 
				                                            WHERE 
					                                            IL.VendaBilheteriaID = {1} 
                                                                AND CDP.StatusCancel IN ('A', 'D', 'P') 
				                                            GROUP BY 
					                                            IL.IngressoID
					                                            ,IL.VendaBilheteriaID) AS Cancel ON tIngresso.ID = Cancel.IngressoID
                                            WHERE 
	                                            tIngresso.ID IN ({0})
	                                            AND tVendaBilheteria.ID = {1}
                                            GROUP BY  
                                                tSetor.LugarMarcado,
                                                tVendaBilheteriaItem.TaxaConvenienciaValor,
	                                            tVendaBilheteria.ValorSeguro
	                                            ,tVendaBilheteria.TaxaEntregaValor", string.Join(",", listIngressos), vendaBilheteriaID);
                retorno = bd.QueryToTable(sql);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(!conexaoExterna)
                    bd.Fechar();
            }
            return retorno;
        }




        #region Estruturas Tables Cancelamento

        #region COLUNAS
        //colunas da GRID
        public const string ID = "ID";
        public const string TABELA_GRID = "Grid";
        public const string TABELA_RESERVA = "Reserva";
        public const string TABELA_INFO_PACOTE = "TabelaInfoPacote";
        public const string TABELA_INFO_ASSINATURA = "TabelaInfoAssinatura";

        public const string PRECOID = "PrecoID";
        public const string CORTESIAID = "CortesiaID";
        public const string BLOQUEIOID = "BloqueioID";
        
        public const string INGRESSOID = "IngressoID";
        public const string RESERVAID = "ReservaID";
        public const string EVENTO_PACOTE = "Evento/Pacote/Vale Ingresso";
        public const string EVENTO = "Evento";
        public const string PACOTE = "Pacote";
        public const string PACOTEID = "PacoteID";
        public const string HORARIO = "Horário";
        public const string SETOR_PRODUTO = "Setor/Produto";
        public const string CODIGO = "Cod";
        public const string PRECO = "Preço";
        public const string CORTESIA = "Cortesia";
        public const string BLOQUEIO = "Bloqueio";
        public const string VALOR = "Valor";
        public const string CONV = "Conv";
        public const string VALOR_CONV = "Valor Conv";
        public const string STATUS = "Status";
        public const string STATUS_DETALHADO = "StatusDetalhado";
        public const string TIPO = "Tipo";
        public const string TIPO_LUGAR = "TipoLugar";
        public const string TAXA_COMISSAO = "TaxaComissao";
        public const string COMISSAO_VALOR = "ComissaoValor";
        public const string PACOTE_GRUPO = "PacoteGrupo";
        public const string PACOTE_COMPLETO = "PacoteCompleto";
        public const string OBSERVACAO = "Obs";
        public const string NIVELRISCO = "NivelRisco";
        public const string CANCELADO = "Cancelado";
        public const string ENTREGA_AGENDA_ID = "EntregaAgendaID";
        public const string PAGAMENTO_PROCESSADO = "PagamentoProcessado";
        public const char TIPO_PACOTE = 'P'; //tipos de reserva
        public const char TIPO_INGRESSO = 'I';
        public const string TIPO_CODIGO_BARRA = "TipoCodigoBarra";
        public const string CODIGO_BARRA = "CodigoBarra";
        public const string EVENTO_ID = "EventoID";
        public const string APRESENTACAO_SETOR_ID = "ApresentacaoSetorID";
        public const string ASSINATURA_CLIENTE_ID = "AssinaturaClienteID";
        public const string TAXAPROCESSAMENTOVALOR = "TaxaProcessamentoValor";
        public const string TAXAPROCESSAMENTOCANCELADA = "TaxaProcessamentoCancelada";
        public const string VALORSEGURO = "ValorSeguro";
        //public const string ACAO = "Ação";
        public const string CLIENTE = "Cliente";
        public const string CLIENTEID = "ClienteID";

        public const string EMAIL = "Email";
        public const string CANAL = "Canal";
        public const string INFO_VENDA = "InfoVenda";
        public const string VENDEDOR = "Vendedor";
        public const string DATA_VENDA = "DataVenda";
        public const string DATA_CAIXA = "DataCaixa";
        public const string SENHA = "Senha";
        public const string STATUS_VENDA = "StatusVenda";
        public const string INGRESSO_ID = "IngressoID";
        public const string VENDA_BILHETERIA_ID = "VendaBilheteriaID";
        public const string VENDA_BILHETERIA_ITEM_ID = "VendaBilheteriaItemID";
        public const string INGRESSOLOG_ID = "IngressoLogID";
        public const string VALOR_TOTAL = "ValorTotal";
        public const string DISPONIVEL_AJUSTE = "DisponivelAjuste";
        public const string TAXA_ENTREGA_VALOR = "TaxaEntregaValor";
        public const string ENTREGA_CONTROLE_ID = "EntregaControleID";
        public const string TAXA_CONV_VALOR_TOTAL = "TaxaConvenienciaValorTotal";
        public const string TAXA_ENTREGA_TIPO = "TaxaEntregaTipo";
        public const string TRANSACTION_ID = "TransactionID";
        #endregion

        public static DataSet EstruturaGrid()
        {

            DataSet ds = new DataSet("Reservas");

            DataTable tGrid = new DataTable(TABELA_GRID);
            DataTable tInfoPacote = new DataTable(TABELA_INFO_PACOTE);
            DataTable tReserva = new DataTable(TABELA_RESERVA);
            DataTable tInfoAssinatura = new DataTable(TABELA_INFO_ASSINATURA);

            DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
            colGridReservaID.Unique = true;
            colGridReservaID.AutoIncrement = true;
            colGridReservaID.AutoIncrementStep = 1;
            colGridReservaID.AutoIncrementSeed = 1;

            tGrid.Columns.Add(colGridReservaID);
            //tGrid.Columns.Add(INGRESSO_ID, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(INGRESSOLOG_ID, typeof(int)).DefaultValue = 0;
            //tGrid.Columns.Add(VENDA_BILHETERIA_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TIPO, typeof(char)).DefaultValue = "-"; //guarda o tipo de reserva (ingresso ou pacote)
            tGrid.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-";//guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
            tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tGrid.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tGrid.Columns.Add(PACOTE_GRUPO, typeof(int));
            tGrid.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));


            tInfoPacote.Columns.Add(RESERVAID, typeof(int));
            tInfoPacote.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoPacote.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoPacote.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoPacote.Columns.Add(TIPO, typeof(char)).DefaultValue = "-"; //guarda o tipo de reserva (ingresso ou pacote)
            tInfoPacote.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-"; //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tInfoPacote.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoPacote.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoPacote.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tInfoPacote.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoPacote.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            tInfoAssinatura.Columns.Add(RESERVAID, typeof(int));
            tInfoAssinatura.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(EVENTO_PACOTE, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(DISPONIVEL_AJUSTE, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(HORARIO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(SETOR_PRODUTO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CODIGO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(PRECO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CORTESIA, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(BLOQUEIO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(VALOR, typeof(decimal));
            tInfoAssinatura.Columns.Add(STATUS, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(STATUS_DETALHADO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tInfoAssinatura.Columns.Add(CANCELADO, typeof(bool)).DefaultValue = false;
            tInfoAssinatura.Columns.Add(TIPO, typeof(char)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(TIPO_LUGAR, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(TAXA_COMISSAO, typeof(int));
            tInfoAssinatura.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tInfoAssinatura.Columns.Add(OBSERVACAO, typeof(string)).DefaultValue = "-";
            tInfoAssinatura.Columns.Add(PACOTE_GRUPO, typeof(int));
            tInfoAssinatura.Columns.Add(PACOTE_COMPLETO, typeof(bool)).DefaultValue = false;
            tInfoAssinatura.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            //TABELA_RESERVA
            tReserva.Columns.Add(RESERVAID, typeof(int));
            tReserva.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
            tReserva.Columns.Add(INGRESSOID, typeof(int));
            tReserva.Columns.Add(PACOTEID, typeof(int)); //Se for Pacote eh o ID do Pacote.
            tReserva.Columns.Add(PRECOID, typeof(int));
            tReserva.Columns.Add(CORTESIAID, typeof(int));
            tReserva.Columns.Add(BLOQUEIOID, typeof(int));
            tReserva.Columns.Add(TIPO_CODIGO_BARRA, typeof(string));
            tReserva.Columns.Add(CODIGO_BARRA, typeof(string));
            tReserva.Columns.Add(EVENTO_ID, typeof(int));
            tReserva.Columns.Add(APRESENTACAO_SETOR_ID, typeof(int));
            tReserva.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));

            ds.Tables.Add(tGrid);
            ds.Tables.Add(tReserva);
            ds.Tables.Add(tInfoPacote);
            ds.Tables.Add(tInfoAssinatura);
            //Grid com a Reserva
            DataColumn colReservaID = tReserva.Columns[RESERVAID];
            DataRelation dr1 = new DataRelation("GridXReserva", colGridReservaID, colReservaID, true);

            ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colGridReservaID, colReservaID);
            idKeyRestraint1.DeleteRule = Rule.Cascade;
            tReserva.Constraints.Add(idKeyRestraint1);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr1);

            //Grid com a InfoPacote
            DataColumn colInfoPacoteID = tInfoPacote.Columns[RESERVAID];
            DataRelation dr2 = new DataRelation("GridXInfoPacote", colGridReservaID, colInfoPacoteID, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colGridReservaID, colInfoPacoteID);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tInfoPacote.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr2);

            //Grid com a InfoAssinatura
            DataColumn colInfoAssinaturaID = tInfoAssinatura.Columns[RESERVAID];
            DataRelation dr3 = new DataRelation("GridXInfoAssinatura", colGridReservaID, colInfoAssinaturaID, true);

            ForeignKeyConstraint idKeyRestraint3 = new ForeignKeyConstraint(colGridReservaID, colInfoAssinaturaID);
            idKeyRestraint3.DeleteRule = Rule.Cascade;
            tInfoAssinatura.Constraints.Add(idKeyRestraint3);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr3);

            return ds;
        }

        public static DataTable EstruturaInfoVenda()
        {

            DataTable tGrid = new DataTable(INFO_VENDA);

            tGrid.Columns.Add("ID", typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(TAXA_CONV_VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(TAXA_ENTREGA_VALOR, typeof(decimal));
            tGrid.Columns.Add(ENTREGA_CONTROLE_ID, typeof(int));
            tGrid.Columns.Add(TAXA_ENTREGA_TIPO, typeof(string));
            tGrid.Columns.Add(VALOR_TOTAL, typeof(decimal));
            tGrid.Columns.Add(STATUS_VENDA, typeof(string));
            tGrid.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tGrid.Columns.Add(CLIENTE, typeof(string));
            tGrid.Columns.Add(CLIENTEID, typeof(int));
            tGrid.Columns.Add(EMAIL, typeof(string));
            tGrid.Columns.Add(CANAL, typeof(string));
            tGrid.Columns.Add(VENDEDOR, typeof(string));
            tGrid.Columns.Add(SENHA, typeof(string));
            tGrid.Columns.Add(DATA_VENDA, typeof(DateTime));
            tGrid.Columns.Add(DATA_CAIXA, typeof(DateTime));
            tGrid.Columns.Add(OBSERVACAO, typeof(string));
            tGrid.Columns.Add(NIVELRISCO, typeof(int));
            tGrid.Columns.Add(ENTREGA_AGENDA_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(PAGAMENTO_PROCESSADO, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(TAXAPROCESSAMENTOVALOR, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TAXAPROCESSAMENTOCANCELADA, typeof(bool)).DefaultValue = false;
            tGrid.Columns.Add(VALORSEGURO, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TRANSACTION_ID, typeof(string)).DefaultValue = string.Empty;

            return tGrid;
        }


        private DataTable EstruturaTabela()
        { //auxiliar

            DataTable tabela = new DataTable("Tabela");

            tabela.Columns.Add(INGRESSO_ID, typeof(int));
            tabela.Columns.Add(INGRESSOLOG_ID, typeof(int));
            tabela.Columns.Add(VENDA_BILHETERIA_ID, typeof(int));
            tabela.Columns.Add(VENDA_BILHETERIA_ITEM_ID, typeof(int));
            tabela.Columns.Add(EVENTO, typeof(string));
            tabela.Columns.Add(PACOTE, typeof(string));
            tabela.Columns.Add(PACOTEID, typeof(int));
            tabela.Columns.Add(HORARIO, typeof(string));
            tabela.Columns.Add(SETOR_PRODUTO, typeof(string));
            tabela.Columns.Add(CODIGO, typeof(string));
            tabela.Columns.Add(PRECO, typeof(string));
            tabela.Columns.Add(CORTESIA, typeof(string));
            tabela.Columns.Add(BLOQUEIO, typeof(string));
            tabela.Columns.Add(VALOR, typeof(decimal));
            tabela.Columns.Add(STATUS, typeof(string));
            tabela.Columns.Add(STATUS_DETALHADO, typeof(string));
            tabela.Columns.Add(CONV, typeof(int));
            tabela.Columns.Add(VALOR_CONV, typeof(decimal));
            tabela.Columns.Add(PRECOID, typeof(int));
            tabela.Columns.Add(CORTESIAID, typeof(int));
            tabela.Columns.Add(BLOQUEIOID, typeof(int));
            tabela.Columns.Add(DISPONIVEL_AJUSTE, typeof(string));
            tabela.Columns.Add(TAXA_COMISSAO, typeof(int));
            tabela.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tabela.Columns.Add(OBSERVACAO, typeof(string));
            tabela.Columns.Add(PACOTE_GRUPO, typeof(int));
            tabela.Columns.Add(TIPO_CODIGO_BARRA, typeof(string));
            tabela.Columns.Add(CODIGO_BARRA, typeof(string));
            tabela.Columns.Add(EVENTO_ID, typeof(int));
            tabela.Columns.Add(APRESENTACAO_SETOR_ID, typeof(int));
            tabela.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int));
            return tabela;
        }

        #endregion
    }
}
