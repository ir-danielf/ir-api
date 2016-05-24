/**************************************************
* Arquivo: EstoqueTransferenciaItem.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela {

	public class EstoqueTransferenciaItem : EstoqueTransferenciaItem_B {
	
		public EstoqueTransferenciaItem() {}
	
		public EstoqueTransferenciaItem(int usuarioIDLogado) : base (usuarioIDLogado){}
		/// <summary>
		/// Sobrecarga de Inserir para atualizar saldo no item de estoque
		/// </summary>
		/// <returns></returns>
		public override bool Inserir(){
			bool ok;
			try {
				ok = base.Inserir();
				if (ok) {
					// Estoque De
					EstoqueItem estoqueItemDe = new EstoqueItem();
					estoqueItemDe.Control.ID = this.EstoqueItemDeID.Valor;
					decimal inverteSinal = -1 * this.Quantidade.Valor;
					estoqueItemDe.AtualizarSaldo(inverteSinal);
					// Estoque Para
					EstoqueItem estoqueItemPara = new EstoqueItem();
					estoqueItemPara.Control.ID = this.EstoqueItemParaID.Valor;
					estoqueItemPara.AtualizarSaldo(this.Quantidade.Valor);
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
					decimal inverteSinal; 
					// === Para Antes da alteracao
					// Para Estoque Para
					EstoqueItem estoqueItemParaAntes = new EstoqueItem(); 
					estoqueItemParaAntes.Control.ID = this.EstoqueItemParaID.ValorAntigo;	// Usado no AtualizarSaldo
					inverteSinal = -1 * this.Quantidade.ValorAntigo;
					estoqueItemParaAntes.AtualizarSaldo(inverteSinal);
					// Para Estoque De
					EstoqueItem estoqueItemDeAntes = new EstoqueItem(); 
					estoqueItemDeAntes.Control.ID = this.EstoqueItemDeID.ValorAntigo;	// Usado no AtualizarSaldo
					estoqueItemDeAntes.AtualizarSaldo(this.Quantidade.ValorAntigo);
					// === Para Depois da alteracao
					// Para Estoque Para
					EstoqueItem estoqueItemParaDepois = new EstoqueItem(); 
					estoqueItemParaDepois.Control.ID = this.EstoqueItemParaID.Valor;	// Usado no AtualizarSaldo
					estoqueItemParaDepois.AtualizarSaldo(this.Quantidade.Valor);
					// Para Estoque Para
					EstoqueItem estoqueItemDeDepois = new EstoqueItem(); 
					estoqueItemDeDepois.Control.ID = this.EstoqueItemDeID.Valor;	// Usado no AtualizarSaldo
					inverteSinal = -1 * this.Quantidade.Valor;
					estoqueItemDeDepois.AtualizarSaldo(inverteSinal);
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
					// Estoque De
					EstoqueItem estoqueItemDe = new EstoqueItem();
					estoqueItemDe.Control.ID = this.EstoqueItemDeID.Valor;
					estoqueItemDe.AtualizarSaldo(this.Quantidade.Valor);
					// Estoque Para
					EstoqueItem estoqueItemPara = new EstoqueItem();
					estoqueItemPara.Control.ID = this.EstoqueItemParaID.Valor;
					decimal inverteSinal = -1 * this.Quantidade.Valor;
					estoqueItemPara.AtualizarSaldo(inverteSinal);
				}
			} catch {
				ok = false;
			}
			return ok;
		}

	}

	public class EstoqueTransferenciaItemLista : EstoqueTransferenciaItemLista_B {
	
		public EstoqueTransferenciaItemLista() {}
	
		public EstoqueTransferenciaItemLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoquetransferenciaitem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoqueTransferenciaItem");
				
			try{
			
				if (this.Primeiro()){
				
					//tabela.Columns.Add("OrdemTransferencia", typeof(int));
					tabela.Columns.Add("EstoqueDe", typeof(string));
					tabela.Columns.Add("EstoquePara", typeof(string));
					tabela.Columns.Add("Produto", typeof(string));
					tabela.Columns.Add("Quantidade", typeof(decimal));

					do{
						DataRow linha = tabela.NewRow();
//						EstoqueTransferencia t = new EstoqueTransferencia();
//						t.Ler(estoquetransferenciaitem.EstoqueTransferenciaID.Valor);
//						linha["OrdemTransferencia"]= t.Ordem.Valor;

						EstoqueItem eItem = new EstoqueItem();
						eItem.Ler(estoqueTransferenciaItem.EstoqueItemDeID.Valor);
						
						Produto p = new Produto();
						p.Ler(eItem.ProdutoID.Valor);
						linha["Produto"]= p.Nome.Valor;

						Estoque e = new Estoque();
						e.Ler(eItem.EstoqueID.Valor);

						linha["EstoqueDe"]= e.Nome.Valor;

						eItem = new EstoqueItem();
						eItem.Ler(estoqueTransferenciaItem.EstoqueItemParaID.Valor);
						e = new Estoque();
						e.Ler(eItem.EstoqueID.Valor);

						linha["EstoquePara"]= e.Nome.Valor;

						linha["Quantidade"]= estoqueTransferenciaItem.Quantidade.Valor;
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
