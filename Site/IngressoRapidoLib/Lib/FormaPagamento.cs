using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using IRCore.Util;

namespace IngressoRapido.Lib
{
    public class FormaPagamento
    {
        public const string FORMATO_PARCELAS = "#x";

        #region Atributos / Propriedades

        private DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        private int parcelas;
        public int Parcelas
        {
            get { return parcelas; }
            set { parcelas = value; }
        }

        public string nome;
        public string Nome
        {
            get { return Nome; }
            set { Nome = value; }
        }

        public enum Bandeiras
        {
            Amex,
            Diners,
            RedecardCredito,
            VisaCredito,
            VisaElectron,
            ItauDebito,
            Aura,
            HiperCard,
            PayPal,
            ValeCultura,
            Elo,
            EloCultura,
            NaoSelecionado,
        }
        #endregion

        #region Construtores

        public FormaPagamento() { }
        public FormaPagamento(int id)
        {
            this.id = id;
        }

        #endregion

        #region Métodos

        public static int GetFormaPagamentoID(string bandeira, int parcelas)
        {
            DAL oDal = new DAL();
            object x = oDal.Scalar("SELECT IR_FormaPagamentoID FROM FormaPagamento (NOLOCK) WHERE (nome = '" + bandeira + "' AND Parcelas = " + parcelas + ")", new System.Data.SqlClient.SqlParameter[] { });
            return x != null ? Convert.ToInt32(x) : 0;
        }

        public bool GetByEventoID(string bandeira, System.Collections.Generic.List<int> lstEventoID)
        {
            System.Text.StringBuilder sbEventoID = new System.Text.StringBuilder();
            bool blnGetByEventoID = false;

            foreach (int EventoID in lstEventoID)
            {
                sbEventoID.Append((sbEventoID.Length > 0 ? ", " : "") + EventoID);
            }
            string strSql = "" +
                "SELECT COUNT(FormaPagamentoEvento.EventoID) AS TotalEventos " +
                "FROM FormaPagamentoEvento (NOLOCK) " +
                "INNER JOIN FormaPagamento (NOLOCK) ON FormaPagamento.IR_FormaPagamentoID = FormaPagamentoEvento.FormaPagamentoID " +
                "WHERE (FormaPagamentoEvento.EventoID IN ( " + sbEventoID.ToString() + ")) " +
                "AND (FormaPagamento.Nome = '" + bandeira + "')";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    // Verifica se conseguiu seleciona a forma de pagamento para todos os eventos reservados
                    blnGetByEventoID = ((int)dr["TotalEventos"] == lstEventoID.Count);
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

            return blnGetByEventoID;
        }

        /// <summary>
        /// Metodo que retorna um inteiro com as formas de pagamento possiveis na tela de Evento
        /// </summary>
        /// <param name="eventoID"></param>
        /// <returns></returns>
        public Dictionary<Bandeiras, int> getByEventoID(int eventoID)
        {
            try
            {
                Dictionary<Bandeiras, int> bandeiras = new Dictionary<Bandeiras, int>();
                string strSQL = @"	SELECT Nome, MAX(Parcelas) AS Parcelas FROM FormaPagamento (NOLOCK) 
									INNER JOIN FormapagamentoEvento (NOLOCK) ON Formapagamentoevento.FormaPagamentoID = FormaPagamento.IR_FormaPagamentoID 
									WHERE FormaPagamentoEvento.EventoID = " + eventoID +
                                    " GROUP BY Nome";

                IDataReader dr = oDAL.SelectToIDataReader(strSQL);

                while (dr.Read())
                {
                    try
                    {
                        bandeiras.Add((Bandeiras)Enum.Parse(typeof(Bandeiras), Convert.ToString(dr["Nome"]), true), Convert.ToInt32(dr["Parcelas"]));
                    }
                    catch (Exception)
                    { }
                }
                oDAL.ConnClose();   // Fecha conexão da classe DataAccess

                return bandeiras;
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

        public List<EstruturaFormasPagamentoMobile> getByEventoIDMobile(int eventoID)
        {
            try
            {
                List<EstruturaFormasPagamentoMobile> bandeiras = new List<EstruturaFormasPagamentoMobile>();

                string strSQL = @"SELECT Nome,MAX(Parcelas) as Parcelas FROM FormaPagamento (NOLOCK) 
								INNER JOIN FormapagamentoEvento (NOLOCK) ON Formapagamentoevento.FormaPagamentoID = FormaPagamento.IR_FormaPagamentoID 
								WHERE FormaPagamentoEvento.EventoID = " + eventoID + @" GROUP BY Nome";

                using (IDataReader dr = oDAL.SelectToIDataReader(strSQL))
                {
                    while (dr.Read())
                    {
                        try
                        {
                            bandeiras.Add(new EstruturaFormasPagamentoMobile()
                            {
                                Bandeira = ((BandeirasMobile)Enum.Parse(typeof(BandeirasMobile), Convert.ToString(dr["Nome"]), true)),
                                Parcelas = dr["Parcelas"].ToInt32()
                            });
                        }
                        catch (Exception)
                        { }
                    }
                }

                return bandeiras;
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

        /// <summary>
        /// Retorna uma lista de Ints que contem o numero da forma de pagamento
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public Dictionary<Bandeiras, int> getBandeiras1x()
        {
            try
            {
                Dictionary<Bandeiras, int> bandeiras = new Dictionary<Bandeiras, int>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT Nome, Parcelas FROM FormaPagamento (NOLOCK) ");
                stbSQL.Append("WHERE Parcelas = 1");

                IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString());

                while (dr.Read())
                {
                    bandeiras.Add((Bandeiras)Enum.Parse(typeof(Bandeiras), Convert.ToString(dr["Nome"]), true), Convert.ToInt32(dr["Parcelas"]));
                }
                oDAL.ConnClose();   // Fecha conexão da classe DataAccess

                return bandeiras;
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
        /// Retorna uma lista de Ints que contem o numero da forma de pagamento
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public List<int> getByEventos(int clienteID, string sessionID)
        {
            try
            {
                List<int> lstID = new List<int>();
                string strSQL = "sp_getFormasPagamento " + clienteID + ", '" + sessionID + "'";

                IDataReader dr = oDAL.SelectToIDataReader(strSQL);

                while (dr.Read())
                {
                    lstID.Add(Convert.ToInt32(dr["FormaPagamentoID"]));
                }
                return lstID;
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

        public int getMinimoByBandeira(FormaPagamento.Bandeiras bandeira, int clienteID, string sessionID)
        {

            try
            {
                int parcelas = 0;
                string strSQL = "sp_getParcelas1 " + clienteID + ", '" + sessionID + "', '" + bandeira + "'";

                IDataReader dr = oDAL.SelectToIDataReader(strSQL);
                if (dr.Read())
                {
                    parcelas = Convert.ToInt32(dr["Parcelas"]);
                }
                return parcelas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public FormaPagamento GetByID(int id)
        {
            string strSql = "SELECT IR_FormaPagamentoID, Nome, Parcelas FROM FormaPagamento (NOLOCK) " +
                            "WHERE (IR_FormaPagamentoID = " + id + ")";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    this.id = (int)dr["IR_FormaPagamentoID"];
                    this.nome = dr["Nome"].ToString();
                    this.parcelas = Convert.ToInt32(dr["Parcelas"]);
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

        public FormaPagamento GetByBandeira(string bandeira)
        {
            string strSql = "SELECT IR_FormaPagamentoID, Nome, Parcelas FROM FormaPagamento (NOLOCK) " +
                            "WHERE (Nome = " + bandeira + ")";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    this.id = (int)dr["IR_FormaPagamentoID"];
                    this.nome = dr["Nome"].ToString();
                    this.parcelas = (int)dr["Parcelas"];
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

        public List<Bandeiras> FormasDePagamentoPrioritarias(int clienteID, string sessionID)
        {
            try
            {
                int Eventos = 0, Cotas = 0, Series = 0;
                List<int> FormasPagtoEvento = new List<int>(), FormasPagtoCotas = new List<int>(), FormasPagtoSerie = new List<int>();

                LogUtil.Debug(string.Format("##FormaPagamento.ObtendoFormasDePagamentoPrioritarias## SESSION {0}, SQL {1}", sessionID, "EXEC FormasDePagamento @ClienteID = " + clienteID + ", @SessionID = '" + sessionID + "'"));

                using (IDataReader dr = oDAL.SelectToIDataReader("EXEC FormasDePagamento @ClienteID = " + clienteID + ", @SessionID = '" + sessionID + "'"))
                {
                    //Qtd Só de Eventos
                    if (dr.Read())
                        Eventos = dr["QuantidadeEventos"].ToInt32();

                    //Qtd só de Cotas, Sendo PA ou Avulso
                    if (dr.NextResult() && dr.Read())
                        Cotas = dr["QuantidadeCotas"].ToInt32();

                    //Qtd só de Séries
                    if (dr.NextResult() && dr.Read())
                        Series = dr["QuantidadeSeries"].ToInt32();


                    //Formas de Pagamento Já Filtradas pelo CanalInternet, ou seja, Não precisa mais Eliminar
                    dr.NextResult();
                    while (dr.Read())
                        FormasPagtoCotas.Add(dr["FormaPagamentoID"].ToInt32());

                    dr.NextResult();
                    while (dr.Read())
                        FormasPagtoEvento.Add(dr["FormaPagamentoID"].ToInt32());

                    dr.NextResult();
                    while (dr.Read())
                        FormasPagtoSerie.Add(dr["FormaPagamentoID"].ToInt32());
                }
                if (Cotas > 0 && FormasPagtoCotas.Count == 0)
                {
                    string strQuery = @"SELECT 
	                                        DISTINCT fpe.FormaPagamentoID  AS FormaPagamentoID
                                        FROM Carrinho c (NOLOCK) 
	                                        INNER JOIN  FormaPagamentoEvento fpe (NOLOCK) ON fpe.EventoID = c.EventoID
	                                        INNER JOIN FormaPagamento fp (NOLOCK) ON fp.IR_FormaPagamentoID = fpe.FormaPagamentoID
                                        WHERE
	                                        c.ClienteID = " + clienteID + @"
	                                        AND c.SessionID = '" + sessionID + @"'
	                                        AND status = 'R'
	                                        AND (c.CotaItemID > 0 OR c.CotaItemIDAPS > 0)
	                                        AND (fp.IR_FormaPagamentoID NOT IN (171) OR fpe.EventoID IN (32908))";
                    using (IDataReader dr = oDAL.SelectToIDataReader(strQuery))
                    {
                        while (dr.Read())
                            FormasPagtoCotas.Add(dr["FormaPagamentoID"].ToInt32());
                    }
                }
                //Só cota
                if (Cotas > 0 && Eventos == 0 && Series == 0)
                    return this.ToBandeira(FormasPagtoCotas);
                //Só eventos
                else if (Eventos > 0 && Cotas == 0 && Series == 0)
                    return this.ToBandeira(FormasPagtoEvento);
                //Só Série
                else if (Series > 0 && Cotas == 0 && Eventos == 0)
                    return this.ToBandeira(FormasPagtoSerie);
                //Os tipos
                else if (Cotas > 0 && Eventos > 0 && Series > 0)
                    return this.ToBandeira((from c in FormasPagtoCotas
                                            join e in FormasPagtoEvento on c equals e
                                            join s in FormasPagtoSerie on e equals s
                                            select c).ToList());
                //Cota e Evento
                else if (Cotas > 0 && Eventos > 0)
                    return this.ToBandeira((from c in FormasPagtoCotas
                                            join e in FormasPagtoEvento on c equals e
                                            select c).ToList());
                //Cota e Serie
                else if (Cotas > 0 && Series > 0)
                    return this.ToBandeira((from c in FormasPagtoCotas
                                            join s in FormasPagtoSerie on c equals s
                                            select c).ToList());
                //Serie e Evento
                else
                    return this.ToBandeira((from s in FormasPagtoSerie
                                            join e in FormasPagtoEvento on s equals e
                                            select s).ToList());

            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##FormaPagamento.FormasDePagamentoPrioritarias.EXCEPTION## SESSION {0}, MSG {1}", sessionID, ex.Message), ex);
                return new List<Bandeiras>();
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int ParcelaMaxima(FormaPagamento.Bandeiras bandeira, int clienteID, string sessionID)
        {
            try
            {
                int Eventos = 0, Cotas = 0, Series = 0;
                int ParcelasEvento = 0, ParcelasCotas = 0, ParcelasSeries = 0;

                using (IDataReader dr = oDAL.SelectToIDataReader("EXEC FormasDePagamentoParcela @ClienteID = " + clienteID + ", @SessionID = '" + sessionID + "', @Bandeira = '" + bandeira + "'"))
                {
                    //Qtd Só de Eventos
                    if (dr.Read())
                        Eventos = dr["QuantidadeEventos"].ToInt32();

                    //Qtd só de Cotas, Sendo PA ou Avulso
                    if (dr.NextResult() && dr.Read())
                        Cotas = dr["QuantidadeCotas"].ToInt32();

                    //Qtd só de Séries
                    if (dr.NextResult() && dr.Read())
                        Series = dr["QuantidadeSeries"].ToInt32();

                    if (dr.NextResult() && dr.Read())
                        ParcelasCotas = dr["Parcelas"].ToInt32();

                    if (dr.NextResult() && dr.Read())
                        ParcelasEvento = dr["Parcelas"].ToInt32();

                    if (dr.NextResult() && dr.Read())
                        ParcelasSeries = dr["Parcelas"].ToInt32();
                }

                if (Cotas > 0 && ParcelasCotas == 0)
                {
                    string strQuery = @"SELECT 
	                                        IsNull(MAX(Parcelas), 0) AS Parcelas
                                        FROM
	                                        (SELECT 
		                                        Parcelas
	                                        FROM 
		                                        Carrinho c (NOLOCK) 
		                                        INNER JOIN  FormaPagamentoEvento fpe (NOLOCK) ON fpe.EventoID = c.EventoID
		                                        INNER JOIN FormaPagamento fp (NOLOCK) ON fp.IR_FormaPagamentoID = fpe.FormaPagamentoID
	                                        WHERE 
		                                        c.ClienteID = " + clienteID + @"
	                                            AND c.SessionID = '" + sessionID + @"'
	                                            AND status = 'R'
	                                            AND (c.CotaItemID > 0 OR c.CotaItemIDAPS > 0)
		                                        AND fp.Nome = '" + bandeira + @"'
	                                        GROUP BY 
		                                        Parcelas
	                                        HAVING COUNT(DISTINCT c.CotaItemID) = " + Cotas + @")
	                                        AS TB";
                    using (IDataReader dr = oDAL.SelectToIDataReader(strQuery))
                    {
                        if (dr.Read())
                            ParcelasCotas = dr["Parcelas"].ToInt32();
                    }
                }

                //Só cota
                if (Cotas > 0 && Eventos == 0 && Series == 0)
                    return ParcelasCotas;
                //Só eventos
                else if (Eventos > 0 && Cotas == 0 && Series == 0)
                    return ParcelasEvento;
                //Só Série
                else if (Series > 0 && Cotas == 0 && Eventos == 0)
                    return ParcelasSeries;
                //Os tipos
                else if (Cotas > 0 && Eventos > 0 && Series > 0)
                    return
                       Math.Min(Math.Min(ParcelasCotas, ParcelasEvento), ParcelasSeries);
                //Cota e Evento
                else if (Cotas > 0 && Eventos > 0)
                    return
                        Math.Min(ParcelasCotas, ParcelasEvento);
                //Cota e Serie
                else if (Cotas > 0 && Series > 0)
                    return
                        Math.Min(ParcelasCotas, ParcelasSeries);
                //Serie e Evento
                else
                    return
                        Math.Min(ParcelasSeries, ParcelasEvento);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        //Transforma a lista de Formas de Pagamento em bandeiras
        private List<Bandeiras> ToBandeira(List<int> FormasPagto)
        {
            List<Bandeiras> bandeiras = new List<Bandeiras>();


            foreach (int PagamentoID in FormasPagto)
            {
                switch (PagamentoID)
                {
                    #region AMEX
                    case 4:
                    case 64:
                    case 65:
                    case 70:
                    case 71:
                    case 72:
                        if (bandeiras.Contains(Bandeiras.Amex))
                            continue;
                        bandeiras.Add(Bandeiras.Amex);
                        break;
                    #endregion

                    #region VISA
                    case 2:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                        if (bandeiras.Contains(Bandeiras.VisaCredito))
                            continue;

                        bandeiras.Add(Bandeiras.VisaCredito);

                        break;
                    #endregion

                    #region MasterCard e Diners
                    case 3:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 42:
                    case 43:
                    case 44:
                    case 45:
                        if (bandeiras.Contains(Bandeiras.RedecardCredito))
                            continue;
                        bandeiras.Add(Bandeiras.RedecardCredito);
                        break;
                    //DINERS OK

                    case 112:
                    case 113:
                    case 114:
                        if (bandeiras.Contains(Bandeiras.Diners))
                            continue;
                        bandeiras.Add(Bandeiras.Diners);
                        break;
                    #endregion

                    #region ITAU

                    case 140:
                        if (bandeiras.Contains(Bandeiras.ItauDebito))
                            continue;
                        bandeiras.Add(Bandeiras.ItauDebito);
                        break;

                    #endregion

                    #region AURA
                    case 110:
                        if (bandeiras.Contains(Bandeiras.Aura))
                            continue;
                        bandeiras.Add(Bandeiras.Aura);
                        break;
                    #endregion

                    #region HIPER
                    case 5:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                        if (bandeiras.Contains(Bandeiras.HiperCard))
                            continue;
                        bandeiras.Add(Bandeiras.HiperCard);
                        break;
                    #endregion

                    #region PayPal
                    case 148:
                    case 150:
                        if (bandeiras.Contains(Bandeiras.PayPal))
                            continue;
                        bandeiras.Add(Bandeiras.PayPal);
                        break;
                    #endregion

                    #region ValeCultura
                    case 160:
                        if (bandeiras.Contains(Bandeiras.ValeCultura))
                            continue;
                        bandeiras.Add(Bandeiras.ValeCultura);
                        break;
                    #endregion

                    #region ELO
                    case 161:
                    case 162:
                    case 163:
                    case 164:
                    case 165:
                    case 166:
                    case 167:
                    case 168:
                    case 169:
                    case 170:
                        if (bandeiras.Contains(Bandeiras.Elo))
                            continue;
                        bandeiras.Add(Bandeiras.Elo);
                        break;
                    #endregion

                    #region EloCultura
                    case 171:
                        if (bandeiras.Contains(Bandeiras.EloCultura))
                            continue;
                        bandeiras.Add(Bandeiras.EloCultura);
                        break;
                    #endregion

                    #region NãoSelecionado

                    case 99:
                        if (bandeiras.Contains(Bandeiras.NaoSelecionado))
                            continue;
                        bandeiras.Add(Bandeiras.NaoSelecionado);
                        break;
                    #endregion
                }
            }
            return bandeiras;
        }

        #endregion

    }
}
