/**************************************************
* Arquivo: EstoqueAjuste.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Diagnostics;

namespace IRLib {

	public class EstoqueAjuste : EstoqueAjuste_B	{
	
		public EstoqueAjuste() {}
	
		public EstoqueAjuste(int usuarioIDLogado) : base (usuarioIDLogado){}

		public override bool Excluir(){
			bool ok1, ok2 = false;
			ok1 = excluirItens();
			if (ok1) //soh exclui o estoque se tiver excluido todos os seus itens.
				ok2 = base.Excluir();
			return ok2;
		}

		private bool excluirItens(){
			bool ok;
			EstoqueAjusteItemLista itens = new EstoqueAjusteItemLista();
			itens.FiltroSQL = "EstoqueAjusteID = " + this.Control.ID;
			itens.Carregar();
			itens.Ultimo();
			//fazer varredura de traz pra frente.
			do
				ok = itens.Excluir();
			while (ok && itens.Anterior());
			return ok;
		} 

		/// <summary>		
		/// Retorna uma lista de itens do ajuste
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
				tabela.Columns.Add("MotivoID", typeof(int));		
				tabela.Columns.Add("EstoqueNome", typeof(string));
				tabela.Columns.Add("ProdutoNome", typeof(string));
				tabela.Columns.Add("MotivoNome", typeof(string));		
				tabela.Columns.Add("Quantidade", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));
				// Atribuindo os dados para o objeto
				BD bd = new BD();
				string sql;
				// Criar uma tabela resultante temporaria
				sql = "SELECT tEstoque.Nome AS EstoqueNome, tProduto.Nome AS ProdutoNome, tEstoqueAjusteItem.EstoqueItemID, tEstoqueAjusteMotivo.Nome AS MotivoNome, " + 
                     "	tEstoqueAjusteItem.EstoqueAjusteMotivoID, tEstoqueAjusteItem.Quantidade, tEstoqueAjusteItem.Obs, tEstoqueAjusteItem.ID, "+ 
							"  tEstoqueItem.EstoqueID, tEstoqueItem.ProdutoID " +
							"	FROM tEstoqueAjusteItem INNER JOIN tEstoqueAjusteMotivo ON tEstoqueAjusteItem.EstoqueAjusteMotivoID = tEstoqueAjusteMotivo.ID " +
							"									INNER JOIN tEstoqueItem ON tEstoqueAjusteItem.EstoqueItemID = tEstoqueItem.ID INNER JOIN tProduto ON tEstoqueItem.ProdutoID = tProduto.ID " +
							"									INNER JOIN tEstoque ON tEstoqueItem.EstoqueID = tEstoque.ID " +
							" WHERE tEstoqueAjusteItem.EstoqueAjusteID = " + this.Control.ID;
				bd.Consulta(sql);
				// Alimentar os registros no Objeto-tabela a partir da tabela temporaria
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["EstoqueItemID"] = bd.LerInt("EstoqueItemID");					
					linha["ProdutoID"] = bd.LerInt("ProdutoID");					
					linha["EstoqueID"] = bd.LerInt("EstoqueID");					
					linha["EstoqueNome"] = bd.LerString("EstoqueNome");
					linha["ProdutoNome"] = bd.LerString("ProdutoNome");
					linha["MotivoNome"] = bd.LerString("MotivoNome");
					linha["MotivoID"] = bd.LerInt("EstoqueAjusteMotivoID");
					linha["Quantidade"] = bd.LerDecimal("Quantidade");
					linha["Obs"] = bd.LerString("Obs");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch {
				Debug.Fail("Arquivo: EstoqueAjuste.cs" + "\tClasse: EstoqueAjuste " + "\tMehtodo: Itens");
				tabela = null;
			}
			// retornar o objeto-tabela
			return tabela;
		} // fim de itens

	} 

	public class EstoqueAjusteLista : EstoqueAjusteLista_B {

		public EstoqueAjusteLista() {}
	
		public EstoqueAjusteLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoqueajuste carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoqueAjuste");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("Responsavel", typeof(string));
					tabela.Columns.Add("Data", typeof(DateTime));

					do{
						DataRow linha = tabela.NewRow();
						linha["Ordem"]= estoqueAjuste.Ordem.Valor;
						Usuario usuario = new Usuario();
						usuario.Ler(estoqueAjuste.ResponsavelID.Valor);
						linha["Responsavel"]= usuario.Nome.Valor;
						linha["Data"]= estoqueAjuste.Data.Valor;
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
