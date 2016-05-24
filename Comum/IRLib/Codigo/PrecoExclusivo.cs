/**************************************************
* Arquivo: PrecoExclusivo.cs
* Gerado: 10/11/2008
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib
{

    public class PrecoExclusivo : PrecoExclusivo_B
    {

        public PrecoExclusivo() { }

        public PrecoExclusivo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        /// Obter todas os Precos Exclusivos e adiciona um valor default
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("PrecoExclusivo");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                tabela.Columns.Add("Ativo", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql = "SELECT ID, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo FROM tPrecoExclusivo ORDER BY Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");
                    linha["QuantidadeMaxima"] = bd.LerInt("QuantidadeMaxima");
                    linha["Ativo"] = bd.LerString("Ativo");
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

        /// <summary>		
        /// Obter todos os Precos Exclusivos
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            try
            {

                DataTable tabela = new DataTable("PrecoExclusivo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                tabela.Columns.Add("Ativo", typeof(string));

                string sql = "SELECT ID, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo FROM tPrecoExclusivo ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");
                    linha["QuantidadeMaxima"] = bd.LerInt("QuantidadeMaxima");
                    linha["Ativo"] = bd.LerString("Ativo");
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

        public override DataTable ListagemPorEventoID(int eventoid)
        {
            try
            {

                DataTable tabela = new DataTable("PrecoExclusivo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                tabela.Columns.Add("Ativo", typeof(string));

                string sql = "SELECT ID, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo FROM tPrecoExclusivo WHERE EventoID = " + eventoid + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");
                    linha["QuantidadeMaxima"] = bd.LerInt("QuantidadeMaxima");
                    linha["Ativo"] = bd.LerString("Ativo");
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

        public override DataTable ListagemPorPrecoExclusivo(int precoexclusivoid)
        {
            try
            {

                DataTable tabela = new DataTable("PrecoExclusivo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                tabela.Columns.Add("Ativo", typeof(string));

                string sql = "SELECT ID, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo FROM tPrecoExclusivo WHERE ID = " + precoexclusivoid;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");
                    linha["QuantidadeMaxima"] = bd.LerInt("QuantidadeMaxima");
                    linha["Ativo"] = bd.LerString("Ativo");
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

    public class PrecoExclusivoLista : PrecoExclusivoLista_B
    {

        public PrecoExclusivoLista() { }

        public PrecoExclusivoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
