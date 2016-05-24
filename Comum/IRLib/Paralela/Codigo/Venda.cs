/**************************************************
* Arquivo: Venda.cs
* Gerado: 16/05/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Diagnostics;

namespace IRLib.Paralela{

	public class Venda : Venda_B{
	
		public Venda(){}
	
		public Venda(int usuarioIDLogado) : base (usuarioIDLogado){}


		/// <summary>		
		/// Obtehm o total das vendas nao pagas de um ingresso (de todos caixas)
		/// </summary>
		/// <returns></returns>
		public override decimal TotalIngresso(int ingresso){
			decimal total =-1;
			try{
				BD bd = new BD();
				string sql = 
					"SELECT        SUM(tComandaItem.PrecoVenda * tComandaItem.Quantidade) AS Soma, tVenda.Pago, tVenda.IngressoID "+
					"FROM          tComanda INNER JOIN "+
										"tComandaItem ON tComanda.ID = tComandaItem.ComandaID INNER JOIN "+
										"tVenda ON tComanda.VendaID = tVenda.ID "+
					"GROUP BY tVenda.Pago, tVenda.IngressoID "+
					"HAVING        (tVenda.Pago = 'F') AND (tVenda.IngressoID = "+ ingresso +")";
				bd.Consulta(sql);
				if (bd.Consulta().Read()){
					total=  bd.LerDecimal("Soma");
				}
				bd.Fechar();
			}catch{
				Debug.Fail("Falha ao obter valor total por Ingresso (conta em aberto)!");
			}
			return total;
		}
		/// <summary>		
		/// Obtehm o total da comanda nao paga 
		/// </summary>
		/// <returns></returns>
		public override decimal TotalComanda(int comandaID){
			decimal total =0;
			try{
				BD bd = new BD();
				string sql = "SELECT SUM(tVendaPagamento.Valor) AS Soma "+
					"FROM tComanda INNER JOIN tVenda ON tComanda.VendaID = tVenda.ID INNER JOIN "+
					"tVendaPagamento ON tVenda.ID = tVendaPagamento.VendaID "+
					"WHERE (tComanda.ID = " + comandaID +") AND (tVenda.Pago = 'F')";
				bd.Consulta(sql);
				if (bd.Consulta().Read()){
					total=  bd.LerDecimal("Soma");
				}
				bd.Fechar();
			}catch{
				Debug.Fail("Falha ao obter Total de Venda por Comanda!");
			}
			return total;
		} // fim de TotalComanda

		/// <summary>		
		/// Cria um Datatable com os campos de Itens de Pagamentos
		/// </summary>
		/// <returns></returns>
		public DataTable EstruturaItensPagamento(){
			DataTable tabela = new DataTable("ItensPagamento");
			try {
				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("FormaPagamentoID", typeof(int));
				tabela.Columns.Add("FormaPagamento", typeof(string));
				tabela.Columns.Add("Valor", typeof(decimal));
			}catch{
				tabela = null;
			}
			return tabela;
		}

		/// <summary>		
		/// Devolve todas as venda pagamentos de um ingresso (de todos caixas)
		/// </summary>
		/// <returns></returns>
		public override DataTable Pagamentos(){

			try{

				DataTable tabela = EstruturaItensPagamento();

				string sql = "SELECT f.ID, f.Nome, vp.Valor FROM tFormaPagamento as f, tVendaPagamento as vp, "+
					"tvenda as v WHERE f.ID=vp.FormaPagamentoID AND v.ID=vp.VendaID AND "+
					"v.Ingresso='"+this.IngressoID.Valor+ " ORDER BY f.Nome";
				
				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= this.Control.ID;
					linha["FormaPagamentoID"]= bd.LerInt("ID");
					linha["FormaPagamento"]= bd.LerString("Nome");
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
		/// Devolve todas as venda pagamentos de um ingresso
		/// </summary>
		/// <returns></returns>
		public override DataTable PagamentosPorIngresso(int ingressoInformado){
			DataTable tabela = EstruturaItensPagamento();
			try{
				BD bd = new BD();
				string sql = 
					"SELECT      tVendaPagamento.Valor, tVenda.ID, tVendaPagamento.FormaPagamentoID, tFormaPagamento.Nome "+
					"FROM        tVenda INNER JOIN "+
									"tVendaPagamento ON tVenda.ID = tVendaPagamento.VendaID INNER JOIN "+
									"tFormaPagamento ON tVendaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
					"WHERE        (tVenda.Pago = 'F') AND (tVenda.IngressoID = '"+ ingressoInformado+ "')";
				bd.Consulta(sql);
				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= this.Control.ID;
					linha["FormaPagamentoID"]= bd.LerInt("ID");
					linha["FormaPagamento"]= bd.LerString("Nome");
					linha["Valor"]= bd.LerDecimal("Valor");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
			}catch{
				Debug.Fail("Falha ao obter Pagamentos por Comanda!!");
			}
			return tabela;
		} // fim de PagamentosPorComanda

	}

	public class VendaLista : VendaLista_B{
	
		public VendaLista(){}
	
		public VendaLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
	}
	
}
