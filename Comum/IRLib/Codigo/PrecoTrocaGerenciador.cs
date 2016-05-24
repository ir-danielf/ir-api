using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib
{
    public class PrecoTrocaGerenciador
    {
        /// <summary>
        /// Pesquisa pela senha
        /// </summary>
        /// <returns></returns>
        public DataTable PesquisarSenha(string senha, int canalID, int localID)
        {
            try
            {
                DataTable ingressosComprados = new DataTable("Ingressos");
                ingressosComprados.Columns.Add("Local", typeof(string));
                ingressosComprados.Columns.Add("IngressoID", typeof(int));
                ingressosComprados.Columns.Add("Evento", typeof(string));
                ingressosComprados.Columns.Add("Apresentacao", typeof(string));
                ingressosComprados.Columns.Add("ApresentacaoID", typeof(int));
                ingressosComprados.Columns.Add("Setor", typeof(string));
                ingressosComprados.Columns.Add("SetorID", typeof(int));
                ingressosComprados.Columns.Add("Codigo", typeof(string));
                ingressosComprados.Columns.Add("Status", typeof(string));
                ingressosComprados.Columns.Add("PrecoAntigo", typeof(string));
                ingressosComprados.Columns.Add("ValorAntigo", typeof(decimal));                
                ingressosComprados.Columns.Add("NovoPreco", typeof(string));
                ingressosComprados.Columns.Add("NovoValor", typeof(decimal));                

                BD bd = new BD();                                

                string sql = @"SELECT DISTINCT l.Nome Local, e.Nome Evento, a.Horario Apresentacao, a.ID ApresentacaoID, s.Nome Setor, s.ID SetorID, p.Nome PrecoAntigo, p.Valor ValorAntigo, i.Codigo, i.Status,i.ID IngressoID
                            FROM tVendaBilheteria vb (NOLOCK)
                            INNER JOIN tIngresso i (NOLOCK) ON vb.ID = i.VendaBilheteriaID AND i.Status IN('V', 'I', 'E')
                            INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.PacoteID = 0
                            INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID AND l.ID = " + localID + @"
                            INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
                            INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID AND a.DisponivelAjuste='T'
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                            INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
                            INNER JOIN tCanalEvento ce (NOLOCK) ON e.ID = ce.EventoID AND ce.CanalID = " + canalID +
                            "WHERE Senha = '" + senha + "'";

                while (bd.Consulta(sql).Read())
                {
                    DataRow linha = ingressosComprados.NewRow();

                    linha["Local"] = bd.LerString("Local");
                    linha["IngressoID"] = bd.LerInt("IngressoID");                    
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["SetorID"] = bd.LerInt("SetorID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    linha["Status"] = bd.LerString("Status");
                    linha["PrecoAntigo"] = bd.LerString("PrecoAntigo");
                    linha["ValorAntigo"] = bd.LerDecimal("ValorAntigo"); 

                    ingressosComprados.Rows.Add(linha);
                }
                bd.Fechar();

                return ingressosComprados;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string TrocarPreco(Dictionary<int, int> ingressoXpreco, decimal valorPagar,
            decimal valorCancelar,int clienteID, int caixaID, int canalID, 
            int empresaID, int lojaID, string motivoTroca, int usuarioID, 
            int BIN, int codigoAutorizacaoCartao, int indiceInstituicaoTransacao, 
            int indiceTipoCartao, int modalidadePagamentoCodigo,
            string modalidadePagamentoTexto, string notaFiscalCliente,
            string notaFiscalEstabelecimento, int NSUHost, int NSUSitef,
            int formaPagamentoEscolhida)
        {
            BD bd = new BD();
            try
            {
                bd.IniciarTransacao();

                /*
                VB
                VBI
                VBFP
                Log
                tIngressO
                 */

                #region Cancelamento
                VendaBilheteria vbCancelamento = new VendaBilheteria();
                vbCancelamento.CaixaID.Valor = caixaID;
                vbCancelamento.ClienteID.Valor = clienteID;
                vbCancelamento.DataVenda.Valor = DateTime.Now;
                vbCancelamento.Obs.Valor = motivoTroca;
                vbCancelamento.Status.Valor = VendaBilheteria.CANCELADO;
                vbCancelamento.TaxaConvenienciaValorTotal.Valor = 0;
                vbCancelamento.TaxaEntregaID.Valor = 0;
                vbCancelamento.TaxaEntregaValor.Valor = 0;
                vbCancelamento.ValorTotal.Valor = valorCancelar;
                vbCancelamento.PagamentoProcessado.Valor = true;
                string sql = vbCancelamento.StringInserir();
                object retorno = bd.ConsultaValor(sql);
                int vendaBilheteriaIDCancelamento = Convert.ToInt32(retorno);                
                if (vendaBilheteriaIDCancelamento == 0)
                    throw new Exception("Não foi possível inserir o cancelamento na VendaBilheteria");

                VendaBilheteriaFormaPagamento vbfpCancelamento = new VendaBilheteriaFormaPagamento();
                vbfpCancelamento.FormaPagamentoID.Valor = formaPagamentoEscolhida;
                vbfpCancelamento.Porcentagem.Valor = 100;
                vbfpCancelamento.Valor.Valor = valorCancelar;
                vbfpCancelamento.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;

                sql = vbfpCancelamento.StringInserir();
                 retorno = bd.ConsultaValor(sql);
                int vendaBilheteriaFormaPagamentoIDCancelamento = Convert.ToInt32(retorno);
                
                if (vendaBilheteriaFormaPagamentoIDCancelamento == 0)
                    throw new Exception("Não foi possível inserir o cancelamento na VendaBilheteriaFormaPagamento");

                foreach (KeyValuePair<int,int> iXp in ingressoXpreco)
                {
                    int ingressoID = iXp.Key;
                    VendaBilheteriaItem vbi = new VendaBilheteriaItem();
                    vbi.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                    vbi.PacoteID.Valor = 0;
                    vbi.TaxaConveniencia.Valor = 0;
                    vbi.TaxaConvenienciaValor.Valor = 0;
                    vbi.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;

                    sql = vbi.StringInserir();
                    retorno = bd.ConsultaValor(sql);
                    int vendaBilheteriaItemIDCancelamento = Convert.ToInt32(retorno);
                    if (vendaBilheteriaItemIDCancelamento == 0)
                        throw new Exception("Não foi possível inserir o cancelamento na VendaBilheteriaItem");
                    

                    Ingresso ingresso = new Ingresso();
                    ingresso.Ler(ingressoID);
                    IngressoLog oIngressoLog = new IngressoLog();
                    oIngressoLog.Acao.Valor = IngressoLog.CANCELAR;
                    oIngressoLog.BloqueioID.Valor = 0;
                    oIngressoLog.CaixaID.Valor = caixaID;
                    oIngressoLog.CanalID.Valor = canalID;
                    oIngressoLog.ClienteID.Valor = ingresso.ClienteID.Valor;
                    oIngressoLog.CortesiaID.Valor = 0;
                    oIngressoLog.EmpresaID.Valor = empresaID;
                    oIngressoLog.IngressoID.Valor = ingressoID;
                    oIngressoLog.LojaID.Valor = lojaID;
                    oIngressoLog.Obs.Valor = motivoTroca;
                    oIngressoLog.PrecoID.Valor = ingresso.PrecoID.Valor;
                    oIngressoLog.TimeStamp.Valor = DateTime.Now;
                    oIngressoLog.UsuarioID.Valor = usuarioID;
                    oIngressoLog.VendaBilheteriaID.Valor = vendaBilheteriaIDCancelamento;
                    oIngressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItemIDCancelamento;

                    sql = oIngressoLog.StringInserir();
                    retorno = bd.ConsultaValor(sql);                  
                }
                #endregion

                #region Nova Venda                
                VendaBilheteria vbVenda = new VendaBilheteria();
                vbVenda.BIN.Valor = BIN;
                vbVenda.CaixaID.Valor = caixaID;
                vbVenda.ClienteID.Valor = clienteID;
                vbVenda.CodigoAutorizacaoCredito.Valor = codigoAutorizacaoCartao;
                vbVenda.DataVenda.Valor = DateTime.Now;
                vbVenda.IndiceInstituicaoTransacao.Valor = indiceInstituicaoTransacao;
                vbVenda.IndiceTipoCartao.Valor = indiceTipoCartao;
                vbVenda.ModalidadePagamentoCodigo.Valor = modalidadePagamentoCodigo;
                vbVenda.ModalidadePagamentoTexto.Valor = modalidadePagamentoTexto;
                vbVenda.NotaFiscalCliente.Valor = notaFiscalCliente;
                vbVenda.NotaFiscalEstabelecimento.Valor = notaFiscalEstabelecimento;
                vbVenda.NSUHost.Valor = NSUHost;
                vbVenda.NSUSitef.Valor = NSUSitef;
                vbVenda.Obs.Valor = string.Empty;
                vbVenda.Status.Valor = VendaBilheteria.PAGO;
                vbVenda.TaxaConvenienciaValorTotal.Valor = 0;
                vbVenda.TaxaEntregaID.Valor = 0;
                vbVenda.TaxaEntregaValor.Valor = 0;
                vbVenda.ValorTotal.Valor = valorPagar;
                vbVenda.PagamentoProcessado.Valor = true;
                sql = vbVenda.StringInserir();
                retorno = bd.ConsultaValor(sql);
                int vendaBilheteriaID = Convert.ToInt32(retorno);
                if (vendaBilheteriaID == 0)
                    throw new Exception("Não foi possível inserir a venda na VendaBilheteria");

                VendaBilheteriaFormaPagamento vbfpVenda = new VendaBilheteriaFormaPagamento();
                vbfpVenda.FormaPagamentoID.Valor = formaPagamentoEscolhida;
                vbfpVenda.Porcentagem.Valor = 100;
                vbfpVenda.Valor.Valor = valorPagar;
                vbfpVenda.VendaBilheteriaID.Valor = vendaBilheteriaID;

                sql = vbfpVenda.StringInserir();
                retorno = bd.ConsultaValor(sql);
                int vendaBilheteriaFormaPagamentoID = Convert.ToInt32(retorno);
                if (vendaBilheteriaFormaPagamentoID == 0)
                    throw new Exception("Não foi possível inserir a venda na VendaBilheteriaFormaPagamento");

                foreach (KeyValuePair<int, int> iXp in ingressoXpreco)
                {
                    int ingressoID = iXp.Key;
                    VendaBilheteriaItem vbi = new VendaBilheteriaItem();
                    vbi.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vbi.PacoteID.Valor = 0;
                    vbi.TaxaConveniencia.Valor = 0;
                    vbi.TaxaConvenienciaValor.Valor = 0;
                    vbi.VendaBilheteriaID.Valor = vendaBilheteriaID;

                    sql = vbi.StringInserir();
                    retorno = bd.ConsultaValor(sql);
                    int vendaBilheteriaItemID = Convert.ToInt32(retorno);
                    if (vendaBilheteriaItemID == 0)
                        throw new Exception("Não foi possível inserir a venda na VendaBilheteriaItem");


                    Ingresso ingresso = new Ingresso();
                    ingresso.Ler(ingressoID);
                    IngressoLog oIngressoLog = new IngressoLog();
                    oIngressoLog.Acao.Valor = IngressoLog.VENDER;
                    oIngressoLog.BloqueioID.Valor = 0;
                    oIngressoLog.CaixaID.Valor = caixaID;
                    oIngressoLog.CanalID.Valor = canalID;
                    oIngressoLog.ClienteID.Valor = ingresso.ClienteID.Valor;
                    oIngressoLog.CortesiaID.Valor = 0;
                    oIngressoLog.EmpresaID.Valor = empresaID;
                    oIngressoLog.IngressoID.Valor = ingressoID;
                    oIngressoLog.LojaID.Valor = lojaID;
                    oIngressoLog.Obs.Valor = string.Empty;
                    int precoID=0;
                    ingressoXpreco.TryGetValue(ingressoID,out precoID);
                    if (precoID == 0)
                        throw new Exception("Não existe preço definido para um dos ingressos!");
                    oIngressoLog.PrecoID.Valor = precoID;
                    oIngressoLog.TimeStamp.Valor = DateTime.Now;
                    oIngressoLog.UsuarioID.Valor = usuarioID;
                    oIngressoLog.VendaBilheteriaID.Valor = vendaBilheteriaID;
                    oIngressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItemID;
                    sql = oIngressoLog.StringInserir();
                    bd.ConsultaValor(sql);

                    //ingresso.CodigoBarra.Valor = new CodigoBarra().GerarCodigoBarra(precoID, ingresso.EventoID.Valor);
                    ingresso.Status.Valor = Ingresso.VENDIDO;
                    ingresso.VendaBilheteriaID.Valor = vendaBilheteriaID;
                    ingresso.UsuarioID.Valor = usuarioID;
                    ingresso.LojaID.Valor = lojaID;
                    ingresso.PrecoID.Valor = precoID;

                    if (!ingresso.Atualizar(bd))
                        throw new Exception("Ingresso não pôde ser atualizado!");

                }
                #endregion
                bd.FinalizarTransacao();
                retorno = bd.ConsultaValor("SELECT Senha FROM tVendaBilheteria WHERE ID = " +vendaBilheteriaID);
                if (retorno is string)
                    return (string)retorno;
                else
                    return string.Empty;                
            }
            catch
            {
                bd.DesfazerTransacao();                
            }                
            return null;
        }
    }
}