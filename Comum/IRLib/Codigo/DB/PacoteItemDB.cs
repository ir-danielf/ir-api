/******************************************************
* Arquivo PacoteItemDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "PacoteItem_B"

	public abstract class PacoteItem_B : BaseBD {
	
		public eventoid EventoID = new eventoid();
		public apresentacaoid ApresentacaoID = new apresentacaoid();
		public setorid SetorID = new setorid();
		public precoid PrecoID = new precoid();
		public cortesiaid CortesiaID = new cortesiaid();
		public pacoteid PacoteID = new pacoteid();
		public quantidade Quantidade = new quantidade();
		
		public PacoteItem_B(){}
					
		// passar o Usuario logado no sistema
		public PacoteItem_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de PacoteItem
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tPacoteItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
					this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
					this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
					this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
					this.CortesiaID.ValorBD = bd.LerInt("CortesiaID").ToString();
					this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
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
		/// Preenche todos os atributos de PacoteItem do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xPacoteItem WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
					this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
					this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
					this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
					this.CortesiaID.ValorBD = bd.LerInt("CortesiaID").ToString();
					this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
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
				sql.Append("INSERT INTO cPacoteItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xPacoteItem (ID, Versao, EventoID, ApresentacaoID, SetorID, PrecoID, CortesiaID, PacoteID, Quantidade) ");
				sql.Append("SELECT ID, @V, EventoID, ApresentacaoID, SetorID, PrecoID, CortesiaID, PacoteID, Quantidade FROM tPacoteItem WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) PacoteItem
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cPacoteItem");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tPacoteItem(ID, EventoID, ApresentacaoID, SetorID, PrecoID, CortesiaID, PacoteID, Quantidade) ");
				sql.Append("VALUES (@ID,@001,@002,@003,@004,@005,@006,@007)");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EventoID.ValorBD);
				sql.Replace("@002", this.ApresentacaoID.ValorBD);
				sql.Replace("@003", this.SetorID.ValorBD);
				sql.Replace("@004", this.PrecoID.ValorBD);
				sql.Replace("@005", this.CortesiaID.ValorBD);
				sql.Replace("@006", this.PacoteID.ValorBD);
				sql.Replace("@007", this.Quantidade.ValorBD);
				
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
		/// Atualiza PacoteItem
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cPacoteItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tPacoteItem SET EventoID = @001, ApresentacaoID = @002, SetorID = @003, PrecoID = @004, CortesiaID = @005, PacoteID = @006, Quantidade = @007 ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EventoID.ValorBD);
				sql.Replace("@002", this.ApresentacaoID.ValorBD);
				sql.Replace("@003", this.SetorID.ValorBD);
				sql.Replace("@004", this.PrecoID.ValorBD);
				sql.Replace("@005", this.CortesiaID.ValorBD);
				sql.Replace("@006", this.PacoteID.ValorBD);
				sql.Replace("@007", this.Quantidade.ValorBD);
				
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
		/// Exclui PacoteItem com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cPacoteItem WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tPacoteItem WHERE ID="+id;
				
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
		/// Exclui PacoteItem
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
		
			this.EventoID.Limpar();
			this.ApresentacaoID.Limpar();
			this.SetorID.Limpar();
			this.PrecoID.Limpar();
			this.CortesiaID.Limpar();
			this.PacoteID.Limpar();
			this.Quantidade.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EventoID.Desfazer();
			this.ApresentacaoID.Desfazer();
			this.SetorID.Desfazer();
			this.PrecoID.Desfazer();
			this.CortesiaID.Desfazer();
			this.PacoteID.Desfazer();
			this.Quantidade.Desfazer();
		}

		public class eventoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EventoID";
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
		
		public class apresentacaoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "ApresentacaoID";
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
		
		public class setorid : IntegerProperty{
		
			public override string Nome{
				get{
					return "SetorID";
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
		
		public class precoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "PrecoID";
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
		
		public class cortesiaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "CortesiaID";
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
		
		public class pacoteid : IntegerProperty{
		
			public override string Nome{
				get{
					return "PacoteID";
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

				DataTable tabela = new DataTable("PacoteItem");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EventoID", typeof(int));
				tabela.Columns.Add("ApresentacaoID", typeof(int));
				tabela.Columns.Add("SetorID", typeof(int));
				tabela.Columns.Add("PrecoID", typeof(int));
				tabela.Columns.Add("CortesiaID", typeof(int));
				tabela.Columns.Add("PacoteID", typeof(int));
				tabela.Columns.Add("Quantidade", typeof(int));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "PacoteItemLista_B"
	
	public abstract class PacoteItemLista_B : BaseLista {
	
		private bool backup = false;
		protected PacoteItem pacoteItem;
	
		// passar o Usuario logado no sistema
		public PacoteItemLista_B(){
			pacoteItem = new PacoteItem();
		}
	
		// passar o Usuario logado no sistema
		public PacoteItemLista_B(int usuarioIDLogado){
			pacoteItem = new PacoteItem(usuarioIDLogado);
		}
		
		public PacoteItem PacoteItem{
			get{ return pacoteItem; }
		}

		/// <summary>
		/// Retorna um IBaseBD de PacoteItem especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					pacoteItem.Ler(id);
					return pacoteItem;
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
					sql = "SELECT ID FROM tPacoteItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tPacoteItem";
				
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
					sql = "SELECT ID FROM tPacoteItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tPacoteItem";
				
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
					sql = "SELECT ID FROM xPacoteItem";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xPacoteItem";
				
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
		/// Preenche PacoteItem corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					pacoteItem.Ler(id);
				else	
					pacoteItem.LerBackup(id);
				
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
		
				bool ok = pacoteItem.Excluir();
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
		/// Inseri novo(a) PacoteItem na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = pacoteItem.Inserir();
				if (ok){
					lista.Add(pacoteItem.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de PacoteItem carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("PacoteItem");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EventoID", typeof(int));
					tabela.Columns.Add("ApresentacaoID", typeof(int));
					tabela.Columns.Add("SetorID", typeof(int));
					tabela.Columns.Add("PrecoID", typeof(int));
					tabela.Columns.Add("CortesiaID", typeof(int));
					tabela.Columns.Add("PacoteID", typeof(int));
					tabela.Columns.Add("Quantidade", typeof(int));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= pacoteItem.Control.ID;
						linha["EventoID"]= pacoteItem.EventoID.Valor;
						linha["ApresentacaoID"]= pacoteItem.ApresentacaoID.Valor;
						linha["SetorID"]= pacoteItem.SetorID.Valor;
						linha["PrecoID"]= pacoteItem.PrecoID.Valor;
						linha["CortesiaID"]= pacoteItem.CortesiaID.Valor;
						linha["PacoteID"]= pacoteItem.PacoteID.Valor;
						linha["Quantidade"]= pacoteItem.Quantidade.Valor;
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

				DataTable tabela = new DataTable("RelatorioPacoteItem");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EventoID", typeof(int));
					tabela.Columns.Add("ApresentacaoID", typeof(int));
					tabela.Columns.Add("SetorID", typeof(int));
					tabela.Columns.Add("PrecoID", typeof(int));
					tabela.Columns.Add("CortesiaID", typeof(int));
					tabela.Columns.Add("PacoteID", typeof(int));
					tabela.Columns.Add("Quantidade", typeof(int));

					do{
						DataRow linha = tabela.NewRow();
						linha["EventoID"]= pacoteItem.EventoID.Valor;
						linha["ApresentacaoID"]= pacoteItem.ApresentacaoID.Valor;
						linha["SetorID"]= pacoteItem.SetorID.Valor;
						linha["PrecoID"]= pacoteItem.PrecoID.Valor;
						linha["CortesiaID"]= pacoteItem.CortesiaID.Valor;
						linha["PacoteID"]= pacoteItem.PacoteID.Valor;
						linha["Quantidade"]= pacoteItem.Quantidade.Valor;
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
					case "EventoID":
						sql = "SELECT ID, EventoID FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY EventoID";
						break;
					case "ApresentacaoID":
						sql = "SELECT ID, ApresentacaoID FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY ApresentacaoID";
						break;
					case "SetorID":
						sql = "SELECT ID, SetorID FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY SetorID";
						break;
					case "PrecoID":
						sql = "SELECT ID, PrecoID FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY PrecoID";
						break;
					case "CortesiaID":
						sql = "SELECT ID, CortesiaID FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY CortesiaID";
						break;
					case "PacoteID":
						sql = "SELECT ID, PacoteID FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY PacoteID";
						break;
					case "Quantidade":
						sql = "SELECT ID, Quantidade FROM tPacoteItem WHERE "+FiltroSQL+" ORDER BY Quantidade";
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

	#region "PacoteItemException"
	
	[Serializable]
	public class PacoteItemException : Exception {

		public PacoteItemException() : base (){}

		public PacoteItemException(string msg) : base (msg){}

		public PacoteItemException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}