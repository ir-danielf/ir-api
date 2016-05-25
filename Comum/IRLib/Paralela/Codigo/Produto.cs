/**************************************************
* Arquivo: Produto.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela {

	public class Produto : Produto_B{
	
		public Produto(){}

		public Produto(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		///Obter os itens de pedido desse produto
		/// </summary>
		/// <returns></returns>
		public override DataTable ItensPedidos(){
			return null;
		}
		
		/// <summary>		
		///Obter os itens de estoque desse produto
		/// </summary>
		/// <returns></returns>
		public override DataTable ItensEstoques(){
			return null;
		}
		

	}

	public class ProdutoLista : ProdutoLista_B{

		public ProdutoLista(){}
	
		public ProdutoLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem uma tabela de todos os campos de produto carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("Produto");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("PrecoVenda", typeof(decimal));
					tabela.Columns.Add("PrecoCompra", typeof(decimal));
					tabela.Columns.Add("Categoria", typeof(string));
					tabela.Columns.Add("ForaDeLinha", typeof(string));
//					tabela.Columns.Add("UnidadeCompra", typeof(string));
//					tabela.Columns.Add("UnidadeUso", typeof(string));
//					tabela.Columns.Add("Coeficiente", typeof(decimal));

					do{
						DataRow linha = tabela.NewRow();
						linha["Nome"]= produto.Nome.Valor;
						ProdutoCategoria cat = new ProdutoCategoria();
						cat.Ler(produto.ProdutoCategoriaID.Valor);
						linha["Categoria"]= cat.Nome.Valor;
						linha["PrecoVenda"]= produto.PrecoVenda.Valor;
						linha["PrecoCompra"]= produto.PrecoCompra.Valor;
						linha["ForaDeLinha"]= (produto.ForaDeLinha.Valor) ? "Sim" : "Nao";
//						linha["UnidadeCompra"]= produto.UnidadeDeCompra.Valor;
//						linha["UnidadeUso"]= produto.UnidadeDeUso.Valor;
//						linha["Coeficiente"]= produto.CoeficienteDeConversao.Valor;
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
