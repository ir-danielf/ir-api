/******************************************************
* Arquivo VersaoDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela {

	#region "Versao_B"

	public abstract class Versao_B : BaseBD {
	
		public major Major = new major();
		public minor Minor = new minor();
		public atualizacaoobrigatoria AtualizacaoObrigatoria = new atualizacaoobrigatoria();
		public avisacliente AvisaCliente = new avisacliente();
		public descricao Descricao = new descricao();
		public obs Obs = new obs();
		
		public Versao_B(){}
					
		// passar o Usuario logado no sistema
		public Versao_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de Versao
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tVersao WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.Major.ValorBD = bd.LerInt("Major").ToString();
					this.Minor.ValorBD = bd.LerInt("Minor").ToString();
					this.AtualizacaoObrigatoria.ValorBD = bd.LerString("AtualizacaoObrigatoria");
					this.AvisaCliente.ValorBD = bd.LerString("AvisaCliente");
					this.Descricao.ValorBD = bd.LerString("Descricao");
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
		/// Preenche todos os atributos de Versao do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xVersao WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.Major.ValorBD = bd.LerInt("Major").ToString();
					this.Minor.ValorBD = bd.LerInt("Minor").ToString();
					this.AtualizacaoObrigatoria.ValorBD = bd.LerString("AtualizacaoObrigatoria");
					this.AvisaCliente.ValorBD = bd.LerString("AvisaCliente");
					this.Descricao.ValorBD = bd.LerString("Descricao");
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
				sql.Append("INSERT INTO cVersao (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xVersao (ID, Versao, Major, Minor, AtualizacaoObrigatoria, AvisaCliente, Descricao, Obs) ");
				sql.Append("SELECT ID, @V, Major, Minor, AtualizacaoObrigatoria, AvisaCliente, Descricao, Obs FROM tVersao WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) Versao
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cVersao");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tVersao(ID, Major, Minor, AtualizacaoObrigatoria, AvisaCliente, Descricao, Obs) ");
				sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.Major.ValorBD);
				sql.Replace("@002", this.Minor.ValorBD);
				sql.Replace("@003", this.AtualizacaoObrigatoria.ValorBD);
				sql.Replace("@004", this.AvisaCliente.ValorBD);
				sql.Replace("@005", this.Descricao.ValorBD);
				sql.Replace("@006", this.Obs.ValorBD);
				
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
		/// Atualiza Versao
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cVersao WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tVersao SET Major = @001, Minor = @002, AtualizacaoObrigatoria = '@003', AvisaCliente = '@004', Descricao = '@005', Obs = '@006' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.Major.ValorBD);
				sql.Replace("@002", this.Minor.ValorBD);
				sql.Replace("@003", this.AtualizacaoObrigatoria.ValorBD);
				sql.Replace("@004", this.AvisaCliente.ValorBD);
				sql.Replace("@005", this.Descricao.ValorBD);
				sql.Replace("@006", this.Obs.ValorBD);
				
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
		/// Exclui Versao com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cVersao WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tVersao WHERE ID="+id;
				
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
		/// Exclui Versao
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
		
			this.Major.Limpar();
			this.Minor.Limpar();
			this.AtualizacaoObrigatoria.Limpar();
			this.AvisaCliente.Limpar();
			this.Descricao.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.Major.Desfazer();
			this.Minor.Desfazer();
			this.AtualizacaoObrigatoria.Desfazer();
			this.AvisaCliente.Desfazer();
			this.Descricao.Desfazer();
			this.Obs.Desfazer();
		}

		public class major : IntegerProperty{
		
			public override string Nome{
				get{
					return "Major";
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
		
		public class minor : IntegerProperty{
		
			public override string Nome{
				get{
					return "Minor";
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
		
		public class atualizacaoobrigatoria : BooleanProperty{
		
			public override string Nome{
				get{
					return "AtualizacaoObrigatoria";
				}
			}
			
			public override int Tamanho{
				get{
					return 0;
				}
			}
			
			public override bool Valor{
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
		
		public class avisacliente : BooleanProperty{
		
			public override string Nome{
				get{
					return "AvisaCliente";
				}
			}
			
			public override int Tamanho{
				get{
					return 0;
				}
			}
			
			public override bool Valor{
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
		
		public class descricao : TextProperty{
		
			public override string Nome{
				get{
					return "Descricao";
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

				DataTable tabela = new DataTable("Versao");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Major", typeof(int));
				tabela.Columns.Add("Minor", typeof(int));
				tabela.Columns.Add("AtualizacaoObrigatoria", typeof(bool));
				tabela.Columns.Add("AvisaCliente", typeof(bool));
				tabela.Columns.Add("Descricao", typeof(string));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
		public abstract bool Atualizada(int major, int minor);

		public abstract DataTable Todas();

		public abstract void UltimaVersao();

	}
	#endregion

	#region "VersaoLista_B"
	
	public abstract class VersaoLista_B : BaseLista {
	
		private bool backup = false;
		protected Versao versao;
	
		// passar o Usuario logado no sistema
		public VersaoLista_B(){
			versao = new Versao();
		}
	
		// passar o Usuario logado no sistema
		public VersaoLista_B(int usuarioIDLogado){
			versao = new Versao(usuarioIDLogado);
		}
		
		public Versao Versao{
			get{ return versao; }
		}

		/// <summary>
		/// Retorna um IBaseBD de Versao especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					versao.Ler(id);
					return versao;
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
					sql = "SELECT ID FROM tVersao";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tVersao";
				
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
					sql = "SELECT ID FROM tVersao";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tVersao";
				
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
					sql = "SELECT ID FROM xVersao";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xVersao";
				
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
		/// Preenche Versao corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					versao.Ler(id);
				else	
					versao.LerBackup(id);
				
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
		
				bool ok = versao.Excluir();
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
		/// Inseri novo(a) Versao na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = versao.Inserir();
				if (ok){
					lista.Add(versao.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de Versao carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("Versao");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("Major", typeof(int));
					tabela.Columns.Add("Minor", typeof(int));
					tabela.Columns.Add("AtualizacaoObrigatoria", typeof(bool));
					tabela.Columns.Add("AvisaCliente", typeof(bool));
					tabela.Columns.Add("Descricao", typeof(string));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= versao.Control.ID;
						linha["Major"]= versao.Major.Valor;
						linha["Minor"]= versao.Minor.Valor;
						linha["AtualizacaoObrigatoria"]= versao.AtualizacaoObrigatoria.Valor;
						linha["AvisaCliente"]= versao.AvisaCliente.Valor;
						linha["Descricao"]= versao.Descricao.Valor;
						linha["Obs"]= versao.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioVersao");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Major", typeof(int));
					tabela.Columns.Add("Minor", typeof(int));
					tabela.Columns.Add("AtualizacaoObrigatoria", typeof(bool));
					tabela.Columns.Add("AvisaCliente", typeof(bool));
					tabela.Columns.Add("Descricao", typeof(string));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["Major"]= versao.Major.Valor;
						linha["Minor"]= versao.Minor.Valor;
						linha["AtualizacaoObrigatoria"]= versao.AtualizacaoObrigatoria.Valor;
						linha["AvisaCliente"]= versao.AvisaCliente.Valor;
						linha["Descricao"]= versao.Descricao.Valor;
						linha["Obs"]= versao.Obs.Valor;
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
					case "Major":
						sql = "SELECT ID, Major FROM tVersao WHERE "+FiltroSQL+" ORDER BY Major";
						break;
					case "Minor":
						sql = "SELECT ID, Minor FROM tVersao WHERE "+FiltroSQL+" ORDER BY Minor";
						break;
					case "AtualizacaoObrigatoria":
						sql = "SELECT ID, AtualizacaoObrigatoria FROM tVersao WHERE "+FiltroSQL+" ORDER BY AtualizacaoObrigatoria";
						break;
					case "AvisaCliente":
						sql = "SELECT ID, AvisaCliente FROM tVersao WHERE "+FiltroSQL+" ORDER BY AvisaCliente";
						break;
					case "Descricao":
						sql = "SELECT ID, Descricao FROM tVersao WHERE "+FiltroSQL+" ORDER BY Descricao";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tVersao WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "VersaoException"
	
	[Serializable]
	public class VersaoException : Exception {

		public VersaoException() : base (){}

		public VersaoException(string msg) : base (msg){}

		public VersaoException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}