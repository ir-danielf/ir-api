/**************************************************
* Arquivo: PerfilEmpresa.cs
* Gerado: segunda-feira, 4 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib.Paralela{

	public class PerfilEmpresa : PerfilEmpresa_B{
	
		public PerfilEmpresa(){}

		public PerfilEmpresa(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		///Devolve uma lista de logins dos usuarios do empresa e do perfil passado como parametro.
		/// </summary>
		/// <returns></returns>
		public override DataTable Logins(int perfilid, int empresaid){

			try{

				DataTable tabela = new DataTable("PerfilEmpresa");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Login", typeof(string));

				string sql = "SELECT tUsuario.ID,tUsuario.Login FROM tUsuario,tPerfilEmpresa "+
					"WHERE tUsuario.Status<>'B' AND tUsuario.ID=tPerfilEmpresa.UsuarioID AND "+
					"tPerfilEmpresa.EmpresaID="+empresaid+" AND tPerfilEmpresa.PerfilID="+perfilid+" ORDER BY tUsuario.Login";

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

	public class PerfilEmpresaLista : PerfilEmpresaLista_B{
	
		public PerfilEmpresaLista(){}

		public PerfilEmpresaLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
