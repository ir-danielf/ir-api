/**************************************************
* Arquivo: FornecedorProduto.cs
* Gerado: 28/04/2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib.Paralela{

	public class FornecedorProduto : FornecedorProduto_B{
	
		public FornecedorProduto(){}
	
		public FornecedorProduto(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		///Obter produtos de fornecedor especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Produtos(int fornecedorid){

			try{

				DataTable tabela = new DataTable("Produto");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT p.ID,p.Nome FROM tProduto as p,tFornecedorProduto as fp "+
					"WHERE p.ID=fp.ProdutoID AND fp.FornecedorID="+fornecedorid+" "+
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
		
		/// <summary>		
		///Obter fornecedores de um produto especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Fornecedores(int produtoid){

			try{

				DataTable tabela = new DataTable("Fornecedor");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT f.ID,f.Nome FROM tFornecedor as f,tFornecedorProduto as fp "+
					"WHERE f.ID=fp.FornecedorID AND fp.ProdutoID="+produtoid+" "+
					"ORDER BY f.Nome";

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

	public class FornecedorProdutoLista : FornecedorProdutoLista_B{
	
		public FornecedorProdutoLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de fornecedorproduto carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("FornecedorProduto");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Fornecedor", typeof(string));
					tabela.Columns.Add("Produto", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						Fornecedor f = new Fornecedor();
						f.Ler(fornecedorProduto.FornecedorID.Valor);
						linha["Fornecedor"]= f.Nome.Valor;
						Produto p = new Produto();
						p.Ler(fornecedorProduto.ProdutoID.Valor);
						linha["Produto"]= p.Nome.Valor;
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
