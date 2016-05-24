/**************************************************
* Arquivo: Comanda.cs
* Gerado: 12/05/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Diagnostics;

namespace IRLib.Paralela{

	public class Comanda : Comanda_B{
	
		public Comanda() {}
	
		public Comanda(int usuarioIDLogado) : base (usuarioIDLogado){}

		public override bool Excluir(){

			bool ok = false;

			excluirItens();

			ok = base.Excluir();

			return ok;

		}

		private void excluirItens(){

			bool ok;

			ComandaItemLista itens = new ComandaItemLista();

			itens.FiltroSQL = "ComandaID = " + this.Control.ID;
			itens.Carregar();
			itens.Ultimo();

			//fazer varredura de traz pra frente.
			do
				ok = itens.Excluir();
			while (ok && itens.Anterior());

		} 

		/// <summary>		
		/// Cria um Datatable com os campos de Itens de Comanda
		/// </summary>
		/// <returns></returns>
		public override DataTable estruturaItensComanda(){
			DataTable tabela = new DataTable("ItensComanda");
			try {
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("Produto", typeof(string));
				tabela.Columns.Add("Quantidade", typeof(int));
				tabela.Columns.Add("Valor", typeof(decimal));
			}catch{
				tabela = null;
			}
			return tabela;
		}
		/// <summary>		
		/// Obter itens dessa comanda pela Ordem
		/// É importante que IngressoID seja igual a zero
		/// </summary>
		/// <returns></returns>
		public override DataTable Itens(string ordem){
			DataTable tabela = estruturaItensComanda();
			try{
				BD bd = new BD();
				string sql = 
					"SELECT     tComandaItem.ID, tComandaItem.ProdutoID, tProduto.Nome AS Produto, tComandaItem.PrecoVenda AS Valor, tComandaItem.Quantidade, tComanda.IngressoID "+
					"FROM       tComanda INNER JOIN "+
									"tComandaItem ON tComanda.ID = tComandaItem.ComandaID INNER JOIN "+
									"tProduto ON tComandaItem.ProdutoID = tProduto.ID INNER JOIN "+
									"tIngresso ON tComanda.IngressoID = tIngresso.ID "+
					"WHERE      (tComanda.Ordem = '"+ ordem +"') ";
				bd.Consulta(sql);
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["ProdutoID"]= bd.LerInt("ProdutoID");
					linha["Produto"]= bd.LerString("Produto");
					linha["Quantidade"]= bd.LerInt("Quantidade");
					linha["Valor"]= bd.LerDecimal("Valor");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch{
				Debug.Fail("Falha ao obter itens da Comanda pela Ordem!!");
			}
			return tabela;
		}
		/// <summary>		
		/// Obter itens dessa comanda
		/// </summary>
		/// <returns></returns>
		public override DataTable Itens(){
			DataTable tabela = estruturaItensComanda();
			try{
				BD bd = new BD();
				string sql = "SELECT i.ID, i.ProdutoID, p.Nome as Produto, p.PrecoVenda as Valor, i.Quantidade "+
					"FROM tComanda as c, tComandaItem as i, tProduto as p WHERE i.ComandaID="+this.Control.ID+" AND "+
					"p.ID=i.ProdutoID AND c.ID=i.ComandaID "+
					"ORDER BY c.Ordem";
				bd.Consulta(sql);
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["ProdutoID"]= bd.LerInt("ProdutoID");
					linha["Produto"]= bd.LerString("Produto");
					linha["Quantidade"]= bd.LerInt("Quantidade");
					linha["Valor"]= bd.LerDecimal("Valor");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch{
				tabela = null;
			}
			return tabela;
		}
		/// <summary>		
		/// Obtem uma tabela com os itens de uma comanda (com valor)
		/// </summary>
		/// <returns></returns>
		public override DataTable Itens(int garconid, int apresentacaoid){

			DataTable tabela = new DataTable("");
			try{
				// Criar DataTable com as colunas
				tabela.Columns.Add("Comanda", typeof(string));
				tabela.Columns.Add("Ingresso", typeof(string));
				tabela.Columns.Add("Valor", typeof(decimal));
				//				tabela.Columns.Add("Apresentacao", typeof(string));
				//				tabela.Columns.Add("Garcon", typeof(string));
				//				tabela.Columns.Add("Comissao", typeof(decimal));
				// Obtendo dados atravehs de SQL
				BD bd = new BD();
				string sql = "SELECT tComanda.Ordem, tComanda.Ingresso, tApresentacao.Horario AS Apresentacao, tGarcon.Nome AS Garcon, tGarcon.Comissao, "+
					"SUM(tComandaItem.PrecoVenda * tComandaItem.Quantidade) AS Valor "+
					"FROM tComanda INNER JOIN tVenda ON tComanda.VendaID = tVenda.ID " + 
					"INNER JOIN tCaixa ON tVenda.CaixaID = tCaixa.ID " + 
					"INNER JOIN tApresentacao ON tCaixa.ApresentacaoID = tApresentacao.ID " + 
					"INNER JOIN tGarcon ON tComanda.GarconID = tGarcon.ID " + 
					"INNER JOIN tComandaItem ON tComanda.ID = tComandaItem.ComandaID " + 
					"WHERE     (tGarcon.ID = "+ garconid +") AND (tApresentacao.ID ="+ apresentacaoid +")" + 
					"GROUP BY tComanda.Ordem, tComanda.Ingresso, tApresentacao.Horario, tGarcon.Nome, tGarcon.Comissao";

				bd.Consulta(sql);
				// Alimentando DataTable
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["Comanda"]= bd.LerString("Ordem");
					linha["Ingresso"]= bd.LerString("Ingresso");
					linha["Valor"]= bd.LerDecimal("Valor");
					//					linha["Apresentacao"]= bd.LerString("Apresentacao");
					//					linha["Garcon"]= bd.LerString("Garcon");
					//					linha["Comissao"]= bd.LerDecimal("Comissao");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch{		
				tabela = null;
			}			
			// retorna DataTable
			return tabela;

		}

		/// <summary>		
		/// Obter vendaID de um determinado ingresso
		/// </summary>
		/// <returns></returns>
		public override int ObterVendaID(string ingresso){
	
			try{

				string sql = "SELECT VendaID FROM tComanda WHERE Ingresso='"+ingresso+"'";

				object obj = bd.ConsultaValor(sql);

				int vendaID = (obj!=null) ? Convert.ToInt32(obj) : 0;

				return vendaID;

			}catch(Exception ex){
				throw ex;
			}

		}

		/// <summary>		
		/// Obter ordens de comanda dado um ingresso
		/// </summary>
		/// <returns></returns>
		public override DataTable Ordens(string ingresso){
			DataTable tabela = new DataTable("Comanda");
			try{
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));
				// Lendo do servidor
				BD bd = new BD();
				string sql = "SELECT ID, Ordem FROM tComanda WHERE Ingresso='"+ingresso+"' ORDER BY Ordem";
				bd.Consulta(sql);
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Ordem"]= bd.LerString("Ordem");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch{
				Debug.Fail("Falha ao obter as Comandas deste Ingresso " + ingresso+" !!");
			}
			return tabela;
		}
		
		/// <summary>		
		/// Obter ordens de comanda dado um garcon
		/// </summary>
		/// <returns></returns>
		public override DataTable Ordens(int garconid){

			try{

				DataTable tabela = new DataTable("Comanda");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));

				string sql = "SELECT ID, Ordem FROM tComanda WHERE GarconID="+garconid+" ORDER BY Ordem";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Ordem"]= bd.LerString("Ordem");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}

		/// <summary>		
		/// Obter o valor total da conta do ingresso
		/// </summary>
		/// <returns></returns>
		public override decimal ValorConta(int ingresso){

			try{

				DataTable tabela = new DataTable("Conta");

				//carregar uma lista de comandas desse ingresso
				ComandaLista lista = new ComandaLista();
				lista.FiltroSQL = "VendaID<>0 AND IngressoID="+ingresso ;
				lista.Carregar();

				string ids = lista.ToString();

				if (ids!=""){

					string sql = "SELECT Sum(i.PrecoVenda*i.Quantidade) as Valor "+
						"FROM tComanda as c, tComandaItem as i WHERE i.ComandaID in ("+ids+") AND "+
						"c.ID=i.ComandaID";

					object o = bd.ConsultaValor(sql);

					decimal valor = (o!=null) ? Convert.ToDecimal(o) : 0;

					return valor;

				}else{
					return 0;
				}

			}catch(Exception ex){
				throw ex;
			}

		}

		/// <summary>		
		/// Obter o valor total da conta por comanda
		/// </summary>
		/// <returns></returns>
		public override decimal ValorConta(){

			try{

				decimal valor = 0;
				
				string sql = "SELECT tComanda.Ordem, SUM(tComandaItem.PrecoVenda * tComandaItem.Quantidade) AS Valor " +
					"FROM tComanda INNER JOIN tComandaItem ON tComanda.ID = tComandaItem.ComandaID " +
					"GROUP BY tComanda.Ordem HAVING " +
					"(tComanda.Ordem = '"+ this.Ordem.Valor.ToString() +"') ";

				bd.Consulta(sql);

				if (bd.Consulta().Read())
					valor = bd.LerDecimal("Valor");

				return valor;

			}catch(Exception ex){
				throw ex;
			}

		}

		/// <summary>		
		/// Fecha a conta
		/// </summary>
		/// <returns></returns>
		public override void FecharConta(int ingresso, int vendaid){

			if (vendaid > 0){ // fechar conta setando o vendaID de todas as comandas do ingresso

				try{
					//carregar uma lista de comandas desse ingresso
					ComandaLista lista = new ComandaLista();
					lista.FiltroSQL = "IngressoID='"+ingresso+"' AND VendaID=0";
					lista.Carregar();		
					lista.Primeiro();

					do{
						lista.Comanda.VendaID.Valor = vendaid;
						lista.Comanda.Atualizar();

					}while(lista.Proximo());

				}catch(Exception ex){
					throw ex;
				}

			}

		}


		/// <summary>		
		/// Obter os itens de um Ingresso especifico nao pagos
		/// </summary>
		/// <returns></returns>
		public DataTable Conta(int ingresso){
			DataTable tabela = estruturaItensComanda();			//carregar uma lista de comandas desse ingresso
			ComandaLista lista = new ComandaLista();
			lista.FiltroSQL = "IngressoID='"+ingresso +"'";
			lista.Carregar();
			string ids = lista.ToString();
			if (ids!=""){
				try{
					BD bd = new BD();
					string sql = 
						"SELECT     i.ID, i.ProdutoID, p.Nome AS Produto, p.PrecoVenda AS Valor, i.Quantidade, i.ComandaID, c.VendaID "+
						"FROM       tProduto p INNER JOIN "+
										"tComandaItem i ON p.ID = i.ProdutoID INNER JOIN "+
										"tComanda c ON i.ComandaID = c.ID "+
						"WHERE     (i.ComandaID IN ("+ids+")) AND (c.VendaID = 0)";
					bd.Consulta(sql);
					while(bd.Consulta().Read()){
						DataRow linha = tabela.NewRow();
						linha["ID"]= bd.LerInt("ID");
						linha["ProdutoID"]= bd.LerInt("ProdutoID");
						linha["Produto"]= bd.LerString("Produto");
						linha["Quantidade"]= bd.LerInt("Quantidade");
						linha["Valor"]= bd.LerDecimal("Valor");
						tabela.Rows.Add(linha);
					}
					bd.Fechar();
				}catch{
					tabela = null;
				}
			}
			return tabela;
		}

		public bool ExisteOrdem(string ordem){
			bool existe= false;
			try{
				BD bd = new BD();
				string sql = "SELECT Ordem FROM tComanda WHERE Ordem="+ ordem;
				bd.Consulta(sql);
				if (bd.Consulta().Read()){
					if (ordem == bd.LerString("Ordem")){
						existe= true;
					}else {
						existe= false;
					}
				}
				bd.Fechar();
			}catch{
				Debug.Fail("Falha ao Pesquisar Ordem existente!!");
			}
			return existe;
		} // fim de ExisteOrdem
		/// <summary>
		/// Ultima Comanda
		/// </summary>
		public override int Ultima(){
			int ultimo = -1;
			try{
				BD bd = new BD();
				string sql = 
					"SELECT      tComanda.IngressoID, tComanda.Ordem, tComanda.ID "+
					"FROM        cComanda INNER JOIN "+
									"tComanda ON cComanda.ID = tComanda.ID "+
					"WHERE        (cComanda.Acao = 'I') "+
					"ORDER BY cComanda.ID DESC";
				bd.Consulta(sql);
				if (bd.Consulta().Read()){
					ultimo = Convert.ToInt32(bd.LerString("Ordem"));
				} else {
					ultimo = -1;
				}
				bd.Fechar();
			}catch(Exception erro){		
				throw erro;
			} // fim de try			
			return ultimo;
		} // fim de ExisteOrdem
		/// <summary>
		/// Obtendo um DataTable das Comandas de um Ingresso
		/// Este método é temporário até se criar o relacionamento entre as tabelas
		/// </summary>
		/// <param name="registroZero">Um string que vai conter a descrição do registro zero, se for vazio não insere</param>
		/// <returns>DataTable de Comanda com ID e Nome</returns>
		public DataTable Comandas(string registroZero){
			// 
			DataTable tabela = new DataTable("Comandas");
			try{
				// Criar DataTable com as colunas
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));
				// Obtendo dados através de SQL
				//BD bd = new BD();
				string sql = "SELECT ID,Ordem FROM tComanda WHERE IngressoID= '"+this.IngressoID.Valor+"' ORDER BY Ordem";
				bd.Consulta(sql);
				// Alimentando DataTable
				if (registroZero!=null)
					tabela.Rows.Add(new Object[] {0, registroZero});
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Ordem"]= bd.LerString("Ordem");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch(Exception erro){		
				throw erro;
			} // fim de try			
			return tabela;
		} // fim de Comandas


	}

	public class ComandaLista : ComandaLista_B{
	
		public ComandaLista() {}
	
		public ComandaLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem uma tabela de todos os campos de estoqueitem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){

			try{

				DataTable tabela = new DataTable("Comanda");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));

				if (this.Primeiro()){
					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= comanda.Control.ID;
						linha["Ordem"]= comanda.Ordem.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());
				}
			
				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}

	}

} 

	

