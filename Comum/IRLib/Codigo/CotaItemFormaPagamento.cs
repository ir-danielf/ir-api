/**************************************************
* Arquivo: CotaItemFormaPagamento.cs
* Gerado: 14/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{

    public class CotaItemFormaPagamento : CotaItemFormaPagamento_B
    {

        public CotaItemFormaPagamento() { }

        /// <summary>
        /// Inserir novo(a) CotaItemFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tCotaItemFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaItemFormaPagamento(ID, CotaItemID, FormaPagamentoID) ");
                sql.Append("VALUES (@ID,@001,@002)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaItemID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclusao em Lote de Formas de Pagamento
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="cotaItemID"></param>
        public void ExcluirPorCotaItemID(BD bd, int cotaItemID)
        {
            try
            {
                string sqlSTR = "DELETE FROM tCotaItemFormaPagamento WHERE CotaItemID = " + cotaItemID;
                bd.Executar(sqlSTR);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExcluirPorIDs(BD bd, int fpID, int cotaItemID)
        {
            try
            {
                string strSQL = "DELETE FROM tCotaItemFormaPagamento WHERE FormaPagamentoID = " + fpID + " AND CotaItemID = " + cotaItemID;
                bd.Executar(strSQL);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public class CotaItemFormaPagamentoLista : CotaItemFormaPagamentoLista_B
    {

        public CotaItemFormaPagamentoLista() { }

    }

}
