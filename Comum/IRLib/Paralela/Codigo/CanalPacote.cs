/**************************************************
* Arquivo: CanalPacote.cs
* Gerado: 04/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela{

	public class CanalPacote : CanalPacote_B{
	
		public CanalPacote(){}
	
		public CanalPacote(int usuarioIDLogado) : base (usuarioIDLogado){ }

        #region Métodos de Manipulação do Pacote Item

        #region Inserir

        /// <summary>
        /// Inserir novo(a) CanalPacote
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        /// <returns></returns>	
        internal bool Inserir(ref BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCanalPacote");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCanalPacote(ID, CanalID, PacoteID, Quantidade, TaxaConveniencia, TaxaMinima, TaxaMaxima, TaxaComissao, ComissaoMinima, ComissaoMaxima) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,'@005','@006',@007,'@008','@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaMinima.ValorBD);
                sql.Replace("@006", this.TaxaMaxima.ValorBD);
                sql.Replace("@007", this.TaxaComissao.ValorBD);
                sql.Replace("@008", this.ComissaoMinima.ValorBD);
                sql.Replace("@009", this.ComissaoMaxima.ValorBD);

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
        /// Atualiza CanalPacote
        /// </summary>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCanalPacote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCanalPacote SET CanalID = @001, PacoteID = @002, Quantidade = @003, TaxaConveniencia = @004,TaxaMinima = @005,TaxaMaxima = @006, TaxaComissao = @007,ComissaoMinima = @008, ComissaoMaxima = @009");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaMinima.ValorBD);
                sql.Replace("@006", this.TaxaMaxima.ValorBD);
                sql.Replace("@007", this.TaxaComissao.ValorBD);
                sql.Replace("@008", this.ComissaoMinima.ValorBD);
                sql.Replace("@009", this.ComissaoMaxima.ValorBD);

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
        /// Exclui CanalPacote com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        internal bool Excluir(int id,ref BD bd)
        {

            try
            {

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalPacote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D", bd);
                InserirLog(bd);

                string sqlDelete = "DELETE FROM tCanalPacote WHERE ID=" + id;

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

        internal void InserirControle(string acao, BD bd)
        {

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.Append("INSERT INTO cCanalPacote (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        internal void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCanalPacote (ID, Versao, CanalID, PacoteID, Quantidade, TaxaConveniencia, TaxaMinima, TaxaMaxima, TaxaComissao, ComissaoMinima, ComissaoMaxima) ");
                sql.Append("SELECT ID, @V, CanalID, PacoteID, Quantidade, TaxaConveniencia, TaxaMinima, TaxaMaxima, TaxaComissao, ComissaoMinima, ComissaoMaxima FROM tCanalPacote WHERE ID = @I");
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

        /// <summary>		
		/// Obter pacotes de um canal especifico c/ Quantidade Disponivel
		/// </summary>
		/// <returns></returns>
		public override DataTable Pacotes(int canalid){

			try{

				DataTable tabela = new DataTable("Pacote");
				
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("Valor", typeof(decimal));
				tabela.Columns.Add("LocalID", typeof(int));	

				string sql = "SELECT tPacote.ID, tPacote.Nome, tPacote.LocalID, SUM(tPreco.Valor * tPacoteItem.Quantidade) AS Valor "+
					"FROM tPacote (NOLOCK), tCanalPacote (NOLOCK), tPacoteItem (NOLOCK), tPreco (NOLOCK) "+
					"WHERE tPacote.ID=tCanalPacote.PacoteID AND tPreco.ID=tPacoteItem.PrecoID AND "+
					"tPacoteItem.PacoteID=tPacote.ID AND tCanalPacote.CanalID="+canalid+" "+
					"GROUP BY tPacote.ID, tPacote.Nome, tPacote.LocalID "+
					"ORDER BY tPacote.Nome";

				bd.Consulta(sql);
				
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					linha["LocalID"]= bd.LerInt("LocalID");
					linha["Valor"]= bd.LerDecimal("Valor");
					tabela.Rows.Add(linha);
				}

				bd.Fechar();
			
				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}


		/// <summary>		
		/// Obter a quantidade disponível dado um preço
		/// </summary>
		/// <returns></returns>
		public override int QuantidadeDisponivel(int canalid, int pacoteid){

			try{

				int qtde;

				string sql = "SELECT Quantidade "+
					"FROM tCanalPacote (NOLOCK) "+
					"WHERE PacoteID="+pacoteid+" AND CanalID="+canalid;

				object obj = bd.ConsultaValor(sql);

				if (obj!=null)
					qtde = (int)obj;
				else
					throw new CanalPacoteException("O canal não está vendendo esse pacote.");

				bd.Fechar();

				return qtde;

			}catch(Exception ex){
				throw ex;
			}

		}

		/// <summary>		
		/// Obter a quantidade disponível dado um preço
		/// </summary>
		/// <returns></returns>
		public override int QuantidadeDisponivel(){

			try{

				int qtde;

				string sql = "SELECT Quantidade "+
					"FROM tCanalPacote (NOLOCK) "+
					"WHERE tCanalPacote.PacoteID="+this.PacoteID.Valor+" AND tCanalPacote.CanalID="+this.CanalID.Valor;

				object obj = bd.ConsultaValor(sql);

				if (obj!=null)
					qtde = (int)obj;
				else
					throw new CanalPacoteException("O canal não está vendendo esse pacote.");

				bd.Fechar();

				return qtde;

			}catch(Exception ex){
				throw ex;
			}

		}

        /// <summary>		
        /// Obter canais de um pacote especifico
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(int empresaID, int pacoteID)
        {

            DataTable tabela = new DataTable("Canal");

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Empresa", typeof(string));
            tabela.Columns.Add("CanalPacoteID", typeof(int));
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("TaxaMinima", typeof(decimal));
            tabela.Columns.Add("TaxaMaxima", typeof(decimal));
            tabela.Columns.Add("TaxaComissao", typeof(int));
            tabela.Columns.Add("ComissaoMinima", typeof(decimal));
            tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tCanal.ID, " +
                    "   tCanal.Nome, " +
                    "   tCanalPacote.ID AS CanalPacoteID, " +
                    "   tCanalPacote.Quantidade, " +
                    "   tCanalPacote.TaxaConveniencia, " +
                    "   tCanalPacote.TaxaMinima, " +
                    "   tCanalPacote.TaxaMaxima, " +
                    "   tEmpresa.Nome AS Empresa, " +
                    "   tCanalPacote.TaxaComissao, " +
                    "   tCanalPacote.ComissaoMinima, " +
                    "   tCanalPacote.ComissaoMaxima " +
                    "FROM tCanal (NOLOCK) " +
                    "INNER JOIN tCanalPacote (NOLOCK) ON tCanalPacote.CanalID = tCanal.ID " +
                    "INNER JOIN tEmpresa (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID " +
                    "WHERE " +
                    "   (1 = 1) " + 
                    ((pacoteID != 0) ? "AND (tCanalPacote.PacoteID = " + pacoteID + ") " : "") + 
                    ((empresaID != 0) ? "AND (tCanal.EmpresaID = " + empresaID + ") " : "") + 
                    "ORDER BY " +
                    "   tCanal.Nome"))
                {
                    DataRow linha;
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();

                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Empresa"] = bd.LerString("Empresa");
                        linha["CanalPacoteID"] = bd.LerInt("CanalPacoteID");
                        linha["Quantidade"] = bd.LerInt("Quantidade");
                        linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                        linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                        linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                        linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                        linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");

                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }
		
		/// <summary>		
		/// Obter canais de um pacote especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Canais(int pacoteID){
            
            return Canais(0, pacoteID);

		}

		/// <summary>		
		/// Obter Taxa de Conveniencia de um canal dada o canal e o pacote
		/// </summary>
		/// <returns></returns>
		public int BuscaTaxaConveniencia(int canalid, int pacoteid, CTLib.BD database)
        {

			try{

				int taxaConveniencia;

				string sql = "SELECT TaxaConveniencia "+
					"FROM tCanalPacote (NOLOCK) "+
					"WHERE CanalID="+canalid+" AND PacoteID="+pacoteid;

				object ret = database.ConsultaValor(sql);
				taxaConveniencia = (ret!=null) ? (int)ret : 0;

				return taxaConveniencia;

			}catch{
				throw;
			}finally{
				database.FecharConsulta();
			}

		}
        /// <summary>		
        /// Obter Taxa de Conveniencia de um canal dada o canal e o pacote
        /// </summary>
        /// <returns></returns>
        public override int BuscaTaxaConveniencia(int canalid, int pacoteid)
        {
            BD database = new BD();
            try
            {

                int taxaConveniencia;

                string sql = "SELECT TaxaConveniencia " +
                    "FROM tCanalPacote (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND PacoteID=" + pacoteid;

                object ret = database.ConsultaValor(sql);
                taxaConveniencia = (ret != null) ? (int)ret : 0;

                return taxaConveniencia;

            }
            catch
            {
                throw;
            }
            finally
            {
                database.FecharConsulta();
            }

        }
        /// <summary>		
        /// Obter Taxa de Conveniencia minima e maxima (nessa ordem) de um canal dada o canal e o pacote
        /// </summary>
        /// <returns></returns>
        public decimal[] BuscaTaxasMinMax(int canalid, int pacoteID)
        {

            try
            {

                decimal[] taxas = new decimal[2];

                string sql = "SELECT TaxaMinima,TaxaMaxima " +
                    "FROM tCanalPacote (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND PacoteID=" + pacoteID;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    taxas[0] = bd.LerDecimal("TaxaMinima");
                    taxas[1] = bd.LerDecimal("TaxaMaxima");
                }
                return taxas;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }
        /// <summary>		
        /// Obter Taxas de conveniência e comissao de um canal dada o canal e o pacote
        /// </summary>
        /// <returns></returns>
        public DataTable BuscaTaxasConvenienciaComissao(int canalid, int pacoteID)
        {

            try
            {

                DataTable taxas = new DataTable();
                taxas.Columns.Add("TaxaConveniencia", typeof(int));
                taxas.Columns.Add("TaxaMinima", typeof(decimal));
                taxas.Columns.Add("TaxaMaxima", typeof(decimal));
                taxas.Columns.Add("TaxaComissao", typeof(int));
                taxas.Columns.Add("ComissaoMinima", typeof(decimal));
                taxas.Columns.Add("ComissaoMaxima", typeof(decimal));


                string sql = "SELECT TaxaConveniencia,TaxaMinima,TaxaMaxima,TaxaComissao,ComissaoMinima,ComissaoMaxima " +
                    "FROM tCanalPacote (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND PacoteID=" + pacoteID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = taxas.NewRow();
                    linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                    taxas.Rows.Add(linha);
                }
                return taxas;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool atualizaTaxasCanalPacote(List<EstruturaTaxasEventoPacotes> listaAtualizacao)
        {
            try
            {
                bool valida = true;

                bd.IniciarTransacao();

                for (int cont = 0; cont < listaAtualizacao.Count; cont++)
                {
                    this.Limpar();
                    this.Control.ID = listaAtualizacao[cont].CanalPacoteId;
                    this.CanalID.Valor = listaAtualizacao[cont].CanalID;
                    this.PacoteID.Valor = listaAtualizacao[cont].PacoteID;
                    this.Quantidade.Valor = listaAtualizacao[cont].Quantidade;
                    this.TaxaConveniencia.Valor = listaAtualizacao[cont].TaxaConveniencia;
                    this.TaxaMinima.Valor = listaAtualizacao[cont].TaxaMinima;
                    this.TaxaMaxima.Valor = listaAtualizacao[cont].TaxaMaxima;
                    this.TaxaComissao.Valor = listaAtualizacao[cont].TaxaComissao;
                    this.ComissaoMinima.Valor = listaAtualizacao[cont].ComissaoMinima;
                    this.ComissaoMaxima.Valor = listaAtualizacao[cont].ComissaoMaxima;

                    valida &= Atualizar(bd);

                    if (!valida)
                    {
                        throw new CanalPacoteException("Não foi possível atualizar as taxas!");
                    }
                }

                bd.FinalizarTransacao();
                return valida;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }
	}

	public class CanalPacoteLista : CanalPacoteLista_B{
	
		public CanalPacoteLista(){}
	
		public CanalPacoteLista(int usuarioIDLogado) : base (usuarioIDLogado){}


        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tCanalPacote(NOLOCK)";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanalPacote (NOLOCK)";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

		public bool AlterarTaxas(int[] canalPacotesIDs, int[] canaisIDs, int pacoteID, int taxa,decimal taxaMin,decimal taxaMax,int comissao, decimal comissaoMin,decimal comissaoMax){

			try{

				bool ok = true;

				bd.IniciarTransacao();

				//CanalPacote canalPacote = new CanalPacote(this.CanalPacote.Control.UsuarioID);

				for(int i=0; i < canalPacotesIDs.Length; i++)
                {
					int canalPacoteID = canalPacotesIDs[i];
					int canalID = canaisIDs[i];

					CanalPacote.Limpar();
					CanalPacote.Control.ID = canalPacoteID;
					CanalPacote.CanalID.Valor = canalID;
					CanalPacote.PacoteID.Valor = pacoteID;
					CanalPacote.TaxaConveniencia.Valor = taxa;
                    canalPacote.TaxaMinima.Valor = taxaMin;
                    canalPacote.TaxaMaxima.Valor = taxaMax;
                    canalPacote.TaxaComissao.Valor = comissao;
                    canalPacote.ComissaoMinima.Valor = comissaoMin;
                    canalPacote.ComissaoMaxima.Valor = comissaoMax;

					ok &= CanalPacote.Atualizar(bd);

					if (!ok)
                    {
						throw new CanalPacoteException("Não conseguiu atualizar canal "+canalID+" e pacote "+pacoteID);
					}

				}

				bd.FinalizarTransacao();

				return ok;

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
		/// Obtem uma tabela a ser jogada num relatorio
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){
				
			try{

				DataTable tabela = new DataTable("RelatorioCanalPacote");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Pacote", typeof(string));
					tabela.Columns.Add("Quantidade", typeof(int));
					tabela.Columns.Add("TaxaConveniencia", typeof(int));

					do{
						DataRow linha = tabela.NewRow();
						Pacote pacote = new Pacote();
						pacote.Ler(canalPacote.PacoteID.Valor);
						linha["Pacote"]= pacote.Nome.Valor;
						linha["Quantidade"]= canalPacote.Quantidade.Valor;
						linha["TaxaConveniencia"]= canalPacote.TaxaConveniencia.Valor;
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
