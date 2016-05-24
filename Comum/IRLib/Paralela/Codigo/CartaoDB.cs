/******************************************************
* Arquivo CartaoDB.cs
* Gerado em: 02/08/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Cartao_B"

    public abstract class Cartao_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public nrocartao NroCartao = new nrocartao();
        public checksumcartao CheckSumCartao = new checksumcartao();
        public bandeiraid BandeiraID = new bandeiraid();
        public status Status = new status();
        public ativo Ativo = new ativo();
        public cartaocr CartaoCr = new cartaocr();
        public cvvcr CVVCr = new cvvcr();
        public datacr DataCr = new datacr();
        public nomecartao NomeCartao = new nomecartao();

        public Cartao_B() { }

        // passar o Usuario logado no sistema
        public Cartao_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Cartao
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCartao WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.NroCartao.ValorBD = bd.LerString("NroCartao");
                    this.CheckSumCartao.ValorBD = bd.LerString("CheckSumCartao");
                    this.BandeiraID.ValorBD = bd.LerInt("BandeiraID").ToString();
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.CartaoCr.ValorBD = bd.LerString("CartaoCr");
                    this.CVVCr.ValorBD = bd.LerString("CVVCr");
                    this.DataCr.ValorBD = bd.LerString("DataCr");
                    this.NomeCartao.ValorBD = bd.LerString("NomeCartao");
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

        /// <summary>
        /// Preenche todos os atributos de Cartao do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCartao WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.NroCartao.ValorBD = bd.LerString("NroCartao");
                    this.CheckSumCartao.ValorBD = bd.LerString("CheckSumCartao");
                    this.BandeiraID.ValorBD = bd.LerInt("BandeiraID").ToString();
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.CartaoCr.ValorBD = bd.LerString("CartaoCr");
                    this.CVVCr.ValorBD = bd.LerString("CVVCr");
                    this.DataCr.ValorBD = bd.LerString("DataCr");
                    this.NomeCartao.ValorBD = bd.LerString("NomeCartao");
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
                sql.Append("INSERT INTO cCartao (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCartao (ID, Versao, ClienteID, NroCartao, CheckSumCartao, BandeiraID, Status, Ativo, CartaoCr, CVVCr, DataCr, NomeCartao) ");
                sql.Append("SELECT ID, @V, ClienteID, NroCartao, CheckSumCartao, BandeiraID, Status, Ativo, CartaoCr, CVVCr, DataCr, NomeCartao FROM tCartao WHERE ID = @I");
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
        /// Inserir novo(a) Cartao
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {
                StringBuilder sql = new StringBuilder();

                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("InserirCartao");

                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@ClienteID", SqlDbType.Int) { Value = this.ClienteID.ValorBD };
                parameters[1] = new SqlParameter("@NroCartao", SqlDbType.NVarChar) { Value = this.NroCartao.ValorBD };
                parameters[2] = new SqlParameter("@CheckSumCartao", SqlDbType.NVarChar) { Value = this.CheckSumCartao.ValorBD };
                parameters[3] = new SqlParameter("@BandeiraID", SqlDbType.Int) { Value = this.BandeiraID.ValorBD };
                parameters[4] = new SqlParameter("@Status", SqlDbType.Char, 2) { Value = this.Status.ValorBD };
                parameters[5] = new SqlParameter("@Ativo", SqlDbType.Char, 1) { Value = this.Ativo.ValorBD };
                parameters[6] = new SqlParameter("@CartaoCripto", SqlDbType.NVarChar) { Value = this.CartaoCr.ValorBD };
                parameters[7] = new SqlParameter("@CVVCripto", SqlDbType.NVarChar) { Value = this.CVVCr.ValorBD };
                parameters[8] = new SqlParameter("@DataCripto", SqlDbType.NVarChar) { Value = this.DataCr.ValorBD };
                parameters[9] = new SqlParameter("@NomeCartao", SqlDbType.NVarChar) { Value = this.NomeCartao.ValorBD };

                var id = bd.ExecutarStoredProcedureComRetorno(sql.ToString(), parameters, "@CartaoID");

                this.Control.ID = id;

                return id > 0;

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
        /// Inserir novo(a) Cartao
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();

                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("InserirCartao");

                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@ClienteID", SqlDbType.Int) { Value = this.ClienteID.ValorBD };
                parameters[1] = new SqlParameter("@NroCartao", SqlDbType.NVarChar) { Value = this.NroCartao.ValorBD };
                parameters[2] = new SqlParameter("@CheckSumCartao", SqlDbType.NVarChar) { Value = this.CheckSumCartao.ValorBD };
                parameters[3] = new SqlParameter("@BandeiraID", SqlDbType.Int) { Value = this.BandeiraID.ValorBD };
                parameters[4] = new SqlParameter("@Status", SqlDbType.Char, 2) { Value = this.Status.ValorBD };
                parameters[5] = new SqlParameter("@Ativo", SqlDbType.Char, 1) { Value = this.Ativo.ValorBD };
                parameters[6] = new SqlParameter("@CartaoCripto", SqlDbType.NVarChar) { Value = this.CartaoCr.ValorBD };
                parameters[7] = new SqlParameter("@CVVCripto", SqlDbType.NVarChar) { Value = this.CVVCr.ValorBD };
                parameters[8] = new SqlParameter("@DataCripto", SqlDbType.NVarChar) { Value = this.DataCr.ValorBD };
                parameters[9] = new SqlParameter("@NomeCartao", SqlDbType.NVarChar) { Value = this.NomeCartao.ValorBD };

                var id = bd.ExecutarStoredProcedureComRetorno(sql.ToString(), parameters, "@CartaoID");

                this.Control.ID = id;

                return id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza Cartao
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCartao SET ClienteID = @001, NroCartao = '@002', CheckSumCartao = '@003', BandeiraID = @004, Status = '@005', Ativo = '@006', CartaoCr = '@007', CVVCr = '@008', DataCr = '@009', NomeCartao = '@010' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.NroCartao.ValorBD);
                sql.Replace("@003", this.CheckSumCartao.ValorBD);
                sql.Replace("@004", this.BandeiraID.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.Ativo.ValorBD);
                sql.Replace("@007", this.CartaoCr.ValorBD);
                sql.Replace("@008", this.CVVCr.ValorBD);
                sql.Replace("@009", this.DataCr.ValorBD);
                sql.Replace("@010", this.NomeCartao.ValorBD);

                int x = bd.Executar(sql.ToString());

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
        /// Atualiza Cartao
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCartao SET ClienteID = @001, NroCartao = '@002', CheckSumCartao = '@003', BandeiraID = @004, Status = '@005', Ativo = '@006', CartaoCr = '@007', CVVCr = '@008', DataCr = '@009', NomeCartao = '@010' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.NroCartao.ValorBD);
                sql.Replace("@003", this.CheckSumCartao.ValorBD);
                sql.Replace("@004", this.BandeiraID.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.Ativo.ValorBD);
                sql.Replace("@007", this.CartaoCr.ValorBD);
                sql.Replace("@008", this.CVVCr.ValorBD);
                sql.Replace("@009", this.DataCr.ValorBD);
                sql.Replace("@010", this.NomeCartao.ValorBD);

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
        /// Exclui Cartao com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCartao WHERE ID=" + id;

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
        /// Exclui Cartao com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCartao WHERE ID=" + id;

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
        /// Exclui Cartao
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

            this.ClienteID.Limpar();
            this.NroCartao.Limpar();
            this.CheckSumCartao.Limpar();
            this.BandeiraID.Limpar();
            this.Status.Limpar();
            this.Ativo.Limpar();
            this.CartaoCr.Limpar();
            this.CVVCr.Limpar();
            this.DataCr.Limpar();
            this.NomeCartao.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.ClienteID.Desfazer();
            this.NroCartao.Desfazer();
            this.CheckSumCartao.Desfazer();
            this.BandeiraID.Desfazer();
            this.Status.Desfazer();
            this.Ativo.Desfazer();
            this.CartaoCr.Desfazer();
            this.CVVCr.Desfazer();
            this.DataCr.Desfazer();
            this.NomeCartao.Desfazer();
        }

        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
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

        public class nrocartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NroCartao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
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

        public class checksumcartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CheckSumCartao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 300;
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

        public class bandeiraid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BandeiraID";
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

        public class ativo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ativo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
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

        public class cartaocr : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CartaoCr";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 500;
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

        public class cvvcr : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CVVCr";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 500;
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

        public class datacr : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataCr";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 500;
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

        public class nomecartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeCartao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("Cartao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("NroCartao", typeof(string));
                tabela.Columns.Add("CheckSumCartao", typeof(string));
                tabela.Columns.Add("BandeiraID", typeof(int));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("CartaoCr", typeof(string));
                tabela.Columns.Add("CVVCr", typeof(string));
                tabela.Columns.Add("DataCr", typeof(string));
                tabela.Columns.Add("NomeCartao", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "CartaoLista_B"

    public abstract class CartaoLista_B : BaseLista
    {

        private bool backup = false;
        protected Cartao cartao;

        // passar o Usuario logado no sistema
        public CartaoLista_B()
        {
            cartao = new Cartao();
        }

        // passar o Usuario logado no sistema
        public CartaoLista_B(int usuarioIDLogado)
        {
            cartao = new Cartao(usuarioIDLogado);
        }

        public Cartao Cartao
        {
            get { return cartao; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Cartao especifico
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
                    cartao.Ler(id);
                    return cartao;
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
                    sql = "SELECT ID FROM tCartao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCartao";

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
                    sql = "SELECT ID FROM tCartao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCartao";

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
                    sql = "SELECT ID FROM xCartao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCartao";

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
        /// Preenche Cartao corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    cartao.Ler(id);
                else
                    cartao.LerBackup(id);

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

                bool ok = cartao.Excluir();
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
        /// Inseri novo(a) Cartao na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = cartao.Inserir();
                if (ok)
                {
                    lista.Add(cartao.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Cartao carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Cartao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("NroCartao", typeof(string));
                tabela.Columns.Add("CheckSumCartao", typeof(string));
                tabela.Columns.Add("BandeiraID", typeof(int));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("CartaoCr", typeof(string));
                tabela.Columns.Add("CVVCr", typeof(string));
                tabela.Columns.Add("DataCr", typeof(string));
                tabela.Columns.Add("NomeCartao", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = cartao.Control.ID;
                        linha["ClienteID"] = cartao.ClienteID.Valor;
                        linha["NroCartao"] = cartao.NroCartao.Valor;
                        linha["CheckSumCartao"] = cartao.CheckSumCartao.Valor;
                        linha["BandeiraID"] = cartao.BandeiraID.Valor;
                        linha["Status"] = cartao.Status.Valor;
                        linha["Ativo"] = cartao.Ativo.Valor;
                        linha["CartaoCr"] = cartao.CartaoCr.Valor;
                        linha["CVVCr"] = cartao.CVVCr.Valor;
                        linha["DataCr"] = cartao.DataCr.Valor;
                        linha["NomeCartao"] = cartao.NomeCartao.Valor;
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

                DataTable tabela = new DataTable("RelatorioCartao");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("NroCartao", typeof(string));
                    tabela.Columns.Add("CheckSumCartao", typeof(string));
                    tabela.Columns.Add("BandeiraID", typeof(int));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("Ativo", typeof(bool));
                    tabela.Columns.Add("CartaoCr", typeof(string));
                    tabela.Columns.Add("CVVCr", typeof(string));
                    tabela.Columns.Add("DataCr", typeof(string));
                    tabela.Columns.Add("NomeCartao", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = cartao.ClienteID.Valor;
                        linha["NroCartao"] = cartao.NroCartao.Valor;
                        linha["CheckSumCartao"] = cartao.CheckSumCartao.Valor;
                        linha["BandeiraID"] = cartao.BandeiraID.Valor;
                        linha["Status"] = cartao.Status.Valor;
                        linha["Ativo"] = cartao.Ativo.Valor;
                        linha["CartaoCr"] = cartao.CartaoCr.Valor;
                        linha["CVVCr"] = cartao.CVVCr.Valor;
                        linha["DataCr"] = cartao.DataCr.Valor;
                        linha["NomeCartao"] = cartao.NomeCartao.Valor;
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
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tCartao WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "NroCartao":
                        sql = "SELECT ID, NroCartao FROM tCartao WHERE " + FiltroSQL + " ORDER BY NroCartao";
                        break;
                    case "CheckSumCartao":
                        sql = "SELECT ID, CheckSumCartao FROM tCartao WHERE " + FiltroSQL + " ORDER BY CheckSumCartao";
                        break;
                    case "BandeiraID":
                        sql = "SELECT ID, BandeiraID FROM tCartao WHERE " + FiltroSQL + " ORDER BY BandeiraID";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tCartao WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tCartao WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;
                    case "CartaoCr":
                        sql = "SELECT ID, CartaoCr FROM tCartao WHERE " + FiltroSQL + " ORDER BY CartaoCr";
                        break;
                    case "CVVCr":
                        sql = "SELECT ID, CVVCr FROM tCartao WHERE " + FiltroSQL + " ORDER BY CVVCr";
                        break;
                    case "DataCr":
                        sql = "SELECT ID, DataCr FROM tCartao WHERE " + FiltroSQL + " ORDER BY DataCr";
                        break;
                    case "NomeCartao":
                        sql = "SELECT ID, NomeCartao FROM tCartao WHERE " + FiltroSQL + " ORDER BY NomeCartao";
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

    #region "CartaoException"

    [Serializable]
    public class CartaoException : Exception
    {

        public CartaoException() : base() { }

        public CartaoException(string msg) : base(msg) { }

        public CartaoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}