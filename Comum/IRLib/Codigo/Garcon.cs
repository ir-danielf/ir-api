/**************************************************
* Arquivo: Garcon.cs
* Gerado: 11/05/2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib{

	public class Garcon : Garcon_B{
	
		public Garcon(){}
	
		public Garcon(int usuarioIDLogado) : base (usuarioIDLogado){}


	}

	public class GarconLista : GarconLista_B{
	
		public GarconLista(){}
	
		public GarconLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
		/// <summary>
		/// Obtem uma tabela de todos os campos de garcon carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("Garcon");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("Comissao", typeof(string));
					tabela.Columns.Add("Endereco", typeof(string));
					tabela.Columns.Add("Cidade", typeof(string));
					tabela.Columns.Add("Estado", typeof(string));
					tabela.Columns.Add("CEP", typeof(string));
					tabela.Columns.Add("Telefone", typeof(string));
					tabela.Columns.Add("Email", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["Nome"]= garcon.Nome.Valor;
						linha["Comissao"]= garcon.Comissao.Valor;
						linha["Endereco"]= garcon.Endereco.Valor;
						linha["Cidade"]= garcon.Cidade.Valor;
						linha["Estado"]= garcon.Estado.Valor;
						linha["CEP"]= garcon.CEP.Valor;
						string ddd = (garcon.DDDTelefone.Valor!="") ? "("+garcon.DDDTelefone.Valor+") " : "";
						linha["Telefone"]= ddd+garcon.Telefone.Valor;
						linha["Email"]= garcon.Email.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}else{ //erro: nao carregou a lista
					tabela = null;
				}
				
			}catch{
				tabela = null;
			}			
			
			return tabela;

		}	
		
	}
	
}
