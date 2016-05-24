/**************************************************
* Arquivo: Estoque.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib {

	public class Estoque : Estoque_B	{
	
		public Estoque() {}
	
		public Estoque(int usuarioIDLogado) : base (usuarioIDLogado){}

		public override bool Excluir(){

			bool ok1, ok2 = false;

			EstoqueItemLista itens = new EstoqueItemLista();

			itens.FiltroSQL = "EstoqueID = " + this.Control.ID;
			itens.Carregar();
			ok1 = itens.ExcluirTudo();

			if (ok1) //soh exclui o estoque se tiver excluido todos os seus itens.
				ok2 = base.Excluir();

			return ok2;

		} 

		/// <summary>		
		///Obter itens deste estoque
		/// </summary>
		/// <returns></returns>
		public override DataTable Itens(){

			try{

				DataTable tabela = new DataTable("EstoqueItem");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EstoqueID", typeof(int));
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("Produto", typeof(string));
				tabela.Columns.Add("SaldoAtual", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));

				string sql = "SELECT t.ID, t.ProdutoID, p.Nome AS Produto, "+
					"t.SaldoAtual, t.Obs FROM tEstoqueItem as t, tProduto as p, tEstoque as e WHERE "+
					"e.ID=t.EstoqueID AND p.ID=t.ProdutoID AND e.ID="+this.Control.ID;

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["EstoqueID"]= this.Control.ID;
					linha["ProdutoID"]= bd.LerInt("ProdutoID");
					linha["Produto"]= bd.LerString("Produto");
					linha["SaldoAtual"]= bd.LerDecimal("SaldoAtual");
					linha["Obs"]= bd.LerString("Obs");
					tabela.Rows.Add(linha);
				}

				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}
		
		/// <summary>		
		///Obter produtos deste estoque
		/// </summary>
		/// <returns></returns>
		public override DataTable Produtos(){

			try{

				DataTable tabela = new DataTable("Produto");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				string sql = "SELECT p.ID, p.Nome FROM tEstoqueItem as t, tProduto as p, tEstoque as e WHERE "+
					"e.ID=t.EstoqueID AND p.ID=t.ProdutoID AND e.ID="+this.Control.ID;

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

	public class EstoqueLista : EstoqueLista_B{
	
		public EstoqueLista() {}
		private int UsuarioIDLogado;
	
		public EstoqueLista(int usuarioIDLogado) : base (usuarioIDLogado){
			UsuarioIDLogado = usuarioIDLogado;
		}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoque carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("Estoque");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Local", typeof(string));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("Descricao", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						Local local = new Local(UsuarioIDLogado);
						local.Ler(estoque.LocalID.Valor);
						linha["Local"]= local.Nome.Valor;
						linha["Nome"]= estoque.Nome.Valor;
						linha["Descricao"]= estoque.Descricao.Valor;
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

