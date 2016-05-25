using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class PontoVendaHorarioLista : List<PontoVendaHorario>
    {
        DAL oDAL = new DAL();
        PontoVendaHorario objHorario;

        public PontoVendaHorarioLista()
        {
            this.Clear();
        }


        public PontoVendaHorarioLista CarregarHorarioPorPDV(int id)
        {
            string strSql = "SELECT * FROM PontoVendaHorario (NOLOCK) WHERE PontoVendaID = " + id;

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        objHorario = new PontoVendaHorario(Convert.ToInt32(dr["IR_PontoVendaHorarioID"].ToString()));
                        objHorario.PontoVendaID = Convert.ToInt32(dr["PontoVendaID"].ToString());
                        objHorario.HorarioInicial = dr["HorarioInicial"].ToString().Trim();
                        objHorario.HorarioFinal = dr["HorarioFinal"].ToString().Trim();
                        objHorario.DiaSemana = Convert.ToInt32(dr["DiaSemana"].ToString());

                        this.Add(objHorario);
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
