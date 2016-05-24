/**************************************************
* Arquivo: EventoDespesa.cs
* Gerado: 05/01/2006
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib.Paralela{

	public class EventoDespesa : EventoDespesa_B{
	
		public EventoDespesa(){}
	
		public EventoDespesa(int usuarioIDLogado) : base (usuarioIDLogado){}


	}

	public class EventoDespesaLista : EventoDespesaLista_B{
	
		public EventoDespesaLista(){}
			
		/// <summary>
		/// Obtem uma tabela a ser jogada num relatorio
		/// </summary>
		/// <returns></returns>
		public override DataTable Relatorio(){
				
			try{

				DataTable tabela = new DataTable("RelatorioEventoDespesa");
			
				if (this.Primeiro()){
				
					tabela.Columns.Add("Evento", typeof(string));
					tabela.Columns.Add("Nome", typeof(string));
					tabela.Columns.Add("PorValor", typeof(string));
					tabela.Columns.Add("Porcentagem", typeof(decimal));
					tabela.Columns.Add("ValorMinimo", typeof(decimal));

					do{
						DataRow linha = tabela.NewRow();
						Evento evento = new Evento();
						evento.Ler(eventoDespesa.EventoID.Valor);
						linha["Evento"]= evento.Nome.Valor;
						linha["Nome"]= eventoDespesa.Nome.Valor;
						linha["PorValor"]= (eventoDespesa.PorValor.Valor) ? "T" : "F";
						linha["Porcentagem"]= eventoDespesa.Porcentagem.Valor;
						linha["ValorMinimo"]= eventoDespesa.ValorMinimo.Valor;
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
	
		public EventoDespesaLista(int usuarioIDLogado) : base (usuarioIDLogado){}
		
		/// <summary>
		/// Carrega a lista
		/// </summary>
		/// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
		/// <returns></returns>		
		public new void Carregar(int localID)
		{
		
			try
			{
			
				string sql;
			
				sql = "SELECT DISTINCT tEventoDespesa.ID, tEventoDespesa.Nome FROM tEventoDespesa(NOLOCK), tEvento(NOLOCK), tApresentacao(NOLOCK) WHERE tEventoDespesa.EventoID = tEvento.ID AND tEvento.ID = tApresentacao.EventoID AND DisponivelAjuste = 'T' AND tEventoDespesa.LocalID = "+localID + "ORDER BY tEventoDespesa.Nome" ;
				
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
	}
	
}
