using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib
{
    public partial class VendaBilheteria
    {
        /// <summary>
        /// Captura Vouchers de Compra
        /// </summary>
        /// <param name="lstVendaBilheteriaID">Lista contendo os VendaBilheteriaID</param>
        /// /// <param name="lstSenha">Lista contendo as Senhas</param>
        /// <returns>Lista de Vouchers de Compra</returns>
        public List<EstruturaVoucherCompra> VoucherCompra(List<int> lstVendaBilheteriaID, List<string> lstSenha)
        {
            List<EstruturaVoucherCompra> lstVoucherCompra = new List<EstruturaVoucherCompra>();
            DataTable oDataTable = new DataTable();

            try
            {
                // Captura VendaBilheteriaID e transforma em Clausula SQL
                StringBuilder sqlVendaBilheteriaID = new StringBuilder();
                if (lstVendaBilheteriaID != null)
                {
                    foreach (int VendaBilheteriaID in lstVendaBilheteriaID)
                    {
                        if (sqlVendaBilheteriaID.Length > 0)
                            sqlVendaBilheteriaID.Append("OR ");

                        sqlVendaBilheteriaID.Append("(tVendaBilheteria.ID = " + VendaBilheteriaID + ") ");
                    }
                }
                
                // Captura VendaBilheteriaID e transforma em Clausula SQL
                StringBuilder sqlSenha = new StringBuilder();
                if (lstSenha != null)
                {
                    foreach (string Senha in lstSenha)
                    {
                        if (sqlSenha.Length > 0)
                            sqlSenha.Append("OR ");

                        sqlSenha.Append("(tVendaBilheteria.Senha = '" + Senha + "') ");
                    }
                }                

                using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT " +
                        "   tVendaBilheteria.ID AS VendaBilheteriaID, " +
                        "   tVendaBilheteria.DataVenda AS DataCompra, " +
                        "   tVendaBilheteria.Senha, " +
                        "   tUsuario.Nome AS Operador, " +
                        "   tLoja.Nome AS Loja, " +
                        "   tVendaBilheteria.BIN, " +
                        "   tCliente.Nome, " +
                        "   tCliente.CPF, " +
                        "   tCliente.EnderecoEntrega, " +
                        "   tCliente.NumeroEntrega, " +
                        "   tCliente.ComplementoEntrega, " +
                        "   tCliente.BairroEntrega, " +
                        "   tCliente.CidadeEntrega, " +
                        "   tCliente.EstadoEntrega, " +
                        "   tCliente.CEPEntrega, " +
                        "   tCliente.DDDTelefone AS TelefoneResidencialDDD, " +
                        "   tCliente.Telefone AS TelefoneResidencial, " +
                        "   tCliente.DDDCelular AS TelefoneCelularDDD, " +
                        "   tCliente.Celular AS TelefoneCelular, " +
                        "   tCliente.Email, " + 
                        "   tCliente.Obs AS Observacao, " +
                        "   tCanal.PoliticaTroca, " +
                        "   tTaxaEntrega.Nome AS ProcedimentoRetiradaNome, " +
                        "   tTaxaEntrega.ProcedimentoEntrega AS ProcedimentoRetiradaDescricao, " +
                        "   (tVendaBilheteria.ValorTotal - tVendaBilheteria.TaxaEntregaValor - tVendaBilheteria.TaxaConvenienciaValorTotal) AS TotaisIngressos, " +
                        "   tVendaBilheteria.TaxaEntregaValor AS TotaisTaxaEntrega, " +
                        "   tVendaBilheteria.TaxaConvenienciaValorTotal AS TotaisTaxaConveniencia, " +
                        "   tVendaBilheteria.ValorTotal AS TotaisGeral " +
                        "FROM " +
                        "   tVendaBilheteria (NOLOCK) " +
                        "INNER JOIN " +
                        "   tCaixa (NOLOCK)  ON tVendaBilheteria.CaixaID = tCaixa.ID " +
                        "INNER JOIN " +
                        "   tUsuario (NOLOCK)  ON tCaixa.UsuarioID = tUsuario.ID " +
                        "INNER JOIN " +
                        "   tLoja (NOLOCK)  ON tCaixa.LojaID = tLoja.ID " +
                        "INNER JOIN " +
                        "   tCanal (NOLOCK)  ON tLoja.CanalID = tCanal.ID " +
                        "LEFT OUTER JOIN " +
                        "   tTaxaEntrega (NOLOCK)  ON tVendaBilheteria.TaxaEntregaID = tTaxaEntrega.ID " +
                        "LEFT OUTER JOIN " +
                        "   tCliente (NOLOCK)  ON tVendaBilheteria.ClienteID = tCliente.ID " +
                        "WHERE " +
                        "   (tCanal.TipoVenda = '" + Convert.ToChar(Canal.TipoDeVenda.ImpressaoVoucher) + "')  " +
                        ((sqlVendaBilheteriaID.Length > 0) ? "AND " +
                        "( " + sqlVendaBilheteriaID.ToString() + ") " : "") +
                        ((sqlSenha.Length > 0) ? "AND " +
                        "( " + sqlSenha.ToString() + ") " : "") +
                    ""))
                {
                    while (oDataReader.Read())
                    {
                        EstruturaVoucherCompra oVoucherCompra = new EstruturaVoucherCompra();

                        // Dados da Venda
                        oVoucherCompra.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        oVoucherCompra.DataCompra = bd.LerDateTime("DataCompra");
                        oVoucherCompra.Senha = bd.LerString("Senha");
                        oVoucherCompra.Operador = bd.LerString("Operador");
                        oVoucherCompra.Loja = bd.LerString("Loja");
                        oVoucherCompra.BIN = bd.LerString("BIN");
                        oVoucherCompra.PoliticaTroca = bd.LerString("PoliticaTroca");
                        oVoucherCompra.ProcedimentoRetiradaNome = bd.LerString("ProcedimentoRetiradaNome");
                        oVoucherCompra.ProcedimentoRetiradaDescricao = bd.LerString("ProcedimentoRetiradaDescricao");
                        oVoucherCompra.TotaisIngressos = bd.LerDecimal("TotaisIngressos");
                        oVoucherCompra.TotaisTaxaEntrega = bd.LerDecimal("TotaisTaxaEntrega");
                        oVoucherCompra.TotaisTaxaConveniencia = bd.LerDecimal("TotaisTaxaConveniencia");
                        oVoucherCompra.TotaisGeral = bd.LerDecimal("TotaisGeral");

                        // Dados do Cliente
                        oVoucherCompra.Cliente = new EstruturaCliente();
                        oVoucherCompra.Cliente.Nome = bd.LerString("Nome");
                        oVoucherCompra.Cliente.CPF = bd.LerString("CPF");
                        oVoucherCompra.Cliente.EnderecoEntrega = bd.LerString("EnderecoEntrega");
                        oVoucherCompra.Cliente.EnderecoNumeroEntrega = bd.LerString("NumeroEntrega");
                        oVoucherCompra.Cliente.EnderecoComplementoEntrega = bd.LerString("ComplementoEntrega");
                        oVoucherCompra.Cliente.BairroEntrega = bd.LerString("BairroEntrega");
                        oVoucherCompra.Cliente.CidadeEntrega = bd.LerString("CidadeEntrega");
                        oVoucherCompra.Cliente.EstadoEntrega = bd.LerString("EstadoEntrega");
                        oVoucherCompra.Cliente.CEPEntrega = bd.LerString("CEPEntrega");
                        oVoucherCompra.Cliente.TelefoneResidencialDDD = bd.LerString("TelefoneResidencialDDD");
                        oVoucherCompra.Cliente.TelefoneResidencial = bd.LerString("TelefoneResidencial");
                        oVoucherCompra.Cliente.TelefoneCelularDDD = bd.LerString("TelefoneCelularDDD");
                        oVoucherCompra.Cliente.TelefoneCelular = bd.LerString("TelefoneCelular");
                        oVoucherCompra.Cliente.Email = bd.LerString("Email");
                        oVoucherCompra.Cliente.Observacao = bd.LerString("Observacao");

                        // Adiciona o Item a Lista
                        lstVoucherCompra.Add(oVoucherCompra);
                    }
                }

                for (int itemIndex = 0; itemIndex < lstVoucherCompra.Count; itemIndex++)
                {
                    // Objeto Correspondente ao Índice Atual
                    EstruturaVoucherCompra oVoucherCompra = lstVoucherCompra[itemIndex];

                    // Formas de Pagamento
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT " +
                        "   tFormaPagamento.ID AS FormaPagamentoID, " +
                        "   tFormaPagamento.Nome AS FormaPagamentoNome " +
                        "FROM " +
                        "   tVendaBilheteriaFormaPagamento (NOLOCK)  " +
                        "INNER JOIN " +
                        "   tFormaPagamento (NOLOCK)  ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                        "WHERE " +
                        "   (tVendaBilheteriaFormaPagamento.VendaBilheteriaID = " + oVoucherCompra.VendaBilheteriaID + ") " +
                        ""))
                    {
                        // Cria a Lista de Forma de Pagamento
                        List<EstruturaIDNome> lstFormasPagamento = new List<EstruturaIDNome>();

                        while (oDataReader.Read())
                        {
                            EstruturaIDNome oFormaPagamento = new EstruturaIDNome();

                            oFormaPagamento.ID = bd.LerInt("FormaPagamentoID");
                            oFormaPagamento.Nome = bd.LerString("FormaPagamentoNome");

                            lstFormasPagamento.Add(oFormaPagamento);
                        }
                        // Atualiza a Lista de Formas de Pagamento
                        oVoucherCompra.FormasPagamento = lstFormasPagamento;
                    }

                    // Ingressos
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT " +
                        "   tLocal.Nome AS Local, " +
                        "   tEvento.Nome AS Evento, " +
                        "   tPacote.Nome AS Pacote, " +
                        "   tApresentacao.Horario AS Data, " +
                        "   tSetor.Nome AS Setor, " +
                        "   tPreco.Valor AS Preco, " +
                        "   tPreco.Nome AS PrecoNome, " +
                        "   tVendaBilheteriaItem.TaxaConvenienciaValor AS TaxaConveniencia, " +
                        "   tIngresso.Codigo AS LugarAcentoCodigo, " +
                        "   tIngresso.Status " +
                        "FROM " +
                        "   tVendaBilheteriaItem (NOLOCK) " +
                        "INNER JOIN " +
                        "   tIngressoLog (NOLOCK) ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID " +
                        "INNER JOIN " +
                        "   tIngresso (NOLOCK) ON tIngressoLog.IngressoID = tIngresso.ID AND tIngressoLog.VendaBilheteriaID = tIngresso.VendaBilheteriaID " +
                        "INNER JOIN " +
                        "   tLocal (NOLOCK) ON tIngresso.LocalID = tLocal.ID " +
                        "INNER JOIN " +
                        "   tApresentacao (NOLOCK) ON tIngresso.ApresentacaoID = tApresentacao.ID " +
                        "INNER JOIN " +
                        "   tEvento (NOLOCK) ON tIngresso.EventoID = tEvento.ID " +
                        "INNER JOIN " +
                        "   tSetor (NOLOCK) ON tIngresso.SetorID = tSetor.ID " +
                        "INNER JOIN " +
                        "   tPreco (NOLOCK) ON tIngresso.PrecoID = tPreco.ID " +
                        "LEFT OUTER JOIN " +
                        "   tPacote (NOLOCK) ON tPacote.ID = tIngresso.PacoteID " +
                        "WHERE " +
                        "  (tIngresso.VendaBilheteriaID = " + oVoucherCompra.VendaBilheteriaID + ") " + 
                        "ORDER BY " + 
                        "  tIngresso.VendaBilheteriaID"))
                    {
                        // Cria a Lista de Ingressos
                        List<EstruturaIngressoVoucherCompra> lstIngressos = new List<EstruturaIngressoVoucherCompra>();
                        while (oDataReader.Read())
                        {
                            if (bd.LerString("Status") != "A")
                                throw new IngressoException("Não é possível imprimir o voucher quando existem ingressos que já foram trocados ou cancelados.");

                            EstruturaIngressoVoucherCompra oIngresso = new EstruturaIngressoVoucherCompra();
                            oIngresso.Local = bd.LerString("Local");
                            oIngresso.EventoPacote = ((bd.LerString("Evento") != "") ? bd.LerString("Evento") : bd.LerString("Pacote"));
                            oIngresso.Data = bd.LerDateTime("Data");
                            oIngresso.Setor = bd.LerString("Setor");
                            oIngresso.Preco = bd.LerDecimal("Preco");
                            oIngresso.PrecoNome = bd.LerString("PrecoNome");
                            oIngresso.TaxaConveniencia = bd.LerDecimal("TaxaConveniencia");
                            oIngresso.LugarAcentoCodigo = bd.LerString("LugarAcentoCodigo");
                            lstIngressos.Add(oIngresso);
                        }
                        // Atualiza a Lista de Ingressos
                        if (lstIngressos.Count == 0)
                            throw new IngressoException("Não é possível imprimir o voucher quando existem ingressos já foram trocados ou cancelados.");
                        oVoucherCompra.Ingressos = lstIngressos;
                    }

                    // Atualiza o Objeto Correspondente ao Índice Atual
                    lstVoucherCompra[itemIndex] = oVoucherCompra;

                }

                bd.Fechar();
            }
            catch (IngressoException iex)
            {
                throw new Exception(iex.Message);
            }
            catch
            {
                throw new Exception("Não foi possível capturar os Vouchers de Compra");
            }
            finally
            {
                bd.Fechar();
            }


            return lstVoucherCompra;
        }

        /// <summary>
        /// Captura Vouchers de Compra através da Lista de VendaBilheteriaID
        /// </summary>
        /// <param name="lstVendaBilheteriaID">Lista contendo os VendaBilheteriaID</param>
        /// <returns></returns>
        public List<EstruturaVoucherCompra> VoucherCompra(List<int> lstVendaBilheteriaID)
        {
            return VoucherCompra(lstVendaBilheteriaID, null);
        }

        /// <summary>
        /// Captura Vouchers de Compra através do VendaBilheteriaID
        /// </summary>
        /// <param name="lstVendaBilheteriaID">VendaBilheteriaID</param>
        /// <returns></returns>
        public List<EstruturaVoucherCompra> VoucherCompra(int VendaBilheteriaID)
        {
            List<int> lstVendaBilheteriaID = new List<int>();
            lstVendaBilheteriaID.Add(VendaBilheteriaID);
            return VoucherCompra(lstVendaBilheteriaID, null);
        }

        /// <summary>
        /// Captura Vouchers de Compra através da Lista de Senhas
        /// </summary>
        /// <param name="lstVendaBilheteriaID">Lista de contendo as Senhas</param>
        /// <returns></returns>
        public List<EstruturaVoucherCompra> VoucherCompra(List<string> lstSenha)
        {
            return VoucherCompra(null, lstSenha);
        }

        /// <summary>
        /// Captura Vouchers de Compra através da Senha
        /// </summary>
        /// <param name="lstVendaBilheteriaID">Senha</param>
        /// <returns></returns>
        public List<EstruturaVoucherCompra> VoucherCompra(string Senha)
        {
            List<string> lstSenha = new List<string>();
            lstSenha.Add(Senha);
            return VoucherCompra(null, lstSenha);
        }   
    }
}
