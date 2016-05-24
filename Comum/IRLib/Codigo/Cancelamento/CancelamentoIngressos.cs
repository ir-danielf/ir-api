using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class CancelamentoIngressos : MarshalByRefObject
    {
        #region Constantes
        public const string TIPO = "Tipo";
        public const char TIPO_INGRESSO = 'I';
        public const string TIPO_LUGAR = "TipoLugar";
        public const char TIPO_PACOTE = 'P'; //tipos de reserva
        public const string ASSINATURA_CLIENTE_ID = "AssinaturaClienteID";
        public const string RESERVAID = "ReservaID";
        public const string TAXA_COMISSAO = "TaxaComissao";
        public const string COMISSAO_VALOR = "ComissaoValor";
        public const string CONV = "Conv";
        public const string VALOR_CONV = "Valor Conv";
        public const string CODIGO_BARRA = "CodigoBarra";
        public const string TIPO_CODIGO_BARRA = "TipoCodigoBarra";
        public const string EVENTOID = "EventoID";
        public const string APRESENTACAOSETORID = "ApresentacaoSetorID";
        public const string INGRESSOID = "IngressoID";
        public const string BLOQUEIOID = "BloqueioID";
        public const string CORTESIAID = "CortesiaID";
        public const string PRECOID = "PrecoID";
        public const string PACOTEID = "PacoteID";
        #endregion


        public EstruturaCancelamento SolicitarCancelamento(EstruturaCancelamento estrutura, BD bd = null, bool estornoParcial = false, List<int> ingressosNaoLiberarIds = null)
        {
            bool transacaoExterna = bd != null;
            EstruturaCancelamento retorno = null;
            VendaBilheteria vendaBilheteria = null;
            string sqlTemp = string.Empty;
            object ret = null;

            if (ingressosNaoLiberarIds == null)
                ingressosNaoLiberarIds = new List<int>();

            if (ingressosNaoLiberarIds.Count == 0)
                ingressosNaoLiberarIds.Add(0);

            try
            {
                if (estrutura.CaixaID == 0)
                    throw new CancelamentoException("Caixa não pode ser nulo.");

                if (estrutura.LojaID == 0)
                    throw new CancelamentoException("Loja nao pode ser nula.");

                if (estrutura.CanalID == 0)
                    throw new CancelamentoException("Canal nao pode ser nulo.");

                if (estrutura.UsuarioID == 0)
                    throw new CancelamentoException("Usuario nao pode ser nulo.");

                if (estrutura.EmpresaID == 0)
                    throw new CancelamentoException("Empresa nao pode ser nula.");

                try
                {
                    if (!transacaoExterna)
                    {
                        bd = new BD();

                        bd.IniciarTransacao();
                    }

                    //Se foi registrado chamado para o cancelamento, não deve registrar nada na VendaBilheteria
                    if (string.IsNullOrEmpty(estrutura.NumeroChamado))
                    {
                        //Se ficar pendente de devolução não deve registrar na VendaBilheteria
                        if (estrutura.TemDevolucao && !estrutura.EhCanalPresente)
                        {
                            //Pendente
                        }
                        else
                        {
                            vendaBilheteria = new VendaBilheteria();

                            vendaBilheteria.ClienteID.Valor = estrutura.ClienteID;
                            vendaBilheteria.CaixaID.Valor = estrutura.CaixaID;
                            vendaBilheteria.Status.Valor = VendaBilheteria.CANCELADO;
                            vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                            vendaBilheteria.NotaFiscalCliente.Valor = estrutura.NotaFiscalCliente;
                            vendaBilheteria.NotaFiscalEstabelecimento.Valor = estrutura.NotaFiscalEstabelecimento;

                            if (estrutura.ValorEntregaEstornado > 0m)
                            {
                                vendaBilheteria.EntregaControleID.Valor = estrutura.EntregaControleID;
                                vendaBilheteria.TaxaEntregaValor.Valor = estrutura.ValorEntregaEstornado;
                                vendaBilheteria.EntregaAgendaID.Valor = estrutura.EntregaAgendaID;
                                vendaBilheteria.EntregaCancelada.Valor = true;
                            }

                            vendaBilheteria.ValorTotal.Valor = estrutura.ValorEstornoTotal;
                            vendaBilheteria.ValorSeguro.Valor = estrutura.ValorSeguroEstornado;
                            vendaBilheteria.TaxaConvenienciaValorTotal.Valor = estrutura.ValorConvenienciaEstornada;
                            vendaBilheteria.SeguroCancelado.Valor = estrutura.ValorSeguroEstornado > 0m;
                            vendaBilheteria.ConvenienciaCancelada.Valor = estrutura.ValorConvenienciaEstornada > 0m;
                            vendaBilheteria.VendaCancelada.Valor = estrutura.TipoCancelamento != EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento;
                            vendaBilheteria.Fraude.Valor = estrutura.CancelamentoFraude ? 1 : 0;

                            vendaBilheteria.VendaBilheteriaIDOrigem.Valor = estrutura.VendaBilheteriaIDVenda;

                            sqlTemp = vendaBilheteria.StringInserir();
                            ret = bd.ConsultaValor(sqlTemp);
                            vendaBilheteria.Control.ID = (ret != null) ? Convert.ToInt32(ret) : 0;


                            if (vendaBilheteria.Control.ID == 0)
                                throw new BilheteriaException("Falha ao processar o Cancelamento.");

                            estrutura.VendaBilheteriaIDCancelamento = vendaBilheteria.Control.ID;
                        }
                    }
                    else
                    {
                        //Se foi registrado chamado para o cancelamento, deve utilizar o Id VendaBilheteria de venda
                        estrutura.VendaBilheteriaIDCancelamento = estrutura.VendaBilheteriaIDVenda;
                    }

                    sqlTemp = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + estrutura.VendaBilheteriaIDCancelamento;
                    ret = bd.ConsultaValor(sqlTemp);
                    estrutura.SenhaCancelamento = (ret != null) ? Convert.ToString(ret) : null;


                    CancelDevolucaoPendente CancelPendente = new CancelDevolucaoPendente();
                    CancelDevolucaoPendenteIngresso cancelPendIngresso = null;

                    CancelPendente.VendaBilheteriaIDVenda.Valor = estrutura.VendaBilheteriaIDVenda;
                    CancelPendente.VendaBilheteriaIDCancel.Valor = estrutura.VendaBilheteriaIDCancelamento;

                    //Se foi registrado chamado para o cancelamento, alterar o status da pendencia
                    if (string.IsNullOrEmpty(estrutura.NumeroChamado))
                    {
                        if (estrutura.TemDevolucao && !estrutura.EhCanalPresente && estrutura.TipoCancelamento != EstruturaCancelamento.enuTipoCancelamento.CancelamentoSemDevolucaoDinheiro)
                        {
                            estrutura.Status = EstruturaCancelamento.enuStatus.CancelamentoPendente;
                            CancelPendente.StatusCancel.Valor = CancelDevolucaoPendente.STATUS_CANCEL_PENDENTE;
                        }
                        else
                        {
                            estrutura.Status = EstruturaCancelamento.enuStatus.CancelamentoAutomatico;
                            CancelPendente.StatusCancel.Valor = CancelDevolucaoPendente.STATUS_CANCEL_AUTOMATICO;

                            if (estrutura.ValorEstornoTotal > 0)
                            {
                                #region Registra as Formas de pagamento de cancelamento
                                CancelamentoGerenciador cancelGer = new CancelamentoGerenciador();

                                //Busca as Formas de pagamento possiveis
                                DataTable pagamentos = cancelGer.FormasPagamento(estrutura.CanalID, estrutura.VendaBilheteriaIDVenda, estrutura.EmpresaID);
                                decimal porcentagemTotal = 0;



                                //Se for devolução em Dinheiro ou Deposito
                                if ((estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro ||
                                    estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito) && !estornoParcial)
                                {
                                    VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                                    DataRow row = null;
                                    if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro)
                                    {
                                        if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 1).Count() > 0)
                                            row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 1).First();
                                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = 1;
                                    }
                                    else
                                    {
                                        if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 9).Count() > 0)
                                            row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 9).First();
                                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = 9;
                                    }

                                    vendaBilheteriaFormaPagamento.Valor.Valor = estrutura.ValorEstornoTotal;
                                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = 100;
                                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = estrutura.VendaBilheteriaIDCancelamento;

                                    if (row != null)
                                    {
                                        vendaBilheteriaFormaPagamento.Dias.Valor = row["Dias"] != DBNull.Value ? (int)row["Dias"] : 0;
                                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = row["TaxaAdm"] != DBNull.Value ? (decimal)row["TaxaADm"] : 0.00m;
                                        vendaBilheteriaFormaPagamento.IR.Valor = row["IR"] != DBNull.Value && (string)row["IR"] == "T" ? true : false;
                                    }
                                    else
                                    {
                                        vendaBilheteriaFormaPagamento.Dias.Valor = 0;
                                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = 0.00m;
                                        vendaBilheteriaFormaPagamento.IR.Valor = false;
                                    }

                                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);

                                    string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                                    int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                                    bool ok = (x >= 1);
                                    if (!ok)
                                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
                                }
                                else
                                {
                                    //Busca as formas originais de pagamento para utilizar no cancelamento
                                    DataTable pagamentosOrigineis = vendaBilheteria.FormasPagamento(estrutura.VendaBilheteriaIDVenda);

                                    pagamentos.Columns.Add("Valor", typeof(decimal));
                                    DataTable pagamentoClone = pagamentos.Clone();
                                    decimal valorEstornoIntegral = (estornoParcial && estrutura.ValorEstornoparcial > 0) ? (estrutura.ValorEstornoTotal - estrutura.ValorEstornoparcial) : estrutura.ValorEstornoTotal;
                                    decimal valorEstornoDecremental = valorEstornoIntegral;


                                    #region Percorre os pagamentos originais para setar no cancelamento.
                                    for (int i = 0; i < pagamentosOrigineis.Rows.Count; i++)
                                    {
                                        DataRow original = pagamentosOrigineis.Rows[i];
                                        DataRow row = null;

                                        if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == Convert.ToInt32(original["ID"])).Count() > 0)
                                            row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == Convert.ToInt32(original["ID"])).First();
                                        else
                                        {
                                            row = pagamentoClone.NewRow();
                                            row["ID"] = original["ID"];
                                        }

                                        //Calcula o valor proporcional conforme a porcentagem na compra
                                        decimal porcentagemOriginal = Convert.ToDecimal(original["Porcentagem"]);
                                        decimal valorProporcional = Math.Round((valorEstornoIntegral * porcentagemOriginal) / 100m, 2);

                                        ///Caso o valor calculado seja superior ao resto deve usar o resto
                                        if (valorEstornoDecremental < valorProporcional)
                                            valorProporcional = valorEstornoDecremental;

                                        if (valorProporcional >= valorEstornoDecremental)
                                        {
                                            row["Valor"] = valorEstornoDecremental;
                                            valorEstornoDecremental = 0m;
                                        }
                                        else
                                        {
                                            row["Valor"] = valorProporcional;
                                            valorEstornoDecremental -= valorProporcional;
                                        }

                                        pagamentoClone.ImportRow(row);

                                        if (valorEstornoDecremental == 0)
                                            break;
                                        else if (valorEstornoDecremental < 0)
                                            throw new BilheteriaException("Forma de pagamento divergente, não foi cadastrada.");
                                    }

                                    if (estornoParcial && estrutura.ValorEstornoparcial > 0)
                                    {
                                        DataRow row = null;

                                        if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro)
                                        {
                                            if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 1).Count() > 0)
                                                row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 1).First();
                                            else
                                            {
                                                row = pagamentoClone.NewRow();
                                                row["ID"] = 1;
                                                row["Dias"] = 0;
                                                row["TaxaAdm"] = 0.00m;
                                                row["IR"] = "F";
                                            }
                                        }
                                        else if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito)
                                        {
                                            if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 9).Count() > 0)
                                                row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 9).First();
                                            else
                                            {
                                                row = pagamentoClone.NewRow();
                                                row["ID"] = 9;
                                                row["Dias"] = 0;
                                                row["TaxaAdm"] = 0.00m;
                                                row["IR"] = "F";
                                            }
                                        }
                                        else
                                        {
                                            if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == Convert.ToInt32(pagamentosOrigineis.Rows[0]["ID"])).Count() > 0)
                                                row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == Convert.ToInt32(pagamentosOrigineis.Rows[0]["ID"])).First();
                                            else
                                            {
                                                row = pagamentoClone.NewRow();
                                                row["ID"] = Convert.ToInt32(pagamentosOrigineis.Rows[0]["ID"]);
                                                row["Dias"] = 0;
                                                row["TaxaAdm"] = 0.00m;
                                                row["IR"] = "F";
                                            }
                                        }

                                        if (row != null)
                                        {
                                            row["Valor"] = estrutura.ValorEstornoparcial;
                                            if (string.IsNullOrWhiteSpace(row["Nome"].ToString()))
                                            {
                                                row["Nome"] = string.Empty;
                                                pagamentoClone.Rows.Add(row);
                                            }
                                            else
                                                pagamentoClone.ImportRow(row);
                                        }
                                        else
                                            throw new BilheteriaException("Forma de pagamento divergente, não foi cadastrada.");

                                    }
                                    #endregion

                                    #region Percorre a lista de forma de pagamento para uilização no cancelamento
                                    for (int i = 0; i < pagamentoClone.Rows.Count; i++)
                                    {
                                        DataRow pagto = pagamentoClone.Rows[i];
                                        VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];

                                        decimal valorFormaPagamento = 0;
                                        Decimal.TryParse(Convert.ToString(pagto["Valor"]), out valorFormaPagamento);
                                        vendaBilheteriaFormaPagamento.Valor.Valor = valorFormaPagamento > estrutura.ValorEstornoTotal ? estrutura.ValorEstornoTotal : valorFormaPagamento;

                                        //calcular porcentagem
                                        decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / estrutura.ValorEstornoTotal;
                                        decimal porcentagem = Math.Round(porc, 2);

                                        //a porcentagem final tem q dar 100%
                                        if (i != (pagamentoClone.Rows.Count - 1))
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
                                        vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                                        vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                        vendaBilheteriaFormaPagamento.Dias.Valor = pagto["Dias"] != DBNull.Value ? (int)pagto["Dias"] : 0;
                                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = pagto["TaxaAdm"] != DBNull.Value ? (decimal)pagto["TaxaADm"] : 0.00m;
                                        vendaBilheteriaFormaPagamento.IR.Valor = pagto["IR"] != DBNull.Value && (string)pagto["IR"] == "T" ? true : false;
                                        vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                                        string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                                        int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                                        bool ok = (x >= 1);
                                        if (!ok)
                                            throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
                                    }
                                    #endregion
                                }
                                // 
                                #endregion
                            }

                        }
                    }
                    else
                    {
                        estrutura.Status = EstruturaCancelamento.enuStatus.CancelamentoNaoAutorizado;
                        CancelPendente.StatusCancel.Valor = CancelDevolucaoPendente.STATUS_CANCEL_NAO_AUTORIZADO;
                    }

                    CancelPendente.SupervisorID.Valor = estrutura.SupervisorID;
                    CancelPendente.NumeroChamado.Valor = estrutura.NumeroChamado;

                    // Dados para restaurar "sessão" atual.
                    CancelPendente.CaixaID.Valor = estrutura.CaixaID;
                    CancelPendente.LocalID.Valor = estrutura.LocalID;
                    CancelPendente.LojaID.Valor = estrutura.LojaID;
                    CancelPendente.CanalID.Valor = estrutura.CanalID;
                    CancelPendente.UsuarioID.Valor = estrutura.UsuarioID;
                    CancelPendente.EmpresaID.Valor = estrutura.EmpresaID;
                    CancelPendente.TipoCancelamento.Valor = (int)estrutura.TipoCancelamento;
                    CancelPendente.FormaDevolucao.Valor = (int)estrutura.FormaDevolucao;
                    CancelPendente.MotivoCancelamento.Valor = estrutura.MotivoCancelamento;
                    CancelPendente.SubMotivoCancelamento.Valor = estrutura.SubMotivoCancelamento;
                    CancelPendente.VlrIngressoEstornado.Valor = estrutura.ValorIngressosEstornado;
                    CancelPendente.VlrSeguroEstornado.Valor = estrutura.ValorSeguroEstornado;
                    CancelPendente.VlrTxConvenienciaEstornado.Valor = estrutura.ValorConvenienciaEstornada;
                    CancelPendente.VlrTxEntregaEstornado.Valor = estrutura.ValorEntregaEstornado;

                    if (!CancelPendente.Inserir(bd))
                        throw new BilheteriaException("Falha ao registrar pendencia.");

                    estrutura.PendenciaID = CancelPendente.Control.ID;


                    sqlTemp = string.Empty;

                    DataRow[] itensNormais = estrutura.DadosItensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                    DataRow[] itensMesaFechada = estrutura.DadosItensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                    DataRow[] itensPacote = estrutura.DadosItensVendidos.Select(TIPO + "='" + TIPO_PACOTE + "'");

                    foreach (DataRow item in itensNormais)
                    {
                        DataRow[] ingressos;

                        if (item["VendaBilheteriaItemID"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(item["VendaBilheteriaItemID"]) > 0)
                                ingressos = estrutura.DadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "' AND VendaBilheteriaItemID = " + (int)item["VendaBilheteriaItemID"]);
                            else
                                ingressos = estrutura.DadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                        }
                        else
                            ingressos = estrutura.DadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                        cancelPendIngresso = new CancelDevolucaoPendenteIngresso();
                        cancelPendIngresso.CancelDevolucaoPendenteID.Valor = CancelPendente.Control.ID;
                        cancelPendIngresso.IngressoID.Valor = (int)ingressos[0][INGRESSOID];
                        sqlTemp += cancelPendIngresso.StringInserir() + Environment.NewLine;

                    }

                    #region Mesa fechada
                    foreach (DataRow item in itensMesaFechada)
                    {
                        DataRow[] ingressos = estrutura.DadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                        foreach (DataRow i in ingressos)
                        {
                            cancelPendIngresso = new CancelDevolucaoPendenteIngresso();
                            cancelPendIngresso.CancelDevolucaoPendenteID.Valor = CancelPendente.Control.ID;
                            cancelPendIngresso.IngressoID.Valor = (int)i[INGRESSOID];
                            sqlTemp += cancelPendIngresso.StringInserir() + Environment.NewLine;
                        }

                    }
                    #endregion

                    #region Pacotes
                    foreach (DataRow item in itensPacote)
                    {
                        DataRow[] ingressosPacote = estrutura.DadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                        foreach (DataRow i in ingressosPacote)
                        {
                            cancelPendIngresso = new CancelDevolucaoPendenteIngresso();
                            cancelPendIngresso.CancelDevolucaoPendenteID.Valor = CancelPendente.Control.ID;
                            cancelPendIngresso.IngressoID.Valor = (int)i[INGRESSOID];
                            sqlTemp += cancelPendIngresso.StringInserir() + Environment.NewLine;
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(sqlTemp))
                    {
                        ret = bd.ConsultaValor(sqlTemp);

                        if (ret == null)
                            throw new BilheteriaException("Falha ao registrar ingressos da solicitação.");
                    }


                    //Se foi registrado chamado para o cancelamento, não deve registrar nada de estorno
                    if (string.IsNullOrEmpty(estrutura.NumeroChamado))
                    {

                        CancelamentoIngressos cancela = new CancelamentoIngressos();
                        bool estornaVlrIngresso = estrutura.ValorIngressosEstornado > 0m;
                        bool cancelaIngressos = (estrutura.TipoCancelamento != EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento && estornaVlrIngresso) ||
                                                 estrutura.TipoCancelamento == EstruturaCancelamento.enuTipoCancelamento.CancelamentoSemDevolucaoDinheiro;
                        bool cancelouTodosIngressos = estrutura.ValorCompraTotal == estrutura.ValorEstornoTotal && cancelaIngressos;
                        bool cancelaTaxaEntrega = estrutura.ValorEntregaEstornado > 0m;
                        bool cancelaTaxaConv = estrutura.ValorConvenienciaEstornada > 0m;
                        bool cancelaTaxaProcessamento = false;
                        bool cancelaSeguro = estrutura.ValorSeguroEstornado > 0m;

                        if (estrutura.TemDevolucao && estrutura.EhCanalPresente)
                        {
                            cancela.EfetivarCancelamentoIngressos(bd, cancelaIngressos, cancelouTodosIngressos, estornaVlrIngresso, cancelaTaxaEntrega, cancelaTaxaConv, cancelaTaxaProcessamento,
                                                                  cancelaSeguro, estrutura.VendaBilheteriaIDVenda, estrutura.VendaBilheteriaIDCancelamento, estrutura.CancelamentoFraude, estrutura.EntregaAgendaID, estrutura.DadosItensVendidos,
                                                                  estrutura.UsuarioID, estrutura.DadosIngressosVendidos, estrutura.CaixaID, estrutura.LojaID, estrutura.CanalID, estrutura.EmpresaID, estrutura.ClienteID,
                                                                  estrutura.MotivoCancelamento, estrutura.SupervisorID, estrutura.SubMotivoCancelamento.ToString(), ingressosNaoLiberarIds);
                        }
                        else if (estrutura.TipoCancelamento == EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento)
                        {
                            cancela.EfetivarCancelamentoIngressos(bd, false, false, estornaVlrIngresso, cancelaTaxaEntrega, cancelaTaxaConv, cancelaTaxaProcessamento,
                                                                  cancelaSeguro, estrutura.VendaBilheteriaIDVenda, estrutura.VendaBilheteriaIDCancelamento, estrutura.CancelamentoFraude, estrutura.EntregaAgendaID, estrutura.DadosItensVendidos,
                                                                  estrutura.UsuarioID, estrutura.DadosIngressosVendidos, estrutura.CaixaID, estrutura.LojaID, estrutura.CanalID, estrutura.EmpresaID, estrutura.ClienteID,
                                                                  estrutura.MotivoCancelamento, estrutura.SupervisorID, estrutura.SubMotivoCancelamento.ToString(), ingressosNaoLiberarIds);
                        }
                        else if (estrutura.TipoCancelamento == EstruturaCancelamento.enuTipoCancelamento.CancelamentoSemDevolucaoDinheiro)
                        {
                            cancela.EfetivarCancelamentoIngressos(bd, cancelaIngressos, cancelouTodosIngressos, estornaVlrIngresso, cancelaTaxaEntrega, cancelaTaxaConv, cancelaTaxaProcessamento,
                                                                  cancelaSeguro, estrutura.VendaBilheteriaIDVenda, estrutura.VendaBilheteriaIDCancelamento, estrutura.CancelamentoFraude, estrutura.EntregaAgendaID, estrutura.DadosItensVendidos,
                                                                  estrutura.UsuarioID, estrutura.DadosIngressosVendidos, estrutura.CaixaID, estrutura.LojaID, estrutura.CanalID, estrutura.EmpresaID, estrutura.ClienteID,
                                                                  estrutura.MotivoCancelamento, estrutura.SupervisorID, estrutura.SubMotivoCancelamento.ToString(), ingressosNaoLiberarIds);

                        }
                        else if (!estrutura.TemDevolucao)
                        {
                            cancela.EfetivarCancelamentoIngressos(bd, cancelaIngressos, cancelouTodosIngressos, estornaVlrIngresso, cancelaTaxaEntrega, cancelaTaxaConv, cancelaTaxaProcessamento,
                                                                  cancelaSeguro, estrutura.VendaBilheteriaIDVenda, estrutura.VendaBilheteriaIDCancelamento, estrutura.CancelamentoFraude, estrutura.EntregaAgendaID, estrutura.DadosItensVendidos,
                                                                  estrutura.UsuarioID, estrutura.DadosIngressosVendidos, estrutura.CaixaID, estrutura.LojaID, estrutura.CanalID, estrutura.EmpresaID, estrutura.ClienteID,
                                                                  estrutura.MotivoCancelamento, estrutura.SupervisorID, estrutura.SubMotivoCancelamento.ToString(), ingressosNaoLiberarIds);

                        }


                        if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro)
                        {
                            EstornoDadosDinheiro dinheiro = new EstornoDadosDinheiro();

                            dinheiro.CancelDevolucaoPendenteID.Valor = CancelPendente.Control.ID;
                            dinheiro.VendaBilheteriaIDCancel.Valor = estrutura.VendaBilheteriaIDCancelamento;
                            dinheiro.VendaBilheteriaIDVenda.Valor = estrutura.VendaBilheteriaIDVenda;
                            dinheiro.Valor.Valor = estornoParcial ? estrutura.ValorEstornoparcial : estrutura.ValorEstornoTotal;
                            dinheiro.Cliente.Valor = estrutura.DadosDinheiro.Nome;
                            dinheiro.CancelamentoPor.Valor = estrutura.UsuarioID.ToString();
                            dinheiro.Email.Valor = estrutura.DadosDinheiro.Email;

                            estrutura.ClienteEmail = estrutura.DadosDinheiro.Email;
                            estrutura.ClienteNome = estrutura.DadosDinheiro.Nome;

                            sqlTemp = dinheiro.StringInserir();



                        }
                        else if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito)
                        {
                            EstornoDadosDepositoBancario deposito = new EstornoDadosDepositoBancario();

                            deposito.CancelDevolucaoPendenteID.Valor = CancelPendente.Control.ID;
                            deposito.VendaBilheteriaIDCancel.Valor = estrutura.VendaBilheteriaIDCancelamento;
                            deposito.VendaBilheteriaIDVenda.Valor = estrutura.VendaBilheteriaIDVenda;
                            deposito.DataDeposito.Valor = DateTime.Now;
                            deposito.Banco.Valor = estrutura.DadosDeposito.Banco;
                            deposito.Agencia.Valor = estrutura.DadosDeposito.Agencia;
                            deposito.Conta.Valor = estrutura.DadosDeposito.Conta;
                            deposito.Valor.Valor = estornoParcial ? estrutura.ValorEstornoparcial : estrutura.ValorEstornoTotal;
                            deposito.Digito.Valor = estrutura.DadosDeposito.Digito;
                            deposito.Cliente.Valor = estrutura.DadosDeposito.Correntista;
                            deposito.CPFCliente.Valor = estrutura.DadosDeposito.CPF;
                            deposito.NomeCorrentista.Valor = estrutura.DadosDeposito.Correntista;
                            deposito.CPFCorrentista.Valor = estrutura.DadosDeposito.CPF;
                            deposito.CancelamentoPor.Valor = estrutura.UsuarioID.ToString();
                            deposito.MotivoLancamentoManual.Valor = "";
                            deposito.ContaPoupanca.Valor = !estrutura.DadosDeposito.IsContaCorrente;
                            deposito.Email.Valor = estrutura.DadosDeposito.Email;

                            if (estrutura.EstornoEfetuado)
                                deposito.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_AUTOMATICO;
                            else if (estrutura.TemDevolucao && !estrutura.EhCanalPresente)
                                deposito.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_SOLICITADO;
                            else
                                deposito.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_PENDENTE;

                            estrutura.ClienteEmail = estrutura.DadosDeposito.Email;
                            estrutura.ClienteNome = estrutura.DadosDeposito.Correntista;

                            sqlTemp = deposito.StringInserir();
                        }
                        else if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.EstornoCC)
                        {
                            EstornoDadosCartaoCredito cartao = new EstornoDadosCartaoCredito();

                            cartao.CancelDevolucaoPendenteID.Valor = CancelPendente.Control.ID;
                            cartao.VendaBilheteriaIDCancel.Valor = estrutura.VendaBilheteriaIDCancelamento;
                            cartao.VendaBilheteriaIDVenda.Valor = estrutura.VendaBilheteriaIDVenda;
                            cartao.Bandeira.Valor = estrutura.DadosEstornoCC.Bandeira;
                            cartao.Cartao.Valor = estrutura.DadosEstornoCC.NumeroCartao;
                            cartao.Valor.Valor = estornoParcial ? estrutura.ValorEstornoparcial : estrutura.ValorEstornoTotal;
                            cartao.Cliente.Valor = estrutura.DadosEstornoCC.TitularCartao;
                            cartao.CPFCliente.Valor = estrutura.DadosEstornoCC.TitularCPF;
                            cartao.CancelamentoPor.Valor = estrutura.UsuarioID.ToString();
                            cartao.Email.Valor = estrutura.DadosEstornoCC.Email;

                            if (estrutura.EstornoEfetuado)
                                cartao.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_AUTOMATICO;
                            else if (estrutura.TemDevolucao && !estrutura.EhCanalPresente)
                                cartao.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_SOLICITADO;
                            else
                                cartao.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_PENDENTE;

                            estrutura.ClienteEmail = estrutura.DadosEstornoCC.Email;
                            estrutura.ClienteNome = estrutura.DadosEstornoCC.TitularCartao;

                            sqlTemp = cartao.StringInserir();
                        }
                        else if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.PayPal)
                        {
                            EstornoDadosCartaoCredito cartao = new EstornoDadosCartaoCredito();
                            Cliente cliente = new Cliente();

                            cliente.Ler(estrutura.ClienteID);
                            cartao.VendaBilheteriaIDCancel.Valor = estrutura.VendaBilheteriaIDCancelamento;
                            cartao.VendaBilheteriaIDVenda.Valor = estrutura.VendaBilheteriaIDVenda;
                            cartao.Bandeira.Valor = "PayPal";
                            cartao.Cartao.Valor = "PayPal";
                            cartao.Valor.Valor = estornoParcial ? estrutura.ValorEstornoparcial : estrutura.ValorEstornoTotal;
                            cartao.Cliente.Valor = cliente.Nome.Valor;
                            cartao.CPFCliente.Valor = cliente.CPF.Valor;
                            cartao.CancelamentoPor.Valor = estrutura.UsuarioID.ToString();
                            cartao.Email.Valor = cliente.Email.Valor;

                            cartao.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_AUTOMATICO;

                            estrutura.ClienteEmail = cliente.Email.Valor;
                            estrutura.ClienteNome = cliente.Nome.Valor;

                            sqlTemp = cartao.StringInserir();
                        }

                        if ((estrutura.FormaDevolucao != EstruturaCancelamento.enuFormaDevolucao.SemDevolucao))
                        {
                            ret = bd.ConsultaValor(sqlTemp);

                            if (ret == null)
                                throw new BilheteriaException("Falha ao registrar informações de cancelamento.");
                        }
                    }

                    if (!transacaoExterna)
                        bd.FinalizarTransacao();

                    retorno = estrutura;
                }
                catch (Exception ex)
                {
                    if (!transacaoExterna)
                        bd.DesfazerTransacao();
                    throw ex;
                }
                finally
                {
                    if (!transacaoExterna)
                        bd.Fechar();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }


        public EstruturaCancelamento CarregaDadosCancelamento(int pendenciaID)
        {
            EstruturaCancelamento retorno = null;
            CancelDevolucaoPendente pendencia = null;
            VendaBilheteria vendaBilheteria_venda = null;
            VendaBilheteria vendaBilheteria_cancel = null;

            try
            {
                retorno = new EstruturaCancelamento();
                pendencia = new CancelDevolucaoPendente();
                vendaBilheteria_venda = new VendaBilheteria();
                vendaBilheteria_cancel = new VendaBilheteria();

                pendencia.Ler(pendenciaID);
                vendaBilheteria_venda.Ler(pendencia.VendaBilheteriaIDVenda.Valor);
                vendaBilheteria_cancel.Ler(pendencia.VendaBilheteriaIDCancel.Valor);

                retorno.PendenciaID = pendenciaID;
                retorno.VendaBilheteriaIDVenda = pendencia.VendaBilheteriaIDVenda.Valor;
                retorno.VendaBilheteriaIDCancelamento = pendencia.VendaBilheteriaIDCancel.Valor;
                retorno.SenhaCancelamento = vendaBilheteria_cancel.Senha.Valor;
                retorno.SenhaVenda = vendaBilheteria_venda.Senha.Valor;
                retorno.MotivoCancelamento = pendencia.MotivoCancelamento.Valor;
                retorno.SubMotivoCancelamento = pendencia.SubMotivoCancelamento.Valor;
                retorno.ValorIngressosEstornado = pendencia.VlrIngressoEstornado.Valor;
                retorno.ValorSeguroEstornado = pendencia.VlrSeguroEstornado.Valor;
                retorno.ValorConvenienciaEstornada = pendencia.VlrTxConvenienciaEstornado.Valor;
                retorno.ValorEntregaEstornado = pendencia.VlrTxEntregaEstornado.Valor;
                retorno.TipoCancelamento = (pendencia.TipoCancelamento.Valor == 0 ?
                                            EstruturaCancelamento.enuTipoCancelamento.Normal :
                                            (pendencia.TipoCancelamento.Valor == 1 ?
                                            EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento :
                                            EstruturaCancelamento.enuTipoCancelamento.CancelamentoSemDevolucaoDinheiro));
                retorno.CancelamentoFraude = vendaBilheteria_venda.Status.Valor == VendaBilheteria.FRAUDE;
                retorno.EntregaControleID = vendaBilheteria_venda.EntregaControleID.Valor;
                retorno.EntregaAgendaID = vendaBilheteria_venda.EntregaAgendaID.Valor;

                switch (pendencia.StatusCancel.Valor)
                {
                    case CancelDevolucaoPendente.STATUS_CANCEL_PENDENTE:
                        retorno.Status = EstruturaCancelamento.enuStatus.CancelamentoPendente;
                        break;
                    case CancelDevolucaoPendente.STATUS_CANCEL_AUTOMATICO:
                        retorno.Status = EstruturaCancelamento.enuStatus.CancelamentoAutomatico;
                        break;
                    case CancelDevolucaoPendente.STATUS_CANCEL_CANCELADO:
                        retorno.Status = EstruturaCancelamento.enuStatus.CancelamentoCancelado;
                        break;
                    case CancelDevolucaoPendente.STATUS_CANCEL_NAO_AUTORIZADO:
                        retorno.Status = EstruturaCancelamento.enuStatus.CancelamentoNaoAutorizado;
                        break;
                    case CancelDevolucaoPendente.STATUS_CANCEL_PROCESSADO:
                        retorno.Status = EstruturaCancelamento.enuStatus.CancelamentoProcessado;
                        break;
                }

                retorno.EstornoEfetuado = false;

                switch (pendencia.FormaDevolucao.Valor)
                {
                    case (int)EstruturaCancelamento.enuFormaDevolucao.Deposito:
                        retorno.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.Deposito;
                        break;
                    case (int)EstruturaCancelamento.enuFormaDevolucao.Dinheiro:
                        retorno.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.Dinheiro;
                        break;
                    case (int)EstruturaCancelamento.enuFormaDevolucao.EstornoCC:
                        retorno.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.EstornoCC;
                        break;
                    case (int)EstruturaCancelamento.enuFormaDevolucao.SemDevolucao:
                        retorno.FormaDevolucao = EstruturaCancelamento.enuFormaDevolucao.SemDevolucao;
                        break;
                }

                retorno.PendenciaID = pendencia.Control.ID;

                if (retorno.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro)
                {
                    EstornoDadosDinheiro dinheiro = new EstornoDadosDinheiro();
                    dinheiro.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    retorno.DadosDinheiro = new EstruturaCancelamento.EstruturaDevolucaoDinheiro();

                    retorno.DadosDinheiro.Nome = dinheiro.Cliente.Valor;
                    retorno.DadosDinheiro.Email = dinheiro.Email.Valor;

                    retorno.ClienteEmail = retorno.DadosDinheiro.Email;
                    retorno.ClienteNome = retorno.DadosDinheiro.Nome;
                }
                else if (retorno.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito)
                {
                    EstornoDadosDepositoBancario deposito = new EstornoDadosDepositoBancario();
                    deposito.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    retorno.DadosDeposito = new EstruturaCancelamento.EstruturaDevolucaoDeposito();

                    retorno.DadosDeposito.Banco = deposito.Banco.Valor;
                    retorno.DadosDeposito.Agencia = deposito.Agencia.Valor;
                    retorno.DadosDeposito.Conta = deposito.Conta.Valor;
                    retorno.DadosDeposito.Correntista = deposito.Cliente.Valor;
                    retorno.DadosDeposito.CPF = deposito.CPFCliente.Valor;
                    retorno.DadosDeposito.Correntista = deposito.NomeCorrentista.Valor;
                    retorno.DadosDeposito.CPF = deposito.CPFCorrentista.Valor;
                    retorno.DadosDeposito.IsContaCorrente = !deposito.ContaPoupanca.Valor;
                    retorno.DadosDeposito.Email = deposito.Email.Valor;

                    retorno.ClienteEmail = retorno.DadosDeposito.Email;
                    retorno.ClienteNome = retorno.DadosDeposito.Correntista;
                }
                else if (retorno.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.EstornoCC)
                {
                    EstornoDadosCartaoCredito cartao = new EstornoDadosCartaoCredito();
                    cartao.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    retorno.DadosEstornoCC = new EstruturaCancelamento.EstruturaDevolucaoEstornoCC();

                    retorno.DadosEstornoCC.Bandeira = cartao.Bandeira.Valor;
                    retorno.DadosEstornoCC.NumeroCartao = cartao.Cartao.Valor;
                    retorno.DadosEstornoCC.TitularCartao = cartao.Cliente.Valor;
                    retorno.DadosEstornoCC.TitularCPF = cartao.CPFCliente.Valor;
                    retorno.DadosEstornoCC.Email = cartao.Email.Valor;

                    retorno.ClienteEmail = retorno.DadosEstornoCC.Email;
                    retorno.ClienteNome = retorno.DadosEstornoCC.TitularCartao;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return retorno;
        }

        public bool EfetivaCancelamento(EstruturaCancelamento estrutura, int pendenciaID, DataTable dadosIngressos)
        {
            bool retorno = false;
            BD bd = null;
            CancelamentoIngressos cancela = null;
            CancelamentoGerenciador cancelGer = null;
            CancelDevolucaoPendente pendencia = null;
            VendaBilheteria vendaBilheteria_venda = null;
            VendaBilheteria vendaBilheteria_cancel = null;

            try
            {
                cancela = new CancelamentoIngressos();
                pendencia = new CancelDevolucaoPendente();
                vendaBilheteria_venda = new VendaBilheteria();
                vendaBilheteria_cancel = new VendaBilheteria();
                cancelGer = new CancelamentoGerenciador();

                pendencia.Ler(pendenciaID);

                pendencia.CanalIDDevolucao.Valor = estrutura.CanalIDDevolucao;
                pendencia.CaixaIDDevolucao.Valor = estrutura.CaixaIDDevolucao;
                pendencia.LocalIDDevolucao.Valor = estrutura.LocalIDDevolucao;
                pendencia.LojaIDDevolucao.Valor = estrutura.LojaIDDevolucao;
                pendencia.EmpresaIDDevolucao.Valor = estrutura.EmpresaIDDevolucao;
                pendencia.UsuarioIDDevolucao.Valor = estrutura.UsuarioIDDevolucao;

                vendaBilheteria_venda.Ler(pendencia.VendaBilheteriaIDVenda.Valor);


                bd = new BD();
                bd.IniciarTransacao();

                //Registra VendaBilheteria de Cancelamento
                vendaBilheteria_cancel.ClienteID.Valor = vendaBilheteria_venda.ClienteID.Valor;
                vendaBilheteria_cancel.CaixaID.Valor = estrutura.CaixaIDDevolucao;
                vendaBilheteria_cancel.Status.Valor = VendaBilheteria.CANCELADO;
                vendaBilheteria_cancel.DataVenda.Valor = System.DateTime.Now;
                vendaBilheteria_cancel.NotaFiscalCliente.Valor = estrutura.NotaFiscalCliente;
                vendaBilheteria_cancel.NotaFiscalEstabelecimento.Valor = estrutura.NotaFiscalEstabelecimento;

                if (estrutura.ValorEntregaEstornado > 0m)
                {
                    vendaBilheteria_cancel.EntregaControleID.Valor = estrutura.EntregaControleID;
                    vendaBilheteria_cancel.TaxaEntregaValor.Valor = estrutura.ValorEntregaEstornado;
                    vendaBilheteria_cancel.EntregaAgendaID.Valor = estrutura.EntregaAgendaID;
                    vendaBilheteria_cancel.EntregaCancelada.Valor = true;
                }

                vendaBilheteria_cancel.ValorTotal.Valor = estrutura.ValorEstornoTotal;
                vendaBilheteria_cancel.ValorSeguro.Valor = estrutura.ValorSeguroEstornado;
                vendaBilheteria_cancel.TaxaConvenienciaValorTotal.Valor = estrutura.ValorConvenienciaEstornada;
                vendaBilheteria_cancel.SeguroCancelado.Valor = estrutura.ValorSeguroEstornado > 0m;
                vendaBilheteria_cancel.ConvenienciaCancelada.Valor = estrutura.ValorConvenienciaEstornada > 0m;
                vendaBilheteria_cancel.VendaCancelada.Valor = estrutura.TipoCancelamento != EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento;
                vendaBilheteria_cancel.Fraude.Valor = estrutura.CancelamentoFraude ? 1 : 0;

                vendaBilheteria_cancel.VendaBilheteriaIDOrigem.Valor = estrutura.VendaBilheteriaIDVenda;

                string sqlTemp = vendaBilheteria_cancel.StringInserir();
                object ret = bd.ConsultaValor(sqlTemp);
                vendaBilheteria_cancel.Control.ID = (ret != null) ? Convert.ToInt32(ret) : 0;

                if (vendaBilheteria_cancel.Control.ID == 0)
                    throw new BilheteriaException("Falha ao processar o Cancelamento.");

                pendencia.VendaBilheteriaIDCancel.Valor = vendaBilheteria_cancel.Control.ID;
                estrutura.VendaBilheteriaIDCancelamento = vendaBilheteria_cancel.Control.ID;

                sqlTemp = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + estrutura.VendaBilheteriaIDCancelamento;
                ret = bd.ConsultaValor(sqlTemp);
                estrutura.SenhaCancelamento = (ret != null) ? Convert.ToString(ret) : null;

                bool cancelaIngressos = pendencia.TipoCancelamento.Valor != (int)EstruturaCancelamento.enuTipoCancelamento.DevolucaoDinheiroSemCancelamento;
                bool cancelaTaxaEntrega = pendencia.VlrTxEntregaEstornado.Valor > 0m;
                bool cancelaTaxaConv = pendencia.VlrTxConvenienciaEstornado.Valor > 0m;
                bool cancelaTaxaProcessamento = false;
                bool cancelaSeguro = pendencia.VlrSeguroEstornado.Valor > 0m;
                bool cancelouTodosIngressos = false;
                bool estornaVlrIngresso = pendencia.VlrIngressoEstornado.Valor > 0m;

                cancelGer.CaixaID = pendencia.CaixaID.Valor;
                cancelGer.CanalID = pendencia.CanalID.Valor;
                cancelGer.EmpresaID = pendencia.EmpresaID.Valor;
                cancelGer.LocalID = pendencia.LocalID.Valor;
                cancelGer.LojaID = pendencia.LojaID.Valor;
                cancelGer.PerfilID = Perfil.SAC_SUPERVISOR;
                cancelGer.UsuarioID = pendencia.UsuarioID.Valor;

                DataSet pesquisa = cancelGer.PesquisarVenda(pendencia.VendaBilheteriaIDVenda.Valor, false);

                DataTable tGrid = pesquisa.Tables[CancelamentoGerenciador.TABELA_GRID];
                DataTable tGridInfoPacote = pesquisa.Tables[CancelamentoGerenciador.TABELA_INFO_PACOTE];
                DataTable tGridInfoAssinatura = pesquisa.Tables[CancelamentoGerenciador.TABELA_INFO_ASSINATURA];
                DataTable tInfoVenda = pesquisa.Tables[CancelamentoGerenciador.INFO_VENDA];

                cancelouTodosIngressos = dadosIngressos.Rows.Count == tGrid.Rows.Count;

                ArrayList listaVendaBilheteriaItemIDs = new ArrayList(tGrid.Rows.Count);

                for (int i = 0; i < dadosIngressos.Rows.Count; i++)
                {
                    DataRow rpend = dadosIngressos.Rows[i];

                    for (int j = 0; j < tGrid.Rows.Count; j++)
                    {
                        DataRow row = tGrid.Rows[j];
                        if (row["PendenteCancelamento"].Equals(rpend["PendenciaIngressoID"]))
                        {
                            listaVendaBilheteriaItemIDs.Add((int)row["VendaBilheteriaItemID"]);
                            break;
                        }
                    }
                }

                listaVendaBilheteriaItemIDs.TrimToSize();


                if (listaVendaBilheteriaItemIDs.Count > 0)
                {
                    DataTable itensReservados = tGridInfoPacote.Clone();

                    DataRow[] itens = tGrid.Select(CancelamentoGerenciador.VENDA_BILHETERIA_ITEM_ID + " in (" + Utilitario.ArrayToString(listaVendaBilheteriaItemIDs) + ") AND ( AssinaturaClienteID <=0 OR AssinaturaClienteID is Null ) ");

                    foreach (DataRow item in itens)
                        itensReservados.ImportRow(item);

                    DataRow[] itensAssinatura = tGridInfoAssinatura.Select(CancelamentoGerenciador.VENDA_BILHETERIA_ITEM_ID + " in (" + Utilitario.ArrayToString(listaVendaBilheteriaItemIDs) + ")");

                    foreach (DataRow itemAss in itensAssinatura)
                        itensReservados.ImportRow(itemAss);

                    DataRow[] itensPacote = tGridInfoPacote.Select(CancelamentoGerenciador.VENDA_BILHETERIA_ITEM_ID + " in (" + Utilitario.ArrayToString(listaVendaBilheteriaItemIDs) + ")");

                    foreach (DataRow item in itensPacote)
                        itensReservados.ImportRow(item);

                    itensReservados.Columns.Remove(CancelamentoGerenciador.EVENTO_PACOTE);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.HORARIO);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.SETOR_PRODUTO);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.CODIGO);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.PRECO);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.CORTESIA);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.BLOQUEIO);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.VALOR);
                    itensReservados.Columns.Remove(CancelamentoGerenciador.CANCELADO);

                    DataTable tReserva = pesquisa.Tables[CancelamentoGerenciador.TABELA_RESERVA];


                    cancela.EfetivarCancelamentoIngressos(bd, cancelaIngressos, cancelouTodosIngressos, estornaVlrIngresso, cancelaTaxaEntrega, cancelaTaxaConv, cancelaTaxaProcessamento,
                                                          cancelaSeguro, pendencia.VendaBilheteriaIDVenda.Valor, pendencia.VendaBilheteriaIDCancel.Valor, vendaBilheteria_cancel.Fraude.Valor > 0,
                                                          vendaBilheteria_venda.EntregaAgendaID.Valor, itensReservados,
                                                          pendencia.UsuarioIDDevolucao.Valor, tReserva, pendencia.CaixaIDDevolucao.Valor, pendencia.LojaIDDevolucao.Valor,
                                                          pendencia.CanalIDDevolucao.Valor, pendencia.EmpresaIDDevolucao.Valor, vendaBilheteria_venda.ClienteID.Valor,
                                                          pendencia.MotivoCancelamento.Valor, pendencia.SupervisorID.Valor,
                                                          pendencia.SubMotivoCancelamento.Valor.ToString());


                }


                if (vendaBilheteria_cancel.ValorTotal.Valor > 0)
                {
                    #region Registra as Formas de pagamento de cancelamento
                    //Busca as Formas de pagamento possiveis
                    DataTable pagamentos = cancelGer.FormasPagamento(pendencia.CanalID.Valor, vendaBilheteria_cancel.Control.ID, pendencia.EmpresaID.Valor);
                    decimal porcentagemTotal = 0;

                    //Se for devolução em Dinheiro ou Deposito
                    if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro ||
                        estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito)
                    {
                        VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                        DataRow row = null;
                        if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Dinheiro)
                        {
                            if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 1).Count() > 0)
                                row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 1).First();
                            vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = 1;
                        }
                        else
                        {
                            if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 9).Count() > 0)
                                row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == 9).First();
                            vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = 9;
                        }

                        vendaBilheteriaFormaPagamento.Valor.Valor = vendaBilheteria_cancel.ValorTotal.Valor;
                        vendaBilheteriaFormaPagamento.Porcentagem.Valor = 100;
                        vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria_cancel.Control.ID;

                        if (row != null)
                        {
                            vendaBilheteriaFormaPagamento.Dias.Valor = row["Dias"] != DBNull.Value ? (int)row["Dias"] : 0;
                            vendaBilheteriaFormaPagamento.TaxaAdm.Valor = row["TaxaAdm"] != DBNull.Value ? (decimal)row["TaxaADm"] : 0.00m;
                            vendaBilheteriaFormaPagamento.IR.Valor = row["IR"] != DBNull.Value && (string)row["IR"] == "T" ? true : false;
                        }
                        else
                        {
                            vendaBilheteriaFormaPagamento.Dias.Valor = 0;
                            vendaBilheteriaFormaPagamento.TaxaAdm.Valor = 0.00m;
                            vendaBilheteriaFormaPagamento.IR.Valor = false;
                        }

                        vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);

                        string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                        int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                        bool ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
                    }
                    else
                    {
                        //Busca as formas originais de pagamento para utilizar no cancelamento
                        DataTable pagamentosOrigineis = vendaBilheteria_cancel.FormasPagamento(pendencia.VendaBilheteriaIDVenda.Valor);

                        pagamentos.Columns.Add("Valor", typeof(decimal));
                        DataTable pagamentoClone = pagamentos.Clone();
                        decimal valorEstornoDecremental = vendaBilheteria_cancel.ValorTotal.Valor;

                        #region Percorre os pagamentos originais para setar no cancelamento.
                        for (int i = 0; i < pagamentosOrigineis.Rows.Count; i++)
                        {
                            DataRow original = pagamentosOrigineis.Rows[i];
                            DataRow row = null;

                            if (pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == Convert.ToInt32(original["ID"])).Count() > 0)
                                row = pagamentos.AsEnumerable().Where(r => Convert.ToInt32(r["ID"]) == Convert.ToInt32(original["ID"])).First();
                            else
                            {
                                row = pagamentoClone.NewRow();
                                row["ID"] = original["ID"];
                            }

                            //Calcula o valor proporcional conforme a porcentagem na compra
                            decimal porcentagemOriginal = Convert.ToDecimal(original["Porcentagem"]);
                            decimal valorProporcional = Math.Round((vendaBilheteria_cancel.ValorTotal.Valor * porcentagemOriginal) / 100m, 2);

                            ///Caso o valor calculado seja superior ao resto deve usar o resto
                            if (valorEstornoDecremental < valorProporcional)
                                valorProporcional = valorEstornoDecremental;

                            if (valorProporcional >= valorEstornoDecremental)
                            {
                                row["Valor"] = valorEstornoDecremental;
                                valorEstornoDecremental = 0m;
                            }
                            else
                            {
                                row["Valor"] = valorProporcional;
                                valorEstornoDecremental -= valorProporcional;
                            }

                            pagamentoClone.ImportRow(row);

                            if (valorEstornoDecremental == 0)
                                break;
                            else if (valorEstornoDecremental < 0)
                                throw new BilheteriaException("Forma de pagamento divergente, não foi cadastrada.");
                        }
                        #endregion

                        #region Percorre a lista de forma de pagamento para uilização no cancelamento
                        for (int i = 0; i < pagamentoClone.Rows.Count; i++)
                        {
                            DataRow pagto = pagamentoClone.Rows[i];
                            VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                            vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];

                            decimal valorFormaPagamento = 0;
                            Decimal.TryParse(Convert.ToString(pagto["Valor"]), out valorFormaPagamento);
                            vendaBilheteriaFormaPagamento.Valor.Valor = valorFormaPagamento > vendaBilheteria_cancel.ValorTotal.Valor ? vendaBilheteria_cancel.ValorTotal.Valor : valorFormaPagamento;

                            //calcular porcentagem
                            decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / vendaBilheteria_cancel.ValorTotal.Valor;
                            decimal porcentagem = Math.Round(porc, 2);

                            //a porcentagem final tem q dar 100%
                            if (i != (pagamentoClone.Rows.Count - 1))
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
                            vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                            vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria_cancel.Control.ID;
                            vendaBilheteriaFormaPagamento.Dias.Valor = pagto["Dias"] != DBNull.Value ? (int)pagto["Dias"] : 0;
                            vendaBilheteriaFormaPagamento.TaxaAdm.Valor = pagto["TaxaAdm"] != DBNull.Value ? (decimal)pagto["TaxaADm"] : 0.00m;
                            vendaBilheteriaFormaPagamento.IR.Valor = pagto["IR"] != DBNull.Value && (string)pagto["IR"] == "T" ? true : false;
                            vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                            string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                            int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                            bool ok = (x >= 1);
                            if (!ok)
                                throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
                        }
                        #endregion
                    }
                    // 
                    #endregion
                }

                if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.EstornoCC)
                {
                    EstornoDadosCartaoCredito cartao = new EstornoDadosCartaoCredito();
                    cartao.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    cartao.VendaBilheteriaIDCancel.Valor = vendaBilheteria_cancel.Control.ID;
                    cartao.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_PENDENTE;
                    cartao.Atualizar(bd);
                }
                else if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito)
                {
                    EstornoDadosDepositoBancario deposito = new EstornoDadosDepositoBancario();
                    deposito.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    deposito.VendaBilheteriaIDCancel.Valor = vendaBilheteria_cancel.Control.ID;
                    deposito.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_PENDENTE;
                    deposito.Atualizar(bd);
                }

                pendencia.VendaBilheteriaIDCancel.Valor = vendaBilheteria_cancel.Control.ID;

                estrutura.Status = EstruturaCancelamento.enuStatus.CancelamentoProcessado;


                pendencia.EfetivarSolicitacao(bd, pendenciaID);

                bd.FinalizarTransacao();

                retorno = true;
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

        public bool CancelarSolicitacao(EstruturaCancelamento estrutura, int pendenciaID, BD bd = null)
        {
            bool retorno = false;
            CancelDevolucaoPendente pendencia = null;
            bool transacaoExterna = bd != null;

            try
            {
                pendencia = new CancelDevolucaoPendente();

                pendencia.Ler(pendenciaID);

                pendencia.CanalIDDevolucao.Valor = estrutura.CanalIDDevolucao;
                pendencia.CaixaIDDevolucao.Valor = estrutura.CaixaIDDevolucao;
                pendencia.LocalIDDevolucao.Valor = estrutura.LocalIDDevolucao;
                pendencia.LojaIDDevolucao.Valor = estrutura.LojaIDDevolucao;
                pendencia.EmpresaIDDevolucao.Valor = estrutura.EmpresaIDDevolucao;
                pendencia.UsuarioIDDevolucao.Valor = estrutura.UsuarioIDDevolucao;

                if (!transacaoExterna)
                {
                    bd = new BD();
                    bd.IniciarTransacao();
                }

                if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.EstornoCC)
                {
                    EstornoDadosCartaoCredito cartao = new EstornoDadosCartaoCredito();
                    cartao.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    cartao.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_CANCELADO;
                    cartao.Atualizar(bd);
                }
                else if (estrutura.FormaDevolucao == EstruturaCancelamento.enuFormaDevolucao.Deposito)
                {
                    EstornoDadosDepositoBancario deposito = new EstornoDadosDepositoBancario();
                    deposito.LerPorCancelDevolucaoPendenteID(pendencia.Control.ID);
                    deposito.Status.Valor = EstruturaCancelamento.STATUS_ESTORNO_CANCELADO;
                    deposito.Atualizar(bd);
                }

                pendencia.CancelarSolicitacao(bd, pendenciaID);

                if (!transacaoExterna)
                    bd.FinalizarTransacao();

                retorno = true;
            }
            catch (Exception ex)
            {
                if (!transacaoExterna)
                    bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                if (!transacaoExterna)
                    bd.Fechar();
            }

            return retorno;
        }

        private bool EfetivarCancelamentoIngressos(BD bd, bool cancelaIngressos, bool cancelouTodosIngressos, bool estornaVlrIngresso, bool cancelaTaxaEntrega, bool cancelaTaxaConv, bool cancelaTaxaProcessamento,
                                                   bool cancelaSeguro, int vendaBilheteriaIDVenda, int vendaBilheteriaIDCancelamento, bool cancelamentoFraude, int entregaAgendaID,
                                                   DataTable dadosItensVendidos, int usuarioID, DataTable dadosIngressosVendidos, int caixaID,
                                                   int lojaID, int canalID, int empresaID, int clienteID, int motivoCancelamento, int supervisorID,
                                                   string motivo, List<int> ingressosNaoLiberarIds = null)
        {
            bool retorno = false;
            BD bdConsulta = null;

            int cotaItemID = 0;
            int apresentacaoID = 0;
            int apresentacaoSetorID = 0;

            try
            {
                bdConsulta = new BD();

                if (cancelouTodosIngressos)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET PagamentoProcessado = 'T', VendaCancelada = 'T' WHERE ID = {0}", vendaBilheteriaIDVenda));
                if (cancelamentoFraude)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET NivelRisco = 3 WHERE ID = {0}", vendaBilheteriaIDVenda));
                if (cancelaTaxaEntrega)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET EntregaCancelada = 'T' WHERE ID = {0}", vendaBilheteriaIDVenda));
                if (cancelaTaxaConv)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET ConvenienciaCancelada = 'T' WHERE ID = {0}", vendaBilheteriaIDVenda));
                if (cancelaTaxaProcessamento)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET TaxaProcessamentoCancelada = 'T' WHERE ID = {0}", vendaBilheteriaIDVenda));
                if (cancelaSeguro)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET SeguroCancelado = 'T' WHERE ID = {0}", vendaBilheteriaIDVenda));

                if (cancelaTaxaEntrega && entregaAgendaID > 0)
                    bd.Executar("UPDATE tEntregaAgenda SET QtdAgendada = QtdAgendada - 1 WHERE ID =" + entregaAgendaID);

                if (cancelaIngressos || estornaVlrIngresso)
                {
                    //VendaBilheteriaItem
                    //dividir cada tipo de item de venda em arrays
                    DataRow[] itensNormais = dadosItensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                    DataRow[] itensMesaFechada = dadosItensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                    DataRow[] itensPacote = dadosItensVendidos.Select(TIPO + "='" + TIPO_PACOTE + "'");
                    string codigoBarra = string.Empty;
                    Enumerators.TipoCodigoBarra tipoCodigoBarra = Enumerators.TipoCodigoBarra.Estruturado;
                    string codigoBarraAntigo = string.Empty;

                    var assinaturas = TabelaMemoria.DistinctComFiltro(dadosItensVendidos, ASSINATURA_CLIENTE_ID, ASSINATURA_CLIENTE_ID + " > 0");
                    Assinatura oAssinatura = new Assinatura(usuarioID);
                    AssinaturaCliente oAssinaturaCliente = new AssinaturaCliente(usuarioID);

                    if (assinaturas.Rows.Count > 0)
                    {
                        var lstAssClienteID = new List<EstruturaAssinaturaBloqueio>();

                        foreach (DataRow linha in assinaturas.Rows)
                        {
                            var assinaturaClienteID = Convert.ToInt32(linha[ASSINATURA_CLIENTE_ID]);

                            var itensCancelar = dadosItensVendidos.Select(ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);

                            var infoCancelamento = oAssinaturaCliente.InfoCancelamento(assinaturaClienteID);

                            if (cancelaIngressos)
                            {
                                CancelamentoItensNormais(itensCancelar, dadosIngressosVendidos, vendaBilheteriaIDVenda, vendaBilheteriaIDCancelamento, codigoBarraAntigo,
                                    cancelaTaxaConv, tipoCodigoBarra, codigoBarra, bd, new BD(), usuarioID, caixaID, lojaID, canalID,
                                    empresaID, clienteID, motivoCancelamento, supervisorID, motivo, infoCancelamento, assinaturaClienteID, ingressosNaoLiberarIds);

                                lstAssClienteID.Add(new EstruturaAssinaturaBloqueio { AssinaturaClienteID = assinaturaClienteID, ClienteID = clienteID });
                            }
                            else
                            {

                                foreach (DataRow item in itensCancelar)
                                {
                                    DataRow[] ingressos;

                                    if (item["VendaBilheteriaItemID"] != DBNull.Value)
                                    {
                                        if (Convert.ToInt32(item["VendaBilheteriaItemID"]) > 0)
                                            ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "' AND VendaBilheteriaItemID = " + (int)item["VendaBilheteriaItemID"]);
                                        else
                                            ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                                    }
                                    else
                                        ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                                    IngressoLog ingressoLog = new IngressoLog();
                                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                    ingressoLog.IngressoID.Valor = (int)ingressos[0][INGRESSOID];
                                    ingressoLog.UsuarioID.Valor = usuarioID;
                                    ingressoLog.BloqueioID.Valor = (int)ingressos[0][BLOQUEIOID];
                                    ingressoLog.CortesiaID.Valor = (int)ingressos[0][CORTESIAID];
                                    ingressoLog.PrecoID.Valor = (int)ingressos[0][PRECOID];
                                    ingressoLog.VendaBilheteriaItemID.Valor = (int)ingressos[0]["VendaBilheteriaItemID"];
                                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                                    ingressoLog.CaixaID.Valor = caixaID;
                                    ingressoLog.LojaID.Valor = lojaID;
                                    ingressoLog.CanalID.Valor = canalID;
                                    ingressoLog.EmpresaID.Valor = empresaID;
                                    ingressoLog.ClienteID.Valor = clienteID;
                                    ingressoLog.Acao.Valor = IngressoLog.ESTORNO;
                                    ingressoLog.Obs.Valor = motivo;
                                    ingressoLog.MotivoId.Valor = motivoCancelamento;
                                    ingressoLog.SupervisorID.Valor = supervisorID;
                                    ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;
                                    ingressoLog.AssinaturaClienteID.Valor = assinaturaClienteID;

                                    string sqlIngressoLogV = ingressoLog.StringInserir();

                                    if (bd.Executar(sqlIngressoLogV) > 0)
                                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                                }
                            }
                        }

                        if (cancelaIngressos)
                            oAssinatura.CancelarAssinaturas(bd, lstAssClienteID);
                    }
                    else
                    {

                        if (cancelaIngressos)
                        {
                            CancelamentoItensNormais(itensNormais, dadosIngressosVendidos, vendaBilheteriaIDVenda, vendaBilheteriaIDCancelamento, codigoBarraAntigo, cancelaTaxaConv, tipoCodigoBarra, codigoBarra, bd, new BD(), usuarioID, caixaID, lojaID, canalID, empresaID,
                                    clienteID, motivoCancelamento, supervisorID, motivo, null, 0, ingressosNaoLiberarIds);
                        }
                        else
                        {
                            foreach (DataRow item in itensNormais)
                            {
                                DataRow[] ingressos;

                                if (item["VendaBilheteriaItemID"] != DBNull.Value)
                                {
                                    if (Convert.ToInt32(item["VendaBilheteriaItemID"]) > 0)
                                        ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "' AND VendaBilheteriaItemID = " + (int)item["VendaBilheteriaItemID"]);
                                    else
                                        ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                                }
                                else
                                    ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                                IngressoLog ingressoLog = new IngressoLog();
                                ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                ingressoLog.IngressoID.Valor = (int)ingressos[0][INGRESSOID];
                                ingressoLog.UsuarioID.Valor = usuarioID;
                                ingressoLog.BloqueioID.Valor = (int)ingressos[0][BLOQUEIOID];
                                ingressoLog.CortesiaID.Valor = (int)ingressos[0][CORTESIAID];
                                ingressoLog.PrecoID.Valor = (int)ingressos[0][PRECOID];
                                ingressoLog.VendaBilheteriaItemID.Valor = (int)ingressos[0]["VendaBilheteriaItemID"];
                                ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                                ingressoLog.CaixaID.Valor = caixaID;
                                ingressoLog.LojaID.Valor = lojaID;
                                ingressoLog.CanalID.Valor = canalID;
                                ingressoLog.EmpresaID.Valor = empresaID;
                                ingressoLog.ClienteID.Valor = clienteID;
                                ingressoLog.Acao.Valor = IngressoLog.ESTORNO;
                                ingressoLog.Obs.Valor = motivo;
                                ingressoLog.MotivoId.Valor = motivoCancelamento;
                                ingressoLog.SupervisorID.Valor = supervisorID;
                                ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;
                                string sqlIngressoLogV = ingressoLog.StringInserir();

                                if (bd.Executar(sqlIngressoLogV) != 1)
                                    throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                            }
                        }
                    }

                    #region Mesa fechada
                    foreach (DataRow item in itensMesaFechada)
                    {
                        DataRow[] ingressos = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                        VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();

                        bool podeAlterarIngresso = (ingressosNaoLiberarIds == null || ingressosNaoLiberarIds.Count(y => y == Convert.ToInt32((int)ingressos[0][INGRESSOID])) == 0);

                        if (cancelaIngressos)
                        {
                            vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                            vendaBilheteriaItem.PacoteID.Valor = 0;
                            vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                            vendaBilheteriaItem.TaxaComissao.Valor = item[TAXA_COMISSAO] != DBNull.Value ? (int)item[TAXA_COMISSAO] : 0;
                            vendaBilheteriaItem.ComissaoValor.Valor = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;

                            if (cancelaTaxaConv)
                            {
                                vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                                vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                            }
                            //vendaBilheteriaItem.Tipo.Valor = VendaBilheteriaItem.Mesa;

                            string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                            object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                            vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                            if (vendaBilheteriaItem.Control.ID == 0)
                                throw new BilheteriaException("Item de venda não foi gerada.");
                        }

                        foreach (DataRow i in ingressos)
                        {
                            int x;
                            bool ok;
                            string sql;

                            //TODO: Eveton, validar com o Maicon teste para não mexer em ingressos não alterados na troca
                            if (podeAlterarIngresso)
                            {
                                codigoBarraAntigo = i[CODIGO_BARRA].ToString();
                                tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(i[TIPO_CODIGO_BARRA]);
                                int eventoID = Convert.ToInt32(i[EVENTOID]);

                                if (cancelaIngressos)
                                {
                                    //deve atualizar a tIngressoCodigoBarra para adicionar os ingressos cancelados na black list
                                    bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarraAntigo + "' AND EventoID = " + eventoID);

                                    if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                                    {
                                        codigoBarra = new CodigoBarra().NovoCodigoBarraListaBranca(bd, Convert.ToInt32(i[APRESENTACAOSETORID]));

                                        //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                                        IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                                        ingressoCodigoBarra.EventoID.Valor = eventoID;
                                        ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                                        bd.Executar(ingressoCodigoBarra.StringInserir());
                                    }
                                    else codigoBarra = string.Empty;


                                    if (0 == (int)i[BLOQUEIOID])
                                    {
                                        sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                                            "VendaBilheteriaID=0, Status='" + Ingresso.DISPONIVEL + "' " +
                                            "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID = 0 AND ID=" + (int)i[INGRESSOID];
                                    }
                                    else
                                    {
                                        sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                                            "VendaBilheteriaID=0, Status='" + Ingresso.BLOQUEADO + "' " +
                                            "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID > 0 AND ID=" + (int)i[INGRESSOID];
                                    }

                                    x = bd.Executar(sql);
                                    ok = (x == 1);
                                }
                                else
                                {
                                    ok = true;
                                }
                                if (ok)
                                {
                                    //inserir na Log

                                    IngressoLog ingressoLog = new IngressoLog();
                                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                    ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                                    ingressoLog.UsuarioID.Valor = usuarioID;
                                    ingressoLog.BloqueioID.Valor = (int)i[BLOQUEIOID];
                                    ingressoLog.CortesiaID.Valor = (int)i[CORTESIAID];
                                    ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                                    ingressoLog.CaixaID.Valor = caixaID;
                                    ingressoLog.LojaID.Valor = lojaID;
                                    ingressoLog.CanalID.Valor = canalID;
                                    ingressoLog.EmpresaID.Valor = empresaID;
                                    ingressoLog.ClienteID.Valor = clienteID;
                                    ingressoLog.Acao.Valor = IngressoLog.CANCELAR;
                                    ingressoLog.Obs.Valor = motivo;
                                    ingressoLog.MotivoId.Valor = motivoCancelamento;
                                    ingressoLog.SupervisorID.Valor = supervisorID;
                                    ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;

                                    string sqlIngressoLogV = ingressoLog.StringInserir();
                                    x = bd.Executar(sqlIngressoLogV);
                                    bool okV = (x == 1);
                                    if (!okV)
                                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                                    //TODO: Eveton, validar com o Maicon teste para não mexer em ingressos não alterados na troca
                                    if (podeAlterarIngresso)
                                    {
                                        bdConsulta.Consulta("SELECT CotaItemID, ApresentacaoID, ApresentacaoSetorID  FROM tIngressoCliente (NOLOCK) WHERE IngressoID = " + (int)i[INGRESSOID]);

                                        if (bdConsulta.Consulta().Read())
                                        {
                                            cotaItemID = bdConsulta.LerInt("CotaItemID");
                                            apresentacaoID = bdConsulta.LerInt("ApresentacaoID");
                                            apresentacaoSetorID = bdConsulta.LerInt("ApresentacaoSetorID");

                                            Ingresso oIngresso = new Ingresso();

                                            string strSql = "UPDATE tIngresso SET CotaItemID = NULL WHERE ID = " + Convert.ToString(i[INGRESSOID]);

                                            bd.Executar(strSql);


                                            //oIngresso.AdicionarCotaItem((int)i[INGRESSOID], cotaItemID);

                                            //BD bd2 = new BD();
                                            //sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = " + apresentacaoSetorID;
                                            //x = bd2.Executar(sql);

                                            //if (x < 1)
                                            //{
                                            //    sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = 0";
                                            //    bd2.Executar(sql);
                                            //}

                                            sql = "DELETE FROM tIngressoCliente WHERE IngressoID = " + (int)i[INGRESSOID];
                                            bd.Executar(sql);
                                        }
                                    }
                                    bdConsulta.FecharConsulta();

                                }
                                else
                                    throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                            }
                            else
                            {
                                IngressoLog ingressoLog = new IngressoLog();
                                ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                                ingressoLog.UsuarioID.Valor = usuarioID;
                                ingressoLog.BloqueioID.Valor = (int)i[BLOQUEIOID];
                                ingressoLog.CortesiaID.Valor = (int)i[CORTESIAID];
                                ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                                ingressoLog.VendaBilheteriaItemID.Valor = (int)i["VendaBilheteriaItemID"];
                                ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                                ingressoLog.CaixaID.Valor = caixaID;
                                ingressoLog.LojaID.Valor = lojaID;
                                ingressoLog.CanalID.Valor = canalID;
                                ingressoLog.EmpresaID.Valor = empresaID;
                                ingressoLog.ClienteID.Valor = clienteID;
                                ingressoLog.Acao.Valor = IngressoLog.ESTORNO;
                                ingressoLog.Obs.Valor = motivo;
                                ingressoLog.MotivoId.Valor = motivoCancelamento;
                                ingressoLog.SupervisorID.Valor = supervisorID;
                                ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;

                                string sqlIngressoLogV = ingressoLog.StringInserir();

                                if (bd.Executar(sqlIngressoLogV) != 1)
                                    throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                            }
                        }

                    }
                    #endregion

                    #region Pacotes
                    foreach (DataRow item in itensPacote)
                    {
                        DataRow[] ingressosPacote = dadosIngressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                        VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();

                        if (cancelaIngressos)
                        {
                            vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                            vendaBilheteriaItem.PacoteID.Valor = (int)ingressosPacote[0][PACOTEID];
                            vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                            vendaBilheteriaItem.TaxaComissao.Valor = item[TAXA_COMISSAO] != DBNull.Value ? (int)item[TAXA_COMISSAO] : 0;
                            vendaBilheteriaItem.ComissaoValor.Valor = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;

                            if (cancelaTaxaConv)
                            {
                                vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                                vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                            }
                            //vendaBilheteriaItem.Tipo.Valor = VendaBilheteriaItem.Pacote;

                            string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                            object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                            vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                            if (vendaBilheteriaItem.Control.ID == 0)
                                throw new BilheteriaException("Item de venda não foi gerada.");
                        }
                        foreach (DataRow i in ingressosPacote)
                        {
                            codigoBarraAntigo = i[CODIGO_BARRA].ToString();
                            tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(i[TIPO_CODIGO_BARRA]);
                            int eventoID = Convert.ToInt32(i[EVENTOID]);

                            if (cancelaIngressos)
                            {
                                //deve atualizar a tIngressoCodigoBarra para adicionar os ingressos cancelados na black list
                                bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarraAntigo + "' AND EventoID = " + eventoID);

                                if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                                {
                                    codigoBarra = new CodigoBarra().NovoCodigoBarraListaBranca(bd, Convert.ToInt32(i[APRESENTACAOSETORID]));

                                    //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                                    IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                                    ingressoCodigoBarra.EventoID.Valor = eventoID;
                                    ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                                    bd.Executar(ingressoCodigoBarra.StringInserir());
                                }
                                else codigoBarra = string.Empty;

                                string sql;
                                if (0 == (int)i[BLOQUEIOID])
                                {
                                    sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='', PacoteID=0 , PacoteGrupo = '', " +
                                        "VendaBilheteriaID=0, Status='" + Ingresso.DISPONIVEL + "' " +
                                        "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID = 0 AND ID=" + (int)i[INGRESSOID];
                                }
                                else
                                {
                                    sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='', PacoteID=0 , PacoteGrupo = '', " +
                                        "VendaBilheteriaID=0, Status='" + Ingresso.BLOQUEADO + "' " +
                                        "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID > 0 AND ID=" + (int)i[INGRESSOID];
                                }

                                int x = bd.Executar(sql);
                                bool ok = (x == 1);
                                if (ok)
                                {
                                    //inserir na Log

                                    IngressoLog ingressoLog = new IngressoLog();
                                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                    ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                                    ingressoLog.UsuarioID.Valor = usuarioID;
                                    ingressoLog.BloqueioID.Valor = (int)i[BLOQUEIOID];
                                    ingressoLog.CortesiaID.Valor = (int)i[CORTESIAID];
                                    ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                                    ingressoLog.CaixaID.Valor = caixaID;
                                    ingressoLog.LojaID.Valor = lojaID;
                                    ingressoLog.CanalID.Valor = canalID;
                                    ingressoLog.EmpresaID.Valor = empresaID;
                                    ingressoLog.ClienteID.Valor = clienteID;
                                    ingressoLog.Acao.Valor = IngressoLog.CANCELAR;
                                    ingressoLog.Obs.Valor = motivo;
                                    ingressoLog.MotivoId.Valor = motivoCancelamento;
                                    ingressoLog.SupervisorID.Valor = supervisorID;

                                    if (codigoBarra is string)
                                        ingressoLog.CodigoBarra.Valor = (string)codigoBarra;

                                    string sqlIngressoLogV = ingressoLog.StringInserir();

                                    x = bd.Executar(sqlIngressoLogV);

                                    bool okV = (x == 1);

                                    if (!okV)
                                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                                    bdConsulta.Consulta("SELECT CotaItemID, ApresentacaoID, ApresentacaoSetorID  FROM tIngressoCliente (NOLOCK) WHERE IngressoID = " + (int)i[INGRESSOID]);

                                    if (bdConsulta.Consulta().Read())
                                    {
                                        cotaItemID = bdConsulta.LerInt("CotaItemID");
                                        apresentacaoID = bdConsulta.LerInt("ApresentacaoID");
                                        apresentacaoSetorID = bdConsulta.LerInt("ApresentacaoSetorID");



                                        Ingresso oIngresso = new Ingresso();

                                        string strSql = "UPDATE tIngresso SET CotaItemID = NULL WHERE ID = " + Convert.ToString(i[INGRESSOID]);

                                        bd.Executar(strSql);


                                        //oIngresso.AdicionarCotaItem((int)i[INGRESSOID], cotaItemID);

                                        //BD bd2 = new BD();
                                        //sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = " + apresentacaoSetorID;
                                        //x = bd2.Executar(sql);

                                        //if (x < 1)
                                        //{
                                        //    sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = 0";
                                        //    bd2.Executar(sql);
                                        //}

                                        sql = "DELETE FROM tIngressoCliente WHERE IngressoID = " + (int)i[INGRESSOID];
                                        bd.Executar(sql);
                                    }

                                    bdConsulta.FecharConsulta();

                                }
                                else
                                    throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                            }
                            else
                            {
                                IngressoLog ingressoLog = new IngressoLog();
                                ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                                ingressoLog.UsuarioID.Valor = usuarioID;
                                ingressoLog.BloqueioID.Valor = (int)i[BLOQUEIOID];
                                ingressoLog.CortesiaID.Valor = (int)i[CORTESIAID];
                                ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                                ingressoLog.VendaBilheteriaItemID.Valor = (int)i["VendaBilheteriaItemID"];
                                ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                                ingressoLog.CaixaID.Valor = caixaID;
                                ingressoLog.LojaID.Valor = lojaID;
                                ingressoLog.CanalID.Valor = canalID;
                                ingressoLog.EmpresaID.Valor = empresaID;
                                ingressoLog.ClienteID.Valor = clienteID;
                                ingressoLog.Acao.Valor = IngressoLog.ESTORNO;
                                ingressoLog.Obs.Valor = motivo;
                                ingressoLog.MotivoId.Valor = motivoCancelamento;
                                ingressoLog.SupervisorID.Valor = supervisorID;

                                string sqlIngressoLogV = ingressoLog.StringInserir();

                                if (bd.Executar(sqlIngressoLogV) != 1)
                                    throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }


        private void CancelamentoItensNormais(DataRow[] itensNormais, DataTable ingressosVendidos, int vendaBilheteriaIDVenda, int vendaBilheteriaIDCancelamento, string codigoBarraAntigo,
                                              bool cancelaTaxaConv, Enumerators.TipoCodigoBarra tipoCodigoBarra, string codigoBarra, BD bd,
                                              BD bdConsulta, int usuarioID, int caixaID, int lojaID, int canalID, int empresaID, int clienteID,
                                              int motivoId, int SupervisorID, string motivo, Assinatura infoAssinatura, int assinaturaClienteID, List<int> ingressosNaoLiberarIds = null)
        {
            #region Cadeira, Mesa Aberta e pista
            foreach (DataRow item in itensNormais)
            {
                DataRow[] ingressos;

                if (item["VendaBilheteriaItemID"] != DBNull.Value)
                {
                    if (Convert.ToInt32(item["VendaBilheteriaItemID"]) > 0)
                        ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "' AND VendaBilheteriaItemID = " + (int)item["VendaBilheteriaItemID"]);
                    else
                        ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                }
                else
                    ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                bool podeAlterarIngresso = (ingressosNaoLiberarIds == null || ingressosNaoLiberarIds.Count(y => y == Convert.ToInt32((int)ingressos[0][INGRESSOID])) == 0);

                VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                vendaBilheteriaItem.PacoteID.Valor = 0;
                vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                vendaBilheteriaItem.TaxaComissao.Valor = item[TAXA_COMISSAO] != DBNull.Value ? (int)item[TAXA_COMISSAO] : 0;
                vendaBilheteriaItem.ComissaoValor.Valor = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;

                if (cancelaTaxaConv)
                {
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                }
                //vendaBilheteriaItem.Tipo.Valor = VendaBilheteriaItem.Ingresso;

                string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                if (vendaBilheteriaItem.Control.ID == 0)
                    throw new BilheteriaException("Item de venda não foi gerada.");

                bool ok;
                int x;
                string sql = string.Empty;


                //TODO: Eveton, validar com o Maicon teste para não mexer em ingressos não alterados na troca
                if (podeAlterarIngresso)
                {
                    codigoBarraAntigo = ingressos[0][CODIGO_BARRA].ToString();
                    if (ingressos[0][TIPO_CODIGO_BARRA] == null || ingressos[0][TIPO_CODIGO_BARRA].ToString().Length == 0)
                        tipoCodigoBarra = Enumerators.TipoCodigoBarra.Estruturado;
                    else
                        tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(ingressos[0][TIPO_CODIGO_BARRA]);
                    int eventoID = Convert.ToInt32(ingressos[0][EVENTOID]);

                    //deve atualizar a tIngressoCodigoBarra para adicionar os ingressos cancelados na black list
                    if (codigoBarraAntigo.Length > 0)
                        bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', Sincronizado = 'F', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarraAntigo + "' AND EventoID = " + eventoID);

                    if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                    {
                        codigoBarra = new CodigoBarra().NovoCodigoBarraListaBranca(bd, Convert.ToInt32(ingressos[0][APRESENTACAOSETORID]));

                        //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                        IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                        ingressoCodigoBarra.EventoID.Valor = eventoID;
                        ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                        bd.Executar(ingressoCodigoBarra.StringInserir());
                    }
                    else codigoBarra = string.Empty;


                    sql = string.Empty;

                    if (assinaturaClienteID > 0 && infoAssinatura.Cancelamento == Assinatura.enumTipoCancelamento.Bloquear)
                    {
                        sql = "UPDATE tIngresso SET UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', SerieID = 0, " +
                                "AssinaturaClienteID = 0, VendaBilheteriaID = 0, Status='" + Ingresso.BLOQUEADO + "', BloqueioID =  " + infoAssinatura.DesistenciaBloqueioID.Valor +
                                " WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND ID=" + (int)ingressos[0][INGRESSOID];
                    }
                    else
                    {
                        if (0 == (int)ingressos[0][BLOQUEIOID])
                        {
                            sql = "UPDATE tIngresso SET ClienteID = 0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                                "VendaBilheteriaID=0, Status='" + Ingresso.DISPONIVEL + "', AssinaturaCLienteID = 0, SerieID = 0 " +
                                "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID = 0 AND ID=" + (int)ingressos[0][INGRESSOID];
                        }
                        else
                        {
                            sql = "UPDATE tIngresso SET ClienteID = 0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                                "VendaBilheteriaID=0, Status='" + Ingresso.BLOQUEADO + "', AssinaturaCLienteID = 0, SerieID = 0 " +
                                "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID > 0 AND ID=" + (int)ingressos[0][INGRESSOID];
                        }
                    }



                    x = bd.Executar(sql);
                    ok = (x == 1);
                }
                else
                {
                    ok = true;
                }

                if (ok)
                {
                    //inserir na Log

                    IngressoLog ingressoLog = new IngressoLog();
                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                    ingressoLog.IngressoID.Valor = (int)ingressos[0][INGRESSOID];
                    ingressoLog.UsuarioID.Valor = usuarioID;
                    ingressoLog.BloqueioID.Valor = (int)ingressos[0][BLOQUEIOID];
                    ingressoLog.CortesiaID.Valor = (int)ingressos[0][CORTESIAID];
                    ingressoLog.PrecoID.Valor = (int)ingressos[0][PRECOID];
                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                    ingressoLog.CaixaID.Valor = caixaID;
                    ingressoLog.LojaID.Valor = lojaID;
                    ingressoLog.CanalID.Valor = canalID;
                    ingressoLog.EmpresaID.Valor = empresaID;
                    ingressoLog.ClienteID.Valor = clienteID;
                    ingressoLog.Acao.Valor = IngressoLog.CANCELAR;
                    ingressoLog.Obs.Valor = motivo;
                    ingressoLog.MotivoId.Valor = motivoId;
                    ingressoLog.SupervisorID.Valor = SupervisorID;
                    ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;
                    ingressoLog.AssinaturaClienteID.Valor = assinaturaClienteID;

                    string sqlIngressoLogV = ingressoLog.StringInserir();

                    x = bd.Executar(sqlIngressoLogV);

                    bool okV = (x == 1);

                    if (!okV)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                    //TODO: Eveton, validar com o Maicon teste para não mexer em ingressos não alterados na troca
                    if (podeAlterarIngresso)
                    {
                        bdConsulta.Consulta("SELECT CotaItemID, ApresentacaoID, ApresentacaoSetorID  FROM tIngressoCliente (NOLOCK) WHERE IngressoID = " + (int)ingressos[0][INGRESSOID]);

                        if (bdConsulta.Consulta().Read())
                        {
                            var cotaItemID = bdConsulta.LerInt("CotaItemID");
                            var apresentacaoID = bdConsulta.LerInt("ApresentacaoID");
                            var apresentacaoSetorID = bdConsulta.LerInt("ApresentacaoSetorID");

                            //Ingresso oIngresso = new Ingresso();

                            //oIngresso.AdicionarCotaItem((int)ingressos[0][INGRESSOID],null);
                            Ingresso oIngresso = new Ingresso();

                            string strSql = "UPDATE tIngresso SET CotaItemID = NULL WHERE ID = " + Convert.ToString(ingressos[0][INGRESSOID]);

                            bd.Executar(strSql);

                            sql = "delete from tingressocliente where ingressoid = " + (int)ingressos[0][INGRESSOID];
                            bd.Executar(sql);
                        }
                    }
                    bdConsulta.FecharConsulta();
                }
                else
                    throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
            }
            #endregion
        }

    }

    [Serializable]
    public class CancelamentoException : Exception
    {
        public CancelamentoException() { }

        public CancelamentoException(string msg)
            : base(msg) { }
    }
}
