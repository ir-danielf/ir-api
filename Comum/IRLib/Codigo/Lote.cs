using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public class Lote : Lote_B
    {
        public Lote() { }

        public Lote(int usuarioIDLogado) : base(usuarioIDLogado) {
            this.Control.UsuarioID = base.Control.UsuarioID;
        }

        public DataTable listaLotePorSetor(int idSetor)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                string sql = @"SELECT tLote.ID, tLote.Nome, tLote.Status, tLote.Quantidade, tLote.DataLimite, tLote.LoteAnterior
                                    FROM tLote (NOLOCK)
                                    LEFT JOIN tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.ID = tLote.ApresentacaoSetorID
                                WHERE tApresentacaoSetor.SetorID = @idSetor ";

                parametros.Add(new SqlParameter() { ParameterName = "@idSetor", Value = idSetor, DbType = System.Data.DbType.Int32 });

                DataTable result = bd.QueryToTable(sql, parametros);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable listaLotePorSetor(int idSetor, string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Lote");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                List<SqlParameter> parametros = new List<SqlParameter>();

                string sql = @"SELECT tLote.ID, tLote.Nome
                                    FROM tLote (NOLOCK)
                                    LEFT JOIN tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.ID = tLote.ApresentacaoSetorID
                                WHERE tApresentacaoSetor.SetorID = @idSetor ";

                parametros.Add(new SqlParameter() { ParameterName = "@idSetor", Value = idSetor, DbType = System.Data.DbType.Int32 });

                bd.Consulta(sql, parametros);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable listaLotePorApresentacaoSetorInclusao(int apresentacaoSetorID, string registroZero, bool desvinculados = false)
        {
            try
            {
                DataTable tabela = new DataTable("Lote");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = String.Format(@"SELECT ID, Nome, LoteAnterior FROM tLote (NOLOCK)
                                             WHERE ApresentacaoSetorID = {0}", apresentacaoSetorID);

                if (desvinculados)
                {
                    sql = sql + String.Format(@" AND ID NOT IN (
                                                    SELECT DISTINCT LoteAnterior FROM tLote
                                                    WHERE ApresentacaoSetorID = {0}
                                                    AND LoteAnterior IS NOT NULL)", apresentacaoSetorID);

                }

                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);

                    if (registroZero != null && bd.LerInt("LoteAnterior") > 0)
                    {
                        DataTable loteAnterior = this.getLoteAssociado(bd.LerInt("ID"));

                        if (loteAnterior.Rows[0]["LoteAnterior"] == DBNull.Value)
                        {
                            tabela.Rows.Remove(tabela.Rows[0]);
                        }
                    }
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable listaLotePorApresentacaoSetor(int apresentacaoSetorID, string registroZero, Lote lote = null, bool desvinculados = false)
        {
            try
            {

                DataTable tabela = new DataTable("Lote");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Status", typeof(string));

                using (CTLib.BD bd = new CTLib.BD())
                {
                    string sql = String.Format(@"SELECT ID, Nome, Status FROM tLote l1 (NOLOCK)
                                                 WHERE l1.ApresentacaoSetorID = {0} AND STATUS <> 'E'", apresentacaoSetorID);

                    if (lote != null && lote.Control.ID > 0)
                    {
                        sql += String.Format(" OR l1.ID = {0}", lote.Control.ID);
                    }

                    if (desvinculados)
                    {
                        if (lote != null && lote.Control.ID > 0)
                        {
                            sql += String.Format(" AND l1.ID NOT IN (SELECT l2.LoteAnterior FROM tLote l2 WHERE l2.LoteAnterior IS NOT NULL AND l1.LoteAnterior = {0})", lote.Control.ID);
                        }
                        else
                        {
                            sql += " AND l1.ID NOT IN (SELECT l2.LoteAnterior FROM tLote l2 WHERE l2.LoteAnterior IS NOT NULL)";
                        }

                    }

                    bd.Consulta(sql);

                    if (registroZero != null)
                        tabela.Rows.Add(new Object[] { 0, registroZero });

                    while (bd.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Status"] = bd.LerString("Status");
                        tabela.Rows.Add(linha);
                    }
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable listaLotePorApresentacaoSetor(int apresentacaoSetorID)
        {
            try
            {
                string sql = String.Format(@"
                                            WITH Recursividade(NivelRecursividade, ID, Nome, Status, Quantidade, DataLimite, ApresentacaoSetorID, LoteAnterior)
                                            AS
                                            (
                                            SELECT
	                                            1
	                                            ,L.ID
	                                            ,L.Nome
	                                            ,L.Status
	                                            ,L.Quantidade
	                                            ,L.DataLimite
	                                            ,L.ApresentacaoSetorID
	                                            ,L.LoteAnterior
                                            FROM 
	                                            tLote (NOLOCK) AS L
                                            WHERE 
	                                            L.ApresentacaoSetorID = {0}
	                                            AND L.LoteAnterior IS NULL
                                            UNION ALL
                                            SELECT
	                                            R.NivelRecursividade + 1
	                                            ,L.ID
	                                            ,L.Nome
	                                            ,L.Status
	                                            ,L.Quantidade
	                                            ,L.DataLimite
	                                            ,L.ApresentacaoSetorID
	                                            ,L.LoteAnterior
                                            FROM 
	                                            tLote (NOLOCK) AS L
	                                            INNER JOIN Recursividade AS R ON L.LoteAnterior = R.ID
                                            )
                                            SELECT 
	                                            ID
	                                            ,Nome
	                                            ,Status
	                                            ,Quantidade
	                                            ,DataLimite
	                                            ,ApresentacaoSetorID
	                                            ,LoteAnterior
                                            FROM Recursividade ORDER BY NivelRecursividade"
                , apresentacaoSetorID);

                DataTable result = bd.QueryToTable(sql);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getLastLoteID(int ApresentacaoSetorID)
        {
            try
            {
                int dtRetorno = 0;

                string sql = String.Format(@"SELECT TOP 1 ID FROM tLote (NOLOCK) WHERE ApresentacaoSetorID = {0} AND ID NOT IN (SELECT LoteAnterior FROM tLote WHERE ApresentacaoSetorID = {0} AND LoteAnterior IS NOT NULL)", ApresentacaoSetorID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (!String.IsNullOrEmpty(bd.LerString("ID")))
                    {
                        dtRetorno = bd.LerInt("ID");
                    }
                    else
                    {
                        dtRetorno = 0;
                    }
                }

                return dtRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime? getDataLote(int loteID)
        {
            try
            {
                DateTime? dtRetorno = new DateTime();

                string sql = String.Format(@"SELECT DataLimite FROM tLote (NOLOCK) WHERE ID = {0}", loteID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (!String.IsNullOrEmpty(bd.LerString("DataLimite")))
                    {
                        dtRetorno = Convert.ToDateTime(bd.LerString("DataLimite"), CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    else
                    {
                        dtRetorno = null;
                    }
                }

                return dtRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getLoteAssociado(int loteID)
        {
            try
            {
                string sql = String.Format(@"SELECT DISTINCT LA.*
                                             FROM tLote L 
                                             INNER JOIN tLoteAnterior LA ON L.LoteAnterior = LA.ID
                                             WHERE L.ID = {0}", loteID);

                return bd.QueryToTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteAtivo(int ApresentacaoSetorID)
        {
            try
            {
                string sql = String.Format(@"SELECT TOP 1 COUNT(ID) FROM tLote (NOLOCK) WHERE ApresentacaoSetorID = {0} AND Status = 'A'", ApresentacaoSetorID);

                return bd.ExecutarScalar(sql) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ClientObjects.Lote.EstruturaLote Carregar(int loteID)
        {
            this.Ler(loteID);
            var lote = new ClientObjects.Lote.EstruturaLote();
            lote.ID = loteID;
            lote.Nome = this.Nome.Valor;
            lote.Quantidade = this.Quantidade.Valor;
            lote.Status = this.Status.Valor;
            lote.LoteAnterior = this.LoteAnterior.Valor;
            lote.DataLimite = null;
            if (this.DataLimite.Valor.Trim() != "")
            {
                lote.DataLimite = DateTime.Parse(this.DataLimite.Valor);
            }
            lote.ApresentacaoSetorID = this.ApresentacaoSetorID.Valor;
            return lote;
        }
    }
}
