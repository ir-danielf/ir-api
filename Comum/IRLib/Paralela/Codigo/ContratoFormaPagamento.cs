/**************************************************
* Arquivo: ContratoFormaPagamento.cs
* Gerado: 23/04/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class ContratoFormaPagamento : ContratoFormaPagamento_B
    {

        public ContratoFormaPagamento() { }

        public ContratoFormaPagamento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        #region Métodos de Manipulação do Contrato

        #region Inserir

        /// <summary>
        /// Inserir novo(a) ContratoFormaPagamento
        /// </summary>
        /// <returns></returns>	
        internal bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cContratoFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContratoFormaPagamento(ID, ContratoID, FormaPagamentoID, Dias, Taxa) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.Dias.ValorBD);
                sql.Replace("@004", this.Taxa.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Atualizar

        /// <summary>
        /// Atualiza ContratoFormaPagamento
        /// </summary>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cContratoFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContratoFormaPagamento SET ContratoID = @001, FormaPagamentoID = @002, Dias = @003, Taxa = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.Dias.ValorBD);
                sql.Replace("@004", this.Taxa.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Controle e Log

        protected void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cContratoFormaPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xContratoFormaPagamento (ID, Versao, ContratoID, FormaPagamentoID, Dias, Taxa) ");
                sql.Append("SELECT ID, @V, ContratoID, FormaPagamentoID, Dias, Taxa FROM tContratoFormaPagamento WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #endregion

        /// <summary>		
        /// Obter todas formas de pagamento do contrato
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(int contratoID)
        {

            DataTable tabela = new DataTable("FormaPagamentoContrato");

            tabela.Columns.Add("FormaPagamentoTipoID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Parcelas", typeof(int));
            tabela.Columns.Add("Dias", typeof(int));
            tabela.Columns.Add("TaxaAdm", typeof(decimal));

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    " 	tFormaPagamentoTipo.ID AS FormaPagamentoTipoID, " +
                    " 	tFormaPagamentoTipo.Nome AS Nome, " +
                    " 	tFormaPagamentoTipo.Parcelas, " +
                    " 	ISNULL(tContratoFormaPagamento.Dias, tFormaPagamentoTipo.Dias) AS Dias, " +
                    " 	ISNULL(tContratoFormaPagamento.Taxa, tFormaPagamentoTipo.Taxa) AS TaxaAdm " +
                    "FROM tFormaPagamentoTipo (NOLOCK) " +
                    "INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.FormaPagamentoTipoID = tFormaPagamentoTipo.ID " +
                    "LEFT JOIN tContratoFormaPagamento (NOLOCK) ON tContratoFormaPagamento.FormaPagamentoID = tFormaPagamento.ID AND tContratoFormaPagamento.ContratoID = " + contratoID + " " +
                    "ORDER BY 	tFormaPagamentoTipo.Nome"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["FormaPagamentoTipoID"] = bd.LerInt("FormaPagamentoTipoID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Parcelas"] = bd.LerInt("Parcelas");
                        linha["Dias"] = bd.LerInt("Dias");
                        linha["TaxaAdm"] = bd.LerDecimal("TaxaAdm");

                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();


            }
            catch (Exception)
            {
                throw;
            }

            return tabela;

        }

    }

    public class ContratoFormaPagamentoLista : ContratoFormaPagamentoLista_B
    {

        public ContratoFormaPagamentoLista() { }

        public ContratoFormaPagamentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
