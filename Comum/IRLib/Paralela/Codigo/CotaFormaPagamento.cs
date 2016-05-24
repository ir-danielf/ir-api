/**************************************************
* Arquivo: CotaFormaPagamento.cs
* Gerado: 04/12/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class CotaFormaPagamento : CotaFormaPagamento_B
    {

        public CotaFormaPagamento() { }

        public CotaFormaPagamento(int usuarioIDLogado) : base(usuarioIDLogado) { }
        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cCotaFormaPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCotaFormaPagamento (ID, Versao, CotaID, FormaPagamentoID) ");
                sql.Append("SELECT ID, @V, CotaID, FormaPagamentoID FROM tCotaFormaPagamento WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) CotaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCotaFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaFormaPagamento(ID, CotaID, FormaPagamentoID) ");
                sql.Append("VALUES (@ID,@001,@002)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle(bd, "I");


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza CotaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public bool Atualizar(BD bd)
        {

            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cCotaFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCotaFormaPagamento SET CotaID = @001, FormaPagamentoID = @002 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Exclui CotaFormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(BD bd, int cotaID, int formaPagamentoID)
        {
            try
            {

                string sql = "SELECT ID FROM tCotaFormaPagamento (NOLOCK) where CotaID = " + cotaID + " AND FormaPagamentoID = " + formaPagamentoID;
                int id = Convert.ToInt32(bd.ConsultaValor(sql));
                if (id == 0)
                    return false;

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCotaFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCotaFormaPagamento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui a forma de pagamento apartir da cotaID
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="cotaID"></param>
        /// <returns></returns>
        public bool Excluir(BD bd, int cotaID)
        {
            try
            {
                bool result = false;
                int x = 0;
                string sql = "SELECT ID FROM tCotaFormaPagamento (NOLOCK) where CotaID = " + cotaID;
                bd.Consulta(sql);

                List<int> ids = new List<int>();

                while (bd.Consulta().Read())
                    ids.Add(bd.LerInt("ID"));

                bd.Consulta().Close();
                BD bd2 = new BD();
                for (int i = 0; i < ids.Count; i++)
                {
                    //bd2 = new BD();
                    if (ids[i] != 0)
                    {
                        //this.Control.ID = ids[i];

                        string sqlSelect = "SELECT MAX(Versao) FROM cCotaFormaPagamento WHERE ID=" + ids[i];
                        object obj = bd.ConsultaValor(sqlSelect);
                        int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                        this.Control.Versao = versao;
                        this.Control.ID = ids[i];
                        InserirControle(bd, "D");
                        InserirLog(bd);

                        string sqlDelete = "DELETE FROM tCotaFormaPagamento WHERE ID=" + ids[i];

                        x = bd.Executar(sqlDelete);

                        result = (x == 1);
                    }
                    bd2.Fechar();
                }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EncontrarFormaPagamento(int formaPagamentoID, int cotaID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM tCotaFormaPagamento (NOLOCK) ");
                stbSQL.Append("WHERE CotaID = ");
                stbSQL.Append(cotaID);
                stbSQL.Append(" AND FormaPagamentoID = ");
                stbSQL.Append(formaPagamentoID);


                bool ok = (Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString())) != 0);
                return ok;
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

        public DataTable getFormaPagamentoCotaItem(int cotaItemID, int canalID)
        {
            try
            {
                DataTable dtt = new DataTable("CotaItemFormaPagamento");
                dtt.Columns.Add("FormaPagamentoID", typeof(int));
                dtt.Columns.Add("CotaItemID", typeof(int));

                bd.Consulta(@"SELECT 
                        cifp.FormaPagamentoID, cifp.CotaItemID 
                            FROM tCotaItemFormaPagamento cifp (NOLOCK) 
                            INNER JOIN tCanalFormaPagamento cfp (NOLOCK) ON cfp.FormaPagamentoID = cifp.FormaPagamentoID
                        WHERE CotaItemID = " + cotaItemID + " AND cfp.CanalID = " + canalID + " ORDER BY FormaPagamentoID ");

                while (bd.Consulta().Read())
                {
                    DataRow dr = dtt.NewRow();
                    dr["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    dr["CotaItemID"] = bd.LerInt("CotaItemID");
                    dtt.Rows.Add(dr);

                }
                return dtt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable getFormaPagamentoCotaItem(int[] cotaItemID)
        {
            try
            {
                DataTable dtt = new DataTable("CotaItemFormaPagamento");
                dtt.Columns.Add("FormaPagamentoID", typeof(int));
                dtt.Columns.Add("CotaItemID", typeof(int));

                bd.Consulta("SELECT FormaPagamentoID, CotaItemID FROM tCotaItemFormaPagamento (NOLOCK) WHERE CotaItemID IN ( " + Utilitario.ArrayToString(cotaItemID) + ") ORDER BY FormaPagamentoID");

                while (bd.Consulta().Read())
                {
                    DataRow dr = dtt.NewRow();
                    dr["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    dr["CotaItemID"] = bd.LerInt("CotaItemID");
                    dtt.Rows.Add(dr);

                }
                return dtt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class CotaFormaPagamentoLista : CotaFormaPagamentoLista_B
    {

        public CotaFormaPagamentoLista() { }

        public CotaFormaPagamentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
