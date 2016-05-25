using CTLib;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace IngressoRapido.Lib
{
    public class CampanhaHit
    {
        private int clienteID;
        private int campanhaTipoID;
        private int pontos;
        private DateTime timestamp;

        public int Pontos
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public int CampanhaTipo
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        DAL DAL = new DAL();


        #region * Methods


        public void GravarHit(int clienteID, int campanhaTipoID)
        {  /*      
        int pontos = 0;
        string status = "";
        
        // Verifica e Retorna os Pontos atuais da promoção
        string strSql = "SELECT Pontos FROM CampanhaTipo WHERE (ID = " + campanhaTipoID + ")";
        SqlDataReader dr = null;
        DataTable dt;
        dt = dbSelect(strSql);

        if (dt.Rows.Count >= 1)
            pontos = (int)dt.Rows[0][0];        

        //insere na Tabela CampanhaHit
        strSql = "INSERT INTO CampanhaHit (ClienteID, CampanhaTipoID, Pontos, DataHora) VALUES (" + clienteID + ", " + campanhaTipoID + ", " + pontos + ", '" + DateTime.Now + "')";
        status = dbInsert(strSql);
        if (status != "")
        {
            // Insere Tabela CampanhaHitLog
            string sessionID = HttpContext.Current.Session.SessionID.ToString();
            strSql = "INSERT INTO CampanhaHitLog (ClienteID, CampanhaTipoID, sessionID, DataHora) VALUES (" + clienteID + ", " + campanhaTipoID + ", '" + sessionID + "', '" + DateTime.Now + "')";
            status = dbInsert(strSql);
        }
        else
        { 
            // Exception
        }
        return status;*/
        }

        /// <summary>
        /// Registrar ou apenas visualização caso já tenha recebido pontuação
        /// </summary>
        public int InserirHit(int clienteID, int campanhaTipoID)
        {
            int status = 0;
            #region Parametros
            string sessionID = HttpContext.Current.Session.SessionID.ToString();

            SqlParameter[] parametros = new SqlParameter[4];

            parametros[0] = new SqlParameter("@ClienteID", SqlDbType.Int);
            parametros[0].Value = clienteID;

            parametros[1] = new SqlParameter("@CampanhaTipoID", SqlDbType.Int);
            parametros[1].Value = campanhaTipoID;

            parametros[2] = new SqlParameter("@Pontos", SqlDbType.Int);
            parametros[2].Value = GetPoints(campanhaTipoID);

            parametros[3] = new SqlParameter("@sessionID", SqlDbType.NVarChar, 200);
            parametros[3].Value = sessionID;

            #endregion

            string[] arraystrSql = new string[2];
            arraystrSql[0] = "INSERT INTO CAMPANHAHIT (ClienteID, CampanhaTipoID, Pontos) VALUES (@ClienteID, @campanhaTipoID, @pontos)";
            arraystrSql[1] = "INSERT INTO CAMPANHAHITLOG (ClienteID, CampanhaTipoID, sessionID) VALUES (@ClienteID, @campanhaTipoID, @sessionID)";

            try
            {
                status = DAL.Execute(arraystrSql, parametros);

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    try
                    {
                        status = DAL.Execute(arraystrSql[1], parametros);
                    }
                    catch (SqlException ex1)
                    {
                        throw ex1;
                    }
                }
            }

            return status;
        }

        private int GetPoints(int campanhaTipoID)
        {
            int pontos = 0;
            string strSql = "SELECT PONTOS FROM CampanhaTipo WHERE (ID = " + campanhaTipoID + ")";

            try
            {
                IDataReader dr = DAL.SelectToIDataReader(strSql);

                pontos = Convert.ToInt32(dr["PONTOS"].ToString());

                // Fecha conexão da classe DataAcess
                DAL.ConnClose();

            }
            catch (SqlException ex)
            {
                DAL.ConnClose();
                throw ex;
            }
            catch (Exception ex)
            {
                //exception geral
                throw ex;
            }
            finally
            {
                DAL.ConnClose();
            }
            return pontos;
        }

        #region temp

        /*
    private string InsereHitPontos(int clienteID, int campanhaTipoID)
    {
        Database db = DatabaseFactory.CreateDatabase();
        
        /*
        int pontos = 0;
        string status;

        Database db = DatabaseFactory.CreateDatabase("strConn");
        string sqlCommand = "SELECT Pontos FROM CampanhaTipo WHERE (ID = " + campanhaTipoID + ")";
        //string sqlCommand = "SELECT COUNT(*) AS counter FROM  CampanhaHit WHERE (ClienteID = " + clienteID + ") AND (CampanhaTipoID = " + campanhaTipoID + ")";        

        DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

        IDataReader dataReader = null;

        try
        {            
            dataReader = db.ExecuteReader(dbCommand);
            
            while (dataReader.Read())
            {
                pontos = (int)dataReader[0];
            }

            sqlCommand = "INSERT INTO CampanhaHit (ClienteID, CampanhaTipoID, Pontos) VALUES (" + clienteID + ", " + campanhaTipoID + ", " + pontos + ")";
            dbCommand = db.GetSqlStringCommand(sqlCommand);

            dataReader = db.ExecuteNonQuery(dbCommand);

            status = dataReader.RecordsAffected.ToString();
        }
        catch (Exception ex)
        {
            status = ex.ToString();
        }
        finally
        {
            if (dataReader != null)
                dataReader.Close();
        }
        
         
        return "ok";
    }             
*/

        #endregion



        #endregion
    }
}

