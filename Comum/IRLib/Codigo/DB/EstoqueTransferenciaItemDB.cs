/******************************************************
* Arquivo EstoqueTransferenciaItemDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "EstoqueTransferenciaItem_B"

	public abstract class EstoqueTransferenciaItem_B : BaseBD {
	
		public estoqueitemdeid EstoqueItemDeID = new estoqueitemdeid();
		public estoqueitemparaid EstoqueItemParaID = new estoqueitemparaid();
		public estoquetransferenciaid EstoqueTransferenciaID = new estoquetransferenciaid();
		public quantidade Quantidade = new quantidade();
		public obs Obs = new obs();
		
		public EstoqueTransferenciaItem_B(){}
					
		// passar o Usuario logado no sistema
		public EstoqueTransferenciaItem_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de EstoqueTransferenciaItem
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tEstoqueTransferenciaItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EstoqueItemDeID.ValorBD = bd.LerInt("EstoqueItemDeID").ToString();
					this.EstoqueItemParaID.ValorBD = bd.LerInt("EstoqueItemParaID").ToString();
					this.EstoqueTransferenciaID.ValorBD = bd.LerInt("EstoqueTransferenciaID").ToString();
					this.Quantidade.ValorBD = bd.LerDecimal("Quantidade").ToString();
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
		/// Preenche todos os atributos de EstoqueTransferenciaItem do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xEstoqueTransferenciaItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EstoqueItemDeID.ValorBD = bd.LerInt("EstoqueItemDeID").ToString();
					this.EstoqueItemParaID.ValorBD = bd.LerInt("EstoqueItemParaID").ToString();
					this.EstoqueTransferenciaID.ValorBD = bd.LerInt("EstoqueTransferenciaID").ToString();
					this.Quantidade.ValorBD = bd.LerDecimal("Quantidade").ToString();
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
				sql.Append("INSERT INTO cEstoqueTransferenciaItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xEstoqueTransferenciaItem (ID, Versao, EstoqueItemDeID, EstoqueItemParaID, EstoqueTransferenciaID, Quantidade, Obs) ");
				sql.Append("SELECT ID, @V, EstoqueItemDeID, EstoqueItemParaID, EstoqueTransferenciaID, Quantidade, Obs FROM tEstoqueTransferenciaItem WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) EstoqueTransferenciaItem
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEstoqueTransferenciaItem");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEstoqueTransferenciaItem(ID, EstoqueItemDeID, EstoqueItemParaID, EstoqueTransferenciaID, Quantidade, Obs) ");
				sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EstoqueItemDeID.ValorBD);
				sql.Replace("@002", this.EstoqueItemParaID.ValorBD);
				sql.Replace("@003", this.EstoqueTransferenciaID.ValorBD);
				sql.Replace("@004", this.Quantidade.ValorBD);
				sql.Replace("@005", this.Obs.ValorBD);
				
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
		/// Atualiza EstoqueTransferenciaItem
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEstoqueTransferenciaItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEstoqueTransferenciaItem SET EstoqueItemDeID = @001, EstoqueItemParaID = @002, EstoqueTransferenciaID = @003, Quantidade = '@004', Obs = '@005' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EstoqueItemDeID.ValorBD);
				sql.Replace("@002", this.EstoqueItemParaID.ValorBD);
				sql.Replace("@003", this.EstoqueTransferenciaID.ValorBD);
				sql.Replace("@004", this.Quantidade.ValorBD);
				sql.Replace("@005", this.Obs.ValorBD);
				
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
		/// Exclui EstoqueTransferenciaItem com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEstoqueTransferenciaItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEstoqueTransferenciaItem WHERE ID="+id;
				
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
		/// Exclui EstoqueTransferenciaItem
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
		
			this.EstoqueItemDeID.Limpar();
			this.EstoqueItemParaID.Limpar();
			this.EstoqueTransferenciaID.Limpar();
			this.Quantidade.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EstoqueItemDeID.Desfazer();
			this.EstoqueItemParaID.Desfazer();
			this.EstoqueTransferenciaID.Desfazer();
			this.Quantidade.Desfazer();
			this.Obs.Desfazer();
		}

		public class estoqueitemdeid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueItemDeID";
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
		
		public class estoqueitemparaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueItemParaID";
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
		
		public class estoquetransferenciaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueTransferenciaID";
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
		
		public class quantidade : NumberProperty{
		
			public override string Nome{
				get{
					return "Quantidade";
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

				DataTable tabela = new DataTable("EstoqueTransferenciaItem");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EstoqueItemDeID", typeof(int));
				tabela.Columns.Add("EstoqueItemParaID", typeof(int));
				tabela.Columns.Add("EstoqueTransferenciaID", typeof(int));
				tabela.Columns.Add("Quantidade", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "EstoqueTransferenciaItemLista_B"
	
	public abstract class EstoqueTransferenciaItemLista_B : BaseLista {
	
		private bool backup = false;
		protected EstoqueTransferenciaItem estoqueTransferenciaItem;
	
		// passar o Usuario logado no sistema
		public EstoqueTransferenciaItemLista_B(){
			estoqueTransferenciaItem = new EstoqueTransferenciaItem();
		}
	
		// passar o Usuario logado no sistema
		public EstoqueTransferenciaItemLista_B(int usuarioIDLogado){
			estoqueTransferenciaItem = new EstoqueTransferenciaItem(usuarioIDLogado);
		}
		
		public EstoqueTransferenciaItem EstoqueTransferenciaItem{
			get{ return estoqueTransferenciaItem; }
		}

		/// <summary>
		/// Retorna um IBaseBD de EstoqueTransferenciaItem especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					estoqueTransferenciaItem.Ler(id);
					return estoqueTransferenciaItem;
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
					sql = "SELECT ID FROM tEstoqueTransferenciaItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueTransferenciaItem";
				
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
					sql = "SELECT ID FROM tEstoqueTransferenciaItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueTransferenciaItem";
				
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
					sql = "SELECT ID FROM xEstoqueTransferenciaItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xEstoqueTransferenciaItem";
				
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
		/// Preenche EstoqueTransferenciaItem corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					estoqueTransferenciaItem.Ler(id);
				else	
					estoqueTransferenciaItem.LerBackup(id);
				
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
		
				bool ok = estoqueTransferenciaItem.Excluir();
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
		/// Inseri novo(a) EstoqueTransferenciaItem na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = estoqueTransferenciaItem.Inserir();
				if (ok){
					lista.Add(estoqueTransferenciaItem.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de EstoqueTransferenciaItem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("EstoqueTransferenciaItem");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EstoqueItemDeID", typeof(int));
					tabela.Columns.Add("EstoqueItemParaID", typeof(int));
					tabela.Columns.Add("EstoqueTransferenciaID", typeof(int));
					tabela.Columns.Add("Quantidade", typeof(decimal));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= estoqueTransferenciaItem.Control.ID;
						linha["EstoqueItemDeID"]= estoqueTransferenciaItem.EstoqueItemDeID.Valor;
						linha["EstoqueItemParaID"]= estoqueTransferenciaItem.EstoqueItemParaID.Valor;
						linha["EstoqueTransferenciaID"]= estoqueTransferenciaItem.EstoqueTransferenciaID.Valor;
						linha["Quantidade"]= estoqueTransferenciaItem.Quantidade.Valor;
						linha["Obs"]= estoqueTransferenciaItem.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioEstoqueTransferenciaItem");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EstoqueItemDeID", typeof(int));
					tabela.Columns.Add("EstoqueItemParaID", typeof(int));
					tabela.Columns.Add("EstoqueTransferenciaID", typeof(int));
					tabela.Columns.Add("Quantidade", typeof(decimal));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EstoqueItemDeID"]= estoqueTransferenciaItem.EstoqueItemDeID.Valor;
						linha["EstoqueItemParaID"]= estoqueTransferenciaItem.EstoqueItemParaID.Valor;
						linha["EstoqueTransferenciaID"]= estoqueTransferenciaItem.EstoqueTransferenciaID.Valor;
						linha["Quantidade"]= estoqueTransferenciaItem.Quantidade.Valor;
						linha["Obs"]= estoqueTransferenciaItem.Obs.Valor;
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
					case "EstoqueItemDeID":
						sql = "SELECT ID, EstoqueItemDeID FROM tEstoqueTransferenciaItem WHERE "+FiltroSQL+" ORDER BY EstoqueItemDeID";
						break;
					case "EstoqueItemParaID":
						sql = "SELECT ID, EstoqueItemParaID FROM tEstoqueTransferenciaItem WHERE "+FiltroSQL+" ORDER BY EstoqueItemParaID";
						break;
					case "EstoqueTransferenciaID":
						sql = "SELECT ID, EstoqueTransferenciaID FROM tEstoqueTransferenciaItem WHERE "+FiltroSQL+" ORDER BY EstoqueTransferenciaID";
						break;
					case "Quantidade":
						sql = "SELECT ID, Quantidade FROM tEstoqueTransferenciaItem WHERE "+FiltroSQL+" ORDER BY Quantidade";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tEstoqueTransferenciaItem WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "EstoqueTransferenciaItemException"
	
	[Serializable]
	public class EstoqueTransferenciaItemException : Exception {

		public EstoqueTransferenciaItemException() : base (){}

		public EstoqueTransferenciaItemException(string msg) : base (msg){}

		public EstoqueTransferenciaItemException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}