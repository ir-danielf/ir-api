/**************************************************
* Arquivo: EstoqueAjusteItem.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela {

	public class EstoqueAjusteItem : EstoqueAjusteItem_B{

		public EstoqueAjusteItem() {}
	
		public EstoqueAjusteItem(int usuarioIDLogado) : base (usuarioIDLogado){}
	
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
					// atualizando 
					estoqueItem.AtualizarSaldo(this.Quantidade.Valor);
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
					decimal inverteSinal = -1 * this.Quantidade.ValorAntigo;
					//	Atualiza no saldo, tirando a Quantidade antiga no antigo estoque e/ou produto
					estoqueItemAntes.AtualizarSaldo(inverteSinal);

					// Para o novo produto, estoque, e quantidade, inclua no saldo do novo EstoqueItemID
					EstoqueItem estoqueItemDepois = new EstoqueItem(); 
					estoqueItemDepois.Control.ID = this.EstoqueItemID.Valor;	// Pega EstoqueItemID para ser procurado
					//	Atualiza no saldo, somando a Quantidade no novo estoque e/ou produto
					estoqueItemDepois.AtualizarSaldo(this.Quantidade.Valor);
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
					decimal inverteSinal = -1 * this.Quantidade.Valor;
					// atualizando 
					estoqueItem.AtualizarSaldo(inverteSinal);
				}
			} catch {
				ok = false;
			}
			return ok;
		}


	}

	public class EstoqueAjusteItemLista : EstoqueAjusteItemLista_B {
	
		public EstoqueAjusteItemLista() {}
	
		public EstoqueAjusteItemLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoqueajusteitem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){
			DataTable tabela = new DataTable("EstoqueAjusteItem");
			try{
				if (this.Primeiro()){
					// Criando DataTable
					tabela.Columns.Add("Estoque", typeof(string));
					tabela.Columns.Add("Produto", typeof(string));
					tabela.Columns.Add("Quantidade", typeof(decimal));
					tabela.Columns.Add("Motivo", typeof(string));
					// Atribuindo dados ao DataTable
					do{
						DataRow linha = tabela.NewRow();
						// Obtendo EstoqueItemID para obter Produto e Estoque
						EstoqueItem eItem = new EstoqueItem();
						eItem.Ler(estoqueAjusteItem.EstoqueItemID.Valor);
						// Produto
						Produto p = new Produto();
						p.Ler(eItem.ProdutoID.Valor);
						linha["Produto"]= p.Nome.Valor;
						// Estoque
						Estoque e = new Estoque();
						e.Ler(eItem.EstoqueID.Valor);
						linha["Estoque"]= e.Nome.Valor;
						//
						linha["Quantidade"]= estoqueAjusteItem.Quantidade.Valor;
						EstoqueAjusteMotivo eam = new EstoqueAjusteMotivo();
						eam.Ler(estoqueAjusteItem.EstoqueAjusteMotivoID.Valor);
						linha["Motivo"]= eam.Nome.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());
				}else{ //erro: nao carregou a lista
					tabela = null;
				}
			}catch{
				tabela = null;
			}			
			return tabela;
		}	// fim do mehtodo
		
	}
	
}
