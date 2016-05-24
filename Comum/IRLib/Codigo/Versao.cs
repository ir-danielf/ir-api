/**************************************************
* Arquivo: Versao.cs
* Gerado: 26/01/2006
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib
{

    public class Versao : Versao_B
    {

        public Versao() { }

        public Versao(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Verifica se essa versao esta atualizada ou nao.
        /// </summary>
        /// <returns></returns>
        public override bool Atualizada(int major, int minor)
        {

            try
            {

                bool upToDate = true;

                try
                {

                    CarregarInfoVersoes(major, minor);

                    if (this.Major.Valor > major)
                    {
                        upToDate = false;
                    }
                    else if (this.Major.Valor == major)
                    {
                        if (this.Minor.Valor > minor)
                        {
                            upToDate = false;
                        }
                    }
                }
                catch { }

                return upToDate;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter a ultima versao registrada e atribuir a esta instancia. Se nao houver versoes, devolve null.
        /// </summary>
        public override void UltimaVersao()
        {

            try
            {

                string sql = "SELECT * FROM tVersao ORDER BY ID desc";
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.Major.Valor = bd.LerInt("Major");
                    this.Minor.Valor = bd.LerInt("Minor");
                    this.AtualizacaoObrigatoria.Valor = bd.LerBoolean("AtualizacaoObrigatoria");
                    this.AvisaCliente.Valor = bd.LerBoolean("AvisaCliente");
                    this.Descricao.Valor = bd.LerString("Descricao");
                }
                else
                {
                    throw new VersaoException("Não há versões.");
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CarregarInfoVersoes(int major, int minor)
        {
            string sql = "SELECT * FROM tVersao WHERE (Major > " + major + " OR (Major >= " + major + " AND Minor > " + minor + ")) ORDER BY ID desc";
            bd.Consulta(sql);

            while (bd.Consulta().Read())
                if (bd.LerBoolean("AtualizacaoObrigatoria") || (!this.AtualizacaoObrigatoria.Valor && !bd.LerBoolean("AtualizacaoObrigatoria")))
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.Major.Valor = bd.LerInt("Major");
                    this.Minor.Valor = bd.LerInt("Minor");
                    this.AtualizacaoObrigatoria.Valor = bd.LerBoolean("AtualizacaoObrigatoria");
                    this.AvisaCliente.Valor = bd.LerBoolean("AvisaCliente");
                    this.Descricao.Valor = bd.LerString("Descricao");
                }


        }

        /// <summary>
        /// Obter todas as versoes.
        /// </summary>
        /// <returns></returns>
        public override DataTable Todas()
        {

            try
            {

                DataTable tabela = new DataTable("Versao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Major", typeof(int));
                tabela.Columns.Add("Minor", typeof(int));
                tabela.Columns.Add("AtualizacaoObrigatoria", typeof(bool));
                tabela.Columns.Add("AvisaCliente", typeof(bool));

                string sql = "SELECT ID,Major,Minor,AtualizacaoObrigatoria " +
                    "FROM tVersao ORDER BY ID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Major"] = bd.LerInt("Major");
                    linha["Minor"] = bd.LerInt("Minor");
                    linha["AtualizacaoObrigatoria"] = bd.LerBoolean("AtualizacaoObrigatoria");
                    linha["AvisaCliente"] = bd.LerBoolean("AvisaCliente");
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

        public override string ToString()
        {

            string versao = "";

            if (this.Control.ID != 0)
                versao = this.Major + "." + this.Minor;

            return versao;

        }


    }

    public class VersaoLista : VersaoLista_B
    {

        public VersaoLista() { }

        public VersaoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }


        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioVersao");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Major", typeof(string));
                    tabela.Columns.Add("Minor", typeof(string));
                    tabela.Columns.Add("Obrigatorio", typeof(string));
                    tabela.Columns.Add("Avisa", typeof(string));
                    tabela.Columns.Add("Descricao", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Major"] = versao.Major.Valor.ToString("00");
                        linha["Minor"] = versao.Minor.Valor.ToString("00000");
                        linha["Obrigatorio"] = (versao.AtualizacaoObrigatoria.Valor) ? "sim" : "não";
                        linha["Avisa"] = (versao.AvisaCliente.Valor) ? "sim" : "não";
                        linha["Descricao"] = versao.Descricao.Valor;
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

    }

}
