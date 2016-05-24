/**************************************************
* Arquivo: VendaPagamento.cs
* Gerado: 10/05/2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib{

	public class VendaPagamento : VendaPagamento_B{
	
		public VendaPagamento(){}
	
		public VendaPagamento(int usuarioIDLogado) : base (usuarioIDLogado){}


	}

	public class VendaPagamentoLista : VendaPagamentoLista_B{
	
		public VendaPagamentoLista(){}
	
		public VendaPagamentoLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obtem uma tabela de campos ID e 'Descricao' para ser alimentada num combobox ou listbox
		/// </summary>
		/// <returns></returns>
		public override DataTable Tabela(){

			DataTable tabela = new DataTable("VendaPagamento");
				
			tabela.Columns.Add("ID", typeof(int));
			tabela.Columns.Add("PagamentoID", typeof(int));
			tabela.Columns.Add("Valor", typeof(decimal));
				
			try{
			
				if (this.Primeiro()){

					do{
						DataRow linha = tabela.NewRow();
						linha["ID"]= vendaPagamento.Control.ID;
						linha["PagamentoID"]= vendaPagamento.FormaPagamentoID.Valor;
						linha["Valor"]= vendaPagamento.Valor.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}
				
			}catch{
				tabela = null;
			}

			return tabela;

		}

		
	}
	
}
