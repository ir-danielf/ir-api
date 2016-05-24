using IRLib.ClientObjects;
using IRLib.Codigo.DB;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo
{
    public class CotaTipo : CotaTipo_B
    {
        
        public CotaTipo() { }

        public CotaTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaCotaTipo> getTiposCota()
        {
            try
            {
                List<EstruturaCotaTipo> lista = new List<EstruturaCotaTipo>();
                EstruturaCotaTipo item;
                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT ID,Descricao FROM dbo.tCotaTipo tct (NOLOCK)");
                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    item = new EstruturaCotaTipo();
                    item.ID = bd.LerInt("ID");
                    item.Descricao = bd.LerString("Descricao");
                    
                    
                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
