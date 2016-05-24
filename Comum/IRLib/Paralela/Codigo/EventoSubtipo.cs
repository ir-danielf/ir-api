/**************************************************
* Arquivo: EventoSubtipo.cs
* Gerado: 07/10/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class EventoSubtipo : EventoSubtipo_B
    {

        public EventoSubtipo() { }

        public EventoSubtipo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obter todos os tipos
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {
            try
            {

                DataTable tabela = new DataTable("EventoSubtipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoTipoID", typeof(int));
                tabela.Columns.Add("Descricao", typeof(string));

                string sql = "SELECT ID, EventoTipoID, Descricao FROM tEventoSubtipo ORDER BY Descricao";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["EventoTipoID"] = bd.LerInt("EventoTipoID");
                    linha["Descricao"] = bd.LerString("Descricao");
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
        /// Obter eventos desse tipo
        /// </summary>
        /// <returns></returns>
        public override DataTable Eventos()
        {

            return null;

        }

        public int CarregarPorSubtipoID()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT EventoTipoID FROM tEventoSubtipo WHERE ");
                stbSQL.Append("ID = " + this.Control.ID);

                int ID = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
                return ID;
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



        public int MontarPorCategoriaEstilo(int categoriaID, string estilo)
        {
            int id = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tEventoSubTipo WHERE EventoTipoID = " + categoriaID + " AND Descricao LIKE '" + estilo.ToSafeString() + "'"));

            if (id > 0)
                return id;

            this.EventoTipoID.Valor = categoriaID;
            this.Descricao.Valor = estilo;
            this.Inserir();

            return this.Control.ID;
        }
    }

    public class EventoSubtipoLista : EventoSubtipoLista_B
    {

        public EventoSubtipoLista() { }

        public EventoSubtipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tEventoSubtipo WHERE ID IN (" + IDsAtuais + ") AND Descricao like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Descricao");

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



