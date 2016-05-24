using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{
    public partial class BilheteriaParalela
    {
        public int VenderInternet(Dictionary<int, decimal> ingressosID, string sessionID, EstruturaReservaInternet estruturaReservaInternet, int entregaControleID, int pdvID, int clienteEnderecoID, int clienteID, string ip)
        {
            BD bd = new BD();
            int vendaBilheteriaID = 0;
            try
            {

                StringBuilder stbSQL = new StringBuilder();
            
                //caixa para registrar a venda
                int caixaID = 0;
            
                if (estruturaReservaInternet.CaixaID == 0)
                    caixaID = VerificaCaixaInternet();
                else
                    //Verificar o caixa do usuario.
                    caixaID = estruturaReservaInternet.CaixaID;

                DataSet ds = MontaDadosVendaInternet(ingressosID,  clienteID, sessionID, estruturaReservaInternet.CanalID, estruturaReservaInternet.UsuarioID);

                DataTable itensReservados = ds.Tables[TABELA_GRID]; // Resumo
                DataTable ingressosVendidos = ds.Tables[TABELA_RESERVA]; // Detalhamento

                //verifica se a quantidade reservada no site é a mesma reservada no sistema
                if (ingressosVendidos.Rows.Count != ingressosID.Count)
                    throw new BilheteriaException("Quantidade de ingressos diferente da reserva.", CodMensagemVenda.FalhaVenda);

                bd.IniciarTransacao();

                //verifica se existem reservas
                if (itensReservados.Rows.Count == 0 && ingressosVendidos.Rows.Count == 0)
                    throw new BilheteriaException("Não existe reserva para o cliente.", CodMensagemVenda.ReservaInexistente);

                // VendaBilheteria
                int empresaID = new Canal().BuscaEmpresaIDporLojaID(estruturaReservaInternet.LojaID);

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;
                vendaBilheteria.ClienteID.Valor = clienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
                vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                vendaBilheteria.PdvID.Valor = pdvID;
                vendaBilheteria.PagamentoProcessado.Valor = true;
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.IP.Valor = ip;
                
                string sqlVendaBilheteria = vendaBilheteria.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                vendaBilheteria.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                var senha = (ret != null) ? Convert.ToString(ret) : null;
                vendaBilheteriaID = vendaBilheteria.Control.ID;

                DataRow[] itensNormais = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                DataRow[] itensMesaFechada = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                DataRow[] itensPacote = itensReservados.Select(TIPO + "='" + TIPO_PACOTE + "'");

                #region Ingressos Normais
                foreach (DataRow item in itensNormais)
                {

                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item["TaxaConvenienciaValor"];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                    int x = 0;
                    bool ok = false;
                    stbSQL = new StringBuilder();


                    stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                    stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '' ");
                    stbSQL.Append("WHERE(AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + clienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)item["IngressoID"]);
                    x = bd.Executar(stbSQL.ToString());
                    ok = (x >= 1);

                    if (!ok)
                        throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);



                    //inserir na Log
                    IngressoLog ingressoLog = new IngressoLog();
                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                    ingressoLog.IngressoID.Valor = (int)item[INGRESSOID];
                    ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //usuario fixo para Internet
                    ingressoLog.BloqueioID.Valor = 0;
                    ingressoLog.CortesiaID.Valor = Convert.ToInt32(ingressos[0][CORTESIAID]);
                    ingressoLog.PrecoID.Valor = (int)item[PRECOID];
                    ingressoLog.GerenciamentoIngressosID.Valor = (int)item[GERENCIAMENTO_INGRESSOS_ID];
                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    ingressoLog.CaixaID.Valor = caixaID; // ABERTURA E FECHAMENTO DE CAIXA DIARIO PARA INTERNET
                    ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //loja fixa Internet
                    ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //canal fixo Internet
                    ingressoLog.EmpresaID.Valor = empresaID; // FIXO IR
                    ingressoLog.ClienteID.Valor = clienteID;
                    ingressoLog.Acao.Valor = IngressoLog.VENDER;
                    string sqlIngressoLogV = ingressoLog.StringInserir();
                    x = bd.Executar(sqlIngressoLogV);
                    bool okV = (x == 1);
                    if (!okV)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                }
                #endregion

                #region Mesa Fechada
                foreach (DataRow item in itensMesaFechada)
                {

                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item["TaxaConvenienciaValor"];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                    foreach (DataRow i in ingressos)
                    {
                        int x = 0;
                        bool ok = false;
                        stbSQL = new StringBuilder();

                        stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                        stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '' ");
                        stbSQL.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + clienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)i[INGRESSOID]);

                        x = bd.Executar(stbSQL.ToString());
                        ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);


                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //USUARIO INTERNET
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = Convert.ToInt32(i[CORTESIAID]);
                        ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID; //CAIXA ABERTO DIARIAMENTE PARA INTERNET
                        ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //Loja Internet
                        ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //Canal Internet
                        ingressoLog.EmpresaID.Valor = empresaID; //Empresa IR
                        ingressoLog.ClienteID.Valor = clienteID;
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                    }
                }
                #endregion

                #region Pacote
                int PacoteGrupo = 0;
                int pacoteIDAtual = 0;
                foreach (DataRow item in itensPacote)
                {

                    DataRow[] ingressosPacote = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    Pacote oPacote = new Pacote();
                    oPacote.Ler((int)ingressosPacote[0][PACOTEID]);

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = oPacote.Control.ID;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;


                    if (pacoteIDAtual != vendaBilheteriaItem.PacoteID.Valor)
                    {
                        PacoteGrupo = 0;
                        pacoteIDAtual = vendaBilheteriaItem.PacoteID.Valor;
                    }
                    PacoteGrupo++;

                    vendaBilheteriaItem.PacoteGrupo.Valor = PacoteGrupo;


                    if (item[CONV] == DBNull.Value)
                        item[CONV] = 0;
                    if (item[VALOR_CONV] == DBNull.Value)
                        item[VALOR_CONV] = 0;
                    if (item[TAXA_COMISSAO] == DBNull.Value)
                        item[TAXA_COMISSAO] = 0;
                    if (item[COMISSAO_VALOR] == DBNull.Value)
                        item[COMISSAO_VALOR] = 0;

                    int conv = (int)item[CONV];
                    decimal valorConv = (decimal)item["TaxaConvenienciaValor"];
                    int taxaComissao = (int)item[TAXA_COMISSAO];
                    decimal comissaovalor = (decimal)item[COMISSAO_VALOR];

                    if (!(bool)item["PermitirCancelamentoAvulso"])
                    {
                        vendaBilheteriaItem.TaxaConveniencia.Valor = conv;
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = valorConv;
                        vendaBilheteriaItem.TaxaComissao.Valor = taxaComissao;
                        vendaBilheteriaItem.ComissaoValor.Valor = comissaovalor;

                        string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                        object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                        vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                        if (vendaBilheteriaItem.Control.ID == 0)
                            throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                    }

                    foreach (DataRow i in ingressosPacote)
                    {
                        if ((bool)item["PermitirCancelamentoAvulso"])
                        {
                            CanalPacote canalPacote = new CanalPacote();

                            //busca as taxas de conveniencia e comissão e popula as variaveis
                            DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(estruturaReservaInternet.CanalID, vendaBilheteriaItem.PacoteID.Valor);
                            Preco precoIngresso = new Preco();
                            precoIngresso.Ler((int)i["PrecoID"]);

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

                            string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                            object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                            vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                            if (vendaBilheteriaItem.Control.ID == 0)
                                throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                        }

                        int x = 0;
                        bool ok = false;
                        stbSQL = new StringBuilder();

                        stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                        stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "' ");
                        stbSQL.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + clienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)i[INGRESSOID]);
                        x = bd.Executar(stbSQL.ToString());
                        ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //usuario
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = Convert.ToInt32(i[CORTESIAID]);
                        ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID; //caixa do usuario
                        ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //loja 
                        ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //canal 
                        ingressoLog.EmpresaID.Valor = empresaID; //empresa IR
                        ingressoLog.ClienteID.Valor = clienteID;
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                    }
                }
                #endregion

                
                
                bd.FinalizarTransacao();
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
            return vendaBilheteriaID;
        }


    }
}
