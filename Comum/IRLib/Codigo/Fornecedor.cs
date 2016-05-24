/**************************************************
* Arquivo: Fornecedor.cs
* Gerado: 28/04/2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class Fornecedor : Fornecedor_B{
	
		public Fornecedor(){}
	
		public Fornecedor(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		///Obter os produtos desse fornecedor
		/// </summary>
		/// <returns></returns>
		public override DataTable Produtos(){

			try{

				DataTable tabela = new DataTable("Produto");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT p.ID,p.Nome FROM tProduto as p,tFornecedorProduto as fp "+
					"WHERE p.ID=fp.ProdutoID AND fp.FornecedorID="+this.Control.ID+" "+
					"ORDER BY p.Nome";

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

	public class FornecedorLista : FornecedorLista_B{
	
		public FornecedorLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem uma tabela de todos os campos de fornecedor carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("Fornecedor");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("ContatoNome", typeof(string));
					tabela.Columns.Add("ContatoCargo", typeof(string));
					tabela.Columns.Add("Endereco", typeof(string));
					tabela.Columns.Add("Cidade", typeof(string));
					tabela.Columns.Add("Estado", typeof(string));
					tabela.Columns.Add("CEP", typeof(string));
					tabela.Columns.Add("Telefone", typeof(string));
					tabela.Columns.Add("Fax", typeof(string));
					tabela.Columns.Add("Email", typeof(string));
					tabela.Columns.Add("Website", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["Nome"]= fornecedor.Nome.Valor;
						linha["ContatoNome"]= fornecedor.ContatoNome.Valor;
						linha["ContatoCargo"]= fornecedor.ContatoCargo.Valor;
						linha["Endereco"]= fornecedor.Endereco.Valor;
						linha["Cidade"]= fornecedor.Cidade.Valor;
						linha["Estado"]= fornecedor.Estado.Valor;
						linha["CEP"]= fornecedor.CEP.Valor;
						string ddd = (fornecedor.DDDTelefone.Valor!="") ? "("+fornecedor.DDDTelefone.Valor+") " : "";
						linha["Telefone"]= ddd+fornecedor.Telefone.Valor;
						ddd = (fornecedor.DDDFax.Valor!="") ? "("+fornecedor.DDDFax.Valor+") " : "";
						linha["Fax"]= ddd+fornecedor.Fax.Valor;
						linha["Email"]= fornecedor.Email.Valor;
						linha["Website"]= fornecedor.Website.Valor;
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
