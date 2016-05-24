/**************************************************
* Arquivo: CompraTemporaria.cs
* Gerado: 17/11/2009
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib.Paralela
{

    public class CompraTemporaria : CompraTemporaria_B
    {
        public CompraTemporaria() { }

        public EstruturaCompraTemporaria RetornarCompraPorClienteSessionID()
        {
            try
            {
                if (this.ClienteID.Valor == 0)
                    throw new Exception("ClienteID nulo, Session Expirou");

                EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT tCompraTemporaria.ID, tCliente.Nome, tCliente.CPF, tCliente.RG, tCliente.CNPJ, IsNull(tFormaPagamento.ID, 0) AS FormaPagamentoID, ");
                stbSQL.Append("IsNull(tFormaPagamento.Nome, '--') AS FormaPagamento, IsNull(tFormaPagamento.Parcelas, 1) AS Parcelas, tCompraTemporaria.ValorTotal, tCompraTemporaria.Bandeira, tCompraTemporaria.BIN, tCompraTemporaria.CodigoTrocaFixo,  ");
                stbSQL.Append("tCompraTemporaria.SomenteVir, tCompraTemporaria.SomenteCortesias, tCompraTemporaria.EnderecoID , tCompraTemporaria.PDVSelecionado , tCompraTemporaria.EntregaControleIDSelecionado, ");
                stbSQL.Append("tCompraTemporaria.DataSelecionada, tCompraTemporaria.EntregaValor, tEntrega.Nome as TaxaEntrega, tEvento.Nome as Evento, tSetor.Nome as Setor, tPreco.Nome as Preco ");
                stbSQL.Append("FROM tCompraTemporaria (NOLOCK) ");
                stbSQL.Append("LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tCompraTemporaria.ClienteID ");
                stbSQL.Append("LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = tCompraTemporaria.FormaPagamentoID ");
                stbSQL.Append("LEFT JOIN tIngresso (NOLOCK) ON tIngresso.ClienteID = tCliente.ID ");
                stbSQL.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID ");
                stbSQL.Append("LEFT JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID ");
                stbSQL.Append("LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID ");
                stbSQL.Append("INNER JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tCompraTemporaria.EntregaControleIDSelecionado ");
                stbSQL.Append("INNER JOIN tEntrega (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID ");
                stbSQL.Append("WHERE tCompraTemporaria.ClienteID = " + this.ClienteID.Valor);
                stbSQL.Append(" AND tCompraTemporaria.SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(stbSQL.ToString());

                if (bd.Consulta().Read())
                {
                    oEstrutura = new EstruturaCompraTemporaria();
                    oEstrutura.ClienteID = this.ClienteID.Valor;
                    oEstrutura.ID = bd.LerInt("ID");
                    oEstrutura.Nome = bd.LerString("Nome");
                    oEstrutura.CPF = bd.LerString("CPF");
                    oEstrutura.RG = bd.LerString("RG");
                    oEstrutura.CNPJ = bd.LerString("CNPJ");
                    oEstrutura.FormaPagamento = Utilitario.LimparTitulo(bd.LerString("FormaPagamento"));
                    oEstrutura.FormaPagamentoID = bd.LerInt("FormaPagamentoID");
                    oEstrutura.Parcelas = bd.LerInt("Parcelas");
                    oEstrutura.TaxaEntrega = bd.LerString("TaxaEntrega");
                    oEstrutura.ValorTotal = bd.LerDecimal("ValorTotal");
                    oEstrutura.Bandeira = Utilitario.LimparTitulo(bd.LerString("Bandeira"));
                    oEstrutura.BIN = bd.LerInt("BIN");
                    oEstrutura.CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    oEstrutura.SomenteVir = bd.LerBoolean("SomenteVir");
                    oEstrutura.SomenteCortesias = bd.LerBoolean("SomenteCortesias");
                    oEstrutura.EnderecoID = bd.LerInt("EnderecoID");
                    oEstrutura.PDVSelecionado = bd.LerInt("PDVSelecionado");
                    oEstrutura.EntregaControleIDSelecionado = bd.LerInt("EntregaControleIDSelecionado");
                    oEstrutura.DataSelecionada = bd.LerString("DataSelecionada");
                    oEstrutura.EntregaValor = bd.LerDecimal("EntregaValor");
                    oEstrutura.Encontrado = true;
                    oEstrutura.Evento = bd.LerString("Evento");
                    oEstrutura.Setor = bd.LerString("Setor");
                    oEstrutura.PrecoNome = bd.LerString("Preco");
                }
                else
                {
                    oEstrutura = new EstruturaCompraTemporaria();
                    oEstrutura.Encontrado = false;
                }
                return oEstrutura;
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

        public EstruturaCompraTemporaria RetornarCompraPorSessionID()
        {
            try
            {
                EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT tCompraTemporaria.ID, tCliente.Nome, tCliente.CPF, tCliente.RG, tCliente.CNPJ, IsNull(tFormaPagamento.ID, 0) AS FormaPagamentoID, ");
                stbSQL.Append("tEvento.Nome as Evento, tSetor.Nome as Setor, tPreco.Nome as Preco, IsNull(tFormaPagamento.Nome, '--') AS FormaPagamento, IsNull(tFormaPagamento.Parcelas, 1) AS Parcelas, tCompraTemporaria.ValorTotal, tCompraTemporaria.Bandeira, tCompraTemporaria.BIN, tCompraTemporaria.CodigoTrocaFixo,  ");
                stbSQL.Append("tCompraTemporaria.SomenteVir, tCompraTemporaria.SomenteCortesias, tCompraTemporaria.EnderecoID , tCompraTemporaria.PDVSelecionado , tCompraTemporaria.EntregaControleIDSelecionado, ");
                stbSQL.Append("tCompraTemporaria.DataSelecionada, tCompraTemporaria.EntregaValor, tEntrega.Nome as TaxaEntrega  ");
                stbSQL.Append("FROM tCompraTemporaria (NOLOCK) ");
                stbSQL.Append("LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tCompraTemporaria.ClienteID ");
                stbSQL.Append("LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = tCompraTemporaria.FormaPagamentoID ");
                stbSQL.Append("LEFT JOIN tIngresso (NOLOCK) ON tIngresso.ClienteID = tCliente.ID ");
                stbSQL.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID ");
                stbSQL.Append("LEFT JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID ");
                stbSQL.Append("LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID ");
                stbSQL.Append("INNER JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tCompraTemporaria.EntregaControleIDSelecionado ");
                stbSQL.Append("INNER JOIN tEntrega (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID ");
                stbSQL.Append("WHERE tCompraTemporaria.SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(stbSQL.ToString());

                if (bd.Consulta().Read())
                {
                    oEstrutura = new EstruturaCompraTemporaria();
                    oEstrutura.ClienteID = this.ClienteID.Valor;
                    oEstrutura.ID = bd.LerInt("ID");
                    oEstrutura.Nome = bd.LerString("Nome");
                    oEstrutura.CPF = bd.LerString("CPF");
                    oEstrutura.RG = bd.LerString("RG");
                    oEstrutura.CNPJ = bd.LerString("CNPJ");
                    oEstrutura.FormaPagamento = Utilitario.LimparTitulo(bd.LerString("FormaPagamento"));
                    oEstrutura.FormaPagamentoID = bd.LerInt("FormaPagamentoID");
                    oEstrutura.Parcelas = bd.LerInt("Parcelas");
                    oEstrutura.TaxaEntrega = bd.LerString("TaxaEntrega");
                    oEstrutura.ValorTotal = bd.LerDecimal("ValorTotal");
                    oEstrutura.Bandeira = Utilitario.LimparTitulo(bd.LerString("Bandeira"));
                    oEstrutura.BIN = bd.LerInt("BIN");
                    oEstrutura.CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    oEstrutura.SomenteVir = bd.LerBoolean("SomenteVir");
                    oEstrutura.SomenteCortesias = bd.LerBoolean("SomenteCortesias");
                    oEstrutura.EnderecoID = bd.LerInt("EnderecoID");
                    oEstrutura.PDVSelecionado = bd.LerInt("PDVSelecionado");
                    oEstrutura.EntregaControleIDSelecionado = bd.LerInt("EntregaControleIDSelecionado");
                    oEstrutura.DataSelecionada = bd.LerString("DataSelecionada");
                    oEstrutura.EntregaValor = bd.LerDecimal("EntregaValor");
                    oEstrutura.Encontrado = true;
                    oEstrutura.Evento = bd.LerString("Evento");
                    oEstrutura.Setor = bd.LerString("Setor");
                    oEstrutura.PrecoNome = bd.LerString("Preco");
                }
                else
                {
                    oEstrutura = new EstruturaCompraTemporaria();
                    oEstrutura.Encontrado = false;
                }
                return oEstrutura;
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

        public EstruturaCompraTemporaria RetornarCompraParcial()
        {
            try
            {
                if (this.ClienteID.Valor == 0)
                    throw new Exception("ClienteID nulo, Session Expirou");

                IRLib.Paralela.ClientObjects.EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID, Bandeira, Parcelas, ValorTotal FROM tCompraTemporaria ");
                stbSQL.Append("WHERE ClienteID = " + this.ClienteID.Valor);
                stbSQL.Append(" AND SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(stbSQL.ToString());

                if (bd.Consulta().Read())
                {
                    oEstrutura = new EstruturaCompraTemporaria();
                    oEstrutura.ID = bd.LerInt("ID");
                    oEstrutura.Bandeira = bd.LerString("Bandeira");
                    oEstrutura.Parcelas = bd.LerInt("Parcelas");
                    oEstrutura.ValorTotal = bd.LerDecimal("ValorTotal");
                    return oEstrutura;
                }
                else
                    throw new Exception("Nenhuma compra parcial encontrada");
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

        public List<EstruturaVirNomePresenteado> RetornaListaPresenteado()
        {
            try
            {
                List<EstruturaVirNomePresenteado> RetornoPresenteado = new List<EstruturaVirNomePresenteado>();
                EstruturaVirNomePresenteado itemRetorno;

                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT ValeIngressoID, NomePresenteado FROM tCompraTemporariaValeIngresso (NOLOCK)  ");
                sql.Append("WHERE ClienteID = @001 AND SessionID = '@002' ");

                sql.Replace("@001", this.ClienteID.Valor.ToString());
                sql.Replace("@002", this.SessionID.Valor);

                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    itemRetorno = new EstruturaVirNomePresenteado();

                    itemRetorno.ID = bd.LerInt("ValeIngressoID");
                    itemRetorno.NomePresenteado = bd.LerString("NomePresenteado");

                    RetornoPresenteado.Add(itemRetorno);
                }



                return RetornoPresenteado;

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

        public void InsereCompraTemporariaValeIngresso(List<EstruturaVirNomePresenteado> estrutura)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("Select ID From tCompraTemporariaValeIngresso (NOLOCK) Where ClienteID = " + this.ClienteID.Valor.ToString() + " AND SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    bd.Executar("DELETE from tCompraTemporariaValeIngresso WHERE ClienteID = " + this.ClienteID.Valor.ToString() + " AND SessionID = '" + this.SessionID.Valor + "'");

                foreach (EstruturaVirNomePresenteado item in estrutura)
                {
                    sql = new StringBuilder();
                    sql.Append("INSERT INTO tCompraTemporariaValeIngresso(ValeIngressoID, NomePresenteado, ClienteID, SessionID ) ");
                    sql.Append("VALUES (@001, '@002', @003, '@004')");

                    sql.Replace("@001", item.ID.ToString());
                    sql.Replace("@002", item.NomePresenteado);
                    sql.Replace("@003", this.ClienteID.Valor.ToString());
                    sql.Replace("@004", this.SessionID.Valor);

                    bd.Executar(sql.ToString());
                }
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

        public EstruturaCompraTemporaria RetornarCompraDadosEntrega()
        {
            try
            {
                if (this.ClienteID.Valor == 0)
                    throw new Exception("ClienteID nulo, Session Expirou");

                IRLib.Paralela.ClientObjects.EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID, EnderecoID, PDVSelecionado, EntregaControleIDSelecionado,DataSelecionada,EntregaValor FROM tCompraTemporaria ");
                stbSQL.Append("WHERE ClienteID = " + this.ClienteID.Valor);
                stbSQL.Append(" AND SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(stbSQL.ToString());

                if (bd.Consulta().Read())
                {
                    oEstrutura = new EstruturaCompraTemporaria();
                    oEstrutura.ID = bd.LerInt("ID");
                    oEstrutura.EnderecoID = bd.LerInt("EnderecoID");
                    oEstrutura.PDVSelecionado = bd.LerInt("PDVSelecionado");
                    oEstrutura.EntregaControleIDSelecionado = bd.LerInt("EntregaControleIDSelecionado");
                    oEstrutura.DataSelecionada = bd.LerString("DataSelecionada");
                    oEstrutura.EntregaValor = bd.LerDecimal("EntregaValor");
                    return oEstrutura;
                }
                else
                    throw new Exception("Nenhuma compra parcial encontrada");
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

        public EstruturaCompraTemporaria ConsultarSeExiste()
        {
            try
            {
                if (this.ClienteID.Valor == 0)
                    throw new Exception("ClienteID nulo, Session Expirou");

                IRLib.Paralela.ClientObjects.EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID FROM tCompraTemporaria ");
                stbSQL.Append("WHERE ClienteID = " + this.ClienteID.Valor);
                stbSQL.Append(" AND SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(stbSQL.ToString());

                oEstrutura = new EstruturaCompraTemporaria();
                if (bd.Consulta().Read())
                {
                    oEstrutura.ID = bd.LerInt("ID");
                    oEstrutura.Encontrado = true;
                }
                else
                {
                    oEstrutura.Encontrado = false;
                }
                return oEstrutura;
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

        public EstruturaCompraTemporaria ConsultarSeExisteSessionID()
        {
            try
            {
                IRLib.Paralela.ClientObjects.EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID FROM tCompraTemporaria ");
                stbSQL.Append("WHERE SessionID = '" + this.SessionID.Valor + "'");

                bd.Consulta(stbSQL.ToString());

                oEstrutura = new EstruturaCompraTemporaria();
                if (bd.Consulta().Read())
                {
                    oEstrutura.ID = bd.LerInt("ID");
                    oEstrutura.Encontrado = true;
                }
                else
                {
                    oEstrutura.Encontrado = false;
                }
                return oEstrutura;
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

        public void ExcluirClienteSessionID(int clienteID, string sessionID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("DELETE FROM tCompraTemporaria ");
                stbSQL.Append("WHERE ClienteID = " + clienteID);
                stbSQL.Append(" AND SessionID = '" + sessionID + "'");
                bd.Executar(stbSQL);
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

        public bool AtualizarEntrega()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCompraTemporaria SET EnderecoID = @003, PDVSelecionado = @004, EntregaControleIDSelecionado = @005, DataSelecionada = '@006', EntregaValor = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@003", this.EnderecoID.ValorBD);
                sql.Replace("@004", this.PDVSelecionado.ValorBD);
                sql.Replace("@005", this.EntregaControleIDSelecionado.ValorBD);
                sql.Replace("@006", this.DataSelecionada.ValorBD);
                sql.Replace("@007", this.EntregaValor.ValorBD);


                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

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

        public bool AtualizaFormaPagamento()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCompraTemporaria SET ClienteID = @001, FormaPagamentoID = @002,Parcelas = @008, ValorTotal = '@009', Bandeira = '@010', SessionID = '@011', BIN = @012, CodigoTrocaFixo = '@013', SomenteVir = '@014', SomenteCortesias = '@015' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@008", this.Parcelas.ValorBD);
                sql.Replace("@009", this.ValorTotal.ValorBD);
                sql.Replace("@010", this.Bandeira.ValorBD);
                sql.Replace("@011", this.SessionID.ValorBD);
                sql.Replace("@012", this.BIN.ValorBD);
                sql.Replace("@013", this.CodigoTrocaFixo.ValorBD);
                sql.Replace("@014", this.SomenteVir.ValorBD);
                sql.Replace("@015", this.SomenteCortesias.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

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

    }

    public class CompraTemporariaLista : CompraTemporariaLista_B
    {

        public CompraTemporariaLista() { }

    }

}
