/******************************************************
* Arquivo ComandaDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela {

	#region "Comanda_B"

	public abstract class Comanda_B : BaseBD {
	
		public garconid GarconID = new garconid();
		public vendaid VendaID = new vendaid();
		public caixaid CaixaID = new caixaid();
		public ordem Ordem = new ordem();
		public ingressoid IngressoID = new ingressoid();
		
		public Comanda_B(){}
					
		// passar o Usuario logado no sistema
		public Comanda_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de Comanda
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tComanda WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.GarconID.ValorBD = bd.LerInt("GarconID").ToString();
					this.VendaID.ValorBD = bd.LerInt("VendaID").ToString();
					this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
					this.Ordem.ValorBD = bd.LerString("Ordem");
					this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
				}else{
					this.Limpar();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		/// <summary>
		/// Preenche todos os atributos de Comanda do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xComanda WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.GarconID.ValorBD = bd.LerInt("GarconID").ToString();
					this.VendaID.ValorBD = bd.LerInt("VendaID").ToString();
					this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
					this.Ordem.ValorBD = bd.LerString("Ordem");
					this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
				}
				bd.Fechar();
				
			}catch(Exception ex){
				throw ex;
			}
						
		}

		protected void InserirControle(string acao){
		
			try{
			
				System.Text.StringBuilder sql = new System.Text.StringBuilder();
				sql.Append("INSERT INTO cComanda (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xComanda (ID, Versao, GarconID, VendaID, CaixaID, Ordem, IngressoID) ");
				sql.Append("SELECT ID, @V, GarconID, VendaID, CaixaID, Ordem, IngressoID FROM tComanda WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) Comanda
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cComanda");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tComanda(ID, GarconID, VendaID, CaixaID, Ordem, IngressoID) ");
				sql.Append("VALUES (@ID,@001,@002,@003,'@004',@005)");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.GarconID.ValorBD);
				sql.Replace("@002", this.VendaID.ValorBD);
				sql.Replace("@003", this.CaixaID.ValorBD);
				sql.Replace("@004", this.Ordem.ValorBD);
				sql.Replace("@005", this.IngressoID.ValorBD);
				
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
		/// Atualiza Comanda
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cComanda WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tComanda SET GarconID = @001, VendaID = @002, CaixaID = @003, Ordem = '@004', IngressoID = @005 ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.GarconID.ValorBD);
				sql.Replace("@002", this.VendaID.ValorBD);
				sql.Replace("@003", this.CaixaID.ValorBD);
				sql.Replace("@004", this.Ordem.ValorBD);
				sql.Replace("@005", this.IngressoID.ValorBD);
				
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
		/// Exclui Comanda com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cComanda WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tComanda WHERE ID="+id;
				
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
		/// Exclui Comanda
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
		
			this.GarconID.Limpar();
			this.VendaID.Limpar();
			this.CaixaID.Limpar();
			this.Ordem.Limpar();
			this.IngressoID.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.GarconID.Desfazer();
			this.VendaID.Desfazer();
			this.CaixaID.Desfazer();
			this.Ordem.Desfazer();
			this.IngressoID.Desfazer();
		}

		public class garconid : IntegerProperty{
		
			public override string Nome{
				get{
					return "GarconID";
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
		
		public class caixaid : IntegerProperty{
		
			public override string Nome{
				get{
					return "CaixaID";
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
		
		public class ingressoid : IntegerProperty{
		
			public override string Nome{
				get{
					return "IngressoID";
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

				DataTable tabela = new DataTable("Comanda");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("GarconID", typeof(int));
				tabela.Columns.Add("VendaID", typeof(int));
				tabela.Columns.Add("CaixaID", typeof(int));
				tabela.Columns.Add("Ordem", typeof(string));
				tabela.Columns.Add("IngressoID", typeof(int));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
		public abstract int Ultima();

		public abstract DataTable estruturaItensComanda();

		public abstract DataTable Itens(int garconid, int apresentacaoid);

		public abstract int ObterVendaID(string ingresso);

		public abstract DataTable Itens();

		public abstract DataTable Itens(string ordem);

		public abstract DataTable Ordens(string ingresso);

		public abstract void FecharConta(int ingresso, int vendaid);

		public abstract decimal ValorConta(int ingresso);

		public abstract decimal ValorConta();

		public abstract DataTable Ordens(int garconid);

	}
	#endregion

	#region "ComandaLista_B"
	
	public abstract class ComandaLista_B : BaseLista {
	
		private bool backup = false;
		protected Comanda comanda;
	
		// passar o Usuario logado no sistema
		public ComandaLista_B(){
			comanda = new Comanda();
		}
	
		// passar o Usuario logado no sistema
		public ComandaLista_B(int usuarioIDLogado){
			comanda = new Comanda(usuarioIDLogado);
		}
		
		public Comanda Comanda{
			get{ return comanda; }
		}

		/// <summary>
		/// Retorna um IBaseBD de Comanda especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					comanda.Ler(id);
					return comanda;
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
					sql = "SELECT ID FROM tComanda";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tComanda";
				
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
					sql = "SELECT ID FROM tComanda";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tComanda";
				
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
					sql = "SELECT ID FROM xComanda";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xComanda";
				
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
		/// Preenche Comanda corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					comanda.Ler(id);
				else	
					comanda.LerBackup(id);
				
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
		
				bool ok = comanda.Excluir();
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
		/// Inseri novo(a) Comanda na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = comanda.Inserir();
				if (ok){
					lista.Add(comanda.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de Comanda carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("Comanda");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("GarconID", typeof(int));
					tabela.Columns.Add("VendaID", typeof(int));
					tabela.Columns.Add("CaixaID", typeof(int));
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("IngressoID", typeof(int));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= comanda.Control.ID;
						linha["GarconID"]= comanda.GarconID.Valor;
						linha["VendaID"]= comanda.VendaID.Valor;
						linha["CaixaID"]= comanda.CaixaID.Valor;
						linha["Ordem"]= comanda.Ordem.Valor;
						linha["IngressoID"]= comanda.IngressoID.Valor;
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

				DataTable tabela = new DataTable("RelatorioComanda");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("GarconID", typeof(int));
					tabela.Columns.Add("VendaID", typeof(int));
					tabela.Columns.Add("CaixaID", typeof(int));
					tabela.Columns.Add("Ordem", typeof(string));
					tabela.Columns.Add("IngressoID", typeof(int));

					do{
						DataRow linha = tabela.NewRow();
						linha["GarconID"]= comanda.GarconID.Valor;
						linha["VendaID"]= comanda.VendaID.Valor;
						linha["CaixaID"]= comanda.CaixaID.Valor;
						linha["Ordem"]= comanda.Ordem.Valor;
						linha["IngressoID"]= comanda.IngressoID.Valor;
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
					case "GarconID":
						sql = "SELECT ID, GarconID FROM tComanda WHERE "+FiltroSQL+" ORDER BY GarconID";
						break;
					case "VendaID":
						sql = "SELECT ID, VendaID FROM tComanda WHERE "+FiltroSQL+" ORDER BY VendaID";
						break;
					case "CaixaID":
						sql = "SELECT ID, CaixaID FROM tComanda WHERE "+FiltroSQL+" ORDER BY CaixaID";
						break;
					case "Ordem":
						sql = "SELECT ID, Ordem FROM tComanda WHERE "+FiltroSQL+" ORDER BY Ordem";
						break;
					case "IngressoID":
						sql = "SELECT ID, IngressoID FROM tComanda WHERE "+FiltroSQL+" ORDER BY IngressoID";
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

	#region "ComandaException"
	
	[Serializable]
	public class ComandaException : Exception {

		public ComandaException() : base (){}

		public ComandaException(string msg) : base (msg){}

		public ComandaException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}