/**************************************************
* Arquivo: Cortesia.cs
* Gerado: 14/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Data;

namespace IRLib
{

    public class Cortesia : Cortesia_B
    {

        public Cortesia() { }

        public Cortesia(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        /// Obter os ingressos dessa cortesia
        /// </summary>
        /// <returns></returns>
        public override DataTable Ingressos()
        {
            return null;
        }

        private int CortesiaPadrao(int EventoID, int LocalID, bool isEntradaFranca = false)
        {
            int retorno = 0;

            try
            {

                string sql = "";

                if (isEntradaFranca)
                {
                    sql = "SELECT TOP 1 " +
                    "	tCortesia.ID " +
                    "FROM " +
                    "	tCortesia (NOLOCK) " +
                    ((EventoID != 0) ? "" +
                        "INNER JOIN " +
                        "	tLocal (NOLOCK) ON tLocal.ID = tCortesia.LocalID " +
                        "INNER JOIN " +
                        "	tEvento (NOLOCK) ON tEvento.LocalID = tLocal.ID " : "") +
                    "WHERE " +
                    "   1 = 1 " +
                    ((EventoID != 0) ? "" +
                        "AND " +
                        "	(tEvento.ID = " + EventoID + ") " : "") +
                    ((LocalID != 0) ? "" +
                        "AND " +
                        "	(tCortesia.LocalID = " + LocalID + ") " : "") +
                    " ORDER BY ID DESC ";
                }
                else
                {
                   sql = "SELECT " +
                    "	tCortesia.ID " +
                    "FROM " +
                    "	tCortesia (NOLOCK) " +
                    ((EventoID != 0) ? "" +
                        "INNER JOIN " +
                        "	tLocal (NOLOCK) ON tLocal.ID = tCortesia.LocalID " +
                        "INNER JOIN " +
                        "	tEvento (NOLOCK) ON tEvento.LocalID = tLocal.ID " : "") +
                    "WHERE " +
                    "   1 = 1 " +
                    ((EventoID != 0) ? "" +
                        "AND " +
                        "	(tEvento.ID = " + EventoID + ") " : "") +
                    ((LocalID != 0) ? "" +
                        "AND " +
                        "	(tCortesia.LocalID = " + LocalID + ") " : "") +
                    "AND " +
                    "	tCortesia.Padrao = 'T'";
                }

                retorno = Convert.ToInt32(bd.ConsultaValor(sql));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        /// <summary>
        /// Captura a Cortesia Padrão pelo Evento
        /// </summary>
        /// <param name="EventoID">ID do Evento</param>
        /// <returns>ID da Cortesia Padrão</returns>
        public int CortesiaPadraoEvento(int EventoID, bool isEntradaFranca = false)
        {
            return CortesiaPadrao(EventoID, 0, isEntradaFranca);
        }

        /// <summary>
        /// Captura a Cortesia Padrão pelo Local
        /// </summary>
        /// <param name="EventoID">ID do Evento</param>
        /// <returns>ID da Cortesia Padrão</returns>
        public int CortesiaPadraoLocal(int LocalID)
        {
            return CortesiaPadrao(0, LocalID);
        }

        /// <summary>
        /// Inserir novo(a) Cortesia
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            // Se o Padrão foi selecionado deve-se Atualizar todos os Padrões do Local para 'F'
            if (Padrao.Valor)
            {
                bd.Executar("UPDATE tCortesia SET Padrao = 'F' WHERE LocalID = " + LocalID.Valor);
            }

            return base.Inserir();
        }

        // <summary>
        /// Atualiza Cortesia
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {
            // Se o Padrão foi selecionado deve-se Atualizar todos os Padrões do Local para 'F'
            if (Padrao.Valor)
            {
                bd.Executar("UPDATE tCortesia SET Padrao = 'F' WHERE LocalID = " + LocalID.Valor);
            }

            return base.Atualizar();
        }

        /// <summary>		
        /// Obtem todas as cortesias
        /// </summary>
        /// <returns></returns>
        public override DataTable Todas()
        {

            try
            {

                DataTable tabela = new DataTable("Cortesia");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                string sql = "SELECT ID,Nome,LocalID " +
                    "FROM tCortesia " +
                    "ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        /// <summary>		
        /// Obtem todas as cortesias
        /// </summary>
        /// <returns></returns>
        public DataTable Todas(ArrayList locaisIDs)
        {

            try
            {

                DataTable tabela = new DataTable("Cortesia");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                string sql = "SELECT ID,Nome,LocalID " +
                    "FROM tCortesia " +
                    "WHERE LocalID in (" + Utilitario.ArrayToString(locaisIDs) + ") " +
                    "ORDER BY LocalID,Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        public bool VerificaVendas()
        {
            try
            {

                bool verifica = true;
                string cortesiaid = this.Control.ID.ToString();

                string sql = "select top 1 ID from tIngressoLog where CortesiaID = " + cortesiaid;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    verifica = false;
                }

                bd.Fechar();

                return verifica;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool VerificaCortesiaLocal(int LocalID)
        {


            try
            {
                bool verifica = false;
                string sql = @"select top 1 * 
                                from tCortesia(nolock) 
                                where Padrao = 'T' and LocalID = " + LocalID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    verifica = true;
                }
                                
                bd.Fechar();

                return verifica;

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
    }

    public class CortesiaLista : CortesiaLista_B
    {

        public CortesiaLista() { }

        public CortesiaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }


        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioCortesia");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Cor", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = cortesia.Nome.Valor;
                        Cor cor = new Cor();
                        cor.Ler(cortesia.CorID.Valor);
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
                bd.Consulta("SELECT ID FROM tCortesia WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  Nome");

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
