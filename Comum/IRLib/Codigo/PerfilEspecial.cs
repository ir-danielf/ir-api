/**************************************************
* Arquivo: PerfilEspecial.cs
* Gerado: segunda-feira, 4 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class PerfilEspecial : PerfilEspecial_B{
	
		public PerfilEspecial(){}

		public PerfilEspecial(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		///Devolve uma tabela de logins dos usuarios do perfil passado como parametro.
		/// </summary>
		/// <returns></returns>
		public override DataTable Logins(int perfilid){

			try{

				DataTable tabela = new DataTable("PerfilEspecial");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Login", typeof(string));

				string sql = "SELECT tUsuario.ID,tUsuario.Login FROM tUsuario,tPerfilEspecial "+
					"WHERE tUsuario.Status<>'B' AND tUsuario.ID=tPerfilEspecial.UsuarioID AND "+
					"tPerfilEspecial.PerfilID="+perfilid+" ORDER BY Login";

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

	public class PerfilEspecialLista : PerfilEspecialLista_B{
	
		public PerfilEspecialLista(){}

		public PerfilEspecialLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
