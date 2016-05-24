using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    
    public class PreReserva
    {
        DAL oDAL = new DAL();

        public List<EstruturaPrecoPreReservaSite> Precos(int apresentacaoID, int setorID)
        {
            string strSql = string.Empty;

            List<EstruturaPrecoPreReservaSite> retorno = new List<EstruturaPrecoPreReservaSite>();
            EstruturaPrecoPreReservaSite item;

            strSql = "SELECT IR_PrecoID, Nome, Valor, ApresentacaoID, SetorID, QuantidadePorCliente " +
                     "FROM Preco " +
                     "WHERE Pacote = 0 AND ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID;

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        if (dr["Nome"].ToString().Trim() != "ClienteVivo")
                        {
                            item = new EstruturaPrecoPreReservaSite();
                            item.ID = Convert.ToInt32(dr["IR_PrecoID"].ToString());
                            item.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                            item.Valor = Convert.ToDecimal(dr["Valor"]);
                            item.QuantidadePorCliente = (int)dr["QuantidadePorCliente"];

                            retorno.Add(item);
                        }
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return retorno;
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
