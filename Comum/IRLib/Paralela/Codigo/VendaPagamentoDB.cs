/******************************************************
* Arquivo VendaPagamentoDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela {

	#region "VendaPagamento_B"

	public abstract class VendaPagamento_B : BaseBD {
	
		public vendaid VendaID = new vendaid();
		public formapagamentoid FormaPagamentoID = new formapagamentoid();
		public valor Valor = new valor();
		
		public VendaPagamento_B(){}
					
		// passar o Usuario logado no sistema
		public VendaPagamento_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de VendaPagamento
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tVendaPagamento WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.VendaID.ValorBD = bd.LerInt("VendaID").ToString();
					this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
					this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
				}else{
					this.Limpar();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		/// <summary>
		/// Preenche todos os atributos de VendaPagamento do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xVendaPagamento WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.VendaID.ValorBD = bd.LerInt("VendaID").ToString();
					this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
					this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		protected void InserirControle(string acao){
		
			try{
			
				System.Text.StringBuilder sql = new System.Text.StringBuilder();
				sql.Append("INSERT INTO cVendaPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xVendaPagamento (ID, Versao, VendaID, FormaPagamentoID, Valor) ");
				sql.Append("SELECT ID, @V, VendaID, FormaPagamentoID, Valor FROM tVendaPagamento WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) VendaPagamento
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cVendaPagamento");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tVendaPagamento(ID, VendaID, FormaPagamentoID, Valor) ");
				sql.Append("VALUES (@ID,@001,@002,'@003')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.VendaID.ValorBD);
				sql.Replace("@002", this.FormaPagamentoID.ValorBD);
				sql.Replace("@003", this.Valor.ValorBD);
				
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
		/// Atualiza VendaPagamento
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cVendaPagamento WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tVendaPagamento SET VendaID = @001, FormaPagamentoID = @002, Valor = '@003' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.VendaID.ValorBD);
				sql.Replace("@002", this.FormaPagamentoID.ValorBD);
				sql.Replace("@003", this.Valor.ValorBD);
				
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
		/// Exclui VendaPagamento com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cVendaPagamento WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tVendaPagamento WHERE ID="+id;
				
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
		/// Exclui VendaPagamento
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
		
			this.VendaID.Limpar();
			this.FormaPagamentoID.Limpar();
			this.Valor.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.VendaID.Desfazer();
			this.FormaPagamentoID.Desfazer();
			this.Valor.Desfazer();
		}

		public class vendaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "VendaID";
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
		
		public class formapagamentoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "FormaPagamentoID";
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
		
		public class valor : NumberProperty{
		
			public override string Nome{
				get{
					return "Valor";
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
		
		/// <summary>
		/// Obtem uma tabela estruturada com todos os campos dessa classe.
		/// </summary>
		/// <returns></returns>
		public static DataTable Estrutura(){
		
			//Isso eh util para desacoplamento.
			//A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.
				
			try{

				DataTable tabela = new DataTable("VendaPagamento");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("VendaID", typeof(int));
				tabela.Columns.Add("FormaPagamentoID", typeof(int));
				tabela.Columns.Add("Valor", typeof(decimal));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "VendaPagamentoLista_B"
	
	public abstract class VendaPagamentoLista_B : BaseLista {
	
		private bool backup = false;
		protected VendaPagamento vendaPagamento;
	
		// passar o Usuario logado no sistema
		public VendaPagamentoLista_B(){
			vendaPagamento = new VendaPagamento();
		}
	
		// passar o Usuario logado no sistema
		public VendaPagamentoLista_B(int usuarioIDLogado){
			vendaPagamento = new VendaPagamento(usuarioIDLogado);
		}
		
		public VendaPagamento VendaPagamento{
			get{ return vendaPagamento; }
		}

		/// <summary>
		/// Retorna um IBaseBD de VendaPagamento especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					vendaPagamento.Ler(id);
					return vendaPagamento;
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
					sql = "SELECT ID FROM tVendaPagamento";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tVendaPagamento";
				
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
					sql = "SELECT ID FROM tVendaPagamento";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tVendaPagamento";
				
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
					sql = "SELECT ID FROM xVendaPagamento";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xVendaPagamento";
				
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
		/// Preenche VendaPagamento corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					vendaPagamento.Ler(id);
				else	
					vendaPagamento.LerBackup(id);
				
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
		
				bool ok = vendaPagamento.Excluir();
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
		/// Inseri novo(a) VendaPagamento na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = vendaPagamento.Inserir();
				if (ok){
					lista.Add(vendaPagamento.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de VendaPagamento carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("VendaPagamento");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("VendaID", typeof(int));
					tabela.Columns.Add("FormaPagamentoID", typeof(int));
					tabela.Columns.Add("Valor", typeof(decimal));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= vendaPagamento.Control.ID;
						linha["VendaID"]= vendaPagamento.VendaID.Valor;
						linha["FormaPagamentoID"]= vendaPagamento.FormaPagamentoID.Valor;
						linha["Valor"]= vendaPagamento.Valor.Valor;
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

				DataTable tabela = new DataTable("RelatorioVendaPagamento");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("VendaID", typeof(int));
					tabela.Columns.Add("FormaPagamentoID", typeof(int));
					tabela.Columns.Add("Valor", typeof(decimal));

					do{
						DataRow linha = tabela.NewRow();
						linha["VendaID"]= vendaPagamento.VendaID.Valor;
						linha["FormaPagamentoID"]= vendaPagamento.FormaPagamentoID.Valor;
						linha["Valor"]= vendaPagamento.Valor.Valor;
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
					case "VendaID":
						sql = "SELECT ID, VendaID FROM tVendaPagamento WHERE "+FiltroSQL+" ORDER BY VendaID";
						break;
					case "FormaPagamentoID":
						sql = "SELECT ID, FormaPagamentoID FROM tVendaPagamento WHERE "+FiltroSQL+" ORDER BY FormaPagamentoID";
						break;
					case "Valor":
						sql = "SELECT ID, Valor FROM tVendaPagamento WHERE "+FiltroSQL+" ORDER BY Valor";
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

	#region "VendaPagamentoException"
	
	[Serializable]
	public class VendaPagamentoException : Exception {

		public VendaPagamentoException() : base (){}

		public VendaPagamentoException(string msg) : base (msg){}

		public VendaPagamentoException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}