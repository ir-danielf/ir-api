/**************************************************
* Arquivo: ValeIngresso.cs
* Gerado: 09/11/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace IRLib
{

    public class ValeIngresso : ValeIngresso_B
    {
        public const int TAMANHO_CODIGO_TROCA = 10;
        public const int TAMANHO_CODIGO_TRCA_NOKIA = 14;
        public int FormaDePagamentoID = Convert.ToInt32(ConfigurationManager.AppSettings["FormaPagamentoIDValeIngresso"]);
        public ValeIngresso() { }

        public List<string> lstCodigosGerados = new List<string>();

        private Random random = new Random();

        public enum enumStatus
        {
            Disponivel = 'D',
            Reservado = 'R',
            Vendido = 'V',
            Impresso = 'I',
            Expirado = 'E',
            Trocado = 'T',
            Aguardando = 'A',
            Cancelado = 'C',
            Entregue = 'N',
        }

        public string stringAtualizarStatusVendido()
        {

            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append("UPDATE tValeIngresso SET Status = 'V' ");
            stbSQL.Append("WHERE ID = " + this.Control.ID + " AND Status <> 'T' ");
            return stbSQL.ToString();
        }

        public string StringAtualizarVendaValeIngresso()
        {
            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append("UPDATE tValeIngresso Set Status = 'V', ");
            stbSQL.Append("VendaBilheteriaID = " + this.VendaBilheteriaID.ValorBD + " ");
            stbSQL.Append("WHERE ID = " + this.Control.ID + " AND Status <> 'T' ");
            return stbSQL.ToString();
        }


        public string VendaTEF(int valeIngressoID)
        {
            BD bd = new BD();
            object retorno = bd.ConsultaValor(@"SELECT NotaFiscalCliente FROM tVendaBilheteria vb
                                INNER JOIN tValeIngresso (NOLOCK) vi ON vi.VendaBilheteriaID = vb.ID AND vi.ID = @ID".Replace("@ID", valeIngressoID.ToString()));
            if (retorno is string)
                return (string)retorno;
            else
                return string.Empty;
        }
        /// <summary>
        /// Inserir novo(a) ValeIngresso
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tValeIngresso(ValeIngressoTipoID, CodigoTroca, DataCriacao, DataExpiracao, Status, VendaBilheteriaID, ClienteID, SessionID, TimeStampReserva) ");
                sql.Append("VALUES (@001,'@002','@003','@004','@005',@006,@007,'@008','@009')");

                sql.Replace("@001", this.ValeIngressoTipoID.ValorBD);
                sql.Replace("@002", this.CodigoTroca.ValorBD);
                sql.Replace("@003", this.DataCriacao.ValorBD);
                sql.Replace("@004", this.DataExpiracao.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@007", this.ClienteID.ValorBD);
                sql.Replace("@008", this.SessionID.ValorBD);
                sql.Replace("@009", this.TimeStampReserva.ValorBD);


                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///1.Reserva a quantidade que tiver disponível para o vale ingresso
        ///2. Verifica se a quantidade reservada foi suficiente
        ///  2.1. Se for suficiente: finaliza e retorna a informação
        ///  2.2. Se não for suficiente: Vale tem quantidade limitada?
        ///      2.2.1. Sim: Não tem vir suficiente para a reserva
        ///     2.2.2. Não: Cria a quantidade de virs restante e reserva novamente
        /// </summary>
        /// <param name="bd"></param>
        public BindingList<EstruturaRetornoReservaValeIngresso> Reservar(CTLib.BD bd, int valeIngressoTipoID, int quantidadeReservar, int clienteID, int canalID, int lojaID, int usuarioID, string sessionID)
        {
            BD bdInserirVir = new BD();
            try
            {

                int quantidadeReservada = 0, quantidadeNaoReservada = 0;
                BindingList<EstruturaRetornoReservaValeIngresso> reserva = new BindingList<EstruturaRetornoReservaValeIngresso>();
                BindingList<EstruturaRetornoReservaValeIngresso> reservados;

                ValeIngressoTipo valeIngressoTipo = new ValeIngressoTipo();
                ValeIngresso valeIngresso;
                valeIngressoTipo.Ler(valeIngressoTipoID);//Preenche as informações do tipo
                if (valeIngressoTipo.Control.ID == 0)
                    throw new Exception("ValeIngresso inexistente");

                if (string.IsNullOrEmpty(sessionID))
                    reserva = this.Reservar(bd, quantidadeReservar, valeIngressoTipoID, canalID, lojaID, usuarioID, clienteID);
                else
                    reserva = this.ReservarInternet(bd, quantidadeReservar, valeIngressoTipoID, canalID, lojaID, usuarioID, clienteID, sessionID);

                quantidadeReservada = reserva.Count;
                quantidadeNaoReservada = quantidadeReservar - quantidadeReservada;

                //Verifica se a quantidade reservada foi suficiente.
                if (quantidadeReservada == quantidadeReservar)
                    return reserva;
                else//Não conseguiu reservar tudo
                {
                    //O vale ingresso tem quantidade limitada?
                    if (valeIngressoTipo.QuantidadeLimitada.Valor)
                        return reserva;
                    else
                    {//Se não tem quantidade limitada deve criar os vale ingressos restante
                        for (int i = 0; i < quantidadeNaoReservada; i++)
                        {
                            //preenche o objeto
                            valeIngresso = new ValeIngresso();
                            valeIngresso.ValeIngressoTipoID.Valor = valeIngressoTipoID;
                            valeIngresso.DataCriacao.Valor = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            valeIngresso.Status.Valor = ((char)ValeIngresso.enumStatus.Disponivel).ToString();
                            valeIngresso.UsuarioID.Valor = usuarioID;

                            if (valeIngressoTipo.CodigoTrocaFixo.Valor.Length > 0)
                                //Codigo fixo. Todos devem ter todos os códigos 
                                valeIngresso.CodigoTroca.Valor = valeIngressoTipo.CodigoTrocaFixo.Valor;
                            else
                                valeIngresso.CodigoTroca.Valor = string.Empty;

                            //Se der algum erro joga exception. e desfaz a transação de reserva.
                            valeIngresso.Inserir(bdInserirVir);
                        }
                        //Após Inserir tenta reservar os vales restantes
                        if (string.IsNullOrEmpty(sessionID))
                            reservados = this.Reservar(bd, quantidadeReservar, valeIngressoTipoID, canalID, lojaID, usuarioID, clienteID);
                        else
                            reservados = this.ReservarInternet(bd, quantidadeReservar, valeIngressoTipoID, canalID, lojaID, usuarioID, clienteID, sessionID);

                        foreach (EstruturaRetornoReservaValeIngresso item in reservados)
                        {
                            reserva.Add(item);
                        }
                    }
                }

                return reserva;
            }
            catch (Exception)
            {
                throw;
            }


        }



        private BindingList<EstruturaRetornoReservaValeIngresso> Reservar(BD bd, int quantidadeReservar, int valeIngressoTipoID, int canalID, int lojaID, int usuarioID, int clienteID)
        {
            try
            {

                string sql;
                int reservadoID;
                BindingList<EstruturaRetornoReservaValeIngresso> retorno = new BindingList<EstruturaRetornoReservaValeIngresso>();

                EstruturaRetornoReservaValeIngresso valeIngresso;
                DateTime validadeData;
                int validadeDiasImpressao;

                //Tenta reservar os VIRs disponíveis.
                sql = "EXEC ReservarValeIngresso2 " +
                      "@Quantidade = " + quantidadeReservar + ", " +
                      "@ValeIngressoTipoID = " + valeIngressoTipoID + ", " +
                      "@TimeStampReserva = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', " +
                      "@LojaID = " + lojaID + ", " +
                      "@UsuarioID =" + usuarioID + ", " +
                      "@CanalID =" + canalID + ", " +
                      "@ClienteID =" + (clienteID > 0 ? clienteID : 0);


                bd.Consulta(sql);


                //Preenche os IDs
                while (bd.Consulta().Read())
                {
                    valeIngresso = new EstruturaRetornoReservaValeIngresso();
                    reservadoID = bd.LerInt("ID");
                    //datas para preencher a string de validade
                    validadeData = bd.LerDateTime("ValidadeData");
                    validadeDiasImpressao = bd.LerInt("ValidadeDiasImpressao");

                    valeIngresso.ID = reservadoID;
                    valeIngresso.Nome = bd.LerString("Nome");
                    valeIngresso.Valor = bd.LerDecimal("Valor");

                    valeIngresso.ValorPagamento = bd.LerDecimal("ValorPagamento");
                    valeIngresso.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));
                    valeIngresso.TrocaConveniencia = bd.LerBoolean("TrocaConveniencia");
                    valeIngresso.TrocaEntrega = bd.LerBoolean("TrocaEntrega");
                    valeIngresso.TrocaIngresso = bd.LerBoolean("TrocaIngresso");


                    valeIngresso.CodigoTroca = bd.LerString("CodigoTrocaFixo");
                    valeIngresso.ValeIngressoTipoID = valeIngressoTipoID;
                    //Validade. Se for em dias da impressão, já calcula.
                    if (validadeDiasImpressao > 0)
                        valeIngresso.ValidadeDias = validadeDiasImpressao;
                    else
                        valeIngresso.Validade = validadeData;

                    retorno.Add(valeIngresso);
                    //Dispara o evento para retornar os objetos de retorno registro a registro
                    //isso é necessário para quando for gerada uma exception 
                    //OnCreate(reservadoID);
                }
                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Consulta().Close();
            }
        }

        public string NovoCodigoTroca()
        {
            string codigoGerado = string.Empty;

            do
            {
                codigoGerado = GerarCodigoTroca();

                while (lstCodigosGerados.Contains(codigoGerado))
                    codigoGerado = GerarCodigoTroca();

                lstCodigosGerados.Add(codigoGerado);

            } while (!CodigoTrocaUnico(codigoGerado));

            return codigoGerado;
        }
        /// <summary>
        /// método responsável por gerar o código de troca no formato correto. 
        /// Não faz validação de existencia no bd
        /// </summary>
        /// <returns></returns>
        private string GerarCodigoTroca()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                //Random random = new Random();
                bool letra = false;
                string item;
                int rnd;
                //codigo de 10 posições
                for (int i = 0; i < TAMANHO_CODIGO_TROCA; i++)
                {
                    //utiliza um randomico para saber se vai ser uma letra ou um numero
                    rnd = random.Next(1, 3);//pode ser 1 ou 2
                    letra = rnd == 1;

                    if (letra)//se for letra gera uma letra randomica
                        item = Convert.ToString(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
                    else // se for numero gera um numero
                        item = random.Next(1, 9).ToString();
                    builder.Append(item);//adiciona no retorno
                }
                return builder.ToString().ToUpper();
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Método responsável por validar a existencia do codigo troca no banco.
        /// </summary>
        /// <param name="codigoBarra">Código de barras a ser analisado</param>
        /// <param name="eventoID">ID do evento.</param>
        /// <returns>Objeto bool onde true = código único</returns>
        private bool CodigoTrocaUnico(string codigoTroca)
        {
            try
            {
                // Verifica se foi encontrado algum ingresso com o código de barras que seja para o evento em questão.
                return !bd.Consulta(
                    "SELECT 1 FROM tValeIngresso(NOLOCK) WHERE CodigoTroca <> '' AND CodigoTroca = '" + codigoTroca + "'"
                    ).Read();
            }
            catch
            {
                throw;
            }

        }

        private BindingList<EstruturaRetornoReservaValeIngresso> ReservarInternet(BD bd, int quantidadeReservar, int valeIngressoTipoID, int canalID, int lojaID, int usuarioID, int clienteID, string sessionID)
        {
            try
            {

                string sql;
                int reservadoID;
                BindingList<EstruturaRetornoReservaValeIngresso> retorno = new BindingList<EstruturaRetornoReservaValeIngresso>();

                EstruturaRetornoReservaValeIngresso valeIngresso;
                DateTime validadeData;
                int validadeDiasImpressao;

                //Tenta reservar os VIRs disponíveis.
                sql = "EXEC ReservarValeIngressoInternet2 " +
                      "@Quantidade = " + quantidadeReservar + ", " +
                      "@ValeIngressoTipoID = " + valeIngressoTipoID + ", " +
                      "@TimeStampReserva = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', " +
                      "@LojaID = " + lojaID + ", " +
                      "@UsuarioID =" + usuarioID + ", " +
                      "@CanalID =" + canalID + ", " +
                      "@ClienteID =" + (clienteID > 0 ? clienteID : 0) + ", " +
                      "@SessionID = '" + sessionID + "'";


                bd.Consulta(sql);


                //Preenche os IDs
                while (bd.Consulta().Read())
                {
                    valeIngresso = new EstruturaRetornoReservaValeIngresso();
                    reservadoID = bd.LerInt("ID");
                    //datas para preencher a string de validade
                    validadeData = bd.LerDateTime("ValidadeData");
                    validadeDiasImpressao = bd.LerInt("ValidadeDiasImpressao");

                    valeIngresso.ID = reservadoID;
                    valeIngresso.Nome = bd.LerString("Nome");
                    valeIngresso.Valor = bd.LerDecimal("Valor");
                    valeIngresso.CodigoTroca = bd.LerString("CodigoTrocaFixo");
                    valeIngresso.TrocaConveniencia = bd.LerBoolean("TrocaConveniencia");
                    valeIngresso.TrocaIngresso = bd.LerBoolean("TrocaIngresso");
                    valeIngresso.TrocaEntrega = bd.LerBoolean("TrocaEntrega");
                    valeIngresso.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));
                    valeIngresso.ValorPagamento = bd.LerDecimal("ValorPagamento");

                    //Validade. Se for em dias da impressão, já calcula.
                    if (validadeDiasImpressao > 0)
                        valeIngresso.Validade = DateTime.Now.AddDays((double)validadeDiasImpressao);
                    else
                        valeIngresso.Validade = validadeData;

                    retorno.Add(valeIngresso);
                    //Dispara o evento para retornar os objetos de retorno registro a registro
                    //isso é necessário para quando for gerada uma exception 
                    //OnCreate(reservadoID);
                }
                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Consulta().Close();
            }
        }

        public List<EstruturaRetornoReservaValeIngresso> ListaReservados(int clienteID, string sessionID)
        {
            try
            {
                List<EstruturaRetornoReservaValeIngresso> lista = new List<EstruturaRetornoReservaValeIngresso>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT vi.ID, vip.Valor, isNull(vi.CodigoTroca, '') AS CodigoTroca, vip.CodigoTrocaFixo, vip.ValidadeDiasImpressao, vip.ValidadeData ");
                stbSQL.Append("FROM tValeIngresso vi (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON vi.ValeIngressoTipoID = vip.ID ");
                stbSQL.Append("WHERE ClienteID = " + clienteID + "AND SessionID = '" + sessionID + "' AND Status = 'R'");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    EstruturaRetornoReservaValeIngresso oItem = new EstruturaRetornoReservaValeIngresso();
                    oItem.ID = bd.LerInt("ID");
                    oItem.Valor = bd.LerDecimal("Valor");
                    oItem.CodigoTroca = bd.LerString("CodigoTrocaFixo");
                    if (oItem.CodigoTroca.Length == 0)
                        oItem.CodigoTroca = bd.LerString("CodigoTroca");
                    oItem.ValidadeDias = bd.LerInt("ValidadeDiasImpressao");
                    oItem.Validade = bd.LerDateTime("ValidadeData");
                    lista.Add(oItem);
                }
                return lista;
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

        public bool CancelarReservas(int[] valeIngressoIDs)
        {
            try
            {
                string IDs = Utilitario.ArrayToString(valeIngressoIDs);
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("UPDATE tValeIngresso SET ");
                stbSQL.Append("ClienteID = 0, SessionID = 0, UsuarioID=0, LojaID=0, CanalID=0, ");
                stbSQL.Append("Status = '" + (char)ValeIngresso.enumStatus.Disponivel + "' ");
                stbSQL.Append("WHERE ID IN (" + IDs + ") AND ");
                stbSQL.Append("Status = '" + (char)ValeIngresso.enumStatus.Reservado + "'");

                bool ok = Convert.ToBoolean(bd.Executar(stbSQL.ToString()));

                return ok;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool CancelarReservasInternet(int clienteID, string sessionID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("UPDATE tValeIngresso SET ");
                stbSQL.Append("ClienteID = 0, SessionID = '', UsuarioID=0, LojaID=0, CanalID=0, ");
                stbSQL.Append("Status = '" + (char)ValeIngresso.enumStatus.Disponivel + "' ");
                stbSQL.Append("WHERE ");
                stbSQL.Append("ClienteID = " + clienteID + " AND ");
                stbSQL.Append("SessionID = '" + sessionID + "' AND ");
                stbSQL.Append("Status = '" + (char)ValeIngresso.enumStatus.Reservado + "'");

                return (bd.Executar(stbSQL.ToString()) > 0);
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

        public bool TransferirReservas()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("UPDATE tValeIngresso  SET ");
                stbSQL.Append("ClienteID = " + this.ClienteID.Valor + " ");
                stbSQL.Append("WHERE ClienteID = 0 AND ");
                stbSQL.Append("SessionID = '" + this.SessionID.Valor + "' AND ");
                stbSQL.Append("Status = '" + (char)ValeIngresso.enumStatus.Reservado + "'");

                int ok = bd.Executar(stbSQL.ToString());
                if (ok > 0)
                    return true;
                else
                    return false;
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

        public static int AumentarTempoReservaVIR(int clienteID, string sessionID)
        {
            BD bd = new BD();
            try
            {
                return bd.Executar("UPDATE tValeIngresso SET TimeStampReserva = '" + DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss") + "' WHERE Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "'");
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

        public int[] LimpaReservasVIRInternet()
        {
            try
            {
                int delayReserva = int.Parse(System.Configuration.ConfigurationManager.AppSettings["delayReserva"]);
                ArrayList ids = new ArrayList();
                DateTime data = DateTime.Now;

                data = data.AddMinutes(-delayReserva);

                bd.FecharConsulta();
                bd.Consulta("SELECT ID,Status FROM tValeIngresso (NOLOCK) WHERE '" + data.ToString("yyyyMMddHHmmss") + "' > TimeStampReserva AND TimeStampReserva IS NOT NULL AND TimeStampReserva <> '' AND (Status = '" + (char)Ingresso.StatusIngresso.RESERVADO + "') AND SessionID IS NOT NULL AND SessionID <> ''");

                BD bdUp = new BD();
                bool ok;
                string status;
                while (bd.Consulta().Read())
                {
                    status = bd.LerString("Status");
                    ok = bdUp.Executar("UPDATE tValeIngresso SET SessionID = '', TimeStampReserva = '', ClienteID = 0, Status = '" + (char)Ingresso.StatusIngresso.DISPONIVEL + "' WHERE Status = '" + (char)Ingresso.StatusIngresso.RESERVADO + "' AND ID = " + bd.LerInt("ID")) > 0;
                    if (ok)
                        ids.Add(bd.LerInt("ID"));
                }

                return (int[])ids.ToArray(typeof(int));
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.DesfazerTransacao();
                bd.Fechar();
            }
        }

        public int ExpirarVIRsNaoTrocados()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.AppendFormat(@"Update vi SET Status = '{0}' FROM tValeIngresso vi
                                    INNER JOIN tValeIngressoTipo vit ON vi.ValeIngressoTipoID = vit.ID
                                    WHERE Status IN ('{1}', '{2}') 
                                    AND ((DataExpiracao <> '' AND DataExpiracao < '{3}') OR (ValidadeData <> '' AND ValidadeData < '{4}'))",
                                    (char)enumStatus.Expirado, (char)enumStatus.Impresso, (char)enumStatus.Vendido,
                                    DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMddHHmmss"));

                return bd.Executar(stbSQL.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaTrocaValeIngresso> ValidarTrocaValeIngresso(List<int> IDs)
        {
            try
            {
                StringBuilder stbSQL;

                stbSQL = new StringBuilder();
                List<EstruturaTrocaValeIngresso> retorno = new List<EstruturaTrocaValeIngresso>();

                stbSQL.Append("SELECT DISTINCT vi.ID, vip.Nome, vi.CodigoTroca, vip.CodigoTrocaFixo, vi.Status, vip.Valor, vip.Acumulativo, ");
                stbSQL.Append(" vip.TrocaConveniencia, vip.TrocaEntrega, vip.TrocaIngresso, vip.ValorPagamento, vip.ValorTipo ");
                stbSQL.Append("FROM tValeIngresso vi (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON vi.ValeIngressoTipoID = vip.ID ");
                stbSQL.Append("WHERE vi.Status = '" + (char)ValeIngresso.enumStatus.Impresso + "' ");
                stbSQL.Append("AND (((vi.ID IN (" + Utilitario.ArrayToString(IDs.ToArray()) + ") ");
                stbSQL.Append("AND DataExpiracao >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "' ) ");
                stbSQL.Append("AND (DataExpiracao >= '" + System.DateTime.Now.Date.ToString("yyyyMMdd") + "'))) ");

                bd.Consulta(stbSQL.ToString());
                if (!bd.Consulta().Read())
                    throw new Exception("Os vale ingressos utilizados não foram encontrados.");

                do
                {
                    retorno.Add(new EstruturaTrocaValeIngresso()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        CodigoTroca = bd.LerString("CodigoTroca"),
                        CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo"),
                        Acumulativo = bd.LerString("Acumulativo"),
                        Status = bd.LerString("Status"),
                        Valor = bd.LerDecimal("Valor"),
                        Encontrado = true,
                        TrocaConveniencia = bd.LerBoolean("TrocaConveniencia"),
                        TrocaEntrega = bd.LerBoolean("TrocaEntrega"),
                        TrocaIngresso = bd.LerBoolean("TrocaIngresso"),
                        ValorPagamento = bd.LerDecimal("ValorPagamento"),
                        ValorTipo = Convert.ToChar(bd.LerString("ValorTipo")),
                    });
                } while (bd.Consulta().Read());

                //this.AlterarStatus(enumStatus.Aguardando, oRetorno.ID);

                if (retorno.Count != IDs.Count)
                    throw new Exception("Não foi possível encontrar todos os Vale Ingresos. Por favor tente novamente.");
                return retorno;
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

        public EstruturaTrocaValeIngresso ValidarTrocaValeIngresso(int ID)
        {
            try
            {
                StringBuilder stbSQL;

                stbSQL = new StringBuilder();
                EstruturaTrocaValeIngresso oRetorno = new EstruturaTrocaValeIngresso();
                stbSQL.Append("SELECT TOP 1 vi.ID, vip.Nome, vi.CodigoTroca, vip.CodigoTrocaFixo, vi.Status, vip.Valor, vip.Acumulativo, ");
                stbSQL.Append(" vip.TrocaConveniencia, vip.TrocaEntrega, vip.TrocaIngresso, vip.ValorPagamento, vip.ValorTipo ");
                stbSQL.Append("FROM tValeIngresso vi (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON vi.ValeIngressoTipoID = vip.ID ");
                stbSQL.Append("WHERE vi.Status = '" + (char)ValeIngresso.enumStatus.Impresso + "' ");
                stbSQL.Append("AND (((vi.ID = '" + ID + "' ");
                stbSQL.Append("AND DataExpiracao >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "' ) ");
                stbSQL.Append("AND (DataExpiracao >= '" + System.DateTime.Now.Date.ToString("yyyyMMdd") + "'))) ");

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    oRetorno.ID = bd.LerInt("ID");
                    oRetorno.Nome = bd.LerString("Nome");
                    oRetorno.CodigoTroca = bd.LerString("CodigoTroca");
                    oRetorno.CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    oRetorno.Acumulativo = bd.LerString("Acumulativo");
                    oRetorno.Status = bd.LerString("Status");
                    oRetorno.Valor = bd.LerDecimal("Valor");
                    oRetorno.Encontrado = true;
                    oRetorno.TrocaConveniencia = bd.LerBoolean("TrocaConveniencia");
                    oRetorno.TrocaEntrega = bd.LerBoolean("TrocaEntrega");
                    oRetorno.TrocaIngresso = bd.LerBoolean("TrocaIngresso");
                    oRetorno.ValorPagamento = bd.LerDecimal("ValorPagamento");
                    oRetorno.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));

                    //this.AlterarStatus(enumStatus.Aguardando, oRetorno.ID);
                }
                else
                    oRetorno.Encontrado = false;
                return oRetorno;
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

        public EstruturaTrocaValeIngresso ValidarTrocaValeIngresso(string codigoTroca)
        {
            try
            {
                StringBuilder stbSQL;

                stbSQL = new StringBuilder();
                EstruturaTrocaValeIngresso oRetorno = new EstruturaTrocaValeIngresso();
                stbSQL.Append("SELECT TOP 1 vi.ID, vip.Nome, vi.CodigoTroca, vip.CodigoTrocaFixo, vi.Status, vip.Valor, vip.Acumulativo, ");
                stbSQL.Append(" vip.TrocaConveniencia, vip.TrocaEntrega, vip.TrocaIngresso, vip.ValorPagamento, vip.ValorTipo ");
                stbSQL.Append("FROM tValeIngresso vi (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON vi.ValeIngressoTipoID = vip.ID ");
                stbSQL.Append("WHERE vi.Status = '" + (char)ValeIngresso.enumStatus.Impresso + "' ");
                stbSQL.Append("AND ((vi.CodigoTroca = '" + codigoTroca.Replace("'", "") + "' ");
                stbSQL.Append("AND DataExpiracao >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "' ) ");
                stbSQL.Append("OR (vip.CodigoTrocaFixo = '" + codigoTroca.Replace("'", "") + "' ");
                stbSQL.Append("AND (DataExpiracao >= '" + System.DateTime.Now.Date.ToString("yyyyMMdd") + "'))) ");

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    oRetorno.ID = bd.LerInt("ID");
                    oRetorno.Nome = bd.LerString("Nome");
                    oRetorno.CodigoTroca = bd.LerString("CodigoTroca");
                    oRetorno.CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    oRetorno.Acumulativo = bd.LerString("Acumulativo");
                    oRetorno.Status = bd.LerString("Status");
                    oRetorno.Valor = bd.LerDecimal("Valor");
                    oRetorno.Encontrado = true;
                    oRetorno.TrocaConveniencia = bd.LerBoolean("TrocaConveniencia");
                    oRetorno.TrocaEntrega = bd.LerBoolean("TrocaEntrega");
                    oRetorno.TrocaIngresso = bd.LerBoolean("TrocaIngresso");
                    oRetorno.ValorPagamento = bd.LerDecimal("ValorPagamento");
                    oRetorno.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));

                    //this.AlterarStatus(enumStatus.Aguardando, oRetorno.ID);
                }
                else
                    oRetorno.Encontrado = false;
                return oRetorno;
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

        /// <summary>
        /// Utilizar somente no momento de vender, alternativa para a proc de atualizar valeingresso,
        /// (Utiliza Transaction)
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="Status"></param>
        /// <param name="valeIngressoID"></param>
        /// <returns></returns>
        public int AlterarStatus(BD bd, enumStatus Status, int valeIngressoID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("UPDATE tValeIngresso SET Status = '" + (char)Status + "' ");
                stbSQL.Append("WHERE ID = " + valeIngressoID + " AND Status <> 'T' ");
                return bd.Executar(stbSQL.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EstruturaValeIngressoaFormaPagamento getFormaPagamentoVIR()
        {
            try
            {
                EstruturaValeIngressoaFormaPagamento Estrutura = new EstruturaValeIngressoaFormaPagamento();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT fp.Tipo, fp.Parcelas, fp.BandeiraID, efp.TaxaAdm, efp.Dias, fp.Padrao, fp.FormaPagamentoTipoID, efp.IR ");
                stbSQL.Append("FROM tFormaPagamento fp (NOLOCK) ");
                stbSQL.Append("INNER JOIN tEmpresaFormaPagamento efp ON efp.FormaPagamentoID = fp.ID ");
                stbSQL.Append("WHERE fp.ID = " + Estrutura.FormaPagamentoID);

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    Estrutura.Tipo = bd.LerInt("Tipo");
                    Estrutura.BandeiraID = bd.LerInt("BandeiraID");
                    Estrutura.TaxaAdm = bd.LerDecimal("TaxaAdm");
                    Estrutura.Dias = bd.LerInt("Dias");
                    Estrutura.Padrao = bd.LerString("Padrao");
                    Estrutura.IR = bd.LerBoolean("IR");
                    Estrutura.FormaPagamentoTipoID = bd.LerInt("FormaPagamentoTipoID");
                }
                return Estrutura;
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

        public decimal ValorTotalVIRs(List<int> valeIngressoIds)
        {
            try
            {
                string Ids = string.Empty;
                if (valeIngressoIds.Count > 0)
                {
                    Ids = Utilitario.ArrayToString(valeIngressoIds.ToArray());
                    StringBuilder stbSQL = new StringBuilder();
                    stbSQL.Append("SELECT SUM(vip.Valor) FROM tValeIngresso (NOLOCK) ");
                    stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON ");
                    stbSQL.Append("tValeIngresso.ValeIngressoTipoID = vip.ID ");
                    stbSQL.Append("WHERE tValeIngresso.ID IN (" + Ids + ") AND (Status = 'V' OR Status = 'I') ");
                    return Convert.ToDecimal(bd.ConsultaValor(stbSQL.ToString()));
                }
                else return 0;
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

        public decimal ValorTotalVIRsFixo(List<int> valeIngressoIds)
        {
            try
            {
                string Ids = string.Empty;
                if (valeIngressoIds.Count > 0)
                {
                    Ids = Utilitario.ArrayToString(valeIngressoIds.ToArray());
                    StringBuilder stbSQL = new StringBuilder();
                    stbSQL.Append("SELECT SUM(vip.Valor) FROM tValeIngresso (NOLOCK) ");
                    stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON ");
                    stbSQL.Append("tValeIngresso.ValeIngressoTipoID = vip.ID ");
                    stbSQL.Append("WHERE tValeIngresso.ID IN (" + Ids + ") ");
                    return Convert.ToDecimal(bd.ConsultaValor(stbSQL.ToString()));
                }
                else return 0;
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

        public decimal ValorVIR(int valeIngressoID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT vip.Valor FROM tValeIngresso (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON ");
                stbSQL.Append("tValeIngresso.ValeIngressoTipoID = vip.ID ");
                stbSQL.Append("WHERE tValeIngresso.ID = " + valeIngressoID);
                return Convert.ToDecimal(bd.ConsultaValor(stbSQL.ToString()));
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

        public EstruturaTrocaValeIngresso ValorIDVIR(string codigoTrocaFixo)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT TOP 1 tValeIngresso.ID, vip.Valor, vip.ValorTipo,vip.TrocaConveniencia,vip.TrocaEntrega,vip.TrocaIngresso FROM tValeIngresso (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vip (NOLOCK) ON ");
                stbSQL.Append("tValeIngresso.ValeIngressoTipoID = vip.ID ");
                stbSQL.Append("WHERE vip.CodigoTrocaFixo = '" + codigoTrocaFixo + "' ");
                stbSQL.Append("AND (Status = 'V' OR Status = 'I') ");
                bd.Consulta(stbSQL.ToString());

                EstruturaTrocaValeIngresso oRetorno = new EstruturaTrocaValeIngresso();
                if (bd.Consulta().Read())
                {
                    oRetorno.Encontrado = true;
                    oRetorno.ID = bd.LerInt("ID");
                    oRetorno.Valor = bd.LerDecimal("Valor");
                    oRetorno.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));
                    oRetorno.TrocaConveniencia = bd.LerString("TrocaConveniencia") == "T";
                    oRetorno.TrocaEntrega = bd.LerString("TrocaEntrega") == "T";
                    oRetorno.TrocaIngresso = bd.LerString("TrocaIngresso") == "T";
                }
                else
                    oRetorno.Encontrado = false;

                return oRetorno;

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

        public void AtribuirPresenteadoVIR(List<EstruturaVirNomePresenteado> lstPresenteados)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();

                int erros = 0;

                for (int i = 0; i < lstPresenteados.Count; i++)
                {
                    stbSQL.Append("UPDATE tValeIngresso SET ClienteNome = '" + lstPresenteados[i].NomePresenteado + "' ");
                    stbSQL.Append("WHERE ID = " + Convert.ToInt32(lstPresenteados[i].ID));
                    int x = bd.Executar(stbSQL.ToString());

                    if (x == 0)
                        erros += 1;
                }

                if (erros > 0)
                    throw new Exception("Erro ao atribuir o nome do presenteado.");
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


        public bool VerificaSeVendeu(int ValeIngressiTipoID)
        {
            try
            {
                string SQL;
                bool retorno = false;

                SQL = @"SELECT COUNT(ID) 
                FROM tValeIngresso
                WHERE ValeIngressoTipoID = " + ValeIngressiTipoID + " AND (Status = '" + (char)enumStatus.Vendido +
                @"' OR Status = '" + (char)enumStatus.Impresso + "' OR Status = '" + (char)enumStatus.Trocado + "')";

                bd.Consulta(SQL.ToString());

                if (bd.Consulta().Read())
                {
                    retorno = true;
                }

                return retorno;
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

        public static decimal ValorPagoVir(List<EstruturaTrocaValeIngresso> lista, decimal totalTaxaEntrega, decimal totalConveniencia, decimal totalIngressos)
        {
            decimal EntregaPagoVir = 0;
            decimal ConvenieciaPagoVir = 0;
            decimal IngressoPagoVir = 0;

            foreach (var oEstrutura in lista)
            {
                decimal ValorVIr = oEstrutura.Valor;
                decimal ValorRealVIr = 0;
                decimal aux = 0;

                if (oEstrutura.TrocaEntrega)
                {
                    if ((EntregaPagoVir < totalTaxaEntrega))
                    {
                        if (oEstrutura.ValorTipo == (char)IRLib.ValeIngressoTipo.EnumValorTipo.Porcentagem)
                            ValorVIr = (totalTaxaEntrega * oEstrutura.Valor / 100);


                        aux = ValorVIr - (totalTaxaEntrega - EntregaPagoVir);
                        if (aux >= 0)
                        {
                            EntregaPagoVir += totalTaxaEntrega;
                            ValorRealVIr += totalTaxaEntrega;
                            ValorVIr -= totalTaxaEntrega;
                        }
                        else
                        {
                            EntregaPagoVir += ValorVIr;
                            ValorRealVIr += ValorVIr;
                            ValorVIr = 0;
                        }
                    }
                }

                if (oEstrutura.TrocaConveniencia)
                {
                    if ((ConvenieciaPagoVir < totalConveniencia))
                    {
                        if (oEstrutura.ValorTipo == (char)IRLib.ValeIngressoTipo.EnumValorTipo.Porcentagem)
                            ValorVIr = (totalConveniencia * oEstrutura.Valor / 100);

                        aux = ValorVIr - (totalConveniencia - ConvenieciaPagoVir);
                        if (aux >= 0)
                        {
                            ConvenieciaPagoVir += totalConveniencia;
                            ValorRealVIr += totalConveniencia;
                            ValorVIr -= totalConveniencia;
                        }
                        else
                        {
                            ConvenieciaPagoVir += ValorVIr;
                            ValorRealVIr += ValorVIr;
                            ValorVIr = 0;
                        }
                    }
                }

                if (oEstrutura.TrocaIngresso)
                {
                    if ((IngressoPagoVir < totalIngressos))
                    {
                        if (oEstrutura.ValorTipo == (char)IRLib.ValeIngressoTipo.EnumValorTipo.Porcentagem)
                            ValorVIr = (totalIngressos * oEstrutura.Valor / 100);

                        aux = ValorVIr - (totalIngressos - IngressoPagoVir);
                        if (aux >= 0)
                        {
                            IngressoPagoVir += totalIngressos;
                            ValorRealVIr += totalIngressos;
                            ValorVIr -= totalIngressos;
                        }
                        else
                        {
                            IngressoPagoVir += ValorVIr;
                            ValorRealVIr += ValorVIr;
                            ValorVIr = 0;
                        }
                    }
                }
            }

            return IngressoPagoVir + EntregaPagoVir + ConvenieciaPagoVir;
        }

    }

    public class ValeIngressoLista : ValeIngressoLista_B
    {

        public ValeIngressoLista() { }

    }

}
