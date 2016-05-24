/**************************************************
* Arquivo: PerfilCanal.cs
* Gerado: segunda-feira, 4 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class PerfilCanal : PerfilCanal_B{
	
		public PerfilCanal(){}

		public PerfilCanal(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		///Devolve uma lista de logins dos usuarios do canal e do perfil passado como parametro.
		/// </summary>
		/// <returns></returns>
		public override DataTable Logins(int perfilid, int canalid){

			try{

				DataTable tabela = new DataTable("PerfilCanal");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Login", typeof(string));

				string sql = "SELECT tUsuario.ID,tUsuario.Login FROM tUsuario,tPerfilCanal "+
					"WHERE tUsuario.Status<>'B' AND tUsuario.ID=tPerfilCanal.UsuarioID AND "+
					"tPerfilCanal.CanalID="+canalid+" AND tPerfilCanal.PerfilID="+perfilid+" ORDER BY tUsuario.Login";

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

	public class PerfilCanalLista : PerfilCanalLista_B{
	
		public PerfilCanalLista(){}

		public PerfilCanalLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
