using CTLib;
using IRLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using IRCore.Util;


namespace IngressoRapido.Lib
{
    public class Sincronizacao
    {
        public bool EstadoDeExecucao { get; set; }
        public enum enumEntryType
        {
            Sucess,
            Error,
            Information,
        }
        DAL oDAL = new DAL();
        List<string> strSqls = new List<string>();

        private BufferInternet buffer;
        private BufferInternet bufferLote;
        private SingleTonObjects spEvent;

        List<string> lstPontosVenda = new List<string>();
        List<string> lstPontoVendaFormaPgto = new List<string>();
        List<string> lstHorario = new List<string>();
        List<string> lstFormaPgto = new List<string>();

        Thread thBanner { get; set; }
        Thread thEventoTipoDestaque { get; set; }
        Thread thFormaPagamentoEvento { get; set; }
        Thread thTipo { get; set; }
        Thread thSubtipo { get; set; }
        Thread thTipos { get; set; }
        Thread thValeIngressoTipo { get; set; }
        Thread thPacote { get; set; }
        Thread thNomenclaturaPacote { get; set; }
        Thread thPacoteItem { get; set; }
        Thread thPontoVenda { get; set; }
        Thread thPontoVendaHorario { get; set; }
        Thread thPontoVendaFormaPgto { get; set; }
        Thread thPontoVendaXFormaPagto { get; set; }
        Thread thSerie { get; set; }
        Thread thSerieItem { get; set; }
        Thread thFormaPagamentoSerie { get; set; }
        Thread thFormaPagamentoCotaItem { get; set; }
        Thread thCotaItem { get; set; }
        Thread thVoceSabia { get; set; }
        Thread thFaq { get; set; }

        bool CarregarTudo { get; set; }

        public Sincronizacao()
        {
            this.EstadoDeExecucao = false;
        }

        public delegate void MessageHandler(string msg, DateTime timeStamp);
        public event MessageHandler OnProgress;
        private void Progress(string msg)
        {
            if (OnProgress != null) OnProgress(msg, DateTime.Now);
        }

        public void SincronizarNovo()
        {
            if (this.EstadoDeExecucao)
                return;

            DateTime inicio = DateTime.Now;
            string Message = string.Empty;
            string stackTrace = string.Empty;
            bool givenError = false;


            try
            {
                this.SalvarLog(false, DateTime.Now, DateTime.Now, "Atualização iniciada às " + inicio, string.Empty, enumEntryType.Sucess, 1);

                if (oDAL != null)
                    oDAL.ConnClose();

                Progress("Carregando Buffer de dados...");
                this.buffer = new BufferInternet();

                this.SalvarLog(false, DateTime.Now, DateTime.Now, "Iniciando carga do buffer", string.Empty, enumEntryType.Sucess, 1);
                this.buffer.Carregar();

                this.SalvarLog(false, DateTime.Now, DateTime.Now, "Iniciando atualização dos setores", string.Empty, enumEntryType.Sucess, 1);
                this.AtualizarSetores();

                this.SalvarLog(false, DateTime.Now, DateTime.Now, "Iniciando carga de dados do site", string.Empty, enumEntryType.Sucess, 1);
                this.buffer.CarregarSite();
                this.CarregarTudo = false;

                this.Progress("Sincronizando SiteIR");
                this.SyncronizeLists(Convert.ToBoolean(ConfigurationManager.AppSettings["RoboLocal"]));

                this.Progress("Sincronização Concluída com sucesso.");

            }
            catch (Exception ex)
            {
                givenError = true;
                Message = ex.Message;
                stackTrace = ex.StackTrace;
                CarregarTudo = true;

                this.Progress(ex.Message);
            }
            finally
            {
                if (!givenError)
                    SalvarLog(true, inicio, DateTime.Now, "Sincronização Concluída.", string.Empty, enumEntryType.Sucess, 1);
                else
                    SalvarLog(true, inicio, DateTime.Now, "Falha ao sincronizar o Site IR.", string.Format("{0} \n {1}", Message, stackTrace), enumEntryType.Error, 1);

                this.EstadoDeExecucao = false;
            }
        }

        public void SincronizarStatusLotes()
        {
            DateTime inicio = DateTime.Now;
            string Message = string.Empty;
            bool givenError = false;

            try
            {
                this.SalvarLogAcoes(false, DateTime.Now, DateTime.Now, "Atualização Lotes iniciada ás " + inicio, string.Empty, enumEntryType.Sucess, 1);

                if (oDAL != null)
                    oDAL.ConnClose();

                Progress("Carregando Buffer de lotes...");
                this.bufferLote = new BufferInternet();
                this.bufferLote.CarregarLotes();
                Progress("Atualizando lotes...");
                this.AtualizarLotes(this.bufferLote.lstLoteStatus);
                this.Progress("Sincronização de lotes Concluída com sucesso.");
            }
            catch (Exception ex)
            {
                givenError = true;
                Message = ex.Message;
                CarregarTudo = true;

                this.Progress(ex.Message);
            }
            finally
            {
                if (!givenError)
                    SalvarLogAcoes(true, inicio, DateTime.Now, "Sincronização dos lotes Concluída.", string.Empty, enumEntryType.Sucess, 1);
                else
                    SalvarLogAcoes(true, inicio, DateTime.Now, "Falha ao sincronizar os lotes.", Message, enumEntryType.Error, 1);
            }
        }

        /// <summary>
        /// Verifica se todos os setores de cada evento estao aprovados para publicação
        /// e atualiza o campo LugarMarcado caso necessário
        /// </summary>
        private void AtualizarSetores()
        {

            var k = (from e in this.buffer.lstEvento
                     join a in this.buffer.lstApresentacao on e.ID equals a.EventoID
                     join st in this.buffer.lstSetor on a.ID equals st.ApresentacaoID
                     where !st.AprovadoPublicacao && st.LugarMarcado != "P" && e.EscolherLugarMarcado
                     select e).Distinct();

            foreach (var item in k)
            {
                item.EscolherLugarMarcado = false;
            }

        }

        private void AtualizarLotes(List<IRLib.BufferInternet.LoteStatus> lstLoteStatus)
        {
            BD bd = new BD();
            try
            {
                foreach (IRLib.BufferInternet.LoteStatus loteStatus in lstLoteStatus)
                {
                    if (loteStatus.UpdateSQL != "")
                    {
                        bd.Executar(loteStatus.UpdateSQL, loteStatus.setParameters());
                    }
                }
            }
            catch (Exception ex)
            {
                SalvarLogAcoes(true, DateTime.Now, DateTime.Now, "Falha ao sincronizar os lotes.", ex.Message, enumEntryType.Error, 1);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void SyncObjetoSpecialEvent()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;

            try
            {
                if (spEvent == null)
                    this.spEvent = SingleTonObjects.RegistrarSpecialEvent();
                else if (spEvent != null && !spEvent.bufferSpecialEvent.IsLoadingQRX)
                    this.spEvent.bufferSpecialEvent.Load();
            }
            catch (Exception ex)
            {
                spEvent = null;
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Atualização Objeto Special Event", errorMessage, enumEntryType.Information, 11);
            }
        }

        public void SyncMapasEsquematicos()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;

            try
            {
                if (spEvent == null)
                {

                    this.spEvent = SingleTonObjects.RegistrarSpecialEvent();

                    var temp = this.spEvent.bufferMapaEsquematico;

                    List<MapaEsquematicoInfo> temp2 = null;
                    if (temp != null)
                        temp2 = temp.MapasEsquematicosInfo;

                    this.spEvent.bufferMapaEsquematico.Carregar();

                    if (temp != null)
                        temp2 = temp.MapasEsquematicosInfo;

                    this.spEvent.bufferMapaEsquematico.AlterarApresentacoes();
                }
                else if (spEvent != null && !spEvent.bufferMapaEsquematico.Carregando && !spEvent.bufferMapaEsquematico.Recarregando)
                {
                    this.spEvent.bufferMapaEsquematico.Carregar();
                    this.spEvent.bufferMapaEsquematico.AlterarApresentacoes();
                }
            }
            catch (Exception ex)
            {
                spEvent = null;
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Atualização do Objeto Mapa Esquematico - Single Ton", errorMessage, enumEntryType.Information, 11);
            }
        }

        public void SyncMensagens()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;

            try
            {
                if (spEvent == null)
                {
                    this.spEvent = SingleTonObjects.RegistrarSpecialEvent();
                    this.spEvent.bufferMensagens.Carregar();
                }
                else
                    this.spEvent.bufferMensagens.Carregar();

                this.spEvent.bufferMensagens.TempoRequisicao();
            }
            catch (Exception ex)
            {
                spEvent = null;
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Atualização do Objeto Mensagens - Single Ton", errorMessage, enumEntryType.Information, 11);
            }
        }

        public void LimparReservas()
        {
            LogUtil.Info(string.Format("##RoboAtzSite.LimparReservas##"));

            DAL oDal = new DAL();
            string errorMessage = string.Empty;
            DateTime inicio = DateTime.Now;
            try
            {
                IRLib.Ingresso oIngresso = new IRLib.Ingresso();
                int[] ingressos = oIngresso.LimpaReservasInternet();

                // TODO: TROCAR ESSE UPDATE POR UM SQL QUE SEMPRE DEIXE A CARRINHO CERTO

                LogUtil.Debug(string.Format("##RoboAtzSite.LimparReservas.LimparCarrinho##"));

                foreach (int id in ingressos)
                    oDal.Execute("UPDATE Carrinho SET Status = 'E' WHERE IngressoID = @IngressoID AND Status = 'R'", new SqlParameter[] { new SqlParameter("@IngressoID", id) });

                LogUtil.Debug(string.Format("##RoboAtzSite.LimparReservas.LimparCarrinho.SUCCESS##"));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                LogUtil.Info(string.Format("##RoboAtzSite.LimparReservas.SUCCESS##"));
                oDal.ConnClose();
                SalvarLogAcoes(true, inicio, DateTime.Now, "Limpar Reservas", errorMessage, enumEntryType.Information, 10);
            }
        }

        public void LimparReservasVIR()
        {
            DateTime inicio = DateTime.Now;

            string errorMessage = string.Empty;

            DAL oDal = new DAL();
            try
            {
                IRLib.ValeIngresso oValeIngresso = new IRLib.ValeIngresso();
                int[] virs = oValeIngresso.LimpaReservasVIRInternet();

                foreach (int id in virs)
                    oDal.Execute("UPDATE Carrinho SET Status = 'E' WHERE ValeIngressoID = @ValeIngressoID AND Status = 'R'", new SqlParameter[] { new SqlParameter("@ValeIngressoID", id) });

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                oDAL.ConnClose();
                SalvarLogAcoes(true, inicio, DateTime.Now, "Limpar Reservas Vale Ingresso", errorMessage, enumEntryType.Information, 10);
            }
        }

        public void LimparReservasAssinaturas()
        {
            LogUtil.Info(string.Format("##RoboAtzSite.LimparReservasAssinaturas##"));

            DateTime inicio = DateTime.Now;

            string errorMessage = string.Empty;

            DAL oDal = new DAL();
            try
            {
                IRLib.Assinatura oAssinatura = new IRLib.Assinatura();
                int[] idsAssin = oAssinatura.LimpaReservasAssinatura();

                //foreach (int id in virs)
                //    oDal.Execute("UPDATE Carrinho SET Status = 'E' WHERE ValeIngressoID = @ValeIngressoID AND Status = 'R'", new SqlParameter[] { new SqlParameter("@ValeIngressoID", id) });
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                LogUtil.Info(string.Format("##RoboAtzSite.LimparReservasAssinaturas.SUCCESS##"));
                oDAL.ConnClose();
                SalvarLogAcoes(true, inicio, DateTime.Now, "Limpar Reservas Assinatura", errorMessage, enumEntryType.Information, 10);
            }
        }

        public void LimparReservasSpecialEvent()
        {
            string errorMessage = string.Empty;
            DateTime inicio = DateTime.Now;
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["SingleTonObjectsAtivo"]))
                    return;


                IRLib.Ingresso oIngresso = new IRLib.Ingresso();
                List<int> precosID = oIngresso.LimparReservasSpecialEventInternet();

                if (precosID.Count > 0)
                {
                    if (spEvent == null || (spEvent.bufferSpecialEvent.IsLoadingQRX || !spEvent.bufferSpecialEvent.AlreadyLoadedQRX))
                    {
                        if (spEvent == null)
                            this.spEvent = SingleTonObjects.RegistrarSpecialEvent();

                        while (spEvent.bufferSpecialEvent.IsLoadingQRX || !spEvent.bufferSpecialEvent.AlreadyLoadedQRX)
                            continue;
                    }

                    spEvent.bufferSpecialEvent.LiberarReserva(precosID);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, inicio, DateTime.Now, "Limpar Reservas Special Event", errorMessage, enumEntryType.Information, 10);
            }
        }

        public void ExpirarVIRsNaoTrocados()
        {
            string errorMessage = string.Empty;
            DateTime inicio = DateTime.Now;
            try
            {
                IRLib.ValeIngresso oValeIngresso = new IRLib.ValeIngresso();
                oValeIngresso.ExpirarVIRsNaoTrocados();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, inicio, DateTime.Now, "Expirar Vale Ingressos não Trocados", errorMessage, enumEntryType.Information, 12);
            }
        }

        public void EnviarAlertas()
        {
            try
            {
                AlertaApresentacao alertaApresentacao = new AlertaApresentacao();
                alertaApresentacao.PesquisarApresentacoes();
                if (alertaApresentacao.lista.Count > 0)
                {
                    SalvarLogAcoes(false, DateTime.Now, DateTime.Now, "Foram enviados " + alertaApresentacao.lista.Count + " alertas para o atendimento.", string.Empty, enumEntryType.Sucess, 55);
                    alertaApresentacao.Enviar();
                }
                else
                    SalvarLogAcoes(false, DateTime.Now, DateTime.Now, "Nenhum alerta encontrado para efetuar o envio.", string.Empty, enumEntryType.Information, 55);
            }
            catch (Exception ex)
            {
                SalvarLogAcoes(false, DateTime.Now, DateTime.Now, "Erro ao enviar os alertas de vendas na bilheteria.", ex.Message, enumEntryType.Error, 981);
            }
        }

        public void EnviarAlertaVendasPendentes()
        {
            try
            {
                AlertaVendaPendente vendaPendente = new AlertaVendaPendente();

                vendaPendente.PesquisaComprasPendentes(Convert.ToInt32(ConfigurationManager.AppSettings["DiasAlertaComprasPendentesPeanut"]));
                vendaPendente.Enviar(ConfigurationManager.AppSettings["EmailsAlertaPeanut"]);

            }
            catch (Exception ex)
            {
                SalvarLogAcoes(false, DateTime.Now, DateTime.Now, "Erro ao enviar os alertas de vendas do peanut.", ex.Message, enumEntryType.Error, 981);
            }
        }

        public void AtribuirHorarioEnvio()
        {
            DateTime inicio = DateTime.Now;
            try
            {
                new IRLib.Feedback().AtribuirHorarios
                        (Temporizador.Instancia.FeedBack.QuantidadePorHorario.Valor,
                        Temporizador.Instancia.FeedBack.HorasAposApresentacao.Valor);
            }
            catch (Exception ex)
            {
                SalvarLogAcoes(true, inicio, DateTime.Now, "Limpar Reservas", ex.Message, enumEntryType.Information, 10);
            }
        }

        public void EnviarEmailFeedback()
        {
            DateTime inicio = DateTime.Now;
            try
            {
                var lista = new IRLib.Feedback().BuscarVendasFeedback(
                    Temporizador.Instancia.FeedBack.QuantidadeDeBusca.Valor);

                if (lista.Count == 0)
                    return;

                new IRLib.Feedback().EncaminhaFeedback(lista,
                    Temporizador.Instancia.FeedBack.Respiro.Valor,
                    Temporizador.Instancia.FeedBack.Quantidade.Valor);
            }
            catch (Exception ex)
            {
                SalvarLogAcoes(true, inicio, DateTime.Now, "Enviar Email de Feedback", ex.Message, enumEntryType.Error, 11);
            }
        }

        private void LimparQuantidades(List<string> ListaApresentacaoIDxSetorID)
        {
            StringBuilder sqls = new StringBuilder();

            string SetorID = string.Empty;
            string ApresentacaoID = string.Empty;

            string SetorIDs = string.Empty;
            string strSql = string.Empty;

            if (ListaApresentacaoIDxSetorID.Count != 0)
            {
                foreach (string str in ListaApresentacaoIDxSetorID)
                {
                    ApresentacaoID = str.Substring(0, str.IndexOf("."));
                    SetorID = str.Substring(str.IndexOf(".") + 1);

                    strSql = "SELECT ID FROM Setor WHERE ApresentacaoID = " + ApresentacaoID + " AND IR_SetorID = " + SetorID;

                    try
                    {
                        using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                        {
                            while (dr.Read())
                                SetorIDs += dr["ID"].ToString() + ",";
                        }

                        oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                    }
                    catch (Exception ex)
                    {
                        oDAL.ConnClose();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        oDAL.ConnClose();
                    }
                }

                SetorIDs = SetorIDs.Substring(0, SetorIDs.LastIndexOf(","));

                oDAL.Execute("UPDATE Setor SET QtdeDisponivel = 0 WHERE ID NOT IN (" + SetorIDs + ")");
            }
        }

        /// <summary>
        /// Nova sincronizacao do Robo utilizando LINQ
        /// Criado por Caio Maganha Rosa
        /// 
        /// Atenção:
        ///     A Lista de Setores Possui tratamento especial já que a chave Primaria no Site é Composta (IR_SetorID e ApresentacaoID)
        ///     
        ///     A Exclusao dos dados é feita através de Bulk utilizando os ID's que não fazem parte do banco de Ingressos
        ///         mas estão no banco SiteIR
        ///         
        ///     Todos os Objetos de Sincronizacao estão dentro da Classe BufferInternet (IRLib -- Folder BufferInternet
        ///     Herde da classe abstrata aSync para que seja possivel utilizar a BufferList
        private void SyncronizeLists(bool local)
        {
            try
            {
                this.Progress("Sincronizando listas.");
                this.SyncronizeItem(BufferInternet.enumTables.Banner, local);
                this.SyncronizeItem(BufferInternet.enumTables.EventoTipoDestaque, local);
                this.SyncronizeItem(BufferInternet.enumTables.FormaPagamentoEvento, local);
                this.SyncronizeItem(BufferInternet.enumTables.Pacote, local);
                this.SyncronizeItem(BufferInternet.enumTables.NomenclaturaPacote, local);
                this.SyncronizeItem(BufferInternet.enumTables.PacoteItem, local);
                this.SyncronizeItem(BufferInternet.enumTables.EventoSubtipo, local);
                this.SyncronizeItem(BufferInternet.enumTables.Tipo, local);
                this.SyncronizeItem(BufferInternet.enumTables.Tipos, local);
                this.SyncronizeItem(BufferInternet.enumTables.ValeIngressoTipo, local);
                this.SyncronizeItem(BufferInternet.enumTables.Local, local);
                this.SyncronizeItem(BufferInternet.enumTables.Evento, local);
                this.SyncronizeItem(BufferInternet.enumTables.Filme, local);
                this.SyncronizeItem(BufferInternet.enumTables.Apresentacao, local);
                this.SyncronizeItem(BufferInternet.enumTables.Setor, local);
                this.SyncronizeItem(BufferInternet.enumTables.Preco, local);
                this.SyncronizeItem(BufferInternet.enumTables.PrecoParceiroMidia, local);
                this.SyncronizeItem(BufferInternet.enumTables.PontoVenda, local);
                this.SyncronizeItem(BufferInternet.enumTables.PontoVendaFormaPgto, local);
                this.SyncronizeItem(BufferInternet.enumTables.PontoVendaXFormaPgto, local);
                this.SyncronizeItem(BufferInternet.enumTables.PontoVendaHorario, local);
                this.SyncronizeItem(BufferInternet.enumTables.Serie, local);
                this.SyncronizeItem(BufferInternet.enumTables.SerieItem, local);
                this.SyncronizeItem(BufferInternet.enumTables.FormaPagamentoCotaItem, local);
                this.SyncronizeItem(BufferInternet.enumTables.FormaPagamentoSerie, local);
                this.SyncronizeItem(BufferInternet.enumTables.CotaItem, local);
                this.SyncronizeItem(BufferInternet.enumTables.VoceSabia, local);
                this.SyncronizeItem(BufferInternet.enumTables.Faq, local);

                if (local)
                    return;

                //Feito o join das threads para não finalizar a execução
                thBanner.Join();
                thEventoTipoDestaque.Join();
                thFormaPagamentoEvento.Join();
                thPacote.Join();
                thNomenclaturaPacote.Join();
                thPacoteItem.Join();
                thPacote.Join();
                thSubtipo.Join();
                thTipo.Join();
                thTipos.Join();
                thValeIngressoTipo.Join();
                thPontoVenda.Join();
                thPontoVendaHorario.Join();
                thPontoVendaFormaPgto.Join();
                thPontoVendaXFormaPagto.Join();
                thSerie.Join();
                thSerieItem.Join();
                thFormaPagamentoSerie.Join();
                thFormaPagamentoCotaItem.Join();
                thCotaItem.Join();
                thVoceSabia.Join();
                thFaq.Join();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sincroniza através de um switch, para que seja possivel utilizar somente thread nas tables certas para que
        /// não haja violacao de FK
        /// </summary>
        /// <param name="table"></param>
        private void SyncronizeItem(BufferInternet.enumTables table, bool local)
        {
            switch (table)
            {
                case BufferInternet.enumTables.Apresentacao:
                    this.buffer.lstApresentacao.Syncronize();
                    break;
                case BufferInternet.enumTables.Banner:
                    if (!local)
                    {
                        thBanner = new Thread(new ThreadStart(this.buffer.lstBanner.Syncronize));
                        thBanner.Start();
                    }
                    else
                        this.buffer.lstBanner.Syncronize();
                    break;
                case BufferInternet.enumTables.EventoTipoDestaque:
                    if (!local)
                    {
                        thEventoTipoDestaque = new Thread(new ThreadStart(this.buffer.lstEventoTipoDestaque.Syncronize));
                        thEventoTipoDestaque.Start();
                    }
                    else
                        this.buffer.lstEventoTipoDestaque.Syncronize();
                    break;
                case BufferInternet.enumTables.Evento:
                    this.buffer.lstEvento.Syncronize();
                    break;
                case BufferInternet.enumTables.Filme:
                    this.buffer.lstFilme.Syncronize();
                    break;
                case BufferInternet.enumTables.EventoSubtipo:
                    if (!local)
                    {
                        thSubtipo = new Thread(new ThreadStart(this.buffer.lstSubtipo.Syncronize));
                        thSubtipo.Start();
                    }
                    else
                        this.buffer.lstSubtipo.Syncronize();
                    break;
                case BufferInternet.enumTables.FormaPagamentoEvento:
                    if (!local)
                    {
                        thFormaPagamentoEvento = new Thread(new ThreadStart(this.buffer.lstFormaPagamentoEvento.Syncronize));
                        thFormaPagamentoEvento.Start();
                    }
                    else
                        this.buffer.lstFormaPagamentoEvento.Syncronize();
                    break;
                case BufferInternet.enumTables.Local:
                    this.buffer.lstLocal.Syncronize();
                    break;
                case BufferInternet.enumTables.Pacote:
                    if (!local)
                    {
                        thPacote = new Thread(new ThreadStart(this.buffer.lstPacote.Syncronize));
                        thPacote.Start();
                    }
                    else
                        this.buffer.lstPacote.Syncronize();
                    break;
                case BufferInternet.enumTables.NomenclaturaPacote:
                    if (!local)
                    {
                        thNomenclaturaPacote = new Thread(new ThreadStart(this.buffer.lstNomenclaturaPacote.Syncronize));
                        thNomenclaturaPacote.Start();
                    }
                    else
                        this.buffer.lstNomenclaturaPacote.Syncronize();
                    break;
                case BufferInternet.enumTables.PacoteItem:
                    if (!local)
                    {
                        thPacoteItem = new Thread(new ThreadStart(this.buffer.lstPacoteItem.Syncronize));
                        thPacoteItem.Start();
                    }
                    else
                        this.buffer.lstPacoteItem.Syncronize();
                    break;
                case BufferInternet.enumTables.Preco:
                    this.buffer.lstPreco.Syncronize();
                    break;
                case BufferInternet.enumTables.PrecoParceiroMidia:
                    this.buffer.lstPrecoParceiroMidia.Syncronize();
                    break;
                case BufferInternet.enumTables.Setor:
                    this.buffer.lstSetor.SyncronizeSetor();
                    break;
                case BufferInternet.enumTables.Tipo:
                    if (!local)
                    {
                        thTipo = new Thread(new ThreadStart(this.buffer.lstTipo.Syncronize));
                        thTipo.Start();
                    }
                    else
                        this.buffer.lstTipo.Syncronize();
                    break;
                case BufferInternet.enumTables.Tipos:
                    if (!local)
                    {
                        thTipos = new Thread(new ThreadStart(this.buffer.lstTipoSubtipo.Syncronize));
                        thTipos.Start();
                    }
                    else
                        this.buffer.lstTipoSubtipo.Syncronize();
                    break;
                case BufferInternet.enumTables.ValeIngressoTipo:
                    if (!local)
                    {
                        thValeIngressoTipo = new Thread(new ThreadStart(this.buffer.lstValeIngressoTipo.Syncronize));
                        thValeIngressoTipo.Start();
                    }
                    else
                        this.buffer.lstValeIngressoTipo.Syncronize();
                    break;
                case BufferInternet.enumTables.PontoVenda:
                    if (!local)
                    {
                        thPontoVenda = new Thread(new ThreadStart(this.buffer.lstPontoVenda.Syncronize));
                        thPontoVenda.Start();
                    }
                    else
                        this.buffer.lstPontoVenda.Syncronize();
                    break;
                case BufferInternet.enumTables.PontoVendaFormaPgto:
                    if (!local)
                    {
                        thPontoVendaFormaPgto = new Thread(new ThreadStart(this.buffer.lstPontoVendaFormaPgto.Syncronize));
                        thPontoVendaFormaPgto.Start();
                    }
                    else
                        this.buffer.lstPontoVendaFormaPgto.Syncronize();
                    break;
                case BufferInternet.enumTables.PontoVendaHorario:
                    if (!local)
                    {
                        thPontoVendaHorario = new Thread(new ThreadStart(this.buffer.lstPontoVendaHorario.Syncronize));
                        thPontoVendaHorario.Start();
                    }
                    else
                        this.buffer.lstPontoVendaHorario.Syncronize();
                    break;
                case BufferInternet.enumTables.PontoVendaXFormaPgto:
                    if (!local)
                    {
                        thPontoVendaXFormaPagto = new Thread(new ThreadStart(this.buffer.lstPontoVendaXFormaPgto.Syncronize));
                        thPontoVendaXFormaPagto.Start();
                    }
                    else
                        this.buffer.lstPontoVendaXFormaPgto.Syncronize();
                    break;
                case BufferInternet.enumTables.Serie:
                    if (local)
                        this.buffer.lstSerie.Syncronize();
                    else
                    {
                        this.thSerie = new Thread(new ThreadStart(this.buffer.lstSerie.Syncronize));
                        this.thSerie.Start();
                    }
                    break;
                case BufferInternet.enumTables.SerieItem:
                    if (local)
                        this.buffer.lstSerieItem.Syncronize();
                    else
                    {
                        this.thSerieItem = new Thread(new ThreadStart(this.buffer.lstSerieItem.Syncronize));
                        this.thSerieItem.Start();
                    }
                    break;
                case BufferInternet.enumTables.FormaPagamentoSerie:
                    if (local)
                        this.buffer.lstFormaPagamentoSerie.Syncronize();
                    else
                    {
                        this.thFormaPagamentoSerie = new Thread(new ThreadStart(this.buffer.lstFormaPagamentoSerie.Syncronize));
                        this.thFormaPagamentoSerie.Start();
                    }
                    break;
                case BufferInternet.enumTables.FormaPagamentoCotaItem:
                    if (local)
                        this.buffer.lstFormaPagamentoCotaItem.Syncronize();
                    else
                    {
                        this.thFormaPagamentoCotaItem = new Thread(new ThreadStart(this.buffer.lstFormaPagamentoCotaItem.Syncronize));
                        this.thFormaPagamentoCotaItem.Start();
                    }
                    break;
                case BufferInternet.enumTables.CotaItem:
                    if (local)
                        this.buffer.lstCotaItem.Syncronize();
                    else
                    {
                        this.thCotaItem = new Thread(new ThreadStart(this.buffer.lstCotaItem.Syncronize));
                        this.thCotaItem.Start();
                    }
                    break;
                case BufferInternet.enumTables.VoceSabia:
                    if (!local)
                    {
                        thVoceSabia = new Thread(new ThreadStart(this.buffer.lstVoceSabia.Syncronize));
                        thVoceSabia.Start();
                    }
                    else
                        this.buffer.lstVoceSabia.Syncronize();
                    break;
                case BufferInternet.enumTables.Faq:
                    if (!local)
                    {
                        thFaq = new Thread(new ThreadStart(this.buffer.lstFaq.Syncronize));
                        thFaq.Start();
                    }
                    else
                        this.buffer.lstFaq.Syncronize();
                    break;

            }
        }

        public void SincronizarMenu()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;
            try
            {
                new CriarMenu().Iniciar();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Atualização do Menu", errorMessage, enumEntryType.Information, 11);
            }
        }

        public void VerificarCodigos()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;
            try
            {
                new IRLib.CodigoBarraEvento().VerificarCodigos();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Gerador de Códigos", errorMessage, enumEntryType.Information, 11);
            }
        }


        public void AtualizarLatitudeLongitude()
        {
            var errorMessage = string.Empty;
            var dtInicio = DateTime.Now;
            try
            {
                LogUtil.Info(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude.InícioMétodoAtualizaCoordenadasPontoVenda## DATA INICIO: {0}", dtInicio));
                new PontoVenda().AtualizarCoordenadas();
                LogUtil.Info(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude.InícioMétodoAtualizaCoordenadasLocal##"));
                new Local().AtualizarCoordenadas();
                LogUtil.Info(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude.FimMétodosAtualizaCoordenadas##"));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude.EXCEPTION## MSG: {0}, STACKTRACE: {1}", ex.Message, ex.StackTrace), ex);
                errorMessage = ex.Message;
            }
            finally
            {
                LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude## DATA INICIO: {0}, DATA TERMINO: {1}, TEMPO DECORRIDO: {2}", dtInicio, DateTime.Now, DateTime.Now - dtInicio));
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Gerador de Códigos", errorMessage, enumEntryType.Information, 11);
            }
        }

        public void AtualizarLatitudeLongitude_SiteIR()
        {
            var errorMessage = string.Empty;
            var dtInicio = DateTime.Now;
            try
            {
                LogUtil.Info(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude_SiteIR.InícioMétodoAtualizaCoordenadasPontoVenda## DATA INICIO: {0}", dtInicio));
                new PontoVenda().AtualizarCoordenadas_SiteIR();
                LogUtil.Info(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude_SiteIR.InícioMétodoAtualizaCoordenadasLocal##"));
                new Local().AtualizarCoordenadas_SiteIR();
                LogUtil.Info(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude_SiteIR.FimMétodosAtualizaCoordenadas##"));
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude_SiteIR.EXCEPTION## MSG: {0}, STACKTRACE: {1}", ex.Message, ex.StackTrace), ex);
                errorMessage = ex.Message;
            }
            finally
            {
                LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarLatitudeLongitude_SiteIR## DATA INICIO: {0}, DATA TERMINO: {1}, TEMPO DECORRIDO: {2}", dtInicio, DateTime.Now, DateTime.Now - dtInicio));
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Gerador de Códigos", errorMessage, enumEntryType.Information, 11);
            }
        }


        public void SalvarLog(bool hasEnd, DateTime inicio, DateTime fim, string mensagem, string Erro, enumEntryType tipo, int eventID)
        {
            try
            {

                string eventoLog = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EventViwerAtualizacao"]))
                {
                    eventoLog = ConfigurationManager.AppSettings["EventViwerAtualizacao"];
                    if (!System.Diagnostics.EventLog.SourceExists(eventoLog))
                        System.Diagnostics.EventLog.CreateEventSource(eventoLog, eventoLog);

                    EventLog log = new EventLog(eventoLog);
                    log.Source = eventoLog;
                    log.Log = eventoLog;

                    if (Erro.Length > 0)
                    {
                        switch (tipo)
                        {
                            case enumEntryType.Error:
                                log.WriteEntry(string.Format("Erro gerado na sincronização de informações.\n{0}\nErro:{1}", mensagem, Erro), EventLogEntryType.Error, eventID);
                                break;
                            default:
                                log.WriteEntry(string.Format("Erro gerado na sincronização de informações.\n{0}\nErro:{1}", mensagem, Erro), EventLogEntryType.Warning, eventID);
                                break;
                        }
                    }
                    else
                    {
                        if (!hasEnd)
                        {
                            switch (tipo)
                            {
                                case enumEntryType.Sucess:
                                    log.WriteEntry(mensagem, EventLogEntryType.SuccessAudit, eventID);
                                    break;
                                case enumEntryType.Error:
                                    log.WriteEntry(mensagem, EventLogEntryType.Error, eventID);
                                    break;
                                default:
                                    log.WriteEntry(mensagem, EventLogEntryType.Information, eventID);
                                    break;
                            }
                        }
                        else
                        {
                            switch (tipo)
                            {
                                case enumEntryType.Sucess:
                                    log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                                    break;
                                case enumEntryType.Error:
                                    log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                                    break;
                                default:
                                    log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.Information, eventID);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SalvarLogAcoes(bool hasEnd, DateTime inicio, DateTime fim, string mensagem, string Erro, enumEntryType tipo, int eventID)
        {
            try
            {
                string eventoLog = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EventViwerAcoes"]))
                {
                    eventoLog = ConfigurationManager.AppSettings["EventViwerAcoes"];
                    if (!System.Diagnostics.EventLog.SourceExists(eventoLog))
                        System.Diagnostics.EventLog.CreateEventSource(eventoLog, eventoLog);

                    EventLog log = new EventLog(eventoLog);
                    log.Source = eventoLog;
                    log.Log = eventoLog;
                    if (Erro.Length > 0)
                    {
                        switch (tipo)
                        {
                            case enumEntryType.Error:
                                log.WriteEntry(string.Format("Erro gerado na sincronização de informações.\n{0}\nErro:{1}", mensagem, Erro), EventLogEntryType.Error, eventID);
                                break;
                            default:
                                log.WriteEntry(string.Format("Erro gerado na sincronização de informações.\n{0}\nErro:{1}", mensagem, Erro), EventLogEntryType.Warning, eventID);
                                break;
                        }
                    }
                    else
                    {
                        if (!hasEnd)
                        {
                            switch (tipo)
                            {
                                case enumEntryType.Sucess:
                                    log.WriteEntry(mensagem, EventLogEntryType.SuccessAudit, eventID);
                                    break;
                                case enumEntryType.Error:
                                    log.WriteEntry(mensagem, EventLogEntryType.Error, eventID);
                                    break;
                                default:
                                    log.WriteEntry(mensagem, EventLogEntryType.Information, eventID);
                                    break;
                            }
                        }
                        else
                        {
                            switch (tipo)
                            {
                                case enumEntryType.Sucess:
                                    log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                                    break;
                                case enumEntryType.Error:
                                    log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                                    break;
                                default:
                                    log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.Information, eventID);
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { }
        }

        public void SincronizarCinemas()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;
            try
            {
                new IRLib.Cinema.SincronizarCinemas().SincronizarTudo();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Atualização de Cinemas", errorMessage, enumEntryType.Information, 11);
            }
        }

        public Dictionary<int, List<int>> GetIngressosParaLiberarParceiroMidia()
        {
            BD bd = new BD();

            try
            {
                Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();

                string strSql = @"SELECT 
                    ApresentacaoSetor as ApresentacaoSetorID,
                    STUFF((
                        SELECT ',' + CAST([ID] AS VARCHAR(MAX)) 
                        FROM EventosDesbloquear (NOLOCK)
                        WHERE (ApresentacaoSetor = Results.ApresentacaoSetor) 
                        FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
                      ,1,1,'') AS IngressosID
                    FROM EventosDesbloquear (NOLOCK) as Results group by ApresentacaoSetor";


                using (IDataReader dr = bd.Consulta(strSql))
                {
                    while (dr.Read())
                        result.Add(dr["ApresentacaoSetorID"].ToInt32(), dr["IngressosID"].ToString().Split(',').Select(t => Int32.Parse(t)).ToList());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.FecharConsulta();
            }

        }

        public List<int> EventosLiberar()
        {
            DAL oDAL = new DAL();

            try
            {
                List<int> result = new List<int>();

                // Parse date-only value with invariant culture.
                var dateString = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                string strSql = @"SELECT e.ID FROM Evento e
                                where CAST(e.DataAberturaVenda as bigint) < " + dateString + " and Publicar = 'S'";


                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                        result.Add(dr["ID"].ToInt32());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }

        }

        public int[] getIngressosLugar(List<int> list)
        {
            List<int> result = new List<int>();

            BD bd = new BD();
            foreach (int i in list)
            {
                string strSql = "SELECT LugarID FROM tIngresso where Status='B' and ParceiroMidiaID is not null and BloqueioID !=0 and ID = " + i;

                try
                {
                    using (IDataReader dr = bd.Consulta(strSql))
                    {
                        while (dr.Read())
                            result.Add(dr["LugarID"].ToInt32());
                    }

                    bd.FecharConsulta();   // Fecha conexão da classe DataAccess
                }
                catch (Exception ex)
                {
                    bd.FecharConsulta();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    bd.FecharConsulta();
                }
            }
            return result.ToArray();
        }

        public List<ManipulacaoModel> getManipulacoes()
        {
            BD bd = new BD();
            List<ManipulacaoModel> result = new List<ManipulacaoModel>();
            string strSql = @"SELECT
	                            vbe.ID, vb.ID AS VBID, vb.Senha, c.Nome, c.Email
                            FROM
	                            tVendaBilheteriaEntrega (NOLOCK) AS vbe JOIN
	                            tVendaBilheteria (NOLOCK) AS vb ON vbe.VendaBilheteriaID = vb.ID JOIN
	                            tCliente (NOLOCK) AS c ON c.ID = vb.ClienteID
                            WHERE
	                            vbe.Tipo IN ('X','L') AND vbe.EmailEnviado = 0";

            try
            {
                using (IDataReader dr = bd.Consulta(strSql))
                {
                    while (dr.Read())
                        result.Add(new ManipulacaoModel()
                        {
                            ID = dr["ID"].ToInt32(),
                            VBID = dr["VBID"].ToInt32(),
                            Senha = dr["Senha"].ToString(),
                            Nome = dr["Nome"].ToString(),
                            Email = dr["Email"].ToString()
                        });
                }
                bd.FecharConsulta();   // Fecha conexão da classe DataAccess
            }
            catch (Exception ex)
            {
                bd.FecharConsulta();
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.FecharConsulta();
            }
            return result;
        }

        public void atualizarParaEmailEnviado(int ID)
        {
            BD bd = new BD();
            string strSql = @"UPDATE tVendaBilheteriaEntrega SET EmailEnviado = 1 WHERE ID = " + ID;
            try
            {
                bd.Executar(strSql);
                bd.FecharConsulta();
            }
            catch (Exception ex)
            {
                bd.FecharConsulta();
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.FecharConsulta();
            }
        }

        public void GeraPlanilhasEstorno()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;
            try
            {
                string strPath = ConfigurationManager.AppSettings["PathSalvarPlanilhas"].ToString();
                string emailCielo = ConfigurationManager.AppSettings["EmailEnvioEstornoCielo"].ToString();
                string emailRede = ConfigurationManager.AppSettings["EmailEnvioEstornoRede"].ToString();
                string emailValeCultura = ConfigurationManager.AppSettings["EmailEnvioEstornoValeCultura"].ToString();
                string emailEloCultura = ConfigurationManager.AppSettings["EmailEnvioEstornoEloCultura"].ToString();
                string emailAmex = ConfigurationManager.AppSettings["EmailEnvioEstornoAmex"].ToString();
                string emailPlanilhasManuais = ConfigurationManager.AppSettings["EmailEnvioEstornoPlanilhasManuais"].ToString();

                new IRLib.Codigo.EstornoDadosPlanilhas.Operadoras.Generator().GerarPlanilhas(strPath, emailRede, emailCielo, emailValeCultura,
                    emailEloCultura, emailAmex, emailPlanilhasManuais);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Gera Planilhas Estorno", errorMessage, enumEntryType.Information, 11);
            }
        }

        public bool SftpClientesCotaNet()
        {
            string errorMessage = string.Empty;
            DateTime dtInicio = DateTime.Now;
            bool retorno = false;
            try
            {
                IRLib.Codigo.Sftp.Sftp sFtp = new IRLib.Codigo.Sftp.Sftp();
                retorno = sFtp.Executa();

            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                SalvarLogAcoes(true, dtInicio, DateTime.Now, "Gera Relatorio Clientes Cota (sFTP)", errorMessage, enumEntryType.Information, 11);
            }
            return retorno;
        }
    }

    public class ManipulacaoModel
    {
        public int ID { get; set; }
        public int VBID { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
