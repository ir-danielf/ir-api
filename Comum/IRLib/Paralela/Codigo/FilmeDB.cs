/******************************************************
* Arquivo FilmeDB.cs
* Gerado em: 21/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Filme_B"

    public abstract class Filme_B : BaseBD
    {

        public filmeid FilmeID = new filmeid();
        public nome Nome = new nome();
        public duracao Duracao = new duracao();
        public datacadastro DataCadastro = new datacadastro();
        public idade Idade = new idade();
        public idadejustificativa IdadeJustificativa = new idadejustificativa();
        public dublado Dublado = new dublado();
        public imdb IMDB = new imdb();
        public sinopse Sinopse = new sinopse();

        public Filme_B() { }

        // passar o Usuario logado no sistema
        public Filme_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Filme
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tFilme WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.FilmeID.ValorBD = bd.LerInt("FilmeID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Duracao.ValorBD = bd.LerInt("Duracao").ToString();
                    this.DataCadastro.ValorBD = bd.LerString("DataCadastro");
                    this.Idade.ValorBD = bd.LerInt("Idade").ToString();
                    this.IdadeJustificativa.ValorBD = bd.LerString("IdadeJustificativa");
                    this.Dublado.ValorBD = bd.LerString("Dublado");
                    this.IMDB.ValorBD = bd.LerString("IMDB");
                    this.Sinopse.ValorBD = bd.LerString("Sinopse");
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
        /// Preenche todos os atributos de Filme do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xFilme WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.FilmeID.ValorBD = bd.LerInt("FilmeID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Duracao.ValorBD = bd.LerInt("Duracao").ToString();
                    this.DataCadastro.ValorBD = bd.LerString("DataCadastro");
                    this.Idade.ValorBD = bd.LerInt("Idade").ToString();
                    this.IdadeJustificativa.ValorBD = bd.LerString("IdadeJustificativa");
                    this.Dublado.ValorBD = bd.LerString("Dublado");
                    this.IMDB.ValorBD = bd.LerString("IMDB");
                    this.Sinopse.ValorBD = bd.LerString("Sinopse");
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
                sql.Append("INSERT INTO cFilme (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xFilme (ID, Versao, FilmeID, Nome, Duracao, DataCadastro, Idade, IdadeJustificativa, Dublado, IMDB, Sinopse) ");
                sql.Append("SELECT ID, @V, FilmeID, Nome, Duracao, DataCadastro, Idade, IdadeJustificativa, Dublado, IMDB, Sinopse FROM tFilme WHERE ID = @I");
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
        /// Inserir novo(a) Filme
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cFilme");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFilme(ID, FilmeID, Nome, Duracao, DataCadastro, Idade, IdadeJustificativa, Dublado, IMDB, Sinopse) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,'@004',@005,'@006','@007','@008','@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.FilmeID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Duracao.ValorBD);
                sql.Replace("@004", this.DataCadastro.ValorBD);
                sql.Replace("@005", this.Idade.ValorBD);
                sql.Replace("@006", this.IdadeJustificativa.ValorBD);
                sql.Replace("@007", this.Dublado.ValorBD);
                sql.Replace("@008", this.IMDB.ValorBD);
                sql.Replace("@009", this.Sinopse.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

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
        /// Inserir novo(a) Filme
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cFilme");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFilme(ID, FilmeID, Nome, Duracao, DataCadastro, Idade, IdadeJustificativa, Dublado, IMDB, Sinopse) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,'@004',@005,'@006','@007','@008','@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.FilmeID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Duracao.ValorBD);
                sql.Replace("@004", this.DataCadastro.ValorBD);
                sql.Replace("@005", this.Idade.ValorBD);
                sql.Replace("@006", this.IdadeJustificativa.ValorBD);
                sql.Replace("@007", this.Dublado.ValorBD);
                sql.Replace("@008", this.IMDB.ValorBD);
                sql.Replace("@009", this.Sinopse.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza Filme
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cFilme WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tFilme SET FilmeID = @001, Nome = '@002', Duracao = @003, DataCadastro = '@004', Idade = @005, IdadeJustificativa = '@006', Dublado = '@007', IMDB = '@008', Sinopse = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.FilmeID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Duracao.ValorBD);
                sql.Replace("@004", this.DataCadastro.ValorBD);
                sql.Replace("@005", this.Idade.ValorBD);
                sql.Replace("@006", this.IdadeJustificativa.ValorBD);
                sql.Replace("@007", this.Dublado.ValorBD);
                sql.Replace("@008", this.IMDB.ValorBD);
                sql.Replace("@009", this.Sinopse.ValorBD);

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
        /// Atualiza Filme
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cFilme WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tFilme SET FilmeID = @001, Nome = '@002', Duracao = @003, DataCadastro = '@004', Idade = @005, IdadeJustificativa = '@006', Dublado = '@007', IMDB = '@008', Sinopse = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.FilmeID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Duracao.ValorBD);
                sql.Replace("@004", this.DataCadastro.ValorBD);
                sql.Replace("@005", this.Idade.ValorBD);
                sql.Replace("@006", this.IdadeJustificativa.ValorBD);
                sql.Replace("@007", this.Dublado.ValorBD);
                sql.Replace("@008", this.IMDB.ValorBD);
                sql.Replace("@009", this.Sinopse.ValorBD);

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
        /// Exclui Filme com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cFilme WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tFilme WHERE ID=" + id;

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
        /// Exclui Filme com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cFilme WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tFilme WHERE ID=" + id;

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
        /// Exclui Filme
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

            this.FilmeID.Limpar();
            this.Nome.Limpar();
            this.Duracao.Limpar();
            this.DataCadastro.Limpar();
            this.Idade.Limpar();
            this.IdadeJustificativa.Limpar();
            this.Dublado.Limpar();
            this.IMDB.Limpar();
            this.Sinopse.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.FilmeID.Desfazer();
            this.Nome.Desfazer();
            this.Duracao.Desfazer();
            this.DataCadastro.Desfazer();
            this.Idade.Desfazer();
            this.IdadeJustificativa.Desfazer();
            this.Dublado.Desfazer();
            this.IMDB.Desfazer();
            this.Sinopse.Desfazer();
        }

        public class filmeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FilmeID";
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

        public class duracao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Duracao";
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

        public class datacadastro : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataCadastro";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class idade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Idade";
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

        public class idadejustificativa : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "IdadeJustificativa";
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

        public class dublado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Dublado";
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

        public class imdb : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "IMDB";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4;
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

        public class sinopse : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sinopse";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 350;
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

                DataTable tabela = new DataTable("Filme");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("FilmeID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Duracao", typeof(int));
                tabela.Columns.Add("DataCadastro", typeof(DateTime));
                tabela.Columns.Add("Idade", typeof(int));
                tabela.Columns.Add("IdadeJustificativa", typeof(string));
                tabela.Columns.Add("Dublado", typeof(bool));
                tabela.Columns.Add("IMDB", typeof(string));
                tabela.Columns.Add("Sinopse", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "FilmeLista_B"

    public abstract class FilmeLista_B : BaseLista
    {

        private bool backup = false;
        protected Filme filme;

        // passar o Usuario logado no sistema
        public FilmeLista_B()
        {
            filme = new Filme();
        }

        // passar o Usuario logado no sistema
        public FilmeLista_B(int usuarioIDLogado)
        {
            filme = new Filme(usuarioIDLogado);
        }

        public Filme Filme
        {
            get { return filme; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Filme especifico
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
                    filme.Ler(id);
                    return filme;
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
                    sql = "SELECT ID FROM tFilme";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tFilme";

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
                    sql = "SELECT ID FROM tFilme";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tFilme";

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
                    sql = "SELECT ID FROM xFilme";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xFilme";

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
        /// Preenche Filme corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    filme.Ler(id);
                else
                    filme.LerBackup(id);

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

                bool ok = filme.Excluir();
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
        /// Inseri novo(a) Filme na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = filme.Inserir();
                if (ok)
                {
                    lista.Add(filme.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Filme carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Filme");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("FilmeID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Duracao", typeof(int));
                tabela.Columns.Add("DataCadastro", typeof(DateTime));
                tabela.Columns.Add("Idade", typeof(int));
                tabela.Columns.Add("IdadeJustificativa", typeof(string));
                tabela.Columns.Add("Dublado", typeof(bool));
                tabela.Columns.Add("IMDB", typeof(string));
                tabela.Columns.Add("Sinopse", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = filme.Control.ID;
                        linha["FilmeID"] = filme.FilmeID.Valor;
                        linha["Nome"] = filme.Nome.Valor;
                        linha["Duracao"] = filme.Duracao.Valor;
                        linha["DataCadastro"] = filme.DataCadastro.Valor;
                        linha["Idade"] = filme.Idade.Valor;
                        linha["IdadeJustificativa"] = filme.IdadeJustificativa.Valor;
                        linha["Dublado"] = filme.Dublado.Valor;
                        linha["IMDB"] = filme.IMDB.Valor;
                        linha["Sinopse"] = filme.Sinopse.Valor;
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

                DataTable tabela = new DataTable("RelatorioFilme");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("FilmeID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Duracao", typeof(int));
                    tabela.Columns.Add("DataCadastro", typeof(DateTime));
                    tabela.Columns.Add("Idade", typeof(int));
                    tabela.Columns.Add("IdadeJustificativa", typeof(string));
                    tabela.Columns.Add("Dublado", typeof(bool));
                    tabela.Columns.Add("IMDB", typeof(string));
                    tabela.Columns.Add("Sinopse", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["FilmeID"] = filme.FilmeID.Valor;
                        linha["Nome"] = filme.Nome.Valor;
                        linha["Duracao"] = filme.Duracao.Valor;
                        linha["DataCadastro"] = filme.DataCadastro.Valor;
                        linha["Idade"] = filme.Idade.Valor;
                        linha["IdadeJustificativa"] = filme.IdadeJustificativa.Valor;
                        linha["Dublado"] = filme.Dublado.Valor;
                        linha["IMDB"] = filme.IMDB.Valor;
                        linha["Sinopse"] = filme.Sinopse.Valor;
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
                    case "FilmeID":
                        sql = "SELECT ID, FilmeID FROM tFilme WHERE " + FiltroSQL + " ORDER BY FilmeID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tFilme WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Duracao":
                        sql = "SELECT ID, Duracao FROM tFilme WHERE " + FiltroSQL + " ORDER BY Duracao";
                        break;
                    case "DataCadastro":
                        sql = "SELECT ID, DataCadastro FROM tFilme WHERE " + FiltroSQL + " ORDER BY DataCadastro";
                        break;
                    case "Idade":
                        sql = "SELECT ID, Idade FROM tFilme WHERE " + FiltroSQL + " ORDER BY Idade";
                        break;
                    case "IdadeJustificativa":
                        sql = "SELECT ID, IdadeJustificativa FROM tFilme WHERE " + FiltroSQL + " ORDER BY IdadeJustificativa";
                        break;
                    case "Dublado":
                        sql = "SELECT ID, Dublado FROM tFilme WHERE " + FiltroSQL + " ORDER BY Dublado";
                        break;
                    case "IMDB":
                        sql = "SELECT ID, IMDB FROM tFilme WHERE " + FiltroSQL + " ORDER BY IMDB";
                        break;
                    case "Sinopse":
                        sql = "SELECT ID, Sinopse FROM tFilme WHERE " + FiltroSQL + " ORDER BY Sinopse";
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

    #region "FilmeException"

    [Serializable]
    public class FilmeException : Exception
    {

        public FilmeException() : base() { }

        public FilmeException(string msg) : base(msg) { }

        public FilmeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}