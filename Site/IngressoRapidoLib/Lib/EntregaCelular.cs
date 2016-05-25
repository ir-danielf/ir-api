using CTLib;
using IRLib.ClientObjects;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IngressoRapido.Lib
{
    public class EntregaCelular
    {
        private DAL oDAL = new DAL();

        #region Atributos
        public int ID { get; set; }
        public int DDD { get; set; }
        public int Area { get; set; }
        public int Numero { get; set; }
        public string Fabricante { get; set; }
        public int ModelID { get; set; }
        #endregion

        #region Construtor

        public EntregaCelular(EstruturaCelular oEstrutura)
        {
            this.DDD = oEstrutura.DDD;
            this.Area = oEstrutura.Area;
            this.Numero = oEstrutura.Numero;
            this.Fabricante = oEstrutura.Frabricante;
            this.ModelID = oEstrutura.ModelID;
        }

        public EntregaCelular(int id)
        {
            this.ID = id;
        }


        #endregion

        #region Métodos

        /// <summary>
        /// Metodo que retorna as informações apartir do ID
        /// </summary>
        /// <returns></returns>
        public EstruturaCelular getByID()
        {
            try
            {
                string sql = "SELECT DDD, Area, Numero, Fabricante, ModelID FROM EntregaCelular WHERE ID = " + ID;

                IDataReader dr = oDAL.SelectToIDataReader(sql);

                if (dr.Read())
                {
                    EstruturaCelular oEstrutura = new EstruturaCelular();
                    oEstrutura.DDD = Convert.ToInt32(dr["DDD"]);
                    oEstrutura.Area = Convert.ToInt32(dr["Area"]);
                    oEstrutura.Numero = Convert.ToInt32(dr["Numero"]);
                    oEstrutura.Frabricante = dr["Fabricante"].ToString();
                    oEstrutura.ModelID = Convert.ToInt32(dr["ModelID"]);
                    return oEstrutura;
                }
                else
                    throw new ApplicationException("Nenhum registro de celular encontrado");

            }
            catch (ApplicationException ex)
            {
                throw ex;
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

        /// <summary>
        /// Insere um novo registro apartir da Estrutura Celular passada no construtor
        /// </summary>
        /// <param name="oCelular"></param>
        /// <returns></returns>
        public int Inserir()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("INSERT INTO EntregaCelular ");
                stbSQL.Append("( DDD, Area, Numero, Fabricante, ModelID ) ");
                stbSQL.Append("VALUES ");
                stbSQL.Append("(@DDD, @Area, @Numero, @Fabricante, @ModelID)");
                stbSQL.Append("SELECT @@IDENTITY");

                SqlParameter[] Parametros = new SqlParameter[5];
                Parametros[0] = new SqlParameter("@DDD", this.DDD);
                Parametros[1] = new SqlParameter("@Area", this.Area);
                Parametros[2] = new SqlParameter("@Numero", this.Numero);
                Parametros[3] = new SqlParameter("@Fabricante", this.Fabricante);
                Parametros[4] = new SqlParameter("@ModelID", this.ModelID);

                return oDAL.Execute(stbSQL.ToString(), Parametros);
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

        /// <summary>
        /// Remove o registro apartir do ID passador no construtor
        /// </summary>
        /// <returns></returns>
        public int Excluir()
        {
            try
            {
                string sql = "DELETE FROM EntregaCelular WHERE ID = " + this.ID;
                return oDAL.Execute(sql);
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

        #endregion

    }
}
