/******************************************************
* Arquivo EstoqueItemDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "EstoqueItem_B"

	public abstract class EstoqueItem_B : BaseBD {
	
		public estoqueid EstoqueID = new estoqueid();
		public produtoid ProdutoID = new produtoid();
		public saldoatual SaldoAtual = new saldoatual();
		public obs Obs = new obs();
		
		public EstoqueItem_B(){}
					
		// passar o Usuario logado no sistema
		public EstoqueItem_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de EstoqueItem
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tEstoqueItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EstoqueID.ValorBD = bd.LerInt("EstoqueID").ToString();
					this.ProdutoID.ValorBD = bd.LerInt("ProdutoID").ToString();
					this.SaldoAtual.ValorBD = bd.LerDecimal("SaldoAtual").ToString();
					this.Obs.ValorBD = bd.LerString("Obs");
				}else{
					this.Limpar();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		/// <summary>
		/// Preenche todos os atributos de EstoqueItem do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xEstoqueItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EstoqueID.ValorBD = bd.LerInt("EstoqueID").ToString();
					this.ProdutoID.ValorBD = bd.LerInt("ProdutoID").ToString();
					this.SaldoAtual.ValorBD = bd.LerDecimal("SaldoAtual").ToString();
					this.Obs.ValorBD = bd.LerString("Obs");
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		protected void InserirControle(string acao){
		
			try{
			
				System.Text.StringBuilder sql = new System.Text.StringBuilder();
				sql.Append("INSERT INTO cEstoqueItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
				sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
				sql.Replace("@ID", this.Control.ID.ToString());
				
				if (!acao.Equals("I"))
					this.Control.Versao++;
				
				sql.Replace("@V", this.Control.Versao.ToString());
				sql.Replace("@A", acao);
				sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
				sql.Replace("@U", this.Control.UsuarioID.ToString());
				
				bd.Executar(sql.ToString());
	
			}catch(Exception ex){
				throw ex;
			}
					
		}
			
		protected void InserirLog(){
			
			try{
				
				StringBuilder sql = new StringBuilder();
				
				sql.Append("INSERT INTO xEstoqueItem (ID, Versao, EstoqueID, ProdutoID, SaldoAtual, Obs) ");
				sql.Append("SELECT ID, @V, EstoqueID, ProdutoID, SaldoAtual, Obs FROM tEstoqueItem WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) EstoqueItem
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEstoqueItem");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEstoqueItem(ID, EstoqueID, ProdutoID, SaldoAtual, Obs) ");
				sql.Append("VALUES (@ID,@001,@002,'@003','@004')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EstoqueID.ValorBD);
				sql.Replace("@002", this.ProdutoID.ValorBD);
				sql.Replace("@003", this.SaldoAtual.ValorBD);
				sql.Replace("@004", this.Obs.ValorBD);
				
				int x = bd.Executar(sql.ToString());
				
				bool result = (x == 1);
				
				if (result)
					InserirControle("I");

				bd.FinalizarTransacao();	
				
				return result;
				
			}catch(Exception ex){
				bd.DesfazerTransacao();
				throw ex;
			}finally{
				bd.Fechar();
			}
			
		}

		/// <summary>
		/// Atualiza EstoqueItem
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEstoqueItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEstoqueItem SET EstoqueID = @001, ProdutoID = @002, SaldoAtual = '@003', Obs = '@004' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EstoqueID.ValorBD);
				sql.Replace("@002", this.ProdutoID.ValorBD);
				sql.Replace("@003", this.SaldoAtual.ValorBD);
				sql.Replace("@004", this.Obs.ValorBD);
				
				int x = bd.Executar(sql.ToString());
				
				bool result = (x == 1);

				bd.FinalizarTransacao();
				
				return result;
				
			}catch(Exception ex){
				bd.DesfazerTransacao();
				throw ex;
			}finally{
				bd.Fechar();
			}
			
		}

		/// <summary>
		/// Exclui EstoqueItem com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEstoqueItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEstoqueItem WHERE ID="+id;
				
				int x = bd.Executar(sqlDelete);
				
				bool result = (x == 1);

				bd.FinalizarTransacao();
				
				return result;
			
			}catch(Exception ex){
				bd.DesfazerTransacao();
				throw ex;
			}finally{
				bd.Fechar();
			}
			
		}
		
		/// <summary>
		/// Exclui EstoqueItem
		/// </summary>
		/// <returns></returns>		
		public override bool Excluir(){
		
			try{
				return this.Excluir(this.Control.ID);
			}catch(Exception ex){
				throw ex;
			}
			
		}

		public override void Limpar(){
		
			this.EstoqueID.Limpar();
			this.ProdutoID.Limpar();
			this.SaldoAtual.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EstoqueID.Desfazer();
			this.ProdutoID.Desfazer();
			this.SaldoAtual.Desfazer();
			this.Obs.Desfazer();
		}

		public class estoqueid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueID";
				}
			}
			
			public override int Tamanho{
				get{
					return 0;
				}
			}
			
			public override int Valor{
				get{
					return base.Valor;
				}
				set{
					base.Valor = value;
				}
			}
			
			public override string ToString(){
				return base.Valor.ToString();
			}
			
		}
		
		public class produtoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "ProdutoID";
				}
			}
			
			public override int Tamanho{
				get{
					return 0;
				}
			}
			
			public override int Valor{
				get{
					return base.Valor;
				}
				set{
					base.Valor = value;
				}
			}
			
			public override string ToString(){
				return base.Valor.ToString();
			}
			
		}
		
		public class saldoatual : NumberProperty{
		
			public override string Nome{
				get{
					return "SaldoAtual";
				}
			}
			
			public override int Tamanho{
				get{
					return 12;
				}
			}
			
			public override decimal Valor{
				get{
					return base.Valor;
				}
				set{
					base.Valor = value;
				}
			}
			
			public override string ToString(){
				return base.Valor.ToString("###,##0.00");
			}
			
		}
		
		public class obs : TextProperty{
		
			public override string Nome{
				get{
					return "Obs";
				}
			}
			
			public override int Tamanho{
				get{
					return 0;
				}
			}
			
			public override string Valor{
				get{
					return base.Valor;
				}
				set{
					base.Valor = value;
				}
			}
			
			public override string ToString(){
				return base.Valor;
			}
			
		}
		
		/// <summary>
		/// Obtem uma tabela estruturada com todos os campos dessa classe.
		/// </summary>
		/// <returns></returns>
		public static DataTable Estrutura(){
		
			//Isso eh util para desacoplamento.
			//A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.
				
			try{

				DataTable tabela = new DataTable("EstoqueItem");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EstoqueID", typeof(int));
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("SaldoAtual", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
		public abstract bool AtualizarSaldo(decimal qtde);

		public abstract int DefinirID();

	}
	#endregion

	#region "EstoqueItemLista_B"
	
	public abstract class EstoqueItemLista_B : BaseLista {
	
		private bool backup = false;
		protected EstoqueItem estoqueItem;
	
		// passar o Usuario logado no sistema
		public EstoqueItemLista_B(){
			estoqueItem = new EstoqueItem();
		}
	
		// passar o Usuario logado no sistema
		public EstoqueItemLista_B(int usuarioIDLogado){
			estoqueItem = new EstoqueItem(usuarioIDLogado);
		}
		
		public EstoqueItem EstoqueItem{
			get{ return estoqueItem; }
		}

		/// <summary>
		/// Retorna um IBaseBD de EstoqueItem especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					estoqueItem.Ler(id);
					return estoqueItem;
				}
			}
		}
		
		/// <summary>
		/// Carrega a lista
		/// </summary>
		/// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
		/// <returns></returns>		
		public void Carregar(int tamanhoMax){
		
			try{
			
				string sql;
			
				if (tamanhoMax==0)
					sql = "SELECT ID FROM tEstoqueItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueItem";
				
				if (FiltroSQL!=null && FiltroSQL.Trim()!="")
					sql += " WHERE " + FiltroSQL.Trim();
					
				if (OrdemSQL!=null && OrdemSQL.Trim()!="")
					sql += " ORDER BY " + OrdemSQL.Trim();
				
				lista.Clear();
				
				bd.Consulta(sql);
				
				while (bd.Consulta().Read())
					lista.Add(bd.LerInt("ID"));
				
				lista.TrimToSize();
				
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
			
		}
				
		/// <summary>
		/// Carrega a lista
		/// </summary>
		public override void Carregar(){
		
			try{
			
				string sql;
			
				if (tamanhoMax==0)
					sql = "SELECT ID FROM tEstoqueItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueItem";
				
				if (FiltroSQL!=null && FiltroSQL.Trim()!="")
					sql += " WHERE " + FiltroSQL.Trim();
					
				if (OrdemSQL!=null && OrdemSQL.Trim()!="")
					sql += " ORDER BY " + OrdemSQL.Trim();
				
				lista.Clear();
				
				bd.Consulta(sql);
				
				while (bd.Consulta().Read())
					lista.Add(bd.LerInt("ID"));
				
				lista.TrimToSize();
				
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
			
		}
				
		/// <summary>
		/// Carrega a lista pela tabela x (de backup)
		/// </summary>
		public void CarregarBackup(){
		
			try{
			
				string sql;
			
				if (tamanhoMax==0)
					sql = "SELECT ID FROM xEstoqueItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xEstoqueItem";
				
				if (FiltroSQL!=null && FiltroSQL.Trim()!="")
					sql += " WHERE " + FiltroSQL.Trim();
					
				if (OrdemSQL!=null && OrdemSQL.Trim()!="")
					sql += " ORDER BY " + OrdemSQL.Trim();
				
				lista.Clear();
				
				bd.Consulta(sql);
				
				while (bd.Consulta().Read())
					lista.Add(bd.LerInt("ID"));
				
				lista.TrimToSize();
				
				bd.Fechar();

				backup = true;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		/// Preenche EstoqueItem corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					estoqueItem.Ler(id);
				else	
					estoqueItem.LerBackup(id);
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		/// Exclui o item corrente da lista
		/// </summary>
		/// <returns></returns>
		public override bool Excluir(){
		
			try{
		
				bool ok = estoqueItem.Excluir();
				if (ok)				
					lista.RemoveAt(Indice);
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}
		
		/// <summary>
		/// Exclui todos os itens da lista carregada
		/// </summary>
		/// <returns></returns>
		public override bool ExcluirTudo(){
			
			try{
				if (lista.Count == 0)
					Carregar();
			}catch(Exception ex){
				throw ex;
			}
			
			try{
		
				bool ok = false;

				if (lista.Count > 0){ //verifica se tem itens

					Ultimo();
					//fazer varredura de traz pra frente.
					do
						ok = Excluir();
					while (ok && Anterior());

				}else{ //nao tem itens na lista
					//Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
					ok = true;
				}
				
				return ok;
			
			}catch(Exception ex){
				throw ex;
			}

		}		
		
		/// <summary>
		/// Inseri novo(a) EstoqueItem na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = estoqueItem.Inserir();
				if (ok){
					lista.Add(estoqueItem.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de EstoqueItem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("EstoqueItem");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EstoqueID", typeof(int));
					tabela.Columns.Add("ProdutoID", typeof(int));
					tabela.Columns.Add("SaldoAtual", typeof(decimal));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= estoqueItem.Control.ID;
						linha["EstoqueID"]= estoqueItem.EstoqueID.Valor;
						linha["ProdutoID"]= estoqueItem.ProdutoID.Valor;
						linha["SaldoAtual"]= estoqueItem.SaldoAtual.Valor;
						linha["Obs"]= estoqueItem.Obs.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}
			
		/// <summary>
		/// Obtem uma tabela a ser jogada num relatorio
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){
				
			try{

				DataTable tabela = new DataTable("RelatorioEstoqueItem");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EstoqueID", typeof(int));
					tabela.Columns.Add("ProdutoID", typeof(int));
					tabela.Columns.Add("SaldoAtual", typeof(decimal));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EstoqueID"]= estoqueItem.EstoqueID.Valor;
						linha["ProdutoID"]= estoqueItem.ProdutoID.Valor;
						linha["SaldoAtual"]= estoqueItem.SaldoAtual.Valor;
						linha["Obs"]= estoqueItem.Obs.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}else{ //erro: nao carregou a lista
					tabela = null;
				}		
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}

		}		
		
		/// <summary>
		/// Retorna um IDataReader com ID e o Campo.
		/// </summary>
		/// <param name="campo">Informe o campo. Exemplo: Nome</param>
		/// <returns></returns>
		public override IDataReader ListaPropriedade(string campo){
		
			try{
				string sql;
				switch (campo){
					case "EstoqueID":
						sql = "SELECT ID, EstoqueID FROM tEstoqueItem WHERE "+FiltroSQL+" ORDER BY EstoqueID";
						break;
					case "ProdutoID":
						sql = "SELECT ID, ProdutoID FROM tEstoqueItem WHERE "+FiltroSQL+" ORDER BY ProdutoID";
						break;
					case "SaldoAtual":
						sql = "SELECT ID, SaldoAtual FROM tEstoqueItem WHERE "+FiltroSQL+" ORDER BY SaldoAtual";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tEstoqueItem WHERE "+FiltroSQL+" ORDER BY Obs";
						break;
					default:
						sql = null;
						break;
				}
				
				IDataReader dataReader = bd.Consulta(sql);

				bd.Fechar();
				
				return dataReader;

			}catch(Exception ex){
				throw ex;
			}
			
		}
		
		/// <summary>
		/// Devolve um array dos IDs que compoem a lista
		/// </summary>
		/// <returns></returns>		
		public override int[] ToArray(){
		
			try{

				int[] a = (int[])lista.ToArray(typeof(int));

				return a;
			
			}catch(Exception ex){
				throw ex;
			}

		}

		/// <summary>
		/// Devolve uma string dos IDs que compoem a lista concatenada por virgula
		/// </summary>
		/// <returns></returns>
		public override string ToString(){
		
			try{

				StringBuilder idsBuffer = new StringBuilder();

				int n = lista.Count;
				for(int i=0; i < n; i++){
					int id = (int)lista[i];
					idsBuffer.Append(id+",");
				}
					
				string ids = "";

				if (idsBuffer.Length > 0){
					ids = idsBuffer.ToString();
					ids = ids.Substring(0, ids.Length -1);
				}

				return ids;
				
			}catch(Exception ex){
				throw ex;
			}

		}		
		
	}
	
	#endregion

	#region "EstoqueItemException"
	
	[Serializable]
	public class EstoqueItemException : Exception {

		public EstoqueItemException() : base (){}

		public EstoqueItemException(string msg) : base (msg){}

		public EstoqueItemException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}