using CTLib;
using IRLib.ClientObjects.Lote;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    #region "Lote_B"
    public abstract class Lote_B : BaseBD
    {
        //public EstruturaLote EstruturaLote = new EstruturaLote();

        public nome Nome = new nome();
        public status Status = new status();
        public quantidade Quantidade = new quantidade();
        public dataLimite DataLimite = new dataLimite();
        public loteAnterior LoteAnterior = new loteAnterior();
        public apresentacaoSetorID ApresentacaoSetorID = new apresentacaoSetorID();

        public Lote_B() { }

        // passar o Usuario logado no sistema
        public Lote_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM tLote WHERE ID = @ID";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@ID", Value = id, DbType = System.Data.DbType.Int32 });

                bd.Consulta(sql, parametros);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.DataLimite.ValorBD = bd.LerString("DataLimite");
                    this.LoteAnterior.ValorBD = bd.LerInt("LoteAnterior").ToString();
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LerBackup(int id)
        {
            try
            {
                string sql = "SELECT * FROM xLote WHERE ID = @ID";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@ID", Value = id, DbType = System.Data.DbType.Int32 });

                bd.Consulta(sql, parametros);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.DataLimite.ValorBD = bd.LerString("DataLimite");
                    this.LoteAnterior.ValorBD = bd.LerInt("LoteAnterior").ToString();
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(string acao)
        {
            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cLote (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog()
        {
            try
            {
                bd.IniciarTransacao();

                string sql = @"INSERT INTO xLote(ID, Nome, Status, Quantidade, DataLimite, LoteAnterior, Versao, ApresentacaoSetorID)
                                    VALUES(@ID, @Nome, @Status, @Quantidade, @DataLimite, @LoteAnterior, @Versao, @ApresentacaoSetorID)";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@ID", Value = this.Control.ID, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                if(String.IsNullOrEmpty(DataLimite.Valor))
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ToString()), DbType = System.Data.DbType.DateTime });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@Versao", Value = this.Control.Versao, DbType = System.Data.DbType.Int32 });
                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                bd.Executar(sql.ToString(), parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.FinalizarTransacao();
            }
        }

        public override bool Inserir()
        {
            try
            {
                bd.IniciarTransacao();

                StringBuilder sqlctrl = new StringBuilder();
                sqlctrl.Append("SELECT MAX(ID) AS ID FROM cLote");
                object obj = bd.ConsultaValor(sqlctrl);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                string sql = @"INSERT INTO tLote(Nome, Status, Quantidade, DataLimite, LoteAnterior, ApresentacaoSetorID)
                                    VALUES(@Nome, @Status, @Quantidade, @DataLimite, @LoteAnterior, @ApresentacaoSetorID) SELECT SCOPE_IDENTITY()";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                if (DataLimite.Valor == null)
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ValorBD.ToString()), DbType = System.Data.DbType.DateTime });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                id = bd.ExecutarScalar(sql, parametros);

                if (id > 0)
                    InserirControle("I");

                return id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
                bd.FinalizarTransacao();
                bd.Fechar();
            }
        }

        public void Ler(EstruturaLote lote)
        {
            this.Control.ID = lote.ID;
            this.Nome.Valor = lote.Nome;
            this.Quantidade.Valor = lote.Quantidade;
            this.Status.Valor = lote.Status;
            this.LoteAnterior.Valor = lote.LoteAnterior.Value;
            if (lote.DataLimite != null)
            {
                this.DataLimite.Valor = lote.DataLimite.Value.ToString();
            }
            this.ApresentacaoSetorID.Valor = lote.ApresentacaoSetorID;
        }

        public bool Inserir(EstruturaLote lote)
        {
            try
            {
                bd.IniciarTransacao();
                this.Ler(lote);
                StringBuilder sqlctrl = new StringBuilder();
                sqlctrl.Append("SELECT MAX(ID) AS ID FROM cLote");
                object obj = bd.ConsultaValor(sqlctrl);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                string sql = @"INSERT INTO tLote(Nome, Status, Quantidade, DataLimite, LoteAnterior, ApresentacaoSetorID)
                                    VALUES(@Nome, @Status, @Quantidade, @DataLimite, @LoteAnterior, @ApresentacaoSetorID) SELECT SCOPE_IDENTITY()";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                if (String.IsNullOrEmpty(DataLimite.Valor))
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ValorBD.ToString()), DbType = System.Data.DbType.DateTime });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                id = bd.ExecutarScalar(sql, parametros);

                if (id > 0)
                    InserirControle("I");

                return id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.FinalizarTransacao();
                bd.Fechar();
            }
        }

        public override bool Inserir(BD bd)
        {
            try
            {
                bd.IniciarTransacao();

                StringBuilder sqlctrl = new StringBuilder();
                sqlctrl.Append("SELECT MAX(ID) AS ID FROM cLote");
                object obj = bd.ConsultaValor(sqlctrl);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                string sql = @"INSERT INTO tLote(Nome, Status, Quantidade, DataLimite, LoteAnterior, ApresentacaoSetorID)
                                    VALUES(@Nome, @Status, @Quantidade, @DataLimite, @LoteAnterior, @ApresentacaoSetorID) SELECT SCOPE_IDENTITY()";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                if (DataLimite.Valor == null)
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ValorBD.ToString()), DbType = System.Data.DbType.DateTime });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                id = bd.ExecutarScalar(sql, parametros);

                if (id > 0)
                    InserirControle("I");

                return id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Atualizar()
        {
            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cLote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirLog();
                InserirControle("U");

                bd.IniciarTransacao();

                string sql = @"UPDATE tLote SET Nome = @Nome, Status = @Status, Quantidade = @Quantidade,
                                DataLimite = @DataLimite, LoteAnterior = @LoteAnterior, ApresentacaoSetorID = @ApresentacaoSetorID WHERE ID = @ID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@ID", Value = this.Control.ID.ToString(), DbType = System.Data.DbType.Int32 });
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                if (!String.IsNullOrEmpty(DataLimite.Valor))
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ToString()), DbType = System.Data.DbType.DateTime });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DBNull.Value });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                return bd.Executar(sql.ToString(), parametros) > 0;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.FinalizarTransacao();
                bd.Fechar();
            }
        }

        public bool Atualizar(EstruturaLote lote)
        {
            try
            {
                this.Ler(lote);
                string sqlVersion = "SELECT MAX(Versao) FROM cLote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirLog();
                InserirControle("U");

                bd.IniciarTransacao();

                string sql = @"UPDATE tLote SET Nome = @Nome, Status = @Status, Quantidade = @Quantidade,
                                DataLimite = @DataLimite, LoteAnterior = @LoteAnterior, ApresentacaoSetorID = @ApresentacaoSetorID WHERE ID = @ID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@ID", Value = this.Control.ID.ToString(), DbType = System.Data.DbType.Int32 });
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                if (!String.IsNullOrEmpty(DataLimite.Valor))
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ToString()), DbType = System.Data.DbType.DateTime });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DBNull.Value });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                return bd.Executar(sql.ToString(), parametros) > 0;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.FinalizarTransacao();
                bd.Fechar();
            }
        }

        public override bool Atualizar(BD bd)
        {
            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cLote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                bd.IniciarTransacao();

                string sql = @"UPDATE tLote SET Nome = @Nome, Status = @Status, Quantidade = @Quantidade,
                                DataLimite = @DataLimite, LoteAnterior = @LoteAnterior, ApresentacaoSetorID = @ApresentacaoSetorID WHERE ID = @ID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@ID", Value = this.Control.ID.ToString(), DbType = System.Data.DbType.Int32 });
                parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = Nome.ValorBD, DbType = System.Data.DbType.String });
                parametros.Add(new SqlParameter() { ParameterName = "@Status", Value = Status.ValorBD, DbType = System.Data.DbType.String });

                if (Quantidade.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@Quantidade", Value = Quantidade.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@DataLimite", Value = DateTime.Parse(DataLimite.ToString()), DbType = System.Data.DbType.DateTime });

                if (LoteAnterior.Valor == 0)
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = DBNull.Value });
                else
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteAnterior", Value = LoteAnterior.ValorBD, DbType = System.Data.DbType.Int32 });

                parametros.Add(new SqlParameter() { ParameterName = "@ApresentacaoSetorID", Value = ApresentacaoSetorID.ValorBD, DbType = System.Data.DbType.Int32 });

                return bd.Executar(sql.ToString(), parametros) > 0;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
        }

        /// <summary>
        /// Exclui Loja com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLote WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui Loja com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLote WHERE ID=" + id;

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
        /// Exclui Lote
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {
            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Limpar()
        {
            this.Nome.Limpar();
            this.Status.Limpar();
            this.Quantidade.Limpar();
            this.DataLimite.Limpar();
            this.LoteAnterior.Limpar();
            this.ApresentacaoSetorID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.Status.Desfazer();
            this.Quantidade.Desfazer();
            this.DataLimite.Desfazer();
            this.LoteAnterior.Desfazer();
            this.ApresentacaoSetorID.Desfazer();
        }

        public class nome : TextProperty
        {
            public override string Nome
            {
                get
                {
                    return "Nome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 250;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }
        }

        public class status : TextProperty
        {
            public override string Nome
            {
                get
                {
                    return "Status";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }
        }

        public class quantidade : IntegerProperty
        {
            public override string Nome
            {
                get
                {
                    return "Quantidade";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }
            public override string ToString()
            {
                return base.Valor.ToString();
            }

            public override string ValorBD
            {
                get
                {
                    return base.ValorBD;
                }
                set
                {
                    if (value == null)
                    {
                        base.ValorBD = value;
                    }
                    else if (value.Trim().Length == 0)
                    {
                        base.ValorBD = null;
                    }
                    else
                    {
                        base.ValorBD = value;
                    }
                }
            }
        }

        public class dataLimite : TextProperty
        {
            public override string Nome
            {
                get
                {
                    return "DataLimite";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }
        }

        public class loteAnterior : IntegerProperty
        {
            public override string Nome
            {
                get
                {
                    return "LoteAnterior";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

            public override string ValorBD
            {
                get
                {
                    return base.ValorBD;
                }
                set
                {
                    if (value == null)
                    {
                        base.ValorBD = value;
                    }
                    else if (value.Trim().Length == 0)
                    {
                        base.ValorBD = null;
                    }
                    else
                    {
                        base.ValorBD = value;
                    }
                }
            }
        }

        public class apresentacaoSetorID : IntegerProperty
        {
            public override string Nome
            {
                get
                {
                    return "ApresentacaoSetorID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }
            public override string ToString()
            {
                return base.Valor.ToString();
            }
        }
    }

    #endregion

    #region "LoteLista_B"

    public abstract class LoteLista_B : BaseLista
    {

        private bool backup = false;
        protected Lote lote;

        // passar o Usuario logado no sistema
        public LoteLista_B()
        {
            lote = new Lote();
        }

        // passar o Usuario logado no sistema
        public LoteLista_B(int usuarioIDLogado)
        {
            lote = new Lote(usuarioIDLogado);
        }

        public Lote Lote
        {
            get { return lote; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Loja especifico
        /// </summary>
        public override IBaseBD this[int indice]
        {
            get
            {
                if (indice < 0 || indice >= lista.Count)
                {
                    return null;
                }
                else
                {
                    int id = (int)lista[indice];
                    lote.Ler(id);
                    return lote;
                }
            }
        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tLote";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLote";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tLoja";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLoja";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xLoja";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xLoja";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche Loja corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    lote.Ler(id);
                else
                    lote.LerBackup(id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui o item corrente da lista
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = lote.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {

            try
            {
                if (lista.Count == 0)
                    Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = false;

                if (lista.Count > 0)
                { //verifica se tem itens

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inseri novo(a) Loja na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = lote.Inserir();
                if (ok)
                {
                    lista.Add(lote.Control.ID);
                    Indice = lista.Count - 1;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Obtem uma tabela de todos os campos de Loja carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {
            try
            {
                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("DataLimite", typeof(DateTime));
                tabela.Columns.Add("LoteAnterior", typeof(int?));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));

                if (this.Primeiro())
                {
                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = lote.Control.ID;
                        linha["Nome"] = lote.Nome.Valor;
                        linha["Status"] = lote.Status.Valor;
                        linha["Quantidade"] = lote.Quantidade.Valor;
                        linha["DataLimite"] = lote.DataLimite.Valor;
                        linha["LoteAnterior"] = lote.LoteAnterior.Valor;
                        linha["ApresentacaoSetorID"] = lote.ApresentacaoSetorID.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {
                DataTable tabela = new DataTable("RelatorioLoja");

                if (this.Primeiro())
                {
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("DataLimite", typeof(DateTime));
                    tabela.Columns.Add("LoteAnterior", typeof(int?));
                    tabela.Columns.Add("ApresentacaoSetorID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = lote.Nome.Valor;
                        linha["Status"] = lote.Status.Valor;
                        linha["Quantidade"] = lote.Quantidade.Valor;
                        linha["DataLimite"] = lote.DataLimite.Valor;
                        linha["LoteAnterior"] = lote.LoteAnterior.Valor;
                        linha["ApresentacaoSetorID"] = lote.ApresentacaoSetorID.Valor;
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

        /// <summary>
        /// Retorna um IDataReader com ID e o Campo.
        /// </summary>
        /// <param name="campo">Informe o campo. Exemplo: Nome</param>
        /// <returns></returns>
        public override IDataReader ListaPropriedade(string campo)
        {

            try
            {
                string sql;
                switch (campo)
                {
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tLote WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tLote WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tLote WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "DataLimite":
                        sql = "SELECT ID, DataLimite FROM tLote WHERE " + FiltroSQL + " ORDER BY DataLimite";
                        break;
                    case "LoteAnterior":
                        sql = "SELECT ID, CEP FROM LoteAnterior WHERE " + FiltroSQL + " ORDER BY LoteAnterior";
                        break;
                    case "ApresentacaoSetorID":
                        sql = "SELECT ID, ApresentacaoSetorID FROM tLote WHERE " + FiltroSQL + " ORDER BY ApresentacaoSetorID";
                        break;
                    default:
                        sql = null;
                        break;
                }

                IDataReader dataReader = bd.Consulta(sql);

                bd.Fechar();

                return dataReader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve um array dos IDs que compoem a lista
        /// </summary>
        /// <returns></returns>		
        public override int[] ToArray()
        {

            try
            {

                int[] a = (int[])lista.ToArray(typeof(int));

                return a;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve uma string dos IDs que compoem a lista concatenada por virgula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                StringBuilder idsBuffer = new StringBuilder();

                int n = lista.Count;
                for (int i = 0; i < n; i++)
                {
                    int id = (int)lista[i];
                    idsBuffer.Append(id + ",");
                }

                string ids = "";

                if (idsBuffer.Length > 0)
                {
                    ids = idsBuffer.ToString();
                    ids = ids.Substring(0, ids.Length - 1);
                }

                return ids;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #endregion

    #region "LoteException"

    [Serializable]
    public class LoteException : Exception
    {
        public LoteException() : base() { }

        public LoteException(string msg) : base(msg) { }

        public LoteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    #endregion
}
