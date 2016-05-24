/**************************************************
* Arquivo: PacoteItem.cs
* Gerado: 26/07/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Text;

namespace IRLib.Paralela{

	public class PacoteItem : PacoteItem_B{
	
		public PacoteItem(){}
	
		public PacoteItem(int usuarioIDLogado) : base (usuarioIDLogado){ }

        #region Métodos de Manipulação do Pacote Item

        #region Inserir

        /// <summary>
        /// Inserir novo(a) PacoteItem
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        /// <returns></returns>
        internal bool Inserir(ref BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPacoteItem");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

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
                    InserirControle("I", bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Atualizar

        /// <summary>
        /// Atualiza PacoteItem
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cPacoteItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

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

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Excluir

        /// <summary>
		/// Exclui PacoteItem com ID especifico
		/// </summary>
		/// <param name="id">Informe o ID</param>
        /// <param name="bd">Objeto de conexão</param>
		/// <returns></returns>
        internal bool Excluir(int id,ref BD bd)
        {

            try
            {

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPacoteItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D", bd);
                InserirLog(bd);

                string sqlDelete = "DELETE FROM tPacoteItem WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #region Controle e Log

        internal protected void InserirControle(string acao, BD bd)
        {

            try
            {

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

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xPacoteItem (ID, Versao, EventoID, ApresentacaoID, SetorID, PrecoID, CortesiaID, PacoteID, Quantidade) ");
                sql.Append("SELECT ID, @V, EventoID, ApresentacaoID, SetorID, PrecoID, CortesiaID, PacoteID, Quantidade FROM tPacoteItem WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        #endregion

        #endregion

        private void CapturaApresentacaoSetorIDs()
        {
            try
            {
                IDataReader dr = bd.Consulta(@"
                    SELECT 
                        tApresentacaoSetor.ApresentacaoID, 
                        tApresentacaoSetor.SetorID 
                    FROM 
                        tApresentacaoSetor 
                    INNER JOIN 
                        tPreco 
                    ON 
                        tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID 
                    WHERE 
                        tPreco.ID = " + this.PrecoID.Valor);
                if (bd.Consulta().Read())
                {
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                }
                bd.Fechar();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        /*
        /// <summary>
        /// Insere o ApresentacaoID e SetorID de acordo com o PrecoID
        /// </summary>
        /// <returns></returns>
        public override bool Inserir()
        {
            CapturaApresentacaoSetorIDs();

            return base.Inserir();
        }
        */

        /*
        /// <summary>
        /// Atualiza o ApresentacaoID e SetorID de acordo com o PrecoID
        /// </summary>
        /// <returns></returns>
        public override bool Atualizar()
        {
            CapturaApresentacaoSetorIDs();

            return base.Atualizar();
        }
        */

        /*
		/// <summary>
		/// Exclui PacoteItem
		/// </summary>
		/// <returns></returns>		
		public override bool Excluir(){
		
			try{
				Preco preco = new Preco();
				preco.Control.ID = this.PrecoID.Valor;
				preco.Excluir();
			}catch{}
					
			return this.Excluir(this.Control.ID);
		
		}
        */
	}

	public class PacoteItemLista : PacoteItemLista_B{
	
		public PacoteItemLista(){}
	
		public PacoteItemLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem uma tabela a ser jogada num relatorio
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){
				
			try{

				DataTable tabela = new DataTable("RelatorioPacoteItem");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Evento", typeof(string));
					tabela.Columns.Add("Horário", typeof(string));
					tabela.Columns.Add("Setor", typeof(string));
					tabela.Columns.Add("Preço", typeof(decimal));
					tabela.Columns.Add("Qtd", typeof(int));
					tabela.Columns.Add("Total", typeof(decimal));

					do{
						DataRow linha = tabela.NewRow();
						Preco preco = new Preco();
						preco.Ler(pacoteItem.PrecoID.Valor);

						ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
						apresentacaoSetor.Ler(preco.ApresentacaoSetorID.Valor);

						Apresentacao apresentacao = new Apresentacao();
						apresentacao.Ler(apresentacaoSetor.ApresentacaoID.Valor);

						Setor setor = new Setor();
						setor.Ler(apresentacaoSetor.SetorID.Valor);

						Evento evento = new Evento();
						evento.Ler(apresentacao.EventoID.Valor);

						linha["Evento"]= evento.Nome;
						linha["Horário"]= apresentacao.Horario.Valor.ToString(Utilitario.FormatoDataHora);
						linha["Setor"]=  setor.Nome;
						linha["Preço"]= preco.Valor.Valor;
						linha["Qtd"]= pacoteItem.Quantidade.Valor;
						linha["Total"]= (preco.Valor.Valor * pacoteItem.Quantidade.Valor);
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
		
	}
	
}
