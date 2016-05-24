/**************************************************
* Arquivo: LocalImagemMapa.cs
* Gerado: 29/11/2006
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace IRLib{

	public class LocalImagemMapa : LocalImagemMapa_B{
	
		public LocalImagemMapa(){}
	
		public LocalImagemMapa(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Obter todas as imagens de mapas
		/// </summary>
		/// <returns></returns>
		public override DataTable Todos(int localid, string registroZero){
		
			DataTable tabela = new DataTable("LocalImagemMapa");
			tabela.Columns.Add("ID", typeof(int));
			tabela.Columns.Add("LocalNome", typeof(string));
			tabela.Columns.Add("Nome", typeof(string));


			string sql = "SELECT tLocalImagemMapa.ID as ID, tLocal.Nome as LocalNome, tLocalImagemMapa.Nome  FROM tLocalImagemMapa, tLocal WHERE tLocal.ID = tLocalImagemMapa.LocalID AND tLocalImagemMapa.LocalID = " + localid;

			if (registroZero!=null)
				tabela.Rows.Add(new Object[] {0, string.Empty, registroZero});

			bd.Consulta(sql);

			

			DataRow dr = null;

			while( bd.Consulta().Read())
			{
				dr = tabela.NewRow();
				dr["ID"] = bd.LerInt("ID");
				dr["LocalNome"] = bd.LerString("LocalNome");
				dr["Nome"] = bd.LerString("Nome");

				tabela.Rows.Add(dr);
			}

			return tabela;
		}
	}

	public class LocalImagemMapaLista : LocalImagemMapaLista_B{
	
		public LocalImagemMapaLista(){}
	
		public LocalImagemMapaLista(int usuarioIDLogado) : base (usuarioIDLogado){}

		/// <summary>
		/// Retorna o ID da imagem que representa o mapa do local em uma determinada apresentação.
		/// </summary>
		/// <param name="apresentacaoID">ApresentacaoID --> ID Da apresentação</param>
		/// <returns>string object</returns>
		public int ImagemLocalApresentacao(int eventoID)
		{
            string sql = string.Empty;
                
            //sql = "SELECT tLocalImagemMapa.ID FROM tLocalImagemMapa, tApresentacao WHERE tApresentacao.LocalImagemMapaID = tLocalImagemMapa.ID AND tApresentacao.ID = " + eventoID;
            StringBuilder strSql = new StringBuilder();

            strSql.Append("      SELECT tLocalImagemMapa.ID ");
            strSql.Append("        FROM tLocalImagemMapa ");
            strSql.Append("  INNER JOIN tEvento  ");
            strSql.Append(" 		 ON tLocalImagemMapa.ID = tEvento.LocalImagemMapaID ");
            strSql.Append("       WHERE tEvento.ID = "+eventoID);

			bd.Consulta(strSql.ToString()); 

			if (bd.Consulta().Read())
				return bd.LerInt("ID");
			else
				return 0;
		}


        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tLocalImagemMapa WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY LocalID, Nome");

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
