using CTLib;
using IRLib.ClientObjects;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class Estado
    {
        public List<EstruturaIDNome> Todos()
        {
            IRLib.Estado oE = new IRLib.Estado();

            List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();
            DataTable estados = oE.Todos();
            foreach (DataRow item in estados.Rows)
            {
                retorno.Add(new EstruturaIDNome
                      {
                          ID = item["ID"].ToInt32(),
                          Nome = item["Sigla"].ToString()
                      });

            }


            return retorno;
        }

        public List<EstruturaIDNome> CarregarTodosPorFilme(int filmeID, bool inicial)
        {
            DAL oDal = new DAL();
            try
            {
                var lista = new List<EstruturaIDNome>();
                using (IDataReader dr = oDal.SelectToIDataReader(
                    @"SELECT DISTINCT 
                        l.ID, e.ID, e.Sigla 
                    FROM Evento ev 
                    INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = ev.LocalID
                    INNER JOIN Estado e ON e.Sigla = l.Estado 
                    WHERE ev.FilmeID = " + filmeID + " ORDER BY Sigla"))
                {
                    while (dr.Read())
                        lista.Add(new EstruturaIDNome()
                        {
                            ID = dr["ID"].ToInt32(),
                            Nome = dr["Sigla"].ToString(),
                        });
                }

                if (inicial)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todos" });

                return lista;
            }
            finally
            {
                oDal.ConnClose();
            }
        }
    }
}
