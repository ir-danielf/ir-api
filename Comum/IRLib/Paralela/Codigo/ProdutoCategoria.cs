/**************************************************
* Arquivo: ProdutoCategoria.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela{

	public class ProdutoCategoria : ProdutoCategoria_B{
	
		public ProdutoCategoria(){}
	
		public ProdutoCategoria(int usuarioIDLogado) : base (usuarioIDLogado){}
		

	}

	public class ProdutoCategoriaLista : ProdutoCategoriaLista_B{
	
		public ProdutoCategoriaLista(){}
	
		public ProdutoCategoriaLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de produtocategoria carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("ProdutoCategoria");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("Descricao", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["Nome"]= produtoCategoria.Nome.Valor;
						linha["Descricao"]= produtoCategoria.Descricao.Valor;
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
