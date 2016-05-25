using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class LogErrosSiteLista : List<LogErrosSite>
    {
        DAL oDAL = new DAL();

        private LogErrosSite oLogErrosSite { get; set; }

        public LogErrosSiteLista CarregaLista(string Clausula)
        {
            string strSql = String.Empty;

            if (Clausula != "")
            {
                strSql = "";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oLogErrosSite = new LogErrosSite(Convert.ToInt32(dr["ID"].ToString()));


                        this.Add(oLogErrosSite);
                    }
                }

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
    }
}
