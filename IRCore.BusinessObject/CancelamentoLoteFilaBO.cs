using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.IRServices;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using IRLib.CancelamentoIngresso;
using IRLib.Codigo;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class CancelamentoLoteFilaBO : MasterBO<CancelamentoLoteFilaADO>
    {
        public const int CancelSolicitadoCliente = 0;
        public const int CancelEventoCancelado = 1;
        public const int CancelEventoAdiado = 2;
        public const int CancelTaxasServicos = 3;
        public const int CancelExcecoesIR = 4;

        public CancelamentoLoteFilaBO(MasterADOBase ado) : base(ado) { }
        public CancelamentoLoteFilaBO() : base(null) { }

        public bool Atualizar(tCancelamentoLoteFila CancelamentoLoteFila)
        {
            return ado.Atualizar(CancelamentoLoteFila);
        }

        public bool AtualizarSituacaoZero(tCancelamentoLoteFila CancelamentoLoteFila)
        {
            return ado.AtualizarSituacaoZero(CancelamentoLoteFila);
        }

        public List<tCancelamentoLoteFila> MontarOperacoesCancelamento(int CancelamentoLoteID, List<int> ApresentacoesID)
        {
            List<tCancelamentoLoteFila> cancelamentoFila = new List<tCancelamentoLoteFila>();
            DateTime dataMovimentacao = DateTime.Now;
            foreach (OperacaoCancelamentoModelQuery ocmq in ado.MontarOperacoesCancelamento(ApresentacoesID))
            {
                tCancelamentoLoteFila clf = cancelamentoFila.Where(x => x.VendaBilheteriaID == ocmq.VendaBilheteriaID).FirstOrDefault();
                if (clf == null)
                {
                    clf = new tCancelamentoLoteFila();
                    clf.CancelamentoLoteID = CancelamentoLoteID;
                    clf.VendaBilheteriaID = ocmq.VendaBilheteriaID;
                    clf.CanalID = ocmq.CanalID;
                    clf.LojaID = ocmq.LojaID;
                    clf.DataMovimentacao = dataMovimentacao;
                    clf.Status = enumCancelamentoLoteFila.NaFila.ValueAsChar();
                    clf.Operacao = ocmq.Operacao;
                    clf.Ingressos = new List<tCancelamentoLoteFilaIngresso>();
                    cancelamentoFila.Add(clf);
                }
                clf.Ingressos.Add(new tCancelamentoLoteFilaIngresso()
                {
                    IngressoID = ocmq.IngressoID
                });
            }
            return cancelamentoFila;
        }

        public List<tCancelamentoLoteFila> ConsultarCancelamentosNaFila(int Quantidade)
        {
            List<tCancelamentoLoteFila> lista = ado.ConsultarCancelamentosNaFila(Quantidade);
            List<tCancelamentoLoteFila> opDE = lista.Where(i => i.Operacao == (char)enumOperacoesCancelamento.OperacaoD || i.Operacao == (char)enumOperacoesCancelamento.OperacaoE).ToList();
            IEnumerable<VerificarOperacoesModelQuery> opCheck = ado.VerificarAlteracaoOperacao(opDE);
            foreach(VerificarOperacoesModelQuery item in opCheck)
            {
                tCancelamentoLoteFila filaItem = opDE.Where(i => i.VendaBilheteriaID == item.VendaBilheteriaID).FirstOrDefault();
                if(filaItem.Operacao != item.Operacao)
                {
                    filaItem.Operacao = item.Operacao;
                    Atualizar(filaItem);
                }
            }
            foreach (tCancelamentoLoteFila item in lista.Where(i => i.Ingressos.Count == 0))
            {
                item.Operacao = (char)enumOperacoesCancelamento.OperacaoZero;
            }
            return lista;
        }

        public void PopularCancelamentoFila(List<tCancelamentoLoteFila> CancelamentoFila)
        {
            ado.PopularCancelamentoFila(CancelamentoFila);
        }

        public void EnvioEmailCancelamentoMassa(tCancelamentoLoteFila item, string atendente, string formaDevolucao)
        {
            IngressoBO iBO = new IngressoBO();
            CancelamentoLoteBO cBO = new CancelamentoLoteBO();
            List<tIngresso> ingressos = iBO.Listar(item.Ingressos.Select(x => x.IngressoID).ToList());

            List<CancelamentoLoteMailModel> mailModel = ado.CarregarCamposEmail(item.VendaBilheteriaID);            

            string motivo = cBO.Consultar(item.CancelamentoLoteID).MotivoCancelamento;

            MailServiceSoapClient service = new MailServiceSoapClient();
            ArrayOfString codIngressos = new ArrayOfString();
            codIngressos.AddRange(ingressos.Select(x => string.Format("<li>Ingresso: {0} - Evento: {1} - Data: {2}</li>",
                                                                                    x.Codigo, 
                                                                                    (x.tEvento != null) ? x.tEvento.Nome : string.Empty,
                                                                                    (x.tApresentacao != null) ? x.tApresentacao.HorarioAsDateTime.ToString("dd/MM/yyyy HH:mm") : string.Empty)
                                                                                    ).ToList());
            service.EnviarEmailCancelamentoMassa(mailModel[0].Cliente, mailModel[0].Email, mailModel[0].Senha, atendente, mailModel[0].Data.ToString("dd/MM/yyyy HH:mm"), mailModel[0].Canal, string.Join(",", mailModel.Select(x => x.Pagamento).ToList()), mailModel[0].Evento, motivo, formaDevolucao, codIngressos,mailModel[0].DescricaoEmail);
        }

        public void ExecutaCancelamentoAutomatico(enumOperacoesCancelamento Operacao, tCancelamentoLoteFila item, int caixaID, int empresaID, int localID, int usuarioID)
        {
            IngressoBO iBO = new IngressoBO();
            CTLib.BD db = new CTLib.BD();
            DbTransaction dbtrans = null;
            List<int> lstIngressosIDs = null;
            List<tIngresso> lstIngressos = null;
            CancelamentoIngressos cancelamento = new CancelamentoIngressos();
            EstruturaCancelamento dadosCancelamento = null;
            CancelamentoLoteModel cancelamentoLoteModel = new CancelamentoLoteModel();

            try
            {
                #region Inicia Transação DB
                db.Cnn = ado.con.conIngresso;
                dbtrans = ado.con.conIngresso.BeginTransaction();
                db.DefinnirEmTransacao(true,dbtrans);
                
                #endregion
                
                lstIngressosIDs = item.Ingressos.Select(x => x.IngressoID).ToList();
                lstIngressos = iBO.Listar(lstIngressosIDs);
                
                #region Resolução de pendencias
                if (ado.TemCancelamentoPendente(dbtrans, item.VendaBilheteriaID, lstIngressos))
                {
                    List<int> cancelados = new List<int>();
                    List<CancelamentoLoteIngressoPendenteDados> lstIngressosParaPendencia = new List<CancelamentoLoteIngressoPendenteDados>();

                    //Pegar lista de Ingresso pendentes
                    List<CancelamentoLoteIngressoPendente> lstIngressosPendentes = ado.ListarIngressosCancelamentoPendente(dbtrans, item.VendaBilheteriaID, lstIngressos);

                    //Executa cancelamento das pendencias
                    foreach (CancelamentoLoteIngressoPendente pendente in lstIngressosPendentes)
                    {
                        if (!cancelados.Contains(pendente.PendenciaID))
                        {
                            dadosCancelamento = cancelamento.CarregaDadosCancelamento(pendente.PendenciaID);
                            cancelados.Add(pendente.PendenciaID);

                            //TODO: validar dados da Devolucao
                            dadosCancelamento.CaixaIDDevolucao = caixaID;
                            dadosCancelamento.CanalIDDevolucao = item.CanalID;
                            dadosCancelamento.EmpresaIDDevolucao = empresaID;
                            dadosCancelamento.LocalIDDevolucao = localID;
                            dadosCancelamento.LojaIDDevolucao = item.LojaID;
                            dadosCancelamento.UsuarioIDDevolucao = usuarioID;

                            //Cancela a solicitação pendente
                            cancelamento.CancelarSolicitacao(dadosCancelamento, pendente.PendenciaID, db);
                        }

                        //Se o ingresso pendente nao esta na lista de ingressos para cancelar,
                        //este deve ser colocado em pendente novamente
                        if (!lstIngressosIDs.Contains(pendente.IngressoID))
                            lstIngressosParaPendencia.Add(new CancelamentoLoteIngressoPendenteDados()
                                {
                                    IngressoID = pendente.IngressoID,
                                    PendenciaID = pendente.PendenciaID,
                                    DadosCancelamento = dadosCancelamento
                                });
                    }

                    //percorre lista de ingressos que devem ficar com cancelamento pendente
                    foreach (int pendeteID in lstIngressosParaPendencia.Select(p => p.PendenciaID).Distinct().ToList())
                    {
                        CancelamentoLoteIngressoPendenteDados pend = lstIngressosParaPendencia.Where(p => p.PendenciaID == pendeteID).FirstOrDefault();
                      
                        #region Carrega a estrutura de cancelamento dos ingressos
                        //TODO: validar dados da Devolucao
                        cancelamentoLoteModel.CaixaID = caixaID;
                        cancelamentoLoteModel.LojaID = item.LojaID;
                        cancelamentoLoteModel.LocalID = localID;
                        cancelamentoLoteModel.CanalID = item.CanalID;
                        cancelamentoLoteModel.EmpresaID = empresaID;

                        cancelamentoLoteModel.EstornarConveniencia = pend.DadosCancelamento.ValorConvenienciaEstornada > 0;
                        cancelamentoLoteModel.EstornarEntrega = pend.DadosCancelamento.ValorEntregaEstornado > 0;
                        cancelamentoLoteModel.EstornarSeguro = pend.DadosCancelamento.ValorSeguroEstornado > 0;

                        cancelamentoLoteModel.TemDevolucao = true;

                        cancelamentoLoteModel.MotivoCancelamento = pend.DadosCancelamento.MotivoCancelamento;
                        cancelamentoLoteModel.SubMotivoCancelamento = pend.DadosCancelamento.SubMotivoCancelamento;

                        cancelamentoLoteModel.UsuarioID = usuarioID;
                        cancelamentoLoteModel.SupervisorID = usuarioID;

                        cancelamentoLoteModel.TipoCancelamento = pend.DadosCancelamento.TipoCancelamento;
                        cancelamentoLoteModel.FormaDevolucao = pend.DadosCancelamento.FormaDevolucao;

                        cancelamentoLoteModel.dadosBancarios = pend.DadosCancelamento.DadosDeposito;
                        cancelamentoLoteModel.dadosCartaoCredito = pend.DadosCancelamento.DadosEstornoCC;

                        cancelamentoLoteModel.IngressosID = lstIngressosParaPendencia.Where(p => p.PendenciaID == pendeteID).Select(p => p.IngressoID).ToList();
                        #endregion

                        //Carrega a estrutura de cancelamento dos ingressos
                        pend.DadosCancelamento = CarregarEstruturaCancelamento(cancelamentoLoteModel, dbtrans, db);

                        //Efetua a solicitação do Cancelamento
                        cancelamento.SolicitarCancelamento(pend.DadosCancelamento, db);
                    }
                } 
                #endregion
                
                #region Carrega a estrutura de cancelamento dos ingressos
                //TODO: validar dados da Devolucao
                cancelamentoLoteModel.CaixaID = caixaID;
                cancelamentoLoteModel.LojaID = item.LojaID;
                cancelamentoLoteModel.LocalID = localID;
                cancelamentoLoteModel.CanalID = item.CanalID;
                cancelamentoLoteModel.EmpresaID = empresaID;

                cancelamentoLoteModel.EstornarConveniencia = true;
                cancelamentoLoteModel.EstornarEntrega = ado.EstornarEntrega(dbtrans, item.VendaBilheteriaID, item.Ingressos.Count);
                cancelamentoLoteModel.EstornarSeguro = true;

                cancelamentoLoteModel.TemDevolucao = false;

                cancelamentoLoteModel.MotivoCancelamento = CancelEventoCancelado;
                cancelamentoLoteModel.SubMotivoCancelamento = 0;

                cancelamentoLoteModel.UsuarioID = usuarioID;
                cancelamentoLoteModel.SupervisorID = usuarioID;

                cancelamentoLoteModel.TipoCancelamento = EstruturaCancelamento.enuTipoCancelamento.Normal;
                cancelamentoLoteModel.FormaDevolucao = Operacao == enumOperacoesCancelamento.OperacaoE ?
                                                        EstruturaCancelamento.enuFormaDevolucao.PayPal :
                                                        EstruturaCancelamento.enuFormaDevolucao.EstornoCC;

                cancelamentoLoteModel.dadosBancarios = null;

                CancelamentoLoteDadosCliente cliente = ado.CarregarDadosCliente(dbtrans, item.VendaBilheteriaID);
                if (Operacao == enumOperacoesCancelamento.OperacaoE)
                {
                    cancelamentoLoteModel.dadosCartaoCredito = new EstruturaCancelamento.EstruturaDevolucaoEstornoCC()
                        {
                            Bandeira = "Paypal",
                            Email = cliente.Email,
                            NumeroCartao = "Paypal",
                            TitularCartao = cliente.Nome,
                            TitularCPF = cliente.CPF
                        };
                }
                else
                {
                    cancelamentoLoteModel.dadosCartaoCredito = new EstruturaCancelamento.EstruturaDevolucaoEstornoCC()
                    {
                        Bandeira = cliente.Bandeira,
                        Email = cliente.Email,
                        NumeroCartao = cliente.Cartao,
                        TitularCartao = cliente.Nome,
                        TitularCPF = cliente.CPF
                    };
                }

                cancelamentoLoteModel.IngressosID = lstIngressosIDs;
                #endregion

                //Carrega a estrutura de cancelamento dos ingressos
                dadosCancelamento = CarregarEstruturaCancelamento(cancelamentoLoteModel, dbtrans, db);

                if (Operacao == enumOperacoesCancelamento.OperacaoE)
                {
                    #region Executa estorno Paypal
                    IRLib.PayPal.CancelaPayPal paypal = new IRLib.PayPal.CancelaPayPal();
                    IRLib.VendaBilheteriaFormaPagamento vbfp = new IRLib.VendaBilheteriaFormaPagamento();

                    vbfp.LerPorVendaBilheteriaID(db, item.VendaBilheteriaID);
                    if (vbfp.Valor.Valor == dadosCancelamento.ValorEstornoTotal)
                    {
                        if (paypal.RefundPayPal(vbfp.TransactionID.Valor))
                            dadosCancelamento.EstornoEfetuado = true;
                        else
                            throw new Exception("Falha ao executar estorno com PayPal.");
                    }
                    else
                    {
                        if (paypal.RefundPartial(vbfp.TransactionID.Valor, dadosCancelamento.ValorEstornoTotal))
                            dadosCancelamento.EstornoEfetuado = true;
                        else
                            throw new Exception("Falha ao executar estorno parcial com PayPal.");
                    }
                    #endregion
                }

                //Efetua a solicitação do Cancelamento
                cancelamento.SolicitarCancelamento(dadosCancelamento, db);

                dbtrans.Commit();

            }
            catch (Exception ex)
            {
                if (dbtrans != null)
                    dbtrans.Rollback();

                throw ex;
            }
            finally
            {
                db.DefinnirEmTransacao(false);
            }
        }

        private EstruturaCancelamento CarregarEstruturaCancelamento(CancelamentoLoteModel cancelLoteModel, DbTransaction dbtrans, CTLib.BD bd)
        {
            CancelamentoLoteDadosCancelamento cancelLoteDados = ado.CarregarDadosVendaCancelamento(cancelLoteModel.IngressosID,dbtrans);
            CancelamentoLoteValoresPendentes cancelValores;
            Troca troca = new Troca();
            
            //Pendentes
            if(cancelLoteModel.TemDevolucao)
            {
                cancelValores = ado.CarregarValoresDadosPendentes(cancelLoteModel.IngressosID,dbtrans);

                if (!cancelLoteModel.EstornarEntrega)
                    cancelValores.EntregaValor = 0;
                if (!cancelLoteModel.EstornarSeguro)
                    cancelValores.SeguroValor = 0;
                if (!cancelLoteModel.EstornarConveniencia)
                    cancelValores.ConvenienciaValor = 0;
            }
            else
            {
                var dados = troca.CalculaValoresTrocaIngresso(cancelLoteModel.IngressosID, cancelLoteDados.VendaBilheteriaIDVenda,bd).AsEnumerable();
                cancelValores = new CancelamentoLoteValoresPendentes();
                cancelValores.EntregaValor = dados.Select(x=>x.Field<decimal>("ValorEntrega")).FirstOrDefault();
                cancelValores.SeguroValor = dados.Select(x => x.Field<decimal>("ValorSeguro")).FirstOrDefault();
                cancelValores.ConvenienciaValor = dados.Sum(x => x.Field<decimal>("ValorConveniencia"));
                cancelValores.IngressoValor = dados.Sum(x => x.Field<decimal>("ValorIngressos"));
            }
            DataSet dadosItensEIngressosVendidos = troca.MontaDadosItensEIngressosVendidosTrocaCredito(cancelLoteModel.IngressosID, bd);
            
            EstruturaCancelamento estruturaCancelamento = new EstruturaCancelamento
            {
                CaixaID = cancelLoteModel.CaixaID,
                LojaID = cancelLoteModel.LojaID,
                CanalID = cancelLoteModel.CanalID,
                UsuarioID = cancelLoteModel.UsuarioID,
                EmpresaID = cancelLoteModel.EmpresaID,
                ClienteID = cancelLoteDados.ClienteID,
                ValorEntregaEstornado = cancelValores.EntregaValor,
                EntregaControleID = cancelLoteModel.EstornarEntrega ? cancelLoteDados.EntregaControleID : 0,
                EntregaAgendaID = cancelLoteModel.EstornarEntrega ? cancelLoteDados.EntregaAgendaID : 0,
                SenhaVenda = cancelLoteDados.SenhaVenda,
                ValorConvenienciaEstornada = cancelValores.ConvenienciaValor,
                ValorIngressosEstornado = cancelValores.IngressoValor,
                ValorSeguroEstornado = cancelValores.SeguroValor,

                ValorConvenienciaTotal = cancelLoteDados.ValorConvenienciaTotal,
                ValorEntregaTotal = cancelLoteDados.ValorEntregaTotal,
                ValorIngressosTotal = cancelLoteDados.ValorIngressosTotal,
                ValorSeguroTotal = cancelLoteDados.ValorSeguroTotal,

                TipoCancelamento = cancelLoteModel.TipoCancelamento,
                CancelamentoFraude = cancelLoteDados.Fraude,
                VendaBilheteriaIDVenda = cancelLoteDados.VendaBilheteriaIDVenda,
                TemDevolucao = cancelLoteModel.TemDevolucao,
                EhCanalPresente = !(cancelLoteModel.TemDevolucao),
                SupervisorID = cancelLoteModel.SupervisorID,
                LocalID = cancelLoteModel.LocalID,
                MotivoCancelamento = cancelLoteModel.MotivoCancelamento,
                SubMotivoCancelamento = cancelLoteModel.SubMotivoCancelamento,
                DadosItensVendidos = dadosItensEIngressosVendidos.Tables["Grid"],
                DadosIngressosVendidos = dadosItensEIngressosVendidos.Tables["Reserva"],
                FormaDevolucao = cancelLoteModel.FormaDevolucao,
                DadosEstornoCC = cancelLoteModel.dadosCartaoCredito,
                DadosDeposito = cancelLoteModel.dadosBancarios
            };
            return estruturaCancelamento;
        }

        public void ReavaliarCancelamentosManuais()
        {
            List<tCancelamentoLoteFila> lista = ado.CarregarCancelamentoManuaisProcessados();
            foreach (tCancelamentoLoteFila item in lista.Where(i => i.Ingressos.Count == 0))
            {
                item.Status = (char)enumCancelamentoLoteFila.Cancelado;
                Atualizar(item);
            }
        }
    }
}
