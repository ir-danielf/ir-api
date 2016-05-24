/**************************************************
* Arquivo: Cor.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;

namespace IRLib.Paralela{

	public class Cor : Cor_B{
	
		public Cor(){}
	
		public Cor(int usuarioIDLogado) : base (usuarioIDLogado){}
		
		/// <summary>		
		/// Obter todas as cores
		/// </summary>
		/// <returns></returns>
		public override DataTable Todas(){

			try{

				DataTable tabela = new DataTable("Cor");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("RGB", typeof(string));

				string sql = "SELECT * FROM tCor ORDER BY ID";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){
					DataRow linha = tabela.NewRow();
					linha["ID"]= bd.LerInt("ID");
					linha["Nome"]= bd.LerString("Nome");
					linha["RGB"]= bd.LerString("RGB");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}catch(Exception ex){
				throw ex;
			}

		}
		
		/// <summary>		
		/// Obter preços indicados nessa cor
		/// </summary>
		/// <returns></returns>
		public override DataTable Precos(){
			return null;
		}
		
		/// <summary>		
		/// Obter cortesias indicados nessa cor
		/// </summary>
		/// <returns></returns>
		public override DataTable Cortesias(){
			return null;
		}
		
		/// <summary>		
		/// Obter bloqueios indicados nessa cor
		/// </summary>
		/// <returns></returns>
		public override DataTable Bloqueios(){
			return null;
		}
		

	}

	public class CorLista : CorLista_B{
	
		public CorLista(){}
	
		public CorLista(int usuarioIDLogado) : base (usuarioIDLogado){}


        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tCor WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
	
}
