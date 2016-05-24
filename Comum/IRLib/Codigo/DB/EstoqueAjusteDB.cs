/******************************************************
* Arquivo EstoqueAjusteDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "EstoqueAjuste_B"

	public abstract class EstoqueAjuste_B : BaseBD {
	
		public empresaid EmpresaID = new empresaid();
		public ordem Ordem = new ordem();
		public responsavelid ResponsavelID = new responsavelid();
		public data Data = new data();
		public obs Obs = new obs();
		
		public EstoqueAjuste_B(){}
					
		// passar o Usuario logado no sistema
		public EstoqueAjuste_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de EstoqueAjuste
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tEstoqueAjuste WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Ordem.ValorBD = bd.LerString("Ordem");
					this.ResponsavelID.ValorBD = bd.LerInt("ResponsavelID").ToString();
					this.Data.ValorBD = bd.LerString("Data");
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
		/// Preenche todos os atributos de EstoqueAjuste do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xEstoqueAjuste WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Ordem.ValorBD = bd.LerString("Ordem");
					this.ResponsavelID.ValorBD = bd.LerInt("ResponsavelID").ToString();
					this.Data.ValorBD = bd.LerString("Data");
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
				sql.Append("INSERT INTO cEstoqueAjuste (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xEstoqueAjuste (ID, Versao, EmpresaID, Ordem, ResponsavelID, Data, Obs) ");
				sql.Append("SELECT ID, @V, EmpresaID, Ordem, ResponsavelID, Data, Obs FROM tEstoqueAjuste WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) EstoqueAjuste
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEstoqueAjuste");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEstoqueAjuste(ID, EmpresaID, Ordem, ResponsavelID, Data, Obs) ");
				sql.Append("VALUES (@ID,@001,'@002',@003,'@004','@005')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Ordem.ValorBD);
				sql.Replace("@003", this.ResponsavelID.ValorBD);
				sql.Replace("@004", this.Data.ValorBD);
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
		/// Atualiza EstoqueAjuste
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEstoqueAjuste WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEstoqueAjuste SET EmpresaID = @001, Ordem = '@002', ResponsavelID = @003, Data = '@004', Obs = '@005' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Ordem.ValorBD);
				sql.Replace("@003", this.ResponsavelID.ValorBD);
				sql.Replace("@004", this.Data.ValorBD);
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
		/// Exclui EstoqueAjuste com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEstoqueAjuste WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEstoqueAjuste WHERE ID="+id;
				
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
		/// Exclui EstoqueAjuste
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
			this.ResponsavelID.Limpar();
			this.Data.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EmpresaID.Desfazer();
			this.Ordem.Desfazer();
			this.ResponsavelID.Desfazer();
			this.Data.Desfazer();
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

				DataTable tabela = new DataTable("EstoqueAjuste");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EmpresaID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));
				tabela.Columns.Add("ResponsavelID", typeof(int));
				tabela.Columns.Add("Data", typeof(DateTime));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
		public abstract DataTable Itens();

	}
	#endregion

	#region "EstoqueAjusteLista_B"
	
	public abstract class EstoqueAjusteLista_B : BaseLista {
	
		private bool backup = false;
		protected EstoqueAjuste estoqueAjuste;
	
		// passar o Usuario logado no sistema
		public EstoqueAjusteLista_B(){
			estoqueAjuste = new EstoqueAjuste();
		}
	
		// passar o Usuario logado no sistema
		public EstoqueAjusteLista_B(int usuarioIDLogado){
			estoqueAjuste = new EstoqueAjuste(usuarioIDLogado);
		}
		
		public EstoqueAjuste EstoqueAjuste{
			get{ return estoqueAjuste; }
		}

		/// <summary>
		/// Retorna um IBaseBD de EstoqueAjuste especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					estoqueAjuste.Ler(id);
					return estoqueAjuste;
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
					sql = "SELECT ID FROM tEstoqueAjuste";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueAjuste";
				
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
					sql = "SELECT ID FROM tEstoqueAjuste";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEstoqueAjuste";
				
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
					sql = "SELECT ID FROM xEstoqueAjuste";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xEstoqueAjuste";
				
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
		/// Preenche EstoqueAjuste corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					estoqueAjuste.Ler(id);
				else	
					estoqueAjuste.LerBackup(id);
				
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
		
				bool ok = estoqueAjuste.Excluir();
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
		/// Inseri novo(a) EstoqueAjuste na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = estoqueAjuste.Inserir();
				if (ok){
					lista.Add(estoqueAjuste.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de EstoqueAjuste carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("EstoqueAjuste");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("ResponsavelID", typeof(int));
					tabela.Columns.Add("Data", typeof(DateTime));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= estoqueAjuste.Control.ID;
						linha["EmpresaID"]= estoqueAjuste.EmpresaID.Valor;
						linha["Ordem"]= estoqueAjuste.Ordem.Valor;
						linha["ResponsavelID"]= estoqueAjuste.ResponsavelID.Valor;
						linha["Data"]= estoqueAjuste.Data.Valor;
						linha["Obs"]= estoqueAjuste.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioEstoqueAjuste");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("ResponsavelID", typeof(int));
					tabela.Columns.Add("Data", typeof(DateTime));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EmpresaID"]= estoqueAjuste.EmpresaID.Valor;
						linha["Ordem"]= estoqueAjuste.Ordem.Valor;
						linha["ResponsavelID"]= estoqueAjuste.ResponsavelID.Valor;
						linha["Data"]= estoqueAjuste.Data.Valor;
						linha["Obs"]= estoqueAjuste.Obs.Valor;
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
						sql = "SELECT ID, EmpresaID FROM tEstoqueAjuste WHERE "+FiltroSQL+" ORDER BY EmpresaID";
						break;
					case "Ordem":
						sql = "SELECT ID, Ordem FROM tEstoqueAjuste WHERE "+FiltroSQL+" ORDER BY Ordem";
						break;
					case "ResponsavelID":
						sql = "SELECT ID, ResponsavelID FROM tEstoqueAjuste WHERE "+FiltroSQL+" ORDER BY ResponsavelID";
						break;
					case "Data":
						sql = "SELECT ID, Data FROM tEstoqueAjuste WHERE "+FiltroSQL+" ORDER BY Data";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tEstoqueAjuste WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "EstoqueAjusteException"
	
	[Serializable]
	public class EstoqueAjusteException : Exception {

		public EstoqueAjusteException() : base (){}

		public EstoqueAjusteException(string msg) : base (msg){}

		public EstoqueAjusteException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}