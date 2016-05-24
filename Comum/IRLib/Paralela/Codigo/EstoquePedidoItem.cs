/**************************************************
* Arquivo: EstoquePedidoItem.cs
* Gerado: sabado, 23 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;
using System.Diagnostics;

namespace IRLib.Paralela{

	public class EstoquePedidoItem : EstoquePedidoItem_B{

		public EstoquePedidoItem() {}
	
		public EstoquePedidoItem(int usuarioIDLogado) : base (usuarioIDLogado){}
	
		/// <summary>
		/// Sobrecarga de Inserir para atualizar saldo no item de estoque
		/// </summary>
		/// <returns></returns>
		public override bool Inserir(){
			bool ok;
			try {
				ok = base.Inserir();
				if (ok) {
					EstoqueItem estoqueItem = new EstoqueItem();
					estoqueItem.Control.ID = this.EstoqueItemID.Valor;
					estoqueItem.Ler(estoqueItem.Control.ID); // preciso do ProdutoID
					// Em uma compra de produto, eh necessaria a conversao da unidade de compra para unidade de uso
					// Para isso, eh necessario o coeficiente de conversao que esta com o produto
					Produto produto = new Produto();
					// Encontrar o Produto em funcao do EstoqueItemID
					produto.Ler(estoqueItem.ProdutoID.Valor);			
					// atualizando 
					estoqueItem.AtualizarSaldo(this.Quantidade.Valor * produto.CoeficienteDeConversao.Valor);
					// Atualizar o valor do Preço na tabela de Produto
					try {
						produto.PrecoCompra.Valor = this.PrecoCompra.Valor;
						produto.Atualizar();
					} catch {
						Debug.Fail("Falha ao atualizar o Preço no Produto, ao efetuar Pedido");
					}
				}
			} catch {
				ok = false;
			}
			return ok;
		}
		/// <summary>
		/// Sobrecarga de Atualizar para atualizar saldo no item de estoque
		/// </summary>
		/// <returns></returns>
		public override bool Atualizar(){
			bool ok;
			try {
				ok = base.Atualizar();
				if (ok) {
					// Para o antigo produto, estoque, e quantidade, tire do saldo do antigo EstoqueItemID
					EstoqueItem estoqueItemAntes = new EstoqueItem(); 
					estoqueItemAntes.Control.ID = this.EstoqueItemID.ValorAntigo;	// Pega EstoqueItemID antigo
					estoqueItemAntes.Ler(estoqueItemAntes.Control.ID); // obtendo produto e estoque relacionado a este estoque item
					// Em uma compra de produto, eh necessaria a conversao da unidade de compra para unidade de uso
					// Para isso, eh necessario o coeficiente de conversao que esta com o produto
					// Encontrar o Produto em funcao do EstoqueItemID
					Produto produtoAntes = new Produto();
					produtoAntes.Ler(estoqueItemAntes.ProdutoID.Valor);	// aqui nao eh valor antigo, pois ja obtemos no EstoqueItemID antigo
					decimal inverteSinal = -1 * this.Quantidade.ValorAntigo;
					//	Atualiza no saldo, tirando a Quantidade antiga no antigo estoque e/ou produto
					estoqueItemAntes.AtualizarSaldo(inverteSinal * produtoAntes.CoeficienteDeConversao.Valor);
					// ==================================================
					// Para o novo produto, estoque, e quantidade, inclua no saldo do novo EstoqueItemID
					EstoqueItem estoqueItemDepois = new EstoqueItem(); 
					estoqueItemDepois.Control.ID = this.EstoqueItemID.Valor;	// Pega EstoqueItemID para ser procurado
					estoqueItemDepois.Ler(estoqueItemDepois.Control.ID); // obtendo produto e estoque relacionado a este estoque item
					// Nao preciso me preocupar em encontrar um novo EstoqueItemID, pois ja foi tratado antes
					// Em uma compra de produto, eh necessaria a conversao da unidade de compra para unidade de uso
					// Para isso, eh necessario o coeficiente de conversao que esta com o produto
					// Encontrar o Produto em funcao do EstoqueItemID
					Produto produtoDepois = new Produto();
					produtoDepois.Ler(estoqueItemDepois.ProdutoID.Valor);					
					//	Atualiza no saldo, somando a Quantidade no novo estoque e/ou produto
					estoqueItemDepois.AtualizarSaldo(this.Quantidade.Valor * produtoDepois.CoeficienteDeConversao.Valor);
					// Atualizar o valor do Preço na tabela de Produto, neste caso, importa sempre depois da alteracao			
					try {
						produtoDepois.PrecoCompra.Valor = this.PrecoCompra.Valor;
						produtoDepois.Atualizar();
					} catch {
						Debug.Fail("Falha ao atualizar o Preço no Produto, ao efetuar Pedido");
					}

				} // fim de if-ok
			} catch {
				ok = false;
			}
			return ok;
		}
		/// <summary>
		/// Sobrecarga de Excluir para atualizar saldo no item de estoque
		/// </summary>
		/// <returns></returns>
		public override bool Excluir(){
			bool ok;
			try {
				ok = base.Excluir();
				if (ok) {
					EstoqueItem estoqueItem = new EstoqueItem();
					estoqueItem.Control.ID = this.EstoqueItemID.Valor;
					estoqueItem.Ler(estoqueItem.Control.ID); // preciso do ProdutoID
					decimal inverteSinal = -1 * this.Quantidade.Valor;
					// Em uma compra de produto, eh necessaria a conversao da unidade de compra para unidade de uso
					// Para isso, eh necessario o coeficiente de conversao que esta com o produto
					Produto produto = new Produto();
					// Encontrar o Produto em funcao do EstoqueItemID
					produto.Ler(estoqueItem.ProdutoID.Valor);					
					// atualizando 
					estoqueItem.AtualizarSaldo(inverteSinal * produto.CoeficienteDeConversao.Valor);
				}
			} catch {
				ok = false;
			}
			return ok;
		}


	}

	public class EstoquePedidoItemLista : EstoquePedidoItemLista_B{
	
		public EstoquePedidoItemLista() {}
	
		public EstoquePedidoItemLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem uma tabela de todos os campos de estoquepedidoitem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoquePedidoItem");
				
			try{
				if (this.Primeiro()){
					// Criando DataTable
					tabela.Columns.Add("Estoque", typeof(string));
					tabela.Columns.Add("Produto", typeof(string));
					tabela.Columns.Add("PrecoCompra", typeof(decimal));
					tabela.Columns.Add("Quantidade", typeof(decimal));
					// Atribuindo dados ao DataTable
					do{
						DataRow linha = tabela.NewRow();
						// Obtendo EstoqueItemID para obter Produto e Estoque
						EstoqueItem eItem = new EstoqueItem();
						eItem.Ler(estoquePedidoItem.EstoqueItemID.Valor);
						// Produto
						Produto p = new Produto();
						p.Ler(eItem.ProdutoID.Valor);
						linha["Produto"]= p.Nome.Valor;
						// Estoque
						Estoque e = new Estoque();
						e.Ler(eItem.EstoqueID.Valor);
						linha["Estoque"]= e.Nome.Valor;
						// Outrso campos do relatohrio
						linha["PrecoCompra"]= estoquePedidoItem.PrecoCompra.Valor;
						linha["Quantidade"]= estoquePedidoItem.Quantidade.Valor;
						// Adicionando a linha ao DataTable
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
