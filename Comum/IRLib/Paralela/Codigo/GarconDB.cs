/******************************************************
* Arquivo GarconDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela {

	#region "Garcon_B"

	public abstract class Garcon_B : BaseBD {
	
		public empresaid EmpresaID = new empresaid();
		public nome Nome = new nome();
		public sexo Sexo = new sexo();
		public comissao Comissao = new comissao();
		public endereco Endereco = new endereco();
		public cidade Cidade = new cidade();
		public estado Estado = new estado();
		public cep CEP = new cep();
		public dddtelefone DDDTelefone = new dddtelefone();
		public telefone Telefone = new telefone();
		public email Email = new email();
		public obs Obs = new obs();
		
		public Garcon_B(){}
					
		// passar o Usuario logado no sistema
		public Garcon_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de Garcon
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tGarcon WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Nome.ValorBD = bd.LerString("Nome");
					this.Sexo.ValorBD = bd.LerString("Sexo");
					this.Comissao.ValorBD = bd.LerDecimal("Comissao").ToString();
					this.Endereco.ValorBD = bd.LerString("Endereco");
					this.Cidade.ValorBD = bd.LerString("Cidade");
					this.Estado.ValorBD = bd.LerString("Estado");
					this.CEP.ValorBD = bd.LerString("CEP");
					this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
					this.Telefone.ValorBD = bd.LerString("Telefone");
					this.Email.ValorBD = bd.LerString("Email");
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
		/// Preenche todos os atributos de Garcon do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xGarcon WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Nome.ValorBD = bd.LerString("Nome");
					this.Sexo.ValorBD = bd.LerString("Sexo");
					this.Comissao.ValorBD = bd.LerDecimal("Comissao").ToString();
					this.Endereco.ValorBD = bd.LerString("Endereco");
					this.Cidade.ValorBD = bd.LerString("Cidade");
					this.Estado.ValorBD = bd.LerString("Estado");
					this.CEP.ValorBD = bd.LerString("CEP");
					this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
					this.Telefone.ValorBD = bd.LerString("Telefone");
					this.Email.ValorBD = bd.LerString("Email");
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
				sql.Append("INSERT INTO cGarcon (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xGarcon (ID, Versao, EmpresaID, Nome, Sexo, Comissao, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, Obs) ");
				sql.Append("SELECT ID, @V, EmpresaID, Nome, Sexo, Comissao, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, Obs FROM tGarcon WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) Garcon
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cGarcon");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tGarcon(ID, EmpresaID, Nome, Sexo, Comissao, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, Obs) ");
				sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Nome.ValorBD);
				sql.Replace("@003", this.Sexo.ValorBD);
				sql.Replace("@004", this.Comissao.ValorBD);
				sql.Replace("@005", this.Endereco.ValorBD);
				sql.Replace("@006", this.Cidade.ValorBD);
				sql.Replace("@007", this.Estado.ValorBD);
				sql.Replace("@008", this.CEP.ValorBD);
				sql.Replace("@009", this.DDDTelefone.ValorBD);
				sql.Replace("@010", this.Telefone.ValorBD);
				sql.Replace("@011", this.Email.ValorBD);
				sql.Replace("@012", this.Obs.ValorBD);
				
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
		/// Atualiza Garcon
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cGarcon WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tGarcon SET EmpresaID = @001, Nome = '@002', Sexo = '@003', Comissao = '@004', Endereco = '@005', Cidade = '@006', Estado = '@007', CEP = '@008', DDDTelefone = '@009', Telefone = '@010', Email = '@011', Obs = '@012' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Nome.ValorBD);
				sql.Replace("@003", this.Sexo.ValorBD);
				sql.Replace("@004", this.Comissao.ValorBD);
				sql.Replace("@005", this.Endereco.ValorBD);
				sql.Replace("@006", this.Cidade.ValorBD);
				sql.Replace("@007", this.Estado.ValorBD);
				sql.Replace("@008", this.CEP.ValorBD);
				sql.Replace("@009", this.DDDTelefone.ValorBD);
				sql.Replace("@010", this.Telefone.ValorBD);
				sql.Replace("@011", this.Email.ValorBD);
				sql.Replace("@012", this.Obs.ValorBD);
				
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
		/// Exclui Garcon com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cGarcon WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tGarcon WHERE ID="+id;
				
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
		/// Exclui Garcon
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
			this.Nome.Limpar();
			this.Sexo.Limpar();
			this.Comissao.Limpar();
			this.Endereco.Limpar();
			this.Cidade.Limpar();
			this.Estado.Limpar();
			this.CEP.Limpar();
			this.DDDTelefone.Limpar();
			this.Telefone.Limpar();
			this.Email.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EmpresaID.Desfazer();
			this.Nome.Desfazer();
			this.Sexo.Desfazer();
			this.Comissao.Desfazer();
			this.Endereco.Desfazer();
			this.Cidade.Desfazer();
			this.Estado.Desfazer();
			this.CEP.Desfazer();
			this.DDDTelefone.Desfazer();
			this.Telefone.Desfazer();
			this.Email.Desfazer();
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
		
		public class nome : TextProperty{
		
			public override string Nome{
				get{
					return "Nome";
				}
			}
			
			public override int Tamanho{
				get{
					return 50;
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
		
		public class sexo : TextProperty{
		
			public override string Nome{
				get{
					return "Sexo";
				}
			}
			
			public override int Tamanho{
				get{
					return 1;
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
		
		public class comissao : NumberProperty{
		
			public override string Nome{
				get{
					return "Comissao";
				}
			}
			
			public override int Tamanho{
				get{
					return 3;
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
		
		public class endereco : TextProperty{
		
			public override string Nome{
				get{
					return "Endereco";
				}
			}
			
			public override int Tamanho{
				get{
					return 70;
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
		
		public class cidade : TextProperty{
		
			public override string Nome{
				get{
					return "Cidade";
				}
			}
			
			public override int Tamanho{
				get{
					return 50;
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
		
		public class estado : TextProperty{
		
			public override string Nome{
				get{
					return "Estado";
				}
			}
			
			public override int Tamanho{
				get{
					return 2;
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
		
		public class cep : TextProperty{
		
			public override string Nome{
				get{
					return "CEP";
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
		
		public class dddtelefone : TextProperty{
		
			public override string Nome{
				get{
					return "DDDTelefone";
				}
			}
			
			public override int Tamanho{
				get{
					return 2;
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
		
		public class telefone : TextProperty{
		
			public override string Nome{
				get{
					return "Telefone";
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
		
		public class email : TextProperty{
		
			public override string Nome{
				get{
					return "Email";
				}
			}
			
			public override int Tamanho{
				get{
					return 50;
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

				DataTable tabela = new DataTable("Garcon");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EmpresaID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("Sexo", typeof(string));
				tabela.Columns.Add("Comissao", typeof(decimal));
				tabela.Columns.Add("Endereco", typeof(string));
				tabela.Columns.Add("Cidade", typeof(string));
				tabela.Columns.Add("Estado", typeof(string));
				tabela.Columns.Add("CEP", typeof(string));
				tabela.Columns.Add("DDDTelefone", typeof(string));
				tabela.Columns.Add("Telefone", typeof(string));
				tabela.Columns.Add("Email", typeof(string));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
	}
	#endregion

	#region "GarconLista_B"
	
	public abstract class GarconLista_B : BaseLista {
	
		private bool backup = false;
		protected Garcon garcon;
	
		// passar o Usuario logado no sistema
		public GarconLista_B(){
			garcon = new Garcon();
		}
	
		// passar o Usuario logado no sistema
		public GarconLista_B(int usuarioIDLogado){
			garcon = new Garcon(usuarioIDLogado);
		}
		
		public Garcon Garcon{
			get{ return garcon; }
		}

		/// <summary>
		/// Retorna um IBaseBD de Garcon especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					garcon.Ler(id);
					return garcon;
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
					sql = "SELECT ID FROM tGarcon";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tGarcon";
				
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
					sql = "SELECT ID FROM tGarcon";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tGarcon";
				
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
					sql = "SELECT ID FROM xGarcon";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xGarcon";
				
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
		/// Preenche Garcon corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					garcon.Ler(id);
				else	
					garcon.LerBackup(id);
				
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
		
				bool ok = garcon.Excluir();
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
		/// Inseri novo(a) Garcon na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = garcon.Inserir();
				if (ok){
					lista.Add(garcon.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de Garcon carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("Garcon");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("Sexo", typeof(string));
					tabela.Columns.Add("Comissao", typeof(decimal));
					tabela.Columns.Add("Endereco", typeof(string));
					tabela.Columns.Add("Cidade", typeof(string));
					tabela.Columns.Add("Estado", typeof(string));
					tabela.Columns.Add("CEP", typeof(string));
					tabela.Columns.Add("DDDTelefone", typeof(string));
					tabela.Columns.Add("Telefone", typeof(string));
					tabela.Columns.Add("Email", typeof(string));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= garcon.Control.ID;
						linha["EmpresaID"]= garcon.EmpresaID.Valor;
						linha["Nome"]= garcon.Nome.Valor;
						linha["Sexo"]= garcon.Sexo.Valor;
						linha["Comissao"]= garcon.Comissao.Valor;
						linha["Endereco"]= garcon.Endereco.Valor;
						linha["Cidade"]= garcon.Cidade.Valor;
						linha["Estado"]= garcon.Estado.Valor;
						linha["CEP"]= garcon.CEP.Valor;
						linha["DDDTelefone"]= garcon.DDDTelefone.Valor;
						linha["Telefone"]= garcon.Telefone.Valor;
						linha["Email"]= garcon.Email.Valor;
						linha["Obs"]= garcon.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioGarcon");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("Sexo", typeof(string));
					tabela.Columns.Add("Comissao", typeof(decimal));
					tabela.Columns.Add("Endereco", typeof(string));
					tabela.Columns.Add("Cidade", typeof(string));
					tabela.Columns.Add("Estado", typeof(string));
					tabela.Columns.Add("CEP", typeof(string));
					tabela.Columns.Add("DDDTelefone", typeof(string));
					tabela.Columns.Add("Telefone", typeof(string));
					tabela.Columns.Add("Email", typeof(string));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EmpresaID"]= garcon.EmpresaID.Valor;
						linha["Nome"]= garcon.Nome.Valor;
						linha["Sexo"]= garcon.Sexo.Valor;
						linha["Comissao"]= garcon.Comissao.Valor;
						linha["Endereco"]= garcon.Endereco.Valor;
						linha["Cidade"]= garcon.Cidade.Valor;
						linha["Estado"]= garcon.Estado.Valor;
						linha["CEP"]= garcon.CEP.Valor;
						linha["DDDTelefone"]= garcon.DDDTelefone.Valor;
						linha["Telefone"]= garcon.Telefone.Valor;
						linha["Email"]= garcon.Email.Valor;
						linha["Obs"]= garcon.Obs.Valor;
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
						sql = "SELECT ID, EmpresaID FROM tGarcon WHERE "+FiltroSQL+" ORDER BY EmpresaID";
						break;
					case "Nome":
						sql = "SELECT ID, Nome FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Nome";
						break;
					case "Sexo":
						sql = "SELECT ID, Sexo FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Sexo";
						break;
					case "Comissao":
						sql = "SELECT ID, Comissao FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Comissao";
						break;
					case "Endereco":
						sql = "SELECT ID, Endereco FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Endereco";
						break;
					case "Cidade":
						sql = "SELECT ID, Cidade FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Cidade";
						break;
					case "Estado":
						sql = "SELECT ID, Estado FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Estado";
						break;
					case "CEP":
						sql = "SELECT ID, CEP FROM tGarcon WHERE "+FiltroSQL+" ORDER BY CEP";
						break;
					case "DDDTelefone":
						sql = "SELECT ID, DDDTelefone FROM tGarcon WHERE "+FiltroSQL+" ORDER BY DDDTelefone";
						break;
					case "Telefone":
						sql = "SELECT ID, Telefone FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Telefone";
						break;
					case "Email":
						sql = "SELECT ID, Email FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Email";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tGarcon WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "GarconException"
	
	[Serializable]
	public class GarconException : Exception {

		public GarconException() : base (){}

		public GarconException(string msg) : base (msg){}

		public GarconException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}