/**************************************************
* Arquivo: Pais.cs
* Gerado: 23/03/2011
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela
{

    public class Pais : Pais_B
    {

        public Pais() { }

        public Pais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable Todos()
        {
            try
            {
                DataTable dtt = new DataTable();
                dtt.Columns.Add("ID", typeof(int));
                dtt.Columns.Add("Nome", typeof(string));
                DataRow dtr;
                bd.Consulta("SELECT ID, Nome FROM tPais (NOLOCK) ORDER BY Nome");
                while (bd.Consulta().Read())
                {
                    dtr = dtt.NewRow();
                    dtr["ID"] = bd.LerInt("ID");
                    dtr["Nome"] = bd.LerString("Nome");
                    dtt.Rows.Add(dtr);
                }
                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class PaisLista : PaisLista_B
    {

        public PaisLista() { }

        public PaisLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
