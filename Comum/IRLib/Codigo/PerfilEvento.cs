/**************************************************
* Arquivo: PerfilEvento.cs
* Gerado: segunda-feira, 4 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class PerfilEvento : PerfilEvento_B{
	
		public PerfilEvento(){}

		public PerfilEvento(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		///Devolve uma lista de logins dos usuarios do evento e do perfil passado como parametro.
		/// </summary>
		/// <returns></returns>
		public override DataTable Logins(int perfilid, int eventoid){

			try{

				DataTable tabela = new DataTable("PerfilEvento");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Login", typeof(string));

				string sql = "SELECT tUsuario.ID,tUsuario.Login FROM tUsuario,tPerfilEvento "+
					"WHERE tUsuario.Status<>'B' AND tUsuario.ID=tPerfilEvento.UsuarioID AND "+
					"tPerfilEvento.EventoID="+eventoid+" AND tPerfilEvento.PerfilID="+perfilid+" ORDER BY tUsuario.Login";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Login"]= bd.LerString("Login");
					tabela.Rows.Add(linha);
				}

				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}

	}

	public class PerfilEventoLista : PerfilEventoLista_B{
	
		public PerfilEventoLista(){}

		public PerfilEventoLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
