using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for EventoLista
    /// </summary>
    public class EventoLista : List<Evento>
    {
        DAL oDAL = new DAL();

        Evento oEvento;

        public EventoLista()
        {
            this.Clear();
        }

        private int registros;
        public int Registros
        {
            get { return registros; }
            set { registros = value; }
        }

        public EventoLista CarregarDestaquePrincipal(string strEstado, string strCondicao)
        {
            string strSql = string.Empty;

            if (strCondicao == "1")
            {
                strSql = "SELECT TOP 1 Evento.IR_EventoID, Evento.Nome, Evento.Imagem, Evento.Release, Evento.Prioridade, " +
                           " Local.Nome AS Local, Local.Cidade, Local.Estado " +
                           " FROM Evento (NOLOCK) " +
                           " INNER JOIN Local (NOLOCK) ON Evento.LocalID = Local.IR_LocalID " +
                           " INNER JOIN Apresentacao (NOLOCK) ON Evento.IR_EventoID = Apresentacao.EventoID " +
                           " WHERE Apresentacao.Horario > " + DateTime.Now.ToString("yyyyMMddHHmmss") + " AND Imagem <> '' " +
                           " ORDER BY Apresentacao.Horario";
            }
            else
            {
                strSql = " SELECT TOP 1 Evento.IR_EventoID, Evento.Nome, Evento.Imagem, Evento.Release, Evento.Prioridade, " +
                            " Local.Nome AS Local, Local.Cidade, Local.Estado " +
                            " FROM Evento (NOLOCK) " +
                            " INNER JOIN Local (NOLOCK) ON Evento.LocalID = Local.IR_LocalID " +
                            " WHERE (Evento.Destaque = 1) AND DisponivelAvulso = 1" + strEstado +
                            " ORDER BY Evento.Prioridade";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["IR_EventoID"].ToString()));
                        oEvento.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        oEvento.Imagem = dr["Imagem"].ToString();
                        oEvento.Release = dr["Release"].ToString();
                        if (oEvento.Release.Length > 150)
                            oEvento.Release = oEvento.Release.Substring(0, 150) + "...";

                        oEvento.LocalDestaque = dr["Local"].ToString() + " - " + dr["Cidade"].ToString() + " - " + dr["Estado"].ToString();

                        this.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
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

        public int CarregarQuantidadeEventos(string clausula)
        {
            int qtde = 0;

            string strSql = "";

            if (clausula.Trim() != string.Empty)
                strSql = "SELECT COUNT(DISTINCT E.ID) Total FROM Evento AS E (NOLOCK) INNER JOIN Local AS L (NOLOCK) ON E.LocalID = L.IR_LocalID WHERE " + clausula + " AND  DisponivelAvulso = 1";
            else
                strSql = "SELECT COUNT(DISTINCT ID) Total FROM Evento (NOLOCK) WHERE DisponivelAvulso = 1";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        qtde = Convert.ToInt32(dr["Total"].ToString());
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return qtde;
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

        public EventoLista CarregarDestaques(string clausula, int status, string strEstado)
        {
            string strSql = string.Empty;

            // status: Se for 0 está carregando novos Eventos para a Default, se for 1 está navegando entre
            //         os eventos já existentes.

            strSql = "sp_GetDestaques " + "'" + clausula + "'," + status + ",'" + strEstado + "'";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["ID"].ToString()));
                        oEvento.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        oEvento.Imagem = dr["Imagem"].ToString();
                        oEvento.LocalID = Convert.ToInt32(dr["IDLocal"]);
                        oEvento.LocalDestaque = dr["Local"].ToString() + " - " + dr["Cidade"].ToString() + " - " + dr["Estado"].ToString();

                        if (DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") != DateTime.ParseExact(dr["HorarioMax"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                            oEvento.Horario = "De " + DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") + " a " + DateTime.ParseExact(dr["HorarioMax"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        else
                            oEvento.Horario = "Dia " + DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                        this.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
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

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de uma clausula WHERE 
        /// </summary>    
        private EventoLista CarregarLista(string clausula, int startRowIndex, int numRows)
        {
            string strSql = string.Empty;

            if (clausula != "")
            {
                if (numRows == 0)
                {
                    strSql = "SELECT DISTINCT " +
                            "Evento.IR_EventoID, Evento.Nome, Evento.Prioridade, Evento.Imagem, Local.Nome as LocalNome, Local.Estado, Local.Cidade, Local.Pais, Tipo.IR_TipoID as TipoID, Evento.SubtipoID " +
                            "FROM Evento (NOLOCK) " +
                            "	LEFT JOIN EventoSubtipo (NOLOCK) ON Evento.SubtipoID = EventoSubtipo.IR_SubtipoID " +
                            "   LEFT JOIN Tipo (NOLOCK) ON EventoSubtipo.TipoID = Tipo.IR_TipoID " +
                            "INNER JOIN Local (NOLOCK) ON Evento.LocalID = Local.IR_LocalID " +
                            "INNER JOIN Apresentacao (NOLOCK) ON Evento.IR_EventoID = Apresentacao.EventoID " +
                            "WHERE (" + clausula + ") ORDER BY Evento.Nome";
                }
                else
                {
                    strSql =
                        //DECLARE @startRowIndex int;
                        //DECLARE @numRows int;
                        //SET @startRowIndex = 3;   
                        //SET @numRows = 3;

                            "WITH tbGeral AS( " +
                            "	SELECT DISTINCT	Evento.IR_EventoID, Evento.Nome, Evento.Imagem, Local.Nome as LocalNome, Local.Estado, Local.Cidade, Local.Pais, Tipo.IR_TipoID as TipoID, Evento.SubtipoID  " +
                            "	FROM Evento (NOLOCK) " +
                            "	LEFT JOIN EventoSubtipo (NOLOCK) ON Evento.SubtipoID = EventoSubtipo.IR_SubtipoID " +
                            "   LEFT JOIN Tipo (NOLOCK) ON EventoSubtipo.TipoID = Tipo.IR_TipoID " +
                            "	INNER JOIN Local (NOLOCK) ON Evento.LocalID = Local.IR_LocalID " +
                            "	INNER JOIN Apresentacao (NOLOCK) ON Evento.IR_EventoID = Apresentacao.EventoID " +
                            "   WHERE " + clausula + " )," +

                            "tbCount AS( " +
                            "	SELECT Count(IR_EventoID) as Registros FROM tbGeral), " +

                            "tbOrdenada AS( " +
                            "	SELECT *, ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber' FROM tbGeral) " +

                            "SELECT * FROM tbOrdenada, tbCount " +
                            "WHERE RowNumber between " + startRowIndex + " and " + (startRowIndex + numRows - 1) + " ORDER BY Nome";
                }
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["IR_EventoID"].ToString()));
                        oEvento.Nome = dr["Nome"].ToString();
                        oEvento.Imagem = dr["Imagem"].ToString();
                        oEvento.Local = dr["LocalNome"].ToString();
                        oEvento.Cidade = dr["Cidade"].ToString();
                        oEvento.Estado = dr["Estado"].ToString();
                        oEvento.Pais = dr["Pais"].ToString();
                        oEvento.TipoID = dr["TipoID"].ToInt32();
                        oEvento.SubtipoID = dr["SubtipoID"].ToInt32();

                        if (numRows != 0)
                        {
                            this.registros = Convert.ToInt32(dr["Registros"].ToString());
                        }
                        this.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess

                VerificarEventosRC(this);

                return this;
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

        private EventoLista CarregarLista(string clausula)
        {
            return CarregarLista(clausula, 0, 0);
        }

        /// <summary>
        /// Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de um LocalID
        /// </summary>
        public EventoLista CarregarDadosPorLocalID(int id)
        {
            return CarregarLista("LocalID = " + id);
        }

        public EventoLista CarregarDadosPorLocalID(int id, int index, int qtdeLinhas)
        {
            int EventoMorro = ConfigurationManager.AppSettings["EventoMorro"].ToInt32();
            string busca = "LocalID = " + id + " AND Evento.IR_EventoID <> " + EventoMorro;

            return CarregarLista("LocalID = " + id, index, qtdeLinhas);
        }

        /// <summary>
        /// Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de um Nome de Evento
        /// </summary>
        public EventoLista CarregarDadosPorEventoNome1(string eventoNome)
        {
            eventoNome = eventoNome.Replace("'", "''");
            return CarregarLista("Evento.Nome LIKE '%" + eventoNome + "%'");
        }

        public EventoLista CarregarDadosPorEventoNomeComPrefeitura(string eventoNome, string prefeitura, int index, int qtdeLinhas)
        {
            eventoNome = eventoNome.Replace("'", "''");
            return CarregarLista("(Evento.PalavraChave LIKE '%" + eventoNome + "%' COLLATE SQL_Latin1_General_CP1_CI_AI or Evento.Nome LIKE '%" + eventoNome + "%' COLLATE SQL_Latin1_General_CP1_CI_AI) " + prefeitura, index, qtdeLinhas);
        }

        public EventoLista CarregarDadosPorEventoNome(string eventoNome, int index, int qtdeLinhas)
        {
            int EventoMorro = ConfigurationManager.AppSettings["EventoMorro"].ToInt32();

            eventoNome = eventoNome.Replace("'", "''");

            string busca = "Evento.PalavraChave LIKE '%" + eventoNome + "%' COLLATE SQL_Latin1_General_CP1_CI_AI or Evento.Nome LIKE '%" + eventoNome + "%' COLLATE SQL_Latin1_General_CP1_CI_AI AND Evento.IR_EventoID <> " + EventoMorro;

            return CarregarLista(busca, index, qtdeLinhas);
        }

        public EventoLista CarregarEventosHome()
        {
            try
            {
                string strSql = string.Empty;
                strSql = "sp_getEventos";
                try
                {
                    using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                    {
                        while (dr.Read())
                        {
                            oEvento = new Evento(Convert.ToInt32(dr["ID"].ToString()));
                            oEvento.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                            oEvento.Imagem = dr["Imagem"].ToString();
                            oEvento.LocalID = Convert.ToInt32(dr["IDLocal"]);
                            oEvento.Cidade = Convert.ToString(dr["Cidade"]);
                            oEvento.Estado = Convert.ToString(dr["Estado"]);
                            oEvento.Local = Convert.ToString(dr["Local"]);
                            if (DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") != DateTime.ParseExact(dr["HorarioMax"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                oEvento.Horario = "De " + DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") + " a " + DateTime.ParseExact(dr["HorarioMax"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                            else
                                oEvento.Horario = "Dia " + DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                            this.Add(oEvento);
                        }
                    }

                    oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                    return this;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de um TipoNome
        /// </summary>
        public EventoLista CarregarDadosPorEventoTipo(string eventoTipo)
        {
            eventoTipo = eventoTipo.Replace("'", "''");
            return CarregarLista("Tipo.Nome LIKE = '%" + eventoTipo + "%'");
        }

        /// <summary>
        /// Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de um TipoID
        /// </summary>
        public EventoLista CarregarDadosPorEventoTipo(int eventoTipoID)
        {
            ///POG: Se for -1 carrega todos menos: Teatro, show e esportes
            if (eventoTipoID == -1)
                return CarregarLista("Tipo.IR_TipoID NOT IN (1,2,6)", 1, 8);
            else
                return CarregarLista("Tipo.IR_TipoID = " + eventoTipoID, 1, 8);
        }

        public EventoLista CarregarDadosPorEventoTipo(int eventoTipoID, int index, int qtdeLinhas)
        {
            ///POG: Se for -1 carrega todos
            if (eventoTipoID == -1)
                return CarregarLista("Tipo.IR_TipoID NOT IN (-999)", index, qtdeLinhas);
            else
                return CarregarLista("Tipo.IR_TipoID = " + eventoTipoID, index, qtdeLinhas);
        }

        /// <summary>
        /// Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de um período
        /// </summary>
        public EventoLista CarregarDadosPorUFLocalTipoData(string uf, string cidade, int local, int tipo, DateTime dataInicio, DateTime dataFim, int index, int qtdeLinhas)
        {
            string clausula = string.Empty;

            if (uf != string.Empty)
                clausula = "Local.Estado ='" + Util.StringToBD(uf) + "' ";

            if (cidade != string.Empty)
            {
                clausula += " AND Local.Cidade ='" + Util.StringToBD(cidade) + "' ";
            }

            if (local != 0)
                clausula += " AND Local.IR_LocalID = " + local + "";

            if (tipo != 0)
                clausula += " AND Tipo.IR_TipoID = " + tipo + "";

            if (dataInicio != null)
            {
                if (uf != string.Empty)
                    clausula += " AND Horario >= '" + Util.StringToBD(dataInicio.ToString("yyyyMMdd")) + "'";
                else
                    clausula = "Horario >= '" + Util.StringToBD(dataInicio.ToString("yyyyMMdd")) + "'";
            }

            if (dataFim != DateTime.MinValue)
                clausula += " AND Horario <= '" + Util.StringToBD(dataFim.ToString("yyyyMMdd")) + "'";

            if (qtdeLinhas == 0)
                return CarregarLista(clausula);
            else
                return CarregarLista(clausula, index, qtdeLinhas);
        }

        /// <summary>
        /// Retorna uma Lista de Eventos do tipo Evento, 
        /// a partir de um TipoID
        /// </summary>
        public EventoLista CarregarDados()
        {
            return CarregarLista("1=1", 1, 8);
        }

        private List<EstruturaEvento> CarregarListaMobile(string clausula)
        {
            try
            {
                List<EstruturaEvento> retorno = new List<EstruturaEvento>();
                EstruturaEvento evento = new EstruturaEvento();

                string strSql = @"		SELECT DISTINCT
                    Evento.IR_EventoID, Evento.Nome, Evento.Prioridade, Evento.Imagem, Local.IR_LocalID, Local.Nome as LocalNome, Local.Estado, Local.Cidade, Local.Pais, 
                    Tipo.IR_TipoID as TipoID, Evento.SubtipoID, Apresentacao.ID AS ApresentacaoID, Apresentacao.Horario
                    FROM Evento (NOLOCK)
					LEFT JOIN  Tipos (NOLOCK) ON Evento.IR_EventoID = Tipos.EventoID
                    LEFT JOIN EventoSubtipo (NOLOCK) ON EventoSubtipo.IR_SubtipoID = Evento.SubtipoID OR EventoSubtipo.IR_SubtipoID = Tipos.EventoSubtipoID 
                    LEFT JOIN Tipo (NOLOCK) ON EventoSubtipo.TipoID = Tipo.IR_TipoID
                    INNER JOIN Local (NOLOCK) ON Evento.LocalID = Local.IR_LocalID
                    INNER JOIN Apresentacao (NOLOCK) ON Evento.IR_EventoID = Apresentacao.EventoID
                    WHERE ( " + clausula + " ) ORDER BY Evento.Nome";

                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        if (retorno.Where(c => c.ID == dr["IR_EventoID"].ToInt32()).Count() == 0)
                        {
                            evento = new EstruturaEvento();

                            evento.ID = dr["IR_EventoID"].ToInt32();
                            evento.Nome = dr["Nome"].ToString();
                            evento.Imagem = dr["Imagem"].ToString();
                            evento.LocalID = dr["IR_LocalID"].ToInt32();
                            evento.Local = dr["LocalNome"].ToString();
                            evento.Cidade = dr["Cidade"].ToString();
                            evento.Estado = dr["Estado"].ToString();
                            evento.Pais = dr["Pais"].ToString();
                            evento.Tipo = dr["TipoID"].ToInt32();
                            evento.SubTipo = dr["SubtipoID"].ToInt32();

                            retorno.Add(evento);
                        }

                        evento.Apresentacoes.Add(new EstruturaApresentacaoSimples()
                        {
                            EventoID = dr["IR_EventoID"].ToInt32(),
                            IR_ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                            HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM"),
                            Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture),
                        });
                    }
                }
                return retorno;
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

        private List<EstruturaEvento> CarregarListaMobile(string HorarioAtual, string SubTipoID, string DatasProximas, string Pesquisa, string LocalID, string TipoID, string Cidade, string Estado, string Latitude, string Longitude, int Distancia, string Coordenada, string eventosNaoExibir = "")
        {
            try
            {
                List<EstruturaEvento> retorno = new List<EstruturaEvento>();
                EstruturaEvento evento = new EstruturaEvento();
                int distanciaEvento = 0;
                string cWhere = string.Empty;

                string strSql = @"SELECT DISTINCT
                    Evento.IR_EventoID, Evento.Nome, Evento.Prioridade, Evento.Imagem, Evento.PublicarSemVendaMotivo, Evento.DataAberturaVenda, Local.IR_LocalID, Local.Nome as LocalNome, Local.Estado, Local.Cidade, Local.Pais,
                    Local.Latitude, Local.Longitude, Tipo.IR_TipoID as TipoID, Evento.SubtipoID, Apresentacao.ID AS ApresentacaoID, Apresentacao.Horario
                    FROM Evento (NOLOCK)
                    	LEFT JOIN EventoSubtipo (NOLOCK) ON Evento.SubtipoID = EventoSubtipo.IR_SubtipoID
                       LEFT JOIN Tipo (NOLOCK) ON EventoSubtipo.TipoID = Tipo.IR_TipoID
                    INNER JOIN Local (NOLOCK) ON Evento.LocalID = Local.IR_LocalID
                    INNER JOIN Apresentacao (NOLOCK) ON Evento.IR_EventoID = Apresentacao.EventoID ";

                 
                cWhere = @" WHERE (CONVERT(VARCHAR,DATEADD(day, Evento.MenorPeriodoEntrega, Substring('" + HorarioAtual + @"', 0 ,9)), 112) <= Substring(Horario, 0, 9))
                        AND Horario > '" + HorarioAtual + "' ";

                if (!String.IsNullOrEmpty(eventosNaoExibir))
                {
                    cWhere += " AND IR_EventoID not in (" + eventosNaoExibir + ") ";
                }

                cWhere += SubTipoID + DatasProximas + Pesquisa + LocalID + TipoID + Cidade + Estado + Coordenada;

                if (cWhere != string.Empty)
                    strSql += cWhere;

                strSql += " Order by Apresentacao.Horario";

                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        if (!string.IsNullOrEmpty(Latitude) || !string.IsNullOrEmpty(Longitude))
                        {
                            distanciaEvento = IRLib.CEP.CalcularDistancia(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude), Convert.ToDouble(dr["Latitude"]), Convert.ToDouble(dr["Longitude"]));
                        }

                        if (retorno.Where(c => c.ID == dr["IR_EventoID"].ToInt32()).Count() == 0)
                        {
                            evento = new EstruturaEvento();

                            evento.ID = dr["IR_EventoID"].ToInt32();
                            evento.Nome = dr["Nome"].ToString();
                            evento.Imagem = dr["Imagem"].ToString();
                            evento.LocalID = dr["IR_LocalID"].ToInt32();
                            evento.Local = dr["LocalNome"].ToString();
                            evento.Cidade = dr["Cidade"].ToString();
                            evento.Estado = dr["Estado"].ToString();
                            evento.Pais = dr["Pais"].ToString();
                            evento.Tipo = dr["TipoID"].ToInt32();
                            evento.SubTipo = dr["SubtipoID"].ToInt32();
                            switch (dr["PublicarSemVendaMotivo"].ToInt32())
                            {
                                case 1:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaOnlineNaoDisponivel;
                                    break;
                                case 2:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaSomenteCallCenter;
                                    break;
                                case 3:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendasEncerradas;
                                    break;
                                case 4:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendasNaoIniciadas;
                                    break;
                                case 5:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivelApenasParaPacotes;
                                    break;
                                case 6:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivelDeterminadaData;
                                    break;
                                default:
                                    evento.NaoVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivel;
                                    break;
                            }
                            if ((dr["DataAberturaVenda"] != null) && (dr["DataAberturaVenda"].ToString().Length == 14))
                            {
                                evento.NaoVendaDataInicio = DateTime.ParseExact(dr["DataAberturaVenda"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                            }
                            if (!string.IsNullOrEmpty(Latitude) || !string.IsNullOrEmpty(Longitude)) { evento.Distancia = distanciaEvento; }

                            retorno.Add(evento);
                        }

                        evento.Apresentacoes.Add(new EstruturaApresentacaoSimples()
                        {
                            EventoID = dr["IR_EventoID"].ToInt32(),
                            IR_ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                            HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM"),
                            Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture),
                        });
                    }
                }

                if (Distancia > 0)
                {
                    return retorno.Where(c => c.Distancia <= Distancia).OrderBy(c => c.Distancia).ToList();
                }

                return retorno;
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

        public List<EstruturaEvento> CarregarDadosMobile()
        {
            return this.CarregarListaMobile("1=1").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorDatasProximas(int meses)
        {
            string data = DateTime.Now.AddMonths(meses).ToString("yyyyMMddhhmmss");

            return this.CarregarListaMobile("Apresentacao.Horario < '" + data + "'").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorNomeOuLocal(string Nome)
        {
            return this.CarregarListaMobile(" Local.Nome LIKE '%" + Nome + "%' OR Evento.PalavraChave LIKE '%" + Nome + "%' or Evento.Nome LIKE '%" + Nome + "%'").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorNome(string eventoNome)
        {
            return this.CarregarListaMobile(" Evento.Nome LIKE '%" + eventoNome + "%'").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorData(string DataEvento)
        {
            DateTime dataEvento = Convert.ToDateTime(DataEvento);

            return this.CarregarListaMobile("  Apresentacao.Horario >'" + dataEvento.ToString("yyyyMMddhhmmss") + "' AND Apresentacao.Horario >'" + dataEvento.AddDays(30).ToString("yyyyMMddhhmmss") + "' ").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorNomeLocal(string NomeLocal)
        {
            return this.CarregarListaMobile(" Local.Nome LIKE '%" + NomeLocal + "%'").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorGenero(int GeneroID)
        {
            return this.CarregarListaMobile("Tipo.IR_TipoID = " + GeneroID).OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorCategoria(int CategoriaID)
        {
            return this.CarregarListaMobile("EventoSubtipo.IR_SubtipoID = " + CategoriaID).OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorEstado(string estado)
        {
            return this.CarregarListaMobile("Local.Estado = '" + estado + "'").OrderBy(c => c.ID).ToList();
        }

        public List<EstruturaEvento> CarregarDadosMobilePorCidade(string cidade)
        {
            return this.CarregarListaMobile("Local.Cidade = '" + cidade + "'").OrderBy(c => c.ID).ToList();
        }

        public EventoLista CarregarDadosComPrefeitura(string prefeitura, int index, int qtdeLinhas)
        {
            return CarregarLista(prefeitura, index, qtdeLinhas);
        }

        public EventoLista CarregarDados(int index, int qtdeLinhas)
        {
            return CarregarLista("1=1", index, qtdeLinhas);
        }

        public EventoLista CarregarRandomico(int quantidade)
        {
            string strSql = "SELECT TOP " + quantidade + " Evento.ID,IR_EventoID, Evento.Nome, Evento.Release, Evento.Imagem, Local.Nome, Cidade, Estado FROM Evento (NOLOCK), Local (NOLOCK) WHERE LocalID = IR_LocalID AND DisponivelAvulso = 1 ORDER BY NEWID()";
            this.Clear();
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["IR_EventoID"].ToString()));
                        oEvento.Nome = dr["Nome"].ToString();
                        oEvento.Release = dr["Release"].ToString();
                        if (oEvento.Release.Length > 150)
                            oEvento.Release = oEvento.Release.Substring(0, 150) + "...";

                        oEvento.Imagem = dr["Imagem"].ToString();
                        oEvento.LocalDestaque = dr["Cidade"].ToString() + " - " + dr["Estado"].ToString();
                        this.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
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

        public EventoLista CarregarRandomico(int quantidade, int tipo)
        {
            string strSql = "";

            if (tipo == -1)
                strSql = "SELECT TOP " + quantidade + " Evento.ID,IR_EventoID, Evento.Nome, Evento.Release, Evento.Imagem, Local.Nome, Cidade, Estado FROM Evento (NOLOCK), Local (NOLOCK) WHERE LocalID = IR_LocalID AND Evento.Imagem <> '' AND DisponivelAvulso = 1 ORDER BY NEWID()";
            else
                strSql = "SELECT TOP " + quantidade + " Evento.ID,IR_EventoID, Evento.Nome, Evento.Release, Evento.Imagem, Local.Nome, Cidade, Estado FROM Evento (NOLOCK), Local (NOLOCK) WHERE Evento.TipoID = " + tipo + " AND LocalID = IR_LocalID AND Evento.Imagem <> ''  AND DisponivelAvulso = 1 ORDER BY NEWID()";

            this.Clear();
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["IR_EventoID"].ToString()));
                        oEvento.Nome = dr["Nome"].ToString();
                        oEvento.Release = dr["Release"].ToString();
                        if (oEvento.Release.Length > 150)
                            oEvento.Release = oEvento.Release.Substring(0, 150) + "...";

                        oEvento.Imagem = dr["Imagem"].ToString();
                        oEvento.LocalDestaque = dr["Cidade"].ToString() + " - " + dr["Estado"].ToString();
                        this.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
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

        public EventoLista CarregarPesquisa(int tipoID, int subtipoID, string estado, string cidade, string horarioMinimo, string horarioMaximo)
        {
            StringBuilder stbClausula = new StringBuilder();

            if (tipoID != 0)
                stbClausula.Append("Evento.IR_EventoID" + tipoID);

            if (stbClausula.ToString().Trim() != string.Empty)
                stbClausula.Append(" AND ");

            //if (subtipoID != 0)
            //    stbClausula.Append("Evento
            return CarregarPesquisa(stbClausula.ToString());
        }

        public EventoLista CarregarPesquisa(string Clausula)
        {
            try
            {


                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT ");
                stbSQL.Append("Evento.IR_EventoID, ");
                stbSQL.Append("Evento.Nome, ");
                stbSQL.Append("Evento.Imagem, ");
                stbSQL.Append("Evento.Prioridade, ");
                stbSQL.Append("Evento.LocalID, ");
                stbSQL.Append("Local.Nome AS Local, ");
                stbSQL.Append("Local.Cidade, ");
                stbSQL.Append("Local.Estado, ");
                stbSQL.Append("MIN(Apresentacao.Horario) AS HorarioMin, ");
                stbSQL.Append("MAX(Apresentacao.Horario) AS HorarioMax ");
                stbSQL.Append("FROM Evento (NOLOCK) ");
                stbSQL.Append("INNER JOIN Local (NOLOCK) On ");
                stbSQL.Append("Evento.LocalID = Local.IR_LocalID ");
                stbSQL.Append("INNER JOIN Apresentacao (NOLOCK) ON ");
                stbSQL.Append("Apresentacao.EventoID = Evento.IR_EventoID ");
                stbSQL.Append(Clausula);
                stbSQL.Append("GROUP BY Evento.LocalID, Evento.IR_EventoID, ");
                stbSQL.Append("Evento.Nome, Evento.Imagem, Evento.Prioridade, Local.Nome, Local.Cidade, Local.Estado ");


                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["ID"].ToString()));
                        oEvento.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        oEvento.Imagem = Convert.ToString(dr["Imagem"]);
                        oEvento.LocalID = Convert.ToInt32(dr["IDLocal"]);
                        oEvento.Cidade = Convert.ToString(dr["Cidade"]);
                        oEvento.Estado = Convert.ToString(dr["Estado"]);
                        oEvento.Local = Convert.ToString(dr["Local"]);
                        if (DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") != DateTime.ParseExact(dr["HorarioMax"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                            oEvento.Horario = "De " + DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") + " a " + DateTime.ParseExact(dr["HorarioMax"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        else
                            oEvento.Horario = "Dia " + DateTime.ParseExact(dr["HorarioMin"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                        this.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
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

        public static EventoLista Pesquisar(int startRowIndex, int numRows, int tipoID, int subTipoID, string estado, string cidade, int localID, bool filtroDataUnica, DateTime? data, DateTime? dataDe, DateTime? dataAte)
        {
            DAL oDAL = new DAL();
            StringBuilder strFiltros = new StringBuilder(" WHERE 1=1 ");

            int EventoMorro = ConfigurationManager.AppSettings["EventoMorro"].ToInt32();

            if (tipoID > 0)
                strFiltros.Append(" AND (TipoIDEvento = " + tipoID + ") ");

            if (subTipoID > 0)
                strFiltros.Append(" AND (SubTipoIDEvento = " + subTipoID + " OR SubTipoID = " + subTipoID + ") ");

            if (estado.Length > 0)
                strFiltros.Append(" AND Estado = '" + Util.StringToBD(estado) + "'");

            if (cidade.Length > 0)
                strFiltros.Append(" AND Cidade = '" + Util.StringToBD(cidade) + "'");

            if (localID > 0)
                strFiltros.Append(" AND LocalID = " + localID);

            if (filtroDataUnica)
            {
                if (data != null)
                    strFiltros.Append(" AND Horario LIKE '" + data.Value.ToString("yyyyMMdd") + "%'");
            }
            else
            {
                if (dataDe != null)
                    strFiltros.Append(" AND Horario >= '" + dataDe.Value.ToString("yyyyMMdd") + "' ");

                if (dataAte != null)
                    strFiltros.Append(" AND Horario < '" + dataAte.Value.AddDays(1).ToString("yyyyMMdd") + "' ");
            }

            strFiltros.Append(" AND IR_EventoID <> " + EventoMorro);

            string strSql =
                               "WITH tbGeral AS( " +
                               "	SELECT " +
                               "	DISTINCT IR_EventoID, EventoNome, Imagem, LocalNome, Estado, Cidade, SubTipo, Tipo, TipoIDEvento, SubTipoIDEvento" +
                               "	FROM vwBaseBusca " +
                               strFiltros.ToString() + ")," +
                               "tbCount AS( " +
                               "	SELECT Count(IR_EventoID) as Registros FROM tbGeral), " +
                               "tbOrdenada AS( " +
                               "	SELECT IR_EventoID, EventoNome, Imagem, LocalNome, Estado, Cidade, SubTipo, Tipo,TipoIDEvento, SubTipoIDEvento, ROW_NUMBER() OVER (ORDER BY EventoNome) AS 'RowNumber' FROM tbGeral) " +
                               "SELECT IR_EventoID, EventoNome, Imagem, LocalNome, Estado, Cidade, SubTipo, Tipo, TipoIDEvento, RowNumber, Registros FROM tbOrdenada, tbCount " +
                               "WHERE RowNumber between " + startRowIndex + " and " + (startRowIndex + numRows - 1) + " ORDER BY EventoNome";

            EventoLista objEventoLista = new EventoLista();
            Evento oEvento = new Evento();

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oEvento = new Evento(Convert.ToInt32(dr["IR_EventoID"].ToString()));
                        oEvento.Nome = dr["EventoNome"].ToString();
                        oEvento.Local = dr["LocalNome"].ToString();
                        oEvento.Imagem = dr["Imagem"].ToString();
                        oEvento.Estado = dr["Estado"].ToString();
                        oEvento.Cidade = dr["Cidade"].ToString();
                        oEvento.LocalDestaque = dr["LocalNome"].ToString() + " - " + dr["Estado"].ToString() + "-" + dr["Cidade"].ToString();
                        oEvento.Tipo = dr["Tipo"].ToString();
                        oEvento.Subtipo = dr["SubTipo"].ToString();

                        if (numRows != 0)
                        {
                            objEventoLista.registros = Convert.ToInt32(dr["Registros"].ToString());
                        }
                        objEventoLista.Add(oEvento);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess

                objEventoLista.VerificarEventosRC(objEventoLista);

                return objEventoLista;
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

        public List<EstruturaEvento> BuscaTodosEventosPorCoordenada(string Latitude, string Longitude, int distancia)
        {
            try
            {
                oDAL = new DAL();

                List<EstruturaEvento> retorno = new List<EstruturaEvento>();
                EstruturaEvento evento = new EstruturaEvento();

                DateTime data = DateTime.Now;

                string sql = string.Format(@"SELECT DISTINCT e.IR_EventoID, e.Nome, e.Imagem, l.Nome as Local, l.IR_LocalID , l.Cidade, l.Estado,l.Pais, l.Latitude, l.Longitude,
                                        Tipo.IR_TipoID as TipoID, e.SubtipoID, e.SubtipoID, ap.ID AS ApresentacaoID, ap.Horario
                                        FROM Evento e
                                        LEFT JOIN EventoSubtipo (NOLOCK) ON e.SubtipoID = EventoSubtipo.IR_SubtipoID
                                        LEFT JOIN Tipo (NOLOCK) ON EventoSubtipo.TipoID = Tipo.IR_TipoID
                                        INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID
                                        INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = e.LocalID
                                        WHERE ap.Horario BETWEEN '{0}' AND '{1}' AND (l.Latitude IS NOT NULL AND l.Longitude IS NOT NULL)
                                        ORDER BY e.IR_EventoID", data.Date.ToString("yyyyMMddHHmmss"), data.AddDays(5).Date.ToString("yyyyMMddHHmmss"));

                using (IDataReader dr = oDAL.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                    {
                        int distanciaEvento = IRLib.CEP.CalcularDistancia(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude), Convert.ToDouble(dr["Latitude"]), Convert.ToDouble(dr["Longitude"]));


                        if (retorno.Where(c => c.ID == dr["IR_EventoID"].ToInt32()).Count() == 0)
                        {
                            evento = new EstruturaEvento();

                            evento.ID = dr["IR_EventoID"].ToInt32();
                            evento.Nome = dr["Nome"].ToString();
                            evento.Imagem = dr["Imagem"].ToString();
                            evento.LocalID = dr["IR_LocalID"].ToInt32();
                            evento.Local = dr["Local"].ToString();
                            evento.Cidade = dr["Cidade"].ToString();
                            evento.Estado = dr["Estado"].ToString();
                            evento.Pais = dr["Pais"].ToString();
                            evento.Tipo = dr["TipoID"].ToInt32();
                            evento.SubTipo = dr["SubtipoID"].ToInt32();
                            evento.Distancia = distanciaEvento;

                            retorno.Add(evento);
                        }

                        evento.Apresentacoes.Add(new EstruturaApresentacaoSimples()
                        {
                            EventoID = dr["IR_EventoID"].ToInt32(),
                            IR_ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                            HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM"),
                            Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture),
                        });
                    }
                }

                return retorno.Where(c => c.Distancia <= distancia).OrderBy(c => c.Distancia).ToList();
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

        public List<EstruturaEvento> BuscarEventosCombinados(string HorarioAtual, string Pesquisa, int LocalID, int TipoID, int SubTipoID, int DatasProximas, string Cidade, string Estado, string Latitude, string Longitude, int Distancia, string eventosNaoExibir = "")
        {
            try
            {

                #region Variaveis

                List<EstruturaEvento> retorno = new List<EstruturaEvento>();
                EstruturaEvento evento = new EstruturaEvento();
                string Data = DateTime.Now.AddMonths(DatasProximas).ToString("yyyyMMddhhmmss");
                string _DatasProximas = "";
                string _SubTipoID = "";
                string _Pesquisa = "";
                string _TipoID = "";
                string _DataEvento = "";
                string _Cidade = "";
                string _Estado = "";
                string Cordenada = "";
                string _LocalID = "";


                int numParametros = 0;

                #endregion Variaveis

                #region String Format


                if (SubTipoID > 0)
                {
                    _SubTipoID = string.Format(" AND Evento.SubtipoID = {0}", SubTipoID);
                }
                if (DatasProximas > 0)
                {
                    _DatasProximas = string.Format(" AND Apresentacao.Horario < '{0}'", Data);
                }
                if (!string.IsNullOrEmpty(Pesquisa))
                {
                    _Pesquisa = string.Format(" AND (Local.Nome COLLATE Latin1_General_CI_AI LIKE '%" + Pesquisa
                        + "%' OR Evento.PalavraChave COLLATE Latin1_General_CI_AI LIKE '%" + Pesquisa + "%' OR Evento.Nome COLLATE Latin1_General_CI_AI LIKE '%" + Pesquisa + "%')");
                }
                if (LocalID > 0)
                {
                    _LocalID = string.Format(" AND Local.IR_LocalID = {0}", LocalID);
                }
                if (TipoID > 0)
                {    
                    _TipoID = string.Format(" AND Tipo.IR_TipoID = {0}", TipoID);
                }
                if (!string.IsNullOrEmpty(Cidade))
                {
                    _Cidade = string.Format(" AND Local.Cidade = '{0}'", Cidade);
                }
                if (!string.IsNullOrEmpty(Estado))
                {
                    _Estado = string.Format(" AND Local.Estado = '{0}'", Estado);
                }
                if (!string.IsNullOrEmpty(Latitude) || !string.IsNullOrEmpty(Longitude))
                {
                    Cordenada = string.Format(" AND (local.Latitude IS NOT NULL AND local.Longitude IS NOT NULL)");
                }

                #endregion

                return this.CarregarListaMobile(HorarioAtual, _SubTipoID, _DatasProximas, _Pesquisa, _LocalID, _TipoID, _Cidade, _Estado, Latitude, Longitude, Distancia, Cordenada, eventosNaoExibir);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void VerificarEventosRC(EventoLista lista)
        {
            List<Evento> listaRC = new List<Evento>();

            foreach (string str in ConfigurationManager.AppSettings["EventosRoberto"].Split(','))
            {
                foreach (Evento evento in lista)
                {
                    if (Convert.ToInt32(str) == evento.Id)
                    {
                        listaRC.Add(evento);
                    }
                }

            }

            if (listaRC.Count > 0)
            {
                Evento turneRC = listaRC[0];
                turneRC.Nome = "Turnê Roberto Carlos";
                turneRC.Local = "";
                turneRC.Cidade = "";
                turneRC.Estado = "";

                lista.Add(turneRC);

                foreach (Evento evento in listaRC)
                {
                    lista.Remove(evento);
                }
            }
        }


    }
}