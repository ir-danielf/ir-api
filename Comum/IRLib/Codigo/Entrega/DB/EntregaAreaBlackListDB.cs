/******************************************************
* Arquivo EntregaAreaBlackListDB.cs
* Gerado em: 30/07/2013
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib {

	#region "EntregaAreaBlackList_B"

	public abstract class EntregaAreaBlackList_B : BaseBD {
	
		public cepinicial CepInicial = new cepinicial();
		public cepfinal CepFinal = new cepfinal();
		public motivo Motivo = new motivo();
		
		public EntregaAreaBlackList_B(){}
					
		// passar o Usuario logado no sistema
		public EntregaAreaBlackList_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de EntregaAreaBlackList
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tEntregaAreaBlackList WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.CepInicial.ValorBD = bd.LerString("CepInicial");
					this.CepFinal.ValorBD = bd.LerString("CepFinal");
					this.Motivo.ValorBD = bd.LerString("Motivo");
				}else{
					this.Limpar();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		/// <summary>
		/// Preenche todos os atributos de EntregaAreaBlackList do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xEntregaAreaBlackList WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.CepInicial.ValorBD = bd.LerString("CepInicial");
					this.CepFinal.ValorBD = bd.LerString("CepFinal");
					this.Motivo.ValorBD = bd.LerString("Motivo");
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		protected void InserirControle(string acao){
		
			try{
			
				System.Text.StringBuilder sql = new System.Text.StringBuilder();
				sql.Append("INSERT INTO cEntregaAreaBlackList (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xEntregaAreaBlackList (ID, Versao, CepInicial, CepFinal, Motivo) ");
				sql.Append("SELECT ID, @V, CepInicial, CepFinal, Motivo FROM tEntregaAreaBlackList WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) EntregaAreaBlackList
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEntregaAreaBlackList");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEntregaAreaBlackList(ID, CepInicial, CepFinal, Motivo) ");
				sql.Append("VALUES (@ID,'@001','@002','@003')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.CepInicial.ValorBD);
				sql.Replace("@002", this.CepFinal.ValorBD);
				sql.Replace("@003", this.Motivo.ValorBD);
				
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
		/// Inserir novo(a) EntregaAreaBlackList
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(BD bd){
		
			try{
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cEntregaAreaBlackList");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tEntregaAreaBlackList(ID, CepInicial, CepFinal, Motivo) ");
				sql.Append("VALUES (@ID,'@001','@002','@003')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.CepInicial.ValorBD);
				sql.Replace("@002", this.CepFinal.ValorBD);
				sql.Replace("@003", this.Motivo.ValorBD);
				
				int x = bd.Executar(sql.ToString());
				
				bool result = (x == 1);
				
				if (result)
					InserirControle("I");
					
				return result;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}
		

		/// <summary>
		/// Atualiza EntregaAreaBlackList
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEntregaAreaBlackList WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEntregaAreaBlackList SET CepInicial = '@001', CepFinal = '@002', Motivo = '@003' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.CepInicial.ValorBD);
				sql.Replace("@002", this.CepFinal.ValorBD);
				sql.Replace("@003", this.Motivo.ValorBD);
				
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
		/// Atualiza EntregaAreaBlackList
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(BD bd){
		
			try{
		
				string sqlVersion = "SELECT MAX(Versao) FROM cEntregaAreaBlackList WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tEntregaAreaBlackList SET CepInicial = '@001', CepFinal = '@002', Motivo = '@003' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.CepInicial.ValorBD);
				sql.Replace("@002", this.CepFinal.ValorBD);
				sql.Replace("@003", this.Motivo.ValorBD);
				
				int x = bd.Executar(sql.ToString());
				
				bool result = (x == 1);
				
				return result;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}
		

		/// <summary>
		/// Exclui EntregaAreaBlackList com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEntregaAreaBlackList WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEntregaAreaBlackList WHERE ID="+id;
				
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
		/// Exclui EntregaAreaBlackList com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(BD bd, int id){
		
			try{
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cEntregaAreaBlackList WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tEntregaAreaBlackList WHERE ID="+id;
				
				int x = bd.Executar(sqlDelete);
				
				bool result = (x == 1);
				
				return result;
			
			}catch(Exception ex){
				throw ex;
			}
			
		}
		
		
		/// <summary>
		/// Exclui EntregaAreaBlackList
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
		
			this.CepInicial.Limpar();
			this.CepFinal.Limpar();
			this.Motivo.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.CepInicial.Desfazer();
			this.CepFinal.Desfazer();
			this.Motivo.Desfazer();
		}

		public class cepinicial : TextProperty{
		
			public override string Nome{
				get{
					return "CepInicial";
				}
			}
			
			public override int Tamanho{
				get{
					return 8;
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
		
		public class cepfinal : TextProperty{
		
			public override string Nome{
				get{
					return "CepFinal";
				}
			}
			
			public override int Tamanho{
				get{
					return 8;
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
		
		public class motivo : TextProperty{
		
			public override string Nome{
				get{
					return "Motivo";
				}
			}
			
			public override int Tamanho{
				get{
					return 100;
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

				DataTable tabela = new DataTable("EntregaAreaBlackList");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("CepInicial", typeof(string));
				tabela.Columns.Add("CepFinal", typeof(string));
				tabela.Columns.Add("Motivo", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "EntregaAreaBlackListLista_B"
	
	public abstract class EntregaAreaBlackListLista_B : BaseLista {
	
		private bool backup = false;
		protected EntregaAreaBlackList entregaAreaBlackList;
	
		// passar o Usuario logado no sistema
		public EntregaAreaBlackListLista_B(){
			entregaAreaBlackList = new EntregaAreaBlackList();
		}
	
		// passar o Usuario logado no sistema
		public EntregaAreaBlackListLista_B(int usuarioIDLogado){
			entregaAreaBlackList = new EntregaAreaBlackList(usuarioIDLogado);
		}
		
		public EntregaAreaBlackList EntregaAreaBlackList{
			get{ return entregaAreaBlackList; }
		}

		/// <summary>
		/// Retorna um IBaseBD de EntregaAreaBlackList especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					entregaAreaBlackList.Ler(id);
					return entregaAreaBlackList;
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
					sql = "SELECT ID FROM tEntregaAreaBlackList";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEntregaAreaBlackList";
				
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
					sql = "SELECT ID FROM tEntregaAreaBlackList";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tEntregaAreaBlackList";
				
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
					sql = "SELECT ID FROM xEntregaAreaBlackList";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xEntregaAreaBlackList";
				
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
		/// Preenche EntregaAreaBlackList corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					entregaAreaBlackList.Ler(id);
				else	
					entregaAreaBlackList.LerBackup(id);
				
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
		
				bool ok = entregaAreaBlackList.Excluir();
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
		/// Inseri novo(a) EntregaAreaBlackList na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = entregaAreaBlackList.Inserir();
				if (ok){
					lista.Add(entregaAreaBlackList.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de EntregaAreaBlackList carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("EntregaAreaBlackList");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("CepInicial", typeof(string));
					tabela.Columns.Add("CepFinal", typeof(string));
					tabela.Columns.Add("Motivo", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= entregaAreaBlackList.Control.ID;
						linha["CepInicial"]= entregaAreaBlackList.CepInicial.Valor;
						linha["CepFinal"]= entregaAreaBlackList.CepFinal.Valor;
						linha["Motivo"]= entregaAreaBlackList.Motivo.Valor;
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

				DataTable tabela = new DataTable("RelatorioEntregaAreaBlackList");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("CepInicial", typeof(string));
					tabela.Columns.Add("CepFinal", typeof(string));
					tabela.Columns.Add("Motivo", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["CepInicial"]= entregaAreaBlackList.CepInicial.Valor;
						linha["CepFinal"]= entregaAreaBlackList.CepFinal.Valor;
						linha["Motivo"]= entregaAreaBlackList.Motivo.Valor;
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
					case "CepInicial":
						sql = "SELECT ID, CepInicial FROM tEntregaAreaBlackList WHERE "+FiltroSQL+" ORDER BY CepInicial";
						break;
					case "CepFinal":
						sql = "SELECT ID, CepFinal FROM tEntregaAreaBlackList WHERE "+FiltroSQL+" ORDER BY CepFinal";
						break;
					case "Motivo":
						sql = "SELECT ID, Motivo FROM tEntregaAreaBlackList WHERE "+FiltroSQL+" ORDER BY Motivo";
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

	#region "EntregaAreaBlackListException"
	
	[Serializable]
	public class EntregaAreaBlackListException : Exception {

		public EntregaAreaBlackListException() : base (){}

		public EntregaAreaBlackListException(string msg) : base (msg){}

		public EntregaAreaBlackListException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}