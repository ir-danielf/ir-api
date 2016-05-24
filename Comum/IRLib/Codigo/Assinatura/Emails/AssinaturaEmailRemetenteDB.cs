/******************************************************
* Arquivo AssinaturaEmailRemetenteDB.cs
* Gerado em: 19/10/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaEmailRemetente_B"

    public abstract class AssinaturaEmailRemetente_B : BaseBD
    {

        public remetente Remetente = new remetente();
        public nomeexibicao NomeExibicao = new nomeexibicao();
        public smtp SMTP = new smtp();
        public senha Senha = new senha();

        public AssinaturaEmailRemetente_B() { }

        // passar o Usuario logado no sistema
        public AssinaturaEmailRemetente_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaEmailEnviar
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaEmailEnviar WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Remetente.ValorBD = bd.LerString("Remetente");
                    this.NomeExibicao.ValorBD = bd.LerString("NomeExibicao");
                    this.SMTP.ValorBD = bd.LerString("SMTP");
                    this.Senha.ValorBD = bd.LerString("Senha");
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
        /// Preenche todos os atributos de AssinaturaEmailEnviar do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xAssinaturaEmailEnviar WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Remetente.ValorBD = bd.LerString("Remetente");
                    this.NomeExibicao.ValorBD = bd.LerString("NomeExibicao");
                    this.SMTP.ValorBD = bd.LerString("SMTP");
                    this.Senha.ValorBD = bd.LerString("Senha");
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
                sql.Append("INSERT INTO cAssinaturaEmailEnviar (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xAssinaturaEmailEnviar (ID, Versao, Remetente, NomeExibicao, SMTP, Senha) ");
                sql.Append("SELECT ID, @V, Remetente, NomeExibicao, SMTP, Senha FROM tAssinaturaEmailEnviar WHERE ID = @I");
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
        /// Inserir novo(a) AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinaturaEmailEnviar");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaEmailEnviar(ID, Remetente, NomeExibicao, SMTP, Senha) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Remetente.ValorBD);
                sql.Replace("@002", this.NomeExibicao.ValorBD);
                sql.Replace("@003", this.SMTP.ValorBD);
                sql.Replace("@004", this.Senha.ValorBD);

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
        /// Inserir novo(a) AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinaturaEmailEnviar");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaEmailEnviar(ID, Remetente, NomeExibicao, SMTP, Senha) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Remetente.ValorBD);
                sql.Replace("@002", this.NomeExibicao.ValorBD);
                sql.Replace("@003", this.SMTP.ValorBD);
                sql.Replace("@004", this.Senha.ValorBD);

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
        /// Atualiza AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinaturaEmailEnviar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaEmailEnviar SET Remetente = '@001', NomeExibicao = '@002', SMTP = '@003', Senha = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Remetente.ValorBD);
                sql.Replace("@002", this.NomeExibicao.ValorBD);
                sql.Replace("@003", this.SMTP.ValorBD);
                sql.Replace("@004", this.Senha.ValorBD);

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
        /// Atualiza AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinaturaEmailEnviar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaEmailEnviar SET Remetente = '@001', NomeExibicao = '@002', SMTP = '@003', Senha = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Remetente.ValorBD);
                sql.Replace("@002", this.NomeExibicao.ValorBD);
                sql.Replace("@003", this.SMTP.ValorBD);
                sql.Replace("@004", this.Senha.ValorBD);

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
        /// Exclui AssinaturaEmailEnviar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinaturaEmailEnviar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinaturaEmailEnviar WHERE ID=" + id;

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
        /// Exclui AssinaturaEmailEnviar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinaturaEmailEnviar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinaturaEmailEnviar WHERE ID=" + id;

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
        /// Exclui AssinaturaEmailEnviar
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

            this.Remetente.Limpar();
            this.NomeExibicao.Limpar();
            this.SMTP.Limpar();
            this.Senha.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Remetente.Desfazer();
            this.NomeExibicao.Desfazer();
            this.SMTP.Desfazer();
            this.Senha.Desfazer();
        }

        public class remetente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Remetente";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

        public class nomeexibicao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeExibicao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

        public class smtp : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SMTP";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

        public class senha : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Senha";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

                DataTable tabela = new DataTable("AssinaturaEmailRemetente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Remetente", typeof(string));
                tabela.Columns.Add("NomeExibicao", typeof(string));
                tabela.Columns.Add("SMTP", typeof(string));
                tabela.Columns.Add("Senha", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "AssinaturaEmailRemetenteLista_B"

    public abstract class AssinaturaEmailRemetenteLista_B : BaseLista
    {

        private bool backup = false;
        protected AssinaturaEmailRemetente assinaturaEmailRemetente;

        // passar o Usuario logado no sistema
        public AssinaturaEmailRemetenteLista_B()
        {
            assinaturaEmailRemetente = new AssinaturaEmailRemetente();
        }

        // passar o Usuario logado no sistema
        public AssinaturaEmailRemetenteLista_B(int usuarioIDLogado)
        {
            assinaturaEmailRemetente = new AssinaturaEmailRemetente(usuarioIDLogado);
        }

        public AssinaturaEmailRemetente AssinaturaEmailRemetente
        {
            get { return assinaturaEmailRemetente; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaEmailRemetente especifico
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
                    assinaturaEmailRemetente.Ler(id);
                    return assinaturaEmailRemetente;
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
                    sql = "SELECT ID FROM tAssinaturaEmailEnviar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaEmailEnviar";

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
                    sql = "SELECT ID FROM tAssinaturaEmailEnviar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaEmailEnviar";

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
                    sql = "SELECT ID FROM xAssinaturaEmailEnviar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xAssinaturaEmailEnviar";

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
        /// Preenche AssinaturaEmailRemetente corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    assinaturaEmailRemetente.Ler(id);
                else
                    assinaturaEmailRemetente.LerBackup(id);

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

                bool ok = assinaturaEmailRemetente.Excluir();
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
        /// Inseri novo(a) AssinaturaEmailRemetente na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaEmailRemetente.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaEmailRemetente.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de AssinaturaEmailRemetente carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaEmailRemetente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Remetente", typeof(string));
                tabela.Columns.Add("NomeExibicao", typeof(string));
                tabela.Columns.Add("SMTP", typeof(string));
                tabela.Columns.Add("Senha", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaEmailRemetente.Control.ID;
                        linha["Remetente"] = assinaturaEmailRemetente.Remetente.Valor;
                        linha["NomeExibicao"] = assinaturaEmailRemetente.NomeExibicao.Valor;
                        linha["SMTP"] = assinaturaEmailRemetente.SMTP.Valor;
                        linha["Senha"] = assinaturaEmailRemetente.Senha.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaEmailRemetente");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Remetente", typeof(string));
                    tabela.Columns.Add("NomeExibicao", typeof(string));
                    tabela.Columns.Add("SMTP", typeof(string));
                    tabela.Columns.Add("Senha", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Remetente"] = assinaturaEmailRemetente.Remetente.Valor;
                        linha["NomeExibicao"] = assinaturaEmailRemetente.NomeExibicao.Valor;
                        linha["SMTP"] = assinaturaEmailRemetente.SMTP.Valor;
                        linha["Senha"] = assinaturaEmailRemetente.Senha.Valor;
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
                    case "Remetente":
                        sql = "SELECT ID, Remetente FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY Remetente";
                        break;
                    case "NomeExibicao":
                        sql = "SELECT ID, NomeExibicao FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY NomeExibicao";
                        break;
                    case "SMTP":
                        sql = "SELECT ID, SMTP FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY SMTP";
                        break;
                    case "Senha":
                        sql = "SELECT ID, Senha FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY Senha";
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

    #region "AssinaturaEmailRemetenteException"

    [Serializable]
    public class AssinaturaEmailRemetenteException : Exception
    {

        public AssinaturaEmailRemetenteException() : base() { }

        public AssinaturaEmailRemetenteException(string msg) : base(msg) { }

        public AssinaturaEmailRemetenteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}