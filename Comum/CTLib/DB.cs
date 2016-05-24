
#region "Instruções"

/*
Instruções:
 1) Acrescentar o trecho abaixo no arquivo Web.Config, logo após a tab <configuration>.
 2) Substituir o string de conexão Default pelo desejado.
 3) Acrescentar conexões a outros bancos de dados, se necessário.
*/

/*
<appSettings>
	<appSettings>
		<add key="DefaultType" value="SQL"></add>
		<add key="DefaultCnnStr" value="Integrated Security=true;server=(local);database=Testes"></add>
	</appSettings>
</appSettings>
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading;
using IRCore.Util;

namespace CTLib
{
    public class DataReaderCount
    {
        public IDataReader dataReader;

        public int rowCount;

        public DataReaderCount(IDataReader _dataReader, int _rowCount)
        {
            this.dataReader = _dataReader;
            this.rowCount = _rowCount;
        }
    }

    public class BD : MarshalByRefObject, IDisposable
    {

        private bool apenasUmBancoDados = true;

        private string conexao = "";
        private string nome;
        private string tipo = "SQL";					// Tipo de conexão: SQL, OLEDB ou ODBC
        private string comando;				// Texto do comando a ser executado
        private SqlConnection cnn;			// Conexão com o banco de dados
        private SqlConnection cnnDS;		// Conexão com o banco de dados
        private SqlCommand comm;
        private SqlTransaction trans;
        private bool cnnOpenDS = false;	// Flag que indica se já está conectado
        private IDataReader consulta;       // Resultado da consulta
        private bool emTransacao = false;
        private bool criarTransacao = false;
        private int rowCount = 0;
        private const string CINTER_CULTURE = "pt-BR";

        public bool ConsultaAberta
        {
            get
            {
                return consulta == null || !consulta.IsClosed;
            }
        }

        #region "Propriedades"

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        /// <summary>
        /// Se sim, nao procura no arquivo config. Se nao, procura pelo nome do banco de dados.
        /// </summary>
        public bool ApenasUmBancoDados
        {
            get { return apenasUmBancoDados; }
            set { apenasUmBancoDados = value; }
        }

        public string Conexao
        {
            get { return conexao; }
            set { conexao = value; }
        }

        public string Nome
        {
            get { return nome; }
            set
            {
                try
                {
                    //					// Antes de trocar o banco de dados, verifica se tem uma conexão aberta
                    Fechar();
                    //CnnCloseDS();
                    // Le o tipo do banco de dados e string de conexão do arquivo Web.config

                    // Se não for passado o nome de um banco de dados, assume os valores default da classe
                    if (!apenasUmBancoDados)
                    {
                        if (value.Trim().Length != 0)
                        {
                            tipo = ConfigurationManager.AppSettings[nome + "Type"];
                            conexao = ConfigurationManager.AppSettings[nome + "CnnStr"];
                        }
                        else
                        {
                            nome = value;
                        }
                    }

                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                tipo = value;
            }
        }

        public string Comando
        {
            get
            {
                return comando;
            }
            set
            {
                try
                {
                    FecharConsulta();
                    //QueryCloseDS();
                    comando = value;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // Devolve a conexão com o banco de dados
        public IDbConnection Cnn
        {
            get
            {


                // Se a conexão estiver fechada, abre-a
                if (cnn == null || cnn.State == ConnectionState.Closed)
                {
                    cnn = new SqlConnection(conexao);
                    cnn.Open();
                }

                return cnn;
            }
            set
            {
                cnn = (SqlConnection)value;
                if (cnn.State == ConnectionState.Closed)
                    cnn.Open();
            }
        }

        // Devolve a conexão com o banco de dados
        public SqlConnection CnnDS
        {
            get
            {
                if (!cnnOpenDS)
                {
                    cnnDS = new SqlConnection(conexao);
                    cnnDS.Open();

                    cnnOpenDS = true;
                }
                return cnnDS;
            }
        }

        #endregion

        #region "Construtores"

        public BD()
        {
            Nome = "";
            this.conexao = Servidor.ConexaoBD;
        }

        public BD(string nome, string conexao)
        {
            Nome = nome;
            this.conexao = conexao;
        }

        public BD(string conexao)
        {
            Nome = "";
            this.conexao = conexao;
        }

        #endregion

        #region "Metodos"

        public void IniciarTransacao()
        {
            criarTransacao = true;
            emTransacao = true;
        }

        /// <summary>
        /// Método que define que está em transação mas não abre uma nova
        /// </summary>
        public void DefinnirEmTransacao(bool emTransacao, DbTransaction trans = null)
        {
            criarTransacao = false;
            this.emTransacao = emTransacao;
            this.trans = emTransacao ? (SqlTransaction)trans : null;
        }

        public void FinalizarTransacao()
        {
            try
            {
                FecharConsulta();
                emTransacao = false;
                if (trans != null)
                    trans.Commit();
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##DB.FinalizarTransacao.EXCEPTION## MSG {0}", ex.Message), ex);

                throw ex;
            }
        }

        public void DesfazerTransacao()
        {
            try
            {
                if (emTransacao && trans != null)
                    trans.Rollback();

                emTransacao = false;
            }
            catch
            {
            }

        }

        public ArrayList ConsultaDR(string consulta)
        {
            try
            {
                SqlDataReader dr = null;
                //comm = new SqlCommand(comando, (SqlConnection)this.cnn);
                SqlCommand sqlcm = new SqlCommand(consulta, (SqlConnection)this.Cnn);
                sqlcm.CommandTimeout = 480;
                if (criarTransacao)
                {
                    trans = cnn.BeginTransaction(IsolationLevel.RepeatableRead);
                    criarTransacao = false;
                }
                if (emTransacao)
                    comm.Transaction = trans;
                dr = sqlcm.ExecuteReader();
                ArrayList gruposCadeira = new ArrayList();
                while (dr.Read())
                {
                    gruposCadeira.Add(dr["Grupo"]);
                }
                return gruposCadeira;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.FecharConsulta();
                this.Fechar();

            }
        }

        // Executa uma SELECT e devolve o DataReader correspondente
        public IDataReader Consulta(List<SqlParameter> parameters = null)
        {
            // Se a consulta já estiver aberta, não deve executá-la novamente
            if (consulta == null || consulta.IsClosed)
            {

                comm = new SqlCommand(comando, (SqlConnection)this.Cnn);
                comm.CommandTimeout = 480;

                if (parameters != null)
                    ((SqlCommand)comm).Parameters.AddRange(parameters.ToArray());

                if (criarTransacao)
                {
                    trans = cnn.BeginTransaction(IsolationLevel.RepeatableRead);
                    criarTransacao = false;
                }
                if (emTransacao)
                    comm.Transaction = trans;
                consulta = comm.ExecuteReader();

            }
            return consulta;

        }

        /// <summary>
        /// Executa a query no banco de dados e retorna um DataReader dos registros.
        /// </summary>
        /// <param name="comando">Comando SQL</param>
        /// <returns></returns>
        public IDataReader Consulta(string comando, List<SqlParameter> parameters = null)
        {
            try
            {
                // Se mudou o comando, fecha a consulta
                if (comando == null)
                    return null;

                if (comando != this.comando)
                {
                    FecharConsulta();
                    this.comando = comando;
                }
                return Consulta(parameters);
            }
                catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Executa uma SELECT e devolve o DataReader correspondente + a quantidade de linhas afetadas
        public DataReaderCount ConsultaCount(List<SqlParameter> parameters = null)
        {
            // Se a consulta já estiver aberta, não deve executá-la novamente
            if (consulta == null || consulta.IsClosed)
            {

                comm = new SqlCommand(comando, (SqlConnection)this.Cnn) { CommandTimeout = 480 };

                if (parameters != null)
                    comm.Parameters.AddRange(parameters.ToArray());

                if (criarTransacao)
                {
                    trans = cnn.BeginTransaction(IsolationLevel.RepeatableRead);
                    criarTransacao = false;
                }
                if (emTransacao)
                    comm.Transaction = trans;

                using (var reader = comm.ExecuteReader())
                {
                    consulta = reader;
                }

                comm.CommandText = "SELECT @@rowcount";
                rowCount = (int)comm.ExecuteScalar();
            }

            var retorno = new DataReaderCount(consulta, rowCount);
            return retorno;

        }

        /// <summary>
        /// Executa a query no banco de dados e retorna um DataReader dos registros.
        /// </summary>
        /// <param name="_comando">Comando SQL</param>
        /// <param name="parameters">Parâmetros</param>
        /// <returns></returns>
        public DataReaderCount ConsultaCount(string _comando, List<SqlParameter> parameters = null)
        {
            try
            {
                // Se mudou o comando, fecha a consulta
                if (_comando == null)
                    return null;

                if (_comando != this.comando)
                {
                    FecharConsulta();
                    this.comando = _comando;
                }

                return ConsultaCount(parameters);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa a query no banco de dados e retorna um DataReader dos registros.
        /// </summary>
        /// <param name="comando">Comando SQL</param>
        /// <returns></returns>
        public IDataReader Consulta(StringBuilder comando)
        {
            try
            {
                // Se mudou o comando, fecha a consulta
                // MSB: Tentar executar sem os ToString()
                if (comando == null)
                    return null;

                if (comando.ToString() != this.comando)
                {
                    FecharConsulta();
                    this.comando = comando.ToString();
                }
                return Consulta();

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa uma SELECT e devolve o DataTable correspondente
        /// </summary>
        public DataTable QueryToTable(string comando, List<SqlParameter> parameter = null)
        {

            return QueryToDataSet(comando, parameter).Tables[0];

            //return tabela;
        }

        /// <summary>
        /// Executa uma SELECT e devolve o DataSet correspondente
        /// </summary>
        public DataSet QueryToDataSet(string comando, List<SqlParameter> parameters = null)
        {
            DataSet dsResult = new DataSet();

            SqlDataAdapter sa = new SqlDataAdapter(comando, conexao);

            if (parameters != null)
                sa.SelectCommand.Parameters.AddRange(parameters.ToArray());


                 sa.Fill(dsResult);
            return dsResult;
        }

        public object ConsultaValor(params SqlParameter[] parametros)
        {

            object result = null;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            FecharConsulta();
            //QueryCloseDS();


            comm = new SqlCommand(comando, (SqlConnection)Cnn);
            comm.CommandTimeout = 480;
            comm.Parameters.Clear();
            if (parametros != null && parametros.Length > 0)
                comm.Parameters.AddRange(parametros);
            if (criarTransacao)
            {
                trans = cnn.BeginTransaction(IsolationLevel.RepeatableRead);
                criarTransacao = false;
            }
            if (emTransacao)
                comm.Transaction = trans;
            result = comm.ExecuteScalar();


            if (result == DBNull.Value)
                result = null;

            return result;
        }

        public object ConsultaValor(string comando, params SqlParameter[] parametros)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);

            this.comando = comando;
            return ConsultaValor(parametros);
        }

        public object ConsultaValor(StringBuilder comando)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);

            this.comando = comando.ToString();
            return ConsultaValor();
        }

        // Executa um comando que não devolve registros (insert, update, etc)
        // Devolve o número de registros afetados
        public int Executar(SqlParameter[] parametros = null)
        {


            // Fecha consulta pendente, se houver
            FecharConsulta();


            comm = new SqlCommand(comando, (SqlConnection)this.Cnn);
            comm.CommandTimeout = 480;

            comm.Parameters.Clear();

            if (parametros != null && parametros.Length > 0)
                comm.Parameters.AddRange(parametros);
            if (criarTransacao)
            {
                trans = cnn.BeginTransaction(IsolationLevel.RepeatableRead);
                criarTransacao = false;
            }
            if (emTransacao)
                comm.Transaction = trans;
            return comm.ExecuteNonQuery();


        }

        public int ExecutarComParametros(string comando, params SqlParameter[] parametros)
        {
            this.comando = comando;
            return Executar(parametros);
        }

        public int Executar(string comando, List<SqlParameter> parametros = null)
        {
            this.comando = comando;
            if (parametros == null)
                return Executar();
            else
                return Executar(parametros.ToArray());

        }

        public int ExecutarScalar(string comando, List<SqlParameter> parametros = null)
        {


            // Fecha consulta pendente, se houver
            FecharConsulta();


            comm = new SqlCommand(comando, (SqlConnection)this.Cnn);
            comm.CommandTimeout = 480;

            comm.Parameters.Clear();

            if (parametros != null && parametros.Count > 0)
                comm.Parameters.AddRange(parametros.ToArray());
            if (criarTransacao)
            {
                trans = cnn.BeginTransaction(IsolationLevel.RepeatableRead);
                criarTransacao = false;
            }
            if (emTransacao)
                comm.Transaction = trans;


            int id = 0;
            try
            {
                id = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch { }

            return id;

        }

        public int Executar(StringBuilder comando)
        {
            this.comando = comando.ToString();
            return Executar();
        }

        // Executa uma proc que devolve um ID (int)
        public int ExecutarStoredProcedureComRetorno(SqlParameter[] parameters, string outputName)
        {
            try
            {
                // Fecha consulta pendente, se houver
                FecharConsulta();

                comm = new SqlCommand(comando, (SqlConnection)this.Cnn) { CommandType = CommandType.StoredProcedure };

                SqlParameter returnValue = new SqlParameter(outputName, SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                comm.Parameters.Add(returnValue);

                foreach (var param in parameters)
                    comm.Parameters.Add(param);

                comm.ExecuteNonQuery();

                return Convert.ToInt32(returnValue.Value);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ExecutarStoredProcedureComRetorno(string comando, SqlParameter[] parameters, string outputName)
        {
            try
            {
                this.comando = comando;
                return ExecutarStoredProcedureComRetorno(parameters, outputName);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Executa um comando que não devolve registros (insert, update, etc)
        // Devolve o número de registros afetados
        public int ExecutarStoredProcedure()
        {

            // Fecha consulta pendente, se houver
            FecharConsulta();

            comm = new SqlCommand(comando, (SqlConnection)this.Cnn);
            comm.CommandType = CommandType.StoredProcedure;
            return comm.ExecuteNonQuery();


        }

        public int ExecutarStoredProcedure(string comando)
        {

            this.comando = comando;
            return ExecutarStoredProcedure();

        }

        /// <summary>
        /// Atenção !!! O Create Table só aceita UMA coluna (ID INT)!!!!
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tableName"></param>
        /// <param name="eraseData"></param>
        /// <param name="createTable"></param>
        public void BulkInsert(DataTable table, string tableName, bool eraseData, bool createTable)
        {
            try
            {
                if (createTable)
                    this.Executar(string.Format("CREATE TABLE {0} ( ID INT )", tableName));
                else if (eraseData)
                    this.Executar(string.Format("TRUNCATE TABLE {0}", tableName));
                SqlBulkCopy bulkCopy;
                if (emTransacao)
                    bulkCopy = new SqlBulkCopy((SqlConnection)this.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, (SqlTransaction)this.trans);
                else
                    bulkCopy = new SqlBulkCopy((SqlConnection)this.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(table);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao gerar registros de exclusão na tabela " + tableName + ": " + ex.Message);
            }
        }

        public void BulkInsert(System.Collections.Generic.List<int> ids, string tableName, bool eraseData, bool createTable)
        {
            try
            {

                DataTable dtt = new DataTable();
                dtt.Columns.Add("ID", typeof(int));

                DataRow dtr;
                foreach (int id in ids)
                {
                    dtr = dtt.NewRow();
                    dtr["ID"] = id;
                    dtt.Rows.Add(dtr);
                }
                this.BulkInsert(dtt, tableName, eraseData, createTable);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao gerar registros de exclusão na tabela " + tableName + ": " + ex.Message);
            }
        }

        public void BulkInsert(DataTable table, string tableName, bool createTable, string columnName, string columnType)
        {
            try
            {
                if (createTable)
                    this.Executar(string.Format("CREATE TABLE {0} ( {1} {2} )", tableName, columnName, columnType));

                SqlBulkCopy bulkCopy;
                if (emTransacao)
                    bulkCopy = new SqlBulkCopy((SqlConnection)this.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, (SqlTransaction)this.trans);
                else
                    bulkCopy = new SqlBulkCopy((SqlConnection)this.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(table);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao gerar registros de exclusão na tabela " + tableName + ": " + ex.Message);
            }
        }

        public void BulkInsert(DataTable table, string tableName, bool createTable, string columnName, string columnType, string collation)
        {
            try
            {
                if (createTable)
                    this.Executar(string.Format("CREATE TABLE {0} ( {1} {2} COLLATE {3})", tableName, columnName, columnType, collation));

                SqlBulkCopy bulkCopy;
                if (emTransacao)
                    bulkCopy = new SqlBulkCopy((SqlConnection)this.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, (SqlTransaction)this.trans);
                else
                    bulkCopy = new SqlBulkCopy((SqlConnection)this.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(table);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao gerar registros de exclusão na tabela " + tableName + ": " + ex.Message);
            }
        }

        public void Fechar()
        {

            try
            {
                criarTransacao = emTransacao = false;
                if (cnn != null && cnn.State == ConnectionState.Open)
                {

                    if (consulta != null && !consulta.IsClosed)
                    {
                        consulta.Close();
                        consulta.Dispose();
                    }

                    cnn.Close();
                    cnn.Dispose();
                }

            }
            catch (Exception ex) { }

        }

        public void FecharConsulta()
        {
            if (consulta != null && !consulta.IsClosed)
            {
                consulta.Close();
                consulta = null;
            }
        }

        public int LerInt(int field)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            int resp = (Consulta().IsDBNull(field)) ? 0 : Convert.ToInt32(Consulta()[field]);
            return resp;
        }

        public int LerInt(string field)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            int resp = (Consulta()[field].Equals(DBNull.Value)) ? 0 : Convert.ToInt32(Consulta()[field]);
            return resp;
        }

        public long LerLong(int field)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            long resp = (Consulta().IsDBNull(field)) ? 0L : Convert.ToInt64(Consulta()[field]);
            return resp;
        }

        public long LerLong(string field)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            long resp = (Consulta()[field].Equals(DBNull.Value)) ? 0L : Convert.ToInt64(Consulta()[field]);
            return resp;
        }

        public string LerString(int field)
        {
            string resp = (Consulta().IsDBNull(field)) ? null : Consulta().GetString(field);
            return resp;
        }

        public string LerString(string field)
        {
            object obj = Consulta()[field];
            if (obj != null)
                return Convert.ToString(obj);
            else
                throw new Exception("campo " + field + " não existe.");
        }

        public T LerEnum<T>(string field)
        {
            object obj = Consulta()[field];
            if (obj != null)
                return (T)Enum.Parse(typeof(T), Convert.ToString(obj), true);
            else
                throw new Exception("campo " + field + " não existe.");
        }

        public string LerStringFormatoDataHora(string field)
        {
            string datahora;
            object obj = Consulta()[field];
            if (obj != null)
                datahora = Convert.ToString(obj);
            else
                throw new Exception("campo " + field + " não existe.");

            if (datahora.Trim() == "")
                return "";

            else if (datahora.Length >= 12)
            {
                string ano = datahora.Substring(0, 4);
                string mes = datahora.Substring(4, 2);
                string dia = datahora.Substring(6, 2);
                string hora = datahora.Substring(8, 2);
                string minuto = datahora.Substring(10, 2);

                string resp = dia + "/" + mes + "/" + ano + " " + hora + ":" + minuto;

                return resp;

            }
            else
            {
                return "";
            }

        }

        public string LerStringFormatoSemanaDataHora(string field)
        {
            string datahora;
            object obj = Consulta()[field];
            if (obj != null)
                datahora = Convert.ToString(obj);
            else
                throw new Exception("campo " + field + " não existe.");

            if (datahora.Trim() == "")
                return "";

            else if (datahora.Length >= 12)
            {
                string ano = datahora.Substring(0, 4);
                string mes = datahora.Substring(4, 2);
                string dia = datahora.Substring(6, 2);
                DateTime data = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia));
                string diaSemanaImp;
                switch (data.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        diaSemanaImp = "segunda, ";
                        break;
                    case DayOfWeek.Tuesday:
                        diaSemanaImp = "terça, ";
                        break;
                    case DayOfWeek.Wednesday:
                        diaSemanaImp = "quarta, ";
                        break;
                    case DayOfWeek.Thursday:
                        diaSemanaImp = "quinta, ";
                        break;
                    case DayOfWeek.Friday:
                        diaSemanaImp = "sexta, ";
                        break;
                    case DayOfWeek.Saturday:
                        diaSemanaImp = "sábado, ";
                        break;
                    default:
                        diaSemanaImp = "domingo, ";
                        break;
                }
                string hora = datahora.Substring(8, 2);
                string minuto = datahora.Substring(10, 2);

                string resp = diaSemanaImp + dia + "/" + mes + "/" + ano + " " + hora + ":" + minuto;

                return resp;

            }
            else
            {
                return "";
            }

        }

        public string LerStringFormatoData(string field)
        {
            string data;
            object obj = Consulta()[field];
            if (obj != null)
                data = Convert.ToString(obj);
            else
                throw new Exception("campo " + field + " não existe.");

            if (data.Trim() == "")
                return "";

            else if (data.Length >= 8)
            {
                string ano = data.Substring(0, 4);
                string mes = data.Substring(4, 2);
                string dia = data.Substring(6, 2);

                string resp = dia + "/" + mes + "/" + ano;

                return resp;

            }
            else
            {
                return "";
            }

        }

        public DateTime LerDateTime(string field)
        {

            DateTime retorno;
            string datahora;
            object obj = Consulta()[field];
            if (obj != null)
                datahora = Convert.ToString(obj);
            else
                throw new Exception("campo " + field + " não existe.");

            if (datahora.Trim() == "") //se nao tiver data vai retornar DateTime.MinValue
                return System.DateTime.MinValue;

            else if (datahora.Length >= 8)
            {
                string ano = datahora.Substring(0, 4);
                string mes = datahora.Substring(4, 2);
                string dia = datahora.Substring(6, 2);
                string hora = "0";
                string minuto = "0";
                string segundo = "0";

                if (datahora.Length >= 12)
                { //o metodo trata Data ou DataHora...
                    hora = datahora.Substring(8, 2);
                    minuto = datahora.Substring(10, 2);
                }
                if (datahora.Length >= 14)
                { //o metodo trata Data ou DataHora...
                    segundo = datahora.Substring(12, 2);
                }

                try
                {
                    retorno = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), int.Parse(segundo));
                }
                catch
                {
                    throw new Exception("campo não é uma data correta.");
                }

                return retorno;

            }
            else
            {
                return System.DateTime.MinValue;
            }

        }

        public string LerDateTimeAccertify(string field)
        {
            string retorno;
            string datahora;
            object obj = Consulta()[field];
            if (obj != null)
                datahora = Convert.ToString(obj);
            else
                throw new Exception("campo " + field + " não existe.");

            if (string.IsNullOrEmpty(datahora)) //se nao tiver data vai retornar DateTime.MinValue
                return string.Empty;

            else if (datahora.Length >= 8)
            {
                string ano = datahora.Substring(0, 4);
                string mes = datahora.Substring(4, 2);
                string dia = datahora.Substring(6, 2);
                string hora = "0";
                string minuto = "0";
                string segundo = "0";

                if (datahora.Length >= 12)
                { //o metodo trata Data ou DataHora...
                    hora = datahora.Substring(8, 2);
                    minuto = datahora.Substring(10, 2);
                }
                if (datahora.Length >= 14)
                { //o metodo trata Data ou DataHora...
                    segundo = datahora.Substring(12, 2);
                }

                try
                {
                    retorno = string.Format("{0}/{1}/{2} {3}:{4}:{5}", ano, mes, dia, hora, minuto, segundo);
                }
                catch
                {
                    throw new Exception("campo não é uma data correta.");
                }

                return retorno;

            }
            else
            {
                return string.Empty;
            }

        }

        public bool LerBoolean(int field)
        {
            string resp = (Consulta().IsDBNull(field)) ? "F" : Consulta().GetString(field);
            bool ok = (resp.Equals("T"));
            return ok;
        }

        public bool LerBoolean(string field)
        {
            string resp = (Consulta()[field].Equals(DBNull.Value)) ? "F" : Convert.ToString(Consulta()[field]);
            bool ok = (resp.Equals("T"));
            return ok;
        }

        public decimal LerDecimal(int field)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            decimal resp = (Consulta().IsDBNull(field)) ? 0m : Convert.ToDecimal(Consulta()[field]);
            return resp;
        }

        public decimal LerDecimal(string field)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CINTER_CULTURE);
            decimal resp = (Consulta()[field].Equals(DBNull.Value)) ? 0m : Convert.ToDecimal(Consulta()[field]);
            return resp;
        }

        #endregion

        public void Dispose()
        {
            this.Fechar();
        }
    }
}
