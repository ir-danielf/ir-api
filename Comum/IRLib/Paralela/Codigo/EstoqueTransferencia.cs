/**************************************************
* Arquivo: EstoqueTransferencia.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;
using System.Text;

namespace IRLib.Paralela {

	public class EstoqueTransferencia : EstoqueTransferencia_B	{
	
		public EstoqueTransferencia() {}
	
		public EstoqueTransferencia(int usuarioIDLogado) : base (usuarioIDLogado){}

		public override bool Excluir(){
			bool ok1, ok2 = false;
			ok1 = excluirItens();
			if (ok1) //soh exclui o estoque se tiver excluido todos os seus itens.
				ok2 = base.Excluir();
			return ok2;
		}

		private bool excluirItens(){
			bool ok;
			EstoqueTransferenciaItemLista itens = new EstoqueTransferenciaItemLista();
			itens.FiltroSQL = "EstoqueTransferenciaID = " + this.Control.ID;
			itens.Carregar();
			itens.Ultimo();
			//fazer varredura de traz pra frente.
			do
				ok = itens.Excluir();
			while (ok && itens.Anterior());
			return ok;
		} 

		/// <summary>		
		/// Obter itens dessa transferencia
		/// </summary>
		/// <returns></returns>
		public override DataTable Itens(){

			try{

				DataTable tabela = new DataTable("EstoqueTransferenciaItem");

				DataTable tmp = new DataTable(); //tabela temporaria para conter apenas o estoqueDE
				tmp.Columns.Add("EstoqueDeID", typeof(int));
				tmp.Columns.Add("EstoqueDe", typeof(string));

				//Select para pegar o item DE
				StringBuilder sqlEstoqueDe = new StringBuilder();

				sqlEstoqueDe.Append("SELECT ");
				sqlEstoqueDe.Append("e.ID as EstoqueDeID,");
				sqlEstoqueDe.Append("e.Nome as EstoqueDe");
				sqlEstoqueDe.Append(" FROM ");
				sqlEstoqueDe.Append("tEstoque as e,");
				sqlEstoqueDe.Append("tEstoqueTransferenciaItem as t,");
				sqlEstoqueDe.Append("tEstoqueItem as i");
				sqlEstoqueDe.Append(" WHERE ");
				sqlEstoqueDe.Append("t.EstoqueTransferenciaID="+this.Control.ID+" AND t.EstoqueItemDeID=i.ID AND i.EstoqueID=e.ID");

				bd.Consulta(sqlEstoqueDe);

				while(bd.Consulta().Read()){
					DataRow linha = tmp.NewRow();
					linha["EstoqueDeID"]= bd.LerInt("EstoqueDeID");
					linha["EstoqueDe"] = bd.LerString("EstoqueDe");
					tmp.Rows.Add(linha);
				}
				bd.Fechar();

				//Select para pegar o item PARA
				StringBuilder sqlEstoqueItemPara = new StringBuilder();

				sqlEstoqueItemPara.Append("SELECT ");

				sqlEstoqueItemPara.Append("t.ID,");
				sqlEstoqueItemPara.Append("t.EstoqueItemDeID,");
				sqlEstoqueItemPara.Append("t.EstoqueItemParaID,");
				sqlEstoqueItemPara.Append("p.ID as ProdutoID,");
				sqlEstoqueItemPara.Append("e.ID as EstoqueParaID,");
				sqlEstoqueItemPara.Append("p.Nome as Produto,");
				sqlEstoqueItemPara.Append("e.Nome as EstoquePara,");
				sqlEstoqueItemPara.Append("t.Quantidade,");
				sqlEstoqueItemPara.Append("t.Obs");
				sqlEstoqueItemPara.Append(" FROM ");
				sqlEstoqueItemPara.Append("tEstoque as e,");
				sqlEstoqueItemPara.Append("tProduto as p,");
				sqlEstoqueItemPara.Append("tEstoqueTransferenciaItem as t,");
				sqlEstoqueItemPara.Append("tEstoqueItem as i");
				sqlEstoqueItemPara.Append(" WHERE ");
				sqlEstoqueItemPara.Append("t.EstoqueTransferenciaID="+this.Control.ID+" AND t.EstoqueItemParaID=i.ID AND i.ProdutoID=p.ID AND i.EstoqueID=e.ID");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EstoqueTransferenciaID", typeof(int));
				tabela.Columns.Add("EstoqueItemDeID", typeof(int));
				tabela.Columns.Add("EstoqueItemParaID", typeof(int));
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("EstoqueDeID", typeof(int));
				tabela.Columns.Add("EstoqueParaID", typeof(int));
				tabela.Columns.Add("Estoque De", typeof(string));
				tabela.Columns.Add("Estoque Para", typeof(string));
				tabela.Columns.Add("Produto", typeof(string));				
				tabela.Columns.Add("Quantidade", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));

				bd.Consulta(sqlEstoqueItemPara);

				int i=0;
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["EstoqueTransferenciaID"] = this.Control.ID;
					linha["EstoqueItemDeID"] = bd.LerInt("EstoqueItemDeID");
					linha["EstoqueItemParaID"] = bd.LerInt("EstoqueItemParaID");
					linha["ProdutoID"] = bd.LerInt("ProdutoID");
					linha["EstoqueDeID"] = tmp.Rows[i]["EstoqueDeID"]; //coloca o ID do estoqueDE apartir da tabela tmp
					linha["EstoqueParaID"] = bd.LerInt("EstoqueParaID");
					linha["Estoque De"] = tmp.Rows[i]["EstoqueDe"]; //coloca o estoqueDE apartir da tabela tmp
					linha["Estoque Para"] = bd.LerString("EstoquePara");
					linha["Produto"] = bd.LerString("Produto");
					linha["Quantidade"] = bd.LerDecimal("Quantidade");
					linha["Obs"] = bd.LerString("Obs");
					tabela.Rows.Add(linha);
					i++;
				}
				bd.Fechar();

				return tabela;


			}catch(Exception ex){
				throw ex;
			}

		}

	} 

	public class EstoqueTransferenciaLista : EstoqueTransferenciaLista_B {
	
		public EstoqueTransferenciaLista() {}
	
		public EstoqueTransferenciaLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoquetransferencia carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoqueTransferencia");
				
			try{
			
				if (this.Primeiro()){

					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("Data", typeof(DateTime));
					tabela.Columns.Add("Responsavel", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["Ordem"]= estoqueTransferencia.Ordem.Valor;
						linha["Data"]= estoqueTransferencia.Data.Valor;
						Usuario usuario = new Usuario();
						usuario.Ler(estoqueTransferencia.ResponsavelID.Valor);
						linha["Responsavel"]= usuario.Nome.Valor;
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
