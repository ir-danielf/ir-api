/**************************************************
* Arquivo: PerfilRegional.cs
* Gerado: 03/07/2008
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib
{

    public class PerfilRegional : PerfilRegional_B
    {

        public PerfilRegional() { }

        public PerfilRegional(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Devolve uma lista de logins dos usuarios da Regional e do perfil passado como parametro.
        /// </summary>
        /// <returns></returns>
        public override DataTable Logins(int perfilid, int regionalid)
        {
            try
            {
                DataTable tabela = new DataTable("PerfilRegional");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Login", typeof(string));

                string sql = "SELECT tUsuario.ID,tUsuario.Login FROM tUsuario,tPerfilRegional " +
                    "WHERE tUsuario.Status<>'B' AND tUsuario.ID=tPerfilRegional.UsuarioID AND " +
                    "tPerfilRegional.RegionalID=" + regionalid + " AND tPerfilRegional.PerfilID=" + perfilid + " ORDER BY tUsuario.Login";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Login"] = bd.LerString("Login");
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

    public class PerfilRegionalLista : PerfilRegionalLista_B
    {

        public PerfilRegionalLista() { }

        public PerfilRegionalLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
