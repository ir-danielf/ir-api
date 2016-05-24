/**************************************************
* Arquivo: FormaPagamentoEvento.cs
* Gerado: 26/02/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace IRLib
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
            bool result = false;

            var sql = "INSERT INTO tFormaPagamentoEvento(EventoID, FormaPagamentoID) VALUES (@EventoID, @FormaPagamentoID);";

            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@EventoID", this.EventoID.ValorBD));
            parametros.Add(new SqlParameter("@FormaPagamentoID", this.FormaPagamentoID.ValorBD));

            
            if (bd.Executar(sql, parametros) == 1)
            {
                object x = bd.ConsultaValor("SELECT @@IDENTITY AS [@@IDENTITY];");
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                result = this.Control.ID > 0;
            }

            return result;

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

        public void DistribuiFormasPagamento(BD bd, int eventoID = 0)
        {
            if (eventoID == 0)
                eventoID = this.Control.ID;
            
            bd.ExecutarComParametros(@"
                INSERT INTO tFormaPagamentoEvento (EventoID, FormaPagamentoID)
                SELECT  
                @EventoID,
                tFormaPagamento.ID  
                FROM  
                tFormaPagamento (NOLOCK)  
                WHERE  
                tFormaPagamento.Padrao = 'T'", new SqlParameter("@EventoID", eventoID));
        }


    }

    public class FormaPagamentoEventoLista : FormaPagamentoEventoLista_B
    {

        public FormaPagamentoEventoLista() { }

        public FormaPagamentoEventoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
