/******************************************************
* Arquivo ComandaItemDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "ComandaItem_B"

	public abstract class ComandaItem_B : BaseBD {
	
		public comandaid ComandaID = new comandaid();
		public produtoid ProdutoID = new produtoid();
		public precovenda PrecoVenda = new precovenda();
		public quantidade Quantidade = new quantidade();
		
		public ComandaItem_B(){}
					
		// passar o Usuario logado no sistema
		public ComandaItem_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de ComandaItem
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tComandaItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.ComandaID.ValorBD = bd.LerInt("ComandaID").ToString();
					this.ProdutoID.ValorBD = bd.LerInt("ProdutoID").ToString();
					this.PrecoVenda.ValorBD = bd.LerDecimal("PrecoVenda").ToString();
					this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
				}else{
					this.Limpar();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		/// <summary>
		/// Preenche todos os atributos de ComandaItem do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xComandaItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.ComandaID.ValorBD = bd.LerInt("ComandaID").ToString();
					this.ProdutoID.ValorBD = bd.LerInt("ProdutoID").ToString();
					this.PrecoVenda.ValorBD = bd.LerDecimal("PrecoVenda").ToString();
					this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		protected void InserirControle(string acao){
		
			try{
			
				System.Text.StringBuilder sql = new System.Text.StringBuilder();
				sql.Append("INSERT INTO cComandaItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xComandaItem (ID, Versao, ComandaID, ProdutoID, PrecoVenda, Quantidade) ");
				sql.Append("SELECT ID, @V, ComandaID, ProdutoID, PrecoVenda, Quantidade FROM tComandaItem WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) ComandaItem
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cComandaItem");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tComandaItem(ID, ComandaID, ProdutoID, PrecoVenda, Quantidade) ");
				sql.Append("VALUES (@ID,@001,@002,'@003',@004)");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.ComandaID.ValorBD);
				sql.Replace("@002", this.ProdutoID.ValorBD);
				sql.Replace("@003", this.PrecoVenda.ValorBD);
				sql.Replace("@004", this.Quantidade.ValorBD);
				
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
		/// Atualiza ComandaItem
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cComandaItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tComandaItem SET ComandaID = @001, ProdutoID = @002, PrecoVenda = '@003', Quantidade = @004 ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.ComandaID.ValorBD);
				sql.Replace("@002", this.ProdutoID.ValorBD);
				sql.Replace("@003", this.PrecoVenda.ValorBD);
				sql.Replace("@004", this.Quantidade.ValorBD);
				
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
		/// Exclui ComandaItem com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cComandaItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tComandaItem WHERE ID="+id;
				
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
		/// Exclui ComandaItem
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
		
			this.ComandaID.Limpar();
			this.ProdutoID.Limpar();
			this.PrecoVenda.Limpar();
			this.Quantidade.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.ComandaID.Desfazer();
			this.ProdutoID.Desfazer();
			this.PrecoVenda.Desfazer();
			this.Quantidade.Desfazer();
		}

		public class comandaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "ComandaID";
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
		
		public class precovenda : NumberProperty{
		
			public override string Nome{
				get{
					return "PrecoVenda";
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
		
		public class quantidade : IntegerProperty{
		
			public override string Nome{
				get{
					return "Quantidade";
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
		
		/// <summary>
		/// Obtem uma tabela estruturada com todos os campos dessa classe.
		/// </summary>
		/// <returns></returns>
		public static DataTable Estrutura(){
		
			//Isso eh util para desacoplamento.
			//A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.
				
			try{

				DataTable tabela = new DataTable("ComandaItem");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("ComandaID", typeof(int));
				tabela.Columns.Add("ProdutoID", typeof(int));
				tabela.Columns.Add("PrecoVenda", typeof(decimal));
				tabela.Columns.Add("Quantidade", typeof(int));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "ComandaItemLista_B"
	
	public abstract class ComandaItemLista_B : BaseLista {
	
		private bool backup = false;
		protected ComandaItem comandaItem;
	
		// passar o Usuario logado no sistema
		public ComandaItemLista_B(){
			comandaItem = new ComandaItem();
		}
	
		// passar o Usuario logado no sistema
		public ComandaItemLista_B(int usuarioIDLogado){
			comandaItem = new ComandaItem(usuarioIDLogado);
		}
		
		public ComandaItem ComandaItem{
			get{ return comandaItem; }
		}

		/// <summary>
		/// Retorna um IBaseBD de ComandaItem especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					comandaItem.Ler(id);
					return comandaItem;
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
					sql = "SELECT ID FROM tComandaItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tComandaItem";
				
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
					sql = "SELECT ID FROM tComandaItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tComandaItem";
				
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
					sql = "SELECT ID FROM xComandaItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xComandaItem";
				
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
		/// Preenche ComandaItem corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					comandaItem.Ler(id);
				else	
					comandaItem.LerBackup(id);
				
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
		
				bool ok = comandaItem.Excluir();
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
		/// Inseri novo(a) ComandaItem na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = comandaItem.Inserir();
				if (ok){
					lista.Add(comandaItem.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de ComandaItem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("ComandaItem");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("ComandaID", typeof(int));
					tabela.Columns.Add("ProdutoID", typeof(int));
					tabela.Columns.Add("PrecoVenda", typeof(decimal));
					tabela.Columns.Add("Quantidade", typeof(int));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= comandaItem.Control.ID;
						linha["ComandaID"]= comandaItem.ComandaID.Valor;
						linha["ProdutoID"]= comandaItem.ProdutoID.Valor;
						linha["PrecoVenda"]= comandaItem.PrecoVenda.Valor;
						linha["Quantidade"]= comandaItem.Quantidade.Valor;
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

				DataTable tabela = new DataTable("RelatorioComandaItem");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("ComandaID", typeof(int));
					tabela.Columns.Add("ProdutoID", typeof(int));
					tabela.Columns.Add("PrecoVenda", typeof(decimal));
					tabela.Columns.Add("Quantidade", typeof(int));

					do{
						DataRow linha = tabela.NewRow();
						linha["ComandaID"]= comandaItem.ComandaID.Valor;
						linha["ProdutoID"]= comandaItem.ProdutoID.Valor;
						linha["PrecoVenda"]= comandaItem.PrecoVenda.Valor;
						linha["Quantidade"]= comandaItem.Quantidade.Valor;
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
					case "ComandaID":
						sql = "SELECT ID, ComandaID FROM tComandaItem WHERE "+FiltroSQL+" ORDER BY ComandaID";
						break;
					case "ProdutoID":
						sql = "SELECT ID, ProdutoID FROM tComandaItem WHERE "+FiltroSQL+" ORDER BY ProdutoID";
						break;
					case "PrecoVenda":
						sql = "SELECT ID, PrecoVenda FROM tComandaItem WHERE "+FiltroSQL+" ORDER BY PrecoVenda";
						break;
					case "Quantidade":
						sql = "SELECT ID, Quantidade FROM tComandaItem WHERE "+FiltroSQL+" ORDER BY Quantidade";
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

	#region "ComandaItemException"
	
	[Serializable]
	public class ComandaItemException : Exception {

		public ComandaItemException() : base (){}

		public ComandaItemException(string msg) : base (msg){}

		public ComandaItemException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}