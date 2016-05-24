using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    /// <summary>
    /// Gerenciador do IngressoHistoricoGerenciador
    /// </summary>
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class IngressoHistoricoGerenciadorParalela : MarshalByRefObject
    {

        public const string ACAO = "Ação";

        public IngressoHistoricoGerenciadorParalela() { }

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        public static DataTable EstruturaHistorico()
        {

            DataTable tabela = new DataTable("Grid");

            tabela.Columns.Add("IngressoID", typeof(int));
            tabela.Columns.Add("IngressoLogID", typeof(int));
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Data", typeof(string));

            tabela.Columns.Add("Cliente", typeof(string));
            tabela.Columns.Add("Senha", typeof(string));
            tabela.Columns.Add("Ação", typeof(string));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("Preço", typeof(string));
            tabela.Columns.Add("Cortesia", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("Bloqueio", typeof(string));
            tabela.Columns.Add("Usuario", typeof(string));
            tabela.Columns.Add("Loja", typeof(string));
            tabela.Columns.Add("Obs", typeof(string));
            tabela.Columns.Add("Motivo", typeof(string));
            tabela.Columns.Add("Codigo", typeof(string));
            tabela.Columns.Add("NotaFiscalEstabelecimento", typeof(string));
            tabela.Columns.Add("VendaBilheteriaID", typeof(int));

            tabela.Columns.Add("Local", typeof(string));
            tabela.Columns.Add("Apresentacao", typeof(string));
            tabela.Columns.Add("ApresentacaoID", typeof(int));
            tabela.Columns.Add("Setor", typeof(string));

            tabela.Columns.Add("SenhaCancelamento", typeof(string));
            tabela.Columns.Add("CodigoBarra", typeof(string));

            tabela.Columns.Add("StatusDetalhado", typeof(string));
            tabela.Columns.Add("Supervisor", typeof(string));
            tabela.Columns.Add("PagamentoProcessado", typeof(bool));

            tabela.Columns.Add("CNPJ", typeof(string));
            tabela.Columns.Add("NomeFantasia", typeof(string));
            tabela.Columns.Add("EntregaID", typeof(int));
            tabela.Columns.Add("Entrega", typeof(string));
            tabela.Columns.Add("Periodo", typeof(string));
            tabela.Columns.Add("DataAgenda", typeof(DateTime));
            tabela.Columns.Add("AreaEntrega", typeof(string));
            tabela.Columns.Add("EnderecoEntrega", typeof(int));
            tabela.Columns.Add("PDVEntrega", typeof(string));

            tabela.Columns.Add("TipoCodigoBarra", typeof(string));
            tabela.Columns.Add("ApresentacaoSetorID", typeof(int));

            tabela.Columns.Add("ClienteID", typeof(int));

            tabela.Columns.Add("Email", typeof(string));

            tabela.Columns.Add("Canal", typeof(string));

            return tabela;

        }

        public static DataTable EstruturaIngressoEmail()
        {

            DataTable tabela = new DataTable("Ingressos");

            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Local", typeof(string));
            tabela.Columns.Add("Apresentacao", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Codigo", typeof(string));
            tabela.Columns.Add("LugarMarcado", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("LugarID", typeof(int));

            return tabela;
        }

        public DataTable PesquisarCodigoBarras(string codigoBarra)
        {

            BD bd = null;
            DataTable tabela = EstruturaHistorico();
            int ingressoID;

            try
            {

                bd = new BD();
                ingressoID = 0;

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tIngresso.ID " +
                    "FROM " +
                    "   tIngresso (NOLOCK) " +
                    "WHERE " +
                    "   (CodigoBarra = '" + codigoBarra + "' OR CodigoBarraCliente='" + codigoBarra + "') " +
                    "ORDER BY " +
                    "   tIngresso.ID"))
                {
                    if (!oDataReader.Read())
                        throw new ApplicationException("Código de Barras não encontrado!");

                    ingressoID = bd.LerInt("ID");

                    if (ingressoID < 1)
                        throw new ApplicationException("Código de Barras não encontrado!");

                    Ingresso ingresso = new Ingresso();
                    ingresso.Control.ID = ingressoID;

                    tabela = ingresso.Historico();

                }

                return AssinaturaBancoIngresso.VerificarSeExisteBancoIngresso(ingressoID, tabela);

            }
            finally
            {
                if (bd != null)
                    bd.Fechar();
            }



        }

        public DataTable PesquisarCodigoIngresso(int apresentacaoSetorID, string codigo)
        {

            BD bd = null;
            DataTable tabela = EstruturaHistorico();
            int ingressoID;

            try
            {

                bd = new BD();
                ingressoID = 0;

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tIngresso.ID " +
                    "FROM " +
                    "   tIngresso (NOLOCK) " +
                    "WHERE " +
                    "   (tIngresso.ApresentacaoSetorID = " + apresentacaoSetorID + ") " +
                    "AND " +
                    "   (tIngresso.Codigo = '" + codigo + "') " +
                    "ORDER BY " +
                    "   tIngresso.ID"))
                {
                    if (!oDataReader.Read())
                        throw new ApplicationException("Código do Ingresso não encontrado!");

                    ingressoID = bd.LerInt("ID");

                    if (ingressoID < 1)
                        throw new ApplicationException("Código do Ingresso não encontrado!");

                    int contadorIngressos = 1;

                    while (oDataReader.Read())
                    {
                        contadorIngressos++;
                    }

                    if (contadorIngressos > 1)
                        throw new IngressoHistoricoGerenciadorException("Apresentação possui mais de 1 lugar com código " + codigo);

                    Ingresso ingresso = new Ingresso();
                    ingresso.Control.ID = ingressoID;

                    tabela = ingresso.Historico();

                }

                return AssinaturaBancoIngresso.VerificarSeExisteBancoIngresso(ingressoID, tabela);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (bd != null)
                    bd.Fechar();
            }

        }

        public DataTable PesquisarSenha(string senha)
        {
            DataTable tabela = EstruturaHistorico();
            BD bd = new BD();
            string strStatus = "";
            bool pagamentoProcessado = false;
            int intVendaBilheteriaID = 0;
            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tVendaBilheteria.ID, " +
                    "   tVendaBilheteria.Status, " +
                    "   tVendaBilheteria.PagamentoProcessado " +
                    "FROM " +
                    "  tVendaBilheteria (NOLOCK) " +
                    "WHERE " +
                    "  (Senha = '" + senha + "')"

                    ))
                {
                    if (!oDataReader.Read())
                        throw new ApplicationException("Venda não encontrada!");

                    if (bd.LerInt("ID") < 1)
                        throw new ApplicationException("Venda não encontrada!");

                    intVendaBilheteriaID = bd.LerInt("ID");

                    strStatus = bd.LerString("Status");
                    pagamentoProcessado = bd.LerBoolean("PagamentoProcessado");
                }

                bd.Fechar();

                StringBuilder strSql = new StringBuilder();

                strSql.Append(" SELECT ");
                strSql.Append("( ");
                strSql.Append(" SELECT  ");
                strSql.Append(" COUNT(tIngressoLog.ID) ");
                strSql.Append(" FROM ");
                strSql.Append(" tIngressoLog (NOLOCK) ");
                strSql.Append(" WHERE ");
                strSql.Append(" (tIngressoLog.VendaBilheteriaID = " + intVendaBilheteriaID + ") ");
                strSql.Append(" ) AS TotIngressos, ");
                strSql.Append(" i.ID AS IngressoID, ");
                strSql.Append(" il.VendaBilheteriaID, ");
                strSql.Append(" i.Codigo, ");
                strSql.Append(" il.IngressoID, ");
                strSql.Append(" ev.ID EventoID, ");
                strSql.Append(" ev.Nome Evento, ");
                strSql.Append(" il.ID, ");
                strSql.Append(" il.[TimeStamp], ");
                strSql.Append(" il.Acao, ");
                strSql.Append(" il.Obs, ");
                strSql.Append(" p.Nome AS Preco, ");
                strSql.Append(" p.Valor, ");
                strSql.Append(" b.Nome AS Bloqueio, ");
                strSql.Append(" c.Nome AS Cortesia, ");
                strSql.Append(" lo.Nome AS Loja, ");
                strSql.Append(" i.Status, ");
                strSql.Append(" p.ID AS PrecoID, ");
                strSql.Append(" u.Nome AS Usuario, ");
                strSql.Append(" ci.Nome AS Cliente, ");
                strSql.Append(" vb.NotaFiscalEstabelecimento, ");
                strSql.Append(" vb.Senha, ");
                strSql.Append(" l.Nome AS Local, ");
                strSql.Append(" a.Horario AS Apresentacao, ");
                strSql.Append(" a.ID AS ApresentacaoID, ");
                strSql.Append(" s.Nome AS Setor, ");
                strSql.Append(" il.CodigoBarra, ");
                strSql.Append(" en.Tipo AS TaxaEntregaTipo, ");
                strSql.Append(" IsNull(us.Nome, ' - ') as  Supervisor, ");
                strSql.Append(" ep.Nome AS PeriodoAgenda ,");
                strSql.Append(" en.Nome AS EntregaNome ,");
                strSql.Append(" ea.Data AS DataAgenda ,");
                strSql.Append(" ear.Nome AS AreaEntrega ,");
                strSql.Append(" ISNULL(vb.ClienteEnderecoID,0) AS EnderecoEntrega , ");
                strSql.Append(" pdv.Nome as PDVEntrega, ");
                strSql.Append(" ev.TipoCodigoBarra, i.ApresentacaoSetorID ");
                strSql.Append(" ,ci.Email ");
                strSql.Append(" ,cl.Nome AS Canal ");
                strSql.Append(" ,en.ID AS EntregaID ");
                strSql.Append(" FROM tIngressoLog il (NOLOCK) ");
                strSql.Append(" LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = il.VendaBilheteriaID  ");
                strSql.Append(" INNER JOIN tUsuario u (NOLOCK) ON u.ID = il.UsuarioID ");
                strSql.Append(" LEFT JOIN tUsuario us (NOLOCK) ON us.ID = il.SupervisorID ");
                strSql.Append(" LEFT OUTER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID ");
                strSql.Append(" INNER JOIN tIngresso i (NOLOCK) ON i.ID = IngressoID ");
                strSql.Append(" INNER JOIN tEvento ev (NOLOCK) ON ev.ID = i.EventoID ");
                strSql.Append(" INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID ");
                strSql.Append(" INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID ");
                strSql.Append(" INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID ");
                strSql.Append(" LEFT JOIN tLoja lo (NOLOCK) ON lo.ID = il.LojaID ");
                strSql.Append(" LEFT JOIN tCanal cl (NOLOCK) ON cl.ID = il.CanalID  ");
                strSql.Append(" LEFT JOIN tCliente ci (NOLOCK) ON ci.ID = vb.ClienteID ");
                strSql.Append(" LEFT JOIN tCortesia c (NOLOCK) ON c.ID = il.CortesiaID ");
                strSql.Append(" LEFT JOIN tBloqueio b (NOLOCK) ON b.ID = il.BloqueioID ");
                strSql.Append(" LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                strSql.Append(" LEFT JOIN tEntrega en (NOLOCK) ON ec.EntregaID = en.ID ");
                strSql.Append(" LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ec.PeriodoID = ep.ID ");
                strSql.Append(" LEFT JOIN tEntregaArea ear (NOLOCK) ON ec.EntregaAreaID = ear.ID ");
                strSql.Append(" LEFT JOIN tEntregaAgenda ea (NOLOCK) ON vb.EntregaAgendaID = ea.ID ");
                strSql.Append(" LEFT JOIN tPontoVenda pdv (NOLOCK) ON vb.PdvID = pdv.ID ");
                strSql.Append(" WHERE ");
                strSql.Append(" ( ");
                strSql.Append(" il.IngressoID IN ( ");
                strSql.Append(" SELECT ");
                strSql.Append(" IngressoID ");
                strSql.Append(" FROM  ");
                strSql.Append(" tIngressoLog (NOLOCK) ");
                strSql.Append(" WHERE ");
                strSql.Append(" (VendaBilheteriaID = " + intVendaBilheteriaID + ") ");
                strSql.Append(" ) ");
                strSql.Append(" ) ");
                strSql.Append(" ORDER BY ");
                strSql.Append(" il.IngressoID, ");
                strSql.Append(" il.ID ");

                using (IDataReader oDataReader = bd.Consulta(strSql.ToString()))
                {
                    int ingressoID = 0;
                    bool blnInserirDados = false;
                    string strSenhaVenda = "";
                    DataRow linha = null;

                    while (oDataReader.Read())
                    {
                        if (ingressoID != bd.LerInt("IngressoID"))
                        {
                            ingressoID = bd.LerInt("IngressoID");
                            blnInserirDados = false;
                            strSenhaVenda = "";
                        }

                        if (bd.LerString("Acao") == IngressoLog.VENDER)
                            strSenhaVenda = bd.LerString("Senha");

                        // Se a operação for venda, Liga o Flag de Inserir Dados
                        if (((bd.LerString("Acao") == IngressoLog.VENDER || bd.LerString("Acao") == IngressoLog.PRE_RESERVA) && strStatus != VendaBilheteria.CANCELADO && bd.LerInt("VendaBilheteriaID") == intVendaBilheteriaID) || (bd.LerString("Acao") == IngressoLog.CANCELAR && strStatus == VendaBilheteria.CANCELADO && bd.LerInt("VendaBilheteriaID") == intVendaBilheteriaID))
                            blnInserirDados = true;

                        // Insere as Informações dos Ingressos
                        if (blnInserirDados)
                        {
                            if (strSenhaVenda == "")
                                if (strStatus == VendaBilheteria.PRE_RESERVADO)
                                    strSenhaVenda = "-";
                                else
                                    strSenhaVenda = bd.LerString("Senha");

                            linha = tabela.NewRow();
                            linha["IngressoID"] = bd.LerInt("IngressoID");
                            linha["IngressoLogID"] = bd.LerInt("ID");
                            linha["Codigo"] = bd.LerString("Codigo");
                            linha["Data"] = bd.LerStringFormatoDataHora("TimeStamp");
                            linha["Cliente"] = bd.LerString("Cliente");
                            linha["Evento"] = bd.LerString("Evento");
                            linha["EventoID"] = bd.LerString("EventoID");
                            linha["ApresentacaoID"] = bd.LerString("ApresentacaoID");
                            linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                            linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                            // Se for uma senha de cancelamento alimentar a Senha de Venda e Senha de Cancelamento
                            if (strStatus == VendaBilheteria.CANCELADO)
                            {
                                linha["Senha"] = strSenhaVenda;
                                linha["SenhaCancelamento"] = bd.LerString("Senha");
                            }
                            else
                            {
                                // Se for uma senha de venda, armazenar senha de venda, e somente a senha de cancalemento se for uma ação cancelar                                                                
                                linha["Senha"] = strSenhaVenda;

                                if (bd.LerString("Acao") == IngressoLog.CANCELAR)
                                    linha["SenhaCancelamento"] = bd.LerString("Senha");
                                else
                                    linha["SenhaCancelamento"] = " - ";
                            }

                            linha["Ação"] = IngressoLog.AcaoDescritiva(bd.LerString("Acao"));
                            linha["Status"] = Ingresso.StatusDescritivo(bd.LerString("Status"));
                            linha["StatusDetalhado"] = Ingresso.StatusDetalhado(bd.LerString("Status"), bd.LerString("TaxaEntregaTipo"));
                            linha["PrecoID"] = bd.LerInt("PrecoID");
                            linha["Preço"] = bd.LerString("Preco");
                            linha["Loja"] = bd.LerString("Loja");
                            linha["PagamentoProcessado"] = pagamentoProcessado;
                            linha["VendaBilheteriaID"] = bd.LerString("VendaBilheteriaID");
                            linha["EntregaID"] = bd.LerInt("EntregaID");
                            linha["Entrega"] = bd.LerString("EntregaNome");
                            linha["Periodo"] = bd.LerString("PeriodoAgenda");
                            linha["DataAgenda"] = bd.LerDateTime("DataAgenda");
                            linha["AreaEntrega"] = bd.LerString("AreaEntrega");
                            linha["EnderecoEntrega"] = bd.LerInt("EnderecoEntrega");
                            linha["PDVEntrega"] = bd.LerString("PDVEntrega");

                            if (bd.LerString("Acao") == IngressoLog.VENDER || bd.LerString("Acao") == IngressoLog.CANCELAR)
                                linha["Valor"] = bd.LerDecimal("Valor");

                            if (bd.LerString("Cortesia") != "")
                                linha["Cortesia"] = bd.LerString("Cortesia");
                            if (bd.LerString("Bloqueio") != "")
                                linha["Bloqueio"] = bd.LerString("Bloqueio");
                            linha["Usuario"] = bd.LerString("Usuario");
                            linha["Obs"] = bd.LerString("Obs");

                            linha["Local"] = bd.LerString("Local");
                            linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                            linha["Setor"] = bd.LerString("Setor");
                            linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                            linha["Supervisor"] = bd.LerString("Supervisor");
                            linha["Email"] = bd.LerString("Email");
                            linha["Canal"] = bd.LerString("Canal");

                            tabela.Rows.Add(linha);
                        }

                        // Caso a operação inserida for Cancelar, Desligar a Flag de Inserir Dados
                        if (bd.LerString("Acao") == IngressoLog.CANCELAR)
                            blnInserirDados = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Problemas ao acessar o banco de dados: " + ex.Message);
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        public DataTable PesquisarSenhaValeIngresso(string senha)
        {

            DataTable tabela = EstruturaHistorico();
            BD bd = new BD();
            string strStatus = "";
            int intVendaBilheteriaID = 0;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tVendaBilheteria.ID, " +
                    "   tVendaBilheteria.Status " +
                    "FROM " +
                    "  tVendaBilheteria (NOLOCK) " +
                    "WHERE " +
                    "  (Senha = '" + senha + "')"))
                {
                    if (!oDataReader.Read())
                        throw new ApplicationException("Venda não encontrada!");

                    if (bd.LerInt("ID") < 1)
                        throw new ApplicationException("Venda não encontrada!");

                    intVendaBilheteriaID = bd.LerInt("ID");

                    strStatus = bd.LerString("Status");
                }

                bd.Fechar();

                StringBuilder strSql = new StringBuilder();

                strSql.Append(" SELECT ");
                strSql.Append(" ( ");
                strSql.Append(" SELECT COUNT(tValeIngressoLog.ID) ");
                strSql.Append(" FROM tValeIngressoLog (NOLOCK) ");
                strSql.Append(" WHERE (tValeIngressoLog.VendaBilheteriaID = " + intVendaBilheteriaID + ") ");
                strSql.Append(" ) ");
                strSql.Append(" AS TotIngressos ");
                strSql.Append(" , vir.ID AS IngressoID     ");
                strSql.Append(" , vir.ID Codigo     ");
                strSql.Append(" , vir.Status   ");
                strSql.Append(" , il.VendaBilheteriaID     ");
                strSql.Append(" , il.ValeIngressoID     ");
                strSql.Append(" , il.ID ");
                strSql.Append(" , il.[TimeStamp] ");
                strSql.Append(" , il.Acao ");
                strSql.Append(" , il.Obs ");
                strSql.Append(" , il.CodigoBarra  ");
                strSql.Append(" , lo.Nome AS Loja    ");
                strSql.Append(" , u.Nome AS Usuario     ");
                strSql.Append(" , ci.Nome AS Cliente ");
                strSql.Append(" , vb.NotaFiscalEstabelecimento ");
                strSql.Append(" , vb.Senha ");
                strSql.Append(" , t.Tipo AS TaxaEntregaTipo ");
                strSql.Append(" , IsNull(us.Nome, ' - ') AS Supervisor ");
                strSql.Append(" , ep.Nome AS PeriodoAgenda ");
                strSql.Append(" , en.Nome AS EntregaNome ");
                strSql.Append(" , ea.Data AS DataAgenda ");
                strSql.Append(" ,ear.Nome AS AreaEntrega ");
                strSql.Append(" ,vb.ClienteEnderecoID AS EnderecoEntrega  ");
                strSql.Append(" ,pdv.Nome as PDVEntrega ");
                strSql.Append(" ,ci.Email ");
                strSql.Append(" ,cl.Nome AS Canal ");
                strSql.Append(" ,en.ID AS EntregaID ");
                strSql.Append("  FROM tValeIngressoLog il (NOLOCK)  ");
                strSql.Append("  LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = il.VendaBilheteriaID  ");
                strSql.Append("  LEFT JOIN tEntregaControle as te on te.ID = vb.EntregaControleID ");
                strSql.Append("  LEFT JOIN tEntrega as t on te.EntregaID = t.ID ");
                strSql.Append("  LEFT JOIN tUsuario u (NOLOCK) ON u.ID = il.UsuarioID  ");
                strSql.Append("  LEFT JOIN tUsuario us (NOLOCK) ON us.ID = il.SupervisorID  ");
                strSql.Append("  INNER JOIN tValeIngresso vir (NOLOCK) ON vir.ID = il.ValeIngressoID  ");
                strSql.Append("  LEFT JOIN tLoja lo (NOLOCK) ON lo.ID = il.LojaID  ");
                strSql.Append("  LEFT JOIN tCanal cl (NOLOCK) ON cl.ID = il.CanalID  ");
                strSql.Append("  LEFT JOIN tCliente ci (NOLOCK) ON ci.ID = vir.ClienteID  ");
                strSql.Append("  LEFT JOIN tEntregaControle ec (NOLOCK) ON vb.EntregaControleID = ec.ID ");
                strSql.Append("  LEFT JOIN tEntrega en (NOLOCK) ON ec.EntregaID = en.ID ");
                strSql.Append("  LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ec.PeriodoID = ep.ID ");
                strSql.Append("  LEFT JOIN tEntregaArea ear (NOLOCK) ON ec.EntregaAreaID = ear.ID ");
                strSql.Append("  LEFT JOIN tEntregaAgenda ea (NOLOCK) ON vb.EntregaAgendaID = ea.ID ");
                strSql.Append("  LEFT JOIN tPontoVenda pdv (NOLOCK) ON vb.PdvID = pdv.ID ");
                strSql.Append("  WHERE (il.ValeIngressoID IN (            ");
                strSql.Append(" SELECT ValeIngressoID             ");
                strSql.Append(" FROM tValeIngressoLog (NOLOCK)            ");
                strSql.Append(" WHERE VendaBilheteriaID = " + intVendaBilheteriaID + ")     ");
                strSql.Append(" )  ");
                strSql.Append(" ORDER BY   il.ValeIngressoID,   il.ID  ");

                using (IDataReader oDataReader = bd.Consulta(strSql.ToString()))
                {
                    int ingressoID = 0;
                    bool blnInserirDados = false;
                    string strSenhaVenda = "";
                    DataRow linha = null;

                    while (oDataReader.Read())
                    {
                        // Atribui o ID do Ingresso Corrente e Desliga o Flag de Inserir Dados
                        if (ingressoID != bd.LerInt("IngressoID"))
                        {
                            ingressoID = bd.LerInt("IngressoID");
                            blnInserirDados = false;
                            strSenhaVenda = "";
                        }

                        if (bd.LerString("Acao") == IngressoLog.VENDER)
                            strSenhaVenda = bd.LerString("Senha");

                        // Se a operação for venda, Liga o Flag de Inserir Dados
                        if (((bd.LerString("Acao") == IngressoLog.VENDER || bd.LerString("Acao") == IngressoLog.PRE_RESERVA) && strStatus != VendaBilheteria.CANCELADO && bd.LerInt("VendaBilheteriaID") == intVendaBilheteriaID) || (bd.LerString("Acao") == IngressoLog.CANCELAR && strStatus == VendaBilheteria.CANCELADO && bd.LerInt("VendaBilheteriaID") == intVendaBilheteriaID))
                            blnInserirDados = true;

                        // Insere as Informações dos Ingressos
                        if (blnInserirDados)
                        {
                            if (strSenhaVenda == "")
                                if (strStatus == VendaBilheteria.PRE_RESERVADO)
                                    strSenhaVenda = "-";
                                else
                                    strSenhaVenda = bd.LerString("Senha");

                            linha = tabela.NewRow();
                            linha["IngressoID"] = bd.LerInt("IngressoID");
                            linha["IngressoLogID"] = bd.LerInt("ID");
                            linha["Codigo"] = bd.LerString("Codigo");
                            linha["Data"] = bd.LerStringFormatoDataHora("TimeStamp");
                            linha["Cliente"] = bd.LerString("Cliente");
                            linha["Loja"] = bd.LerString("Loja");
                            // Se for uma senha de cancelamento alimentar a Senha de Venda e Senha de Cancelamento
                            if (strStatus == VendaBilheteria.CANCELADO)
                            {
                                linha["Senha"] = strSenhaVenda;
                                linha["SenhaCancelamento"] = bd.LerString("Senha");
                            }
                            else
                            {
                                // Se for uma senha de venda, armazenar senha de venda, e somente a senha de cancalemento se for uma ação cancelar                                                                
                                linha["Senha"] = strSenhaVenda;

                                if (bd.LerString("Acao") == IngressoLog.CANCELAR)
                                    linha["SenhaCancelamento"] = bd.LerString("Senha");
                                else
                                    linha["SenhaCancelamento"] = "-";

                            }
                            linha["Ação"] = ValeIngressoLog.AcaoDescritiva(bd.LerString("Acao"));
                            linha["Status"] = ValeIngressoLog.StatusDescritivo(bd.LerString("Status"));
                            linha["StatusDetalhado"] = ValeIngressoLog.StatusDetalhado(bd.LerString("Status"), bd.LerString("TaxaEntregaTipo"));
                            linha["VendaBilheteriaID"] = bd.LerString("VendaBilheteriaID");
                            linha["Usuario"] = bd.LerString("Usuario");
                            linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                            linha["Supervisor"] = bd.LerString("Supervisor");
                            linha["EntregaID"] = bd.LerInt("EntregaID");
                            linha["Entrega"] = bd.LerString("EntregaNome");
                            linha["Periodo"] = bd.LerString("PeriodoAgenda");
                            linha["DataAgenda"] = bd.LerDateTime("DataAgenda");
                            linha["AreaEntrega"] = bd.LerString("AreaEntrega");
                            linha["EnderecoEntrega"] = bd.LerInt("EnderecoEntrega");
                            linha["PDVEntrega"] = bd.LerString("PDVEntrega");
                            linha["Email"] = bd.LerString("Email");
                            linha["Canal"] = bd.LerString("Canal");

                            tabela.Rows.Add(linha);
                        }
                        // Caso a operação inserida for Cancelar, Desligar a Flag de Inserir Dados
                        if (bd.LerString("Acao") == IngressoLog.CANCELAR)
                            blnInserirDados = false;
                    }
                }

                bd.Fechar();

            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Problemas ao acessar o banco de dados: " + ex.Message);
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        public DataTable BuscaIngressos(string Senha)
        {
            BD bd = new BD();

            DataTable tabela = EstruturaIngressoEmail();

            string sql = string.Format(@"SELECT ev.Nome AS Evento, l.Nome AS Local, a.Horario AS Apresentacao, s.Nome AS Setor, i.Codigo, s.LugarMarcado, p.Valor, p.Nome AS Preco, tvbi.TaxaConvenienciaValor, i.LugarID
                FROM tIngressoLog il (NOLOCK)  
                INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = il.VendaBilheteriaID
                INNER JOIN tVendaBilheteriaItem tvbi ON tvbi.VendaBilheteriaID = vb.ID AND il.VendaBilheteriaItemID = tvbi.ID
                INNER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID  
                INNER JOIN tIngresso i (NOLOCK) ON i.ID = IngressoID  
                INNER JOIN tEvento ev (NOLOCK) ON ev.ID = i.EventoID  
                INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID
                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID  
                INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID  
                WHERE vb.Senha = '{0}'  ORDER BY  il.IngressoID,  il.ID ", Senha);

            try
            {
                using (IDataReader oDataReader = bd.Consulta(sql))
                {
                    DataRow linha = null;

                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["Evento"] = bd.LerString("Evento");
                        linha["Local"] = bd.LerString("Local");
                        linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                        linha["Setor"] = bd.LerString("Setor");
                        linha["Codigo"] = bd.LerString("Codigo");
                        linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                        linha["Valor"] = bd.LerDecimal("Valor");
                        linha["Preco"] = bd.LerString("Preco");
                        linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                        linha["LugarID"] = bd.LerInt("LugarID");

                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Problemas ao acessar o banco de dados: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        public List<EstruturaRetornoReservaValeIngresso> BuscaValeIngressos(string Senha)
        {
            BD bd = new BD();

            List<EstruturaRetornoReservaValeIngresso> retorno = new List<EstruturaRetornoReservaValeIngresso>();

            string sql = string.Format(@"SELECT tvit.Nome, tvit.ValidadeData, tvit.ValidadeDiasImpressao, tvit.TrocaIngresso, tvit.TrocaConveniencia, tvit.TrocaEntrega, tvit.Valor, tvit.ValorPagamento
            FROM tValeIngresso vir (NOLOCK)    
            INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = vir.VendaBilheteriaID  
            INNER JOIN tValeIngressoTipo tvit (NOLOCK) ON tvit.ID = vir.ValeIngressoTipoID
            WHERE vb.Senha = '{0}'
            ORDER BY  vir.ID", Senha);

            try
            {
                using (IDataReader oDataReader = bd.Consulta(sql))
                {
                    while (oDataReader.Read())
                    {
                        retorno.Add(new EstruturaRetornoReservaValeIngresso()
                            {
                                Nome = bd.LerString("Nome"),
                                Validade = bd.LerDateTime("ValidadeData"),
                                ValidadeDias = bd.LerInt("ValidadeDiasImpressao"),
                                TrocaConveniencia = bd.LerBoolean("TrocaConveniencia"),
                                TrocaEntrega = bd.LerBoolean("TrocaEntrega"),
                                TrocaIngresso = bd.LerBoolean("TrocaIngresso"),
                                Valor = bd.LerDecimal("Valor"),
                                ValorPagamento = bd.LerDecimal("ValorPagamento"),

                            });
                    }
                }

                bd.Fechar();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Problemas ao acessar o banco de dados: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }
    }

    [Serializable]
    public class IngressoHistoricoGerenciadorException : Exception
    {

        public IngressoHistoricoGerenciadorException() : base() { }

        public IngressoHistoricoGerenciadorException(string msg) : base(msg) { }

        public IngressoHistoricoGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
