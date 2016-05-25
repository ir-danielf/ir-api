/**************************************************
* Arquivo: Regiao.cs
* Gerado: 19/09/2005
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib.Paralela
{

    public class Regiao : Regiao_B
    {

        public const int TAXA_WEB = 4;//para não carregar na VendaBilheteria

        public Regiao() { }

        public Regiao(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obter as taxas de entrega dessa regiao
        /// </summary>
        /// <returns></returns>
        public override DataTable Taxas()
        {

            try
            {

                DataTable tabela = new DataTable("TaxaEntrega");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("RegiaoID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT ID,Nome,Prazo,Valor " +
                    "FROM tTaxaEntrega " +
                    "WHERE Disponivel='T' AND " +
                    "RegiaoID=" + this.Control.ID + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    int prazo = bd.LerInt("Prazo");
                    string complemento = (prazo > 1) ? " dias" : " dia";
                    linha["Nome"] = bd.LerString("Nome") + " - " + prazo + complemento;
                    linha["Valor"] = bd.LerDecimal("Valor");
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
        /// Obter as taxas de entrega dessa regiao (com registro zero)
        /// </summary>
        /// <returns></returns>
        public DataTable Taxas(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("TaxaEntrega");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("RegiaoID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT ID,Nome,Prazo,Valor FROM tTaxaEntrega WHERE Disponivel='T' AND " +
                    "RegiaoID=" + this.Control.ID + " ORDER BY Nome";

                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    int prazo = bd.LerInt("Prazo");
                    string complemento = (prazo > 1) ? " dias" : " dia";
                    linha["Nome"] = bd.LerString("Nome") + " - " + prazo + complemento;
                    linha["Valor"] = bd.LerDecimal("Valor");
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

        public DataTable GetRegioes(int eventoID)
        {
            return this.GetRegioes(new int[] { eventoID });
        }

        public DataTable GetRegioes(int[] eventoID)
        {
            try
            {
                DataTable tabela = new DataTable("Regioes");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("TaxaEntregaID", typeof(int));

                string eventosID = string.Empty;

                foreach (int item in eventoID)
                    eventosID += item.ToString() + ",";

                eventosID = eventosID.Remove(eventosID.Length - 1, 1);

                string sql = @"SELECT DISTINCT tRegiao.ID AS ID, tRegiao.Nome AS Nome, tEventoTaxaEntrega.EventoID, tEventoTaxaEntrega.TaxaEntregaID  
                                FROM tEventoTaxaEntrega (NOLOCK) 
                                INNER JOIN tTaxaEntrega (NOLOCK) ON tTaxaEntrega.ID = tEventoTaxaEntrega.TaxaEntregaID
                                INNER JOIN tRegiao (NOLOCK) ON tRegiao.ID = tTaxaEntrega.RegiaoID
                                WHERE EventoID IN ( " + eventosID + ") AND tRegiao.ID <> " + Regiao.TAXA_WEB.ToString();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
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
        /// Obter todas as regioes menos a web
        /// </summary>
        /// <returns></returns>
        public override DataTable Todas()
        {

            try
            {

                DataTable tabela = new DataTable("Regiao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID,Nome FROM tRegiao ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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
        /// Obter todas as regioes (com registro zero)
        /// </summary>
        /// <param name="registroZero">Um string que vai conter a descrição do registro zero, se for vazio não insere</param>
        /// <returns>DataTable de Regioess com ID e Nome</returns>
        public DataTable Todas(string registroZero)
        {
            DataTable tabela = new DataTable("Regiao");
            try
            {
                // Criar DataTable com as colunas
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                // Obtendo dados através de SQL
                //BD bd = new BD();
                string sql = "SELECT ID,Nome FROM tRegiao ORDER BY Nome";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        } // fim de Regiaos

        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> CarregarLista(bool registroZero)
        {
            try
            {
                List<IRLib.Paralela.ClientObjects.EstruturaIDNome> lista = new List<IRLib.Paralela.ClientObjects.EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Todos" });
                bd.Consulta("SELECT ID, Nome FROM tRegiao (NOLOCK) ORDER BY Nome");
                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class RegiaoLista : RegiaoLista_B
    {

        public RegiaoLista() { }

        public RegiaoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
