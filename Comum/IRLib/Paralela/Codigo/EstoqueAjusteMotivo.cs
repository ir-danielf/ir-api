/**************************************************
* Arquivo: EstoqueAjusteMotivo.cs
* Gerado: quarta-feira, 13 de abril de 2005
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela {

	public class EstoqueAjusteMotivo : EstoqueAjusteMotivo_B	{
	
		public EstoqueAjusteMotivo() {}
	
		public EstoqueAjusteMotivo(int usuarioIDLogado) : base (usuarioIDLogado){}

	}

	public class EstoqueAjusteMotivoLista : EstoqueAjusteMotivoLista_B {
	
		public EstoqueAjusteMotivoLista() {}
	
		public EstoqueAjusteMotivoLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
		/// <summary>
		/// Obtem uma tabela de todos os campos de estoqueajustemotivo carregados na lista
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){

			DataTable tabela = new DataTable("EstoqueAjusteMotivo");
				
			try{
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("Descricao", typeof(string));

					do{
						DataRow linha = tabela.NewRow();
						linha["Nome"]= estoqueAjusteMotivo.Nome.Valor;
						linha["Descricao"]= estoqueAjusteMotivo.Descricao.Valor;
						tabela.Rows.Add(linha);
					}while(this.Proximo());

				}else{ //erro: nao carregou a lista
					tabela = null;
				}
				
			}catch{
				tabela = null;
			}			
			
			return tabela;

		}	

	}
	
}
