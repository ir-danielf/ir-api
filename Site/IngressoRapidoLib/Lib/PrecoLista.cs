using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for PrecoLista
    /// </summary>
    public class PrecoLista : List<Preco>
    {
        DAL oDAL = new DAL();

        Preco oPreco;

        public PrecoLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Preços do tipo Preco, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        // ARRUMAR ********************************************************************        
        private PrecoLista CarregarLista(string clausula)
        {
            string strSql = string.Empty;

            if (clausula != "")
            {
                strSql = "SELECT IR_PrecoID, Nome, Valor, ApresentacaoID, SetorID, QuantidadePorCliente " +
                         "FROM Preco (NOLOCK) " +
                         "WHERE " + clausula + "";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        if (dr["Nome"].ToString().Trim() != "ClienteVivo")
                        {
                            oPreco = new Preco(Convert.ToInt32(dr["IR_PrecoID"].ToString()));
                            oPreco.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                            oPreco.Valor = Convert.ToDecimal(dr["Valor"]);
                            oPreco.QuantidadePorCliente = (int)dr["QuantidadePorCliente"];

                            this.Add(oPreco);
                        }
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
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

        public PrecoLista CarregarDadosporApresentacaoSetorID(int apresentacaoID, int setorID)
        {
            return CarregarLista("Pacote = 0 AND ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID);
        }
    }

}