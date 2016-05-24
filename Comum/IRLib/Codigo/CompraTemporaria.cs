/**************************************************
* Arquivo: CompraTemporaria.cs
* Gerado: 17/11/2009
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using IRCore.Util;

namespace IRLib
{

    public class CompraTemporaria : CompraTemporaria_B
    {
        public CompraTemporaria() { }

        public EstruturaCompraTemporaria RetornarCompraPorClienteSessionID()
        {
            try
            {
                if (this.ClienteID.Valor == 0)
                    throw new Exception("Cliente inválido. Por favor, faça o login");

                EstruturaCompraTemporaria oEstrutura;

                var sql = @"
                                SELECT
                                  tCompraTemporaria.ID,
                                  tCliente.Nome,
                                  tCliente.CPF,
                                  tCliente.RG,
                                  tCliente.CNPJ,
                                  IsNull(tFormaPagamento.ID, 0)       AS FormaPagamentoID,
                                  IsNull(tFormaPagamento.Nome, '--')  AS FormaPagamento,
                                  IsNull(tFormaPagamento.Parcelas, 1) AS Parcelas,
                                  tCompraTemporaria.ValorTotal,
                                  tCompraTemporaria.Bandeira,
                                  tCompraTemporaria.BIN,
                                  tCompraTemporaria.CodigoTrocaFixo,
                                  tCompraTemporaria.SomenteVir,
                                  tCompraTemporaria.SomenteCortesias,
                                  tCompraTemporaria.EnderecoID,
                                  tCompraTemporaria.PDVSelecionado,
                                  tCompraTemporaria.EntregaControleIDSelecionado,
                                  tCompraTemporaria.DataSelecionada,
                                  tCompraTemporaria.EntregaValor,
                                  tEntrega.Nome                       AS TaxaEntrega,
                                  tEvento.Nome                        AS Evento,
                                  tSetor.Nome                         AS Setor,
                                  tPreco.Nome                         AS Preco
                                FROM tCompraTemporaria ( NOLOCK )
                                LEFT JOIN tCliente  ( NOLOCK ) ON tCliente.ID = tCompraTemporaria.ClienteID
                                LEFT JOIN tFormaPagamento ( NOLOCK ) ON tFormaPagamento.ID = tCompraTemporaria.FormaPagamentoID
                                LEFT JOIN tIngresso ( NOLOCK ) ON tIngresso.SessionID = tCompraTemporaria.SessionID
                                LEFT JOIN tEvento ( NOLOCK ) ON tEvento.ID = tIngresso.EventoID
                                LEFT JOIN tSetor ( NOLOCK ) ON tSetor.ID = tIngresso.SetorID
                                LEFT JOIN tPreco ( NOLOCK ) ON tPreco.ID = tIngresso.PrecoID
                                INNER JOIN tEntregaControle ( NOLOCK ) ON tEntregaControle.ID = tCompraTemporaria.EntregaControleIDSelecionado
                                INNER JOIN tEntrega ( NOLOCK ) ON tEntrega.ID = tEntregaControle.EntregaID
                                WHERE tCompraTemporaria.ClienteID = @clienteId AND tCompraTemporaria.SessionID = @sessionId
                          ";

                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("clienteId", this.ClienteID.Valor),
                    new SqlParameter("sessionId", this.SessionID.Valor)
                };

                bd.Consulta(sql, parametros);

                if (bd.Consulta().Read())
                {
                    oEstrutura = new EstruturaCompraTemporaria
                    {
                        ClienteID = this.ClienteID.Valor,
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        CPF = bd.LerString("CPF"),
                        RG = bd.LerString("RG"),
                        CNPJ = bd.LerString("CNPJ"),
                        FormaPagamento = Utilitario.LimparTitulo(bd.LerString("FormaPagamento")),
                        FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                        Parcelas = bd.LerInt("Parcelas"),
                        TaxaEntrega = bd.LerString("TaxaEntrega"),
                        ValorTotal = bd.LerDecimal("ValorTotal"),
                        Bandeira = Utilitario.LimparTitulo(bd.LerString("Bandeira")),
                        BIN = bd.LerInt("BIN"),
                        CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo"),
                        SomenteVir = bd.LerBoolean("SomenteVir"),
                        SomenteCortesias = bd.LerBoolean("SomenteCortesias"),
                        EnderecoID = bd.LerInt("EnderecoID"),
                        PDVSelecionado = bd.LerInt("PDVSelecionado"),
                        EntregaControleIDSelecionado = bd.LerInt("EntregaControleIDSelecionado"),
                        DataSelecionada = bd.LerString("DataSelecionada"),
                        EntregaValor = bd.LerDecimal("EntregaValor"),
                        Encontrado = true,
                        Evento = bd.LerString("Evento"),
                        Setor = bd.LerString("Setor"),
                        PrecoNome = bd.LerString("Preco")
                    };
                }
                else
                {
                    oEstrutura = new EstruturaCompraTemporaria {Encontrado = false};
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

                var sql = @"
                                SELECT
                                  tCompraTemporaria.ID,
                                  tCliente.Nome,
                                  tCliente.CPF,
                                  tCliente.RG,
                                  tCliente.CNPJ,
                                  IsNull(tFormaPagamento.ID, 0)       AS FormaPagamentoID,
                                  tEvento.Nome                        AS Evento,
                                  tSetor.Nome                         AS Setor,
                                  tPreco.Nome                         AS Preco,
                                  IsNull(tFormaPagamento.Nome, '--')  AS FormaPagamento,
                                  IsNull(tFormaPagamento.Parcelas, 1) AS Parcelas,
                                  tCompraTemporaria.ValorTotal,
                                  tCompraTemporaria.Bandeira,
                                  tCompraTemporaria.BIN,
                                  tCompraTemporaria.CodigoTrocaFixo,
                                  tCompraTemporaria.SomenteVir,
                                  tCompraTemporaria.SomenteCortesias,
                                  tCompraTemporaria.EnderecoID,
                                  tCompraTemporaria.PDVSelecionado,
                                  tCompraTemporaria.EntregaControleIDSelecionado,
                                  tCompraTemporaria.DataSelecionada,
                                  tCompraTemporaria.EntregaValor,
                                  tEntrega.Nome                       AS TaxaEntrega
                                FROM tCompraTemporaria ( NOLOCK )
                                  LEFT JOIN tCliente ( NOLOCK ) ON tCliente.ID = tCompraTemporaria.ClienteID
                                  LEFT JOIN tFormaPagamento ( NOLOCK ) ON tFormaPagamento.ID = tCompraTemporaria.FormaPagamentoID
                                  LEFT JOIN tIngresso ( NOLOCK ) ON tIngresso.SessionID = tCompraTemporaria.SessionID
                                  LEFT JOIN tEvento ( NOLOCK ) ON tEvento.ID = tIngresso.EventoID
                                  LEFT JOIN tSetor ( NOLOCK ) ON tSetor.ID = tIngresso.SetorID
                                  LEFT JOIN tPreco ( NOLOCK ) ON tPreco.ID = tIngresso.PrecoID
                                  INNER JOIN tEntregaControle ( NOLOCK ) ON tEntregaControle.ID = tCompraTemporaria.EntregaControleIDSelecionado
                                  INNER JOIN tEntrega ( NOLOCK ) ON tEntrega.ID = tEntregaControle.EntregaID
                                WHERE tCompraTemporaria.SessionID = @sessionId
                          ";

                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("sessionId", this.SessionID.Valor)
                };

                bd.Consulta(sql, parametros);

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
                    throw new Exception("Cliente inválido. Por favor, faça o login");

                IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura;
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

                IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura;
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
            LogUtil.Debug(string.Format("##CompraTemporaria.ConsultandoSeExiste## SESSION {0}", this.SessionID.Valor));

            try
            {
                if (this.ClienteID.Valor == 0)
                    throw new Exception("ClienteID nulo, Session Expirou");

                IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID FROM tCompraTemporaria (NOLOCK) ");
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

                LogUtil.Debug(string.Format("##CompraTemporaria.ConsultandoSeExiste.SUCCESS## SESSION {0}, MSG {1}", this.SessionID.Valor, "Encontrado: " + oEstrutura.Encontrado));

                return oEstrutura;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##CompraTemporaria.ConsultandoSeExiste.EXCEPTION## SESSION {0}", this.SessionID.Valor, ex.Message), ex);

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
                IRLib.ClientObjects.EstruturaCompraTemporaria oEstrutura;
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
            LogUtil.Debug(string.Format("##CompraTemporaria.AtualizandoFormaPagamento## SESSION {0}", this.SessionID.ValorBD));

            try
            {
                var sql = @"
                                UPDATE tCompraTemporaria
                                SET ClienteID = @clienteId, FormaPagamentoID = @formaPagamentoId, Parcelas = @parcelas, ValorTotal = @valorTotal, Bandeira = @bandeira,
                                    SessionID = @sessionId, BIN = @bin, CodigoTrocaFixo = @codigoTrocaFixo, SomenteVir = @somenteVir, SomenteCortesias = @somenteCortesias,
                                    EntregaControleIDSelecionado = @entregaControleIdSelecionado, EntregaValor = @entregaValor, EnderecoID = @enderecoId
                                WHERE ID = @id
                          ";

                var parametros = new List<SqlParameter>()
                {
                    new SqlParameter("clienteId", this.ClienteID.ValorBD),
                    new SqlParameter("formaPagamentoId", this.FormaPagamentoID.ValorBD),
                    new SqlParameter("parcelas", this.Parcelas.ValorBD),
                    new SqlParameter("valorTotal", this.ValorTotal.ValorBD),
                    new SqlParameter("bandeira", this.Bandeira.ValorBD),
                    new SqlParameter("sessionId", this.SessionID.ValorBD),
                    new SqlParameter("bin", this.BIN.ValorBD),
                    new SqlParameter("CodigoTrocaFixo", this.CodigoTrocaFixo.ValorBD),
                    new SqlParameter("somenteVir", this.SomenteVir.ValorBD),
                    new SqlParameter("somenteCortesias", this.SomenteCortesias.ValorBD),
                    new SqlParameter("entregaControleIdSelecionado", this.EntregaControleIDSelecionado.ValorBD),
                    new SqlParameter("entregaValor", this.EntregaValor.ValorBD),
                    new SqlParameter("enderecoId", this.EnderecoID.ValorBD),
                    new SqlParameter("id", this.Control.ID.ToString())
                };

                var linhas = bd.Executar(sql, parametros);

                var result = Convert.ToBoolean(linhas);

                LogUtil.Debug(string.Format("##CompraTemporaria.AtualizandoFormaPagamento.SUCCESS## SESSION {0}, MSG ATUALIZADO {1}", this.SessionID.ValorBD, result));

                return result;

            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##CompraTemporaria.AtualizandoFormaPagamento.EXCEPTION## SESSION {0}, MSG {1}", this.SessionID.ValorBD, ex.Message), ex);

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
