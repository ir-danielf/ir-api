using System;
using System.Data;

namespace IRLib.Paralela {
	/// <summary>
	/// Summary description for VendaAeB.
	/// </summary>
	public class VendaAeB {
		private Empresa empresa; 
		private int usuarioID; 


		
		public VendaAeB(Empresa empresaObjeto, int usuarioIDLogado) {
			empresa = empresaObjeto;
			usuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataSet CarregarDataSet() {
			DataSet bancoDados = new DataSet("AeB");
			try {
				// Carregando a tabela FormaPagamento
				FormaPagamento formaPagamento = new FormaPagamento(usuarioID);
				DataTable formaPagamentoTabela = formaPagamento.Todas();
				formaPagamentoTabela.TableName = "FormaPagamentoTabela";
				bancoDados.Tables.Add(formaPagamentoTabela);
				// Carregando a tabela Garcon
				DataTable garconTabela = empresa.Garcons(true);
				garconTabela.TableName = "GarconTabela";
				bancoDados.Tables.Add(garconTabela);
			}catch{
				bancoDados = null;
			}
			return bancoDados;
		} // fim de CarregarDataSet

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataTable VendaItemEstruturaMemoria() {
			DataTable tabela = new DataTable("");
			try {
				tabela.Columns.Add("FormaPagamentoID", typeof(int));
				tabela.Columns.Add("Valor", typeof(decimal));
			}catch{
				tabela = null;
			}
			return tabela;
		} // fim de VendaItemEstruturaMemoria
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataTable ComandaItemEstruturaMemoria() {
			DataTable tabela = new DataTable("");
			try {
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("Quantidade", typeof(int));
			}catch{
				tabela = null;
			}
			return tabela;
		} // fim de ComandaItemEstruturaMemoria

		// Não usado, era tentativa de desempenho
		//		public int GravarComandaVazia(
		//			out int vendaID,
		//			bool trabalharComIngresso,
		//			int caixaID, 
		//			string ingresso, 
		//			string comandaOrdem, 
		//			int garconID, 
		//			int [,] comandaItemMatrizMemoria) {
		//
		//			vendaID = 0;
		////			int comandaID;
		//			Comanda comanda = new Comanda(usuarioID);
		//			// Checa se a Ordem da Comanda ja existe, antes de gravar
		//			if (comanda.ExisteOrdem(comandaOrdem))
		//				return -1;
		//			// Gravar na tabela Comanda
		//			comanda.CaixaID.Valor = caixaID;
		//			comanda.VendaID.Valor = 0;
		////			comanda.IngressoID.Valor = ingresso;
		//			comanda.Ordem.Valor = comandaOrdem;
		//			comanda.GarconID.Valor = garconID;
		////			comanda.Inserir();	// *****
		////			comandaID = comanda.Control.ID; // retornar ID da Comanda
		//			int total = 0;
		//			int tamanho = (int) comandaItemMatrizMemoria.Length/2; // 2 colunas
		//			for (int conta = 0; conta< tamanho; conta++) {
		//				// Gravar os itens
		//				int produtoID = Convert.ToInt32(comandaItemMatrizMemoria[conta,0]);
		//				int quantidade = Convert.ToInt32(comandaItemMatrizMemoria[conta,1]);
		//				ComandaItem comandaItem = new ComandaItem(usuarioID);
		////				comandaItem.ComandaID.Valor = comandaID;
		//				comandaItem.ProdutoID.Valor = produtoID;
		//				comandaItem.Quantidade.Valor = quantidade;
		//				//
		//				total = total + comandaItem.Quantidade.Valor;
		//			}
		//			return total;
		//		}



		public string GravarComandaPagamento(
			int vendaID,
			bool trabalharComIngresso,
			int caixaID, 
			int apresentacaoID,
			int setorID,
			string codigo, 
			int garconID, 
			int [,] comandaItemMatrizMemoria, 
			decimal [,] vendaItemMatrizMemoria) {
			//
			string comandaOrdem="";
			try {
				if (!trabalharComIngresso) {
					comandaOrdem =  GravarComanda(out vendaID, trabalharComIngresso, caixaID, apresentacaoID, setorID, codigo, garconID, comandaItemMatrizMemoria); // neste método cria registro de Venda para caso do Ticket
				}
				GravarPagamento(vendaID, vendaItemMatrizMemoria);
			}catch(Exception erro){
				throw erro;
			}
			return comandaOrdem;
		} // fim de GravarComandaPagamento
		
		
		/// <summary>		
		/// Gravar informações de Comanda e seus itens, antes de imprimir Comanda no Bar
		/// Usa como parâmetro Matriz em vez de DataTable
		/// </summary>
		/// <returns></returns>
		public string GravarComanda(
			out int vendaID,
			bool trabalharComIngresso,
			int caixaID, 
			int apresentacaoID,
			int setorID,
			string codigo, 
			int garconID, 
			int [,] comandaItemMatrizMemoria) {
			//

			int comandaID;
			Comanda comanda = new Comanda(usuarioID);
			try{
				vendaID = 0;
				// Gravar na tabela Comanda
				comanda.CaixaID.Valor = caixaID;
				comanda.VendaID.Valor = 0;
				Ingresso ingresso = new Ingresso();
				if (trabalharComIngresso) {
					comanda.IngressoID.Valor = IngressoID(apresentacaoID, setorID, codigo);
					comanda.GarconID.Valor = garconID;
				} else {
					comanda.IngressoID.Valor = 0;
					comanda.GarconID.Valor = 0;
				}
				int incremento = comanda.Ultima()+1;
				comanda.Ordem.Valor = incremento.ToString();
				comanda.Inserir();	// *****
				comandaID = comanda.Control.ID; // retornar ID da Comanda
				int tamanho = (int) comandaItemMatrizMemoria.Length/2; // 2 colunas
				for (int conta = 0; conta< tamanho; conta++) {
					// Gravar os itens
					int produtoID = Convert.ToInt32(comandaItemMatrizMemoria[conta,0]);
					int quantidade = Convert.ToInt32(comandaItemMatrizMemoria[conta,1]);
					ComandaItem comandaItem = new ComandaItem(usuarioID);
					comandaItem.ComandaID.Valor = comandaID;
					comandaItem.ProdutoID.Valor = produtoID;
					comandaItem.Quantidade.Valor = quantidade;
					// Pesquisar PrecoVenda
					Produto produto = new Produto();
					produto.Ler(produtoID);	// *****
					comandaItem.PrecoVenda.Valor = produto.PrecoVenda.Valor;
					// Inserir o item de comanda
					comandaItem.Inserir();	// *****
					// Atualizar quantidade no estoque para cada item de venda
					Caixa caixa = new Caixa(usuarioID);
					caixa.Ler(caixaID);	// *****
					Loja loja = new Loja(usuarioID);
					loja.Ler(caixa.LojaID.Valor);
					EstoqueItem estoqueItem = new EstoqueItem(usuarioID);
					estoqueItem.ProdutoID.Valor = produtoID;
					estoqueItem.EstoqueID.Valor = loja.EstoqueID.Valor;
					estoqueItem.DefinirID();	// *****
					estoqueItem.AtualizarSaldo(-1 * quantidade);	// *****
				}
				// Em sistema de Ticket, assim que imprimiu a comanda, salva a venda
				if (!trabalharComIngresso) { 
					// Chame um mehtodo que cria um registro de Venda e atualiza VendaID nesta Comanda 
					vendaID = InserirVenda(caixaID, comanda.IngressoID.Valor, comandaID);	// *****
				}
			}catch(Exception erro){
				throw erro;
			}
			return comanda.Ordem.Valor;
		} // fim de GravarComanda

		/// <summary>		
		/// Gravar informações de Comanda e seus itens, antes de imprimir Comanda no Bar
		/// </summary>
		/// <returns></returns>
		public string GravarComanda(
			out int vendaID,
			bool trabalharComIngresso,
			int caixaID, 
			int apresentacaoID,
			int setorID,
			string codigo, 
			int garconID, 
			DataTable comandaItemTabelaMemoria) {

			int comandaID;
			Comanda comanda = new Comanda(usuarioID);
			try{
				vendaID = 0;
				// Gravar na tabela Comanda
				comanda.CaixaID.Valor = caixaID;
				comanda.VendaID.Valor = 0;
				Ingresso ingresso = new Ingresso();
				if (trabalharComIngresso) {
					comanda.IngressoID.Valor = IngressoID(apresentacaoID, setorID, codigo);
					comanda.GarconID.Valor = garconID;
				} else {
					comanda.IngressoID.Valor = 0;
					comanda.GarconID.Valor = 0;
				}
				int incremento = comanda.Ultima()+1;
				comanda.Ordem.Valor = incremento.ToString();
				comanda.Inserir();	// *****
				comandaID = comanda.Control.ID; // retornar ID da Comanda
				foreach (DataRow linha in comandaItemTabelaMemoria.Rows) {
					// Gravar os itens
					ComandaItem comandaItem = new ComandaItem(usuarioID);
					comandaItem.ComandaID.Valor = comandaID;
					comandaItem.ProdutoID.Valor = Convert.ToInt32(linha["ProdutoID"]);
					comandaItem.Quantidade.Valor = Convert.ToInt32(linha["Quantidade"]);
					// Pesquisar PrecoVenda
					Produto produto = new Produto();
					produto.Ler(Convert.ToInt32(linha["ProdutoID"]));
					comandaItem.PrecoVenda.Valor = produto.PrecoVenda.Valor;
					// Inserir o item de comanda
					comandaItem.Inserir();
					// Atualizar quantidade no estoque para cada item de venda
					Caixa caixa = new Caixa(usuarioID);
					caixa.Ler(caixaID);
					Loja loja = new Loja(usuarioID);
					loja.Ler(caixa.LojaID.Valor);
					EstoqueItem estoqueItem = new EstoqueItem(usuarioID);
					estoqueItem.ProdutoID.Valor = Convert.ToInt32(linha["ProdutoID"]);
					estoqueItem.EstoqueID.Valor = loja.EstoqueID.Valor;
					estoqueItem.DefinirID();
					estoqueItem.AtualizarSaldo(-1 * Convert.ToInt32(linha["Quantidade"]));
				}
				// Em sistema de Ticket, assim que imprimiu a comanda, salva a venda
				if (!trabalharComIngresso) { 
					// Chame um mehtodo que cria um registro de Venda e atualiza VendaID nesta Comanda 
					vendaID = InserirVenda(caixaID, comanda.IngressoID.Valor, comandaID);
				}
			}catch(Exception erro){
				throw erro;
			}
			return comanda.Ordem.Valor;
		} // fim de GravarComanda

		/// <summary>
		/// Registra os pagamentos salvando na tabela VendaPagamento e atualizando Pago no Venda
		/// Os itens vao estar em um DataTable (vendaItemTabelaMemoria) com FormaPagamentoID e Valor
		/// vendaID eh um parâmetro a parte
		/// </summary>
		/// <param name="vendaID"></param>
		/// <param name="vendaItemTabelaMemoria"></param>
		/// <returns></returns>
		public void GravarPagamento(
			int vendaID, 
			decimal [,] vendaItemMatrizMemoria) {			
			try{
				// Gravar na tabela Venda
				Venda venda = new Venda(usuarioID);
				venda.Ler(vendaID);
				venda.Pago.Valor = true;
				venda.Atualizar();
				// Gravar os pagamentos
				int tamanho = (int) vendaItemMatrizMemoria.Length/2; // 2 colunas
				for (int conta = 0; conta< tamanho; conta++) {
					// Gravar na tabela VendaPagamento
					VendaPagamento vendaPagamento = new VendaPagamento(usuarioID);
					vendaPagamento.VendaID.Valor = vendaID;
					vendaPagamento.FormaPagamentoID.Valor = Convert.ToInt32(vendaItemMatrizMemoria[conta,0]); // FormaPagamentoID
					vendaPagamento.Valor.Valor = Convert.ToDecimal(vendaItemMatrizMemoria[conta,1]); // Valor
					vendaPagamento.Inserir();
				}
			}catch(Exception erro){
				throw erro;
			}
		} // fim de GravarPagamento


		/// <summary>
		/// Registra os pagamentos salvando na tabela VendaPagamento e atualizando Pago no Venda
		/// Os itens vao estar em um DataTable (vendaItemTabelaMemoria) com FormaPagamentoID e Valor
		/// vendaID eh um parâmetro a parte
		/// </summary>
		/// <param name="vendaID"></param>
		/// <param name="vendaItemTabelaMemoria"></param>
		/// <returns></returns>
		public void GravarPagamento(
			int vendaID, 
			DataTable vendaItemTabelaMemoria) {
			try{
				// Gravar na tabela Venda
				Venda venda = new Venda(usuarioID);
				venda.Ler(vendaID);
				venda.Pago.Valor = true;
				venda.Atualizar();
				// Gravar os pagamentos
				foreach (DataRow linha in vendaItemTabelaMemoria.Rows) {
					// Gravar na tabela VendaPagamento
					VendaPagamento vendaPagamento = new VendaPagamento(usuarioID);
					vendaPagamento.VendaID.Valor = vendaID;
					vendaPagamento.FormaPagamentoID.Valor = Convert.ToInt32(linha["FormaPagamentoID"]);;
					vendaPagamento.Valor.Valor = Convert.ToDecimal(linha["Valor"]);;
					vendaPagamento.Inserir();
				}
			}catch(Exception erro){
				throw erro;
			}
		} // fim de GravarPagamento

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataTable EstruturaNovaComanda() {
			DataTable tabela = new DataTable("");
			try {
				tabela.Columns.Add("UsuarioID", typeof(int));
				tabela.Columns.Add("TrabalharComIngresso", typeof(bool));
				tabela.Columns.Add("OrdemComanda", typeof(string));
				tabela.Columns.Add("GarconID", typeof(int));
				tabela.Columns.Add("Ingresso", typeof(string));
				tabela.Columns.Add("CaixaID", typeof(int));
			}catch{
				tabela = null;
			}
			return tabela;
		} // fim de EstruturaNovaComanda
		/// <summary>		
		/// 
		/// </summary>
		/// <returns></returns>
		public bool InserirNovaComanda(DataTable tabelaAux, out int comandaID){
			comandaID= 0;
			try { 
				DataRow linha = (DataRow) tabelaAux.Rows[0];
				if (linha != null) {
					// verificar se existe uma comanda com essa ordem antes de incluir uma nova.
					if (linha["OrdemComanda"].ToString() != ""){ 
						ComandaLista lista = new ComandaLista(Convert.ToInt32(linha["UsuarioID"]));
						lista.FiltroSQL = "ORDEM='"+ linha["OrdemComanda"].ToString() +"'";
						lista.Carregar();
						if (lista.Tamanho > 0){
							comandaID= 0;
							return false;
						}
					}
					// Criar uma comanda no BD (inserir registro)
					Comanda comanda = new Comanda(Convert.ToInt32(linha["UsuarioID"]));
					comanda.Ordem.Valor = linha["OrdemComanda"].ToString();
					if ((bool) linha["TrabalharComIngresso"]){
						//					garcon.Ler(garconID); // soh pra pegar o nome?
						comanda.GarconID.Valor = Convert.ToInt32(linha["GarconID"]);
						//						codigoIngresso = linha["Ingresso"].ToString();
						//						comanda.IngressoID.Valor = codigoIngresso;
					}else{
						comanda.GarconID.Valor = 0;
						comanda.IngressoID.Valor = 0;
					}
					comanda.CaixaID.Valor = Convert.ToInt32(linha["CaixaID"]);
					comanda.Inserir();
					comandaID = comanda.Control.ID;
				}
				return true;
			} catch(Exception ex) {
				throw ex;
			}
		} // fim de InserirNovaComanda
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataTable EstruturaVenda() {
			DataTable tabela = new DataTable("");
			try {
				tabela.Columns.Add("UsuarioID", typeof(int));
				tabela.Columns.Add("TrabalharComIngresso", typeof(bool));
				tabela.Columns.Add("OrdemComanda", typeof(string));
				tabela.Columns.Add("GarconID", typeof(int));
				tabela.Columns.Add("Ingresso", typeof(string));
				tabela.Columns.Add("CaixaID", typeof(int));
			}catch{
				tabela = null;
			}
			return tabela;
		} // fim de EstruturaVenda
		/// <summary>		
		/// Usado ao Fechar Conta, isto eh reune todas Comandas por Ingresso
		/// </summary>
		/// <returns></returns>
		public DataTable InserirVenda(out int vendaID, int caixaID, int ingresso){
			// Criar um registro de Venda
			Venda venda = new Venda(usuarioID);
			venda.IngressoID.Valor = ingresso;
			venda.CaixaID.Valor = caixaID;
			venda.Pago.Valor = false;
			venda.Inserir();
			vendaID = venda.Control.ID;
			// Devolve o DataTable com todas as comandas (todos os itens de todas)
			Comanda comanda = new Comanda();
			DataTable tabela = comanda.Conta(ingresso); // esta operacao depende do VendaID=0, portanto nao pode ser executada depois da atualizacao abaixo
			// Atualizar todas Comandas desse Ingresso com VendaID em branco
			comanda.FecharConta(ingresso, vendaID);
			return tabela;
		} // fim de InserirVenda
		/// <summary>		
		/// Qdo o sistema eh por Comanda, eh soh atualizar as tabelas Venda e Comanda
		/// </summary>
		/// <returns></returns>
		public int InserirVenda(int caixaID, int ingresso, int comandaID){
			// Criar um registro de Venda
			Venda venda = new Venda(usuarioID);
			venda.IngressoID.Valor = ingresso;
			venda.CaixaID.Valor = caixaID;
			venda.Pago.Valor = false;
			venda.Inserir();
			int vendaID = venda.Control.ID;
			// Atualizar VendaID da Comanda
			Comanda comanda = new Comanda(usuarioID);
			comanda.Ler(comandaID);
			comanda.VendaID.Valor = (int) venda.Control.ID;
			comanda.Atualizar();
			return vendaID;
		} // fim de InserirVenda

		public int IngressoID(int apresentacaoID, int setorID, string codigo){
			Ingresso ingresso = new Ingresso();
			ingresso.Codigo.Valor = codigo;
			ingresso.Identifica(apresentacaoID, setorID);
			return ingresso.Control.ID;
		}
	} // fim da classe

} // fim do namespace


