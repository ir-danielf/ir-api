/**************************************************
* Arquivo: PerfilTipo.cs
* Gerado: 28/09/2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class PerfilTipo : PerfilTipo_B{
	
		public const int EMPRESA = 1;
		public const int LOCAL = 2;
		public const int CANAL = 3;
		public const int EVENTO = 4;
		public const int ESPECIAL = 5;
        public const int REGIONAL = 6;

		public PerfilTipo(){}
	
		public PerfilTipo(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem os perfis desse tipo
		/// </summary>
		/// <returns></returns>
		public override DataTable Perfis(){

			try{

				DataTable tabela = new DataTable("Perfil");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT ID,Nome FROM tPerfil WHERE ID NOT IN (3,11,12,13) AND PerfilTipoID="+this.Control.ID+" ORDER BY Nome";

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
		

	}

	public class PerfilTipoLista : PerfilTipoLista_B{
	
		public PerfilTipoLista(){}
	
		public PerfilTipoLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
