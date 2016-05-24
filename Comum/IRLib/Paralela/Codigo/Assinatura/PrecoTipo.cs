/**************************************************
* Arquivo: PrecoTipo.cs
* Gerado: 08/09/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib.Paralela
{

    public class PrecoTipo : PrecoTipo_B
    {

        public PrecoTipo() { }

        public PrecoTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }
        

        public DataTable BuscarTodos(bool Selecione)
        {
            try
            {
                DataTable tabela = new DataTable("PrecoTipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT * FROM tPrecoTipo ORDER BY ID";

                bd.Consulta(sql);

                if (Selecione) {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = "0";
                    linha["Nome"] = "Selecione";
                    tabela.Rows.Add(linha);
                
                }

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class PrecoTipoLista : PrecoTipoLista_B
    {

        public PrecoTipoLista() { }

        public PrecoTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
