/******************************************************
* Arquivo AssinaturaEmailModeloDB.cs
* Gerado em: 24/10/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "AssinaturaEmailModelo_B"

    public abstract class AssinaturaEmailModelo_B : BaseBD
    {

        public nome Nome = new nome();
        public assinaturatipoid AssinaturaTipoID = new assinaturatipoid();
        public assinautraemailremetenteid AssinautraEmailRemetenteID = new assinautraemailremetenteid();
        public assunto Assunto = new assunto();
        public corpo Corpo = new corpo();
        public salvo Salvo = new salvo();

        public AssinaturaEmailModelo_B() { }

        // passar o Usuario logado no sistema
        public AssinaturaEmailModelo_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaEmailModelo
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaEmailModelo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.AssinaturaTipoID.ValorBD = bd.LerInt("AssinaturaTipoID").ToString();
                    this.AssinautraEmailRemetenteID.ValorBD = bd.LerInt("AssinautraEmailRemetenteID").ToString();
                    this.Assunto.ValorBD = bd.LerString("Assunto");
                    this.Corpo.ValorBD = bd.LerString("Corpo");
                    this.Salvo.ValorBD = bd.LerString("Salvo");
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
        /// Preenche todos os atributos de AssinaturaEmailModelo do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xAssinaturaEmailModelo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.AssinaturaTipoID.ValorBD = bd.LerInt("AssinaturaTipoID").ToString();
                    this.AssinautraEmailRemetenteID.ValorBD = bd.LerInt("AssinautraEmailRemetenteID").ToString();
                    this.Assunto.ValorBD = bd.LerString("Assunto");
                    this.Corpo.ValorBD = bd.LerString("Corpo");
                    this.Salvo.ValorBD = bd.LerString("Salvo");
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
                sql.Append("INSERT INTO cAssinaturaEmailModelo (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xAssinaturaEmailModelo (ID, Versao, Nome, AssinaturaTipoID, AssinautraEmailRemetenteID, Assunto, Corpo, Salvo) ");
                sql.Append("SELECT ID, @V, Nome, AssinaturaTipoID, AssinautraEmailRemetenteID, Assunto, Corpo, Salvo FROM tAssinaturaEmailModelo WHERE ID = @I");
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
        /// Inserir novo(a) AssinaturaEmailModelo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinaturaEmailModelo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaEmailModelo(ID, Nome, AssinaturaTipoID, AssinautraEmailRemetenteID, Assunto, Corpo, Salvo) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,'@004','@005','@006')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@003", this.AssinautraEmailRemetenteID.ValorBD);
                sql.Replace("@004", this.Assunto.ValorBD);
                sql.Replace("@005", this.Corpo.ValorBD);
                sql.Replace("@006", this.Salvo.ValorBD);

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
        /// Inserir novo(a) AssinaturaEmailModelo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinaturaEmailModelo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaEmailModelo(ID, Nome, AssinaturaTipoID, AssinautraEmailRemetenteID, Assunto, Corpo, Salvo) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,'@004','@005','@006')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@003", this.AssinautraEmailRemetenteID.ValorBD);
                sql.Replace("@004", this.Assunto.ValorBD);
                sql.Replace("@005", this.Corpo.ValorBD);
                sql.Replace("@006", this.Salvo.ValorBD);

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
        /// Atualiza AssinaturaEmailModelo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinaturaEmailModelo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaEmailModelo SET Nome = '@001', AssinaturaTipoID = @002, AssinautraEmailRemetenteID = @003, Assunto = '@004', Corpo = '@005', Salvo = '@006' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@003", this.AssinautraEmailRemetenteID.ValorBD);
                sql.Replace("@004", this.Assunto.ValorBD);
                sql.Replace("@005", this.Corpo.ValorBD);
                sql.Replace("@006", this.Salvo.ValorBD);

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
        /// Atualiza AssinaturaEmailModelo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinaturaEmailModelo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaEmailModelo SET Nome = '@001', AssinaturaTipoID = @002, AssinautraEmailRemetenteID = @003, Assunto = '@004', Corpo = '@005', Salvo = '@006' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@003", this.AssinautraEmailRemetenteID.ValorBD);
                sql.Replace("@004", this.Assunto.ValorBD);
                sql.Replace("@005", this.Corpo.ValorBD);
                sql.Replace("@006", this.Salvo.ValorBD);

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
        /// Exclui AssinaturaEmailModelo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinaturaEmailModelo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinaturaEmailModelo WHERE ID=" + id;

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
        /// Exclui AssinaturaEmailModelo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinaturaEmailModelo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinaturaEmailModelo WHERE ID=" + id;

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
        /// Exclui AssinaturaEmailModelo
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
            this.AssinaturaTipoID.Limpar();
            this.AssinautraEmailRemetenteID.Limpar();
            this.Assunto.Limpar();
            this.Corpo.Limpar();
            this.Salvo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.AssinaturaTipoID.Desfazer();
            this.AssinautraEmailRemetenteID.Desfazer();
            this.Assunto.Desfazer();
            this.Corpo.Desfazer();
            this.Salvo.Desfazer();
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
                    return 100;
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

        public class assinaturatipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaTipoID";
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

        public class assinautraemailremetenteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinautraEmailRemetenteID";
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

        public class assunto : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Assunto";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
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

        public class corpo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Corpo";
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

        public class salvo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Salvo";
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

                DataTable tabela = new DataTable("AssinaturaEmailModelo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("AssinaturaTipoID", typeof(int));
                tabela.Columns.Add("AssinautraEmailRemetenteID", typeof(int));
                tabela.Columns.Add("Assunto", typeof(string));
                tabela.Columns.Add("Corpo", typeof(string));
                tabela.Columns.Add("Salvo", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "AssinaturaEmailModeloLista_B"

    public abstract class AssinaturaEmailModeloLista_B : BaseLista
    {

        private bool backup = false;
        protected AssinaturaEmailModelo assinaturaEmailModelo;

        // passar o Usuario logado no sistema
        public AssinaturaEmailModeloLista_B()
        {
            assinaturaEmailModelo = new AssinaturaEmailModelo();
        }

        // passar o Usuario logado no sistema
        public AssinaturaEmailModeloLista_B(int usuarioIDLogado)
        {
            assinaturaEmailModelo = new AssinaturaEmailModelo(usuarioIDLogado);
        }

        public AssinaturaEmailModelo AssinaturaEmailModelo
        {
            get { return assinaturaEmailModelo; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaEmailModelo especifico
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
                    assinaturaEmailModelo.Ler(id);
                    return assinaturaEmailModelo;
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
                    sql = "SELECT ID FROM tAssinaturaEmailModelo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaEmailModelo";

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
                    sql = "SELECT ID FROM tAssinaturaEmailModelo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaEmailModelo";

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
                    sql = "SELECT ID FROM xAssinaturaEmailModelo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xAssinaturaEmailModelo";

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
        /// Preenche AssinaturaEmailModelo corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    assinaturaEmailModelo.Ler(id);
                else
                    assinaturaEmailModelo.LerBackup(id);

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

                bool ok = assinaturaEmailModelo.Excluir();
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
        /// Inseri novo(a) AssinaturaEmailModelo na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaEmailModelo.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaEmailModelo.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de AssinaturaEmailModelo carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaEmailModelo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("AssinaturaTipoID", typeof(int));
                tabela.Columns.Add("AssinautraEmailRemetenteID", typeof(int));
                tabela.Columns.Add("Assunto", typeof(string));
                tabela.Columns.Add("Corpo", typeof(string));
                tabela.Columns.Add("Salvo", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaEmailModelo.Control.ID;
                        linha["Nome"] = assinaturaEmailModelo.Nome.Valor;
                        linha["AssinaturaTipoID"] = assinaturaEmailModelo.AssinaturaTipoID.Valor;
                        linha["AssinautraEmailRemetenteID"] = assinaturaEmailModelo.AssinautraEmailRemetenteID.Valor;
                        linha["Assunto"] = assinaturaEmailModelo.Assunto.Valor;
                        linha["Corpo"] = assinaturaEmailModelo.Corpo.Valor;
                        linha["Salvo"] = assinaturaEmailModelo.Salvo.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaEmailModelo");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("AssinaturaTipoID", typeof(int));
                    tabela.Columns.Add("AssinautraEmailRemetenteID", typeof(int));
                    tabela.Columns.Add("Assunto", typeof(string));
                    tabela.Columns.Add("Corpo", typeof(string));
                    tabela.Columns.Add("Salvo", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = assinaturaEmailModelo.Nome.Valor;
                        linha["AssinaturaTipoID"] = assinaturaEmailModelo.AssinaturaTipoID.Valor;
                        linha["AssinautraEmailRemetenteID"] = assinaturaEmailModelo.AssinautraEmailRemetenteID.Valor;
                        linha["Assunto"] = assinaturaEmailModelo.Assunto.Valor;
                        linha["Corpo"] = assinaturaEmailModelo.Corpo.Valor;
                        linha["Salvo"] = assinaturaEmailModelo.Salvo.Valor;
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
                        sql = "SELECT ID, Nome FROM tAssinaturaEmailModelo WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "AssinaturaTipoID":
                        sql = "SELECT ID, AssinaturaTipoID FROM tAssinaturaEmailModelo WHERE " + FiltroSQL + " ORDER BY AssinaturaTipoID";
                        break;
                    case "AssinautraEmailRemetenteID":
                        sql = "SELECT ID, AssinautraEmailRemetenteID FROM tAssinaturaEmailModelo WHERE " + FiltroSQL + " ORDER BY AssinautraEmailRemetenteID";
                        break;
                    case "Assunto":
                        sql = "SELECT ID, Assunto FROM tAssinaturaEmailModelo WHERE " + FiltroSQL + " ORDER BY Assunto";
                        break;
                    case "Corpo":
                        sql = "SELECT ID, Corpo FROM tAssinaturaEmailModelo WHERE " + FiltroSQL + " ORDER BY Corpo";
                        break;
                    case "Salvo":
                        sql = "SELECT ID, Salvo FROM tAssinaturaEmailModelo WHERE " + FiltroSQL + " ORDER BY Salvo";
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

    #region "AssinaturaEmailModeloException"

    [Serializable]
    public class AssinaturaEmailModeloException : Exception
    {

        public AssinaturaEmailModeloException() : base() { }

        public AssinaturaEmailModeloException(string msg) : base(msg) { }

        public AssinaturaEmailModeloException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}