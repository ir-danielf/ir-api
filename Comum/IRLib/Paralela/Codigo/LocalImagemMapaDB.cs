/******************************************************
* Arquivo LocalImagemMapaDB.cs
* Gerado em: 03/12/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela 
{

	#region "LocalImagemMapa_B"

	public abstract class LocalImagemMapa_B : BaseBD 
	{
	
		public localid LocalID = new localid();
		public nome Nome = new nome();
		public versaoimagem VersaoImagem = new versaoimagem();
		
		public LocalImagemMapa_B(){}
					
		// passar o Usuario logado no sistema
		public LocalImagemMapa_B(int usuarioIDLogado)
		{
			this.Control.UsuarioID = usuarioIDLogado;
		}

		/// <summary>
		/// Preenche todos os atributos de LocalImagemMapa
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override void Ler(int id)
		{
		
			try
			{
		
				string sql = "SELECT * FROM tLocalImagemMapa WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read())
				{
					this.Control.ID = id;
					this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
					this.Nome.ValorBD = bd.LerString("Nome");
					this.VersaoImagem.ValorBD = bd.LerInt("VersaoImagem").ToString();
				}
				else
				{
					this.Limpar();
				}
				bd.Fechar();
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
						
		}

		/// <summary>
		/// Preenche todos os atributos de LocalImagemMapa do backup
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public void LerBackup(int id)
		{
		
			try
			{
		
				string sql = "SELECT * FROM xLocalImagemMapa WHERE ID = " + id;
				bd.Consulta(sql);
				
				if (bd.Consulta().Read())
				{
					this.Control.ID = id;
					this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
					this.Nome.ValorBD = bd.LerString("Nome");
					this.VersaoImagem.ValorBD = bd.LerInt("VersaoImagem").ToString();
				}
				bd.Fechar();
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
						
		}

		protected void InserirControle(string acao)
		{
		
			try
			{
			
				System.Text.StringBuilder sql = new System.Text.StringBuilder();
				sql.Append("INSERT INTO cLocalImagemMapa (ID, Versao, Acao, TimeStamp, UsuarioID) ");
				sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
				sql.Replace("@ID", this.Control.ID.ToString());
				
				if (!acao.Equals("I"))
					this.Control.Versao++;
				
				sql.Replace("@V", this.Control.Versao.ToString());
				sql.Replace("@A", acao);
				sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
				sql.Replace("@U", this.Control.UsuarioID.ToString());
				
				bd.Executar(sql.ToString());
	
			}
			catch(Exception ex)
			{
				throw ex;
			}
					
		}
			
		protected void InserirLog()
		{
			
			try
			{
				
				StringBuilder sql = new StringBuilder();
				
				sql.Append("INSERT INTO xLocalImagemMapa (ID, Versao, LocalID, Nome, VersaoImagem) ");
				sql.Append("SELECT ID, @V, LocalID, Nome, VersaoImagem FROM tLocalImagemMapa WHERE ID = @I");
				sql.Replace("@I", this.Control.ID.ToString());
				sql.Replace("@V", this.Control.Versao.ToString());
				
				bd.Executar(sql.ToString());
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
					
		}

		/// <summary>
		/// Inserir novo(a) LocalImagemMapa
		/// </summary>
		/// <returns></returns>	
		public override bool Inserir()
		{
		
			try
			{

				bd.IniciarTransacao();
		
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT MAX(ID) AS ID FROM cLocalImagemMapa");
				object obj = bd.ConsultaValor(sql);
				int id = (obj!=null) ? Convert.ToInt32(obj) : 0;
				
				this.Control.ID = ++id;
				this.Control.Versao = 0;
				
				sql = new StringBuilder();
				sql.Append("INSERT INTO tLocalImagemMapa(ID, LocalID, Nome, VersaoImagem) ");
				sql.Append("VALUES (@ID,@001,'@002',@003)");
				
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.LocalID.ValorBD);
				sql.Replace("@002", this.Nome.ValorBD);
				sql.Replace("@003", this.VersaoImagem.ValorBD);
				
				int x = bd.Executar(sql.ToString());
				
				bool result = (x == 1);
				
				if (result)
					InserirControle("I");

				bd.FinalizarTransacao();	
				
				return result;
				
			}
			catch(Exception ex)
			{
				bd.DesfazerTransacao();
				throw ex;
			}
			finally
			{
				bd.Fechar();
			}
			
		}

		/// <summary>
		/// Atualiza LocalImagemMapa
		/// </summary>
		/// <returns></returns>	
		public override bool Atualizar()
		{
		
			try
			{

				bd.IniciarTransacao();
		
				string sqlVersion = "SELECT MAX(Versao) FROM cLocalImagemMapa WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlVersion);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("U");
				InserirLog();
					
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tLocalImagemMapa SET LocalID = @001, Nome = '@002', VersaoImagem = @003 ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.LocalID.ValorBD);
				sql.Replace("@002", this.Nome.ValorBD);
				sql.Replace("@003", this.VersaoImagem.ValorBD);
				
				int x = bd.Executar(sql.ToString());
				
				bool result = (x == 1);

				bd.FinalizarTransacao();
				
				return result;
				
			}
			catch(Exception ex)
			{
				bd.DesfazerTransacao();
				throw ex;
			}
			finally
			{
				bd.Fechar();
			}
			
		}

		/// <summary>
		/// Exclui LocalImagemMapa com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		public override bool Excluir(int id)
		{
		
			try
			{

				bd.IniciarTransacao();
			
				this.Control.ID=id;
			
				string sqlSelect = "SELECT MAX(Versao) FROM cLocalImagemMapa WHERE ID="+this.Control.ID;
				object obj = bd.ConsultaValor(sqlSelect);
				int versao = (obj!=null) ? Convert.ToInt32(obj) : 0;
				this.Control.Versao = versao;
				
				InserirControle("D");
				InserirLog();
					
				string sqlDelete = "DELETE FROM tLocalImagemMapa WHERE ID="+id;
				
				int x = bd.Executar(sqlDelete);
				
				bool result = (x == 1);

				bd.FinalizarTransacao();
				
				return result;
			
			}
			catch(Exception ex)
			{
				bd.DesfazerTransacao();
				throw ex;
			}
			finally
			{
				bd.Fechar();
			}
			
		}
		
		/// <summary>
		/// Exclui LocalImagemMapa
		/// </summary>
		/// <returns></returns>		
		public override bool Excluir()
		{
		
			try
			{
				return this.Excluir(this.Control.ID);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}

		public override void Limpar()
		{
		
			this.LocalID.Limpar();
			this.Nome.Limpar();
			this.VersaoImagem.Limpar();
			this.Control.ID = 0;
			this.Control.Versao = 0;
		}

		public override void Desfazer()
		{
		
			this.Control.Desfazer();
			this.LocalID.Desfazer();
			this.Nome.Desfazer();
			this.VersaoImagem.Desfazer();
		}

		public class localid : IntegerProperty
		{
		
			public override string Nome
			{
				get
				{
					return "LocalID";
				}
			}
			
			public override int Tamanho
			{
				get
				{
					return 0;
				}
			}
			
			public override int Valor
			{
				get
				{
					return base.Valor;
				}
				set
				{
					base.Valor = value;
				}
			}
			
			public override string ToString()
			{
				return base.Valor.ToString();
			}
			
		}
		
		public class nome : TextProperty
		{
		
			public override string Nome
			{
				get
				{
					return "Nome";
				}
			}
			
			public override int Tamanho
			{
				get
				{
					return 50;
				}
			}
			
			public override string Valor
			{
				get
				{
					return base.Valor;
				}
				set
				{
					base.Valor = value;
				}
			}
			
			public override string ToString()
			{
				return base.Valor;
			}
			
		}
		
		public class versaoimagem : IntegerProperty
		{
		
			public override string Nome
			{
				get
				{
					return "VersaoImagem";
				}
			}
			
			public override int Tamanho
			{
				get
				{
					return 0;
				}
			}
			
			public override int Valor
			{
				get
				{
					return base.Valor;
				}
				set
				{
					base.Valor = value;
				}
			}
			
			public override string ToString()
			{
				return base.Valor.ToString();
			}
			
		}
		
		/// <summary>
		/// Obtem uma tabela estruturada com todos os campos dessa classe.
		/// </summary>
		/// <returns></returns>
		public static DataTable Estrutura()
		{
		
			//Isso eh util para desacoplamento.
			//A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.
				
			try
			{

				DataTable tabela = new DataTable("LocalImagemMapa");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("LocalID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("VersaoImagem", typeof(int));
			
				return tabela;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}		
				
		public abstract DataTable Todos(int localid, string registrozero);

	}
	#endregion

	#region "LocalImagemMapaLista_B"
	
	public abstract class LocalImagemMapaLista_B : BaseLista 
	{
	
		private bool backup = false;
		protected LocalImagemMapa localImagemMapa;
	
		// passar o Usuario logado no sistema
		public LocalImagemMapaLista_B()
		{
			localImagemMapa = new LocalImagemMapa();
		}
	
		// passar o Usuario logado no sistema
		public LocalImagemMapaLista_B(int usuarioIDLogado)
		{
			localImagemMapa = new LocalImagemMapa(usuarioIDLogado);
		}
		
		public LocalImagemMapa LocalImagemMapa
		{
			get{ return localImagemMapa; }
		}

		/// <summary>
		/// Retorna um IBaseBD de LocalImagemMapa especifico
		/// </summary>
		public override IBaseBD this[int indice]
		{
			get
			{
				if (indice < 0 || indice >= lista.Count)
				{
					return null;
				}
				else
				{
					int id = (int)lista[indice];
					localImagemMapa.Ler(id);
					return localImagemMapa;
				}
			}
		}
		
		/// <summary>
		/// Carrega a lista
		/// </summary>
		/// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
		/// <returns></returns>		
		public void Carregar(int tamanhoMax)
		{
		
			try
			{
			
				string sql;
			
				if (tamanhoMax==0)
					sql = "SELECT ID FROM tLocalImagemMapa";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tLocalImagemMapa";
				
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
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}
				
		/// <summary>
		/// Carrega a lista
		/// </summary>
		public override void Carregar()
		{
		
			try
			{
			
				string sql;
			
				if (tamanhoMax==0)
					sql = "SELECT ID FROM tLocalImagemMapa";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM tLocalImagemMapa";
				
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
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}
				
		/// <summary>
		/// Carrega a lista pela tabela x (de backup)
		/// </summary>
		public void CarregarBackup()
		{
		
			try
			{
			
				string sql;
			
				if (tamanhoMax==0)
					sql = "SELECT ID FROM xLocalImagemMapa";
				else
					sql = "SELECT top "+tamanhoMax+" ID FROM xLocalImagemMapa";
				
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
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}

		/// <summary>
		/// Preenche LocalImagemMapa corrente da lista
		/// </summary>
		/// <param name="id">Informe o ID</param>
		/// <returns></returns>
		protected override void Ler(int id)
		{
			
			try
			{
		
				if (!backup)
					localImagemMapa.Ler(id);
				else	
					localImagemMapa.LerBackup(id);
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}

		/// <summary>
		/// Exclui o item corrente da lista
		/// </summary>
		/// <returns></returns>
		public override bool Excluir()
		{
		
			try
			{
		
				bool ok = localImagemMapa.Excluir();
				if (ok)				
					lista.RemoveAt(Indice);
			
				return ok;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}
		
		/// <summary>
		/// Exclui todos os itens da lista carregada
		/// </summary>
		/// <returns></returns>
		public override bool ExcluirTudo()
		{
			
			try
			{
				if (lista.Count == 0)
					Carregar();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
			try
			{
		
				bool ok = false;

				if (lista.Count > 0)
				{ //verifica se tem itens

					Ultimo();
					//fazer varredura de traz pra frente.
					do
						ok = Excluir();
					while (ok && Anterior());

				}
				else
				{ //nao tem itens na lista
					//Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
					ok = true;
				}
				
				return ok;
			
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}		
		
		/// <summary>
		/// Inseri novo(a) LocalImagemMapa na lista
		/// </summary>
		/// <returns></returns>		
		public override bool Inserir()
		{
		
			try
			{
		
				bool ok = localImagemMapa.Inserir();
				if (ok)
				{
					lista.Add(localImagemMapa.Control.ID);
					Indice = lista.Count - 1;
				}
			
				return ok;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}

		/// <summary>
		///  Obtem uma tabela de todos os campos de LocalImagemMapa carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela()
		{
				
			try
			{
				
				DataTable tabela = new DataTable("LocalImagemMapa");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("LocalID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("VersaoImagem", typeof(int));
			
				if (this.Primeiro())
				{

					do
					{
						DataRow linha = tabela.NewRow();
						linha["ID"]= localImagemMapa.Control.ID;
						linha["LocalID"]= localImagemMapa.LocalID.Valor;
						linha["Nome"]= localImagemMapa.Nome.Valor;
						linha["VersaoImagem"]= localImagemMapa.VersaoImagem.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}
			
				return tabela;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}
			
		/// <summary>
		/// Obtem uma tabela a ser jogada num relatorio
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio()
		{
				
			try
			{

				DataTable tabela = new DataTable("RelatorioLocalImagemMapa");
			
				if (this.Primeiro())
				{
				
					tabela.Columns.Add("LocalID", typeof(int));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("VersaoImagem", typeof(int));

					do
					{
						DataRow linha = tabela.NewRow();
						linha["LocalID"]= localImagemMapa.LocalID.Valor;
						linha["Nome"]= localImagemMapa.Nome.Valor;
						linha["VersaoImagem"]= localImagemMapa.VersaoImagem.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}
				else
				{ //erro: nao carregou a lista
					tabela = null;
				}		
			
				return tabela;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}		
		
		/// <summary>
		/// Retorna um IDataReader com ID e o Campo.
		/// </summary>
		/// <param name="campo">Informe o campo. Exemplo: Nome</param>
		/// <returns></returns>
		public override IDataReader ListaPropriedade(string campo)
		{
		
			try
			{
				string sql;
				switch (campo)
				{
					case "LocalID":
						sql = "SELECT ID, LocalID FROM tLocalImagemMapa WHERE "+FiltroSQL+" ORDER BY LocalID";
						break;
					case "Nome":
						sql = "SELECT ID, Nome FROM tLocalImagemMapa WHERE "+FiltroSQL+" ORDER BY Nome";
						break;
					case "VersaoImagem":
						sql = "SELECT ID, VersaoImagem FROM tLocalImagemMapa WHERE "+FiltroSQL+" ORDER BY VersaoImagem";
						break;
					default:
						sql = null;
						break;
				}
				
				IDataReader dataReader = bd.Consulta(sql);

				bd.Fechar();
				
				return dataReader;

			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}
		
		/// <summary>
		/// Devolve um array dos IDs que compoem a lista
		/// </summary>
		/// <returns></returns>		
		public override int[] ToArray()
		{
		
			try
			{

				int[] a = (int[])lista.ToArray(typeof(int));

				return a;
			
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// Devolve uma string dos IDs que compoem a lista concatenada por virgula
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
		
			try
			{

				StringBuilder idsBuffer = new StringBuilder();

				int n = lista.Count;
				for(int i=0; i < n; i++)
				{
					int id = (int)lista[i];
					idsBuffer.Append(id+",");
				}
					
				string ids = "";

				if (idsBuffer.Length > 0)
				{
					ids = idsBuffer.ToString();
					ids = ids.Substring(0, ids.Length -1);
				}

				return ids;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}		
		
	}
	
	#endregion

	#region "LocalImagemMapaException"
	
	[Serializable]
	public class LocalImagemMapaException : Exception 
	{

		public LocalImagemMapaException() : base (){}

		public LocalImagemMapaException(string msg) : base (msg){}

		public LocalImagemMapaException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

	}

	#endregion
	
}