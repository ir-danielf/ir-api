using CTLib;
using System;
using System.Data;

namespace IngressoRapido.Lib
{
    public class VoceSabia
    {
        public string Texto { get; set; }

        private DAL oDAL = new DAL();

        public VoceSabia VoceSabiaRandon()
        {
            string strSql = "SELECT TOP 1 texto FROM voceSabia ORDER BY NEWID()";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    this.Texto = dr["texto"].ToString();
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
    }
}
