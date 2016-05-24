/**************************************************
* Arquivo: Bloqueio.cs
* Gerado: 04/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;

namespace IRLib
{

    public class Bloqueio : Bloqueio_B
    {

        private int UsuarioIDLogado;

        public Bloqueio() { }

        public Bloqueio(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            this.UsuarioIDLogado = usuarioIDLogado;
        }

        /// <summary>
        /// Exclui Bloqueio
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            LugarLista lugarLista = new LugarLista();
            lugarLista.FiltroSQL = "BloqueioID=" + this.Control.ID;
            lugarLista.TamanhoMax = 1;
            lugarLista.Carregar();

            if (lugarLista.Tamanho > 0)
            {
                throw new BloqueioException("Não pode excluir esse bloqueio. Ele está sendo usado em mapa(s).");
            }
            else
            {
                return this.Excluir(this.Control.ID);
            }

        }

        /// <summary>		
        /// Obter os ingressos desse bloqueio
        /// </summary>
        /// <returns></returns>
        public override DataTable Ingressos()
        {
            return null;
        }

        /// <summary>		
        /// Obtem todas os bloqueios
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(ArrayList locaisIDs)
        {

            try
            {

                DataTable tabela = new DataTable("Bloqueio");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                string sql = "SELECT b.ID,b.Nome,b.CorID,b.LocalID,c.RGB " +
                    "FROM tBloqueio AS b,tCor AS c " +
                    "WHERE c.ID=b.CorID AND b.LocalID in (" + Utilitario.ArrayToString(locaisIDs) + ") " +
                    "ORDER BY b.LocalID,b.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["RGB"] = bd.LerString("RGB");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Todos(int localID, bool primeiro)
        {

            try
            {

                DataTable tabela = new DataTable("Bloqueio");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                string sql = "SELECT b.ID,b.Nome,b.CorID,b.LocalID,c.RGB " +
                    "FROM tBloqueio AS b,tCor AS c " +
                    "WHERE c.ID=b.CorID AND b.LocalID = " + localID +
                    " ORDER BY b.LocalID,b.Nome";

                bd.Consulta(sql);

                if (primeiro)
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = 0;
                    linha["Nome"] = "Selecione....";
                    linha["CorID"] = 0;
                    linha["RGB"] = "";
                    linha["LocalID"] = 0;
                    tabela.Rows.Add(linha);
                }

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["RGB"] = bd.LerString("RGB");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable QuantidadePorBloqueioID(int[] ids, int apresentacaoSetorID)
        {
            BD bd = new BD();
            bd.Consulta(@"SELECT BloqueioID, COUNT(ID) Qtd
                        FROM tIngresso (NOLOCK) 
                        WHERE BloqueioID IN(" + Utilitario.ArrayToString(ids) + @") AND ApresentacaoSetorID = " + apresentacaoSetorID +
                        @"AND Status = 'B'
                        GROUP BY BloqueioID");
            DataTable retorno = new DataTable("Bloqueios");
            retorno.Columns.Add("BloqueioID", typeof(int));
            retorno.Columns.Add("Qtd", typeof(int));

            DataRow novalinha;
            while (bd.Consulta().Read())
            {
                novalinha = retorno.NewRow();
                novalinha["BloqueioID"] = bd.LerInt("BloqueioID");
                novalinha["Qtd"] = bd.LerInt("Qtd");
                retorno.Rows.Add(novalinha);
            }

            return retorno;
        }
    }

    public class BloqueioLista : BloqueioLista_B
    {

        public BloqueioLista() { }

        public BloqueioLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioBloqueio");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Cor", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = bloqueio.Nome.Valor;
                        Cor cor = new Cor();
                        cor.Ler(bloqueio.CorID.Valor);
                        linha["Cor"] = cor.Nome.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tBloqueio WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Nome");

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
