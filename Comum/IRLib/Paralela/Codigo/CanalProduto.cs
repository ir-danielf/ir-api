/**************************************************
* Arquivo: CanalProduto.cs
* Gerado: 14/05/2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib.Paralela{

	public class CanalProduto : CanalProduto_B{
	
		public CanalProduto() {}
	
		public CanalProduto(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>		
		/// Obter produtos de uma categoria especifica num canal especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Produtos(int canalid, int categoriaid){

			try{

				DataTable tabela = new DataTable("Produto");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("Valor", typeof(decimal));

				string sql = "SELECT ID,Nome,PrecoVenda FROM tProduto WHERE ProdutoCategoriaID="+categoriaid+" AND ID in "+
					"(SELECT ProdutoID FROM tCanalProduto WHERE CanalID="+canalid+") ORDER BY Nome";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					linha["Valor"]= bd.LerDecimal("PrecoVenda");
					tabela.Rows.Add(linha);
				}

				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}
		
		/// <summary>		
		/// Obter produtos de um canal especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Produtos(int canalid){

			try{

				DataTable tabela = new DataTable("Produto");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT ID,Nome FROM tProduto WHERE ID in "+
					"(SELECT ProdutoID FROM tCanalProduto WHERE CanalID="+canalid+") ORDER BY Nome";

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
		/// Obter canais de um produto especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Canais(int produtoid){

			try{

				DataTable tabela = new DataTable("Canal");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT c.ID,c.Nome FROM tCanal as c, tCanalProduto as cp "+
					"WHERE c.ID=cp.CanalID AND "+
					"cp.ProdutoID="+produtoid+" ORDER BY c.Nome";

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

	public class CanalProdutoLista : CanalProdutoLista_B{
	
		public CanalProdutoLista() {}
	
		public CanalProdutoLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
