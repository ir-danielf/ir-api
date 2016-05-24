/**************************************************
* Arquivo: Ingresso.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib
{

    public class IngressoLog : IngressoLog_B
    {

        public const string DESBLOQUEAR = "D";
        public const string BLOQUEAR = "B";
        public const string VENDER = "V";
        public const string CANCELAR = "C";
        public const string CANCELAR_PREIMPRESSO = "K";
        public const string IMPRIMIR = "I";
        public const string EMISSAO_PREIMPRESSO = "P";
        public const string TRANSFERENCIA_PREIMPRESSO = "T";
        public const string REIMPRIMIR = "R";
        public const string ENTREGAR = "E";
        public const string ANULAR_IMPRESSAO = "A";
        public const string VOUCHER_IMPRESSAO = "O";
        public const string VOUCHER_REIMPRESSAO = "U";
        public const string PRE_RESERVA = "M";
        public const string ESTORNO = "S";

        // Se for const não acessso do lado de fora
        public string Cancelados = "'" + CANCELAR + "'";
        public string Vendidos = "'" + VENDER + "'"; // normalmente usado no in do SELECT
        //public const string VendidosCancelados= "'"+Cancelar+"'"+","+"'"+Vender+"'"; // normalmente usado no in do SELECT

        public IngressoLog() { }

        /// <summary>
        /// Inserir novo(a) IngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.Append("INSERT INTO tIngressoLog(IngressoID, UsuarioID, TimeStamp, Acao, PrecoID, CortesiaID, BloqueioID, VendaBilheteriaItemID, Obs, VendaBilheteriaID, CaixaID, LojaID, CanalID, EmpresaID, ClienteID, CodigoBarra, CodigoImpressao, MotivoId, SupervisorID, GerenciamentoIngressosID, AssinaturaClienteID) ");
                sql.Append("VALUES (@001, @002, '@003', '@004', @005, @006, @007, @008, '@009', @010, @011, @012, @013, @014, @015, '@016', '@017', '@018', '@019', @020, @021)");

                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.PrecoID.ValorBD);
                sql.Replace("@006", this.CortesiaID.ValorBD);
                sql.Replace("@007", this.BloqueioID.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@011", this.CaixaID.ValorBD);
                sql.Replace("@012", this.LojaID.ValorBD);
                sql.Replace("@013", this.CanalID.ValorBD);
                sql.Replace("@014", this.EmpresaID.ValorBD);
                sql.Replace("@015", this.ClienteID.ValorBD);
                sql.Replace("@016", this.CodigoBarra.ValorBD);
                sql.Replace("@017", this.CodigoImpressao.ValorBD);
                sql.Replace("@018", this.MotivoId.ValorBD);
                sql.Replace("@019", this.SupervisorID.ValorBD);
                sql.Replace("@020", this.GerenciamentoIngressosID.ValorBD);
                sql.Replace("@021", this.AssinaturaClienteID.ValorBD);

                object id = bd.ConsultaValor(sql.ToString());

                this.Control.ID = (id != DBNull.Value) ? Convert.ToInt32(id) : 0;

                bd.Fechar();

                bool result = Convert.ToBoolean(this.Control.ID);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) IngressoLog
        /// </summary>
        /// <returns></returns>	
        public string StringInserir()
        {

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO tIngressoLog(IngressoID, UsuarioID, TimeStamp, Acao, PrecoID, CortesiaID, BloqueioID, VendaBilheteriaItemID, Obs, VendaBilheteriaID, CaixaID, LojaID, CanalID, EmpresaID, ClienteID, CodigoBarra, CodigoImpressao, MotivoId, SupervisorID, GerenciamentoIngressosID, AssinaturaClienteID) ");
                sql.Append("VALUES (@001, @002, '@003', '@004', @005, @006, @007, @008, '@009', @010, @011, @012, @013, @014, @015, '@016', '@017', '@018', '@019', @020, @021)");

                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.PrecoID.ValorBD);
                sql.Replace("@006", this.CortesiaID.ValorBD);
                sql.Replace("@007", this.BloqueioID.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@011", this.CaixaID.ValorBD);
                sql.Replace("@012", this.LojaID.ValorBD);
                sql.Replace("@013", this.CanalID.ValorBD);
                sql.Replace("@014", this.EmpresaID.ValorBD);
                sql.Replace("@015", this.ClienteID.ValorBD);
                sql.Replace("@016", this.CodigoBarra.ValorBD);
                sql.Replace("@017", this.CodigoImpressao.ValorBD);
                sql.Replace("@018", this.MotivoId.ValorBD);
                sql.Replace("@019", this.SupervisorID.ValorBD);
                sql.Replace("@020", this.GerenciamentoIngressosID.ValorBD);
                sql.Replace("@021", this.AssinaturaClienteID.ValorBD);

                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

     
        public static string AcaoDescritiva(string letraAcao)
        {
            string resultado = "";
            switch (letraAcao)
            {
                case ESTORNO:
                    resultado = "Estorno";
                    break;
                case PRE_RESERVA:
                    resultado = "Pré-reserva";
                    break;
                case DESBLOQUEAR:
                    resultado = "Desbloquear";
                    break;
                case BLOQUEAR:
                    resultado = "Bloquear";
                    break;
                case VENDER:
                    resultado = "Vender";
                    break;
                case CANCELAR:
                    resultado = "Cancelar";
                    break;
                case CANCELAR_PREIMPRESSO:
                    resultado = "Kancelamento";
                    break;
                case ANULAR_IMPRESSAO:
                    resultado = "Anular Impressão";
                    break;
                case IMPRIMIR:
                    resultado = "Imprimir";
                    break;
                case EMISSAO_PREIMPRESSO:
                    resultado = "Pré-Impresso";
                    break;
                case REIMPRIMIR:
                    resultado = "Reimprimir";
                    break;
                case ENTREGAR:
                    resultado = "Entregar";
                    break;
                case TRANSFERENCIA_PREIMPRESSO:
                    resultado = "Tranferência";
                    break;
                case VOUCHER_IMPRESSAO:
                    resultado = "Imprimir Voucher";
                    break;
                case VOUCHER_REIMPRESSAO:
                    resultado = "Reimprimir Voucher";
                    break;
                default:
                    resultado = letraAcao;
                    break;
            }
            return resultado;
        }
        /// <summary>
        /// Arruma a inconsistencia da tIngressoLog no campo VendaBilheteriaItemID.
        /// Retorna o número de registros com erro.
        /// </summary>
        /// <returns></returns>
        public int ArrumarInconsistenciaVendaBilheteriaItemID()
        {
            try
            {
                List<int> vendaBilheteriaIDs = new List<int>();
                List<int> ingressoLogIDs;
                int countErros = 0;
                bool erro = false;
                //Busca as VendaBilheteriaID com problemas
                //a VendaBilheteriaID 2066872 tem taxa de conveniencia...e deve ser feita na mao
                string sql = "SELECT DISTINCT VendaBilheteriaID FROM tIngressoLog (NOLOCK) WHERE VendaBilheteriaItemID = 1 AND VendaBilheteriaID <>1 AND VendaBilheteriaID <> 2066872 AND Acao = 'V'";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    vendaBilheteriaIDs.Add(bd.LerInt("VendaBilheteriaID"));
                }
                bd.Consulta().Close();
                BD bdConsulta = new BD();
                //Passa por todas as vendas.
                foreach (int vendaBilheteriaID in vendaBilheteriaIDs)
                {
                    if (erro)
                    {
                        break;
                    }
                    else { erro = false; }


                    //busca os ingressoslog com problema.
                    bdConsulta.Consulta("SELECT ID FROM tIngressoLog WHERE VendaBilheteriaID = " + vendaBilheteriaID);
                    ingressoLogIDs = new List<int>();
                    while (bdConsulta.Consulta().Read())
                    {
                        ingressoLogIDs.Add(bdConsulta.LerInt("ID"));
                    }

                    bd.IniciarTransacao();

                    //Deleta os registros da tVendaBilheteriaItem dessa venda.
                    string deleta = "DELETE FROM tVendaBilheteriaItem WHERE VendaBilheteriaID = " + vendaBilheteriaID;
                    int deleted = bd.Executar(deleta);

                    //Passa log por log
                    foreach (int ingressoLogID in ingressoLogIDs)
                    {
                        object itemID = bd.ConsultaValor("INSERT INTO tVendaBilheteriaItem (VendaBilheteriaID,PacoteID,Acao,TaxaConveniencia,TaxaConvenienciaValor,TaxaComissao,ComissaoValor) VALUES (" + vendaBilheteriaID + ",0,'V',0,0,0,0);SELECT SCOPE_IDENTITY()");
                        //bd.Consulta().Close();
                        int result = 0;
                        //if (Convert.ToInt32(itemID) != 1) throw new Exception("Pipipi parou parou parou!");

                        if (Convert.ToInt32(itemID) > 1)
                        {
                            result = bd.Executar("UPDATE tIngressoLog SET VendaBilheteriaItemID = " + Convert.ToInt32(itemID) + " WHERE ID = " + ingressoLogID);
                        }
                        if (result != 1)
                        {
                            erro = true;
                            countErros = countErros + ingressoLogIDs.Count;
                            break;
                        }

                    }
                    if (erro)
                        bd.DesfazerTransacao();
                    else
                        bd.FinalizarTransacao();

                }
                return countErros;

            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
        }
        public List<EstruturaInconsistenciaIngressoLog> BuscaInconsistenciasPrecoID()
        {
            List<EstruturaInconsistenciaIngressoLog> retorno = new List<EstruturaInconsistenciaIngressoLog>();
            EstruturaInconsistenciaIngressoLog item;
            List<EstruturaInconsistenciaIngressoLog> itensAux = new List<EstruturaInconsistenciaIngressoLog>();
            String query = "SELECT IngressoID,IngressoLogID,Acao,IngressoPreco,LogPreco FROM IngressosLogInconsistenciaPreco (NOLOCK)";

            bd.Consulta(query);
            int ingressoIDAtual = 0, ingressoIDAnterior = 0;


            while (bd.Consulta().Read())
            {
                ingressoIDAtual = bd.LerInt("IngressoID");
                //Primeiro popula o objeto aux com todos os logs do ingressoID. Até chegar no próximo ingresso.
                if (ingressoIDAtual == ingressoIDAnterior || ingressoIDAnterior == 0)
                {
                    item = new EstruturaInconsistenciaIngressoLog();
                    item.IngressoID = ingressoIDAtual;
                    item.IngressoLogID = bd.LerInt("IngressoLogID");
                    item.AcaoLog = Convert.ToChar(bd.LerString("Acao"));
                    item.IngressoPrecoID = bd.LerInt("IngressoPreco");
                    item.LogPrecoID = bd.LerInt("LogPreco");
                    itensAux.Add(item);
                    if (item.AcaoLog == Convert.ToChar(IngressoLog.CANCELAR))
                    {
                        itensAux.Clear();//Remove todas as ações anteriores a um cancelamento
                        ingressoIDAnterior = 0;
                    }
                }
                else
                {
                    //mudou o ingresso:
                    // 1. Verifica na lista se existe algum preco diferente entre a log e a ingresso
                    // 2. Se tem inconsistencia insere na tabela retorno.
                    // 3. Se não tem passa pro proximo item.
                    foreach (EstruturaInconsistenciaIngressoLog registro in itensAux)
                    {
                        if (registro.IngressoPrecoID != registro.LogPrecoID && (registro.AcaoLog != 'D' && registro.AcaoLog != 'B'))
                        {
                            retorno.AddRange(itensAux);
                            //limpa o objeto para passar para o próximo

                            break;
                        }
                    }
                    itensAux.Clear();

                    //Adiciona o item atual
                    item = new EstruturaInconsistenciaIngressoLog();
                    item.IngressoID = ingressoIDAtual;
                    item.IngressoLogID = bd.LerInt("IngressoLogID");
                    item.AcaoLog = Convert.ToChar(bd.LerString("Acao"));
                    item.IngressoPrecoID = bd.LerInt("IngressoPreco");
                    item.LogPrecoID = bd.LerInt("LogPreco");
                    itensAux.Add(item);
                }

                ingressoIDAnterior = ingressoIDAtual;
            }



            return retorno;
        }

        public List<EstruturaInconsistenciaIngressoLog> BuscaInconsistenciasIngressoLog()
        {
            try
            {
                EstruturaInconsistenciaIngressoLog item;
                List<EstruturaInconsistenciaIngressoLog> retorno = new List<EstruturaInconsistenciaIngressoLog>();
                List<EstruturaInconsistenciaIngressoLog> itensAux = new List<EstruturaInconsistenciaIngressoLog>();
                //essa tabela tem que ser populada previamente com os dados necessários
                String query = "SELECT  IngressoID,IngressoLogID,AcaoLog,StatusIngresso FROM IngressosLogInconsistencia (NOLOCK)";

                bd.Consulta(query);
                int ingressoIDAtual = 0, ingressoIDAnterior = 0;
                while (bd.Consulta().Read())
                {
                    ingressoIDAtual = bd.LerInt("IngressoID");
                    //Primeiro popula o objeto aux com todos os logs do ingressoID. Até chegar no próximo ingresso.
                    if (ingressoIDAtual == ingressoIDAnterior || ingressoIDAnterior == 0)
                    {
                        item = new EstruturaInconsistenciaIngressoLog();
                        item.IngressoID = ingressoIDAtual;
                        item.IngressoLogID = bd.LerInt("IngressoLogID");
                        item.AcaoLog = Convert.ToChar(bd.LerString("AcaoLog"));
                        item.StatusIngresso = Convert.ToChar(bd.LerString("StatusIngresso"));
                        itensAux.Add(item);
                    }
                    else
                    {
                        //mudou o ingresso:
                        // 1. Verifica as regras
                        // 2. Se tem inconsistencia insere na tabela retorno.
                        // 3. Se não tem passa pro proximo item.

                        if (!ValidaInconsistencia(itensAux))
                            retorno.AddRange(itensAux);

                        //limpa o objeto para passar para o próximo
                        itensAux.Clear();

                        //Adiciona o item atual
                        item = new EstruturaInconsistenciaIngressoLog();
                        item.IngressoID = ingressoIDAtual;
                        item.IngressoLogID = bd.LerInt("IngressoLogID");
                        item.AcaoLog = Convert.ToChar(bd.LerString("AcaoLog"));
                        item.StatusIngresso = Convert.ToChar(bd.LerString("StatusIngresso"));
                        itensAux.Add(item);
                    }

                    ingressoIDAnterior = ingressoIDAtual;
                }
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            { bd.Fechar(); }

        }
        private bool ValidaInconsistencia(List<EstruturaInconsistenciaIngressoLog> lista)
        {
            try
            {
                bool ok = true;
                char acaoAtual;
                char acaoAnterior = '*';
                foreach (EstruturaInconsistenciaIngressoLog item in lista)
                {

                    acaoAtual = item.AcaoLog;
                    switch (acaoAtual)
                    {
                        case 'V':
                            if (acaoAnterior != '*' && acaoAnterior != 'C' && acaoAnterior != 'B' && acaoAnterior != 'D' && acaoAnterior != 'P' && acaoAnterior != 'K' && acaoAnterior != 'T' && acaoAnterior != 'M')
                                return false;
                            break;
                        case 'I':
                            if (acaoAnterior != 'V' && acaoAnterior != 'A')
                                return false;
                            break;
                        case 'R':
                            if (acaoAnterior != 'I' && acaoAnterior != 'R')
                                return false;
                            break;
                        case 'C':
                            if (acaoAnterior != 'V' && acaoAnterior != 'I' && acaoAnterior != 'R' && acaoAnterior != 'A' && acaoAnterior != 'E')
                                return false;
                            break;
                        case 'B':
                            if (acaoAnterior != '*' && acaoAnterior != 'D' && acaoAnterior != 'C' && acaoAnterior != 'B' && acaoAnterior != 'K')
                                return false;
                            break;
                        case 'A':
                            if (acaoAnterior != 'R' && acaoAnterior != 'I')
                                return false;
                            break;
                        case 'P':
                            if (acaoAnterior != '*' && acaoAnterior != 'C' && acaoAnterior != 'B' && acaoAnterior != 'D' && acaoAnterior != 'K')
                                return false;
                            break;
                        case 'T':
                            if (acaoAnterior != 'T' && acaoAnterior != 'P')
                                return false;
                            break;
                        case 'K':
                            if (acaoAnterior != 'T' && acaoAnterior != 'P')
                                return false;
                            break;
                        case 'D':
                            if (acaoAnterior != 'B' && acaoAnterior != '*' && acaoAnterior != 'C')
                                return false;
                            break;
                        case 'E':
                            if (acaoAnterior != 'I' && acaoAnterior != 'R')
                                return false;
                            break;
                        default:
                            //não as outras acoes por enquanto
                            ok = true;
                            break;
                    }

                    acaoAnterior = acaoAtual;
                }
                return ok;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Obter IngressoIDs por acao
        /// Depende tb dos IngressoIDs informados
        /// </summary>
        /// <returns></returns>
        public override string IngressoIDsPorAcao(string ingressoIDs)
        {
            try
            {
                // Obtendo dados
                string sql;
                sql =
                    "SELECT DISTINCT MAX(DISTINCT IngressoID) AS IngressoID, ID AS IngressoLogID, Acao " +
                    "FROM            tIngressoLog " +
                    "GROUP BY ID, Acao " +
                    "HAVING        (Acao = N'" + this.Acao.Valor + "') AND (MAX(DISTINCT IngressoID) IN (" + ingressoIDs + ")) ";
                bd.Consulta(sql);
                string IDsObtidos = "";
                if (bd.Consulta().Read())
                {
                    IDsObtidos = bd.LerString("IngressoID");
                }
                while (bd.Consulta().Read())
                {
                    IDsObtidos = IDsObtidos + "," + bd.LerString("IngressoID");
                }
                bd.Fechar();
                return IDsObtidos;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de IngressoIDsPorAcao

        /// <summary>
        /// Alimenta esse IngressoLog de acordo com o Ingresso passado como parametro
        /// </summary>
        public override void Ingresso(Ingresso ingresso)
        {
            this.IngressoID.Valor = ingresso.Control.ID;
            this.UsuarioID.Valor = ingresso.UsuarioID.Valor;
            this.BloqueioID.Valor = ingresso.BloqueioID.Valor;
            this.CortesiaID.Valor = ingresso.CortesiaID.Valor;
            this.PrecoID.Valor = ingresso.PrecoID.Valor;
            this.TimeStamp.Valor = System.DateTime.Now;
        }


        /// <summary>
        /// Alimenta esse IngressoLog de acordo com o ID do Ingresso passado como parametro
        /// </summary>
        public void Ingresso(int ingressoID)
        {

            string sql = "SELECT * FROM tIngressoLog WHERE Acao='V' AND IngressoID=" + ingressoID +
                " ORDER BY TimeStamp,ID DESC";
            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {
                this.IngressoID.Valor = bd.LerInt("IngressoID");
                this.UsuarioID.Valor = bd.LerInt("UsuarioID");
                this.BloqueioID.Valor = bd.LerInt("BloqueioID");
                this.CortesiaID.Valor = bd.LerInt("CortesiaID");
                this.PrecoID.Valor = bd.LerInt("PrecoID");
                this.CaixaID.Valor = bd.LerInt("CaixaID");
                this.CanalID.Valor = bd.LerInt("CanalID");
                this.ClienteID.Valor = bd.LerInt("ClienteID");
                this.EmpresaID.Valor = bd.LerInt("EmpresaID");
                this.LojaID.Valor = bd.LerInt("LojaID");
                this.VendaBilheteriaID.Valor = bd.LerInt("VendaBilheteriaID");
                this.VendaBilheteriaItemID.Valor = bd.LerInt("VendaBilheteriaItemID");
                this.TimeStamp.Valor = System.DateTime.Now;
            }

            bd.Fechar();

        }


    }

    public class IngressoLogLista : IngressoLogLista_B
    {

        public IngressoLogLista() { }

    }

}
