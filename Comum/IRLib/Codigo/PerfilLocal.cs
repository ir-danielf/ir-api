/**************************************************
* Arquivo: PerfilLocal.cs
* Gerado: segunda-feira, 4 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class PerfilLocal : PerfilLocal_B{
	
		public PerfilLocal(){}

		public PerfilLocal(int usuarioIDLogado) : base (usuarioIDLogado){}


		/// <summary>		
		///Devolve uma lista de logins dos usuarios do local e do perfil passado como parametro.
		/// </summary>
		/// <returns></returns>
		public override DataTable Logins(int perfilid, int localid){

			try{

				DataTable tabela = new DataTable("PerfilLocal");
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Login", typeof(string));

				string sql = "SELECT tUsuario.ID,tUsuario.Login FROM tUsuario,tPerfilLocal "+
					"WHERE tUsuario.Status<>'B' AND tUsuario.ID=tPerfilLocal.UsuarioID AND "+
					"tPerfilLocal.LocalID="+localid+" AND tPerfilLocal.PerfilID="+perfilid+" ORDER BY tUsuario.Login";

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

	public class PerfilLocalLista : PerfilLocalLista_B{

		public PerfilLocalLista(){}

		public PerfilLocalLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
