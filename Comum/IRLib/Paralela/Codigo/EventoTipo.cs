/**************************************************
* Arquivo: EventoTipo.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace IRLib.Paralela{

	public class EventoTipo : EventoTipo_B{
	
		public EventoTipo(){}
	
		public EventoTipo(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		/// Obter todos os tipos
		/// </summary>
		/// <returns></returns>
		public override DataTable Todos(){

			try{

				DataTable tabela = new DataTable("EventoTipo");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT ID, Nome FROM tEventoTipo ORDER BY Nome";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}
		
		/// <summary>		
		/// Obter eventos desse tipo
		/// </summary>
		/// <returns></returns>
		public override DataTable Eventos(){

			try{

				DataTable tabela = new DataTable("Evento");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT e.ID, e.Nome "+
					"FROM tEvento as e,tEventoTipo as et "+
					"WHERE e.EventoTipoID=et.ID AND et.ID="+this.Control.ID+" "+
					"ORDER BY e.Nome";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}

        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> ListaEvento()
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                string sql = string.Format(@"SELECT ID, Nome FROM tEventoTipo ORDER BY Nome");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

	}

	public class EventoTipoLista : EventoTipoLista_B{
	
		public EventoTipoLista(){}
	
		public EventoTipoLista(int usuarioIDLogado) : base (usuarioIDLogado){}


        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tEventoTipo WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Nome");

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
