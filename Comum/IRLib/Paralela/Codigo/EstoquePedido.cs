/**************************************************
* Arquivo: EstoquePedido.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Diagnostics;

namespace IRLib.Paralela {

	public class EstoquePedido : EstoquePedido_B	{
	
		public EstoquePedido() {}
	
		public EstoquePedido(int usuarioIDLogado) : base (usuarioIDLogado){}

		public override bool Excluir(){
			bool ok1, ok2 = false;
			ok1 = excluirItens();
			if (ok1) //soh exclui o estoque se tiver excluido todos os seus itens.
				ok2 = base.Excluir();
			return ok2;
		}

		private bool excluirItens(){
			bool ok;
			EstoquePedidoItemLista itens = new EstoquePedidoItemLista();
			itens.FiltroSQL = "EstoquePedidoID = " + this.Control.ID;
			itens.Carregar();
			itens.Ultimo();
			//fazer varredura de traz pra frente.
			do
				ok = itens.Excluir();
			while (ok && itens.Anterior());
			return ok;
		} 

		/// <summary>		
		/// Retorna uma lista de itens do transferencia
		/// </summary>
		/// <returns></returns>
		public override System.Data.DataTable Itens(){
			// Criando um DataTable (objeto-tabela) com ID 
			DataTable tabela = new DataTable();
			try{
				// Criando colunas do DataTable
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("EstoqueID", typeof(int));
				tabela.Columns.Add("EstoqueItemID", typeof(int));
				tabela.Columns.Add("EstoqueNome", typeof(string));
				tabela.Columns.Add("ProdutoNome", typeof(string));
				tabela.Columns.Add("PrecoCompra", typeof(decimal));
				tabela.Columns.Add("Quantidade", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));
				// Atribuindo os dados para o objeto
				BD bd = new BD();
				string sql;
				// Criar uma tabela resultante temporaria
				sql = "SELECT tEstoquePedidoItem.Quantidade, tEstoquePedidoItem.PrecoCompra, tEstoquePedidoItem.Obs, tEstoquePedidoItem.ID, " +
	                     "tProduto.Nome AS ProdutoNome, tEstoque.Nome AS EstoqueNome, tEstoquePedidoItem.EstoqueItemID, tEstoqueItem.EstoqueID, tEstoqueItem.ProdutoID " +
								"FROM tEstoquePedidoItem INNER JOIN tEstoqueItem ON tEstoquePedidoItem.EstoqueItemID = tEstoqueItem.ID INNER JOIN " +
								"tEstoque ON tEstoqueItem.EstoqueID = tEstoque.ID INNER JOIN "+
								"tProduto ON tEstoqueItem.ProdutoID = tProduto.ID "+
								"WHERE tEstoquePedidoItem.EstoquePedidoID = " + this.Control.ID;
				bd.Consulta(sql);
				// Alimentar os registros no Objeto-tabela a partir da tabela temporaria
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["EstoqueID"]= bd.LerInt("EstoqueID");
					linha["ProdutoID"]= bd.LerInt("ProdutoID");
					linha["EstoqueItemID"]= bd.LerInt("EstoqueItemID");
					linha["ProdutoNome"] = bd.LerString("ProdutoNome");
					linha["EstoqueNome"] = bd.LerString("EstoqueNome");
					linha["Quantidade"] = bd.LerDecimal("Quantidade");
					linha["PrecoCompra"] = bd.LerDecimal("PrecoCompra");
					linha["Obs"] = bd.LerString("Obs");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch {
				Debug.Fail("Arquivo: IRLIB.EstoquePedido.cs" + "\tClasse: EstoquePedido " + "\tMehtodo: Itens");
				tabela = null;
			}
			// retornar o objeto-tabela
			return tabela;
		} // fim de itens

	} 

	public class EstoquePedidoLista : EstoquePedidoLista_B {
	
		public EstoquePedidoLista() {}
	
		public EstoquePedidoLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoquepedido carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoquePedido");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("Fornecedor", typeof(string));
					tabela.Columns.Add("Responsavel", typeof(string));
					tabela.Columns.Add("Data", typeof(DateTime));
					tabela.Columns.Add("EntregaPrevista", typeof(DateTime));
					tabela.Columns.Add("EntregaReal", typeof(DateTime));

					do{
						DataRow linha = tabela.NewRow();
						linha["Ordem"]= estoquePedido.Ordem.Valor;
						Fornecedor fornecedor = new Fornecedor();
						fornecedor.Ler(estoquePedido.FornecedorID.Valor);
						linha["Fornecedor"]= fornecedor.Nome.Valor;
						Usuario usuario = new Usuario();
						usuario.Ler(estoquePedido.ResponsavelID.Valor);
						linha["Responsavel"]= usuario.Nome.Valor;
						linha["Data"]= estoquePedido.Data.Valor;
						linha["EntregaPrevista"]= estoquePedido.EntregaPrevista.Valor;
						linha["EntregaReal"]= estoquePedido.EntregaReal.Valor;
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
