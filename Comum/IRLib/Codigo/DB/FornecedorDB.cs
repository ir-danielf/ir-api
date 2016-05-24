/******************************************************
* Arquivo FornecedorDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib {

	#region "Fornecedor_B"

	public abstract class Fornecedor_B : BaseBD {
	
		public empresaid EmpresaID = new empresaid();
		public nome Nome = new nome();
		public contatonome ContatoNome = new contatonome();
		public contatocargo ContatoCargo = new contatocargo();
		public endereco Endereco = new endereco();
		public cidade Cidade = new cidade();
		public estado Estado = new estado();
		public cep CEP = new cep();
		public dddtelefone DDDTelefone = new dddtelefone();
		public telefone Telefone = new telefone();
		public dddfax DDDFax = new dddfax();
		public fax Fax = new fax();
		public email Email = new email();
		public website Website = new website();
		public obs Obs = new obs();
		
		public Fornecedor_B(){}
					
		// passar o Usuario logado no sistema
		public Fornecedor_B(int usuarioIDLogado){
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de Fornecedor
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id){
		
			try{
		
				string sql = "SELECT * FROM tFornecedor WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Nome.ValorBD = bd.LerString("Nome");
					this.ContatoNome.ValorBD = bd.LerString("ContatoNome");
					this.ContatoCargo.ValorBD = bd.LerString("ContatoCargo");
					this.Endereco.ValorBD = bd.LerString("Endereco");
					this.Cidade.ValorBD = bd.LerString("Cidade");
					this.Estado.ValorBD = bd.LerString("Estado");
					this.CEP.ValorBD = bd.LerString("CEP");
					this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
					this.Telefone.ValorBD = bd.LerString("Telefone");
					this.DDDFax.ValorBD = bd.LerString("DDDFax");
					this.Fax.ValorBD = bd.LerString("Fax");
					this.Email.ValorBD = bd.LerString("Email");
					this.Website.ValorBD = bd.LerString("Website");
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
		/// Preenche todos os atributos de Fornecedor do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id){
		
			try{
		
				string sql = "SELECT * FROM xFornecedor WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read()){
					this.Control.ID = id;
					this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
					this.Nome.ValorBD = bd.LerString("Nome");
					this.ContatoNome.ValorBD = bd.LerString("ContatoNome");
					this.ContatoCargo.ValorBD = bd.LerString("ContatoCargo");
					this.Endereco.ValorBD = bd.LerString("Endereco");
					this.Cidade.ValorBD = bd.LerString("Cidade");
					this.Estado.ValorBD = bd.LerString("Estado");
					this.CEP.ValorBD = bd.LerString("CEP");
					this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
					this.Telefone.ValorBD = bd.LerString("Telefone");
					this.DDDFax.ValorBD = bd.LerString("DDDFax");
					this.Fax.ValorBD = bd.LerString("Fax");
					this.Email.ValorBD = bd.LerString("Email");
					this.Website.ValorBD = bd.LerString("Website");
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
				sql.Append("INSERT INTO cFornecedor (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
				
				sql.Append("INSERT INTO xFornecedor (ID, Versao, EmpresaID, Nome, ContatoNome, ContatoCargo, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, DDDFax, Fax, Email, Website, Obs) ");
				sql.Append("SELECT ID, @V, EmpresaID, Nome, ContatoNome, ContatoCargo, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, DDDFax, Fax, Email, Website, Obs FROM tFornecedor WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}catch(Exception ex){
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) Fornecedor
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir(){
		
			try{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cFornecedor");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tFornecedor(ID, EmpresaID, Nome, ContatoNome, ContatoCargo, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, DDDFax, Fax, Email, Website, Obs) ");
				sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014','@015')");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Nome.ValorBD);
				sql.Replace("@003", this.ContatoNome.ValorBD);
				sql.Replace("@004", this.ContatoCargo.ValorBD);
				sql.Replace("@005", this.Endereco.ValorBD);
				sql.Replace("@006", this.Cidade.ValorBD);
				sql.Replace("@007", this.Estado.ValorBD);
				sql.Replace("@008", this.CEP.ValorBD);
				sql.Replace("@009", this.DDDTelefone.ValorBD);
				sql.Replace("@010", this.Telefone.ValorBD);
				sql.Replace("@011", this.DDDFax.ValorBD);
				sql.Replace("@012", this.Fax.ValorBD);
				sql.Replace("@013", this.Email.ValorBD);
				sql.Replace("@014", this.Website.ValorBD);
				sql.Replace("@015", this.Obs.ValorBD);
				
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
		/// Atualiza Fornecedor
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar(){
		
			try{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cFornecedor WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tFornecedor SET EmpresaID = @001, Nome = '@002', ContatoNome = '@003', ContatoCargo = '@004', Endereco = '@005', Cidade = '@006', Estado = '@007', CEP = '@008', DDDTelefone = '@009', Telefone = '@010', DDDFax = '@011', Fax = '@012', Email = '@013', Website = '@014', Obs = '@015' ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.EmpresaID.ValorBD);
				sql.Replace("@002", this.Nome.ValorBD);
				sql.Replace("@003", this.ContatoNome.ValorBD);
				sql.Replace("@004", this.ContatoCargo.ValorBD);
				sql.Replace("@005", this.Endereco.ValorBD);
				sql.Replace("@006", this.Cidade.ValorBD);
				sql.Replace("@007", this.Estado.ValorBD);
				sql.Replace("@008", this.CEP.ValorBD);
				sql.Replace("@009", this.DDDTelefone.ValorBD);
				sql.Replace("@010", this.Telefone.ValorBD);
				sql.Replace("@011", this.DDDFax.ValorBD);
				sql.Replace("@012", this.Fax.ValorBD);
				sql.Replace("@013", this.Email.ValorBD);
				sql.Replace("@014", this.Website.ValorBD);
				sql.Replace("@015", this.Obs.ValorBD);
				
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
		/// Exclui Fornecedor com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id){
		
			try{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cFornecedor WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tFornecedor WHERE ID="+id;
				
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
		/// Exclui Fornecedor
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
			this.ContatoNome.Limpar();
			this.ContatoCargo.Limpar();
			this.Endereco.Limpar();
			this.Cidade.Limpar();
			this.Estado.Limpar();
			this.CEP.Limpar();
			this.DDDTelefone.Limpar();
			this.Telefone.Limpar();
			this.DDDFax.Limpar();
			this.Fax.Limpar();
			this.Email.Limpar();
			this.Website.Limpar();
			this.Obs.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer(){
		
			this.Control.Desfazer();
			this.EmpresaID.Desfazer();
			this.Nome.Desfazer();
			this.ContatoNome.Desfazer();
			this.ContatoCargo.Desfazer();
			this.Endereco.Desfazer();
			this.Cidade.Desfazer();
			this.Estado.Desfazer();
			this.CEP.Desfazer();
			this.DDDTelefone.Desfazer();
			this.Telefone.Desfazer();
			this.DDDFax.Desfazer();
			this.Fax.Desfazer();
			this.Email.Desfazer();
			this.Website.Desfazer();
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
		
		public class contatonome : TextProperty{
		
			public override string Nome{
				get{
					return "ContatoNome";
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
		
		public class contatocargo : TextProperty{
		
			public override string Nome{
				get{
					return "ContatoCargo";
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
		
		public class dddfax : TextProperty{
		
			public override string Nome{
				get{
					return "DDDFax";
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
		
		public class fax : TextProperty{
		
			public override string Nome{
				get{
					return "Fax";
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
		
		public class website : TextProperty{
		
			public override string Nome{
				get{
					return "Website";
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

				DataTable tabela = new DataTable("Fornecedor");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("EmpresaID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("ContatoNome", typeof(string));
				tabela.Columns.Add("ContatoCargo", typeof(string));
				tabela.Columns.Add("Endereco", typeof(string));
				tabela.Columns.Add("Cidade", typeof(string));
				tabela.Columns.Add("Estado", typeof(string));
				tabela.Columns.Add("CEP", typeof(string));
				tabela.Columns.Add("DDDTelefone", typeof(string));
				tabela.Columns.Add("Telefone", typeof(string));
				tabela.Columns.Add("DDDFax", typeof(string));
				tabela.Columns.Add("Fax", typeof(string));
				tabela.Columns.Add("Email", typeof(string));
				tabela.Columns.Add("Website", typeof(string));
				tabela.Columns.Add("Obs", typeof(string));
			
				return tabela;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}		
				
		public abstract DataTable Produtos();

	}
	#endregion

	#region "FornecedorLista_B"
	
	public abstract class FornecedorLista_B : BaseLista {
	
		private bool backup = false;
		protected Fornecedor fornecedor;
	
		// passar o Usuario logado no sistema
		public FornecedorLista_B(){
			fornecedor = new Fornecedor();
		}
	
		// passar o Usuario logado no sistema
		public FornecedorLista_B(int usuarioIDLogado){
			fornecedor = new Fornecedor(usuarioIDLogado);
		}
		
		public Fornecedor Fornecedor{
			get{ return fornecedor; }
		}

		/// <summary>
		/// Retorna um IBaseBD de Fornecedor especifico
		/// </summary>
		public override IBaseBD this[int indice]{
			get{
				if (indice < 0 || indice >= lista.Count){
					return null;
				}else{
					int id = (int)lista[indice];
					fornecedor.Ler(id);
					return fornecedor;
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
					sql = "SELECT ID FROM tFornecedor";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tFornecedor";
				
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
					sql = "SELECT ID FROM tFornecedor";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tFornecedor";
				
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
					sql = "SELECT ID FROM xFornecedor";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xFornecedor";
				
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
		/// Preenche Fornecedor corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id){
			
			try{
		
				if (!backup)
					fornecedor.Ler(id);
				else	
					fornecedor.LerBackup(id);
				
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
		
				bool ok = fornecedor.Excluir();
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
		/// Inseri novo(a) Fornecedor na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir(){
		
			try{
		
				bool ok = fornecedor.Inserir();
				if (ok){
					lista.Add(fornecedor.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}catch(Exception ex){
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de Fornecedor carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){
				
			try{
				
					DataTable tabela = new DataTable("Fornecedor");
				
					tabela.Columns.Add("ID", typeof(int));
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("ContatoNome", typeof(string));
					tabela.Columns.Add("ContatoCargo", typeof(string));
					tabela.Columns.Add("Endereco", typeof(string));
					tabela.Columns.Add("Cidade", typeof(string));
					tabela.Columns.Add("Estado", typeof(string));
					tabela.Columns.Add("CEP", typeof(string));
					tabela.Columns.Add("DDDTelefone", typeof(string));
					tabela.Columns.Add("Telefone", typeof(string));
					tabela.Columns.Add("DDDFax", typeof(string));
					tabela.Columns.Add("Fax", typeof(string));
					tabela.Columns.Add("Email", typeof(string));
					tabela.Columns.Add("Website", typeof(string));
					tabela.Columns.Add("Obs", typeof(string));
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= fornecedor.Control.ID;
						linha["EmpresaID"]= fornecedor.EmpresaID.Valor;
						linha["Nome"]= fornecedor.Nome.Valor;
						linha["ContatoNome"]= fornecedor.ContatoNome.Valor;
						linha["ContatoCargo"]= fornecedor.ContatoCargo.Valor;
						linha["Endereco"]= fornecedor.Endereco.Valor;
						linha["Cidade"]= fornecedor.Cidade.Valor;
						linha["Estado"]= fornecedor.Estado.Valor;
						linha["CEP"]= fornecedor.CEP.Valor;
						linha["DDDTelefone"]= fornecedor.DDDTelefone.Valor;
						linha["Telefone"]= fornecedor.Telefone.Valor;
						linha["DDDFax"]= fornecedor.DDDFax.Valor;
						linha["Fax"]= fornecedor.Fax.Valor;
						linha["Email"]= fornecedor.Email.Valor;
						linha["Website"]= fornecedor.Website.Valor;
						linha["Obs"]= fornecedor.Obs.Valor;
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

				DataTable tabela = new DataTable("RelatorioFornecedor");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("EmpresaID", typeof(int));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("ContatoNome", typeof(string));
					tabela.Columns.Add("ContatoCargo", typeof(string));
					tabela.Columns.Add("Endereco", typeof(string));
					tabela.Columns.Add("Cidade", typeof(string));
					tabela.Columns.Add("Estado", typeof(string));
					tabela.Columns.Add("CEP", typeof(string));
					tabela.Columns.Add("DDDTelefone", typeof(string));
					tabela.Columns.Add("Telefone", typeof(string));
					tabela.Columns.Add("DDDFax", typeof(string));
					tabela.Columns.Add("Fax", typeof(string));
					tabela.Columns.Add("Email", typeof(string));
					tabela.Columns.Add("Website", typeof(string));
					tabela.Columns.Add("Obs", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["EmpresaID"]= fornecedor.EmpresaID.Valor;
						linha["Nome"]= fornecedor.Nome.Valor;
						linha["ContatoNome"]= fornecedor.ContatoNome.Valor;
						linha["ContatoCargo"]= fornecedor.ContatoCargo.Valor;
						linha["Endereco"]= fornecedor.Endereco.Valor;
						linha["Cidade"]= fornecedor.Cidade.Valor;
						linha["Estado"]= fornecedor.Estado.Valor;
						linha["CEP"]= fornecedor.CEP.Valor;
						linha["DDDTelefone"]= fornecedor.DDDTelefone.Valor;
						linha["Telefone"]= fornecedor.Telefone.Valor;
						linha["DDDFax"]= fornecedor.DDDFax.Valor;
						linha["Fax"]= fornecedor.Fax.Valor;
						linha["Email"]= fornecedor.Email.Valor;
						linha["Website"]= fornecedor.Website.Valor;
						linha["Obs"]= fornecedor.Obs.Valor;
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
						sql = "SELECT ID, EmpresaID FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY EmpresaID";
						break;
					case "Nome":
						sql = "SELECT ID, Nome FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Nome";
						break;
					case "ContatoNome":
						sql = "SELECT ID, ContatoNome FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY ContatoNome";
						break;
					case "ContatoCargo":
						sql = "SELECT ID, ContatoCargo FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY ContatoCargo";
						break;
					case "Endereco":
						sql = "SELECT ID, Endereco FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Endereco";
						break;
					case "Cidade":
						sql = "SELECT ID, Cidade FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Cidade";
						break;
					case "Estado":
						sql = "SELECT ID, Estado FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Estado";
						break;
					case "CEP":
						sql = "SELECT ID, CEP FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY CEP";
						break;
					case "DDDTelefone":
						sql = "SELECT ID, DDDTelefone FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY DDDTelefone";
						break;
					case "Telefone":
						sql = "SELECT ID, Telefone FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Telefone";
						break;
					case "DDDFax":
						sql = "SELECT ID, DDDFax FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY DDDFax";
						break;
					case "Fax":
						sql = "SELECT ID, Fax FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Fax";
						break;
					case "Email":
						sql = "SELECT ID, Email FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Email";
						break;
					case "Website":
						sql = "SELECT ID, Website FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Website";
						break;
					case "Obs":
						sql = "SELECT ID, Obs FROM tFornecedor WHERE "+FiltroSQL+" ORDER BY Obs";
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

	#region "FornecedorException"
	
	[Serializable]
	public class FornecedorException : Exception {

		public FornecedorException() : base (){}

		public FornecedorException(string msg) : base (msg){}

		public FornecedorException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}