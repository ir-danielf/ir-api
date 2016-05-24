using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{

    public class CancelDevolucaoPendente : CancelDevolucaoPendente_B
    {
        /// <summary>
        /// Status = "P"
        /// </summary>
        public const string STATUS_CANCEL_PENDENTE = "P";    //Ingressos pendentes de devolução
        /// <summary>
        /// Status = "D"
        /// </summary>
        public const string STATUS_CANCEL_PROCESSADO = "D";  //Ingressos Devolvidos
        /// <summary>
        /// Status = "A"
        /// </summary>
        public const string STATUS_CANCEL_AUTOMATICO = "A";  //Devolução automática, durante o cancelamento
        /// <summary>
        /// Status = "C"
        /// </summary>
        public const string STATUS_CANCEL_CANCELADO = "C";   //Solicitação foi cancelada
        /// <summary>
        /// Status = "N"
        /// </summary>
        public const string STATUS_CANCEL_NAO_AUTORIZADO = "N";   //Solicitação não autorizada

        /// <summary>
        /// Busca asPendencias a partir das senhas de Venda ou de Cancelamento
        /// </summary>
        /// <param name="senhaVenda"></param>
        /// <returns></returns>
        public DataTable BuscarPendenciasPorSenhaVenda(string senhaVenda)
        {
            DataTable tabela = new DataTable("CancelDevolucaoPendentes");

            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaIDCancel", typeof(int));
                tabela.Columns.Add("SenhaVenda", typeof(string));
                tabela.Columns.Add("SenhaCancel", typeof(string));
                tabela.Columns.Add("DataVenda", typeof(string));
                tabela.Columns.Add("CanalCompra", typeof(string));
                tabela.Columns.Add("Cliente", typeof(string));
                tabela.Columns.Add("StatusCancel", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("DataInsert", typeof(DateTime));

                BD bd = new BD();
                string sql = "   SELECT  dp.ID, " +
                             "           dp.VendaBilheteriaIDCancel, " +
                             "           vbv.Senha SenhaVenda, " +
                             "           vbc.Senha SenhaCancel," +
                             "           vbv.DataVenda, " +
                             "           cn.Nome CanalCompra, " +
                             "           c.Nome Cliente, " +
                             "           dp.StatusCancel, " +
                             "           dp.DataInsert " +
                             "      FROM tCancelDevolucaoPendente dp (nolock) " +
                             "INNER JOIN tVendaBilheteria vbv (nolock) on vbv.id = dp.VendaBilheteriaIDVenda " +
                             " LEFT JOIN tVendaBilheteria vbc (nolock) on vbc.id = dp.VendaBilheteriaIDCancel " +
                             " LEFT JOIN tCaixa cx (nolock) on cx.ID = vbv.CaixaID " +
                             " LEFT JOIN tLoja lj (nolock) on lj.id = cx.LojaID " +
                             " LEFT JOIN tCanal cn (nolock) on cn.ID = lj.CanalID " +
                             " LEFT JOIN tCliente c (nolock) on c.id = vbv.ClienteID " +
                             "     WHERE vbv.Senha = '" + senhaVenda + "' " +
                             "       AND dp.StatusCancel != '" + STATUS_CANCEL_NAO_AUTORIZADO + "' " +
                             "  ORDER BY dp.DataInsert Desc;";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["VendaBilheteriaIDCancel"] = bd.LerInt("VendaBilheteriaIDCancel");
                    linha["SenhaVenda"] = bd.LerString("SenhaVenda");
                    linha["SenhaCancel"] = bd.LerString("SenhaCancel");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["CanalCompra"] = bd.LerString("CanalCompra");
                    linha["Cliente"] = bd.LerString("Cliente");
                    linha["StatusCancel"] = bd.LerString("StatusCancel").Trim();
                    linha["Status"] = bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_PENDENTE
                                      ? "Aguardando Devolução"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_AUTOMATICO
                                      ? "Devolução Automática"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_CANCELADO
                                      ? "Solicitação Cancelada"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_NAO_AUTORIZADO
                                      ? "Não Autorizado"
                                      : "Devolução Efetuada";
                    linha["DataInsert"] = Convert.ToDateTime(bd.LerString("DataInsert"));

                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tabela;

        }

        /// <summary>
        /// Busca asPendencias a partir das senhas de Venda ou de Cancelamento
        /// </summary>
        /// <param name="senhaVenda"></param>
        /// <returns></returns>
        public DataTable BuscarPendenciasPorPendenciaID(int pendenciaID)
        {
            DataTable tabela = new DataTable("CancelDevolucaoPendentes");

            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaIDCancel", typeof(int));
                tabela.Columns.Add("SenhaVenda", typeof(string));
                tabela.Columns.Add("SenhaCancel", typeof(string));
                tabela.Columns.Add("DataVenda", typeof(string));
                tabela.Columns.Add("CanalCompra", typeof(string));
                tabela.Columns.Add("Cliente", typeof(string));
                tabela.Columns.Add("StatusCancel", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("DataInsert", typeof(DateTime));

                BD bd = new BD();
                string sql = "   SELECT  dp.ID, " +
                             "           dp.VendaBilheteriaIDCancel, " +
                             "           vbv.Senha SenhaVenda, " +
                             "           vbc.Senha SenhaCancel," +
                             "           vbv.DataVenda, " +
                             "           cn.Nome CanalCompra, " +
                             "           c.Nome Cliente, " +
                             "           dp.StatusCancel, " +
                             "           dp.DataInsert " +
                             "      FROM tCancelDevolucaoPendente dp (nolock) " +
                             "INNER JOIN tVendaBilheteria vbv (nolock) on vbv.id = dp.VendaBilheteriaIDVenda " +
                             " LEFT JOIN tVendaBilheteria vbc (nolock) on vbc.id = dp.VendaBilheteriaIDCancel " +
                             " LEFT JOIN tCaixa cx (nolock) on cx.ID = vbv.CaixaID " +
                             " LEFT JOIN tLoja lj (nolock) on lj.id = cx.LojaID " +
                             " LEFT JOIN tCanal cn (nolock) on cn.ID = lj.CanalID " +
                             " LEFT JOIN tCliente c (nolock) on c.id = vbv.ClienteID " +
                             "     WHERE dp.ID = " + pendenciaID.ToString() + " " +
                             "       AND dp.StatusCancel != '" + STATUS_CANCEL_NAO_AUTORIZADO + "' " +
                             "  ORDER BY dp.DataInsert Desc;";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["VendaBilheteriaIDCancel"] = bd.LerInt("VendaBilheteriaIDCancel");
                    linha["SenhaVenda"] = bd.LerString("SenhaVenda");
                    linha["SenhaCancel"] = bd.LerString("SenhaCancel");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["CanalCompra"] = bd.LerString("CanalCompra");
                    linha["Cliente"] = bd.LerString("Cliente");
                    linha["StatusCancel"] = bd.LerString("StatusCancel").Trim();
                    linha["Status"] = bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_PENDENTE
                                      ? "Aguardando Devolução"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_AUTOMATICO
                                      ? "Devolução Automática"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_CANCELADO
                                      ? "Solicitação Cancelada"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_NAO_AUTORIZADO
                                      ? "Não Autorizado"
                                      : "Devolução Efetuada";
                    linha["DataInsert"] = Convert.ToDateTime(bd.LerString("DataInsert"));

                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tabela;

        }

        /// <summary>
        /// Busca as pendencias referentes a um cliente
        /// </summary>
        /// <param name="ClienteID"></param>
        /// <returns></returns>
        public DataTable BuscarPendenciasPorClienteID(int ClienteID)
        {
            DataTable tabela = new DataTable("CancelDevolucaoPendentes");

            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaIDCancel", typeof(int));
                tabela.Columns.Add("SenhaVenda", typeof(string));
                tabela.Columns.Add("SenhaCancel", typeof(string));
                tabela.Columns.Add("DataVenda", typeof(string));
                tabela.Columns.Add("CanalCompra", typeof(string));
                tabela.Columns.Add("Cliente", typeof(string));
                tabela.Columns.Add("StatusCancel", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("DataInsert", typeof(DateTime));

                BD bd = new BD();
                string sql = "   SELECT  dp.ID, " +
                             "           dp.VendaBilheteriaIDCancel, " +
                             "           vbv.Senha SenhaVenda, " +
                             "           vbc.Senha SenhaCancel," +
                             "           vbv.DataVenda, " +
                             "           cn.Nome CanalCompra, " +
                             "           c.Nome Cliente, " +
                             "           dp.StatusCancel, " +
                             "           dp.DataInsert " +
                             "      FROM tCancelDevolucaoPendente dp (nolock) " +
                             "INNER JOIN tVendaBilheteria vbv (nolock) on vbv.id = dp.VendaBilheteriaIDVenda " +
                             "left JOIN tVendaBilheteria vbc (nolock) on vbc.id = dp.VendaBilheteriaIDCancel " +
                             "INNER JOIN tCaixa cx (nolock) on cx.ID = vbv.CaixaID " +
                             "INNER JOIN tLoja lj (nolock) on lj.id = cx.LojaID " +
                             "INNER JOIN tCanal cn (nolock) on cn.ID = lj.CanalID " +
                             "INNER JOIN tCliente c (nolock) on c.id = vbv.ClienteID " +
                             "     WHERE c.ID = " + ClienteID.ToString() +
                             "       AND dp.StatusCancel != '" + STATUS_CANCEL_NAO_AUTORIZADO + "' " +
                             "  ORDER BY dp.DataInsert Desc;";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["VendaBilheteriaIDCancel"] = bd.LerInt("VendaBilheteriaIDCancel");
                    linha["SenhaVenda"] = bd.LerString("SenhaVenda");
                    linha["SenhaCancel"] = bd.LerString("SenhaCancel");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["CanalCompra"] = bd.LerString("CanalCompra");
                    linha["Cliente"] = bd.LerString("Cliente");
                    linha["StatusCancel"] = bd.LerString("StatusCancel").Trim();
                    linha["Status"] = bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_PENDENTE
                                      ? "Aguardando Devolução"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_AUTOMATICO
                                      ? "Devolução Automática"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_CANCELADO
                                      ? "Solicitação Cancelada"
                                      : bd.LerString("StatusCancel").Trim() == STATUS_CANCEL_NAO_AUTORIZADO
                                      ? "Não Autorizado"
                                      : "Devolução Efetuada";
                    linha["DataInsert"] = Convert.ToDateTime(bd.LerString("DataInsert"));

                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tabela;

        }

        /// <summary>
        /// Busca os ingresso referentes a uma pendencia
        /// </summary>
        /// <param name="idPendencia"></param>
        /// <param name="Todos"></param>
        /// <returns></returns>
        public DataTable BuscarIngressosPendentes(int idPendencia, bool Todos = false)
        {
            DataTable tabela = new DataTable("IngressosPendentes");

            try
            {
                tabela.Columns.Add("Evento", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("DataEvento", typeof(string));
                tabela.Columns.Add("Setor", typeof(string));
                tabela.Columns.Add("Preco", typeof(string));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("PendenciaIngressoID", typeof(int));
                tabela.Columns.Add("PendenciaID", typeof(int));

                BD bd = new BD();
                string sql = @"SELECT e.Nome Evento, 
                                      l.Nome Local, 
                                      a.Horario DataEvento, 
                                      s.Nome Setor, 
                                      p.Nome Preco, 
                                      i.Codigo,
                                      i.ID as IngressoID,
                                      dpi.ID as PendenciaIngressoID,
                                      dp.ID as PendenciaID
                                 FROM tIngresso i (nolock) 
                           INNER JOIN tCancelDevolucaoPendenteIngresso dpi (nolock) ON i.ID = dpi.IngressoID 
                           INNER JOIN tCancelDevolucaoPendente dp (nolock) ON dp.ID = dpi.CancelDevolucaoPendenteID 
                            LEFT JOIN tEvento e (nolock) ON e.ID = i.EventoID 
                            LEFT JOIN tLocal l (nolock) ON l.ID = i.LocalID 
                            LEFT JOIN tApresentacao a (nolock) ON a.ID = i.ApresentacaoID 
                            LEFT JOIN tSetor s (nolock) ON s.ID = i.SetorID 
                            LEFT JOIN tPreco p (nolock) ON p.ID = i.PrecoID 
                                WHERE dp.ID = " + idPendencia.ToString() + (Todos ? ";" : " AND dp.StatusCancel = '" + STATUS_CANCEL_PENDENTE + "';");

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Local"] = bd.LerString("Local");
                    linha["DataEvento"] = bd.LerStringFormatoDataHora("DataEvento");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Codigo"] = bd.LerString("Codigo");
                    linha["IngressoID"] = bd.LerInt("IngressoID");
                    linha["PendenciaIngressoID"] = bd.LerInt("PendenciaIngressoID");
                    linha["PendenciaID"] = bd.LerInt("PendenciaID");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tabela;
        }
        

		/// <summary>
		/// Busca os ingresso referentes a uma pendencia
		/// </summary>
		/// <param name="idPendencia"></param>
		/// <returns></returns>
		public DataTable BuscarIngressos(int idPendencia)
		{
			DataTable tabela = new DataTable("IngressosPendentes");

			try
			{
				tabela.Columns.Add("Evento", typeof(string));
				tabela.Columns.Add("Local", typeof(string));
				tabela.Columns.Add("DataEvento", typeof(string));
				tabela.Columns.Add("Setor", typeof(string));
				tabela.Columns.Add("Preco", typeof(string));
				tabela.Columns.Add("Codigo", typeof(string));
				tabela.Columns.Add("IngressoID", typeof(int));
				tabela.Columns.Add("PendenciaIngressoID", typeof(int));
				tabela.Columns.Add("PendenciaID", typeof(int));

				BD bd = new BD();
				string sql = @"SELECT e.Nome Evento, 
                                      l.Nome Local, 
                                      a.Horario DataEvento, 
                                      s.Nome Setor, 
                                      p.Nome Preco, 
                                      i.Codigo,
                                      i.ID as IngressoID,
                                      dpi.ID as PendenciaIngressoID,
                                      dp.ID as PendenciaID
                                 FROM tIngresso i (nolock) 
                           INNER JOIN tCancelDevolucaoPendenteIngresso dpi (nolock) ON i.ID = dpi.IngressoID 
                           INNER JOIN tCancelDevolucaoPendente dp (nolock) ON dp.ID = dpi.CancelDevolucaoPendenteID 
                            LEFT JOIN tEvento e (nolock) ON e.ID = i.EventoID 
                            LEFT JOIN tLocal l (nolock) ON l.ID = i.LocalID 
                            LEFT JOIN tApresentacao a (nolock) ON a.ID = i.ApresentacaoID 
                            LEFT JOIN tSetor s (nolock) ON s.ID = i.SetorID 
                            LEFT JOIN tPreco p (nolock) ON p.ID = i.PrecoID 
                                WHERE dp.ID = " + idPendencia.ToString() + @"";

				bd.Consulta(sql);
				while (bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["Evento"] = bd.LerString("Evento");
					linha["Local"] = bd.LerString("Local");
					linha["DataEvento"] = bd.LerStringFormatoDataHora("DataEvento");
					linha["Setor"] = bd.LerString("Setor");
					linha["Preco"] = bd.LerString("Preco");
					linha["Codigo"] = bd.LerString("Codigo");
					linha["IngressoID"] = bd.LerInt("IngressoID");
					linha["PendenciaIngressoID"] = bd.LerInt("PendenciaIngressoID");
					linha["PendenciaID"] = bd.LerInt("PendenciaID");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return tabela;
		}
        internal string StringInserir2()
        {

            try
            {
                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.EnsureCapacity(800000);
                sql.Append("INSERT INTO tCancelDevolucaoPendente(VendaBilheteriaIDVenda, VendaBilheteriaIDCancel, StatusCancel, SupervisorID, NumeroChamado, CaixaID, LojaID, CanalID, UsuarioID, EmpresaID, TipoCancelamento, MotivoCancelamento, SubMotivoCancelamento, DataInsert) ");
                sql.Append("VALUES (@001, @002, '@003', @004, '@005', @006, @007, @008, @009, @010, @011, @012, @013, getdate()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@003", this.StatusCancel.ValorBD);
                sql.Replace("@004", this.SupervisorID.ValorBD);
                sql.Replace("@005", this.NumeroChamado.ValorBD);
                sql.Replace("@006", this.CaixaID.ValorBD);
                sql.Replace("@007", this.LojaID.ValorBD);
                sql.Replace("@008", this.CanalID.ValorBD);
                sql.Replace("@009", this.UsuarioID.ValorBD);
                sql.Replace("@010", this.EmpresaID.ValorBD);
                sql.Replace("@011", this.TipoCancelamento.ValorBD);
                sql.Replace("@012", this.MotivoCancelamento.ValorBD);
                sql.Replace("@013", this.SubMotivoCancelamento.ValorBD);

                return sql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CancelarSolicitacao(BD bd, int pendenciaID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"UPDATE tCancelDevolucaoPendente 
                            SET StatusCancel = '@001',
                                CaixaIDDevolucao = @002,
                                LocalIDDevolucao = @003,
                                LojaIDDevolucao = @004,
                                CanalIDDevolucao = @005,
                                EmpresaIDDevolucao = @006,
                                UsuarioIDDevolucao = @007,
                                VendaBilheteriaIDCancel = @008,
                                DataDevolucao = getdate()
                          ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", pendenciaID.ToString());
            sql.Replace("@001", STATUS_CANCEL_CANCELADO);
            sql.Replace("@002", CaixaIDDevolucao.Valor.ToString());
            sql.Replace("@003", LocalIDDevolucao.Valor.ToString());
            sql.Replace("@004", LojaIDDevolucao.Valor.ToString());
            sql.Replace("@005", CanalIDDevolucao.Valor.ToString());
            sql.Replace("@006", EmpresaIDDevolucao.Valor.ToString());
            sql.Replace("@007", UsuarioIDDevolucao.Valor.ToString());
            sql.Replace("@008", VendaBilheteriaIDCancel.Valor.ToString());

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;
        }

        internal bool EfetivarSolicitacao(BD bd, int pendenciaID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"UPDATE tCancelDevolucaoPendente
                            SET StatusCancel = '@001',
                                CaixaIDDevolucao = @002,
                                LocalIDDevolucao = @003,
                                LojaIDDevolucao = @004,
                                CanalIDDevolucao = @005,
                                EmpresaIDDevolucao = @006,
                                UsuarioIDDevolucao = @007,
                                VendaBilheteriaIDCancel = @008,
                                DataDevolucao = getdate()
                          ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", pendenciaID.ToString());
            sql.Replace("@001", STATUS_CANCEL_PROCESSADO);
            sql.Replace("@002", CaixaIDDevolucao.Valor.ToString());
            sql.Replace("@003", LocalIDDevolucao.Valor.ToString());
            sql.Replace("@004", LojaIDDevolucao.Valor.ToString());
            sql.Replace("@005", CanalIDDevolucao.Valor.ToString());
            sql.Replace("@006", EmpresaIDDevolucao.Valor.ToString());
            sql.Replace("@007", UsuarioIDDevolucao.Valor.ToString());
            sql.Replace("@008", VendaBilheteriaIDCancel.Valor.ToString());

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;
        }
    }

    public class CancelDevolucaoPendenteLista : CancelDevolucaoPendenteLista_B
    {

    }
}
