using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class FormaPagamentoLista : List<FormaPagamento>
    {
        private DAL oDAL = new DAL();
        FormaPagamento oFormaPagamento;

        public FormaPagamentoLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Apresentações do tipo Apresentação, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        private FormaPagamentoLista CarregarLista(string clausula)
        {
            string strSql = string.Empty;

            if (clausula != "")
            {
                strSql = "SELECT IR_FormaPagamentoID, Nome, Parcelas FROM FormaPagamento " +
                            "FROM FormaPagamento (NOLOCK) " +
                            "WHERE " + clausula + " ORDER BY Nome, Parcelas";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oFormaPagamento = new FormaPagamento(Convert.ToInt32(dr["IR_FormaPagamentoID"].ToString()));
                        oFormaPagamento.Nome = dr["Nome"].ToString();
                        oFormaPagamento.Parcelas = (int)dr["Parcelas"];                            

                        this.Add(oFormaPagamento);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// Retorna uma Lista de Apresentacoes do tipo apresentacao, 
        /// a partir de um EventoID
        /// </summary>
        public FormaPagamentoLista CarregarDadosPorEventoID(int id)
        {
            return CarregarLista("(EventoID = " + id + ") ");
        }

        public FormaPagamentoLista CarregarDadosPorEventos(int clienteID, string sessionID)
        {
            try
            {
                string strSql = "sp_getFormasPagamento " + clienteID + ", " + sessionID;
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oFormaPagamento = new FormaPagamento(Convert.ToInt32(dr["IR_FormaPagamentoID"].ToString()));
                        this.Add(oFormaPagamento);
                    }
                }
                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}
