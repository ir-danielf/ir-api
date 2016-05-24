/**************************************************
* Arquivo: FormaPagamentoEvento.cs
* Gerado: 26/02/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;

namespace IRLib.Paralela
{

    public class FormaPagamentoEvento : FormaPagamentoEvento_B
    {

        public FormaPagamentoEvento() { }

        public FormaPagamentoEvento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Inserir novo(a) FormaPagamentoEvento
        /// </summary>
        /// <returns></returns>	
        internal bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cFormaPagamentoEvento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFormaPagamentoEvento(ID, EventoID, FormaPagamentoID) ");
                sql.Append("VALUES (@ID,@001,@002)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I", bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cFormaPagamentoEvento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable GetFormasPagamento(int eventoID, int canalID)
        {
            try
            {
                DataTable tabela = new DataTable("EventoTaxaEntrega");
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));

                bool amexAtivo = false;
                if (canalID == Canal.CANAL_CALL_CENTER && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["InicioAmex"]) || !string.IsNullOrEmpty(ConfigurationManager.AppSettings["FimAmex"]))
                {
                    DateTime dtInicio = DateTime.ParseExact(ConfigurationManager.AppSettings["InicioAmex"].ToString(), "yyyyMMddHHmmss", new CultureInfo("pt-BR", true));
                    DateTime dtFinal = DateTime.ParseExact(ConfigurationManager.AppSettings["FimAmex"].ToString(), "yyyyMMddHHmmss", new CultureInfo("pt-BR", true));

                    if (dtInicio <= DateTime.Now || dtFinal >= DateTime.Now)
                        amexAtivo = true;
                }

                string sql = string.Empty;
                if (!amexAtivo)
                    sql = @"SELECT DISTINCT tFormaPagamentoEvento.FormaPagamentoID, tFormaPagamentoEvento.EventoID 
                                FROM tFormaPagamentoEvento (NOLOCK)
                                INNER JOIN tCanalFormaPagamento cfp (NOLOCK) ON tFormaPagamentoEvento.FormaPagamentoID = cfp.FormaPagamentoID
                                WHERE EventoID = " + eventoID + " AND cfp.CanalID = " + canalID;
                else
                    sql = @"EXEC sp_getFormasPagamentoCampanhaAmex @EventoID = " + eventoID + ", @CanalID = " + canalID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    tabela.Rows.Add(linha);
                }

                if (bd.Consulta().NextResult() && bd.Consulta().Read())
                {
                    int fpID = bd.LerInt("ID");
                    DataRow linha = tabela.NewRow();
                    linha["EventoID"] = eventoID;
                    linha["FormaPagamentoID"] = fpID;
                    tabela.Rows.Add(linha);

                }
                return tabela;
            }
            finally
            {
                bd.Fechar();
            }
        }


    }

    public class FormaPagamentoEventoLista : FormaPagamentoEventoLista_B
    {

        public FormaPagamentoEventoLista() { }

        public FormaPagamentoEventoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
