/**************************************************
* Arquivo: EstoqueItem.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;
using System.Diagnostics;

namespace IRLib.Paralela {

	public class EstoqueItem : EstoqueItem_B {
	
		int usuarioIDLogado;

		public EstoqueItem() {}
	
		public EstoqueItem(int usuarioIDLogado) : base (usuarioIDLogado){
			this.usuarioIDLogado = usuarioIDLogado;
		}

		/// <summary>		
		/// Atualizar saldo atual desse item de estoque
		/// </summary>
		/// <returns></returns>
		public override bool AtualizarSaldo(decimal qtde){
			
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

			bool ok = false;
			string sql ="";

			try{

				sql = "UPDATE tEstoqueItem SET SaldoAtual= SaldoAtual+("+qtde+") WHERE ID="+this.Control.ID;

				int x = bd.Executar(sql.ToString());
				bd.Fechar();
				ok = Convert.ToBoolean(x);

			}catch {
				Debug.WriteLine(sql);
				Debug.Fail("Arquivo: IRLIB.EstoquePedidoItem.cs" + "\tClasse: EstoquePedidoItem " + "\tMehtodo: AtualizarSaldo");
				ok = false;
			}
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
			return ok;
		}

		/// <summary>		
		/// Retorna o ID do EstoqueItem, caso nao haja cria um novo Item de Estoque
		/// </summary>
		/// <returns></returns>
		public override int DefinirID(){

			try{

				int id;

				string sql = "SELECT t.ID FROM tEstoqueItem as t, tProduto as p, tEstoque as e WHERE "+
					"e.ID=t.EstoqueID AND p.ID=t.ProdutoID AND e.ID="+this.EstoqueID.Valor+" AND p.ID="+this.ProdutoID.Valor;
				
				object resp = bd.ConsultaValor(sql);

				bd.Fechar();

				bool ok = (resp!=null);

				if (ok){

					id = Convert.ToInt32(resp);
					this.Ler(id);

				}else{ //EstoqueItem nao existe, vamos criar uma.

					this.SaldoAtual.Valor = 0;
					this.Obs.Valor = "";
					this.Control.UsuarioID = usuarioIDLogado;
					this.Inserir();
					id = this.Control.ID;

				}

				return id;

			}catch(Exception ex){
				throw ex;
			}

		}

	}

	public class EstoqueItemLista : EstoqueItemLista_B {
	
		public EstoqueItemLista() {}
	
		public EstoqueItemLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoqueitem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoqueItem");
				
			try{
			
				if (this.Primeiro()){
				
					//tabela.Columns.Add("Estoque", typeof(string));
					tabela.Columns.Add("Produto", typeof(string));
					tabela.Columns.Add("SaldoAtual", typeof(decimal));

					do{
						DataRow linha = tabela.NewRow();
//						Estoque estoque = new Estoque();
//						estoque.Ler(estoqueitem.EstoqueID.Valor);
//						linha["Estoque"]= estoque.Nome.Valor;
						Produto produto = new Produto();
						produto.Ler(estoqueItem.ProdutoID.Valor);
						linha["Produto"]= produto.Nome.Valor;
						linha["SaldoAtual"]= estoqueItem.SaldoAtual.Valor;
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
