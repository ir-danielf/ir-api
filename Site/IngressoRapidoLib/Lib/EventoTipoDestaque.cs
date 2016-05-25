using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class EventoTipoDestaque
    {
        DAL oDAL = new DAL();

        public List<EstruturaEventoDestaque> BuscarDestaque(int EventoTipoID)
        {
            try
            {
                List<EstruturaEventoDestaque> lista = new List<EstruturaEventoDestaque>();

                string sql = string.Format(@"SELECT Top 1 Evento.IR_EventoID AS ID, Evento.Nome, Evento.ImagemDestaque
                FROM EventoTipoDestaque (NOLOCK)
                INNER JOIN Evento (NOLOCK) ON Evento.IR_EventoID = EventoTipoDestaque.EventoID
                Where (Evento.FilmeID IS NULL OR Evento.FilmeID = 0)
                AND EventoTipoDestaque.EventoTipoID = {0} ORDER BY NEWID()", EventoTipoID);

                using (IDataReader dr = oDAL.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                    {
                        lista.Add(new IRLib.ClientObjects.EstruturaEventoDestaque()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Evento = dr["Nome"].ToString(),
                            Imagem = dr["ImagemDestaque"].ToString(),
                        });
                    }
                }
                return lista;
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
