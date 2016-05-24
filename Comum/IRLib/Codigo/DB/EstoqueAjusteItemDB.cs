/******************************************************
* Arquivo EstoqueAjusteItemDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "EstoqueAjusteItem_B"

	public abstract class EstoqueAjusteItem_B : BaseBD {
	
		public estoqueajusteid EstoqueAjusteID = new estoqueajusteid();
		public estoqueitemid EstoqueItemID = new estoqueitemid();
		public estoqueajustemotivoid EstoqueAjusteMotivoID = new estoqueajustemotivoid();
		public quantidade Quantidade = new quantidade();
		public obs Obs = new obs();
		
		public EstoqueAjusteItem_B(){}
					
		// passar o Usuario logado no sistema
		public EstoqueAjusteItem_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de EstoqueAjusteItem
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tEstoqueAjusteItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EstoqueAjusteID.ValorBD = bd.LerInt("EstoqueAjusteID").ToString();
					this.EstoqueItemID.ValorBD = bd.LerInt("EstoqueItemID").ToString();
					this.EstoqueAjusteMotivoID.ValorBD = bd.LerInt("EstoqueAjusteMotivoID").ToString();
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
		/// Preenche todos os atributos de EstoqueAjusteItem do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xEstoqueAjusteItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EstoqueAjusteID.ValorBD = bd.LerInt("EstoqueAjusteID").ToString();
					this.EstoqueItemID.ValorBD = bd.LerInt("EstoqueItemID").ToString();
					this.EstoqueAjusteMotivoID.ValorBD = bd.LerInt("EstoqueAjusteMotivoID").ToString();
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
				sql.Append("INSERT INTO cEstoqueAjusteItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xEstoqueAjusteItem (ID, Versao, EstoqueAjusteID, EstoqueItemID, EstoqueAjusteMotivoID, Quantidade, Obs) ");
				sql.Append("SELECT ID, @V, EstoqueAjusteID, EstoqueItemID, EstoqueAjusteMotivoID, Quantidade, Obs FROM tEstoqueAjusteItem WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) EstoqueAjusteItem
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEstoqueAjusteItem");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEstoqueAjusteItem(ID, EstoqueAjusteID, EstoqueItemID, EstoqueAjusteMotivoID, Quantidade, Obs) ");
				sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EstoqueAjusteID.ValorBD);
				sql.Replace("@002", this.EstoqueItemID.ValorBD);
				sql.Replace("@003", this.EstoqueAjusteMotivoID.ValorBD);
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
		/// Atualiza EstoqueAjusteItem
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEstoqueAjusteItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEstoqueAjusteItem SET EstoqueAjusteID = @001, EstoqueItemID = @002, EstoqueAjusteMotivoID = @003, Quantidade = '@004', Obs = '@005' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EstoqueAjusteID.ValorBD);
				sql.Replace("@002", this.EstoqueItemID.ValorBD);
				sql.Replace("@003", this.EstoqueAjusteMotivoID.ValorBD);
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
		/// Exclui EstoqueAjusteItem com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEstoqueAjusteItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEstoqueAjusteItem WHERE ID="+id;
				
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
		/// Exclui EstoqueAjusteItem
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
		
			this.EstoqueAjusteID.Limpar();
			this.EstoqueItemID.Limpar();
			this.EstoqueAjusteMotivoID.Limpar();
			this.Quantidade.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EstoqueAjusteID.Desfazer();
			this.EstoqueItemID.Desfazer();
			this.EstoqueAjusteMotivoID.Desfazer();
			this.Quantidade.Desfazer();
			this.Obs.Desfazer();
		}

		public class estoqueajusteid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueAjusteID";
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
		
		public class estoqueitemid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueItemID";
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
		
		public class estoqueajustemotivoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EstoqueAjusteMotivoID";
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

				DataTable tabela = new DataTable("EstoqueAjusteItem");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EstoqueAjusteID", typeof(int));
				tabela.Columns.Add("EstoqueItemID", typeof(int));
				tabela.Columns.Add("EstoqueAjusteMotivoID", typeof(int));
				tabela.Columns.Add("Quantidade", typeof(decimal));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "EstoqueAjusteItemLista_B"
	
	public abstract class EstoqueAjusteItemLista_B : BaseLista {
	
		private bool backup = false;
		protected EstoqueAjusteItem estoqueAjusteItem;
	
		// passar o Usuario logado no sistema
		public EstoqueAjusteItemLista_B(){
			estoqueAjusteItem = new EstoqueAjusteItem();
		}
	
		// passar o Usuario logado no sistema
		public EstoqueAjusteItemLista_B(int usuarioIDLogado){
			estoqueAjusteItem = new EstoqueAjusteItem(usuarioIDLogado);
		}
		
		public EstoqueAjusteItem EstoqueAjusteItem{
			get{ return estoqueAjusteItem; }
		}

		/// <summary>
		/// Retorna um IBaseBD de EstoqueAjusteItem especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					estoqueAjusteItem.Ler(id);
					return estoqueAjusteItem;
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
					sql = "SELECT ID FROM tEstoqueAjusteItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueAjusteItem";
				
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
					sql = "SELECT ID FROM tEstoqueAjusteItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueAjusteItem";
				
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
					sql = "SELECT ID FROM xEstoqueAjusteItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xEstoqueAjusteItem";
				
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
		/// Preenche EstoqueAjusteItem corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					estoqueAjusteItem.Ler(id);
				else	
					estoqueAjusteItem.LerBackup(id);
				
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
		
				bool ok = estoqueAjusteItem.Excluir();
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
		/// Inseri novo(a) EstoqueAjusteItem na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = estoqueAjusteItem.Inserir();
				if (ok){
					lista.Add(estoqueAjusteItem.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de EstoqueAjusteItem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("EstoqueAjusteItem");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EstoqueAjusteID", typeof(int));
					tabela.Columns.Add("EstoqueItemID", typeof(int));
					tabela.Columns.Add("EstoqueAjusteMotivoID", typeof(int));
					tabela.Columns.Add("Quantidade", typeof(decimal));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= estoqueAjusteItem.Control.ID;
						linha["EstoqueAjusteID"]= estoqueAjusteItem.EstoqueAjusteID.Valor;
						linha["EstoqueItemID"]= estoqueAjusteItem.EstoqueItemID.Valor;
						linha["EstoqueAjusteMotivoID"]= estoqueAjusteItem.EstoqueAjusteMotivoID.Valor;
						linha["Quantidade"]= estoqueAjusteItem.Quantidade.Valor;
						linha["Obs"]= estoqueAjusteItem.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioEstoqueAjusteItem");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EstoqueAjusteID", typeof(int));
					tabela.Columns.Add("EstoqueItemID", typeof(int));
					tabela.Columns.Add("EstoqueAjusteMotivoID", typeof(int));
					tabela.Columns.Add("Quantidade", typeof(decimal));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EstoqueAjusteID"]= estoqueAjusteItem.EstoqueAjusteID.Valor;
						linha["EstoqueItemID"]= estoqueAjusteItem.EstoqueItemID.Valor;
						linha["EstoqueAjusteMotivoID"]= estoqueAjusteItem.EstoqueAjusteMotivoID.Valor;
						linha["Quantidade"]= estoqueAjusteItem.Quantidade.Valor;
						linha["Obs"]= estoqueAjusteItem.Obs.Valor;
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
					case "EstoqueAjusteID":
						sql = "SELECT ID, EstoqueAjusteID FROM tEstoqueAjusteItem WHERE "+FiltroSQL+" ORDER BY EstoqueAjusteID";
						break;
					case "EstoqueItemID":
						sql = "SELECT ID, EstoqueItemID FROM tEstoqueAjusteItem WHERE "+FiltroSQL+" ORDER BY EstoqueItemID";
						break;
					case "EstoqueAjusteMotivoID":
						sql = "SELECT ID, EstoqueAjusteMotivoID FROM tEstoqueAjusteItem WHERE "+FiltroSQL+" ORDER BY EstoqueAjusteMotivoID";
						break;
					case "Quantidade":
						sql = "SELECT ID, Quantidade FROM tEstoqueAjusteItem WHERE "+FiltroSQL+" ORDER BY Quantidade";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tEstoqueAjusteItem WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "EstoqueAjusteItemException"
	
	[Serializable]
	public class EstoqueAjusteItemException : Exception {

		public EstoqueAjusteItemException() : base (){}

		public EstoqueAjusteItemException(string msg) : base (msg){}

		public EstoqueAjusteItemException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}