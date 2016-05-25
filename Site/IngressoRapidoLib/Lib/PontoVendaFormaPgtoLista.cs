using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class PontoVendaFormaPgtoLista : List<PontoVendaFormaPgto>
    {
        DAL oDAL = new DAL();
        PontoVendaFormaPgto objFormaPgto;

        public PontoVendaFormaPgtoLista()
        {
            this.Clear();
        }


        public PontoVendaFormaPgtoLista CarregarPDVFormaPgto(int id)
        {
            string strSql = "SELECT PontoVendaFormaPgto.IR_PontoVendaFormaPgtoID, Nome FROM PontoVendaXFormaPgto (NOLOCK) " +
                    " INNER JOIN PontoVendaFormaPgto (NOLOCK) ON PontoVendaFormaPgto.IR_PontoVendaFormaPgtoID = PontoVendaXFormaPgto.PontoVendaFormaPgtoID" +
                    " WHERE PontoVendaID = " + id + " ORDER BY Nome";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        objFormaPgto = new PontoVendaFormaPgto(Convert.ToInt32(dr["IR_PontoVendaFormaPgtoID"].ToString()));
                        objFormaPgto.Nome = dr["Nome"].ToString();

                        this.Add(objFormaPgto);
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
    }
}
