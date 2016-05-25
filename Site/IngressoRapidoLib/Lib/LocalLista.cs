using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for LocalLista
    /// </summary>
    public class LocalLista : List<Local>
    {

        DAL oDAL = new DAL();
        Local oLocal;

        public LocalLista()
        {
            this.Clear();
        }

        private int registros;
        public int Registros
        {
            get { return registros; }
            set { registros = value; }
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Locais do tipo Local, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        public LocalLista CarregarLista(string clausula, int startRowIndex, int numRows)
        {
            string strSql = string.Empty;

            if (clausula == string.Empty)
                clausula = "1=1";

            if (numRows == 0)
            {
                strSql = "SELECT  DISTINCT " +
                         "IR_LocalID, Nome, Cidade, Estado, Pais " +
                         "FROM Local (NOLOCK) " +
                         "WHERE " + clausula + " ORDER BY Nome";
            }
            else
            {
                strSql = "WITH tbGeral AS( " +
                            "SELECT  DISTINCT " +
                            "IR_LocalID, Nome, Cidade, Estado, Pais " +
                            "FROM Local (NOLOCK) " +
                            "WHERE " + clausula + "), " +

                            "tbCount AS( " +
                            "	SELECT Count(IR_LocalID) as Registros FROM tbGeral), " +

                            "tbOrdenada AS( " +
                            "	SELECT *, ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber' FROM tbGeral) " +

                            "SELECT * FROM tbOrdenada, tbCount " +
                            "WHERE RowNumber between " + startRowIndex + " and " + (startRowIndex + numRows - 1) + " ORDER BY Nome";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oLocal = new Local(Convert.ToInt32(dr["IR_LocalID"].ToString()));
                        oLocal.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        oLocal.Cidade = dr["Cidade"].ToString();
                        oLocal.Uf = dr["Estado"].ToString().ToUpper();
                        oLocal.Pais = dr["Pais"].ToString();
                        if (numRows != 0)
                            this.Registros = Convert.ToInt32(dr["Registros"].ToString());

                        this.Add(oLocal);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
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

        private LocalLista CarregarListaCidade(string clausula)
        {
            string strSql = string.Empty;

            if (clausula != string.Empty)
            {
                strSql = "SELECT  DISTINCT " +
                         "Cidade " +
                         "FROM Local (NOLOCK) " +
                         "WHERE " + clausula + " ORDER BY Cidade";
            }
            else
            {
                strSql = "SELECT  DISTINCT " +
                         "Cidade  " +
                         "FROM Local (NOLOCK) ORDER BY Cidade";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oLocal = new Local();
                        oLocal.Cidade = Util.LimparTitulo(dr["Cidade"].ToString());
                        this.Add(oLocal);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
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
        /// Retorna uma Lista de Locais do tipo Local, 
        /// a partir de um estado
        /// </summary>
        public LocalLista CarregarDadosporEstado(string uf)
        {
            return CarregarListaCidade("Estado = '" + uf.Replace("'", "''") + "'");
        }

        public LocalLista CarregarLocalPorEstado(string uf, string primeiroRegistro)
        {
            if (primeiroRegistro.Length > 0)
                this.Add(new Local(0) { Nome = primeiroRegistro });

            return CarregarLista("Estado = '" + uf.Replace("'", "''") + "'", 0, 0);
        }

        public LocalLista CarregarLocalPorCidade(string cidade, string primeiroRegistro)
        {
            if (primeiroRegistro.Length > 0)
                this.Add(new Local(0) { Nome = primeiroRegistro });
            return CarregarLista("Cidade = '" + cidade.Replace("'", "''") + "'", 0, 0);
        }

        /// <summary>
        /// Retorna uma Lista de Locais do tipo Local
        /// </summary>
        public LocalLista CarregarDados()
        {
            return CarregarLista("1=1", 0, 0);
        }

        public LocalLista EstadosAtivos(string primeiroRegistro)
        {
            try
            {
                if (primeiroRegistro.Length > 0)
                    this.Add(new Local { Uf = primeiroRegistro });

                string strSql = "SELECT DISTINCT Estado FROM Local (NOLOCK) where Estado <> '' ORDER BY Estado ";
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oLocal = new Local();
                        oLocal.Uf = dr["Estado"].ToString();
                        this.Add(oLocal);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
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
        public LocalLista CidadesPorEstado(string estado, string primeiroRegistro)
        {
            try
            {
                if (primeiroRegistro.Length > 0)
                    this.Add(new Local { Cidade = primeiroRegistro });

                string strSql = "SELECT DISTINCT Cidade FROM Local (NOLOCK) where Estado = '" + estado.Replace("'", "''") + "' ORDER BY Cidade ";
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oLocal = new Local();
                        oLocal.Cidade = dr["Cidade"].ToString();
                        this.Add(oLocal);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
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

        public int CarregarQuantidadeLocais(string clausula)
        {
            int qtde = 0;

            string strSql = "";

            if (clausula != "")
                strSql = "SELECT COUNT(*) Total FROM Local (NOLOCK) AS L WHERE " + clausula;
            else
                strSql = "SELECT COUNT(*) Total FROM Local (NOLOCK)";

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


        public static LocalLista Pesquisar(int startRowIndex, int numRows, string estado, string cidade, bool filtroDataUnica, DateTime? data, DateTime? dataDe, DateTime? dataAte)
        {
            DAL oDAL = new DAL();
            StringBuilder strFiltros = new StringBuilder(" WHERE 1=1 ");

            if (estado.Length > 0)
                strFiltros.Append(" AND Estado = '" + Util.StringToBD(estado) + "'");

            if (cidade.Length > 0)
                strFiltros.Append(" AND Cidade = '" + Util.StringToBD(cidade) + "'");

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
                    strFiltros.Append(" AND Horario < '" + dataAte.Value.AddDays(1).ToString("yyyyMMdd") + "'");
            }

            string strSql =

                            "WITH tbGeral AS( " +
                            "	SELECT  " +
                            "	DISTINCT IR_LocalID, LocalNome, Estado, Cidade, Pais, COUNT(DISTINCT IR_EventoID) AS Eventos " +
                            "	FROM vwBaseBusca (NOLOCK)  " +
                            strFiltros.ToString() +
                            "	GROUP BY  IR_LocalID, LocalNome, Estado, Cidade, Pais " +
                            "), " +
                            "tbCount AS(  " +
                            "	SELECT Count(IR_LocalID) as Registros FROM tbGeral),  " +
                            "tbOrdenada AS(  " +
                            "	SELECT IR_LocalID, LocalNome, Estado, Cidade, Pais, Eventos, ROW_NUMBER() OVER (ORDER BY LocalNome) AS 'RowNumber' FROM tbGeral)  " +
                            "SELECT IR_LocalID, LocalNome, Estado, Cidade, Pais, RowNumber, Registros, Eventos FROM tbOrdenada, tbCount  " +
                            "WHERE RowNumber between " + startRowIndex + " and " + (startRowIndex + numRows - 1) + " ORDER BY LocalNome";


            LocalLista objLocalLista = new LocalLista();
            Local oLocal = new Local();

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oLocal = new Local(Convert.ToInt32(dr["IR_LocalID"].ToString()));
                        oLocal.Nome = dr["LocalNome"].ToString();
                        oLocal.Uf = dr["Estado"].ToString();
                        oLocal.Cidade = dr["Cidade"].ToString();
                        oLocal.QtdEventos = Convert.ToInt32(dr["Eventos"]);
                        oLocal.Pais = dr["Pais"].ToString();
                        if (numRows != 0)
                        {
                            objLocalLista.registros = Convert.ToInt32(dr["Registros"].ToString());
                        }
                        objLocalLista.Add(oLocal);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return objLocalLista;
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
    }
}