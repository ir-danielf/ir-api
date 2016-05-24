/**************************************************
* Arquivo: CanalFormaPagamento.cs
* Gerado: 04/07/2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib{

	public class CanalFormaPagamento : CanalFormaPagamento_B{
	
		public CanalFormaPagamento(){}
	
		public CanalFormaPagamento(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>		
		/// Obter formas de pagamento de um canal especifico
		/// </summary>
		/// <returns></returns>
		public DataTable FormasPagamento(int canalid, string registroZero){

            this.CanalID.Valor = canalid;

            DataTable tabela = new DataTable("FormaPagamento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                using (IDataReader oDataReader = bd.Consulta("" + 
                    "SELECT " + 
                    "   fp.ID, " + 
                    "   fp.Nome " + 
                    "FROM tFormaPagamento (NOLOCK) as fp " +
                    "INNER JOIN tCanalFormaPagamento (NOLOCK) as cfp ON cfp.FormaPagamentoID = fp.ID " +
                    "WHERE " + 
                    "   cfp.CanalID = " + canalid + " " + 
                    "ORDER BY fp.Nome"))
                {
                    while (oDataReader.Read())
                    {

                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
		}
		
		/// <summary>		
		/// Obter formas de pagamento de um canal especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable FormasPagamento(int canalid){
            return FormasPagamento(canalid, null);
		}

		public DataTable FormasPagamentoVariosCanais(string Canais) //Obtem as formas de pagamento para diversos canais
		{

			try
			{

				DataTable tabela = new DataTable("FormaPagamento");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				//if (registroZero!=null)
					//tabela.Rows.Add(new Object[] {0, registroZero});

                string sql = "SELECT DISTINCT fp.ID,fp.Nome FROM tFormaPagamento (NOLOCK) as fp, tCanalFormaPagamento (NOLOCK) as cfp " +
					"WHERE cfp.FormaPagamentoID=fp.ID AND "+
					"cfp.CanalID IN ("+Canais+") ORDER BY fp.Nome";

				bd.Consulta(sql);

				DataRow linhaTodos = tabela.NewRow();
				linhaTodos["ID"]= -1;
				linhaTodos["Nome"]= "Todas";
				tabela.Rows.Add(linhaTodos); 

				while(bd.Consulta().Read())
				{

					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}
			catch(Exception ex)
			{
				throw ex;
			}

		}
		
		/// <summary>		
		/// Obter canais de uma forma de pagamento especifico
		/// </summary>
		/// <returns></returns>
		public override DataTable Canais(int formapagamentoid){

			try{

				DataTable tabela = new DataTable("Canal");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT c.ID,c.Nome FROM tCanal (NOLOCK) as c, tCanalFormaPagamento (NOLOCK) as cfp " +
					"WHERE cfp.CanalID=c.ID AND "+
					"cfp.FormaPagamentoID="+formapagamentoid+" ORDER BY c.Nome";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){

					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}
		
	}

	public class CanalFormaPagamentoLista : CanalFormaPagamentoLista_B{
	
		public CanalFormaPagamentoLista(){}
	
		public CanalFormaPagamentoLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
			
		/// <summary>
		/// Obtem uma tabela a ser jogada num relatorio
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){
				
			try{

				DataTable tabela = new DataTable("RelatorioCanalFormaPagamento");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Canal", typeof(string));
					tabela.Columns.Add("FormaPagamento", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						Canal canal = new Canal();
						canal.Ler(canalFormaPagamento.CanalID.Valor);
						linha["Canal"]= canal.Nome.Valor;
						FormaPagamento fp = new FormaPagamento();
						fp.Ler(canalFormaPagamento.FormaPagamentoID.Valor);
						linha["FormaPagamento"]= fp.Nome.Valor;
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
