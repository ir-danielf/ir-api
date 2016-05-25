using CTLib;
using IRLib.ClientObjects;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class Cidade
    {
        public string Nome { get; set; }
        public string EstadoSigla { get; set; }

        public List<Cidade> Todos(int EstadoID)
        {
            IRLib.Cidade oE = new IRLib.Cidade();

            List<Cidade> retorno = new List<Cidade>();
            DataTable estados = oE.TabelaPorEstadocomSigla(EstadoID);

            retorno.Add(new Cidade
            {
                Nome = "Selecione uma cidade",
                EstadoSigla = ""
            });

            foreach (DataRow item in estados.Rows)
            {
                retorno.Add(new Cidade
                {
                    Nome = item["Nome"].ToString(),
                    EstadoSigla = item["EstadoSigla"].ToString()
                });

            }


            return retorno;
        }

        public List<EstruturaIDNome> CarregarPorFileEstado(int filmeID, string estado)
        {
            DAL oDal = new DAL();
            try
            {
                var lista = new List<EstruturaIDNome>();
                using (IDataReader dr = oDal.SelectToIDataReader(
                    string.Format(@"
                          SELECT  c.ID, c.Nome 
                            FROM Evento ev 
                            INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = ev.LocalID
                            INNER JOIN Estado e (NOLOCK) ON e.Sigla = l.Estado 
                            LEFT JOIN Cidade c (NOLOCK) ON c.EstadoID = e.ID AND c.Nome LIKE l.Cidade
                        WHERE e.Sigla = '{0}' AND ev.FilmeID = " + filmeID + " ORDER BY c.Nome", estado.ToSafeString())))
                {
                    while (dr.Read())
                        lista.Add(new EstruturaIDNome()
                        {
                            ID = dr["ID"].ToInt32(),
                            Nome = dr["Nome"].ToString(),
                        });
                }

                return lista;
            }
            finally
            {
                oDal.ConnClose();
            }
        }
    }
}
