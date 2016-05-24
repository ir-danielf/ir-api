/**************************************************
* Arquivo: CanalTipo.cs
* Gerado: 19/09/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;

namespace IRLib
{

    public class CanalTipo : CanalTipo_B
    {

        public CanalTipo() { }

        public CanalTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }


        /// <summary>
        /// Obter todos os tipos de canais
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(string registroZero)
        {

            try
            {

                DataTable dtt = this.Todos();

                if (string.IsNullOrEmpty(registroZero))
                    return dtt;

                DataRow linhaZero = dtt.NewRow();
                linhaZero["ID"] = 0;
                linhaZero["Nome"] = registroZero;

                dtt.Rows.InsertAt(linhaZero, 0);

                return dtt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Obter todos os tipos de canais
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            try
            {

                DataTable tabela = new DataTable("CanalTipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID,Nome FROM tCanalTipo ORDER BY Nome";

                bd.Consulta(sql);

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

        }


    }

    public class CanalTipoLista : CanalTipoLista_B
    {

        public CanalTipoLista() { }

        public CanalTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }


        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tCanalTipo WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

}
