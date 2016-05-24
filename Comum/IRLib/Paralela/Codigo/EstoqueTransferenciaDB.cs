/******************************************************
* Arquivo EstoqueTransferenciaDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela {

	#region "EstoqueTransferencia_B"

	public abstract class EstoqueTransferencia_B : BaseBD {
	
		public empresaid EmpresaID = new empresaid();
		public ordem Ordem = new ordem();
		public data Data = new data();
		public responsavelid ResponsavelID = new responsavelid();
		public obs Obs = new obs();
		
		public EstoqueTransferencia_B(){}
					
		// passar o Usuario logado no sistema
		public EstoqueTransferencia_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de EstoqueTransferencia
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tEstoqueTransferencia WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Ordem.ValorBD = bd.LerString("Ordem");
					this.Data.ValorBD = bd.LerString("Data");
					this.ResponsavelID.ValorBD = bd.LerInt("ResponsavelID").ToString();
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
		/// Preenche todos os atributos de EstoqueTransferencia do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xEstoqueTransferencia WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Ordem.ValorBD = bd.LerString("Ordem");
					this.Data.ValorBD = bd.LerString("Data");
					this.ResponsavelID.ValorBD = bd.LerInt("ResponsavelID").ToString();
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
				sql.Append("INSERT INTO cEstoqueTransferencia (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xEstoqueTransferencia (ID, Versao, EmpresaID, Ordem, Data, ResponsavelID, Obs) ");
				sql.Append("SELECT ID, @V, EmpresaID, Ordem, Data, ResponsavelID, Obs FROM tEstoqueTransferencia WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) EstoqueTransferencia
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEstoqueTransferencia");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEstoqueTransferencia(ID, EmpresaID, Ordem, Data, ResponsavelID, Obs) ");
				sql.Append("VALUES (@ID,@001,'@002','@003',@004,'@005')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Ordem.ValorBD);
				sql.Replace("@003", this.Data.ValorBD);
				sql.Replace("@004", this.ResponsavelID.ValorBD);
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
		/// Atualiza EstoqueTransferencia
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEstoqueTransferencia WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEstoqueTransferencia SET EmpresaID = @001, Ordem = '@002', Data = '@003', ResponsavelID = @004, Obs = '@005' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Ordem.ValorBD);
				sql.Replace("@003", this.Data.ValorBD);
				sql.Replace("@004", this.ResponsavelID.ValorBD);
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
		/// Exclui EstoqueTransferencia com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEstoqueTransferencia WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEstoqueTransferencia WHERE ID="+id;
				
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
		/// Exclui EstoqueTransferencia
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
		
			this.EmpresaID.Limpar();
			this.Ordem.Limpar();
			this.Data.Limpar();
			this.ResponsavelID.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EmpresaID.Desfazer();
			this.Ordem.Desfazer();
			this.Data.Desfazer();
			this.ResponsavelID.Desfazer();
			this.Obs.Desfazer();
		}

		public class empresaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "EmpresaID";
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
		
		public class ordem : TextProperty{
		
			public override string Nome{
				get{
					return "Ordem";
				}
			}
			
			public override int Tamanho{
				get{
					return 10;
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
		
		public class data : DateProperty{
		
			public override string Nome{
				get{
					return "Data";
				}
			}
			
			public override int Tamanho{
				get{
					return 0;
				}
			}
			
			public override DateTime Valor{
				get{
					return base.Valor;
				}
				set{
					base.Valor = value;
				}
			}
			
			public override string ToString(){
				return base.Valor.ToString("dd/MM/yyyy");
			}
			
		}
		
		public class responsavelid : IntegerProperty{
		
			public override string Nome{
				get{
					return "ResponsavelID";
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

				DataTable tabela = new DataTable("EstoqueTransferencia");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EmpresaID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));
				tabela.Columns.Add("Data", typeof(DateTime));
				tabela.Columns.Add("ResponsavelID", typeof(int));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
		public abstract DataTable Itens();

	}
	#endregion

	#region "EstoqueTransferenciaLista_B"
	
	public abstract class EstoqueTransferenciaLista_B : BaseLista {
	
		private bool backup = false;
		protected EstoqueTransferencia estoqueTransferencia;
	
		// passar o Usuario logado no sistema
		public EstoqueTransferenciaLista_B(){
			estoqueTransferencia = new EstoqueTransferencia();
		}
	
		// passar o Usuario logado no sistema
		public EstoqueTransferenciaLista_B(int usuarioIDLogado){
			estoqueTransferencia = new EstoqueTransferencia(usuarioIDLogado);
		}
		
		public EstoqueTransferencia EstoqueTransferencia{
			get{ return estoqueTransferencia; }
		}

		/// <summary>
		/// Retorna um IBaseBD de EstoqueTransferencia especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					estoqueTransferencia.Ler(id);
					return estoqueTransferencia;
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
					sql = "SELECT ID FROM tEstoqueTransferencia";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueTransferencia";
				
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
					sql = "SELECT ID FROM tEstoqueTransferencia";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueTransferencia";
				
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
					sql = "SELECT ID FROM xEstoqueTransferencia";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xEstoqueTransferencia";
				
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
		/// Preenche EstoqueTransferencia corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					estoqueTransferencia.Ler(id);
				else	
					estoqueTransferencia.LerBackup(id);
				
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
		
				bool ok = estoqueTransferencia.Excluir();
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
		/// Inseri novo(a) EstoqueTransferencia na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = estoqueTransferencia.Inserir();
				if (ok){
					lista.Add(estoqueTransferencia.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de EstoqueTransferencia carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("EstoqueTransferencia");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("Data", typeof(DateTime));
					tabela.Columns.Add("ResponsavelID", typeof(int));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= estoqueTransferencia.Control.ID;
						linha["EmpresaID"]= estoqueTransferencia.EmpresaID.Valor;
						linha["Ordem"]= estoqueTransferencia.Ordem.Valor;
						linha["Data"]= estoqueTransferencia.Data.Valor;
						linha["ResponsavelID"]= estoqueTransferencia.ResponsavelID.Valor;
						linha["Obs"]= estoqueTransferencia.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioEstoqueTransferencia");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("Data", typeof(DateTime));
					tabela.Columns.Add("ResponsavelID", typeof(int));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EmpresaID"]= estoqueTransferencia.EmpresaID.Valor;
						linha["Ordem"]= estoqueTransferencia.Ordem.Valor;
						linha["Data"]= estoqueTransferencia.Data.Valor;
						linha["ResponsavelID"]= estoqueTransferencia.ResponsavelID.Valor;
						linha["Obs"]= estoqueTransferencia.Obs.Valor;
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
					case "EmpresaID":
						sql = "SELECT ID, EmpresaID FROM tEstoqueTransferencia WHERE "+FiltroSQL+" ORDER BY EmpresaID";
						break;
					case "Ordem":
						sql = "SELECT ID, Ordem FROM tEstoqueTransferencia WHERE "+FiltroSQL+" ORDER BY Ordem";
						break;
					case "Data":
						sql = "SELECT ID, Data FROM tEstoqueTransferencia WHERE "+FiltroSQL+" ORDER BY Data";
						break;
					case "ResponsavelID":
						sql = "SELECT ID, ResponsavelID FROM tEstoqueTransferencia WHERE "+FiltroSQL+" ORDER BY ResponsavelID";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tEstoqueTransferencia WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "EstoqueTransferenciaException"
	
	[Serializable]
	public class EstoqueTransferenciaException : Exception {

		public EstoqueTransferenciaException() : base (){}

		public EstoqueTransferenciaException(string msg) : base (msg){}

		public EstoqueTransferenciaException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}