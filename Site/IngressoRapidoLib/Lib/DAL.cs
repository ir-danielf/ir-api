
namespace IngressoRapido.Lib
{
    //public class DAL
    //{
    //    private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["strConn"].ConnectionString);

    //    public SqlConnection Conn
    //    {
    //        get { return conn; }
    //        set { conn = value; }
    //    }

    //    public void BulkInsert(DataTable table, string tableName, bool eraseData)
    //    {
    //        try
    //        {
    //            if (eraseData) this.Execute(string.Format("TRUNCATE TABLE {0}", tableName));

    //            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)this.Conn, System.Data.SqlClient.SqlBulkCopyOptions.TableLock | System.Data.SqlClient.SqlBulkCopyOptions.FireTriggers | System.Data.SqlClient.SqlBulkCopyOptions.UseInternalTransaction, null);
    //            bulkCopy.DestinationTableName = tableName;
    //            bulkCopy.WriteToServer(table);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new ApplicationException("Falha ao gerar registros de exclusão na tabela " + tableName + ": " + ex.Message);
    //        }
    //    }

    //    private SqlCommand dbCommand;

    //    public void ConnOpen()
    //    {
    //        if (conn.State != ConnectionState.Open)
    //            conn.Open();
    //    }

    //    public void ConnClose()
    //    {
    //        conn.Close();
    //    }


    //    /// <summary>
    //    /// Retorna um IDataReader a partir de uma SELECT
    //    /// </summary>
    //    public IDataReader SelectToIDataReader(string strSql)
    //    {
    //        SqlCommand cmd = new SqlCommand(strSql, conn);

    //        try
    //        {
    //            ConnOpen();
    //            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //        }
    //        catch (Exception ex)
    //        {
    //            ConnClose();
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public IDataReader SelectToIDataReader(string strSql, SqlParameter[] Parametros)
    //    {
    //        SqlCommand cmd = new SqlCommand(strSql, conn);
    //        try
    //        {
    //            ConnOpen();
    //            cmd.Parameters.AddRange(Parametros);
    //            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //        }
    //        catch (Exception ex)
    //        {
    //            ConnClose();
    //            throw ex;
    //        }
    //    }

    //    public IDataReader SelectToIDataReader(string strSql, CommandType commandType)
    //    {
    //        SqlCommand cmd = new SqlCommand(strSql, conn);

    //        try
    //        {
    //            ConnOpen();
    //            cmd.CommandType = CommandType.Text;
    //            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //        }
    //        catch (Exception ex)
    //        {
    //            ConnClose();
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public DataTable SelectToDataTable(string strSql)
    //    {
    //        DataTable dt = new DataTable();

    //        try
    //        {
    //            ConnOpen();
    //            SqlDataAdapter da = new SqlDataAdapter(strSql, conn);
    //            da.Fill(dt);

    //        }
    //        catch (SqlException ex)
    //        {
    //            //
    //            ConnClose();
    //            throw ex;
    //        }
    //        finally
    //        {
    //            ConnClose();
    //        }

    //        return dt;

    //    }
    //    public DataTable SelectToDataTable(string strSql, SqlParameter[] parametros)
    //    {
    //        DataTable dt = new DataTable();
    //        SqlCommand cmd = new SqlCommand(strSql, conn);
    //        try
    //        {
    //            ConnOpen();
    //            cmd.Parameters.AddRange(parametros);
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);
    //            da.Fill(dt);

    //        }
    //        catch (SqlException ex)
    //        {
    //            //
    //            ConnClose();
    //            throw ex;
    //        }
    //        finally
    //        {
    //            ConnClose();
    //        }

    //        return dt;

    //    }
    //    public DataTable SelectToDataTable(string strSql, SqlParameter parametro)
    //    {
    //        DataTable dt = new DataTable();
    //        SqlCommand cmd = new SqlCommand(strSql, conn);
    //        try
    //        {
    //            ConnOpen();
    //            cmd.Parameters.Add(parametro);
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);
    //            da.Fill(dt);

    //        }
    //        catch (SqlException ex)
    //        {
    //            //
    //            ConnClose();
    //            throw ex;
    //        }
    //        finally
    //        {
    //            ConnClose();
    //        }

    //        return dt;

    //    }
    //    public int Execute(string[] strSql, SqlParameter[] parametros)
    //    {
    //        SqlTransaction transacao = null;
    //        int status = 0;

    //        try
    //        {
    //            ConnOpen();
    //            //transacao = conn.BeginTransaction();

    //            if (this.dbCommand == null)
    //            {
    //                dbCommand = new SqlCommand();
    //                dbCommand.Connection = conn;
    //            }

    //            if (parametros != null && parametros.Length > 0)
    //            {
    //                dbCommand.Parameters.Clear();
    //                dbCommand.Parameters.AddRange(parametros);
    //            }

    //            //dbCommand.Transaction = transacao;
    //            dbCommand.CommandTimeout = int.MaxValue;

    //            foreach (string str in strSql)
    //            {
    //                if (str != string.Empty)
    //                {
    //                    try
    //                    {
    //                        this.dbCommand.CommandText = str;
    //                        status = dbCommand.ExecuteNonQuery();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine("Erro " + ex.Message);
    //                    }
    //                }
    //                else
    //                    status = 1;
    //            }
    //            return status;
    //        }
    //        catch (Exception ex)
    //        {
    //            //transacao.Rollback();
    //            throw ex;
    //        }
    //        finally
    //        {
    //            //ConnClose();
    //        }
    //    }

    //    public int Execute(string[] strSql, SqlParameter[] parametros, bool transaction)
    //    {
    //        SqlTransaction transacao = null;
    //        int status = 0;

    //        if (transaction)
    //        {
    //            try
    //            {
    //                ConnOpen();
    //                transacao = conn.BeginTransaction();

    //                if (this.dbCommand == null)
    //                {
    //                    dbCommand = new SqlCommand();
    //                    dbCommand.Connection = conn;
    //                }

    //                if (parametros != null && parametros.Length > 0)
    //                {
    //                    dbCommand.Parameters.Clear();
    //                    dbCommand.Parameters.AddRange(parametros);
    //                }

    //                dbCommand.Transaction = transacao;
    //                dbCommand.CommandTimeout = int.MaxValue;


    //                for (int i = 0; i <= strSql.Length - 1; i++)
    //                {
    //                    this.dbCommand.CommandText = strSql[i];
    //                    status = dbCommand.ExecuteNonQuery();
    //                }

    //                transacao.Commit();
    //                //ConnClose();
    //                return status;
    //            }

    //            catch (SqlException ex)
    //            {
    //                transacao.Rollback();
    //                throw ex;
    //            }
    //            finally
    //            {
    //                //ConnClose();
    //            }
    //        }
    //        else
    //        {
    //            try
    //            {
    //                ConnOpen();
    //                //transacao = conn.BeginTransaction();

    //                if (this.dbCommand == null)
    //                {
    //                    dbCommand = new SqlCommand();
    //                    dbCommand.Connection = conn;
    //                }

    //                if (parametros != null && parametros.Length > 0)
    //                {
    //                    dbCommand.Parameters.Clear();
    //                    dbCommand.Parameters.AddRange(parametros);
    //                }

    //                //dbCommand.Transaction = transacao;
    //                dbCommand.CommandTimeout = int.MaxValue;


    //                for (int i = 0; i <= strSql.Length - 1; i++)
    //                {
    //                    this.dbCommand.CommandText = strSql[i];
    //                    status = dbCommand.ExecuteNonQuery();
    //                }

    //                //transacao.Commit();
    //                ConnClose();
    //                return status;
    //            }

    //            catch (SqlException ex)
    //            {
    //                //transacao.Rollback();
    //                throw ex;
    //            }
    //            finally
    //            {
    //                ConnClose();
    //            }
    //        }
    //    }


    //    public int Execute(string strSql)
    //    {
    //        return this.Execute(new string[] { strSql }, null);
    //    }

    //    public int Execute(string strSql, SqlParameter[] parametros)
    //    {
    //        return this.Execute(new string[] { strSql }, parametros);
    //    }

    //    public object Scalar(string strSql, SqlParameter[] parametros)
    //    {
    //        try
    //        {
    //            ConnOpen();

    //            if (this.dbCommand == null)
    //            {
    //                dbCommand = new SqlCommand();
    //                dbCommand.Connection = conn;
    //            }

    //            if (parametros != null && parametros.Length > 0)
    //            {
    //                dbCommand.Parameters.Clear();
    //                dbCommand.Parameters.AddRange(parametros);
    //            }

    //            this.dbCommand.CommandText = strSql;

    //            object ret = dbCommand.ExecuteScalar();
    //            return ret;
    //        }
    //        catch (Exception ex)
    //        {
    //            ConnClose();
    //            throw new Exception(ex.Message);
    //        }
    //        finally
    //        {
    //            ConnClose();
    //        }
    //    }

    //    public object Scalar(string strSql)//, CommandType commandType)
    //    {
    //        SqlCommand cmd = new SqlCommand(strSql, conn);
    //        try
    //        {
    //            ConnOpen();
    //            //cmd.CommandType = commandType;
    //            return cmd.ExecuteScalar();
    //        }
    //        catch (Exception ex)
    //        {
    //            ConnClose();
    //            throw new Exception(ex.Message);
    //        }
    //        finally
    //        {
    //            conn.Close();
    //        }
    //    }

    //    public bool HasRows(string strSql)
    //    {
    //        SqlCommand cmd = new SqlCommand(strSql, conn);

    //        try
    //        {
    //            ConnOpen();

    //            if (cmd.ExecuteScalar() == null)
    //            {
    //                return false;
    //            }
    //        }
    //        catch (SqlException ex)
    //        {
    //            //
    //            ConnClose();
    //            return false;
    //            //throw ex;
    //        }
    //        finally
    //        {
    //            //
    //            //ConnClose();
    //        }
    //        return true;
    //    }


        
    //}
}